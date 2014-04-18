namespace RightpointLabs.Pourcast.Web.Controllers.Api
{
    using System.Collections.Generic;
    using System.Web.Http;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;

    [System.Web.Http.RoutePrefix("api/keg")]
    public class KegController : ApiController
    {
        private readonly IKegOrchestrator _kegOrchestrator;

        public KegController(IKegOrchestrator kegOrchestrator)
        {
            _kegOrchestrator = kegOrchestrator;
        }

        // GET api/<controller>

        public IEnumerable<Keg> Get()
        {
            return _kegOrchestrator.GetKegs();
        }

        // GET api/<controller>/5
        public IEnumerable<Keg> Get(bool ontap)
        {
            return ontap ? _kegOrchestrator.GetKegsOnTap() : _kegOrchestrator.GetKegs();
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