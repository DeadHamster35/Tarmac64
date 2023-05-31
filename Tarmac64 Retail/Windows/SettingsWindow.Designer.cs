namespace Tarmac64_Retail
{
    partial class SettingsWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.CourseDIRBox = new System.Windows.Forms.TextBox();
            this.ObjectDIRBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CourseDIRButton = new System.Windows.Forms.Button();
            this.ObjectDIRButton = new System.Windows.Forms.Button();
            this.ScaleBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AlphaBox = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(252, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Course Models";
            // 
            // CourseDIRBox
            // 
            this.CourseDIRBox.Location = new System.Drawing.Point(55, 16);
            this.CourseDIRBox.Margin = new System.Windows.Forms.Padding(5);
            this.CourseDIRBox.Name = "CourseDIRBox";
            this.CourseDIRBox.Size = new System.Drawing.Size(187, 20);
            this.CourseDIRBox.TabIndex = 1;
            // 
            // ObjectDIRBox
            // 
            this.ObjectDIRBox.Location = new System.Drawing.Point(55, 46);
            this.ObjectDIRBox.Margin = new System.Windows.Forms.Padding(5);
            this.ObjectDIRBox.Name = "ObjectDIRBox";
            this.ObjectDIRBox.Size = new System.Drawing.Size(187, 20);
            this.ObjectDIRBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(252, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Object Models";
            // 
            // CourseDIRButton
            // 
            this.CourseDIRButton.Location = new System.Drawing.Point(12, 15);
            this.CourseDIRButton.Name = "CourseDIRButton";
            this.CourseDIRButton.Size = new System.Drawing.Size(35, 23);
            this.CourseDIRButton.TabIndex = 4;
            this.CourseDIRButton.Text = "...";
            this.CourseDIRButton.UseVisualStyleBackColor = true;
            this.CourseDIRButton.Click += new System.EventHandler(this.CourseDIRButton_Click);
            // 
            // ObjectDIRButton
            // 
            this.ObjectDIRButton.Location = new System.Drawing.Point(12, 45);
            this.ObjectDIRButton.Name = "ObjectDIRButton";
            this.ObjectDIRButton.Size = new System.Drawing.Size(35, 23);
            this.ObjectDIRButton.TabIndex = 5;
            this.ObjectDIRButton.Text = "...";
            this.ObjectDIRButton.UseVisualStyleBackColor = true;
            this.ObjectDIRButton.Click += new System.EventHandler(this.ObjectDIRButton_Click);
            // 
            // ScaleBox
            // 
            this.ScaleBox.Location = new System.Drawing.Point(14, 86);
            this.ScaleBox.Margin = new System.Windows.Forms.Padding(5);
            this.ScaleBox.Name = "ScaleBox";
            this.ScaleBox.Size = new System.Drawing.Size(64, 20);
            this.ScaleBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(88, 89);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Import Scale";
            // 
            // AlphaBox
            // 
            this.AlphaBox.AutoSize = true;
            this.AlphaBox.Location = new System.Drawing.Point(175, 88);
            this.AlphaBox.Name = "AlphaBox";
            this.AlphaBox.Size = new System.Drawing.Size(143, 17);
            this.AlphaBox.TabIndex = 8;
            this.AlphaBox.Text = "Vertex Alpha - Channel 2";
            this.AlphaBox.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(258, 125);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 156);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.AlphaBox);
            this.Controls.Add(this.ScaleBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ObjectDIRButton);
            this.Controls.Add(this.CourseDIRButton);
            this.Controls.Add(this.ObjectDIRBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CourseDIRBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsWindow";
            this.Text = "Tarmac64 Settings";
            this.Load += new System.EventHandler(this.SettingsWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CourseDIRBox;
        private System.Windows.Forms.TextBox ObjectDIRBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CourseDIRButton;
        private System.Windows.Forms.Button ObjectDIRButton;
        private System.Windows.Forms.TextBox ScaleBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox AlphaBox;
        private System.Windows.Forms.Button button3;
    }
}