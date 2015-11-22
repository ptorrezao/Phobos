using Phobos.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Interfaces
{
    public interface IUserManagementService
    {
        bool CheckIfUserIsValid(string userName, string password, out string error);
        UserAccount GetUser(string userName);
    }
}
