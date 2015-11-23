using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phobos.ActionFilter
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SideTabFilterAttribute : ActionFilterAttribute
    {
        SideTabViewModel sideTab;

        public SideTabFilterAttribute(string action, string controller, string fontAwesomeIcon)
        {
            sideTab = new SideTabViewModel();
            sideTab.Id = Guid.NewGuid();
            sideTab.FontAwesomeIcon = fontAwesomeIcon;
            sideTab.IsActive = false;
            sideTab.Action = action;
            sideTab.Controller = controller;
            sideTab.RouteValues = new { };
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sideBars = new List<SideTabViewModel>();
            if (filterContext.Controller.ViewBag.SideTabs != null)
            {
                sideBars = filterContext.Controller.ViewBag.SideTabs;
            }
            sideBars.Add(sideTab);
            filterContext.Controller.ViewBag.SideTabs = sideBars;

            base.OnActionExecuting(filterContext);
        }
    }
}