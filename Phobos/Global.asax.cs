using Ninject;
using Ninject.Web.Common;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            //kernel.Bind<IInterface>().To<Interface>();
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
