using Ninject;
using Phobos.Library.Interfaces;
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
        private IUserManagementService userManagementService;
        private bool AllowEvenIfNotCreated;

        public ActionAutorizeAttribute() : this(false, MvcApplication.GetKernel().Get<IUserManagementService>()) { }
        public ActionAutorizeAttribute(bool allowEvenIfNotCreated) : this(allowEvenIfNotCreated, MvcApplication.GetKernel().Get<IUserManagementService>()) { }

        public ActionAutorizeAttribute(bool allowEvenIfNotCreated, IUserManagementService userMngSvc)
        {
            this.AllowEvenIfNotCreated = allowEvenIfNotCreated;
            this.userManagementService = userMngSvc;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string currentControllerName = (string)filterContext.RouteData.Values["controller"];
            string currentActionName = (string)filterContext.RouteData.Values["action"];

            if (!userManagementService.CheckIfActionIsAllowed(currentControllerName, currentActionName, SessionManager.UserAccount.Username)  && !this.AllowEvenIfNotCreated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                base.OnActionExecuting(filterContext);
            }
        }
    }
}