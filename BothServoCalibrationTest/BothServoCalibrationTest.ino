
#include <ESP32Servo.h> 

Servo gripperServo;
Servo armServo;

int gripperServoPin = MOSI;
int armServoPin = TX;

int gripperServoReadPin = A3;
int armServoReadPin = A2;

int gripperServoRead;
int armServoRead;

int gripperAngle = 34;
int armAngle = 30;

int gripperVal;
int armVal;

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

  gripperServo.setPeriodHertz(50);
  gripperServo.attach(gripperServoPin, 500, 2400);
  gripperServo.write(gripperAngle);

  armServo.setPeriodHertz(50);
  armServo.attach(armServoPin, 500, 2400);
  armServo.write(armAngle);

}

void loop() {

  gripperServoRead = analogRead(gripperServoReadPin);
  armServoRead = analogRead(armServoReadPin);

  
  if (Serial.available() > 0) {
    String receivedData = Serial.readStringUntil('\n');
    receivedData.trim();
    int commaIndex = receivedData.indexOf(',');
    if (commaIndex != -1) {
 // Extract the values before and after the comma
      String firstValue = receivedData.substring(0, commaIndex);
      String secondValue = receivedData.substring(commaIndex + 1);

      // Convert the string values to integers or other data types if needed
      gripperVal = constrain(firstValue.toInt(), 34, 120);
      armVal = constrain(secondValue.toInt(), 0, 120);
      gripperServo.write(gripperVal);
      armServo.write(armVal);
      
  }
}
      Serial.println("a: " + String(gripperVal) + "\tb: " +  String(armVal) + "\treadGripper: " + String(gripperServoRead) + "\treadArm: " + String(armServoRead));
      Serial.flush();
      delay(100);
}
