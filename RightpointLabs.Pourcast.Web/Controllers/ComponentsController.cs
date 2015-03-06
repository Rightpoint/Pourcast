using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using RightpointLabs.Pourcast.Web.Models;

namespace RightpointLabs.Pourcast.Web.Controllers
{
    public class ComponentsController : Controller
    {
        // GET: Components
        public ActionResult Templates()
        {
            var componentsPath = this.Server.MapPath("~/Scripts/app/components");
            var directory = new DirectoryInfo(componentsPath);

            var mainComponentDirectories = directory.EnumerateDirectories().Where(x => x.Name.ToLower() != "movember");
            var templates = mainComponentDirectories.SelectMany(x => x.GetFiles("template.html"));

            var viewModels = templates.Select(x => new ComponentTemplateViewModel(x));
            return this.PartialView(viewModels);
        }
    }
}