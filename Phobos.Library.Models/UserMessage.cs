using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models
{
    public class UserMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Message { get;  set; }
        public DateTime SentDate { get;  set; }
        public string Title { get;  set; }
        public virtual UserAccount User { get; set; }
    }
}
