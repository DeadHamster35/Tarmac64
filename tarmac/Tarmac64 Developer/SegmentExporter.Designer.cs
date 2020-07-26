namespace Tarmac64
{
    partial class SegmentExporter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SegmentExporter));
            this.proc7btn = new System.Windows.Forms.Button();
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
            this.proc4btn = new System.Windows.Forms.Button();
            this.seg6btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.surfacemapbtn = new System.Windows.Forms.Button();
            this.proc9btn = new System.Windows.Forms.Button();
            this.proc6btn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.seg5btn = new System.Windows.Forms.Button();
            this.cseg4btn = new System.Windows.Forms.Button();
            this.cseg7btn = new System.Windows.Forms.Button();
            this.cseg6btn = new System.Windows.Forms.Button();
            this.asmbtn = new System.Windows.Forms.Button();
            this.dumpasmbtn = new System.Windows.Forms.Button();
            this.dump4btn = new System.Windows.Forms.Button();
            this.dump6btn = new System.Windows.Forms.Button();
            this.dump7btn = new System.Windows.Forms.Button();
            this.seg4btn = new System.Windows.Forms.Button();
            this.seg7btn = new System.Windows.Forms.Button();
            this.dump9btn = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.seg7rom = new System.Windows.Forms.TextBox();
            this.headerBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // proc7btn
            // 
            this.proc7btn.Enabled = false;
            this.proc7btn.Location = new System.Drawing.Point(9, 81);
            this.proc7btn.Name = "proc7btn";
            this.proc7btn.Size = new System.Drawing.Size(140, 25);
            this.proc7btn.TabIndex = 18;
            this.proc7btn.Text = "Process Seg 7 - (Faces)";
            this.proc7btn.UseVisualStyleBackColor = true;
            this.proc7btn.Click += new System.EventHandler(this.dumpface2);
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
            this.coursebox.Location = new System.Drawing.Point(112, 14);
            this.coursebox.Name = "coursebox";
            this.coursebox.Size = new System.Drawing.Size(158, 21);
            this.coursebox.TabIndex = 2;
            this.coursebox.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // seg6a
            // 
            this.seg6a.Location = new System.Drawing.Point(170, 40);
            this.seg6a.Name = "seg6a";
            this.seg6a.Size = new System.Drawing.Size(100, 20);
            this.seg6a.TabIndex = 3;
            this.seg6a.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // seg6e
            // 
            this.seg6e.Location = new System.Drawing.Point(170, 66);
            this.seg6e.Name = "seg6e";
            this.seg6e.Size = new System.Drawing.Size(100, 20);
            this.seg6e.TabIndex = 4;
            this.seg6e.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // seg7e
            // 
            this.seg7e.Location = new System.Drawing.Point(170, 118);
            this.seg7e.Name = "seg7e";
            this.seg7e.Size = new System.Drawing.Size(100, 20);
            this.seg7e.TabIndex = 6;
            this.seg7e.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // seg4a
            // 
            this.seg4a.Location = new System.Drawing.Point(170, 92);
            this.seg4a.Name = "seg4a";
            this.seg4a.Size = new System.Drawing.Size(100, 20);
            this.seg4a.TabIndex = 5;
            this.seg4a.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // seg9e
            // 
            this.seg9e.Location = new System.Drawing.Point(355, 40);
            this.seg9e.Name = "seg9e";
            this.seg9e.Size = new System.Drawing.Size(100, 20);
            this.seg9e.TabIndex = 8;
            this.seg9e.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // seg9a
            // 
            this.seg9a.Location = new System.Drawing.Point(355, 14);
            this.seg9a.Name = "seg9a";
            this.seg9a.Size = new System.Drawing.Size(100, 20);
            this.seg9a.TabIndex = 7;
            this.seg9a.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // flagbox
            // 
            this.flagbox.Location = new System.Drawing.Point(540, 66);
            this.flagbox.Name = "flagbox";
            this.flagbox.Size = new System.Drawing.Size(100, 20);
            this.flagbox.TabIndex = 14;
            this.flagbox.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // texra
            // 
            this.texra.Location = new System.Drawing.Point(540, 40);
            this.texra.Name = "texra";
            this.texra.Size = new System.Drawing.Size(100, 20);
            this.texra.TabIndex = 13;
            this.texra.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // seg7s
            // 
            this.seg7s.Location = new System.Drawing.Point(540, 14);
            this.seg7s.Name = "seg7s";
            this.seg7s.Size = new System.Drawing.Size(100, 20);
            this.seg7s.TabIndex = 12;
            this.seg7s.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // seg7ra
            // 
            this.seg7ra.Location = new System.Drawing.Point(355, 118);
            this.seg7ra.Name = "seg7ra";
            this.seg7ra.Size = new System.Drawing.Size(100, 20);
            this.seg7ra.TabIndex = 11;
            this.seg7ra.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // nv
            // 
            this.nv.Location = new System.Drawing.Point(355, 92);
            this.nv.Name = "nv";
            this.nv.Size = new System.Drawing.Size(100, 20);
            this.nv.TabIndex = 10;
            this.nv.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // seg47ra
            // 
            this.seg47ra.Location = new System.Drawing.Point(355, 66);
            this.seg47ra.Name = "seg47ra";
            this.seg47ra.Size = new System.Drawing.Size(100, 20);
            this.seg47ra.TabIndex = 9;
            this.seg47ra.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // padbox
            // 
            this.padbox.Location = new System.Drawing.Point(540, 92);
            this.padbox.Name = "padbox";
            this.padbox.Size = new System.Drawing.Size(100, 20);
            this.padbox.TabIndex = 15;
            this.padbox.TextChanged += new System.EventHandler(this.textbox_change);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(272, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 16;
            this.label1.Text = "Seg4-7 RSP@";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(272, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "# Vertices";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(477, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 15);
            this.label3.TabIndex = 19;
            this.label3.Text = "Seg7 Size";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(272, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "Seg7 RSP@";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(484, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 15);
            this.label5.TabIndex = 21;
            this.label5.Text = "? Flag";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(484, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 15);
            this.label6.TabIndex = 20;
            this.label6.Text = "Textures RSP@";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(295, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 15);
            this.label7.TabIndex = 27;
            this.label7.Text = "Seg9 End";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(294, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 15);
            this.label8.TabIndex = 26;
            this.label8.Text = "Seg9 @";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(109, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 15);
            this.label9.TabIndex = 25;
            this.label9.Text = "Seg7 End";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(109, 94);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 15);
            this.label10.TabIndex = 24;
            this.label10.Text = "Seg4 @";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(109, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 15);
            this.label11.TabIndex = 23;
            this.label11.Text = "Seg6 End";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(109, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 15);
            this.label12.TabIndex = 22;
            this.label12.Text = "Seg6 @";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(484, 94);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 15);
            this.label13.TabIndex = 28;
            this.label13.Text = "? Padding";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // proc4btn
            // 
            this.proc4btn.Enabled = false;
            this.proc4btn.Location = new System.Drawing.Point(9, 19);
            this.proc4btn.Name = "proc4btn";
            this.proc4btn.Size = new System.Drawing.Size(140, 25);
            this.proc4btn.TabIndex = 16;
            this.proc4btn.Text = "Process Seg 4 - (Verts)";
            this.proc4btn.UseVisualStyleBackColor = true;
            this.proc4btn.Click += new System.EventHandler(this.DumpVerts);
            // 
            // seg6btn
            // 
            this.seg6btn.Enabled = false;
            this.seg6btn.Location = new System.Drawing.Point(174, 50);
            this.seg6btn.Name = "seg6btn";
            this.seg6btn.Size = new System.Drawing.Size(140, 25);
            this.seg6btn.TabIndex = 26;
            this.seg6btn.Text = "Decompress Seg 6";
            this.seg6btn.UseVisualStyleBackColor = true;
            this.seg6btn.Click += new System.EventHandler(this.Segment6_decompress);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.surfacemapbtn);
            this.groupBox1.Controls.Add(this.proc9btn);
            this.groupBox1.Controls.Add(this.proc6btn);
            this.groupBox1.Controls.Add(this.proc7btn);
            this.groupBox1.Controls.Add(this.proc4btn);
            this.groupBox1.Location = new System.Drawing.Point(11, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 175);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Level Geometry";
            // 
            // surfacemapbtn
            // 
            this.surfacemapbtn.Enabled = false;
            this.surfacemapbtn.Location = new System.Drawing.Point(9, 143);
            this.surfacemapbtn.Name = "surfacemapbtn";
            this.surfacemapbtn.Size = new System.Drawing.Size(140, 25);
            this.surfacemapbtn.TabIndex = 20;
            this.surfacemapbtn.Text = "Export Surface Maps";
            this.surfacemapbtn.UseVisualStyleBackColor = true;
            this.surfacemapbtn.Click += new System.EventHandler(this.sfcbtn_click);
            // 
            // proc9btn
            // 
            this.proc9btn.Enabled = false;
            this.proc9btn.Location = new System.Drawing.Point(9, 112);
            this.proc9btn.Name = "proc9btn";
            this.proc9btn.Size = new System.Drawing.Size(140, 25);
            this.proc9btn.TabIndex = 19;
            this.proc9btn.Text = "Process Seg 9 - (Textures)";
            this.proc9btn.UseVisualStyleBackColor = true;
            this.proc9btn.Click += new System.EventHandler(this.Proc9btn_Click);
            // 
            // proc6btn
            // 
            this.proc6btn.Enabled = false;
            this.proc6btn.Location = new System.Drawing.Point(9, 50);
            this.proc6btn.Name = "proc6btn";
            this.proc6btn.Size = new System.Drawing.Size(140, 25);
            this.proc6btn.TabIndex = 17;
            this.proc6btn.Text = "Process Seg 6 - (Regions)";
            this.proc6btn.UseVisualStyleBackColor = true;
            this.proc6btn.Click += new System.EventHandler(this.dumpdlists);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.seg5btn);
            this.groupBox2.Controls.Add(this.cseg4btn);
            this.groupBox2.Controls.Add(this.cseg7btn);
            this.groupBox2.Controls.Add(this.cseg6btn);
            this.groupBox2.Controls.Add(this.asmbtn);
            this.groupBox2.Controls.Add(this.dumpasmbtn);
            this.groupBox2.Controls.Add(this.dump4btn);
            this.groupBox2.Controls.Add(this.dump6btn);
            this.groupBox2.Controls.Add(this.dump7btn);
            this.groupBox2.Controls.Add(this.seg4btn);
            this.groupBox2.Controls.Add(this.seg7btn);
            this.groupBox2.Controls.Add(this.seg6btn);
            this.groupBox2.Controls.Add(this.dump9btn);
            this.groupBox2.Location = new System.Drawing.Point(175, 153);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(465, 175);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ROM Data";
            // 
            // seg5btn
            // 
            this.seg5btn.Enabled = false;
            this.seg5btn.Location = new System.Drawing.Point(174, 112);
            this.seg5btn.Name = "seg5btn";
            this.seg5btn.Size = new System.Drawing.Size(140, 25);
            this.seg5btn.TabIndex = 36;
            this.seg5btn.Text = "Decompress Seg 5";
            this.seg5btn.UseVisualStyleBackColor = true;
            this.seg5btn.Click += new System.EventHandler(this.Seg5btn_Click);
            // 
            // cseg4btn
            // 
            this.cseg4btn.Enabled = false;
            this.cseg4btn.Location = new System.Drawing.Point(320, 19);
            this.cseg4btn.Name = "cseg4btn";
            this.cseg4btn.Size = new System.Drawing.Size(140, 25);
            this.cseg4btn.TabIndex = 30;
            this.cseg4btn.Text = "Compress Seg 4";
            this.cseg4btn.UseVisualStyleBackColor = true;
            this.cseg4btn.Click += new System.EventHandler(this.cseg4btn_click);
            // 
            // cseg7btn
            // 
            this.cseg7btn.Enabled = false;
            this.cseg7btn.Location = new System.Drawing.Point(320, 81);
            this.cseg7btn.Name = "cseg7btn";
            this.cseg7btn.Size = new System.Drawing.Size(140, 25);
            this.cseg7btn.TabIndex = 32;
            this.cseg7btn.Text = "Compress Seg 7";
            this.cseg7btn.UseVisualStyleBackColor = true;
            this.cseg7btn.Click += new System.EventHandler(this.cseg7btn_click);
            // 
            // cseg6btn
            // 
            this.cseg6btn.Enabled = false;
            this.cseg6btn.Location = new System.Drawing.Point(320, 50);
            this.cseg6btn.Name = "cseg6btn";
            this.cseg6btn.Size = new System.Drawing.Size(140, 25);
            this.cseg6btn.TabIndex = 31;
            this.cseg6btn.Text = "Compress Seg 6";
            this.cseg6btn.UseVisualStyleBackColor = true;
            this.cseg6btn.Click += new System.EventHandler(this.cseg6btn_click);
            // 
            // asmbtn
            // 
            this.asmbtn.Enabled = false;
            this.asmbtn.Location = new System.Drawing.Point(174, 143);
            this.asmbtn.Name = "asmbtn";
            this.asmbtn.Size = new System.Drawing.Size(140, 25);
            this.asmbtn.TabIndex = 28;
            this.asmbtn.Text = "Process ASM";
            this.asmbtn.UseVisualStyleBackColor = true;
            this.asmbtn.Click += new System.EventHandler(this.procasmbtn);
            // 
            // dumpasmbtn
            // 
            this.dumpasmbtn.Enabled = false;
            this.dumpasmbtn.Location = new System.Drawing.Point(6, 143);
            this.dumpasmbtn.Name = "dumpasmbtn";
            this.dumpasmbtn.Size = new System.Drawing.Size(140, 25);
            this.dumpasmbtn.TabIndex = 24;
            this.dumpasmbtn.Text = "Dump ASM";
            this.dumpasmbtn.UseVisualStyleBackColor = true;
            this.dumpasmbtn.Click += new System.EventHandler(this.dumpasm_btn);
            // 
            // dump4btn
            // 
            this.dump4btn.Enabled = false;
            this.dump4btn.Location = new System.Drawing.Point(6, 19);
            this.dump4btn.Name = "dump4btn";
            this.dump4btn.Size = new System.Drawing.Size(140, 25);
            this.dump4btn.TabIndex = 20;
            this.dump4btn.Text = "Dump Seg 4";
            this.dump4btn.UseVisualStyleBackColor = true;
            this.dump4btn.Click += new System.EventHandler(this.dump4_btn);
            // 
            // dump6btn
            // 
            this.dump6btn.Enabled = false;
            this.dump6btn.Location = new System.Drawing.Point(6, 50);
            this.dump6btn.Name = "dump6btn";
            this.dump6btn.Size = new System.Drawing.Size(140, 25);
            this.dump6btn.TabIndex = 21;
            this.dump6btn.Text = "Dump Seg 6";
            this.dump6btn.UseVisualStyleBackColor = true;
            this.dump6btn.Click += new System.EventHandler(this.dump6_btn);
            // 
            // dump7btn
            // 
            this.dump7btn.Enabled = false;
            this.dump7btn.Location = new System.Drawing.Point(6, 81);
            this.dump7btn.Name = "dump7btn";
            this.dump7btn.Size = new System.Drawing.Size(140, 25);
            this.dump7btn.TabIndex = 22;
            this.dump7btn.Text = "Dump Seg 7";
            this.dump7btn.UseVisualStyleBackColor = true;
            this.dump7btn.Click += new System.EventHandler(this.dump7_btn);
            // 
            // seg4btn
            // 
            this.seg4btn.Enabled = false;
            this.seg4btn.Location = new System.Drawing.Point(174, 19);
            this.seg4btn.Name = "seg4btn";
            this.seg4btn.Size = new System.Drawing.Size(140, 25);
            this.seg4btn.TabIndex = 25;
            this.seg4btn.Text = "Decompress Seg 4";
            this.seg4btn.UseVisualStyleBackColor = true;
            this.seg4btn.Click += new System.EventHandler(this.segment4_decompress);
            // 
            // seg7btn
            // 
            this.seg7btn.Enabled = false;
            this.seg7btn.Location = new System.Drawing.Point(174, 81);
            this.seg7btn.Name = "seg7btn";
            this.seg7btn.Size = new System.Drawing.Size(140, 25);
            this.seg7btn.TabIndex = 27;
            this.seg7btn.Text = "Decompress Seg 7";
            this.seg7btn.UseVisualStyleBackColor = true;
            this.seg7btn.Click += new System.EventHandler(this.segment7_decomp);
            // 
            // dump9btn
            // 
            this.dump9btn.Enabled = false;
            this.dump9btn.Location = new System.Drawing.Point(6, 112);
            this.dump9btn.Name = "dump9btn";
            this.dump9btn.Size = new System.Drawing.Size(140, 25);
            this.dump9btn.TabIndex = 23;
            this.dump9btn.Text = "Dump Seg 9";
            this.dump9btn.UseVisualStyleBackColor = true;
            this.dump9btn.Click += new System.EventHandler(this.dump9_btn);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(457, 120);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(78, 15);
            this.label14.TabIndex = 34;
            this.label14.Text = "Seg7 @";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // seg7rom
            // 
            this.seg7rom.Location = new System.Drawing.Point(540, 118);
            this.seg7rom.Name = "seg7rom";
            this.seg7rom.Size = new System.Drawing.Size(100, 20);
            this.seg7rom.TabIndex = 33;
            // 
            // headerBox
            // 
            this.headerBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.headerBox.FormattingEnabled = true;
            this.headerBox.Items.AddRange(new object[] {
            "Stock",
            "Set 1",
            "Set 2",
            "Set 3",
            "Set 4"});
            this.headerBox.Location = new System.Drawing.Point(12, 66);
            this.headerBox.Name = "headerBox";
            this.headerBox.Size = new System.Drawing.Size(75, 21);
            this.headerBox.TabIndex = 35;
            // 
            // SegmentExporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 335);
            this.Controls.Add(this.headerBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.seg7rom);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SegmentExporter";
            this.Text = "Segment Exporter";
            this.Load += new System.EventHandler(this.SegmentExporter_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button proc7btn;
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
        private System.Windows.Forms.Button proc4btn;
        private System.Windows.Forms.Button seg6btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button dump9btn;
        private System.Windows.Forms.Button seg7btn;
        private System.Windows.Forms.Button seg4btn;
        private System.Windows.Forms.Button proc6btn;
        private System.Windows.Forms.Button proc9btn;
        private System.Windows.Forms.Button dump7btn;
        private System.Windows.Forms.Button dump4btn;
        private System.Windows.Forms.Button dump6btn;
        private System.Windows.Forms.Button dumpasmbtn;
        private System.Windows.Forms.Button asmbtn;
        private System.Windows.Forms.Button surfacemapbtn;
        private System.Windows.Forms.Button cseg4btn;
        private System.Windows.Forms.Button cseg7btn;
        private System.Windows.Forms.Button cseg6btn;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox seg7rom;
        private System.Windows.Forms.Button seg5btn;
        private System.Windows.Forms.ComboBox headerBox;
    }
}