using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Timers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.IO;
using System.Threading;

namespace SmartHouseSD
{
    public class camera
    {
        public Capture _cameraCapture; //create a camera capture
        public string name;
        public event EventHandler dataChange; //event to set whenever the homeserver needs to be updated
        public bool isDoorLocked;
        public bool isRecording;

        Image<Bgr, Byte> initImage;  //initial image for reference later

        System.Timers.Timer lengthTimer;  //timer for keeping track of how many seconds of video should be recorded
        System.Timers.Timer frameTimer;  // timer for when to get the next frame of the video

        VideoWriter recordVid; //videoWriter for recording video to file

        public camera(int camNumber, string tname)
        {
            isDoorLocked = false;
            name = tname;
            try
            {
                _cameraCapture = new Capture(camNumber);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            initImage = _cameraCapture.QueryFrame();


            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\Snapshots") == false)
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Snapshots");
            }
            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\Videos") == false)
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Videos");
            }

            //            startVideo("test" + tname + ".avi");
            //Thread.Sleep(500);
            //stopVideo();
            //File.Delete(Directory.GetCurrentDirectory() + "\\Videos\\" + name.Replace(" ", string.Empty) + "test" + tname + ".avi");
        }

        public void checkDoorLock()
        {//Derek put code here, return true if door is locked, return false if door is unlocked
            //set up 
            Image<Bgr, Byte> lckFrame = _cameraCapture.QueryFrame();
            Size area = new Size(lckFrame.Width / 15, lckFrame.Height / 15);
            Point mypoint = new Point((lckFrame.Width / 2) + 50, 2 * (lckFrame.Height / 3) + 33);
            Rectangle myBox = new Rectangle(mypoint, area);
            double slope;
            double slopesum = 0;
            double rslope;

            //canny stuff 
            Image<Gray, Byte> gryfirst = lckFrame.Convert<Gray, Byte>();
            Image<Gray, Byte> smoth = gryfirst.SmoothGaussian(7);
            Image<Gray, Byte> edges = smoth.Canny(25, 100);

            //line stuff
            edges.ROI = myBox;
            LineSegment2D[] linesfound = edges.HoughLinesBinary(1.0, Math.PI / 30, 10, 10, 15)[0];

            //while loop to make sure it works 
            while (linesfound.Length == 0)
            {
                lckFrame = _cameraCapture.QueryFrame();
                area = new Size(lckFrame.Width / 15, lckFrame.Height / 15);
                mypoint = new Point((lckFrame.Width / 2) + 50, 2 * (lckFrame.Height / 3) + 33);
                myBox = new Rectangle(mypoint, area);
                slopesum = 0;

                //canny stuff 
                gryfirst = lckFrame.Convert<Gray, Byte>();
                smoth = gryfirst.SmoothGaussian(7);
                edges = smoth.Canny(25, 100);

                //line stuff
                edges.ROI = myBox;
                linesfound = edges.HoughLinesBinary(1.0, Math.PI / 30, 10, 10, 15)[0];
            }

            //slope stuff
            foreach (LineSegment2D line in linesfound)
            {
                if (line.P2.X - line.P1.X != 0)
                {
                    slope = (Math.Abs(line.P2.Y - line.P1.Y)) / (Math.Abs(line.P2.X - line.P1.X));
                }
                else
                {
                    slope = 1;
                }
                slopesum += slope;
            }

            slopesum = slopesum / linesfound.Length;
            rslope = Math.Round(slopesum);
            //  Random myrng = new Random();
            //  int numbo = myrng.Next(1, 100); 
            if (rslope == 0)
            {
                isDoorLocked =  true;
            }
            else
            {
                isDoorLocked = false;
            }
            OnDataChange(EventArgs.Empty);
        }

        public void takeFrame(string filename)
        {//save a frame from the video feed at the location filename
            //assuming the user is giving file extension (ex: .png, .jpg) in filename 
            _cameraCapture.QueryFrame().ToBitmap().Save(Directory.GetCurrentDirectory() + "\\Snapshots\\" + name.Replace(" ", string.Empty) + filename);
        }

        public void takeVideo(string filename, double length)
        {   //filename should be a "xxxxxxxxx.avi"
            //assuming length is in seconds, can change that later if need be 
            //also this is going to be set to lifecam settings (FPS specifically) and I am assuming I have to get my own frames 
            //FPS for Lifecam: up 30FPS, so I am using 30. 
            //as above, assuming .avi or whatever will be included in filename
            if (!isRecording)
            {
                lengthTimer = new System.Timers.Timer(length * 1000);
                lengthTimer.Elapsed += new ElapsedEventHandler(stopLengthTimer);  //when the video is the desired length, stop recording

                startVideo(filename);
                lengthTimer.Enabled = true;
            }
            else
            {
                MessageBox.Show("Already recording a video");
            }
        }

        public void startVideo(string filename)
        {
            //VW: new VW(filename, fps, width, height, bool iscolor);
            if (!isRecording)
            {
            //    initImage = _cameraCapture.QueryFrame();
                recordVid = new VideoWriter(Directory.GetCurrentDirectory() + "\\Videos\\" + name.Replace(" ", String.Empty) + filename, 30, initImage.Width, initImage.Height, true);  //use the filename and reference image to start a video at 30fps
                frameTimer = new System.Timers.Timer(33.3333);  //interupt about 30 times a second
                frameTimer.Elapsed += new ElapsedEventHandler(frameTimeEvent);
                frameTimer.Enabled = true;
                isRecording = true;
            }
            else
            {
                MessageBox.Show("Already recording a video");
            }
        }

        public void stopVideo()
        {  //stop recording the video
            isRecording = false; 
            frameTimer.Enabled = false;
            frameTimer.Dispose();
            //recordVid.Dispose();
        }

        public void stopLengthTimer(object source, ElapsedEventArgs e)
        {  //Event handler for whenever the requested video length has been reached
            lengthTimer.Enabled = false;
            lengthTimer.Dispose();
            stopVideo();
        }

        private void frameTimeEvent(object source, ElapsedEventArgs e)
        {  //Event handler for getting the next from of the video for a 30 fps video
            if (isRecording)
            {
                recordVid.WriteFrame<Bgr, Byte>(_cameraCapture.QueryFrame());
            }
        }

        //JAKE THINGS
        protected virtual void OnDataChange(EventArgs e)
        {  //Run this function whenever there is a true data change that the home server needs to know about
            EventHandler handler = dataChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
