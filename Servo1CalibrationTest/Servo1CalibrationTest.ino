#include <Adafruit_INA260.h>
#include <ESP32Servo.h> 
#include <movingAvg.h>

movingAvg avgCurrentObject(10);

Servo myservo1;
int servo1Pin = MOSI;
int servo1ReadPin = A3;
int servo1Read;
int angle1 = 40;



Adafruit_INA260 ina260 = Adafruit_INA260();

void setup() {
  Serial.begin(115200);
  // Wait until serial port is opened
  while (!Serial) { delay(10); }

  myservo1.attach(servo1Pin, 500, 2400);
  myservo1.write(angle1);
  avgCurrentObject.begin();
}

void loop() {
  if (Serial.available() > 0) {
    String receivedString = Serial.readStringUntil('\n');
    receivedString.trim();
    int x = receivedString.toInt();
    x = constrain(x, 0, 180);
    myservo1.write(x);
  }
}
