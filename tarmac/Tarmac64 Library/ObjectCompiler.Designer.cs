namespace Tarmac64
{
    partial class ObjectCompiler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectCompiler));
            this.actionBtn = new System.Windows.Forms.Button();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.cyBox = new System.Windows.Forms.TextBox();
            this.czBox = new System.Windows.Forms.TextBox();
            this.cxBox = new System.Windows.Forms.TextBox();
            this.about_button = new System.Windows.Forms.Button();
            this.csBox = new System.Windows.Forms.TextBox();
            this.cName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tBox = new System.Windows.Forms.CheckBox();
            this.classBox = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.textureBox = new System.Windows.Forms.ComboBox();
            this.formatBox = new System.Windows.Forms.ComboBox();
            this.heightBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.widthBox = new System.Windows.Forms.TextBox();
            this.bitm = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.surfaceobjectBox = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bitm)).BeginInit();
            this.SuspendLayout();
            // 
            // actionBtn
            // 
            this.actionBtn.AutoSize = true;
            this.actionBtn.Location = new System.Drawing.Point(12, 13);
            this.actionBtn.Name = "actionBtn";
            this.actionBtn.Size = new System.Drawing.Size(67, 23);
            this.actionBtn.TabIndex = 19;
            this.actionBtn.Text = "Load";
            this.actionBtn.UseVisualStyleBackColor = true;
            this.actionBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // openGLControl
            // 
            this.openGLControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openGLControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.openGLControl.DrawFPS = false;
            this.openGLControl.Enabled = false;
            this.openGLControl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.openGLControl.Location = new System.Drawing.Point(8, 19);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(904, 616);
            this.openGLControl.TabIndex = 39;
            this.openGLControl.Visible = false;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.Load += new System.EventHandler(this.OpenGLControl_Load);
            this.openGLControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openGLControl_KeyDown);
            // 
            // cyBox
            // 
            this.cyBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cyBox.Location = new System.Drawing.Point(88, 640);
            this.cyBox.Margin = new System.Windows.Forms.Padding(5, 8, 5, 5);
            this.cyBox.Name = "cyBox";
            this.cyBox.Size = new System.Drawing.Size(70, 20);
            this.cyBox.TabIndex = 84;
            // 
            // czBox
            // 
            this.czBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.czBox.Location = new System.Drawing.Point(168, 640);
            this.czBox.Margin = new System.Windows.Forms.Padding(5, 8, 5, 5);
            this.czBox.Name = "czBox";
            this.czBox.Size = new System.Drawing.Size(70, 20);
            this.czBox.TabIndex = 83;
            // 
            // cxBox
            // 
            this.cxBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cxBox.Location = new System.Drawing.Point(8, 640);
            this.cxBox.Margin = new System.Windows.Forms.Padding(5, 8, 5, 5);
            this.cxBox.Name = "cxBox";
            this.cxBox.Size = new System.Drawing.Size(70, 20);
            this.cxBox.TabIndex = 82;
            // 
            // about_button
            // 
            this.about_button.Location = new System.Drawing.Point(252, 13);
            this.about_button.Name = "about_button";
            this.about_button.Size = new System.Drawing.Size(54, 23);
            this.about_button.TabIndex = 88;
            this.about_button.Text = "About";
            this.about_button.UseVisualStyleBackColor = true;
            this.about_button.Click += new System.EventHandler(this.Button1_Click);
            // 
            // csBox
            // 
            this.csBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.csBox.Location = new System.Drawing.Point(248, 640);
            this.csBox.Margin = new System.Windows.Forms.Padding(5, 8, 5, 5);
            this.csBox.Name = "csBox";
            this.csBox.Size = new System.Drawing.Size(70, 20);
            this.csBox.TabIndex = 91;
            // 
            // cName
            // 
            this.cName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cName.Location = new System.Drawing.Point(716, 640);
            this.cName.Margin = new System.Windows.Forms.Padding(5, 8, 5, 5);
            this.cName.Name = "cName";
            this.cName.Size = new System.Drawing.Size(196, 20);
            this.cName.TabIndex = 92;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.openGLControl);
            this.groupBox3.Controls.Add(this.cName);
            this.groupBox3.Controls.Add(this.csBox);
            this.groupBox3.Controls.Add(this.cxBox);
            this.groupBox3.Controls.Add(this.czBox);
            this.groupBox3.Controls.Add(this.cyBox);
            this.groupBox3.Controls.Add(this.pictureBox1);
            this.groupBox3.Location = new System.Drawing.Point(332, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(920, 668);
            this.groupBox3.TabIndex = 93;
            this.groupBox3.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Desktop;
            this.pictureBox1.Location = new System.Drawing.Point(8, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(900, 616);
            this.pictureBox1.TabIndex = 93;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Controls.Add(this.surfaceobjectBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.bitm);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Location = new System.Drawing.Point(12, 57);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(8);
            this.panel1.Size = new System.Drawing.Size(314, 613);
            this.panel1.TabIndex = 94;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(17, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(211, 20);
            this.textBox1.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.tBox);
            this.groupBox5.Controls.Add(this.classBox);
            this.groupBox5.Controls.Add(this.label24);
            this.groupBox5.Controls.Add(this.textureBox);
            this.groupBox5.Controls.Add(this.formatBox);
            this.groupBox5.Controls.Add(this.heightBox);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.widthBox);
            this.groupBox5.Location = new System.Drawing.Point(17, 487);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(280, 115);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Material ";
            // 
            // tBox
            // 
            this.tBox.AutoSize = true;
            this.tBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.tBox.Location = new System.Drawing.Point(189, 21);
            this.tBox.Name = "tBox";
            this.tBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tBox.Size = new System.Drawing.Size(81, 17);
            this.tBox.TabIndex = 12;
            this.tBox.Text = "Transparent";
            this.tBox.UseVisualStyleBackColor = true;
            // 
            // classBox
            // 
            this.classBox.Enabled = false;
            this.classBox.FormattingEnabled = true;
            this.classBox.Items.AddRange(new object[] {
            "Class 0",
            "Class 1",
            "Class 2",
            "Class 3",
            "Class 4",
            "Class 5"});
            this.classBox.Location = new System.Drawing.Point(196, 58);
            this.classBox.Name = "classBox";
            this.classBox.Size = new System.Drawing.Size(75, 21);
            this.classBox.TabIndex = 11;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(158, 61);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(32, 13);
            this.label24.TabIndex = 10;
            this.label24.Text = "Class";
            // 
            // textureBox
            // 
            this.textureBox.FormattingEnabled = true;
            this.textureBox.Location = new System.Drawing.Point(39, 19);
            this.textureBox.Name = "textureBox";
            this.textureBox.Size = new System.Drawing.Size(144, 21);
            this.textureBox.TabIndex = 1;
            // 
            // formatBox
            // 
            this.formatBox.Enabled = false;
            this.formatBox.FormattingEnabled = true;
            this.formatBox.Items.AddRange(new object[] {
            "RGBA16",
            "CI8"});
            this.formatBox.Location = new System.Drawing.Point(65, 58);
            this.formatBox.Name = "formatBox";
            this.formatBox.Size = new System.Drawing.Size(75, 21);
            this.formatBox.TabIndex = 9;
            // 
            // heightBox
            // 
            this.heightBox.Enabled = false;
            this.heightBox.Location = new System.Drawing.Point(66, 85);
            this.heightBox.Name = "heightBox";
            this.heightBox.Size = new System.Drawing.Size(75, 20);
            this.heightBox.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(154, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Width";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(21, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Format";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Height";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "ID #";
            // 
            // widthBox
            // 
            this.widthBox.Enabled = false;
            this.widthBox.Location = new System.Drawing.Point(195, 85);
            this.widthBox.Name = "widthBox";
            this.widthBox.Size = new System.Drawing.Size(75, 20);
            this.widthBox.TabIndex = 3;
            // 
            // bitm
            // 
            this.bitm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bitm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bitm.Location = new System.Drawing.Point(17, 201);
            this.bitm.Name = "bitm";
            this.bitm.Size = new System.Drawing.Size(280, 280);
            this.bitm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bitm.TabIndex = 11;
            this.bitm.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(234, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Size Radius";
            // 
            // surfaceobjectBox
            // 
            this.surfaceobjectBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.surfaceobjectBox.FormattingEnabled = true;
            this.surfaceobjectBox.Location = new System.Drawing.Point(16, 48);
            this.surfaceobjectBox.Name = "surfaceobjectBox";
            this.surfaceobjectBox.Size = new System.Drawing.Size(281, 147);
            this.surfaceobjectBox.TabIndex = 19;
            // 
            // ObjectCompiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 682);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.about_button);
            this.Controls.Add(this.actionBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "ObjectCompiler";
            this.Text = "Tarmac64";
            this.Load += new System.EventHandler(this.GeometryCompiler_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openGLControl_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bitm)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button actionBtn;
        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.TextBox cyBox;
        private System.Windows.Forms.TextBox czBox;
        private System.Windows.Forms.TextBox cxBox;
        private System.Windows.Forms.Button about_button;
        private System.Windows.Forms.TextBox csBox;
        private System.Windows.Forms.TextBox cName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox tBox;
        private System.Windows.Forms.ComboBox classBox;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox textureBox;
        private System.Windows.Forms.ComboBox formatBox;
        private System.Windows.Forms.TextBox heightBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox widthBox;
        private System.Windows.Forms.PictureBox bitm;
        private System.Windows.Forms.ListBox surfaceobjectBox;
    }
}