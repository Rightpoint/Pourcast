using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> Can edit brewery
using System.Web.Routing;
using RightpointLabs.Pourcast.Application.Commands;
=======
>>>>>>> Can create brewery.
=======
using RightpointLabs.Pourcast.Application.Commands;
>>>>>>> Can create brewery and can add/create beers for the brewery.  Also, added commands
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Web.Areas.Admin.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    public class BreweryController : Controller
    {
        private readonly IBreweryOrchestrator _breweryOrchestrator;
        private readonly IBeerOrchestrator _beerOrchestrator;

        public BreweryController(IBreweryOrchestrator breweryOrchestrator, IBeerOrchestrator beerOrchestrator)
        {
            if(breweryOrchestrator == null) throw new ArgumentNullException("breweryOrchestrator");
            if(beerOrchestrator == null) throw new ArgumentNullException("beerOrchestrator");

            _breweryOrchestrator = breweryOrchestrator;
            _beerOrchestrator = beerOrchestrator;
        }

        //
        // GET: /Admin/Brewery/
        public ActionResult Index()
        {
            var breweries = _breweryOrchestrator.GetBreweries();

            return View(breweries);
        }

<<<<<<< HEAD
<<<<<<< HEAD
        public ActionResult Details(string id)
        {
            var brewery = new BreweryViewModel()
            {
                Brewery = _breweryOrchestrator.GetById(id),
                Beers = _beerOrchestrator.GetBeersByBrewery(id)
            };
            return View("Details", brewery);
        }

        //
        // GET: /Admin/Brewery/Details/berweryidwer23r
        public ActionResult PartialDetails(string id)
=======
        //
        // GET: /Admin/Brewery/Details/5
=======
>>>>>>> Can create brewery and can add/create beers for the brewery.  Also, added commands
        public ActionResult Details(string id)
>>>>>>> Can create brewery.
        {
            var brewery = new BreweryViewModel()
            {
                Brewery = _breweryOrchestrator.GetById(id),
                Beers = _beerOrchestrator.GetBeersByBrewery(id)
            };
            return View("Details", brewery);
        }

        //
        // GET: /Admin/Brewery/Details/berweryidwer23r
        public ActionResult PartialDetails(string id)
        {
            var breweryViewModel = new BreweryViewModel()
            {
                Brewery = _breweryOrchestrator.GetById(id),
                Beers = _beerOrchestrator.GetBeersByBrewery(id)
            };
            return PartialView("_BreweryDetails", breweryViewModel);
        }

        //
        // GET: /Admin/Brewery/Create
        public ActionResult Create()
        {
            return View("CreateBrewery");
        }

        //
        // POST: /Admin/Brewery/Create
        [HttpPost]
<<<<<<< HEAD
<<<<<<< HEAD
        public ActionResult Create(CreateBrewery breweryCommand)
        {
            try
            {
                var brewreyId = _breweryOrchestrator.Create(breweryCommand);
                return RedirectToAction("Details", new { id = brewreyId });
<<<<<<< HEAD
=======
        public ActionResult Create(FormCollection collection)
=======
        public ActionResult Create(CreateBrewery breweryCommand)
>>>>>>> Can create brewery and can add/create beers for the brewery.  Also, added commands
        {
            try
            {
                _breweryOrchestrator.Create(breweryCommand);
                return RedirectToAction("Index");
>>>>>>> Can create brewery.
=======
>>>>>>> Can edit brewery
            }
            catch
            {
                return View("CreateBrewery");
            }
        }

        //
        // GET: /Admin/Brewery/Edit/5
        public ActionResult Edit(string id)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            var brewery = _breweryOrchestrator.EditBrewery(id);
=======
            var brewery = _breweryOrchestrator.GetById(id);
>>>>>>> Can create brewery.
=======
            var brewery = _breweryOrchestrator.EditBrewery(id);
>>>>>>> Can edit brewery
            return View("Edit", brewery);
        }

        //
        // POST: /Admin/Brewery/Edit/5
        [HttpPost]
<<<<<<< HEAD
<<<<<<< HEAD
        public ActionResult Edit(EditBrewery editBreweryCommand)
        {
            try
            {
                _breweryOrchestrator.EditBrewery(editBreweryCommand);
                return RedirectToAction("Details", new { id = editBreweryCommand.Id });
=======
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                var brewery = _breweryOrchestrator.GetById(id);
                brewery.Name = collection["Name"];
                brewery.City = collection["City"];
                brewery.State = collection["State"];
                brewery.PostalCode = collection["PostalCode"];
                brewery.Website = collection["Website"];
                brewery.Logo = collection["Logo"];

                return RedirectToAction("Index");
>>>>>>> Can create brewery.
=======
        public ActionResult Edit(EditBrewery editBreweryCommand)
        {
            try
            {
                _breweryOrchestrator.EditBrewery(editBreweryCommand);
                return RedirectToAction("Details", new { id = editBreweryCommand.Id });
>>>>>>> Can edit brewery
            }
            catch
            {
                return View("Edit");
            }
        }

        //
        // GET: /Admin/Brewery/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Brewery/Delete/5
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
        }
    }
}
