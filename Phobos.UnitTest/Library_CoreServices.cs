using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phobos.Library.CoreServices;
using Phobos.Library.Interfaces;
using Ninject;
using Phobos.Library.Models;
using System.Collections.Generic;
using System.Linq;
using Phobos.Library.Interfaces.Repos;
using Phobos.UnitTest.Repos;

namespace Phobos.UnitTest
{
    [TestClass]
    public class Library_CoreServices
    {
        string name = "John";
        string goodPassword = "GoodPassword123";
        string badPassword = "BadPassword";

        IUserManagementService usrMngSvc;

        [TestInitialize]
        public void Initialize()
        {
            var kernel = MvcApplication.GetKernel();

            kernel.Unbind<ICoreRepo>();
            kernel.Bind<ICoreRepo>().To<TestCoreRepo>();

            kernel.Unbind<IUserManagementRepo>();
            kernel.Bind<IUserManagementRepo>().To<TestUserManagementRepo>();

            usrMngSvc = kernel.Get<IUserManagementService>();

            var coreRepo = kernel.Get<ICoreRepo>();
            coreRepo.AddConfiguration("PasswordSalt", "Phobos");
        }

        #region RegisterUser
        [TestMethod]
        [TestCategory("UserAccount")]
        public void RegisterUser_Sucessfully()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);
        }

        [TestMethod]
        [TestCategory("UserAccount")]
        public void RegisterUser_DiferentPasswords()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, goodPassword, badPassword, out error);

            Assert.IsFalse(sucess, error);
        }

        [TestMethod]
        [TestCategory("UserAccount")]
        public void RegisterUser_DupplicatedUser()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);
            sucess = usrMngSvc.RegisterUser(name, nonexisingUser, goodPassword, goodPassword, out error);
            Assert.IsFalse(sucess, error);
        }

        [TestMethod]
        [TestCategory("UserAccount")]
        public void RegisterUser_PasswordWithoutMinimumSecurity()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, "A", "A", out error);

            Assert.IsFalse(sucess, error);
        }
        #endregion

        #region UpdateAccount
        [TestMethod]
        [TestCategory("UserAccount")]
        public void UpdateAccount_ChangeName()
        {
            //// Create User
            var username = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var oldName = "A";
            var newName = "B";
            var sucess = usrMngSvc.RegisterUser(oldName, username, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);
            if (sucess)
            {
                var user = usrMngSvc.GetUser(username);
                user.FirstName = newName;

                sucess = usrMngSvc.UpdateAccount(user);
                Assert.IsTrue(sucess, "Couldn't update user.");

                user = usrMngSvc.GetUser(username);
                Assert.IsTrue(user.FirstName == newName, "The names doesn't match.");
            }
        }
        #endregion

        #region CheckIfUserIsValid
        [TestMethod]
        [TestCategory("UserAccount")]
        public void CheckIfUserIsValid()
        {
            //// Create User
            var username = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(Guid.NewGuid().ToString().Substring(0, 10), username, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);
            if (sucess)
            {
                sucess = usrMngSvc.CheckIfUserIsValid(username, goodPassword, out error);
                Assert.IsTrue(sucess, error);
            }
        }

        [TestMethod]
        [TestCategory("UserAccount")]
        public void CheckIfUserIsValid_WithBadPassword()
        {
            //// Create User
            var username = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(Guid.NewGuid().ToString().Substring(0, 10), username, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);
            if (sucess)
            {
                sucess = usrMngSvc.CheckIfUserIsValid(username, badPassword, out error);
                Assert.IsTrue(!sucess, error);
            }
        }

        [TestMethod]
        [TestCategory("UserAccount")]
        public void CheckIfUserIsValid_WithLockedUser()
        {
            //// Create User
            var username = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(Guid.NewGuid().ToString().Substring(0, 10), username, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);
            if (sucess)
            {
                for (int i = 0; i <= 4; i++)
                {
                    sucess = usrMngSvc.CheckIfUserIsValid(username, badPassword, out error);
                }
                sucess = error.Contains("The user account is locked");

                Assert.IsTrue(sucess, error);
            }
        }

        [TestMethod]
        [TestCategory("UserAccount")]
        public void CheckIfUserIsValid_WithNonExistingUser()
        {
            //// Create User
            var error = "";
            var username = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var sucess = usrMngSvc.CheckIfUserIsValid(username, goodPassword, out error);

            Assert.IsTrue(!sucess, error);
        }

        [TestMethod]
        [TestCategory("UserAccount")]
        public void CheckIfUserIsValid_WithOldLockedUser()
        {
            //// Create User
            var username = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(Guid.NewGuid().ToString().Substring(0, 10), username, goodPassword, goodPassword, out error);

            var user = usrMngSvc.GetUser(username);
            user.LockedDate = DateTime.MinValue;
            user.IsLocked = true;

            sucess = usrMngSvc.UpdateAccount(user);

            user = usrMngSvc.GetUser(username);
            sucess = user.IsLocked;
            Assert.IsTrue(sucess, "The user isn't locked");

            sucess = usrMngSvc.CheckIfUserIsValid(username, goodPassword, out error);
            Assert.IsTrue(sucess, error);
        }
        #endregion

        #region RecoverProfile
        [TestMethod]
        [TestCategory("UserAccount")]
        public void RecoverProfile()
        {
            var username = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(Guid.NewGuid().ToString().Substring(0, 10), username, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);
            if (sucess)
            {
                var user = usrMngSvc.GetUser(username);
                sucess = usrMngSvc.RecoverProfile(username, out error);

                sucess = usrMngSvc.GetUser(username).Password != goodPassword;
                Assert.IsTrue(sucess, "");
            }
        }

        [TestMethod]
        [TestCategory("UserAccount")]
        public void RecoverProfile_NonExistingUser()
        {
            var username = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RecoverProfile(username, out error);
            Assert.IsTrue(!sucess, error);
        }
        #endregion

        #region Messages
        [TestMethod]
        [TestCategory("Messages")]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetLastMessages()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);

            var messages = usrMngSvc.GetLastMessages(nonexisingUser, 10);

            if (messages.Any(x => x.Receiver.Username != nonexisingUser))
            {
                Assert.Fail("There are messages from another userr.");
            }
        } 
        #endregion

        #region Notifications
        [TestMethod]
        [TestCategory("Notifications")]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetLastNotifications()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);

            var messages = usrMngSvc.GetLastNotifications(nonexisingUser, 10);

            if (messages.Any(x => x.User.Username != nonexisingUser))
            {
                Assert.Fail("There are Notifications from another userr.");
            }
        } 
        #endregion

        #region Tasks
        [TestMethod]
        [TestCategory("Tasks")]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetLastTasks()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);

            var messages = usrMngSvc.GetLastTasks(nonexisingUser, 10);

            if (messages.Any(x => x.User.Username != nonexisingUser))
            {
                Assert.Fail("There are Tasks from another userr.");
            }
        } 
        #endregion
    }
}
