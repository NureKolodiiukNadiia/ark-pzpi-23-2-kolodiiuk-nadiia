#include "smart_lock.h"

#include <iostream>
#include <nlohmann/json.hpp>

SmartLock::SmartLock(std::string initial_state)
    : locked(true),
      relock_delay_ms(2000u) {
    auto initialState = initial_state;
    if (initialState == "unlocked") {
        locked = false;
    }

    std::cout << "[INFO] Smart lock initialized in state: " << status() << "\n";
}

bool SmartLock::lock() {
    if (locked) {
        std::cout << "[WARN] Lock command ignored. Already locked.\n";
        return false;
    }

    std::string payload = fmt::format(
        R"({{"deviceId": {}, "userId": {}, "qrCode": "{}", "isOwnerOverride": {}}})",
        device_id,
        userId,
        escapeJson(qrCode),
        isOwnerOverride ? "true" : "false");

    locked = true;
    std::cout << "[INFO] Lock engaged.\n";
    return true;
}

bool SmartLock::unlock() {
    if (!locked) {
        std::cout << "[WARN] Unlock command ignored. Already unlocked.\n";
        return false;
    }

    std::string payload = fmt::format(
        R"({{"deviceId": {}, "userId": {}, "qrCode": "{}", "isOwnerOverride": {}}})",
        device_id,
        userId,
        escapeJson(qrCode),
        isOwnerOverride ? "true" : "false");

    std::string response = postJson("IoT/unlock", payload);

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

    cpr::Response response = cpr::Post(
        cpr::Url{url},
        cpr::Body{jsonBody},
        cpr::Header{{"Content-Type", "application/json"}},
        cpr::Timeout{5000}
    );

    if (response.error.code != cpr::ErrorCode::OK) {
        std::cerr << "[ERROR] POST " << url << " failed: " << response.error.message << "\n";
        if (outPayload) {
            *outPayload = response.error.message;
        }
        return false;
    }

    if (outPayload) {
        *outPayload = response.text;
    }

    if (response.status_code >= 200 && response.status_code < 300) {
        try {
            nlohmann::json jsonResponse = nlohmann::json::parse(response.text);
            bool success = jsonResponse["success"];
        } catch (const std::exception& e) {
            std::cerr << "JSON parsing error: " << e.what() << "\n";
            return false;
        }
    }

    std::cout << "[HTTP] POST " << url << " status " << response.status_code << " body: " << response.text << "\n";

    return response.status_code >= 200 && response.status_code < 300;
}
