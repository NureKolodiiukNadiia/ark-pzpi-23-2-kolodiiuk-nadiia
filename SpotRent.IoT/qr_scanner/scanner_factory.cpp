#include <iostream>

#include "scanner_source.h"
#include <toml.hpp>
#include <random>

// Simulated Scanner for testing
class SimulatedScanner : public ScannerSource {
private:
    std::vector<std::string> valid_codes;
    double invalid_probability;
    double scan_probability_per_check;
    std::mt19937 rng;
    std::exponential_distribution<> interval_dist;

public:
    SimulatedScanner(const toml::table& config) : rng(std::random_device{}()) {
        invalid_probability = config["simulated"]["invalid_data_probability"].value_or(0.1);
        
        double mean_interval = config["simulated"]["scan_interval_mean_sec"].value_or(5.0);
        interval_dist = std::exponential_distribution<>(1.0 / mean_interval);

        if (auto codes = config["simulated"]["valid_data"].as_array()) {
            for (const auto& code : *codes) {
                if (auto str = code.value<std::string>()) {
                    valid_codes.push_back(*str);
                }
            }
        }
        // Add some default codes if none configured
        if (valid_codes.empty()) {
            valid_codes = {"https://example.com/product/default", "TEST-1234-5678"};
        }
    }

    std::optional<ScannedCode> getScan() override {
        // Simulate random arrival of scans using an exponential distribution
        static double time_until_next_scan = interval_dist(rng);
        time_until_next_scan -= 0.05; // Subtract the poll interval (approx.)

        if (time_until_next_scan <= 0) {
            time_until_next_scan = interval_dist(rng); // Reset the timer

            ScannedCode scan;
            scan.format = "QR_CODE";

            if ((std::rand() / static_cast<double>(RAND_MAX)) < invalid_probability) {
                scan.data = "INVALID_RANDOM_DATA_" + std::to_string(std::rand());
            } else {
                // Pick a random valid code
                std::uniform_int_distribution<size_t> index_dist(0, valid_codes.size() - 1);
                scan.data = valid_codes[index_dist(rng)];
            }
            return scan;
        }
        return std::nullopt; // No scan at this moment
    }

    void triggerScan() override {
        std::cout << "[SIM] Manual trigger ignored in auto mode.\n";
    }
};

// Factory function
ScannerSource* createScannerSource(const toml::table& config) {
    const auto mech_type = config["scanner"]["mechanism"].value<std::string>();

    if (mech_type == "simulated") {
        std::cout << "Creating simulated scanner source.\n";
        return new SimulatedScanner(config);
    }
    else {
        std::cerr << "Unknown scanner mechanism type: " << mech_type.value() << "\n";
        return nullptr;
    }
}
