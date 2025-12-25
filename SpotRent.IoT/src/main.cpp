#include <Arduino.h>
#include <WiFi.h>
#include <Wire.h>
#include <LiquidCrystal_I2C.h>
#include <ESP32Servo.h>
#include <device.h>

// [server]
const std::string host = "http://localhost:5041/api";

// [device]
const int id = 1;
const int default_user_id = 101;
const int default_booking_id = 2001;
const int register_on_start = true;

// [lock]
const std::string initial_state = "locked";
const int relock_delay_ms = 2000;

const int servoPin = 14;
const int unlockPosition = 0;
const int lockPosition = 180;

Servo lockServo;
LiquidCrystal_I2C lcd(0x27, 16, 2);

bool locked = true;

Device *device;

void updateLockStatus() {
  lcd.clear();
  lcd.print(locked ? "Door Locked" : "Door Unlocked");
}

void unlockDoor() {
  bool res = device.unlock();
  if (res) {
    locked = false;
    lockServo.write(unlockPosition);
    updateLockStatus();
  }
  else {
    lcd.print("Couldn't unlock");
  }
}

void lockDoor() {
  bool res = device.lock();
  if (res) {
    locked = true;
    lockServo.write(lockPosition);
    updateLockStatus();
  }
  else {
    lcd.print("Couldn't lock");
  }
}

void handleQRCode(String qrData) {
  qrData.trim();

  if (qrData == "1234") {
    unlockDoor();
  } else if (qrData == "LOCK") {
    lockDoor();
  } else {
    lcd.clear();
    lcd.print("Invalid QR");
    delay(2000);
    updateLockStatus();
  }
}

void setup() {
  delay(2000);
  Serial.begin(9600);

  lockServo.attach(servoPin);

  lcd.init();
  lcd.backlight();


  device = new Device();
  if (device == nullptr) {
    return -1;
  }
  device.addSmartLock();

  bool res = device.registerDevice();
  if (res == false) {
    return -2;
  }

  lockDoor();

  Serial.println("READY");
}

void loop() {
  if (Serial.available()) {
    String qrData = Serial.readStringUntil('\n');
    handleQRCode(qrData);
  }
}
