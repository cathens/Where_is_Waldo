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



            String filepathA = "C:/Users/" + Environment.UserName.ToString() + "/Source/Repos/Where_is_Waldo/Where is waldo/11.png";
            String filepathB = "C:/Users/" + Environment.UserName.ToString() + "/Source/Repos/Where_is_Waldo/Where is waldo/4.jpeg";

            CvInvoke.NamedWindow(win1, NamedWindowType.Normal); //Create the window using the specific name


            Image<Bgr, byte> template1 = new Image<Bgr, byte>(filepathA); // Image A
            Image<Bgr, byte> source = new Image<Bgr, byte>(filepathB); // Image B
            Image<Bgr, byte> imageToShow = source.Copy();

            LinkedList<double> list = new LinkedList<double>();
            LinkedList<Point> points = new LinkedList<Point>();
            LinkedList<Size> size = new LinkedList<Size>();

            for (double x = .1;x<1;x+=0.1) {
                Image<Bgr, byte> template = template1.Resize(x, Inter.Linear);
                using (Image<Gray, float> result = source.MatchTemplate(template, TemplateMatchingType.Ccoeff))
                {
                    double[] minValues, maxValues;
                    Point[] minLocations, maxLocations;
                    result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                    // You can try different values of the threshold. I guess somewhere between 0.75 and 0.95 would be good.
                    if (maxValues[0] >= .9)
                    {
                        // This is a match. Do something with it, for example draw a rectangle around it.
                        list.AddFirst(maxValues[0]);
                        points.AddFirst(maxLocations[0]);
                        size.AddFirst(template.Size);

                        Console.WriteLine("" + maxValues[0]);
                    }
                }
            }

            int i = 0;
            double max = list.Max();
            for(i = 0;i<list.Count;i++)
            {
                if (list.ToArray()[i] == max)
                    break;
            }



            Rectangle match = new Rectangle(points.ToArray()[i], size.ToArray()[i]);
            imageToShow.Draw(match, new Bgr(Color.Red), 3);
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
