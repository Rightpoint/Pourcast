using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Web.Areas.Admin.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITapOrchestrator _tapOrchestrator;
        private readonly IKegOrchestrator _kegOrchestrator;
        private readonly IBeerOrchestrator _beerOrchestrator;

        public HomeController(ITapOrchestrator tapOrchestrator, IKegOrchestrator kegOrchestrator,
            IBeerOrchestrator beerOrchestrator)
        {
            if(tapOrchestrator == null) throw new ArgumentNullException("tapOrchestrator");
            if(kegOrchestrator == null) throw new ArgumentNullException("kegOrchestrator");
            if(beerOrchestrator == null) throw new ArgumentNullException("beerOrchestrator");
            _kegOrchestrator = kegOrchestrator;
        }

        //
        // GET: /Admin/Home/
        public ActionResult Index()
        {
            // TODO Talk about how to figure out which keg is on which tap
            var kegsOnTap = new KegsOnTap();
            var kegs = _kegOrchestrator.GetKegsOnTap();
            var count = 0;
            foreach (var keg in kegs)
            {
                if (count == 0)
                {
                    kegsOnTap.LeftKeg = new OnTapKeg()
                    {
                        Keg = keg,
                        Beer = _beerOrchestrator.GetById(keg.BeerId)
                    };
                }
                else
                {
                    kegsOnTap.RightKeg = new OnTapKeg()
                    {
                        Keg = keg,
                        Beer = _beerOrchestrator.GetById(keg.BeerId)
                    };                    
                }
                count++;
            }

            return View("Index", kegsOnTap);
        }
	}
}