using Phobos.ActionFilter;
using Phobos.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phobos.Controllers
{
    [ActionAutorize]
    [PhobosInitialization]
    public class HomeController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ActionAutorize]
       // [SideTab("TestA", "Home", "fa-home", Order = 0)]
      //  [SideTab("TestB", "Home", "fa-home", Order = 0)]
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult TestA()
        {
            return PartialView();
        }

        public PartialViewResult TestB()
        {
            return PartialView();
        }
    }
}