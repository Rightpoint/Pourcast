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

        private const int NUMBER_OF_TAPS = 2;

        /// <summary>
        /// Main method runs on startup
        /// </summary>
        public static void Main()
        {
            var writer = new WifiHttpMessageWriter();
            do
            {
                if (writer.Start("Rightpoint", "CHANGETHIS", WiFlyGSX.AuthMode.MixedWPA1_WPA2))
                {
                    break;
                }
                Debug.Print("Well, let's try reboot/reconect....");
                // didn't get an IP - wonder if rebooting the module will help?
                writer.Reboot();
                writer.Dispose();
                writer = new WifiHttpMessageWriter();
            } while (true);

            //var writer = new EthernetHttpMessageWriter();
            //if (!writer.Start())
            //{
            //    // didn't get an IP
            //    return;
            //}

            //writer.SendStartAsync(1);

            //while (true)
            //{
            //    try
            //    {
            //        writer.SendMessage(1);
            //        Thread.Sleep(5000);
            //        writer.SendMessage(1, 10, false);
            //        Thread.Sleep(10000);
            //    }
            //    catch (Exception)
            //    {
            //        Thread.Sleep(1000);
            //    }
            //}

            //var fs = new FlowSensor(new InterruptPort(Cpu.Pin.GPIO_Pin0, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth), writer, 1);
            //while (true)
            //{
            //    fs.CheckPulses();
            //    Thread.Sleep(1);
            //}



            //HttpMessageWriter.SendStartAsync(1);
            //Debug.Print("Starting");
            //{
            //    var port = new InterruptPort(Pins.GPIO_PIN_D13, false, Port.ResistorMode.Disabled,
            //        Port.InterruptMode.InterruptEdgeBoth);
            //    port.OnInterrupt += (i, ii, iii) => Debug.Print("Pressed 13: " + port.Read() + " -- " + i + " - " + ii + " - " + iii);
            //}
            //Debug.Print("Middle");
            //{
            //    var port = new InterruptPort(Pins.GPIO_PIN_D12, false, Port.ResistorMode.Disabled,
            //        Port.InterruptMode.InterruptEdgeBoth);
            //    port.OnInterrupt += (i, ii, iii) => Debug.Print("Pressed 12: " + port.Read() + " -- " + i + " - " + ii + " - " + iii);
            //}

            //Debug.Print("Sleeping");

            //Thread.Sleep(30000);

            //Debug.Print("Done");

            var sensors = new FlowSensor[NUMBER_OF_TAPS];
            // Flow sensor plugged into pin 13, no resistor necessary, fire on the rising edge of the pulse
            sensors[0] = new FlowSensor(new InterruptPort(Pins.GPIO_PIN_D13, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh), writer, 1);
            // Flow sensor plugged into pin 12, no resistor necessary, fire on the rising edge of the pulse
            sensors[1] = new FlowSensor(new InterruptPort(Pins.GPIO_PIN_D12, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh), writer, 2);
            Debug.Print("Starting");
            for (var i = 0; i < 100; i++)
            {
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
