using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phobos.Library.Models;
using Phobos.Library.Utils;

namespace Phobos.UnitTest
{
    [TestClass]
    public class Library_Utils
    {
        [TestMethod]
        [TestCategory("Utils.Extensions")]
        public void GetFullName()
        {
            string fullName = Extensions.GetFullName(new UserAccount() { FirstName = "Pedro", LastName = "Torrezão" });

            Assert.IsTrue(fullName == "Pedro Torrezão");

             fullName = Extensions.GetFullName(new UserAccount() { FirstName = "Pedro"});

            Assert.IsTrue(fullName == "Pedro");
        }

        [TestMethod]
        [TestCategory("Utils.Extensions")]
        public void GetFullName_NullAccount()
        {
            string fullName = Extensions.GetFullName(null);

            Assert.IsTrue(fullName == "");
        }

        [TestMethod]
        [TestCategory("Utils.Extensions")]
        public void SetUser()
        {
            UserAccount user = new UserAccount() { FirstName = "Pedro", LastName = "Torrezão" };

            UserNotification userNotification = UserNotification.LastLogin(user);

            userNotification.SetUser(user);

            Assert.IsTrue(userNotification.User == user);
        }

        [TestMethod]
        [TestCategory("Utils.Extensions")]
        public void SetUser_Nullable()
        {
            UserAccount user = null;

            UserNotification userNotification = new UserNotification();

            userNotification.SetUser(user);

            Assert.IsTrue(userNotification != null);

            userNotification = null;

            userNotification.SetUser(user);

        }

        [TestMethod]
        [TestCategory("Utils.Extensions")]
        public void TruncateLongString()
        {
            int qtd = 30;
            string suffix = "...";
            var truncatedString = Extensions.TruncateLongString("Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit", qtd, suffix);

            Assert.IsTrue(truncatedString.Length == qtd + suffix.Length);
            Assert.IsTrue(truncatedString.LastIndexOf(suffix) == qtd);
            Assert.IsTrue(truncatedString.Substring(truncatedString.LastIndexOf(suffix), suffix.Length) == suffix);
        }

        [TestMethod]
        [TestCategory("Utils.Extensions")]
        public void TruncateLongString_WithHtml()
        {
            int qtd = 30;
            string suffix = "...";
            var truncatedString = Extensions.TruncateLongString("Neque <strong>porro</strong> quisquam est qui <strong>dolorem</strong> ipsum quia dolor sit amet, consectetur, adipisci velit", qtd, suffix, true);

            Assert.IsTrue(truncatedString.Length == qtd + suffix.Length);
            Assert.IsTrue(truncatedString.LastIndexOf(suffix) == qtd);
            Assert.IsTrue(truncatedString.Substring(truncatedString.LastIndexOf(suffix), suffix.Length) == suffix);
        }

        [TestMethod]
        [TestCategory("Utils.Extensions")]
        public void TruncateLongString_WithHtml_short()
        {
            int qtd = 30;
            string suffix = "...";
            var truncatedString = Extensions.TruncateLongString("Neque <strong>porro</strong>", qtd, suffix, true);

            Assert.IsTrue(truncatedString.Length <= qtd + suffix.Length);

            truncatedString = Extensions.TruncateLongString(null, qtd, suffix, true);

            Assert.IsTrue(truncatedString.Length <= qtd + suffix.Length);
        }

    }
}
