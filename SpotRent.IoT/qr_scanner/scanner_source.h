#ifndef COWORKINGACCESS_IOT_SCANNER_SOURCE_H
#define COWORKINGACCESS_IOT_SCANNER_SOURCE_H

#endif //COWORKINGACCESS_IOT_SCANNER_SOURCE_H

#pragma once

#include <string>
#include <optional>

struct ScannedCode {
    std::string data;
    std::string format; // e.g., "QR_CODE", "CODE_128"
    long timestamp;
};

class ScannerSource {
public:
    virtual ~ScannerSource() = default;
    virtual std::optional<ScannedCode> getScan() = 0;
    virtual void triggerScan() = 0; // For manual trigger simulation
};
