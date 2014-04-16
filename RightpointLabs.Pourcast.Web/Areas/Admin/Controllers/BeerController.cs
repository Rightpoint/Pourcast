using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    public class BeerController : ApiController
    {
        private readonly IBeerOrchestrator _beerOrchestrator;

        public BeerController(IBeerOrchestrator beerOrchestrator)
        {
            _beerOrchestrator = beerOrchestrator;
        }

        // GET api/beer
        public IEnumerable<Beer> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/beer/5
        public string Get(string id)
        {
            throw new NotImplementedException();
        }

        //Get api/beer/
        public IEnumerable<Beer> GetByName(string name)
        {
            return _beerOrchestrator.GetBeersByName(name);
        }

        // POST api/beer
        public void Post([FromBody]Beer value)
        {
        }

        // PUT api/beer/5
        public void Put(string id, [FromBody]Beer value)
        {
        }

        // DELETE api/beer/5
        public void Delete(string id)
        {
        }
    }
}
