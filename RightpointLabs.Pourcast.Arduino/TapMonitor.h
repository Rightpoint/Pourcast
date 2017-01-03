extern "C" {
  void deviceSetup(const char* connectionString);
  void deviceTeardown();
  void deviceProcess(int delay);
  void deviceSend(long timeSinceLastData, double temperature, int weight, long pulses, const char* kegId);
}

