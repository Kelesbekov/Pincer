
float xf, yf, zf, wf, af, bf;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  String x = "1.2";
  String y = "1.23";
  String z = "1.234";
  String w = "123.456";
  String a = "123456789";
  String b = "123.4567891234";

  xf = x.toFloat();
  yf = y.toFloat();
  zf = z.toFloat();
  wf = w.toFloat();
  af = a.toFloat();
  bf = b.toFloat();

  while (!Serial);

  Serial.print(xf);
  Serial.print("\t");
  Serial.print(yf);
  Serial.print("\t");
  Serial.print(zf);
  Serial.print("\t");
  Serial.print(wf);
  Serial.print("\t");
  Serial.print(af);
  Serial.print("\t");
  Serial.println(bf);
}

void loop() {
  // put your main code here, to run repeatedly:
  delay(1000);
  Serial.print(xf, 10);
  Serial.print("\t");
  Serial.print(yf, 10);
  Serial.print("\t");
  Serial.print(zf, 10);
  Serial.print("\t");
  Serial.print(wf, 10);
  Serial.print("\t");
  Serial.print(af, 10);
  Serial.print("\t");
  Serial.println(bf, 10);
}
