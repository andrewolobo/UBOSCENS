using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UBOSCENS.Models;

namespace UBOSCENS.Controllers
{
    public class UBOSDMController : Controller
    {
        // GET: UBOSDM
        public ActionResult Index()
        {
            Tables psmr = new Tables();


            return View();
        }
    }
}
