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
                    newColor = Color.FromArgb(originalColor.R, originalColor.G, originalColor.B);
                    newImage.SetPixel(i, j, newColor);
                }
            }

            return newImage;
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

            pictureBox2.Image = imagePal;

        }
    }
}
