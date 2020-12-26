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

                output.Add(p);
            }

            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModels(
            this List<string> lines, 
            string teamFileName, 
            string peopleFileName, 
            string prizesFileName)
        {
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);
            List<PrizeModel> prizes = prizesFileName.FullFilePath().LoadFile().ConvertToPrizeModels();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(columns[0]);
                tm.TournamentName = columns[1];
                tm.EntryFee = decimal.Parse(columns[2]);

                string[] teamIds = columns[3].Split('|');
                
                foreach (string id in teamIds)
                {
                    tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());
                }

                string[] prizeIds = columns[4].Split('|');

                foreach (string id in prizeIds)
                {
                    tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(id)).First());
                }

                // TODO - Capture rounds information.
                output.Add(tm);
            }

            return output;
        }

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

        public static void SaveToTournamentsFile(this List<TournamentModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel tm in models)
            {
                // TODO - Make it lines.Add( $@"{ tm.Id }," +  if it doesnt work. @
                lines.Add(
                        $"{ tm.Id }," +
                        $"{ tm.TournamentName }," +
                        $"{ tm.EntryFee }," +
                        $"{ ConvertTeamListToString(tm.EnteredTeams) }," +
                        $"{ ConvertPrizeListToString(tm.Prizes) }," +
                        $"{ ConvertRoundListToString(tm.Rounds) }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        private static string ConvertRoundListToString(List<List<MatchupModel>> rounds)
        {
            string output = "";

            if (rounds.Count == 0)
            {
                return "";
            }

            foreach (List<MatchupModel> r in rounds)
            {
                output += $"{ ConvertMatchupListToString(r) }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            foreach (MatchupModel m in matchups)
            {
                output += $"{ m.Id }^";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertPrizeListToString(List<PrizeModel> prizes)
        {
            string output = "";

            if (prizes.Count == 0)
            {
                return "";
            }

            foreach (PrizeModel p in prizes)
            {
                output += $"{ p.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count == 0)
            {
                return "";
            }

            foreach (TeamModel t in teams)
            {
                output += $"{ t.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
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
