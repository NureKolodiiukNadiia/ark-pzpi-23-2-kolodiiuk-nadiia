cmake_minimum_required(VERSION 3.16)
project(SpotRent_IoT)

set(CMAKE_CXX_STANDARD 17)

add_executable(SpotRent_IoT
        main.cpp
        device/device.cpp
        device/device.h
        lock/smart_lock.cpp
        lock/smart_lock.h
        qr_scanner/qr_scanner.cpp
        qr_scanner/qr_scanner.h)

find_package(tomlplusplus CONFIG REQUIRED PATHS /opt/tomlplusplus/lib64/cmake/tomlplusplus)
target_include_directories(SpotRent_IoT PRIVATE /opt/tomlplusplus/include/toml++)
target_link_libraries(SpotRent_IoT PRIVATE tomlplusplus::tomlplusplus)

find_package(fmt CONFIG REQUIRED)

target_link_libraries(SpotRent_IoT
        PRIVATE fmt::fmt
)
find_package(cpr CONFIG REQUIRED PATHS /opt/cpr/lib64/cmake/cpr NO_DEFAULT_PATH)

if (NOT TARGET cpr::cpr)
    message(FATAL_ERROR "cpr::cpr target not found. Check your cpr installation.")
endif()

target_sources(SpotRent_IoT
        PRIVATE
        /opt/cpr/include/cpr/cpr.h
)
target_include_directories(SpotRent_IoT PRIVATE /opt/cpr/include /opt/cpr/include/cpr)
target_link_libraries(SpotRent_IoT PRIVATE cpr::cpr)
