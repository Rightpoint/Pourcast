﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
using RightpointLabs.Pourcast.Web.Areas.Analytics.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Analytics.Controllers
{
    public class PoursController : Controller
    {
        private readonly IAnalyticsOrchestrator _analyticsOrchestrator;

        public PoursController(IAnalyticsOrchestrator analyticsOrchestrator)
        {
            _analyticsOrchestrator = analyticsOrchestrator;
        }

        public ActionResult Index()
        {
            var pours = _analyticsOrchestrator.GetRealPoursSince(DateTime.Today.AddDays(-28).ToUniversalTime()).ToList();
            var byDay = (from p in pours
                         let d = p.OccuredOn.ToLocalTime()
                         group p by d.DayOfWeek into g
                         orderby g.Key
                         select new PourAnalysis
                         {
                             Date = g.First().OccuredOn.ToLocalTime(),
                             Day = g.Key,
                             TotalVolume = g.Sum(i => i.Volume),
                             Count = g.Count(),
                         }).ToList();
            var byTime = (from p in pours
                          let d = p.OccuredOn.ToLocalTime()
                          group p by d.Hour into g
                          orderby g.Key
                          select new PourAnalysis
                          {
                              Date = g.First().OccuredOn.ToLocalTime(),
                              Hour = g.Key,
                              TotalVolume = g.Sum(i => i.Volume),
                              Count = g.Count(),
                          }).ToList();
            var lastWeekStart = DateTime.Today.AddDays(-7);
            var byDayAndTime = (from p in pours
                                let d = p.OccuredOn.ToLocalTime()
                                where d >= lastWeekStart
                                group p by new { d.DayOfWeek, d.Hour } into g
                                orderby g.First().OccuredOn
                                select new PourAnalysis
                                {
                                    Date = g.First().OccuredOn.ToLocalTime(),
                                    Day = g.Key.DayOfWeek,
                                    Hour = g.Key.Hour,
                                    TotalVolume = g.Sum(i => i.Volume),
                                    Count = g.Count()
                                }).ToList();
            var byDate = (from p in pours
                                let d = p.OccuredOn.ToLocalTime()
                                group p by d.Date into g
                                orderby g.Key
                                select new PourAnalysis
                                {
                                    Date = g.Key,
                                    TotalVolume = g.Sum(i => i.Volume),
                                    Count = g.Count()
                                }).ToList();

            return View(new PoursIndexModel
            {
                ByDay = byDay,
                ByTime = byTime,
                ByDayAndTime = byDayAndTime,
                ByDate = byDate,
            });
        }
    }
}