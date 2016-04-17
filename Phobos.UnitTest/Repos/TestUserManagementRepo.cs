using Ninject;
using Phobos.Library.Interfaces.Repos;
using Phobos.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Utils;

namespace Phobos.UnitTest.Repos
{
    public class TestUserManagementRepo : IUserManagementRepo
    {
        [Inject]
        public ICoreRepo CoreRepository { get; set; }

        static List<UserAccount> list = new List<UserAccount>() { };

        public bool CheckIfUserExist(string userName)
        {
            return list.Any(x => x.Username == userName);
        }

        public UserAccount GetUser(string userName)
        {
            return list.FirstOrDefault(x => x.Username == userName);
        }

        public bool LockUserAccount(string userName)
        {
            var usr = this.GetUser(userName);
            usr.IsLocked = true;
            usr.LockedDate = DateTime.Now;
            return UpdateAccount(usr);
        }

        public bool AddFailedLoginAttempt(string userName)
        {
            var usr = this.GetUser(userName);
            usr.FailedAttempts++;
            return UpdateAccount(usr);
        }

        public bool UnlockUserAccount(string userName)
        {
            var usr = this.GetUser(userName);
            usr.FailedAttempts = 0;
            usr.IsLocked = false;
            usr.LockedDate = null;
            return UpdateAccount(usr);
        }

        public bool UpdateLastLoginDate(string userName, DateTime now)
        {
            var usr = this.GetUser(userName);
            usr.LastLoginDate = now;
            return UpdateAccount(usr);
        }

        public ActionAuthorization GetAutorizationForAction(string currentControllerName, string currentActionName)
        {
            throw new NotImplementedException();
        }

        public UserAccount CreateUser(string name, string userName, string password)
        {
            if (!list.Any(x => x.Username == userName))
            {
                list.Add(new UserAccount() { Username = userName, FirstName = name, Password = password });
                return list.First(x => x.Username == userName);
            }
            else
            {
                return list.First(x => x.Username == userName);
            }
        }

        public bool UpdateAccount(UserAccount userAccount)
        {
            if (list.Any(x => x.Username == userAccount.Username))
            {
                list.RemoveAll(x => x.Username == userAccount.Username);
                list.Add(userAccount);
                return list.Any(x => x.Username == userAccount.Username);
            }
            return false;
        }

        public List<UserMessage> GetLastMessagesForUser(string userName, int qtd)
        {
            throw new NotImplementedException();
        }

        public List<UserNotification> GetLastNotificationsForUser(string userName, int qtd)
        {
            throw new NotImplementedException();
        }

        public List<UserTask> GetLastTasksForUser(string userName, int qtd)
        {
            throw new NotImplementedException();
        }

        public List<UserAccount> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}
