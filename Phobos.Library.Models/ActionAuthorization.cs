using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models
{
    public class ActionAuthorization
    {
        public ActionAuthorization()
        {
            Roles = new List<UserRole>();
            UserAccounts = new List<UserAccount>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Action { get; set; }
        public string Controller { get; set; }
        public virtual List<UserRole> Roles { get; set; }
        public virtual List<UserAccount> UserAccounts { get; set; }
    }
}
