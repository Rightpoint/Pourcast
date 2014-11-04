using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace RightpointLabs.Pourcast.Application.Payloads.Analytics
{
    public class KegDurationOnTap
    {
        public KegDurationOnTap(string kegId, string beerId, string beerName, IEnumerable<Burndown> burndowns, int interval)
        {
            if(string.IsNullOrEmpty(kegId)) throw new ArgumentNullException("kegId");
            if(string.IsNullOrEmpty(beerId)) throw new ArgumentNullException("beerId");
            if(string.IsNullOrEmpty(beerName)) throw new ArgumentNullException("beerName");
            KegId = kegId;
            BeerId = beerId;
            BeerName = beerName;
            Burndowns = burndowns;
            BurndownIntervalMintues = interval;
        }

        public string KegId { get; private set; }
        public string BeerName { get; private set; }
        public string BeerId { get; private set; }
        public int BurndownIntervalMintues { get; private set; }
        public IEnumerable<Burndown> Burndowns { get; private set; }

    }
}