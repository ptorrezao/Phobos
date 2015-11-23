﻿using Phobos.ActionFilter;
using Phobos.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phobos.Controllers
{
    [PhobosInitializationFilterAttribute]

    public class HomeController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [SideTabFilter("TestA", "Home", "fa-home", Order = 0)]
        [SideTabFilter("TestB", "Home", "fa-home", Order = 0)]
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