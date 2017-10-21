using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Where_is_waldo
{
    class Program
    {
        static void Main(string[] args)
        {

            String win1 = "Test Window"; //The name of the window
            String win2 = "Template";



            String filepathA = "C:/Users/" + Environment.UserName.ToString() + "/Source/Repos/Where_is_Waldo/Where is waldo/Pics/waldoyellow.png";
            String filepathB = "C:/Users/" + Environment.UserName.ToString() + "/Source/Repos/Where_is_Waldo/Where is waldo/Pics/4.jpeg";

            CvInvoke.NamedWindow(win1, NamedWindowType.Normal); //Create the window using the specific name
            CvInvoke.NamedWindow(win2, NamedWindowType.Normal); //Create the window using the specific name


           // Mat z = CvInvoke.Imread(filepathA, ImreadModes.Unchanged);
           // Image<Bgra, byte>  template1 = z.ToImage<Bgra, byte>();
           
            Image<Bgra, byte> template1 = new Image<Bgra, byte>(filepathA); // Image A
            Image<Bgra, byte> source = new Image<Bgra, byte>(filepathB); // Image B
            Image<Bgra, byte> imageToShow = source.Copy();


            Bitmap bitmap = new Bitmap(filepathB);

            for(int x1 = 0; x1 < bitmap.Width; x1++)
            {
                for(int y1 = 0; y1< bitmap.Height; y1++)
                {
                    Color color = bitmap.GetPixel(x1, y1);
                    
                }
            }
         

            double maxMatchValue = 0;
            Point maxPoint = new Point();
            Size maxSize = new Size();

            for (double x = .1;x<=1;x+=0.1) {

                for (double y = .1; y <= 1; y += 0.1)
                {
                    Image<Bgra, byte> template = template1.Resize((int)(template1.Width * x), (int)(template1.Height * y), Inter.Linear);
                    Console.WriteLine(x + " "+ y);

                    using (Image<Gray, float> result = source.MatchTemplate(template, TemplateMatchingType.CcorrNormed))
                    {
                        double[] minValues, maxValues;
                        Point[] minLocations, maxLocations;
                        result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                        Console.WriteLine(maxValues[0]);

                        // You can try different values of the threshold. I guess somewhere between 0.75 and 0.95 would be good.
                        if (maxValues[0] >= .7 && maxValues[0] > maxMatchValue)
                        {
                            // This is a match. Do something with it, for example draw a rectangle around it.
                            maxMatchValue = maxValues[0];
                            maxPoint = maxLocations[0];
                            maxSize = template.Size;
                            
                            Console.WriteLine("" + maxMatchValue + " " + x + " " + y);
                            CvInvoke.Imshow(win2, template);
                        }
                    }
                }
            }

            Rectangle match = new Rectangle(maxPoint, maxSize);
            imageToShow.Draw(match, new Bgra(0,0,255,255), 3);
            //ImageBox imageBox1 = new ImageBox();

            // Show imageToShow in an ImageBox (here assumed to be called imageBox1)
            //imageBox1.Image = imageToShow;


            //imageToShow = imageToShow.Resize(.5, Inter.Linear);
            CvInvoke.Imshow(win1, imageToShow); //Show the image
            CvInvoke.WaitKey(0);
            CvInvoke.DestroyWindow(win1);
        }
    }
}
