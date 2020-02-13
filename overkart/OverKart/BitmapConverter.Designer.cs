namespace OverKart64
{
    partial class BitmapConverter
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
            this.import = new System.Windows.Forms.Button();
            this.codex = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // import
            // 
            this.import.Location = new System.Drawing.Point(12, 37);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(109, 23);
            this.import.TabIndex = 16;
            this.import.Text = "Convert .FBX";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.convert_click);
            // 
            // codex
            // 
            this.codex.FormattingEnabled = true;
            this.codex.Items.AddRange(new object[] {
            "RGBA16",
            "CI8"});
            this.codex.Location = new System.Drawing.Point(12, 12);
            this.codex.Name = "codex";
            this.codex.Size = new System.Drawing.Size(71, 21);
            this.codex.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(89, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Class";
            // 
            // BitmapConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(133, 74);
            this.Controls.Add(this.import);
            this.Controls.Add(this.codex);
            this.Controls.Add(this.label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BitmapConverter";
            this.Text = "Bitmap Converter";
            this.Load += new System.EventHandler(this.TextureLists_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button import;
        private System.Windows.Forms.ComboBox codex;
        private System.Windows.Forms.Label label6;
    }
}