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

        public ActionAutorizeAttribute() : this(MvcApplication.GetKernel().Get<IUserManagementService>()) { }

        public ActionAutorizeAttribute(IUserManagementService userMngSvc)
        {
            this.userManagementService = userMngSvc;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string currentControllerName = (string)filterContext.RouteData.Values["controller"];
            string currentActionName = (string)filterContext.RouteData.Values["action"];

            if (!userManagementService.CheckIfActionIsAllowed(currentControllerName, currentActionName, SessionManager.UserAccount.Username))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                base.OnActionExecuting(filterContext);
            }
        }
    }
}