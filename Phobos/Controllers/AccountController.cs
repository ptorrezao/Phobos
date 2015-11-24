using Phobos.Library.Interfaces;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Phobos.Controllers
{
    [PhobosInitializationFilter]
    public class AccountController : Controller
    {
        private IAuthenticationService AuthenticationService;
        private IUserManagementService UserManagement;

        public AccountController(IUserManagementService usrMngSvc, IAuthenticationService authSvc)
        {
            this.UserManagement = usrMngSvc;
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
                if (this.UserManagement.CheckIfUserIsValid(user.UserName, user.Password, out error))
                {
                    AuthenticationService.Login(user.UserName, user.RememberMe);

                    SessionManager.UserAccount = UserAccountViewModel.AsUserAccountViewModel(this.UserManagement.GetUser(user.UserName));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    error = string.IsNullOrEmpty(error) ? "Something went wrong, please try again later." : error;
                    ModelState.AddModelError("", error);
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
                if (this.UserManagement.CheckIfRegisterIsAllowed(user.Name, user.UserName, user.Password, user.ConfirmPassword, out error))
                {
                    if (this.UserManagement.CheckSecurityMesurements( user.UserName, user.Password, user.ConfirmPassword, out error))
                    {
                        if (this.UserManagement.RegisterUser(user.Name, user.UserName, user.Password, user.ConfirmPassword, out error))
                        {
                            AuthenticationService.Login(user.UserName, false);

                            SessionManager.UserAccount = UserAccountViewModel.AsUserAccountViewModel(this.UserManagement.GetUser(user.UserName));

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                error = string.IsNullOrEmpty(error) ? "Something went wrong, please try again later." : error;
                ModelState.AddModelError("", error);
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
            if (ModelState.IsValid && this.UserManagement.RecoverProfile(user.Usename, out error))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Message = error;
            return View(user);
        }
    }
}