
namespace Tarmac64_Retail
{
    partial class LoadBarWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadBarWindow));
            this.LoadingBar = new System.Windows.Forms.ProgressBar();
            this.LoadingLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoadingBar
            // 
            this.LoadingBar.Location = new System.Drawing.Point(12, 12);
            this.LoadingBar.Name = "LoadingBar";
            this.LoadingBar.Size = new System.Drawing.Size(240, 23);
            this.LoadingBar.TabIndex = 0;
            this.LoadingBar.Value = 20;
            // 
            // LoadingLabel
            // 
            this.LoadingLabel.Location = new System.Drawing.Point(12, 41);
            this.LoadingLabel.Name = "LoadingLabel";
            this.LoadingLabel.Size = new System.Drawing.Size(240, 23);
            this.LoadingLabel.TabIndex = 1;
            this.LoadingLabel.Text = "label1";
            this.LoadingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoadBarWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 73);
            this.Controls.Add(this.LoadingLabel);
            this.Controls.Add(this.LoadingBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoadBarWindow";
            this.Text = "TitleName";
            this.Load += new System.EventHandler(this.LoadBarWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ProgressBar LoadingBar;
        public System.Windows.Forms.Label LoadingLabel;
    }
}