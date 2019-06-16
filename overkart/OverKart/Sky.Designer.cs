namespace OverKart64
{
    partial class Sky
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
            this.button1 = new System.Windows.Forms.Button();
            this.coursebox = new System.Windows.Forms.ComboBox();
            this.rtbox = new System.Windows.Forms.TextBox();
            this.gtbox = new System.Windows.Forms.TextBox();
            this.btbox = new System.Windows.Forms.TextBox();
            this.bbox = new System.Windows.Forms.TextBox();
            this.gbbox = new System.Windows.Forms.TextBox();
            this.rbbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.savebtn = new System.Windows.Forms.Button();
            this.cptop = new System.Windows.Forms.Button();
            this.cpbot = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // coursebox
            // 
            this.coursebox.FormattingEnabled = true;
            this.coursebox.Location = new System.Drawing.Point(124, 12);
            this.coursebox.Name = "coursebox";
            this.coursebox.Size = new System.Drawing.Size(314, 21);
            this.coursebox.TabIndex = 1;
            this.coursebox.SelectedIndexChanged += new System.EventHandler(this.Coursebox_SelectedIndexChanged);
            // 
            // rtbox
            // 
            this.rtbox.Enabled = false;
            this.rtbox.Location = new System.Drawing.Point(96, 55);
            this.rtbox.Name = "rtbox";
            this.rtbox.Size = new System.Drawing.Size(100, 20);
            this.rtbox.TabIndex = 2;
            this.rtbox.TextChanged += new System.EventHandler(this.Rtbox_TextChanged);
            // 
            // gtbox
            // 
            this.gtbox.Enabled = false;
            this.gtbox.Location = new System.Drawing.Point(202, 55);
            this.gtbox.Name = "gtbox";
            this.gtbox.Size = new System.Drawing.Size(100, 20);
            this.gtbox.TabIndex = 3;
            this.gtbox.TextChanged += new System.EventHandler(this.Gtbox_TextChanged);
            // 
            // btbox
            // 
            this.btbox.Enabled = false;
            this.btbox.Location = new System.Drawing.Point(308, 55);
            this.btbox.Name = "btbox";
            this.btbox.Size = new System.Drawing.Size(100, 20);
            this.btbox.TabIndex = 4;
            this.btbox.TextChanged += new System.EventHandler(this.Btbox_TextChanged);
            // 
            // bbox
            // 
            this.bbox.Enabled = false;
            this.bbox.Location = new System.Drawing.Point(308, 94);
            this.bbox.Name = "bbox";
            this.bbox.Size = new System.Drawing.Size(100, 20);
            this.bbox.TabIndex = 7;
            this.bbox.TextChanged += new System.EventHandler(this.Bbox_TextChanged);
            // 
            // gbbox
            // 
            this.gbbox.Enabled = false;
            this.gbbox.Location = new System.Drawing.Point(202, 94);
            this.gbbox.Name = "gbbox";
            this.gbbox.Size = new System.Drawing.Size(100, 20);
            this.gbbox.TabIndex = 6;
            this.gbbox.TextChanged += new System.EventHandler(this.Gbbox_TextChanged);
            // 
            // rbbox
            // 
            this.rbbox.Enabled = false;
            this.rbbox.Location = new System.Drawing.Point(96, 94);
            this.rbbox.Name = "rbbox";
            this.rbbox.Size = new System.Drawing.Size(100, 20);
            this.rbbox.TabIndex = 5;
            this.rbbox.TextChanged += new System.EventHandler(this.Rbbox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Top Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Bottom Color";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(96, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "R";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(202, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "G";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(305, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "B";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // savebtn
            // 
            this.savebtn.Enabled = false;
            this.savebtn.Location = new System.Drawing.Point(68, 10);
            this.savebtn.Name = "savebtn";
            this.savebtn.Size = new System.Drawing.Size(50, 23);
            this.savebtn.TabIndex = 13;
            this.savebtn.Text = "Save";
            this.savebtn.UseVisualStyleBackColor = true;
            this.savebtn.Click += new System.EventHandler(this.Button2_Click);
            // 
            // cptop
            // 
            this.cptop.BackColor = System.Drawing.Color.Fuchsia;
            this.cptop.Enabled = false;
            this.cptop.Location = new System.Drawing.Point(414, 53);
            this.cptop.Name = "cptop";
            this.cptop.Size = new System.Drawing.Size(24, 23);
            this.cptop.TabIndex = 14;
            this.cptop.UseVisualStyleBackColor = false;
            this.cptop.Click += new System.EventHandler(this.Cptop_Click);
            // 
            // cpbot
            // 
            this.cpbot.BackColor = System.Drawing.Color.Fuchsia;
            this.cpbot.Enabled = false;
            this.cpbot.Location = new System.Drawing.Point(414, 92);
            this.cpbot.Name = "cpbot";
            this.cpbot.Size = new System.Drawing.Size(24, 23);
            this.cpbot.TabIndex = 15;
            this.cpbot.UseVisualStyleBackColor = false;
            this.cpbot.Click += new System.EventHandler(this.Cpbot_Click);
            // 
            // Sky
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 137);
            this.Controls.Add(this.cpbot);
            this.Controls.Add(this.cptop);
            this.Controls.Add(this.savebtn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bbox);
            this.Controls.Add(this.gbbox);
            this.Controls.Add(this.rbbox);
            this.Controls.Add(this.btbox);
            this.Controls.Add(this.gtbox);
            this.Controls.Add(this.rtbox);
            this.Controls.Add(this.coursebox);
            this.Controls.Add(this.button1);
            this.Name = "Sky";
            this.Text = "Sky Editor";
            this.Load += new System.EventHandler(this.Sky_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Sky_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox coursebox;
        private System.Windows.Forms.TextBox rtbox;
        private System.Windows.Forms.TextBox gtbox;
        private System.Windows.Forms.TextBox btbox;
        private System.Windows.Forms.TextBox bbox;
        private System.Windows.Forms.TextBox gbbox;
        private System.Windows.Forms.TextBox rbbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button savebtn;
        private System.Windows.Forms.Button cptop;
        private System.Windows.Forms.Button cpbot;
    }
}