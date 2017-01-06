extern "C" {
  void deviceSetup(const char* connectionString);
  void deviceTeardown();
  void deviceProcess(int delay);
  void deviceSend(long timeSinceLastData, int weight, long pulses, int kegNumber, int reportingSpeed);
  void temperatureSend(const char* sensor, float temperature);
  void sendError(const char* error);
}

