using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models
{
    public class UserMessageFolder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public TextColor IconColor { get; set; }

        public string Icon { get; set; }

        public List<UserMessage> Messages { get; set; }

        public UserAccount User { get; set; }

    }
}
