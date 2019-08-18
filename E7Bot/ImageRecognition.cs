using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Image = System.Drawing.Image;
using Rect = OpenCvSharp.Rect;


namespace E7Bot
{
    [Serializable]
    public class ImageRecognition
    {
        // private readonly Mat _img;
        public string name;
        public string filePath;


        private byte[] imgByte;

        public ImageRecognition(string template)
        {
            var temp = "Images/" + template;
            name = template;
            filePath = temp;
            Image img = Image.FromFile(filePath);
            imgByte = ImageToByte(img);
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[]) converter.ConvertTo(img, typeof(byte[]));
        }

        public void RunTemplateMatch(out bool found, out Rect rect)
        {
            if (imgByte == null)
            {
                Image img = Image.FromFile(filePath);
                imgByte = ImageToByte(img);
            }

            Mat _img = Mat.FromImageData(imgByte); //new Mat(filePath);

            found = false;
            Mat refMat = null; //new Mat(reference);
            Screen[] temp = Screen.AllScreens;
            Rectangle bounds;
            if (Config.use2ndMntr)
            {
                bounds = temp[1].Bounds;
            }
            else
            {
                bounds = temp[0].Bounds;
            }

            //Screen.GetBounds(System.Drawing.Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(bounds.Location.X, bounds.Location.Y, 0, 0, bitmap.Size);
                    refMat = OpenCvSharp.Mat.ImDecode(ImageToByte(bitmap), ImreadModes.Grayscale);
                }

                //bitmap.Save("test.jpg", ImageFormat.Jpeg);
            }

            //using ()//new Mat(template))
            using (Mat res = new Mat(refMat.Rows - _img.Rows + 1, refMat.Cols - _img.Cols + 1, MatType.CV_32FC1))
            {
                //Convert input images to gray
                Mat gref = refMat; //refMat.CvtColor(ColorConversionCodes.BGR2GRAY);
                Mat gtpl = _img.CvtColor(ColorConversionCodes.BGR2GRAY);

                Cv2.MatchTemplate(gref, gtpl, res, TemplateMatchModes.CCoeffNormed);
                Cv2.Threshold(res, res, 0.8, 1.0, ThresholdTypes.Tozero);

                while (true)
                {
                    double minval, maxval, threshold = 0.8;
                    OpenCvSharp.Point minloc, maxloc;
                    Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);

                    if (maxval >= threshold)
                    {
                        //Setup the rectangle to draw
                        Rect r = new Rect(new OpenCvSharp.Point(maxloc.X, maxloc.Y),
                            new OpenCvSharp.Size(_img.Width, _img.Height));
                        rect = r;
                        Console.WriteLine("Found" + name);
                        // if (isClick)
                        {
                            //VirtualMouse.LeftClick();
                            if(Config.use2ndMntr){
                            r.X += temp[0].Bounds.Right;
                            r.Y += temp[1].Bounds.Top;
                            }
                            VirtualMouse.ClickImg(r);
                            Console.WriteLine("Click");
                        }

                        //Fill in the res Mat so you don't find the same area again in the MinMaxLoc
                        Rect outRect;
                        Cv2.FloodFill(res, maxloc, new Scalar(0), out outRect, new Scalar(0.1), new Scalar(1.0), 4);

                        found = true;
                        /*Cv2.NamedWindow( "Display window", WindowMode.AutoSize );// Create a window for display.
                        Cv2.ImShow("Display window", gref );   
                        Cv2.WaitKey(0);*/
                        break;
                    }
                    else
                    {
                        rect = Rect.Empty;
                        break;
                    }
                }
            }
        }
    }
}