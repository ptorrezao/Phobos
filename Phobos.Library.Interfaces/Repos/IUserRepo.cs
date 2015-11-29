using Phobos.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Interfaces.Repos
{
    public interface IUserManagementRepo
    {
        bool CheckIfUserExist(string userName);
        UserAccount GetUser(string userName);
        bool AddFailedLoginAttempt(string userName);
        bool UnlockUserAccount(string userName);
        bool UpdateLastLoginDate(string userName, DateTime now);
        ActionAuthorization GetAutorizationForAction(string currentControllerName, string currentActionName);
        UserAccount CreateUser(string name, string userName, string password);
        bool UpdateAccount(UserAccount userAccount);
        List<UserMessage> GetLastMessagesForUser(string userName, int qtd);
        List<UserNotification> GetLastNotificationsForUser(string userName, int qtd);
        List<UserTask> GetLastTasksForUser(string userName, int qtd);
    }
}
