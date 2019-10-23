namespace OverKart64
{
    partial class CourseSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CourseSelect));
            this.c1 = new System.Windows.Forms.ComboBox();
            this.savebtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.r0 = new System.Windows.Forms.ComboBox();
            this.r1 = new System.Windows.Forms.ComboBox();
            this.r3 = new System.Windows.Forms.ComboBox();
            this.r2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // c1
            // 
            this.c1.Enabled = false;
            this.c1.FormattingEnabled = true;
            this.c1.Location = new System.Drawing.Point(65, 49);
            this.c1.Name = "c1";
            this.c1.Size = new System.Drawing.Size(302, 21);
            this.c1.TabIndex = 0;
            this.c1.SelectedIndexChanged += new System.EventHandler(this.C1_SelectedIndexChanged);
            // 
            // savebtn
            // 
            this.savebtn.Enabled = false;
            this.savebtn.Location = new System.Drawing.Point(73, 12);
            this.savebtn.Name = "savebtn";
            this.savebtn.Size = new System.Drawing.Size(50, 23);
            this.savebtn.TabIndex = 15;
            this.savebtn.Text = "Save";
            this.savebtn.UseVisualStyleBackColor = true;
            this.savebtn.Click += new System.EventHandler(this.Savebtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // r0
            // 
            this.r0.Enabled = false;
            this.r0.FormattingEnabled = true;
            this.r0.Location = new System.Drawing.Point(65, 111);
            this.r0.Name = "r0";
            this.r0.Size = new System.Drawing.Size(302, 21);
            this.r0.TabIndex = 16;
            this.r0.SelectedIndexChanged += new System.EventHandler(this.R1_SelectedIndexChanged);
            // 
            // r1
            // 
            this.r1.Enabled = false;
            this.r1.FormattingEnabled = true;
            this.r1.Location = new System.Drawing.Point(65, 149);
            this.r1.Name = "r1";
            this.r1.Size = new System.Drawing.Size(302, 21);
            this.r1.TabIndex = 17;
            this.r1.SelectedIndexChanged += new System.EventHandler(this.R2_SelectedIndexChanged);
            // 
            // r3
            // 
            this.r3.Enabled = false;
            this.r3.FormattingEnabled = true;
            this.r3.Location = new System.Drawing.Point(65, 225);
            this.r3.Name = "r3";
            this.r3.Size = new System.Drawing.Size(302, 21);
            this.r3.TabIndex = 19;
            this.r3.SelectedIndexChanged += new System.EventHandler(this.R4_SelectedIndexChanged);
            // 
            // r2
            // 
            this.r2.Enabled = false;
            this.r2.FormattingEnabled = true;
            this.r2.Location = new System.Drawing.Point(65, 187);
            this.r2.Name = "r2";
            this.r2.Size = new System.Drawing.Size(302, 21);
            this.r2.TabIndex = 18;
            this.r2.SelectedIndexChanged += new System.EventHandler(this.R3_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Race 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Race 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Race 4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Race 3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Course";
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label23.Location = new System.Drawing.Point(17, 89);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(351, 2);
            this.label23.TabIndex = 48;
            // 
            // CourseSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 259);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.r3);
            this.Controls.Add(this.r2);
            this.Controls.Add(this.r1);
            this.Controls.Add(this.r0);
            this.Controls.Add(this.savebtn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.c1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CourseSelect";
            this.Text = "Cup Editor";
            this.Load += new System.EventHandler(this.CourseSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox c1;
        private System.Windows.Forms.Button savebtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox r0;
        private System.Windows.Forms.ComboBox r1;
        private System.Windows.Forms.ComboBox r3;
        private System.Windows.Forms.ComboBox r2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label23;
    }
}