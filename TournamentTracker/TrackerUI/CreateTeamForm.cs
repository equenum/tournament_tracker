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

        /// <summary>
        /// Initializes CreateTeamForm.
        /// </summary>
        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();

            callingForm = caller;

            WireUpLists();
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

                GlobalConfig.Connection.CreatePerson(model);

                selectedTeamMembers.Add(model);
                WireUpLists();

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

        private bool ValidateAddNewMemberForm()
        {
            bool output = true;

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
