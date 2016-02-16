using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Phobos.Library.Utils
{
    public static class Extensions
    {
        public static string GetAsHash(this string value, string saltString)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(value + saltString);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2").ToLower());
            }

            return sb.ToString();
        }

        public static IEnumerable<SelectListItem> AsSelectListItem(this List<MessageMailBoxFolderItemViewModel> folders, object selectedValue)
        {
            var newList = new List<SelectListItem>();

            foreach (MessageMailBoxFolderItemViewModel item in folders)
            {
                newList.Add(new SelectListItem()
                {
                    Selected = item.FolderId == (int)selectedValue,
                    Text = item.Title,
                    Value = item.FolderId.ToString()
                });
            }

            return newList;
        }
    }
}
