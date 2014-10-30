using System;

namespace RightpointLabs.Pourcast.Application.Payloads.Analytics
{
    public class KegDurationOnTap
    {
        public KegDurationOnTap(string kegId, string beerId, string beerName, DateTime dateTapped, DateTime dateRemoved)
        {
            if(string.IsNullOrEmpty(kegId)) throw new ArgumentNullException("kegId");
            if(string.IsNullOrEmpty(beerId)) throw new ArgumentNullException("beerId");
            if(string.IsNullOrEmpty(beerName)) throw new ArgumentNullException("beerName");
            KegId = kegId;
            BeerId = beerId;
            BeerName = beerName;
            Tapped = dateTapped;
            RemovedFromTap = dateRemoved;
        }

        public string KegId { get; private set; }
        public string BeerName { get; private set; }
        public string BeerId { get; private set; }
        public DateTime Tapped { get; private set; }
        public DateTime RemovedFromTap { get; private set; }
        public TimeSpan DurationOnTap
        {
            get { return RemovedFromTap - Tapped; }
        }

        public int DaysOnTap { get { return DurationOnTap.Days; } }
    }
}