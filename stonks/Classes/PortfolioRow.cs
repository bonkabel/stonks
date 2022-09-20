using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Classes
{
    /// <summary>
    /// A Row for a persons portfolio table
    /// </summary>
    public class PortfolioRow
    {

        private string ticker;

        private int shares;

        private decimal totalPricePurchase;

        private decimal totalPriceCurrent;

        private decimal percentPortfolio;

        public string Ticker { get => ticker; set => ticker = value; }
        public int Shares { get => shares; set => shares = value; }
        public decimal AveragePricePurchase { get => TotalPricePurchase / Shares; }
        public decimal AveragePriceCurrent { get => totalPriceCurrent / Shares; }
        public decimal TotalPricePurchase { get => totalPricePurchase; set => totalPricePurchase = value; }
        public decimal TotalPriceCurrent { get => totalPriceCurrent; set => totalPriceCurrent = value; }
        public decimal PercentPortfolio { get => percentPortfolio; set => percentPortfolio = value; }


        public decimal PercentReturn { get => (totalPriceCurrent - totalPricePurchase) / totalPricePurchase * 100; }

        public PortfolioRow(string ticker, int shares, decimal totalPricePurchase, decimal totalPriceCurrent)
        {
            this.Ticker = ticker;
            this.Shares = shares;
            this.TotalPricePurchase = totalPricePurchase;
            this.TotalPriceCurrent = totalPriceCurrent;
        }

        public PortfolioRow()
        {

        }



    }
}
