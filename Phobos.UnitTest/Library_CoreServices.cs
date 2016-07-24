using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phobos.Library.CoreServices;
using Phobos.Library.Interfaces;
using Ninject;
using Phobos.Library.Models;
using System.Collections.Generic;
using System.Linq;
using Phobos.Library.Interfaces.Repos;
using Phobos.Library.Interfaces.Services;
using Rhino.Mocks;
using Phobos.UnitTest.MockedRepositories;
using System.Configuration;
using Phobos.Library.Utils;
using Phobos.Library.Models.Enums;

namespace Phobos.UnitTest
{
    [TestClass]
    public class Library_CoreServices
    {
        string name = "John";
        string goodPassword = "GoodPassword123";
        string badPassword = "BadPassword";

        IUserManagementService usrMngSvc;
        IMessageService msgSvc;
        INotificationService notificatioSvc;
        INavigationService navigationService;

        [TestInitialize]
        public void Initialize()
        {
            var kernel = MvcApplication.GetKernel();
            bool isLocal = bool.Parse(ConfigurationManager.AppSettings["UseDatabaseToTest"]); ;

            if (!isLocal)
            {
                kernel.Rebind<IUserManagementRepo>().To<MockedUserManagementRepo>();
                kernel.Rebind<INotificationRepo>().To<MockedUserManagementRepo>();
                kernel.Rebind<IMessageRepo>().To<MockedUserManagementRepo>();
                kernel.Rebind<ICoreRepo>().To<MockedUserManagementRepo>();
            }

            usrMngSvc = kernel.Get<IUserManagementService>();
            msgSvc = kernel.Get<IMessageService>();
            notificatioSvc = kernel.Get<INotificationService>();
            navigationService = kernel.Get<INavigationService>();

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
            Assert.IsFalse(!error.Contains(" existing username"), error);
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

        [TestMethod]
        [TestCategory("UserAccount")]
        public void UpdateAccount_NonExistingUser()
        {
            //// Create User
            var username = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var newName = "B";
            var user = new UserAccount();
            user.Username = username;
            user.FirstName = newName;

            var sucess = usrMngSvc.UpdateAccount(user);
            Assert.IsFalse(sucess, "Couldn't update user.");
        }
        #endregion

        #region UnlockUser
        [TestMethod]
        [TestCategory("UserAccount")]
        public void UnlockUser()
        {
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

            sucess = usrMngSvc.UnlockUserAccount(username);

            Assert.IsFalse(usrMngSvc.GetUser(username).IsLocked);
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
            user.LockedDate = DateTime.Now;
            user.IsLocked = true;
            sucess = usrMngSvc.UpdateAccount(user);

            user = usrMngSvc.GetUser(username);
            sucess = user.IsLocked;
            Assert.IsTrue(sucess, "The user isn't locked");

            sucess = usrMngSvc.CheckIfUserIsValid(username, goodPassword, out error);

            Assert.IsTrue(error.Contains("The user account is locked"), "The error message don't identify the account as locked.");
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

        [TestMethod]
        [TestCategory("Roles")]
        public void CreateRole()
        {
            string error = "";
            string roleName = Guid.NewGuid().ToString();
            bool sucess = usrMngSvc.CreateRole(roleName, out error);

            Assert.IsTrue(usrMngSvc.GetRole(roleName) != null);
        }

        [TestMethod]
        [TestCategory("Roles")]
        public void UpdateRole()
        {
            string error = "";
            string roleName = Guid.NewGuid().ToString();
            string newroleName = Guid.NewGuid().ToString();
            bool sucess = usrMngSvc.CreateRole(roleName, out error);

            var role = usrMngSvc.GetRole(roleName);

            Assert.IsTrue(role != null);

            usrMngSvc.UpdateRole(roleName, newroleName, role.UserAccounts.Select(x => x.Username).ToList(), out error);

            Assert.IsTrue(usrMngSvc.GetRole(newroleName) != null);
        }

        [TestMethod]
        [TestCategory("Roles")]
        public void DeleteRole()
        {
            string error = "";
            string roleName = Guid.NewGuid().ToString();
            bool sucess = usrMngSvc.CreateRole(roleName, out error);

            Assert.IsTrue(usrMngSvc.GetRole(roleName) != null);
            Assert.IsTrue(usrMngSvc.DeleteRole(roleName, out error));
            Assert.IsTrue(usrMngSvc.GetRole(roleName) == null);
        }

        [TestMethod]
        [TestCategory("ActionAuthorization")]
        public void CheckIfActionIsAllowed()
        {
            var username = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            bool sucess = navigationService.CheckIfActionIsAllowed("Home", "Index", username);
        }


        [TestMethod]
        [TestCategory("UserNotification")]
        public void SendGetDeleteNotificiations()
        {
            var username = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(Guid.NewGuid().ToString().Substring(0, 10), username, goodPassword, goodPassword, out error);
            var user = usrMngSvc.GetUser(username);

            Assert.IsTrue(sucess, error);
            if (sucess)
            {
                var notifications = notificatioSvc.GetNotifications(username);

                foreach (var item in notifications)
                {
                    notificatioSvc.DeleteNotification(username, item.Id);
                }
                
                sucess = notificatioSvc.SendNotification(UserNotification.Welcome.SetUser(user));

                Assert.IsTrue(sucess);

                notifications = notificatioSvc.GetNotifications(username);

                Assert.IsTrue(notificatioSvc.GetLastNotifications(username, 5, true).Any());

                var id = notifications.First().Id;

                Assert.IsTrue(notifications.Count > 0);

                notificatioSvc.DeleteNotification(username, id);

                notifications = notificatioSvc.GetNotifications(username);

                Assert.IsFalse(notifications.Any(x => x.Id == id));

                Assert.IsTrue(notificatioSvc.SendNotification(UserNotification.LastLogin(user)));

                notificatioSvc.ClearNotifications(NotificationType.Login, username);

                notifications = notificatioSvc.GetNotifications(username);

                Assert.IsTrue(notifications.Any(x => x.Type == NotificationType.Login && x.Read == true));

                Assert.IsTrue(notificatioSvc.SendNotification(UserNotification.LastLogin(user)));

                notificatioSvc.MarkNotificationAsRead(notificatioSvc.GetNotifications(username).Where(x => x.Read == false).First().Id);
                notifications = notificatioSvc.GetNotifications(username);
                Assert.IsFalse(notifications.Any(x => x.Read == false));
            }

        }

        #region Messages
        [TestMethod]
        [TestCategory("Messages")]
        public void GetLastMessages()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);

            var messages = msgSvc.GetLastMessages(nonexisingUser, 10);

            Assert.IsNotNull(messages);
        }
        #endregion

        #region Tasks
        [TestMethod]
        [TestCategory("Tasks")]
        public void GetLastTasks()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);

            var messages = usrMngSvc.GetLastTasks(nonexisingUser, 10);

            Assert.IsNotNull(messages);
        }
        #endregion
    }
}
