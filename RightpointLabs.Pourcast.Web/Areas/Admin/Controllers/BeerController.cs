using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RightpointLabs.Pourcast.Application.Commands;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    public class BeerController : Controller
    {
        private readonly IBeerOrchestrator _beerOrchestrator;

        public BeerController(IBeerOrchestrator beerOrchestrator)
        {
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

        }
    }
}
