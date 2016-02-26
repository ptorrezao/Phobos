using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace Phobos.Library.Utils
{
    public static class Extensions
    {
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