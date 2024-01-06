
#include <ESP32Servo.h> 

Servo gripperServo;
Servo armServo;

int gripperServoPin = A1;
int armServoPin = A5;

int gripperServoReadPin = A4;
int armServoReadPin = A2;

int ADC_Max = 4096;

int gripperServoRead;
int armServoRead;

int gripperAngle = 0;
int armAngle = 0;

int firstIntValue;
int secondIntValue;

const int capacitivePinRight = T5;
const int capacitivePinLeft = T8; 

const int TOUCH_THRESHOLD = 30;

String receivedString = "";
String stringToSend = "";

unsigned long prevMillis = 0;


void setup()
{
  Serial.begin(115200);

}

void loop() {
  int lo = 0;
  int hi = 80;
  int touchValRight = touchRead(capacitivePinRight);
  delayMicroseconds(5);
  int touchValLeft = touchRead(capacitivePinLeft);
 
  Serial.print("Lo:");
  Serial.print(lo);
  Serial.print(",");
  Serial.print("Hi:");
  Serial.print(hi);
  Serial.print(",");
  Serial.print("Threshold:");
  Serial.print(TOUCH_THRESHOLD);
  Serial.print(",");
  Serial.print("C1:");
  Serial.print(touchValRight);
  Serial.print(",");
  Serial.print("C2:");
  Serial.println(touchValLeft);
  delay(50);

}

