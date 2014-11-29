using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using RightpointLabs.Pourcast.Application.Orchestrators.Concrete;
using RightpointLabs.Pourcast.Application.Payloads.Analytics;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    public interface IAnalyticsOrchestrator
    {
        KegDurationOnTap GetKegsDurationOnTap(string kegId, int minuteInterval);
        IEnumerable<KegDurationOnTap> GetKegDurationsOnTap();
        IEnumerable<BeerBeenOnTap> GetBeersThatHaveBeenOnTap();
        IEnumerable<BeerBeenOnTap> GetBeersThatHaveBeenOnTap(DateTime beginDate, DateTime endDate);
        IEnumerable<Pour> GetRealPoursSince(DateTime beginDate);
        IEnumerable<Pour> GetPhantomPoursSince(DateTime beginDate);
    }
}