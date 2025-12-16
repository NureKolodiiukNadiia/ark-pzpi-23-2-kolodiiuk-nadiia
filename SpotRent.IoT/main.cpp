#include <iostream>
#include <toml.hpp>

#include "device/device.h"

int main() {
    try {
        auto config = toml::parse_file("config.toml");
        Device device(config);
        device.addSmartLock();
        device.run();
    } catch (const std::exception& ex) {
        std::cerr << "[FATAL] " << ex.what() << "\n";
        return 1;
    }

    return 0;
}
