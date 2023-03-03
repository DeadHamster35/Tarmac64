namespace Tarmac64_Retail
{
    partial class GLViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GLViewer));
            this.SettingsPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.FPSBox = new System.Windows.Forms.TextBox();
            this.RenderCheckbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SpeedBox = new System.Windows.Forms.TextBox();
            this.CheckboxPaths = new System.Windows.Forms.CheckBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.chkPop = new System.Windows.Forms.CheckBox();
            this.chkWireframe = new System.Windows.Forms.CheckBox();
            this.CheckboxHover = new System.Windows.Forms.CheckBox();
            this.CheckboxTextured = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.GLWindow = new SharpGL.OpenGLControl();
            this.FPSDisplay = new System.Windows.Forms.Label();
            this.ScreenshotBtn = new System.Windows.Forms.Button();
            this.SettingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GLWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // SettingsPanel
            // 
            this.SettingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SettingsPanel.Controls.Add(this.label2);
            this.SettingsPanel.Controls.Add(this.FPSBox);
            this.SettingsPanel.Controls.Add(this.RenderCheckbox);
            this.SettingsPanel.Controls.Add(this.label1);
            this.SettingsPanel.Controls.Add(this.SpeedBox);
            this.SettingsPanel.Controls.Add(this.CheckboxPaths);
            this.SettingsPanel.Controls.Add(this.pictureBox2);
            this.SettingsPanel.Controls.Add(this.chkPop);
            this.SettingsPanel.Controls.Add(this.chkWireframe);
            this.SettingsPanel.Controls.Add(this.CheckboxHover);
            this.SettingsPanel.Controls.Add(this.CheckboxTextured);
            this.SettingsPanel.Location = new System.Drawing.Point(6, 472);
            this.SettingsPanel.Name = "SettingsPanel";
            this.SettingsPanel.Padding = new System.Windows.Forms.Padding(3);
            this.SettingsPanel.Size = new System.Drawing.Size(627, 196);
            this.SettingsPanel.TabIndex = 2;
            this.SettingsPanel.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 114;
            this.label2.Text = "FPS";
            // 
            // FPSBox
            // 
            this.FPSBox.Location = new System.Drawing.Point(50, 39);
            this.FPSBox.Name = "FPSBox";
            this.FPSBox.Size = new System.Drawing.Size(67, 20);
            this.FPSBox.TabIndex = 113;
            this.FPSBox.Text = "60";
            this.FPSBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RenderCheckbox
            // 
            this.RenderCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RenderCheckbox.AutoSize = true;
            this.RenderCheckbox.Location = new System.Drawing.Point(6, 172);
            this.RenderCheckbox.Margin = new System.Windows.Forms.Padding(2);
            this.RenderCheckbox.Name = "RenderCheckbox";
            this.RenderCheckbox.Size = new System.Drawing.Size(103, 17);
            this.RenderCheckbox.TabIndex = 112;
            this.RenderCheckbox.TabStop = false;
            this.RenderCheckbox.Text = "Render Screens";
            this.RenderCheckbox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 110;
            this.label1.Text = "Speed";
            // 
            // SpeedBox
            // 
            this.SpeedBox.Enabled = false;
            this.SpeedBox.Location = new System.Drawing.Point(50, 13);
            this.SpeedBox.Name = "SpeedBox";
            this.SpeedBox.Size = new System.Drawing.Size(67, 20);
            this.SpeedBox.TabIndex = 109;
            this.SpeedBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CheckboxPaths
            // 
            this.CheckboxPaths.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckboxPaths.AutoSize = true;
            this.CheckboxPaths.Checked = true;
            this.CheckboxPaths.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckboxPaths.Location = new System.Drawing.Point(6, 88);
            this.CheckboxPaths.Margin = new System.Windows.Forms.Padding(2);
            this.CheckboxPaths.Name = "CheckboxPaths";
            this.CheckboxPaths.Size = new System.Drawing.Size(81, 17);
            this.CheckboxPaths.TabIndex = 108;
            this.CheckboxPaths.TabStop = false;
            this.CheckboxPaths.Text = "Draw Paths";
            this.CheckboxPaths.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(123, 7);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(500, 182);
            this.pictureBox2.TabIndex = 105;
            this.pictureBox2.TabStop = false;
            // 
            // chkPop
            // 
            this.chkPop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkPop.AutoSize = true;
            this.chkPop.Checked = true;
            this.chkPop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPop.Location = new System.Drawing.Point(6, 151);
            this.chkPop.Margin = new System.Windows.Forms.Padding(2);
            this.chkPop.Name = "chkPop";
            this.chkPop.Size = new System.Drawing.Size(102, 17);
            this.chkPop.TabIndex = 3;
            this.chkPop.Text = "Draw POP Data";
            this.chkPop.UseVisualStyleBackColor = true;
            // 
            // chkWireframe
            // 
            this.chkWireframe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkWireframe.AutoSize = true;
            this.chkWireframe.Checked = true;
            this.chkWireframe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWireframe.Location = new System.Drawing.Point(6, 130);
            this.chkWireframe.Margin = new System.Windows.Forms.Padding(2);
            this.chkWireframe.Name = "chkWireframe";
            this.chkWireframe.Size = new System.Drawing.Size(111, 17);
            this.chkWireframe.TabIndex = 2;
            this.chkWireframe.Text = "Wireframe Hidden";
            this.chkWireframe.UseVisualStyleBackColor = true;
            // 
            // CheckboxHover
            // 
            this.CheckboxHover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckboxHover.AutoSize = true;
            this.CheckboxHover.Checked = true;
            this.CheckboxHover.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckboxHover.Location = new System.Drawing.Point(6, 109);
            this.CheckboxHover.Margin = new System.Windows.Forms.Padding(2);
            this.CheckboxHover.Name = "CheckboxHover";
            this.CheckboxHover.Size = new System.Drawing.Size(83, 17);
            this.CheckboxHover.TabIndex = 1;
            this.CheckboxHover.Text = "Flash Hover";
            this.CheckboxHover.UseVisualStyleBackColor = true;
            // 
            // CheckboxTextured
            // 
            this.CheckboxTextured.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckboxTextured.AutoSize = true;
            this.CheckboxTextured.Checked = true;
            this.CheckboxTextured.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckboxTextured.Location = new System.Drawing.Point(6, 67);
            this.CheckboxTextured.Margin = new System.Windows.Forms.Padding(2);
            this.CheckboxTextured.Name = "CheckboxTextured";
            this.CheckboxTextured.Size = new System.Drawing.Size(68, 17);
            this.CheckboxTextured.TabIndex = 0;
            this.CheckboxTextured.TabStop = false;
            this.CheckboxTextured.Text = "Textured";
            this.CheckboxTextured.UseVisualStyleBackColor = true;
            this.CheckboxTextured.CheckedChanged += new System.EventHandler(this.CheckboxTextured_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.BackColor = System.Drawing.Color.Transparent;
            this.button4.Location = new System.Drawing.Point(898, 646);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(23, 23);
            this.button4.TabIndex = 1;
            this.button4.TabStop = false;
            this.button4.Text = "?";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Desktop;
            this.pictureBox1.Location = new System.Drawing.Point(3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(920, 669);
            this.pictureBox1.TabIndex = 98;
            this.pictureBox1.TabStop = false;
            // 
            // GLWindow
            // 
            this.GLWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GLWindow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.GLWindow.Cursor = System.Windows.Forms.Cursors.Cross;
            this.GLWindow.DrawFPS = false;
            this.GLWindow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.GLWindow.FrameRate = 120;
            this.GLWindow.Location = new System.Drawing.Point(3, 3);
            this.GLWindow.Name = "GLWindow";
            this.GLWindow.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.GLWindow.RenderContextType = SharpGL.RenderContextType.NativeWindow;
            this.GLWindow.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.GLWindow.Size = new System.Drawing.Size(922, 668);
            this.GLWindow.TabIndex = 0;
            this.GLWindow.TabStop = false;
            this.GLWindow.OpenGLDraw += new SharpGL.RenderEventHandler(this.GLWindow_OpenGLDraw);
            this.GLWindow.Resized += new System.EventHandler(this.GLWindow_Resized);
            this.GLWindow.Load += new System.EventHandler(this.GLWindow_Load);
            this.GLWindow.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GLWindow_KeyDown);
            this.GLWindow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GLWindow_MouseClick);
            
            this.GLWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GLWindow_MouseMove);
            this.GLWindow.Resize += new System.EventHandler(this.GLWindow_Resized);
            // 
            // FPSDisplay
            // 
            this.FPSDisplay.AutoSize = true;
            this.FPSDisplay.BackColor = System.Drawing.Color.Black;
            this.FPSDisplay.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FPSDisplay.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.FPSDisplay.Location = new System.Drawing.Point(10, 10);
            this.FPSDisplay.Name = "FPSDisplay";
            this.FPSDisplay.Size = new System.Drawing.Size(47, 14);
            this.FPSDisplay.TabIndex = 99;
            this.FPSDisplay.Text = "Infinity";
            // 
            // ScreenshotBtn
            // 
            this.ScreenshotBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScreenshotBtn.BackColor = System.Drawing.Color.Transparent;
            this.ScreenshotBtn.Location = new System.Drawing.Point(869, 646);
            this.ScreenshotBtn.Name = "ScreenshotBtn";
            this.ScreenshotBtn.Size = new System.Drawing.Size(23, 23);
            this.ScreenshotBtn.TabIndex = 100;
            this.ScreenshotBtn.TabStop = false;
            this.ScreenshotBtn.Text = "#";
            this.ScreenshotBtn.UseVisualStyleBackColor = false;
            this.ScreenshotBtn.Click += new System.EventHandler(this.ScreenshotBtn_Click);
            // 
            // GLViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ScreenshotBtn);
            this.Controls.Add(this.FPSDisplay);
            this.Controls.Add(this.SettingsPanel);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.GLWindow);
            this.Controls.Add(this.pictureBox1);
            this.Name = "GLViewer";
            this.Size = new System.Drawing.Size(928, 674);
            this.Load += new System.EventHandler(this.GLViewer_Load);
            this.SettingsPanel.ResumeLayout(false);
            this.SettingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GLWindow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel SettingsPanel;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.CheckBox chkPop;
        private System.Windows.Forms.CheckBox chkWireframe;
        private System.Windows.Forms.CheckBox CheckboxHover;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PictureBox pictureBox1;
        public SharpGL.OpenGLControl GLWindow;
        public System.Windows.Forms.CheckBox CheckboxTextured;
        public System.Windows.Forms.CheckBox CheckboxPaths;
        private System.Windows.Forms.Label FPSDisplay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SpeedBox;
        private System.Windows.Forms.Button ScreenshotBtn;
        public System.Windows.Forms.CheckBox RenderCheckbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FPSBox;
    }
}
