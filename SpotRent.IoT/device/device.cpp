#include "device.h"

#include <chrono>
#include <fmt/core.h>
#include <iostream>
#include <thread>

#include "../lock/smart_lock.h"
#include "../qr_scanner/qr_scanner.h"

namespace {
std::string escapeJson(const std::string& value) {
    std::string escaped;
    escaped.reserve(value.size());
    for (char ch : value) {
        switch (ch) {
        case '\"':
            escaped += "\\\"";
            break;
        case '\\':
            escaped += "\\\\";
            break;
        case '\n':
            escaped += "\\n";
            break;
        case '\r':
            escaped += "\\r";
            break;
        case '\t':
            escaped += "\\t";
            break;
        default:
            escaped += ch;
            break;
        }
    }
    return escaped;
}
} // namespace

Device::Device(const toml::table& config)
    : config_table(config),
      device_id(config["device"]["id"].value_or(1)),
      api_host(config["server"]["host"].value_or("")),
      auto_register(config["device"]["register_on_start"].value_or(true)),
      default_booking_id(config["device"]["default_booking_id"].value_or(0)) {
    if (api_host.empty()) {
        throw std::runtime_error("server.host must be configured");
    }

    while (!api_host.empty() && api_host.back() == '/') {
        api_host.pop_back();
    }
}

Device::~Device() = default;

void Device::addQrScanner() {
    scanner = std::make_unique<QrScanner>(config_table);
}

void Device::addSmartLock() {
    smart_lock = std::make_unique<SmartLock>(config_table);
}

bool Device::lock() {
    if (!smart_lock) {
        return false;
    }
    bool result = smart_lock->lock();
    if (result) {
        updateDeviceStatus(smart_lock->status(), true);
    }
    return result;
}

bool Device::unlock() {
    if (!smart_lock) {
        return false;
    }
    bool result = smart_lock->unlock();
    if (result) {
        updateDeviceStatus(smart_lock->status(), true);
    }
    return result;
}

void Device::run() {
    if (!scanner) {
        addQrScanner();
    }
    if (!smart_lock) {
        addSmartLock();
    }

    if (auto_register && !registerDevice()) {
        std::cerr << "[WARN] Device registration failed. Continuing with simulation.\n";
    }

    updateDeviceStatus(smart_lock->status(), true);

    QrScanEvent scan{};
    while (scanner->next(scan)) {
        std::cout << "[INFO] Processing scan for user " << scan.userId
                  << " with QR token '" << scan.qrCode << "'\n";

        bool shouldUnlock = scan.shouldUnlock;
        bool unlockResult = false;
        std::string errorMessage;

        if (shouldUnlock) {
            unlockResult = unlock();
            if (!unlockResult) {
                errorMessage = "Lock is already unlocked";
            }
        } else {
            errorMessage = "Access denied by simulation";
        }

        if (scan.isOwner) {
            logEventOwner(scan.userId, scan.accessType, unlockResult, errorMessage);
        } else {
            int bookingId = scan.bookingId > 0 ? scan.bookingId : default_booking_id;
            logEvent(scan.userId, bookingId, scan.accessType, unlockResult, errorMessage);
        }

        if (unlockResult && smart_lock->relockDelayMs() > 0) {
            std::this_thread::sleep_for(std::chrono::milliseconds(smart_lock->relockDelayMs()));
            lock();
        }

        unsigned int pause = scan.pauseAfterMs > 0 ? scan.pauseAfterMs : scanner->pollIntervalMs();
        std::this_thread::sleep_for(std::chrono::milliseconds(pause));
    }

    updateDeviceStatus(smart_lock->status(), true);
}

bool Device::registerDevice() const {
    std::string payload = fmt::format(R"({{"deviceId": {}}})", device_id);
    return postJson("/IoT/register", payload);
}

void Device::logEvent(int userId, int bookingId, int accessType, bool isSuccessful, const std::string& errorMessage) const {
    if (bookingId <= 0) {
        std::cerr << "[WARN] Unable to log access event without valid booking id.\n";
        return;
    }

    std::string payload = fmt::format(
        R"({{"userId": {}, "deviceId": {}, "accessType": {}, "bookingId": {}, "isSuccessful": {}, "errorMessage": "{}"}})",
        userId,
        device_id,
        accessType,
        bookingId,
        isSuccessful ? "true" : "false",
        escapeJson(errorMessage));

    postJson("/AccessLog", payload);
}

void Device::logEventOwner(int userId, int accessType, bool isSuccessful, const std::string& errorMessage) const {
    std::string payload = fmt::format(
        R"({{"userId": {}, "deviceId": {}, "accessType": {}, "isSuccessful": {}, "errorMessage": "{}"}})",
        userId,
        device_id,
        accessType,
        isSuccessful ? "true" : "false",
        escapeJson(errorMessage));

    postJson("/AccessLog/owner", payload);
}

void Device::updateDeviceStatus(const std::string& statusMessage, bool isOnline) const {
    std::string payload = fmt::format(
        R"({{"deviceId": {}, "isOnline": {}, "statusMessage": "{}"}})",
        device_id,
        isOnline ? "true" : "false",
        escapeJson(statusMessage));

    postJson("/IoT/device-status", payload);
}

bool Device::postJson(const std::string& path, const std::string& jsonBody) const {
    std::cout << "[SIM] POST " << api_host << path << " with body: " << jsonBody << "\n";
    return true;
}
