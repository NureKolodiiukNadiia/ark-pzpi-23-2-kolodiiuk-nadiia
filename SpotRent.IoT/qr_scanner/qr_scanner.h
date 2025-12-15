#ifndef SPOTRENT_IOT_QR_SCANNER_H
#define SPOTRENT_IOT_QR_SCANNER_H

#include <cstddef>
#include <string>
#include <toml.hpp>
#include <vector>

struct QrScanEvent {
    int userId{0};
    int bookingId{0};
    std::string qrCode;
    int accessType{0};
    bool isOwner{false};
    bool shouldUnlock{true};
    unsigned int pauseAfterMs{0};
};

class QrScanner {
public:
    explicit QrScanner(const toml::table& config);

    bool next(QrScanEvent& event);
    [[nodiscard]] unsigned int pollIntervalMs() const;

private:
    unsigned int poll_interval_ms;
    std::vector<QrScanEvent> simulated_events;
    std::size_t next_index;
};

#endif
