using Ninject;
using Phobos.Library.Interfaces;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phobos
{
    public class PhobosInitializationFilterAttribute : ActionFilterAttribute
    {
        private IUserManagementService UserManagement;

        public PhobosInitializationFilterAttribute() : this(MvcApplication.GetKernel().Get<IUserManagementService>()) { }

        public PhobosInitializationFilterAttribute(IUserManagementService _service)
        {
            this.UserManagement = _service;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region Set User Account
            if (SessionManager.UserAccount == null)
            {
                SessionManager.UserAccount = UserAccountViewModel.AsUserAccountViewModel(this.UserManagement.GetUser(filterContext.HttpContext.User.Identity.Name));
            }
            filterContext.Controller.ViewBag.UserAccount = SessionManager.UserAccount;
            #endregion

            filterContext.Controller.ViewBag.Menus = new MenuEntriesListViewModel();
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}