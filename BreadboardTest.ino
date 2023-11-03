#include <ESP32Servo.h> 

Servo myservo1;
Servo myservo2;

int servo1Pin = MOSI;
int servo2Pin = TX;

int servo1ReadPin = A3;
int servo2ReadPin = A2;

int ADC_Max = 4096;

int servo1Read;
int servo2Read;

int angle1 = 0;
int angle2 = 0;

unsigned long mytime;

const int capacitivePin1 = T9;
const int capacitivePin2 = T8;

const int TOUCH_THRESHOLD = 30;

void setup()
{
  Serial.begin(115200);
	// Allow allocation of all timers
	ESP32PWM::allocateTimer(0);
	ESP32PWM::allocateTimer(1);
	ESP32PWM::allocateTimer(2);
	ESP32PWM::allocateTimer(3);

  myservo1.setPeriodHertz(50);
  myservo1.attach(servo1Pin, 500, 2400);
  myservo1.write(angle1);

  myservo2.setPeriodHertz(50);
  myservo2.attach(servo2Pin, 500, 2400);
  myservo2.write(angle2);

}

void loop() {

  int touchVal1 = touchRead(capacitivePin1);
  int touchVal2 = touchRead(capacitivePin2);
  
  if(touchVal1 < TOUCH_THRESHOLD){
    angle1 = 50;
  }
  else {
    angle1 = 30;
  }

  if(touchVal2 < TOUCH_THRESHOLD){
    angle2 = 50;
  }
  else {
    angle2 = 30;
  }

  myservo1.write(angle1);
  myservo2.write(angle2);

  servo1Read = analogRead(servo1ReadPin);
  servo2Read = analogRead(servo2ReadPin);

  char buffer[120];
  sprintf(buffer, "Angle1: %3u Angle2: %3u S1 us: %4u S2 us: %4u S1 FB: %4u S2 FB: %4u Cap1: %3u Cap2: %3u", angle1, angle2, myservo1.readMicroseconds(), myservo2.readMicroseconds(), servo1Read, servo2Read, touchVal1, touchVal2);
  Serial.println(buffer);
  // Serial.print("Angle1:");
  // Serial.print(angle1);
  // Serial.print("\t");
  // Serial.print("Feedback1:");
  // Serial.println(servo1Read);
  // Serial.print("Angle1: " + (String)angle1 + "\t" + "\tS1 us: " + String(myservo1.readMicroseconds()) + "\tS1 FB: " + String(servo1Read) + "\tCap1: " + (String)touchVal1);
  // Serial.println("\tAngle2: " + (String)angle2 + "\t" + "\tS2 us: " + String(myservo2.readMicroseconds()) + "\tS2 FB: " + String(servo2Read) + "\tCap2: " + (String)touchVal2);
  delay(50);

}

void movement(int from, int towards) {

}

