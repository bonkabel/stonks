using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// A reply to a post
    /// </summary>
    public class PostReply
    {
        /// <summary>
        /// The id of the PostReply
        /// </summary>
        [Key]
        [Required]
        public Guid ReplyId { get; set; }

        /// <summary>
        /// The id of the Post
        /// foreign key
        /// </summary>
        [Required]
        public Guid PostId { get; set; }

        /// <summary>
        /// The id of the User
        /// foreign key
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The text of the PostReply
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// The Date of the PostReply
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Generic empty constructor
        /// </summary>
        public PostReply()
        {

        }

        public PostReply(Guid replyId, Guid postId, Guid userId, string text, DateTime date)
        {
            ReplyId = replyId;
            PostId = postId;
            UserId = userId;
            Text = text;
            Date = date;
        }
    }
}
