#include <iostream>
#include <chrono>
#include <cstring>
#include <fmt/core.h> // Include fmt library
#include "smart_lock.h"

#include <unistd.h>
#include "lock_mechanism.h"

// Factory function declaration (implemented in a separate file, e.g., mechanism_factory.cpp)
LockMechanism *createLockMechanism(const toml::table &config);

SmartLock::SmartLock(const toml::table &config) {
    const auto api_host = config["server"]["host"].value<std::string>();
    if (!api_host) {
        throw std::runtime_error("Failed to initialize: \"host\" under \"[server]\" is not specified!");
    }
    this->api_host = api_host.value();

    this->check_interval_sec = config["lock"]["check_interval_sec"].value_or(5u);

    this->lock_mechanism = createLockMechanism(config);
    if (this->lock_mechanism == nullptr) {
        throw std::runtime_error("Failed to create lock mechanism!");
    }

    std::memset(access_history, 0, sizeof(access_history));
    std::cout << "SmartLock initialized. Starting main loop.\n";
}

SmartLock::~SmartLock() {
    delete lock_mechanism;
}

void SmartLock::run() {
    while (true) {
        cpr::Response resp = cpr::Get(
            cpr::Url{api_host + "/commands"},
            cpr::Parameters{{"lock_id", "office_door_1"}}
        );

        if (resp.status_code == 200 && !resp.text.empty()) {
            std::string response_text = resp.text;
            if (response_text.find("\"command\": \"unlock\"") != std::string::npos) {
                std::string credential_id = "remote_server";
                std::cout << "Received unlock command from server.\n";

                bool success = lock_mechanism->unlock();
                AccessAttempt attempt;
                attempt.timestamp = std::chrono::system_clock::to_time_t(std::chrono::system_clock::now());
                attempt.credential_id = credential_id;
                attempt.granted = success;
                attempt.reason = success ? "Remote command" : "Lock mechanism failed";

                access_history[history_index] = attempt;
                history_index = (history_index + 1) % max_history;
                sendAccessLog(attempt);

                if (success) {
                    logEvent("UNLOCK", "Via remote command");
                }
            }
        } else if (resp.status_code != 200) {
            std::cerr << "Failed to check commands: " << resp.status_code << " - " << resp.text << "\n";
        }

        std::string local_event = lock_mechanism->getStatus();
        if (local_event != "idle") {
            AccessAttempt attempt;
            attempt.timestamp = std::chrono::system_clock::to_time_t(std::chrono::system_clock::now());
            attempt.credential_id = "nfc_card_simulated";
            attempt.granted = true;
            attempt.reason = "NFC tap";

            access_history[history_index] = attempt;
            history_index = (history_index + 1) % max_history;
            sendAccessLog(attempt);

            if (attempt.granted) {
                logEvent("UNLOCK", "Via NFC: " + attempt.credential_id);
                bool success = lock_mechanism->unlock();
                if (!success) {
                    logEvent("ERROR", "Unlock command failed after granted access!");
                }
            }
        }

        sleep(check_interval_sec);
    }
}

void SmartLock::sendAccessLog(const AccessAttempt &attempt) {
    char time_buf[100];
    std::strftime(time_buf, sizeof(time_buf), "%FT%TZ", std::gmtime(&attempt.timestamp));

    std::string json_body = fmt::format(
        R"({{ "timestamp": "{}", "lock_id": "office_door_1", "credential_id": "{}", "access_granted": {}, "reason": "{}" }})",
        time_buf, attempt.credential_id, (attempt.granted ? "true" : "false"), attempt.reason
    );

    cpr::Response resp = cpr::Post(
        cpr::Url{api_host + "/access_logs"},
        cpr::Body{json_body}
    );

    if (resp.status_code != 201) {
        std::cerr << "Failed to send access log: " << resp.status_code << " - " << resp.text << "\n";
    } else {
        std::cout << "Access log sent successfully.\n";
    }
}

void SmartLock::logEvent(const std::string &event, const std::string &detail) {
    std::time_t now = std::chrono::system_clock::to_time_t(std::chrono::system_clock::now());
    char time_buf[100];
    std::strftime(time_buf, sizeof(time_buf), "%T", std::localtime(&now));
    std::cout << "[" << time_buf << "] " << event << ": " << detail << "\n";
}
