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
    [PhobosInitializationFilterAttribute]
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
                    AuthenticationService.Login(user.UserName);

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
    }
}