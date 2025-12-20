#include "smart_lock.h"

#include <iostream>

SmartLock::SmartLock(const toml::table& config)
    : locked(true),
      relock_delay_ms(config["lock"]["relock_delay_ms"].value_or(2000u)) {
    auto initialState = config["lock"]["initial_state"].value_or(std::string("locked"));
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

    locked = true;
    std::cout << "[INFO] Lock engaged.\n";
    return true;
}

bool SmartLock::unlock() {
    if (!locked) {
        std::cout << "[WARN] Unlock command ignored. Already unlocked.\n";
        return false;
    }

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
