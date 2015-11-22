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
    }
}
