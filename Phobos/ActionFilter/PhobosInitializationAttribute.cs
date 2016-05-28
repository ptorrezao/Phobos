using Ninject;
using Phobos.App_Utils;
using Phobos.Controllers;
using Phobos.Library.Interfaces;
using Phobos.Library.Interfaces.Services;
using Phobos.Library.Models;
using Phobos.Library.Models.ViewModels;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Phobos
{
    public class PhobosInitializationAttribute : ActionFilterAttribute
    {
        private IUserManagementService userManagementService;
        private INavigationService navigationService;

        public PhobosInitializationAttribute()
            : this(
                MvcApplication.GetKernel().Get<IUserManagementService>(),
                MvcApplication.GetKernel().Get<INavigationService>())
        { }

        public PhobosInitializationAttribute(IUserManagementService userMngSvc, INavigationService navSvc)
        {
            this.userManagementService = userMngSvc;
            this.navigationService = navSvc;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ResolveCurrentUser(filterContext); // Gets the user from Session or retrevies it again from Service

            if (SessionManager.UserAccount == null)
            {
                if (SessionManager.UserAccount == null)
                {
                    new AuthenticationService().Logout(filterContext.HttpContext.User.Identity.Name);

                    SessionManager.UserAccount = null;

                    filterContext.Result = new RedirectResult("/Account/Login");
                }
            }
            else
            {
                ResolveMenus(filterContext); // Gets the Menus.

                ResolveUserMessages(filterContext); // Get Usermessages

                ResolveUserNotifications(filterContext); // Get Usermessages

                ResolveUserTasks(filterContext); // Get UserTasks

                ResolveFooter(filterContext); // Get version

            }


            base.OnActionExecuting(filterContext);
        }

        private static void ResolveFooter(ActionExecutingContext filterContext)
        {
            using (MiniProfiler.Current.Step("ResolveFooter"))
            {
                UrlHelper helper = new UrlHelper(filterContext.RequestContext, RouteTable.Routes);
                filterContext.Controller.ViewBag.Version = Assembly.GetAssembly(typeof(MvcApplication)).GetName().Version.ToString();
                filterContext.Controller.ViewBag.CompanyUrl = helper.Action("", "", new { });
                filterContext.Controller.ViewBag.CompanyName = "PTZ";
                filterContext.Controller.ViewBag.ShortPageTitle = "";
                filterContext.Controller.ViewBag.PageTitle = "Phobos";
            }
        }

        private void ResolveUserTasks(ActionExecutingContext filterContext)
        {
            using (MiniProfiler.Current.Step("ResolveUserTasks"))
            {
                filterContext.Controller.ViewBag.UserTasks = UserTaskViewModel.AsListOfUserTaskViewModel(this.userManagementService.GetLastTasks(filterContext.HttpContext.User.Identity.Name, 10));
            }
        }

        private void ResolveUserMessages(ActionExecutingContext filterContext)
        {
            using (MiniProfiler.Current.Step("ResolveUserMessages"))
            {
                filterContext.Controller.ViewBag.UserMessages = AutoMapperConfiguration.GetMapper().Map<List<UserMessage>, List<UserMessageViewModel>>(this.userManagementService.GetLastMessages(filterContext.HttpContext.User.Identity.Name, 10));
            }
        }

        private void ResolveUserNotifications(ActionExecutingContext filterContext)
        {
            using (MiniProfiler.Current.Step("ResolveUserNotifications"))
            {
                filterContext.Controller.ViewBag.UserNotifications = UserNotificationViewModel.AsListOfUserNotificationViewModel(this.userManagementService.GetLastNotifications(filterContext.HttpContext.User.Identity.Name, 10));
            }
        }

        private void ResolveMenus(ActionExecutingContext filterContext)
        {
            using (MiniProfiler.Current.Step("ResolveMenus"))
            {
                UrlHelper helper = new UrlHelper(filterContext.RequestContext, RouteTable.Routes);
                string currentControllerName = (string)filterContext.RouteData.Values["controller"];
                string currentActionName = (string)filterContext.RouteData.Values["action"];

                var menus = navigationService.GetMenusForUser(SessionManager.UserAccount.Username);

                menus.ForEach(menu =>
                {
                    menu.IsActive = (currentControllerName == menu.Controller && currentActionName == menu.Action);
                    menu.Url = helper.Action(menu.Action, menu.Controller, new { });
                });

                filterContext.Controller.ViewBag.Menus = menus;
            }
        }

        private void ResolveCurrentUser(ActionExecutingContext filterContext)
        {
            using (MiniProfiler.Current.Step("ResolveCurrentUser"))
            {
                if (SessionManager.UserAccount == null)
                {
                    SessionManager.UserAccount = AutoMapperConfiguration.GetMapper().Map<UserAccountViewModel>(this.userManagementService.GetUser(filterContext.HttpContext.User.Identity.Name));
                }

                filterContext.Controller.ViewBag.UserAccount = SessionManager.UserAccount;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}