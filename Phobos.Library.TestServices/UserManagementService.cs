using Phobos.Library.Interfaces;
using Phobos.Library.Models;
using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.TestServices
{
    public class UserManagementService : IUserManagementService
    {
        public bool CheckIfUserIsValid(string username, string password, out string msg)
        {
            msg = "";
            return true;
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
                  Username="ptorrezao"
            };
        }
    }
}
