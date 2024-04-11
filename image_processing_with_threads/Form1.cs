using Microsoft.VisualBasic.Devices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace image_processing_with_threads
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private Bitmap? img;

		private void button_choose_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "Open Image";
			dlg.Filter = "jpg files (*.jpg)|*.jpg";
			dlg.ShowDialog();

			/*if (dlg.ShowDialog() == DialogResult.OK)
			{
				img = new Bitmap(dlg.FileName);
				pictureBox1.Image = img;
			}*/

			img = new Bitmap(dlg.FileName);
			pictureBox1.Image = img;
			dlg.Dispose();
		}

		private void button_process_Click(object sender, EventArgs e)
		{
			Image image = new Image();

			Bitmap b1 = new Bitmap(img);
			Bitmap b2 = new Bitmap(img);
			Bitmap b3 = new Bitmap(img);
			Bitmap b4 = new Bitmap(img);

			int thread_nr = 4;
			Thread[] threads = new Thread[thread_nr];
			threads[0] = new Thread(() => image.Grayscale(b1));
			threads[1] = new Thread(() => image.Negative(b2));
			threads[2] = new Thread(() => image.Green(b3));
			threads[3] = new Thread(() => image.Threshold(b4));

			foreach (Thread x in threads)
			{
				x.Start();
			}

			foreach (Thread x in threads)
			{
				x.Join();
			}

			pictureBox2.Image = b1;
			pictureBox3.Image = b2;
			pictureBox4.Image = b3;
			pictureBox5.Image = b4;

			img.Dispose();
		}
	}
}
