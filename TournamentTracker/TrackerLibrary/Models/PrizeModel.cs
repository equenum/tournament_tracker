using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    /// <summary>
    /// Represents a tournament prize.
    /// </summary>
    public class PrizeModel
    {
        /// <summary>
        /// Represents an unique identifier for the prize.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the place number for which the prize is given.
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// Represents the place name for which the prize is given.
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Represents the value of the prize amount.
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// Represents the percentage of the prize amount.
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;

            // Place number parsing.
            int.TryParse(placeNumber, out int placeNumberValue);
            PlaceNumber = placeNumberValue;

            // Prize amount parsing.
            decimal.TryParse(prizeAmount, out decimal prizeAmountValue);
            PrizeAmount = prizeAmountValue;

            // Prize percentage parsing.
            double.TryParse(prizePercentage, out double prizePercentageValue);
            PrizePercentage = prizePercentageValue;
        }
    }
}
