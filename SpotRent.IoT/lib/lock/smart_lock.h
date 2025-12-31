#ifndef SPOTRENT_IOT_SMART_LOCK_H
#define SPOTRENT_IOT_SMART_LOCK_H

#include <string>

class SmartLock {
public:
    explicit SmartLock(const std::string& initial_state = "locked");

    bool lock(int device_id, int user_id, const std::string& qrCode, bool isOwnerOverride);
    bool unlock(int device_id, int user_id, const std::string& qrCode, bool isOwnerOverride);

    [[nodiscard]] bool isLocked() const;
    [[nodiscard]] unsigned int relockDelayMs() const;
    [[nodiscard]] std::string status() const;

private:
    bool locked;
    unsigned int relock_delay_ms;
    std::string api_host;

    bool postJson(const std::string& path, const std::string& jsonBody) const;
    bool postJsonWithPayload(const std::string& path, const std::string& jsonBody, std::string* outPayload) const;
};

#endif
