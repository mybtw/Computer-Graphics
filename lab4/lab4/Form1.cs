using System;
using System.Drawing;
using System.Reflection;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;

namespace lab4
{
    public partial class Form1 : Form
    {
        PointF prevPoint;
        PointF startPoint;
        PointF rotationPoint;
        List<PointF> polygonVertices = new List<PointF>();
        List<PointF> points = new List<PointF>();
        List<Section> sections = new List<Section>();
        Pen pen;
        Bitmap bmp;
        bool flag = false;
        Graphics bmpGraphics;
        private List<List<PointF>> polygons = new List<List<PointF>>();
        private class Section
        {
            public PointF leftP, rightP;
            public Section() { }
            public Section(PointF l, PointF r) { leftP = l; rightP = r; }
        }
        public Form1()
        {
            pen = new Pen(Color.Black, 1);
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            bmpGraphics = Graphics.FromImage(bmp);
            clear();
            numericUpDownX.Value = 100;
            numericUpDownY.Value = 100;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void clear()
        {
            bmpGraphics.Clear(Color.White);
            polygons.Clear();
            points.Clear();
            sections.Clear();
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
                rotationPoint = startPoint;
                bmpGraphics.DrawRectangle(pen, startPoint.X, startPoint.Y, 1, 1);
                points.Add(startPoint);
                pictureBox1.Invalidate();
            }
            else if (radioButton2.Checked)
            {
                if (flag)
                {
                    bmpGraphics.DrawLine(pen, prevPoint, startPoint);
                    sections.Add(new Section(startPoint, prevPoint));
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
                polygons.Add(polygonVertices.ToList());
                polygonVertices.Clear();
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
            if (polygons.Count > 0)
            {
                foreach (PointF p in polygons.Last())
                {
                    Matrix<float> oldPoint = Matrix<float>.Build.DenseOfArray(new float[,] {
                { p.X, p.Y, 1},
                });
                    Matrix<float> resPoint = oldPoint * matrix;
                    nextPolygonVertices.Add(new PointF(resPoint[0, 0], resPoint[0, 1]));
                }
                polygons.RemoveAt(polygons.Count - 1);
                polygons.Add(nextPolygonVertices);
                redrawPolygon();
            }
        }
        void redrawPolygon()
        {
            bmpGraphics.Clear(Color.White);
            foreach (var poly in polygons)
            {
                bmpGraphics.DrawPolygon(pen, poly.ToArray());
            }

            foreach (PointF point in points)
            {
                bmpGraphics.DrawRectangle(pen, point.X, point.Y, 1, 1);
            }
            foreach (Section section in sections)
            {
                bmpGraphics.DrawLine(pen, section.leftP, section.rightP);
            }
            // bmpGraphics.DrawPolygon(pen, polygons.Last().ToArray());
            pictureBox1.Invalidate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (polygons.Count > 0)
            {
                PointF center = new PointF(0, 0);
                foreach (PointF p in polygons.Last())
                {
                    center.X += p.X;
                    center.Y += p.Y;
                }
                center.X /= polygons.Last().Count;
                center.Y /= polygons.Last().Count;


                double rad = int.Parse(textBox3.Text) * Math.PI / 180.0;
                float cosA = (float)Math.Cos(rad);
                float sinA = (float)Math.Sin(rad);


                Matrix<float> matrix = Matrix<float>.Build.DenseOfArray(new float[,] {
            { cosA, sinA, 0 },
            { -sinA, cosA, 0 },
            { 0, 0, 1}
            });


                List<PointF> nextPolygonVertices = new List<PointF>();
                foreach (PointF p in polygons.Last())
                {
                    Matrix<float> oldPoint = Matrix<float>.Build.DenseOfArray(new float[,] {
                { p.X - center.X, p.Y - center.Y, 1},
                });
                    Matrix<float> resPoint = oldPoint * matrix;
                    nextPolygonVertices.Add(new PointF(resPoint[0, 0] + center.X, resPoint[0, 1] + center.Y));
                }
                polygons.RemoveAt(polygons.Count - 1);
                polygons.Add(nextPolygonVertices);
                redrawPolygon();
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (rotationPoint != PointF.Empty)
            {
                float angleDegrees = float.Parse(textBox3.Text);

                // Переносим центр вращения к началу координат
                List<PointF> translatedVertices = new List<PointF>();
                foreach (PointF p in polygons.Last())
                {
                    float xTranslated = p.X - rotationPoint.X;
                    float yTranslated = p.Y - rotationPoint.Y;
                    translatedVertices.Add(new PointF(xTranslated, yTranslated));
                }

                // Вычисляем угол в радианах
                double angleRadians = angleDegrees * Math.PI / 180.0;
                float cosA = (float)Math.Cos(angleRadians);
                float sinA = (float)Math.Sin(angleRadians);

                // Матрица для поворота
                Matrix<float> rotationMatrix = Matrix<float>.Build.DenseOfArray(new float[,] {
            { cosA, -sinA, 0 },
            { sinA, cosA, 0 },
            { 0, 0, 1 }
        });

                // Поворачиваем вершины
                List<PointF> rotatedVertices = new List<PointF>();
                foreach (PointF p in translatedVertices)
                {
                    // Проверяем, является ли вершина центром вращения
                    if (p != PointF.Empty)
                    {
                        Matrix<float> oldPoint = Matrix<float>.Build.DenseOfArray(new float[,] {
                    { p.X, p.Y, 1 }
                });

                        Matrix<float> resPoint = oldPoint * rotationMatrix;
                        rotatedVertices.Add(new PointF(resPoint[0, 0], resPoint[0, 1]));
                    }
                    else
                    {
                        // Если вершина является центром вращения, не поворачиваем её
                        rotatedVertices.Add(p);
                    }
                }

                // Переносим вершины обратно
                List<PointF> nextPolygonVertices = new List<PointF>();
                for (int i = 0; i < rotatedVertices.Count; i++)
                {
                    // Восстанавливаем координаты вершин исключая центр вращения
                    if (polygons.Last()[i] != rotationPoint)
                    {
                        float xFinal = rotatedVertices[i].X + rotationPoint.X;
                        float yFinal = rotatedVertices[i].Y + rotationPoint.Y;
                        nextPolygonVertices.Add(new PointF(xFinal, yFinal));
                    }
                    else
                    {
                        nextPolygonVertices.Add(rotationPoint);
                    }
                }
                polygons.RemoveAt(polygons.Count - 1);
                polygons.Add(nextPolygonVertices);
                redrawPolygon();
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {

            float scaleX = (float)numericUpDownX.Value / 100f; // Переводим в проценты
            float scaleY = (float)numericUpDownY.Value / 100f;

            // Масштабирование относительно центральной точки
            ScalePolygon(scaleX, scaleY);

        }

        private void ScalePolygon(float scaleX, float scaleY)
        {
            if (polygons.Count > 0)
            {
                if (polygons.Last().Count == 0)
                    return;

                // Находим центр полигона
                PointF center = new PointF(0, 0);
                foreach (PointF p in polygons.Last())
                {
                    center.X += p.X;
                    center.Y += p.Y;
                }
                center.X /= polygons.Last().Count;
                center.Y /= polygons.Last().Count;

                // Матрица масштабирования
                Matrix<float> scaleMatrix = Matrix<float>.Build.DenseOfArray(new float[,]
                {
        { scaleX, 0, 0 },
        { 0, scaleY, 0 },
        { 0, 0, 1 }
                });

                // Масштабируем каждую вершину относительно центра
                for (int i = 0; i < polygons.Last().Count; i++)
                {
                    float deltaX = polygons.Last()[i].X - center.X;
                    float deltaY = polygons.Last()[i].Y - center.Y;

                    // Применяем матрицу масштабирования
                    Vector<float> vector = Vector<float>.Build.DenseOfArray(new float[] { deltaX, deltaY, 1 });
                    Vector<float> resultVector = scaleMatrix * vector;

                    polygons.Last()[i] = new PointF(center.X + resultVector[0], center.Y + resultVector[1]);
                }
                redrawPolygon();
            }
        }
        private void ScalePolygon1(float scaleX, float scaleY, PointF scaleCenter)
        {
            if (polygons.Last().Count == 0)
                return;

            // Матрица масштабирования
            Matrix<float> scaleMatrix = Matrix<float>.Build.DenseOfArray(new float[,]
            {
        { scaleX, 0, 0 },
        { 0, scaleY, 0 },
        { 0, 0, 1 }
            });

            // Масштабируем каждую вершину относительно центра масштабирования
            for (int i = 0; i < polygons.Last().Count; i++)
            {
                float deltaX = polygons.Last()[i].X - scaleCenter.X;
                float deltaY = polygons.Last()[i].Y - scaleCenter.Y;

                // Применяем матрицу масштабирования
                Vector<float> vector = Vector<float>.Build.DenseOfArray(new float[] { deltaX, deltaY, 1 });
                Vector<float> resultVector = scaleMatrix * vector;

                polygons.Last()[i] = new PointF(scaleCenter.X + resultVector[0], scaleCenter.Y + resultVector[1]);
            }

            redrawPolygon();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            float scaleX = (float)numericUpDownX.Value / 100f;
            float scaleY = (float)numericUpDownY.Value / 100f;

            if (rotationPoint != PointF.Empty)
            {
                ScalePolygon1(scaleX, scaleY, rotationPoint);
            }
        }

        private void RotateEdge90Degrees()
        {
            if (sections.Count > 0) { 
            // Находим центр ребра
            PointF edgeCenter = new PointF((sections.Last().leftP.X + sections.Last().rightP.X) / 2, (sections.Last().leftP.Y + sections.Last().rightP.Y) / 2);

            // Переносим центр ребра в начало координат
            Matrix<float> translationToOrigin = Matrix<float>.Build.DenseOfArray(new float[,]
            {
        { 1, 0, -edgeCenter.X },
        { 0, 1, -edgeCenter.Y },
        { 0, 0, 1 }
            });

            // Поворачиваем на 90 градусов
            Matrix<float> rotationMatrix = Matrix<float>.Build.DenseOfArray(new float[,]
            {
        { 0, -1, 0 },
        { 1, 0, 0 },
        { 0, 0, 1 }
            });

            // Переносим обратно в исходное положение
            Matrix<float> translationBack = Matrix<float>.Build.DenseOfArray(new float[,]
            {
        { 1, 0, edgeCenter.X },
        { 0, 1, edgeCenter.Y },
        { 0, 0, 1 }
            });

            Matrix<float> transformationMatrix = translationBack * rotationMatrix * translationToOrigin;

            // Применяем преобразование к начальной и конечной точкам ребра
            Vector<float> startVector = Vector<float>.Build.DenseOfArray(new float[] { sections.Last().leftP.X, sections.Last().leftP.Y, 1 });
            Vector<float> rotatedStartVector = transformationMatrix * startVector;
            sections.Last().leftP = new PointF(rotatedStartVector[0], rotatedStartVector[1]);

            Vector<float> endVector = Vector<float>.Build.DenseOfArray(new float[] { sections.Last().rightP.X, sections.Last().rightP.Y, 1 });
            Vector<float> rotatedEndVector = transformationMatrix * endVector;
            sections.Last().rightP = new PointF(rotatedEndVector[0], rotatedEndVector[1]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RotateEdge90Degrees();
            redrawLine();
            bmpGraphics.DrawLine(pen, sections.Last().rightP, sections.Last().leftP);
            // sections[sections.Count - 1] = new Section(prevPoint, startPoint);
            pictureBox1.Invalidate();
        }
        private void redrawLine()
        {
            bmpGraphics.Clear(Color.White);
            foreach (var poly in polygons)
            {
                bmpGraphics.DrawPolygon(pen, poly.ToArray());
            }
            if (polygonVertices.Count > 0)
            {
                bmpGraphics.DrawPolygon(pen, polygonVertices.ToArray());
            }
            foreach (PointF point in points)
            {
                bmpGraphics.DrawRectangle(pen, point.X, point.Y, 1, 1);
            }
            for (int i = 0; i < sections.Count - 1; i++)
            {
                bmpGraphics.DrawLine(pen, sections[i].leftP, sections[i].rightP);
            }

            pictureBox1.Invalidate();
        }
        private PointF GetIntersectionPoint(PointF p1, PointF p2, PointF q1, PointF q2)
        {
            float a1 = p2.Y - p1.Y;
            float b1 = p1.X - p2.X;
            float c1 = a1 * p1.X + b1 * p1.Y;

            float a2 = q2.Y - q1.Y;
            float b2 = q1.X - q2.X;
            float c2 = a2 * q1.X + b2 * q1.Y;

            float determinant = a1 * b2 - a2 * b1;

            if (Math.Abs(determinant) < 1e-9)
            {
                // Линии параллельны, нет точки пересечения
                return PointF.Empty;
            }

            float x = (b2 * c1 - b1 * c2) / determinant;
            float y = (a1 * c2 - a2 * c1) / determinant;

            // Проверка, что точка пересечения находится на обоих отрезках
            if (IsPointOnSegment(p1, p2, new PointF(x, y)) && IsPointOnSegment(q1, q2, new PointF(x, y)))
            {
                return new PointF(x, y);
            }

            return PointF.Empty;
        }

        private bool IsPointOnSegment(PointF p1, PointF p2, PointF testPoint)
        {
            float minX = Math.Min(p1.X, p2.X);
            float maxX = Math.Max(p1.X, p2.X);
            float minY = Math.Min(p1.Y, p2.Y);
            float maxY = Math.Max(p1.Y, p2.Y);

            return testPoint.X >= minX && testPoint.X <= maxX && testPoint.Y >= minY && testPoint.Y <= maxY;
        }

        private void DrawIntersectingPoint()
        {
            foreach (var section in sections)
            {
                foreach (var otherSection in sections)
                {
                    if (section != otherSection)
                    {
                        PointF intersectionPoint = GetIntersectionPoint(section.leftP, section.rightP, otherSection.leftP, otherSection.rightP);

                        if (!intersectionPoint.IsEmpty)
                        {
                            // Выделить точку пересечения
                            bmpGraphics.FillEllipse(Brushes.Red, intersectionPoint.X - 3, intersectionPoint.Y - 3, 6, 6);
                        }
                    }
                }
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            // Очищаем изображение
            bmpGraphics.Clear(Color.White);

            // Перерисовываем все объекты
            foreach (var poly in polygons)
            {
                bmpGraphics.DrawPolygon(pen, poly.ToArray());
            }

            foreach (var point in points)
            {
                bmpGraphics.DrawRectangle(pen, point.X, point.Y, 1, 1);
            }

            foreach (var section in sections)
            {
                bmpGraphics.DrawLine(pen, section.leftP, section.rightP);
            }

            if (polygonVertices.Count > 0)
            {
                bmpGraphics.DrawPolygon(pen, polygonVertices.ToArray());
            }

            // Рисуем точки пересечения
            DrawIntersectingPoint();

            pictureBox1.Invalidate();
        }
        private bool IsPointInsidePolygon(PointF point, List<PointF> polygon)
        {
            int numVertices = polygon.Count;
            int j = numVertices - 1;
            bool isInside = false;

            for (int i = 0; i < numVertices; i++)
            {
                if ((polygon[i].Y < point.Y && polygon[j].Y >= point.Y || polygon[j].Y < point.Y && polygon[i].Y >= point.Y)
                    && (polygon[i].X + (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < point.X))
                {
                    isInside = !isInside;
                }
                j = i;
            }

            return isInside;
        }

        private bool CheckPolygon(PointF point, List<PointF> polygon)
        {
            bool isInside = true;

            for (int i = 0; i < polygon.Count; i++)
            {
                int j = (i + 1) % polygon.Count;

                // Вычислим вектора от вершины i до точки и до следующей вершины j
                PointF edge = new PointF(polygon[j].X - polygon[i].X, polygon[j].Y - polygon[i].Y);
                PointF toPoint = new PointF(point.X - polygon[i].X, point.Y - polygon[i].Y);

                // Векторное произведение между векторами edge и toPoint
                float crossProduct = edge.X * toPoint.Y - edge.Y * toPoint.X;

                if (crossProduct < 0)
                {
                    isInside = false;
                    break;
                }
            }

            return isInside;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            bool isInsideAnyPolygon = false;
            bool convex = false;
            foreach (List<PointF> polygon in polygons)
            {
                isInsideAnyPolygon = IsPointInsidePolygon(startPoint, polygon);
                if (isInsideAnyPolygon)
                {
                    convex = CheckPolygon(startPoint, polygon);
                    break;
                }
            }

            if (isInsideAnyPolygon)
            {
                if (convex)
                {
                    label7.Text = "Да";
                }
                else
                {
                    label7.Text = "Нет";
                }
            }
            else { label7.Text = "Вне"; }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            bool isInsideAnyPolygon = false;
            bool convex = true;
            foreach (List<PointF> polygon in polygons)
            {
                isInsideAnyPolygon = IsPointInsidePolygon(startPoint, polygon);
                if (isInsideAnyPolygon)
                {
                    convex = CheckPolygon(startPoint, polygon);
                    break;
                }
            }

            if (isInsideAnyPolygon)
            {
                if (!convex)
                {
                    label8.Text = "Да";
                }
                else
                {
                    label8.Text = "Нет";
                }
            }
            else { label8.Text = "Вне"; }
        }
        private bool ClassifyPointToEdge(PointF point, Section edge)
        {
            float xa = edge.leftP.X;
            float ya = edge.leftP.Y;
            float xb = edge.rightP.X;
            float yb = edge.rightP.Y;

            float sinTheta = (xb - xa) * (point.Y - ya) - (yb - ya) * (point.X - xa);

            if (Math.Abs(sinTheta) < float.Epsilon)
            {
                // Тут на ребро упала
                label9.Text = "На ребре";
            }
            else if (sinTheta > 0)
            {
                // Слева
                label9.Text = "Слева";
            }
            else
            {
                // Справа
                label9.Text = "Справа";
            }
            return true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (sections.Count > 0)
            {
                bool r = ClassifyPointToEdge(startPoint, sections.Last());

            }

        }
    }



}