#ifndef SPOTRENT_IOT_SMART_LOCK_H
#define SPOTRENT_IOT_SMART_LOCK_H

#include <string>
#include <toml.hpp>

class SmartLock {
public:
    explicit SmartLock(const toml::table& config);

    bool lock();
    bool unlock();
    [[nodiscard]] bool isLocked() const;
    [[nodiscard]] unsigned int relockDelayMs() const;
    [[nodiscard]] std::string status() const;

private:
    bool locked;
    unsigned int relock_delay_ms;
};

#endif
