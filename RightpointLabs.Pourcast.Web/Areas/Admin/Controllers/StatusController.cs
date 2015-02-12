using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RightpointLabs.Pourcast.Domain.Repositories;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class StatusController : Controller
    {
        private readonly IStoredEventRepository _storedEventRepository;

        public StatusController(IStoredEventRepository storedEventRepository)
        {
            _storedEventRepository = storedEventRepository;
        }

        public ActionResult Index()
        {
            return View(_storedEventRepository.GetLatest(50));
        }
    }
}