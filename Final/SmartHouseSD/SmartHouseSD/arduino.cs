using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

namespace SmartHouseSD
{
    public class arduino
    {
        public SerialPort device = new SerialPort();   //declare that there is a SerialPort for all functions

        //All sensor, and type (1 = ServerReads, 2 = ServerWrites, 3 = both)
        //bit 0 is read, bit 1 is write
        public sensor init = new sensor("I", 3, "Initialization");       //checks if both parties are ready
        public sensor armed = new sensor("S", 3, "Armed");      //If the system is armed or not
        public sensor fullStat = new sensor("F", 2, "Full Status Command");   //request a status update for all sensors
        public sensor rawActi = new sensor("R", 2, "Raw Data Command");    //request a raw data update
        public sensor actLevel = new sensor("A", 3, "Activity Level");   //Acceptable activity level sent, current activity level recieved
        public sensor light = new sensor("L", 3, "Light");  //TODO: all keys need to be updated to match arduino
        public sensor dooropen = new sensor("D", 1, "Door Open");   //whether door is open or closed
        public sensor trigger = new sensor("W", 1, "Trigger");     //whether activity threshhold is met
        public sensor sound = new sensor("U", 1, "Sound");      //current audio level
        public sensor motion = new sensor("M", 1, "Motion");     //Aggregate motion levels
        public sensor temper = new sensor("T", 1, "Temperature");     //current temperature
        public sensor alert = new sensor("Z", 1, "Alert Level");

        public Dictionary<string, sensor> sensors = new Dictionary<string, sensor>();

        public event EventHandler dataChange;  //event to set whenever there is action required from the home server for a data change

        public arduino(string port)
        //public arduino()
        {
            //prepare arduino communications
            device.BaudRate = 115200;
            device.ReadTimeout = 500;
            device.WriteTimeout = 500;
            device.ReceivedBytesThreshold = 200;
            /*  Attempts to make the event handling for data recived work //was never succesfull, going back to non-event handled arduino communications
            device.Parity = Parity.None;
            device.DataBits = 8;
            device.StopBits = StopBits.One;
            device.Handshake = Handshake.None;  //test from MSDN  //didnt work
            device.RtsEnable = true;
            device.DtrEnable = true;
            //device.RtsEnable = true;  //online said this might make events work  //didnt work
             */
            device.PortName = port;
            device.Open();   //open a new serial port at the location specified

            //initialize the sensor list (must be done insside a method, so using constructor)
            //TODO: may have problems of passing by value rather than referece??
            sensors.Add(init.key, init);
            sensors.Add(armed.key, armed);      //If the system is armed or not
            sensors.Add(fullStat.key, fullStat);   //request a status update for all sensors
            sensors.Add(rawActi.key, rawActi);    //request a raw data update
            sensors.Add(actLevel.key, actLevel);   //Acceptable activity level sent, current activity level recieved
            sensors.Add(light.key, light);  //TODO: all keys need to be updated to match arduino
            sensors.Add(dooropen.key, dooropen);   //whether door is open or closed
            sensors.Add(trigger.key, trigger);     //whether activity threshhold is met
            sensors.Add(sound.key, sound);      //current audio level
            sensors.Add(motion.key, motion);     //Aggregate motion levels
            sensors.Add(temper.key, temper);     //current temperature
            sensors.Add(alert.key, alert);

            //give arduino base activity level but do not arm
            init.val = 0;
            actLevel.val = 100;
            armed.val = 0;
            light.val = 0;
            dooropen.val = 0;
            trigger.val = 0;

            //            send("I","0");  //set intialization
            //send("A", "100");  //set activity Level

            //device.DataReceived += new SerialDataReceivedEventHandler(comm);  //this event would never trigger, uncertain why
            initializeArduino();

            Thread readloop = new Thread(loop);  //should have all been replaced with event handler version
            readloop.IsBackground = true;
            readloop.Start();
        }

        private void loop()
        {  //Attemps to replace this with a serial data handler did not work
            string temp = ""; //want to make this static somehow, but not sure the right way to do it in C#
            while (true)
            {
                if (device.BytesToRead != 0)
                {
                    try
                    {
                        temp = device.ReadLine();
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show("The input buffer is not responding");
                        MessageBox.Show(e1.Message);
                    }
                    parseCommunication(temp);
                }
                Thread.Sleep(100);  //May be needed to keep it consistent
            }
        }

        public void initializeArduino()
        {
            device.Write("I:1 A:1234");
        }

        public bool isOpen
        {
            get
            {
                return device.IsOpen;
            }
        }

        public void send(string key, string val)     //function to send a message to the mbed
        {
            sensor tempsen;
            if (sensors.TryGetValue(key, out tempsen))
            {   //this will handle keys that we expected
                if (tempsen.serverCanWrite)
                {   //if server should be able to write from this value
                    if (init.val == 1)
                    {   //and the arduino has told us it is ready
                        device.Write(key + ":" + val);
                        device.Write("F:1");
                    }
                    else
                    {
                        MessageBox.Show("Arduino has not sent InitReceived.  Attempting to initialize");
                        initializeArduino();
                    }
                }
                else
                {   //code if the sensor is not supposed to be written
                    string output = String.Format("Server should not read from {0} with value {1}", key, val);
                    MessageBox.Show(output);
                }
            }
            else
            {   //if key was unexpected
                string output = String.Format("Not expecting sensor key of {0}", key);
                MessageBox.Show(output);
            }
        }

        internal void send(string key, int val)
        {  //Method uses int for val instead fo string
            send(key, val.ToString());
        }

        private void parseCommunication(string s)
        {  //decipher a command recieved
            //break the recieved string into pieces
            string[] temp = s.Split(':');  //break string into 2 pieces, key, and value
            //temp[0] is the key, temp[1] is the value
            string val = "";  //val is the number value recieved
            //TODO: try without using val, just using temp[0/1]
            bool triggered = false;
            try
            {
                val = temp[1];
            }
            catch (Exception e)
            {
                string output = String.Format("Communcication recieved was not parsed : {0}", s);
                MessageBox.Show(output);
                MessageBox.Show(e.Message);
                return;
            }
            //find corresponding sensor
            sensor tempsen;
            if (sensors.TryGetValue(temp[0], out tempsen))
            {   //this will handle keys that we expected
                if (tempsen.serverCanRead)
                {   //if server should be able to read from this value
                    if (tempsen.val != Int32.Parse(temp[1]))
                    {
                        triggered = true;
                    }
                    tempsen.val = Int32.Parse(temp[1]);
                    if (triggered)
                    {
                        OnDataChange(EventArgs.Empty);
                    }
                }
                else
                {   //code if the sensor is not supposed to be read from
                    string output = String.Format("Server should not read from {0} with value {1}", temp[0], temp[1]);
                    MessageBox.Show(output);
                }
            }
            else
            {   //if key was unexpected
                string output = String.Format("Not expecting sensor key of {0}", temp[0]);
                MessageBox.Show(output);
            }
        }

        public string getAllDataString()
        {  //get a formatted string of all sensor data for human readability
            string s = "";
            foreach (KeyValuePair<string, sensor> entry in sensors)
            {
                s = s + entry.Value.name + " = " + entry.Value.val + "\r\n";
            }
            return s;
        }

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
