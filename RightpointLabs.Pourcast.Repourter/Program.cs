using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Toolbox.NETMF.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace RightpointLabs.Pourcast.Repourter
{
    public class Program
    {
        // count flow every half a second
        private const int MILLISECONDS_BETWEEN_COUNT = 500;

        private static readonly TimeSpan ResetWatchdogEvery = new TimeSpan(0, 0, 45);

        private const int NUMBER_OF_TAPS = 2;

        /// <summary>
        /// Main method runs on startup
        /// </summary>
        public static void Main()
        {
            var watchdog = new Watchdog(new TimeSpan(0, 1, 0));
            watchdog.Start();
            var writer = new WifiHttpMessageWriter(watchdog);
            do
            {
                if (writer.Start("Rightpoint", "CHANGEME", WiFlyGSX.AuthMode.MixedWPA1_WPA2))
                {
                    break;
                }
                Debug.Print("Well, let's try reboot/reconect....");
                // didn't get an IP - wonder if rebooting the module will help?
                writer.Reboot();
                writer.Dispose();
                writer = new WifiHttpMessageWriter(watchdog);
            } while (true);

            var sensors = new FlowSensor[NUMBER_OF_TAPS];
            // Flow sensor plugged into pin 13, no resistor necessary, fire on the rising edge of the pulse
            sensors[0] = new FlowSensor(new InterruptPort(Pins.GPIO_PIN_D13, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh), writer, 1);
            // Flow sensor plugged into pin 12, no resistor necessary, fire on the rising edge of the pulse
            sensors[1] = new FlowSensor(new InterruptPort(Pins.GPIO_PIN_D12, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh), writer, 2);
            Debug.Print("Starting");

            DateTime lastWatchdogTrigger = DateTime.MinValue;
            for (var i = 0; i < 2000; i++)
            {
                if (DateTime.UtcNow.Subtract(lastWatchdogTrigger) >= ResetWatchdogEvery)
                {
                    writer.SendHeartbeatAsync();
                    lastWatchdogTrigger = DateTime.UtcNow;
                }
                foreach (var flowSensor in sensors)
                {
                    flowSensor.CheckPulses();
                }
                Thread.Sleep(MILLISECONDS_BETWEEN_COUNT);
            }

            writer.Stop();
            Debug.Print("Exiting");
        }
    }
}
