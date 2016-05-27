using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Phobos.Library.Models
{
    public class UserAccount
    {
        #region Constructor
        public UserAccount()
        {
            this.Messages = new List<UserMessage>();
            this.Notifications = new List<UserNotification>();
            this.Tasks = new List<UserTask>();
            this.Roles = new List<UserRole>();
            this.ActionAuthorizations = new List<ActionAuthorization>();
        } 
        #endregion

        #region Properties
        [Key]
        public string Username { get; set; }
        public DateTime? LockedDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime MemberSinceDate { get; set; }
        public UserStatusEnum CurrentStatus { get; set; }
        public string FirstName { get; set; }
        public bool IsLocked { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Position { get; set; }
        public int FailedAttempts { get; set; }

        public virtual List<UserMessage> Messages { get; set; }
        public virtual List<UserNotification> Notifications { get; set; }
        public virtual List<UserTask> Tasks { get; set; }
        public virtual List<ActionAuthorization> ActionAuthorizations { get; set; }
        public virtual List<UserRole> Roles { get; set; } 
        #endregion
    }
}
