namespace Tarmac64_Library
{
    partial class CourseCompiler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CourseCompiler));
            this.actionBtn = new System.Windows.Forms.Button();
            this.SurfaceMap = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.BattleBoxC = new System.Windows.Forms.CheckBox();
            this.VSBoxC = new System.Windows.Forms.CheckBox();
            this.ExtraBoxC = new System.Windows.Forms.CheckBox();
            this.HundredFiftyBoxC = new System.Windows.Forms.CheckBox();
            this.HundredBoxC = new System.Windows.Forms.CheckBox();
            this.FiftyBoxC = new System.Windows.Forms.CheckBox();
            this.TTBoxC = new System.Windows.Forms.CheckBox();
            this.GPBoxC = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.surfpropertybox = new System.Windows.Forms.ComboBox();
            this.surfmaterialBox = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.surfvertBox = new System.Windows.Forms.TextBox();
            this.surffaceBox = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.surfsectionBox = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.surfaceobjectBox = new System.Windows.Forms.ListBox();
            this.SectionViews = new System.Windows.Forms.TabPage();
            this.CopyBtn = new System.Windows.Forms.Button();
            this.CopySectionIndexBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.objectCountBox = new System.Windows.Forms.TextBox();
            this.faceBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SVL3Load = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.WaveBox = new System.Windows.Forms.CheckBox();
            this.BattleBoxR = new System.Windows.Forms.CheckBox();
            this.VSBoxR = new System.Windows.Forms.CheckBox();
            this.ExtraBoxR = new System.Windows.Forms.CheckBox();
            this.HundredFiftyBoxR = new System.Windows.Forms.CheckBox();
            this.HundredBoxR = new System.Windows.Forms.CheckBox();
            this.FiftyBoxR = new System.Windows.Forms.CheckBox();
            this.TTBoxR = new System.Windows.Forms.CheckBox();
            this.GPBoxR = new System.Windows.Forms.CheckBox();
            this.masterBox = new System.Windows.Forms.TreeView();
            this.sectionBox = new System.Windows.Forms.ComboBox();
            this.TextureData = new System.Windows.Forms.TabPage();
            this.TextureControl = new Tarmac64_Retail.TextureEditor();
            this.Settings = new System.Windows.Forms.TabPage();
            this.SettingsControl = new Tarmac64_Retail.CourseSettings();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Object = new System.Windows.Forms.TabPage();
            this.ObjectControl = new Tarmac64_Retail.ObjectEditor();
            this.ExportBtn = new System.Windows.Forms.Button();
            this.ImportBtn = new System.Windows.Forms.Button();
            this.TypeBox = new System.Windows.Forms.ComboBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.GLControl = new Tarmac64_Retail.GLViewer();
            this.SurfaceMap.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SectionViews.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.TextureData.SuspendLayout();
            this.Settings.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Object.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // actionBtn
            // 
            this.actionBtn.AutoSize = true;
            this.actionBtn.Location = new System.Drawing.Point(14, 11);
            this.actionBtn.Name = "actionBtn";
            this.actionBtn.Size = new System.Drawing.Size(50, 23);
            this.actionBtn.TabIndex = 0;
            this.actionBtn.Text = "Load";
            this.actionBtn.UseVisualStyleBackColor = true;
            this.actionBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // SurfaceMap
            // 
            this.SurfaceMap.Controls.Add(this.groupBox6);
            this.SurfaceMap.Controls.Add(this.surfaceobjectBox);
            this.SurfaceMap.Cursor = System.Windows.Forms.Cursors.Default;
            this.SurfaceMap.Location = new System.Drawing.Point(4, 22);
            this.SurfaceMap.Name = "SurfaceMap";
            this.SurfaceMap.Size = new System.Drawing.Size(192, 74);
            this.SurfaceMap.TabIndex = 3;
            this.SurfaceMap.Text = "Surfaces";
            this.SurfaceMap.UseVisualStyleBackColor = true;
            this.SurfaceMap.Click += new System.EventHandler(this.SurfaceMap_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox6.Controls.Add(this.BattleBoxC);
            this.groupBox6.Controls.Add(this.VSBoxC);
            this.groupBox6.Controls.Add(this.ExtraBoxC);
            this.groupBox6.Controls.Add(this.HundredFiftyBoxC);
            this.groupBox6.Controls.Add(this.HundredBoxC);
            this.groupBox6.Controls.Add(this.FiftyBoxC);
            this.groupBox6.Controls.Add(this.TTBoxC);
            this.groupBox6.Controls.Add(this.GPBoxC);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.surfpropertybox);
            this.groupBox6.Controls.Add(this.surfmaterialBox);
            this.groupBox6.Controls.Add(this.label20);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.surfvertBox);
            this.groupBox6.Controls.Add(this.surffaceBox);
            this.groupBox6.Controls.Add(this.label23);
            this.groupBox6.Controls.Add(this.surfsectionBox);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Location = new System.Drawing.Point(3, -99);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(281, 170);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Object Info";
            this.groupBox6.Enter += new System.EventHandler(this.groupBox6_Enter);
            // 
            // BattleBoxC
            // 
            this.BattleBoxC.AutoSize = true;
            this.BattleBoxC.Checked = true;
            this.BattleBoxC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BattleBoxC.Location = new System.Drawing.Point(217, 117);
            this.BattleBoxC.Name = "BattleBoxC";
            this.BattleBoxC.Size = new System.Drawing.Size(53, 17);
            this.BattleBoxC.TabIndex = 35;
            this.BattleBoxC.Text = "Battle";
            this.BattleBoxC.UseVisualStyleBackColor = true;
            // 
            // VSBoxC
            // 
            this.VSBoxC.AutoSize = true;
            this.VSBoxC.Checked = true;
            this.VSBoxC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VSBoxC.Location = new System.Drawing.Point(171, 117);
            this.VSBoxC.Name = "VSBoxC";
            this.VSBoxC.Size = new System.Drawing.Size(40, 17);
            this.VSBoxC.TabIndex = 34;
            this.VSBoxC.Text = "VS";
            this.VSBoxC.UseVisualStyleBackColor = true;
            // 
            // ExtraBoxC
            // 
            this.ExtraBoxC.AutoSize = true;
            this.ExtraBoxC.Checked = true;
            this.ExtraBoxC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExtraBoxC.Location = new System.Drawing.Point(156, 140);
            this.ExtraBoxC.Name = "ExtraBoxC";
            this.ExtraBoxC.Size = new System.Drawing.Size(50, 17);
            this.ExtraBoxC.TabIndex = 33;
            this.ExtraBoxC.Text = "Extra";
            this.ExtraBoxC.UseVisualStyleBackColor = true;
            this.ExtraBoxC.CheckedChanged += new System.EventHandler(this.GPBoxC_CheckedChanged);
            // 
            // HundredFiftyBoxC
            // 
            this.HundredFiftyBoxC.AutoSize = true;
            this.HundredFiftyBoxC.Checked = true;
            this.HundredFiftyBoxC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HundredFiftyBoxC.Location = new System.Drawing.Point(106, 140);
            this.HundredFiftyBoxC.Name = "HundredFiftyBoxC";
            this.HundredFiftyBoxC.Size = new System.Drawing.Size(44, 17);
            this.HundredFiftyBoxC.TabIndex = 32;
            this.HundredFiftyBoxC.Text = "150";
            this.HundredFiftyBoxC.UseVisualStyleBackColor = true;
            this.HundredFiftyBoxC.CheckedChanged += new System.EventHandler(this.GPBoxC_CheckedChanged);
            // 
            // HundredBoxC
            // 
            this.HundredBoxC.AutoSize = true;
            this.HundredBoxC.Checked = true;
            this.HundredBoxC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HundredBoxC.Location = new System.Drawing.Point(56, 140);
            this.HundredBoxC.Name = "HundredBoxC";
            this.HundredBoxC.Size = new System.Drawing.Size(44, 17);
            this.HundredBoxC.TabIndex = 31;
            this.HundredBoxC.Text = "100";
            this.HundredBoxC.UseVisualStyleBackColor = true;
            this.HundredBoxC.CheckedChanged += new System.EventHandler(this.GPBoxC_CheckedChanged);
            // 
            // FiftyBoxC
            // 
            this.FiftyBoxC.AutoSize = true;
            this.FiftyBoxC.Checked = true;
            this.FiftyBoxC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FiftyBoxC.Location = new System.Drawing.Point(12, 140);
            this.FiftyBoxC.Name = "FiftyBoxC";
            this.FiftyBoxC.Size = new System.Drawing.Size(38, 17);
            this.FiftyBoxC.TabIndex = 30;
            this.FiftyBoxC.Text = "50";
            this.FiftyBoxC.UseVisualStyleBackColor = true;
            this.FiftyBoxC.CheckedChanged += new System.EventHandler(this.GPBoxC_CheckedChanged);
            // 
            // TTBoxC
            // 
            this.TTBoxC.AutoSize = true;
            this.TTBoxC.Checked = true;
            this.TTBoxC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TTBoxC.Location = new System.Drawing.Point(93, 117);
            this.TTBoxC.Name = "TTBoxC";
            this.TTBoxC.Size = new System.Drawing.Size(72, 17);
            this.TTBoxC.TabIndex = 29;
            this.TTBoxC.Text = "Time Trial";
            this.TTBoxC.UseVisualStyleBackColor = true;
            this.TTBoxC.CheckedChanged += new System.EventHandler(this.GPBoxC_CheckedChanged);
            // 
            // GPBoxC
            // 
            this.GPBoxC.AutoSize = true;
            this.GPBoxC.Checked = true;
            this.GPBoxC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GPBoxC.Location = new System.Drawing.Point(12, 117);
            this.GPBoxC.Name = "GPBoxC";
            this.GPBoxC.Size = new System.Drawing.Size(75, 17);
            this.GPBoxC.TabIndex = 28;
            this.GPBoxC.Text = "Grand Prix";
            this.GPBoxC.UseVisualStyleBackColor = true;
            this.GPBoxC.CheckedChanged += new System.EventHandler(this.GPBoxC_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(140, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Property";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // surfpropertybox
            // 
            this.surfpropertybox.FormattingEnabled = true;
            this.surfpropertybox.Items.AddRange(new object[] {
            "Standard",
            "Force Tumble",
            "No Collision",
            "Darken Player",
            "Out of Bounds"});
            this.surfpropertybox.Location = new System.Drawing.Point(192, 25);
            this.surfpropertybox.Name = "surfpropertybox";
            this.surfpropertybox.Size = new System.Drawing.Size(75, 21);
            this.surfpropertybox.TabIndex = 2;
            this.surfpropertybox.SelectedIndexChanged += new System.EventHandler(this.surfacepropertybox_SelectedIndexChanged);
            // 
            // surfmaterialBox
            // 
            this.surfmaterialBox.FormattingEnabled = true;
            this.surfmaterialBox.Location = new System.Drawing.Point(59, 52);
            this.surfmaterialBox.Name = "surfmaterialBox";
            this.surfmaterialBox.Size = new System.Drawing.Size(208, 21);
            this.surfmaterialBox.TabIndex = 1;
            this.surfmaterialBox.SelectedIndexChanged += new System.EventHandler(this.SurfmaterialBox_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(8, 55);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(44, 13);
            this.label20.TabIndex = 12;
            this.label20.Text = "Material";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(88, 82);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "# Verts";
            // 
            // surfvertBox
            // 
            this.surfvertBox.Enabled = false;
            this.surfvertBox.Location = new System.Drawing.Point(12, 79);
            this.surfvertBox.Name = "surfvertBox";
            this.surfvertBox.Size = new System.Drawing.Size(70, 20);
            this.surfvertBox.TabIndex = 20;
            this.surfvertBox.TabStop = false;
            this.surfvertBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // surffaceBox
            // 
            this.surffaceBox.Enabled = false;
            this.surffaceBox.Location = new System.Drawing.Point(134, 79);
            this.surffaceBox.Name = "surffaceBox";
            this.surffaceBox.Size = new System.Drawing.Size(70, 20);
            this.surffaceBox.TabIndex = 21;
            this.surffaceBox.TabStop = false;
            this.surffaceBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(210, 82);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(44, 13);
            this.label23.TabIndex = 23;
            this.label23.Text = "# Faces";
            // 
            // surfsectionBox
            // 
            this.surfsectionBox.FormattingEnabled = true;
            this.surfsectionBox.Location = new System.Drawing.Point(59, 25);
            this.surfsectionBox.Name = "surfsectionBox";
            this.surfsectionBox.Size = new System.Drawing.Size(75, 21);
            this.surfsectionBox.TabIndex = 0;
            this.surfsectionBox.SelectedIndexChanged += new System.EventHandler(this.SurfsectionBox_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 28);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(43, 13);
            this.label17.TabIndex = 13;
            this.label17.Text = "Section";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // surfaceobjectBox
            // 
            this.surfaceobjectBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.surfaceobjectBox.FormattingEnabled = true;
            this.surfaceobjectBox.Location = new System.Drawing.Point(3, 9);
            this.surfaceobjectBox.Name = "surfaceobjectBox";
            this.surfaceobjectBox.Size = new System.Drawing.Size(281, 4);
            this.surfaceobjectBox.TabIndex = 0;
            this.surfaceobjectBox.SelectedIndexChanged += new System.EventHandler(this.SurfaceobjectBox_SelectedIndexChanged);
            // 
            // SectionViews
            // 
            this.SectionViews.Controls.Add(this.CopyBtn);
            this.SectionViews.Controls.Add(this.CopySectionIndexBox);
            this.SectionViews.Controls.Add(this.groupBox1);
            this.SectionViews.Controls.Add(this.SVL3Load);
            this.SectionViews.Controls.Add(this.button2);
            this.SectionViews.Controls.Add(this.groupBox2);
            this.SectionViews.Controls.Add(this.masterBox);
            this.SectionViews.Controls.Add(this.sectionBox);
            this.SectionViews.Location = new System.Drawing.Point(4, 22);
            this.SectionViews.Name = "SectionViews";
            this.SectionViews.Padding = new System.Windows.Forms.Padding(3);
            this.SectionViews.Size = new System.Drawing.Size(307, 602);
            this.SectionViews.TabIndex = 0;
            this.SectionViews.Text = "Sections";
            this.SectionViews.UseVisualStyleBackColor = true;
            // 
            // CopyBtn
            // 
            this.CopyBtn.Location = new System.Drawing.Point(3, 36);
            this.CopyBtn.Margin = new System.Windows.Forms.Padding(5);
            this.CopyBtn.Name = "CopyBtn";
            this.CopyBtn.Size = new System.Drawing.Size(70, 23);
            this.CopyBtn.TabIndex = 102;
            this.CopyBtn.Text = "Copy From";
            this.CopyBtn.UseVisualStyleBackColor = true;
            this.CopyBtn.Click += new System.EventHandler(this.CopyBtn_Click);
            // 
            // CopySectionIndexBox
            // 
            this.CopySectionIndexBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CopySectionIndexBox.FormattingEnabled = true;
            this.CopySectionIndexBox.Location = new System.Drawing.Point(81, 37);
            this.CopySectionIndexBox.Name = "CopySectionIndexBox";
            this.CopySectionIndexBox.Size = new System.Drawing.Size(123, 21);
            this.CopySectionIndexBox.TabIndex = 101;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.objectCountBox);
            this.groupBox1.Controls.Add(this.faceBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(3, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 51);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Section Stats";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(173, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Objects";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // objectCountBox
            // 
            this.objectCountBox.Enabled = false;
            this.objectCountBox.Location = new System.Drawing.Point(110, 19);
            this.objectCountBox.Name = "objectCountBox";
            this.objectCountBox.Size = new System.Drawing.Size(50, 20);
            this.objectCountBox.TabIndex = 10;
            this.objectCountBox.TabStop = false;
            this.objectCountBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // faceBox
            // 
            this.faceBox.Enabled = false;
            this.faceBox.Location = new System.Drawing.Point(6, 19);
            this.faceBox.Name = "faceBox";
            this.faceBox.Size = new System.Drawing.Size(50, 20);
            this.faceBox.TabIndex = 7;
            this.faceBox.TabStop = false;
            this.faceBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(69, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Faces";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SVL3Load
            // 
            this.SVL3Load.Location = new System.Drawing.Point(212, 36);
            this.SVL3Load.Margin = new System.Windows.Forms.Padding(5);
            this.SVL3Load.Name = "SVL3Load";
            this.SVL3Load.Size = new System.Drawing.Size(70, 23);
            this.SVL3Load.TabIndex = 100;
            this.SVL3Load.Text = "Load SVL3";
            this.SVL3Load.UseVisualStyleBackColor = true;
            this.SVL3Load.Click += new System.EventHandler(this.SVL3Load_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(212, 5);
            this.button2.Margin = new System.Windows.Forms.Padding(5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 23);
            this.button2.TabIndex = 99;
            this.button2.Text = "Save SVL3";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.WaveBox);
            this.groupBox2.Controls.Add(this.BattleBoxR);
            this.groupBox2.Controls.Add(this.VSBoxR);
            this.groupBox2.Controls.Add(this.ExtraBoxR);
            this.groupBox2.Controls.Add(this.HundredFiftyBoxR);
            this.groupBox2.Controls.Add(this.HundredBoxR);
            this.groupBox2.Controls.Add(this.FiftyBoxR);
            this.groupBox2.Controls.Add(this.TTBoxR);
            this.groupBox2.Controls.Add(this.GPBoxR);
            this.groupBox2.Location = new System.Drawing.Point(3, 529);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 67);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Object Settings";
            // 
            // WaveBox
            // 
            this.WaveBox.AutoSize = true;
            this.WaveBox.Checked = true;
            this.WaveBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WaveBox.Location = new System.Drawing.Point(206, 43);
            this.WaveBox.Name = "WaveBox";
            this.WaveBox.Size = new System.Drawing.Size(67, 17);
            this.WaveBox.TabIndex = 8;
            this.WaveBox.Text = "Wave64";
            this.WaveBox.UseVisualStyleBackColor = true;
            this.WaveBox.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // BattleBoxR
            // 
            this.BattleBoxR.AutoSize = true;
            this.BattleBoxR.Checked = true;
            this.BattleBoxR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BattleBoxR.Location = new System.Drawing.Point(212, 20);
            this.BattleBoxR.Name = "BattleBoxR";
            this.BattleBoxR.Size = new System.Drawing.Size(53, 17);
            this.BattleBoxR.TabIndex = 7;
            this.BattleBoxR.Text = "Battle";
            this.BattleBoxR.UseVisualStyleBackColor = true;
            // 
            // VSBoxR
            // 
            this.VSBoxR.AutoSize = true;
            this.VSBoxR.Checked = true;
            this.VSBoxR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VSBoxR.Location = new System.Drawing.Point(166, 20);
            this.VSBoxR.Name = "VSBoxR";
            this.VSBoxR.Size = new System.Drawing.Size(40, 17);
            this.VSBoxR.TabIndex = 6;
            this.VSBoxR.Text = "VS";
            this.VSBoxR.UseVisualStyleBackColor = true;
            // 
            // ExtraBoxR
            // 
            this.ExtraBoxR.AutoSize = true;
            this.ExtraBoxR.Checked = true;
            this.ExtraBoxR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExtraBoxR.Location = new System.Drawing.Point(151, 43);
            this.ExtraBoxR.Name = "ExtraBoxR";
            this.ExtraBoxR.Size = new System.Drawing.Size(50, 17);
            this.ExtraBoxR.TabIndex = 5;
            this.ExtraBoxR.Text = "Extra";
            this.ExtraBoxR.UseVisualStyleBackColor = true;
            this.ExtraBoxR.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // HundredFiftyBoxR
            // 
            this.HundredFiftyBoxR.AutoSize = true;
            this.HundredFiftyBoxR.Checked = true;
            this.HundredFiftyBoxR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HundredFiftyBoxR.Location = new System.Drawing.Point(101, 43);
            this.HundredFiftyBoxR.Name = "HundredFiftyBoxR";
            this.HundredFiftyBoxR.Size = new System.Drawing.Size(44, 17);
            this.HundredFiftyBoxR.TabIndex = 4;
            this.HundredFiftyBoxR.Text = "150";
            this.HundredFiftyBoxR.UseVisualStyleBackColor = true;
            this.HundredFiftyBoxR.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // HundredBoxR
            // 
            this.HundredBoxR.AutoSize = true;
            this.HundredBoxR.Checked = true;
            this.HundredBoxR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HundredBoxR.Location = new System.Drawing.Point(51, 43);
            this.HundredBoxR.Name = "HundredBoxR";
            this.HundredBoxR.Size = new System.Drawing.Size(44, 17);
            this.HundredBoxR.TabIndex = 3;
            this.HundredBoxR.Text = "100";
            this.HundredBoxR.UseVisualStyleBackColor = true;
            this.HundredBoxR.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // FiftyBoxR
            // 
            this.FiftyBoxR.AutoSize = true;
            this.FiftyBoxR.Checked = true;
            this.FiftyBoxR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FiftyBoxR.Location = new System.Drawing.Point(7, 43);
            this.FiftyBoxR.Name = "FiftyBoxR";
            this.FiftyBoxR.Size = new System.Drawing.Size(38, 17);
            this.FiftyBoxR.TabIndex = 2;
            this.FiftyBoxR.Text = "50";
            this.FiftyBoxR.UseVisualStyleBackColor = true;
            this.FiftyBoxR.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // TTBoxR
            // 
            this.TTBoxR.AutoSize = true;
            this.TTBoxR.Checked = true;
            this.TTBoxR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TTBoxR.Location = new System.Drawing.Point(88, 20);
            this.TTBoxR.Name = "TTBoxR";
            this.TTBoxR.Size = new System.Drawing.Size(72, 17);
            this.TTBoxR.TabIndex = 1;
            this.TTBoxR.Text = "Time Trial";
            this.TTBoxR.UseVisualStyleBackColor = true;
            this.TTBoxR.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // GPBoxR
            // 
            this.GPBoxR.AutoSize = true;
            this.GPBoxR.Checked = true;
            this.GPBoxR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GPBoxR.Location = new System.Drawing.Point(7, 20);
            this.GPBoxR.Name = "GPBoxR";
            this.GPBoxR.Size = new System.Drawing.Size(75, 17);
            this.GPBoxR.TabIndex = 0;
            this.GPBoxR.Text = "Grand Prix";
            this.GPBoxR.UseVisualStyleBackColor = true;
            this.GPBoxR.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // masterBox
            // 
            this.masterBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.masterBox.CheckBoxes = true;
            this.masterBox.HideSelection = false;
            this.masterBox.Location = new System.Drawing.Point(3, 123);
            this.masterBox.Name = "masterBox";
            this.masterBox.Size = new System.Drawing.Size(279, 400);
            this.masterBox.TabIndex = 2;
            this.masterBox.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.masterBox_AfterCheck);
            this.masterBox.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.masterBox_AfterSelect);
            // 
            // sectionBox
            // 
            this.sectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sectionBox.FormattingEnabled = true;
            this.sectionBox.Location = new System.Drawing.Point(3, 6);
            this.sectionBox.Name = "sectionBox";
            this.sectionBox.Size = new System.Drawing.Size(201, 21);
            this.sectionBox.TabIndex = 0;
            this.sectionBox.SelectedIndexChanged += new System.EventHandler(this.SectionBox_SelectedIndexChanged);
            // 
            // TextureData
            // 
            this.TextureData.AutoScroll = true;
            this.TextureData.Controls.Add(this.TextureControl);
            this.TextureData.Location = new System.Drawing.Point(4, 22);
            this.TextureData.Name = "TextureData";
            this.TextureData.Padding = new System.Windows.Forms.Padding(3);
            this.TextureData.Size = new System.Drawing.Size(192, 74);
            this.TextureData.TabIndex = 1;
            this.TextureData.Text = "Textures";
            this.TextureData.UseVisualStyleBackColor = true;
            this.TextureData.Click += new System.EventHandler(this.TextureData_Click);
            // 
            // TextureControl
            // 
            this.TextureControl.Location = new System.Drawing.Point(0, 0);
            this.TextureControl.Name = "TextureControl";
            this.TextureControl.Size = new System.Drawing.Size(289, 780);
            this.TextureControl.TabIndex = 0;
            this.TextureControl.Load += new System.EventHandler(this.TextureControl_Load);
            // 
            // Settings
            // 
            this.Settings.AutoScroll = true;
            this.Settings.Controls.Add(this.SettingsControl);
            this.Settings.Location = new System.Drawing.Point(4, 22);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(307, 602);
            this.Settings.TabIndex = 2;
            this.Settings.Text = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            this.Settings.Click += new System.EventHandler(this.CourseInfo_Click);
            // 
            // SettingsControl
            // 
            this.SettingsControl.Location = new System.Drawing.Point(0, 0);
            this.SettingsControl.Name = "SettingsControl";
            this.SettingsControl.Size = new System.Drawing.Size(289, 1225);
            this.SettingsControl.TabIndex = 0;
            this.SettingsControl.Load += new System.EventHandler(this.SettingsControl_Load);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.Settings);
            this.tabControl1.Controls.Add(this.TextureData);
            this.tabControl1.Controls.Add(this.SectionViews);
            this.tabControl1.Controls.Add(this.SurfaceMap);
            this.tabControl1.Controls.Add(this.Object);
            this.tabControl1.Location = new System.Drawing.Point(16, 42);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(315, 628);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // Object
            // 
            this.Object.AutoScroll = true;
            this.Object.Controls.Add(this.ObjectControl);
            this.Object.Location = new System.Drawing.Point(4, 22);
            this.Object.Name = "Object";
            this.Object.Size = new System.Drawing.Size(192, 74);
            this.Object.TabIndex = 4;
            this.Object.Text = "Objects";
            this.Object.UseVisualStyleBackColor = true;
            // 
            // ObjectControl
            // 
            this.ObjectControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ObjectControl.Location = new System.Drawing.Point(3, 3);
            this.ObjectControl.Name = "ObjectControl";
            this.ObjectControl.Size = new System.Drawing.Size(289, 0);
            this.ObjectControl.TabIndex = 1;
            this.ObjectControl.Load += new System.EventHandler(this.ObjectControl_Load);
            // 
            // ExportBtn
            // 
            this.ExportBtn.AutoSize = true;
            this.ExportBtn.Enabled = false;
            this.ExportBtn.Location = new System.Drawing.Point(225, 11);
            this.ExportBtn.Name = "ExportBtn";
            this.ExportBtn.Size = new System.Drawing.Size(50, 23);
            this.ExportBtn.TabIndex = 4;
            this.ExportBtn.Text = "Export";
            this.ExportBtn.UseVisualStyleBackColor = true;
            this.ExportBtn.Click += new System.EventHandler(this.ExportBtn_Click);
            // 
            // ImportBtn
            // 
            this.ImportBtn.AutoSize = true;
            this.ImportBtn.Enabled = false;
            this.ImportBtn.Location = new System.Drawing.Point(281, 11);
            this.ImportBtn.Name = "ImportBtn";
            this.ImportBtn.Size = new System.Drawing.Size(50, 23);
            this.ImportBtn.TabIndex = 5;
            this.ImportBtn.Text = "Import";
            this.ImportBtn.UseVisualStyleBackColor = true;
            this.ImportBtn.Click += new System.EventHandler(this.Import_Click);
            // 
            // TypeBox
            // 
            this.TypeBox.FormattingEnabled = true;
            this.TypeBox.Items.AddRange(new object[] {
            "Race",
            "Battle"});
            this.TypeBox.Location = new System.Drawing.Point(70, 11);
            this.TypeBox.Name = "TypeBox";
            this.TypeBox.Size = new System.Drawing.Size(93, 21);
            this.TypeBox.TabIndex = 98;
            this.TypeBox.SelectedIndexChanged += new System.EventHandler(this.TypeBox_SelectedIndexChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(1085, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(164, 23);
            this.pictureBox2.TabIndex = 97;
            this.pictureBox2.TabStop = false;
            // 
            // GLControl
            // 
            this.GLControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GLControl.Location = new System.Drawing.Point(337, 11);
            this.GLControl.Name = "GLControl";
            this.GLControl.Size = new System.Drawing.Size(915, 659);
            this.GLControl.TabIndex = 3;
            this.GLControl.Load += new System.EventHandler(this.GLControl_Load);
            // 
            // CourseCompiler
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1264, 682);
            this.Controls.Add(this.TypeBox);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.ImportBtn);
            this.Controls.Add(this.ExportBtn);
            this.Controls.Add(this.GLControl);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.actionBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "CourseCompiler";
            this.Text = "Course Compiler";
            this.Load += new System.EventHandler(this.FormLoad);
            this.SurfaceMap.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.SectionViews.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.TextureData.ResumeLayout(false);
            this.Settings.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.Object.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button actionBtn;
        private System.Windows.Forms.TabPage SurfaceMap;
        private System.Windows.Forms.ListBox surfaceobjectBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox surfvertBox;
        private System.Windows.Forms.TextBox surffaceBox;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox surfsectionBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TabPage SectionViews;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox objectCountBox;
        private System.Windows.Forms.TextBox faceBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox sectionBox;
        private System.Windows.Forms.TabPage TextureData;
        private System.Windows.Forms.TabPage Settings;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox surfpropertybox;
        private System.Windows.Forms.TreeView masterBox;
        private System.Windows.Forms.TabPage Object;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Tarmac64_Retail.GLViewer GLControl;
        private Tarmac64_Retail.ObjectEditor ObjectControl;
        private System.Windows.Forms.Button ExportBtn;
        private System.Windows.Forms.Button ImportBtn;
        private System.Windows.Forms.ComboBox surfmaterialBox;
        private System.Windows.Forms.Label label20;
        private Tarmac64_Retail.TextureEditor TextureControl;
        private Tarmac64_Retail.CourseSettings SettingsControl;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox TypeBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ExtraBoxR;
        private System.Windows.Forms.CheckBox HundredFiftyBoxR;
        private System.Windows.Forms.CheckBox HundredBoxR;
        private System.Windows.Forms.CheckBox FiftyBoxR;
        private System.Windows.Forms.CheckBox TTBoxR;
        private System.Windows.Forms.CheckBox GPBoxR;
        private System.Windows.Forms.CheckBox ExtraBoxC;
        private System.Windows.Forms.CheckBox HundredFiftyBoxC;
        private System.Windows.Forms.CheckBox HundredBoxC;
        private System.Windows.Forms.CheckBox FiftyBoxC;
        private System.Windows.Forms.CheckBox TTBoxC;
        private System.Windows.Forms.CheckBox GPBoxC;
        private System.Windows.Forms.CheckBox BattleBoxR;
        private System.Windows.Forms.CheckBox VSBoxR;
        private System.Windows.Forms.CheckBox BattleBoxC;
        private System.Windows.Forms.CheckBox VSBoxC;
        private System.Windows.Forms.CheckBox WaveBox;
        private System.Windows.Forms.Button SVL3Load;
        private System.Windows.Forms.Button CopyBtn;
        private System.Windows.Forms.ComboBox CopySectionIndexBox;
    }
}