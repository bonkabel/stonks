using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// A reported user
    /// </summary>
    public class ReportedUser
    {
        /// <summary>
        /// The id of the report
        /// </summary>
        [Key]
        [Required]
        public Guid ReportId { get; set; }

        /// <summary>
        /// The id of the user
        /// foreign key
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

    }
}
