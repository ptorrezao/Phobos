using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models.ViewModels
{
    public class UserAccountViewModel
    {
        public UserAccountViewModel()
        {
            FavoriteLinks = new List<MenuEntriesViewModel>();
        }
        public UserStatusEnum CurrentStatus { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public DateTime MemberSince { get; set; }
        public string ImageUrl { get; set; }
        public List<MenuEntriesViewModel> FavoriteLinks { get; set; }
        public static UserAccountViewModel AsUserAccountViewModel(UserAccount model)
        {
            return new UserAccountViewModel()
            {
                CurrentStatus = UserStatusEnum.Online,
                FavoriteLinks = new List<MenuEntriesViewModel>(),
                FullName = model.FirstName + " " + model.LastName,
                ImageUrl = "/Content/themes/AdminLTE/img/user2-160x160.jpg",
                MemberSince = model.MemberSinceDate,
                Name = model.Username,
                Position = model.Position
            };
        }
    }
}
