using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// Represents a price of a stock on a specific day
    /// </summary>
    public class StockPrice
    {
        /// <summary>
        /// The id of the stock
        /// foreign key
        /// </summary>
        [Key]
        [Required]
        public Guid StockId { get; set; }

        /// <summary>
        /// The Date of the stock price
        /// </summary>
        [Key]
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// The price at open
        /// </summary>
        [Required]
        public decimal Open { get; set; }

        /// <summary>
        /// The price at close
        /// </summary>
        [Required]
        public decimal Close { get; set; }

        /// <summary>
        /// The high price of the day
        /// </summary>
        [Required]
        public decimal High { get; set; }

        /// <summary>
        /// The low price of the day
        /// </summary>
        [Required]
        public decimal Low { get; set; }

        /// <summary>
        /// The volume traded
        /// </summary>
        [Required]
        public long Volume { get; set; }

        /// <summary>
        /// The number of shares
        /// </summary>
        public int numShares;


    }
}
