namespace Tarmac64_Retail
{
    partial class HitboxCompiler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HitboxCompiler));
            this.LoadFBXButton = new System.Windows.Forms.Button();
            this.IndexBox = new System.Windows.Forms.ComboBox();
            this.OriginXBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SizeZBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SizeXBox = new System.Windows.Forms.TextBox();
            this.SizeYBox = new System.Windows.Forms.TextBox();
            this.XLabel = new System.Windows.Forms.Label();
            this.OriginYBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OriginZBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CompileButton = new System.Windows.Forms.Button();
            this.AddBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ScaleBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.DmgResultBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ColResultBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.EffectBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.StatusBox = new System.Windows.Forms.ComboBox();
            this.TypeBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoadFBXButton
            // 
            this.LoadFBXButton.Enabled = false;
            this.LoadFBXButton.Location = new System.Drawing.Point(12, 10);
            this.LoadFBXButton.Name = "LoadFBXButton";
            this.LoadFBXButton.Size = new System.Drawing.Size(75, 23);
            this.LoadFBXButton.TabIndex = 0;
            this.LoadFBXButton.Text = "Load .FBX";
            this.LoadFBXButton.UseVisualStyleBackColor = true;
            this.LoadFBXButton.Click += new System.EventHandler(this.LoadFBXClick);
            // 
            // IndexBox
            // 
            this.IndexBox.FormattingEnabled = true;
            this.IndexBox.Location = new System.Drawing.Point(12, 41);
            this.IndexBox.Name = "IndexBox";
            this.IndexBox.Size = new System.Drawing.Size(231, 21);
            this.IndexBox.TabIndex = 2;
            this.IndexBox.SelectedIndexChanged += new System.EventHandler(this.IndexBox_SelectedIndexChanged);
            // 
            // OriginXBox
            // 
            this.OriginXBox.Location = new System.Drawing.Point(12, 203);
            this.OriginXBox.Name = "OriginXBox";
            this.OriginXBox.Size = new System.Drawing.Size(84, 20);
            this.OriginXBox.TabIndex = 14;
            this.OriginXBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateHBData);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 226);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(281, 202);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 20);
            this.label9.TabIndex = 16;
            this.label9.Text = "Offset";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(281, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Size";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(192, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Z";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SizeZBox
            // 
            this.SizeZBox.Location = new System.Drawing.Point(192, 157);
            this.SizeZBox.Name = "SizeZBox";
            this.SizeZBox.Size = new System.Drawing.Size(84, 20);
            this.SizeZBox.TabIndex = 13;
            this.SizeZBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateHBData);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(102, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 20);
            this.label5.TabIndex = 12;
            this.label5.Text = "Y";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SizeXBox
            // 
            this.SizeXBox.Location = new System.Drawing.Point(12, 157);
            this.SizeXBox.Name = "SizeXBox";
            this.SizeXBox.Size = new System.Drawing.Size(84, 20);
            this.SizeXBox.TabIndex = 11;
            this.SizeXBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateHBData);
            // 
            // SizeYBox
            // 
            this.SizeYBox.Location = new System.Drawing.Point(102, 157);
            this.SizeYBox.Name = "SizeYBox";
            this.SizeYBox.Size = new System.Drawing.Size(84, 20);
            this.SizeYBox.TabIndex = 12;
            this.SizeYBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateHBData);
            // 
            // XLabel
            // 
            this.XLabel.Location = new System.Drawing.Point(12, 180);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(84, 20);
            this.XLabel.TabIndex = 10;
            this.XLabel.Text = "X";
            this.XLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OriginYBox
            // 
            this.OriginYBox.Location = new System.Drawing.Point(102, 203);
            this.OriginYBox.Name = "OriginYBox";
            this.OriginYBox.Size = new System.Drawing.Size(84, 20);
            this.OriginYBox.TabIndex = 15;
            this.OriginYBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateHBData);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(102, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OriginZBox
            // 
            this.OriginZBox.Location = new System.Drawing.Point(192, 203);
            this.OriginZBox.Name = "OriginZBox";
            this.OriginZBox.Size = new System.Drawing.Size(84, 20);
            this.OriginZBox.TabIndex = 16;
            this.OriginZBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateHBData);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(192, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Z";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CompileButton
            // 
            this.CompileButton.Location = new System.Drawing.Point(249, 10);
            this.CompileButton.Name = "CompileButton";
            this.CompileButton.Size = new System.Drawing.Size(75, 23);
            this.CompileButton.TabIndex = 17;
            this.CompileButton.Text = "Compile";
            this.CompileButton.UseVisualStyleBackColor = true;
            this.CompileButton.Click += new System.EventHandler(this.CompileClick);
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(249, 39);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(34, 23);
            this.AddBtn.TabIndex = 3;
            this.AddBtn.Text = "+";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Location = new System.Drawing.Point(290, 39);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(34, 23);
            this.DeleteBtn.TabIndex = 4;
            this.DeleteBtn.Text = "-";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(95, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(103, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Animated Hitbox";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(118, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 310;
            this.label8.Text = "Scale";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ScaleBox
            // 
            this.ScaleBox.Location = new System.Drawing.Point(12, 122);
            this.ScaleBox.Name = "ScaleBox";
            this.ScaleBox.Size = new System.Drawing.Size(100, 20);
            this.ScaleBox.TabIndex = 9;
            this.ScaleBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ScaleBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateHBData);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(281, 98);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(47, 13);
            this.label19.TabIndex = 308;
            this.label19.Text = "Damage";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DmgResultBox
            // 
            this.DmgResultBox.FormattingEnabled = true;
            this.DmgResultBox.Location = new System.Drawing.Point(175, 95);
            this.DmgResultBox.Name = "DmgResultBox";
            this.DmgResultBox.Size = new System.Drawing.Size(100, 21);
            this.DmgResultBox.TabIndex = 8;
            this.DmgResultBox.SelectedIndexChanged += new System.EventHandler(this.TypeBox_SelectedIndexChanged_1);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(118, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 307;
            this.label7.Text = "Collide";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ColResultBox
            // 
            this.ColResultBox.FormattingEnabled = true;
            this.ColResultBox.Location = new System.Drawing.Point(12, 95);
            this.ColResultBox.Name = "ColResultBox";
            this.ColResultBox.Size = new System.Drawing.Size(100, 21);
            this.ColResultBox.TabIndex = 7;
            this.ColResultBox.SelectedIndexChanged += new System.EventHandler(this.TypeBox_SelectedIndexChanged_1);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(281, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 304;
            this.label11.Text = "Effect";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EffectBox
            // 
            this.EffectBox.FormattingEnabled = true;
            this.EffectBox.Location = new System.Drawing.Point(175, 68);
            this.EffectBox.Name = "EffectBox";
            this.EffectBox.Size = new System.Drawing.Size(100, 21);
            this.EffectBox.TabIndex = 6;
            this.EffectBox.SelectedIndexChanged += new System.EventHandler(this.TypeBox_SelectedIndexChanged_1);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(118, 71);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 303;
            this.label10.Text = "Status";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusBox
            // 
            this.StatusBox.FormattingEnabled = true;
            this.StatusBox.Location = new System.Drawing.Point(12, 68);
            this.StatusBox.Name = "StatusBox";
            this.StatusBox.Size = new System.Drawing.Size(100, 21);
            this.StatusBox.TabIndex = 5;
            this.StatusBox.SelectedIndexChanged += new System.EventHandler(this.TypeBox_SelectedIndexChanged_1);
            // 
            // TypeBox
            // 
            this.TypeBox.FormattingEnabled = true;
            this.TypeBox.Location = new System.Drawing.Point(175, 122);
            this.TypeBox.Name = "TypeBox";
            this.TypeBox.Size = new System.Drawing.Size(100, 21);
            this.TypeBox.TabIndex = 10;
            this.TypeBox.SelectedIndexChanged += new System.EventHandler(this.TypeBox_SelectedIndexChanged_1);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(281, 125);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 311;
            this.label12.Text = "Type";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HitboxCompiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 252);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TypeBox);
            this.Controls.Add(this.SizeZBox);
            this.Controls.Add(this.ScaleBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.SizeXBox);
            this.Controls.Add(this.SizeYBox);
            this.Controls.Add(this.DmgResultBox);
            this.Controls.Add(this.XLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.OriginXBox);
            this.Controls.Add(this.ColResultBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.OriginYBox);
            this.Controls.Add(this.EffectBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.OriginZBox);
            this.Controls.Add(this.StatusBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.CompileButton);
            this.Controls.Add(this.IndexBox);
            this.Controls.Add(this.LoadFBXButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HitboxCompiler";
            this.Text = "Hitbox Compiler";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadFBXButton;
        private System.Windows.Forms.ComboBox IndexBox;
        private System.Windows.Forms.TextBox OriginXBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SizeZBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SizeXBox;
        private System.Windows.Forms.TextBox SizeYBox;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox OriginYBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox OriginZBox;
        private System.Windows.Forms.Button CompileButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button DeleteBtn;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ScaleBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox DmgResultBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ColResultBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox EffectBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox StatusBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox TypeBox;
        private System.Windows.Forms.Label label12;
    }
}