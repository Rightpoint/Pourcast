using System;
using System.Collections.Generic;
using System.Web.Http;

namespace RightpointLabs.Pourcast.Web.Controllers.Api
{
    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Application.Payloads;

    public class BeerOnTapController : ApiController
    {
        private readonly IBeerOrchestrator _beerOrchestrator;

        public BeerOnTapController(IBeerOrchestrator beerOrchestrator)
        {
            if (beerOrchestrator == null) throw new ArgumentNullException(nameof(beerOrchestrator));

            _beerOrchestrator = beerOrchestrator;
        }

        // GET api/<controller>
        public IEnumerable<BeerOnTap> Get()
        {
            return _beerOrchestrator.GetBeersOnTap();
        }

        public BeerOnTap Get([FromUri]string id)
        {
            return _beerOrchestrator.GetBeerOnTap(id);
        }
    }
}