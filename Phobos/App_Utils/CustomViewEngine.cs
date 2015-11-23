using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phobos
{
    public class CustomViewEngine : RazorViewEngine
    {
        private bool isViewsSetted;

        public CustomViewEngine()
        {
            this.isViewsSetted = false;
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            this.SetViewLocations();
            return base.FindPartialView(controllerContext, partialViewName, false);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            this.SetViewLocations();
            return base.FindView(controllerContext, viewName, masterName, false);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            this.SetViewLocations();
            return base.CreatePartialView(controllerContext, partialPath);
        }


        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            this.SetViewLocations();
            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        private void SetViewLocations()
        {
            if (isViewsSetted == false)
            {
                this.MasterLocationFormats = GetMainViews(SessionManager.TemplateName);
                this.isViewsSetted = true;
            }

            this.ViewLocationFormats = this.MasterLocationFormats;
            this.PartialViewLocationFormats = this.ViewLocationFormats;
        }

        private string[] GetMainViews(TemplateEnum templatename)
        {
            var viewLocations = new List<string>();

            switch (templatename)
            {
                case TemplateEnum.AdminLTE:
                default:
                    viewLocations.Add("~/Views/AdminLTE/{1}/{0}.cshtml");
                    viewLocations.Add("~/Views/AdminLTE/Shared/{0}.cshtml");
                    break;
            }

            return viewLocations.ToArray();
        }
    }
}