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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Action { get; set; }
        public string Controller { get; set; }
        public List<UserRole> Roles { get; set; }
        public List<UserAccount> UserAccounts { get; set; }
    }
}
