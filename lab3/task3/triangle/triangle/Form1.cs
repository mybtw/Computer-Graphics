using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace triangle
{
    public partial class Form1 : Form
    {
        private static Color color1 = Color.Red;
        private static Color color2 = Color.Green;
        private static Color color3 = Color.Blue;
        private Bitmap image;
        private int width = 400;
        private int height = 400;
        private Point p1 = new Point(0, 0);
        private Point p2 = new Point(300, 100);
        private Point p3 = new Point(200, 320);

        public Form1()
        {
            InitializeComponent();
            image = new Bitmap(width, height);
            DrawTriangle();
            pictureBox1.Image = image;
        }

        private Color ShowColorDialog(Color color)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = color;
                dialog.AllowFullOpen = true;
                dialog.AnyColor = true;
                dialog.SolidColorOnly = false;
                dialog.ShowDialog();
                return dialog.Color;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            color1 = ShowColorDialog(color1);
            DrawTriangle();
            pictureBox1.Image = image;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            color2 = ShowColorDialog(color2);
            DrawTriangle();
            pictureBox1.Image = image;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            color3 = ShowColorDialog(color3);
            DrawTriangle();
            pictureBox1.Image = image;
        }
        private void DrawTriangle()
        {
            Graphics g2d = Graphics.FromImage(image);
            int totalHeight = p3.Y - p1.Y;

            for (int y = p1.Y; y <= p2.Y; y++)
            {
                int segmentHeight = p2.Y - p1.Y + 1;
                float alpha = (float)(y - p1.Y) / totalHeight;
                float beta = (float)(y - p1.Y) / segmentHeight;

                int x1 = (int)(p1.X + (p3.X - p1.X) * alpha);
                int x2 = (int)(p1.X + (p2.X - p1.X) * beta);

                Color colorA = InterpolateColor(color1, color3, alpha);
                Color colorB = InterpolateColor(color1, color2, beta);

                for (int x = x1; x <= x2; x++)
                {
                    float phi = (x1 == x2) ? 1.0f : (float)(x - x1) / (x2 - x1);
                    Color blendedColor = InterpolateColor(colorA, colorB, phi);
                    image.SetPixel(x, y, blendedColor);
                }
            }

            for (int y = p2.Y + 1; y <= p3.Y; y++)
            {
                int segmentHeight = p3.Y - p2.Y + 1;
                float alpha = (float)(y - p1.Y) / totalHeight;
                float beta = (float)(y - p2.Y) / segmentHeight;

                int x1 = (int)(p1.X + (p3.X - p1.X) * alpha);
                int x2 = (int)(p2.X + (p3.X - p2.X) * beta);

                Color colorA = InterpolateColor(color1, color3, alpha);
                Color colorB = InterpolateColor(color2, color3, beta);

                for (int x = x1; x <= x2; x++)
                {
                    float phi = (x1 == x2) ? 1.0f : (float)(x - x1) / (x2 - x1);
                    Color blendedColor = InterpolateColor(colorA, colorB, phi);
                    image.SetPixel(x, y, blendedColor);
                }
            }
            g2d.Dispose();
        }
        private Color InterpolateColor(Color c1, Color c2, float alpha)
        {
            int r = (int)(c1.R * (1 - alpha) + c2.R * alpha);
            int g = (int)(c1.G * (1 - alpha) + c2.G * alpha);
            int b = (int)(c1.B * (1 - alpha) + c2.B * alpha);
            return Color.FromArgb(r, g, b);
        }
    }
}
