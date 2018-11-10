using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Domain.Repositories;
using WebGrease.Css.Extensions;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using RightpointLabs.Pourcast.Web.Areas.Admin.Models;

    [Authorize(Roles = "Administrators")]
    public class TapController : Controller
    {
        private readonly ITapOrchestrator _tapOrchestrator;
        private readonly IKegOrchestrator _kegOrchestrator;
        private readonly IBeerOrchestrator _beerOrchestrator;
        private readonly IBreweryOrchestrator _breweryOrchestrator;

        static TapController()
        {
            
            AutoMapper.Mapper.CreateMap<Keg, KegModel>();
            AutoMapper.Mapper.CreateMap<KegModel, Keg>(); 
            AutoMapper.Mapper.CreateMap<Tap, TapModel>();
            AutoMapper.Mapper.CreateMap<TapModel, Tap>();
        }

        public TapController(ITapOrchestrator tapOrchestrator, IKegOrchestrator kegOrchestrator, IBeerOrchestrator beerOrchestrator, IBreweryOrchestrator breweryOrchestrator)
        {
            if(null == tapOrchestrator) throw new ArgumentNullException("tapOrchestrator");
            if(null == kegOrchestrator) throw new ArgumentNullException("kegOrchestrator");
            if(null == beerOrchestrator) throw new ArgumentNullException("beerOrchestrator");
            if(null == breweryOrchestrator) throw new ArgumentNullException("breweryOrchestrator");

            _tapOrchestrator = tapOrchestrator;
            _kegOrchestrator = kegOrchestrator;
            _beerOrchestrator = beerOrchestrator;
            _breweryOrchestrator = breweryOrchestrator;
        }

        //
        // GET: /Admin/Tap/
        public ActionResult Index()
        {
            var taps = _tapOrchestrator.GetTaps();
            var vm = new List<TapModel>();
            taps.ForEach(t =>
            {
                var tap = AutoMapper.Mapper.Map<Tap, TapModel>(t);
                if (t.HasKeg)
                {
                    var keg = _kegOrchestrator.GetKeg(t.KegId); 
                    tap.Keg = AutoMapper.Mapper.Map<Keg, KegModel>(keg);    
                }
                vm.Add(tap);
            });

            return View(vm);
        }

        public ActionResult Edit(string id)
        {
            var kegs = _kegOrchestrator.GetKegs(false).ToList();
            var tap = _tapOrchestrator.GetTapById(id);
            var kegModels = kegs.Select(k => AutoMapper.Mapper.Map<Keg, KegModel>(k)).ToList();
            foreach (var keg in kegs)
            {
                var beer = _beerOrchestrator.GetById(keg.BeerId);
                var km = kegModels.FirstOrDefault(k => k.Id == keg.Id);
                if (null != km) km.BeerName = beer.Name;
            }
            kegModels = kegModels.OrderBy(i => i.BeerName).ToList();
            var vm = new EditTapViewModel(kegModels, tap.KegId);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(EditTapViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var tap = _tapOrchestrator.GetTapById(model.Id);

            if (string.IsNullOrEmpty(model.KegId))
            {
                if (tap.HasKeg)
                {
                    _tapOrchestrator.RemoveKegFromTap(tap.Id);                  
                }
            }
            else if (!tap.HasKeg)
            {
                // Add New
                _tapOrchestrator.TapKeg(tap.Id, model.KegId);
            }
            else if(!tap.KegId.Equals(model.KegId))
            {
                // Remove old, add new
                _tapOrchestrator.RemoveKegFromTap(tap.Id);
                _tapOrchestrator.TapKeg(tap.Id, model.KegId);
            }

            return RedirectToAction("Index");

        }

        public ActionResult Create()
        {
            var kegs = _kegOrchestrator.GetKegs(false).ToList();
            var kegModels = kegs.Select(k => AutoMapper.Mapper.Map<Keg, KegModel>(k)).ToList();
            foreach (var keg in kegs)
            {
                var beer = _beerOrchestrator.GetById(keg.BeerId);
                var km = kegModels.FirstOrDefault(k => k.Id == keg.Id);
                if (null != km) km.BeerName = beer.Name;
            }
            return View(new CreateTapViewModel(kegModels));
        }

        [HttpPost]
        public ActionResult Create(CreateTapViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = _tapOrchestrator.GetByName(model.Name);
            if (null != existing)
            {
                ModelState.AddModelError("Name", "A tap with that name already exists.");
                return View(model);
            }

            if (null == model.KegId)
            {
                _tapOrchestrator.CreateTap(model.Name);
            }
            else
            {
                _tapOrchestrator.CreateTap(model.Name, model.KegId);
            }

            return RedirectToAction("Index");
        }
	}
}