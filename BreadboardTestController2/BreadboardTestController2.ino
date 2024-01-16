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

int gripperAngle =  60;
int armAngle =  60;

unsigned long mytime;

const int capacitivePinRight = T5;
const int capacitivePinLeft = T8;

const int TOUCH_THRESHOLD = 35;

void setup()
{
  Serial.begin(115200);
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
  
  if(touchValRight < TOUCH_THRESHOLD){
    gripperAngle = 120;
  }
  else {
    gripperAngle = 60;
  }

  if(touchValLeft < TOUCH_THRESHOLD){
    armAngle = 100;
  }
  else {
    armAngle = 60;
  }

  gripperServo.write(gripperAngle);
  armServo.write(armAngle);

  gripperServoRead = analogRead(gripperServoReadPin);
  armServoRead = analogRead(armServoReadPin);

  char buffer[120];
  sprintf(buffer, "gripperAngle: %3u armAngle: %3u S1 us: %4u S2 us: %4u S1 FB: %4u S2 FB: %4u Cap1: %3u Cap2: %3u", gripperAngle, armAngle, gripperServo.readMicroseconds(), armServo.readMicroseconds(), gripperServoRead, armServoRead, touchValRight, touchValLeft);
  Serial.println(buffer);
  // Serial.print("gripperAngle:");
  // Serial.print(gripperAngle);
  // Serial.print("\t");
  // Serial.print("Feedback1:");
  // Serial.println(gripperServoRead);
  // Serial.print("gripperAngle: " + (String)gripperAngle + "\t" + "\tS1 us: " + String(gripperServo.readMicroseconds()) + "\tS1 FB: " + String(gripperServoRead) + "\tCap1: " + (String)touchVal1);
  // Serial.println("\tarmAngle: " + (String)armAngle + "\t" + "\tS2 us: " + String(armServo.readMicroseconds()) + "\tS2 FB: " + String(armServoRead) + "\tCap2: " + (String)touchVal2);
  delay(50);

}
