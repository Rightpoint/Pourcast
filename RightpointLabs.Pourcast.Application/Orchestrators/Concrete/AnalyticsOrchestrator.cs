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
        private readonly IKegOrchestrator _kegOrchestrator;
        private readonly ITapOrchestrator _tapOrchestrator;
        private readonly IBeerOrchestrator _beerOrchestrator;
        private readonly IStoredEventRepository _storedEventRepository; 

        public AnalyticsOrchestrator(ITapOrchestrator tapOrchestrator, IKegOrchestrator kegOrchestrator, IBeerOrchestrator beerOrchestrator, IStoredEventRepository storedEventRepository)
        {
            if(null == tapOrchestrator) throw new ArgumentNullException("tapOrchestrator");
            if(null == kegOrchestrator) throw new ArgumentNullException("kegOrchestrator");
            if(null == beerOrchestrator) throw new ArgumentNullException("beerOrchestrator");
            if(null == storedEventRepository) throw new ArgumentNullException("storedEventRepository");

            _tapOrchestrator = tapOrchestrator;
            _kegOrchestrator = kegOrchestrator;
            _beerOrchestrator = beerOrchestrator;
            _storedEventRepository = storedEventRepository;
        }


        public IEnumerable<KegDurationOnTap> GetKegDurationsOnTap()
        {
            var kegs = _kegOrchestrator.GetKegs();
            var beers = _beerOrchestrator.GetBeers();
            var pourStartEvents = _storedEventRepository.GetAll<PourStarted>();
            var pourStoppedEvents = _storedEventRepository.GetAll<PourStopped>();
            return (from keg in kegs 
                          let beerName = beers.FirstOrDefault(b => b.Id == keg.BeerId).Name 
                          let start = pourStartEvents.Where(d => ((PourStarted) d.DomainEvent).KegId == keg.Id).Min(d => d.OccuredOn) 
                          let stop = pourStoppedEvents.Where(d => ((PourStopped) d.DomainEvent).KegId == keg.Id).Max(d => d.OccuredOn) 
                          select new KegDurationOnTap(keg.Id, keg.BeerId, beerName, start, stop)).ToList();
        }


        public IEnumerable<BeerBeenOnTap> GetBeersThatHaveBeenOnTap()
        {
            var kegs = _kegOrchestrator.GetKegs().GroupBy(k => k.BeerId).ToList();
            var beers = _beerOrchestrator.GetBeers().ToList();
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