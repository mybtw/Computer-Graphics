using MathNet.Numerics.LinearAlgebra;

namespace lab6
{
    public partial class Form1 : Form
    {
        Graphics g;
        Bitmap bmp;
        List<Shape> figures;
        Shape figure;

        public Form1()
        {
            InitializeComponent();
            AddItemsToComboBox1();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(bmp);
            Point.worldCenter = new PointF(0, 0);
            figures = new List<Shape>();
            radioButton2.Checked = true;
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            numericUpDown1.Value = 100;
            numericUpDown2.Value = 100;
            numericUpDown3.Value = 100;
            numericUpDown4.Value = 100;
            numericUpDown1.Maximum = 200;
            numericUpDown2.Maximum = 200;
            numericUpDown3.Maximum = 200;
            numericUpDown4.Maximum = 200;
            clearScene();

        }

        private void AddItemsToComboBox1()
        {
            string[] items = { "тетраэдр", "гексаэдр", "октаэдр", "икосаэдр", "додекаэдр" };
            foreach (string item in items)
            {
                comboBox1.Items.Add(item);
            }
        }
   

        public void clearScene()
        {
            g.Clear(Color.White);
            pictureBox1.Invalidate();
        }

        public void redraw()
        {
            clearScene();
            draw(figure);
            /*foreach (var fig in figures)
            {
                draw(fig);
            }*/
           
        }

        private void draw(Shape figure)
        {
            drawShape(figure);
        }

        // Рисует выбранную фигуру
        private void button1_Click(object sender, EventArgs e)
        {
            clearScene();
            draw();

        }


        public void draw()
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: figure = Tetrahedron.getTetrahedron(); drawShape(figure); figures.Add(figure); break;
                case 1: figure = Hexahedron.getHexahedron(); drawShape(figure); figures.Add(figure); break;
                case 2: figure = Octahedron.getOctahedron(); drawShape(figure); figures.Add(figure); break;
                case 3: figure = Icosahedron.getIcosahedron(); drawShape(figure); figures.Add(figure); break;
                case 4: figure = Dodecahedron.getDodecahedron(); drawShape(figure); figures.Add(figure); break;
                default: figure = Hexahedron.getHexahedron(); drawShape(figure); figures.Add(figure); break;
            }
        }

        void drawShape(Shape shape)
        {
            foreach (var face in shape.Faces)
            {
                Pen pen = new Pen(Color.Black, 3);
                drawFace(face, pen);
            }
        }

        void drawFace(Face face, Pen pen)
        {
            foreach (var line in face.Edges)
            {
                drawLine(line, pen);
            }
        }

        void drawLine(Line line, Pen pen)
        {
            g.DrawLine(pen, line.start.project(), line.end.project());
        }


        private void radioButton2_MouseClick(object sender, MouseEventArgs e)
        {
            Point.projection = Projection.AXONOMETRIC;
            radioButton1.Checked = false;
            radioButton2.Checked = true;
            //redraw();

        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            Point.projection = Projection.PERSPECTIVE;
            radioButton2.Checked = false;
            radioButton1.Checked = true;
            //redraw();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            figures.Clear();
            clearScene();
        }
        private double[,] GetScaleMatrix(double x, double y, double z)
        {

            double[,] matrix = new double[4, 4];
            matrix[0, 0] = 1;
            matrix[1, 1] = 1;
            matrix[2, 2] = 1;
            matrix[3, 3] = 1;
            matrix[3, 0] = x;
            matrix[3, 1] = y;
            matrix[3, 2] = z;
            foreach (var face in figure.Faces)
            {
                foreach (var line in face.Edges)
                {
                    line.start = ApplyMatrix(matrix, line.start);
                    line.end = ApplyMatrix(matrix, line.end);
                }
            }

            return matrix;
        }
        private void button3_Click(object sender, EventArgs e)
        {
                // Считывание координат X, Y и Z из текстовых полей
                double offsetX = double.Parse(textBox1.Text);
                double offsetY = double.Parse(textBox2.Text);
                double offsetZ = double.Parse(textBox3.Text);

            GetScaleMatrix(offsetX, offsetY, offsetZ);
            redraw();

        }
        // Метод для умножения матрицы на точку
        Point ApplyMatrix(double[,] matrix, Point point)
        {
            double[] newPoint = new double[4];
            double[] pointArray = new double[]
                { point.X, point.Y, point.Z, 1 };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    newPoint[i] += pointArray[j] * matrix[j, i];
            return new Point(newPoint[0], newPoint[1], newPoint[2]);
        }
        // Метод для поворота 3D фигуры
        private void Rotate(double angleX, double angleY, double angleZ)
        {
            // Преобразуйте углы поворота из градусов в радианы
            double radiansX = Math.PI * angleX / 180.0;
            double radiansY = Math.PI * angleY / 180.0;
            double radiansZ = Math.PI * angleZ / 180.0;

            // Примените соответствующие матрицы поворота ко всем точкам фигуры
            ApplyRotation(radiansX, radiansY, radiansZ);
        }

        // Метод для применения матриц поворота ко всем точкам фигуры
        private void ApplyRotation(double radiansX, double radiansY, double radiansZ)
        {
            foreach (var face in figure.Faces)
            {
                foreach (var line in face.Edges)
                {
                    // Примените каждую из матриц поворота ко всем точкам
                    line.start = ApplyMatrix(GetRotationMatrixX(radiansX), line.start);
                    line.start = ApplyMatrix(GetRotationMatrixY(radiansY), line.start);
                    line.start = ApplyMatrix(GetRotationMatrixZ(radiansZ), line.start);

                    line.end = ApplyMatrix(GetRotationMatrixX(radiansX), line.end);
                    line.end = ApplyMatrix(GetRotationMatrixY(radiansY), line.end);
                    line.end = ApplyMatrix(GetRotationMatrixZ(radiansZ), line.end);
                }
            }
        }

        // Метод для создания матрицы поворота вокруг оси X
        private double[,] GetRotationMatrixX(double radians)
        {
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            double[,] rotationMatrixX = new double[,]
            {
                { 1, 0, 0, 0 },
                { 0, cos, sin, 0 },
                { 0, -sin, cos, 0 },
                { 0, 0, 0, 1 }
            };

            return rotationMatrixX;
        }

        // Метод для создания матрицы поворота вокруг оси Y
        private double[,] GetRotationMatrixY(double radians)
        {
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            double[,] rotationMatrixY = new double[,]
            {
                { cos, 0, -sin, 0 },
                { 0, 1, 0, 0 },
                { sin, 0, cos, 0 },
                { 0, 0, 0, 1 }
            };

            return rotationMatrixY;
        }

        // Метод для создания матрицы поворота вокруг оси Z
        private double[,] GetRotationMatrixZ(double radians)
        {
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            double[,] rotationMatrixZ = new double[,]
            {
                { cos, sin, 0, 0 },
                { -sin, cos, 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            };

            return rotationMatrixZ;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double angleX = double.Parse(textBox4.Text);
            double angleY = double.Parse(textBox5.Text);
            double angleZ = double.Parse(textBox6.Text);
            Rotate(angleX, angleY, angleZ);
            redraw();
        }
        //Матрица масштабирования
        private double[,] GetRotationMatrix(double scaleX, double scaleY, double scaleZ)
        {
            //Создание матрицы масштабирования
            double[,] scaleMatrix = new double[,]
            {
                { scaleX, 0, 0, 0 },
                { 0, scaleY, 0, 0 },
                { 0, 0, scaleZ, 0 },
                { 0, 0, 0, 1 }
            };
            // Применение матрицы масштабирования ко всем точкам фигуры
            foreach (var face in figure.Faces)
            {
                foreach (var line in face.Edges)
                {
                    line.start = ApplyMatrix(scaleMatrix, line.start);
                    line.end = ApplyMatrix(scaleMatrix, line.end);
                }
            }
            return scaleMatrix;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            double scaleX = (double)numericUpDown1.Value / 100;
            double scaleY = (double)numericUpDown2.Value / 100;
            double scaleZ = (double)numericUpDown3.Value / 100;
            GetRotationMatrix(scaleX, scaleY, scaleZ);
            redraw();
        }
        // Метод для нахождения центральной точки фигуры
        private Point CalculateCenter()
        {
            double totalX = 0;
            double totalY = 0;
            double totalZ = 0;
            int totalPoints = 0;

            foreach (var face in figure.Faces)
            {
                foreach (var line in face.Edges)
                {
                    totalX += line.start.X;
                    totalY += line.start.Y;
                    totalZ += line.start.Z;
                    totalPoints++;

                    totalX += line.end.X;
                    totalY += line.end.Y;
                    totalZ += line.end.Z;
                    totalPoints++;
                }
            }

            return new Point(totalX / totalPoints, totalY / totalPoints, totalZ / totalPoints);
        }
        // Матрица масштабирования относительно центра
        private double[,] GetRotationMatrixRelativeCenter(double scaleFactor)
        {
            // Найдите центральную точку фигуры
            Point center = CalculateCenter();
            //Создание матрицы масштабирования относительно центра
            double[,] scaleMatrix = new double[,]
            {
                  { scaleFactor, 0, 0, center.X * (1 - scaleFactor) },
                { 0, scaleFactor, 0, center.Y * (1 - scaleFactor) },
                { 0, 0, scaleFactor, center.Z * (1 - scaleFactor) },
                { 0, 0, 0, 1 }
            };
            // Применение матрицы масштабирования ко всем точкам фигуры
            foreach (var face in figure.Faces)
            {
                foreach (var line in face.Edges)
                {
                    line.start = ApplyMatrix(scaleMatrix, line.start);
                    line.end = ApplyMatrix(scaleMatrix, line.end);
                }
            }
            return scaleMatrix;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            double scaleFactor = (double)numericUpDown4.Value / 100;
            GetRotationMatrixRelativeCenter(scaleFactor);
            redraw();
        }
    }
}