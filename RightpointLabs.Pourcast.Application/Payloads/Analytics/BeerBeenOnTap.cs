using System;

namespace RightpointLabs.Pourcast.Application.Payloads.Analytics
{
    public class BeerBeenOnTap
    {
        public BeerBeenOnTap(string beerId, string beerName, string[] kegIds, DateTime firstDate, DateTime lastDate)
        {
            if(string.IsNullOrEmpty(beerId)) throw new ArgumentNullException("beerId");
            if(string.IsNullOrEmpty(beerName)) throw new ArgumentNullException("beerName");

            BeerId = beerId;
            BeerName = beerName;
            KegIds = kegIds;
            TimeSpan = lastDate - firstDate;
        }

        public string BeerId { get; private set; }
        public string BeerName { get; private set; }
        public string[] KegIds { get; private set; }
        public int KegsOfBeer { get { return KegIds.Length; } }
        public TimeSpan TimeSpan { get; private set; }
    }
}