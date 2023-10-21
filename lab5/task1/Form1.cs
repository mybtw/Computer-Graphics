using System.Text;


namespace task1
{
    public partial class Form1 : Form
    {
        string direction = "";
        string axiom = "";
        float rotate;
        private Graphics g;
        int gen = 0;
        string fileName = "..\\..\\..\\L-systems\\КриваяКоха.txt";
        int randFrom = 0;
        int randTo = 0;
        List<string> LSystem = new List<string>();
        List<Tuple<PointF, PointF>> points = new List<Tuple<PointF, PointF>>();
        public Form1()
        {
            InitializeComponent();
            AddItemsToComboBox();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AddItemsToComboBox()
        {
            // Создайте массив строк для значений, которые вы хотите добавить
            string[] items = { "Кривая Коха ", "Снежинка Коха", "Ковер Серпинского", "Треугольник Серпинского", "Квадратный остров Коха",
             "Наконечник Серпинского", "Кривая Гильберта", "Кривая дракона Хартера-Хейтуэя ", "Шестиугольная кривая Госпера", "Широкое Дерево",
            "Куст1", "Куст2", "Куст3", "ШестиугольнаяМозаика"};

            // Добавьте элементы в ComboBox
            foreach (string item in items)
            {
                comboBox1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearScene();
            chooseFile();
            readConfig();
            drawFractal();
        }

        private void readConfig()
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line = sr.ReadLine();
                string[] parseLine = line.Split(' ');
                axiom = parseLine[0];
                rotate = float.Parse(parseLine[1]);
                direction = parseLine[2];
                line = sr.ReadLine();
                while (line != null)
                {
                    LSystem.Add(line);
                    line = sr.ReadLine();
                }
            }
            string genText = textBox1.Text;
            gen = genText != "" ? Int32.Parse(genText) : 1;
            string randF = textBox2.Text;
            randFrom = randF != "" ? Int32.Parse(randF) : 0;
            string randT = textBox3.Text;
            randTo = randT != "" ? Int32.Parse(randT) : 0;
        }

        private float calcAngle()
        {
            float angle = 0;
            switch (direction)
            {
                case "left":
                    angle = (float)Math.PI; // 180 градусов
                    break;
                case "down":
                    angle = (float)Math.PI / 2; // 0 градусов
                    break;
                case "up":
                    angle = (float)(3 * Math.PI / 2); // 270(или -90) градусов
                    break;
            }
            return angle;
        }

        private void drawFractal()
        {
            // разбиваем каждое правило
            Dictionary<char, string> rules = new Dictionary<char, string>();
            foreach (string line in LSystem)
            {
                rules[line[0]] = line.Substring(2);
            }

            // выводим одно общее правило построения для указанного поколения
            string sb = axiom;
            StringBuilder seq = new StringBuilder();
            for (int i = 0; i < gen; ++i)
            {
                seq.Clear();
                foreach (char lex in sb.ToString())
                {
                    if (rules.ContainsKey(lex))
                    {
                        seq.Append(rules[lex]);
                    }
                    else
                    {
                        seq.Append(lex);
                    }
                }
                sb = seq.ToString();
            }
            String rule = sb.ToString();

            float rot = rotate * (float)Math.PI / 180; // градусы в радианы
            PointF point = new PointF(0, 0);
            float angle = calcAngle();
            float minX = 0, minY = 0, maxX = 0, maxY = 0;
            int randRotate = 0;
            Random rand = new Random();
            Color color = Color.FromArgb(64, 0, 0);
            Dictionary<PointF, Tuple<Color, float>> gr = new Dictionary<PointF, Tuple<Color, float>>(); // точка : цвет и угол
            Stack<Tuple<PointF, float>> st = new Stack<Tuple<PointF, float>>(); //
            int count = 0;
            float width = 7;

           // Color brownColor = Color.FromArgb(153, 102, 51);
           // Color greenColor = Color.FromArgb(0, 128, 0);

            foreach (char lex in rule)
            {
                if (lex == 'F')
                {
                    // следующая точка фрактала переносится от старой по направлению синуса
                    float nextX = (float)(point.X + Math.Cos(angle)), nextY = (float)(point.Y + Math.Sin(angle));
                    if (nextX < minX) minX = nextX;
                    if (nextX > maxX) maxX = nextX;
                    if (nextY < minY) minY = nextY;
                    if (nextY > maxY) maxY = nextY;
                    points.Add(Tuple.Create(point, new PointF(nextX, nextY)));

                    if (fileName.Contains("Дерево") || fileName.Contains("Куст"))
                    {
                        if (count < 3)
                        {
                            width--;
                            count++;
                        }
                        gr[point] = new Tuple<Color, float>(color, width);
                    }

                    point = new PointF(nextX, nextY);
                }
                else if (lex == '[')
                {
                    st.Push(Tuple.Create(point, angle));
                    if (fileName.Contains("Дерево") || fileName.Contains("Куст"))
                    {
                        color = color.G + 20 > 255 ? Color.FromArgb(color.R, 255, color.B) : Color.FromArgb(color.R, color.G + 20, color.B);
                        width--;
                    }
                }
                else if (lex == ']')
                {
                    Tuple<PointF, float> tuple = st.Pop();
                    point = tuple.Item1;
                    angle = tuple.Item2;
                    if (fileName.Contains("Дерево") || fileName.Contains("Куст"))
                    {
                        color = color.G - 20 < 0 ? Color.FromArgb(color.R, 0, color.B) : Color.FromArgb(color.R, color.G - 20, color.B);
                        width++;
                    }
                }

                else if (lex == '-')
                {
                    randRotate = rand.Next(randFrom, randTo + 1);
                    angle -= rot + randRotate * (float)Math.PI / 180;
                }
                else if (lex == '+')
                {
                    randRotate = rand.Next(randFrom, randTo + 1);
                    angle += rot + randRotate * (float)Math.PI / 180;
                }
            }

            minX -= 1;
            maxX +=  1;
            minY -= 1;
            maxY += 1;


            // центр окна
            PointF center = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            // центр полученного фрактала
            PointF fractal = new PointF(minX + (maxX - minX) / 2, minY + (maxY - minY) / 2);
            // шаг для масштабирования
            float step = Math.Min(pictureBox1.Width / (maxX - minX), pictureBox1.Height / (maxY - minY));

            List<Tuple<PointF, PointF>> scalePoints = new List<Tuple<PointF, PointF>>(points);

            for (int i = 0; i < points.Count(); i++)
            {
                float scaleX = center.X + (points[i].Item1.X - fractal.X) * step;
                float scaleY = center.Y + (points[i].Item1.Y - fractal.Y) * step;
                float scaleNextX = center.X + (points[i].Item2.X - fractal.X) * step;
                float scaleNextY = center.Y + (points[i].Item2.Y - fractal.Y) * step;
                scalePoints[i] = new Tuple<PointF, PointF>(new PointF(scaleX, scaleY), new PointF(scaleNextX, scaleNextY));
            }
            if (fileName.Contains("Дерево") || fileName.Contains("Куст"))
            {
                for (int i = 0; i < points.Count(); i++)
                {
                    g.DrawLine(new Pen(gr[points[i].Item1].Item1, gr[points[i].Item1].Item2), scalePoints[i].Item1, scalePoints[i].Item2);
                }
            }
            else
            {
                for (int i = 0; i < points.Count(); i++)
                {
                    g.DrawLine(new Pen(Color.Black), scalePoints[i].Item1, scalePoints[i].Item2);
                }
            }

            pictureBox1.Invalidate();
        }
        private void chooseFile()
        {
            int ind = comboBox1.SelectedIndex;
            switch (ind)
            {
                case 0:
                    fileName = "..\\..\\..\\L-systems\\КриваяКоха.txt";
                    break;
                case 1:
                    fileName = "..\\..\\..\\L-systems\\СнежинкаКоха.txt";
                    break;
                case 2:
                    fileName = "..\\..\\..\\L-systems\\КоверСерпинского.txt";
                    break;
                case 3:
                    fileName = "..\\..\\..\\L-systems\\ТреугольникСерпинского.txt";
                    break;
                case 4:
                    fileName = "..\\..\\..\\L-systems\\ОстровКоха.txt";
                    break;
                case 5:
                    fileName = "..\\..\\..\\L-systems\\НаконечникСерпинского.txt";
                    break;
                case 6:
                    fileName = "..\\..\\..\\L-systems\\КриваяГильберта.txt";
                    break;
                case 7:
                    fileName = "..\\..\\..\\L-systems\\КриваяДракона.txt";
                    break;
                case 8:
                    fileName = "..\\..\\..\\L-systems\\ШестиугольнаяКриваяГоспера.txt";
                    break;
                case 9:
                    fileName = "..\\..\\..\\L-systems\\ШирокоеДерево.txt";
                    break;
                case 10:
                    fileName = "..\\..\\..\\L-systems\\Куст1.txt";
                    break;
                case 11:
                    fileName = "..\\..\\..\\L-systems\\Куст2.txt";
                    break;
                case 12:
                    fileName = "..\\..\\..\\L-systems\\Куст3.txt";
                    break;
                case 13:
                    fileName = "..\\..\\..\\L-systems\\ШестиугольнаяМозаика.txt";
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearScene();
        }

        private void clearScene()
        {
            g.Clear(Color.White);
            points.Clear();
            LSystem.Clear();
            pictureBox1.Invalidate();

        }
    }
}