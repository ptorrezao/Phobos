using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Phobos.Library.Models
{
    public class UserAccount
    {
        public DateTime? LockedDate { get; set; }
        public DateTime BirthDate { get; set; }
        public UserStatusEnum CurrentStatus { get; set; }
        public string FirstName { get; set; }
        public bool IsLocked { get; set; }
        public string LastName { get; set; }
        public DateTime MemberSinceDate { get; set; }
        public string Password { get; set; }
        public string Position { get; set; }
        [Key]
        public string Username { get; set; }
        public int FailedAttempts { get; set; }

        public virtual List<UserMessage> Messages { get; set; }
        public virtual List<UserNotification> Notifications { get; set; }
        public virtual List<UserTask> Tasks { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
