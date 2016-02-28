using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
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

        [DisplayName("Current Status")]
        public UserStatusEnum CurrentStatus { get; set; }

        [DisplayName("Full Name")]
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        [DisplayName("Username")]
        public string Username { get; set; }

        [DisplayName("Position")]
        public string Position { get; set; }

        [DisplayName("Use Gravatar")]
        public bool UseGravatar { get; set; }

        [DisplayName("Member Since")]
        public DateTime MemberSince { get; set; }

        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }

        [DisplayName("Image Alt")]
        public string ImageAlt { get; set; }

        [DisplayName("Favorite Links")]
        public List<MenuEntriesViewModel> FavoriteLinks { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Date of Birth")]
        public DateTime BirthDate { get; set; }
    
    }
}
