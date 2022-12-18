using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// A table of messages
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The unique id of the message
        /// </summary>
        [Key]
        [Required]
        public Guid MessageId { get; set; }

        /// <summary>
        /// The unique id of the sendor of the message
        /// </summary>
        [Required]
        public Guid SendorId { get; set; }

        /// <summary>
        /// The unique id of the recipient of the message
        /// </summary>
        [Required]
        public Guid RecipientId { get; set; }


        /// <summary>
        /// The date the message was sent
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// The title of the message
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The text of the message
        /// </summary>
        [Required]
        public string Text { get; set; }

        public Message()
        {

        }

        public Message(Guid messageId, Guid sendorId, Guid recipientId, DateTime date, string title, string text)
        {
            MessageId = messageId;
            SendorId = sendorId;
            RecipientId = recipientId;
            Date = date;
            Title = title;
            Text = text;
        }
    }
}
