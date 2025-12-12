#ifndef COWORKINGACCESS_IOT_QR_SCANNER_H
#define COWORKINGACCESS_IOT_QR_SCANNER_H
#include "scanner_source.h"

#endif //COWORKINGACCESS_IOT_QR_SCANNER_H

#pragma once
#include <string>
#include <toml.hpp>
#include <cpr/cpr.h>
#include <vector>

class ScannerSource; // Forward declaration

class QrScanner {
private:
    std::string api_host;
    std::string device_id;
    unsigned int poll_interval_ms;

    ScannerSource* scanner_source;

    // History and rate limiting
    std::vector<ScannedCode> scan_history;
    size_t max_history_size;
    long last_scan_time = 0;
    int min_scan_interval_ms; // Prevent duplicate scans

    // Validation patterns (e.g., for URL, UUID, custom format)
    std::vector<std::string> validation_patterns;

    void sendScanToServer(const ScannedCode& scan);
    bool validateScan(const ScannedCode& scan);
    void logEvent(const std::string& event, const std::string& detail = "");

public:
    explicit QrScanner(const toml::table& config);
    ~QrScanner();
    void run(); // Main loop
};
