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
