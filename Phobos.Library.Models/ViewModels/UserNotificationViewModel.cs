using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models.ViewModels
{
    public class UserNotificationViewModel
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string FontAwesome { get; set; }
        public TextColor Color { get; set; }
        public object Id { get; private set; }
        public bool Read { get; set; }
    }
}
