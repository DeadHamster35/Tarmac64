namespace OverKart64
{
    partial class MIO0
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.offsetbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(151, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Export";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.export_Click);
            // 
            // offsetbox
            // 
            this.offsetbox.Location = new System.Drawing.Point(12, 12);
            this.offsetbox.Name = "offsetbox";
            this.offsetbox.Size = new System.Drawing.Size(133, 20);
            this.offsetbox.TabIndex = 1;
            // 
            // MIO0
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 43);
            this.Controls.Add(this.offsetbox);
            this.Controls.Add(this.button1);
            this.Name = "MIO0";
            this.Text = "MI0O Exporter";
            this.Load += new System.EventHandler(this.Textures_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox offsetbox;
    }
}