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
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace SmartHouseSD
{
    public partial class cameraForm : Form
    {
        private ControlPanelForm parentForm;
        private camera cam;

        public cameraForm(ControlPanelForm parentForm, ref camera cam)
        {
            InitializeComponent();
            
            // Store pointer to Smart House Control Panel
            // for addressing/accessing cameras
            this.parentForm = parentForm;

            // Inform this form which camera it should look at
            this.cam = cam;

            // Change title of form to be camera specific
            this.Text = this.Text + cam.name;

            // Set feed to fit image to imageBox
            imageBox1.SizeMode = PictureBoxSizeMode.Zoom;

            Application.Idle += getCam;
        }

        private void getCam(object sender, EventArgs arg)
        {
            //Image<Bgr, Byte> myFrame;
            //if (camnum == 1) myFrame = parentForm.home.cam1._cameraCapture.QueryFrame();
            //else myFrame = parentForm.home.cam2._cameraCapture.QueryFrame();
            imageBox1.Image = cam._cameraCapture.QueryFrame();
        }

        private void snapshotButton_Click(object sender, EventArgs e)
        {
            // Add code to take snapshot
            //MICHAEL TODO  // add a new folder for pictures and a new folder for videos that these are stored to
            cam.takeFrame( DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss") + ".png");
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            if (recordButton.Text == "Record")
            {
                recordButton.Text = "Stop";
                recordButton.Image = global::SmartHouseSD.Properties.Resources.stop_icon_resized;

                cam.startVideo(DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss") + ".avi");

                // Add code to start camera recording but not stop until user wants to stop
            }
            else if (recordButton.Text == "Stop")
            {
                recordButton.Text = "Record";
                recordButton.Image = global::SmartHouseSD.Properties.Resources.record_icon_resized;
                cam.stopVideo();
                // Add code to stop camera recording
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cameraForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (cam.isRecording)
            {
                cam.stopVideo();
            }
            if (cam.name.Contains('1'))
            {
                parentForm.cam1form = null;
            }
            else
            {
                parentForm.cam2form = null;
            }
        }
    }
}
