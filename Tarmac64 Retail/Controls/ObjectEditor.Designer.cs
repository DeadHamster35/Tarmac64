namespace Tarmac64_Retail
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
            this.ClassBox = new System.Windows.Forms.ComboBox();
            this.ModeBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BPlayerBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
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
            this.AddTypeBtn = new System.Windows.Forms.Button();
            this.label75 = new System.Windows.Forms.Label();
            this.ObjectIndexBox = new System.Windows.Forms.ComboBox();
            this.label53 = new System.Windows.Forms.Label();
            this.LocationYBox = new System.Windows.Forms.TextBox();
            this.LocationXBox = new System.Windows.Forms.TextBox();
            this.LocationZBox = new System.Windows.Forms.TextBox();
            this.ObjectListBox = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.FlagBox = new System.Windows.Forms.TextBox();
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
            this.groupBox8.Controls.Add(this.label4);
            this.groupBox8.Controls.Add(this.ClassBox);
            this.groupBox8.Controls.Add(this.FlagBox);
            this.groupBox8.Controls.Add(this.label3);
            this.groupBox8.Controls.Add(this.ModeBox);
            this.groupBox8.Controls.Add(this.BPlayerBox);
            this.groupBox8.Controls.Add(this.label2);
            this.groupBox8.Controls.Add(this.label1);
            this.groupBox8.Controls.Add(this.label6);
            this.groupBox8.Controls.Add(this.label24);
            this.groupBox8.Controls.Add(this.label52);
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
            this.groupBox8.Controls.Add(this.AddTypeBtn);
            this.groupBox8.Controls.Add(this.label75);
            this.groupBox8.Controls.Add(this.ObjectIndexBox);
            this.groupBox8.Controls.Add(this.label53);
            this.groupBox8.Controls.Add(this.LocationYBox);
            this.groupBox8.Controls.Add(this.LocationXBox);
            this.groupBox8.Controls.Add(this.LocationZBox);
            this.groupBox8.Location = new System.Drawing.Point(5, 308);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.groupBox8.Size = new System.Drawing.Size(279, 249);
            this.groupBox8.TabIndex = 3;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Object Information";
            // 
            // ClassBox
            // 
            this.ClassBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ClassBox.FormattingEnabled = true;
            this.ClassBox.Location = new System.Drawing.Point(168, 85);
            this.ClassBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ClassBox.Name = "ClassBox";
            this.ClassBox.Size = new System.Drawing.Size(89, 21);
            this.ClassBox.TabIndex = 242;
            this.ClassBox.SelectedIndexChanged += new System.EventHandler(this.ClassBox_SelectedIndexChanged);
            // 
            // ModeBox
            // 
            this.ModeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ModeBox.FormattingEnabled = true;
            this.ModeBox.Location = new System.Drawing.Point(168, 55);
            this.ModeBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ModeBox.Name = "ModeBox";
            this.ModeBox.Size = new System.Drawing.Size(89, 21);
            this.ModeBox.TabIndex = 241;
            this.ModeBox.SelectedIndexChanged += new System.EventHandler(this.ModeBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.label3.Location = new System.Drawing.Point(16, 88);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 240;
            this.label3.Text = "Player";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // BPlayerBox
            // 
            this.BPlayerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BPlayerBox.Location = new System.Drawing.Point(62, 85);
            this.BPlayerBox.Margin = new System.Windows.Forms.Padding(5);
            this.BPlayerBox.Name = "BPlayerBox";
            this.BPlayerBox.Size = new System.Drawing.Size(52, 20);
            this.BPlayerBox.TabIndex = 239;
            this.BPlayerBox.Text = "0";
            this.BPlayerBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BPlayerBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Location = new System.Drawing.Point(126, 90);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 238;
            this.label2.Text = "Class";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(124, 58);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 236;
            this.label1.Text = "Mode";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label6.Location = new System.Drawing.Point(150, 114);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 12);
            this.label6.TabIndex = 234;
            this.label6.Text = "Z";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label24.Location = new System.Drawing.Point(13, 114);
            this.label24.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(60, 12);
            this.label24.TabIndex = 233;
            this.label24.Text = "X";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label52
            // 
            this.label52.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label52.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label52.Location = new System.Drawing.Point(83, 114);
            this.label52.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(60, 12);
            this.label52.TabIndex = 232;
            this.label52.Text = "Y";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KillTypeBtn
            // 
            this.KillTypeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.KillTypeBtn.Location = new System.Drawing.Point(240, 21);
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
            this.label65.Location = new System.Drawing.Point(223, 224);
            this.label65.Margin = new System.Windows.Forms.Padding(5);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(47, 13);
            this.label65.TabIndex = 231;
            this.label65.Text = "Rotation";
            // 
            // RotationYBox
            // 
            this.RotationYBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RotationYBox.Location = new System.Drawing.Point(83, 221);
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
            this.RotationXBox.Location = new System.Drawing.Point(13, 221);
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
            this.RotationZBox.Location = new System.Drawing.Point(153, 221);
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
            this.label64.Location = new System.Drawing.Point(223, 194);
            this.label64.Margin = new System.Windows.Forms.Padding(5);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(44, 13);
            this.label64.TabIndex = 227;
            this.label64.Text = "Velocity";
            // 
            // VelocityYBox
            // 
            this.VelocityYBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VelocityYBox.Location = new System.Drawing.Point(83, 191);
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
            this.VelocityXBox.Location = new System.Drawing.Point(13, 191);
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
            this.VelocityZBox.Location = new System.Drawing.Point(153, 191);
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
            this.label54.Location = new System.Drawing.Point(223, 164);
            this.label54.Margin = new System.Windows.Forms.Padding(5);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(34, 13);
            this.label54.TabIndex = 223;
            this.label54.Text = "Angle";
            // 
            // AngleYBox
            // 
            this.AngleYBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AngleYBox.Location = new System.Drawing.Point(83, 161);
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
            this.AngleXBox.Location = new System.Drawing.Point(13, 161);
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
            this.AngleZBox.Location = new System.Drawing.Point(153, 161);
            this.AngleZBox.Margin = new System.Windows.Forms.Padding(5);
            this.AngleZBox.Name = "AngleZBox";
            this.AngleZBox.Size = new System.Drawing.Size(60, 20);
            this.AngleZBox.TabIndex = 8;
            this.AngleZBox.Text = "0";
            this.AngleZBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AngleZBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
            // 
            // AddTypeBtn
            // 
            this.AddTypeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddTypeBtn.Location = new System.Drawing.Point(209, 21);
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
            this.label75.Location = new System.Drawing.Point(13, 26);
            this.label75.Margin = new System.Windows.Forms.Padding(5);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(31, 13);
            this.label75.TabIndex = 216;
            this.label75.Text = "Type";
            this.label75.Click += new System.EventHandler(this.label75_Click);
            // 
            // ObjectIndexBox
            // 
            this.ObjectIndexBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ObjectIndexBox.FormattingEnabled = true;
            this.ObjectIndexBox.Location = new System.Drawing.Point(54, 23);
            this.ObjectIndexBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
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
            this.label53.Location = new System.Drawing.Point(221, 134);
            this.label53.Margin = new System.Windows.Forms.Padding(5);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(48, 13);
            this.label53.TabIndex = 192;
            this.label53.Text = "Location";
            // 
            // LocationYBox
            // 
            this.LocationYBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocationYBox.Location = new System.Drawing.Point(83, 131);
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
            this.LocationXBox.Location = new System.Drawing.Point(13, 131);
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
            this.LocationZBox.Location = new System.Drawing.Point(153, 131);
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
            this.ObjectListBox.Size = new System.Drawing.Size(251, 290);
            this.ObjectListBox.TabIndex = 0;
            this.ObjectListBox.SelectedIndexChanged += new System.EventHandler(this.ObjectListBox_SelectedIndexChanged);
            this.ObjectListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ObjectListBox_MouseDoubleClick);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Cursor = System.Windows.Forms.Cursors.Default;
            this.label4.Location = new System.Drawing.Point(16, 57);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 244;
            this.label4.Text = "Flag";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // FlagBox
            // 
            this.FlagBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FlagBox.Location = new System.Drawing.Point(62, 55);
            this.FlagBox.Margin = new System.Windows.Forms.Padding(5);
            this.FlagBox.Name = "FlagBox";
            this.FlagBox.Size = new System.Drawing.Size(52, 20);
            this.FlagBox.TabIndex = 243;
            this.FlagBox.Text = "0";
            this.FlagBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FlagBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateLocationHandler);
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
        private System.Windows.Forms.Button AddTypeBtn;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox LocationYBox;
        private System.Windows.Forms.TextBox LocationXBox;
        private System.Windows.Forms.TextBox LocationZBox;
        public System.Windows.Forms.ListBox ObjectListBox;
        public System.Windows.Forms.ComboBox ObjectIndexBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox BPlayerBox;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox ClassBox;
        public System.Windows.Forms.ComboBox ModeBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox FlagBox;
    }
}
