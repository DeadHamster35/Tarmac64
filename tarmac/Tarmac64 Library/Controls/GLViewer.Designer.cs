namespace Tarmac64_Library.Controls
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
            this.SettingsPanel = new System.Windows.Forms.Panel();
            this.DistanceBox = new System.Windows.Forms.TextBox();
            this.TargetBox = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.chkPop = new System.Windows.Forms.CheckBox();
            this.chkWireframe = new System.Windows.Forms.CheckBox();
            this.CheckboxHover = new System.Windows.Forms.CheckBox();
            this.CheckboxTextured = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.GLWindow = new SharpGL.OpenGLControl();
            this.SettingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GLWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // SettingsPanel
            // 
            this.SettingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SettingsPanel.Controls.Add(this.DistanceBox);
            this.SettingsPanel.Controls.Add(this.TargetBox);
            this.SettingsPanel.Controls.Add(this.pictureBox2);
            this.SettingsPanel.Controls.Add(this.chkPop);
            this.SettingsPanel.Controls.Add(this.chkWireframe);
            this.SettingsPanel.Controls.Add(this.CheckboxHover);
            this.SettingsPanel.Controls.Add(this.CheckboxTextured);
            this.SettingsPanel.Location = new System.Drawing.Point(6, 476);
            this.SettingsPanel.Name = "SettingsPanel";
            this.SettingsPanel.Padding = new System.Windows.Forms.Padding(3);
            this.SettingsPanel.Size = new System.Drawing.Size(627, 192);
            this.SettingsPanel.TabIndex = 2;
            this.SettingsPanel.Visible = false;
            // 
            // DistanceBox
            // 
            this.DistanceBox.Location = new System.Drawing.Point(65, 6);
            this.DistanceBox.Name = "DistanceBox";
            this.DistanceBox.Size = new System.Drawing.Size(52, 20);
            this.DistanceBox.TabIndex = 107;
            // 
            // TargetBox
            // 
            this.TargetBox.Location = new System.Drawing.Point(6, 6);
            this.TargetBox.Name = "TargetBox";
            this.TargetBox.Size = new System.Drawing.Size(52, 20);
            this.TargetBox.TabIndex = 106;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Image = global::Tarmac64_Library.Properties.Resources.controls2;
            this.pictureBox2.Location = new System.Drawing.Point(123, 4);
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
            this.chkPop.Location = new System.Drawing.Point(6, 169);
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
            this.chkWireframe.Location = new System.Drawing.Point(6, 146);
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
            this.CheckboxHover.Location = new System.Drawing.Point(6, 123);
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
            this.CheckboxTextured.Location = new System.Drawing.Point(6, 100);
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
            this.GLWindow.DrawFPS = false;
            this.GLWindow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.GLWindow.Location = new System.Drawing.Point(3, 3);
            this.GLWindow.Name = "GLWindow";
            this.GLWindow.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL4_4;
            this.GLWindow.RenderContextType = SharpGL.RenderContextType.DIBSection;
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
            // GLViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
        private System.Windows.Forms.TextBox DistanceBox;
        private System.Windows.Forms.TextBox TargetBox;
    }
}
