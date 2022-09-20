using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Classes;
using stonks.Models;

namespace stonks.Pages
{
    public class _PortfolioRowsAndStocksModel : PageModel
    {
        
        private PortfolioRow[] portfolioRows;

        /// <summary>
        /// The Portfolio rows to place into the table
        /// </summary>
        public PortfolioRow[] PortfolioRows
        {
            get { return portfolioRows; }
            set { portfolioRows = value; }
        }


        private PortfolioStock[] portfolioStocks;

        /// <summary>
        /// The PortfolioStocks relevant to this stock table
        /// </summary>
        public PortfolioStock[] PortfolioStocks
        {
            get { return portfolioStocks; }
            set { portfolioStocks = value; }
        }


        
        private Stock[] stocks;

        /// <summary>
        /// The Stocks to display
        /// </summary>
        public Stock[] Stocks
        {
            get { return stocks; }
            set { stocks = value; }
        }

        private string successMessage;

        /// <summary>
        /// The success message to display
        /// </summary>
        public string SuccessMessage
        {
            get { return successMessage; }
            set { successMessage = value; }
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
        public _PortfolioRowsAndStocksModel()
        {

        }

        /// <summary>
        /// Full constructor
        /// </summary>
        /// <param name="portfolioRows">The Portfolio rows to place into the table</param>
        /// <param name="stocks">The stocks to display</param>
        /// <param name="portfolioStocks">The PortfolioStocks relevant to this stock table</param>
        /// <param name="successMessage">The success message to display</param>
        /// <param name="errorMessage">The error message to display</param>
        public _PortfolioRowsAndStocksModel(PortfolioRow[] portfolioRows, Stock[] stocks, PortfolioStock[] portfolioStocks, string successMessage = null, string errorMessage = null)
        {
            PortfolioRows = portfolioRows;
            Stocks = stocks;
            PortfolioStocks = portfolioStocks;
            SuccessMessage = successMessage;
            ErrorMessage = errorMessage;
        }

        
        public void OnGet()
        {
        }
    }
}
