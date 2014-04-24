using System;
using System.Collections.Generic;
using System.Web.Http;

namespace RightpointLabs.Pourcast.Web.Controllers.Api
{
    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Application.Payloads;

    public class BeerController : ApiController
    {
        private readonly IBeerOrchestrator _beerOrchestrator;

        public BeerController(IBeerOrchestrator beerOrchestrator)
        {
            if (beerOrchestrator == null) throw new ArgumentNullException("beerOrchestrator");

            _beerOrchestrator = beerOrchestrator;
        }

        // GET api/<controller>
        public IEnumerable<BeerOnTap> Get()
        {
            return _beerOrchestrator.GetBeersOnTap();
        }

        public BeerOnTap Get(string tapId)
        {
            return _beerOrchestrator.GetBeerOnTap(tapId);
        }
    }
}