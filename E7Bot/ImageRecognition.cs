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
       
        

        public ImageRecognition(string template)
        {
            var temp = "Images/" + template;
            name = template;
            filePath = temp;
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[]) converter.ConvertTo(img, typeof(byte[]));
        }

        public void RunTemplateMatch(out bool found, out Rect rect)
        {
            Mat _img = new Mat(filePath);
            found = false;
            Mat refMat = null; //new Mat(reference);
            var bounds = Screen.GetBounds(System.Drawing.Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
                    refMat = OpenCvSharp.Mat.ImDecode(ImageToByte(bitmap), ImreadModes.Color);
                }

                //bitmap.Save("test.jpg", ImageFormat.Jpeg);
            }

            //using ()//new Mat(template))
            using (Mat res = new Mat(refMat.Rows - _img.Rows + 1, refMat.Cols - _img.Cols + 1, MatType.CV_32FC1))
            {
                //Convert input images to gray
                Mat gref = refMat.CvtColor(ColorConversionCodes.BGR2GRAY);
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
                        /*Console.WriteLine(
                            $"MinVal={minval.ToString()} MaxVal={maxval.ToString()} MinLoc={minloc.ToString()} MaxLoc={maxloc.ToString()} Rect={r.ToString()}");*/

                        //Draw a rectangle of the matching area
                        //Cv2.Rectangle(refMat, r, Scalar.LimeGreen, 2);
                        rect = r;
                        //VirtualMouse.MoveTo(r);
                        Console.WriteLine("Found" + name);
                        // if (isClick)
                        {
                            //VirtualMouse.LeftClick();
                            VirtualMouse.ClickImg(r);
                            Console.WriteLine("Click");
                        }

                        //Fill in the res Mat so you don't find the same area again in the MinMaxLoc
                        Rect outRect;
                        Cv2.FloodFill(res, maxloc, new Scalar(0), out outRect, new Scalar(0.1), new Scalar(1.0), 4);
                        found = true;
                       // break;
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