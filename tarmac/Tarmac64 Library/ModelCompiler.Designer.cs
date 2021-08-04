namespace Tarmac64_Library
{
    partial class ModelCompiler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelCompiler));
            this.button1 = new System.Windows.Forms.Button();
            this.SegmentBox = new System.Windows.Forms.ComboBox();
            this.MagicBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.FBXBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextureControl = new Tarmac64_Library.Controls.TextureEditor();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Folder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SegmentBox
            // 
            this.SegmentBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SegmentBox.FormattingEnabled = true;
            this.SegmentBox.Location = new System.Drawing.Point(174, 14);
            this.SegmentBox.Name = "SegmentBox";
            this.SegmentBox.Size = new System.Drawing.Size(54, 21);
            this.SegmentBox.TabIndex = 1;
            // 
            // MagicBox
            // 
            this.MagicBox.Location = new System.Drawing.Point(234, 14);
            this.MagicBox.Name = "MagicBox";
            this.MagicBox.Size = new System.Drawing.Size(60, 20);
            this.MagicBox.TabIndex = 2;
            this.MagicBox.Text = "0";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FBXBox
            // 
            this.FBXBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FBXBox.FormattingEnabled = true;
            this.FBXBox.Location = new System.Drawing.Point(55, 43);
            this.FBXBox.Margin = new System.Windows.Forms.Padding(5);
            this.FBXBox.Name = "FBXBox";
            this.FBXBox.Size = new System.Drawing.Size(243, 21);
            this.FBXBox.TabIndex = 28;
            this.FBXBox.SelectedIndexChanged += new System.EventHandler(this.FBXBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "FBX";
            // 
            // TextureControl
            // 
            this.TextureControl.Location = new System.Drawing.Point(12, 72);
            this.TextureControl.Name = "TextureControl";
            this.TextureControl.Size = new System.Drawing.Size(289, 571);
            this.TextureControl.TabIndex = 34;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(221, 649);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 33;
            this.button3.Text = "Build";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // ModelCompiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 681);
            this.Controls.Add(this.TextureControl);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.FBXBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.MagicBox);
            this.Controls.Add(this.SegmentBox);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModelCompiler";
            this.Text = "Model Compiler";
            this.Load += new System.EventHandler(this.ModelCompiler_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox SegmentBox;
        private System.Windows.Forms.TextBox MagicBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox FBXBox;
        private System.Windows.Forms.Label label1;
        private Controls.TextureEditor TextureControl;
        private System.Windows.Forms.Button button3;
    }
}