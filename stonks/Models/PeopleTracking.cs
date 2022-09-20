using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// Represents a stock and the number of people tracking it
    /// </summary>
    public class PeopleTracking
    {
        /// <summary>
        /// The id of the stock
        /// </summary>
        [Key]
        [Required]
        public Guid StockId { get; set; }

        [Key]
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The number of people tracking 
        /// </summary>
        public int Tracking { get; set; }
    }
}
