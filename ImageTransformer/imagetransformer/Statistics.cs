using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTransformer
{
	class Statistics
	{
		public static double ExpectedValue(double[] arr)
		{
			double sum = 0;
			for (int i = 0; i < arr.Length; i++)
			{
				sum += arr[i];
			}
			return sum / arr.Length;
		}
		public static double Variance(double[] arr)
		{
			double Mx = ExpectedValue(arr);
			double sum = 0;
			foreach (var point in arr)
			{

				sum += Math.Pow((point - Mx), 2);

			}
			return sum / arr.Length;
		}
		public static double StandartDeviation(double[] arr)
		{
			return Math.Sqrt(Variance(arr));
		}
	}
}
