using Phobos.Library.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phobos.Library.Models
{
    public class UserNotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public TextColor IconColor { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public bool Read { get; set; }
        public NotificationType Type { get; set; }
        public virtual UserAccount User { get; set; }
        public static UserNotification Welcome
        {
            get
            {
                return new UserNotification()
                {
                    Type = NotificationType.Welcome,
                    Read = false,
                    Icon = "child",
                    IconColor = TextColor.Blue,
                    Title = "Welcome!!!",
                };
            }
        }
        public static UserNotification LastLogin(UserAccount user)
        {
            return new UserNotification()
            {
                Type = NotificationType.Login,
                Read = false,
                User = user,
                Icon = "search-minus",
                IconColor = TextColor.Blue,
                Title = string.Format("Welcome back {0} last time you been here was {1}.", user.FirstName, user.LastLoginDate.ToString())
            };
        }
    }
}