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
            this.button1 = new System.Windows.Forms.Button();
            this.SegmentBox = new System.Windows.Forms.ComboBox();
            this.MagicBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Compile";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SegmentBox
            // 
            this.SegmentBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SegmentBox.FormattingEnabled = true;
            this.SegmentBox.Location = new System.Drawing.Point(93, 14);
            this.SegmentBox.Name = "SegmentBox";
            this.SegmentBox.Size = new System.Drawing.Size(54, 21);
            this.SegmentBox.TabIndex = 1;
            // 
            // MagicBox
            // 
            this.MagicBox.Location = new System.Drawing.Point(153, 14);
            this.MagicBox.Name = "MagicBox";
            this.MagicBox.Size = new System.Drawing.Size(117, 20);
            this.MagicBox.TabIndex = 2;
            this.MagicBox.Text = "0";
            // 
            // ModelCompiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 46);
            this.Controls.Add(this.MagicBox);
            this.Controls.Add(this.SegmentBox);
            this.Controls.Add(this.button1);
            this.Name = "ModelCompiler";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.ModelCompiler_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox SegmentBox;
        private System.Windows.Forms.TextBox MagicBox;
    }
}