using Ninject;
using Phobos.Library.Interfaces.Repos;
using Phobos.Library.Interfaces.Services;
using Phobos.Library.Models;
using Phobos.Library.Models.Enums;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.UnitTest.MockedRepositories
{
    public class MockedUserManagementRepo : IUserManagementRepo, ICoreRepo, INotificationRepo, IMessageRepo
    {
        public static List<UserAccount> users = new List<UserAccount>();
        public static List<UserMessage> userMessages = new List<UserMessage>();
        public static List<ActionAuthorization> actionAuthorizations = new List<ActionAuthorization>();
        public static List<UserRole> roles = new List<UserRole>();
        public static List<UserTask> userTasks = new List<UserTask>();
        public static List<UserNotification> userNotifications = new List<UserNotification>();
        public static List<Configuration> configurations = new List<Configuration>();
        
        public MockedUserManagementRepo()
        {
            if (actionAuthorizations.Count == 0)
            {
                actionAuthorizations.Add(new ActionAuthorization() { Action = "Index", Controller = "Home" });
            }
        }

        #region IUserManagementRepo
        public bool CheckIfUserExist(string userName)
        {
            var selectedUser = users.FirstOrDefault(x => x.Username == userName);

            if (selectedUser != default(UserAccount))
            {
                selectedUser.FailedAttempts++;

                if (selectedUser.FailedAttempts > 3)
                {
                    selectedUser.LockedDate = DateTime.Now;
                    selectedUser.IsLocked = true;
                }

                return true;
            }

            return false;

        }

        public UserAccount GetUser(string userName)
        {
            var selectedUser = users.FirstOrDefault(x => x.Username == userName);

            return selectedUser;
        }

        public List<UserAccount> GetAllUsers()
        {
            var listOfUsers = users.ToList();

            return listOfUsers;
        }

        public bool AddFailedLoginAttempt(string userName)
        {
            var selectedUser = users.FirstOrDefault(x => x.Username == userName);

            if (selectedUser != default(UserAccount))
            {
                selectedUser.FailedAttempts++;

                if (selectedUser.FailedAttempts > 3)
                {
                    selectedUser.LockedDate = DateTime.Now;
                    selectedUser.IsLocked = true;
                }

                return true;
            }

            return false;
        }

        public bool UnlockUserAccount(string userName)
        {
            var selectedUser = users.FirstOrDefault(x => x.Username == userName);

            if (selectedUser != default(UserAccount))
            {
                selectedUser.LockedDate = null;
                selectedUser.FailedAttempts = 0;
                selectedUser.IsLocked = false;

                return true;
            }

            return false;
        }

        public bool LockUserAccount(string userName)
        {
            var selectedUser = users.FirstOrDefault(x => x.Username == userName);

            if (selectedUser != default(UserAccount))
            {
                selectedUser.LockedDate = DateTime.Now;
                selectedUser.IsLocked = true;

                return true;
            }

            return false;
        }

        public bool UpdateLastLoginDate(string userName, DateTime lastDate)
        {
            var selectedUser = users.FirstOrDefault(x => x.Username == userName);

            if (selectedUser != default(UserAccount))
            {
                selectedUser.LastLoginDate = lastDate;
                selectedUser.FailedAttempts = 0;
                selectedUser.IsLocked = false;
                selectedUser.CurrentStatus = UserStatusEnum.Online;
                return true;
            }

            return false;
        }

        public ActionAuthorization GetAutorizationForAction(string currentControllerName, string currentActionName)
        {
            var actionAuthorization = actionAuthorizations.FirstOrDefault(x => x.Controller == currentControllerName && x.Action == currentActionName);

            return actionAuthorization;
        }

        public UserAccount CreateUser(string name, string userName, string password)
        {
            Configuration salt = this.GetConfiguration("PasswordSalt");

            var newUser = new UserAccount();
            newUser.FirstName = name;
            newUser.Username = userName;
            newUser.Password = password;
            newUser.MemberSinceDate = DateTime.Now;

            newUser.Roles.AddRange(this.GetRolesForUser());

            users.Add(newUser);
            return newUser;
        }

        public bool UpdateAccount(UserAccount userAccount)
        {
            var selectedUser = users.FirstOrDefault(x => x.Username == userAccount.Username);

            if (selectedUser != default(UserAccount))
            {
                selectedUser.BirthDate = userAccount.BirthDate;
                selectedUser.CurrentStatus = userAccount.CurrentStatus;
                selectedUser.FirstName = userAccount.FirstName;
                selectedUser.LastName = userAccount.LastName;
                selectedUser.MemberSinceDate = userAccount.MemberSinceDate;
                selectedUser.Position = userAccount.Position;
                selectedUser.IsLocked = userAccount.IsLocked;
                selectedUser.LockedDate = userAccount.LockedDate;
                selectedUser.LastLoginDate = userAccount.LastLoginDate;
                return true;
            }

            return false;
        }

        public List<UserTask> GetLastTasksForUser(string userName, int qtd)
        {
            var userMessages = userTasks.Where(x => x.User.Username == userName);

            if (userMessages.Count() == 0)
            {
                return new List<UserTask>();
            }

            return userMessages.Take(qtd).ToList();
        }

        public UserRole CreateRole(string username)
        {
            var newUserRole = new UserRole();
            newUserRole.Name = username;

            roles.Add(newUserRole);
            return newUserRole;
        }

        public List<UserRole> GetAllRoles()
        {
            var listOfUsers = roles.ToList();

            return listOfUsers;
        }

        public UserRole GetRole(string name)
        {
            var listOfUsers = roles
                               .Where(r => r.Name == name)
                               .ToList();

            return listOfUsers.FirstOrDefault();
        }

        public bool UpdateRole(UserRole role, string name)
        {
            var selectedRole = roles.FirstOrDefault(x => x.Name == name);

            if (selectedRole != default(UserRole))
            {
                selectedRole.Name = role.Name;
                return true;
            }

            return false;
        }

        public bool DeleteRole(string name)
        {
            var selectedRole = roles.FirstOrDefault(x => x.Name == name);

            return roles.Remove(selectedRole);
        }

        public bool UpdateRoleUsers(string name, List<string> usersInRole)
        {
            var selectedRole = roles.FirstOrDefault(x => x.Name == name);

            if (selectedRole != default(UserRole))
            {
                selectedRole.UserAccounts.RemoveAll(x => !usersInRole.Any(z => z == x.Username));

                foreach (var userName in usersInRole.Where(x => !selectedRole.UserAccounts.Any(z => z.Username == x)))
                {
                    selectedRole.UserAccounts.Add(users.FirstOrDefault(x => x.Username == userName));
                }

                return true;
            }

            return false;
        }
        #endregion

        #region Support IUserManagementRepo
        private List<UserRole> GetRolesForUser()
        {
            var listOfRoles = new List<UserRole>();

            var role = roles.FirstOrDefault(x => x.Name == "User");
            if (role == default(UserRole))
            {
                role = new UserRole()
                {
                    Name = "User"
                };

                roles.Add(role);
            }
            listOfRoles.Add(role);

            return listOfRoles;
        }
        #endregion

        #region INotificationRepo
        public bool SendNotification(UserNotification userNotification)
        {
            userNotification.User = users.First(x => x.Username == userNotification.User.Username);

            userNotifications.Add(userNotification);

            return true;
        }

        public List<UserNotification> GetLastNotificationsForUser(string userName, int qtd, bool onlyUnread)
        {
            var userMessages = userNotifications.Where(x => x.User.Username == userName);

            if (onlyUnread)
            {
                userMessages = userMessages.Where(x => x.Read == false);
            }

            if (userMessages.Count() == 0)
            {
                return new List<UserNotification>();

            }
            return userMessages.Take(qtd).ToList();
        }

        public void ClearNotifications(NotificationType notificationType, string userName)
        {
            var userMessages = userNotifications
                .Where(x => x.User.Username == userName
                    && x.Type == notificationType
                    && !x.Read);

            foreach (var item in userMessages)
            {
                item.Read = true;
            }
        }

        public void MarkNotificationAsRead(int id)
        {
            var userMessages = userNotifications
                .Where(x => x.Id == id);

            foreach (var item in userMessages)
            {
                item.Read = true;
            }
        }

        public List<UserNotification> GetNotifications(string userName)
        {
            var userMessages = userNotifications
                .Where(x => x.User.Username == userName);

            return userMessages.ToList();
        }

        public void DeleteNotification(string userName, int selectedInt)
        {
            var notifications = userNotifications
                .RemoveAll(x => x.User.Username == userName
                    && x.Id == selectedInt);
        }
        #endregion

        #region IMessageRepo
        public List<UserMessage> GetLastMessages(string userName, int qtd)
        {
            return userMessages.OrderByDescending(x => x.SendDate).Take(qtd).ToList();
        }

        public List<UserMessageFolder> GetAllFolders(string userName)
        {
            throw new NotImplementedException();
        }

        public UserMessageFolder GetFolder(string userName, int folderId)
        {
            throw new NotImplementedException();
        }

        public UserMessageFolder GetInboxFolder(string userName)
        {
            throw new NotImplementedException();
        }

        public UserMessageFolder GetSentFolder(string userName)
        {
            throw new NotImplementedException();
        }

        public List<UserMessage> GetMessages(string userName, int folderId)
        {
            throw new NotImplementedException();
        }

        public UserMessageFolder CreateDefaultFolder(string userName)
        {
            throw new NotImplementedException();
        }

        public UserMessage SaveMessage(UserMessage sentMessage)
        {
            throw new NotImplementedException();
        }

        public void DeleteMessage(int messageId)
        {
            throw new NotImplementedException();
        }

        public UserMessage GetMessage(string userName, int id)
        {
            throw new NotImplementedException();
        }

        public UserMessageFolder SaveFolder(UserMessageFolder model)
        {
            throw new NotImplementedException();
        }

        public void MoveMessageToFolder(string userName, int msgId, int newFolderId)
        {
            throw new NotImplementedException();
        }

        public void DeleteFolder(string userName, int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ICoreRepo
        public void AddConfiguration(string key, string value)
        {
            var config = configurations.FirstOrDefault(x => x.Key == key);
            if (config == default(Configuration))
            {
                configurations.Add(new Configuration() { Key = key, Value = value });
            }
        }

        public Configuration GetConfiguration(string key)
        {
            var config = configurations.FirstOrDefault(x => x.Key == key);
            if (config == default(Configuration))
            {
                throw new Exception("Configuration (" + key + ") is not set.");
            }

            return config;
        }
        #endregion
    }
}
