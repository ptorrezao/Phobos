using Phobos.Library.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Models.ViewModels;
using Ninject;
using Phobos.Library.Interfaces.Repos;

namespace Phobos.Library.CoreServices
{
    public class NavigationCoreService : INavigationService
    {
        #region Injects
        [Inject]
        public IUserManagementRepo Repository { get; set; }

        [Inject]
        public IAuditTrailService AuditTrail { get; set; }
        #endregion

        public MenuEntriesListViewModel GetMenusForUser(string username)
        {
            var menus = new MenuEntriesListViewModel();

            menus.Title = "Main Navigation";

            menus.Add(new MenuEntriesViewModel() { Controller = "Home", Action = "Index", Text = "Home" });
            return menus;
        }

        public bool CheckIfActionIsAllowed(string currentControllerName, string currentActionName, string username)
        {
            var selectedAction = this.Repository.GetAutorizationForAction(currentControllerName, currentActionName);

            if (selectedAction != null)
            {
                if (selectedAction.Roles.Any(x => x.UserAccounts.Any(z => z.Username == username)) ||
                    selectedAction.UserAccounts.Any(x => x.Username == username))
                {
                    var msg = string.Format("Was found action authorizations for the specified action/controller ({0}/{1}) for user {2}.", currentActionName, currentControllerName, username);

                    AuditTrail.LogInfoMessage(msg, username, DateTime.Now);

                    return true;
                }
                else
                {
                    //// Action is not authorized for this user

                    var msg = string.Format("Was not found action authorizations for the specified action/controller ({0}/{1}) for user {2}.", currentActionName, currentControllerName, username);

                    AuditTrail.LogInfoMessage(msg, username, DateTime.Now);

                    return false;
                }
            }
            else
            {
                //// Action is not authorized

                var msg = string.Format("Was not found action authorizations for the specified action/controller ({0}/{1}).", currentActionName, currentControllerName);

                AuditTrail.LogInfoMessage(msg, username, DateTime.Now);

                return false;
            }
        }
    }
}
