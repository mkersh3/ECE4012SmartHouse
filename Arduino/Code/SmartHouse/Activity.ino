//**************************//
//*** Activity Functions ***//
//**************************//

int getAudioActivity() {
  const int noiseScale = 10;
  const int audioArrLength = 50;
  static int indx = 0;
  static long audioAvgArr[audioArrLength];
  static long audioAvgTotal = 0;
  static long audioValue1, audioValue2;

  audioValue1 = analogRead(audioSensorPin1);
  audioValue2 = analogRead(audioSensorPin2);

  audioAvgTotal -= audioAvgArr[indx];
  audioAvgArr[indx] = (audioValue1 + audioValue2) * noiseScale;
  audioAvgTotal += audioAvgArr[indx++];

  if(indx >= audioArrLength)
    indx = 0;

  audioActivity = audioAvgTotal/audioArrLength * audioActivityScale;

  return audioActivity;
}



int getJumpMotionActivity() {
  static long noiseScale = 5;
  static long motionActivity1, motionActivity2;
  static long motionJumpTotalValue = 0;
  long substractValue = 40;

  motionActivity1 = analogRead(motionSensorPin1) - 530;
  motionActivity2 = analogRead(motionSensorPin2) - 530;
  
  if(motionActivity1 < 0) motionActivity1 = 0;
  if(motionActivity2 < 0) motionActivity2 = 0;

  motionJumpTotalValue -= substractValue;
  motionJumpTotalValue += (motionActivity1 + motionActivity2) / noiseScale;

  if(motionJumpTotalValue < 0)
    motionJumpTotalValue = 0;

  return motionJumpTotalValue * motionActivityScale;
}



int getMotionActivity() {
  const int noiseScale = 1;
  const int motionArrLength = 50;
  static int indx = 0;
  static long motionAvgArr[motionArrLength];
  static long motionAvgTotal = 0;

  motionAvgTotal -= motionAvgArr[indx];
  motionAvgArr[indx] = getJumpMotionActivity() * noiseScale;
  motionAvgTotal += motionAvgArr[indx++];

  if(indx >= motionArrLength)
    indx = 0;

  motionActivity = motionAvgTotal/motionArrLength;

  return motionActivity;
}


int getCurrentActivity() {
  return getAudioActivity() + getMotionActivity();
}



void checkActivity() {
  const long actDelay = 10000;
  static long currActDelay = -actDelay;
  const long getDelay = 50;
  static long currGetDelay = -getDelay;

  if(millis() - currGetDelay > getDelay) {
    activity = getCurrentActivity();
    currGetDelay = millis();
  }
  
  if (activity > activityLevel && millis() - currActDelay > actDelay) {
    if(isSystemArmed) {
      sendAboveActivityWarning();
    }
    currActDelay = millis();
  }
}
