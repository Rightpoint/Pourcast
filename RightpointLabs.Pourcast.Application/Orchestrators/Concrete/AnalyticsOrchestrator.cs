using System;
using System.Collections.Generic;
using System.Linq;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Application.Payloads.Analytics;
using RightpointLabs.Pourcast.Domain.Events;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Domain.Repositories;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    public class AnalyticsOrchestrator : IAnalyticsOrchestrator
    {
        private readonly IKegRepository _kegRepository;
        private readonly IBeerRepository _beerRepository;
        private readonly IStoredEventRepository _storedEventRepository; 

        public AnalyticsOrchestrator(IKegRepository kegRepository, IBeerRepository beerRepository, IStoredEventRepository storedEventRepository)
        {
            if(null == kegRepository) throw new ArgumentNullException("kegRepository");
            if(null == beerRepository) throw new ArgumentNullException("beerRepository");
            if(null == storedEventRepository) throw new ArgumentNullException("storedEventRepository");

            _kegRepository = kegRepository;
            _beerRepository = beerRepository;
            _storedEventRepository = storedEventRepository;
        }


        public KegDurationOnTap GetKegsDurationOnTap(string kegId, int minuteInterval)
        {
            var keg = _kegRepository.GetById(kegId);
            if(null == keg) throw new Exception(string.Format("Keg with Id: {0} does not exit", kegId));
            var beer = _beerRepository.GetById(keg.BeerId);
            var pours =
                _storedEventRepository.Find<PourStopped>(p => ((PourStopped) p.DomainEvent).KegId == kegId)
                    .OrderBy(d => d.OccuredOn).ToList();

            var burndowns = new List<Burndown>();
            double totalPours = 0;

            for (var i = 0; i < pours.Count(); i++)
            {
                var e = pours[i];
                var pour = e.DomainEvent as PourStopped;
                var totalVol = pour.Volume;
                StoredEvent last = null;

                for (var x = i + 1; x < pours.Count(); x++)
                {
                    var e2 = pours[x];
                    var ts = e2.OccuredOn - e.OccuredOn;
                    // See if we're over the interval given
                    if (ts.TotalMinutes > minuteInterval)
                    {
                        i = x - 1;
                        break;
                    }
                    last = e2;
                    totalVol += ((PourStopped) e2.DomainEvent).Volume;
                    i = x;
                }
                totalPours += totalVol;
                burndowns.Add(new Burndown()
                {
                    PercentRemaining = ((keg.Capacity - totalPours) / keg.Capacity) * 100.0,
                    StartOfBurndown = e.OccuredOn,
                    EndOfBurndown = (null == last) ? e.OccuredOn : last.OccuredOn
                });
            }

            return new KegDurationOnTap(keg.Id, keg.BeerId, beer.Name, burndowns, minuteInterval);
        }

        public IEnumerable<KegDurationOnTap> GetKegDurationsOnTap()
        {
            throw new NotImplementedException();
        }


        public IEnumerable<BeerBeenOnTap> GetBeersThatHaveBeenOnTap()
        {
            var kegs = _kegRepository.GetAll().GroupBy(k => k.BeerId).ToList();
            var beers = _beerRepository.GetAll().ToList();
            var pourStartEvents = _storedEventRepository.GetAll<PourStarted>().ToList();
            var pourStoppedEvents = _storedEventRepository.GetAll<PourStopped>().ToList();

            //var result = (from grp in kegs
            //    let beerName = beers.FirstOrDefault(b => b.Id == grp.Key).Name
            //    let kegIds = grp.Select(k => k.Id).ToArray()
            //    let start = pourStartEvents.Where(d => grp.Select(k => k.Id).Contains(((PourStarted)d.DomainEvent).KegId)).Min(d => d.OccuredOn)
            //    let stop = pourStoppedEvents.Where(d => grp.Select(k => k.Id).Contains(((PourStarted)d.DomainEvent).KegId)).Max(d => d.OccuredOn)
            //    select new BeerBeenOnTap(grp.Key, beerName, kegIds, start, stop)).ToList();

            return (from @group in kegs 
                    let beerName = beers.FirstOrDefault(b => b.Id == @group.Key).Name 
                    let kegIds = @group.Select(k => k.Id).ToArray() 
                    let startEvents = pourStartEvents.Where(d => kegIds.Contains(((PourStarted) d.DomainEvent).KegId)).ToList() 
                    let stopEvents = pourStoppedEvents.Where(d => kegIds.Contains(((PourStopped) d.DomainEvent).KegId)).ToList() 
                    where startEvents.Any() && stopEvents.Any()
                    select new BeerBeenOnTap(@group.Key, beerName, kegIds, startEvents.Min(d => d.OccuredOn), stopEvents.Max(d => d.OccuredOn))
                    ).ToList();
        }

        public IEnumerable<BeerBeenOnTap> GetBeersThatHaveBeenOnTap(DateTime beginDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}