using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using RightpointLabs.Pourcast.Domain.Repositories;

namespace RightpointLabs.Pourcast.Web.Controllers
{
    public class FallController : Controller
    {
        private readonly ITapRepository _tapRepository;
        private readonly IKegRepository _kegRepository;
        private readonly IBeerRepository _beerRepository;

        public FallController(ITapRepository tapRepository, IKegRepository kegRepository, IBeerRepository beerRepository)
        {
            _tapRepository = tapRepository;
            _kegRepository = kegRepository;
            _beerRepository = beerRepository;
        }

        // GET: Fall
        public ActionResult Index()
        {
            var taps = _tapRepository.GetAll().Select(tap =>
            {
                var keg = string.IsNullOrEmpty(tap.KegId) ? null : _kegRepository.GetById(tap.KegId);
                var beer = null == keg || string.IsNullOrEmpty(keg.BeerId) ? null : _beerRepository.GetById(keg.BeerId);
                return new
                {
                    tap.Id,
                    KegId = null == keg ? null : keg.Id,
                    BeerId = null == beer ? null : beer.Id,
                    BeerName = null == beer ? null : beer.Name,
                };
            }).ToList();

            return View(taps);
        }

        public ContentResult GetBeerNameForKeg(string kegId)
        {
            var keg = _kegRepository.GetById(kegId);
            if (null == keg)
                return null;
            var beer = _beerRepository.GetById(keg.BeerId);
            if (null == beer)
                return null;
            return Content(beer.Name);
        }
    }
}