using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace SmartHouseSD
{
    public partial class ControlPanelForm : Form
    {

        public delegate void guiCallback();  //delegate for handling GUI updates from home server
        public guiCallback g;
        //declaring global variables
        public homeServer home;
        public cameraForm cam1form;
        public cameraForm cam2form;

        public ControlPanelForm()
        {
            InitializeComponent();
            home = new homeServer("COM3");  //TODO find the com port
            home.dataChange += updateFormHandler;

            cam1form = null;
            cam2form = null;
        }

        private void updateFormHandler(object sender, EventArgs e)
        {
            g = new guiCallback(updateForm);
            this.Invoke(this.g);
        }

        private void updateForm()
        {
            // Debug information outputted to text box
            textBox1.Text = home.getAllDataString();

            // Information to change text of lamp button
            switch (home.ard.light.val)
            {
                case 0:
                    lampButton.Text = "Turn Lamp On";
                    break;
                case 1:
                    lampButton.Text = "Turn Lamp Off";
                    break;
            }

            // Information to change text of "arm system" button and selection of combo box
            comboBox1.SelectedIndex = home.alarmLevel;

            // Information to change written temperature of the house
            label2.Text = "Current Temperature: " + home.ard.temper.val.ToString() + "°F";
        }

        // Lamp
        private void button1_Click(object sender, EventArgs e)
        {
            if (home.ard.light.val == 1)
            {
                // Add code to turn lamp off
                home.ard.send(home.ard.light.key, "0");
                //home.ard.send("F", "0");
                //button1.Text = "Turn Lamp Off"; // Shouldn't be necessary anymore
                //TODO: tell joe to always send a status change back whenever it makes a change
            }
            else
            {
                // Add code to turn lamp on
                home.ard.send(home.ard.light.key, "1");
                //home.ard.send("F", "0");
                //button1.Text = "Turn Lamp On"; // Shouldn't be necessary anymore
            }
        }

        // Arm/Disarm Security System
        //private void button5_Click(object sender, EventArgs e)
        //{
        //    //TODO require password
        //    if (button5.Text == "Arm System")
        //    {
        //        home.setAlarmLevel(comboBox1.SelectedIndex);
        //        button5.Text = "Disarm System";
        //    }
        //    else if (button5.Text == "Disarm System")
        //    {
        //        home.setAlarmLevel(0);
        //        button5.Text = "Arm System";
        //    }
        //}

        // Update Email Address
        private void button9_Click(object sender, EventArgs e)
        {
            EmailListForm emailList = new EmailListForm(this);
            emailList.ShowDialog();
        }

        // "Escape" key to close form
        private void SmartHouseControlPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        // View Camera 1 Button
        private void button10_Click(object sender, EventArgs e)
        {
            if (cam1form == null)
            {
                cam1form = new cameraForm(this, ref home.cam1);
                cam1form.Show();
            }
            else cam1form.Activate();
        }

        // View Camera 2 Button
        private void button11_Click(object sender, EventArgs e)
        {
            if (cam2form == null)
            {
                cam2form = new cameraForm(this, ref home.cam2);
                cam2form.Show();
            }
            else cam2form.Activate();
        }

        // Activity level changed
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if the system is already armed, update the expected activity level
            if (comboBox1.SelectedIndex != home.alarmLevel) // TODO: may need to check that update isn't being made to activity level from home server so same change isn't being reported multiple times. This may be fixed by checking that form activity level doesn't equal home server activity level.
            {
                home.setAlarmLevel(comboBox1.SelectedIndex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            home.ard.send(home.ard.fullStat.key, "1");
        }

        private void DoorLockButton_Click(object sender, EventArgs e)
        {
            // TODO: Jake insert code to check door lock
            home.cam1.checkDoorLock();
        }

        private void EmailButton_Click(object sender, EventArgs e)
        {
            home.email.updateAll("Email Debug Button was clicked at " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), "Current Status \n\n" + home.getAllDataString());
        }
    }
}
