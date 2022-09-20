using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// Represents a stock
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// The id of the stock
        /// </summary>
        [Key]
        [Required]
        public Guid StockId { get; set; }

        /// <summary>
        /// The name of the stock
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The number of people tracking the stock
        /// </summary>
        [Required]
        public int PeopleTracking { get; set; }


    }
}
