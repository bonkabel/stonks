using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Classes;

namespace stonks.Pages
{
    public class _PortfolioTableModel : PageModel
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

        /// <summary>
        /// Empty constructor
        /// </summary>
        public _PortfolioTableModel()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="portfolioRows">The Portfolio rows to place into the table</param>
        public _PortfolioTableModel(PortfolioRow[] portfolioRows)
        {
            PortfolioRows = portfolioRows;
        }

        public void OnGet()
        {
        }
    }
}
