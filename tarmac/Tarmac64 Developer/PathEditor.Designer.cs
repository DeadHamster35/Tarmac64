namespace Tarmac64
{
    partial class PathEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathEditor));
            this.button1 = new System.Windows.Forms.Button();
            this.coursebox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pathgroupbox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.markerselect = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.fbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pathselect = new System.Windows.Forms.ComboBox();
            this.zbox = new System.Windows.Forms.TextBox();
            this.ybox = new System.Windows.Forms.TextBox();
            this.xbox = new System.Windows.Forms.TextBox();
            this.expbtn = new System.Windows.Forms.Button();
            this.impbtn = new System.Windows.Forms.Button();
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
            this.coursebox.Size = new System.Drawing.Size(204, 21);
            this.coursebox.TabIndex = 1;
            this.coursebox.SelectedIndexChanged += new System.EventHandler(this.Coursebox_SelectedIndexChanged_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.pathgroupbox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.markerselect);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.fbox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pathselect);
            this.groupBox1.Controls.Add(this.zbox);
            this.groupBox1.Controls.Add(this.ybox);
            this.groupBox1.Controls.Add(this.xbox);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 148);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(31, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Path Group";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pathgroupbox
            // 
            this.pathgroupbox.FormattingEnabled = true;
            this.pathgroupbox.Location = new System.Drawing.Point(18, 31);
            this.pathgroupbox.Name = "pathgroupbox";
            this.pathgroupbox.Size = new System.Drawing.Size(100, 21);
            this.pathgroupbox.TabIndex = 2;
            this.pathgroupbox.SelectedIndexChanged += new System.EventHandler(this.Pathgroupbox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(31, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 15);
            this.label5.TabIndex = 23;
            this.label5.Text = "Marker Index";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // markerselect
            // 
            this.markerselect.FormattingEnabled = true;
            this.markerselect.Location = new System.Drawing.Point(18, 114);
            this.markerselect.Name = "markerselect";
            this.markerselect.Size = new System.Drawing.Size(100, 21);
            this.markerselect.TabIndex = 4;
            this.markerselect.SelectedIndexChanged += new System.EventHandler(this.Markerselect_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(145, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Flag";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // fbox
            // 
            this.fbox.Location = new System.Drawing.Point(178, 30);
            this.fbox.Name = "fbox";
            this.fbox.Size = new System.Drawing.Size(100, 20);
            this.fbox.TabIndex = 5;
            this.fbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Fbox_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(159, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Z";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(159, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Y";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(159, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "X";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(31, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Path Index";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pathselect
            // 
            this.pathselect.FormattingEnabled = true;
            this.pathselect.Location = new System.Drawing.Point(18, 72);
            this.pathselect.Name = "pathselect";
            this.pathselect.Size = new System.Drawing.Size(100, 21);
            this.pathselect.TabIndex = 3;
            this.pathselect.SelectedIndexChanged += new System.EventHandler(this.Pathselect_SelectedIndexChanged);
            // 
            // zbox
            // 
            this.zbox.Location = new System.Drawing.Point(179, 114);
            this.zbox.Name = "zbox";
            this.zbox.Size = new System.Drawing.Size(100, 20);
            this.zbox.TabIndex = 8;
            this.zbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Zbox_KeyUp);
            // 
            // ybox
            // 
            this.ybox.Location = new System.Drawing.Point(179, 88);
            this.ybox.Name = "ybox";
            this.ybox.Size = new System.Drawing.Size(100, 20);
            this.ybox.TabIndex = 7;
            this.ybox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Ybox_KeyUp);
            // 
            // xbox
            // 
            this.xbox.Location = new System.Drawing.Point(179, 62);
            this.xbox.Name = "xbox";
            this.xbox.Size = new System.Drawing.Size(100, 20);
            this.xbox.TabIndex = 6;
            this.xbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Xbox_KeyUp);
            // 
            // expbtn
            // 
            this.expbtn.Enabled = false;
            this.expbtn.Location = new System.Drawing.Point(81, 195);
            this.expbtn.Name = "expbtn";
            this.expbtn.Size = new System.Drawing.Size(58, 23);
            this.expbtn.TabIndex = 10;
            this.expbtn.Text = "Export";
            this.expbtn.UseVisualStyleBackColor = true;
            this.expbtn.Click += new System.EventHandler(this.expbtn_Click);
            // 
            // impbtn
            // 
            this.impbtn.Enabled = false;
            this.impbtn.Location = new System.Drawing.Point(21, 195);
            this.impbtn.Name = "impbtn";
            this.impbtn.Size = new System.Drawing.Size(58, 23);
            this.impbtn.TabIndex = 9;
            this.impbtn.Text = "Import";
            this.impbtn.UseVisualStyleBackColor = true;
            this.impbtn.Click += new System.EventHandler(this.impbtn_Click);
            // 
            // PathEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 222);
            this.Controls.Add(this.coursebox);
            this.Controls.Add(this.expbtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.impbtn);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PathEditor";
            this.Text = "Path Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox coursebox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox zbox;
        private System.Windows.Forms.TextBox ybox;
        private System.Windows.Forms.TextBox xbox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox fbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox pathselect;
        private System.Windows.Forms.Button impbtn;
        private System.Windows.Forms.Button expbtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox markerselect;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox pathgroupbox;
    }
}