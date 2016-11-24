using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ChristmasHatsOnAnimeGirlsTheProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            var hat = new Bitmap("hat.png");
            var cascade = new CascadeClassifier("lbpcascade_animeface.xml");

            foreach (var file in Directory.GetFiles(args[0]))
            {
                var bmp = new Bitmap(file);
                var image = new Image<Bgr, byte>(bmp);
                var gray = image.Convert<Gray, byte>();
                gray._EqualizeHist();

                var faces = cascade.DetectMultiScale(gray, scaleFactor: 1.1, minNeighbors: 5, minSize: new Size(24, 24));

                using (var gfx = Graphics.FromImage(bmp))
                {
                    foreach (var face in faces)
                    {
                        var hatWitdh = face.Width + face.Width / 10;
                        var hatHeight = hat.Height * ((double)hatWitdh / hat.Width);
                        var dstRect = new Rectangle(face.X, face.Y - face.Height / 2, hatWitdh, (int)hatHeight);
                        //image.Draw(face, new Bgr(0, double.MaxValue, 0), 3);
                        gfx.DrawImage(hat, dstRect);
                    }
                }

                bmp.Save($@"{args[0]}Results\{Path.GetFileName(file)}");
            }
            //image.Save("test.png");
        }
    }
}
