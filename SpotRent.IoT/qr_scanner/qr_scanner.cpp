#include "qr_scanner.h"

#include <iostream>

QrScanner::QrScanner(const toml::table& config)
    : poll_interval_ms(config["scanner"]["poll_interval_ms"].value_or(1000u)),
      next_index(0) {
    if (auto scans = config["scanner"]["simulated_scans"].as_array()) {
        for (const auto& item : *scans) {
            if (!item.is_table()) {
                continue;
            }

            const auto& scan_table = *item.as_table();

            QrScanEvent event{};
            event.userId = scan_table["user_id"].value_or(0);
            event.bookingId = scan_table["booking_id"].value_or(0);

            if (auto v = scan_table["qr_code"].value<std::string>()) {
                event.qrCode = *v;
            }

            event.accessType = scan_table["access_type"].value_or(0);
            event.isOwner = scan_table["is_owner"].value_or(false);
            event.shouldUnlock = scan_table["should_unlock"].value_or(true);
            event.pauseAfterMs = scan_table["pause_after_ms"].value_or(0u);

            simulated_events.push_back(event);
        }
    }

    if (simulated_events.empty()) {
        QrScanEvent defaultEvent{};
        defaultEvent.userId = 1;
        defaultEvent.bookingId = 1;
        defaultEvent.qrCode = "SIMULATED-DEFAULT";
        simulated_events.push_back(defaultEvent);
    }

    std::cout << "[INFO] QR scanner prepared with " << simulated_events.size()
              << " simulated scans.\n";
}

bool QrScanner::next(QrScanEvent& event) {
    if (next_index >= simulated_events.size()) {
        return false;
    }

    event = simulated_events[next_index++];
    return true;
}

unsigned int QrScanner::pollIntervalMs() const {
    return poll_interval_ms;
}
