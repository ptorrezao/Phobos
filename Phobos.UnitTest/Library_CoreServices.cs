using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phobos.Library.CoreServices;
using Phobos.Library.Interfaces;
using Ninject;
using Phobos.Library.Models;
using System.Collections.Generic;
using Phobos.Library.Interfaces.Repos;

namespace Phobos.UnitTest
{
    [TestClass]
    public class Library_CoreServices
    {
        string name = "John";
        string goodPassword = "GoodPassword123";
        string badPassword = "BadPassword";


        IUserManagementService usrMngSvc = new UserManagementCoreService();

        [TestInitialize]
        public void Initialize()
        {
            var kernel = MvcApplication.GetKernel();
            usrMngSvc = kernel.Get<IUserManagementService>();

            var coreRepo = kernel.Get<ICoreRepo>();
            coreRepo.AddConfiguration("PasswordSalt", "Phobos");
        }
        
        #region RegisterUser

        [TestMethod]
        public void RegisterUser_Sucessfully()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, goodPassword, goodPassword, out error);

            Assert.IsTrue(sucess, error);
        }

        [TestMethod]
        public void RegisterUser_DiferentPasswords()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, goodPassword, badPassword, out error);

            Assert.IsFalse(sucess, error);
        }

        [TestMethod]
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
        public void RegisterUser_PasswordWithoutMinimumSecurity()
        {
            var nonexisingUser = Guid.NewGuid().ToString().Substring(0, 10) + "@email.com";
            var error = "";
            var sucess = usrMngSvc.RegisterUser(name, nonexisingUser, "A", "A", out error);

            Assert.IsFalse(sucess, error);
        }
        #endregion

        #region NotImplemented
        public void CheckIfActionIsAllowed()
        {
            throw new NotImplementedException();
            // usrMngSvc.CheckIfActionIsAllowed(controller, auhtorizedAction, username);
        }
        public bool CheckIfRegisterIsAllowed(string name, string userName, string password, string confirmPassword, out string error)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfUserIsValid(string userName, string password, out string error)
        {
            throw new NotImplementedException();
        }

        public bool CheckSecurityMesurements(string userName, string password, string confirmPassword, out string error)
        {
            throw new NotImplementedException();
        }

        public List<UserMessage> GetLastMessages(string userName, int qtd)
        {
            throw new NotImplementedException();
        }

        public List<UserNotification> GetLastNotifications(string userName, int qtd)
        {
            throw new NotImplementedException();
        }

        public List<UserTask> GetLastTasks(string userName, int qtd)
        {
            throw new NotImplementedException();
        }

        public UserAccount GetUser(string userName)
        {
            throw new NotImplementedException();
        }

        public bool RecoverProfile(string userName, out string error)
        {
            throw new NotImplementedException();
        }



        public bool UpdateAccount(UserAccount userAccount)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
