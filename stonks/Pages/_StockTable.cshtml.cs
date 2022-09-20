using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Models;

namespace stonks.Pages
{
    public class _StockTableModel : PageModel
    {

        private PortfolioStock[] portfolioStocks;

        /// <summary>
        /// The list of PortfolioStocks relevant to this stock table
        /// </summary>
        public PortfolioStock[] PortfolioStocks
        {
            get { return portfolioStocks; }
            set { portfolioStocks = value; }
        }


        
        private Stock stock;

        /// <summary>
        /// The stock this table is for
        /// </summary>
        public Stock Stock
        {
            get { return stock; }
            set { stock = value; }
        }


        private string errorMessage;

        /// <summary>
        /// The error message to display
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public _StockTableModel()
        {

        }

        /// <summary>
        /// Constructor with errormessage parameter
        /// </summary>
        /// <param name="errorMessage">The error message to display</param>
        public _StockTableModel(string errorMessage)
        {
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stock">The stock this table is for</param>
        /// <param name="portfolioStocks">The list of PortfolioStocks relevant to this stock table</param>
        /// <param name="errorMessage">Constructor with errormessage parameter</param>
        public _StockTableModel(Stock stock, PortfolioStock[] portfolioStocks, string errorMessage = null)
        {
            this.stock = stock;
            this.portfolioStocks = portfolioStocks;
            this.errorMessage = errorMessage;
        }

        public void OnGet()
        {

        }
    }
}
