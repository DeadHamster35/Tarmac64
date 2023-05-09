namespace Tarmac64_Library
{
    partial class GameBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameBuilder));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.KeyBox = new System.Windows.Forms.TextBox();
            this.NameBox = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.HeaderBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "-";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(337, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 56;
            this.button2.Text = "Compile";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // KeyBox
            // 
            this.KeyBox.Location = new System.Drawing.Point(251, 15);
            this.KeyBox.Name = "KeyBox";
            this.KeyBox.Size = new System.Drawing.Size(161, 20);
            this.KeyBox.TabIndex = 57;
            this.KeyBox.Text = "HHHHHHHHHH-HHHHHHHH";
            this.KeyBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NameBox
            // 
            this.NameBox.FormattingEnabled = true;
            this.NameBox.Location = new System.Drawing.Point(74, 14);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(171, 21);
            this.NameBox.TabIndex = 58;
            this.NameBox.SelectedIndexChanged += new System.EventHandler(this.NameBox_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(43, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(25, 23);
            this.button3.TabIndex = 59;
            this.button3.Text = "+";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button1_Click);
            // 
            // HeaderBox
            // 
            this.HeaderBox.Location = new System.Drawing.Point(170, 44);
            this.HeaderBox.Name = "HeaderBox";
            this.HeaderBox.Size = new System.Drawing.Size(161, 20);
            this.HeaderBox.TabIndex = 60;
            this.HeaderBox.Text = "BE9178";
            this.HeaderBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 61;
            this.label1.Text = "Course Header Table";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GameBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 76);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HeaderBox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.KeyBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameBuilder";
            this.Text = "Game Builder";
            this.Load += new System.EventHandler(this.CourseLoader_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox KeyBox;
        private System.Windows.Forms.ComboBox NameBox;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox HeaderBox;
        private System.Windows.Forms.Label label1;
    }
}