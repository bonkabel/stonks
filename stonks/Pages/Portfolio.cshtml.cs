using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using stonks.Classes;
using stonks.Data;
using stonks.Models;

namespace stonks.Pages
{
	public class PortfolioModel : PageModel
	{
		/// <summary>
		/// The db context
		/// </summary>
		private readonly ApplicationDbContext db;

        private List<PortfolioStock> portfolioStocks;

		/// <summary>
		/// The users stocks they have in their portfolio
		/// </summary>
		public List<PortfolioStock> PortfolioStocks
        {
            get { return portfolioStocks; }
            set { portfolioStocks = value; }
        }


        private List<PortfolioRow> portfolioRows;

		/// <summary>
		/// The rows of the users portfolio
		/// </summary>
		public List<PortfolioRow> PortfolioRows
        {
            get { return portfolioRows; }
            set { portfolioRows = value; }
        }

        private List<Stock> stocks;

		/// <summary>
		/// The stocks the user has
		/// </summary>
		public List<Stock> Stocks
        {
            get { return stocks; }
            set { stocks = value; }
        }


        private StockPrice[] stockPrices;

		/// <summary>
		/// The StockPrices to display in the chart
		/// </summary>
		public StockPrice[] StockPrices
        {
            get { return stockPrices; }
            set { stockPrices = value; }
        }

        private Guid? guid;

		/// <summary>
		/// The users unique id
		/// </summary>
        public Guid? UserId
        {
            get { return guid; }
            set { guid = value; }
        }


        /// <summary>
        /// Constructor.  Sets the db context
        /// </summary>
        /// <param name="db">The db context</param>
        public PortfolioModel(ApplicationDbContext db)
		{
			this.db = db;

		}

		/// <summary>
		/// Sets up everything
		/// </summary>
		public void OnGet()
		{
			if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
				UserId = new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
				SetStocks();
				SetupChart();
			}
			
		}

		// TODO: should this be async
		// TODO: Is this too many database calls?  Could/should it be cleaner?
		/// <summary>
		/// Sets Stocks, PortfolioStocks and PortfolioRows
		/// </summary>
		/// <param name="currentStocks">The users currently selected stocks</param>
		public void SetStocks(List<String> currentStocks = null)
		{

			// Getting the PortfolioStocks for this user
			PortfolioStocks = db.PortfolioStocks.Where(ps => ps.UserId.Equals(UserId)).ToList();

			if (currentStocks != null)
			{
				// Getting the users unique Stocks that are currently selected
				Stocks = db.Stocks.Where(s => currentStocks.Contains(s.Name)).ToList();
				PortfolioStocks = PortfolioStocks.Where(ps => Stocks.Any(s => s.StockId == ps.StockId)).ToList();
			}
			else
			{
				// Getting the users unique Stocks
				Stocks = db.Stocks.Where(s => PortfolioStocks.Select(ps => ps.StockId).Distinct().Contains(s.StockId)).ToList();
			}

			// Not sure how to do this in other notation
			// Also a lot of wildly uneccesary calculations and iteration
			var result = from ps in PortfolioStocks
						 join sp in db.StockPrices on ps.StockId equals sp.StockId

						 group new { ps, sp } by new { id = ps.StockId, sp.StockId } into grp

						 join s in db.Stocks on grp.Key.StockId equals s.StockId
						 select new PortfolioRow
						 {
							 // TODO: BETTER  WAY TO DO THIS GROUP STUFF
							 Ticker = s.Name,
							 Shares = grp.Select(i => i.ps).Distinct().Select(ps => ps.Shares).Sum(),
							 TotalPricePurchase = grp.Select(i => i.ps).Distinct().Select(ps => ps.Price).Sum() * grp.Select(i => i.ps).Distinct().Select(ps => ps.Shares).Sum(), 
							 TotalPriceCurrent = grp.Where(i => i.sp.Date == grp.Max(x => x.sp.Date)).First().sp.Open * grp.Select(i => i.ps).Distinct().Select(ps => ps.Shares).Sum()
						 };

			PortfolioRows = result.ToList();

			decimal totalValue = 0;

			foreach (PortfolioRow portfolioRow in PortfolioRows)
            {
				totalValue += portfolioRow.TotalPriceCurrent;
            }

			foreach (PortfolioRow portfolioRow in PortfolioRows)
            {
				portfolioRow.PercentPortfolio = portfolioRow.TotalPriceCurrent / totalValue * 100;
            }
		}

		/// <summary>
		/// Sets up the data that is required for the stock chart.
		/// Takes the users stocks and creates an array of StockPrices that combines all of them.
		/// Sets StockPrices.
		/// </summary>
		public void SetupChart()
		{
			// Initializing StockPrices
			StockPrices = new StockPrice[100];

			if (Stocks.Count() != 0 && PortfolioStocks.Count() != 0)
			{
				// The list of StockPrices for each Stock
				List<List<StockPrice>> stockPricesList = new List<List<StockPrice>>();

				// For each of the users stocks, add the stocks list of stockprices to stockPricesList
				foreach (PortfolioStock portfolioStock in PortfolioStocks)
				{
					stockPricesList.Add(db.StockPrices.Where(sp => sp.StockId.Equals(portfolioStock.StockId) && sp.Date >= portfolioStock.Date).ToList());
				}

				// Data set requirements: no duplicates, sorted/sortable, and can access easily
				// SortedSet does not work because I need to access them still
				// Dictionary with DateTime as key?  But datetime is already in StockPrice so is that dumb? whatever it works
				Dictionary<DateTime, StockPrice> StockPriceDict = new Dictionary<DateTime, StockPrice>();
				foreach (List<StockPrice> stockPrices in stockPricesList)
				{
					// The number of shares of this specific stock the user has
					if (stockPrices.Count > 0)
                    {
						int shares = PortfolioStocks.Where(ps => ps.StockId == stockPrices[0].StockId).FirstOrDefault().Shares;

						for (int i = 0; i < stockPrices.Count(); i++)
						{
							if (StockPriceDict.ContainsKey(stockPrices[i].Date))
							{
								StockPriceDict[stockPrices[i].Date].Open = StockPriceDict[stockPrices[i].Date].Open + stockPrices[i].Open * shares;
								StockPriceDict[stockPrices[i].Date].Close = StockPriceDict[stockPrices[i].Date].Close + stockPrices[i].Close * shares;
								StockPriceDict[stockPrices[i].Date].High = StockPriceDict[stockPrices[i].Date].High + stockPrices[i].High * shares;
								StockPriceDict[stockPrices[i].Date].Low = StockPriceDict[stockPrices[i].Date].Low + stockPrices[i].Low * shares;
								StockPriceDict[stockPrices[i].Date].Volume = StockPriceDict[stockPrices[i].Date].Volume + stockPrices[i].Volume * shares;
								StockPriceDict[stockPrices[i].Date].numShares = StockPriceDict[stockPrices[i].Date].numShares + stockPrices[i].numShares * shares;
							}
							else
							{
								stockPrices[i].Open = stockPrices[i].Open * shares;
								stockPrices[i].Close = +stockPrices[i].Close * shares;
								stockPrices[i].High = stockPrices[i].High * shares;
								stockPrices[i].Low = stockPrices[i].Low * shares;
								stockPrices[i].Volume = stockPrices[i].Volume * shares;
								stockPrices[i].numShares = stockPrices[i].numShares * shares;
								StockPriceDict.Add(stockPrices[i].Date, stockPrices[i]);
							}
						}
					}
					
				}

				// Ordering StockPrices
				var pairList = StockPriceDict.OrderBy(pair => pair.Key).ToArray();
				for (int i = 0; i < pairList.Length; i++)
				{
					StockPrices[i] = pairList[i].Value;
				}

				// Cutting out nulls in StockPrices
				List<StockPrice> tempStockPrices = StockPrices.ToList();
				for (int i = 0; i < StockPrices.Length; i++)
				{
					if (StockPrices[i] == null)
					{
						tempStockPrices.Remove(StockPrices[i]);
					}
				}
				StockPrices = tempStockPrices.ToArray();

			}

		}

		/// <summary>
		/// Handles the updating of the _Chart
		/// Changes what stocks are displayed in the chart based on currentStocks
		/// </summary>
		/// <param name="currentStocks">The current stocks that are selected in the chart</param>
		/// <returns>The updated _Chart</returns>
		public async Task<IActionResult> OnPostUpdateStocks(List<String> currentStocks)
        {
			PartialViewResult result = null;

			if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				UserId = new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

				SetStocks(currentStocks);

				SetupChart();

				// Creating the PartialViewResult
				_ChartModel model = new _ChartModel(StockPrices);
				ViewDataDictionary viewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { { "_Chart", model } };
				viewData.Model = model;
				result = new PartialViewResult()
				{
					ViewName = "_Chart",
					ViewData = viewData,
				};
			}

			return result;
		}


		/// <summary>
		/// Handles the updating of PortfolioStocks from an ajax request
		/// </summary>
		/// <param name="idInputs">The ids of the PortfolioStocks to update</param>
		/// <param name="priceInputs">The prices of the PortfolioStocks to update</param>
		/// <param name="shareInputs">The shares of the PortfolioStocks to update</param>
		/// <param name="dateInputs">The dates of the PortfolioStocks to update</param>
		/// <returns>The updated _PortfolioRowsAndStocksModel</returns>
		public async Task<IActionResult> OnPostUpdatePortfolioStocks(string[] idInputs, string[] priceInputs, string[] shareInputs, string[] dateInputs)
        {
			PartialViewResult result = null;

			if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				UserId = new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

				decimal[] prices = null;
				int[] shares = null;
				DateTime[] dates = null;
				Guid[] ids = null;

				// Validate the inputs, store results in variables
				if (ValidateIds(idInputs, out ids) && ValidatePrices(priceInputs, out prices) && ValidateShares(shareInputs, out shares) && ValidateDates(dateInputs, out dates))
				{

					// Make changes to the PortfolioStocks
					List<PortfolioStock> portfolioStocks = db.PortfolioStocks.Where(ps => ids.Contains(ps.PortfolioStockId) && ps.UserId == UserId.Value).ToList();
					for (int i = 0; i < portfolioStocks.Count(); i++)
					{
						portfolioStocks[i].Price = prices[i];
						portfolioStocks[i].Shares = shares[i];
						portfolioStocks[i].Date = dates[i].Date;
					}

					// Setup removal of PortfolioStocks that have 0 shares
					List<PortfolioStock> toRemove = new List<PortfolioStock>();
					for (int i = 0; i < portfolioStocks.Count(); i++)
					{
						if (portfolioStocks[i].Shares == 0)
						{
							toRemove.Add(portfolioStocks[i]);
						}
					}

					for (int i = 0; i < toRemove.Count(); i++)
					{
						if (portfolioStocks.Contains(toRemove[i]))
						{
							portfolioStocks.Remove(toRemove[i]);
						}
					}

					// Update db
					db.PortfolioStocks.RemoveRange(toRemove);
					PeopleTracking[] tracking = db.PeopleTracking.Where(pt => (toRemove.Select(tr => tr.StockId)).ToArray().Contains(pt.StockId)).ToArray();
					db.PeopleTracking.RemoveRange(tracking);
                    db.PortfolioStocks.UpdateRange(portfolioStocks);
					await db.SaveChangesAsync();

					// Set Portfolio model
					SetStocks();
					SetupChart();

					if (portfolioStocks.Count() != 0)
					{
						// Create the new _PortfolioRowsAndStocksModel with updated values
						_PortfolioRowsAndStocksModel model = new _PortfolioRowsAndStocksModel(PortfolioRows.ToArray(), Stocks.ToArray(), PortfolioStocks.ToArray(), "Your portfolio has been updated");

						ViewDataDictionary viewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { { "_PortfolioRowsAndStocks", model } };
						viewData.Model = model;

						result = new PartialViewResult()
						{
							ViewName = "_PortfolioRowsAndStocks",
							ViewData = viewData,
						};
					}
				}
				else
				{
					_StockTableModel model = new _StockTableModel("The data you input is invalid");

					ViewDataDictionary viewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { { "_StockTable", model } };
					viewData.Model = model;

					result = new PartialViewResult()
					{
						ViewName = "_StockTable",
						ViewData = viewData,
					};

				}
			}

			

			return result;

        }


		/// <summary>
		/// Deletes the user stock specified by stockName
		/// </summary>
		/// <param name="stockName">The stock to delete</param>
		/// <returns>The updated _PortfolioRowsAndStocksModel</returns>
		public async Task<IActionResult> OnPostDeleteStock(string stockName)
        {
			// The result to return
			PartialViewResult result = null;

			if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				UserId = new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

				// The stock specified by stockName
				Stock stock = null;

				if (Helper.ValidateStockName(stockName, out stock, db))
				{
					db.PortfolioStocks.RemoveRange(db.PortfolioStocks.Where(ps => ps.StockId == stock.StockId && ps.UserId == UserId.Value));
					db.PeopleTracking.RemoveRange(db.PeopleTracking.Where(pt => pt.StockId == stock.StockId && pt.UserId == UserId.Value));
					await db.SaveChangesAsync();



					// Re-set Portfolio model
					SetStocks();
					SetupChart();

					if (PortfolioStocks.Count() != 0)
					{
						// Create the new _PortfolioRowsAndStocksModel with updated values
						_PortfolioRowsAndStocksModel model = new _PortfolioRowsAndStocksModel(PortfolioRows.ToArray(), Stocks.ToArray(), PortfolioStocks.ToArray(), "Your portfolio has been updated");
						ViewDataDictionary viewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { { "_PortfolioRowsAndStocks", model } };
						viewData.Model = model;

						result = new PartialViewResult()
						{
							ViewName = "_PortfolioRowsAndStocks",
							ViewData = viewData,
						};
					}
				}
			}

			return result;
        }
		
		

		

		/// <summary>
		/// Validate a list of prices as decimals
		/// </summary>
		/// <param name="priceInputs">The prices to validate</param>
		/// <param name="prices">The variable the results will be stored in</param>
		/// <returns>If the prices are valid</returns>
		private bool ValidatePrices(string[] priceInputs, out decimal[] prices)
        {
			bool valid = true;
			prices = new decimal[priceInputs.Length];

			for (int i = 0; i < priceInputs.Length; i++)
            {
				decimal result;
				if (decimal.TryParse(priceInputs[i], out result))
                {
					prices[i] = result;
                }
				else
                {
					valid = false;
                }
            }

			return valid;
        }

		/// <summary>
		/// Validate a list of shares as integers
		/// </summary>
		/// <param name="shareInputs">The shares to validate</param>
		/// <param name="shares">The variable the results will be stored in</param>
		/// <returns>If the shares are valid</returns>
		private bool ValidateShares(string[] shareInputs, out int[] shares)
        {
			bool valid = true;
			shares = new int[shareInputs.Length];

			for (int i = 0; i < shareInputs.Length; i++)
            {
				int result;
				if (int.TryParse(shareInputs[i], out result))
                {
					shares[i] = result;
                } 
				else
                {
					valid = false;
                }
            }

			return valid;
        } 

		/// <summary>
		/// Validate a list of dates as dates
		/// </summary>
		/// <param name="dateInputs">The dates to validate</param>
		/// <param name="dates">The variable the results will be stored in</param>
		/// <returns>If the dates are valid</returns>
		private bool ValidateDates(string[] dateInputs, out DateTime[] dates)
        {
			bool valid = true;
			dates = new DateTime[dateInputs.Length];

			for (int i = 0; i < dateInputs.Length; i++)
            {
				DateTime result;
				if (DateTime.TryParse(dateInputs[i], out result))
				{
					dates[i] = result;
				}
				else
                {
					valid = false;
                }
            }

			return valid;
        }

		/// <summary>
		/// Validate a list of ids as guids
		/// </summary>
		/// <param name="idInputs">The guids to validate</param>
		/// <param name="ids">The variable the result will be stored in</param>
		/// <returns>If the guids are valid</returns>
		private bool ValidateIds(string[] idInputs, out Guid[] ids)
        {
			bool valid = true;
			ids = new Guid[idInputs.Length];

			for (int i = 0; i < idInputs.Length; i++)
			{
				Guid result;
				if (Guid.TryParse(idInputs[i], out result))
				{
					ids[i] = result;
				}
				else
				{
					valid = false;
				}
            }

			return valid;
        }

		
	}
}
