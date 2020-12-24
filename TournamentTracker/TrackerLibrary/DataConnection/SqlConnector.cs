using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrackerLibrary.Models;
using System.Data.SqlClient;
using Dapper;

namespace TrackerLibrary.DataConnection
{
    public class SqlConnector : IDataConnection
    {
        /// <summary>
        /// Saves a new prize to the database.
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the unique identifier.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // Opening SQL connection.
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                // Passing the values via dynamic parameters.
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Inserting the values to the database.
                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                // Getting back id value from the database.
                model.Id = p.Get<int>("@id");

                return model;
            }
        }

        /// <summary>
        /// Saves a new person to the database.
        /// </summary>
        /// <param name="model">The person information.</param>
        /// <returns>The person information, including the unique identifier.</returns>
        public PersonModel CreatePerson(PersonModel model)
        {
            // Opening SQL connection.
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                // Passing the values via dynamic parameters.
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddress", model.EmailAddress);
                p.Add("@CellphoneNumber", model.CellphoneNumber);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Inserting the values to the database.
                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                // Getting back id value from the database.
                model.Id = p.Get<int>("@id");

                return model;
            }
        }
    }
}
