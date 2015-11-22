using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models.ViewModels
{
    public class MenuEntriesListViewModel : List<MenuEntriesViewModel>
    {
        public string Title { get; set; }
    }
}
