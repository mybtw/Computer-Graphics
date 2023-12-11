using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace lab6
{
    class Camera
    {
        public Point Position { get; set; } = new Point(0, 0, 0);
        //public Point Focus { get; set; } = new Point(0, 0, 0);
        public PointF Offset { get; set; } = new PointF(0, 0);
        public double AngleX { get; set; } = 0;
        public double AngleY { get; set; } = 0;
        public double AngleZ { get; set; } = 0;


        public void MoveCamera(double dx, double dy, double dz)
        {
            Position = new Point(dx, dy, dz);
            //Focus = new Point(dx, dy, 1000);
        }

        public void RotateCamera(double ax, double ay, double az)
        {
            AngleX = ax;
            AngleY = ay;
            AngleZ = az;

            if (AngleX > 360) AngleX -= 360;
            else if (AngleX < 0) AngleX += 360;

            if (AngleY > 360) AngleY -= 360;
            else if (AngleY < 0) AngleY += 360;

            if (AngleZ > 360) AngleZ -= 360;
            else if (AngleZ < 0) AngleZ += 360;

            // При поворотах обновим координаты Focus
            double radianX = AngleX * Math.PI / 180.0;
            double radianY = AngleY * Math.PI / 180.0;
            double radianZ = AngleZ * Math.PI / 180.0;

            // Здесь можно использовать уже обновленные углы для вычисления новых координат Focus
            //Focus.X = Position.X + (int)(1000 * Math.Sin(radianX) * Math.Cos(radianY));
            //Focus.Y = Position.Y + (int)(1000 * Math.Sin(radianX) * Math.Sin(radianY));
            //Focus.Z = Position.Z + (int)(1000 * Math.Cos(radianX));
        }
        public double[,] GetViewMatrix()
        {
            double[,] translationMatrix = new double[4, 4]
            {
        {1, 0, 0, 0},
        {0, 1, 0, 0},
        {0, 0, 1, 0},
        {-Position.X, -Position.Y, -Position.Z, 1}
            };

            double cosX = Math.Cos(AngleX * Math.PI / 180);
            double sinX = Math.Sin(AngleX * Math.PI / 180);

            double cosY = Math.Cos(AngleY * Math.PI / 180);
            double sinY = Math.Sin(AngleY * Math.PI / 180);

            double cosZ = Math.Cos(AngleZ * Math.PI / 180);
            double sinZ = Math.Sin(AngleZ * Math.PI / 180);

            double[,] rotationMatrixX = new double[4, 4]
            {
        {1, 0, 0, 0},
        {0, cosX, -sinX, 0},
        {0, sinX, cosX, 0},
        {0, 0, 0, 1}
            };

            double[,] rotationMatrixY = new double[4, 4]
            {
        {cosY, 0, sinY, 0},
        {0, 1, 0, 0},
        {-sinY, 0, cosY, 0},
        {0, 0, 0, 1}
            };

            double[,] rotationMatrixZ = new double[4, 4]
            {
        {cosZ, -sinZ, 0, 0},
        {sinZ, cosZ, 0, 0},
        {0, 0, 1, 0},
        {0, 0, 0, 1}
            };

            double[,] viewMatrix = MatrixMultiply(rotationMatrixX, rotationMatrixY);
            viewMatrix = MatrixMultiply(viewMatrix, rotationMatrixZ);
            viewMatrix = MatrixMultiply(viewMatrix, translationMatrix);

            return viewMatrix;
        }

        private double[,] MatrixMultiply(double[,] matrix1, double[,] matrix2)
        {
            int rows1 = matrix1.GetLength(0);
            int cols1 = matrix1.GetLength(1);
            int rows2 = matrix2.GetLength(0);
            int cols2 = matrix2.GetLength(1);

            double[,] result = new double[rows1, cols2];

            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < cols2; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < cols1; k++)
                    {
                        result[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }

            return result;
        }

    }
    public enum Projection { PERSPECTIVE, AXONOMETRIC }
    [Serializable]

    class Point
    {
        double x, y, z;
        public static Projection projection = Projection.AXONOMETRIC;
        public static PointF worldCenter;
        const double k = 0.001f;
        Matrix<double> centralProjectionMatrix = Matrix<double>.Build.DenseOfArray(new double[,]
        {
                {1, 0, 0, 0 },
                {0, 1, 0, 0 },
                { 0, 0, 0, -k},
                { 0, 0, 0, 1}
        });
        // вид аксонометрической проекции (вид будто пользователь смотрит в 1 октант)
        /* Matrix<double> isometricProjectionMatrix = Matrix<double>.Build.DenseOfArray(new double[,]
         {
                 {Math.Sqrt(3), 0, -Math.Sqrt(3)},
                 {1, 2, 1},
                 { Math.Sqrt(2), -Math.Sqrt(2), Math.Sqrt(2)},
         }).Multiply(1 / Math.Sqrt(6));*/

        Matrix<double> isometricProjectionMatrix = Matrix<double>.Build.DenseOfArray(new double[,]
{
                {1, 0, 0, 0 },
                {0, 1, 0, 0 },
                { 0, 0, 0, 0},
                { 0, 0, 0, 1}
});

        Matrix<double> utilMatr = Matrix<double>.Build.DenseOfArray(new double[,]
        {
                    {1, 0, 0},
                    {0, 1, 0},
                    {0, 0, 0}
        });

        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }


        // X' = X * d / (d + Z) и Y' = Y * d / (d + Z) - перспективная
        public PointF project()
        {
            // только центральная проекция
            if (projection == Projection.PERSPECTIVE)
            {
                Matrix<double> oldCoords = Matrix<double>.Build.DenseOfArray(new double[,] { { X, Y, Z, 1 } });
                Matrix<double> res = (oldCoords * centralProjectionMatrix).Multiply(1 / (k * Z + 1));
                return new PointF(worldCenter.X + (float)res[0, 0], worldCenter.Y + (float)res[0, 1]);
            }
            else
            {
                /*   Matrix<double> oldCoords = Matrix<double>.Build.DenseOfArray(new double[,] { { Xf }, { Yf }, { Zf } });
                   Matrix<double> res = utilMatr * isometricProjectionMatrix * oldCoords;
                   return new PointF(worldCenter.X + 150 + (float)res[0,0], worldCenter.Y + (float)res[1,0]);*/
                // return new PointF(worldCenter.X + 150 + (float)Xf, worldCenter.Y + (float)Yf);

                Matrix<double> oldCoords = Matrix<double>.Build.DenseOfArray(new double[,] { { X, Y, Z, 1 } });
                Matrix<double> res = (oldCoords * isometricProjectionMatrix);
                return new PointF(worldCenter.X + (float)res[0, 0], worldCenter.Y + (float)res[0, 1]);
            }
        }
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Point operator *(Point a, int x)
        {
            return new Point(a.X * x, a.Y * x, a.Z * x);
        }
    }

    class Funcs {
        public static double degreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static Matrix<double> getRotationMatrix(double startAngle, string axis)
        {
            double angle = Funcs.degreesToRadians(startAngle);
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);
            if (axis.Equals("X"))
            {
                Matrix<double> matrix = Matrix<double>.Build.DenseOfArray(new double[,] {
                { 1, 0, 0 },
                { 0, cos, -sin },
                { 0, sin, cos}
                });
                return matrix;
            }
            else if (axis.Equals("Y"))
            {
                Matrix<double> matrix = Matrix<double>.Build.DenseOfArray(new double[,] {
                { cos, 0, sin },
                { 0, 1, 0 },
                { -sin, 0, cos}
                });
                return matrix;

            }
            else
            {
                Matrix<double> matrix = Matrix<double>.Build.DenseOfArray(new double[,] {
                { cos, -sin, 0 },
                { sin, cos, 0 },
                { 0, 0, 1}
                });
                return matrix;
            }
        }
    }
    [Serializable]
    class Line
    {
        public Point start { get; set; }
        public Point end { get; set; }
        public Line(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }
        
    }
    class Vector
    {
        public Vector()
        {
        }

        double X { get; set;}
        double Y { get; set; }
        double Z { get; set; }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector(Point p)
        {
            X = p.X;
            Y = p.Y;
            Z = p.Z;
        }
        public Vector(PointF p)
        {
            X = p.X;
            Y = p.Y;
            Z = 0;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v2.X - v1.X, v2.Y - v1.Y, v2.Z - v1.Z);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator *(Vector a, Vector b)
        {
            return new Vector(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public static Vector operator *(double k, Vector b)
        {
            return new Vector(k * b.X, k * b.Y, k * b.Z);
        }
        public static double scalar(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
        public Vector normalize()
        {
            double normalization = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
            X = X / normalization;
            Y = Y / normalization;
            Z = Z / normalization;
            return this;
        }
    }

    [Serializable]
    class Face
    {

        // взял 3 точки
        // перемножил их в CCW, получил вектор нормали
        List<Line> edges;
        public Vector normal { get; set; }


        public Face()
        {
            edges = new List<Line>();
            normal = new Vector(0, 0, 0);
        }

        public Face(IEnumerable<Line> edges) : this()
        {
            this.edges.AddRange(edges);
        }

        public Face addEdge(Line edge)
        {
            edges.Add(edge);
            return this;
        }
        public Face addEdges(IEnumerable<Line> edges)
        {
            this.edges.AddRange(edges);
            return this;
        }

        public List<Line> Edges { get => edges; }

        public Point getCenter()
        {
            double x = 0, y = 0, z = 0;
            foreach (var line in edges)
            {
                x += line.start.X;
                y += line.start.Y;
                z += line.start.Z;
            }
            return new Point(x / edges.Count, y / edges.Count, z / edges.Count);
        }

        public void setNormal()
        {
            Line first = edges[0];
            Line last = edges[edges.Count - 1];
            Vector start = new Vector(first.start);
            Vector a = start - new Vector(first.end);
            Vector b = start - new Vector(last.start);
            this.normal = a * b;
        }
    }



    [Serializable]
    class Shape
    {
        private List<Point> points = new List<Point>();
        private List<Line> edges = new List<Line>();

        public void addPoint(Point point)
        {
            points.Add(point);
        }

        public Point getLastPoint()
        {
            if (points.Count > 0)
                return points[points.Count - 1];
            else
                throw new InvalidOperationException("The shape does not contain any points.");
        }

        public void addEdge(Line edge)
        {
            edges.Add(edge);
        }
        List<Face> faces;

        public Shape()
        {
            faces = new List<Face>();
        }

        public Shape addFace(Face face)
        {
            faces.Add(face);
            return this;
        }

        public void calcNormals()
        {
            foreach (Face f in faces)
            {
                f.setNormal();
            }
        }
        public double GetAverageZ()
        {
            if (faces == null || faces.Count == 0)
            {
                return 0.0; // или другое значение по умолчанию
            }

            double sumZ = 0.0;
            int vertexCount = 0;

            foreach (var face in faces)
            {
                foreach (var line in face.Edges)
                {
                    sumZ += line.start.Z;
                    sumZ += line.end.Z;
                    vertexCount += 2; // Учитываем две вершины для каждой линии
                }
            }

            return sumZ / vertexCount;
        }


        public List<Face> Faces { get => faces; }

    }
    [Serializable]
    // тетраэдр
    class Tetrahedron : Shape
    {
        public static Tetrahedron getTetrahedron()
        {
            Tetrahedron res = new Tetrahedron();
            Point a = new Point(0, 0, 0);
            Point c = new Point(150, 0, 150);
            Point f = new Point(150, 150, 0);
            Point h = new Point(0, 150, 150);
            res.addFace(new Face().addEdge(new Line(a, f)).addEdge(new Line(f, c)).addEdge(new Line(c, a)));
            res.addFace(new Face().addEdge(new Line(f, h)).addEdge(new Line(h, c)).addEdge(new Line(c, f)));
            res.addFace(new Face().addEdge(new Line(c, h)).addEdge(new Line(h, a)).addEdge(new Line(a, c)));
            res.addFace(new Face().addEdge(new Line(f, a)).addEdge(new Line(a, h)).addEdge(new Line(h, f)));
            return res;
        }
    }
    [Serializable]
    class Octahedron : Shape
    {
        public static Octahedron getOctahedron()
        {
            Octahedron res = new Octahedron();
            var cube = Hexahedron.getHexahedron();
            Point a = cube.Faces[0].getCenter();
            Point b = cube.Faces[1].getCenter();
            Point c = cube.Faces[2].getCenter();
            Point d = cube.Faces[3].getCenter();
            Point e = cube.Faces[4].getCenter();
            Point f = cube.Faces[5].getCenter();

            res.addFace(new Face().addEdge(new Line(a, b)).addEdge(new Line(b, f)).addEdge(new Line(f, a)));
            res.addFace(new Face().addEdge(new Line(b, c)).addEdge(new Line(c, f)).addEdge(new Line(f, b)));
            res.addFace(new Face().addEdge(new Line(c, d)).addEdge(new Line(d, f)).addEdge(new Line(f, c)));
            res.addFace(new Face().addEdge(new Line(d, a)).addEdge(new Line(a, f)).addEdge(new Line(f, d)));
            res.addFace(new Face().addEdge(new Line(a, e)).addEdge(new Line(e, b)).addEdge(new Line(b, a)));
            res.addFace(new Face().addEdge(new Line(b, e)).addEdge(new Line(e, c)).addEdge(new Line(c, b)));
            res.addFace(new Face().addEdge(new Line(c, e)).addEdge(new Line(e, d)).addEdge(new Line(d, c)));
            res.addFace(new Face().addEdge(new Line(d, e)).addEdge(new Line(e, a)).addEdge(new Line(a, d)));
            return res;
        }

    }

    // куб
    [Serializable]
    class Hexahedron : Shape
    {
        public static Hexahedron getHexahedron()
        {
            Hexahedron res = new Hexahedron();
            Point a = new Point(0, 0, 0);
            Point b = new Point(150, 0, 0);
            Point c = new Point(150, 0, 150);
            Point d = new Point(0, 0, 150);
            Point e = new Point(0, 150, 0);
            Point f = new Point(150, 150, 0);
            Point g = new Point(150, 150, 150);
            Point h = new Point(0, 150, 150);
            res.addFace(new Face().addEdge(new Line(a, b)).addEdge(new Line(b, c)).addEdge(new Line(c, d)).addEdge(new Line(d, a)));
            res.addFace(new Face().addEdge(new Line(b, f)).addEdge(new Line(f, g)).addEdge(new Line(g, c)).addEdge(new Line(c, b)));
            res.addFace(new Face().addEdge(new Line(f, e)).addEdge(new Line(e, h)).addEdge(new Line(h, g)).addEdge(new Line(g, f)));
            res.addFace(new Face().addEdge(new Line(h, e)).addEdge(new Line(e, a)).addEdge(new Line(a, d)).addEdge(new Line(d, h)));
            res.addFace(new Face().addEdge(new Line(a, e)).addEdge(new Line(e, f)).addEdge(new Line(f, b)).addEdge(new Line(b, a)));
            res.addFace(new Face().addEdge(new Line(d, c)).addEdge(new Line(c, g)).addEdge(new Line(g, h)).addEdge(new Line(h, d)));
            return res;
        }

    }
    [Serializable]
    class Icosahedron : Shape
    {
        public static Icosahedron getIcosahedron()
        {
            Icosahedron res = new Icosahedron();
            /* Point circleCenter = new Point(100, 100, 100);
             List<Point> circlePoints = new List<Point>();
             for (int angle = 0; angle < 360; angle += 36)
             {
                 if (angle % 72 == 0)
                 {
                     circlePoints.Add(new Point(circleCenter.X + (100 * Math.Cos(Funcs.degreesToRadians(angle))), circleCenter.Y + 100, circleCenter.Z + (100 * Math.Sin(Funcs.degreesToRadians(angle)))));
                     continue;
                 }
                 circlePoints.Add(new Point(circleCenter.X + (100 * Math.Cos(Funcs.degreesToRadians(angle))), circleCenter.Y, circleCenter.Z + (100 * Math.Sin(Funcs.degreesToRadians(angle)))));
             }
             Point a = new Point(100, 50, 100);
             Point b = new Point(100, 250, 100);
             for (int i = 0; i < 10; i++)
             {
                 res.addFace(new Face().addEdge(new Line(circlePoints[i], circlePoints[(i + 1) % 10])).addEdge(new Line(circlePoints[(i + 1) % 10], circlePoints[(i + 2) % 10])).addEdge(new Line(circlePoints[(i + 2) % 10], circlePoints[i])));
             }
             res.addFace(new Face().addEdge(new Line(circlePoints[1], a)).addEdge(new Line(a, circlePoints[3])).addEdge(new Line(circlePoints[3], circlePoints[1])));
             res.addFace(new Face().addEdge(new Line(circlePoints[3], a)).addEdge(new Line(a, circlePoints[5])).addEdge(new Line(circlePoints[5], circlePoints[3])));
             res.addFace(new Face().addEdge(new Line(circlePoints[5], a)).addEdge(new Line(a, circlePoints[7])).addEdge(new Line(circlePoints[7], circlePoints[5])));
             res.addFace(new Face().addEdge(new Line(circlePoints[7], a)).addEdge(new Line(a, circlePoints[9])).addEdge(new Line(circlePoints[9], circlePoints[7])));
             res.addFace(new Face().addEdge(new Line(circlePoints[9], a)).addEdge(new Line(a, circlePoints[1])).addEdge(new Line(circlePoints[1], circlePoints[9])));

             res.addFace(new Face().addEdge(new Line(circlePoints[0], b)).addEdge(new Line(b, circlePoints[2])).addEdge(new Line(circlePoints[2], circlePoints[0])));
             res.addFace(new Face().addEdge(new Line(circlePoints[2], b)).addEdge(new Line(b, circlePoints[4])).addEdge(new Line(circlePoints[4], circlePoints[2])));
             res.addFace(new Face().addEdge(new Line(circlePoints[4], b)).addEdge(new Line(b, circlePoints[6])).addEdge(new Line(circlePoints[6], circlePoints[4])));
             res.addFace(new Face().addEdge(new Line(circlePoints[6], b)).addEdge(new Line(b, circlePoints[8])).addEdge(new Line(circlePoints[8], circlePoints[6])));
             res.addFace(new Face().addEdge(new Line(circlePoints[8], b)).addEdge(new Line(b, circlePoints[0])).addEdge(new Line(circlePoints[0], circlePoints[8])));*/

            Point A = new Point(140, 74, 0);
            Point A1 = new Point(140, -74, 0);
            Point A2 = new Point(-140, -74, 0);
            Point A3 = new Point(-140, 74, 0);

            Point B = new Point(74, 0, 140);
            Point B1 = new Point(-74, 0, 140);
            Point B2 = new Point(-74, 0, -140);
            Point B3 = new Point(74, 0, -140);

            Point C = new Point(0, 140, 74);
            Point C1 = new Point(0, -140, 74);
            Point C2 = new Point(0, -140, -74);
            Point C3 = new Point(0, 140, -74);



            // шапка
            // a2 b1 a3
            res.addFace(new Face().addEdge(new Line(A2, B1)).addEdge(new Line(B1, A3)).addEdge(new Line(A3, A2)));
            // c1 b1 a2
            res.addFace(new Face().addEdge(new Line(C1, B1)).addEdge(new Line(B1, A2)).addEdge(new Line(A2, C1)));
            // b b1 c1
            res.addFace(new Face().addEdge(new Line(B, B1)).addEdge(new Line(B1, C1)).addEdge(new Line(C1, B)));
            // c b1 b
            res.addFace(new Face().addEdge(new Line(C, B1)).addEdge(new Line(B1, B)).addEdge(new Line(B, C)));
            // a3 b1 c
            res.addFace(new Face().addEdge(new Line(A3, B1)).addEdge(new Line(B1, C)).addEdge(new Line(C, A3)));

            //барабан

            // a c b
            res.addFace(new Face().addEdge(new Line(A, C)).addEdge(new Line(C, B)).addEdge(new Line(B, A)));
            // c3 c a
            res.addFace(new Face().addEdge(new Line(C3, C)).addEdge(new Line(C, A)).addEdge(new Line(A, C3)));
            //c3 a3 c
            res.addFace(new Face().addEdge(new Line(C3, A3)).addEdge(new Line(A3, C)).addEdge(new Line(C, C3)));
            //b2 a3 c3
            res.addFace(new Face().addEdge(new Line(B2, A3)).addEdge(new Line(A3, C3)).addEdge(new Line(C3, B2)));
            //b2 a2 a3
            res.addFace(new Face().addEdge(new Line(B2, A2)).addEdge(new Line(A2, A3)).addEdge(new Line(A3, B2)));
            //c2 a2 b2
            res.addFace(new Face().addEdge(new Line(C2, A2)).addEdge(new Line(A2, B2)).addEdge(new Line(B2, C2)));
            //c2 c1 a2
            res.addFace(new Face().addEdge(new Line(C2, C1)).addEdge(new Line(C1, A2)).addEdge(new Line(A2, C2)));
            //a1 c1 c2
            res.addFace(new Face().addEdge(new Line(A1, C1)).addEdge(new Line(C1, C2)).addEdge(new Line(C2, A1)));
            //a1 b c1
            res.addFace(new Face().addEdge(new Line(A1, B)).addEdge(new Line(B, C1)).addEdge(new Line(C1, A1)));
            // a b a1
            res.addFace(new Face().addEdge(new Line(A, B)).addEdge(new Line(B, A1)).addEdge(new Line(A1, A)));

            // шапка
            //с3 b3 b2
            res.addFace(new Face().addEdge(new Line(C3, B3)).addEdge(new Line(B3, B2)).addEdge(new Line(B2, C3)));
            //a b3 c3
            res.addFace(new Face().addEdge(new Line(A, B3)).addEdge(new Line(B3, C3)).addEdge(new Line(C3, A)));
            //a1 b3 a
            res.addFace(new Face().addEdge(new Line(A1, B3)).addEdge(new Line(B3, A)).addEdge(new Line(A, A1)));
            //c2 b3 a1
            res.addFace(new Face().addEdge(new Line(C2, B3)).addEdge(new Line(B3, A1)).addEdge(new Line(A1, C2)));
            //b2 b3 c2
            res.addFace(new Face().addEdge(new Line(B2, B3)).addEdge(new Line(B3, C2)).addEdge(new Line(C2, B2)));
            return res;
        }

    }
    [Serializable]
    class Dodecahedron : Shape // 12 граней
    {
        public static Dodecahedron getDodecahedron() // 12 граней
        {
            Dodecahedron res = new Dodecahedron();

            Point A = new Point((1 / (Math.Sqrt(5) - 1)), (1 / (Math.Sqrt(5) - 1)), (1 / (Math.Sqrt(5) - 1))) * 100;
            Point B = new Point((1 / (Math.Sqrt(5) - 1)), (1 / (Math.Sqrt(5) - 1)), (-1 / (Math.Sqrt(5) - 1))) * 100;
            Point C = new Point(((1) / (Math.Sqrt(5) - 1)) * 100, ((-1) / (Math.Sqrt(5) - 1)) * 100, ((1) / (Math.Sqrt(5) - 1)) * 100);
            Point D = new Point(((1) / (Math.Sqrt(5) - 1)) * 100, ((-1) / (Math.Sqrt(5) - 1)) * 100, ((-1) / (Math.Sqrt(5) - 1)) * 100);
            Point E = new Point(((-1) / (Math.Sqrt(5) - 1)) * 100, ((1) / (Math.Sqrt(5) - 1)) * 100, ((1) / (Math.Sqrt(5) - 1)) * 100);
            Point F = new Point(((-1) / (Math.Sqrt(5) - 1)), ((1) / (Math.Sqrt(5) - 1)), ((-1) / (Math.Sqrt(5) - 1))) * 100;
            Point G = new Point(((-1) / (Math.Sqrt(5) - 1)), ((-1) / (Math.Sqrt(5) - 1)), ((1) / (Math.Sqrt(5) - 1))) * 100;
            Point H = new Point(((-1) / (Math.Sqrt(5) - 1)), ((-1) / (Math.Sqrt(5) - 1)), ((-1) / (Math.Sqrt(5) - 1))) * 100;
            Point I = new Point(0, ((((1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1)), ((((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1))) * 100;
            Point J = new Point(0, ((((1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1)), ((-((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1))) * 100;
            Point K = new Point(0, ((((-1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1)), ((-((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1))) * 100;
            Point L = new Point(0, ((((-1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1)), ((((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1))) * 100;
            Point M = new Point(((((1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1)), ((((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1)), 0) * 100;
            Point N = new Point(((((-1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1)), ((((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1)), 0) * 100;
            Point O = new Point(((((1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1)), -((((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1)), 0) * 100;
            Point P = new Point(((((-1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1)), -((((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1)), 0) * 100;
            Point Q = new Point(((((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1)), 0, ((((1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1))) * 100;
            Point R = new Point(((-((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1)), 0, ((((1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1))) * 100;
            Point S = new Point(((((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1)), 0, ((((-1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1))) * 100;
            Point T = new Point(-((((Math.Sqrt(5) + 1) / (2))) / (Math.Sqrt(5) - 1)), 0, ((((-1) / (((Math.Sqrt(5) + 1) / (2))))) / (Math.Sqrt(5) - 1))) * 100;


            //Polygon(S,Q,A,M,B)
            // B M A Q S
            res.addFace(new Face().addEdge(new Line(B, M)).addEdge(new Line(M, A)).addEdge(new Line(A, Q)).addEdge(new Line(Q, S)).addEdge(new Line(S, B)));
            // Polygon(D, S, Q, C, O)
            // D S Q C O
            res.addFace(new Face().addEdge(new Line(D, S)).addEdge(new Line(S, Q)).addEdge(new Line(Q, C)).addEdge(new Line(C, O)).addEdge(new Line(O, D)));
            //Polygon(D,K,J,B,S)
            //J B S D K
            res.addFace(new Face().addEdge(new Line(J, B)).addEdge(new Line(B, S)).addEdge(new Line(S, D)).addEdge(new Line(D, K)).addEdge(new Line(K, J)));
            //Polygon(B,J,F,N,M)
            // F N M B J
            res.addFace(new Face().addEdge(new Line(F, N)).addEdge(new Line(N, M)).addEdge(new Line(M, B)).addEdge(new Line(B, J)).addEdge(new Line(J, F)));
            //Polygon(A, I, E, N, M)
            //N E I A M
            res.addFace(new Face().addEdge(new Line(N, E)).addEdge(new Line(E, I)).addEdge(new Line(I, A)).addEdge(new Line(A, M)).addEdge(new Line(M, N)));
            //Polygon(C,Q,A,I,L)
            // A I L C Q
            res.addFace(new Face().addEdge(new Line(A, I)).addEdge(new Line(I, L)).addEdge(new Line(L, C)).addEdge(new Line(C, Q)).addEdge(new Line(Q, A)));
            //Polygon(E,R,G,L,I)
            // E R G L I
            res.addFace(new Face().addEdge(new Line(E, R)).addEdge(new Line(R, G)).addEdge(new Line(G, L)).addEdge(new Line(L, I)).addEdge(new Line(I, E)));
            //Polygon(R,T,F,N,E)
            // T R E N F
            res.addFace(new Face().addEdge(new Line(T, R)).addEdge(new Line(R, E)).addEdge(new Line(E, N)).addEdge(new Line(N, F)).addEdge(new Line(F, T)));
            //Polygon(T,H,P,G,R)
            //H P G R T
            res.addFace(new Face().addEdge(new Line(H, P)).addEdge(new Line(P, G)).addEdge(new Line(G, R)).addEdge(new Line(R, T)).addEdge(new Line(T, H)));
            //Polygon(G,P,O,C,L)
            // O C L G P
            res.addFace(new Face().addEdge(new Line(O, C)).addEdge(new Line(C, L)).addEdge(new Line(L, G)).addEdge(new Line(G, P)).addEdge(new Line(P, O)));
            //Polygon(T,F,J,K,H)
            // T F J K H
            res.addFace(new Face().addEdge(new Line(T, F)).addEdge(new Line(F, J)).addEdge(new Line(J, K)).addEdge(new Line(K, H)).addEdge(new Line(H, T)));
            //Polygon(H, K, D, O, P)
            // H K D O P
            res.addFace(new Face().addEdge(new Line(H, K)).addEdge(new Line(K, D)).addEdge(new Line(D, O)).addEdge(new Line(O, P)).addEdge(new Line(P, H)));
            return res;
        }

    }


    class RotationShape : Shape
    {
        List<Point> formingline;
        Line axiz;
        int Divisions;
        List<Point> allpoints;
        List<Line> edges;//ребра
        public RotationShape()
        {
            allpoints = new List<Point>();
            edges = new List<Line>();
        }
        public List<Line> Edges { get => edges; }
        public Shape addEdge(Line edge)
        {
            edges.Add(edge);
            return this;
        }
        public Shape addEdges(IEnumerable<Line> ed)
        {
            this.edges.AddRange(ed);
            return this;
        }
        public RotationShape(IEnumerable<Point> points) : this()
        {
            this.allpoints.AddRange(points);
        }
        public RotationShape(Line ax, int Div, IEnumerable<Point> line) : this()
        {
            this.axiz = ax;
            this.Divisions = Div;
            this.formingline.AddRange(line);
        }
        public RotationShape addPoint(Point p)
        {
            allpoints.Add(p);
            return this;
        }
        public RotationShape addPoints(IEnumerable<Point> points)
        {
            this.allpoints.AddRange(points);
            return this;
        }
        public List<Point> Points { get => allpoints; }


        public static RotationShape getRotationShape(int divs, string axis, List<PointF> points, int z)
        {
            var rotMatrix = Funcs.getRotationMatrix(360 / divs, axis);
            List<Point> initialPoints = points.Select(point => new Point(point.X, point.Y, z)).ToList();
            RotationShape figure = new RotationShape();
            List<Point> nextPoints = initialPoints;
            for (int i = 0; i < divs - 1; i++)
            {
                List<Point> futurePoints = new List<Point>();
                foreach (Point point in nextPoints)
                {
                    Matrix<double> oldPoint = Matrix<double>.Build.DenseOfArray(new double[,] { { point.X }, { point.Y }, { point.Z } });
                    Matrix<double> resPoint = rotMatrix * oldPoint;
                    futurePoints.Add(new Point(resPoint[0, 0], resPoint[1, 0], resPoint[2, 0]));
                }
                List<Point> prevPair = new List<Point> { nextPoints[0], futurePoints[0] };

                // формируем грани на основе 2 наборов точек фигур вращения
                for (int j = 0; j < initialPoints.Count; j++)
                {
                    Face f1 = new Face();
                    f1.addEdge(new Line(prevPair[0], prevPair[1]));
                    f1.addEdge(new Line(nextPoints[j], futurePoints[j]));
                    f1.addEdge(new Line(prevPair[0], nextPoints[j]));
                    f1.addEdge(new Line(prevPair[1], futurePoints[j]));
                    figure.addFace(f1);
                    prevPair[0] = nextPoints[j];
                    prevPair[1] = futurePoints[j];
                }
                Face f = new Face();
                f.addEdge(new Line(nextPoints[0], futurePoints[0]));
                f.addEdge(new Line(nextPoints[0], nextPoints[nextPoints.Count - 1]));
                f.addEdge(new Line(nextPoints[nextPoints.Count - 1], futurePoints[futurePoints.Count - 1]));
                f.addEdge(new Line(futurePoints[0], futurePoints[futurePoints.Count - 1]));
                figure.addFace(f);
                nextPoints = futurePoints;
            }

            List<Point> prevP = new List<Point> { initialPoints[0], nextPoints[0] };
            for (int j = 1; j < initialPoints.Count; j++)
            {
                Face f = new Face();
                f.addEdge(new Line(prevP[0], prevP[1]));
                f.addEdge(new Line(initialPoints[j], nextPoints[j]));
                f.addEdge(new Line(prevP[0], initialPoints[j]));
                f.addEdge(new Line(prevP[1], nextPoints[j]));
                figure.addFace(f);
                prevP[0] = initialPoints[j];
                prevP[1] = nextPoints[j];
            }

            Face ff = new Face();
            ff.addEdge(new Line(initialPoints[0], nextPoints[0]));
            ff.addEdge(new Line(initialPoints[0], initialPoints[initialPoints.Count - 1]));
            ff.addEdge(new Line(initialPoints[initialPoints.Count - 1], nextPoints[nextPoints.Count - 1]));
            ff.addEdge(new Line(nextPoints[0], nextPoints[nextPoints.Count - 1]));
            figure.addFace(ff);

            return figure;
        }
    }
}
