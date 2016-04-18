using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phobos.Library.Models.ViewModels
{
    public class MessageMailBoxItemViewModel
    {
        public bool IsFavorite { get; set; }
        public UserAccountViewModel Owner { get; set; }
        public UserAccountViewModel Sender { get; set; }
        public UserAccountViewModel Receiver { get; set; }
        public string Intro { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool HasAttachment { get; set; }
        public DateTime Date { get; set; }
        public int MessageId { get; set; }
    }
}
