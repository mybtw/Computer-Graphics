namespace lab6
{
    public partial class Form1 : Form
    {
        Graphics g;
        Bitmap bmp;
        List<Shape> figures;

        public Form1()
        {
            InitializeComponent();
            AddItemsToComboBox();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(bmp);
            Point.worldCenter = new PointF(0, 0);
            figures = new List<Shape>();
            radioButton1.Checked = true;
            clearScene();

        }

        private void AddItemsToComboBox()
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
            foreach (var figure in figures)
            {
                draw(figure);
            }
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
            Shape figure;
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
            figures.Clear();
            clearScene();
        }
    }
}