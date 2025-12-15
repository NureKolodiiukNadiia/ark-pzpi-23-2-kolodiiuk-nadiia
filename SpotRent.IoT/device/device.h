#ifndef SPOTRENT_IOT_DEVICE_H
#define SPOTRENT_IOT_DEVICE_H

#include <memory>
#include <string>
#include <toml.hpp>
#include <vector>

class QrScanner;
class SmartLock;

class Device {
public:
    explicit Device(const toml::table& config);
    ~Device();

    void addQrScanner();
    void addSmartLock();
    void run();
    bool lock();
    bool unlock();

private:
    toml::table config_table;
    int device_id;
    std::string api_host;
    bool auto_register;
    int default_booking_id;

    std::unique_ptr<QrScanner> scanner;
    std::unique_ptr<SmartLock> smart_lock;

    bool registerDevice() const;
    void logEvent(int userId, int bookingId, int accessType, bool isSuccessful, const std::string& errorMessage) const;
    void logEventOwner(int userId, int accessType, bool isSuccessful, const std::string& errorMessage) const;
    void updateDeviceStatus(const std::string& statusMessage, bool isOnline) const;
    bool postJson(const std::string& path, const std::string& jsonBody) const;
};

#endif
