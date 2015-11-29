using Phobos.Library.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Models.ViewModels;

namespace Phobos.Library.CoreServices
{
    public class NavigationCoreService : INavigationService
    {
        public MenuEntriesListViewModel GetMenusForUser(string username)
        {
            var menus = new MenuEntriesListViewModel();

            menus.Title = "Main Navigation";

            menus.Add(new MenuEntriesViewModel() { Controller = "Home", Action = "Index", Text = "Home" });

            return menus;
        }
    }
}
