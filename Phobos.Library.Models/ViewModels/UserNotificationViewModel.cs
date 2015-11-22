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
        public object Id { get; private set; }

        public static List<UserNotificationViewModel> AsListOfUserNotificationViewModel(List<UserNotification> items)
        {
            return items.Select(x => UserNotificationViewModel.AsUserNotificationViewModel(x)).ToList();
        }

        private static UserNotificationViewModel AsUserNotificationViewModel(UserNotification item)
        {
            return new UserNotificationViewModel()
            {
                Title = item.Title,
                FontAwesome = " fa-users",
                Id=item.Id,
            };
        }
    }
}
