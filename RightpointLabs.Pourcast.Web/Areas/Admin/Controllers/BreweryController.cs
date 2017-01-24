using System;
using System.Linq;
using System.Web.Mvc;

using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Web.Areas.Admin.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class BreweryController : Controller
    {
        static BreweryController()
        {
            AutoMapper.Mapper.CreateMap<Brewery, EditBreweryViewModel>();
            AutoMapper.Mapper.CreateMap<EditBreweryViewModel, Brewery>();
        }

        private readonly IBreweryOrchestrator _breweryOrchestrator;
        private readonly IBeerOrchestrator _beerOrchestrator;

        public BreweryController(IBreweryOrchestrator breweryOrchestrator, IBeerOrchestrator beerOrchestrator)
        {
            if(breweryOrchestrator == null) throw new ArgumentNullException(nameof(breweryOrchestrator));
            if(beerOrchestrator == null) throw new ArgumentNullException(nameof(beerOrchestrator));

            _breweryOrchestrator = breweryOrchestrator;
            _beerOrchestrator = beerOrchestrator;
        }

        //
        // GET: /Admin/Brewery/
        public ActionResult Index()
        {
            var breweries = _breweryOrchestrator.GetBreweries().ToList();

            return View(breweries);
        }

        public ActionResult Details(string id)
        {
            var brewery = new BreweryViewModel()
            {
                Brewery = _breweryOrchestrator.GetById(id),
                Beers = _beerOrchestrator.GetByBrewery(id)
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
                Beers = _beerOrchestrator.GetByBrewery(id)
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
        public ActionResult Create(CreateBreweryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateBrewery", model);
            }

            var existing = _breweryOrchestrator.GetByName(model.Name);
            if (null == existing)
            {
                var brewreyId = _breweryOrchestrator.Create(model.Name, model.City, model.State, model.Country,
                    model.PostalCode, model.Website, model.Logo);
                return RedirectToAction("Details", new {id = brewreyId});
            }

            ModelState.AddModelError("BreweryExists", "A brewery with that name already exists");
            return View("CreateBrewery", model);
        }

        //
        // GET: /Admin/Brewery/Edit/5
        public ActionResult Edit(string id)
        {
            var brewery = _breweryOrchestrator.GetById(id);
            return View("Edit", AutoMapper.Mapper.Map<Brewery, EditBreweryViewModel>(brewery));
        }

        [HttpPost]
        public ActionResult Edit(EditBreweryViewModel model)
        {
            var existing = _breweryOrchestrator.GetById(model.Id);
            if (null == existing)
            {
                // Return view with Error
                ModelState.AddModelError("Brewery", "No brewery with that id exists.");
            }

            _breweryOrchestrator.Save(AutoMapper.Mapper.Map<EditBreweryViewModel, Brewery>(model));
            return RedirectToAction("Details", new {id = model.Id});
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
