using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartHouseSD
{
    public partial class EmailInput : Form
    {

        public emailAddress loc_email;

        public EmailInput()
        {
            InitializeComponent();
        }

        public void OKButton_Click(object sender, EventArgs e)
        {
            if ((textBox2.Text != "") && (textBox3.Text != ""))
            {
                if (isValid() == true) // code for properly closing the form and updating the home server
                {
                    this.loc_email.address = textBox2.Text.ToLower();
                    this.loc_email.canSendCommands = commandsCheckBox.Checked;
                    this.loc_email.notifyWithUpdates = notificationsCheckBox.Checked;
                    MessageBox.Show("Your new email address has been successfully updated.");
                    this.Close();
                }
            }
            else if (textBox1.Text != "")
            {
                this.loc_email.canSendCommands = commandsCheckBox.Checked;
                this.loc_email.notifyWithUpdates = notificationsCheckBox.Checked;
                MessageBox.Show("Your new email address has been successfully updated.");
                this.Close();
            }
        }

        public void CancelButton_Click(object sender, EventArgs e)
        {
            //if (loc_email == null) MessageBox.Show("Please type in a valid email address to start.");
            //else this.Close();

            this.Close();
        }

        public void ShowDialog(ref emailAddress cur_email)
        {
            // Assign cur_email to local parameters
            textBox1.Text = cur_email.address; // uncomment if you want current email address to appear in first text box
            notificationsCheckBox.Checked = cur_email.notifyWithUpdates;
            commandsCheckBox.Checked = cur_email.canSendCommands;
            loc_email = cur_email;
            
            // Hide current email address if it is null
            if (loc_email.address == null)
            {
                label2.Visible = false;
                textBox1.Visible = false;
            }

            // Display and activate this form
            this.ShowDialog();

            // Return parameters
            cur_email = loc_email;
        }

        public bool isValid()
        {
            // Check if current email address is typed in correctly
            //Code Section pulled from MSDN

            if (textBox2.Text.ToLower() != textBox3.Text.ToLower())
            {
                MessageBox.Show("Please correctly confirm your new email address.");
                return false;
            }
            try
            {
                return Regex.IsMatch(textBox3.Text,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,24}))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                MessageBox.Show("Please type a valid email address"); 
                return false;
            }
        }

        private void EmailInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (loc_email == null)
            //{
            //    MessageBox.Show("Please type in a valid email address to start.");
            //    e.Cancel = true;
            //}
        }

        private void EmailInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
