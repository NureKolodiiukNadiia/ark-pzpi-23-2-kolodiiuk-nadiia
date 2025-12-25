#include "device.h"

#include <chrono>
#include <iostream>
#include <thread>
#include <cpr/cpr.h>

#include "../lock/smart_lock.h"

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

Device::Device(int id, std::string host, bool register_on_start, int default_user_id, int default_booking_id)
    : device_id(id),
      api_host(host),
      auto_register(register_on_start),
      default_user_id(default_user_id),
      default_booking_id(default_booking_id) {

    if (api_host.empty()) {

        throw std::runtime_error("server.host must be configured");
    }
}

Device::~Device() = default;

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

bool Device::registerDevice() const {
    std::string payload = fmt::format(R"({{"deviceId": {}}})", device_id);

    return postJson("/IoT/register", payload);
}

void Device::logEvent(int userId, int bookingId, int accessType,
    bool isSuccessful, const std::string& errorMessage) const {

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

    std::string url = api_host;
    if (!url.empty() && url.back() == '/' && !path.empty() && path.front() == '/') {
        url.pop_back();
    }

    url += path;

    cpr::Response response = cpr::Post(
        cpr::Url{url},
        cpr::Body{jsonBody},
        cpr::Header{{"Content-Type", "application/json"}},
        cpr::Timeout{5000}
    );

    if (response.error.code != cpr::ErrorCode::OK) {
        std::cerr << "[ERROR] POST " << url << " failed: " << response.error.message << "\n";
        return false;
    }

    std::cout << "[HTTP] POST " << url << " status " << response.status_code << " body: " << response.text << "\n";

    return response.status_code >= 200 && response.status_code < 300;
}
