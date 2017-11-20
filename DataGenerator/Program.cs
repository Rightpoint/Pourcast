using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RightpointLabs.Pourcast.DataGenerator;

namespace RightpointLabs.Pourcast.DataGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var uri = new Uri(ConfigurationManager.AppSettings["PostEventUrl"]);
            new TapSimulator(1, true, true, new Sender(uri, "912u42rsifd321")).Start();
            new TapSimulator(2, true, true, new Sender(uri, "912u42rsifd322")).Start();

            while (true)
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
