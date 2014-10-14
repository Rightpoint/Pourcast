#define DEBUG_LEVEL 5

#include "Arduino.h"
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

class WiFlyHttp  {
  public:
    WiFlyHttp(const char* host, int port, byte pin);
    void MakeRequest(String url);
  private:
    WiFlyClient* _client;
    byte _pin;
};
