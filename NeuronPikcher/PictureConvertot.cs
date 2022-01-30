using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace NeuronPikcher
{
    public class PictureConvertot
    {
        public int Boundary { get; set; } = 180;
        private int Width;
        private int Height;
        public int GroupSize { get; set; } = 10;
         public List<double> Convert(string path)
        {
            var dir = Directory.GetCurrentDirectory();
            
            var result = new List<double>();
            
            var imOpen = new Bitmap(path);
            var image = new Bitmap(imOpen, new Size(50,50));
            Width = image.Width;
            Height = image.Height;

            for(int x = 0; x < image.Width; x++)
            {
                for(int y = 0; y < image.Height; y++)
                {
                     var pixel = image.GetPixel(x, y);
                    result.Add(Brightness(pixel));
                }
            }
           
            return result;
        }
        private double Brightness(Color pixel)
        {
            
            var result = 0.299 + pixel.R + 0.578 * pixel.G + 0.114 * pixel.B;
            return result < Boundary ? 0 : 1;
        }
        public void Save(string path,List<double> pixels)
        {
            var image = new Bitmap(Width, Height);
            for (int y = 0; y < image.Width; y++)
            {
                for (int x = 0; x < image.Height; x++)
                {
                    var color = pixels[y * Width + x] > 0.5 ? Color.White : Color.Black;
                    image.SetPixel(x,y,color);
                }
            }
            image.Save(path);
        }
    }
}
