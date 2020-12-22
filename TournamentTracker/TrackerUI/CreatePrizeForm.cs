using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.DataConnection;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        public CreatePrizeForm()
        {
            InitializeComponent();
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PrizeModel model = new PrizeModel(
                    placeNameValue.Text, 
                    placeNumberValue.Text, 
                    prizeAmountValue.Text, 
                    prizePercentageValue.Text);

                // Saving the prize to the databases.
                GlobalConfig.Connection.CreatePrize(model);

                // Wiping out previous form input values.
                placeNameValue.Text = "";
                placeNumberValue.Text = "";
                prizeAmountValue.Text = "0";
                prizePercentageValue.Text = "0";
            }
            else
            {
                MessageBox.Show("This form has invalid information. Please check it and try again.");
            }
        }

        private bool ValidateForm()
        {
            bool output = true;

            // Place number textbox input validation.
            bool placeNumberIsValidNumber = int.TryParse(placeNumberValue.Text, out int placeNumber);

            if (placeNumberIsValidNumber == false)
            {
                output = false;
            }

            if (placeNumber < 1)
            {
                output = false;
            }

            // Place name textbox input validation.
            if (placeNameValue.Text.Length == 0)
            {
                output = false;
            }

            // Prize amount and Prize percentage textbox input validation.
            bool prizeAmountIsValid = decimal.TryParse(prizeAmountValue.Text, out decimal prizeAmount);
            bool prizePercentageIsValid = double.TryParse(prizePercentageValue.Text, out double prizePercentage);

            if (prizeAmountIsValid == false || prizePercentageIsValid == false)
            {
                output = false;
            }

            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                output = false;
            }

            // Prize percentage range validation.
            if (prizePercentage > 100 || prizePercentage < 0)
            {
                output = false;
            }

            return output;
        }
    }
}
