/////////////////////////////////////////////////////////////////////////////

#include <ESP32Servo.h> 

Servo myservo1;
Servo myservo2;

int servo1Pin = MOSI;
int servo2Pin = TX;

int servo1ReadPin = A3;
int servo2ReadPin = A2;

int servo1Read;
int servo2Read;

int angle1 = 30;
int angle2 = 30;


const int capacitivePin1 = T9;
const int capacitivePin2 = T8;

bool capacitiveLogic1 = false;
bool capacitiveLogic2 = false; 

const int TOUCH_THRESHOLD = 30;

String receivedString = "";
String stringToSend = "";

unsigned long prevMillis = 0;


void setup()
{
  Serial.begin(115200);
  while (!Serial);

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
    capacitiveLogic1 = true;
  }
  else {
    capacitiveLogic1 = false;
  }

  if (touchVal2 < TOUCH_THRESHOLD){
    capacitiveLogic2 = true;
  }
  else {
    capacitiveLogic2 = false;
  }


  if (Serial.available() > 0) {
    receivedString = Serial.readStringUntil('\n');
    receivedString.trim();
    if (receivedString == "Cube1") {
    }
    else if (receivedString == "Cube2") {
    }
    else {
    }
  }

  if (millis() - prevMillis > 10) {
    prevMillis = millis();
    stringToSend = String(int(capacitiveLogic1)) + String(int(capacitiveLogic2));
    Serial.println(stringToSend);
    Serial.flush();
  }
  
}


