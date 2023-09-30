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
            radioButton2.Checked = true;
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
            return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
        }

        /*private void fill(Point p)
        {
            if (p.X >= 0 && p.X < bmp.Width && p.Y >= 0 && p.Y < bmp.Height && !bmp.GetPixel(p.X, p.Y).Equals(Color.Black) && !bmp.GetPixel(p.X, p.Y).Equals(selectedColor.Color))
            {
                Color formColor = bmp.GetPixel(p.X, p.Y);
                Point leftBound = new Point(p.X, p.Y);
                Point rightBound = new Point(p.X, p.Y);
                Color currentColor = formColor;
                while (0 < leftBound.X && !currentColor.Equals(Color.Black))      
                {
                    leftBound.X -= 1;
                    currentColor = bmp.GetPixel(leftBound.X, p.Y);
                }
                currentColor = formColor;
                while (rightBound.X < bmp.Width - 1 && !currentColor.Equals(Color.Black))
                {
                    rightBound.X += 1;
                    currentColor = bmp.GetPixel(rightBound.X, p.Y);
                }
                if (leftBound.X != 0)
                {
                    leftBound.X += 1;
                }
                if (rightBound.X != bmp.Width - 1)
                {
                    rightBound.X -= 1;
                }
                using (Graphics graphics = Graphics.FromImage(bmp))
                {
                    graphics.DrawLine(selectedColor,leftBound, rightBound);
                }
                pictureBox1.Invalidate();
                for (int i = leftBound.X; i <= rightBound.X; ++i)
                {
                    fill(new Point(i, p.Y + 1));
                    if (p.Y - 1 >= 0)
                    {
                        fill(new Point(i, p.Y - 1));
                    }
                }

            }
        }*/


        private void fill(Point p)
        {

            if (p.X < 0 || p.X >= bmp.Width || p.Y < 0 || p.Y >= bmp.Height || equalColors(bmp.GetPixel(p.X, p.Y), selectedColor.Color) || equalColors(bmp.GetPixel(p.X, p.Y), Color.Black))
            {
                return;
            }
            Color formColor = bmp.GetPixel(p.X, p.Y);
            Point leftBound = new Point(p.X, p.Y);
            Point rightBound = new Point(p.X, p.Y);
            Color currentColor = formColor;
            while (0 < leftBound.X && !equalColors(currentColor, Color.Black))
            {
                leftBound.X -= 1;
                currentColor = bmp.GetPixel(leftBound.X, p.Y);
            }
            currentColor = formColor;
            while (rightBound.X < pictureBox1.Width - 1 && !equalColors(currentColor, Color.Black))
            {
                rightBound.X += 1;
                currentColor = bmp.GetPixel(rightBound.X, p.Y);
            }
            if (leftBound.X != 0)
            {
                leftBound.X += 1;
            }
            if (rightBound.X != bmp.Width - 1)
            {
                rightBound.X -= 1;
            }
            if (rightBound.X - leftBound.X == 0)
            {
                bmp.SetPixel(rightBound.X, rightBound.Y, selectedColor.Color);
            }
            else
            {
                using (Graphics graphics = Graphics.FromImage(bmp))
                {
                    graphics.DrawLine(selectedColor, leftBound, rightBound);
                }
            }

            pictureBox1.Image = bmp;

            for (int i = leftBound.X; i < rightBound.X + 1; ++i)
            {
                fill(new Point(i, p.Y + 1));
            }

            for (int i = leftBound.X; i < rightBound.X + 1; ++i)
            {
                fill(new Point(i, p.Y - 1));
            }
        }


        private void fill2(Point p)
        {

            if (p.X < 0 || p.X >= bmp.Width || p.Y < 0 || p.Y >= bmp.Height || equalColors(bmp.GetPixel(p.X, p.Y), Color.Black) || equalColors(bmp.GetPixel(p.X, p.Y), bmp2.GetPixel(p.X % imageWidth, p.Y % imageHeight)))
            {
                return;
            }
            Color formColor = bmp.GetPixel(p.X, p.Y);
            Point leftBound = new Point(p.X, p.Y);
            Point rightBound = new Point(p.X, p.Y);
            Color currentColor = formColor;

            while (0 < leftBound.X && !equalColors(currentColor, Color.Black))
            {
                leftBound.X -= 1;
                currentColor = bmp.GetPixel(leftBound.X, p.Y);
            }
            currentColor = formColor;
            while (rightBound.X < pictureBox1.Width - 1 && !equalColors(currentColor, Color.Black))
            {
                rightBound.X += 1;
                currentColor = bmp.GetPixel(rightBound.X, p.Y);
            }
            if (leftBound.X != 0)
            {
                leftBound.X += 1;
            }
            if (rightBound.X != bmp.Width - 1)
            {
                rightBound.X -= 1;
            }
            /* for (int x = leftBound.X; x <= rightBound.X; x++)
             {
                 bmp.SetPixel(rightBound.X, rightBound.Y, bmp2.GetPixel(x % imageWidth, leftBound.Y % imageHeight));
             }*/
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                for (int i = leftBound.X; i <= rightBound.X; i++)
                {
                    graphics.FillRectangle(new SolidBrush(bmp2.GetPixel(i % imageWidth, leftBound.Y % imageHeight)), new Rectangle(i, leftBound.Y, 1, 1));
                }
            }

            pictureBox1.Invalidate();

            for (int i = leftBound.X; i < rightBound.X + 1; ++i)
            {
                fill2(new Point(i, p.Y + 1));
            }

            for (int i = leftBound.X; i < rightBound.X + 1; ++i)
            {
                fill2(new Point(i, p.Y - 1));
            }
        }


        private void fill3(Point p)
        {

            Stack<Point> stack = new Stack<Point>();
            stack.Push(p);
            while (stack.Count > 0)
            {
                if (p.X < 0 || p.X >= bmp.Width || p.Y < 0 || p.Y >= bmp.Height || equalColors(bmp.GetPixel(p.X, p.Y), Color.Black) || equalColors(bmp.GetPixel(p.X, p.Y), bmp2.GetPixel(p.X % imageWidth, p.Y % imageHeight)))
                {
                    stack.Pop();
                }
                else
                {
                    Point currentPoint = stack.Pop();
                    Color formColor = bmp.GetPixel(currentPoint.X, currentPoint.Y);
                    Point leftBound = new Point(currentPoint.X, currentPoint.Y);
                    Point rightBound = new Point(currentPoint.X, currentPoint.Y);
                    Color currentColor = formColor;

                    while (0 < leftBound.X && !equalColors(currentColor, Color.Black))
                    {
                        leftBound.X -= 1;
                        currentColor = bmp.GetPixel(leftBound.X, currentPoint.Y);
                    }
                    currentColor = formColor;
                    while (rightBound.X < pictureBox1.Width - 1 && !equalColors(currentColor, Color.Black))
                    {
                        rightBound.X += 1;
                        currentColor = bmp.GetPixel(rightBound.X, currentPoint.Y);
                    }
                    if (leftBound.X != 0)
                    {
                        leftBound.X += 1;
                    }
                    if (rightBound.X != bmp.Width - 1)
                    {
                        rightBound.X -= 1;
                    }
                    /* for (int x = leftBound.X; x <= rightBound.X; x++)
                     {
                         bmp.SetPixel(rightBound.X, rightBound.Y, bmp2.GetPixel(x % imageWidth, leftBound.Y % imageHeight));
                     }*/
                    using (Graphics graphics = Graphics.FromImage(bmp))
                    {
                        for (int i = leftBound.X; i < rightBound.X; i++)
                        {
                            graphics.FillRectangle(new SolidBrush(bmp2.GetPixel(i % imageWidth, leftBound.Y % imageHeight)), new Rectangle(i, leftBound.Y, 1, 1));
                        }
                    }

                    pictureBox1.Invalidate();

                    for (int i = leftBound.X; i < rightBound.X + 1; ++i)
                    {
                        stack.Push(new Point(i, currentPoint.Y + 1));
                    }

                    for (int i = leftBound.X; i < rightBound.X + 1; ++i)
                    {
                        stack.Push(new Point(i, currentPoint.Y - 1));
                    }
                }
            }
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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            selectedColor.Width = trackBar1.Value;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            start = new Point(e.X, e.Y);
            if (radioButton2.Checked)
            {
                drawing = true;
            }
            else if (radioButton1.Checked)
            {
                fill2(start);
            }
            else if (radioButton3.Checked)
            {
                if (flag)
                {
                    points.Add(new Point(e.X, e.Y));
                    using (Graphics bmpGraphics = Graphics.FromImage(bmp))
                    {
                        bmpGraphics.DrawLines(new Pen(Color.Black, trackBar1.Value), points.ToArray());
                    }
                    pictureBox1.Invalidate();
                }
                else
                {
                    points.Add(new Point(e.X, e.Y));
                    flag = true;
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!drawing) return;
            var finish = new Point(e.X, e.Y);
            var pen = new Pen(Color.Black, trackBar1.Value);
            using (Graphics bmpGraphics = Graphics.FromImage(bmp))
            {
                bmpGraphics.DrawLine(pen, start, finish);
            }
            pictureBox1.Invalidate();

            start = finish;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }


        private void radioButton2_MouseClick(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton1.Checked = false;
            }
            if (radioButton3.Checked)
            {
                radioButton3.Checked = false;
            }
            radioButton2.Checked = true;
        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton2.Checked = false;
            }
            if (radioButton3.Checked)
            {
                radioButton3.Checked = false;
            }
            radioButton1.Checked = true;

        }

        private void radioButton3_MouseClick(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton1.Checked = false;
            }
            if (radioButton2.Checked)
            {
                radioButton3.Checked = false;
            }
            radioButton3.Checked = true;
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
                    bmp2 = new Bitmap(selectedImagePath);
                    pictureBox2.Image = bmp2;
                    imageWidth = bmp2.Width; // Получить ширину изображения
                    imageHeight = bmp2.Height;
                }
            }
        }
    }
}
