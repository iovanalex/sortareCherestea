using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace WpfApplication1
{
    class AppConfigStore
    {
        private static AppConfigStore instance;

        private AppConfigStore() { }

        public static AppConfigStore getInstance() 
        {
           
            if (instance == null){
                instance=new AppConfigStore();
                instance.loadConfig("app_config.ini");
            }
            return instance;
        }
        

        public static String plcIp
        {
            get{return plcIp;}
            private set { }
        }
        public static uint plcRack
        {
            get { return plcRack; }
            private set { }
        }
        public static uint plcSlot
        {
            get { return plcSlot; }
            private set {}
        }

        private void loadConfig(String iniFile)
        {
            IniFileIO f = new IniFileIO(iniFile);
            plcIp = f.IniReadValue("plc", "ip");
            Console.Out.WriteLine("Loaded plc ip " + plcIp);
            String slotTmp = f.IniReadValue("plc", "slot");
            String rackTemp = f.IniReadValue("plc", "rack");
            Console.Out.WriteLine("Loaded lsot " + slotTmp);
            Console.Out.WriteLine("Loaded rack " + rackTemp);
         //   plcSlot = Convert.ToUInt16(f.IniReadValue("plc", "slot"));
         //   plcRack = Convert.ToUInt16(f.IniReadValue("plc", "rack"));
        }
    }
}
