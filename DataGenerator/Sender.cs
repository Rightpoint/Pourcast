using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RightpointLabs.Pourcast.DataGenerator
{
    public class Sender
    {
        private readonly Uri _url;
        private readonly string _deviceId;

        public Sender(Uri url, string deviceId)
        {
            _url = url;
            _deviceId = deviceId;
        }

        public async Task Flow(int number, int speed, long pulses)
        {
            await Send(new Dictionary<string, string>()
            {
                {"Event", "flow"},
                {"Number", number.ToString()},
                {"Speed", speed.ToString()},
                {"Pulses", pulses.ToString()},
            });
        }

        public async Task Weight(int number, int weight)
        {
            await Send(new Dictionary<string, string>()
            {
                {"Event", "weight"},
                {"Number", number.ToString()},
                {"Weight", weight.ToString()},
            });
        }

        public async Task Temperature(string sensor, float temperature)
        {
            await Send(new Dictionary<string, string>()
            {
                {"Event", "temp"},
                {"Sensor", sensor},
                {"Temp", temperature.ToString()},
            });
        }

        public async Task Error(string message)
        {
            await Send(new Dictionary<string, string>()
            {
                {"Event", "error"},
                {"Message", message},
            });
        }

        private async Task Send(IDictionary<string, string> data)
        {
            data.Add("DeviceId", _deviceId);
            var c = new HttpClient();
            var r = await c.PostAsync(_url, new FormUrlEncodedContent(data));
            try
            {
                r.EnsureSuccessStatusCode();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
