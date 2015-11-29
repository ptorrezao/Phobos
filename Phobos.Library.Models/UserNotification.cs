﻿namespace Phobos.Library.Models
{
    public class UserNotification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual UserAccount User { get; set; }
    }
}