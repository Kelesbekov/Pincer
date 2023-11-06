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

  Serial.println("Adafruit INA260 Test");

  if (!ina260.begin()) {
    Serial.println("Couldn't find INA260 chip");
    while (1);
  }
  Serial.println("Found INA260 chip");
  myservo1.attach(servo1Pin, 500, 2400);
  myservo1.write(angle1);
  avgCurrentObject.begin();
}

void loop() {
  int readCurrent = ina260.readCurrent();
  int avgCurrent = avgCurrentObject.reading(readCurrent);
  Serial.print("AverageCurrent:");
  Serial.print(avgCurrent);
  Serial.print("\t");
  Serial.print("ReadingCurrent:");
  Serial.println(readCurrent);
  //Serial.println(" mA");
  // Serial.print("\t");
  // Serial.print("Bus Voltage:");
  // Serial.print(ina260.readBusVoltage());
  // //Serial.println(" mV");
  // Serial.print("\t");
  // Serial.print("Power:");
  // Serial.println(ina260.readPower());
  //Serial.println(" mW");

  delay(50);
}
