using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// Represents a post on the discussion forum
    /// </summary>
    public class Post
    {
        /// <summary>
        /// The Id of the post
        /// </summary>
        [Key]
        [Required]
        public Guid PostId { get; set; }

        /// <summary>
        /// The id of the user who made the post
        /// foreign key
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The title of the post
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The text of the post
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// The date the post was made
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// The number of replies to the post
        /// </summary>
        [Required]
        public int NumberReplies { get; set; }


        public Post(Guid userId, string title, string text)
        {
            PostId = Guid.NewGuid();
            UserId = userId;
            Title = title;
            Text = text;
            Date = DateTime.UtcNow;
            NumberReplies = 0;

        }
    }
}
