/////////////////////////////////////////////////////////////////////////////

#include <ESP32Servo.h> 

class SlowServo {
  protected:
  uint16_t target = 35;
  uint16_t current = 35;
  uint8_t interval = 10;
  uint32_t previousMillis = 0;

  public:
  Servo servo;
  
  void attach(byte pin)
  {
    servo.attach(pin);
  }

  void setSpeed(uint8_t newSpeed)
  {
    interval = newSpeed;
  }

  void set(uint16_t newTarget)
  {
    target = newTarget;
  }

  void update()
    {
      if (millis() - previousMillis > interval)
      {
        previousMillis = millis();
        if (target < current)
        {
          current--;
          servo.write(current);
        }
        else if (target > current)
        {
          current++;
          servo.write(current);
        }
      }
    }
};



int servo1Pin = MOSI;
int servo2Pin = TX;

int servo1ReadPin = A3;
int servo2ReadPin = A2;

int servo1Read;
int servo2Read;

int angle1 = 30;
int angle2 = 40;

int commaIdx, firstItem, secondItem;

const int capacitivePin1 = T9;
const int capacitivePin2 = T8;

bool capacitiveLogic1 = false;
bool capacitiveLogic2 = false; 

bool proximity = false;

const int TOUCH_THRESHOLD = 30;

String receivedString = "";
String stringToSend = "";

unsigned long prevMillis = 0;

SlowServo myservo1;
SlowServo myservo2;


void setup()
{
  Serial.begin(115200);
  while (!Serial);

	// Allow allocation of all timers
	ESP32PWM::allocateTimer(0);
	ESP32PWM::allocateTimer(1);
	ESP32PWM::allocateTimer(2);
	ESP32PWM::allocateTimer(3);

  myservo1.servo.setPeriodHertz(50);
  myservo1.attach(servo1Pin);
  myservo1.set(angle1);

  myservo2.servo.setPeriodHertz(50);
  myservo2.attach(servo2Pin);
  myservo2.set(angle2);

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


  // read as fast as possible
   if (Serial.available() > 0) {
     receivedString = Serial.readStringUntil('\n');
     receivedString.trim();
     commaIdx = receivedString.indexOf(',');
     firstItem = receivedString.substring(0,commaIdx).toInt();
     secondItem = receivedString.substring(commaIdx+1).toInt();
    proximity = (firstItem == 1 ? true : false);
     angle2 = (proximity ? 10 : 40);
     angle1 = (secondItem == 1 ? 70 : (secondItem == 0 ? 90 : 30));
     myservo1.set(constrain(angle1, 30, 90));
     myservo2.set(constrain(angle2, 10, 40));
   }

  

  
    myservo1.update();
 
    myservo2.update();
  
  // if (millis() - prevMillis > 1000) 
  // {
  //   prevMillis = millis();
  //   Serial.println(String(firstItem) + ":" + String(secondItem) + ";" + String(angle1) + "--" + String(angle2));
  //   Serial.flush();
  // }

  //Sending with delay 10ms
  if (millis() - prevMillis > 10) {
    prevMillis = millis();
    stringToSend = String(int(capacitiveLogic1)+3) + String(int(capacitiveLogic2)+3);
    Serial.println(stringToSend);
    Serial.flush();
  }
  
}


