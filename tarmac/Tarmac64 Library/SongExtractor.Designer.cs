﻿namespace Tarmac64_Library
{
    partial class SongExtractor
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bankBox = new System.Windows.Forms.TextBox();
            this.seqBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Export";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Bank ID";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sequence ID";
            // 
            // bankBox
            // 
            this.bankBox.Location = new System.Drawing.Point(120, 14);
            this.bankBox.Name = "bankBox";
            this.bankBox.Size = new System.Drawing.Size(22, 20);
            this.bankBox.TabIndex = 5;
            this.bankBox.Text = "1";
            this.bankBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // seqBox
            // 
            this.seqBox.Location = new System.Drawing.Point(120, 41);
            this.seqBox.Name = "seqBox";
            this.seqBox.Size = new System.Drawing.Size(22, 20);
            this.seqBox.TabIndex = 6;
            this.seqBox.Text = "1";
            this.seqBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SongExtractor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(147, 71);
            this.Controls.Add(this.seqBox);
            this.Controls.Add(this.bankBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "SongExtractor";
            this.Text = "♫";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox bankBox;
        private System.Windows.Forms.TextBox seqBox;
    }
}