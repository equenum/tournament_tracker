using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;
using TrackerLibrary.DataConnection.TextConnect;
using System.Linq;

namespace TrackerLibrary.DataConnection
{
    public class TextConnector : IDataConnection
    {
        /// <summary>
        /// Represents PrizeModel output text file name including extension.
        /// </summary>
        private const string PrizesFile = "PrizeModels.csv";

        /// <summary>
        /// Represents PrizeModel output text file name including extension.
        /// </summary>
        private const string PeopleFile = "PersonModels.csv";

        /// <summary>
        /// Saves a new prize to the text file.
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the unique identifier.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // Read and load the text file as a List<string>.
            // Convert the text to a List<PrizeModel>.
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // Finding the current highest ID.
            int currentId = 1;

            if (prizes.Count() > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1; // TODO - Examine
            }

            model.Id = currentId;

            // Add the new record with the new ID (current max + 1).
            prizes.Add(model);

            // Convert the updated List<PrizeModel> to a List<string>.
            // Save the List<string> to the text file.
            prizes.SaveToPrizeFile(PrizesFile);

            return model;
        }

        /// <summary>
        /// Saves a new person to the text file.
        /// </summary>
        /// <param name="model">The person information.</param>
        /// <returns>The person information, including the unique identifier.</returns>
        public PersonModel CreatePerson(PersonModel model)
        {
            // Read and load the text file as a List<string>.
            // Convert the text to a List<PersonModel>.
            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            // Finding the current highest ID.
            int currentId = 1;

            if (people.Count() > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            // Add the new record with the new ID (current max + 1).
            people.Add(model);

            // Convert the updated List<PersonModel> to a List<string>.
            // Save the List<string> to the text file.
            people.SaveToPersonFile(PeopleFile);

            return model;
        }
    }
}
