using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// Represents a tag for a specific post
    /// </summary>
    public class PostTag
    {
        /// <summary>
        /// The id of the tag
        /// foreign key
        /// </summary>
        [Key]
        [Required]
        public Guid TagId { get; set; }

        /// <summary>
        /// The id of the post
        /// foreign key
        /// </summary>
        [Key]
        [Required]
        public Guid PostId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tagId">The id of the tag</param>
        /// <param name="postId">The id of the post</param>
        public PostTag(Guid tagId, Guid postId)
        {
            TagId = tagId;
            PostId = postId;
        }
        
    }
}
