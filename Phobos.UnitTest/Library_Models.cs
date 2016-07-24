using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phobos.Library.Models;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.UnitTest
{
    [TestClass]
    public class Library_Models
    {
        [TestMethod]
        [TestCategory("UserMessage")]
        public void UserMessage()
        {
            var userMsg = new UserMessage();

            Assert.AreEqual(userMsg.Message, "");
        }


        [TestMethod]
        [TestCategory("ActionAuthorization")]
        public void ActionAuthorization()
        {
            var userMsg = new ActionAuthorization();

            Assert.AreNotEqual(userMsg.Roles, null);
            Assert.AreNotEqual(userMsg.UserAccounts, null);
        }

        [TestMethod]
        [TestCategory("MenuEntriesViewModel")]
        public void MenuEntriesViewModel()
        {
            var userMsg = new MenuEntriesViewModel();

            Assert.AreNotEqual(userMsg.Childs, null);
        }


        [TestMethod]
        [TestCategory("MenuEntriesViewModel")]
        public void MessageMailBoxViewModel()
        {
            var model = new MessageMailBoxViewModel();

            Assert.AreNotEqual(model.Folders, null);
            Assert.AreNotEqual(model.Tags, null);
        }

        [TestMethod]
        [TestCategory("UserAccountViewModel")]
        public void UserAccountViewModel()
        {
            var model = new UserAccountViewModel();

            Assert.AreNotEqual(model.FavoriteLinks, null);

            model.FirstName="Pedro";
            model.LastName="Torrezão";

            Assert.AreEqual(model.FullName, "Pedro Torrezão");

            model.LastName ="";
            Assert.AreEqual(model.FullName, "Pedro");

            model.FirstName = "";
            model.LastName = "Torrezão";
            Assert.AreEqual(model.FullName, "Torrezão");
        }
    }
}
