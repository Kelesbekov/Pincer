#include <ESP32Servo.h> 

constexpr uint8_t servo1Pin = MOSI;
int servo1ReadPin = A3;
uint16_t servo1Read;

class SlowServo {
  protected:
    uint16_t target = 40;       // target angle
    uint16_t current = 40;      // current angle
    uint8_t interval = 20;      // delay time
    uint32_t previousMillis = 0;
    Servo servo;

    public:
    void begin(byte pin)
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

SlowServo myservo1;  // create a servo object

uint32_t prev = 0;
uint8_t x = 0;


void setup() {
  Serial.begin(115200);
  myservo1.begin(servo1Pin);
  myservo1.set(40);
}

void loop() {

  if 
  (millis() - prev > 2000) {
    prev = millis();
    if (x == 0) {
      myservo1.set(40);
      x += 1;
    }
    else {
      myservo1.set(70);
      x-= 1;
    }
  }

  myservo1.update();
}
