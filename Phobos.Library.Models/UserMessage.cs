using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models
{
    [Serializable]
    public class UserMessage
    {
        public UserMessage()
        {
            Message = "";
            MessageDate = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime MessageDate { get; set; }
        public string Title { get; set; }
        public bool HasAttachment { get; set; }
        public bool IsFavorite { get; set; }
        public virtual UserAccount Receiver { get; set; }
        public virtual UserAccount Sender { get; set; }
        public virtual UserAccount Owner { get; set; }
        public virtual UserMessageFolder Folder { get; set; }
        public List<string> Attachments { get; set; }
        public bool Sent { get; set; }
        public bool IsDraft { get; set; }
    }
}
