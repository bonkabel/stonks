using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// A reported post 
    /// </summary>
    public class ReportedPost
    {

        /// <summary>
        /// The id of the report
        /// </summary>
        [Key]
        [Required]
        public Guid ReportId { get; set; }

        /// <summary>
        /// The id of the post that was reported
        /// foreign key
        /// </summary>
        [Required]
        public Guid PostId { get; set; }

        public ReportedPost()
        {

        }

        public ReportedPost(Guid reportId, Guid postId)
        {
            ReportId = reportId;
            PostId = postId;
        }
    }
}
