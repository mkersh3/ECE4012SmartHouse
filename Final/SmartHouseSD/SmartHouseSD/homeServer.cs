using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Threading;


namespace SmartHouseSD
{
    public class homeServer
    {
        public arduino ard;
        public camera cam1, cam2;
        public webInterface web;
        public emailInterface email;

        public event EventHandler dataChange;  //create an event handler for when data changes

        private bool alert;  //whether or not there is currently an alert, this will remain set until the user is confirmed to be aware of the alert
        public bool isDoorLocked;  //whether or not the door is locked

        public int expectedActivityLevel;  //activity leveled tied with the alarm level
        public int alarmLevel;  //what level the user has set to want to be notified about  (user set from control panel, web, or email)
        public bool canAlert; //for keeping track of if we can create another alert

        System.Timers.Timer alertTimer;  //function for whether or not another alert can happen

        public homeServer(string port)
        //public homeServer()
        {
            string temp = port;
            //emailAddress = null;
            alarmLevel = 0;
            canAlert = true;
            ard = new arduino(port);
            cam1 = new camera(1, "Camera 1");
            cam2 = new camera(0, "Camera 2");
            web = new webInterface("sensor_data.xml", ref isDoorLocked, ref ard.sensors);
            email = new emailInterface();

            ard.dataChange += changeFromArduino;
            web.dataChange += changeFromWeb;
            email.dataChange += changeFromEmail;
            cam1.dataChange += changeFromCamera;  //May need to be cam2, depending on which one is facing the door

            //should be able to get rid of this, or at least part of it
            /*
            Thread serverThread = new Thread(homeServerloop);
            serverThread.IsBackground = true;
            serverThread.Start();
             */
        }



        public string getAllDataString()
        { //returns the status of all sensors as a formatted string

            string s = "DoorLock:" + Convert.ToInt32(isDoorLocked).ToString() + "\r\n";  //TODO: add message about door lock
            s = s + ard.getAllDataString() + "\r\n";
            return s;

        }

        private void lowAlert()
        { //what to do upon a low alert level
            MessageBox.Show("Low alert");
            cam1.takeVideo("Low Alert" + DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss") + ".avi", 30);
            cam2.takeVideo("Low Alert" + DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss") + ".avi", 30);
            email.updateAll("Low Alert from SmartHouse", "Current Status \n\n" + getAllDataString() + "\n\n" + "A 30 second video has been recorded");  //Need to test it later
        }

        private void mediumAlert()
        {  //what to do upon a medium alert level
            MessageBox.Show("medium alert");
            cam1.takeVideo("Medium Alert" + DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss") + ".avi", 30);
            cam2.takeVideo("Medium Alert" + DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss") + ".avi", 30);
            email.updateAll("Medium Alert from SmartHouse", "Current Status \n\n" + getAllDataString() + "\n\n" + "A 30 second video has been recorded");
        }

        private void highAlert()
        {  //what to do upon a high alert level
            MessageBox.Show("high alert");
            cam1.takeVideo("High Alert" + DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss") + ".avi", 30);
            cam2.takeVideo("High Alert" + DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss") + ".avi", 30);
            email.updateAll("High Alert from SmartHouse", "Current Status \n\n" + getAllDataString() + "\n\n" + "A 30 second video has been recorded");
        }

        public void setAlarmLevel(int lev)  //Run whenever a change has been made to the alarm level
        {
            alarmLevel = lev;
            ard.alert.val = lev;
            web.alarmLevel = lev;
            email.alarmLevel = lev;
            switch (lev)
            {
                case 0:  //alarm is off
                    expectedActivityLevel = 2222; //essentially infinite
                    ard.send(ard.actLevel.key, expectedActivityLevel);
                    //Thread.Sleep(30);
                    ard.send(ard.armed.key, "0");
                    break;
                case 1:  //low activity
                    expectedActivityLevel = 100; //TODO: needs tuning
                    ard.send(ard.actLevel.key, expectedActivityLevel);
                    //Thread.Sleep(30);
                    ard.send(ard.armed.key, "1");
                    break;
                case 2:  //medium
                    expectedActivityLevel = 400;
                    ard.send(ard.actLevel.key, expectedActivityLevel);
                    //Thread.Sleep(30);
                    ard.send(ard.armed.key, "1");
                    break;
                case 3:  //Essentially off
                    expectedActivityLevel = 2222;
                    ard.send(ard.actLevel.key, expectedActivityLevel);
                    //Thread.Sleep(30);
                    ard.send(ard.armed.key, "1");
                    break;
                default:
                    MessageBox.Show("Alert Level not valid: Choose 0, 1, 2, or 3");
                    break;
            }
            ard.send(ard.fullStat.key, "1");
        }
        public void OnDataChange(EventArgs e)
        {  //Run this function whenever there is a true data change that the Control Panel needs to know about
            EventHandler handler = dataChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //TODO:  add setArmed into all the functions
        private void changeFromArduino(object sender, EventArgs e)
        {  //need a full status update to the UI, and check if an alarm has been set

            if (ard.trigger.val != 0)
            {
                if (canAlert)
                {
                    canAlert = false;
                    alertTimer = new System.Timers.Timer(180 * 1000);  //set a three minute timer
                    alertTimer.Elapsed += new ElapsedEventHandler(endAlertTimer);
                    alertTimer.Enabled = true;
                    alert = true;
                    MessageBox.Show("ALERT");
                    switch (alarmLevel)
                    {
                        case 0: // alarm is off
                            break;
                        case 1:
                            alert = true;
                            lowAlert();
                            break;
                        case 2:
                            alert = true;
                            mediumAlert();
                            break;
                        case 3:
                            alert = true;
                            highAlert();
                            break;
                    }
                }
            }
            OnDataChange(EventArgs.Empty);  //Inform the control panel that there has been a change
            //update the other interfaces expectations
            web.light = ard.light.val;
            email.light = ard.light.val;
            web.updateWeb();
        }

        public void endAlertTimer(object source, ElapsedEventArgs e)
        {  //Event handler for whenever the requested video length has been reached
            canAlert = true;
            alert = false;
            alertTimer.Enabled = false;
        }

        private void changeFromWeb(object sender, EventArgs e)
        {
            //check for changed alarm level
            //Can update light, alarmLevel, and something else that Jake can't remember
            //ard.light.val = web.light;  //this should be updated from the change request that the arduino returns
            setAlarmLevel(web.alarmLevel);
            ard.send(ard.light.key, web.light);
            
            //update the other interfaces expectations

            //send info back to GUI
            OnDataChange(EventArgs.Empty);
        }

        private void changeFromEmail(object sender, EventArgs e)
        {
            //check for changed alarm level
            //ard.light.val = email.light;  //this should be updated from the change request that the arduino returns
            ard.send(ard.light.key, email.light);
            setAlarmLevel(email.alarmLevel);
            web.updateWeb();
            //update other interfaces expectations

            //send info back to GUI
            OnDataChange(EventArgs.Empty);
        }

        private void changeFromCamera(object sender, EventArgs e)
        {
            isDoorLocked = cam1.isDoorLocked;  //will need to be changed if cam2 is facing the door
            web.isDoorLocked = isDoorLocked;
            web.updateWeb();

            //send info back to GUI
            OnDataChange(EventArgs.Empty);
        }
    }


}
