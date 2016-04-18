using Phobos.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Phobos
{
    public class AuthenticationService : IAuthenticationService
    {
        public void Login(string username, bool rememberMe)
        {
            FormsAuthentication.SetAuthCookie(username, rememberMe);
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

    }
}