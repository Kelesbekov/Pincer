/////////////////////////////////////////////////////////////////////////////

#include <ESP32Servo.h> 

// Servo Objects
Servo gripperServo;
Servo armServo;


// Servo PWM pins
int gripperServoPin = A1;
int armServoPin = A5;


// Servo feedback pins
int gripperServoReadPin = A4;
int armServoReadPin = A2;

// Servo feedback values
int gripperServoRead;
int armServoRead;


// Servo angles
int gripperAngle = 60;
int armAngle = 60;


// Indeces
int comma1, comma2;


// Capacitance vars
const int capacitivePinRight = T5;
const int capacitivePinLeft = T8; 

bool capacitiveLogic1 = false;
bool capacitiveLogic2 = false; 

const int TOUCH_THRESHOLD1 = 35;
const int TOUCH_THRESHOLD2 = 40;


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
int armClose = 60;
int armFar = 100;
int gripperClosed = 22;
int gripperOpen = 100;

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
    armAngleRx = receivedString.substring(comma2+1).toInt();

    switch (mode) {
      // Standby mode (do nothing)
      case -1:
        gripperAngle = gripperAngleRx;
        armAngle = armAngleRx;
        break;
      // Calibration mode (adjust the armClose constant)
      case 0:
        gripperAngle = gripperAngleRx;
        armAngle = armAngleRx;
        armClose = armAngleRx;
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

    gripperServo.write(constrain(gripperAngle, 22, 150));
    armServo.write(constrain(armAngle, 50, 100));
  


  // Check for touch
  int touchVal1 = touchRead(capacitivePinRight);
  int touchVal2 = touchRead(capacitivePinLeft);
  
  capacitiveLogic1 = (touchVal1 < TOUCH_THRESHOLD1 ? true : false);
  capacitiveLogic2 = (touchVal2 < TOUCH_THRESHOLD2 ? true : false);

  //Sending with delay 10ms
  // if (millis() - prevMillis > 10) {
  //   prevMillis = millis();
  //   stringToSend = (String)armAngleRx + (String)gripperAngleRx;
  //   Serial.println(stringToSend);
  //   Serial.flush();
  // }

  if (millis() - prevMillis > 10) {
    prevMillis = millis();
    stringToSend = String(int(capacitiveLogic1)+3) + String(int(capacitiveLogic2)+3);
    Serial.println(stringToSend);
    Serial.flush();
  }
  
}


