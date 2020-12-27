using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        private List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All();
        private List<TeamModel> selectedTeams = new List<TeamModel>();
        private List<PrizeModel> seletedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();

            WireUpLists();
        }

        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null;
            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = seletedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }
        
        private void selectTeamDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem;

            if (t != null)
            {
                availableTeams.Remove(t);
                selectedTeams.Add(t);

                WireUpLists();

            }
            else
            {
                MessageBox.Show("There is no team selected!");
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            CreatePrizeForm form = new CreatePrizeForm(this);
            form.Show();
        }

        public void PrizeComplete(PrizeModel model)
        {
            seletedPrizes.Add(model);
            WireUpLists();
        }

        private void createNewLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm form = new CreateTeamForm(this);
            form.Show();
        }

        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            WireUpLists();
        }

        private void removeCelectedPlayersButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)tournamentTeamsListBox.SelectedItem;

            if (t != null)
            {
                selectedTeams.Remove(t);
                availableTeams.Add(t);

                WireUpLists();
            }
            else
            {
                MessageBox.Show("There is no team selected!");
            }
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel p = (PrizeModel)prizesListBox.SelectedItem;

            if (p != null)
            {
                seletedPrizes.Remove(p);

                WireUpLists();
            }
            else
            {
                MessageBox.Show("There is no prizes selected!");
            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            if (ValidateTournamentName() && ValidateTournamentFee())
            {
                TournamentModel tm = new TournamentModel();

                tm.TournamentName = tournamentNameValue.Text;
                tm.EntryFee = decimal.Parse(entryFeeValue.Text);
                tm.Prizes = seletedPrizes;
                tm.EnteredTeams = selectedTeams;

                TournamentLogic.CreateRounds(tm);

                GlobalConfig.Connection.CreateTournament(tm);
            }
        }

        private bool ValidateTournamentName()
        {
            bool output = true;

            if (tournamentNameValue.Text.Length == 0)
            {
                MessageBox.Show("\"Tournament Name\" field is empty!", "Invalid Tournament Name", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                output = false;
            }

            return output;
        }

        private bool ValidateTournamentFee()
        {
            bool output = true;

            if (entryFeeValue.Text.Length == 0)
            {
                output = false;
            }

            bool entryFeeValueIsDecimal = decimal.TryParse(entryFeeValue.Text, out decimal fee);

            if (entryFeeValueIsDecimal == false)
            {
                MessageBox.Show("Please enter valid \"Entry Fee\"! The value should be a decimal positive number.", 
                    "Invalid Fee", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                output = false;
            }
            else if (fee < 0)
            {
                MessageBox.Show("Please enter valid \"Entry Fee\". The value should not be less than zero!", 
                    "Invalid Fee", MessageBoxButtons.OK, MessageBoxIcon.Error);

                output = false;
            }

            return output;
        }
    }
}
