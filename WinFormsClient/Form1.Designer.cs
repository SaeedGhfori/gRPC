namespace WinFormsClient
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
            dataGridView1 = new DataGridView();
            button1 = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            groupBox1 = new GroupBox();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 250);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(776, 188);
            dataGridView1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(20, 61);
            button1.Name = "button1";
            button1.Size = new Size(174, 29);
            button1.TabIndex = 1;
            button1.Text = "افزودن کالا به سرور";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(492, 63);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(346, 63);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(125, 27);
            textBox2.TabIndex = 2;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(215, 63);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(125, 27);
            textBox3.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(567, 23);
            label1.Name = "label1";
            label1.Size = new Size(32, 20);
            label1.TabIndex = 3;
            label1.Text = "برند";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(421, 23);
            label2.Name = "label2";
            label2.Size = new Size(27, 20);
            label2.TabIndex = 3;
            label2.Text = "نام";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(271, 23);
            label3.Name = "label3";
            label3.Size = new Size(45, 20);
            label3.TabIndex = 3;
            label3.Text = "قیمت";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(textBox3);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(49, 46);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(640, 125);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            // 
            // button2
            // 
            button2.Location = new Point(577, 204);
            button2.Name = "button2";
            button2.Size = new Size(174, 29);
            button2.TabIndex = 1;
            button2.Text = "دریافت دیتا از سرور";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox1);
            Controls.Add(button2);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Button button1;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        private GroupBox groupBox1;
        private Button button2;
    }
}
