using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models.ViewModels
{
    public class UserTaskViewModel
    {
        public string Title { get; set; }
        public double Percentage { get; set; }

        public static List<UserTaskViewModel> AsListOfUserTaskViewModel(List<UserTask> items)
        {
            return items.Select(x => UserTaskViewModel.AsUserTaskViewModel(x)).ToList();
        }

        private static UserTaskViewModel AsUserTaskViewModel(UserTask item)
        {
            return new UserTaskViewModel()
            {
                Percentage = item.Percentage,
                Title = item.Title
            };
        }
    }
}
