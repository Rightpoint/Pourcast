using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

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
            HttpMessageWriter.SendStartAsync(1);
            //HttpMessageWriter.SendStartAsync(1);
            //var sensors = new FlowSensor[NUMBER_OF_TAPS];
            //// Flow sensor plugged into pin 13, no resistor necessary, fire on the rising edge of the pulse
            //sensors[0] = new FlowSensor(new InterruptPort(Cpu.Pin.GPIO_Pin13, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh), 1);
            //// Flow sensor plugged into pin 12, no resistor necessary, fire on the rising edge of the pulse
            //sensors[1] = new FlowSensor(new InterruptPort(Cpu.Pin.GPIO_Pin12, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh), 2);
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
