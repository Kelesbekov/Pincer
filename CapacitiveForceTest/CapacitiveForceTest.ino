
int servo1Pin = MOSI;
int servo2Pin = TX;

int ADC_Max = 4096;

const int capacitivePin1 = T9;
const int capacitivePin2 = T8;

const int TOUCH_THRESHOLD = 40;

void setup()
{
  Serial.begin(115200);

}

void loop() {
  int lo = 0;
  int hi = 80;
  int touchVal1 = touchRead(capacitivePin1);
  int touchVal2 = touchRead(capacitivePin2);
 
  Serial.print("Lo:");
  Serial.print(lo);
  Serial.print(",");
  Serial.print("Hi:");
  Serial.print(hi);
  Serial.print(",");
  Serial.print("Threshold:");
  Serial.print(TOUCH_THRESHOLD);
  Serial.print(",");
  Serial.print("C1:");
  Serial.print(touchVal1);
  Serial.print(",");
  Serial.print("C2:");
  Serial.println(touchVal2);
  delay(50);

}

