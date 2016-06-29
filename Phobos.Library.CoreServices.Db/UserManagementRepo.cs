using Ninject;
using Phobos.Library.Interfaces;
using Phobos.Library.Interfaces.Repos;
using Phobos.Library.Interfaces.Services;
using Phobos.Library.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.CoreServices.Db
{
    public class UserManagementRepo : IUserManagementRepo
    {
        [Inject]
        public ICoreRepo CoreRepository { get; set; }

        [Inject]
        public IMessageService MsgService { get; set; }

        public bool AddFailedLoginAttempt(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var selectedUser = context.Users.FirstOrDefault(x => x.Username == userName);

                if (selectedUser != default(UserAccount))
                {
                    selectedUser.FailedAttempts++;

                    if (selectedUser.FailedAttempts > 3)
                    {
                        selectedUser.LockedDate = DateTime.Now;
                        selectedUser.IsLocked = true;
                    }

                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public bool CheckIfUserExist(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var selectedUser = context.Users.FirstOrDefault(x => x.Username == userName);

                return selectedUser != default(UserAccount);
            }
        }

        public UserAccount CreateUser(string name, string userName, string password)
        {
            using (var context = new PhobosCoreContext())
            {
                Configuration salt = CoreRepository.GetConfiguration("PasswordSalt");

                var newUser = new UserAccount();
                newUser.FirstName = name;
                newUser.Username = userName;
                newUser.Password = password;
                newUser.MemberSinceDate = DateTime.Now;

                newUser.Roles.AddRange(this.GetRolesForUser(context));

                context.Users.Add(newUser);
                context.SaveChanges();
                return newUser;
            }
        }

        private List<UserRole> GetRolesForUser(PhobosCoreContext context)
        {
            var listOfRoles = new List<UserRole>();

            var role = context.Roles.FirstOrDefault(x => x.Name == "User");
            if (role == default(UserRole))
            {
                role = new UserRole()
                {
                    Name = "User"
                };
            }
            listOfRoles.Add(role);

            return listOfRoles;
        }

        public ActionAuthorization GetAutorizationForAction(string currentControllerName, string currentActionName)
        {
            using (var context = new PhobosCoreContext())
            {
                var actionAuthorization = context.ActionAuthorizations
                    .Include(x => x.Roles)
                    .Include(x => x.UserAccounts)
                    .Include(x => x.Roles.Select(r => r.UserAccounts))
                    .FirstOrDefault(x => x.Controller == currentControllerName && x.Action == currentActionName);
                return actionAuthorization;
            }
        }

        public List<UserMessage> GetLastMessagesForUser(string userName, int qtd)
        {
            return this.MsgService.GetLastMessages(userName, qtd, true);
        }

        public List<UserNotification> GetLastNotificationsForUser(string userName, int qtd)
        {
            using (var context = new PhobosCoreContext())
            {
                var userMessages = context.UserNotifications
                    .Where(x => x.User.Username == userName);

                if (userMessages.Count() == 0)
                {
                    return new List<UserNotification>();

                }
                return userMessages.Take(qtd).ToList();
            }
        }

        public List<UserTask> GetLastTasksForUser(string userName, int qtd)
        {
            using (var context = new PhobosCoreContext())
            {
                var userMessages = context.UserTasks
                    .Where(x => x.User.Username == userName);

                if (userMessages.Count() == 0)
                {
                    return new List<UserTask>();

                }
                return userMessages.Take(qtd).ToList();
            }
        }

        public UserAccount GetUser(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var selectedUser = context.Users.Include(x => x.Roles)
                    .FirstOrDefault(x => x.Username == userName);

                return selectedUser;
            }
        }

        public List<UserAccount> GetAllUsers()
        {
            using (var context = new PhobosCoreContext())
            {
                var listOfUsers = context.Users
                                .Include(x => x.Roles)
                                .ToList();

                return listOfUsers;
            }
        }

        public bool UnlockUserAccount(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var selectedUser = context.Users.FirstOrDefault(x => x.Username == userName);

                if (selectedUser != default(UserAccount))
                {
                    selectedUser.LockedDate = null;
                    selectedUser.FailedAttempts = 0;
                    selectedUser.IsLocked = false;

                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public bool LockUserAccount(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var selectedUser = context.Users.FirstOrDefault(x => x.Username == userName);

                if (selectedUser != default(UserAccount))
                {
                    selectedUser.LockedDate = DateTime.Now;
                    selectedUser.IsLocked = true;

                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public bool UpdateAccount(UserAccount userAccount)
        {
            using (var context = new PhobosCoreContext())
            {
                var selectedUser = context.Users.FirstOrDefault(x => x.Username == userAccount.Username);

                if (selectedUser != default(UserAccount))
                {
                    selectedUser.BirthDate = userAccount.BirthDate;
                    selectedUser.CurrentStatus = userAccount.CurrentStatus;
                    selectedUser.FirstName = userAccount.FirstName;
                    selectedUser.LastName = userAccount.LastName;
                    selectedUser.MemberSinceDate = userAccount.MemberSinceDate;
                    selectedUser.Position = userAccount.Position;
                    selectedUser.LastLoginDate = userAccount.LastLoginDate;
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public bool UpdateLastLoginDate(string userName, DateTime lastDate)
        {
            using (var context = new PhobosCoreContext())
            {
                var selectedUser = context.Users.FirstOrDefault(x => x.Username == userName);

                if (selectedUser != default(UserAccount))
                {
                    selectedUser.LastLoginDate = lastDate;
                    selectedUser.FailedAttempts = 0;
                    selectedUser.IsLocked = false;
                    selectedUser.CurrentStatus = Models.Enums.UserStatusEnum.Online;
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        private string GetSaltedHashPassword(string password, string saltString)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password + saltString);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2").ToLower());
            }

            return sb.ToString();
        }

        public UserRole CreateRole(string username)
        {
            using (var context = new PhobosCoreContext())
            {
                var newUserRole = new UserRole();
                newUserRole.Name = username;

                context.Roles.Add(newUserRole);
                context.SaveChanges();
                return newUserRole;
            }
        }

        public List<UserRole> GetAllRoles()
        {
            using (var context = new PhobosCoreContext())
            {
                var listOfUsers = context.Roles
                                .ToList();

                return listOfUsers;
            }
        }

        public UserRole GetRole(string name)
        {
            using (var context = new PhobosCoreContext())
            {
                var listOfUsers = context.Roles
                                .Where(r => r.Name == name)
                                .ToList();

                return listOfUsers.FirstOrDefault();
            }
        }


        public bool UpdateRole(UserRole role, string name)
        {
            using (var context = new PhobosCoreContext())
            {
                var selectedRole = context.Roles.FirstOrDefault(x => x.Name == name);

                if (selectedRole != default(UserRole))
                {
                    selectedRole.Name = role.Name;
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public bool DeleteRole(string name)
        {
            using (var context = new PhobosCoreContext())
            {
                var selectedRole = context.Roles.FirstOrDefault(x => x.Name == name);

                context.Roles.Remove(selectedRole);
                return context.SaveChanges() > 0;
            }
        }
    }
}
