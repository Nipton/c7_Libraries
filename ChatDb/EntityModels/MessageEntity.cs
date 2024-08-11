using ChatDb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDb.EntityModels
{
    public class MessageEntity
    {
        [Key] public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime TimeMessage { get; set; }
        public bool ReceivedStatus { get; set; } = false;
        public int? SenderId { get; set; }
        [ForeignKey("SenderId")]
        public UserEntity? Sender { get; set; }

        public int? RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public UserEntity? Recipient { get; set; }
    }
}
