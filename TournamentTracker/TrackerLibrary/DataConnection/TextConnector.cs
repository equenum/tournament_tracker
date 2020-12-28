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
        // TODO - Probably delete this parameters later during refactoring.

        /// <summary>
        /// Represents PrizeModel output text file name including extension.
        /// </summary>
        private const string PrizesFile = "PrizeModels.csv";

        /// <summary>
        /// Represents PrizeModel output text file name including extension.
        /// </summary>
        private const string PeopleFile = "PersonModels.csv";

        /// <summary>
        /// Represents TeameModel output text file name including extension.
        /// </summary>
        private const string TeamsFile = "TeamModels.csv";

        /// <summary>
        /// Represents TournamentModel output text file name including extension.
        /// </summary>
        private const string TournamentsFile = "TournamentModel.csv";

        /// <summary>
        /// Represents Matchup output text file name including extension.
        /// </summary>
        private const string MatchupFile = "MatchupModel.csv";

        /// <summary>
        /// Represents MatchupEntry output text file name including extension.
        /// </summary>
        private const string MatchupEntryFile = "MatchupEntryModel.csv";

        /// <summary>
        /// Saves a new prize to the text file.
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the unique identifier.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            int currentId = 1;

            if (prizes.Count() > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1; // TODO - Examine how it works.
            }

            model.Id = currentId;

            prizes.Add(model);

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
            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int currentId = 1;

            if (people.Count() > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            people.Add(model);

            people.SaveToPersonFile(PeopleFile);

            return model;
        }

        /// <summary>
        /// Saves a new team to the text file.
        /// </summary>
        /// <param name="model">The team information.</param>
        /// <returns>The team information, including the unique identifier.</returns>
        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = TeamsFile.FullFilePath().LoadFile().ConvertToTeamModels(PeopleFile);

            int currentId = 1;

            if (teams.Count() > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            teams.Add(model);

            teams.SaveToTeamFile(TeamsFile);

            return model;
        }

        // TODO - write a full xml comment later. This method saves a lot of things. 
        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = TournamentsFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels(TeamsFile, PeopleFile, PrizesFile);

            int currentId = 1;

            if (tournaments.Count() > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            model.SaveRoundsToFile(MatchupFile, MatchupEntryFile);

            tournaments.Add(model);

            tournaments.SaveToTournamentsFile(TournamentsFile);
        }

        /// <summary>
        /// Gets all people information from the text-file.
        /// </summary>
        /// <returns>A list of people information.</returns>
        public List<PersonModel> GetPerson_All()
        {
            return PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public List<TeamModel> GetTeam_All()
        {
            return TeamsFile.FullFilePath().LoadFile().ConvertToTeamModels(PeopleFile);
        }

        public List<TournamentModel> GetTournament_All()
        {
            List<TournamentModel> tournaments = TournamentsFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels(TeamsFile, PeopleFile, PrizesFile);

            return tournaments;
        }
    }
}
