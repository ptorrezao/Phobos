using Phobos.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phobos.Library.Models;
using Phobos.Library.Interfaces.Repos;
using Ninject;
using Phobos.Library.Interfaces.Services;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Phobos.Library.Utils;

namespace Phobos.Library.CoreServices
{
    public class UserManagementCoreService : IUserManagementService
    {
        public UserManagementCoreService()
        {

        }

        #region Injects
        [Inject]
        public IUserManagementRepo Repository { get; set; }

        [Inject]
        public IAuditTrailService AuditTrail { get; set; }

        [Inject]
        public IMessageService NotificationService { get; set; }

        [Inject]
        public ICoreRepo CoreRepository { get; set; }
        #endregion

        #region IUserManagementService

        bool CheckIfRegisterIsAllowed(string name, string userName, string password, string confirmPassword, out string msg)
        {
            msg = "";

            var userAlreadyExists = this.Repository.CheckIfUserExist(userName);

            if (userAlreadyExists)
            {
                msg = string.Format("Was requested a creation of a user for an existing username ({0}).", userName);

                AuditTrail.LogInfoMessage(msg, userName, DateTime.Now);

                return false;
            }

            return true;
        }

        public bool CheckIfUserIsValid(string userName, string password, out string msg)
        {
            Configuration salt = CoreRepository.GetConfiguration("PasswordSalt");

            var selectedUser = this.Repository.GetUser(userName);
            msg = "";

            if (selectedUser != null)
            {
                //// Existing User
                if (!selectedUser.IsLocked)
                {
                    //// Check if pwd is correct
                    if (password.GetAsHash(salt.Value) == selectedUser.Password)
                    {
                        //// Update the user info to have last login.
                        if (this.Repository.UpdateLastLoginDate(userName, DateTime.Now))
                        {
                            return true;
                        }
                        else
                        {
                            msg = string.Format("Was not possible update the last login date for user: ({0}).", userName);

                            AuditTrail.LogWarningMessage(msg, userName, DateTime.Now);

                            return false;
                        }
                    }
                    else
                    {
                        //// Add Login attempt.
                        this.Repository.AddFailedLoginAttempt(userName);

                        //// Lock user
                        if (selectedUser.FailedAttempts > 3)
                        {
                            this.Repository.LockUserAccount(userName);
                        }

                        //// Password is diferent than expected
                        msg = string.Format("Was requested a login user with the username({0}) but the passwords didn't match.", userName);

                        AuditTrail.LogWarningMessage(msg, userName, DateTime.Now);

                        return false;
                    }
                }
                else
                {
                    if (selectedUser.LockedDate < DateTime.Now.AddMinutes(-30))
                    {
                        //// Unlock user.
                        this.Repository.UnlockUserAccount(userName);

                        //// Try again login
                        return this.CheckIfUserIsValid(userName, password, out msg);
                    }
                    else
                    {
                        //// The user account is locked
                        msg = string.Format("The user account is locked ({0}).", userName);

                        AuditTrail.LogWarningMessage(msg, userName, DateTime.Now);

                        return false;
                    }
                }
            }
            else
            {
                //// Non Existing User

                msg = string.Format("Was requested a non existing user with the username({0}).", userName);

                AuditTrail.LogWarningMessage(msg, userName, DateTime.Now);

                return false;
            }
        }

        bool CheckSecurityMesurements(string userName, string password, string confirmPassword, out string msg)
        {
            msg = "";
            bool haveMinimumLength, haveMinimunQtdOfUpper, haveMinimunQtdOfLower, haveMinimunQtdOfDigits;
            bool isValid = this.CheckPassword(password, out haveMinimumLength, out haveMinimunQtdOfUpper, out haveMinimunQtdOfLower, out haveMinimunQtdOfDigits, out msg);

            if (!isValid)
            {
                AuditTrail.LogInfoMessage(msg, userName, DateTime.Now);
            }

            return isValid;
        }

        public List<UserMessage> GetLastMessages(string userName, int qtd)
        {
            return this.Repository.GetLastMessagesForUser(userName, qtd);
        }

        public List<UserNotification> GetLastNotifications(string userName, int qtd)
        {
            return this.Repository.GetLastNotificationsForUser(userName, qtd);
        }

        public List<UserTask> GetLastTasks(string userName, int qtd)
        {
            return this.Repository.GetLastTasksForUser(userName, qtd);
        }

        public UserAccount GetUser(string userName)
        {
            var selectedUser = this.Repository.GetUser(userName);
            return selectedUser;
        }

        public List<UserAccount> GetAllUsers()
        {
            var listOfUsers = this.Repository.GetAllUsers();
            return listOfUsers;
        }

        public bool RecoverProfile(string userName, out string msg)
        {
            var selectedUser = this.Repository.GetUser(userName);
            msg = "";

            if (selectedUser != null)
            {
                const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                StringBuilder res = new StringBuilder();
                Random rnd = new Random();
                selectedUser.Password = "";

                while (!CheckPassword(selectedUser.Password))
                {
                    var length = 8;
                    while (0 < length--)
                    {
                        res.Append(valid[rnd.Next(valid.Length)]);
                    }
                    selectedUser.Password = res.ToString();
                }

                this.Repository.UpdateAccount(selectedUser);

                return NotificationService.SendMessage(selectedUser.Username, string.Format("Your new password is {0}", selectedUser.Password));
            }
            else
            {
                //// Non Existing User

                msg = string.Format("Was requested a non existing user with the username({0}).", userName);

                AuditTrail.LogWarningMessage(msg, userName, DateTime.Now);

                return false;
            }
        }

        public bool RegisterUser(string name, string userName, string password, string confirmPassword, out string error)
        {
            Configuration salt = CoreRepository.GetConfiguration("PasswordSalt");

            UserAccount selectedUser = null;
            error = "";

            userName = userName ?? "";
            password = password ?? "";
            confirmPassword = confirmPassword ?? "";

            if (password == confirmPassword)
            {
                if (this.CheckIfRegisterIsAllowed(name, userName, password, confirmPassword, out error))
                {
                    if (this.CheckSecurityMesurements(userName, password, confirmPassword, out error))
                    {
                        selectedUser = this.Repository.CreateUser(name, userName, password.GetAsHash(salt.Value));
                    }
                }
                return selectedUser != null;
            }
            else
            {
                error = string.Format("The confirm confirm password and the password does not match.", userName);

                AuditTrail.LogInfoMessage(error, userName, DateTime.Now);

                return false;
            }
        }

        public bool UpdateAccount(UserAccount userAccount)
        {
            return this.Repository.UpdateAccount(userAccount);
        }
        #endregion

        #region Password Verifications
        private bool CheckPassword(string password)
        {
            var msg = "";
            bool haveMinimumLength, haveMinimunQtdOfUpper, haveMinimunQtdOfLower, haveMinimunQtdOfDigits;
            return this.CheckPassword(password, out haveMinimumLength, out haveMinimunQtdOfUpper, out haveMinimunQtdOfLower, out haveMinimunQtdOfDigits, out msg);
        }
        private bool CheckPassword(string password, out bool haveMinimumLength, out bool haveMinimunQtdOfUpper, out bool haveMinimunQtdOfLower, out bool haveMinimunQtdOfDigits, out string msg)
        {
            Regex uppercaseCharacterMatcher = new Regex("[A-Z]");
            Regex lowerCharacterMatcher = new Regex("[a-z]");
            Regex lowerCharacterDigits = new Regex("[0-9]");
            int minimumLength = 3;
            int minimunQtdOfUpper = 1;
            int minimunQtdOfLower = 1;
            int minimunQtdOfDigits = 1;

            haveMinimumLength = password.Length > minimumLength;
            haveMinimunQtdOfUpper = uppercaseCharacterMatcher.Matches(password).Count >= minimunQtdOfUpper;
            haveMinimunQtdOfLower = lowerCharacterMatcher.Matches(password).Count >= minimunQtdOfLower;
            haveMinimunQtdOfDigits = lowerCharacterDigits.Matches(password).Count >= minimunQtdOfDigits;
            msg = "The password does not meet the security mesuraments:";
            if (!haveMinimumLength)
            {
                msg += string.Format(Environment.NewLine + " Minimum Lenth: {0} ", minimumLength);
            }

            if (!haveMinimunQtdOfUpper)
            {
                msg += string.Format(Environment.NewLine + " Minimum Qtd of Uppercases: {0} ", minimunQtdOfUpper);
            }

            if (!haveMinimunQtdOfLower)
            {
                msg += string.Format(Environment.NewLine + " Minimum Qtd of Lowercases: {0} ", minimunQtdOfLower);
            }

            if (!haveMinimunQtdOfDigits)
            {
                msg += string.Format(Environment.NewLine + " Minimum Qtd of Digits: {0} ", minimunQtdOfDigits);
            }

            if (haveMinimumLength && haveMinimunQtdOfDigits && haveMinimunQtdOfLower && haveMinimunQtdOfUpper)
            {
                msg = "";
            }
            return haveMinimumLength && haveMinimunQtdOfDigits && haveMinimunQtdOfLower && haveMinimunQtdOfUpper;
        }
        #endregion

        public void UpdateAccountForLogin(string username)
        {
            var user = this.GetUser(username);

            user.CurrentStatus = Models.Enums.UserStatusEnum.Online;

            user.LastLoginDate = DateTime.Now;

            this.UpdateAccount(user);
        }

        public void UpdateAccountForLogout(string username)
        {
            var user = this.GetUser(username);

            user.CurrentStatus = Models.Enums.UserStatusEnum.Offline;

            this.UpdateAccount(user);
        }

        public bool CreateRole(string username, out string error)
        {
            error = "";

            return this.Repository.CreateRole(username) != null;
        }

        public List<UserRole> GetAllRoles()
        {
            var listOfroles = this.Repository.GetAllRoles();
            return listOfroles;
        }

        public UserRole GetRole(string name)
        {
            var role = this.Repository.GetRole(name);
            return role;
        }


        public bool UpdateRole(string oldName, string newName, out string error)
        {
            var role = this.GetRole(oldName);

            error = "";

            role.Name = newName;

            return this.Repository.UpdateRole(role, oldName);
        }


        public bool DeleteRole(string name, out string error)
        {
            error = "";
            return this.Repository.DeleteRole(name);
        }
    }
}
