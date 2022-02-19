namespace Tarmac64_Library
{
    partial class AnimationCompiler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationCompiler));
            this.SegmentBox = new System.Windows.Forms.ComboBox();
            this.MagicBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TextureControl = new Tarmac64_Retail.TextureEditor();
            this.MCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SegmentBox
            // 
            this.SegmentBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SegmentBox.FormattingEnabled = true;
            this.SegmentBox.Location = new System.Drawing.Point(93, 11);
            this.SegmentBox.Name = "SegmentBox";
            this.SegmentBox.Size = new System.Drawing.Size(54, 21);
            this.SegmentBox.TabIndex = 1;
            // 
            // MagicBox
            // 
            this.MagicBox.Location = new System.Drawing.Point(153, 11);
            this.MagicBox.Name = "MagicBox";
            this.MagicBox.Size = new System.Drawing.Size(60, 20);
            this.MagicBox.TabIndex = 2;
            this.MagicBox.Text = "0";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = ".FBX";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(251, 11);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 33;
            this.button3.Text = "Build";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.TextureControl);
            this.panel1.Location = new System.Drawing.Point(12, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(314, 588);
            this.panel1.TabIndex = 240;
            // 
            // TextureControl
            // 
            this.TextureControl.AutoScroll = true;
            this.TextureControl.Location = new System.Drawing.Point(3, 3);
            this.TextureControl.Name = "TextureControl";
            this.TextureControl.Size = new System.Drawing.Size(292, 780);
            this.TextureControl.TabIndex = 227;
            // 
            // MCheckBox
            // 
            this.MCheckBox.AutoSize = true;
            this.MCheckBox.Location = new System.Drawing.Point(12, 40);
            this.MCheckBox.Name = "MCheckBox";
            this.MCheckBox.Size = new System.Drawing.Size(154, 17);
            this.MCheckBox.TabIndex = 241;
            this.MCheckBox.Text = "Don\'t Generate Model Lists";
            this.MCheckBox.UseVisualStyleBackColor = true;
            // 
            // AnimationCompiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 661);
            this.Controls.Add(this.MCheckBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.MagicBox);
            this.Controls.Add(this.SegmentBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AnimationCompiler";
            this.Text = "Animation Compiler";
            this.Load += new System.EventHandler(this.ModelCompiler_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox SegmentBox;
        private System.Windows.Forms.TextBox MagicBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel1;
        private Tarmac64_Retail.TextureEditor TextureControl;
        private System.Windows.Forms.CheckBox MCheckBox;
    }
}