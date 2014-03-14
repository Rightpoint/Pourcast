using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using RightpointLabs.Pourcast.DataModel.Entities;
using RightpointLabs.Pourcast.Repository.Abstract;

namespace RightpointLabs.Pourcast.Web.Controllers
{
    [System.Web.Http.RoutePrefix("api/keg")]
    public class KegController : ApiController
    {


        private readonly IKegRepository _kegRepository;
        public KegController()
        {
            _kegRepository = DependencyResolver.Current.GetService<IKegRepository>();
        }
        public KegController(IKegRepository kegRepository)
        {
            
        }

        // GET api/<controller>

        public IEnumerable<Keg> Get()
        {
            return _kegRepository.GetAll();
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