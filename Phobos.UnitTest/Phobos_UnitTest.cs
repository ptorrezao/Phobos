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
            List<MessageMailBoxFolderViewModel> listOfViewModels = new List<MessageMailBoxFolderViewModel>();

            listOfViewModels.Add(new MessageMailBoxFolderViewModel() { FolderId = 1, Name = "Inbox" });
            listOfViewModels.Add(new MessageMailBoxFolderViewModel() { FolderId = 2, Name = "Draft" });
            listOfViewModels.Add(new MessageMailBoxFolderViewModel() { FolderId = 3, Name = "Trash" });

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
