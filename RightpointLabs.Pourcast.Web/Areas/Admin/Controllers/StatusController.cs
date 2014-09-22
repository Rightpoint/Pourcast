using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class StatusController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}