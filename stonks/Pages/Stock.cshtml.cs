using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Data;
using stonks.Models;

namespace stonks.Pages
{
    public class StockModel : PageModel
    {
        /// <summary>
        /// The database context
        /// </summary>
        private readonly ApplicationDbContext db;

        private Guid? userId;

        /// <summary>
        /// The users unique id
        /// </summary>
        public Guid? UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private Stock stock;
        /// <summary>
        /// The stock this page is for
        /// </summary>
        public Stock Stock
        {
            get { return stock; }
            set { stock = value; }
        }

        //public List<StockPrice> StockPrices { get; set; }

        private StockPrice[] stockPrices;

        /// <summary>
        /// The StockPrices for the stock chart
        /// </summary>
        public StockPrice[] StockPrices
        {
            get { return stockPrices; }
            set { stockPrices = value; }
        }

        private StockInputModel input;
        
   
        [BindProperty]
        public StockInputModel Input
        {
            get { return input; }
            set { input = value; }
        }

        private List<String> errorMessages;

        /// <summary>
        /// The error messages to display
        /// </summary>
        public List<String> ErrorMessages
        {
            get { return errorMessages; }
            set { errorMessages = value; }
        }

        /// <summary>
        /// The stock input model.  This is for verifying the user input
        /// </summary>
        public class StockInputModel
        {
            [Required(ErrorMessage = "Please enter a date")]
            [DataType(DataType.Date, ErrorMessage = "Please ensure date is a valid date")]
            public DateTime Date { get; set; }

            [Required(ErrorMessage = "Please enter a price")]
            [DataType(DataType.Currency, ErrorMessage = "Please ensure price is a valid currency")]
            [Range(1, (Double)Decimal.MaxValue, ErrorMessage = "Please ensure that price is above zero")]
            public Decimal Price { get; set; }

            [Required(ErrorMessage = "Please enter a number of shares")]
            [Range(1, Int32.MaxValue, ErrorMessage = "Please ensure that shares is above zero")]
            public int Shares { get; set; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="db">The database context</param>
        public StockModel(ApplicationDbContext db)
        {
            this.db = db;
            errorMessages = new List<String>();
            
        }

        /// <summary>
        /// Sets up the values using the stock id
        /// </summary>
        /// <param name="id">The stock id</param>
        public void OnGet(Guid id)
        {
            if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null && id != Guid.Empty)
            {
                UserId = new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                SetupValues(id);
            }
        }

        // TODO: Should this be async
        /// <summary>
        /// Sets up values using the stock id for this page
        /// </summary>
        /// <param name="id">The id of the stock this page is for</param>
        private void SetupValues(Guid id)
        {

            // Surely theres a more sensical way to to this?
            IEnumerable<Stock> tempStocks = db.Stocks.Where(s => s.StockId.Equals(id));
            Stock = tempStocks.First();

            StockPrices = db.StockPrices.Where(sp => sp.StockId.Equals(id)).ToArray();
        }

        /// <summary>
        /// Handler for the stock form being submitted.
        /// </summary>
        /// <param name="id">The id of the Stock</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(Guid id)
        {

            SetupValues(id);
            if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null && id != Guid.Empty && CheckFormData())
            {
                UserId = new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                

                IEnumerable<StockPrice> tempPrices = db.StockPrices.Where(sp => sp.StockId.Equals(Stock.StockId) && sp.Date.Equals(Input.Date));
                StockPrice stockPrice = tempPrices.FirstOrDefault();

                // Create it if it doesn't already exist in PortfolioStocks, else just increment the shares
                PortfolioStock portfolioStock = db.PortfolioStocks.Where(ps => ps.StockId.Equals(Stock.StockId) && ps.Date.Equals(Input.Date) && ps.UserId.Equals(userId)).FirstOrDefault();
                if (portfolioStock != null)
                {
                    portfolioStock.Shares += Input.Shares;
                }
                else
                {
                    db.PortfolioStocks.Add(new PortfolioStock(Guid.NewGuid(), Stock.StockId, UserId.Value, Input.Date, Input.Shares, Input.Price));
                }

                // TODO: confirm addition of stock
                await db.SaveChangesAsync();
            }

            return Redirect("/Portfolio");
        }

        // TODO: make this better maybe
        /// <summary>
        /// Checks if the form data is valid
        /// </summary>
        /// <returns>Whether or not the form data is valid</returns>
        private bool CheckFormData()
        {
            bool valid = true;

            if (!ModelState.IsValid)
            {
                valid = false;
                errorMessages.Add("Please enter the required data");
            }
            else if (db.StockPrices.Where(sp => sp.StockId.Equals(Stock.StockId) && sp.Date.Equals(Input.Date)).FirstOrDefault() == null)
            {
                valid = false;
                errorMessages.Add("Please choose a valid stock");
            }
            else if (userId == Guid.Empty)
            {
                valid = false;
                errorMessages.Add("Please log in");
            }

            return valid;
        }
    }
}
