using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// A reply to a message
    /// </summary>
    public class MessageReply
    {
        /// <summary>
        /// The unique id of the reply
        /// foreign key
        /// </summary>
        [Key]
        [Required]
        public Guid ReplyId { get; set; }

        /// <summary>
        /// The unique id of the message the reply is to
        /// </summary>
        [Required]
        public Guid MessageId { get; set; }

        /// <summary>
        /// The unique id of the user that sent the reply
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The text of the reply
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// The date of the reply
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        public MessageReply()
        {

        }

        public MessageReply(Guid replyId, Guid messageId, Guid userId, string text, DateTime date)
        {
            ReplyId = replyId;
            MessageId = messageId;
            UserId = userId;
            Text = text;
            Date = date;
        }
    }
}
