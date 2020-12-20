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
    }
}
