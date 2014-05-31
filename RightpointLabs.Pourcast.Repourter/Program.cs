using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Toolbox.NETMF.Hardware;

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
            var writer = new HttpMessageWriter();
            if (!writer.Start("Rightpoint", "CHANGETHIS", WiFlyGSX.AuthMode.MixedWPA1_WPA2))
            {
                // didn't get an IP
                return;
            }

            writer.SendStartAsync(1);

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
            Debug.Print("Starting");
            {
                var port = new InterruptPort(Cpu.Pin.GPIO_Pin13, false, Port.ResistorMode.PullUp,
                    Port.InterruptMode.InterruptEdgeBoth);
                port.OnInterrupt += (i, ii, iii) => Debug.Print("Pressed");
                port.EnableInterrupt();
            }
            Debug.Print("Middle");
            {
                var x = new InputPort(Cpu.Pin.GPIO_Pin12, false, Port.ResistorMode.Disabled);
                x.OnInterrupt += (data1, data2, time) => Debug.Print("12");
            }

            Debug.Print("Sleeping");

            Thread.Sleep(10000000);


            //var sensors = new FlowSensor[NUMBER_OF_TAPS];
            //// Flow sensor plugged into pin 13, no resistor necessary, fire on the rising edge of the pulse
            //sensors[0] = new FlowSensor(new InterruptPort(Cpu.Pin.GPIO_Pin13, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh), writer, 1);
            //// Flow sensor plugged into pin 12, no resistor necessary, fire on the rising edge of the pulse
            //sensors[1] = new FlowSensor(new InterruptPort(Cpu.Pin.GPIO_Pin12, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh), writer, 2);
            //while (true)
            //{
            //    foreach (var flowSensor in sensors)
            //    {
            //        flowSensor.CheckPulses();
            //    }
            //    Thread.Sleep(MILLISECONDS_BETWEEN_COUNT);
            //}
        }
    }
}
