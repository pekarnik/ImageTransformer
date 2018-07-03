using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace ImageTransformer
{
    class ImageByteWorker
    {
        public static Bitmap LoadBitmap(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                return new Bitmap(fs);
        }
        public static byte[,,] BitmapToByteRgbNaive(Bitmap jpg)
        {
            int width = jpg.Width,
                height = jpg.Height;
            byte[,,] res = new byte[3, height, width];
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    System.Drawing.Color color = jpg.GetPixel(x, y);
                    res[0, y, x] = color.R;
                    res[1, y, x] = color.G;
                    res[2, y, x] = color.B;
                }
            }
            return res;
        }
		public static ushort[,] BitmapToUshortRgbNaive(Bitmap jpg)
		{
			int width = jpg.Width,
				height = jpg.Height;
			ushort[,] res = new ushort[width, height];
			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					System.Drawing.Color color = jpg.GetPixel(x, y);
					res[x, y] = color.R;
				}
			}
			return res;
		}
		public static ushort[,] XCRToByteRgb(string xcr, ref ushort[] ondres)
		{
			int width = 1024;
			int height = 1025;
			ushort[,] res = new ushort[height, width];
			using (FileStream fs = File.OpenRead(xcr))
			{
				BinaryReader reader = new BinaryReader(fs);
				//byte[] res1=new byte[width*height*2];
				//int offset = 0;
				//byte[] res2 = new byte[2];
				int index = 0;
				for (int y = 0; y < height; ++y)
				{
					for (int x = 0; x < width; ++x)
					{
						res[y, x] = reader.ReadUInt16();
						ondres[index++] = res[y, x];
					}
				}
				for (int i = 0; i < ondres.Length - 1024; i++)
				{
					ondres[i] = ondres[i + 1024];
				}
				for (int i = 0; i < ondres.Length; i++)
				{
					byte[] reverse = new byte[2];
					reverse = BitConverter.GetBytes(ondres[i]);
					ondres[i] = (ushort)(256 * reverse[0] + reverse[1]);
				}

			}
			return res;
		}
		public static ushort[,] XCRToByteRgbNormal(string xcr, int width, int height)
		{			
			ushort[,] res = new ushort[width, height];
			using (FileStream fs = File.OpenRead(xcr))
			{
				BinaryReader reader = new BinaryReader(fs);
				//byte[] res1=new byte[width*height*2];
				//int offset = 0;
				//byte[] res2 = new byte[2];
				int index = 0;
				for (int y = 0; y < height; ++y)
				{
					for (int x = 0; x < width; ++x)
					{
						res[x, y] = reader.ReadUInt16();						
					}
				}	
				
				

			}
			return res;
		}
		public static Bitmap Resize(Bitmap bmp, double scale)
		{
			var res = new Bitmap((int)(bmp.Width * scale), (int)(bmp.Height * scale));
			for (int x = 0; x < res.Width; x++)
				for (int y = 0; y < res.Height; y++)
					res.SetPixel(x, y, bmp.GetPixel((int)(x / scale), (int)(y / scale)));

			return res;
		}
		public static Bitmap BResize(Bitmap bmp, double scale)
		{
			var res = new Bitmap((int)(bmp.Width * scale), (int)(bmp.Height * scale));
			double nXFactor = (double)bmp.Width / (double)res.Width;
			double nYFactor = (double)bmp.Height / (double)res.Height;
			double fraction_x, fraction_y, one_minus_x, one_minus_y;
			int ceil_x, ceil_y, floor_x, floor_y;
			Color c1 = new Color();
			Color c2 = new Color();
			Color c3 = new Color();
			Color c4 = new Color();
			byte v;

			byte b1, b2;

			for (int x = 0; x < res.Width; ++x)
			{
				for (int y = 0; y < res.Height; ++y)
				{
					floor_x = (int)Math.Floor(x * nXFactor);
					floor_y = (int)Math.Floor(y * nYFactor);
					ceil_x = floor_x + 1;
					if (ceil_x >= bmp.Width) ceil_x = floor_x;
					ceil_y = floor_y + 1;
					if (ceil_y >= bmp.Height) ceil_y = floor_y;
					fraction_x = x * nXFactor - floor_x;
					fraction_y = y * nYFactor - floor_y;
					one_minus_x = 1.0 - fraction_x;
					one_minus_y = 1.0 - fraction_y;

					c1 = bmp.GetPixel(floor_x, floor_y);
					c2 = bmp.GetPixel(ceil_x, floor_y);
					c3 = bmp.GetPixel(floor_x, ceil_y);
					c4 = bmp.GetPixel(ceil_x, ceil_y);

					b1 = (byte)(one_minus_x * c1.B + fraction_x * c2.B);
					b2 = (byte)(one_minus_x * c3.B + fraction_x * c4.B);
					v = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

					res.SetPixel(x, y, Color.FromArgb(255, v, v, v));
				}
			}
			return res;
		}
		public static Bitmap Negate(Bitmap bmp)
		{
			var res = new Bitmap(bmp.Width, bmp.Height);
			for (int x = 0; x < bmp.Width; x++)
			{
				for (int y = 0; y < bmp.Height; y++)
				{
					Color pixelColor = bmp.GetPixel(x, y);
					Color newColor = Color.FromArgb(255 - pixelColor.R, 255 - pixelColor.G, 255 - pixelColor.B);
					res.SetPixel(x, y, newColor);
				}
			}

			return res;
		}
		public static Bitmap GammaTransform(Bitmap bmp)
		{
			var res = new Bitmap(bmp.Width, bmp.Height);
			for (int x = 0; x < bmp.Width; x++)
			{
				for (int y = 0; y < bmp.Height; y++)
				{
					Color pixelColor = bmp.GetPixel(x, y);
					var cr = (byte)(255 * Math.Pow((pixelColor.R / 255.0), (1 / 2.0)));
					Color newColor = Color.FromArgb(cr, cr, cr);
					res.SetPixel(x, y, newColor);
				}
			}

			return res;
		}
		public static Bitmap LogTransform(Bitmap bmp)
		{
			var res = new Bitmap(bmp.Width, bmp.Height);
			for (int x = 0; x < bmp.Width; x++)
			{
				for (int y = 0; y < bmp.Height; y++)
				{
					Color pixelColor = bmp.GetPixel(x, y);
					Color newColor = Color.FromArgb((byte)(82*Math.Log10(pixelColor.R + 1)), (byte)(82*Math.Log10(pixelColor.G + 1)), (byte)(82*Math.Log10(pixelColor.B + 1)));
					res.SetPixel(x, y, newColor);
				}
			}

			return res;
		}
		public static Bitmap Gray(Bitmap image)
		{
			byte[] imagebyte = new byte[image.Width*image.Height];
			int index = 0;
			for(int y=0;y<image.Height;y++)
				for(int x=0;x<image.Width;x++)
				{
					imagebyte[index++]=image.GetPixel(x,y).R;
				}
			byte min = imagebyte.Min();
			byte max = imagebyte.Max();
			for(int i=0;i<imagebyte.Length;i++)
			{
				imagebyte[i] = (byte)(double)((double)((imagebyte[i] - min) / (double)(max - min)) * 255);
			}
			index = 0;
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					image.SetPixel(x, y, Color.FromArgb(imagebyte[index], imagebyte[index], imagebyte[index]));
					index++;
				}
			return image;
		}
	}
}
