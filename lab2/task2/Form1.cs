using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Lesson2_task2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static Bitmap ConvertColor(Bitmap img, char channel)
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

                    if (channel == 'R')
                        newColor = Color.FromArgb(originalColor.R, 0, 0);
                    else if (channel == 'G')
                        newColor = Color.FromArgb(0, originalColor.G, 0);
                    else if (channel == 'B')
                        newColor = Color.FromArgb(0, 0, originalColor.B);

                    newImage.SetPixel(i, j, newColor);
                }
            }

            return newImage;
        }

        private static Bitmap Histogram(Bitmap img, char cl)
        {
            int[] colorHistogram = new int[256]; 
            int maxCount = 0;
            for (int row = 0; row < img.Height; ++row)
            {
                for (int col = 0; col < img.Width; ++col)
                {
                    Color pixelColor = img.GetPixel(col, row);

                    int intensity = 0;
                    if (cl == 'R')
                        intensity = pixelColor.R; 
                    else if (cl == 'G')
                        intensity = pixelColor.G; 
                    else if (cl == 'B')
                        intensity = pixelColor.B; 


                    colorHistogram[intensity]++;


                    if (colorHistogram[intensity] > maxCount)
                        maxCount = colorHistogram[intensity];
                }
            }

            Bitmap histogramImage = new Bitmap(256, 256);

            int histogramHeight = histogramImage.Height;

            using (Graphics g = Graphics.FromImage(histogramImage))
            using (Pen pen = new Pen(Color.Black))
            {
                for (int i = 0; i < 256; i++)
                {
                    int barHeight = (int)((double)colorHistogram[i] / maxCount * histogramHeight);

                    if (cl == 'R')
                        pen.Color = Color.Red;
                    else if (cl == 'G')
                        pen.Color = Color.Green;
                    else if (cl == 'B')
                        pen.Color = Color.Blue;

                    g.DrawLine(pen, i, histogramHeight, i, histogramHeight - barHeight);
                }
            }
            return histogramImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            Bitmap redImage = ConvertColor(originalImage, 'R');
            Bitmap greenImage = ConvertColor(originalImage, 'G');
            Bitmap blueImage = ConvertColor(originalImage, 'B');

            pictureBox2.Image = redImage;
            pictureBox3.Image = greenImage;
            pictureBox4.Image = blueImage;

            pictureBox5.Image = Histogram(redImage, 'R');
            pictureBox6.Image = Histogram(greenImage, 'G');
            pictureBox7.Image = Histogram(blueImage, 'B');
        }

        private void button2_Click(object sender, EventArgs e)
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
    }
}
