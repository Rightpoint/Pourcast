using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RightpointLabs.Pourcast.Application.Commands;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Infrastructure.Data.Repositories;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    public class BeerController : Controller
=======
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    public class BeerController : ApiController
>>>>>>> Adding create beer files
    {
        private readonly IBeerOrchestrator _beerOrchestrator;

        public BeerController(IBeerOrchestrator beerOrchestrator)
        {
<<<<<<< HEAD
            if (beerOrchestrator == null) throw new ArgumentNullException("beerOrchestrator");
            _beerOrchestrator = beerOrchestrator;
        }

        //
        // GET: /Admin/Beer/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Admin/Beer/Details/My-Beer-Name
        public ActionResult Details(string slug)
        {
            return View();
        }

        //
        // GET: /Admin/Beer/Create
        public ActionResult Create(string breweryId)
        {
            var command = _beerOrchestrator.CreateBeer(breweryId);
            if (command != null) return View("Create", command);

            ViewBag.Error = "Brewery with that id does not exist.";
            return View("Create", null);
        }

        //
        // POST: /Admin/Beer/Create
        [HttpPost]
        public ActionResult Create(CreateBeer createBeerCommand)
        {
            try
            {
                _beerOrchestrator.CreateBeer(createBeerCommand);
                return RedirectToAction("Details", "Brewery", new { id = createBeerCommand.BreweryId});
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Beer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Beer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Beer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Beer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
=======
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
>>>>>>> Adding create beer files
        }
    }
}
