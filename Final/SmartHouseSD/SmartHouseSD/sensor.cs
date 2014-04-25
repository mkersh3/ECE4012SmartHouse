using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseSD
{
    //  would like to have both sensor types inherit, but not sure if this is reasonable
    public class sensor
    {
        public string key;
        public string name;
        public bool serverCanWrite;
        public bool serverCanRead;
        public int val { get; set; }

        public sensor(string temp, int coms, string tname)
        {
            key = temp;
            name = tname;
            switch (coms)
            {
                case 1:
                    serverCanRead = true;
                    serverCanWrite = false;
                    break;
                case 2:
                    serverCanRead = false;
                    serverCanWrite = true;
                    break;
                case 3:
                    serverCanRead = true;
                    serverCanWrite = true;
                    break;
            }
        }
    }

}