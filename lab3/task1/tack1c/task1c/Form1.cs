using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace task1
{
    public partial class Form1 : Form
    {
        private Point start;
        private bool flag = false;
        private bool drawing = false;
        private Pen selectedColor = new Pen(Color.Green, 1);
        private Bitmap bmp;
        private Bitmap bmp2;
        List<Point> points = new List<Point>();
        int imageWidth = 0;
        int imageHeight = 0;
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.Clear(Color.White);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.selectedColor.Color = colorDialog.Color;
            }
        }
        private bool equalColors(Color c1, Color c2)
        {
            return Math.Abs(c1.R - c2.R) < 175 && Math.Abs(c1.G - c2.G) < 175 && Math.Abs(c1.B - c2.B) < 175;
        }

       
        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Выберите изображение";
                openFileDialog.Filter = "Изображения (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|Все файлы (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImagePath = openFileDialog.FileName;
                    pictureBox1.Image = System.Drawing.Image.FromFile(selectedImagePath);
                    this.bmp = new Bitmap(pictureBox1.Image);
                }
            }
        }
        List<Point> getBorderPoints(Point p)
        {
            Color formColor = bmp.GetPixel(p.X, p.Y);
            Point rightBound = new Point(p.X, p.Y);
            Color currentColor = formColor;
            while (rightBound.X < pictureBox1.Width - 1 && !equalColors(currentColor, Color.Black))
            {
                rightBound.X += 1;
                currentColor = bmp.GetPixel(rightBound.X, p.Y);
            }
            if (rightBound.X != bmp.Width - 1) 
            {
                rightBound.X += 1;
            }
            List<Point> res = new List<Point>();
            Stack<Point> s = new Stack<Point>();
            HashSet<Point> set = new HashSet<Point>();
            s.Push(rightBound);
            while (s.Count > 0)
            {
                Point curP = s.Pop();
                Color c = bmp.GetPixel(curP.X, curP.Y);
                if (curP.X >= 0 && curP.X < bmp.Width && curP.Y >= 0 && curP.Y < bmp.Height && equalColors(bmp.GetPixel(curP.X, curP.Y), Color.Black) && !set.Contains(curP))
                { 
                    res.Add(curP);
                    set.Add(curP);
                    foreach (var dir in dirs)
                    {
                        s.Push(new Point(curP.X + dir[0], curP.Y + dir[1]));
                    }
                }
            }
            return res;
        }

        List<List<int>> dirs = new List<List<int>> {new List<int>() {1, 0}, new List<int>() {1, 1}, new List<int>() { 0, 1 }, new List<int>() { -1, 1 },
        new List<int>() { -1, 0}, new List<int>() { -1, -1}, new List<int>() { 0, -1}, new List<int>() { 1, -1}, };

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            start = new Point(e.X, e.Y);
            List<Point> points1 = getBorderPoints(start);
            List<Point> bord = points1;
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                foreach (Point p in bord)
                {
                    graphics.FillRectangle(new SolidBrush(selectedColor.Color), new Rectangle(p.X, p.Y, 1, 1));
                }
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }






        private void button1_Click(object sender, EventArgs e)
        {
            points.Clear();
            flag = false;
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.Clear(Color.White);
            }
            pictureBox1.Invalidate();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Выберите изображение";
                openFileDialog.Filter = "Изображения (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|Все файлы (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImagePath = openFileDialog.FileName;
                    bmp = new Bitmap(selectedImagePath);
                    pictureBox1.Image = bmp;
                    imageWidth = bmp.Width; // Получить ширину изображения
                    imageHeight = bmp.Height;
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.selectedColor.Color = colorDialog.Color;
            }
        }
    }
}
