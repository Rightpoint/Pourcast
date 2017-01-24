using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Web.Areas.Admin.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Controllers
{
    public class StyleController : Controller
    {
        static StyleController()
        {
            AutoMapper.Mapper.CreateMap<Style, CreateStyleViewModel>();
            AutoMapper.Mapper.CreateMap<CreateStyleViewModel, Style>();
            AutoMapper.Mapper.CreateMap<Style, EditStyleViewModel>();
            AutoMapper.Mapper.CreateMap<EditStyleViewModel, Style>();
        }

        private readonly IStyleOrchestrator _styleOrchestrator;

        public StyleController(IStyleOrchestrator styleOrchestrator)
        {
            if(null == styleOrchestrator)
                throw new ArgumentNullException(nameof(styleOrchestrator));

            _styleOrchestrator = styleOrchestrator;
        }

        // GET: Admin/Style
        public ActionResult Index()
        {
            var styles = _styleOrchestrator.GetStyles();

            return View(styles);
        }

        // GET: Admin/Style/Details/234ew
        public ActionResult Details(string id)
        {
            var style = _styleOrchestrator.GetStyleById(id);
            if (null == style)
                return RedirectToAction("Index");

            return View(style);
        }

        // GET: Admin/Style/Create
        public ActionResult Create()
        {
            var viewModel = new CreateStyleViewModel();

            return View(viewModel);
        }

        // POST: Admin/Style/Create
        [HttpPost]
        public ActionResult Create(CreateStyleViewModel model)
        {
            try
            {
                if (false == ModelState.IsValid)
                    return View(model);

                var style = _styleOrchestrator.CreateStyle(model.Name, model.Color, model.Glass);
                return RedirectToAction("Details", "Style", new {id = style.Id});
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Admin/Style/Edit/5
        public ActionResult Edit(string id)
        {
            var style = _styleOrchestrator.GetStyleById(id);
            if (null == style)
                return RedirectToAction("Index");

            var model = AutoMapper.Mapper.Map<Style, EditStyleViewModel>(style);
            return View(model);
        }

        // POST: Admin/Style/Edit/5
        [HttpPost]
        public ActionResult Edit(EditStyleViewModel model)
        {
            var style = AutoMapper.Mapper.Map<EditStyleViewModel, Style>(model);
            _styleOrchestrator.Save(style);
            return RedirectToAction("Details", new { id = model.Id });
        }

        // GET: Admin/Style/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Style/Delete/5
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
