using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    /// <summary>
    /// Represents one team in a tournament.
    /// </summary>
    public class TeamModel
    {
        /// <summary>
        /// Represents a list of persons who are a part of the team.
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();

        /// <summary>
        /// Represents the team name.
        /// </summary>
        public string TeamName { get; set; }
    }
}