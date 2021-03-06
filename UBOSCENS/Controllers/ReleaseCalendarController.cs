﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UBOSCENS.Models;

namespace UBOSCENS.Controllers
{
    public class ReleaseCalendarController : Controller
    {
        // GET: ReleaseCalendar
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();
            var getEvents = db.Events.Where(v=>v.Active==true && v.When>DateTime.Now).OrderBy(t=>t.When).Select(x => x).Take(5);
            var getNextEvent = db.Events.Where(v => v.When > DateTime.Now).Select(x=>x).FirstOrDefault();
            ViewBag.firstEvent = getNextEvent;
            ViewBag.events = getEvents;
            return View();
        }
        public ActionResult Publications()
        {
            DatabaseContext db = new DatabaseContext();
            var stories = db.Stories.Select(x => x).ToList();
            ViewBag.stories = stories;
            return View();
        }
    }
}