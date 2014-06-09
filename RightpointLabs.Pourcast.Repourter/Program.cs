using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace RightpointLabs.Pourcast.Repourter
{
    public class Program
    {
        /// <summary>
        /// Main method runs on startup
        /// </summary>
        public static void Main()
        {
            // START CONFIG
#if WIFI
            string wifiSSID = "Rightpoint";
            string wifiPassword = "ChangeThis";
            var wifiSecurityMode = Toolbox.NETMF.Hardware.WiFlyGSX.AuthMode.MixedWPA1_WPA2;
#endif
            var baseUrl = "http://pourcast.labs.rightpoint.com/api/Tap/";
            var httpLight = Pins.GPIO_PIN_D9;
            var taps =
                new[]
                    {
                        new TapConfig {Input = Pins.GPIO_PIN_D13, Light = Pins.GPIO_PIN_D11, TapId = "535c61a951aa0405287989ec"},
                        new TapConfig {Input = Pins.GPIO_PIN_D12, Light = Pins.GPIO_PIN_D10, TapId = "537d28db51aa04289027cde5"},
                    };
            var watchdogCheckInterval = new TimeSpan(0, 15, 0);
            // END CONFIG

            // turn on the tap lights on - they'll turn off as they initialize
            LightsOn(taps);
            // turn the HTTP light *off*, it'll go on as it initializes
            LightOff(httpLight);


#if WIFI
            var sender = new WifiMessageSender(wifiSSID, wifiPassword, wifiSecurityMode);
#elif ETHERNET
            var sender = new EthernetMessageSender();
#elif NO_NETWORKING
            var sender = new NullMessageSender();
#endif
            sender.Initalize();

            HttpMessageWriter writer = null;
            // first watchdog makes sure we send *something* every watchdogCheckInterval.  Second reboots us 30s later if the message hasn't been sent yet (ie. if networking dies)
            // sending *any* message resets both watchdogs
            var heartbeatWatchdog = new Watchdog(watchdogCheckInterval, () => writer.SendHeartbeatAsync());
            var rebootWatchdog = new RebootWatchdog(watchdogCheckInterval + new TimeSpan(0, 0, 30));

            writer = new HttpMessageWriter(sender, baseUrl, new OutputPort(httpLight, true), new [] { heartbeatWatchdog, rebootWatchdog });
            var sensors = new FlowSensor[taps.Length];
            for(var i=0; i<taps.Length; i++)
            {
                sensors[i] = new FlowSensor(
                    new InterruptPort(taps[i].Input, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh), 
                    new OutputPort(taps[i].Light, false),
                    writer, 
                    taps[i].TapId);
            }

            heartbeatWatchdog.Start();
            rebootWatchdog.Start();
            Debug.Print("Starting");

            while(true)
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }

        /// <summary>
        /// Turns on the lights for the passed stuff
        /// </summary>
        private static void LightsOn(TapConfig[] taps)
        {
            foreach (var tap in taps)
            {
                using (var p = new OutputPort(tap.Light, true))
                {
                    p.Write(true);
                }
            }
        }

        private static void LightOff(Cpu.Pin httpLight)
        {
            using (var p = new OutputPort(httpLight, false))
            {
                p.Write(false);
            }
        }
    }
}
