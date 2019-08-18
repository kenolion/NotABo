using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Timers;
using System.Windows.Controls;


namespace E7Bot
{
    public static class Config
    {
        public static List<String> fileList = new List<string>();

        public static List<String> profiles = new List<string>();

        public static Tree actionBT = new Tree();

        public static string CFG_PATH = "./cfg/";

        public static E7Timer shutDowntime = new E7Timer(10000);

        public static bool use2ndMntr { get; set; }
        
        public static void getImages()
        {
            fileList.Clear();
            DirectoryInfo dirInfo = new DirectoryInfo("Images");
            FileInfo[] info = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);
            for (int j = 0; j < info.Length; j++)
            {
                fileList.Add(info[j].Name);
            }
        }

        public static void getCfg(ComboBox cb)
        {
            try
            {
                profiles.Clear();
                cb.Items.Clear();
                DirectoryInfo dirInfo = new DirectoryInfo("cfg");
                FileInfo[] info = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);
                for (int j = 0; j < info.Length; j++)
                {
                    profiles.Add(info[j].Name);
                    cb.Items.Add(info[j].Name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
       
        }
        
        

        public static void shutDownPc(Object source, ElapsedEventArgs e)
        {
            Console.Write("shutting down PC...");
            Profile shutdownCfg = SaveNLoad.deSerialize( Config.CFG_PATH + "ShutDownPc");
            shutdownCfg.actionBT.run();
            Thread.Sleep(2000);
            shutdownCfg.actionBT.run();
            E7Timer shutDown = new E7Timer(5000);
            shutDown.SetFunction(shutFct);
            shutDown.Start();
        }

        public static void shutFct(Object source, ElapsedEventArgs e)
        {
            var psi = new ProcessStartInfo("shutdown","/s /t 0");
          psi.CreateNoWindow = true;
          psi.UseShellExecute = false;
          Process.Start(psi);
        }
    }
}