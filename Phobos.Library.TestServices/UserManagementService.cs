using Phobos.Library.Interfaces;
using Phobos.Library.Models;
using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Models.ViewModels;

namespace Phobos.Library.TestServices
{
    public class UserManagementService : IUserManagementService
    {
        static List<string> users = new List<string>();
        public bool CheckIfRegisterIsAllowed(string name, string userName, string password, string confirmPassword, out string error)
        {
            if (users.Any(x => x == userName))
            {
                error = "Duplicated User";
                return false;
            }

            error = "";
            users.Add(userName);
            return true;
        }

        public bool RegisterUser(string name, string userName, string password, string confirmPassword, out string error)
        {
            error = "";
            return true;
        }

        public bool CheckIfUserIsValid(string username, string password, out string msg)
        {
            if (username == "fakeUser")
            {
                msg = "Non Existing User";
                return false;
            }

            Guid guid = Guid.Empty;
            if (Guid.TryParse(username, out guid))
            {
                msg = "";
                return false;
            }

            msg = "";
            return true;

        }

        public List<UserMessage> GetLastMessages(string userName, int qtd)
        {
            var list = new List<UserMessage>();
            return list;
        }

        public List<UserNotification> GetLastNotifications(string name, int qtd)
        {
            var list = new List<UserNotification>();
            return list;
        }

        public List<UserTask> GetLastTasks(string userName, int qtd)
        {
            var list = new List<UserTask>();
            return list;
        }

        public UserAccount GetUser(string username)
        {
            return new UserAccount()
            {
                FirstName = "Pedro",
                LastName = "Torrezao",
                BirthDate = new DateTime(1988, 10, 3),
                MemberSinceDate = new DateTime(1988, 10, 3),
                Position = "Software Engineer",
                CurrentStatus = UserStatusEnum.Online,
                Username = "ptorrezao"
            };
        }

        public bool RecoverProfile(string userName, out string error)
        {
            error = "";
            if (userName == "NonExistingUser")
            {
                error = "Non Existing User";
                return false;
            }

            if (userName == "UserWithoutEmail")
            {
                error = "User Without Email";
                return false;
            }
            return true;
        }

        public bool CheckSecurityMesurements(string userName, string password, string confirmPassword, out string error)
        {
            if (string.IsNullOrEmpty(password))
            {
                error = "Password does not match";
                return false;
            }

            if (password.Length<=3)
            {
                error = "Password Should have more than";
                return false;
            }

            if (password!= confirmPassword)
            {
                error = "Password does not match";
                return false;
            }
            error = "";
            return true;
        }
    }
}
