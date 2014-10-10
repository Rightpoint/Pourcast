#define DEBUG_LEVEL 5

#include <SPI.h>
#include <Configuration.h>
#include <Debug.h>
#include <ParsedStream.h>
#include <SpiUart.h>
#include <WiFly.h>
#include <WiFlyClient.h>
#include <WiFlyDevice.h>
#include <WiFlyServer.h>
#include <_Spi.h>

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
  _pulses = 0;
  _lastPulses = 0;
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

class MultiReporter: public Reporter {
public:
  MultiReporter(Reporter* reporter1, Reporter* reporter2, Reporter* reporter3);
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
private:
  Reporter* _reporter1;
  Reporter* _reporter2;
  Reporter* _reporter3;
};

MultiReporter::MultiReporter(Reporter* reporter1, Reporter* reporter2, Reporter* reporter3) { 
  _reporter1 = reporter1;
  _reporter2 = reporter2;
  _reporter3 = reporter3;
}

void MultiReporter::ReportStop(long pulses){
  if(_reporter1 != NULL) {
    _reporter1->ReportStop(pulses);
  }
  if(_reporter2 != NULL) {
    _reporter2->ReportStop(pulses);
  }
  if(_reporter3 != NULL) {
    _reporter3->ReportStop(pulses);
  }
}
void MultiReporter::ReportContinue(long pulses) {
  if(_reporter1 != NULL) {
    _reporter1->ReportContinue(pulses);
  }
  if(_reporter2 != NULL) {
    _reporter2->ReportContinue(pulses);
  }
  if(_reporter3 != NULL) {
    _reporter3->ReportContinue(pulses);
  }
}
void MultiReporter::ReportStart(long pulses){
  if(_reporter1 != NULL) {
    _reporter1->ReportStart(pulses);
  }
  if(_reporter2 != NULL) {
    _reporter2->ReportStart(pulses);
  }
  if(_reporter3 != NULL) {
    _reporter3->ReportStart(pulses);
  }
}
void MultiReporter::ReportIgnore(long pulses){
  if(_reporter1 != NULL) {
    _reporter1->ReportIgnore(pulses);
  }
  if(_reporter2 != NULL) {
    _reporter2->ReportIgnore(pulses);
  }
  if(_reporter3 != NULL) {
    _reporter3->ReportIgnore(pulses);
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


class LEDReporter: public Reporter {
public:
  LEDReporter(byte pin);
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
private:
  byte _pin;
};

LEDReporter::LEDReporter(byte pin) { 
  _pin = pin;
  pinMode(_pin, OUTPUT);
}

void LEDReporter::ReportStop(long pulses){
  digitalWrite(_pin, LOW);
}
void LEDReporter::ReportContinue(long pulses) {
}
void LEDReporter::ReportStart(long pulses){
  digitalWrite(_pin, HIGH);
}
void LEDReporter::ReportIgnore(long pulses){
}

class WiFlyHttp  {
  public:
    WiFlyHttp(const char* host, int port, byte pin);
    void MakeRequest(String url);
  private:
    WiFlyClient* _client;
    byte _pin;
};
WiFlyHttp::WiFlyHttp(const char* host, int port, byte pin) { 
  _client = new WiFlyClient(host, port);
  _pin = pin;
  pinMode(_pin, OUTPUT);
  digitalWrite(_pin, HIGH);
}

void WiFlyHttp::MakeRequest(String url){
  digitalWrite(_pin, LOW);
  Serial.println(url);
  if (_client->connect()) {
    _client->println("GET " + url + " HTTP/1.0");
    _client->println();
    while(_client->available() || _client->connected()) {
      Serial.print(_client->read());
    }
  }
  digitalWrite(_pin, HIGH);
}

class WiFlyReporter: public Reporter {
public:
  WiFlyReporter(WiFlyHttp* http, String tapId);
  virtual void ReportStop(long pulses);
  virtual void ReportContinue(long pulses);
  virtual void ReportStart(long pulses);
  virtual void ReportIgnore(long pulses);
  void Heartbeat();
private:
  void MakeRequest(String url);
  String _tapId;
  WiFlyHttp* _http;
};

WiFlyReporter::WiFlyReporter(WiFlyHttp* http, String tapId) { 
  _http = http;
  _tapId = tapId;
}

void WiFlyReporter::MakeRequest(String url){
  _http->MakeRequest(url);
}
void WiFlyReporter::ReportStop(long pulses){
  MakeRequest("/api/Tap/" + _tapId + "/StopPour?volume=" + (pulses / 1000));
}
void WiFlyReporter::ReportContinue(long pulses) {
  MakeRequest("/api/Tap/" + _tapId + "/Pouring?volume=" + (pulses / 1000));
}
void WiFlyReporter::ReportStart(long pulses){
  MakeRequest("/api/Tap/" + _tapId + "/StartPour");
}
void WiFlyReporter::ReportIgnore(long pulses){
}
void WiFlyReporter::Heartbeat(){
  MakeRequest("/api/Status/heartbeat");
}


Tap* tap1;
Tap* tap2;
WiFlyHttp* http;

// prep - initialize serial (TX on pin 1) and wire up the interrupts for pulses from the taps (pins 2 and 3)
void setup() {
  Serial.begin(9600);
  while(!Serial);
  
  Serial.println("Starting");
  WiFly.begin();
  Serial.println("Joining");
  if (!WiFly.join("Rightpoint-Guest", "RP@29NWacker")) {
     // Handle the failure
     Serial.println("Failed to connect");
  }
  else{
     Serial.println("Connected");
  }
  
  WiFly.configure(WIFLY_BAUD, 38400);

  Serial.println(WiFly.ip());

  http = new WiFlyHttp("pourcast.labs.rightpoint.com", 80, 9);
  tap1 = new Tap(new MultiReporter(new SerialReporter(1), new LEDReporter(10), new WiFlyReporter(http, "535c61a951aa0405287989ec")));
  attachInterrupt(0, tap1Pulse, RISING);
  tap2 = new Tap(new MultiReporter(new SerialReporter(2), new LEDReporter(11), new WiFlyReporter(http, "537d28db51aa04289027cde5")));
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

