using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTransformer
{
	class LCG
	{
		
			private int _state;
			public LCG()
			{
				_state = (int)DateTime.Now.Ticks;
			}

			public LCG(int n)
			{
				_state = n;
			}

			public int Next()
			{
				return _state = (1103515245 * _state + 12345) & int.MaxValue;
			}

			public int Next(int min, int max)
			{
				return min + Next() % (max - min);
			}

		
	}
}
