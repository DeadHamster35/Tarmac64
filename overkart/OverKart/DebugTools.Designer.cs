namespace OverKart64
{
    partial class DebugTools
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
            this.leftshiftbtn = new System.Windows.Forms.Button();
            this.input = new System.Windows.Forms.TextBox();
            this.rightshiftbtn = new System.Windows.Forms.Button();
            this.shiftbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.andbtn = new System.Windows.Forms.Button();
            this.logicbox = new System.Windows.Forms.TextBox();
            this.customvert = new System.Windows.Forms.Button();
            this.v2box = new System.Windows.Forms.TextBox();
            this.v1box = new System.Windows.Forms.TextBox();
            this.v0box = new System.Windows.Forms.TextBox();
            this.orbtn = new System.Windows.Forms.Button();
            this.imgbox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.imgbox2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.parabox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.f5out = new System.Windows.Forms.TextBox();
            this.offsetbox = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // leftshiftbtn
            // 
            this.leftshiftbtn.Location = new System.Drawing.Point(9, 57);
            this.leftshiftbtn.Name = "leftshiftbtn";
            this.leftshiftbtn.Size = new System.Drawing.Size(75, 23);
            this.leftshiftbtn.TabIndex = 3;
            this.leftshiftbtn.Text = "Left Shift";
            this.leftshiftbtn.UseVisualStyleBackColor = true;
            this.leftshiftbtn.Click += new System.EventHandler(this.Leftshift_Click);
            // 
            // input
            // 
            this.input.Location = new System.Drawing.Point(9, 34);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(115, 20);
            this.input.TabIndex = 1;
            // 
            // rightshiftbtn
            // 
            this.rightshiftbtn.Location = new System.Drawing.Point(9, 86);
            this.rightshiftbtn.Name = "rightshiftbtn";
            this.rightshiftbtn.Size = new System.Drawing.Size(75, 23);
            this.rightshiftbtn.TabIndex = 4;
            this.rightshiftbtn.Text = "Right Shift";
            this.rightshiftbtn.UseVisualStyleBackColor = true;
            this.rightshiftbtn.Click += new System.EventHandler(this.Rightshiftbtn_Click);
            // 
            // shiftbox
            // 
            this.shiftbox.Location = new System.Drawing.Point(90, 71);
            this.shiftbox.Name = "shiftbox";
            this.shiftbox.Size = new System.Drawing.Size(34, 20);
            this.shiftbox.TabIndex = 2;
            this.shiftbox.Text = "0";
            this.shiftbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 15);
            this.label1.TabIndex = 35;
            this.label1.Text = "Hex String Input";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // andbtn
            // 
            this.andbtn.Location = new System.Drawing.Point(9, 129);
            this.andbtn.Name = "andbtn";
            this.andbtn.Size = new System.Drawing.Size(75, 23);
            this.andbtn.TabIndex = 6;
            this.andbtn.Text = "Logical AND";
            this.andbtn.UseVisualStyleBackColor = true;
            this.andbtn.Click += new System.EventHandler(this.andbtn_Click);
            // 
            // logicbox
            // 
            this.logicbox.Location = new System.Drawing.Point(90, 145);
            this.logicbox.Name = "logicbox";
            this.logicbox.Size = new System.Drawing.Size(88, 20);
            this.logicbox.TabIndex = 5;
            this.logicbox.Text = "0";
            this.logicbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // customvert
            // 
            this.customvert.Location = new System.Drawing.Point(64, 55);
            this.customvert.Name = "customvert";
            this.customvert.Size = new System.Drawing.Size(75, 23);
            this.customvert.TabIndex = 10;
            this.customvert.Text = "0x29 Verts";
            this.customvert.UseVisualStyleBackColor = true;
            this.customvert.Click += new System.EventHandler(this.Custom_Click);
            // 
            // v2box
            // 
            this.v2box.Location = new System.Drawing.Point(18, 83);
            this.v2box.Name = "v2box";
            this.v2box.Size = new System.Drawing.Size(40, 20);
            this.v2box.TabIndex = 9;
            // 
            // v1box
            // 
            this.v1box.Location = new System.Drawing.Point(18, 57);
            this.v1box.Name = "v1box";
            this.v1box.Size = new System.Drawing.Size(40, 20);
            this.v1box.TabIndex = 8;
            // 
            // v0box
            // 
            this.v0box.Location = new System.Drawing.Point(18, 31);
            this.v0box.Name = "v0box";
            this.v0box.Size = new System.Drawing.Size(40, 20);
            this.v0box.TabIndex = 7;
            // 
            // orbtn
            // 
            this.orbtn.Location = new System.Drawing.Point(9, 158);
            this.orbtn.Name = "orbtn";
            this.orbtn.Size = new System.Drawing.Size(75, 23);
            this.orbtn.TabIndex = 36;
            this.orbtn.Text = "Logical OR";
            this.orbtn.UseVisualStyleBackColor = true;
            this.orbtn.Click += new System.EventHandler(this.Orbtn_Click);
            // 
            // imgbox1
            // 
            this.imgbox1.Location = new System.Drawing.Point(18, 124);
            this.imgbox1.Name = "imgbox1";
            this.imgbox1.Size = new System.Drawing.Size(40, 20);
            this.imgbox1.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 150);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "0xF5";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.imgclick);
            // 
            // imgbox2
            // 
            this.imgbox2.Location = new System.Drawing.Point(64, 124);
            this.imgbox2.Name = "imgbox2";
            this.imgbox2.Size = new System.Drawing.Size(40, 20);
            this.imgbox2.TabIndex = 12;
            this.imgbox2.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.leftshiftbtn);
            this.groupBox1.Controls.Add(this.input);
            this.groupBox1.Controls.Add(this.rightshiftbtn);
            this.groupBox1.Controls.Add(this.orbtn);
            this.groupBox1.Controls.Add(this.shiftbox);
            this.groupBox1.Controls.Add(this.andbtn);
            this.groupBox1.Controls.Add(this.logicbox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 208);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bitwise Ops";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.offsetbox);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.parabox);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.f5out);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.customvert);
            this.groupBox2.Controls.Add(this.imgbox2);
            this.groupBox2.Controls.Add(this.v2box);
            this.groupBox2.Controls.Add(this.imgbox1);
            this.groupBox2.Controls.Add(this.v1box);
            this.groupBox2.Controls.Add(this.v0box);
            this.groupBox2.Location = new System.Drawing.Point(209, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(308, 208);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "OK64 Emu";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(64, 150);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 23);
            this.button3.TabIndex = 40;
            this.button3.Text = "0xFD";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // parabox
            // 
            this.parabox.Location = new System.Drawing.Point(169, 31);
            this.parabox.Name = "parabox";
            this.parabox.Size = new System.Drawing.Size(75, 20);
            this.parabox.TabIndex = 37;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(169, 57);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 37;
            this.button2.Text = "Parameter";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // f5out
            // 
            this.f5out.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.f5out.Location = new System.Drawing.Point(6, 179);
            this.f5out.Name = "f5out";
            this.f5out.Size = new System.Drawing.Size(133, 15);
            this.f5out.TabIndex = 14;
            this.f5out.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // offsetbox
            // 
            this.offsetbox.Location = new System.Drawing.Point(146, 137);
            this.offsetbox.Name = "offsetbox";
            this.offsetbox.Size = new System.Drawing.Size(156, 20);
            this.offsetbox.TabIndex = 44;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(227, 108);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 43;
            this.button4.Text = "Decompress";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.export_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(143, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 15);
            this.label2.TabIndex = 37;
            this.label2.Text = "MIO0 Offset";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // DebugTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 229);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DebugTools";
            this.Text = "Debug Tools";
            this.Load += new System.EventHandler(this.DebugTools_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button leftshiftbtn;
        private System.Windows.Forms.TextBox input;
        private System.Windows.Forms.Button rightshiftbtn;
        private System.Windows.Forms.TextBox shiftbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button andbtn;
        private System.Windows.Forms.TextBox logicbox;
        private System.Windows.Forms.Button customvert;
        private System.Windows.Forms.TextBox v2box;
        private System.Windows.Forms.TextBox v1box;
        private System.Windows.Forms.TextBox v0box;
        private System.Windows.Forms.Button orbtn;
        private System.Windows.Forms.TextBox imgbox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox imgbox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox f5out;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox parabox;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox offsetbox;
        private System.Windows.Forms.Button button4;
    }
}