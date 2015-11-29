using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models
{
    public class ActionAuthorization
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public List<UserRole> Roles { get; set; }
        public List<UserAccount> UserAccounts { get; set; }
    }
}
