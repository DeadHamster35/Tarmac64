namespace Tarmac64_Library.Controls
{
    partial class ObjectEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.KillObjBtn = new System.Windows.Forms.Button();
            this.AddObjBtn = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.KillTypeBtn = new System.Windows.Forms.Button();
            this.label65 = new System.Windows.Forms.Label();
            this.RotationYBox = new System.Windows.Forms.TextBox();
            this.RotationXBox = new System.Windows.Forms.TextBox();
            this.RotationZBox = new System.Windows.Forms.TextBox();
            this.label64 = new System.Windows.Forms.Label();
            this.VelocityYBox = new System.Windows.Forms.TextBox();
            this.VelocityXBox = new System.Windows.Forms.TextBox();
            this.VelocityZBox = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.AngleYBox = new System.Windows.Forms.TextBox();
            this.AngleXBox = new System.Windows.Forms.TextBox();
            this.AngleZBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.AddTypeBtn = new System.Windows.Forms.Button();
            this.label75 = new System.Windows.Forms.Label();
            this.ObjectIndexBox = new System.Windows.Forms.ComboBox();
            this.label53 = new System.Windows.Forms.Label();
            this.LocationYBox = new System.Windows.Forms.TextBox();
            this.LocationXBox = new System.Windows.Forms.TextBox();
            this.LocationZBox = new System.Windows.Forms.TextBox();
            this.ObjectListBox = new System.Windows.Forms.ListBox();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // KillObjBtn
            // 
            this.KillObjBtn.Location = new System.Drawing.Point(259, 57);
            this.KillObjBtn.Name = "KillObjBtn";
            this.KillObjBtn.Size = new System.Drawing.Size(25, 23);
            this.KillObjBtn.TabIndex = 2;
            this.KillObjBtn.Text = "-";
            this.KillObjBtn.UseVisualStyleBackColor = true;
            this.KillObjBtn.Click += new System.EventHandler(this.KillObjBtn_Click);
            // 
            // AddObjBtn
            // 
            this.AddObjBtn.Location = new System.Drawing.Point(259, 28);
            this.AddObjBtn.Name = "AddObjBtn";
            this.AddObjBtn.Size = new System.Drawing.Size(25, 23);
            this.AddObjBtn.TabIndex = 1;
            this.AddObjBtn.Text = "+";
            this.AddObjBtn.UseVisualStyleBackColor = true;
            this.AddObjBtn.Click += new System.EventHandler(this.AddObjBtn_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox8.Controls.Add(this.KillTypeBtn);
            this.groupBox8.Controls.Add(this.label65);
            this.groupBox8.Controls.Add(this.RotationYBox);
            this.groupBox8.Controls.Add(this.RotationXBox);
            this.groupBox8.Controls.Add(this.RotationZBox);
            this.groupBox8.Controls.Add(this.label64);
            this.groupBox8.Controls.Add(this.VelocityYBox);
            this.groupBox8.Controls.Add(this.VelocityXBox);
            this.groupBox8.Controls.Add(this.VelocityZBox);
            this.groupBox8.Controls.Add(this.label54);
            this.groupBox8.Controls.Add(this.AngleYBox);
            this.groupBox8.Controls.Add(this.AngleXBox);
            this.groupBox8.Controls.Add(this.AngleZBox);
            this.groupBox8.Controls.Add(this.label6);
            this.groupBox8.Controls.Add(this.label24);
            this.groupBox8.Controls.Add(this.label52);
            this.groupBox8.Controls.Add(this.AddTypeBtn);
            this.groupBox8.Controls.Add(this.label75);
            this.groupBox8.Controls.Add(this.ObjectIndexBox);
            this.groupBox8.Controls.Add(this.label53);
            this.groupBox8.Controls.Add(this.LocationYBox);
            this.groupBox8.Controls.Add(this.LocationXBox);
            this.groupBox8.Controls.Add(this.LocationZBox);
            this.groupBox8.Location = new System.Drawing.Point(5, 365);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.groupBox8.Size = new System.Drawing.Size(279, 192);
            this.groupBox8.TabIndex = 3;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Object Information";
            // 
            // KillTypeBtn
            // 
            this.KillTypeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.KillTypeBtn.Location = new System.Drawing.Point(236, 21);
            this.KillTypeBtn.Name = "KillTypeBtn";
            this.KillTypeBtn.Size = new System.Drawing.Size(25, 23);
            this.KillTypeBtn.TabIndex = 2;
            this.KillTypeBtn.Text = "-";
            this.KillTypeBtn.UseVisualStyleBackColor = true;
            this.KillTypeBtn.Click += new System.EventHandler(this.KillTypeBtn_Click);
            // 
            // label65
            // 
            this.label65.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label65.AutoSize = true;
            this.label65.Cursor = System.Windows.Forms.Cursors.Default;
            this.label65.Location = new System.Drawing.Point(221, 147);
            this.label65.Margin = new System.Windows.Forms.Padding(5);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(47, 13);
            this.label65.TabIndex = 231;
            this.label65.Text = "Rotation";
            // 
            // RotationYBox
            // 
            this.RotationYBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RotationYBox.Location = new System.Drawing.Point(81, 144);
            this.RotationYBox.Margin = new System.Windows.Forms.Padding(5);
            this.RotationYBox.Name = "RotationYBox";
            this.RotationYBox.Size = new System.Drawing.Size(60, 20);
            this.RotationYBox.TabIndex = 13;
            this.RotationYBox.Text = "0";
            this.RotationYBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RotationYBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // RotationXBox
            // 
            this.RotationXBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RotationXBox.Location = new System.Drawing.Point(11, 144);
            this.RotationXBox.Margin = new System.Windows.Forms.Padding(5);
            this.RotationXBox.Name = "RotationXBox";
            this.RotationXBox.Size = new System.Drawing.Size(60, 20);
            this.RotationXBox.TabIndex = 12;
            this.RotationXBox.Text = "0";
            this.RotationXBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RotationXBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // RotationZBox
            // 
            this.RotationZBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RotationZBox.Location = new System.Drawing.Point(151, 144);
            this.RotationZBox.Margin = new System.Windows.Forms.Padding(5);
            this.RotationZBox.Name = "RotationZBox";
            this.RotationZBox.Size = new System.Drawing.Size(60, 20);
            this.RotationZBox.TabIndex = 14;
            this.RotationZBox.Text = "0";
            this.RotationZBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RotationZBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // label64
            // 
            this.label64.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label64.AutoSize = true;
            this.label64.Cursor = System.Windows.Forms.Cursors.Default;
            this.label64.Location = new System.Drawing.Point(221, 117);
            this.label64.Margin = new System.Windows.Forms.Padding(5);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(44, 13);
            this.label64.TabIndex = 227;
            this.label64.Text = "Velocity";
            // 
            // VelocityYBox
            // 
            this.VelocityYBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VelocityYBox.Location = new System.Drawing.Point(81, 114);
            this.VelocityYBox.Margin = new System.Windows.Forms.Padding(5);
            this.VelocityYBox.Name = "VelocityYBox";
            this.VelocityYBox.Size = new System.Drawing.Size(60, 20);
            this.VelocityYBox.TabIndex = 10;
            this.VelocityYBox.Text = "0";
            this.VelocityYBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.VelocityYBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // VelocityXBox
            // 
            this.VelocityXBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VelocityXBox.Location = new System.Drawing.Point(11, 114);
            this.VelocityXBox.Margin = new System.Windows.Forms.Padding(5);
            this.VelocityXBox.Name = "VelocityXBox";
            this.VelocityXBox.Size = new System.Drawing.Size(60, 20);
            this.VelocityXBox.TabIndex = 9;
            this.VelocityXBox.Text = "0";
            this.VelocityXBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.VelocityXBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // VelocityZBox
            // 
            this.VelocityZBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VelocityZBox.Location = new System.Drawing.Point(151, 114);
            this.VelocityZBox.Margin = new System.Windows.Forms.Padding(5);
            this.VelocityZBox.Name = "VelocityZBox";
            this.VelocityZBox.Size = new System.Drawing.Size(60, 20);
            this.VelocityZBox.TabIndex = 11;
            this.VelocityZBox.Text = "0";
            this.VelocityZBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.VelocityZBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // label54
            // 
            this.label54.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label54.AutoSize = true;
            this.label54.Cursor = System.Windows.Forms.Cursors.Default;
            this.label54.Location = new System.Drawing.Point(221, 87);
            this.label54.Margin = new System.Windows.Forms.Padding(5);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(34, 13);
            this.label54.TabIndex = 223;
            this.label54.Text = "Angle";
            // 
            // AngleYBox
            // 
            this.AngleYBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AngleYBox.Location = new System.Drawing.Point(81, 84);
            this.AngleYBox.Margin = new System.Windows.Forms.Padding(5);
            this.AngleYBox.Name = "AngleYBox";
            this.AngleYBox.Size = new System.Drawing.Size(60, 20);
            this.AngleYBox.TabIndex = 7;
            this.AngleYBox.Text = "0";
            this.AngleYBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AngleYBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // AngleXBox
            // 
            this.AngleXBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AngleXBox.Location = new System.Drawing.Point(11, 84);
            this.AngleXBox.Margin = new System.Windows.Forms.Padding(5);
            this.AngleXBox.Name = "AngleXBox";
            this.AngleXBox.Size = new System.Drawing.Size(60, 20);
            this.AngleXBox.TabIndex = 6;
            this.AngleXBox.Text = "0";
            this.AngleXBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AngleXBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // AngleZBox
            // 
            this.AngleZBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AngleZBox.Location = new System.Drawing.Point(151, 84);
            this.AngleZBox.Margin = new System.Windows.Forms.Padding(5);
            this.AngleZBox.Name = "AngleZBox";
            this.AngleZBox.Size = new System.Drawing.Size(60, 20);
            this.AngleZBox.TabIndex = 8;
            this.AngleZBox.Text = "0";
            this.AngleZBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AngleZBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label6.Location = new System.Drawing.Point(151, 169);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 12);
            this.label6.TabIndex = 219;
            this.label6.Text = "Z";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label24.Location = new System.Drawing.Point(9, 169);
            this.label24.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(62, 12);
            this.label24.TabIndex = 218;
            this.label24.Text = "X";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label52
            // 
            this.label52.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label52.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label52.Location = new System.Drawing.Point(84, 169);
            this.label52.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(57, 12);
            this.label52.TabIndex = 217;
            this.label52.Text = "Y";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AddTypeBtn
            // 
            this.AddTypeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddTypeBtn.Location = new System.Drawing.Point(205, 21);
            this.AddTypeBtn.Name = "AddTypeBtn";
            this.AddTypeBtn.Size = new System.Drawing.Size(25, 23);
            this.AddTypeBtn.TabIndex = 1;
            this.AddTypeBtn.Text = "+";
            this.AddTypeBtn.UseVisualStyleBackColor = true;
            this.AddTypeBtn.Click += new System.EventHandler(this.AddTypeBtn_Click);
            // 
            // label75
            // 
            this.label75.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label75.AutoSize = true;
            this.label75.Cursor = System.Windows.Forms.Cursors.Default;
            this.label75.Location = new System.Drawing.Point(9, 26);
            this.label75.Margin = new System.Windows.Forms.Padding(5);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(31, 13);
            this.label75.TabIndex = 216;
            this.label75.Text = "Type";
            // 
            // ObjectIndexBox
            // 
            this.ObjectIndexBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ObjectIndexBox.FormattingEnabled = true;
            this.ObjectIndexBox.Location = new System.Drawing.Point(50, 23);
            this.ObjectIndexBox.Margin = new System.Windows.Forms.Padding(5);
            this.ObjectIndexBox.Name = "ObjectIndexBox";
            this.ObjectIndexBox.Size = new System.Drawing.Size(147, 21);
            this.ObjectIndexBox.TabIndex = 0;
            this.ObjectIndexBox.SelectedIndexChanged += new System.EventHandler(this.ObjectIndexBox_SelectedIndexChanged);
            // 
            // label53
            // 
            this.label53.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label53.AutoSize = true;
            this.label53.Cursor = System.Windows.Forms.Cursors.Default;
            this.label53.Location = new System.Drawing.Point(219, 57);
            this.label53.Margin = new System.Windows.Forms.Padding(5);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(48, 13);
            this.label53.TabIndex = 192;
            this.label53.Text = "Location";
            // 
            // LocationYBox
            // 
            this.LocationYBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocationYBox.Location = new System.Drawing.Point(79, 54);
            this.LocationYBox.Margin = new System.Windows.Forms.Padding(5);
            this.LocationYBox.Name = "LocationYBox";
            this.LocationYBox.Size = new System.Drawing.Size(60, 20);
            this.LocationYBox.TabIndex = 4;
            this.LocationYBox.Text = "0";
            this.LocationYBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LocationYBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // LocationXBox
            // 
            this.LocationXBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocationXBox.Location = new System.Drawing.Point(9, 54);
            this.LocationXBox.Margin = new System.Windows.Forms.Padding(5);
            this.LocationXBox.Name = "LocationXBox";
            this.LocationXBox.Size = new System.Drawing.Size(60, 20);
            this.LocationXBox.TabIndex = 3;
            this.LocationXBox.Text = "0";
            this.LocationXBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LocationXBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // LocationZBox
            // 
            this.LocationZBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocationZBox.Location = new System.Drawing.Point(149, 54);
            this.LocationZBox.Margin = new System.Windows.Forms.Padding(5);
            this.LocationZBox.Name = "LocationZBox";
            this.LocationZBox.Size = new System.Drawing.Size(60, 20);
            this.LocationZBox.TabIndex = 5;
            this.LocationZBox.Text = "0";
            this.LocationZBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LocationZBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // ObjectListBox
            // 
            this.ObjectListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ObjectListBox.FormattingEnabled = true;
            this.ObjectListBox.Location = new System.Drawing.Point(5, 12);
            this.ObjectListBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ObjectListBox.Name = "ObjectListBox";
            this.ObjectListBox.Size = new System.Drawing.Size(251, 342);
            this.ObjectListBox.TabIndex = 0;
            this.ObjectListBox.SelectedIndexChanged += new System.EventHandler(this.ObjectListBox_SelectedIndexChanged);
            // 
            // ObjectEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.ObjectListBox);
            this.Controls.Add(this.KillObjBtn);
            this.Controls.Add(this.AddObjBtn);
            this.Name = "ObjectEditor";
            this.Size = new System.Drawing.Size(289, 568);
            this.Load += new System.EventHandler(this.ObjectEditor_Load);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button KillObjBtn;
        private System.Windows.Forms.Button AddObjBtn;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button KillTypeBtn;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.TextBox RotationYBox;
        private System.Windows.Forms.TextBox RotationXBox;
        private System.Windows.Forms.TextBox RotationZBox;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.TextBox VelocityYBox;
        private System.Windows.Forms.TextBox VelocityXBox;
        private System.Windows.Forms.TextBox VelocityZBox;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.TextBox AngleYBox;
        private System.Windows.Forms.TextBox AngleXBox;
        private System.Windows.Forms.TextBox AngleZBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Button AddTypeBtn;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox LocationYBox;
        private System.Windows.Forms.TextBox LocationXBox;
        private System.Windows.Forms.TextBox LocationZBox;
        public System.Windows.Forms.ListBox ObjectListBox;
        public System.Windows.Forms.ComboBox ObjectIndexBox;
    }
}
