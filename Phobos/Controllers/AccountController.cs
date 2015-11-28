using Phobos.ActionFilter;
using Phobos.Library.Interfaces;
using Phobos.Library.Models.ViewModels;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Phobos.Controllers
{
    [PhobosInitialization]
    public class AccountController : Controller
    {
        private IAuthenticationService AuthenticationService;
        private IUserManagementService userManagementService;

        public AccountController(IUserManagementService usrMngSvc, IAuthenticationService authSvc)
        {
            this.userManagementService = usrMngSvc;
            this.AuthenticationService = authSvc;
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

                            SessionManager.UserAccount = UserAccountViewModel.AsUserAccountViewModel(this.userManagementService.GetUser(user.UserName));

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            else
            {
                error = string.IsNullOrEmpty(error) ? "Something went wrong, please try again later." : error;
                ModelState.AddModelError("", error);
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
                using (MiniProfiler.Current.Step("CheckIfRegisterIsAllowed"))
                {
                    if (this.userManagementService.CheckIfRegisterIsAllowed(user.Name, user.UserName, user.Password, user.ConfirmPassword, out error))
                    {
                        using (MiniProfiler.Current.Step("CheckSecurityMesurements"))
                        {
                            if (this.userManagementService.CheckSecurityMesurements(user.UserName, user.Password, user.ConfirmPassword, out error))
                            {
                                using (MiniProfiler.Current.Step("RegisterUser"))
                                {
                                    if (this.userManagementService.RegisterUser(user.Name, user.UserName, user.Password, user.ConfirmPassword, out error))
                                    {
                                        AuthenticationService.Login(user.UserName, false);

                                        SessionManager.UserAccount = UserAccountViewModel.AsUserAccountViewModel(this.userManagementService.GetUser(user.UserName));

                                        return RedirectToAction("Index", "Home");
                                    }
                                }
                            }
                        }
                    }

                    error = string.IsNullOrEmpty(error) ? "Something went wrong, please try again later." : error;
                    ModelState.AddModelError("", error);
                }
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
                    return RedirectToAction("Login", "Account");
                }
            }

            ViewBag.Message = error;
            return View(user);
        }

        [ActionAutorize]
        public ActionResult EditProfile()
        {
            var model = SessionManager.UserAccount;
            if (model == null)
            {
                model = SessionManager.UserAccount = UserAccountViewModel.AsUserAccountViewModel(this.userManagementService.GetUser(this.User.Identity.Name));
            }

            return View(model);
        }

        [HttpPost, ActionAutorize]
        public ActionResult EditProfile(UserAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userAccount = UserAccountViewModel.AsUserAccount(model);
                this.userManagementService.UpdateAccount(userAccount);
            }
            return PartialView("_ProfileDetails", model);
        }
    }
}