using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace E7Bot
{
    public class Action
    {
        public string name { get; set; }
        
        public ImageRecognition img { get; set; }
        public Rect r;
        //private bool _isClick;

        public List<int> link;
        public bool isAnd;
        public bool IsClick{ get; set; }
        
        public BitmapImage ImageData
        {
            get
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    img.img.ToBitmap().Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                    memory.Position = 0;
                    BitmapImage bitmapimage = new BitmapImage();
                    bitmapimage.BeginInit();
                    bitmapimage.StreamSource = memory;
                    bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapimage.EndInit();
                    
                    return bitmapimage;
                }
            }
        }
      

        public Action(string name, bool isclick)
        {
            this.name = name;
            IsClick = isclick;
            img = new ImageRecognition(name);
            link = new List<int>();
            
        }

        public bool run()
        {
            bool found = false;
            
            img.RunTemplateMatch(out found,out r);
            
            return found;
        }

   
    }
}
