using System.Runtime.InteropServices;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Web.Areas.Admin.Models;
using WebGrease.Css.Extensions;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class KegController : Controller
    {
        private readonly IKegOrchestrator _kegOrchestrator;
        private readonly IBeerOrchestrator _beerOrchestrator;
        private readonly IBreweryOrchestrator _breweryOrchestrator;

        static KegController()
        {
            AutoMapper.Mapper.CreateMap<Keg, KegModel>();
            AutoMapper.Mapper.CreateMap<KegModel, Keg>();
            AutoMapper.Mapper.CreateMap<Keg, EditKegViewModel>();
        }

        public KegController(IKegOrchestrator kegOrchestrator, IBeerOrchestrator beerOrchestrator,
            IBreweryOrchestrator breweryOrchestrator)
        {
            if(null == kegOrchestrator) throw new ArgumentNullException("kegOrchestrator");    
            if(null == beerOrchestrator) throw new ArgumentNullException("beerOrchestrator");
            if(null == breweryOrchestrator) throw new ArgumentNullException("breweryOrchestrator");

            _kegOrchestrator = kegOrchestrator;
            _beerOrchestrator = beerOrchestrator;
            _breweryOrchestrator = breweryOrchestrator;
        }
        //
        // GET: /Admin/Keg/
        public ActionResult Index()
        {
            var kegs = _kegOrchestrator.GetKegs();
            var model = new KegViewModel(){Kegs = new List<KegModel>()};
            kegs.ForEach((k) =>
            {
                var keg = AutoMapper.Mapper.Map<Keg, KegModel>(k);
                keg.BeerName = _beerOrchestrator.GetById(k.BeerId).Name;
                model.Kegs.Add(keg);
            });

            return View(model);
        }

        public ActionResult Details(string id)
        {
            var existing = _kegOrchestrator.GetKeg(id);

            if (null == existing)
            {
                ViewBag.Error = "No keg exists with that id";
                return View();
            }
            var model = AutoMapper.Mapper.Map<Keg, KegModel>(existing);
            model.BeerName = _beerOrchestrator.GetById(existing.BeerId).Name;
            return View(model);
        }

        public ActionResult Create()
        {
            var vm = new CreateKegViewModel(_beerOrchestrator.GetBeers().OrderBy(i => i.Name).ToList());

            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(CreateKegViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var id = _kegOrchestrator.CreateKeg(model.BeerId, model.Capacity);
            return RedirectToAction("Details", new {id = id});
        }

        // GET: Admin/Keg/Edit/5
        public ActionResult Edit(string id)
        {
            var keg = _kegOrchestrator.GetKeg(id);
            if (null == keg)
                return RedirectToAction("Index");

            var model = AutoMapper.Mapper.Map<Keg, EditKegViewModel>(keg);
            var beer = _beerOrchestrator.GetById(keg.BeerId);
            model.BeerName = beer.Name;
            return View(model);
        }

        // POST: Admin/Keg/Edit/5
        [HttpPost]
        public ActionResult Edit(EditKegViewModel model)
        {
            _kegOrchestrator.UpdateCapacityAndPoured(model.Id, model.Capacity, model.AmountOfBeerPoured);
            return RedirectToAction("Details", new { id = model.Id });
        }
	}
}