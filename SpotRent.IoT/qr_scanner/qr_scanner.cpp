#include "qr_scanner.h"
#include <iostream>
#include <chrono>
#include <regex>
#include <fmt/core.h> // Include fmt library
#include "scanner_source.h"

// Factory function declaration
ScannerSource *createScannerSource(const toml::table &config);

QrScanner::QrScanner(const toml::table &config) {
    const auto api_host = config["server"]["host"].value<std::string>();
    if (!api_host) {
        throw std::runtime_error("Failed to initialize: \"host\" under \"[server]\" is not specified!");
    }
    this->api_host = api_host.value();

    this->device_id = config["scanner"]["device_id"].value_or("scanner_01");
    this->poll_interval_ms = config["scanner"]["poll_interval_ms"].value_or(100u);
    this->min_scan_interval_ms = config["scanner"]["min_scan_interval_ms"].value_or(2000u);
    this->max_history_size = config["scanner"]["max_history_size"].value_or(50u);

    if (auto patterns = config["scanner"]["validation_patterns"].as_array()) {
        for (const auto &pattern: *patterns) {
            if (auto str = pattern.value<std::string>()) {
                validation_patterns.push_back(*str);
            }
        }
    }

    this->scanner_source = createScannerSource(config);
    if (this->scanner_source == nullptr) {
        throw std::runtime_error("Failed to create scanner source!");
    }

    std::cout << "QR Scanner '" << device_id << "' initialized.\n";
}

QrScanner::~QrScanner() {
    delete scanner_source;
}

void QrScanner::run() {
    while (true) {
        std::optional<ScannedCode> maybe_scan = scanner_source->getScan();

        if (maybe_scan.has_value()) {
            ScannedCode scan = maybe_scan.value();
            auto now = std::chrono::system_clock::now();
            long current_time = std::chrono::system_clock::to_time_t(now);

            if ((current_time - last_scan_time) * 1000 < min_scan_interval_ms) {
                logEvent("REJECTED", "Scan too frequent: " + scan.data);
                continue;
            }
            last_scan_time = current_time;
            scan.timestamp = current_time;

            if (!validateScan(scan)) {
                logEvent("INVALID", "Format rejected: " + scan.data);
                continue;
            }

            bool is_duplicate = false;
            for (const auto &past_scan: scan_history) {
                if (past_scan.data == scan.data &&
                    (scan.timestamp - past_scan.timestamp) < 30) {
                    is_duplicate = true;
                    break;
                }
            }

            if (is_duplicate) {
                logEvent("DUPLICATE", "Scan ignored: " + scan.data);
            } else {
                logEvent("SCAN", "Valid " + scan.format + " code: " + scan.data);

                scan_history.push_back(scan);
                if (scan_history.size() > max_history_size) {
                    scan_history.erase(scan_history.begin());
                }

                sendScanToServer(scan);
            }
        }

        std::this_thread::sleep_for(std::chrono::milliseconds(poll_interval_ms));
    }
}

bool QrScanner::validateScan(const ScannedCode &scan) {
    if (validation_patterns.empty()) {
        return true;
    }

    for (const auto &pattern: validation_patterns) {
        try {
            std::regex re(pattern);
            if (std::regex_match(scan.data, re)) {
                return true;
            }
        } catch (const std::regex_error &e) {
            logEvent("ERROR", "Invalid regex pattern: " + pattern);
        }
    }
    return false;
}

void QrScanner::sendScanToServer(const ScannedCode &scan) {
    char time_buf[100];
    std::strftime(time_buf, sizeof(time_buf), "%FT%TZ", std::gmtime(&scan.timestamp));

    std::string json_body = fmt::format(
        R"({{ "device_id": "{}", "timestamp": "{}", "code_data": "{}", "code_format": "{}", "validated": true }})",
        device_id, time_buf, scan.data, scan.format
    );

    cpr::Response resp = cpr::Post(
        cpr::Url{api_host + "/scans"},
        cpr::Body{json_body}
    );

    if (resp.status_code >= 400) {
        std::cerr << "Failed to send scan data: " << resp.status_code << " - " << resp.text << "\n";
        logEvent("ERROR", "Server rejected scan: " + std::to_string(resp.status_code));
    } else if (resp.status_code == 200) {
        std::cout << "Scan data sent and processed successfully.\n";
    } else {
        std::cout << "Scan data sent. Status: " << resp.status_code << "\n";
    }
}

void QrScanner::logEvent(const std::string &event, const std::string &detail) {
    auto now = std::chrono::system_clock::now();
    std::time_t now_time = std::chrono::system_clock::to_time_t(now);
    char time_buf[100];
    std::strftime(time_buf, sizeof(time_buf), "%T", std::localtime(&now_time));
    std::cout << "[" << time_buf << "] " << event << ": " << detail << "\n";
}

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