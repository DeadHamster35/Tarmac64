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
            this.surfaceobjectBox = new System.Windows.Forms.ListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.surfpropertybox = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.surfvertBox = new System.Windows.Forms.TextBox();
            this.surffaceBox = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.surfsectionBox = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.SectionViews = new System.Windows.Forms.TabPage();
            this.masterBox = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.objectCountBox = new System.Windows.Forms.TextBox();
            this.faceBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.sectionBox = new System.Windows.Forms.ComboBox();
            this.viewBox = new System.Windows.Forms.ComboBox();
            this.TextureData = new System.Windows.Forms.TabPage();
            this.TextureControl = new Tarmac64_Library.Controls.TextureEditor();
            this.Settings = new System.Windows.Forms.TabPage();
            this.SettingsControl = new Tarmac64_Library.Controls.CourseSettings();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Object = new System.Windows.Forms.TabPage();
            this.ObjectControl = new Tarmac64_Library.Controls.ObjectEditor();
            this.raycastBox = new System.Windows.Forms.CheckBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.ExportBtn = new System.Windows.Forms.Button();
            this.ImportBtn = new System.Windows.Forms.Button();
            this.GLControl = new Tarmac64_Library.Controls.GLViewer();
            this.surfmaterialBox = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.SurfaceMap.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SectionViews.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.actionBtn.Margin = new System.Windows.Forms.Padding(5);
            this.actionBtn.Name = "actionBtn";
            this.actionBtn.Size = new System.Drawing.Size(65, 23);
            this.actionBtn.TabIndex = 0;
            this.actionBtn.Text = "Load";
            this.actionBtn.UseVisualStyleBackColor = true;
            this.actionBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // SurfaceMap
            // 
            this.SurfaceMap.Controls.Add(this.surfaceobjectBox);
            this.SurfaceMap.Controls.Add(this.groupBox6);
            this.SurfaceMap.Cursor = System.Windows.Forms.Cursors.Default;
            this.SurfaceMap.Location = new System.Drawing.Point(4, 22);
            this.SurfaceMap.Name = "SurfaceMap";
            this.SurfaceMap.Size = new System.Drawing.Size(307, 602);
            this.SurfaceMap.TabIndex = 3;
            this.SurfaceMap.Text = "Surfaces";
            this.SurfaceMap.UseVisualStyleBackColor = true;
            // 
            // surfaceobjectBox
            // 
            this.surfaceobjectBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.surfaceobjectBox.FormattingEnabled = true;
            this.surfaceobjectBox.Location = new System.Drawing.Point(3, 10);
            this.surfaceobjectBox.Name = "surfaceobjectBox";
            this.surfaceobjectBox.Size = new System.Drawing.Size(281, 472);
            this.surfaceobjectBox.TabIndex = 0;
            this.surfaceobjectBox.SelectedIndexChanged += new System.EventHandler(this.SurfaceobjectBox_SelectedIndexChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
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
            this.groupBox6.Location = new System.Drawing.Point(3, 491);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(281, 108);
            this.groupBox6.TabIndex = 17;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Object Info";
            this.groupBox6.Enter += new System.EventHandler(this.groupBox6_Enter);
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
            // SectionViews
            // 
            this.SectionViews.Controls.Add(this.masterBox);
            this.SectionViews.Controls.Add(this.groupBox1);
            this.SectionViews.Controls.Add(this.sectionBox);
            this.SectionViews.Controls.Add(this.viewBox);
            this.SectionViews.Location = new System.Drawing.Point(4, 22);
            this.SectionViews.Name = "SectionViews";
            this.SectionViews.Padding = new System.Windows.Forms.Padding(3);
            this.SectionViews.Size = new System.Drawing.Size(307, 602);
            this.SectionViews.TabIndex = 0;
            this.SectionViews.Text = "Sections";
            this.SectionViews.UseVisualStyleBackColor = true;
            // 
            // masterBox
            // 
            this.masterBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.masterBox.CheckBoxes = true;
            this.masterBox.HideSelection = false;
            this.masterBox.Location = new System.Drawing.Point(3, 33);
            this.masterBox.Name = "masterBox";
            this.masterBox.Size = new System.Drawing.Size(279, 506);
            this.masterBox.TabIndex = 2;
            this.masterBox.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.masterBox_AfterCheck);
            this.masterBox.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.masterBox_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.objectCountBox);
            this.groupBox1.Controls.Add(this.faceBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(6, 545);
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
            this.label9.Location = new System.Drawing.Point(205, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Object Count";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // objectCountBox
            // 
            this.objectCountBox.Enabled = false;
            this.objectCountBox.Location = new System.Drawing.Point(142, 19);
            this.objectCountBox.Name = "objectCountBox";
            this.objectCountBox.Size = new System.Drawing.Size(57, 20);
            this.objectCountBox.TabIndex = 10;
            this.objectCountBox.TabStop = false;
            this.objectCountBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // faceBox
            // 
            this.faceBox.Enabled = false;
            this.faceBox.Location = new System.Drawing.Point(6, 19);
            this.faceBox.Name = "faceBox";
            this.faceBox.Size = new System.Drawing.Size(57, 20);
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
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Face Count";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sectionBox
            // 
            this.sectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sectionBox.FormattingEnabled = true;
            this.sectionBox.Location = new System.Drawing.Point(3, 6);
            this.sectionBox.Name = "sectionBox";
            this.sectionBox.Size = new System.Drawing.Size(167, 21);
            this.sectionBox.TabIndex = 0;
            this.sectionBox.SelectedIndexChanged += new System.EventHandler(this.SectionBox_SelectedIndexChanged);
            // 
            // viewBox
            // 
            this.viewBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewBox.FormattingEnabled = true;
            this.viewBox.Location = new System.Drawing.Point(176, 6);
            this.viewBox.Name = "viewBox";
            this.viewBox.Size = new System.Drawing.Size(106, 21);
            this.viewBox.TabIndex = 1;
            this.viewBox.SelectedIndexChanged += new System.EventHandler(this.ViewBox_SelectedIndexChanged);
            // 
            // TextureData
            // 
            this.TextureData.AutoScroll = true;
            this.TextureData.Controls.Add(this.TextureControl);
            this.TextureData.Location = new System.Drawing.Point(4, 22);
            this.TextureData.Name = "TextureData";
            this.TextureData.Padding = new System.Windows.Forms.Padding(3);
            this.TextureData.Size = new System.Drawing.Size(307, 602);
            this.TextureData.TabIndex = 1;
            this.TextureData.Text = "Textures";
            this.TextureData.UseVisualStyleBackColor = true;
            this.TextureData.Click += new System.EventHandler(this.TextureData_Click);
            // 
            // TextureControl
            // 
            this.TextureControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TextureControl.Location = new System.Drawing.Point(0, 0);
            this.TextureControl.Name = "TextureControl";
            this.TextureControl.Size = new System.Drawing.Size(289, 569);
            this.TextureControl.TabIndex = 0;
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
            this.SettingsControl.Size = new System.Drawing.Size(289, 950);
            this.SettingsControl.TabIndex = 0;
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
            this.Object.Size = new System.Drawing.Size(307, 602);
            this.Object.TabIndex = 4;
            this.Object.Text = "Objects";
            this.Object.UseVisualStyleBackColor = true;
            // 
            // ObjectControl
            // 
            this.ObjectControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ObjectControl.Location = new System.Drawing.Point(-2, 2);
            this.ObjectControl.Name = "ObjectControl";
            this.ObjectControl.Size = new System.Drawing.Size(289, 597);
            this.ObjectControl.TabIndex = 1;
            this.ObjectControl.Load += new System.EventHandler(this.ObjectControl_Load);
            // 
            // raycastBox
            // 
            this.raycastBox.AutoSize = true;
            this.raycastBox.Location = new System.Drawing.Point(89, 15);
            this.raycastBox.Margin = new System.Windows.Forms.Padding(5);
            this.raycastBox.Name = "raycastBox";
            this.raycastBox.Size = new System.Drawing.Size(65, 17);
            this.raycastBox.TabIndex = 1;
            this.raycastBox.TabStop = false;
            this.raycastBox.Text = "Raycast";
            this.raycastBox.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = global::Tarmac64_Library.Properties.Resources.TextLogo;
            this.pictureBox2.Location = new System.Drawing.Point(1085, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(164, 23);
            this.pictureBox2.TabIndex = 97;
            this.pictureBox2.TabStop = false;
            // 
            // ExportBtn
            // 
            this.ExportBtn.AutoSize = true;
            this.ExportBtn.Enabled = false;
            this.ExportBtn.Location = new System.Drawing.Point(164, 11);
            this.ExportBtn.Margin = new System.Windows.Forms.Padding(5);
            this.ExportBtn.Name = "ExportBtn";
            this.ExportBtn.Size = new System.Drawing.Size(55, 23);
            this.ExportBtn.TabIndex = 4;
            this.ExportBtn.Text = "Export";
            this.ExportBtn.UseVisualStyleBackColor = true;
            this.ExportBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // ImportBtn
            // 
            this.ImportBtn.AutoSize = true;
            this.ImportBtn.Location = new System.Drawing.Point(229, 11);
            this.ImportBtn.Margin = new System.Windows.Forms.Padding(5);
            this.ImportBtn.Name = "ImportBtn";
            this.ImportBtn.Size = new System.Drawing.Size(55, 23);
            this.ImportBtn.TabIndex = 5;
            this.ImportBtn.Text = "Import";
            this.ImportBtn.UseVisualStyleBackColor = true;
            this.ImportBtn.Click += new System.EventHandler(this.Import_Click);
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
            // CourseCompiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 682);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.ImportBtn);
            this.Controls.Add(this.ExportBtn);
            this.Controls.Add(this.GLControl);
            this.Controls.Add(this.raycastBox);
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
        private System.Windows.Forms.ComboBox viewBox;
        private System.Windows.Forms.TabPage TextureData;
        private System.Windows.Forms.TabPage Settings;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox surfpropertybox;
        private System.Windows.Forms.TreeView masterBox;
        private System.Windows.Forms.CheckBox raycastBox;
        private System.Windows.Forms.TabPage Object;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Controls.TextureEditor TextureControl;
        private Controls.CourseSettings SettingsControl;
        private Controls.GLViewer GLControl;
        private Controls.ObjectEditor ObjectControl;
        private System.Windows.Forms.Button ExportBtn;
        private System.Windows.Forms.Button ImportBtn;
        private System.Windows.Forms.ComboBox surfmaterialBox;
        private System.Windows.Forms.Label label20;
    }
}