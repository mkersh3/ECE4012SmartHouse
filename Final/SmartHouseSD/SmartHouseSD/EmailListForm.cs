using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
// using System.Collections;

namespace SmartHouseSD
{
    public partial class EmailListForm : Form
    {
        private ControlPanelForm parentForm;
        public emailAddressListing emailList = new emailAddressListing();
        //public List<emailAddr> addresses = new List<emailAddr>();

        public EmailListForm(ControlPanelForm parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            getEmailList();
        }

        public void getEmailList()
        {

            emailListBox.DataSource = parentForm.home.email.emailAddresses.getAllEmailAddresses();
        //    if (File.Exists(Directory.GetCurrentDirectory() + "/emailList.txt"))
        //    {
        //        try
        //        {
        //            using (StreamReader sr = new StreamReader("emailList.txt"))
        //            {
        //                string line = sr.ReadToEnd();
        //                string[] strSeparators = new string[] {"\r\n"};
        //                string [] emails = line.Split(strSeparators, StringSplitOptions.RemoveEmptyEntries);
        //                emailListBox.DataSource = emails;

        //                for (int i = 0; i < emails.Length; i++ ) emailList.Add(emails[i]);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show("emailList.txt could not be read.");
        //            MessageBox.Show(e.Message);
        //        }
        //    }
        //    else
        //    {
        //        File.Create(Directory.GetCurrentDirectory() + "/emailList.txt");
        //        emailListBox.DataSource = null;
        //    }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            // Create new email entry for list
            emailAddress newEmail = new emailAddress();

            // create mail entry form and pass newEmail reference
            EmailInput emailInput = new EmailInput();
            emailInput.ShowDialog(ref newEmail);

            // Process new entry if newEmail != null
            if (newEmail.address != null)
            {
                parentForm.home.email.emailAddresses.Add(newEmail.address, newEmail.canSendCommands, newEmail.notifyWithUpdates);
                emailListBox.DataSource = parentForm.home.email.emailAddresses.getAllEmailAddresses();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                emailAddress selectedEmail = parentForm.home.email.emailAddresses.emailList[emailListBox.SelectedIndex];

                string message = "Are you sure you would like to delete " + '\u0022' + selectedEmail.address + '\u0022' + " from your list of approved email addresses?";
                string caption = "Confirm Email Deletion";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons);

                if (result == DialogResult.Yes)
                {
                    parentForm.home.email.emailAddresses.RemoveAt(emailListBox.SelectedIndex);
                    emailListBox.DataSource = parentForm.home.email.emailAddresses.getAllEmailAddresses();
                }
            }
            catch
            {
                MessageBox.Show("There are no emails to delete!");
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Create new email entry for list
                emailAddress curEmail = parentForm.home.email.emailAddresses.emailList[emailListBox.SelectedIndex];
                emailAddress newEmail = curEmail;

                // create mail entry form and pass newEmail reference
                EmailInput emailInput = new EmailInput();
                emailInput.ShowDialog(ref newEmail);

                // Process new entry if newEmail != curEmail
                if ((newEmail.address != curEmail.address) ||  (newEmail.canSendCommands != curEmail.canSendCommands) || (newEmail.notifyWithUpdates != curEmail.notifyWithUpdates))
                {
                    //string message = "Are you sure you would like to edit " + curEmail + " to " + newEmail + "?";
                    //string caption = "Confirm Email Edit";
                    //MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    //DialogResult result;

                    //result = MessageBox.Show(message, caption, buttons);

                    //if (result == DialogResult.Yes)
                    //{

                    //emailList.emailList[emailListBox.SelectedIndex] = newEmail;
                    parentForm.home.email.emailAddresses.emailList[emailListBox.SelectedIndex] = newEmail;
                    emailListBox.DataSource = parentForm.home.email.emailAddresses.getAllEmailAddresses();
                    //}
                }
            }
            catch
            {
                MessageBox.Show("There are no emails to edit!");
            }
        }

        private void EmailList_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save list to text file

            string[] finalEmails = parentForm.home.email.emailAddresses.ToArray();
            string curdir = Directory.GetCurrentDirectory() + "\\emailList.txt";
            File.WriteAllLines(curdir, finalEmails);
        }

        //public void ShowDialog(ref emailAddressList curEmails)
        //{
        //    // Assign curEmails to local list
        //    emailList = curEmails;
            
        //    // Populate local emails into listBox
        //    getEmailList();

        //    // Display and activate this form
        //    this.ShowDialog();

        //    // Return parameters
        //    curEmails = emailList;
        //}
    }
}
