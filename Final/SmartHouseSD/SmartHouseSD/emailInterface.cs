using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Sockets;
using System.Net.Security;
using System.Threading;
using System.ComponentModel;
using System.Text;

namespace SmartHouseSD
{
    // Refer to this class when making all email calls for simplicity
    public class emailInterface
    {
        // Parses email to get information as needed. *Need to change for actual email parsing instead of simply deleting email*

        //JAKE THINGS
        public event EventHandler dataChange;  //event to update whenever the homeserver needs work

        public int light;  //what the email thinks that light is
        public int alarmLevel; //what the email thinks the alarmLevel is
        public static bool mailSent;
        public List<string> emails;
        public emailInfo info;

        public emailAddressListing emailAddresses;

        public emailInterface()
        {
            emailAddresses = new emailAddressListing();

            emails = new List<string>();
            Thread mailloop = new Thread(runMail);  //should have all been replaced with event handler version
            mailloop.IsBackground = true;
            mailloop.Start();
        }

        public void OnDataChange(EventArgs e)
        {  //Run this function whenever there is a true data change that the home server needs to know about
            EventHandler handler = dataChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //END JAKE THINGS

        // Checks for new email and saves entire message body to "MessageTextX.txt" for X number of new emails. IMPORTANT NOTE: Run in its own thread as this function will run forever.
        public void runMail()
        { //TODO: JAke and michael needs to fix this some hardcore shit yo
            //string CurDir = Directory.GetCurrentDirectory();
            //string FileName = CurDir + "\\MessageText1.txt"; // assumes there is only one new email since it runs forever.
            while (true)
            {
                Receive();  //get all emails and put the raws into emails list
                foreach (string email in emails)
                {

                    info = new emailInfo(email);  //parse the raw email into an emailInfo class
                    int ndx = emailAddresses.emailList.FindIndex(x => x.address == info.fromAddress);
                    if (ndx != -1)  //check if that email address is known
                    {
                        if (emailAddresses.emailList[ndx].canSendCommands)
                        {
                            getCommands(info.body);
                        }
                    }
                }
                emails.Clear();  //remove all the emails from the list
                Thread.Sleep(1000); // sleep for 1 seconds.
            }
        }

        public void getCommands(string b)
        {
            string[] lines = b.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string[] temp;
            foreach (string line in lines)
            {
                if (line.Contains(":"))  //assume that a line with a : is a command
                {
                    temp = line.Split(':');
                    //the commands we can get are Light (L), and alarmLevel (V)
                    switch (temp[0])
                    {
                        case "L":
                            light = Convert.ToInt32(temp[1]);
                            break;
                        case "V":
                            alarmLevel = Convert.ToInt32(temp[1]);
                            break;
                    }
                    OnDataChange(EventArgs.Empty);
                }
            }
        }

        public void updateAll(string emailSubject, string emailBody)
        {  //This function will send an email to all email addresses which are able to recieve updates
            foreach (emailAddress email in emailAddresses.emailList)
            {
                if (email.notifyWithUpdates)
                {
                    Send(email.address, emailSubject, emailBody);
                }
            }
        }

        // Helper function for "Send"
        public static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }

        // Function used for actually sending emails from HomeServer. 
        public static void Send(string To, string Subject, string Body, string FileName)
        {
            // Open new client and set properties.
            mailSent = false;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential("gatechsmarthouse", "SmartHouse4012");
            client.EnableSsl = true;

            // Send from Smart House email address.
            MailAddress from = new MailAddress("gatechsmarthouse@gmail.com", "Smart House", System.Text.Encoding.UTF8);

            // Send to Smart House user email address.
            MailAddress to = new MailAddress(To, "Smart House User", System.Text.Encoding.UTF8);

            // Open new mail message.
            MailMessage message = new MailMessage(from, to);
            message.Body = Body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = Subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            // If a filename was given, then attach the file.
            if (FileName != null)
            {
                if (File.Exists(FileName))
                {
                    // Create  the file attachment for this e-mail message.
                    Attachment data = new Attachment(FileName, MediaTypeNames.Application.Octet);

                    // Add time stamp information for the file.
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(FileName);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(FileName);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(FileName);

                    // Add the file attachment to this e-mail message.
                    message.Attachments.Add(data);
                }
                else
                {
                    Console.WriteLine("The file with filename : {0} does not exist. Please check the filename and try again.", FileName);
                }
            }

            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            // The userState can be any object that allows your callback  
            // method to identify this send operation. 
            // For this example, the userToken is a string constant. 
            string userState = Subject;
            client.SendAsync(message, userState);
            //Console.WriteLine("Sending message...");
            /* 
            string answer = Console.ReadLine();

            // If the user canceled the send, and mail hasn't been sent yet, 
            // then cancel the pending operation. 
            if (answer.StartsWith("c") && mailSent == false)
            {
                client.SendAsyncCancel();
            }
          
            // Clean up.
            message.Dispose();
            */
            while (mailSent == false) { }
        }

        public static void Send(string To, string Subject, string Body)
        {
            Send(To, Subject, Body, null);
        }

        // Function for receiving email. Will only run once per call and will place any new emails found into "CurDir\MessageTextX.txt" for X number of new emails. 
        public void Receive()
        {
            try
            {
                // Preare Pop Client
                Pop3MailClient client = new Pop3MailClient("pop.gmail.com", 995, true, "gatechsmarthouse@gmail.com", "SmartHouse4012");
                client.IsAutoReconnect = true;

                // Remove the following line if no tracing is needed
                client.Trace += new TraceHandler(Console.WriteLine);
                client.ReadTimeout = 60000; // give Pop server 60 seconds to answer.

                // Establish a connection
                client.Connect();

                // Get mailbox statistics
                int NumberOfMails, MailboxSize;
                client.GetMailboxStats(out NumberOfMails, out MailboxSize);

                // Get a list of mails
                List<int> EmailIds;
                client.GetEmailIdList(out EmailIds);

                /* Shouldn't be necessary from original code block
                //get a list of unique mail ids
                List<MailHelper.EmailUid> EmailUids;
                client.GetUniqueEmailIdList(out EmailUids);

                //get email size
                client.GetEmailSize(1);
                 */

                string email;
                foreach (int Txt in EmailIds)
                {
                    // Get email
                    client.GetRawEmail(Txt, out email);
                    emails.Add(email);  //add each email to the listing

                    // Delete email
                    client.DeleteEmail(Txt);
                }

                /* Code not necessary to run
                //get a list of mails
                List<int> EmailIds2;
                DemoClient.GetEmailIdList(out EmailIds2);

                //undelete all emails
                DemoClient.UndeleteAllEmails();

                //ping server
                DemoClient.NOOP();

                //test some error conditions
                //        DemoClient.GetRawEmail(1000000, out Email);
                //        DemoClient.DeleteEmail(1000000);
                */

                client.Disconnect();

            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Run Time Error Occured:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
