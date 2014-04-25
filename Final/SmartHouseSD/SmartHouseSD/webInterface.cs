using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Xml;
using System.Windows;

namespace SmartHouseSD
{
    public class webInterface
    {
        /*Notes
            * xmldocument.load(filename)
            * xmldocument.save(filename)
            * xmlreader.create(filename)
            * xmlreader.Read() - goes node-by-node down the page
            * xmlreader.Close()
            * reader.Name
         * 
         * 
         * webInterface(ref doorlock, ref ard1){
             interal_doorlock = doorlock
             internal_ard = ard1
             }
            * */
        public string filename = "sensor_data.xml";
        public XmlDocument interf_doc = new XmlDocument();
        public XmlReader reader;// = new XmlReader();
        public XmlWriter writer;// = new XmlWriter();
        public Dictionary<string, sensor> sensors;

        public int light;
        public bool isDoorLocked;
        public int alarmLevel;
        public int armed;

        public event EventHandler dataChange;  //event to set whenever data is changed that need action from the home server

        public webInterface(string fn, ref bool doorlock, ref Dictionary<string, sensor> sens)
        {
            sensors = sens;
            filename = fn;
            try
            {
                interf_doc.Load(filename);
                //updateWeb();
            }
            catch (Exception e)
            {
                using (writer = XmlWriter.Create(filename))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("sensors");
                    writer.WriteAttributeString("dataFromWebToHome", "0");
                    writer.WriteAttributeString("dataFromHomeToWeb", "1");

                    foreach (KeyValuePair<string, sensor> sen in sensors)
                    {
                        sensorToNode(sen.Value);
                    }
                    writer.WriteEndElement();
                    writer.Close();
                }
            }
            //updateWeb(filename);
            Thread readloop = new Thread(loop);  //should have all been replaced with event handler version
            readloop.IsBackground = true;
            readloop.Start();
        }

        public void loop()
        {
            while (true)
            {
                readXml();
                Thread.Sleep(100);
            }
        }

        public void updateWeb()
        {
            try
            {
                try
                {
                    interf_doc.Load(filename);
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                    Thread.Sleep(2000);
                    updateWeb();
                    return;
                }
                XmlNode root = interf_doc.SelectNodes("/sensors")[0];
                XmlNode curr_node;
                try
                {
                    string w2h = root.Attributes["dataFromWebToHome"].Value;
                    string h2w = root.Attributes["dataFromHomeToWeb"].Value;
                }
                catch (Exception e)
                {
                    return;
                }
                //can we wait until readXml has finished?
               /* while (w2h == "1")
                {
                    interf_doc.Load(filename);
                    root = interf_doc.SelectNodes("/sensors")[0];
                    w2h = root.Attributes["dataFromWebToHome"].Value;
                    readXml();
                }*/
                if (root.HasChildNodes)
                {
                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        curr_node = root.ChildNodes[i];
                        XmlAttributeCollection attrs = curr_node.Attributes;
                        string key = attrs["key"].Value;
                        sensor s;
                        sensors.TryGetValue(key, out s);  //Jake was here, this should be the correct way of doing this rather than the switch statement
                        attrs["value"].Value = Convert.ToString(s.val);
                    }
                }
                root.Attributes["dataFromHomeToWeb"].Value = "1";
                try
                {
                    interf_doc.Save(filename);
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                    return;
                }
            }
            catch (FileNotFoundException e)
            {
                //MessageBox.Show(e.Message);
                /*catch code*/
                return;
            }
            return;
        }

        public void sensorToNode(sensor s)
        {
            /*writing attributes requires a writer*/
            writer.WriteStartElement("sensor");
            writer.WriteAttributeString("name", s.name);
            writer.WriteAttributeString("value", Convert.ToString(s.val));
            writer.WriteAttributeString("key", s.key);
            writer.WriteFullEndElement();

        }

        public void readXml()
        {
            try
            {
                try
                {
                    interf_doc.Load(filename);
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                    return;
                }
                XmlNode root = interf_doc.SelectNodes("/sensors")[0];
                XmlNode curr_node;
                string w2h;
                try
                {
                    w2h = root.Attributes["dataFromWebToHome"].Value;
                }
                catch (Exception e)
                {
                    w2h = "0";
                }
                if (root.Attributes["dataFromWebToHome"].Value == "1")
                {
                    if (root.HasChildNodes)
                    {
                        for (int i = 0; i < root.ChildNodes.Count; i++)
                        {
                            curr_node = root.ChildNodes[i];
                            //check light value
                            if (curr_node.Attributes["key"].Value == "L")
                            {
                                light = Convert.ToInt32(curr_node.Attributes["value"].Value);
                            }
                            if (curr_node.Attributes["key"].Value == "S")
                            {
                                //may be deprecated
                                armed = Convert.ToInt32(curr_node.Attributes["value"].Value);
                            }
                            if (curr_node.Attributes["key"].Value == "Z")
                            {
                                alarmLevel = Convert.ToInt32(curr_node.Attributes["value"].Value);
                                //bandaid. alarm level not available? or should we use "arm"
                                if (alarmLevel > 3)
                                {
                                    alarmLevel = 3;
                                }
                            }

                        }
                    }
                    root.Attributes["dataFromWebToHome"].Value = "0";
                    interf_doc.Save(filename);
                    //root.Attributes["dataFromHomeToWeb"].Value = "0";
                    //interf_doc.Save(filename);
                    OnDataChange(EventArgs.Empty);
                }
            }
            catch (FileNotFoundException e)
            {
                //MessageBox.Show(e.Message);
                return;
                /*catch code*/
            }
            return;
        }

        //JAKE THINGS, NO TOUCHING PLEASE
        protected virtual void OnDataChange(EventArgs e)
        {  //Run this function whenever there is a true data change that the home server needs to know about, after all webInterface level updates have been made
            EventHandler handler = dataChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
