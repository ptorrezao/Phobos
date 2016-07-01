using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Phobos.Library.Models.ViewModels
{
    public class UserRoleUpdateViewModel : UserRoleViewModel
    {
        [Display(Name = "Current Name")]
        public string OldName { get; set; }

        [Display(Name = "Users")]
        public string[] SelectedUsersInRole { get; set; }
        public List<UserAccountRoleItemViewModel> Users { get; set; }
        public List<UserAccountRoleItemViewModel> AllUsers { get; set; }
    }
}
