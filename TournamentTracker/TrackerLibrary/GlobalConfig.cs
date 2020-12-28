using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TrackerLibrary.DataConnection;

namespace TrackerLibrary
{
    /// <summary>
    /// Holds globally accessible configuration parameters.
    /// </summary>
    public static class GlobalConfig
    {
        /// <summary>
        /// Represents PrizeModel output text file name including extension.
        /// </summary>
        public const string PrizesFile = "PrizeModels.csv";

        /// <summary>
        /// Represents PrizeModel output text file name including extension.
        /// </summary>
        public const string PeopleFile = "PersonModels.csv";

        /// <summary>
        /// Represents TeameModel output text file name including extension.
        /// </summary>
        public const string TeamsFile = "TeamModels.csv";

        /// <summary>
        /// Represents TournamentModel output text file name including extension.
        /// </summary>
        public const string TournamentsFile = "TournamentModel.csv";

        /// <summary>
        /// Represents Matchup output text file name including extension.
        /// </summary>
        public const string MatchupFile = "MatchupModel.csv";

        /// <summary>
        /// Represents MatchupEntry output text file name including extension.
        /// </summary>
        public const string MatchupEntryFile = "MatchupEntryModel.csv";


        /// <summary>
        /// Represents selected database connection type.
        /// </summary>
        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections(DatabaseType db)
        {
            if (db == DatabaseType.Sql)
            {
                // TODO - Set up the SQL connector properly.
                SqlConnector sql = new SqlConnector();
                Connection = sql;
            }
            else if (db == DatabaseType.TextFile)
            {
                // TODO - Set up the text connector properly.
                TextConnector text = new TextConnector();
                Connection = text;
            }
        }

        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
