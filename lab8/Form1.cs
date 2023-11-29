using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Data;
using System.Linq.Expressions;
using NCalc;
using Expression = NCalc.Expression;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;

namespace lab6
{
    public partial class Form1 : Form
    {

        Graphics g;
        Bitmap bmp;
        List<Shape> figures;
        Shape figure;
        String axis;
        int divs;
        int z;
        Vector viewVector = new Vector(0, 0, -1);
        private List<PointF> points = new List<PointF>();
        Pen pen = new Pen(Color.Black, 2);
        Line linse;
        private Camera camera = new Camera();
        private double[,] depthBuffer;
        private bool useZBuffer = false;
        private bool insidePolygon = false;
        private CheckBox checkBox2;

        private class Section
        {
            public Point leftP, rightP;
            public Section() { }
            public Section(Point l, Point r) { leftP = l; rightP = r; }
        }

        public Form1()
        {
            InitializeComponent();
            AddItemsToComboBox1();
            AddItemsToComboBox2();
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
            textBox22.Text = "0";
            textBox23.Text = "0";
            textBox24.Text = "-1";
            textBox25.Text = "0";
            textBox26.Text = "0";
            textBox27.Text = "0";
            textBox28.Text = "0";
            textBox29.Text = "0";
            textBox30.Text = "0";
            numericUpDown1.Value = 100;
            numericUpDown2.Value = 100;
            numericUpDown3.Value = 100;
            numericUpDown4.Value = 100;
            numericUpDown1.Maximum = 200;
            numericUpDown2.Maximum = 200;
            numericUpDown3.Maximum = 200;
            numericUpDown4.Maximum = 200;
            Point.worldCenter = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            camera.Offset = Point.worldCenter;
            //camera.Focus = new Point(0, 0, 1000);
            pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            depthBuffer = new double[ClientSize.Width, ClientSize.Height];
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

        private void AddItemsToComboBox2()
        {
            comboBox2.Items.Add("X");
            comboBox2.Items.Add("Y");
            comboBox2.Items.Add("Z");
        }

        public void clearScene()
        {
            g.Clear(Color.White);
            pictureBox1.Invalidate();
        }
        private void ApplyTransformationToFigure(Shape shape, double[,] transformationMatrix)
        {
            foreach (var face in shape.Faces)
            {
                foreach (var line in face.Edges)
                {
                    line.start = ApplyMatrix(transformationMatrix, line.start);
                    line.end = ApplyMatrix(transformationMatrix, line.end);
                }
            }
        }
        public void redraw()
        {
            clearScene();
            double[,] viewMatrix = camera.GetViewMatrix();
            foreach (var figure in figures)
            {
                if (figure != null)
                {
                    ApplyTransformationToFigure(figure, viewMatrix);
                    if (useZBuffer)
                    {
                        ClearDepthBuffer();
                        drawWithZBuffer(figure);
                    }
                    else
                    {
                        draw(figure);
                    }
                }
            }
            if (linse != null && linse.start != null && linse.end != null)
            {
                Pen pen = new Pen(Color.Black, 3);
                drawLine(linse, pen);
            }
            pictureBox1.Invalidate();
        }

        private void draw(Shape figure)
        {
            drawShape(figure);
        }

        // Рисует выбранную фигуру
        private void button1_Click(object sender, EventArgs e)
        {
            // clearScene();
            draw();
            if (linse != null && linse.start != null && linse.end != null)
            {
                Pen pen = new Pen(Color.Black, 3);
                drawLine(linse, pen);
            }
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
            pictureBox1.Invalidate();
        }

        void drawShape(Shape shape)
        {
            shape.calcNormals();
            foreach (var face in shape.Faces)
            {
                if (Vector.scalar(face.normal, viewVector) > 0)
                {
                    Pen pen = new Pen(Color.Black, 3);
                    drawFace(face, pen);
                }
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

        private void drawWithZBuffer(Shape shape)
        {
            shape.calcNormals();
            foreach (var face in shape.Faces)
            {
                if (checkBox2.Checked) 
                {
                if (Vector.scalar(face.normal, viewVector) > 0)
                {
                    Pen pen = new Pen(Color.Black, 3);
                    drawFaceWithZBuffer(face, pen);
                }
                }
                else
                {
                    Pen pen = new Pen(Color.Black, 3);
                    drawFaceWithZBuffer(face, pen);
                }
            }
        }

        private void drawFaceWithZBuffer(Face face, Pen pen)
        {
            foreach (var line in face.Edges)
            {
                drawLineWithZBuffer(line, pen);
            }
        }

        private void drawLineWithZBuffer(Line line, Pen pen)
        {
            PointF startPoint = line.start.project();
            PointF endPoint = line.end.project();

            int x1 = (int)startPoint.X;
            int y1 = (int)startPoint.Y;
            int x2 = (int)endPoint.X;
            int y2 = (int)endPoint.Y;

            for (int t = 0; t <= 100; t++)
            {
                float x = x1 + (float)t / 100.0f * (x2 - x1);
                float y = y1 + (float)t / 100.0f * (y2 - y1);


                double z = line.start.Z + (t / 100.0) * (line.end.Z - line.start.Z);

                if (CheckDepth((int)x, (int)y, z))
                {
                    g.DrawLine(pen, x, y, x + 1, y + 1); // рисовать только если прошла проверка по z
                }
                else
                {
                    insidePolygon = true;
                }
            }
        }

        private void ClearDepthBuffer()
        {
            // Очистка буфера глубины
            for (int x = 0; x < ClientSize.Width; x++)
            {
                for (int y = 0; y < ClientSize.Height; y++)
                {
                    depthBuffer[x, y] = double.MaxValue;
                }
            }
        }

        private bool CheckDepth(int x, int y, double z)
        {
            if (x >= 0 && x < ClientSize.Width && y >= 0 && y < ClientSize.Height)
            {
                if (z > 0 && z < depthBuffer[x, y])

                {
                    depthBuffer[x, y] = z;
                    return true;
                }
            }
            return false;
        }

        private void radioButton2_MouseClick(object sender, MouseEventArgs e)
        {
            Point.projection = Projection.AXONOMETRIC;
            radioButton1.Checked = false;
            radioButton2.Checked = true;
            redraw();

        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            Point.projection = Projection.PERSPECTIVE;
            radioButton2.Checked = false;
            radioButton1.Checked = true;
            redraw();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            points.Clear();
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

        private void RotateAroundX(double rotationAngleDegrees)
        {
            Point center = CalculateCenter(); // Найдите центр многогранника
            double angle = Math.PI / 180.0 * rotationAngleDegrees; // Угол в радианах
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            double[,] rotationMatrixX = new double[,]
            {
        { 1, 0, 0, 0 },
        { 0, cos, -sin, 0 },
        { 0, sin, cos, 0 },
        { 0, 0, 0, 1 }
            };

            foreach (var face in figure.Faces)
            {
                foreach (var line in face.Edges)
                {
                    line.start = ApplyMatrix(rotationMatrixX, line.start - center) + center;
                    line.end = ApplyMatrix(rotationMatrixX, line.end - center) + center;
                }
            }

            redraw();
        }

        private void RotateAroundY(double rotationAngleDegrees)
        {
            Point center = CalculateCenter(); // Найдите центр многогранника
            double angle = Math.PI / 180.0 * rotationAngleDegrees; // Угол в радианах
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            double[,] rotationMatrixY = new double[,]
            {
        { cos, 0, sin, 0 },
        { 0, 1, 0, 0 },
        { -sin, 0, cos, 0 },
        { 0, 0, 0, 1 }
            };

            foreach (var face in figure.Faces)
            {
                foreach (var line in face.Edges)
                {
                    line.start = ApplyMatrix(rotationMatrixY, line.start - center) + center;
                    line.end = ApplyMatrix(rotationMatrixY, line.end - center) + center;
                }
            }

            redraw();
        }

        private void RotateAroundZ(double rotationAngleDegrees)
        {
            Point center = CalculateCenter(); // Найдите центр многогранника
            double angle = Math.PI / 180.0 * rotationAngleDegrees; // Угол в радианах
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            double[,] rotationMatrixZ = new double[,]
            {
        { cos, -sin, 0, 0 },
        { sin, cos, 0, 0 },
        { 0, 0, 1, 0 },
        { 0, 0, 0, 1 }
            };

            foreach (var face in figure.Faces)
            {
                foreach (var line in face.Edges)
                {
                    line.start = ApplyMatrix(rotationMatrixZ, line.start - center) + center;
                    line.end = ApplyMatrix(rotationMatrixZ, line.end - center) + center;
                }
            }

            redraw();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            foreach (var face in figure.Faces)
            {
                foreach (var line in face.Edges)
                {
                    // Отражение относительно плоскости XY
                    line.start.Z = -line.start.Z;
                    line.end.Z = -line.end.Z;
                }
            }

            redraw();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            foreach (var face in figure.Faces)
            {
                foreach (var line in face.Edges)
                {
                    // Отражение относительно плоскости XZ
                    line.start.Y = -line.start.Y;
                    line.end.Y = -line.end.Y;
                }
            }

            redraw();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (var face in figure.Faces)
            {
                foreach (var line in face.Edges)
                {
                    // Отражение относительно плоскости YZ
                    line.start.X = -line.start.X;
                    line.end.X = -line.end.X;
                }
            }

            redraw();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            double degree = (double)numericUpDown5.Value;
            RotateAroundX(degree);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            double degree = (double)numericUpDown5.Value;
            RotateAroundY(degree);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            double degree = (double)numericUpDown5.Value;
            RotateAroundZ(degree);
        }



        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void RotateShapeAroundLine(Point point1, Point point2, double angleDegrees)
        {
            double angleRadians = Funcs.degreesToRadians(angleDegrees);

            double cosAngle = Math.Cos(angleRadians);
            double sinAngle = Math.Sin(angleRadians);

            foreach (Face face in figure.Faces)
            {
                foreach (Line line in face.Edges)
                {
                    Point relativeStart = line.start - point2;
                    Point relativeEnd = line.end - point2;

                    // Поворот относительных координат
                    double newXStart = relativeStart.X * cosAngle - relativeStart.Y * sinAngle;
                    double newYStart = relativeStart.X * sinAngle + relativeStart.Y * cosAngle;

                    double newXEnd = relativeEnd.X * cosAngle - relativeEnd.Y * sinAngle;
                    double newYEnd = relativeEnd.X * sinAngle + relativeEnd.Y * cosAngle;

                    // Возврат точек в абсолютные координаты
                    line.start = new Point(newXStart, newYStart, relativeStart.Z) + point2;
                    line.end = new Point(newXEnd, newYEnd, relativeEnd.Z) + point2;
                }
            }
        }


        private void button13_Click(object sender, EventArgs e)
        {
            double degree = (double)numericUpDown5.Value;
            if (linse != null)
            {
                RotateShapeAroundLine(linse.start, linse.end, degree);
                redraw();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Point start = new Point(int.Parse(textBox7.Text), int.Parse(textBox8.Text), int.Parse(textBox9.Text));
            Point end = new Point(int.Parse(textBox10.Text), int.Parse(textBox11.Text), int.Parse(textBox12.Text));
            linse = new Line(start, end);
            redraw();
        }
        private void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var face in figure.Faces)
                {
                    writer.WriteLine("Face");
                    foreach (var edge in face.Edges)
                    {
                        writer.WriteLine($"Line {edge.start.X} {edge.start.Y} {edge.start.Z} {edge.end.X} {edge.end.Y} {edge.end.Z}");
                    }
                }
            }
        }

        private void LoadFromFile(string filePath)
        {
            figure = new Shape();
            using (StreamReader reader = new StreamReader(filePath))
            {
                Face currentFace = new Face();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "Face")
                    {
                        currentFace = new Face();
                        figure.addFace(currentFace);
                    }
                    else if (line.StartsWith("Line"))
                    {
                        string[] parts = line.Split(' ');
                        double x1 = double.Parse(parts[1]);
                        double y1 = double.Parse(parts[2]);
                        double z1 = double.Parse(parts[3]);
                        double x2 = double.Parse(parts[4]);
                        double y2 = double.Parse(parts[5]);
                        double z2 = double.Parse(parts[6]);

                        currentFace.addEdge(new Line(new Point(x1, y1, z1), new Point(x2, y2, z2)));
                    }
                }
            }

            redraw();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            clearScene();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog1.FileName;
                if (File.Exists(fName))
                {
                    LoadFromFile(fName);
                    redraw();
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fName = saveFileDialog1.FileName;
                SaveToFile(fName);
            }
        }


        Shape BuildSurface(Func<double, double, double> f, double x0, double x1, double y0, double y1, int stepsX, int stepsY)
        {
            Shape surface = new Shape();

            double stepX = (x1 - x0) / stepsX;
            double stepY = (y1 - y0) / stepsY;

            for (int i = 0; i < stepsX; i++)
            {
                for (int j = 0; j < stepsY; j++)
                {
                    double x = x0 + i * stepX;
                    double y = y0 + j * stepY;
                    double z = f(x, y);

                    Point p1 = new Point(x, y, z);
                    Point p2 = new Point(x + stepX, y, f(x + stepX, y));
                    Point p3 = new Point(x + stepX, y + stepY, f(x + stepX, y + stepY));
                    Point p4 = new Point(x, y + stepY, f(x, y + stepY));

                    Face face = new Face();
                    face.addEdge(new Line(p1, p2))
                        .addEdge(new Line(p2, p3))
                        .addEdge(new Line(p3, p4))
                        .addEdge(new Line(p4, p1));

                    surface.addFace(face);
                }
            }

            return surface;
        }


        double SinCos(double x, double y)
        {
            return Math.Sin(x) + Math.Cos(y);
        }

        static double EvaluateExpression(string expression, double x, double y)
        {
            // Создаем экземпляр NCalc Expression
            Expression exp = new Expression(expression);

            exp.Parameters["x"] = x;
            exp.Parameters["y"] = y;

            object result = exp.Evaluate();

            return Convert.ToDouble(result);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            double x0 = double.Parse(textBox14.Text);
            double x1 = double.Parse(textBox13.Text);
            double y0 = double.Parse(textBox15.Text);
            double y1 = double.Parse(textBox16.Text);
            int stepsX = int.Parse(textBox17.Text);
            int stepsY = int.Parse(textBox18.Text);
            string expression = textBox19.Text;
            Func<double, double, double> Func = (x, y) => EvaluateExpression(expression, x, y);

            figure = BuildSurface(Func, x0, x1, y0, y1, stepsX, stepsY);
            drawShape(figure);
            figures.Add(figure);
            redraw();
        }



        // ставим точки на pictureBox
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            points.Add(new PointF(e.Location.X - Point.worldCenter.X, e.Location.Y - Point.worldCenter.Y));
            g.FillEllipse(Brushes.Black, e.Location.X - 5, e.Location.Y - 5, 5, 5);
            pictureBox1.Invalidate();
        }
        // соединяем точки в многоугольник по кнопке draw
        private void button16_Click(object sender, EventArgs e)
        {
            var pointsToDraw = points.Select(point => new PointF(point.X + Point.worldCenter.X, point.Y + Point.worldCenter.Y));
            g.DrawPolygon(pen, pointsToDraw.ToArray());
            pictureBox1.Invalidate();
        }


        // получаем матрицу вращения относительно выбранной оси


        // получаем значения параметров для фигуры вращения (ось, кол-во разбиений, z)
        public void getFieldsValue()
        {
            axis = comboBox2.Text;
            divs = int.Parse(textBox21.Text);
            z = int.Parse(textBox20.Text);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            getFieldsValue();
            figure = RotationShape.getRotationShape(divs, axis, points, z);
            redraw();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            double x = double.Parse(textBox22.Text);
            double y = double.Parse(textBox23.Text);
            double z = double.Parse(textBox24.Text);
            camera.Position = new Point(x, y, z);
            this.viewVector = (new Vector(x, y, z)).normalize();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            double rotateX = double.Parse(textBox28.Text);
            double rotateY = double.Parse(textBox29.Text);
            double rotateZ = double.Parse(textBox30.Text);
            camera.RotateCamera(rotateX, rotateY, rotateZ);
            double offsetX = double.Parse(textBox25.Text);
            double offsetY = double.Parse(textBox26.Text);
            double offsetZ = double.Parse(textBox27.Text);

            camera.MoveCamera(offsetX, offsetY, offsetZ);
            redraw();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2 = new CheckBox();
                checkBox2.Text = "Грани?";
                checkBox2.Location = new System.Drawing.Point(checkBox1.Location.X, checkBox1.Location.Y + checkBox1.Height + 10);
                this.Controls.Add(checkBox2);
                checkBox2.Checked = true;
            }
            else
            {
                if (checkBox2 != null)
                {
                    this.Controls.Remove(checkBox2);
                    checkBox2.Dispose();
                    checkBox2 = null;
                }
            }
            if (checkBox1.Checked)
            {
                useZBuffer = true;
                clearScene();
                redraw();
            }
            else
            {
                useZBuffer = false;
                clearScene();
                redraw();
            }
            
        }

    }
}