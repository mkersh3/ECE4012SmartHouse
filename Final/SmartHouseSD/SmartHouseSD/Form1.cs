using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace SmartHouseSD
{
    public partial class Form1 : Form
    {
        //ImageViewer viewer1, viewer2;
        //homeServer home;

        /// test start
       /*
        public static Capture _cameraCapture1; //create a camera capture
        public static Capture _cameraCapture2; //create a camera capture
        */
        //test stop
        
        public Form1()  //perform initializations in teh constructors and start threads for action
        {
            InitializeComponent();
            //home = new homeServer("COM1");  //replace with the port for communciation with arduino

            Thread formloop = new Thread(updateForm);
            formloop.IsBackground = true;
            formloop.Start();
        }
        public void updateForm(){

            //Test Start
            //Image<Bgr, Byte> frame1 = 
            //Image<Bgr, Byte> frame2 = 
            /*
            viewer1.Image = _cameraCapture1.QueryFrame();
            viewer2.Image = _cameraCapture2.QueryFrame();
            viewer1.ShowDialog();
            viewer2.ShowDialog();
            */
            //textBox1.Text = home.getAllDataString();

            //TEST STOP
            /*
            Image<Bgr, Byte> frame1 = home.cam1.getFrame();
            Image<Bgr, Byte> frame2 = home.cam2.getFrame();

            //viewer1.Image = home.cam1.getFrame();
            //viewer2.Image = home.cam2.getFrame();
            viewer1.Image = frame1;
            viewer2.Image = frame2;
            viewer1.ShowDialog();
            viewer2.ShowDialog();
             */
        }

        //TODO
        public void newEmail()
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateTextBox(object sender, EventArgs e)
        {
            //textBox1.Text = home.getAllDataString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
            
    }
}
