using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace net_threads
{
	internal class MatrixGenerator
	{
		public int[,] Generate(int r, int c)
		{
			Random random = new Random();
			int[,] matrix = new int[r, c];
			for (int i = 0; i < r; i++)
			{
				for (int j = 0; j < c; j++)
				{
					matrix[i, j] = random.Next(0, 10);
				}
			}
			return matrix;
		}
	}
}
