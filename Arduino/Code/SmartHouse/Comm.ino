//*************************//
//*** Command Functions ***//
//*************************//

void initialize() {
  initialized = true;
  isSystemArmed = (Serial.parseInt()==1)? true:false;
  activityLevel = Serial.parseInt();

  sprintf(temporary, "I:%d\n", (isSystemArmed)? 1:0);
  Serial.write(temporary);
}

void confirmInitialize() {
  Serial.parseInt();
  
  sprintf(temporary, "I:%d\n", (initialized)? 1:0);
  Serial.write(temporary);
}


void changeSystemArmed() {
  isSystemArmed = (Serial.parseInt()==1)? true:false;
  
  sprintf(temporary, "S:%d\n", (isSystemArmed)? 1:0);
  Serial.write(temporary);
}


void changeActivityLevel() {
  activityLevel = Serial.parseInt();
  
  sprintf(temporary, "A:%d\n", activityLevel);
  Serial.write(temporary);
}


void changeLightStatus() {
  isLightOn = (Serial.parseInt()==1)? true:false;
  digitalWrite(powerSwitchTailPin, (isLightOn)? LOW : HIGH);
  currDebounceDelay = millis();
  
  sprintf(temporary, "L:%d\n", (isLightOn)? 1:0);
  Serial.write(temporary);
}

//*******************************//
//*** Communication Functions ***//
//*******************************//

void sendFullStatus() {
  Serial.read();

  sprintf(temporary, "S:%d\n", (isSystemArmed)? 1:0);
  Serial.write(temporary);

  sprintf(temporary, "T:%d\n", (int)temperature);
  Serial.write(temporary);

  sprintf(temporary, "L:%d\n", (isLightOn)? 1:0);
  Serial.write(temporary);

  sprintf(temporary, "D:%d\n", (isDoorOpen)? 1:0);
  Serial.write(temporary);

  sprintf(temporary, "A:%d\n", activity);
  Serial.write(temporary);
}



void sendRawActivityStatus() {
  Serial.read();

  sprintf(temporary, "A:%d\n", activity);
  Serial.write(temporary);

  sprintf(temporary, "U:%d\n", audioActivity);
  Serial.write(temporary);

  sprintf(temporary, "M:%d\n", motionActivity);
  Serial.write(temporary);
}


//*************************//
//*** Warning Functions ***//
//*************************//
void sendAboveActivityWarning() {
  sprintf(temporary, "W:%d\n", activity);
  Serial.write(temporary);
}



void sendDoorChangeWarning() {
  sprintf(temporary, "D:%d\n", (isDoorOpen)? 1:0);
  Serial.write(temporary);
}



void sendLightChangeWarning() {
  sprintf(temporary, "L:%d\n", (isLightOn)? 1:0);
  Serial.write(temporary);
}



//************************//
//*** Message Handling ***//
//************************//
boolean haveMessages() {
  return (Serial.available() > 0);
}



void handleMessages() {
  switch(Serial.peek()) {
  case 'F':
    sendFullStatus();
    break;
  case 'R':
    sendRawActivityStatus();
    break;
  case 'S':
    changeSystemArmed();
    break;
  case 'A':
    changeActivityLevel();
    break;
  case 'L':
    changeLightStatus();
    break;
  case 'I':
    confirmInitialize();
    break;
  default:
    Serial.read();
    break;
  }
}

