#ifndef SPOTRENT_IOT_SMART_LOCK_H
#define SPOTRENT_IOT_SMART_LOCK_H

#endif //SPOTRENT_IOT_SMART_LOCK_H

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

    virtual bool unlock() = 0;
    virtual bool lock() = 0;
    virtual bool isLocked() = 0;
    virtual std::string getStatus() const = 0;

public:
    explicit SmartLock(const toml::table& config);
    ~SmartLock();
    void run();
};
