using System;
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