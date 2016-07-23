using Phobos.Library.Models;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Interfaces.Services;

namespace Phobos.Library.Interfaces
{
    public interface IUserManagementService
    {
        bool CheckIfUserIsValid(string userName, string password, out string error);
        List<UserAccount> GetAllUsers();
        UserAccount GetUser(string userName);
        List<UserTask> GetLastTasks(string userName, int qtd);
        bool RegisterUser(string name, string userName, string password, string confirmPassword, out string error);
        bool RecoverProfile(string userName, out string error);
        bool UpdateAccount(UserAccount userAccount);
        void UpdateAccountForLogin(string username);
        void UpdateAccountForLogout(string username);
        bool CreateRole(string username, out string error);
        List<UserRole> GetAllRoles();
        UserRole GetRole(string name);
        bool UpdateRole(string oldName, string newName,List<string> usersInRole, out string error);
        bool DeleteRole(string name, out string error);
    }
}
