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
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();
        private ITeamRequester callingForm;

        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();

            callingForm = caller;

            WireUpLists();
        }

        // This method is used just for testing purposes. 
        private void CreateSampleData()
        {
            availableTeamMembers.Add(new PersonModel("Mike", "Rose", "TestEmail", "TestCellPhone"));
            availableTeamMembers.Add(new PersonModel("Helen", "Storm", "TestEmail", "TestCellPhone"));

            selectedTeamMembers.Add(new PersonModel("Regina", "Mayers", "TestEmail", "TestCellPhone"));
            selectedTeamMembers.Add(new PersonModel("Oliver", "Cage", "TestEmail", "TestCellPhone"));
        }

        private void WireUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;
            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void CreateTeamForm_Load(object sender, EventArgs e)
        {

        }

        private void headerLabel_Click(object sender, EventArgs e)
        {

        }

        private void tournamentNameValue_TextChanged(object sender, EventArgs e)
        {

        }

        /*
            Create Team Section
            Contains all methods for creating a team. 
        */

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            if (ValidateTeamName())
            {
                if (selectedTeamMembers.Count > 0)
                {
                    TeamModel model = new TeamModel(selectedTeamMembers, teamNameValue.Text);

                    GlobalConfig.Connection.CreateTeam(model);

                    callingForm.TeamComplete(model);
                    
                    this.Close();
                }
                else
                {
                    MessageBox.Show("There should be at least 1 team member selected " +
                        "in order to create a team!");
                }
            }
            else
            {
                MessageBox.Show("Team Name field is empty!");
            }
        }

        private bool ValidateTeamName()
        {
            bool output = true;

            if (teamNameValue.Text.Length == 0)
            {
                output = false;
            }

            return output;
        }

        /*
            Add New Member Section
            Contains all methods for Adding New Member. 
        */

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateAddNewMemberForm())
            {
                PersonModel model = new PersonModel(
                    firstNameValue.Text, 
                    lastNameValue.Text, 
                    emailValue.Text, 
                    cellPhoneValue.Text
                    );

                // Saving the prize to the databases.
                model = GlobalConfig.Connection.CreatePerson(model);

                // Putting the updated person (with id assigned during CreatePerson completion) 
                // to teamMember ListBox.
                selectedTeamMembers.Add(model);
                WireUpLists();

                // Wiping out previous form input values.
                firstNameValue.Text = ""; 
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellPhoneValue.Text = "";
            }
            else
            {
                MessageBox.Show("Add New Member form has invalid information or empty fields." +
                    " Please check it and try again.");
            }

        }

        private bool ValidateAddNewMemberForm() // TODO - Make a proper validation.
        {
            bool output = true;

            // Check if the form fields are ampty.
            if (firstNameValue.Text.Length == 0)
            {
                output = false;
            }

            if (lastNameValue.Text.Length == 0)
            {
                output = false;
            }

            if (emailValue.Text.Length == 0)
            {
                output = false;
            }

            if (cellPhoneValue.Text.Length == 0)
            {
                output = false;
            }

            return output;
        }

        /*
            Select and Remove Member Section
            Contains all methods for selecting existing members to teamMembersListBox and removing them from there. 
        */

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;
            
            if (p != null)
            {
                availableTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);

                WireUpLists();
            }
            else
            {
                MessageBox.Show("There is no team members available!");
            }
        }

        private void removeSelectedMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;

            if (p != null)
            {
                selectedTeamMembers.Remove(p);
                availableTeamMembers.Add(p);

                WireUpLists();
            }
            else
            {
                MessageBox.Show("There is no team member selected!");
            }
        }
    }
}
