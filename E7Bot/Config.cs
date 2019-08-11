using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;


namespace E7Bot
{
    public static class Config
    {
        public static List<String> fileList = new List<string>();

        public static List<String> profiles = new List<string>();

        public static Tree actionBT = new Tree();

        public static string CFG_PATH = "./cfg/";
        
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
    }
}