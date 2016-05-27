using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phobos.Library.Models
{
    public class UserRole
    {
        public UserRole()
        {
            ActionAuthorizations = new List<ActionAuthorization>();
            UserAccounts = new List<UserAccount>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<UserAccount> UserAccounts { get; set; }
        public virtual List<ActionAuthorization> ActionAuthorizations { get; set; }
    }
}