//************************//
//*** Device Functions ***//
//************************//

boolean getDoorStatus() {
  return (digitalRead(reedSwitchPin) == 0)? false : true;
}



void checkDoor() {

  const long long doorDelay = 500;
  static long long currDoorDelay = -doorDelay;

  if(millis() - currDoorDelay > doorDelay) {
    if (isDoorOpen != getDoorStatus()) {
      isDoorOpen = !isDoorOpen;
      sendDoorChangeWarning(); 
    }  

    currDoorDelay = millis();
  }
}



void updateTemperature() {
  const long long tempDelay = 5000;
  static long long currTempDelay = -tempDelay;

  if(millis() - currTempDelay > tempDelay) {
    float voltage = (float)analogRead(tempSensorPin) * 5.0 / 1024.0; 
    float newTemp = (((voltage - 0.5) * 100.0) * 9/5 + 32);

    if(temperature == 0)
      temperature = newTemp;
    else
      temperature = (newTemp + temperature)/2.0;

    currTempDelay = millis();
  }
}



void lampButtonFunc() {
  const long long debounceDelay = 500;

  if(millis() - currDebounceDelay > debounceDelay) {
    isLightOn = !isLightOn;
    digitalWrite(powerSwitchTailPin, (isLightOn)? LOW : HIGH);
    sendLightChangeWarning();
    currDebounceDelay = millis();
  }
}
