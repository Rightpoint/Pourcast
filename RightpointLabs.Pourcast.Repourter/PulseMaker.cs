using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RightpointLabs.Pourcast.Repourter
{
    public class PulseMaker
    {
        private readonly InterruptPort _triggerPort;
        private readonly PWM _outputPort;

        public PulseMaker(Cpu.Pin triggerPin, Cpu.PWMChannel outputChannel, double frequency)
        {
            _triggerPort = new InterruptPort(triggerPin, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
            _outputPort = new PWM(outputChannel, frequency, 0.1, false);
        }

        public void Start()
        {
            var started = false;
            _triggerPort.OnInterrupt += (data1, data2, time) =>
            {
                var read = _triggerPort.Read();
                if (!read && !started)
                    return;
                if (read)
                {
                    Debug.Print("Starting PWM on " + _outputPort.Pin);
                    _outputPort.Start();
                    started = true;
                }
                else
                {
                    _outputPort.Stop();
                     Debug.Print("Stopped PWM on " + _outputPort.Pin);
               }
            };
        }
    }
}
