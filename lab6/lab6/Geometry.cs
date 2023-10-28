using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace lab6
{
    public enum Projection { PERSPECTIVE, AXONOMETRIC }
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
        Matrix<double> isometricProjectionMatrix = Matrix<double>.Build.DenseOfArray(new double[,]
        {
                {Math.Sqrt(3), 0, -Math.Sqrt(3)},
                {1, 2, 1},
                { Math.Sqrt(2), -Math.Sqrt(2), Math.Sqrt(2)},
        }).Multiply(1 / Math.Sqrt(6));

        Matrix<double> utilMatr = Matrix<double>.Build.DenseOfArray(new double[,]
        {
                    {1, 0, 0},
                    {0, 1, 0},
                    {0, 0, 0}
        });

        public Point(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        public int X { get => (int)x; set => x = value; }
        public int Y { get => (int)y; set => y = value; }
        public int Z { get => (int)z; set => z = value; }

        public double Xf { get => x; set => x = value; }
        public double Yf { get => y; set => y = value; }
        public double Zf { get => z; set => z = value; }


        // X' = X * d / (d + Z) и Y' = Y * d / (d + Z) - перспективная
        public PointF project()
        {
            // только центральная проекция
            if (projection == Projection.PERSPECTIVE)
            {
                Matrix<double> oldCoords = Matrix<double>.Build.DenseOfArray(new double[,] { { Xf, Yf, Zf, 1 } });
                Matrix<double> res = (oldCoords * centralProjectionMatrix).Multiply(1 / (k * Zf + 1));
                return new PointF(worldCenter.X + 150 + (float)res[0, 0], worldCenter.Y + (float)res[0, 1]);
            }
            else 
            {
                Matrix<double> oldCoords = Matrix<double>.Build.DenseOfArray(new double[,] { { Xf }, { Yf }, { Zf } });
                Matrix<double> res = utilMatr * isometricProjectionMatrix * oldCoords;
                return new PointF(worldCenter.X + 150 + (float)res[0,0], worldCenter.Y + (float)res[1,0]);
            }
        }
    }

    class Funcs {
        public static double degreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }

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


    class Face
    {
        List<Line> edges;

        public Face()
        {
            edges = new List<Line>();
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
                x += line.start.Xf;
                y += line.start.Yf;
                z += line.start.Zf;
            }
            return new Point(x / edges.Count, y / edges.Count, z / edges.Count);
        }
    }




    class Shape
    {
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

        public List<Face> Faces { get => faces; }

    }

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
            res.addFace(new Face().addEdge(new Line(f, c)).addEdge(new Line(c, h)).addEdge(new Line(h, f)));
            res.addFace(new Face().addEdge(new Line(c, h)).addEdge(new Line(h, a)).addEdge(new Line(a, c)));
            res.addFace(new Face().addEdge(new Line(f, h)).addEdge(new Line(h, a)).addEdge(new Line(a, f)));
            return res;
        }
    }

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

            res.addFace(new Face().addEdge(new Line(a, f)).addEdge(new Line(f, b)).addEdge(new Line(b, a)));
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
            res.addFace(new Face().addEdge(new Line(b, c)).addEdge(new Line(c, g)).addEdge(new Line(g, f)).addEdge(new Line(f, b)));
            res.addFace(new Face().addEdge(new Line(f, g)).addEdge(new Line(g, h)).addEdge(new Line(h, e)).addEdge(new Line(e, f)));
            res.addFace(new Face().addEdge(new Line(h, e)).addEdge(new Line(e, a)).addEdge(new Line(a, d)).addEdge(new Line(d, h)));
            res.addFace(new Face().addEdge(new Line(a, b)).addEdge(new Line(b, f)).addEdge(new Line(f, e)).addEdge(new Line(e, a)));
            res.addFace(new Face().addEdge(new Line(d, c)).addEdge(new Line(c, g)).addEdge(new Line(g, h)).addEdge(new Line(h, d)));
            return res;
        }

    }

    class Icosahedron : Shape
    {
        public static Icosahedron getIcosahedron()
        {
            Icosahedron res = new Icosahedron();
            Point circleCenter = new Point(100, 100, 100);
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
            res.addFace(new Face().addEdge(new Line(circlePoints[8], b)).addEdge(new Line(b, circlePoints[0])).addEdge(new Line(circlePoints[0], circlePoints[8])));
            return res;
        }

    }

    class Dodecahedron : Shape // 12 граней
    {
        public static Dodecahedron getDodecahedron() // 12 граней
        {
            Dodecahedron res = new Dodecahedron();
            var icosahedron = Icosahedron.getIcosahedron();
            List<Point> centers = new List<Point>();
            for (int i = 0; i < icosahedron.Faces.Count; i++)
            {
                Face face = icosahedron.Faces[i];
                var c = face.getCenter();
                centers.Add(c);
            }

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    res.addFace(new Face().addEdge(new Line(centers[i], centers[(i + 1) % 10])).addEdge(new Line(centers[(i + 1) % 10], centers[(i + 2) % 10])).addEdge(new Line(centers[(i + 2) % 10], centers[15 + (i / 2 + 1) % 5])).addEdge(new Line(centers[15 + (i / 2 + 1) % 5], centers[15 + i / 2])).addEdge(new Line(centers[15 + i / 2], centers[i])));

                    continue;
                }
                res.addFace(new Face().addEdge(new Line(centers[i], centers[(i + 1) % 10])).addEdge(new Line(centers[(i + 1) % 10], centers[(i + 2) % 10])).addEdge(new Line(centers[(i + 2) % 10], centers[10 + (i / 2 + 1) % 5])).addEdge(new Line(centers[10 + (i / 2 + 1) % 5], centers[10 + i / 2])).addEdge(new Line(centers[10 + i / 2], centers[i])));
            }
            res.addFace(new Face().addEdge(new Line(centers[15], centers[16])).addEdge(new Line(centers[16], centers[17])).addEdge(new Line(centers[17], centers[18])).addEdge(new Line(centers[18], centers[19])).addEdge(new Line(centers[19], centers[15])));
            res.addFace(new Face().addEdge(new Line(centers[10], centers[11])).addEdge(new Line(centers[11], centers[12])).addEdge(new Line(centers[12], centers[13])).addEdge(new Line(centers[13], centers[14])).addEdge(new Line(centers[14], centers[10])));

            return res;
        }

    }
}
