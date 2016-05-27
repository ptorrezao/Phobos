using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phobos.Utils
{
    public static class ViewModelUtils
    {
        public static IEnumerable<SelectListItem> AsSelectListItem(this List<MessageMailBoxFolderViewModel> folders, object selectedValue)
        {
            int selectedValueId = -1;
            var isSelectedIdValid = int.TryParse(selectedValue.ToString(), out selectedValueId);

            var newList = new List<SelectListItem>();

            foreach (MessageMailBoxFolderViewModel item in folders)
            {
                newList.Add(new SelectListItem()
                {
                    Selected = isSelectedIdValid && item.FolderId == selectedValueId,
                    Text = item.Name,
                    Value = item.FolderId.ToString()
                });
            }

            return newList;
        }
    }
}