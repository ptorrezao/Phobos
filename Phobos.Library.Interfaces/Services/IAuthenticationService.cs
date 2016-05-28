using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Interfaces
{
    public interface IAuthenticationService
    {
        void Login(string username, bool rememberMe);
        void Logout(string username);
    }
}
