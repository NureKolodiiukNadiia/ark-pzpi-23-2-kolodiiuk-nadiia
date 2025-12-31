#include "smart_lock.h"

#include <iostream>
#include <cstdio>
#include <WiFi.h>
#include <HTTPClient.h>
#include <WiFiClientSecure.h>

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

SmartLock::SmartLock(const std::string& initial_state)
    : locked(true),
      relock_delay_ms(2000u),
      api_host("")
{
    if (initial_state == "unlocked") {
        locked = false;
    }
    std::cout << "[INFO] Smart lock initialized in state: " << status() << "\n";
}

bool SmartLock::lock(int device_id, int user_id, const std::string& qrCode, bool isOwnerOverride) {
    if (locked) {
        std::cout << "[WARN] Lock command ignored. Already locked.\n";
        return false;
    }

    char buffer[512];
    std::snprintf(buffer, sizeof(buffer),
        R"({"deviceId": %d, "userId": %d, "qrCode": "%s", "isOwnerOverride": %s})",
        device_id,
        user_id,
        escapeJson(qrCode).c_str(),
        isOwnerOverride ? "true" : "false");
    std::string payload(buffer);

    locked = true;
    std::cout << "[INFO] Lock engaged.\n";
    return true;
}

bool SmartLock::unlock(int device_id, int user_id, const std::string& qrCode, bool isOwnerOverride) {
    if (!locked) {
        std::cout << "[WARN] Unlock command ignored. Already unlocked.\n";
        return false;
    }

    char buffer[512];
    std::snprintf(buffer, sizeof(buffer),
        R"({"deviceId": %d, "userId": %d, "qrCode": "%s", "isOwnerOverride": %s})",
        device_id,
        user_id,
        escapeJson(qrCode).c_str(),
        isOwnerOverride ? "true" : "false");
    std::string payload(buffer);

    postJson("IoT/unlock", payload);

    locked = false;
    std::cout << "[INFO] Lock opened.\n";

    return true;
}

bool SmartLock::isLocked() const {
    return locked;
}

unsigned int SmartLock::relockDelayMs() const {
    return relock_delay_ms;
}

std::string SmartLock::status() const {
    return locked ? "Locked" : "Unlocked";
}

bool SmartLock::postJsonWithPayload(const std::string& path, const std::string& jsonBody, std::string* outPayload) const {
    std::string url = api_host;
    if (!url.empty() && url.back() == '/' && !path.empty() && path.front() == '/') {
        url.pop_back();
    }

    url += path;

    // Dummy implementation for compatibility
    if (outPayload) {
        *outPayload = "";
    }
    return true;
}

bool SmartLock::postJson(const std::string& path, const std::string& jsonBody) const {

    std::string url = api_host;
    if (!url.empty() && url.back() == '/' && !path.empty() && path.front() == '/') {
        url.pop_back();
    }

    url += path;

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