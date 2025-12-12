#ifndef COWORKINGACCESS_IOT_SMART_LOCK_H
#define COWORKINGACCESS_IOT_SMART_LOCK_H

#endif //COWORKINGACCESS_IOT_SMART_LOCK_H

#pragma once
#include <string>
#include <toml.hpp>
#include <cpr/cpr.h>

class LockMechanism; // Forward declaration

class SmartLock {
private:
    std::string api_host;
    unsigned int check_interval_sec;

    LockMechanism* lock_mechanism;

    // Keep a history of access attempts
    struct AccessAttempt {
        long timestamp;
        std::string credential_id;
        bool granted;
        //todo
        std::string reason;
    };
    static const int max_history = 100;
    AccessAttempt access_history[max_history];
    int history_index = 0;

    void sendAccessLog(const AccessAttempt& attempt);
    void logEvent(const std::string& event, const std::string& detail = "");

public:
    explicit SmartLock(const toml::table& config);
    ~SmartLock();
    void run(); // Main loop
};
