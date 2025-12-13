#ifndef SPOTRENT_IOT_QR_SCANNER_H
#define SPOTRENT_IOT_QR_SCANNER_H

#pragma once

#include <string>
#include <toml.hpp>
#include <vector>
#include <qr_scanner.h>
#include <smart_lock.h>

using namespace std;

class Device;

class Device {
private:

    QrScanner scanner;
    SmartLock lock;

    void logEvent(const int userId, const int bookingId, const int accessType, 
        const bool isSuccessfull, const string& errorMessage);
    void logEventOwner(const int userId, const int accessType, 
        const bool isSuccessfull, const string& errorMessage);
    bool registerDevice();

public:
    explicit Device(const toml::table& config);
    ~QrScanner();
    void addQrScanner();
    void addSmartLock();
    void run();
    void lock();
    void unlock();
};

#endif
