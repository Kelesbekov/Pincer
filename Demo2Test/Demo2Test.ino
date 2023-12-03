/////////////////////////////////////////////////////////////////////////////

#include <ESP32Servo.h> 

// Servo Objects
Servo gripperServo;
Servo armServo;


// Servo PWM pins
int gripperServoPin = MOSI;
int armServoPin = TX;


// Servo feedback pins
int gripperServoReadPin = A3;
int armServoReadPin = A2;


// Servo feedback values
int gripperServoRead;
int armServoRead;


// Servo angles
int gripperAngle = 30;
int armAngle = 40;


// Indeces
int comma1, comma2;


// Capacitance vars
const int capacitivePin1 = T9;
const int capacitivePin2 = T8;

bool capacitiveLogic1 = false;
bool capacitiveLogic2 = false; 

const int TOUCH_THRESHOLD = 30;


// Object interaction variables
int mode = -1;
int gripperAngleRx;
int armAngleRx;

// Transmission message buffers
String receivedString = "";
String stringToSend = "";

// Transmission delay counter
unsigned long prevMillis = 0;

// PWM constants/variables
int armClose = 10;
int armFar = 40;
int gripperClosed = 30;
int gripperOpen = 70;

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

  // Receive Messages (as soon as they arrive)
   if (Serial.available() > 0) {
    receivedString = Serial.readStringUntil('\n');
    receivedString.trim();

    comma1 = receivedString.indexOf(',');
    comma2 = receivedString.indexOf(',', comma1 + 1);

    mode = receivedString.substring(0,comma1).toInt();
    gripperAngleRx = receivedString.substring(comma1 + 1, comma2).toInt();
    armAngleRx = receivedString.substring(comma2).toInt();

    switch (mode) {
      // Standby mode (do nothing)
      case -1:
        gripperAngle = gripperClosed;
        armAngle = armFar;
        break;
      // Calibration mode (adjust the armClose constant)
      case 0:
        gripperAngle = gripperOpen;
        armAngle = armClose = armAngleRx;
        break;
      // Encountered-type 
      case 1:
        armAngle = armAngleRx;
        gripperAngle = gripperAngleRx;
        break;
      // Constant-contact
      case 2:
        armAngle = armClose;
        gripperAngle = gripperAngleRx;
        break;

     }
   }

    gripperServo.write(constrain(gripperAngle, 30, 90));
    armServo.write(constrain(armAngle, 10, 40));
  


  // Check for touch
  int touchVal1 = touchRead(capacitivePin1);
  int touchVal2 = touchRead(capacitivePin2);
  
  capacitiveLogic1 = (touchVal1 < TOUCH_THRESHOLD ? true : false);
  capacitiveLogic2 = (touchVal2 < TOUCH_THRESHOLD ? true : false);

  //Sending with delay 10ms
  if (millis() - prevMillis > 10) {
    prevMillis = millis();
    stringToSend = String(int(capacitiveLogic1)+3) + String(int(capacitiveLogic2)+3);
    Serial.println(stringToSend);
    Serial.flush();
  }
  
}


