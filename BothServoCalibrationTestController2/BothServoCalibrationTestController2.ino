
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

const int capacitivePinRight = T7;
const int capacitivePinLeft = T8; 

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

  int touchValRight = touchRead(capacitivePinRight);
  int touchValLeft = touchRead(capacitivePinLeft);


  if (Serial.available() > 0) {
    String receivedData = Serial.readStringUntil('\n');
    receivedData.trim();
    int commaIndex = receivedData.indexOf(',');
    if (commaIndex != -1) {
 // Extract the values before and after the comma
      String firstValue = receivedData.substring(0, commaIndex);
      String secondValue = receivedData.substring(commaIndex + 1);

      // Convert the string values to integers or other data types if needed
      firstIntValue = constrain(firstValue.toInt(), 0, 120);
      secondIntValue = constrain(secondValue.toInt(), 0, 120);
      gripperServo.write(firstIntValue);
      armServo.write(secondIntValue);
    }
  }
  Serial.println("Gripper: " + String(firstIntValue) + "\tArm: " +  String(secondIntValue) + "\tCapRight: " + String(touchValRight) + "\tCapLeft: " + String(touchValLeft));
  Serial.flush();
  delay(50);
}