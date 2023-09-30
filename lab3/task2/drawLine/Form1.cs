using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lesson3_Task2
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private Point startPoint;
        private Point endPoint;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);
        }

        /*private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
         {
             if (startPoint.IsEmpty)
             {
                 startPoint = e.Location;
             }
             else if (endPoint.IsEmpty)
             {
                 endPoint = e.Location;
                 DrawBresenhamLine(startPoint, endPoint);
                 startPoint = Point.Empty;
                 endPoint = Point.Empty;
             }
         }*/
        private void SetStartPoint(int x, int y)
        {
            startPoint = new Point(x, y);
        }

        private void SetEndPoint(int x, int y)
        {
            endPoint = new Point(x, y);
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            graphics.Clear(Color.White);
            int x1 = 45; //45
            int y1 = 500;//500
            int x2 = 900;//900
            int y2 = -90;//-90
            SetStartPoint(x1, y1);
            SetEndPoint(x2, y2);

            int A = y2 - y1;
            int B = x1 - x2;

            // Вычисляем C для исходной прямой
            int C = -(A * x1 + B * y1);

            int C1 = C - 10000;


            // Рассчитываем новые координаты начала и конца параллельной прямой
            int startX = x1;
            int startY = (-C1 - A * x1) / B;

            int endX = x2;
            int endY = (-C1 - A * x2) / B;
            DrawBresenhamLine(startPoint,endPoint);
            SetStartPoint(startX, startY);
            SetEndPoint(endX, endY);
            DrawWuLine(startPoint, endPoint);
        }

        /*private void DrawBresenhamLine(Point p1, Point p2)
          {
              int x1 = p1.X;
              int y1 = p1.Y;
              int x2 = p2.X;
              int y2 = p2.Y;

              int dx = Math.Abs(x2 - x1);
              int dy = Math.Abs(y2 - y1);

              int xi = x1;
              int yi = y1;

              int d0;

              if (dy <= dx)  // Gradient <= 1
              {
                  if (x1 < x2)
                  {
                      xi = x1;
                      yi = y1;
                  }
                  else
                  {
                      xi = x2;
                      yi = y2;
                      x2 = x1;
                      y2 = y1;
                  }

                  int d = 2 * dy - dx;

                  while (xi <= x2)
                  {
                      graphics.FillRectangle(Brushes.Black, xi, yi, 1, 1);

                      if (d < 0)
                      {
                          xi++;
                          d += 2 * dy;
                      }
                      else
                      {
                          xi++;
                          yi++;
                          d += 2 * (dy - dx);
                      }
                  }
              }
              else  // Gradient > 1
              {
                  if (y1 < y2)
                  {
                      xi = x1;
                      yi = y1;
                  }
                  else
                  {
                      xi = x2;
                      yi = y2;
                      y2 = y1;
                      x2 = x1;
                  }

                  int d = 2 * dx - dy;

                  while (yi <= y2)
                  {
                      graphics.FillRectangle(Brushes.Black, xi, yi, 1, 1);

                      if (d < 0)
                      {
                          yi++;
                          d += 2 * dx;
                      }
                      else
                      {
                          xi++;
                          yi++;
                          d += 2 * (dx - dy);
                      }
                  }
              }

              pictureBox1.Invalidate();
          }*/
       private void DrawBresenhamLine(Point p1, Point p2)
        {
            int x1 = p1.X;
            int y1 = p1.Y;
            int x2 = p2.X;
            int y2 = p2.Y;

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);

            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;

            int err;
            int e2;

            if (dy <= dx) // Gradient <= 1
            {
                err = 2 * dy - dx;

                while (x1 != x2)
                {
                    graphics.FillRectangle(Brushes.Black, x1, y1, 1, 1);

                    e2 = 2 * err;

                    if (e2 > -dx)
                    {
                        err -= dx;
                        y1 += sy;
                    }

                    if (e2 < dy)
                    {
                        err += dy;
                        x1 += sx;
                    }
                }
            }
            else // Gradient > 1
            {
                err = 2 * dx - dy;

                while (y1 != y2)
                {
                    graphics.FillRectangle(Brushes.Black, x1, y1, 1, 1);

                    e2 = 2 * err;

                    if (e2 > -dy)
                    {
                        err -= dy;
                        x1 += sx;
                    }

                    if (e2 < dx)
                    {
                        err += dx;
                        y1 += sy;
                    }
                }
            }

            pictureBox1.Invalidate();
        }
       private void DrawWuLine(Point p1, Point p2)
        {
            int x0 = p1.X;
            int y0 = p1.Y;
            int x1 = p2.X;
            int y1 = p2.Y;

            var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            DrawPoint(steep, x0, y0, 1);
            DrawPoint(steep, x1, y1, 1);

            float dx = x1 - x0;
            float dy = y1 - y0;
            float gradient = dy / dx;
            float y = y0 + gradient;

            for (var x = x0 + 1; x <= x1 - 1; x++)
            {
                DrawPoint(steep, x, (int)y, 1 - (y - (int)y));
                DrawPoint(steep, x, (int)y + 1, y - (int)y);
                y += gradient;
            }
        }

        private void DrawPoint(bool steep, int x, int y, float c)
        {
            int alpha = (int)(c * 255); 

            if (alpha < 0) alpha = 0; 
            if (alpha > 255) alpha = 255;

            if (steep)
            {
                Color currentColor = Color.FromArgb(alpha, Color.Black);
                graphics.FillRectangle(new SolidBrush(currentColor), y, x, 1, 1);
            }
            else
            {
                Color currentColor = Color.FromArgb(alpha, Color.Black);
                graphics.FillRectangle(new SolidBrush(currentColor), x, y, 1, 1);
            }
        }
        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}


