﻿using Ninject;
using Ninject.Web.Common;
using Phobos.Library.Interfaces;
using Phobos.Library.TestServices;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace Phobos
{
    public class MvcApplication : NinjectHttpApplication
    {

        protected override void OnApplicationStarted()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CustomViewEngine());

            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IUserManagementService>().To<UserManagementService>();
            return kernel;

        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }

            HttpContext.Current.Response.AddHeader("x-frame-options", "SAMEORIGIN");
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

    }
}
