namespace parallel_and_thread_comparision
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button_start_Click(object sender, EventArgs e)
		{
			MatrixGenerator matrix = new MatrixGenerator();

			int[,] matrix1;
			int[,] matrix2;
			int rows = 400;
			int cols = 400;

			matrix1 = matrix.Generate(rows, cols);
			matrix2 = matrix.Generate(rows, cols);

			int[,] matrixP = new int[rows, cols];
			int[,] matrixT = new int[rows, cols];

			void Mult(int thread_nr)
			{
				double x = (double)rows / thread_nr;
				double elementsPerThread_d = Math.Ceiling(x);
				int elementsPerThread = (int)elementsPerThread_d;

				Thread[] threads = new Thread[thread_nr];

				for (int t = 0; t < thread_nr; t++)
				{
					int start = t * elementsPerThread;
					int end;
					if (t == thread_nr - 1)
					{
						end = rows;
					}
					else
					{
						end = start + elementsPerThread - 1;
					}

					threads[t] = new Thread(() =>
					{
						for (int i = start; i < end; i++)
						{
							for (int j = 0; j < cols; j++)
							{
								matrixT[i, j] = 0;
								for (int k = 0; k < rows; k++)
								{
									matrixT[i, j] += matrix1[i, k] * matrix2[k, j];
								}
							}
						}
					});

					threads[t].Start();
				}

				foreach (Thread thread in threads)
				{
					thread.Join();
				}
			}

			void Mult_Parallel(int x)
			{
				ParallelOptions opt = new ParallelOptions()
				{
					MaxDegreeOfParallelism = x
				};

				Parallel.For(0, rows, opt, i =>
				{
					for (int j = 0; j < cols; j++)
					{
						matrixP[i, j] = 0;
						for (int k = 0; k < rows; k++)
						{
							matrixP[i, j] += matrix1[i, k] * matrix2[k, j];
						}
					}
				});
			}

			// time measuring
			textBox_table.AppendText("number of threads   | parallel time | threads time");
			textBox_table.AppendText(Environment.NewLine);

			for (int x = 1; x < 5; x++)
			{
				long time_sum_parallel = 0;
				long time_sum_threads = 0;
				long time_threads_result = 0;
				long time_parallel_result = 0;

				int t = 5;
				while (t != 0)
				{
					/*ParallelOptions opt = new ParallelOptions()
					{
						MaxDegreeOfParallelism = x
					};*/

					matrix1 = matrix.Generate(rows, cols);
					matrix2 = matrix.Generate(rows, cols);

					var watch = System.Diagnostics.Stopwatch.StartNew();
					Mult_Parallel(x);
					watch.Stop();
					time_sum_parallel += watch.ElapsedMilliseconds;

					var watch2 = System.Diagnostics.Stopwatch.StartNew();
					Mult(x);
					watch2.Stop();
					time_sum_threads += watch2.ElapsedMilliseconds;

					t--;
				}

				time_parallel_result = time_sum_parallel / 5;
				time_threads_result = time_sum_threads / 5;
				string s1 = time_parallel_result.ToString();
				string s2 = time_threads_result.ToString();

				textBox_table.AppendText($"{x.ToString()}\t\t| {s1} ms       | {s2} ms");
				textBox_table.AppendText(Environment.NewLine);
			}
		}
	}
}
