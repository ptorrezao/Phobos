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
        public string FirstName { get;  set; }

        [DisplayName("Last Name")]
        public string LastName { get;  set; }

        public static UserAccountViewModel AsUserAccountViewModel(UserAccount model)
        {
            var viewModel= new UserAccountViewModel()
            {
                CurrentStatus = model.CurrentStatus,
                FavoriteLinks = new List<MenuEntriesViewModel>(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageUrl = "/Content/themes/AdminLTE/img/user2-160x160.jpg",
                ImageAlt = model.FirstName + " " + model.LastName,
                MemberSince = model.MemberSinceDate,
                Username = model.Username,
                Position = model.Position,
                UseGravatar=true
            };

            if (viewModel.UseGravatar)
            {
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(viewModel.Username);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2").ToLower());
                }

                viewModel.ImageUrl = @"http://www.gravatar.com/avatar/" + sb.ToString() +".jpg";
            }

            return viewModel;
        }

        
    }
}
