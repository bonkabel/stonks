using Microsoft.AspNetCore.Mvc;
using stonks.Data;
using stonks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.ViewComponents
{
    [ViewComponent(Name = "Search")]
    public class SearchViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public SearchViewComponent(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Stock[] stocks = db.Stocks.ToArray();
            List<string> stockNames = new List<string>();

            foreach (Stock stock in stocks)
            {
                stockNames.Add(stock.Name);
            }

            return View("Index", stockNames);
        }
    }
}
