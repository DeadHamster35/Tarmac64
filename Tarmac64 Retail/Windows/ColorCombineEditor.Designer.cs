namespace OverKart64_Retail.Windows
{
    partial class ColorCombineEditor
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
            this.ColorA = new System.Windows.Forms.ComboBox();
            this.AlphaD = new System.Windows.Forms.ComboBox();
            this.AlphaC = new System.Windows.Forms.ComboBox();
            this.AlphaB = new System.Windows.Forms.ComboBox();
            this.AlphaA = new System.Windows.Forms.ComboBox();
            this.ColorD = new System.Windows.Forms.ComboBox();
            this.ColorC = new System.Windows.Forms.ComboBox();
            this.ColorB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(584, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "( A - B ) * C + D";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ColorA
            // 
            this.ColorA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorA.FormattingEnabled = true;
            this.ColorA.Location = new System.Drawing.Point(14, 74);
            this.ColorA.Margin = new System.Windows.Forms.Padding(5);
            this.ColorA.Name = "ColorA";
            this.ColorA.Size = new System.Drawing.Size(125, 21);
            this.ColorA.TabIndex = 1;
            this.ColorA.SelectedIndexChanged += new System.EventHandler(this.MasterSelectedIndexChanged);
            // 
            // AlphaD
            // 
            this.AlphaD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AlphaD.FormattingEnabled = true;
            this.AlphaD.Location = new System.Drawing.Point(419, 105);
            this.AlphaD.Margin = new System.Windows.Forms.Padding(5);
            this.AlphaD.Name = "AlphaD";
            this.AlphaD.Size = new System.Drawing.Size(125, 21);
            this.AlphaD.TabIndex = 8;
            this.AlphaD.SelectedIndexChanged += new System.EventHandler(this.MasterSelectedIndexChanged);
            // 
            // AlphaC
            // 
            this.AlphaC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AlphaC.FormattingEnabled = true;
            this.AlphaC.Location = new System.Drawing.Point(284, 105);
            this.AlphaC.Margin = new System.Windows.Forms.Padding(5);
            this.AlphaC.Name = "AlphaC";
            this.AlphaC.Size = new System.Drawing.Size(125, 21);
            this.AlphaC.TabIndex = 7;
            this.AlphaC.SelectedIndexChanged += new System.EventHandler(this.MasterSelectedIndexChanged);
            // 
            // AlphaB
            // 
            this.AlphaB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AlphaB.FormattingEnabled = true;
            this.AlphaB.Location = new System.Drawing.Point(149, 105);
            this.AlphaB.Margin = new System.Windows.Forms.Padding(5);
            this.AlphaB.Name = "AlphaB";
            this.AlphaB.Size = new System.Drawing.Size(125, 21);
            this.AlphaB.TabIndex = 6;
            this.AlphaB.SelectedIndexChanged += new System.EventHandler(this.MasterSelectedIndexChanged);
            // 
            // AlphaA
            // 
            this.AlphaA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AlphaA.FormattingEnabled = true;
            this.AlphaA.Location = new System.Drawing.Point(14, 105);
            this.AlphaA.Margin = new System.Windows.Forms.Padding(5);
            this.AlphaA.Name = "AlphaA";
            this.AlphaA.Size = new System.Drawing.Size(125, 21);
            this.AlphaA.TabIndex = 5;
            this.AlphaA.SelectedIndexChanged += new System.EventHandler(this.MasterSelectedIndexChanged);
            // 
            // ColorD
            // 
            this.ColorD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorD.FormattingEnabled = true;
            this.ColorD.Location = new System.Drawing.Point(419, 74);
            this.ColorD.Margin = new System.Windows.Forms.Padding(5);
            this.ColorD.Name = "ColorD";
            this.ColorD.Size = new System.Drawing.Size(125, 21);
            this.ColorD.TabIndex = 11;
            this.ColorD.SelectedIndexChanged += new System.EventHandler(this.MasterSelectedIndexChanged);
            // 
            // ColorC
            // 
            this.ColorC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorC.FormattingEnabled = true;
            this.ColorC.Location = new System.Drawing.Point(284, 74);
            this.ColorC.Margin = new System.Windows.Forms.Padding(5);
            this.ColorC.Name = "ColorC";
            this.ColorC.Size = new System.Drawing.Size(125, 21);
            this.ColorC.TabIndex = 10;
            this.ColorC.SelectedIndexChanged += new System.EventHandler(this.MasterSelectedIndexChanged);
            // 
            // ColorB
            // 
            this.ColorB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorB.FormattingEnabled = true;
            this.ColorB.Location = new System.Drawing.Point(149, 74);
            this.ColorB.Margin = new System.Windows.Forms.Padding(5);
            this.ColorB.Name = "ColorB";
            this.ColorB.Size = new System.Drawing.Size(125, 21);
            this.ColorB.TabIndex = 9;
            this.ColorB.SelectedIndexChanged += new System.EventHandler(this.MasterSelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(552, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Color";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(552, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Alpha";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(516, 136);
            this.button1.Margin = new System.Windows.Forms.Padding(5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Confirm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(431, 136);
            this.button2.Margin = new System.Windows.Forms.Padding(5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ColorCombiner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 170);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ColorD);
            this.Controls.Add(this.ColorC);
            this.Controls.Add(this.ColorB);
            this.Controls.Add(this.AlphaD);
            this.Controls.Add(this.AlphaC);
            this.Controls.Add(this.AlphaB);
            this.Controls.Add(this.AlphaA);
            this.Controls.Add(this.ColorA);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ColorCombiner";
            this.Text = "Color Combiner";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ColorA;
        private System.Windows.Forms.ComboBox AlphaD;
        private System.Windows.Forms.ComboBox AlphaC;
        private System.Windows.Forms.ComboBox AlphaB;
        private System.Windows.Forms.ComboBox AlphaA;
        private System.Windows.Forms.ComboBox ColorD;
        private System.Windows.Forms.ComboBox ColorC;
        private System.Windows.Forms.ComboBox ColorB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}