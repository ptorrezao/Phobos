using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models.ViewModels
{
    public class SideTabViewModel
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string FontAwesomeIcon { get; set; }
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public object RouteValues { get; set; }
    }
}
