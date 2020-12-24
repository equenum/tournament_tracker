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
        public CreateTeamForm()
        {
            InitializeComponent();
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

        private void createTeamButton_Click(object sender, EventArgs e)
        {

        }

        /*
            Add New Member Form Section
            Contains all methods for Add New Member Form processing. 
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
                GlobalConfig.Connection.CreatePerson(model);

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

        private bool ValidateAddNewMemberForm()
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

            // TODO - Make proper validation.
            // firstName
            //laastName
            //email
            //cellPhone

            return output;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {

        }
    }
}
