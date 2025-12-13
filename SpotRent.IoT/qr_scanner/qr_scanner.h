#ifndef SPOTRENT_IOT_QR_SCANNER_H
#define SPOTRENT_IOT_QR_SCANNER_H
#include "scanner_source.h"

#pragma once
#include <string>
#include <toml.hpp>
#include <cpr/cpr.h>
#include <vector>
struct ScannedCode 
{
    std::string data;
    long timestamp;
};

class ScannerSource {
public:
    virtual ~ScannerSource() = default;
    virtual std::optional<ScannedCode> getScan() = 0;
    virtual void triggerScan() = 0; // For manual trigger simulation
};


class ScannerSource;

class QrScanner {
private:
    std::string api_host;
    std::string device_id;
    unsigned int poll_interval_ms;

    ScannerSource* scanner_source;
public:
    explicit QrScanner(const toml::table& config);
    ~QrScanner();
    void run();
};

#endif
