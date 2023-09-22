using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lesson2_task1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static Bitmap ToGrayPal(Bitmap img)
        {
            int width = img.Width;
            int height = img.Height;
            Bitmap newImage = new Bitmap(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color originalColor = img.GetPixel(i, j);
                    Color newColor = Color.Black;
                    int grayValue = (int)(0.299 * originalColor.R + 0.587 * originalColor.G + 0.114 * originalColor.B);
                    newColor = Color.FromArgb(grayValue, grayValue, grayValue);
                    newImage.SetPixel(i, j, newColor);
                }
            }

            return newImage;
        }

        private static Bitmap ToGrayHDTV(Bitmap img)
        {
            int width = img.Width;
            int height = img.Height;
            Bitmap newImage = new Bitmap(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color originalColor = img.GetPixel(i, j);
                    Color newColor = Color.Black;
                    int grayValue = (int)(0.2126 * originalColor.R + 0.7152 * originalColor.G + 0.0722 * originalColor.B);
                    newColor = Color.FromArgb(grayValue, grayValue, grayValue);
                    newImage.SetPixel(i, j, newColor);
                }
            }
            return newImage;
        }

        private static Bitmap ImagesDifference(Bitmap img1, Bitmap img2)
        {
            int width = img1.Width;
            int height = img1.Height;
            Bitmap diffImage = new Bitmap(img1.Width, img1.Height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color color1 = img1.GetPixel(i, j);
                    Color color2 = img2.GetPixel(i, j);
                    int diffR = Math.Abs(color1.R - color2.R);
                    int diffG = Math.Abs(color1.G - color2.G);
                    int diffB = Math.Abs(color1.B - color2.B);
                    diffImage.SetPixel(i, j, Color.FromArgb(diffR, diffG, diffB));
                }
            }
            return diffImage;
        }
        private static Bitmap Histogram(Bitmap img)
        {
            Bitmap histogramImage = new Bitmap(256, 256);

            int histogramHeight = histogramImage.Height;
            int[] intensity = new int[256];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color color = img.GetPixel(i, j);
                    intensity[color.R]++;
                }
            }

            Graphics g = Graphics.FromImage(histogramImage);
            g.Clear(Color.White);
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < 256; i++)
            {
                g.DrawLine(pen, i, histogramImage.Height, i, histogramImage.Height - intensity[i]);
            }

            return histogramImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Unable to open the selected file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap originalImage = new Bitmap(pictureBox1.Image);

            Bitmap imagePal = ToGrayPal(originalImage);
            Bitmap imageHDTV = ToGrayHDTV(originalImage);

            pictureBox2.Image = imagePal;
            pictureBox3.Image = imageHDTV;

            Bitmap imageRz = ImagesDifference(imagePal, imageHDTV);

            pictureBox4.Image = imageRz;

            Bitmap HistogramPal = Histogram(imagePal);
            pictureBox5.Image = HistogramPal;
            Bitmap HistogramHDTV = Histogram(imageHDTV);
            pictureBox6.Image = HistogramHDTV;
        }
    }
}
