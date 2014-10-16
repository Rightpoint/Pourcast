#include <WiFlySerial.h>

class NetworkRequester  {
  public:
    NetworkRequester(WiFlySerial* wiFly, const char* host, int port, byte pin);
    void MakeRequest(String url);
    void LogMessage(String message);
    void Heartbeat();
  private:
    WiFlySerial* _wiFly;
    const char* _host;
    int _port;
    byte _pin;
};
