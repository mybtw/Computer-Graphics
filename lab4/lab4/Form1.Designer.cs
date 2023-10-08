namespace lab4
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton3 = new RadioButton();
            button1 = new Button();
            button2 = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            textBox3 = new TextBox();
            label3 = new Label();
            button6 = new Button();
            numericUpDownX = new NumericUpDown();
            numericUpDownY = new NumericUpDown();
            label4 = new Label();
            button7 = new Button();
            button8 = new Button();
            label5 = new Label();
            label6 = new Label();
            button9 = new Button();
            button10 = new Button();
            button11 = new Button();
            button12 = new Button();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownY).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.Location = new Point(11, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(907, 633);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(977, 31);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(71, 24);
            radioButton1.TabIndex = 1;
            radioButton1.TabStop = true;
            radioButton1.Text = "Точки";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(1100, 31);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(72, 24);
            radioButton2.TabIndex = 2;
            radioButton2.TabStop = true;
            radioButton2.Text = "Ребра";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(1030, 61);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(91, 24);
            radioButton3.TabIndex = 3;
            radioButton3.TabStop = true;
            radioButton3.Text = "Полигон";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(1088, 120);
            button1.Name = "button1";
            button1.Size = new Size(106, 56);
            button1.TabIndex = 4;
            button1.Text = "Очистить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(976, 122);
            button2.Name = "button2";
            button2.Size = new Size(106, 52);
            button2.TabIndex = 5;
            button2.Text = "Добавить полигон";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(1007, 200);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(71, 27);
            textBox1.TabIndex = 6;
            textBox1.Text = "0";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(1126, 200);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(68, 27);
            textBox2.TabIndex = 7;
            textBox2.Text = "0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(976, 207);
            label1.Name = "label1";
            label1.Size = new Size(25, 20);
            label1.TabIndex = 8;
            label1.Text = "dx";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1088, 207);
            label2.Name = "label2";
            label2.Size = new Size(25, 20);
            label2.TabIndex = 9;
            label2.Text = "dy";
            // 
            // button3
            // 
            button3.Location = new Point(1030, 233);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 10;
            button3.Text = "Сместить";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(972, 546);
            button4.Name = "button4";
            button4.Size = new Size(222, 40);
            button4.TabIndex = 12;
            button4.Text = "Повернуть ребро на 90";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(972, 383);
            button5.Name = "button5";
            button5.Size = new Size(222, 29);
            button5.TabIndex = 13;
            button5.Text = "Повернуть вокруг центра";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(1043, 288);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(70, 27);
            textBox3.TabIndex = 14;
            textBox3.Text = "0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(1117, 278);
            label3.Name = "label3";
            label3.Size = new Size(15, 20);
            label3.TabIndex = 15;
            label3.Text = "°";
            // 
            // button6
            // 
            button6.Location = new Point(972, 321);
            button6.Name = "button6";
            button6.Size = new Size(222, 56);
            button6.TabIndex = 13;
            button6.Text = "Повернуть вокруг произвольной точки";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // numericUpDownX
            // 
            numericUpDownX.Location = new Point(1006, 442);
            numericUpDownX.Margin = new Padding(3, 4, 3, 4);
            numericUpDownX.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDownX.Name = "numericUpDownX";
            numericUpDownX.Size = new Size(61, 27);
            numericUpDownX.TabIndex = 16;
            // 
            // numericUpDownY
            // 
            numericUpDownY.Location = new Point(1088, 442);
            numericUpDownY.Margin = new Padding(3, 4, 3, 4);
            numericUpDownY.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDownY.Name = "numericUpDownY";
            numericUpDownY.Size = new Size(51, 27);
            numericUpDownY.TabIndex = 16;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(1007, 418);
            label4.Name = "label4";
            label4.Size = new Size(141, 20);
            label4.TabIndex = 17;
            label4.Text = "Масштабирование";
            // 
            // button7
            // 
            button7.Location = new Point(972, 476);
            button7.Name = "button7";
            button7.Size = new Size(95, 29);
            button7.TabIndex = 13;
            button7.Text = "Центр";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new Point(1073, 476);
            button8.Name = "button8";
            button8.Size = new Size(114, 29);
            button8.TabIndex = 13;
            button8.Text = "Вокруг точки";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(1041, 177);
            label5.Name = "label5";
            label5.Size = new Size(83, 20);
            label5.TabIndex = 17;
            label5.Text = "Смещение";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(1041, 265);
            label6.Name = "label6";
            label6.Size = new Size(70, 20);
            label6.TabIndex = 17;
            label6.Text = "Поворот";
            // 
            // button9
            // 
            button9.Location = new Point(972, 592);
            button9.Name = "button9";
            button9.Size = new Size(222, 53);
            button9.TabIndex = 5;
            button9.Text = "Выделить точки пересечения рёбер";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.Location = new Point(12, 656);
            button10.Name = "button10";
            button10.Size = new Size(244, 29);
            button10.TabIndex = 18;
            button10.Text = "Точка в выпуклом?";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // button11
            // 
            button11.Location = new Point(336, 656);
            button11.Name = "button11";
            button11.Size = new Size(246, 29);
            button11.TabIndex = 19;
            button11.Text = "Точка в невыпуклом?";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // button12
            // 
            button12.Location = new Point(662, 656);
            button12.Name = "button12";
            button12.Size = new Size(193, 29);
            button12.TabIndex = 20;
            button12.Text = "Относительно ребра?";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(262, 660);
            label7.Name = "label7";
            label7.Size = new Size(68, 20);
            label7.TabIndex = 21;
            label7.Text = "Не знаю";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(588, 660);
            label8.Name = "label8";
            label8.Size = new Size(68, 20);
            label8.TabIndex = 22;
            label8.Text = "Не знаю";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(861, 660);
            label9.Name = "label9";
            label9.Size = new Size(68, 20);
            label9.TabIndex = 23;
            label9.Text = "Не знаю";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(1013, 523);
            label10.Name = "label10";
            label10.Size = new Size(135, 20);
            label10.TabIndex = 24;
            label10.Text = "Работа с рёбрами";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(1006, 97);
            label11.Name = "label11";
            label11.Size = new Size(151, 20);
            label11.TabIndex = 25;
            label11.Text = "Работа с полигоном";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1214, 694);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(button12);
            Controls.Add(button11);
            Controls.Add(button10);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(numericUpDownY);
            Controls.Add(numericUpDownX);
            Controls.Add(label3);
            Controls.Add(textBox3);
            Controls.Add(button6);
            Controls.Add(button7);
            Controls.Add(button8);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(button9);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(radioButton3);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Полигоналка";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownX).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownY).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private Button button1;
        private Button button2;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label1;
        private Label label2;
        private Button button3;
        private Button button4;
        private Button button5;
        private TextBox textBox3;
        private Label label3;
        private Button button6;
        private NumericUpDown numericUpDownX;
        private NumericUpDown numericUpDownY;
        private Label label4;
        private Button button7;
        private Button button8;
        private Label label5;
        private Label label6;
        private Button button9;
        private Button button10;
        private Button button11;
        private Button button12;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
    }
}