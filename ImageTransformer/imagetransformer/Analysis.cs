using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Text.RegularExpressions;

namespace ImageTransformer
{
	class Analysis
	{
		public static double[] Frequencies(double[] x, double[] xAxis)
		{
			double m = Statistics.ExpectedValue(x);
			double sqv = Math.Sqrt(Statistics.Variance(x));
			double deltaF = (xAxis[1] - xAxis[0]) / xAxis.Length;
			double[] peaks = new double[x.Length];
			for (int i = 0; i < x.Length; i++)
				if (x[i] > m + sqv / 4)
					peaks[i] = i * deltaF;
			double max = peaks.Max(), min = peaks.Max();
			for (int i = 0; i < peaks.Length; i++)
			{
				if (min > peaks[i] && peaks[i] != 0)
					min = peaks[i];
			}
			double[] peakss = new double[2];
			peakss[0] = min;
			peakss[1] = max;
			return peakss;
		}
		public static double[] FurieXTransform(double[] x)
		{
			double[] newX = new double[x.Length / 2];
			double deltaT = x[1] - x[0];

			double Fgr = 1 / (2 * deltaT);

			double deltaF = Fgr / (x.Length / 2);
			double left = 0;

			Parallel.For(0, newX.Length, i =>
			{
				newX[i] = left + i * deltaF;

			}); // Parallel.For

			return newX;
		}
		public static float[] FurieFunction(float[] y)
		{
			int n = y.Length;
			float[] res = new float[n];
			Parallel.For(0, n, i =>
			{

				float Rei = 0;
				for (int k = 0; k < n; k++)
				{
					Rei += y[k] * (float)Math.Cos((Math.PI * 2 * i * k) / n);
				}
				float Imi = 0;
				for (int k = 0; k < n; k++)
				{
					Imi += y[k] * (float)Math.Sin((Math.PI * 2 * i * k) / n);
				}
				Rei = Rei / n;
				Imi = Imi / n;

				//res[i] = (float)Math.Sqrt(Math.Pow(Rei, 2) + Math.Pow(Imi, 2));
				res[i] = Rei + Imi;
			});

			return res;
		}
		public static double[] FurieFunction(double[] y)
		{
			int n = y.Length;
			double[] res = new double[n];
			Parallel.For(0, n, i =>
			{

				double Rei = 0;
				for (int k = 0; k < n; k++)
				{
					Rei += y[k] * Math.Cos((Math.PI * 2 * i * k) / n);
				}
				double Imi = 0;
				for (int k = 0; k < n; k++)
				{
					Imi += y[k] * Math.Sin((Math.PI * 2 * i * k) / n);
				}
				Rei = Rei / n;
				Imi = Imi / n;

				//res[i] = (float)Math.Sqrt(Math.Pow(Rei, 2) + Math.Pow(Imi, 2));
				res[i] = Rei + Imi;
			});

			return res;
		}
		public static float[,] FurieComplexFunction(float[] y)
		{
			int n = y.Length;
			float[,] res = new float[2, n];
			Parallel.For(0, n, i =>
			{

				float Rei = 0;
				for (int k = 0; k < n; k++)
				{
					Rei += y[k] * (float)Math.Cos((Math.PI * 2 * i * k) / n);
				}
				float Imi = 0;
				for (int k = 0; k < n; k++)
				{
					Imi += y[k] * (float)Math.Sin((Math.PI * 2 * i * k) / n);
				}


				res[0, i] = Rei / n;
				res[1, i] = Imi / n;
			});

			return res;
		}

		public static float[] ReverseFurieFunction(float[] z)
		{
			int n = z.Length;
			float[] res = new float[n];
			Parallel.For(0, n, i =>
			{

				float Rei = 0;
				for (int k = 0; k < n; k++)
				{
					Rei += z[k] * (float)Math.Cos((Math.PI * 2 * i * k) / n);
				}
				float Imi = 0;
				for (int k = 0; k < n; k++)
				{
					Imi += z[k] * (float)Math.Sin((Math.PI * 2 * i * k) / n);
				}

				res[i] = Rei + Imi;

			});
			return res;
		}
		public static Complex[] ForwardFurie(float[] y)
		{
			int n = y.Length;
			Complex[] res = new Complex[n];
			Parallel.For(0, n, i =>
			{

				double Rei = 0;
				for (int k = 0; k < n; k++)
				{
					Rei += y[k] * Math.Cos((Math.PI * 2 * i * k) / n);
				}
				double Imi = 0;
				for (int k = 0; k < n; k++)
				{
					Imi += y[k] * Math.Sin((Math.PI * 2 * i * k) / n);
				}
				Rei = Rei / n;
				Imi = Imi / n;

				Complex result = new Complex(Rei, Imi);

				res[i] = result;

			});

			return res;
		}
		public static Complex[] ForwardFurie(double[] y)
		{
			int n = y.Length;
			Complex[] res = new Complex[n];
			Parallel.For(0, n, i =>
			{

				double Rei = 0;
				for (int k = 0; k < n; k++)
				{
					Rei += y[k] * Math.Cos((Math.PI * 2 * i * k) / n);
				}
				double Imi = 0;
				for (int k = 0; k < n; k++)
				{
					Imi += y[k] * Math.Sin((Math.PI * 2 * i * k) / n);
				}
				Rei = Rei / n;
				Imi = Imi / n;

				Complex result = new Complex(Rei, Imi);

				res[i] = result;

			});

			return res;
		}


		public static Complex[] ReverseFurie(float[] y)
		{

			int n = y.Length;
			Complex[] res = new Complex[n];
			Parallel.For(0, n, i =>
			{

				double Rei = 0;
				for (int k = 0; k < n; k++)
				{
					Rei += y[k] * Math.Cos((Math.PI * 2 * i * k) / n);
				}
				double Imi = 0;
				for (int k = 0; k < n; k++)
				{
					Imi += y[k] * Math.Sin((Math.PI * 2 * i * k) / n);
				}

				Complex result = new Complex(Rei, Imi);


				res[i] = result;
			});

			return res;
		}
		public static double Crosscorrelation(double[] arr1, double[] arr2, int lag)
		{
			if (arr1.Length != arr2.Length)
			{
				throw new Exception("Lengths of function arrays are different");
			}
			double avg1 = Statistics.ExpectedValue(arr1);
			double avg2 = Statistics.ExpectedValue(arr2);
			double variance = Statistics.Variance(arr1);
			double sum = 0;

			for (int i = 0; i < arr2.Length - lag; i++)
			{
				sum += (arr1[i] - avg1) * (arr2[i + lag] - avg2);
			}
			return sum / ((arr1.Length - lag) * variance);

		}
		public static double Autocorrelation(double[] arr, int lag)
		{
			return Crosscorrelation(arr, arr, lag);
		}
        public static double Max(double[][] picture)
        {
            double max = 0;
            int W = picture[0].GetLength(0);
            int H = picture.GetLength(0);
            for (int y = 0; y < H; y++)
                for (int x = 0; x < W; x++)
                {
                    if (max < picture[y][x])
                        max = picture[y][x];
                       }
                    return max;
        }
        public static double Min(double[][] picture)
		{
            double min = Max(picture);
            int W = picture[0].GetLength(0);
            int H = picture.GetLength(0);
            for (int y = 0; y < H; y++)
                for (int x = 0; x < W; x++)
                {
                    if (min > picture[y][x])
                        min = picture[y][x];
                }
            return min;
        }
	}
}
