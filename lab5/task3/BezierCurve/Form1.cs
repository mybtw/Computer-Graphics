using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BezierCurve
{
    public partial class Form1 : Form
    {
        private List<PointF> points;
        private Bitmap bmp;

        private float[,] BezierMatrix = { {  1,  -3, 3,  -1 },  // Матрица Безье
                                          {  0,  3,  -6,  3 },
                                          {  0,  0,  3,  -3 },
                                          {  0,  0,  0,  1 }};

        private PointF newPoint;
        private int index_point; // индекс точки, которую будем передвигать

        //перемножение матриц
        private float[,] multMatrix(float[,] m1, float[,] m2)
        {
            float[,] res = new float[m1.GetLength(0), m2.GetLength(1)];

            for (int i = 0; i < m1.GetLength(0); ++i)
                for (int j = 0; j < m2.GetLength(1); ++j)
                    for (int k = 0; k < m2.GetLength(0); k++)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }

            return res;
        }
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            pictureBox1.BackColor = Color.White;
            points = new List<PointF>();
            radioButton1.Checked = true;
            index_point = -1;
            newPoint = new Point();
        }
        // Очистка 
        private void button2_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            points = new List<PointF>();
            radioButton1.Checked = true;
            index_point = -1;
            newPoint = new PointF();
            pictureBox1.BackColor = Color.White;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked) // если выбрано "добавление точки"
            {
                points.Add(e.Location);
            }
            if (radioButton2.Checked) // если выбрано "удаление точки"
            {
                DeletePoint(e.Location.X, e.Location.Y);
            }
            DrawElements();
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioButton3.Checked)
            {
                index_point = points.FindIndex(el => (el.X > e.X - 5) && (el.X < e.X + 5) && (el.Y > e.Y - 5) && (el.Y < e.Y + 5));
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (radioButton3.Checked)
            {
                if (index_point >= 0)
                {
                    points[index_point] = new PointF(e.X, e.Y);
                    DeletePoint(newPoint.X, newPoint.Y);
                    DrawElements();
                    index_point = -1;
                }
            }
        }
        // Удаление точки
        private void DeletePoint(float x, float y)
        {
            int indexDel = points.FindIndex(el => (el.X > x - 5) && (el.X < x + 5) && (el.Y > y - 5) && (el.Y < y + 5));
            if (indexDel >= 0)
            {
                points.RemoveAt(indexDel);
                DrawElements();
            }
        }

        private void DrawElements()
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            DrawPoints();
            DrawCurve();
            
        }

        // Рисуем опорные точки
        private void DrawPoints()
        {
            SolidBrush solidBrush = new SolidBrush(Color.Red);
            Graphics g = Graphics.FromImage(bmp);
            Pen p = new Pen(Color.Red);
            foreach (var point in points)
            {
                if (point != newPoint)
                {
                    g.DrawEllipse(p, point.X - 3, point.Y - 3, 6, 6);
                    g.FillEllipse(solidBrush, point.X - 3, point.Y - 3, 6, 6);
                }
            }
            pictureBox1.Image = bmp;
        }
        
        // Функция для отрисовки каждой точки кривой
        private PointF GetNextPointOfCurve(PointF p0, PointF p1, PointF p2, PointF p3, float t)
        {
            float[,] MatrPointsX = { { p0.X, p1.X, p2.X, p3.X } };
            float[,] MatrPointsY = { { p0.Y, p1.Y, p2.Y, p3.Y } };

            float[,] MatrParametrs = { { 1 }, { t }, { t * t }, {t*t*t } };

            
            float X = multMatrix(multMatrix(MatrPointsX,BezierMatrix),MatrParametrs)[0, 0];
            float Y = multMatrix(multMatrix(MatrPointsY, BezierMatrix), MatrParametrs)[0, 0];

            return new PointF(X, Y);
        }

        // Нарисовать кривую по 4 опорным точкам
        private void DrawCurveFor4Points(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            int segments = 100; // Количество сегментов для отрисовки кривой
            PointF prevPixel = GetNextPointOfCurve(p0, p1, p2, p3, 0);

            using (Graphics g = Graphics.FromImage(bmp))
            using (Pen pen = new Pen(Color.Black))
            {
                for (int i = 1; i <= segments; i++)
                {
                    float t = i / (float)segments;
                    var pixel = GetNextPointOfCurve(p0, p1, p2, p3, t);

                    // Рисуем линию между предыдущей и текущей точкой
                    g.DrawLine(pen, prevPixel, pixel);

                    prevPixel = pixel;
                }
            }
        }

        // Получение дополнительных точек
        private PointF GetExtraPoint(PointF point1, PointF point2)
        {
            return new PointF((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
        }


        private void DrawCurveFor3Points(PointF p0, PointF p1, PointF p2)
        {
            PointF p12 = GetExtraPoint(p1, p2);
            DrawCurveFor4Points(p0, p1, p12, p2);

        }
        // Функция для отрисовки кривой по множеству точек
        private void DrawCurve()
        {
            PointF prev, next;
            prev = points[0];
            int points_size = points.Count();
            if (points_size == 4)
            {
                DrawCurveFor4Points(points[0], points[1], points[2], points[3]);
            }
            else if (points_size > 4)
            {
                for (int i = 0; i < points.Count - 4; i += 2)
                {
                    next = new PointF((points[i + 2].X + points[i + 3].X) / 2, (points[i + 2].Y + points[i + 3].Y) / 2);
                    DrawCurveFor4Points(prev, points[i + 1], points[i + 2], next);
                    prev = next;
                }
                if (points.Count % 2 == 0)
                    DrawCurveFor4Points(prev, points[points.Count - 3], points[points.Count - 2], points[points.Count - 1]);
                else
                    DrawCurveFor3Points(prev, points[points.Count - 2], points[points.Count - 1]);
            }
        }
    }
}
