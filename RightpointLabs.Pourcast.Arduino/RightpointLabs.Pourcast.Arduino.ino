const long _startThreshold = 30;
const long _continueThreshold = 5;
class Reporter {
public: 
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
};

class Tap {
public: 
  Tap(Reporter* reporter);
  inline void HandlePulse();
  void Loop(byte cycleNumber);
private:
  Reporter* _reporter;
  volatile long _pulses;
  long _lastPulses;
};

Tap::Tap(Reporter* reporter) {
  _reporter = reporter;
}
void Tap::HandlePulse() {
  _pulses++;
}
void Tap::Loop(byte cycle) {
  long pulses = _pulses;
  if(_lastPulses != 0 && cycle % 10 == 0) {
    // in progress and on the second
    if(pulses - _lastPulses < _continueThreshold) {
      // complete
      _pulses = 0;
      _lastPulses = 0;
      _reporter->ReportStop(pulses);
    } 
    else {
      // continuing
      _lastPulses = pulses;
      _reporter->ReportContinue(pulses);
    }
  } 
  else if(_lastPulses == 0) {
    // not in progress
    if(pulses >= _startThreshold) {
      // starting
      _lastPulses = pulses;
      _reporter->ReportStart(pulses);
    } 
    else if(pulses > 0 && cycle % 10 == 0) {
      // ignored
      _pulses = 0;
      _reporter->ReportIgnore(pulses);
    }
  }
}

class SerialReporter: public Reporter {
public:
  SerialReporter(byte number);
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
private:
  byte _number;
};

SerialReporter::SerialReporter(byte number) { 
  _number = number;
}

void SerialReporter::ReportStop(long pulses){
  Serial.print("STOP ");
  Serial.print(_number, DEC);
  Serial.print(" ");
  Serial.println(pulses, DEC);
}
void SerialReporter::ReportContinue(long pulses) {
  Serial.print("CONTINUE ");
  Serial.print(_number, DEC);
  Serial.print(" ");
  Serial.println(pulses, DEC);
}
void SerialReporter::ReportStart(long pulses){
  Serial.print("START ");
  Serial.print(_number, DEC);
  Serial.print(" ");
  Serial.println(pulses, DEC);
}
void SerialReporter::ReportIgnore(long pulses){
  Serial.print("IGNORE ");
  Serial.print(_number, DEC);
  Serial.print(" ");
  Serial.println(pulses, DEC);
}


Tap* tap1;
Tap* tap2;

// prep - initialize serial (TX on pin 1) and wire up the interrupts for pulses from the taps (pins 2 and 3)
void setup() {
  Serial.begin(9600);
  while(!Serial);
  tap1 = new Tap(new SerialReporter(1));
  attachInterrupt(0, tap1Pulse, RISING);
  tap2 = new Tap(new SerialReporter(2));
  attachInterrupt(1, tap2Pulse, RISING);
}

// handle interrupt pulses from the taps
void tap1Pulse() {
  tap1->HandlePulse();
}
void tap2Pulse() {
  tap2->HandlePulse();
}

// main loop - once a second, check the accumulated pulse counts and send the START/STOP/CONTINUE/IGNORE message as necessary
// send an ALIVE message every loop to assist debugging
void loop() {
  byte cycle = 0;
  while(true) {
    tap1->Loop(cycle);
    tap2->Loop(cycle);

    if(cycle % 10 == 0) {
      Serial.println("ALIVE");
    }
    cycle = (cycle + 1) % 100;
    delay(100);
  }
}

