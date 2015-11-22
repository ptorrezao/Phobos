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
        public string Title { get; set; }

        public string Message { get; set; }
        public DateTime SentDate { get; set; }

        public static List<UserMessageViewModel> AsListOfUserMessageViewModel(List<UserMessage> items)
        {
            return items.Select(x => UserMessageViewModel.AsUserMessageViewModel(x)).ToList();
        }

        private static UserMessageViewModel AsUserMessageViewModel(UserMessage item)
        {
            return new UserMessageViewModel()
            {
                Message = item.Message,
                User = UserAccountViewModel.AsUserAccountViewModel(item.User),
                SentDate = item.SentDate,
                Title = item.Title
            };
        }
    }
}
