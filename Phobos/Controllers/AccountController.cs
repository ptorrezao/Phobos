using Phobos.ActionFilter;
using Phobos.App_Utils;
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
using System.Web.Security;

namespace Phobos.Controllers
{
    public class AccountController : Controller
    {
        private IAuthenticationService AuthenticationService;
        private IUserManagementService userManagementService;
        private IAuditTrailService auditTrailService;

        public AccountController(IUserManagementService usrMngSvc, IAuthenticationService authSvc, IAuditTrailService auditTrailService)
        {
            this.userManagementService = usrMngSvc;
            this.AuthenticationService = authSvc;
            this.auditTrailService = auditTrailService;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
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

            base.OnActionExecuted(filterContext);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new AccountViewModel());
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Login(AccountViewModel user)
        {
            var error = "";
            if (ModelState.IsValid)
            {
                using (MiniProfiler.Current.Step("CheckIfUserIsValid"))
                {
                    if (this.userManagementService.CheckIfUserIsValid(user.UserName, user.Password, out error))
                    {
                        using (MiniProfiler.Current.Step("GetUser"))
                        {
                            AuthenticationService.Login(user.UserName, user.RememberMe);

                            SessionManager.UserAccount = AutoMapperConfiguration.GetMapper().Map<UserAccountViewModel>(this.userManagementService.GetUser(user.UserName));

                            return RedirectToAction("Index", "Home");
                        }
                    }
                    error = string.IsNullOrEmpty(error) ? "Something went wrong, please try again later." : error;
                    ModelState.AddModelError("UserName", error);
                }
            }


            return View(user);
        }

        public ActionResult Logout()
        {
            AuthenticationService.Logout();

            SessionManager.UserAccount = null;

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Register(RegisterViewModel user)
        {
            var error = "";
            if (ModelState.IsValid)
            {
                using (MiniProfiler.Current.Step("RegisterUser"))
                {
                    if (this.userManagementService.RegisterUser(user.Name, user.UserName, user.Password, user.ConfirmPassword, out error))
                    {
                        AuthenticationService.Login(user.UserName, false);

                        SessionManager.UserAccount = AutoMapperConfiguration.GetMapper().Map<UserAccountViewModel>(this.userManagementService.GetUser(user.UserName));

                        this.auditTrailService.LogMessage(string.Format("A new user ({0}) had been created.", user.UserName), user.UserName, user);

                        return RedirectToAction("Index", "Home");
                    }
                }

                error = string.IsNullOrEmpty(error) ? "Something went wrong, please try again later." : error;
                ModelState.AddModelError("ConfirmPassword", error);
            }

            return View(user);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(new RecoverProfileViewModel());
        }

        [HttpPost, AllowAnonymous]
        public ActionResult ForgotPassword(RecoverProfileViewModel user)
        {
            var error = "";
            using (MiniProfiler.Current.Step("RecoverProfile"))
            {
                if (ModelState.IsValid && this.userManagementService.RecoverProfile(user.Usename, out error))
                {
                    this.auditTrailService.LogMessage(string.Format("The user {0} had request a new password.", SessionManager.UserAccount.Username), SessionManager.UserAccount.Username, user);

                    return RedirectToAction("Login", "Account");
                }
            }

            ViewBag.Message = error;
            return View(user);
        }

        [ActionAutorize]
        [PhobosInitialization]
        public ActionResult EditProfile()
        {
            var model = SessionManager.UserAccount;
            if (model == null)
            {
                model = SessionManager.UserAccount = AutoMapperConfiguration.GetMapper().Map<UserAccountViewModel>(this.userManagementService.GetUser(this.User.Identity.Name));
            }

            return View(model);
        }

        [HttpPost]
        [ActionAutorize]
        [PhobosInitialization]
        public ActionResult EditProfile(UserAccountViewModel model)
        {
            var userAccount = AutoMapperConfiguration.GetMapper().Map<UserAccount>(model);

            this.userManagementService.UpdateAccount(userAccount);

            SessionManager.UserAccount = model;

            this.auditTrailService.LogMessage(string.Format("The user {0} had changed his profile.", SessionManager.UserAccount.Username), SessionManager.UserAccount.Username, userAccount);
            
            return PartialView("_ProfileDetails", model);
        }
    }
}
