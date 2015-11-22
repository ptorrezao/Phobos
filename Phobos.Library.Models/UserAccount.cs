using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Models.Enums;

namespace Phobos.Library.Models
{
    public class UserAccount
    {
        public DateTime BirthDate { get; set; }
        public UserStatusEnum CurrentStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime MemberSinceDate { get; set; }
        public string Position { get; set; }
        public string Username { get; set; }
    }
}
