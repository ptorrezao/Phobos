using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Phobos.Library.Models.ViewModels;
using Phobos.Utils;
using System.Web.Mvc;
using System.Collections;
using System.Linq;
using Phobos.ActionFilter;
namespace Phobos.UnitTest
{
    [TestClass]
    public class Phobos_UnitTest
    {
        [TestMethod]
        public void ViewModelUtils_UnitTest()
        {
            List<MessageMailBoxFolderItemViewModel> listOfViewModels = new List<MessageMailBoxFolderItemViewModel>();

            listOfViewModels.Add(new MessageMailBoxFolderItemViewModel() { FolderId = 1, Title = "Inbox" });
            listOfViewModels.Add(new MessageMailBoxFolderItemViewModel() { FolderId = 2, Title = "Draft" });
            listOfViewModels.Add(new MessageMailBoxFolderItemViewModel() { FolderId = 3, Title = "Trash" });

            //// This shoud return a List with the Folder 1 selected
            List<SelectListItem> selectList = listOfViewModels.AsSelectListItem(1).ToList();

            Assert.IsNotNull(selectList);
            Assert.IsTrue(selectList.Count == listOfViewModels.Count());
            Assert.IsTrue(selectList.Any(x => x.Selected));

            //// This shoud return a List with none selected
            selectList = listOfViewModels.AsSelectListItem("A").ToList();

            Assert.IsNotNull(selectList);
            Assert.IsTrue(selectList.Count == listOfViewModels.Count());
            Assert.IsFalse(selectList.Any(x => x.Selected));
        }
    }
}
