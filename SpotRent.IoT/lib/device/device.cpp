#include "device.h"

#include <chrono>
#include <iostream>
#include <thread>
#include <cstdio>

#include <WiFi.h>
#include <HTTPClient.h>
#include <WiFiClientSecure.h>

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

Device::Device()
    : device_id(1),
      api_host("https://irrigative-bessie-evidentially.ngrok-free.dev/api"),
      auto_register(true),
      default_user_id(101),
      default_booking_id(2001) {
}

Device::~Device() = default;

void Device::addSmartLock() {
    smart_lock.reset(new SmartLock("locked"));
}

bool Device::lock(int user_id, const std::string& qrCode, bool isOwnerOverride) {
    if (!smart_lock) {
        return false;
    }

    bool result = smart_lock->lock(device_id, user_id, qrCode, isOwnerOverride);
    if (result) {
        updateDeviceStatus(smart_lock->status(), true);
    }

    return result;
}

bool Device::unlock(int user_id, const std::string& qrCode, bool isOwnerOverride) {
    if (!smart_lock) {
        return false;
    }

    bool result = smart_lock->unlock(device_id, user_id, qrCode, isOwnerOverride);
    if (result) {
        updateDeviceStatus(smart_lock->status(), true);
    }

    return result;
}

bool Device::registerDevice() const {
    char buffer[128];
    std::snprintf(buffer, sizeof(buffer), "{\"deviceId\": %d}", device_id);
    std::string payload(buffer);

    return postJson("/IoT/register", payload);
}

void Device::logEvent(int userId, int bookingId, int accessType,
    bool isSuccessful, const std::string& errorMessage) const {

    if (bookingId <= 0) {
        std::cerr << "[WARN] Unable to log access event without valid booking id.\n";

        return;
    }

    std::string escapedError = escapeJson(errorMessage);
    char buffer[512];
    std::snprintf(buffer, sizeof(buffer),
        "{\"userId\": %d, \"deviceId\": %d, \"accessType\": %d, \"bookingId\": %d, \"isSuccessful\": %s, \"errorMessage\": \"%s\"}",
        userId,
        device_id,
        accessType,
        bookingId,
        isSuccessful ? "true" : "false",
        escapedError.c_str());

    std::string payload(buffer);

    postJson("/AccessLog", payload);
}

void Device::logEventOwner(int userId, int accessType, bool isSuccessful, const std::string& errorMessage) const {
    std::string escapedError = escapeJson(errorMessage);
    char buffer[512];
    std::snprintf(buffer, sizeof(buffer),
        "{\"userId\": %d, \"deviceId\": %d, \"accessType\": %d, \"isSuccessful\": %s, \"errorMessage\": \"%s\"}",
        userId,
        device_id,
        accessType,
        isSuccessful ? "true" : "false",
        escapedError.c_str());

    std::string payload(buffer);

    postJson("/AccessLog/owner", payload);
}

void Device::updateDeviceStatus(const std::string& statusMessage, bool isOnline) const {

    std::string escapedStatus = escapeJson(statusMessage);
    char buffer[512];
    std::snprintf(buffer, sizeof(buffer),
        "{\"deviceId\": %d, \"isOnline\": %s, \"statusMessage\": \"%s\"}",
        device_id,
        isOnline ? "true" : "false",
        escapedStatus.c_str());

    std::string payload(buffer);

    postJson("/IoT/device-status", payload);
}

bool Device::postJson(const std::string& path, const std::string& jsonBody) const {

    std::string url = api_host;
    if (!url.empty() && url.back() == '/' && !path.empty() && path.front() == '/') {
        url.pop_back();
    }

    url += path;
    url = "https://irrigative-bessie-evidentially.ngrok-free.dev/api/iot/register";

    if (WiFi.status() == WL_CONNECTED) {
        HTTPClient http;
        http.begin((url).c_str());
        http.addHeader("Content-Type", "application/json");
        int httpResponseCode = http.POST(jsonBody.c_str());

        if (httpResponseCode < 0) {
            std::cerr << "[ERROR] POST " << url << " failed: " << http.errorToString(httpResponseCode).c_str() << "\n";
            http.end();
            return false;
        }

        std::cout << "[HTTP] POST " << url << " status " << httpResponseCode << " body: " << http.getString().c_str() << "\n";
        bool ok = httpResponseCode >= 200 && httpResponseCode < 300;
        http.end();
        return ok;
    } else {
        std::cerr << "[ERROR] WiFi not connected, cannot POST " << url << "\n";
        return false;
    }
}
