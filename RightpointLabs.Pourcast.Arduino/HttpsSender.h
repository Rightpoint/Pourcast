#include "config.h"

#ifdef USE_HTTPS
void deviceTeardown();
void deviceProcess(int delay);
void flowSend(int number, int reportingSpeed, long pulses);
void weightSend(int number, int weight);
void temperatureSend(const char* sensor, float temperature);
void errorSend(const char* error);
#endif
