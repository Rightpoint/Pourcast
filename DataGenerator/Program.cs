using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var uri = new Uri("http://localhost:7071/api/PostEvent");
            while (true)
            {
                var c = new HttpClient();

                {
                    var res = await c.PostAsync(uri, new FormUrlEncodedContent(new Dictionary<string, string>()
                    {
                        { "Event", "flow" },
                        { "Number", "1" },
                        { "Speed", "1" },
                        { "Pulses", "12" },
                    }));
                    res.EnsureSuccessStatusCode();
                }

                {
                    var res = await c.PostAsync(uri, new FormUrlEncodedContent(new Dictionary<string, string>()
                    {
                        { "Event", "weight" },
                        { "Number", "1" },
                        { "Weight", "3123443" },
                    }));
                    res.EnsureSuccessStatusCode();
                }

                {
                    var res = await c.PostAsync(uri, new FormUrlEncodedContent(new Dictionary<string, string>()
                    {
                        { "Event", "temp" },
                        { "Sensor", "1823u198f3h983fh198" },
                        { "Temp", "64" },
                    }));
                    res.EnsureSuccessStatusCode();
                }

                {
                    var res = await c.PostAsync(uri, new FormUrlEncodedContent(new Dictionary<string, string>()
                    {
                        { "Event", "error" },
                        { "Message", "Things are ok" },
                    }));
                    res.EnsureSuccessStatusCode();
                }

                await Task.Delay(TimeSpan.FromSeconds(15));
            }

        }
    }
}
