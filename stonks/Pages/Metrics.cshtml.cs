using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Data;
using stonks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace stonks.Pages
{
    public class MetricsModel : PageModel
    {
        public Dictionary<Guid, Stock> Stocks = new Dictionary<Guid, Stock>();

        private readonly ApplicationDbContext db;

        private User currentUser;

        /// <summary>
        /// The users that viewing this page
        /// </summary>
        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

        public MetricsModel(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void OnGet()
        {
            if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                Guid userId = new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                CurrentUser = db.Users.First(u => u.Id == userId.ToString());

                PeopleTracking[] tracking = db.PeopleTracking.ToArray();

                foreach (PeopleTracking person in tracking)
                {
                    if (Stocks.ContainsKey(person.StockId))
                    {
                        Stocks[person.StockId].PeopleTracking++;
                    }
                    else
                    {
                        Stocks.Add(person.StockId, new Stock(person.StockId, db.Stocks.First(s => s.StockId == person.StockId).Name, 1));
                    }
                }


            }

                
        }
    }
}
