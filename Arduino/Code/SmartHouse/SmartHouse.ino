/*
 *  Smart House - Sensor Center - Arduino Code
 *  Authors: Joe Greubel, YongChing Tee, Derek Gobin, Jake Conroy
 *  
 *  Inputs: two motion sensors, two audio intensity sensors, a reed switch, a temperature sensor and serial communication
 *  Outputs: a power switch tail and serial communication
 *
 *  Description: Using the readings from the sensors and information via the serial connection, this system is able
 *  to monitor room activity based on motion and sound, determine the status of a door and the ability to switch
 *  on and off a lamp.
 */

//************************//
//*** Global Variables ***//
//************************//

//Real World Variables
boolean initialized = false;
boolean isSystemArmed = false;
boolean isDoorOpen = false;
boolean isLightOn = false;
float temperature = 0; 

//Sensor Pins
const int audioSensorPin1 = A0;
const int audioSensorPin2 = A1;
const int motionSensorPin1 = A2;
const int motionSensorPin2 = A3;
const int tempSensorPin = A5;

const int reedSwitchPin = 6;
const int powerSwitchTailPin = 7;
const int lampPushInterrupt = 0; //Pin 2

//Activity Variables
long activity = 0;
int activityLevel = 0;
int audioActivity = 0;
int motionActivity = 0;
const double audioActivityScale = 1;
const double motionActivityScale = .11;

//Light Debounce
long long currDebounceDelay = 500;

//Global String Buffer
char temporary[10];

//*************************//
//*** Arduino Functions ***//
//*************************//
void setup() {
  //Wait For HomeServer Initialization
  Serial.begin(115200);
  while(Serial.read() != 'I');
  initialize();

  //Setup Pins
  pinMode(reedSwitchPin, INPUT);
  pinMode(powerSwitchTailPin, OUTPUT);

  //Attach Interrupt
  attachInterrupt(lampPushInterrupt, lampButtonFunc, RISING);
}

void loop() {
  //Check And Update Systems
  checkActivity();
  checkDoor();
  updateTemperature();

  //Check For Messages
  if (haveMessages()) {
    handleMessages();
  }
  
  /*
  //Test Prints
  const long testDelay = 100;
  static long currTestDelay = -testDelay;
  if(millis() - currTestDelay > testDelay) {
    
    Serial.print(motionActivity);
    Serial.print(",");
    
    currTestDelay = millis();
  }
  */
}
