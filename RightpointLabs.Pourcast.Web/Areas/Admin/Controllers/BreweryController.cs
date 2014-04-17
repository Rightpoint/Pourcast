﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        //
        // GET: /Admin/Brewery/Details/5
        public ActionResult Details(string id)
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
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var shell = _breweryOrchestrator.GetShell();
                shell.Name = collection["Name"];
                shell.City = collection["City"];
                shell.State = collection["State"];
                shell.PostalCode = collection["PostalCode"];
                shell.Website = collection["Website"];
                shell.Logo = collection["Logo"];
                _breweryOrchestrator.Create(shell);
                return RedirectToAction("Index");
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
            var brewery = _breweryOrchestrator.GetById(id);
            return View("Edit", brewery);
        }

        //
        // POST: /Admin/Brewery/Edit/5
        [HttpPost]
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
