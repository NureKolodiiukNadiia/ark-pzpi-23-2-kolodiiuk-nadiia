#include <Arduino.h>
#include <WiFi.h>
#include <Wire.h>
#include <LiquidCrystal_I2C.h>
#include <ESP32Servo.h>

const int servoPin = 14;
const int unlockPosition = 0;
const int lockPosition = 180;

Servo lockServo;
LiquidCrystal_I2C lcd(0x27, 16, 2);

bool locked = true;

void updateLockStatus() {
  lcd.clear();
  lcd.print(locked ? "Door Locked" : "Door Unlocked");
}

void unlockDoor() {
  locked = false;
  lockServo.write(unlockPosition);
  updateLockStatus();
}

void lockDoor() {
  locked = true;
  lockServo.write(lockPosition);
  updateLockStatus();
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
  delay(2000);                 // <-- CRITICAL for Wokwi
  Serial.begin(9600);

  lockServo.attach(servoPin);

  lcd.init();
  lcd.backlight();

  lockDoor();

  Serial.println("READY");     // <-- confirms input mode
}

void loop() {
  if (Serial.available()) {
    String qrData = Serial.readStringUntil('\n');
    handleQRCode(qrData);
  }
}
