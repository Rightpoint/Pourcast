using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using RightpointLabs.Pourcast.Repourter.Configuration;

namespace RightpointLabs.Pourcast.Repourter
{
    public class FlowSensor
    {
        private readonly ArduinoWrapper _arduino;
        private readonly OutputPort _light;
        private readonly IHttpMessageWriter _httpMessageWriter;
        private readonly string _tapId;
        private readonly int _tapNumber;
        private readonly double _pulsesPerOz;
        private readonly ILogger _logger;

        public FlowSensor(ArduinoWrapper arduino, OutputPort light, IHttpMessageWriter httpMessageWriter, string tapId, int tapNumber, double pulsesPerOz, ILogger logger)
        {
            _arduino = arduino;
            _light = light;
            _httpMessageWriter = httpMessageWriter;
            _tapId = tapId;
            _tapNumber = tapNumber;
            _pulsesPerOz = pulsesPerOz;
            _logger = logger;

            _arduino.StartPour += ArduinoOnStartPour;
            _arduino.ContinuePour += ArduinoOnContinuePour;
            _arduino.StopPour += ArduinoOnStopPour;
            _arduino.IgnorePour += ArduinoOnIgnorePour;
        }

        private void ArduinoOnStartPour(object sender, ArduinoWrapper.TapEventArgs args)
        {
            if (args.TapNumber != _tapNumber)
                return;
            _light.Write(true);
            _httpMessageWriter.SendStartAsync(_tapId);
            _logger.Log("Started pour " + DateTime.Now.ToString("s"));
        }

        private void ArduinoOnContinuePour(object sender, ArduinoWrapper.TapEventArgs args)
        {
            if (args.TapNumber != _tapNumber)
                return;
            _logger.Log("Pouring @ " + args.PulseCount + ": " + DateTime.Now.ToString("s"));
            _httpMessageWriter.SendPouringAsync(_tapId, args.PulseCount / _pulsesPerOz);
        }

        private void ArduinoOnStopPour(object sender, ArduinoWrapper.TapEventArgs args)
        {
            if (args.TapNumber != _tapNumber)
                return;
            _light.Write(false);
            _logger.Log("Stopped pour @ " + args.PulseCount + ": " + DateTime.Now.ToString("s"));
            _httpMessageWriter.SendStopAsync(_tapId, args.PulseCount/_pulsesPerOz);
        }

        private void ArduinoOnIgnorePour(object sender, ArduinoWrapper.TapEventArgs args)
        {
            if (args.TapNumber != _tapNumber)
                return;
            _logger.Log("Ignored pour @ " + args.PulseCount + ": " + DateTime.Now.ToString("s"));
        }
    }
}
