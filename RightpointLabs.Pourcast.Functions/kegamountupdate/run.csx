#r "Newtonsoft.Json"

using System;
using Newtonsoft.Json.Linq;

public static void Run(string msg, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {msg}");
    dynamic obj = JObject.Parse(msg);
    var wData = new List<dynamic>(obj.alldata).Where(i => i.Weight != null).Select((i => new { date = DateTime.Parse(i.eventenqueuedutctime.ToString()), Weight = (int)i.Weight })).ToArray();
    log.Info($"{obj.deviceid} {obj.kegnumber}");
    var multi = 2.0m / (Math.Min(wData.Length, 5) + 1);
    if (wData.Any())
    {
        var first = wData.Select(i => i.Weight).First();
        var weight = wData.Aggregate((decimal)first, (a, v) => a + (v.Weight - a) * multi);
        log.Info($"  W: {weight}");
    }

    //log.Info(JArray.FromObject(wData).ToString());
}