using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ImageTransformer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string a = textBox1.Text;
			switch (a)
			{
				case ("1"):
					{
						pictureBox1.Image = null;
						openFileDialog1.ShowDialog();
						Bitmap fs = ImageByteWorker.LoadBitmap(openFileDialog1.FileName);

						byte[,,] jpgbyte = ImageByteWorker.BitmapToByteRgbNaive(fs);
						Bitmap image = new Bitmap(fs.Width, fs.Height);

						for (int y = 0; y < image.Height; ++y)
						{
							for (int x = 0; x < image.Width; ++x)
							{
								System.Drawing.Color imagecolor = System.Drawing.Color.FromArgb(jpgbyte[0, y, x], jpgbyte[1, y, x], jpgbyte[2, y, x]);
								image.SetPixel(x, y, imagecolor);
							}
						}

						// image.RotateFlip(RotateFlipType.Rotate90FlipX);
						pictureBox1.Image = image;
						break;
					}
				case ("2"):
					{
						pictureBox2.Image = null;
						openFileDialog1.ShowDialog();
						Bitmap fs = ImageByteWorker.LoadBitmap(openFileDialog1.FileName);

						byte[,,] jpgbyte = ImageByteWorker.BitmapToByteRgbNaive(fs);
						Bitmap image = new Bitmap(fs.Width, fs.Height);

						for (int y = 0; y < image.Height; ++y)
						{
							for (int x = 0; x < image.Width; ++x)
							{
								System.Drawing.Color imagecolor = System.Drawing.Color.FromArgb(jpgbyte[0, y, x], jpgbyte[1, y, x], jpgbyte[2, y, x]);
								image.SetPixel(x, y, imagecolor);
							}
						}

						// image.RotateFlip(RotateFlipType.Rotate90FlipX);
						pictureBox2.Image = image;
						break;
					}
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			pictureBox1.Image = null;
			openFileDialog1.ShowDialog();
			ushort[] odnres = new ushort[1024 * 1025];
			ushort[,] res = ImageByteWorker.XCRToByteRgb(openFileDialog1.FileName, ref odnres);
			double max = odnres.Max();
			double min = odnres.Min();
			double[] odnres1 = new double[1024 * 1024];
			for (int i = 0; i < odnres1.Length; i++)
			{
				odnres1[i] = odnres[i];
			}
			for (int i = 0; i < odnres1.Length; i++)
			{
				odnres1[i] = ((odnres1[i] - min) / (max - min)) * 255;
			}
			ushort[] odnshortres = new ushort[1024 * 1024];
			for (int i = 0; i < odnres.Length - 1024; i++)
			{
				odnshortres[i] = (ushort)odnres1[i];
			}
			int index = 0;
			for (int y = 0; y < 1024; ++y)
			{
				for (int x = 0; x < 1024; ++x)
				{
					res[y, x] = (ushort)odnres1[index++];
				}
			}
			Bitmap image = new Bitmap(1024, 1024);
			index = 0;
			for (int y = 1023; y >= 0; --y)
			{
				for (int x = 0; x < 1024; ++x)
				{
					System.Drawing.Color imagecolor = System.Drawing.Color.FromArgb(odnshortres[index], odnshortres[index], odnshortres[index]);
					image.SetPixel(x, y, imagecolor);
					index++;
				}
			}
			pictureBox1.Image = image;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			int width = pictureBox1.Width;
			int height = pictureBox1.Height;
			Bitmap image = (Bitmap)pictureBox1.Image;
			image = ImageByteWorker.Resize(image, 1.7);
			pictureBox1.Image = image;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			int width = pictureBox1.Width;
			int height = pictureBox1.Height;
			Bitmap image = (Bitmap)pictureBox1.Image;
			pictureBox1.Image = null;
			image = ImageByteWorker.Resize(image, 1 / 1.7);
			pictureBox1.Image = image; ;
		}

		private void button5_Click(object sender, EventArgs e)
		{
			int width = pictureBox1.Width;
			int height = pictureBox1.Height;
			Bitmap image = (Bitmap)pictureBox1.Image;
			pictureBox1.Image = null;
			image = ImageByteWorker.BResize(image, 2.3);
			pictureBox1.Image = image;
		}

		private void button6_Click(object sender, EventArgs e)
		{
			int width = pictureBox1.Width;
			int height = pictureBox1.Height;
			Bitmap image = (Bitmap)pictureBox1.Image;
			pictureBox1.Image = null;
			image = ImageByteWorker.BResize(image, 1 / 1.7);
			pictureBox1.Image = image;
		}

		private void button7_Click(object sender, EventArgs e)
		{
			int width = pictureBox1.Width;
			int height = pictureBox1.Height;
			Bitmap image = (Bitmap)pictureBox1.Image;
			pictureBox1.Image = null;
			image = ImageByteWorker.Negate(image);
			pictureBox1.Image = image;
		}

		private void button8_Click(object sender, EventArgs e)
		{
			int width = pictureBox1.Width;
			int height = pictureBox1.Height;
			Bitmap image = (Bitmap)pictureBox1.Image;
			pictureBox1.Image = null;
			image = ImageByteWorker.GammaTransform(image);
			pictureBox1.Image = image;
		}

		private void button9_Click(object sender, EventArgs e)
		{
			int width = pictureBox1.Width;
			int height = pictureBox1.Height;
			Bitmap image = (Bitmap)pictureBox1.Image;
			pictureBox1.Image = null;
			image = ImageByteWorker.LogTransform(image);
			pictureBox1.Image = image;
		}

		private void button10_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			float[] colors = new float[256];
			int[] chartX = new int[256];
			for (int i = 0; i < 255; i++)
			{
				chartX[i] = i;
			}
			float maxY = 0;
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color r = image.GetPixel(x, y);
					int color = r.R;
					colors[color] = colors[color] + 1;
					if (maxY < colors[color])
					{
						maxY = colors[color];
					}
				}
			chart1.ChartAreas[0].AxisX.Minimum = 0;
			chart1.ChartAreas[0].AxisX.Maximum = 255;
			chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			chart1.ChartAreas[0].AxisY.Minimum = 0;
			chart1.ChartAreas[0].AxisY.Maximum = maxY;
			chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart1.Series[0].Points.DataBindXY(chartX, colors);
			for (int i = 0; i < 256; i++)
			{
				colors[i] = colors[i] / (image.Width * image.Height);
			}
			for (int i = 1; i < 256; i++)
			{
				colors[i] = colors[i - 1] + colors[i];
			}

			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color r = image.GetPixel(x, y);
					double clr = Math.Round(255 * colors[r.R]);
					r = Color.FromArgb((int)clr, (int)clr, (int)clr);
					image.SetPixel(x, y, r);
				}
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color r = image.GetPixel(x, y);
					int color = r.R;
					colors[color] = colors[color] + 1;
					if (maxY < colors[color])
					{
						maxY = colors[color];
					}
				}
			pictureBox1.Image = null;
			pictureBox1.Image = image;
			chart2.ChartAreas[0].AxisX.Minimum = 0;
			chart2.ChartAreas[0].AxisX.Maximum = 255;
			chart2.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			chart2.ChartAreas[0].AxisY.Minimum = 0;
			chart2.ChartAreas[0].AxisY.Maximum = maxY;
			chart2.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart2.Series[0].Points.DataBindXY(chartX, colors);
		}

		private void button11_Click(object sender, EventArgs e)
		{
			int width = Convert.ToInt32(textBox5.Text);
			int height = Convert.ToInt32(textBox6.Text);
			float[] imagefloat = new float[width * height];
			using (BinaryReader br = new BinaryReader(File.Open(textBox3.Text, FileMode.Open)))
			{
				for (int y = 0; y < height * width; y++)
				{
					imagefloat[y] = br.ReadSingle();
				}
			}
			float[,] image2float = new float[width, height];
			int index = 0;
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					image2float[x, y] = imagefloat[index];
					index++;
				}
			}
			Bitmap image = new Bitmap(width, height);
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					Color color = Color.FromArgb((int)image2float[x, y], (int)image2float[x, y], (int)image2float[x, y]);
					image.SetPixel(x, y, color);
				}
			}
			pictureBox1.Image = image;

		}

		private void button12_Click(object sender, EventArgs e)
		{

			float[] kernelfunc = new float[259];
			using (BinaryReader br = new BinaryReader(File.Open("kernL64_f4.dat", FileMode.Open)))
			{
				for (var i = 0; i < 64; i++)
				{
					kernelfunc[i] = br.ReadSingle();
				}
			}
			Complex[] fouriekernelfunc = Analysis.ForwardFurie(kernelfunc);
			int width = Convert.ToInt32(textBox5.Text);
			int height = Convert.ToInt32(textBox6.Text);
			float[] imagefloat = new float[width * height];

			using (BinaryReader br = new BinaryReader(File.Open(textBox3.Text, FileMode.Open)))
			{
				for (int y = 0; y < height * width; y++)
				{
					imagefloat[y] = br.ReadSingle();
				}
			}
			float[,] image2float = new float[width, height];
			int index = 0;
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					image2float[x, y] = imagefloat[index];
					index++;
				}
			}

			Complex[,] image3fouriefloat = new Complex[width, height];

			for (int i = 0; i < height; i++)
			{
				float[] mass = new float[width];
				for (int j = 0; j < width; j++)
					mass[j] = image2float[j, i];

				Complex[] mass2 = Analysis.ForwardFurie(mass);
				for (int i2 = 0; i2 < width; i2++)
					image3fouriefloat[i2, i] = mass2[i2];
			}
			Complex[,] newimage3fouriefloat = new Complex[width, height];
			float[,] newimage2fouriefloat = new float[width, height];
			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
				{
					newimage3fouriefloat[x, y] = image3fouriefloat[x, y] / fouriekernelfunc[x];
					//newimage3fouriefloat[0, x, y] = (image3fouriefloat[0, x, y] * fouriekernelfunc[0, x] + image3fouriefloat[1, x, y] * fouriekernelfunc[1, x])
					/// (float)(Math.Pow(fouriekernelfunc[0, x], 2) + Math.Pow(fouriekernelfunc[1, x], 2));

					//newimage3fouriefloat[1, x, y] = (fouriekernelfunc[0, x] * image3fouriefloat[1, x, y] - image3fouriefloat[0, x, y] * fouriekernelfunc[1, x])
					/// (float)(Math.Pow(fouriekernelfunc[0, x], 2) + Math.Pow(fouriekernelfunc[1, x], 2));
					newimage2fouriefloat[x, y] = (float)newimage3fouriefloat[x, y].Real + (float)newimage3fouriefloat[x, y].Imaginary;
				}
			float[] new1image = new float[width];
			float[] new2image = new float[width * height];
			index = 0;
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					new1image[x] = newimage2fouriefloat[x, y];
				}
				new1image = Analysis.ReverseFurieFunction(new1image);
				for (int x = 0; x < width; x++)
				{
					new2image[index++] = new1image[x];
				}
			}
			float[,] new2imagexy = new float[width, height];
			index = 0;
			float max = new2image.Max();
			float min = new2image.Min();
			for (int i = 0; i < new2image.Length; i++)
			{
				new2image[i] = ((new2image[i] - min) / (max - min)) * 255;
			}
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					new2imagexy[x, y] = new2image[index++];
				}
			}

			Bitmap image = new Bitmap(width, height);
			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
				{
					Color color = Color.FromArgb(255, (int)(new2imagexy[x, y]), (int)(new2imagexy[x, y]), (int)(new2imagexy[x, y]));
					image.SetPixel(x, y, color);
				}
			pictureBox1.Image = image;


		}

		private void button13_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			float[] colors = new float[256];
			int[] chartX = new int[256];
			for (int i = 0; i < 255; i++)
			{
				chartX[i] = i;
			}
			float maxY = 0;
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color r = image.GetPixel(x, y);
					int color = r.R;
					colors[color] = colors[color] + 1;
					if (maxY < colors[color])
					{
						maxY = colors[color];
					}
				}

			chart1.ChartAreas[0].AxisX.Minimum = 0;
			chart1.ChartAreas[0].AxisX.Maximum = 255;
			chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			chart1.ChartAreas[0].AxisY.Minimum = 0;
			chart1.ChartAreas[0].AxisY.Maximum = maxY + 1;
			chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart1.Series[0].Points.DataBindXY(chartX, colors);
			maxY = 0;
			for (int i = 0; i < colors.Length; i++)
			{
				colors[i] = colors[i] / (image.Width * image.Height);
				if (maxY < colors[i])
					maxY = colors[i];
			}
			float[] colorskum = new float[256];
            

            for (int i = 0; i < 256; i++)
			{
				colorskum[i] = colors.Take(i + 1).Sum() / colors.Sum();
			}
			double[] normX = new double[colorskum.Length];
            
            for (int i = 0; i < colorskum.Length; i++)
			{
				normX[i] = (double)i / 255;
			}
			double[] ans = new double[colorskum.Length];

            chart3.Series[0].Points.DataBindXY(normX, colorskum);

            for (int i = 0; i < colorskum.Length; i++)
			{
				ans[(int)(colorskum[i] * 255)] = normX[i];
			}

			for (int i = 0; i < ans.Length; i++)
			{
				if (ans[i] == 0)
				{
					ans[i] = colorskum[i];
				}
			}
            
            float[,] colpix = new float[image.Width, image.Height];
			for (int j = 0; j < image.Height; j++)
			{
				for (int i = 0; i < image.Width; i++)
				{
					Color ncol = image.GetPixel(i, j);
					colpix[i, j] = ncol.R;
				}
			}
			for (int j = 0; j < image.Height; j++)
			{
				for (int i = 0; i < image.Width; i++)
				{
					for (int h = 0; h < ans.Length; h++)
					{


						if (colorskum[(int)colpix[i, j]] < ans[h])
						{
							colpix[i, j] = h;
							break;
						}
					}
				}
			}
			for (int j = 0; j < image.Height; j++)
			{
				for (int i = 0; i < image.Width; i++)
				{
					Color ncol = System.Drawing.Color.FromArgb((int)colpix[i, j], (int)colpix[i, j], (int)colpix[i, j]);
					image.SetPixel(i, j, ncol);
				}
			}
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color r = image.GetPixel(x, y);
					int color = r.R;
					colors[color] = colors[color] + 1;
					if (maxY < colors[color])
					{
						maxY = colors[color];
					}
				}
			pictureBox1.Image = image;
			chart2.ChartAreas[0].AxisX.Minimum = 0;
			chart2.ChartAreas[0].AxisX.Maximum = 255;
			chart2.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			chart2.ChartAreas[0].AxisY.Minimum = 0;
			chart2.ChartAreas[0].AxisY.Maximum = maxY;
			chart2.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart2.Series[0].Points.DataBindXY(chartX, colors);
		}


		private void button14_Click(object sender, EventArgs e)
		{
			float[] kernelfunc = new float[307];
			using (BinaryReader br = new BinaryReader(File.Open("kernL64_f4.dat", FileMode.Open)))
			{
				for (var i = 0; i < 64; i++)
				{
					kernelfunc[i] = br.ReadSingle();
				}
			}
			Complex[] fouriekernelfunc = Analysis.ForwardFurie(kernelfunc);
			int width = Convert.ToInt32(textBox5.Text);
			int height = Convert.ToInt32(textBox6.Text);
			float[] imagefloat = new float[width * height];

			using (BinaryReader br = new BinaryReader(File.Open(textBox3.Text, FileMode.Open)))
			//using (BinaryReader br = new BinaryReader(File.Open("sblur307x221D.dat", FileMode.Open)))
			{
				for (int y = 0; y < height * width; y++)
				{
					imagefloat[y] = br.ReadSingle();
				}
			}
			float[,] image2float = new float[width, height];
			int index = 0;
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					image2float[x, y] = imagefloat[index];
					index++;
				}
			}

			Complex[,] image3fouriefloat = new Complex[width, height];

			for (int i = 0; i < height; i++)
			{
				float[] mass = new float[width];
				for (int j = 0; j < width; j++)
					mass[j] = image2float[j, i];

				Complex[] mass2 = Analysis.ForwardFurie(mass);
				for (int i2 = 0; i2 < width; i2++)
					image3fouriefloat[i2, i] = mass2[i2];
			}
			Complex[,] newimage3fouriefloat = new Complex[width, height];
			float[,] newimage2fouriefloat = new float[width, height];
			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
				{
					newimage3fouriefloat[x, y] = image3fouriefloat[x, y] * Complex.Conjugate(fouriekernelfunc[x]) / (Math.Pow(Complex.Abs(fouriekernelfunc[x]), 2) + Math.Pow(0.01, 2));
					//newimage3fouriefloat[0, x, y] = (image3fouriefloat[0, x, y] * fouriekernelfunc[0, x] + image3fouriefloat[1, x, y] * fouriekernelfunc[1, x])
					/// (float)(Math.Pow(fouriekernelfunc[0, x], 2) + Math.Pow(fouriekernelfunc[1, x], 2));

					//newimage3fouriefloat[1, x, y] = (fouriekernelfunc[0, x] * image3fouriefloat[1, x, y] - image3fouriefloat[0, x, y] * fouriekernelfunc[1, x])
					/// (float)(Math.Pow(fouriekernelfunc[0, x], 2) + Math.Pow(fouriekernelfunc[1, x], 2));
					newimage2fouriefloat[x, y] = (float)newimage3fouriefloat[x, y].Real + (float)newimage3fouriefloat[x, y].Imaginary;
				}
			float[] new1image = new float[width];
			float[] new2image = new float[width * height];
			index = 0;
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					new1image[x] = newimage2fouriefloat[x, y];
				}
				new1image = Analysis.ReverseFurieFunction(new1image);
				for (int x = 0; x < width; x++)
				{
					new2image[index++] = new1image[x];
				}
			}
			float[,] new2imagexy = new float[width, height];
			index = 0;
			float max = new2image.Max();
			float min = new2image.Min();
			for (int i = 0; i < new2image.Length; i++)
			{
				new2image[i] = ((new2image[i] - min) / (max - min)) * 255;
			}
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					new2imagexy[x, y] = new2image[index++];
				}
			}

			Bitmap image = new Bitmap(width, height);
			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
				{
					Color color = Color.FromArgb(255, (int)(new2imagexy[x, y]), (int)(new2imagexy[x, y]), (int)(new2imagexy[x, y]));
					image.SetPixel(x, y, color);
				}
			pictureBox1.Image = image;

		}

		private void button15_Click(object sender, EventArgs e)
		{
			openFileDialog1.ShowDialog();
			ushort[,] res = ImageByteWorker.XCRToByteRgbNormal(openFileDialog1.FileName, 400, 300);
			ushort max = 0;
			ushort min = (ushort)(Math.Pow(2, 14));

			for (int y = 0; y < 300; y++)
				for (int x = 0; x < 400; x++)
				{
					if (max < res[x, y])
						max = res[x, y];
					if (min > res[x, y])
						min = res[x, y];
				}
			Bitmap image = new Bitmap(400, 300);
			for (int y = 0; y < 300; y++)
				for (int x = 0; x < 400; x++)
				{
					res[x, y] = (ushort)(((res[x, y] - min) / (double)(max - min)) * 255);
					Color color = Color.FromArgb(res[x, y], res[x, y], res[x, y]);
					image.SetPixel(x, y, color);
				}
			pictureBox1.Image = image;
		}

		private void button16_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			ushort[,] picture = new ushort[image.Width, image.Height];
			picture = ImageByteWorker.BitmapToUshortRgbNaive(image);
			double[] derivative = new double[image.Width];
			for (int x = 0; x < image.Width - 1; x++)
			{
				derivative[x] = picture[x + 1, 10] - picture[x, 10];
			}
			double[] ac = new double[image.Width];
			for (int x = 0; x < image.Width; x++)
				ac[x] = Analysis.Autocorrelation(derivative, x);
			double[] xChart = new double[image.Width];
			for (int i = 0; i < xChart.Length; i++)
			{
				xChart[i] = i;
			}
			Complex[] acfourie = new Complex[image.Width];
			double[] acfourienorm = new double[image.Width / 2];
			acfourie = Analysis.ForwardFurie(ac);
			for (int i = 0; i < image.Width / 2; i++)
				acfourienorm[i] = Math.Sqrt(Math.Pow(acfourie[i].Real, 2) + Math.Pow(acfourie[i].Imaginary, 2));


			double[] xchartfourie = Analysis.FurieXTransform(xChart);
			//chart1.ChartAreas[0].AxisX.Minimum = 0;
			//chart1.ChartAreas[0].AxisX.Maximum = image.Width;
			//chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			//chart1.ChartAreas[0].AxisY.Minimum = 0-ac.Max();
			//chart1.ChartAreas[0].AxisY.Maximum = ac.Max();
			//chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart3.Series[0].Points.DataBindXY(xChart, ac);
			//chart3.ChartAreas[0].AxisX.Minimum = 0;
			//chart3.ChartAreas[0].AxisX.Maximum = xchartfourie[xchartfourie.Length-1];
			//chart3.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			//chart3.ChartAreas[0].AxisY.Minimum = 0;
			//chart3.ChartAreas[0].AxisY.Maximum = acfourienorm.Max();
			//chart3.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart4.Series[0].Points.DataBindXY(xchartfourie, acfourienorm);

		}

		private void button17_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			ushort[,] picture = new ushort[image.Width, image.Height];
			picture = ImageByteWorker.BitmapToUshortRgbNaive(image);
			double[] derivative1 = new double[image.Width];
			double[] derivative2 = new double[image.Width];
			for (int x = 0; x < image.Width - 1; x++)
			{
				derivative1[x] = picture[x + 1, 10] - picture[x, 10];
			}
			for (int x = 0; x < image.Width - 1; x++)
			{
				derivative2[x] = picture[x + 1, 40] - picture[x, 40];
			}

			double[] ac = new double[image.Width];
			for (int x = 0; x < image.Width; x++)
				ac[x] = Analysis.Crosscorrelation(derivative1, derivative2, x);
			double[] xChart = new double[image.Width];
			for (int i = 0; i < xChart.Length; i++)
			{
				xChart[i] = i;
			}
			Complex[] acfourie = new Complex[image.Width];
			Complex[] acfourienorm = new Complex[image.Width / 2];
			acfourie = Analysis.ForwardFurie(ac);
			for (int i = 0; i < image.Width / 2; i++)
				acfourienorm[i] = acfourie[i];
			double max = 0;
			for (int i = 0; i < image.Width / 2; i++)
				if (max < Math.Sqrt(Math.Pow(acfourienorm[i].Real, 2) + Math.Pow(acfourienorm[i].Imaginary, 2)))
					max = Math.Sqrt(Math.Pow(acfourienorm[i].Real, 2) + Math.Pow(acfourienorm[i].Imaginary, 2));
			double[] xchartfourie = Analysis.FurieXTransform(xChart);
			double[] furieac = new double[image.Width / 2];
			for (int i = 0; i < image.Width / 2; i++)
				furieac[i] = Math.Sqrt(Math.Pow(acfourienorm[i].Real, 2) + Math.Pow(acfourienorm[i].Imaginary, 2));
			chart1.ChartAreas[0].AxisX.Minimum = 0;
			chart1.ChartAreas[0].AxisX.Maximum = image.Width;
			chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			chart1.ChartAreas[0].AxisY.Minimum = 0 - ac.Max();
			chart1.ChartAreas[0].AxisY.Maximum = ac.Max();
			chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart1.Series[0].Points.DataBindXY(xChart, ac);
			chart3.ChartAreas[0].AxisX.Minimum = 0;
			chart3.ChartAreas[0].AxisX.Maximum = xchartfourie[xchartfourie.Length - 1];
			chart3.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			chart3.ChartAreas[0].AxisY.Minimum = 0;
			chart3.ChartAreas[0].AxisY.Maximum = max;
			chart3.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart3.Series[0].Points.DataBindXY(xchartfourie, furieac);

		}

		private void button18_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			ushort[,] picture = new ushort[image.Width, image.Height];
			picture = ImageByteWorker.BitmapToUshortRgbNaive(image);
			double[] derivative = new double[image.Width];
			for (int x = 0; x < image.Width - 1; x++)
			{
				derivative[x] = picture[x + 1, 100] - picture[x, 100];
			}
			double[] ac = new double[image.Width];
			for (int x = 0; x < image.Width; x++)
				ac[x] = Analysis.Autocorrelation(derivative, x);
			double[] xChart = new double[image.Width];
			for (int i = 0; i < ac.Length; i++)
			{
				xChart[i] = i;
			}
			Complex[] acfourie = new Complex[image.Width];
			Complex[] acfourienorm = new Complex[image.Width / 2];
			for (int i = 0; i < image.Width / 2; i++)
				acfourienorm[i] = acfourie[i];
			acfourie = Analysis.ForwardFurie(ac);
			for (int i = 0; i < image.Width / 2; i++)
				acfourienorm[i] = acfourie[i];
			double[] furieac = new double[image.Width / 2];
			for (int i = 0; i < image.Width / 2; i++)
				furieac[i] = Math.Sqrt(Math.Pow(acfourienorm[i].Real, 2) + Math.Pow(acfourienorm[i].Imaginary, 2));
			double[] xchartfourie = Analysis.FurieXTransform(xChart);
			double[,] withoutgrid = new double[image.Width, image.Height];
			double[] line = new double[image.Width];
			double[] convline = new double[image.Width];
			double[] acf = new double[acfourie.Length];
			for (int i = 0; i < acf.Length; i++)
				acf[i] = Math.Sqrt(Math.Pow(acfourie[i].Real, 2) + Math.Pow(acfourie[i].Imaginary, 2));
			double[] freq = Analysis.Frequencies(acf, xChart);
			double deltaF = (xchartfourie[1] - xchartfourie[0]) / xchartfourie.Length;
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					line[x] = picture[x, y];
				}
				convline = Transformations.ConwolutionWithBsf(line, freq[0], freq[1] / 2.0, 64, 1);
				for (int x = 0; x < image.Width; x++)
				{
					withoutgrid[x, y] = convline[x + 64];
				}
			}
			double max = 0;

			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
					if (max < withoutgrid[x, y])
						max = withoutgrid[x, y];
			}
			double min = max;
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
					if (min > withoutgrid[x, y])
						min = withoutgrid[x, y];
			}
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
					withoutgrid[x, y] = ((withoutgrid[x, y] - min) / (max - min)) * 255;
			}
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)withoutgrid[x, y], (int)withoutgrid[x, y], (int)withoutgrid[x, y]);
					image.SetPixel(x, y, color);
				}
			}

			pictureBox2.Image = image;
		}

		private void button19_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			ushort[,] picture = new ushort[image.Width, image.Height];
			picture = ImageByteWorker.BitmapToUshortRgbNaive(image);
			double[] derivative = new double[image.Height];
			for (int y = 0; y < image.Height - 1; y++)
			{
				derivative[y] = picture[0, y + 1] - picture[0, y];
			}
			double[] ac = new double[image.Height];
			for (int y = 0; y < image.Height; y++)
				ac[y] = Analysis.Autocorrelation(derivative, y);
			double[] xChart = new double[image.Height];
			for (int i = 0; i < xChart.Length; i++)
			{
				xChart[i] = i;
			}
			Complex[] acfourie = new Complex[image.Height];
			double[] acfourienorm = new double[image.Height / 2];
			acfourie = Analysis.ForwardFurie(ac);
			for (int i = 0; i < image.Height / 2; i++)
				acfourienorm[i] = Math.Sqrt(Math.Pow(acfourie[i].Real, 2) + Math.Pow(acfourie[i].Imaginary, 2));


			double[] xchartfourie = Analysis.FurieXTransform(xChart);
			chart1.ChartAreas[0].AxisX.Minimum = 0;
			chart1.ChartAreas[0].AxisX.Maximum = image.Height;
			chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			chart1.ChartAreas[0].AxisY.Minimum = 0 - ac.Max();
			chart1.ChartAreas[0].AxisY.Maximum = ac.Max();
			chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart1.Series[0].Points.DataBindXY(xChart, ac);
			chart3.ChartAreas[0].AxisX.Minimum = 0;
			chart3.ChartAreas[0].AxisX.Maximum = xchartfourie[xchartfourie.Length - 1];
			chart3.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			chart3.ChartAreas[0].AxisY.Minimum = 0;
			chart3.ChartAreas[0].AxisY.Maximum = acfourienorm.Max();
			chart3.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart3.Series[0].Points.DataBindXY(xchartfourie, acfourienorm);
		}

		private void button20_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			ushort[,] picture = new ushort[image.Width, image.Height];
			picture = ImageByteWorker.BitmapToUshortRgbNaive(image);
			double[] derivative1 = new double[image.Height];
			double[] derivative2 = new double[image.Height];
			for (int y = 0; y < image.Height - 1; y++)
			{
				derivative1[y] = picture[10, y + 1] - picture[10, y];
			}
			for (int y = 0; y < image.Height - 1; y++)
			{
				derivative2[y] = picture[40, y + 1] - picture[40, y];
			}

			double[] ac = new double[image.Height];
			for (int y = 0; y < image.Height; y++)
				ac[y] = Analysis.Crosscorrelation(derivative1, derivative2, y);
			double[] xChart = new double[image.Height];
			for (int i = 0; i < xChart.Length; i++)
			{
				xChart[i] = i;
			}
			Complex[] acfourie = new Complex[image.Height];
			Complex[] acfourienorm = new Complex[image.Height / 2];
			acfourie = Analysis.ForwardFurie(ac);
			for (int i = 0; i < image.Height / 2; i++)
				acfourienorm[i] = acfourie[i];
			double max = 0;
			for (int i = 0; i < image.Height / 2; i++)
				if (max < Math.Sqrt(Math.Pow(acfourienorm[i].Real, 2) + Math.Pow(acfourienorm[i].Imaginary, 2)))
					max = Math.Sqrt(Math.Pow(acfourienorm[i].Real, 2) + Math.Pow(acfourienorm[i].Imaginary, 2));
			double[] xchartfourie = Analysis.FurieXTransform(xChart);
			double[] furieac = new double[image.Height / 2];
			for (int i = 0; i < image.Height / 2; i++)
				furieac[i] = Math.Sqrt(Math.Pow(acfourienorm[i].Real, 2) + Math.Pow(acfourienorm[i].Imaginary, 2));
			chart1.ChartAreas[0].AxisX.Minimum = 0;
			chart1.ChartAreas[0].AxisX.Maximum = image.Height;
			chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			chart1.ChartAreas[0].AxisY.Minimum = 0 - ac.Max();
			chart1.ChartAreas[0].AxisY.Maximum = ac.Max();
			chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart1.Series[0].Points.DataBindXY(xChart, ac);
			chart3.ChartAreas[0].AxisX.Minimum = 0;
			chart3.ChartAreas[0].AxisX.Maximum = xchartfourie[xchartfourie.Length - 1];
			chart3.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			chart3.ChartAreas[0].AxisY.Minimum = 0;
			chart3.ChartAreas[0].AxisY.Maximum = max;
			chart3.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart3.Series[0].Points.DataBindXY(xchartfourie, furieac);
		}

		private void button21_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			ushort[,] picture = new ushort[image.Width, image.Height];
			picture = ImageByteWorker.BitmapToUshortRgbNaive(image);
			double[] derivative = new double[image.Height];
			for (int y = 0; y < image.Height - 1; y++)
			{
				derivative[y] = picture[100, y + 1] - picture[100, y];
			}
			double[] ac = new double[image.Height];
			for (int y = 0; y < image.Height; y++)
				ac[y] = Analysis.Autocorrelation(derivative, y);
			double[] xChart = new double[image.Height];
			for (int i = 0; i < ac.Length; i++)
			{
				xChart[i] = i;
			}
			Complex[] acfourie = new Complex[image.Height];
			Complex[] acfourienorm = new Complex[image.Height / 2];
			for (int i = 0; i < image.Height / 2; i++)
				acfourienorm[i] = acfourie[i];
			acfourie = Analysis.ForwardFurie(ac);
			for (int i = 0; i < image.Height / 2; i++)
				acfourienorm[i] = acfourie[i];
			double[] furieac = new double[image.Height / 2];
			for (int i = 0; i < image.Height / 2; i++)
				furieac[i] = Math.Sqrt(Math.Pow(acfourienorm[i].Real, 2) + Math.Pow(acfourienorm[i].Imaginary, 2));
			double[] xchartfourie = Analysis.FurieXTransform(xChart);
			double[,] withoutgrid = new double[image.Width, image.Height];
			double[] line = new double[image.Height];
			double[] convline = new double[image.Height];
			double[] acf = new double[acfourie.Length];
			for (int i = 0; i < acf.Length; i++)
				acf[i] = Math.Sqrt(Math.Pow(acfourie[i].Real, 2) + Math.Pow(acfourie[i].Imaginary, 2));
			double[] freq = Analysis.Frequencies(acf, xChart);
			double deltaF = (xchartfourie[1] - xchartfourie[0]) / xchartfourie.Length;
			for (int x = 0; x < image.Width; x++)
			{
				for (int y = 0; y < image.Height; y++)
				{
					line[y] = picture[x, y];
				}
				convline = Transformations.ConwolutionWithBsf(line, freq[0], freq[1] / 2.0, 64, 1);
				for (int y = 0; y < image.Height; y++)
				{
					withoutgrid[x, y] = convline[y + 64];
				}
			}
			double max = 0;

			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
					if (max < withoutgrid[x, y])
						max = withoutgrid[x, y];
			}
			double min = max;
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
					if (min > withoutgrid[x, y])
						min = withoutgrid[x, y];
			}
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
					withoutgrid[x, y] = ((withoutgrid[x, y] - min) / (max - min)) * 255;
			}
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)withoutgrid[x, y], (int)withoutgrid[x, y], (int)withoutgrid[x, y]);
					image.SetPixel(x, y, color);
				}
			}

			pictureBox2.Image = image;
		}

		private void button22_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			//image = ImageByteWorker.Gray(image);
			ushort[,] picture = new ushort[image.Width, image.Height];
			ushort[,] newpicture1 = new ushort[image.Width, image.Height];
			picture = ImageByteWorker.BitmapToUshortRgbNaive(image);
			newpicture1 = Transformations.StepFunction(picture, 190, 190);
			Bitmap newimage1 = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < newimage1.Height; y++)
				for (int x = 0; x < newimage1.Width; x++)
				{
					Color color = Color.FromArgb(newpicture1[x, y], newpicture1[x, y], newpicture1[x, y]);
					newimage1.SetPixel(x, y, color);
				}
			//newimage1 = ImageByteWorker.Gray(newimage1);
			newpicture1 = ImageByteWorker.BitmapToUshortRgbNaive(newimage1);
			pictureBox2.Image = newimage1;
			double[] line = new double[image.Width];
			double[] convline = new double[image.Width + 33];
			ushort[,] newpicture2 = new ushort[newimage1.Width, newimage1.Height];
			Bitmap newimage2 = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < newimage2.Height; y++)
			{
				for (int x = 0; x < newimage2.Width; x++)
				{
					line[x] = newpicture1[x, y];
				}
				convline = Transformations.ConwolutionWithLpf(line, 0.05, 16, 1.0 / image.Width);
				for (int x = 0; x < image.Width; x++)
				{
					newpicture2[x, y] = (ushort)convline[x + 16];
				}
			}

			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb(newpicture2[x, y], newpicture2[x, y], newpicture2[x, y]);
					newimage2.SetPixel(x, y, color);
				}
			//newimage2 = ImageByteWorker.Gray(newimage2);
			newpicture2 = ImageByteWorker.BitmapToUshortRgbNaive(newimage2);
			pictureBox3.Image = newimage2;
			ushort[,] newpicture3 = new ushort[newimage1.Width, newimage1.Height];
			Bitmap newimage3 = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < newimage3.Height; y++)
				for (int x = 0; x < newimage3.Width; x++)
				{
					if (newpicture2[x, y] > newpicture1[x, y])
						newpicture3[x, y] = (ushort)(newpicture2[x, y] - newpicture1[x, y]);
				}
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb(newpicture3[x, y], newpicture3[x, y], newpicture3[x, y]);
					newimage3.SetPixel(x, y, color);
				}
			//newimage3 = ImageByteWorker.Gray(newimage3);
			newpicture3 = ImageByteWorker.BitmapToUshortRgbNaive(newimage3);
			pictureBox4.Image = newimage3;
			Bitmap newimage4 = new Bitmap(image.Width, image.Height);
			ushort[,] newpicture4 = Transformations.StepFunction(newpicture3, 90, 90);
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb(newpicture4[x, y], newpicture4[x, y], newpicture4[x, y]);
					newimage4.SetPixel(x, y, color);
				}
			//newimage4 = ImageByteWorker.Gray(newimage4);
			newpicture4 = ImageByteWorker.BitmapToUshortRgbNaive(newimage4);
			pictureBox5.Image = newimage4;
		}

		private void button23_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			//image = ImageByteWorker.Gray(image);
			ushort[,] picture = new ushort[image.Width, image.Height];
			ushort[,] newpicture1 = new ushort[image.Width, image.Height];
			picture = ImageByteWorker.BitmapToUshortRgbNaive(image);
			newpicture1 = Transformations.StepFunction(picture, 128, 200);
			Bitmap newimage1 = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < newimage1.Height; y++)
				for (int x = 0; x < newimage1.Width; x++)
				{
					Color color = Color.FromArgb(newpicture1[x, y], newpicture1[x, y], newpicture1[x, y]);
					newimage1.SetPixel(x, y, color);
				}
			//newimage1 = ImageByteWorker.Gray(newimage1);
			newpicture1 = ImageByteWorker.BitmapToUshortRgbNaive(newimage1);
			pictureBox2.Image = newimage1;
			double[] line = new double[image.Width];
			double[] convline = new double[image.Width + 33];
			ushort[,] newpicture2 = new ushort[newimage1.Width, newimage1.Height];
			Bitmap newimage2 = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < newimage2.Height; y++)
			{
				for (int x = 0; x < newimage2.Width; x++)
				{
					line[x] = newpicture1[x, y];
				}
				convline = Transformations.ConwolutionWithLpf(line, 0.05, 16, 1.0 / image.Width);
				for (int x = 0; x < image.Width; x++)
				{
					newpicture2[x, y] = (ushort)convline[x + 16];
				}
			}

			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb(newpicture2[x, y], newpicture2[x, y], newpicture2[x, y]);
					newimage2.SetPixel(x, y, color);
				}
			//newimage2 = ImageByteWorker.Gray(newimage2);
			newpicture2 = ImageByteWorker.BitmapToUshortRgbNaive(newimage2);
			pictureBox3.Image = newimage2;
			ushort[,] newpicture3 = new ushort[newimage1.Width, newimage1.Height];
			Bitmap newimage3 = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < newimage3.Height; y++)
				for (int x = 0; x < newimage3.Width; x++)
				{
					if (newpicture2[x, y] > newpicture1[x, y])
						newpicture3[x, y] = (ushort)(newpicture2[x, y] - newpicture1[x, y]);
				}
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb(newpicture3[x, y], newpicture3[x, y], newpicture3[x, y]);
					newimage3.SetPixel(x, y, color);
				}
			//newimage3 = ImageByteWorker.Gray(newimage3);
			newpicture3 = ImageByteWorker.BitmapToUshortRgbNaive(newimage3);
			pictureBox4.Image = newimage3;
			Bitmap newimage4 = new Bitmap(image.Width, image.Height);
			ushort[,] newpicture4 = Transformations.StepFunction(newpicture3, 20, 20);
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb(newpicture4[x, y], newpicture4[x, y], newpicture4[x, y]);
					newimage4.SetPixel(x, y, color);
				}
			//newimage4 = ImageByteWorker.Gray(newimage4);
			newpicture4 = ImageByteWorker.BitmapToUshortRgbNaive(newimage4);
			pictureBox5.Image = newimage4;
		}

		private void button24_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			//image = ImageByteWorker.Gray(image);
			ushort[,] picture = new ushort[image.Width, image.Height];
			ushort[,] newpicture1 = new ushort[image.Width, image.Height];
			picture = ImageByteWorker.BitmapToUshortRgbNaive(image);
			newpicture1 = Transformations.StepFunction(picture, 200, 200);
			Bitmap newimage1 = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < newimage1.Height; y++)
				for (int x = 0; x < newimage1.Width; x++)
				{
					Color color = Color.FromArgb(newpicture1[x, y], newpicture1[x, y], newpicture1[x, y]);
					newimage1.SetPixel(x, y, color);
				}
			//newimage1 = ImageByteWorker.Gray(newimage1);
			newpicture1 = ImageByteWorker.BitmapToUshortRgbNaive(newimage1);
			pictureBox2.Image = newimage1;
			//newimage1 = ImageByteWorker.Gray(newimage1);
			newpicture1 = ImageByteWorker.BitmapToUshortRgbNaive(newimage1);
			pictureBox2.Image = newimage1;
			double[] line = new double[image.Width];
			double[] convline = new double[image.Width + 33];
			ushort[,] newpicture2 = new ushort[newimage1.Width, newimage1.Height];
			Bitmap newimage2 = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < newimage2.Height; y++)
			{
				for (int x = 0; x < newimage2.Width; x++)
				{
					line[x] = newpicture1[x, y];
				}
				convline = Transformations.ConwolutionWithHpf(line, 0.05, 16, 1.0 / image.Width);
				for (int x = 0; x < image.Width; x++)
				{
					newpicture2[x, y] = (ushort)convline[x + 16];
					if (newpicture2[x, y] > 255)
						newpicture2[x, y] = 0;
				}
			}
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb(newpicture2[x, y], newpicture2[x, y], newpicture2[x, y]);
					newimage2.SetPixel(x, y, color);
				}
			//newimage2 = ImageByteWorker.Gray(newimage2);
			newpicture2 = ImageByteWorker.BitmapToUshortRgbNaive(newimage2);
			pictureBox3.Image = newimage2;
			Bitmap newimage3 = new Bitmap(image.Width, image.Height);
			ushort[,] newpicture3 = Transformations.StepFunction(newpicture2, 30, 30);
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb(newpicture3[x, y], newpicture3[x, y], newpicture3[x, y]);
					newimage3.SetPixel(x, y, color);
				}
			//newimage3 = ImageByteWorker.Gray(newimage3);
			newpicture3 = ImageByteWorker.BitmapToUshortRgbNaive(newimage3);
			pictureBox4.Image = newimage3;
		}

		private void button25_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			double[][] picture = new double[image.Height][];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
				}
			}
			double[][] newpicture = new double[image.Width][];

			newpicture = Transformations.Gradient(picture);
			double max = Analysis.Max(picture);
			double min = Analysis.Min(picture);
			newpicture = Transformations.ThresholdFun(newpicture, 0, 255);
			Bitmap newimage = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture[y][x], (int)newpicture[y][x], (int)newpicture[y][x]);
					newimage.SetPixel(x, y, color);
				}
			pictureBox2.Image = newimage;
		}

		private void button26_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			double[][] picture = new double[image.Height][];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
				}
			}
			double[][] newpicture = new double[image.Width][];

			newpicture = Transformations.Laplasian(picture);
			double max = Analysis.Max(picture);
			double min = Analysis.Min(picture);
			newpicture = Transformations.ThresholdFun(newpicture, 0, 255);
			Bitmap newimage = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture[y][x], (int)newpicture[y][x], (int)newpicture[y][x]);
					newimage.SetPixel(x, y, color);
				}
			pictureBox2.Image = newimage;
		}

		private void button27_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			double[][] picture = new double[image.Height][];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
				}
			}
			double[][] newpicture = Transformations.AddRandom(picture, 25);
			Bitmap newimage = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture[y][x], (int)newpicture[y][x], (int)newpicture[y][x]);
					newimage.SetPixel(x, y, color);
				}
			pictureBox2.Image = newimage;
		}

		private void button28_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			double[][] picture = new double[image.Height][];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
				}
			}
			double[][] newpicture = Transformations.AddSP(picture, 10);
			Bitmap newimage = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture[y][x], (int)newpicture[y][x], (int)newpicture[y][x]);
					newimage.SetPixel(x, y, color);
				}
			pictureBox2.Image = newimage;
		}

		private void button29_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox2.Image;
			double[][] picture = new double[image.Height][];
			double[] autocorrelation = new double[image.Width];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
					if (y == 1)
						autocorrelation[x] = picture[y][x];
				}
			}
			for (int i = 0; i < autocorrelation.Length; i++)
				autocorrelation[i] = Analysis.Autocorrelation(autocorrelation, i);
			double[] xChart = new double[autocorrelation.Length];
			for (int i = 0; i < autocorrelation.Length; i++)
				xChart[i] = i;
			Complex[] Cfac = Analysis.ForwardFurie(autocorrelation);
			double[] fac = new double[Cfac.Length / 2];
			for (int i = 0; i < Cfac.Length / 2; i++)
			{
				fac[i] = Math.Sqrt(Math.Pow(Cfac[i].Real, 2) + Math.Pow(Cfac[i].Imaginary, 2));
			}
			double[] fxc = Analysis.FurieXTransform(xChart);
			chart3.Series[0].Points.DataBindXY(fxc, fac);
		}

		private void button30_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox2.Image;
			double[][] picture = new double[image.Height][];
			double[] autocorrelation = new double[image.Width];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
					autocorrelation[x] = picture[y][x];
				}
			}
			double[] autocorrelationfourie = Analysis.FurieFunction(autocorrelation);
			double[] acf = new double[autocorrelation.Length / 2];
			double[] line = new double[image.Width];
			double[] convline = new double[line.Length];
			double[][] newpicture = new double[image.Height][];
			double[] xchart = new double[autocorrelation.Length];
			for (int i = 0; i < xchart.Length; i++)
				xchart[i] = i;
			double[] freq = Analysis.Frequencies(autocorrelationfourie, xchart);
			for (int y = 0; y < image.Height; y++)
			{
				newpicture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					line[x] = picture[y][x];
				}
				convline = Transformations.ConwolutionWithLpf(line, 0.1, 32, 1);
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = convline[x + 32];
				}
			}
			Bitmap newimage = new Bitmap(image.Width, image.Height);
			double max = Analysis.Max(newpicture);
			double min = Analysis.Min(newpicture);
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = ((newpicture[y][x] - min) / (max - min)) * 255;
				}
			}
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture[y][x], (int)newpicture[y][x], (int)newpicture[y][x]);
					newimage.SetPixel(x, y, color);
				}
			pictureBox3.Image = newimage;
		}

		private void button31_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox2.Image;
			double[][] picture = new double[image.Height][];
			double[] autocorrelation = new double[image.Width];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
					autocorrelation[x] = picture[y][x];
				}
			}
			double[] autocorrelationfourie = Analysis.FurieFunction(autocorrelation);
			double[] acf = new double[autocorrelation.Length / 2];
			double[] line = new double[image.Width];
			double[] convline = new double[line.Length];
			double[][] newpicture = new double[image.Height][];
			double[] xchart = new double[autocorrelation.Length];
			for (int i = 0; i < xchart.Length; i++)
				xchart[i] = i;
			double[] freq = Analysis.Frequencies(autocorrelationfourie, xchart);
			for (int y = 0; y < image.Height; y++)
			{
				newpicture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					line[x] = picture[y][x];
				}
				convline = Transformations.ConwolutionWithLpf(line, 0.05, 32, 1);
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = convline[x + 32];
				}
			}
			Bitmap newimage = new Bitmap(image.Width, image.Height);
			double max = Analysis.Max(newpicture);
			double min = Analysis.Min(newpicture);
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = ((newpicture[y][x] - min) / (max - min)) * 255;
				}
			}
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture[y][x], (int)newpicture[y][x], (int)newpicture[y][x]);
					newimage.SetPixel(x, y, color);
				}
			pictureBox3.Image = newimage;
		}

		private void button32_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox2.Image;
			double[][] picture = new double[image.Height][];
			double[] autocorrelation = new double[image.Width];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
					autocorrelation[x] = picture[y][x];
				}
			}
			double[] autocorrelationfourie = Analysis.FurieFunction(autocorrelation);
			double[] acf = new double[autocorrelation.Length / 2];
			double[] line = new double[image.Width];
			double[] convline = new double[line.Length];
			double[][] newpicture = new double[image.Height][];
			double[] xchart = new double[autocorrelation.Length];
			for (int i = 0; i < xchart.Length; i++)
				xchart[i] = i;
			double[] freq = Analysis.Frequencies(autocorrelationfourie, xchart);
			for (int y = 0; y < image.Height; y++)
			{
				newpicture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					line[x] = picture[y][x];
				}
				convline = Transformations.ConwolutionWithHpf(line, 0.1, 32, 1);
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = convline[x + 32];
				}
			}
			Bitmap newimage = new Bitmap(image.Width, image.Height);

			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = picture[y][x] - newpicture[y][x];
				}
			}
			double max = Analysis.Max(newpicture);
			double min = Analysis.Min(newpicture);
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = ((newpicture[y][x] - min) / (max - min)) * 255;
				}
			}
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture[y][x], (int)newpicture[y][x], (int)newpicture[y][x]);
					newimage.SetPixel(x, y, color);
				}
			pictureBox3.Image = newimage;
		}

		private void button33_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox2.Image;
			double[][] picture = new double[image.Height][];
			double[] autocorrelation = new double[image.Width];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
					autocorrelation[x] = picture[y][x];
				}
			}
			double[] autocorrelationfourie = Analysis.FurieFunction(autocorrelation);
			double[] acf = new double[autocorrelation.Length / 2];
			double[] line = new double[image.Width];
			double[] convline = new double[line.Length];
			double[][] newpicture = new double[image.Height][];
			double[] xchart = new double[autocorrelation.Length];
			for (int i = 0; i < xchart.Length; i++)
				xchart[i] = i;
			double[] freq = Analysis.Frequencies(autocorrelationfourie, xchart);
			for (int y = 0; y < image.Height; y++)
			{
				newpicture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					line[x] = picture[y][x];
				}
				convline = Transformations.ConwolutionWithHpf(line, 0.05, 32, 1);
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = convline[x + 32];
				}
			}
			Bitmap newimage = new Bitmap(image.Width, image.Height);

			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = picture[y][x] - newpicture[y][x];
				}
			}
			double max = Analysis.Max(newpicture);
			double min = Analysis.Min(newpicture);
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = ((newpicture[y][x] - min) / (max - min)) * 255;
				}
			}
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture[y][x], (int)newpicture[y][x], (int)newpicture[y][x]);
					newimage.SetPixel(x, y, color);
				}
			pictureBox3.Image = newimage;
		}

		private void button34_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox2.Image;
			double[][] picture = new double[image.Height][];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
				}
			}
			double[][] newpicture = Transformations.AvgFilt(picture, 3);
			Bitmap newimage = new Bitmap(image.Width, image.Height);
			double max = Analysis.Max(newpicture);
			double min = Analysis.Min(newpicture);
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = ((newpicture[y][x] - min) / (max - min)) * 255;
				}
			}
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture[y][x], (int)newpicture[y][x], (int)newpicture[y][x]);
					newimage.SetPixel(x, y, color);
				}
			pictureBox3.Image = newimage;
		}

		private void button35_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			double[][] picture = new double[image.Height][];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
				}
			}
			double[][] newpicture = Transformations.MedFilt(picture, 2);
			Bitmap newimage = new Bitmap(image.Width, image.Height);
			double max = Analysis.Max(newpicture);
			double min = Analysis.Min(newpicture);
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = ((newpicture[y][x] - min) / (max - min)) * 255;
				}
			}
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture[y][x], (int)newpicture[y][x], (int)newpicture[y][x]);
					newimage.SetPixel(x, y, color);
				}
			pictureBox2.Image = newimage;
		}

		private void button36_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			double[][] picture = new double[image.Height][];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
				}
			}
			double[][] newpicture = Transformations.Erosion(picture, 3, 100);
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					newpicture[y][x] = picture[y][x] - newpicture[y][x];
				}
			}

			Bitmap newimage = new Bitmap(image.Width, image.Height);
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture[y][x], (int)newpicture[y][x], (int)newpicture[y][x]);
					newimage.SetPixel(x, y, color);
				}
			//newimage = ImageByteWorker.Gray(newimage);
			pictureBox2.Image = newimage;
			double[][] newpicture1 = Transformations.Dilatation(picture, 3, 100);
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					newpicture1[y][x] = newpicture1[y][x] - picture[y][x];
				}
			}
			Bitmap newimage1 = new Bitmap(image.Width, image.Height);

			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)newpicture1[y][x], (int)newpicture1[y][x], (int)newpicture1[y][x]);
					newimage1.SetPixel(x, y, color);
				}
			//newimage1 = ImageByteWorker.Gray(newimage);
			pictureBox3.Image = newimage;
		}

		private void button37_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox2.Image;
			ushort[,] picture = new ushort[image.Width, image.Height];
			picture = ImageByteWorker.BitmapToUshortRgbNaive(image);
			double[] derivative = new double[image.Width];
			for (int x = 0; x < image.Width - 1; x++)
			{
				derivative[x] = picture[x + 1, 10] - picture[x, 10];
			}
			double[] ac = new double[image.Width];
			for (int x = 0; x < image.Width; x++)
				ac[x] = Analysis.Autocorrelation(derivative, x);
			double[] xChart = new double[image.Width];
			for (int i = 0; i < xChart.Length; i++)
			{
				xChart[i] = i;
			}
			Complex[] acfourie = new Complex[image.Width];
			double[] acfourienorm = new double[image.Width / 2];
			acfourie = Analysis.ForwardFurie(ac);
			for (int i = 0; i < image.Width / 2; i++)
				acfourienorm[i] = Math.Sqrt(Math.Pow(acfourie[i].Real, 2) + Math.Pow(acfourie[i].Imaginary, 2));


			double[] xchartfourie = Analysis.FurieXTransform(xChart);
			//chart1.ChartAreas[0].AxisX.Minimum = 0;
			//chart1.ChartAreas[0].AxisX.Maximum = image.Width;
			//chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			//chart1.ChartAreas[0].AxisY.Minimum = 0-ac.Max();
			//chart1.ChartAreas[0].AxisY.Maximum = ac.Max();
			//chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart3.Series[0].Points.DataBindXY(xChart, ac);
			//chart3.ChartAreas[0].AxisX.Minimum = 0;
			//chart3.ChartAreas[0].AxisX.Maximum = xchartfourie[xchartfourie.Length-1];
			//chart3.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			//chart3.ChartAreas[0].AxisY.Minimum = 0;
			//chart3.ChartAreas[0].AxisY.Maximum = acfourienorm.Max();
			//chart3.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart4.Series[0].Points.DataBindXY(xchartfourie, acfourienorm);
		}

		private void button38_Click(object sender, EventArgs e)
		{
			pictureBox1.Image = null;
			pictureBox2.Image = null;
			pictureBox3.Image = null;
			pictureBox4.Image = null;
			pictureBox5.Image = null;
			chart1.Series[0].Points.Clear();
			chart2.Series[0].Points.Clear();
			chart3.Series[0].Points.Clear();
			chart4.Series[0].Points.Clear();
		}

		private void button39_Click(object sender, EventArgs e)
		{
			Bitmap image = (Bitmap)pictureBox1.Image;
			float[] colors = new float[256];
			int[] chartX = new int[256];
			for (int i = 0; i < 255; i++)
			{
				chartX[i] = i;
			}
			float maxY = 0;
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color r = image.GetPixel(x, y);
					int color = r.R;
					colors[color] = colors[color] + 1;
					if (maxY < colors[color])
					{
						maxY = colors[color];
					}
				}
			chart1.ChartAreas[0].AxisX.Minimum = 0;
			chart1.ChartAreas[0].AxisX.Maximum = 255;
			chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
			chart1.ChartAreas[0].AxisY.Minimum = 0;
			chart1.ChartAreas[0].AxisY.Maximum = maxY;
			chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
			chart1.Series[0].Points.DataBindXY(chartX, colors);
			int min = -1;
			int max = -1;
			for (int i = 0; i < colors.Length; i++)
			{
				if (colors[i] > 0 && min == -1)
				{
					min = i;
				}
				if (colors[i] > 0)
				{
					max = i;
				}
			}
			double[][] picture = new double[image.Height][];
			for (int y = 0; y < image.Height; y++)
			{
				picture[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					Color color = image.GetPixel(x, y);
					picture[y][x] = color.R;
				}
			}

			List<int> Step = new List<int> { };
			int T = (min + max) / 2;
			int mu1 = 0;
			int div = 0;
			for (int i = min; i <= T; i++)
			{
				mu1 += (int)colors[i] * i;
				div += (int)colors[i];
			}
			mu1 = mu1 / div;
			int mu2 = 0;
			div = 0;
			for (int i = T + 1; i < max; i++)
			{
				mu2 += (int)colors[i] * i;
				div += (int)colors[i];
			}
			mu2 = mu2 / div;
			Step.Add((int)((mu1 + mu2) / 2));

			if (Step[0] - T <= 1)
			{
				double[][] newpicture = new double[image.Height][];
				for (int y = 0; y < image.Height; y++)
				{
					newpicture[y] = new double[image.Width];
					for (int x = 0; x < image.Width; x++)
					{
						Color color = image.GetPixel(x, y);
						newpicture[y][x] = color.R;
					}
				}
				Transformations.ThresholdFun(newpicture, Step[0], Step[0]);

				Bitmap newimage = new Bitmap(image);
				for (int y = 0; y < newimage.Height; y++)
					for (int x = 0; x < newimage.Width; x++)
					{
						Color color = Color.FromArgb((int)newpicture[x][y], (int)newpicture[x][y], (int)newpicture[x][y]);
						newimage.SetPixel(x, y, color);
					}
				pictureBox2.Image = newimage;

			}
			else
			{
				mu1 = 0;
				div = 0;
				for (int i = min; i <= T; i++)
				{
					mu1 += (int)colors[i] * i;
					div += (int)colors[i];
				}
				mu1 = mu1 / div;
				mu2 = 0;
				div = 0;
				for (int i = T + 1; i < max; i++)
				{
					mu2 += (int)colors[i] * i;
					div += (int)colors[i];
				}
				mu2 = mu2 / div;
				Step.Add((int)((mu1 + mu2) / 2));
				while (Step[Step.Count - 1] - Step[Step.Count - 2] > 1)
				{
					mu1 = 0;
					div = 0;
					for (int i = min; i <= T; i++)
					{
						mu1 += (int)colors[i] * i;
						div += (int)colors[i];
					}
					mu1 = mu1 / div;
					mu2 = 0;
					div = 0;
					for (int i = T + 1; i < max; i++)
					{
						mu2 += (int)colors[i] * i;
						div += (int)colors[i];
					}
					mu2 = mu2 / div;
					Step.Add((int)((mu1 + mu2) / 2));
				}

				double[][] newpicture = new double[image.Height][];
				for (int y = 0; y < image.Height; y++)
				{
					newpicture[y] = new double[image.Width];
					for (int x = 0; x < image.Width; x++)
					{
						Color color = image.GetPixel(x, y);
						newpicture[y][x] = color.R;
					}
				}
				Transformations.ThresholdFun(newpicture, Step[Step.Count - 1], 255);

				Bitmap newimage = new Bitmap(image);
				for (int y = 0; y < newimage.Height; y++)
					for (int x = 0; x < newimage.Width; x++)
					{
						Color color = Color.FromArgb((int)newpicture[x][y], (int)newpicture[x][y], (int)newpicture[x][y]);
						newimage.SetPixel(x, y, color);
					}
				pictureBox2.Image = newimage;
				int[] colors1 = new int[256];
				for (int y = 0; y < image.Height; y++)
					for (int x = 0; x < image.Width; x++)
					{
						Color r = newimage.GetPixel(x, y);
						int color = r.R;
						colors1[color] = colors1[color] + 1;
					}
				chart2.ChartAreas[0].AxisX.Minimum = 0;
				chart2.ChartAreas[0].AxisX.Maximum = 255;
				chart2.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
				chart2.ChartAreas[0].AxisY.Minimum = 0;
				chart2.ChartAreas[0].AxisY.Maximum = maxY;
				chart2.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
				chart2.Series[0].Points.DataBindXY(chartX, colors1);
				int maxT = Step[Step.Count - 1];
				min = -1;
				max = -1;
				for (int i = 0; i < Step[Step.Count - 1]; i++)
				{
					if (colors[i] > 0 && min == -1)
					{
						min = i;
					}
					if (colors[i] > 0)
					{
						max = i;
					}
				}
				Step.Clear();
				T = (min + max) / 2;
				mu1 = 0;
				div = 0;
				for (int i = min; i <= T; i++)
				{
					mu1 += (int)colors[i] * i;
					div += (int)colors[i];
				}
				mu1 = mu1 / div;
				mu2 = 0;
				div = 0;
				for (int i = T + 1; i < max; i++)
				{
					mu2 += (int)colors[i] * i;
					div += (int)colors[i];
				}
				mu2 = mu2 / div;
				Step.Add((int)((mu1 + mu2) / 2));

				if (Step[0] - T <= 1)
				{
					double[][] newpicture1 = new double[image.Height][];
					for (int y = 0; y < image.Height; y++)
					{
						newpicture1[y] = new double[image.Width];
						for (int x = 0; x < image.Width; x++)
						{
							Color color = image.GetPixel(x, y);
							newpicture1[y][x] = color.R;
						}
					}
					Transformations.ThresholdFun(newpicture1, Step[0], Step[0]);

					Bitmap newimage1 = new Bitmap(image);
					for (int y = 0; y < newimage.Height; y++)
						for (int x = 0; x < newimage.Width; x++)
						{
							Color color = Color.FromArgb((int)newpicture1[x][y], (int)newpicture1[x][y], (int)newpicture1[x][y]);
							newimage1.SetPixel(x, y, color);
						}
					pictureBox3.Image = newimage1;

				}
				else
				{
					mu1 = 0;
					div = 0;
					for (int i = min; i <= T; i++)
					{
						mu1 += (int)colors[i] * i;
						div += (int)colors[i];
					}
					mu1 = mu1 / div;
					mu2 = 0;
					div = 0;
					for (int i = T + 1; i < max; i++)
					{
						mu2 += (int)colors[i] * i;
						div += (int)colors[i];
					}
					mu2 = mu2 / div;
					Step.Add((int)((mu1 + mu2) / 2));
					while (Step[Step.Count - 1] - Step[Step.Count - 2] > 1)
					{
						mu1 = 0;
						div = 0;
						for (int i = min; i <= T; i++)
						{
							mu1 += (int)colors[i] * i;
							div += (int)colors[i];
						}
						mu1 = mu1 / div;
						mu2 = 0;
						div = 0;
						for (int i = T + 1; i < max; i++)
						{
							mu2 += (int)colors[i] * i;
							div += (int)colors[i];
						}
						mu2 = mu2 / div;
						Step.Add((int)((mu1 + mu2) / 2));
					}
					double[][] newpicture1 = new double[image.Height][];
					for (int y = 0; y < image.Height; y++)
					{
						newpicture1[y] = new double[image.Width];
						for (int x = 0; x < image.Width; x++)
						{
							Color color = image.GetPixel(x, y);
							newpicture1[y][x] = color.R;
						}
					}
					Transformations.ThresholdFun(newpicture1, Step[Step.Count - 1], 255);

					Bitmap newimage1 = new Bitmap(image);
					for (int y = 0; y < newimage.Height; y++)
						for (int x = 0; x < newimage.Width; x++)
						{
							Color color = Color.FromArgb((int)newpicture1[x][y], (int)newpicture1[x][y], (int)newpicture1[x][y]);
							newimage1.SetPixel(x, y, color);
						}
					pictureBox3.Image = newimage1;
					//	double[][] newpicture2 = new double[image.Height][];
					//	for (int y = 0; y < image.Height; y++)
					//	{
					//		newpicture2[y] = new double[image.Width];
					//		for (int x = 0; x < image.Width; x++)
					//		{
					//			newpicture2[y][x] = picture[y][x] - newpicture1[y][x];
					//		}
					//	}
					//	Bitmap newimage2 = new Bitmap(image);
					//	Transformations.ThresholdFun(newpicture2, 0, 1);
					//	for (int y = 0; y < newimage.Height; y++)
					//		for (int x = 0; x < newimage.Width; x++)
					//		{
					//			Color color = Color.FromArgb((int)newpicture2[x][y], (int)newpicture2[x][y], (int)newpicture2[x][y]);
					//			newimage2.SetPixel(x, y, color);
					//		}

					//	pictureBox4.Image = newimage2;
					//	double[][] newpicture3 = new double[image.Height][];
					//	for (int y = 0; y < image.Height; y++)
					//	{
					//		newpicture3[y] = new double[image.Width];
					//		for (int x = 0; x < image.Width; x++)
					//		{
					//			newpicture3[y][x] = newpicture1[y][x] + newpicture2[y][x];
					//		}
					//	}
					//	Bitmap newimage3 = new Bitmap(image);
					//	for (int y = 0; y < newimage.Height; y++)
					//		for (int x = 0; x < newimage.Width; x++)
					//		{
					//			Color color = Color.FromArgb((int)newpicture3[x][y], (int)newpicture3[x][y], (int)newpicture3[x][y]);
					//			newimage3.SetPixel(x, y, color);
					//		}

					//	pictureBox5.Image = newimage3;	
					int maskWidth = 5;
					double[][] result = new double[image.Height][];
					int maxMi = maskWidth / 2;
					int maxMj = maskWidth / 2;
					for (int i = 0; i < image.Height; i++)
					{
						result[i] = new double[image.Height];
						//++
						for (int j = 0; j < image.Width; j++)
						{
							double value = newpicture1[i][j];
							if (i >= maxMi && i < image.Width - maxMi && j >= maxMj && j < image.Height - maxMj)
							{
								double[][] range = newpicture1.Skip(i - maxMi).Take(maskWidth)
									.Select(x => x.Skip(j - maxMj).Take(maskWidth).ToArray())
									.ToArray();


								for (int mi = 0; mi < range.Length - 1; mi++)
								{
									for (int mj = 0; mj < range[mi].Length - 1; mj++)
									{
										if (Math.Abs(range[mi][mj] - range[mi + 1][mj + 1]) < 30)
										{
											value = 0;
										}
										else
										{
											value = newpicture1[i][j];
											break;
										}
									}
								}


							}
							result[i][j] = value;
						}
					}
					Bitmap newimage2 = new Bitmap(image);
					for (int y = 0; y < image.Height; y++)
						for (int x = 0; x < image.Width; x++)
						{
							Color color = Color.FromArgb((int)result[x][y], (int)result[x][y], (int)result[x][y]);
							newimage2.SetPixel(x, y, color);
						}
					pictureBox4.Image = newimage2;
				}
			}

		}


		private void button40_Click(object sender, EventArgs e)
		{
			string a = textBox1.Text;

			Bitmap image = null;
			switch (a)
			{
				case ("1"):
					{
						image = (Bitmap)pictureBox1.Image;
						break;
					}
				case ("2"):
					{
						image = (Bitmap)pictureBox2.Image;
						break;
					}
				case ("3"):
					{
						image = (Bitmap)pictureBox3.Image;
						break;
					}
				case ("4"):
					{
						image = (Bitmap)pictureBox4.Image;
						break;
					}
				case ("5"):
					{
						image = (Bitmap)pictureBox5.Image;
						break;
					}
			}
			if (pictureBox1.Image != null) //если в pictureBox есть изображение
			{
				//создание диалогового окна "Сохранить как..", для сохранения изображения
				SaveFileDialog savedialog = new SaveFileDialog();
				savedialog.Title = "Сохранить картинку как...";
				//отображать ли предупреждение, если пользователь указывает имя уже существующего файла
				savedialog.OverwritePrompt = true;
				//отображать ли предупреждение, если пользователь указывает несуществующий путь
				savedialog.CheckPathExists = true;
				//список форматов файла, отображаемый в поле "Тип файла"
				savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
				//отображается ли кнопка "Справка" в диалоговом окне
				savedialog.ShowHelp = true;
				if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
				{
					try
					{
						image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
					}
					catch
					{
						MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void button41_Click(object sender, EventArgs e)
		{
			string a, b, c;
			a = text1.Text;
			b = text2.Text;
			c = text3.Text;
			Bitmap image1 = new Bitmap(pictureBox1.Image);
			Bitmap image2 = new Bitmap(pictureBox1.Image);
			Bitmap image3 = new Bitmap(pictureBox1.Image);
			switch (a)
			{
				case ("1"):
					{
						image1 = (Bitmap)pictureBox1.Image;
						break;
					}
				case ("2"):
					{
						image1 = (Bitmap)pictureBox2.Image;
						break;
					}
				case ("3"):
					{
						image1 = (Bitmap)pictureBox3.Image;
						break;
					}
				case ("4"):
					{
						image1 = (Bitmap)pictureBox4.Image;
						break;
					}
				case ("5"):
					{
						image1 = (Bitmap)pictureBox5.Image;
						break;
					}
				case ("6"):
					{
						image1 = (Bitmap)pictureBox6.Image;
						break;
					}
			}
			switch (b)
			{
				case ("1"):
					{
						image2 = (Bitmap)pictureBox1.Image;
						break;
					}
				case ("2"):
					{
						image2 = (Bitmap)pictureBox2.Image;
						break;
					}
				case ("3"):
					{
						image2 = (Bitmap)pictureBox3.Image;
						break;
					}
				case ("4"):
					{
						image2 = (Bitmap)pictureBox4.Image;
						break;
					}
				case ("5"):
					{
						image2 = (Bitmap)pictureBox5.Image;
						break;
					}
				case ("6"):
					{
						image2 = (Bitmap)pictureBox6.Image;
						break;
					}
			}
			switch (c)
			{
				case ("1"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								picture3[y][x] = picture1[y][x] - picture2[y][x];
							}
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox1.Image = image3;
						break;
					}
				case ("2"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								picture3[y][x] = picture1[y][x] - picture2[y][x];
							}
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox2.Image = image3;
						break;
					}
				case ("3"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								picture3[y][x] = picture1[y][x] - picture2[y][x];
							}
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
                                if (picture3[x][y] < 0)
                                    picture3[x][y] = 0;
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox3.Image = image3;
						break;
					}
				case ("4"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								picture3[y][x] = picture1[y][x] - picture2[y][x];
							}
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox4.Image = image3;
						break;
					}
				case ("5"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								picture3[y][x] = picture1[y][x] - picture2[y][x];
							}
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox5.Image = image3;
						break;
					}
				case ("6"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								picture3[y][x] = picture1[y][x] - picture2[y][x];
							}
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox6.Image = image3;
						break;
					}
			}
		}

		private void button42_Click(object sender, EventArgs e)
		{
			Bitmap image = new Bitmap(pictureBox1.Image);
			double[][] newpicture1 = new double[image.Height][];
			for (int y = 0; y < image.Height; y++)
			{
				newpicture1[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					newpicture1[y][x] = image.GetPixel(x, y).R;
				}
			}
			int maskWidth = 5;
			double[][] result = new double[image.Height][];
			int maxMi = maskWidth / 2;
			int maxMj = maskWidth / 2;
			for (int i = 0; i < image.Height; i++)
			{
				result[i] = new double[image.Height];
				//++
				for (int j = 0; j < image.Width; j++)
				{
					double value = newpicture1[i][j];
					//if (i >= maxMi && i < image.Width - maxMi && j >= maxMj && j < image.Height - maxMj)
					{
						double[][] range = newpicture1.Skip(i - maxMi).Take(maskWidth)
							.Select(x => x.Skip(j - maxMj).Take(maskWidth).ToArray())
							.ToArray();


						for (int mi = 0; mi < range.Length - 1; mi++)
						{
							for (int mj = 0; mj < range[mi].Length - 1; mj++)
							{
								if (Math.Abs(range[mi][mj] - range[mi + 1][mj + 1]) < 30)
								{
									value = 0;
								}
								else
								{
									value = newpicture1[i][j];
									break;
								}
							}
						}


					}
					result[i][j] = value;
				}
			}
			Bitmap newimage2 = new Bitmap(image);
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color color = Color.FromArgb((int)result[x][y], (int)result[x][y], (int)result[x][y]);
					newimage2.SetPixel(x, y, color);
				}
			pictureBox4.Image = newimage2;
		}

		private void button43_Click(object sender, EventArgs e)
		{
			string a, b, c;
			a = text1.Text;
			b = text2.Text;
			c = text3.Text;
			Bitmap image1 = new Bitmap(pictureBox1.Image);
			Bitmap image2 = new Bitmap(pictureBox1.Image);
			Bitmap image3 = new Bitmap(pictureBox1.Image);
			switch (a)
			{
				case ("1"):
					{
						image1 = (Bitmap)pictureBox1.Image;
						break;
					}
				case ("2"):
					{
						image1 = (Bitmap)pictureBox2.Image;
						break;
					}
				case ("3"):
					{
						image1 = (Bitmap)pictureBox3.Image;
						break;
					}
				case ("4"):
					{
						image1 = (Bitmap)pictureBox4.Image;
						break;
					}
				case ("5"):
					{
						image1 = (Bitmap)pictureBox5.Image;
						break;
					}
				case ("6"):
					{
						image1 = (Bitmap)pictureBox6.Image;
						break;
					}
			}
			switch (b)
			{
				case ("1"):
					{
						image2 = (Bitmap)pictureBox1.Image;
						break;
					}
				case ("2"):
					{
						image2 = (Bitmap)pictureBox2.Image;
						break;
					}
				case ("3"):
					{
						image2 = (Bitmap)pictureBox3.Image;
						break;
					}
				case ("4"):
					{
						image2 = (Bitmap)pictureBox4.Image;
						break;
					}
				case ("5"):
					{
						image2 = (Bitmap)pictureBox5.Image;
						break;
					}
				case ("6"):
					{
						image2 = (Bitmap)pictureBox6.Image;
						break;
					}
			}
			switch (c)
			{
				case ("1"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
                                if (picture1[y][x] == 0)
                                    picture3[y][x] = picture1[y][x] + picture2[y][x];
                                else
                                    picture3[y][x] = picture1[y][x];
                            }
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox1.Image = image3;
						break;
					}
				case ("2"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
                                if (picture1[y][x] == 0)
                                    picture3[y][x] = picture1[y][x] + picture2[y][x];
                                else
                                    picture3[y][x] = picture1[y][x];
                            }
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
								if(picture3[x][y]>255)
								{
									picture3[x][y] = 255;
								}
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox2.Image = image3;
						break;
					}
				case ("3"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
                                if (picture1[y][x] == 0)
                                    picture3[y][x] = picture1[y][x] + picture2[y][x];
                                else
                                    picture3[y][x] = picture1[y][x];
                            }
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox3.Image = image3;
						break;
					}
				case ("4"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
                                if (picture1[y][x] == 0)
                                    picture3[y][x] = picture1[y][x] + picture2[y][x];
                                else
                                    picture3[y][x] = picture1[y][x];
                            }
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox4.Image = image3;
						break;
					}
				case ("5"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
                                if (picture1[y][x] == 0)
                                    picture3[y][x] = picture1[y][x] + picture2[y][x];
                                else
                                    picture3[y][x] = picture1[y][x];
                            }
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox5.Image = image3;
						break;
					}
				case ("6"):
					{
						double[][] picture1 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture1[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = image1.GetPixel(x, y);
								picture1[y][x] = color.R;
							}
						}
						double[][] picture2 = new double[image2.Height][];
						for (int y = 0; y < image2.Height; y++)
						{
							picture2[y] = new double[image2.Width];
							for (int x = 0; x < image2.Width; x++)
							{
								Color color = image2.GetPixel(x, y);
								picture2[y][x] = color.R;
							}
						}
						double[][] picture3 = new double[image1.Height][];
						for (int y = 0; y < image1.Height; y++)
						{
							picture3[y] = new double[image1.Width];
							for (int x = 0; x < image1.Width; x++)
							{
                                if (picture1[y][x] == 0)
                                    picture3[y][x] = picture1[y][x] + picture2[y][x];
                                else
                                    picture3[y][x] = picture1[y][x];
                            }
						}
						for (int y = 0; y < image1.Height; y++)
						{
							for (int x = 0; x < image1.Width; x++)
							{
								Color color = Color.FromArgb((int)picture3[x][y], (int)picture3[x][y], (int)picture3[x][y]);
								image3.SetPixel(x, y, color);
							}
						}
						pictureBox6.Image = image3;
						break;

					}
			}
		}

		private void button44_Click(object sender, EventArgs e)
		{
			int count = 0;
			Bitmap image = (Bitmap)pictureBox1.Image;
			double[][] newpicture1 = new double[image.Height][];
			for (int y = 0; y < image.Height; y++)
			{
				newpicture1[y] = new double[image.Width];
				for (int x = 0; x < image.Width; x++)
				{
					newpicture1[y][x] = image.GetPixel(x, y).R;
				}
			}
			int maskWidth= 14;
			double[][] result = new double[image.Height][];
			int maxMi = maskWidth / 2;
			int maxMj = maskWidth / 2;
			for (int i = 0; i < image.Height; i++)
			{
				result[i] = new double[image.Height];
				//++
				for (int j = 0; j < image.Width; j++)
				{
					double value = newpicture1[i][j];
					if (i >= maxMi && i < image.Width - maxMi && j >= maxMj && j < image.Height - maxMj)
					{
						double[][] range = newpicture1.Skip(i - maxMi).Take(maskWidth)
							.Select(x => x.Skip(j - maxMj).Take(maskWidth).ToArray())
							.ToArray();

						bool exit = false;
						for(int mj=0;mj<range[0].Length;mj++)
						{
							if (range[0][mj] == 255 || range[range[0].Length - 1][mj] == 255)
							{
								exit = true;
								break;
							}
							
						}
						for (int mi = 0; mi < range.Length; mi++)
						{
							if (range[mi][0] == 255 || range[mi][range.Length - 1] == 255)
							{
								exit = true;
								break;
							}
						}
						if(exit==true)
						{
							continue;
						}
						for (int mj = 1; mj < range[0].Length-1; mj++)
						{
							if (range[1][mj] == 255 || range[range[1].Length - 2][mj] == 255)
							{
								exit = false;
								break;
							}
						}
						for (int mi = 1; mi < range.Length-1; mi++)
						{
							if (range[mi][1] == 255 || range[mi][range.Length - 2] == 255)
							{
								exit = false;
								break;
							}
						}
						if (exit == true)
						{
							continue;
						}
						if(range[range.Length/2][range[0].Length/2]==255)
						{
							for(int mj=1;mj<range.Length-1;mj++)
							{
								if(range[range.Length/2][mj]==0)
								{
									exit = true;
									break;
								}
							}
							if(!exit)
							{
								count++;
								for(int i1=i-maskWidth/2;i1<i+maskWidth/2;i1++)
									for (int j1 = j - maskWidth / 2; j1 < j + maskWidth / 2; j1++)
									{
										if (newpicture1[j1][i1] == 255)
											image.SetPixel(i1, j1, Color.FromArgb((int)newpicture1[j1][i1], 0, 0));
									}
										continue;
							}
							exit = false;
							for (int mi = 1; mi < range.Length - 1; mi++)
							{
								if (range[mi][range.Length / 2] == 0)
								{
									exit = true;
									break;
								}
							}
							if (!exit)
							{
								count++;
								for (int i1 = i - maskWidth / 2; i1 < i + maskWidth / 2; i1++)
									for (int j1 = j - maskWidth / 2; j1 < j + maskWidth / 2; j1++)
									{
										if (newpicture1[j1][i1] == 255)
											image.SetPixel(j1, i1, Color.FromArgb((int)newpicture1[j1][i1], 0, 0));
									}
								continue;
							}
							exit = false;
							for(int index=1;index< range.Length - 1;index++)
							{
								if (range[index][index] == 0)
								{
									exit = true;
									break;
								}								
							}
							if (!exit)
							{
								count++;
								for (int i1 = i - maskWidth / 2; i1 < i + maskWidth / 2; i1++)
									for (int j1 = j - maskWidth / 2; j1 < j + maskWidth / 2; j1++)
									{
										if (newpicture1[j1][i1] == 255)
											image.SetPixel(j1, i1, Color.FromArgb((int)newpicture1[j1][i1], 0, 0));
									}
								continue;
							}
							exit = false;
							for (int index = 1; index < range.Length - 1; index++)
							{
								if (range[index][range.Length - 1 - index] == 0)
								{
									exit = true;
									break;
								}
							}
							if (!exit)
							{
								count++;
								for (int i1 = i - maskWidth / 2; i1 < i + maskWidth / 2; i1++)
									for (int j1 = j - maskWidth / 2; j1 < j + maskWidth / 2; j1++)
									{
										if (newpicture1[j1][i1] > 0)
											image.SetPixel(j1, i1, Color.FromArgb((int)newpicture1[j1][i1], 0, 0));
									}
								continue;
							}
						}

					}
					result[i][j] = value;
				}
			}
			stones.Text = count.ToString();
			pictureBox2.Image = image;
		}

        private void button45_Click(object sender, EventArgs e)
        {
            Bitmap image = (Bitmap)pictureBox1.Image;
            double[][] picture = new double[image.Height][];
            for (int y = 0; y < image.Height; y++)
            {
                picture[y] = new double[image.Width];
                for (int x = 0; x < image.Width; x++)
                {
                    Color color = image.GetPixel(x, y);
                    picture[y][x] = color.R;
                }
            }
            double[][] newpicture = Transformations.Erosion(picture, 3, 1);
            Bitmap newimage = new Bitmap(image);
            for (int y = 0; y < image.Height; y++)
            {
                picture[y] = new double[image.Width];
                for (int x = 0; x < image.Width; x++)
                {
                    newimage.SetPixel(x, y, Color.FromArgb((int)newpicture[x][y], (int)newpicture[x][y], (int)newpicture[x][y]));
                }
            }
            pictureBox3.Image = newimage;
        }

        private void button46_Click(object sender, EventArgs e)
        {
            Bitmap image = (Bitmap)pictureBox1.Image;
            double[][] picture = new double[image.Height][];
            for (int y = 0; y < image.Height; y++)
            {
                picture[y] = new double[image.Width];
                for (int x = 0; x < image.Width; x++)
                {
                    Color color = image.GetPixel(x, y);
                    picture[y][x] = color.R;
                }
            }
            double[][] newpicture = Transformations.Dilatation(picture,3,0 );
            Bitmap newimage = new Bitmap(image);
            for (int y = 0; y < image.Height; y++)
            {
                picture[y] = new double[image.Width];
                for (int x = 0; x < image.Width; x++)
                {
                    newimage.SetPixel(x, y, Color.FromArgb((int)newpicture[x][y], (int)newpicture[x][y], (int)newpicture[x][y]));
                }
            }
            pictureBox3.Image = newimage;
        }
    }
}
	


