using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace midpoint_displacement
{
   

    public partial class Form1 : Form
    {
        private MountainGenerator mountainGenerator;
        private Bitmap mountainBitmap;
        private int depth; // Глубина рекурсии
        private int displacementRange; // Величина смещения
        private int initialHeight; // Начальные высоты
        private bool smoothing; // Флаг сглаживания
        int delayMilliseconds = 300; // Время задержки между шагами (в миллисекундах)
        public Form1()

        {
            InitializeComponent();
            depth = 7;
            displacementRange = 20;
            initialHeight = pictureBox1.Height / 2;
            smoothing = false;
            mountainGenerator = new MountainGenerator();
            mountainBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            GenerateMountainAsync();
        }

        private async Task GenerateMountainAsync()
        {
            await Task.Run(async () =>
            { 
                mountainBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(mountainBitmap);

                int[] points = new int[pictureBox1.Width];
                points[0] = initialHeight;
                points[pictureBox1.Width - 1] = initialHeight;

                for (int step = 0; step < depth; step++)
                {
                    mountainGenerator.GenerateMountainRecursive(points, 0, pictureBox1.Width - 1, step, displacementRange);

                    // Отрисовка горного массива после каждого шага
                    DrawMountain(g, points);
                    pictureBox1.Image = mountainBitmap;

                    await Task.Delay(delayMilliseconds);
                    if (smoothing)
                    {
                        mountainGenerator.Smooth(points, 18);
                    }
                }
            });
        }

        private void DrawMountain(Graphics g, int[] points)
        {
            g.Clear(Color.White);
            Pen pen = new Pen(Color.Black);
            for (int x = 0; x < points.Length - 1; x++)
            {
                g.DrawLine(pen, x, pictureBox1.Height - points[x], x + 1, pictureBox1.Height - points[x + 1]);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Int32.TryParse(textBox1.Text, out depth);
            Int32.TryParse(textBox2.Text, out displacementRange);
            Int32.TryParse(textBox3.Text, out initialHeight);
            Int32.TryParse(textBox4.Text, out delayMilliseconds);
            GenerateMountainAsync();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            smoothing = checkBox1.Checked;
        }
    }

    public class MountainGenerator
    {
        private Random random = new Random();  

        public void GenerateMountainRecursive(int[] points, int left, int right, int depth, int displacementRange)
        {
            if (depth == 0)
                return;

            int middle = (left + right) / 2;
            int displacement = (points[left] + points[right]) / 2 + random.Next(-displacementRange, displacementRange);

            points[middle] = displacement;

            GenerateMountainRecursive(points, left, middle, depth - 1, displacementRange);
            GenerateMountainRecursive(points, middle, right, depth - 1, displacementRange);
        }

        public void Smooth(int[] points, int passes)
        {
            int[] smoothedPoints = new int[points.Length];

            for (int i = 0; i < passes; i++)
            {
                for (int x = 1; x < points.Length - 1; x++)
                {
                    smoothedPoints[x] = (points[x - 1] + points[x] + points[x + 1]) / 3;
                }

                for (int x = 1; x < points.Length - 1; x++)
                {
                    points[x] = smoothedPoints[x];
                }
            }
        }
    }

}
