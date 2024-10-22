namespace Tarmac64_Library
{
    partial class GameBuilderPro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CourseBox1 = new System.Windows.Forms.TextBox();
            this.CourseBox2 = new System.Windows.Forms.TextBox();
            this.CourseBox3 = new System.Windows.Forms.TextBox();
            this.CourseBox4 = new System.Windows.Forms.TextBox();
            this.SetBox = new System.Windows.Forms.ComboBox();
            this.CupBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.HeaderBox = new System.Windows.Forms.TextBox();
            this.DebugBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CourseBox1
            // 
            this.CourseBox1.BackColor = System.Drawing.SystemColors.Window;
            this.CourseBox1.Location = new System.Drawing.Point(12, 94);
            this.CourseBox1.Margin = new System.Windows.Forms.Padding(5);
            this.CourseBox1.Name = "CourseBox1";
            this.CourseBox1.Size = new System.Drawing.Size(246, 20);
            this.CourseBox1.TabIndex = 1;
            this.CourseBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CourseBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CourseBox1_MouseClick);
            this.CourseBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CourseBox1_MouseDoubleClick);
            this.CourseBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CourseBox1_MouseClick);
            // 
            // CourseBox2
            // 
            this.CourseBox2.BackColor = System.Drawing.SystemColors.Window;
            this.CourseBox2.Location = new System.Drawing.Point(12, 124);
            this.CourseBox2.Margin = new System.Windows.Forms.Padding(5);
            this.CourseBox2.Name = "CourseBox2";
            this.CourseBox2.Size = new System.Drawing.Size(246, 20);
            this.CourseBox2.TabIndex = 3;
            this.CourseBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CourseBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CourseBox2_MouseClick);
            this.CourseBox2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CourseBox2_MouseDoubleClick);
            this.CourseBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CourseBox2_MouseClick);
            // 
            // CourseBox3
            // 
            this.CourseBox3.BackColor = System.Drawing.SystemColors.Window;
            this.CourseBox3.Location = new System.Drawing.Point(12, 154);
            this.CourseBox3.Margin = new System.Windows.Forms.Padding(5);
            this.CourseBox3.Name = "CourseBox3";
            this.CourseBox3.Size = new System.Drawing.Size(246, 20);
            this.CourseBox3.TabIndex = 5;
            this.CourseBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CourseBox3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CourseBox3_MouseClick);
            this.CourseBox3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CourseBox3_MouseDoubleClick);
            this.CourseBox3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CourseBox3_MouseClick);
            // 
            // CourseBox4
            // 
            this.CourseBox4.BackColor = System.Drawing.SystemColors.Window;
            this.CourseBox4.Location = new System.Drawing.Point(12, 184);
            this.CourseBox4.Margin = new System.Windows.Forms.Padding(5);
            this.CourseBox4.Name = "CourseBox4";
            this.CourseBox4.Size = new System.Drawing.Size(246, 20);
            this.CourseBox4.TabIndex = 7;
            this.CourseBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CourseBox4.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CourseBox4_MouseClick);
            this.CourseBox4.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CourseBox4_MouseDoubleClick);
            this.CourseBox4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CourseBox4_MouseClick);
            // 
            // SetBox
            // 
            this.SetBox.FormattingEnabled = true;
            this.SetBox.Items.AddRange(new object[] {
            "Custom Set 1",
            "Custom Set 2",
            "Custom Set 3",
            "Custom Set 4",
            "Custom Set 5"});
            this.SetBox.Location = new System.Drawing.Point(12, 12);
            this.SetBox.Name = "SetBox";
            this.SetBox.Size = new System.Drawing.Size(177, 21);
            this.SetBox.TabIndex = 8;
            this.SetBox.SelectedIndexChanged += new System.EventHandler(this.SetBox_SelectedIndexChanged);
            // 
            // CupBox
            // 
            this.CupBox.FormattingEnabled = true;
            this.CupBox.Items.AddRange(new object[] {
            "Mushroom Cup",
            "Flower Cup",
            "Star Cup",
            "Special Cup",
            "Battle Cup"});
            this.CupBox.Location = new System.Drawing.Point(12, 39);
            this.CupBox.Name = "CupBox";
            this.CupBox.Size = new System.Drawing.Size(177, 21);
            this.CupBox.TabIndex = 9;
            this.CupBox.SelectedIndexChanged += new System.EventHandler(this.CupBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Set Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Cup Number";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(193, 219);
            this.button2.Margin = new System.Windows.Forms.Padding(5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Build";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(9, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 26);
            this.label3.TabIndex = 13;
            this.label3.Text = "Double Click - Add\r\nMiddle Click - Remove";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 319);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 63;
            this.label4.Text = "Course Header Table";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // HeaderBox
            // 
            this.HeaderBox.Location = new System.Drawing.Point(125, 316);
            this.HeaderBox.Name = "HeaderBox";
            this.HeaderBox.Size = new System.Drawing.Size(133, 20);
            this.HeaderBox.TabIndex = 62;
            this.HeaderBox.Text = "BE9178";
            this.HeaderBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DebugBox
            // 
            this.DebugBox.AutoSize = true;
            this.DebugBox.Location = new System.Drawing.Point(167, 276);
            this.DebugBox.Name = "DebugBox";
            this.DebugBox.Size = new System.Drawing.Size(91, 17);
            this.DebugBox.TabIndex = 64;
            this.DebugBox.Text = "Export Debug";
            this.DebugBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 219);
            this.button1.Margin = new System.Windows.Forms.Padding(5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 23);
            this.button1.TabIndex = 65;
            this.button1.Text = "<->";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(125, 354);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(5);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(65, 23);
            this.SaveButton.TabIndex = 67;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(193, 354);
            this.LoadButton.Margin = new System.Windows.Forms.Padding(5);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(65, 23);
            this.LoadButton.TabIndex = 66;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // GameBuilderPro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 388);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DebugBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.HeaderBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CupBox);
            this.Controls.Add(this.SetBox);
            this.Controls.Add(this.CourseBox4);
            this.Controls.Add(this.CourseBox3);
            this.Controls.Add(this.CourseBox2);
            this.Controls.Add(this.CourseBox1);
            this.Name = "GameBuilderPro";
            this.Text = "GameBuilderPro";
            this.Load += new System.EventHandler(this.GameBuilderPro_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox CourseBox1;
        private System.Windows.Forms.TextBox CourseBox2;
        private System.Windows.Forms.TextBox CourseBox3;
        private System.Windows.Forms.TextBox CourseBox4;
        private System.Windows.Forms.ComboBox SetBox;
        private System.Windows.Forms.ComboBox CupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox HeaderBox;
        private System.Windows.Forms.CheckBox DebugBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button LoadButton;
    }
}