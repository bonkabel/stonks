using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// a stock in a users portfolio
    /// </summary>
    public class PortfolioStock
    {
        /// <summary>
        /// The id of the PortfolioStock
        /// </summary>
        [Key]
        [Required]
        public Guid PortfolioStockId { get; set; }

        /// <summary>
        /// The id of the stock
        /// foreign key
        /// </summary>
        [Required]
        public Guid StockId { get; set; }

        /// <summary>
        /// The id of the user that has the stock
        /// foreign key
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The date the stock was "purchased"
        /// foreign key
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// the number of shares
        /// </summary>
        [Required]
        public int Shares { get; set; }

        /// <summary>
        /// The price at "purchase"
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        public PortfolioStock()
        {

        }

        public PortfolioStock(Guid portfolioStockId, Guid stockId, Guid userId, DateTime date, int shares, decimal price)
        {
            this.PortfolioStockId = portfolioStockId;
            this.StockId = stockId;
            this.UserId = userId;
            this.Date = date;
            this.Shares = shares;
            this.Price = price;
        }

    }
}
