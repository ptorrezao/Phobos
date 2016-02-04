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
        UserAccount GetUser(string userName);
        List<UserMessage> GetLastMessages(string userName, int qtd);
        List<UserNotification> GetLastNotifications(string userName, int qtd);
        List<UserTask> GetLastTasks(string userName, int qtd);
        bool RegisterUser(string name, string userName, string password, string confirmPassword, out string error);
        bool RecoverProfile(string userName, out string error);
        bool CheckIfActionIsAllowed(string currentControllerName, string currentActionName, string username);
        bool UpdateAccount(UserAccount userAccount);
    }
}
