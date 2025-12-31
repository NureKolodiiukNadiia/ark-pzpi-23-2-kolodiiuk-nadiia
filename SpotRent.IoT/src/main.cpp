#include <Arduino.h>
#include <WiFi.h>
#include <Wire.h>
#include <LiquidCrystal_I2C.h>
#include <ESP32Servo.h>
#include <device.h>
#include <HTTPClient.h>
#include <WiFiClientSecure.h>

const std::string host = "https://irrigative-bessie-evidentially.ngrok-free.dev/api";

const int id = 1;
const int default_user_id = 101;
const int default_booking_id = 2001;
const int register_on_start = true;

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

void unlockDoor(const std::string& qrData, int user_id = default_user_id, bool isOwnerOverride = false) {
  bool res = device->unlock(user_id, qrData, isOwnerOverride);
  if (res) {
    locked = false;
    lockServo.write(unlockPosition);
    updateLockStatus();
  }
  else {
    lcd.print("Couldn't unlock");
  }
}

void lockDoor(const std::string& qrData, int user_id = default_user_id, bool isOwnerOverride = false) {
  bool res = device->lock(user_id, qrData, isOwnerOverride);
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
  std::string qrStd = qrData.c_str();

  if (qrData == "LOCK") {
    lockDoor(qrStd);
  } else if (qrData == "UNLOCK") {
    unlockDoor(qrStd);
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
    return;
  }

  const char* ssid = "Wokwi-GUEST";
  const char* password = "";
  
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
      delay(500);
      Serial.println("Connecting to WiFi...");
  }

  device->addSmartLock();

  bool res = device->registerDevice();
  if (res == false) {
    return;
  }

  lockDoor("LOCK");

  Serial.println("READY");
}

void loop() {
  if (Serial.available()) {
    String qrData = Serial.readStringUntil('\n');
    handleQRCode(qrData);
  }
}
