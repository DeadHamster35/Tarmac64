
namespace Tarmac64_Retail
{
    partial class GhostExtractor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GhostExtractor));
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.ui_ghostCharacterSelect = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ui_ghostSelect = new System.Windows.Forms.ComboBox();
            this.ui_ghostTimeBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(187, 49);
            this.button7.Margin = new System.Windows.Forms.Padding(5);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(80, 21);
            this.button7.TabIndex = 33;
            this.button7.Text = "OK64.Ghost";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(187, 80);
            this.button6.Margin = new System.Windows.Forms.Padding(5);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(80, 21);
            this.button6.TabIndex = 32;
            this.button6.Text = "Export Raw";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // ui_ghostCharacterSelect
            // 
            this.ui_ghostCharacterSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_ghostCharacterSelect.Enabled = false;
            this.ui_ghostCharacterSelect.FormattingEnabled = true;
            this.ui_ghostCharacterSelect.Location = new System.Drawing.Point(14, 77);
            this.ui_ghostCharacterSelect.Margin = new System.Windows.Forms.Padding(5);
            this.ui_ghostCharacterSelect.Name = "ui_ghostCharacterSelect";
            this.ui_ghostCharacterSelect.Size = new System.Drawing.Size(100, 21);
            this.ui_ghostCharacterSelect.TabIndex = 31;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 14);
            this.button1.Margin = new System.Windows.Forms.Padding(5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "Load Ghost";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ui_ghostSelect
            // 
            this.ui_ghostSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_ghostSelect.FormattingEnabled = true;
            this.ui_ghostSelect.Location = new System.Drawing.Point(122, 16);
            this.ui_ghostSelect.Margin = new System.Windows.Forms.Padding(5);
            this.ui_ghostSelect.Name = "ui_ghostSelect";
            this.ui_ghostSelect.Size = new System.Drawing.Size(145, 21);
            this.ui_ghostSelect.TabIndex = 30;
            this.ui_ghostSelect.SelectedIndexChanged += new System.EventHandler(this.ui_ghostSelect_SelectedIndexChanged);
            // 
            // ui_ghostTimeBox
            // 
            this.ui_ghostTimeBox.Enabled = false;
            this.ui_ghostTimeBox.Location = new System.Drawing.Point(14, 47);
            this.ui_ghostTimeBox.Margin = new System.Windows.Forms.Padding(5);
            this.ui_ghostTimeBox.Name = "ui_ghostTimeBox";
            this.ui_ghostTimeBox.Size = new System.Drawing.Size(100, 20);
            this.ui_ghostTimeBox.TabIndex = 26;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(124, 50);
            this.label15.Margin = new System.Windows.Forms.Padding(5);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(30, 13);
            this.label15.TabIndex = 27;
            this.label15.Text = "Time";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(124, 80);
            this.label14.Margin = new System.Windows.Forms.Padding(5);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Character";
            // 
            // GhostExtractor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 111);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.ui_ghostCharacterSelect);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ui_ghostSelect);
            this.Controls.Add(this.ui_ghostTimeBox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(295, 150);
            this.MinimumSize = new System.Drawing.Size(295, 150);
            this.Name = "GhostExtractor";
            this.Text = "Boo - Ghost Extractor";
            this.Load += new System.EventHandler(this.GhostExtractor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ComboBox ui_ghostCharacterSelect;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox ui_ghostSelect;
        private System.Windows.Forms.TextBox ui_ghostTimeBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
    }
}