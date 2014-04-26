using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RightpointLabs.Pourcast.Application.Commands;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Web.Areas.Admin.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class BeerController : Controller
    {
        static BeerController()
        {
            AutoMapper.Mapper.CreateMap<Beer, CreateBeerViewModel>();
            AutoMapper.Mapper.CreateMap<CreateBeerViewModel, Beer>();
            AutoMapper.Mapper.CreateMap<Beer, BeerViewModel>();
            AutoMapper.Mapper.CreateMap<BeerViewModel, Beer>();
        }
        private readonly IBeerOrchestrator _beerOrchestrator;
        private readonly IBreweryOrchestrator _breweryOrchestrator;

        public BeerController(IBeerOrchestrator beerOrchestrator, IBreweryOrchestrator breweryOrchestrator)
        {
            if (beerOrchestrator == null) throw new ArgumentNullException("beerOrchestrator");
            if (null == breweryOrchestrator) throw new ArgumentNullException("breweryOrchestrator");
            _beerOrchestrator = beerOrchestrator;
            _breweryOrchestrator = breweryOrchestrator;
        }

        //
        // GET: /Admin/Beer/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Admin/Beer/Details/My-Beer-Name
        public ActionResult Details(string id)
        {
            var beer = _beerOrchestrator.GetById(id);

            return View(AutoMapper.Mapper.Map<Beer, BeerViewModel>(beer));
        }

        //
        // GET: /Admin/Beer/Create
        public ActionResult Create(string breweryId)
        {
            var brewery = _breweryOrchestrator.GetById(breweryId);
            if (null != brewery)
                return View("Create", new CreateBeerViewModel() {BreweryId = brewery.Id, BreweryName = brewery.Name});

            ModelState.AddModelError("Brewery", "Brewery with that id does not exist.");
            return View("Create", new CreateBeerViewModel(){BreweryId = breweryId});
        }

        //
        // POST: /Admin/Beer/Create
        [HttpPost]
        public ActionResult Create(CreateBeerViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            var existing = _beerOrchestrator.GetByBrewery(model.BreweryId);
            if (existing.Any(b => b.Name == model.Name))
            {
                ModelState.AddModelError("BeerName", "A beer with that name already exists for this brewery.");
                return View("Create", model);
            }

            string id = _beerOrchestrator.CreateBeer(model.Name, model.ABV, model.BAScore, model.Style, model.Color, model.Glass,
                model.BreweryId);

            return RedirectToAction("Details", id);
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
