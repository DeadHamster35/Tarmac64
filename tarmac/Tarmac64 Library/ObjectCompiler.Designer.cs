namespace Tarmac64_Library
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
            this.courseBox = new System.Windows.Forms.ComboBox();
            this.actionBtn = new System.Windows.Forms.Button();
            this.cupBox = new System.Windows.Forms.ComboBox();
            this.setBox = new System.Windows.Forms.ComboBox();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.cyBox = new System.Windows.Forms.TextBox();
            this.czBox = new System.Windows.Forms.TextBox();
            this.cxBox = new System.Windows.Forms.TextBox();
            this.about_button = new System.Windows.Forms.Button();
            this.csBox = new System.Windows.Forms.TextBox();
            this.cName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.CheckboxTextured = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.raycastBox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label59 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label62 = new System.Windows.Forms.Label();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.tcountBox = new System.Windows.Forms.TextBox();
            this.mcountBox = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tBox = new System.Windows.Forms.CheckBox();
            this.textureBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bitm = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bitm)).BeginInit();
            this.SuspendLayout();
            // 
            // courseBox
            // 
            this.courseBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.courseBox.FormattingEnabled = true;
            this.courseBox.Items.AddRange(new object[] {
            "Course 1",
            "Course 2",
            "Course 3",
            "Course 4"});
            this.courseBox.Location = new System.Drawing.Point(218, 649);
            this.courseBox.Name = "courseBox";
            this.courseBox.Size = new System.Drawing.Size(92, 21);
            this.courseBox.TabIndex = 34;
            // 
            // actionBtn
            // 
            this.actionBtn.AutoSize = true;
            this.actionBtn.Location = new System.Drawing.Point(14, 13);
            this.actionBtn.Name = "actionBtn";
            this.actionBtn.Size = new System.Drawing.Size(67, 23);
            this.actionBtn.TabIndex = 19;
            this.actionBtn.Text = "Load";
            this.actionBtn.UseVisualStyleBackColor = true;
            this.actionBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // cupBox
            // 
            this.cupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cupBox.FormattingEnabled = true;
            this.cupBox.Items.AddRange(new object[] {
            "Mushroom Cup",
            "Flower Cup",
            "Star Cup",
            "Special Cup"});
            this.cupBox.Location = new System.Drawing.Point(91, 649);
            this.cupBox.Name = "cupBox";
            this.cupBox.Size = new System.Drawing.Size(121, 21);
            this.cupBox.TabIndex = 36;
            // 
            // setBox
            // 
            this.setBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setBox.FormattingEnabled = true;
            this.setBox.Items.AddRange(new object[] {
            "Set A",
            "Set B",
            "Set C",
            "Set D"});
            this.setBox.Location = new System.Drawing.Point(15, 649);
            this.setBox.Name = "setBox";
            this.setBox.Size = new System.Drawing.Size(70, 21);
            this.setBox.TabIndex = 37;
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
            this.openGLControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openGLControl_KeyDown);
            this.openGLControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseMove);
            this.openGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseMove);
            // 
            // cyBox
            // 
            this.cyBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cyBox.Location = new System.Drawing.Point(375, 640);
            this.cyBox.Margin = new System.Windows.Forms.Padding(5, 8, 5, 5);
            this.cyBox.Name = "cyBox";
            this.cyBox.Size = new System.Drawing.Size(70, 20);
            this.cyBox.TabIndex = 84;
            // 
            // czBox
            // 
            this.czBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.czBox.Location = new System.Drawing.Point(455, 640);
            this.czBox.Margin = new System.Windows.Forms.Padding(5, 8, 5, 5);
            this.czBox.Name = "czBox";
            this.czBox.Size = new System.Drawing.Size(70, 20);
            this.czBox.TabIndex = 83;
            // 
            // cxBox
            // 
            this.cxBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cxBox.Location = new System.Drawing.Point(295, 640);
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
            this.csBox.Location = new System.Drawing.Point(535, 640);
            this.csBox.Margin = new System.Windows.Forms.Padding(5, 8, 5, 5);
            this.csBox.Name = "csBox";
            this.csBox.Size = new System.Drawing.Size(70, 20);
            this.csBox.TabIndex = 91;
            // 
            // cName
            // 
            this.cName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cName.Location = new System.Drawing.Point(84, 640);
            this.cName.Margin = new System.Windows.Forms.Padding(5, 8, 5, 5);
            this.cName.Name = "cName";
            this.cName.Size = new System.Drawing.Size(188, 20);
            this.cName.TabIndex = 92;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.CheckboxTextured);
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
            // CheckboxTextured
            // 
            this.CheckboxTextured.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckboxTextured.AutoSize = true;
            this.CheckboxTextured.Location = new System.Drawing.Point(8, 642);
            this.CheckboxTextured.Name = "CheckboxTextured";
            this.CheckboxTextured.Size = new System.Drawing.Size(68, 17);
            this.CheckboxTextured.TabIndex = 94;
            this.CheckboxTextured.Text = "Textured";
            this.CheckboxTextured.UseVisualStyleBackColor = true;
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
            // raycastBox
            // 
            this.raycastBox.AutoSize = true;
            this.raycastBox.Location = new System.Drawing.Point(85, 17);
            this.raycastBox.Name = "raycastBox";
            this.raycastBox.Size = new System.Drawing.Size(65, 17);
            this.raycastBox.TabIndex = 94;
            this.raycastBox.Text = "Raycast";
            this.raycastBox.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.label59);
            this.panel1.Controls.Add(this.label60);
            this.panel1.Controls.Add(this.label61);
            this.panel1.Controls.Add(this.textBox8);
            this.panel1.Controls.Add(this.textBox9);
            this.panel1.Controls.Add(this.label62);
            this.panel1.Controls.Add(this.textBox11);
            this.panel1.Controls.Add(this.label54);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.textBox5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label33);
            this.panel1.Controls.Add(this.tcountBox);
            this.panel1.Controls.Add(this.mcountBox);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.bitm);
            this.panel1.Location = new System.Drawing.Point(12, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(314, 601);
            this.panel1.TabIndex = 95;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label59.Location = new System.Drawing.Point(18, 456);
            this.label59.Margin = new System.Windows.Forms.Padding(8, 5, 8, 4);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(46, 13);
            this.label59.TabIndex = 219;
            this.label59.Text = "Rotation";
            this.label59.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label60.Location = new System.Drawing.Point(241, 481);
            this.label60.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(13, 13);
            this.label60.TabIndex = 218;
            this.label60.Text = "Z";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label61.Location = new System.Drawing.Point(72, 481);
            this.label61.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(14, 13);
            this.label61.TabIndex = 217;
            this.label61.Text = "X";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(99, 478);
            this.textBox8.Margin = new System.Windows.Forms.Padding(5);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(50, 20);
            this.textBox8.TabIndex = 214;
            this.textBox8.Text = "0";
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(14, 478);
            this.textBox9.Margin = new System.Windows.Forms.Padding(5);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(50, 20);
            this.textBox9.TabIndex = 213;
            this.textBox9.Text = "0";
            this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label62.Location = new System.Drawing.Point(157, 481);
            this.label62.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(14, 13);
            this.label62.TabIndex = 216;
            this.label62.Text = "Y";
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(183, 478);
            this.textBox11.Margin = new System.Windows.Forms.Padding(5);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(50, 20);
            this.textBox11.TabIndex = 215;
            this.textBox11.Text = "0";
            this.textBox11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label54.Location = new System.Drawing.Point(217, 511);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(59, 13);
            this.label54.TabIndex = 212;
            this.label54.Text = "OK64.POP";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(186, 506);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(25, 23);
            this.button2.TabIndex = 211;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(14, 508);
            this.textBox5.Margin = new System.Windows.Forms.Padding(5);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(164, 20);
            this.textBox5.TabIndex = 210;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(227, 400);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 140;
            this.label6.Text = "Height";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(144, 397);
            this.textBox2.Margin = new System.Windows.Forms.Padding(5);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(75, 20);
            this.textBox2.TabIndex = 139;
            this.textBox2.Text = "5.5";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Bump",
            "Slip",
            "Squish",
            "Explode",
            "Shrink",
            "Star",
            "Boost",
            "Ghost"});
            this.comboBox1.Location = new System.Drawing.Point(14, 427);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(164, 21);
            this.comboBox1.TabIndex = 138;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(188, 432);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 137;
            this.label2.Text = "Collision Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(97, 400);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 136;
            this.label1.Text = "Radius";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 397);
            this.textBox1.Margin = new System.Windows.Forms.Padding(5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(75, 20);
            this.textBox1.TabIndex = 135;
            this.textBox1.Text = "5.5";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(197, 12);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(55, 13);
            this.label33.TabIndex = 131;
            this.label33.Text = "# Textures";
            // 
            // tcountBox
            // 
            this.tcountBox.Enabled = false;
            this.tcountBox.Location = new System.Drawing.Point(145, 9);
            this.tcountBox.Name = "tcountBox";
            this.tcountBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tcountBox.Size = new System.Drawing.Size(46, 20);
            this.tcountBox.TabIndex = 130;
            this.tcountBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // mcountBox
            // 
            this.mcountBox.Enabled = false;
            this.mcountBox.Location = new System.Drawing.Point(29, 9);
            this.mcountBox.Name = "mcountBox";
            this.mcountBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mcountBox.Size = new System.Drawing.Size(46, 20);
            this.mcountBox.TabIndex = 128;
            this.mcountBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(81, 12);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(58, 13);
            this.label22.TabIndex = 129;
            this.label22.Text = "# Materials";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.tBox);
            this.groupBox5.Controls.Add(this.textureBox);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Location = new System.Drawing.Point(5, 312);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(281, 77);
            this.groupBox5.TabIndex = 127;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Material ";
            // 
            // tBox
            // 
            this.tBox.AutoSize = true;
            this.tBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.tBox.Location = new System.Drawing.Point(191, 50);
            this.tBox.Margin = new System.Windows.Forms.Padding(5);
            this.tBox.Name = "tBox";
            this.tBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tBox.Size = new System.Drawing.Size(81, 17);
            this.tBox.TabIndex = 12;
            this.tBox.Text = "Transparent";
            this.tBox.UseVisualStyleBackColor = true;
            // 
            // textureBox
            // 
            this.textureBox.FormattingEnabled = true;
            this.textureBox.Location = new System.Drawing.Point(43, 19);
            this.textureBox.Margin = new System.Windows.Forms.Padding(5);
            this.textureBox.Name = "textureBox";
            this.textureBox.Size = new System.Drawing.Size(229, 21);
            this.textureBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "ID #";
            // 
            // bitm
            // 
            this.bitm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.bitm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bitm.Location = new System.Drawing.Point(5, 35);
            this.bitm.Margin = new System.Windows.Forms.Padding(5);
            this.bitm.Name = "bitm";
            this.bitm.Size = new System.Drawing.Size(280, 271);
            this.bitm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bitm.TabIndex = 126;
            this.bitm.TabStop = false;
            // 
            // ObjectCompiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 682);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.raycastBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.about_button);
            this.Controls.Add(this.setBox);
            this.Controls.Add(this.cupBox);
            this.Controls.Add(this.courseBox);
            this.Controls.Add(this.actionBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "ObjectCompiler";
            this.Text = "Tarmac64";
            this.Load += new System.EventHandler(this.FormLoad);
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
        private System.Windows.Forms.ComboBox courseBox;
        private System.Windows.Forms.Button actionBtn;
        private System.Windows.Forms.ComboBox cupBox;
        private System.Windows.Forms.ComboBox setBox;
        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.TextBox cyBox;
        private System.Windows.Forms.TextBox czBox;
        private System.Windows.Forms.TextBox cxBox;
        private System.Windows.Forms.Button about_button;
        private System.Windows.Forms.TextBox csBox;
        private System.Windows.Forms.TextBox cName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox raycastBox;
        private System.Windows.Forms.CheckBox CheckboxTextured;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox tcountBox;
        private System.Windows.Forms.TextBox mcountBox;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox tBox;
        private System.Windows.Forms.ComboBox textureBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox bitm;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox5;
    }
}