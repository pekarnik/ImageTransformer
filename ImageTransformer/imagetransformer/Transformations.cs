using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTransformer
{
	class Transformations
	{				
		public static double[] Lpf(double fcut, int m, double dt)
		{
			double[] lpw = new double[m + 1];
			double[] d = { 0.35577019, 0.2436983, 0.07211497, 0.00630165 };
			//прямоугольная часть
			double arg = 2 * fcut * dt;
			lpw[0] = arg;
			arg *= Math.PI;
			for (int i = 1; i <= m; i++)
			{
				lpw[i] = Math.Sin(arg * i) / (Math.PI * i);
			}
			//трапеция
			lpw[m] /= 2;
			//окно
			double sumg = lpw[0];

			for (int i = 1; i <= m; i++)
			{
				double sum = d[0];
				arg = (Math.PI * i) / m;

				for (int k = 1; k <= 3; k++)
				{
					sum += 2 * d[k] * Math.Cos(arg * k);
				}
				lpw[i] *= sum;
				sumg += 2 * lpw[i];

			}
			//сглаживание(нормировка)
			for (int i = 0; i <= m; i++)
			{
				lpw[i] = lpw[i] / sumg;
			}
			double[] reverse = lpw.Reverse().ToArray();
			double[] result = new double[2 * m + 1];
			for (int i = 0; i < m; i++)
			{
				result[i] = reverse[i];
			}
			for (int i = 0; i <= m; i++)
			{
				result[m + i] = lpw[i];
			}
			return result;
		}
		public static double[] LpfX(int m)
		{
			double[] x = new double[2 * m + 1];
			for (int i = 0; i < 2 * m + 1; i++)
			{
				x[i] = i;
			}
			return x;
		}
		public static double[] Hpf(double fcut, int m, double dt)
		{
			double[] lpw = Lpf(fcut, m, dt);
			for (int k = 0; k < lpw.Length; k++)
			{
				lpw[k] *= -1;
			}
			lpw[m] = 1 + lpw[m];
			return lpw;
		}
		public static double[] Bpf(double fcut1, double fcut2, int m, double dt)
		{
			double[] lpw1 = Lpf(fcut1, m, dt);
			double[] lpw2 = Lpf(fcut2, m, dt);
			for (int k = 0; k < lpw1.Length; k++)
			{
				lpw1[k] = lpw2[k] - lpw1[k];
			}

			return lpw1;
		}
		public static double[] Bsf(double fcut1, double fcut2, int m, double dt)
		{
			double[] lpw1 = Lpf(fcut1, m, dt);
			double[] lpw2 = Lpf(fcut2, m, dt);
			for (int k = 0; k < lpw1.Length; k++)
			{
				lpw1[k] = lpw1[k] - lpw2[k];
			}
			lpw1[m] += 1;
			return lpw1;
		}
		
		
		
		public static double[] ConwolutionWithLpf(double[] x, double fcut, double m, double dt)
		{

			double[] h = Transformations.Lpf(fcut, (int)m, dt);

			double[] res = Transformations.ConvolutionFunction(h, x);
			return res;
		}
		public static double[] ConvolutionWithPlot(double[] x, double[] h)
		{
			/*
                        alglib.complex[] f;
                        alglib.complex[] hc;
                        double[] x2;
                        alglib.complex[] res;
                        alglib.fftr1d(x, out f);
                        alglib.fftr1d(h, out hc);
                        alglib.convc1d(f, f.Length, hc, hc.Length, out res);
                        alglib.fftr1dinv(res, out x2);
                        x2 = x2.Take(x.Length).ToArray();
                        double max = 0;
                        if (Math.Abs(x2.Max()) > Math.Abs(x2.Min())) {
                            max = Math.Abs(x2.Max());
                        }
                        else {
                            max = Math.Abs(x2.Min());
                        }
                       for (int i = 0; i < x2.Length; i++)
                       {
                           x2[i] = x2[i] / max;
                       }
                        // double[] res = Transformations.ConvolutionFunction(h, x);
                        return x2;*/

			return ConvolutionFunction(h, x);
		}
		public static double[] Normalize(double[] x)
		{
			double min = x.Min();
			double max = x.Max();
			for (int i = 0; i < x.Length; i++)
			{
				x[i] = (x[i] / (max - min));
			}

			return x;
		}
		public static double[] ConwolutionWithHpf(double[] x, double fcut, double m, double dt)
		{

			double[] h = Transformations.Hpf(fcut, (int)m, dt);

			double[] res = Transformations.ConvolutionFunction(h, x);
			return res;
		}
		public static double[] ConwolutionWithBpf(double[] x, double fcut1, double fcut2, double m, double dt)
		{

			double[] h = Bpf(fcut1, fcut2, (int)m, dt);

			double[] res = ConvolutionFunction(h, x);
			return res;
		}
		public static double[] ConwolutionWithBsf(double[] x, double fcut1, double fcut2, double m, double dt)
		{

			double[] h = Transformations.Bsf(fcut1, fcut2, (int)m, dt);

			double[] res = Transformations.ConvolutionFunction(h, x);
			return res;
		}
		public static double[][] ThresholdFun(double[][] f, double min_threshold, double max_threshold, int depth = 8)
		{

			//double[] mass = f.SelectMany(x => x).ToArray();



			for (int i = 0; i < f.Length; i++)
			{
				for (int j = 0; j < f[i].Length; j++)
				{

					int value = (int)f[i][j];
					if (value < min_threshold)
					{
						value = 0;
						f[i][j] = value;
					}
					else if (value >= max_threshold)
					{
						value = (1 << depth) - 1;
						f[i][j] = value;
					}
				}
			}
			return f;

		}
		public static double[][] Minus(double[][] arr1, double[][] arr2)
		{
			double[][] result = new double[arr1.Length][];

			for (int i = 0; i < arr1.Length; i++)
			{
				result[i] = new double[arr1[i].Length];
				for (int j = 0; j < arr1[i].Length; j++)
				{
					int x = (int)arr1[i][j] - (int)arr2[i][j];
					if (x < 0)
					{
						x = 0;

					}
					result[i][j] = x;
				}


			}


			return result;

		}
		
		
		public static double[] ConvolutionFunction(double[] h, double[] y)
		{
			int n = y.Length;
			int m = h.Length;
			double[] res = new double[n + m];
			for (int k = 0; k < res.Length; k++)
			{
				double yk = 0;
				for (int l = 0; l < m; l++)
				{
					if ((k - l < 0) || (k - l >= n))
					{

					}
					else
					{
						yk = yk + y[k - l] * h[l];
					}

				}
				res[k] = yk;

			}
			return res;
		}
		public static ushort[,] StepFunction(ushort[,] picture, int step_Lower, int step_Higher)
		{
			for (int y = 0; y < picture.GetLength(1); y++)
				for (int x = 0; x < picture.GetLength(0); x++)
				{
					if (picture[x, y] < step_Lower)
					{
						picture[x, y] = 0;
					}
					else if (picture[x, y] >= step_Higher)
					{
						picture[x, y] = 255;
					}
				}
			return picture;
		}
			public static double[,] StepFunction(double[,] picture, int step_Lower, int step_Higher)
			{
				for (int y = 0; y < picture.GetLength(1); y++)
					for (int x = 0; x < picture.GetLength(0); x++)
					{
						if (picture[x, y] <= step_Lower)
						{
							picture[x, y] = 0;
						}
						else if (picture[x, y] >= step_Higher)
						{
							picture[x, y] = 255;
						}
					}
				return picture;
			}
		public static double[][] Gradient(double[][] f)
		{

			double[][] horMask = new double[3][] { new double[3] { -1, -2, -1 }, new double[3] { 0, 0, 0 }, new double[3] { 1, 2, 1 } };
			double[][] diagMask45 = new double[3][] { new double[3] { -2, -1, 0 }, new double[3] { -1, 0, 1 }, new double[3] { 0, 1, 2 } };
			double[][] vertMask = new double[3][] { new double[3] { -1, 0, 1 }, new double[3] { -2, 0, 2 }, new double[3] { -1, 0, 1 } };			
			double[][] diagMask135 = new double[3][] { new double[3] { 0, -1, -2 }, new double[3] { -1, 0, 1 }, new double[3] { 2, 1, 0 } };
			
			double[][] Horizontal = ApplyMask(f, horMask);
			double[][] Vertical = ApplyMask(f, vertMask);
			double[][] Diagonal = ApplyMask(f, diagMask45);
			double[][] DiagonalRev = ApplyMask(f, diagMask135);
		
			double tmp;
			for (int i = 0; i < f.Length; i++)
			{
				for (int j = 0; j < f[i].Length; j++)
				{
					tmp = Horizontal[i][j] + Vertical[i][j]+Diagonal[i][j] + DiagonalRev[i][j];
					if (tmp < 0)
					{
						tmp = Math.Abs(tmp);
					}

					f[i][j] = tmp;

				}

			}
			return f;

		}
		public static double[][] ApplyMask(double[][] f, double[][] mask)
		{
			double[][] result = new double[f.Length][];
			for (int i = 0; i < f.Length; i++)
			{
				result[i] = new double[f[i].Length];
				for (int j = 0; j < f[i].Length; j++)
				{
					int maxMi = mask.Length / 2;
					int maxMj = mask[0].Length / 2;
					double[][] range = f.Skip(i - maxMi).Take(i >= maxMi ? mask.Length : i + maxMi + 1)
						.Select(x => x.Skip(j - maxMj).Take(j >= maxMj ? mask[0].Length : j + maxMj + 1).ToArray())
						.ToArray();

					double[][] maskRange = mask.Skip(i < maxMi ? maxMi - i : 0)
						.Take(i + maxMi >= f.Length ? f.Length - i + 1 : mask.Length)
						.Select(x => x.Skip(j < maxMj ? maxMj - j : 0)
									  .Take(j + maxMj >= f[i].Length ? f[i].Length - j + 1 : mask[0].Length)
									  .ToArray())
						.ToArray();

					
					double sum = 0;


					for (int mi = 0; mi < maskRange.Length; mi++)
					{
						for (int mj = 0; mj < maskRange[mi].Length; mj++)
						{
							sum += range[mi][mj] * maskRange[mi][mj];							
						}
					}





					
					result[i][j] = sum;
				}

			}
			return result;
		}
		public static double[][] Laplasian(double[][] f)
		{

			double[][] horMask = new double[3][] { new double[3] { -1, -1, -1 }, new double[3] { -1, 8, -1 }, new double[3] {- 1, -1, -1 } };
			//double[][] vertMask = new double[3][] { new double[3] { -1, 1, 1 }, new double[3] { -2, 0, 2 }, new double[3] { -1, 0, 1 } };
			//double[][] diagMask = new double[3][] { new double[3] { -2, -1, 0 }, new double[3] { -1, 0, 1 }, new double[3] { 0, 1, 2 } };
			//double[][] diagMaskReversed = new double[3][] { new double[3] { 0, -1, -2 }, new double[3] { -1, 0, 1 }, new double[3] { 2, 1, 0 } };

			double[][] Horizontal = ApplyMask(f, horMask);
			//double[][] Vertical = ApplyMask(f, vertMask);
			//double[][] Diagonal = ApplyMask(f, diagMask);
			//double[][] DiagonalRev = ApplyMask(f, diagMaskReversed);




			double tmp;
			for (int i = 0; i < f.Length; i++)
			{
				for (int j = 0; j < f[i].Length; j++)
				{
					tmp = Horizontal[i][j];
					//+ Vertical[i][j]+ Diagonal[i][j] + DiagonalRev[i][j];
					if (tmp < 0)
					{
						tmp = Math.Abs(tmp);
					}

					f[i][j] = tmp;

				}

			}
			return f;

		}
		public static double[][] AddSP(double[][]picture, int percent)
		{
			int W = picture[0].GetLength(0);
			int H = picture.GetLength(0);
			LCG random = new LCG();
			for (int y = 0; y < H; y++)
				for (int x = 0; x < W; x++)
				{
					
					int possibility = random.Next(1,100);
					if (possibility <= percent)
					{
						int rnd=random.Next(0,255);
						if(rnd<128)
							picture[y][x] = 0;
						if (rnd >= 128)
							picture[y][x] = 255;
					}
				}
			picture = ThresholdFun(picture, 0, 255);
			return picture;
		}
		public static double[][] AddRandom(double[][] picture, int percent)
		{
			int W = picture[0].GetLength(0);
			int H = picture.GetLength(0);
			LCG random = new LCG();
			for (int y = 0; y < H; y++)				
				for (int x = 0; x < W; x++)
				{					
					int rnd = random.Next(-100, 99);
					if(rnd<0)
					{
						rnd =- 1;
					}
					if(rnd>=0)
					{
						rnd = 1;
					}
					int possibility = random.Next(1,100);
					if (possibility <= percent)
					{
						picture[y][x]  =picture[y][x]+ rnd*30;
					}
				}
			picture = ThresholdFun(picture, 0, 255);

			return picture;
		}
		public static double[][] AvgFilt(double[][] f, int maskWidth)
		{

			double[][] result = new double[f.Length][];
			for (int i = 0; i < f.Length; i++)
			{
				result[i] = new double[f[i].Length];
				for (int j = 0; j < f[i].Length; j++)
				{
					int maxMi = maskWidth / 2;
					int maxMj = maskWidth / 2;
					double[][] range = f.Skip(i - maxMi).Take(i >= maxMi ? maskWidth : i + maxMi + 1)
						.Select(x => x.Skip(j - maxMj).Take(j >= maxMj ? maskWidth : j + maxMj + 1).ToArray())
						.ToArray();

					double avg = range.SelectMany(x => x).Average();

					result[i][j] = avg;

				}

			}
			return result;
		}
		public static double[][] MedFilt(double[][] f, int maskWidth)
		{

			double[][] result = new double[f.Length][];
			for (int i = 0; i < f.Length; i++)
			{
				result[i] = new double[f[i].Length];
				for (int j = 0; j < f[i].Length; j++)
				{
					int maxMi = maskWidth / 2;
					int maxMj = maskWidth / 2;
					double[][] range = f.Skip(i - maxMi).Take(i >= maxMi ? maskWidth : i + maxMi + 1)
						.Select(x => x.Skip(j - maxMj).Take(j >= maxMj ? maskWidth : j + maxMj + 1).ToArray())
						.ToArray();

					double[] values = range.SelectMany(x => x).OrderBy(x => x).ToArray();
					result[i][j] = values[values.Length / 2];

				}

			}
			return result;
		}
		public static double[][] Erosion(double[][] f, int maskWidth, double step)
		{
			double[][] result = new double[f.Length][];
			int maxMi = maskWidth / 2;
			int maxMj = maskWidth / 2;
			for (int i = 0; i < f.Length; i++)
			{
				result[i] = new double[f[i].Length];
				//++
				for (int j = 0; j < f[i].Length; j++)
				{
					double value = f[i][j];
					if (i >= maxMi && i < f.Length - maxMi && j >= maxMj && j < f[i].Length - maxMj)
					{
						double[][] range = f.Skip(i - maxMi).Take(maskWidth)
							.Select(x => x.Skip(j - maxMj).Take(maskWidth).ToArray())
							.ToArray();
						bool needToDelete = false;


                        for (int mi = 0; mi < range.Length; mi++)
                        {
                            for (int mj = 0; mj < range[mi].Length; mj++)
                            {
                                if (range[mi][mj] < step)
                                {
                                    needToDelete = true;
                                }

                            }
                        }

                        if (needToDelete)
                        {
                            double min = Analysis.Min(range);

                            value = min;
                        }
                    }
					result[i][j] = value;
				}
			}
			return result;
		}

		public static double[][] Dilatation(double[][] f, int maskWidth, double step)
		{
			double[][] result = new double[f.Length][];
			int maxMi = (int)maskWidth / 2;
			int maxMj = (int)maskWidth / 2;
			for (int i = 0; i < f.Length; i++)
			{
				result[i] = new double[f[i].Length];
				//++
				for (int j = 0; j < f[i].Length; j++)
				{
					double value = f[i][j];
					if (i >= maxMi && i < f.Length - maxMi && j >= maxMj && j < f[i].Length - maxMj)
					{
						double[][] range = f.Skip(i - maxMi).Take(maskWidth)
							.Select(x => x.Skip(j - maxMj).Take(maskWidth).ToArray())
							.ToArray();




						bool needToAdd = false;


						for (int mi = 0; mi < range.Length; mi++)
						{
							for (int mj = 0; mj < range[mi].Length; mj++)
							{
								if (range[mi][mj] > step)
								{
									needToAdd = true;
								}
							}
						}

						//if (needToAdd)
						//{
							double max = Analysis.Max(range);

							value = max;
						//}
					}
					result[i][j] = value;
				}
			}
			return result;
		}

	}






}

