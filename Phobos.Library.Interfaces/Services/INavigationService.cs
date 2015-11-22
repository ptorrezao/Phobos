using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Interfaces.Services
{
    public interface INavigationService
    {
        MenuEntriesListViewModel GetMenusForUser(string username);
    }
}
