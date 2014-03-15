using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using RighpointLabs.Pourcast.Orchestrator.Abstract;
using RighpointLabs.Pourcast.Orchestrator.Models;
using RightpointLabs.Pourcast.Repository.Abstract;

namespace RightpointLabs.Pourcast.Web.Controllers
{
    [System.Web.Http.RoutePrefix("api/keg")]
    public class KegController : ApiController
    {


        private readonly IKegRepository _kegRepository;
        private readonly IBreweryOrchestrator _brewery;
        public KegController(IKegRepository kegRepository, IBreweryOrchestrator breweryRepository)
        {
            _kegRepository = kegRepository;
            _brewery = breweryRepository;
        }

        // GET api/<controller>

        public List<Brewery> Get()
        {
            return _brewery.GetBreweries();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}