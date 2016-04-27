using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models.ViewModels
{
    public class UserMessageViewModel
    {
        public UserAccountViewModel User { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime SentDate { get; set; }
    }
}
