void setup() {
  Serial.begin(115200);
  while (!Serial);
}

void loop() {

  String receivedString = "";


  if (Serial.available() > 0) {
    receivedString = Serial.readStringUntil('\n');
    receivedString.trim();
    if (receivedString == "123") {
      Serial.println("I received 123");

    }
    else if (receivedString == "456") {
      Serial.println("I received 456");
    }
    else {
      Serial.println("I received something else." + receivedString);
    }
    Serial.flush();
  }
}