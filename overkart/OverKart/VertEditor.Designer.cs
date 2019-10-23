namespace OverKart64
{
    partial class VertEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VertEditor));
            this.button1 = new System.Windows.Forms.Button();
            this.coursebox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.expbtn = new System.Windows.Forms.Button();
            this.impbtn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.abox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbox = new System.Windows.Forms.TextBox();
            this.sbox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.bbox = new System.Windows.Forms.TextBox();
            this.gbox = new System.Windows.Forms.TextBox();
            this.rbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.vertselect = new System.Windows.Forms.ComboBox();
            this.zbox = new System.Windows.Forms.TextBox();
            this.ybox = new System.Windows.Forms.TextBox();
            this.xbox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load ROM";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // coursebox
            // 
            this.coursebox.Enabled = false;
            this.coursebox.FormattingEnabled = true;
            this.coursebox.Location = new System.Drawing.Point(93, 14);
            this.coursebox.Name = "coursebox";
            this.coursebox.Size = new System.Drawing.Size(121, 21);
            this.coursebox.TabIndex = 1;
            this.coursebox.SelectedIndexChanged += new System.EventHandler(this.Coursebox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.expbtn);
            this.groupBox1.Controls.Add(this.impbtn);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.abox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbox);
            this.groupBox1.Controls.Add(this.sbox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.bbox);
            this.groupBox1.Controls.Add(this.gbox);
            this.groupBox1.Controls.Add(this.rbox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.vertselect);
            this.groupBox1.Controls.Add(this.zbox);
            this.groupBox1.Controls.Add(this.ybox);
            this.groupBox1.Controls.Add(this.xbox);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 189);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vertex Positions";
            // 
            // expbtn
            // 
            this.expbtn.Enabled = false;
            this.expbtn.Location = new System.Drawing.Point(221, 153);
            this.expbtn.Name = "expbtn";
            this.expbtn.Size = new System.Drawing.Size(58, 23);
            this.expbtn.TabIndex = 21;
            this.expbtn.Text = "Export";
            this.expbtn.UseVisualStyleBackColor = true;
            this.expbtn.Click += new System.EventHandler(this.expbtn_Click);
            // 
            // impbtn
            // 
            this.impbtn.Enabled = false;
            this.impbtn.Location = new System.Drawing.Point(149, 153);
            this.impbtn.Name = "impbtn";
            this.impbtn.Size = new System.Drawing.Size(58, 23);
            this.impbtn.TabIndex = 4;
            this.impbtn.Text = "Import";
            this.impbtn.UseVisualStyleBackColor = true;
            this.impbtn.Click += new System.EventHandler(this.Impbtn_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(159, 127);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "A";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // abox
            // 
            this.abox.Location = new System.Drawing.Point(179, 124);
            this.abox.Name = "abox";
            this.abox.Size = new System.Drawing.Size(100, 20);
            this.abox.TabIndex = 19;
            this.abox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Abox_KeyUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "T";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "S";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbox
            // 
            this.tbox.Location = new System.Drawing.Point(26, 150);
            this.tbox.Name = "tbox";
            this.tbox.Size = new System.Drawing.Size(100, 20);
            this.tbox.TabIndex = 16;
            this.tbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Tbox_KeyUp);
            // 
            // sbox
            // 
            this.sbox.Location = new System.Drawing.Point(26, 124);
            this.sbox.Name = "sbox";
            this.sbox.Size = new System.Drawing.Size(100, 20);
            this.sbox.TabIndex = 15;
            this.sbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Sbox_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(159, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "B";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(159, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "G";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(159, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "R";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // bbox
            // 
            this.bbox.Location = new System.Drawing.Point(179, 98);
            this.bbox.Name = "bbox";
            this.bbox.Size = new System.Drawing.Size(100, 20);
            this.bbox.TabIndex = 11;
            this.bbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Bbox_KeyUp);
            // 
            // gbox
            // 
            this.gbox.Location = new System.Drawing.Point(179, 72);
            this.gbox.Name = "gbox";
            this.gbox.Size = new System.Drawing.Size(100, 20);
            this.gbox.TabIndex = 10;
            this.gbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Gbox_KeyUp);
            // 
            // rbox
            // 
            this.rbox.Location = new System.Drawing.Point(179, 46);
            this.rbox.Name = "rbox";
            this.rbox.Size = new System.Drawing.Size(100, 20);
            this.rbox.TabIndex = 9;
            this.rbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Rbox_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Z";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Y";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "X";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Vert Index";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // vertselect
            // 
            this.vertselect.FormattingEnabled = true;
            this.vertselect.Location = new System.Drawing.Point(149, 19);
            this.vertselect.Name = "vertselect";
            this.vertselect.Size = new System.Drawing.Size(121, 21);
            this.vertselect.TabIndex = 4;
            this.vertselect.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // zbox
            // 
            this.zbox.Location = new System.Drawing.Point(26, 98);
            this.zbox.Name = "zbox";
            this.zbox.Size = new System.Drawing.Size(100, 20);
            this.zbox.TabIndex = 2;
            this.zbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Zbox_KeyUp);
            // 
            // ybox
            // 
            this.ybox.Location = new System.Drawing.Point(26, 72);
            this.ybox.Name = "ybox";
            this.ybox.Size = new System.Drawing.Size(100, 20);
            this.ybox.TabIndex = 1;
            this.ybox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Ybox_KeyUp);
            // 
            // xbox
            // 
            this.xbox.Location = new System.Drawing.Point(26, 46);
            this.xbox.Name = "xbox";
            this.xbox.Size = new System.Drawing.Size(100, 20);
            this.xbox.TabIndex = 0;
            this.xbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Xbox_KeyUp);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(222, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Dump Seg4";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click_1);
            // 
            // VertEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 238);
            this.Controls.Add(this.coursebox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VertEditor";
            this.Text = "Vertex Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox coursebox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox zbox;
        private System.Windows.Forms.TextBox ybox;
        private System.Windows.Forms.TextBox xbox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox abox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbox;
        private System.Windows.Forms.TextBox sbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox bbox;
        private System.Windows.Forms.TextBox gbox;
        private System.Windows.Forms.TextBox rbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox vertselect;
        private System.Windows.Forms.Button impbtn;
        private System.Windows.Forms.Button expbtn;
    }
}