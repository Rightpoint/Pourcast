using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Web.Areas.Admin.Models;
using WebGrease.Css.Extensions;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Web.SignalR;

    [Authorize(Roles = "Administrators")]
    public class HomeController : Controller
    {
        private readonly ITapOrchestrator _tapOrchestrator;
        private readonly IKegOrchestrator _kegOrchestrator;
        private readonly IBeerOrchestrator _beerOrchestrator;

        private readonly IConnectionManager _connectionManager;

        static HomeController()
        {

            AutoMapper.Mapper.CreateMap<Keg, KegModel>();
            AutoMapper.Mapper.CreateMap<KegModel, Keg>();
            AutoMapper.Mapper.CreateMap<Tap, TapModel>();
            AutoMapper.Mapper.CreateMap<TapModel, Tap>();
        }

        public HomeController(ITapOrchestrator tapOrchestrator, IKegOrchestrator kegOrchestrator, IBeerOrchestrator beerOrchestrator, IConnectionManager connectionManager)
        {
            if (tapOrchestrator == null) throw new ArgumentNullException("tapOrchestrator");
            if (kegOrchestrator == null) throw new ArgumentNullException("kegOrchestrator");
            if (beerOrchestrator == null) throw new ArgumentNullException("beerOrchestrator");
            if (connectionManager == null) throw new ArgumentNullException("connectionManager");
            _tapOrchestrator = tapOrchestrator;
            _kegOrchestrator = kegOrchestrator;
            _beerOrchestrator = beerOrchestrator;
            _connectionManager = connectionManager;
        }

        //
        // GET: /Admin/Home/
        public ActionResult Index()
        {
            // TODO Talk about how to figure out which keg is on which tap
            var taps = _tapOrchestrator.GetTaps();
            var vm = new List<TapModel>();
            taps.ForEach(t =>
            {
                var tap = AutoMapper.Mapper.Map<Tap, TapModel>(t);
                if (t.HasKeg)
                {
                    var keg = _kegOrchestrator.GetKeg(t.KegId);
                    tap.Keg = AutoMapper.Mapper.Map<Keg, KegModel>(keg);
                    if (null != keg)
                    {
                        var beer = _beerOrchestrator.GetById(keg.BeerId);
                        tap.Keg.BeerName = beer.Name;
                    }
                }
                vm.Add(tap);
            });
            return View("Index", vm);
        }

        public ActionResult Refresh()
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            context.Clients.All.Refresh();

            return null;
        }
	}
}