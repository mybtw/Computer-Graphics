using System.Reflection;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace lab4
{
    public partial class Form1 : Form
    {
        PointF prevPoint;
        PointF startPoint;
        bool drawingDots;
        bool drawingLines;
        bool drawingPolygon;
        List<PointF> polygonVertices = new List<PointF>();
        Pen pen;
        Bitmap bmp;
        bool flag = false;
        Graphics bmpGraphics;
        public Form1()
        {
            pen = new Pen(Color.Black, 1);
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            bmpGraphics = Graphics.FromImage(bmp);
            clear();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void clear()
        {
            bmpGraphics.Clear(Color.White);
            polygonVertices.Clear();
            pictureBox1.Invalidate();
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

        private void radioButton3_MouseClick(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton1.Checked = false;
            }
            if (radioButton2.Checked)
            {
                radioButton2.Checked = false;
            }
            radioButton3.Checked = true;

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = new PointF(e.X, e.Y);
            if (radioButton1.Checked)
            {
                bmpGraphics.DrawRectangle(pen, startPoint.X, startPoint.Y, 1, 1);
                pictureBox1.Invalidate();
            }
            else if (radioButton2.Checked)
            {
                if (flag)
                {
                    bmpGraphics.DrawLine(pen, prevPoint, startPoint);
                    pictureBox1.Invalidate();
                    flag = false;
                }
                else
                {
                    prevPoint = startPoint;
                    flag = true;
                }
            }
            else if (radioButton3.Checked)
            {

                bmpGraphics.DrawRectangle(pen, startPoint.X, startPoint.Y, 1, 1);
                pictureBox1.Invalidate();
                polygonVertices.Add(startPoint);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                bmpGraphics.DrawPolygon(pen, polygonVertices.ToArray());
                pictureBox1.Invalidate();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        /*Смещение полигона на dx и dy*/
        private void button3_Click(object sender, EventArgs e)
        {
            float dx = float.Parse(textBox1.Text);
            float dy = float.Parse(textBox2.Text);
            Matrix<float> matrix = Matrix<float>.Build.DenseOfArray(new float[,] {
            { 1, 0, 0 },
            { 0, 1, 0 },
            { dx, dy, 1}
            });
            List<PointF> nextPolygonVertices = new List<PointF>();
            foreach (PointF p in polygonVertices)
            {
                Matrix<float> oldPoint = Matrix<float>.Build.DenseOfArray(new float[,] {
                { p.X, p.Y, 1},
                });
                Matrix<float> resPoint = oldPoint * matrix;
                nextPolygonVertices.Add(new PointF(resPoint[0, 0], resPoint[0, 1]));
            }
            this.polygonVertices = nextPolygonVertices;
            redrawPolygon();
        }
        void redrawPolygon()
        {
            bmpGraphics.Clear(Color.White);
            bmpGraphics.DrawPolygon(pen, polygonVertices.ToArray());
            pictureBox1.Invalidate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PointF center = new PointF(0, 0);
            foreach (PointF p in polygonVertices)
            {
                center.X += p.X;
                center.Y += p.Y;
            }
            center.X /= polygonVertices.Count;
            center.Y /= polygonVertices.Count;


            double rad = int.Parse(textBox3.Text) * Math.PI / 180.0;
            float cosA = (float)Math.Cos(rad);
            float sinA = (float)Math.Sin(rad);


            Matrix<float> matrix = Matrix<float>.Build.DenseOfArray(new float[,] {
            { cosA, sinA, 0 },
            { -sinA, cosA, 0 },
            { 0, 0, 1}
            });


            List<PointF> nextPolygonVertices = new List<PointF>();
            foreach (PointF p in polygonVertices)
            {
                Matrix<float> oldPoint = Matrix<float>.Build.DenseOfArray(new float[,] {
                { p.X - center.X, p.Y - center.Y, 1},
                });
                Matrix<float> resPoint = oldPoint * matrix;
                nextPolygonVertices.Add(new PointF(resPoint[0, 0] + center.X, resPoint[0, 1] + center.Y));
            }
            this.polygonVertices = nextPolygonVertices;
            redrawPolygon();
        }
    }


}