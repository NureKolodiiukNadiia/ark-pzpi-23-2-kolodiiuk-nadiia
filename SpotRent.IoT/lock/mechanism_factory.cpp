#include <iostream>

#include "lock_mechanism.h"
#include <toml.hpp>

// Simulated Lock Mechanism for testing
class SimulatedLock : public LockMechanism {
private:
    bool is_locked = true;
    double event_probability;
public:
    SimulatedLock(const toml::table& config) {
        event_probability = config["simulated"]["event_probability"].value_or(0.1);
    }
    bool unlock() override { is_locked = false; std::cout << "[SIM] Lock unlocked.\n"; return true; }
    bool lock() override { is_locked = true; std::cout << "[SIM] Lock locked.\n"; return true; }
    bool isLocked() override { return is_locked; }
    std::string getStatus() const override {
        // Randomly simulate an event (like an NFC tap)
        if ( (std::rand() / static_cast<double>(RAND_MAX)) < event_probability ) {
            return "access_requested";
        }
        return "idle";
    }
};

// Factory function
LockMechanism* createLockMechanism(const toml::table& config) {
    const auto mech_type = config["lock"]["mechanism"].value<std::string>();

    if (mech_type == "simulated") {
        std::cout << "Creating simulated lock mechanism.\n";
        return new SimulatedLock(config);
    }
    else {
        std::cerr << "Unknown lock mechanism type: " << mech_type.value() << "\n";
        return nullptr;
    }
}
