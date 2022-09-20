using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Models;

namespace stonks.Pages
{
    /// <summary>
    /// The stock chart for the Portfolio Page
    /// </summary>
    public class _ChartModel : PageModel
    {
        private StockPrice[] stockPrices;

        /// <summary>
        /// The Stock Prices to create the chart with
        /// </summary>
        public StockPrice[] StockPrices
        {
            get { return stockPrices; }
            set { stockPrices = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stockPrices">The Stock Prices to create the chart with</param>
        public _ChartModel(StockPrice[] stockPrices)
        {
            this.stockPrices = stockPrices;
        }

        public void OnGet()
        {
        }
    }
}
