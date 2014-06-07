using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace RightpointLabs.Pourcast.Repourter
{
    public class Program
    {
        // count flow every half a second
        private const int MILLISECONDS_BETWEEN_COUNT = 500;

        private static readonly TimeSpan ResetWatchdogEvery = new TimeSpan(0, 0, 45);

        public class TapConfig
        {
            public Cpu.Pin Input { get; set; }
            public Cpu.Pin Light { get; set; }
            public string TapId { get; set; }
        }

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
            // END CONFIG

#if WIFI
            var sender = new WifiMessageSender(wifiSSID, wifiPassword, wifiSecurityMode);
#elif ETHERNET
            var sender = new EthernetMessageSender();
#elif NO_NETWORKING
            var sender = new NullMessageSender();
#endif
            sender.Initalize();

            var writer = new HttpMessageWriter(sender, baseUrl, new OutputPort(httpLight, false));
            var sensors = new FlowSensor[taps.Length];
            for(var i=0; i<taps.Length; i++)
            {
                sensors[i] = new FlowSensor(
                    new InterruptPort(taps[i].Input, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh), 
                    new OutputPort(taps[i].Light, false),
                    writer, 
                    taps[i].TapId);
            }

            Debug.Print("Starting");

            DateTime lastWatchdogTrigger = DateTime.MinValue;
            while(true)
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
