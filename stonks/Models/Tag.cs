using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// A tag for a post
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// The id of the tag
        /// </summary>
        [Key]
        [Required]
        public Guid TagId { get; set; }

        /// <summary>
        /// The text of the tag
        /// </summary>
        [Required]
        public string Text { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tagId">The id of the tag</param>
        /// <param name="text">the text of the tag</param>
        public Tag(Guid tagId, string text)
        {
            TagId = tagId;
            Text = text;
        }

    }
}
