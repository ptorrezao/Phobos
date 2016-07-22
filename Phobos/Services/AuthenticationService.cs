using Ninject;
using Phobos.Library.Interfaces;
using Phobos.Library.Interfaces.Services;
using Phobos.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Phobos
{
    public class AuthenticationService : IAuthenticationService
    {
        [Inject]
        public INotificationService NotificationService { get; set; }

        public IUserManagementService userManagementService
        {
            get
            {
                return Phobos.MvcApplication.GetKernel().Get<IUserManagementService>();
            }
        }

        public void Login(string username, bool rememberMe)
        {
            userManagementService.UpdateAccountForLogin(username);
            FormsAuthentication.SetAuthCookie(username, rememberMe);
        }

        public void Logout(string username)
        {
            userManagementService.UpdateAccountForLogout(username);
            FormsAuthentication.SignOut();
        }
    }
}