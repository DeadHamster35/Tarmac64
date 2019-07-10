namespace OverKart64
{
    partial class VertConvert
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
            this.facedump = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.coursebox = new System.Windows.Forms.ComboBox();
            this.seg6a = new System.Windows.Forms.TextBox();
            this.seg6e = new System.Windows.Forms.TextBox();
            this.seg7e = new System.Windows.Forms.TextBox();
            this.seg4a = new System.Windows.Forms.TextBox();
            this.seg9e = new System.Windows.Forms.TextBox();
            this.seg9a = new System.Windows.Forms.TextBox();
            this.flagbox = new System.Windows.Forms.TextBox();
            this.texra = new System.Windows.Forms.TextBox();
            this.seg7s = new System.Windows.Forms.TextBox();
            this.seg7ra = new System.Windows.Forms.TextBox();
            this.nv = new System.Windows.Forms.TextBox();
            this.seg47ra = new System.Windows.Forms.TextBox();
            this.padbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.vertdump = new System.Windows.Forms.Button();
            this.seg6btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listdump = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.seg4btn = new System.Windows.Forms.Button();
            this.seg7btn = new System.Windows.Forms.Button();
            this.seg9btn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // facedump
            // 
            this.facedump.Location = new System.Drawing.Point(10, 50);
            this.facedump.Name = "facedump";
            this.facedump.Size = new System.Drawing.Size(140, 25);
            this.facedump.TabIndex = 0;
            this.facedump.Text = "Dump Raw Faces";
            this.facedump.UseVisualStyleBackColor = true;
            this.facedump.Click += new System.EventHandler(this.DumpFaces);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(12, 12);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 1;
            this.LoadButton.Text = "Load ROM";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.Load_Click);
            // 
            // coursebox
            // 
            this.coursebox.FormattingEnabled = true;
            this.coursebox.Location = new System.Drawing.Point(17, 41);
            this.coursebox.Name = "coursebox";
            this.coursebox.Size = new System.Drawing.Size(158, 21);
            this.coursebox.TabIndex = 2;
            this.coursebox.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // seg6a
            // 
            this.seg6a.Location = new System.Drawing.Point(75, 68);
            this.seg6a.Name = "seg6a";
            this.seg6a.Size = new System.Drawing.Size(100, 20);
            this.seg6a.TabIndex = 3;
            // 
            // seg6e
            // 
            this.seg6e.Location = new System.Drawing.Point(75, 94);
            this.seg6e.Name = "seg6e";
            this.seg6e.Size = new System.Drawing.Size(100, 20);
            this.seg6e.TabIndex = 4;
            // 
            // seg7e
            // 
            this.seg7e.Location = new System.Drawing.Point(75, 146);
            this.seg7e.Name = "seg7e";
            this.seg7e.Size = new System.Drawing.Size(100, 20);
            this.seg7e.TabIndex = 6;
            // 
            // seg4a
            // 
            this.seg4a.Location = new System.Drawing.Point(75, 120);
            this.seg4a.Name = "seg4a";
            this.seg4a.Size = new System.Drawing.Size(100, 20);
            this.seg4a.TabIndex = 5;
            // 
            // seg9e
            // 
            this.seg9e.Location = new System.Drawing.Point(75, 198);
            this.seg9e.Name = "seg9e";
            this.seg9e.Size = new System.Drawing.Size(100, 20);
            this.seg9e.TabIndex = 8;
            // 
            // seg9a
            // 
            this.seg9a.Location = new System.Drawing.Point(75, 172);
            this.seg9a.Name = "seg9a";
            this.seg9a.Size = new System.Drawing.Size(100, 20);
            this.seg9a.TabIndex = 7;
            // 
            // flagbox
            // 
            this.flagbox.Location = new System.Drawing.Point(283, 172);
            this.flagbox.Name = "flagbox";
            this.flagbox.Size = new System.Drawing.Size(100, 20);
            this.flagbox.TabIndex = 14;
            // 
            // texra
            // 
            this.texra.Location = new System.Drawing.Point(283, 146);
            this.texra.Name = "texra";
            this.texra.Size = new System.Drawing.Size(100, 20);
            this.texra.TabIndex = 13;
            // 
            // seg7s
            // 
            this.seg7s.Location = new System.Drawing.Point(283, 120);
            this.seg7s.Name = "seg7s";
            this.seg7s.Size = new System.Drawing.Size(100, 20);
            this.seg7s.TabIndex = 12;
            // 
            // seg7ra
            // 
            this.seg7ra.Location = new System.Drawing.Point(283, 94);
            this.seg7ra.Name = "seg7ra";
            this.seg7ra.Size = new System.Drawing.Size(100, 20);
            this.seg7ra.TabIndex = 11;
            // 
            // nv
            // 
            this.nv.Location = new System.Drawing.Point(283, 68);
            this.nv.Name = "nv";
            this.nv.Size = new System.Drawing.Size(100, 20);
            this.nv.TabIndex = 10;
            // 
            // seg47ra
            // 
            this.seg47ra.Location = new System.Drawing.Point(283, 42);
            this.seg47ra.Name = "seg47ra";
            this.seg47ra.Size = new System.Drawing.Size(100, 20);
            this.seg47ra.TabIndex = 9;
            // 
            // padbox
            // 
            this.padbox.Location = new System.Drawing.Point(283, 198);
            this.padbox.Name = "padbox";
            this.padbox.Size = new System.Drawing.Size(100, 20);
            this.padbox.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(181, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Seg4-7 RSP@";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(181, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 17);
            this.label2.TabIndex = 17;
            this.label2.Text = "# Vertices";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(181, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 17);
            this.label3.TabIndex = 19;
            this.label3.Text = "Seg7 Size";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(181, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 17);
            this.label4.TabIndex = 18;
            this.label4.Text = "Seg7 RSP@";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(181, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 17);
            this.label5.TabIndex = 21;
            this.label5.Text = "? Flag";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(181, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 17);
            this.label6.TabIndex = 20;
            this.label6.Text = "Textures RSP@";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(14, 200);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 15);
            this.label7.TabIndex = 27;
            this.label7.Text = "Seg9 End";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(14, 174);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 15);
            this.label8.TabIndex = 26;
            this.label8.Text = "Seg9 @";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(14, 148);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 15);
            this.label9.TabIndex = 25;
            this.label9.Text = "Seg7 End";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(14, 122);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 15);
            this.label10.TabIndex = 24;
            this.label10.Text = "Seg4 @";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(14, 96);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 15);
            this.label11.TabIndex = 23;
            this.label11.Text = "Seg6 End";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(14, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 15);
            this.label12.TabIndex = 22;
            this.label12.Text = "Seg6 @";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(181, 201);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 17);
            this.label13.TabIndex = 28;
            this.label13.Text = "? Padding";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // vertdump
            // 
            this.vertdump.Location = new System.Drawing.Point(10, 19);
            this.vertdump.Name = "vertdump";
            this.vertdump.Size = new System.Drawing.Size(140, 25);
            this.vertdump.TabIndex = 29;
            this.vertdump.Text = "Dump Raw Verts";
            this.vertdump.UseVisualStyleBackColor = true;
            this.vertdump.Click += new System.EventHandler(this.DumpVerts);
            // 
            // seg6btn
            // 
            this.seg6btn.Location = new System.Drawing.Point(12, 50);
            this.seg6btn.Name = "seg6btn";
            this.seg6btn.Size = new System.Drawing.Size(140, 25);
            this.seg6btn.TabIndex = 30;
            this.seg6btn.Text = "Decompress Seg 6";
            this.seg6btn.UseVisualStyleBackColor = true;
            this.seg6btn.Click += new System.EventHandler(this.Segment6_decompress);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listdump);
            this.groupBox1.Controls.Add(this.facedump);
            this.groupBox1.Controls.Add(this.vertdump);
            this.groupBox1.Location = new System.Drawing.Point(17, 239);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 145);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Level Geometry";
            // 
            // listdump
            // 
            this.listdump.Location = new System.Drawing.Point(12, 81);
            this.listdump.Name = "listdump";
            this.listdump.Size = new System.Drawing.Size(140, 25);
            this.listdump.TabIndex = 30;
            this.listdump.Text = "Export DLists";
            this.listdump.UseVisualStyleBackColor = true;
            this.listdump.Click += new System.EventHandler(this.dumpdlists);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.seg4btn);
            this.groupBox2.Controls.Add(this.seg7btn);
            this.groupBox2.Controls.Add(this.seg9btn);
            this.groupBox2.Controls.Add(this.seg6btn);
            this.groupBox2.Location = new System.Drawing.Point(225, 239);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(158, 145);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ROM Data";
            // 
            // seg4btn
            // 
            this.seg4btn.Location = new System.Drawing.Point(12, 19);
            this.seg4btn.Name = "seg4btn";
            this.seg4btn.Size = new System.Drawing.Size(140, 25);
            this.seg4btn.TabIndex = 33;
            this.seg4btn.Text = "Decompress Seg 4";
            this.seg4btn.UseVisualStyleBackColor = true;
            this.seg4btn.Click += new System.EventHandler(this.segment4_decompress);
            // 
            // seg7btn
            // 
            this.seg7btn.Location = new System.Drawing.Point(12, 81);
            this.seg7btn.Name = "seg7btn";
            this.seg7btn.Size = new System.Drawing.Size(140, 25);
            this.seg7btn.TabIndex = 32;
            this.seg7btn.Text = "Dump Seg 7";
            this.seg7btn.UseVisualStyleBackColor = true;
            this.seg7btn.Click += new System.EventHandler(this.segment7_dump);
            // 
            // seg9btn
            // 
            this.seg9btn.Location = new System.Drawing.Point(12, 112);
            this.seg9btn.Name = "seg9btn";
            this.seg9btn.Size = new System.Drawing.Size(140, 25);
            this.seg9btn.TabIndex = 31;
            this.seg9btn.Text = "Dump Seg 9";
            this.seg9btn.UseVisualStyleBackColor = true;
            this.seg9btn.Click += new System.EventHandler(this.Button2_Click);
            // 
            // VertConvert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 406);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.padbox);
            this.Controls.Add(this.flagbox);
            this.Controls.Add(this.texra);
            this.Controls.Add(this.seg7s);
            this.Controls.Add(this.seg7ra);
            this.Controls.Add(this.nv);
            this.Controls.Add(this.seg47ra);
            this.Controls.Add(this.seg9e);
            this.Controls.Add(this.seg9a);
            this.Controls.Add(this.seg7e);
            this.Controls.Add(this.seg4a);
            this.Controls.Add(this.seg6e);
            this.Controls.Add(this.seg6a);
            this.Controls.Add(this.coursebox);
            this.Controls.Add(this.LoadButton);
            this.Name = "VertConvert";
            this.Text = "Level Exporter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button facedump;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.ComboBox coursebox;
        private System.Windows.Forms.TextBox seg6a;
        private System.Windows.Forms.TextBox seg6e;
        private System.Windows.Forms.TextBox seg7e;
        private System.Windows.Forms.TextBox seg4a;
        private System.Windows.Forms.TextBox seg9e;
        private System.Windows.Forms.TextBox seg9a;
        private System.Windows.Forms.TextBox flagbox;
        private System.Windows.Forms.TextBox texra;
        private System.Windows.Forms.TextBox seg7s;
        private System.Windows.Forms.TextBox seg7ra;
        private System.Windows.Forms.TextBox nv;
        private System.Windows.Forms.TextBox seg47ra;
        private System.Windows.Forms.TextBox padbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button vertdump;
        private System.Windows.Forms.Button seg6btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button seg9btn;
        private System.Windows.Forms.Button seg7btn;
        private System.Windows.Forms.Button seg4btn;
        private System.Windows.Forms.Button listdump;
    }
}