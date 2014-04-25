using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace SmartHouseSD
{
    public class emailAddressListing
    {
        public List<emailAddress> emailList = new List<emailAddress>();


        public emailAddressListing()
        {
            initializeEmailList();
        }

        public void initializeEmailList()
        {//reads from the email file (if it exists and populates the emailAddresses list
            emailAddress curemail = new emailAddress();

            if (File.Exists(Directory.GetCurrentDirectory() + "/emailList.txt"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader("emailList.txt"))
                    {
                        string line = sr.ReadToEnd();
                        string[] strSeparators = new string[] { "\r\n" };
                        string[] emails = line.Split(strSeparators, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < (emails.Length - 1); i = i + 3)
                        {
                            Add(emails[i], emails[i + 1], emails[i + 2]);
                        }
                        // emailListBox.DataSource = emails;

                        // for (int i = 0; i < emails.Length; i++) emailList.Add(emails[i]);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("emailList.txt could not be read.");
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                File.Create(Directory.GetCurrentDirectory() + "/emailList.txt");
                // emailListBox.DataSource = null;
            }
        }
        // Function for adding a new emailAddress with permissions to emailList
        // NOTE: This version uses strings for the canSendCommands and notifyWithUpdates
        //       variables and should mainly be used for parsing emailList.txt
        public void Add(string address, string canSendCommands, string notifyWithUpdates)
        {
            // Convert permissions to boolean types before sending to other Add function
            bool commands = Convert.ToBoolean(canSendCommands);
            bool updates = Convert.ToBoolean(notifyWithUpdates);

            // Call other Add function
            Add(address, commands, updates);
        }
        
        // Function for adding a new emailAddress with permissions to emailList
        // NOTE : This version uses boolean inputs for canSendCommands and notifyWithUpdates
        //        and should be the main version of the Add function that is used in the forms.
        public void Add(string address, bool canSendCommands, bool notifyWithUpdates)
        {
            // Local emailAddress variable
            emailAddress curEmail = new emailAddress();
            curEmail.address = address;
            curEmail.canSendCommands = canSendCommands;
            curEmail.notifyWithUpdates = notifyWithUpdates;

            // Add emailAddress to emailList
            emailList.Add(curEmail);
        }

        // Function for returning string array of all email addresses in list in
        // the order they are present in emailList and emailList.txt
        public string[] getAllEmailAddresses()
        {
            // Local string list of all email addresses
            List<string> allEmails = new List<string>();
            
            // Add each email address to the list
            foreach(emailAddress emailAddr in emailList) 
            {
                allEmails.Add(emailAddr.address);
            }

            // Return the string list as a string array using built in List functionality
            return allEmails.ToArray();
        }

        // Function for returning boolean array of permissions that define whether or not 
        // user is allowed to send commands to smart house.
        // NOTE: Each entry in the output corresponds to each entry in the string array 
        //       outputted from getAllEmailAddresses.
        public bool[] getAllCommandPermissions()
        {
            // Local boolean list of all email addresses
            List<bool> allEmails = new List<bool>();

            // Add each send permission to the list
            foreach (emailAddress emailAddr in emailList)
            {
                allEmails.Add(emailAddr.canSendCommands);
            }

            // Return the boolean list as a boolean array using built in List functionality
            return allEmails.ToArray();
        }

        // Function for returning boolean array of permissions that define whether or not 
        // user is allowed to receive updates from smart house.
        // NOTE: Each entry in the output corresponds to each entry in the string array 
        //       outputted from getAllEmailAddresses.
        public bool[] getAllUpdatePermissions()
        {
            // Local boolean list of all email addresses
            List<bool> allEmails = new List<bool>();

            // Add each notify permission to the list
            foreach (emailAddress emailAddr in emailList)
            {
                allEmails.Add(emailAddr.notifyWithUpdates);
            }

            // Return the boolean list as a boolean array using built in List functionality
            return allEmails.ToArray();
        }

        // Function for returning string array of all information about email addresses.
        // This function is mainly to be used for writing back to emailList.txt.
        // NOTE: The output is of the following form for n email addresses:
        // 
        //      out = emailList.ToArray(); // out = {address1, command1, update1, 
        //                                          ..., addressn, commandn, updaten} 
        public string[] ToArray()
        {
            // Local string list of all email addresses
            List<string> output = new List<string>();

            // Add all information about each email address in order to the list
            foreach (emailAddress emailAddr in emailList)
            {
                output.Add(emailAddr.address);
                output.Add(emailAddr.canSendCommands.ToString());
                output.Add(emailAddr.notifyWithUpdates.ToString());
            }

            // Return the string list as a string array using built in List functionality
            return output.ToArray();
        }

        // Function for removing the 'i'th emailAddress from your emailList
        public void RemoveAt(int i)
        {
            emailList.RemoveAt(i);
        }
    }
}
