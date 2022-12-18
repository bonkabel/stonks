using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// A reported message
    /// </summary>
    public class ReportedMessage
    {
        /// <summary>
        /// The id of the report
        /// </summary>
        [Key]
        [Required]
        public Guid ReportId { get; set; }

        /// <summary>
        /// The id of the message
        /// foreign key
        /// </summary>
        [Required]
        public Guid MessageId { get; set; }

        public ReportedMessage(Guid reportId, Guid messageId )
        {
            ReportId = reportId;
            MessageId = messageId;
        }
    }

    
}
