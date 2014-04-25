using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseSD
{
    public class emailInfo
    {
        public string toAddress;
        public string fromAddress;
        public string subject;
        public string body;
        public string rawEmail;
        private string[] lines;


        public emailInfo(string em)
        {
            rawEmail = em;
            parseRaw();
        }
        private void parseRaw()
        {
            lines = rawEmail.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            getFromAddress();
            getToAddress();
            getSubject();
            getBody();
        }

        private void getSubject()
        {
            int start;
            foreach (string line in lines)
            {
                if (line.StartsWith("Subject:"))
                {
                    start = line.IndexOf(":");
                    subject = line.Substring(start+1);
                    break;
                }
            }
        }

        private void getToAddress()
        {
            int start, stop;
            foreach (string line in lines)
            {
                if (line.StartsWith("To:"))
                {
                    start = line.IndexOf("<");
                    stop = line.IndexOf(">");
                    toAddress = line.Substring(start+ 1, stop - start -1);
                    break;
                }
            }
        }

        private void getFromAddress()
        {
            int start, stop;
            foreach (string line in lines)
            {
                if (line.StartsWith("From:"))
                {
                    start = line.IndexOf("<");
                    stop = line.IndexOf(">");
                    fromAddress = line.Substring(start+1, stop - start-1);
                    break;
                }
            }
        }

        // email body format:
        // 
        // <>
        // command
        // another command
        // yet another command
        // <>
        private void getBody()
        {
            bool rec = false;

            foreach (string line in lines)
            {
                if (line.StartsWith("<>")) // check for beginning/end of command
                {
                    if (rec == false)
                    {
                        rec = true;
                        continue; // if we aren't recording, start recording at next line.
                    }
                    if (rec == true) break; // if we are recording, we have reached the end of the recording
                }
                if (rec == true) // if we are recording, then add the current line to the body string
                {
                    body = body + line + "\r\n";
                }
            }
        }
    }
}
