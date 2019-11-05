using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 

namespace E7Bot
{
    public static class SaveNLoad
    {
        public static void serializeFile(String fileNm, Profile c)
        {

            Stream s = File.OpenWrite(fileNm);
            BinaryFormatter b = new BinaryFormatter();  
            c.actionBT.saveChk();
            b.Serialize(s, c);  
            s.Close(); 
        }
        
        public static Profile deSerialize(String fileNm)
        {

            try
            {
                Stream s = File.Open(fileNm, FileMode.Open);
                BinaryFormatter b = new BinaryFormatter();
                Profile t = (Profile) b.Deserialize(s);
                s.Close();
                return t;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        
    }
}