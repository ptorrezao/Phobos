using Phobos.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Phobos.App_Utils
{
    public class AuthenticationService : IAuthenticationService
    {
        public void Login(string username)
        {
            FormsAuthentication.SetAuthCookie(username,true);
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}