namespace RightpointLabs.Pourcast.Web.Controllers.Api
{
    using System.Collections.Generic;
    using System.Web.Http;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;

    public class BreweryController : ApiController
    {
        private readonly IBreweryOrchestrator _breweryOrchestrator;

        public BreweryController(IBreweryOrchestrator breweryOrchestrator)
        {
            _breweryOrchestrator = breweryOrchestrator;
        }
        
        // GET api/<controller>
        public IEnumerable<Brewery> Get()
        {
            return _breweryOrchestrator.GetBreweries();
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