
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
  if (Serial.available() > 0) {
    String receivedData = Serial.readStringUntil('\n');
    receivedData.trim();
    int commaIndex = receivedData.indexOf(',');
    if (commaIndex != -1) {
 // Extract the values before and after the comma
      String firstValue = receivedData.substring(0, commaIndex);
      String secondValue = receivedData.substring(commaIndex + 1);

      // Convert the string values to integers or other data types if needed
      int firstIntValue = constrain(firstValue.toInt(), 30, 120);
      int secondIntValue = constrain(secondValue.toInt(), 0, 120);
      myservo1.write(firstIntValue);
      myservo2.write(secondIntValue);
      Serial.println("a: " + String(firstIntValue) + "\tb: " +  String(secondIntValue));
      Serial.flush();
  }
}
}
