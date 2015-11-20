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
        private IUserManagementService UserManagement;

        public AccountController(IUserManagementService usrMngSvc)
        {
            this.UserManagement = usrMngSvc;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost,AllowAnonymous]
        public ActionResult Login(AccountViewModel user)
        {
            var error = "";
            if (ModelState.IsValid)
            {
                if (this.UserManagement.CheckIfUserIsValid(user.UserName, user.Password, out error))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                  
                    SessionManager.UserAccount = UserAccountViewModel.AsUserAccountViewModel(this.UserManagement.GetUser(user.UserName));
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(user);
        }
       
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            
            SessionManager.UserAccount = null;

            return RedirectToAction("Index", "Home");
        }
    }
}