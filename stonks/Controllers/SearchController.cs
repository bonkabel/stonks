using Microsoft.AspNetCore.Mvc;
using stonks.Classes;
using stonks.Data;
using stonks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Controllers
{
    /// <summary>
    /// A controller for a stock being searched for.
    /// </summary>
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext db;

        public SearchController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public string Index()
        {
            return "This is my default action...";
        }

        /// <summary>
        /// Searches for a stock
        /// </summary>
        /// <param name="SearchTerm">The name of the stock to search for</param>
        /// <returns>The stock page if it exists, otherwise an error page</returns>
        [HttpGet("[action]")]
        public ActionResult DoSearch(string SearchTerm)
        {
            Stock stock = null;


            if (Helper.ValidateStockName(SearchTerm, out stock, db))
            {
                return Redirect("~/Stock/" + stock.StockId);
            }
            else
            {
                return Redirect("~/Stock/ERROR");
            }
        }


    }
}
