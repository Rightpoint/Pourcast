using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightpointLabs.Pourcast.DataGenerator
{
    public class TapSimulator
    {
        private readonly int _number;
        private readonly bool _hasWeight;
        private readonly bool _hasFlow;
        private readonly Sender _sender;

        public TapSimulator(int number, bool hasWeight, bool hasFlow, Sender sender)
        {
            _number = number;
            _hasWeight = hasWeight;
            _hasFlow = hasFlow;
            _sender = sender;
        }

        public void Start()
        {
            var rnd = new Random();
            var weight = 80000000;
            Task.Run(async () =>
            {
                while (true)
                {
                    await _sender.Weight(_number, weight + rnd.Next(-10000, 10000));
                    await Task.Delay(TimeSpan.FromSeconds(60));
                }
            });

            // speeds + transitions
            //   0 = send every minute, if we get pulses, goto 3
            //   1 = send every second, if we get no pulses, goto 0
            //   2 = send now, then go to 1
            //   3 = send now, then go to 2

            Task.Run(async () =>
            {
                var nextPour = DateTime.UtcNow.Add(TimeSpan.FromSeconds(rnd.Next(30, 180)));
                while (true)
                {
                    if (nextPour <= DateTime.UtcNow)
                    {
                        var length = DateTime.UtcNow.AddSeconds(rnd.Next(3, 10));
                        await _sender.Flow(_number, 3, Math.Max(0, rnd.Next(50, 100)));
                        weight -= rnd.Next(50, 100);
                        await Task.Delay(100);
                        await _sender.Flow(_number, 2, Math.Max(0, rnd.Next(100, 200)));
                        weight -= rnd.Next(100, 200);
                        while (DateTime.UtcNow <= length)
                        {
                            await _sender.Flow(_number, 1, Math.Max(0, rnd.Next(100, 200)));
                            weight -= rnd.Next(100, 200);
                            await Task.Delay(100);
                        }
                        await _sender.Flow(_number, 1, Math.Max(0, rnd.Next(-60, 10)));
                        nextPour = DateTime.UtcNow.Add(TimeSpan.FromSeconds(rnd.Next(30, 180)));
                        continue;
                    }
                    await _sender.Flow(_number, 0, Math.Max(0, rnd.Next(-60,30)));
                    await Task.Delay((int)Math.Max(0, Math.Min(DateTime.UtcNow.Subtract(nextPour).TotalMilliseconds, 60000)));
                }
            });

        }
    }
}
