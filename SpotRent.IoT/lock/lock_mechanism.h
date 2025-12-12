#ifndef COWORKINGACCESS_IOT_LOCK_MECHANISM_H
#define COWORKINGACCESS_IOT_LOCK_MECHANISM_H

#endif //COWORKINGACCESS_IOT_LOCK_MECHANISM_H

#pragma once
#include <string>

class LockMechanism {
public:
    virtual ~LockMechanism() = default;
    virtual bool unlock() = 0;
    virtual bool lock() = 0;
    virtual bool isLocked() = 0;
    virtual std::string getStatus() const = 0; // For more detailed status
};
