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
        private string[] GroupsAllowed;
        
        public ActionAutorizeAttribute()
            : this(MvcApplication.GetKernel().Get<INavigationService>())
        {
        }

        public ActionAutorizeAttribute(params string[] groupsAllowed)
            : this(MvcApplication.GetKernel().Get<INavigationService>(), groupsAllowed)
        {
        }

        public ActionAutorizeAttribute(bool adminOnly)
            : this(MvcApplication.GetKernel().Get<INavigationService>())
        {
            GroupsAllowed = new string[1] { "Administrator" };
        }

        public ActionAutorizeAttribute(INavigationService userMngSvc, params string[] groupsAllowed)
        {
            this.GroupsAllowed = groupsAllowed;
            this.navigationService = userMngSvc;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string currentControllerName = (string)filterContext.RouteData.Values["controller"];
            string currentActionName = (string)filterContext.RouteData.Values["action"];

            ViewDataDictionary viewbag = new ViewDataDictionary();
            viewbag.Add(new KeyValuePair<string, object>("Controller", currentControllerName));
            viewbag.Add(new KeyValuePair<string, object>("Action", currentActionName));

            bool userIsSet = SessionManager.UserAccount != null;
            bool userIsAllowed = false;
            bool userIsCurrentUserOrAdmin = this.GroupsAllowed != null ? this.GroupsAllowed.Count() == 0 : true;

            #region User Is Allowed
            if (userIsSet &&
                    navigationService.CheckIfActionIsAllowed(currentControllerName, currentActionName, SessionManager.UserAccount.Username))
            {
                userIsAllowed = true;
            }
            #endregion

            #region User Is CurrentUser
            if (filterContext.ActionParameters.Keys.Any(x => x == "username") &&
                filterContext.ActionParameters["username"]!=null &&
                filterContext.ActionParameters["username"].ToString() != SessionManager.UserAccount.Username)
            {
                userIsAllowed = false;

                if (SessionManager.UserAccount.Roles.Any(x => x.IsAdmin))
                {
                    userIsAllowed = true;
                }
            }

            if (userIsSet && this.GroupsAllowed != null && this.GroupsAllowed.Count() > 0)
            {
                userIsAllowed = SessionManager.UserAccount.Roles.Select(x => x.Name)
                                    .Intersect(GroupsAllowed)
                                    .Any();

                if (SessionManager.UserAccount.Roles.Any(x => x.IsAdmin))
                {
                    userIsAllowed = true;
                }

                userIsCurrentUserOrAdmin = userIsAllowed;
            }

            if (userIsSet && SessionManager.UserAccount.Roles.Any(x => x.IsAdmin))
            {
                userIsAllowed = true;
            }
            #endregion

            #region Redirect
            if (!userIsSet ||
                !userIsAllowed ||
                !userIsCurrentUserOrAdmin)
            {
                if (!filterContext.IsChildAction)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                }
                else
                {
                    filterContext.Result = new ViewResult() { ViewName = "_NotAuthorized", ViewData = viewbag };
                }

                base.OnActionExecuting(filterContext);
            }
            #endregion
        }
    }
}