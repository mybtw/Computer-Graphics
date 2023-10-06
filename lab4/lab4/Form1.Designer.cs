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
            radioButton4 = new RadioButton();
            button4 = new Button();
            button5 = new Button();
            textBox3 = new TextBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(907, 505);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(976, 32);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(71, 24);
            radioButton1.TabIndex = 1;
            radioButton1.TabStop = true;
            radioButton1.Text = "Точки";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            radioButton1.MouseClick += radioButton1_MouseClick;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(976, 62);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(72, 24);
            radioButton2.TabIndex = 2;
            radioButton2.TabStop = true;
            radioButton2.Text = "Ребра";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.MouseClick += radioButton2_MouseClick;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(976, 92);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(91, 24);
            radioButton3.TabIndex = 3;
            radioButton3.TabStop = true;
            radioButton3.Text = "Полигон";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.MouseClick += radioButton3_MouseClick;
            // 
            // button1
            // 
            button1.Location = new Point(991, 147);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 4;
            button1.Text = "Очистить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(1073, 91);
            button2.Name = "button2";
            button2.Size = new Size(71, 26);
            button2.TabIndex = 5;
            button2.Text = "draw";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(976, 203);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(71, 27);
            textBox1.TabIndex = 6;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(1096, 203);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(68, 27);
            textBox2.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(942, 206);
            label1.Name = "label1";
            label1.Size = new Size(25, 20);
            label1.TabIndex = 8;
            label1.Text = "dx";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1060, 206);
            label2.Name = "label2";
            label2.Size = new Size(25, 20);
            label2.TabIndex = 9;
            label2.Text = "dy";
            label2.Click += label2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(1021, 236);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 10;
            button3.Text = "Сместить";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(976, 283);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(188, 24);
            radioButton4.TabIndex = 11;
            radioButton4.TabStop = true;
            radioButton4.Text = "Задать точку поворота";
            radioButton4.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(1050, 313);
            button4.Name = "button4";
            button4.Size = new Size(94, 29);
            button4.TabIndex = 12;
            button4.Text = "Повернуть ";
            button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(942, 359);
            button5.Name = "button5";
            button5.Size = new Size(222, 29);
            button5.TabIndex = 13;
            button5.Text = "Повернуть вокруг центра";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(951, 315);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(59, 27);
            textBox3.TabIndex = 14;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(1007, 313);
            label3.Name = "label3";
            label3.Size = new Size(15, 20);
            label3.TabIndex = 15;
            label3.Text = "°";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1176, 529);
            Controls.Add(label3);
            Controls.Add(textBox3);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(radioButton4);
            Controls.Add(button3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(radioButton3);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
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
        private RadioButton radioButton4;
        private Button button4;
        private Button button5;
        private TextBox textBox3;
        private Label label3;
    }
}