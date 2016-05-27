using Phobos.Library.Models;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        public static string TruncateLongString(this string str, int maxLength, string suffix)
        {
            return str.TruncateLongString(maxLength, suffix, false);
        }

        public static string TruncateLongString(this string str, int maxLength, string suffix, bool removeHtmlImg)
        {
            if (removeHtmlImg)
            {
                str = Regex.Replace(str, @"<img\s[^>]*>(?:\s*?</img>)?", "", RegexOptions.IgnoreCase);
            }
           
            var strLength = str != null ? str.Length : 0;

            return string.Format("{0}{1}", (str ?? "").Substring(0, Math.Min(strLength, maxLength)), (maxLength <= strLength) ? suffix : "");
        }

        public static string GetFullName(this UserAccount userAccount)
        {
            if (userAccount == null)
                return "";

            return string.Format("{0}{1}{2}", userAccount.FirstName, (!string.IsNullOrEmpty(userAccount.FirstName) && !string.IsNullOrEmpty(userAccount.LastName)) ? " " : "", userAccount.LastLoginDate);
        }
    }
}
