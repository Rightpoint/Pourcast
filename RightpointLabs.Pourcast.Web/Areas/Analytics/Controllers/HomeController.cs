using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Web.Areas.Analytics.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Analytics.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsOrchestrator _analytics;

        public HomeController(IAnalyticsOrchestrator analytics)
        {
            if(null == analytics) throw new ArgumentNullException("analytics");
            _analytics = analytics;
        }

        // GET: Analytics/Home
        public ActionResult Index()
        {
            var vm = new HomeIndexViewModel();
            vm.BeersBeenOnTap = _analytics.GetBeersThatHaveBeenOnTap();

            return View(vm);
        }
    }
}