using Ninject;
using Phobos.Library.Interfaces;
using Phobos.Library.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Phobos.ActionFilter
{
    public class ActionAutorizeAttribute : ActionFilterAttribute
    {
        private INavigationService navigationService;
        private bool AllowEvenIfNotCreated;

        public ActionAutorizeAttribute() : this(false, MvcApplication.GetKernel().Get<INavigationService>()) { }
        public ActionAutorizeAttribute(bool allowEvenIfNotCreated) : this(allowEvenIfNotCreated, MvcApplication.GetKernel().Get<INavigationService>()) { }

        public ActionAutorizeAttribute(bool allowEvenIfNotCreated, INavigationService userMngSvc)
        {
            this.AllowEvenIfNotCreated = allowEvenIfNotCreated;
            this.navigationService = userMngSvc;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string currentControllerName = (string)filterContext.RouteData.Values["controller"];
            string currentActionName = (string)filterContext.RouteData.Values["action"];
            if (SessionManager.UserAccount == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                base.OnActionExecuting(filterContext);
            }
            else
            {
                if (!navigationService.CheckIfActionIsAllowed(currentControllerName, currentActionName, SessionManager.UserAccount.Username) && !this.AllowEvenIfNotCreated)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                    base.OnActionExecuting(filterContext);
                }
            }
        }
    }
}