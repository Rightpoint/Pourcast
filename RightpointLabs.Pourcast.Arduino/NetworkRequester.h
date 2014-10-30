
class NetworkRequester  {
  public:
    NetworkRequester(const char* host, byte pin, Print* debug);
    void MakeRequest(const char* url);
    void LogMessage(const char* message);
    void LogMessage(const __FlashStringHelper* message);
    void Heartbeat();
    int _minFreeMemory;
  private:
    const char* _host;
    byte _pin;
    Print* _debug;
};
