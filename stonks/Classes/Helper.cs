using stonks.Data;
using stonks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace stonks.Classes
{
    /// <summary>
    /// A helper class.
    /// Contains methods that are useful to multiple different classes.
    /// </summary>
    public static class Helper
    {

        /// <summary>
        /// Gets the tags formatted properly to display.
        /// </summary>
        /// <returns>The formatted tags</returns>
        public static string GetFormattedTags(Tag[] tags)
        {
            string formatted = "";

            if (tags != null && tags.Length > 0)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    if (i == 0)
                    {
                        formatted += tags[i].Text;
                    }
                    else
                    {
                        formatted += ", " + tags[i].Text;
                    }
                }
            }

            return formatted;
        }

        /// <summary>
		/// Validate stockName consists of only alphabetical values
		/// Validate stockName is the name of an actual Stock in the database
		/// </summary>
		/// <param name="stockName">The stock name to validate</param>
		/// <param name="stock">The variable the result will be stored in</param>
		/// <returns>If the stockName is valid</returns>
		public static bool ValidateStockName(string stockName, out Stock stock, ApplicationDbContext db)
        {
            bool valid = true;
            stock = null;
			

			if (stockName == null || stockName == "")
            {
                valid = false;
            }
            else
            {
				Regex.Replace(stockName, @"\s+", "");
                if (stockName != "")
                {
					valid = stockName.All(c => char.IsLetter(c));
				}
                else
                {
                    valid = false;
                }
				
			}
            

            if (valid)
            {
                stock = db.Stocks.Where(s => s.Name == stockName.ToUpper()).FirstOrDefault();
            }
            if (stock == null)
            {
                valid = false;
            }

            return valid;

        }
    }
}
