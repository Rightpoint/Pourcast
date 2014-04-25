using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace RightpointLabs.Pourcast.Repourter
{
    public class Program
    {

        // count flow every half a second
        private const int MILLISECONDS_BETWEEN_COUNT = 500;

        private const int NUMBER_OF_TAPS = 2;

        /// <summary>
        /// Main method runs on startup
        /// </summary>
        public static void Main()
        {
            var sensors = new FlowSensor[NUMBER_OF_TAPS];
            // Flow sensor plugged into pin 13, no resistor necessary, fire on the rising edge of the pulse
            sensors[0] = new FlowSensor(new InterruptPort(Pins.GPIO_PIN_D13, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh));
            // Flow sensor plugged into pin 12, no resistor necessary, fire on the rising edge of the pulse
            sensors[1] = new FlowSensor(new InterruptPort(Pins.GPIO_PIN_D12, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh));

            while (true)
            {
                foreach (var flowSensor in sensors)
                {
                    flowSensor.CheckPulses();
                }
                Thread.Sleep(MILLISECONDS_BETWEEN_COUNT);
            }
        }
    }
}
