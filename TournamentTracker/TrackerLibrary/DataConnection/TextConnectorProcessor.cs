using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataConnection.TextConnect
{
    public static class TextConnectorProcessor
    {
        /// <summary>
        /// Gets text-file absolute location path. 
        /// </summary>
        /// <param name="fileName">Text-file name (including extension).</param>
        /// <returns>Text-file absolute location path.</returns>
        public static string FullFilePath(this string fileName)
        {
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName }";
        }

        /// <summary>
        /// Loads all text information from a given text-file.
        /// </summary>
        /// <param name="file">Text-file absolute location path.</param>
        /// <returns>All text information from the text-file.</returns>
        public static List<string> LoadFile(this string file)
        {
            if (File.Exists(file) == false)
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        /*
            Convertion section.
            Contains methods that are related to [Class]Model convertion 
            from List<string> to List<[Class]Model>.
        */

        /// <summary>
        /// Converts text input to PrizeModel objects.
        /// </summary>
        /// <param name="lines">Text input.</param>
        /// <returns>A list of PrizeModel objects.</returns>
        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(columns[0]);
                p.PlaceNumber = int.Parse(columns[1]);
                p.PlaceName = columns[2];
                p.PrizeAmount = decimal.Parse(columns[3]);
                p.PrizePercentage = double.Parse(columns[4]);

                output.Add(p);
            }

            return output;
        }

        /// <summary>
        /// Converts text input to PersonModel objects.
        /// </summary>
        /// <param name="lines">Text input.</param>
        /// <returns>A list of PersonModel objects.</returns>
        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                PersonModel p = new PersonModel();
                p.Id = int.Parse(columns[0]);
                p.FirstName = columns[1];
                p.LastName = columns[2];
                p.EmailAddress = columns[3];
                p.CellphoneNumber = columns[4];

                output.Add(p);
            }

            return output;
        }

        /// <summary>
        /// Converts text input to TeamModel objects.
        /// </summary>
        /// <param name="lines">Text input.</param>
        /// <returns>A list of TextModel objects.</returns>
        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            List<TeamModel> output = new List<TeamModel>();

            // Taking people information out of PeopleModels text-file.
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                TeamModel p = new TeamModel();
                p.Id = int.Parse(columns[0]);
                p.TeamName = columns[1];

                string[] personIds = columns[2].Split('|');

                foreach (string id in personIds)
                {
                    p.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                }

                output.Add(p); // TODO - Did Tim COrey made a mistake here?
            }

            return output;
        }

        /*
            Output saving section.
            Contains methods that are related to [Class]Model file output processing.
        */

        /// <summary>
        /// Saves prize information to the specified text-file. 
        /// </summary>
        /// <param name="models">Output prize information to be saved.</param>
        /// <param name="fileName">Output text-file name (including extension).</param>
        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add(
                    $"{ p.Id }," +
                    $"{ p.PlaceNumber }," +
                    $"{ p.PlaceName }," +
                    $"{ p.PrizeAmount }," +
                    $"{ p.PrizePercentage }"
                    );
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        /// <summary>
        /// Saves person information to the specified text-file. 
        /// </summary>
        /// <param name="models">Output person information to be saved.</param>
        /// <param name="fileName">Output text-file name (including extension).</param>
        public static void SaveToPersonFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add(
                    $"{ p.Id }," +
                    $"{ p.FirstName }," +
                    $"{ p.LastName }," +
                    $"{ p.EmailAddress }," +
                    $"{ p.CellphoneNumber }"
                    );
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        /// <summary>
        /// Saves team information to the specified text-file. 
        /// </summary>
        /// <param name="models">Output team information to be saved.</param>
        /// <param name="fileName">Output text-file name (including extension).</param>
        public static void SaveToTeamFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel p in models)
            {
                lines.Add(
                    $"{ p.Id }," +
                    $"{ p.TeamName }," +
                    $"{ ConvertPeopleListToString(p.TeamMembers)}"
                    );
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";

            if (people.Count == 0) 
            {
                return "";
            }      

            foreach (PersonModel p in people)
            {
                output += $"{ p.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }
    }
}
