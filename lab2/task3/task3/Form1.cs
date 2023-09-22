using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace task3
{
    public partial class Form1 : Form
    {
        private int H = 360;
        private double S = 1;
        private double L = 1;
        public Form1()
        {
            InitializeComponent();
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
                    sourceImageBox.Image = new Bitmap(openFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Unable to open the selected file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            double normG = color.G / 255.0;
            double normR = color.R / 255.0;
            double normB = color.B / 255.0;

            double max = Math.Max(normR, Math.Max(normG, normB));
            double min = Math.Min(normR, Math.Min(normG, normB));

            double diff = max - min;

            if (max == min)
            {
                hue = 0;
            }
            else if (max == normR && color.G >= color.B)
            {
                hue = 60 * ((normG - normB) / (diff));
            }
            else if (max == normR && color.G < color.B)
            {
                hue = 60 * (normG - normB) / (diff) + 360;
            }
            else if (max == normG)
            {
                hue = 60 * ((normB - normR) / (diff)) + 120;
            }
            else
            {
                hue = 60 * (normR - normG) / diff + 240;
            }
            saturation = max == 0 ? 0 : 1 - (min / max);
            value = max;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {

            double hi = Convert.ToInt32(Math.Floor(hue / 60.0)) % 6;
            double vmin = Convert.ToInt32(((100 - saturation) * value) / 100);
            double a = Convert.ToInt32((value - vmin) * (hue % 60) / 60);
            double vinc = Convert.ToInt32(vmin + a);
            double vdec = Convert.ToInt32(value - a);

            int v = Convert.ToInt32(value * 255 / 100);
            int p_vinc = Convert.ToInt32(vinc * 255 / 100);
            int p_vmin = Convert.ToInt32(vmin * 255 / 100);
            int p_vdec = Convert.ToInt32(vdec * 255 / 100);

            if (hi == 0)
            {
                return Color.FromArgb(255, v, p_vinc, p_vmin);
            }
            else if (hi == 1)
            {
                return Color.FromArgb(255, p_vdec, v, p_vmin);
            }
            else if (hi == 2)
            {
                return Color.FromArgb(255, p_vmin, v, p_vinc);
            }
            else if (hi == 3)
            {
                return Color.FromArgb(255, p_vmin, p_vdec, v);
            }
            else if (hi == 4)
            {
                return Color.FromArgb(255, p_vinc, p_vmin, v);
            }
            else
            {
                return Color.FromArgb(255, v, p_vmin, p_vdec);
            }
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.H = (int)numericUpDown1.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.S = (int)numericUpDown3.Value / 100;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.L = (int)numericUpDown2.Value / 100;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Image rgbImage = pictureBoxHSV.Image;

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "JPEG Image|*.jpg";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveDialog.FileName;
                    // Сохраните изображение в RGB формате
                    rgbImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap originalImage = new Bitmap(sourceImageBox.Image);

            Bitmap hsvImage = new Bitmap(originalImage.Width, originalImage.Height);

            for (int x = 0; x < originalImage.Width; x++)
            {
                for (int y = 0; y < originalImage.Height; y++)
                {
                    Color rgbColor = originalImage.GetPixel(x, y);

                    double hue, saturation, value;
                    ColorToHSV(rgbColor, out hue, out saturation, out value);

                    Color hsvColor = ColorFromHSV(hue, saturation * 100, value * 100);

                    hsvImage.SetPixel(x, y, hsvColor);
                }
            }

            pictureBoxHSV.Image = hsvImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int v = (int)numericUpDown2.Value;
            int s = (int)numericUpDown3.Value;
            int h = (int)numericUpDown1.Value;

            Bitmap originalImage = new Bitmap(sourceImageBox.Image);

            Bitmap hsvImage = new Bitmap(originalImage.Width, originalImage.Height);

            for (int x = 0; x < originalImage.Width; x++)
            {
                for (int y = 0; y < originalImage.Height; y++)
                {
                    Color rgbColor = originalImage.GetPixel(x, y);

                    double hue, saturation, value;
                    ColorToHSV(rgbColor, out hue, out saturation, out value);

                    hue += (360 / 100) * h / 100;
                    if (hue < 0) 
                    {
                        hue = 0;
                    }
                    if (hue > 360)
                    {
                        hue = 360;
                    }
                    saturation += 0.01 * s;
                    if (saturation < 0)
                    {
                        saturation = 0;
                    }
                    if (saturation > 1)
                    {
                       saturation = 1;
                    }
                    value += 0.01 * v;
                    if (value < 0)
                    {
                        value = 0;
                    }
                    if (value > 1)
                    {
                        value = 1;
                    }

                    Color hsvColor = ColorFromHSV(hue, saturation * 100, value * 100);

                    hsvImage.SetPixel(x, y, hsvColor);
                }
            }

            pictureBoxHSV.Image = hsvImage;
        }
    }
}
