namespace Tarmac64
{
    partial class TarmacAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TarmacAbout));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolsStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.textureListsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.modelCompilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textureCompilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DebugStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.racerEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.courseImporterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vertConverterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugKitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(402, 79);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.Location = new System.Drawing.Point(12, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(402, 34);
            this.label2.TabIndex = 2;
            this.label2.Text = "Yumi\r\nDeadHamster 2021";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(361, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "1";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.Click += new System.EventHandler(this.Label3_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.Location = new System.Drawing.Point(12, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(198, 104);
            this.label4.TabIndex = 30;
            this.label4.Text = "Micro500\r\nRenaKunisaki\r\nmib-f8sm9c\r\nLitronom\r\nzouzzz\r\nPeter Lemon\r\nred";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolsStripMenu,
            this.DebugStripMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(426, 24);
            this.menuStrip1.TabIndex = 31;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolsStripMenu
            // 
            this.ToolsStripMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textureListsToolStripMenuItem1,
            this.modelCompilerToolStripMenuItem,
            this.textureCompilerToolStripMenuItem});
            this.ToolsStripMenu.Name = "ToolsStripMenu";
            this.ToolsStripMenu.Size = new System.Drawing.Size(48, 20);
            this.ToolsStripMenu.Text = "Tools";
            // 
            // textureListsToolStripMenuItem1
            // 
            this.textureListsToolStripMenuItem1.Name = "textureListsToolStripMenuItem1";
            this.textureListsToolStripMenuItem1.Size = new System.Drawing.Size(165, 22);
            this.textureListsToolStripMenuItem1.Text = "Course Compiler";
            this.textureListsToolStripMenuItem1.Click += new System.EventHandler(this.GeometryToolStripMenuItem1_Click);
            // 
            // modelCompilerToolStripMenuItem
            // 
            this.modelCompilerToolStripMenuItem.Name = "modelCompilerToolStripMenuItem";
            this.modelCompilerToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.modelCompilerToolStripMenuItem.Text = "Model Compiler";
            this.modelCompilerToolStripMenuItem.Click += new System.EventHandler(this.modelCompilerToolStripMenuItem_Click);
            // 
            // textureCompilerToolStripMenuItem
            // 
            this.textureCompilerToolStripMenuItem.Name = "textureCompilerToolStripMenuItem";
            this.textureCompilerToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.textureCompilerToolStripMenuItem.Text = "Texture Compiler";
            this.textureCompilerToolStripMenuItem.Click += new System.EventHandler(this.textureCompilerToolStripMenuItem_Click);
            // 
            // DebugStripMenu
            // 
            this.DebugStripMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.racerEditorToolStripMenuItem,
            this.courseImporterToolStripMenuItem,
            this.vertConverterToolStripMenuItem,
            this.itemEditorToolStripMenuItem,
            this.pathEditorToolStripMenuItem,
            this.debugKitToolStripMenuItem});
            this.DebugStripMenu.Name = "DebugStripMenu";
            this.DebugStripMenu.Size = new System.Drawing.Size(53, 20);
            this.DebugStripMenu.Text = "DevKit";
            // 
            // racerEditorToolStripMenuItem
            // 
            this.racerEditorToolStripMenuItem.Name = "racerEditorToolStripMenuItem";
            this.racerEditorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.racerEditorToolStripMenuItem.Text = "Racer Editor";
            this.racerEditorToolStripMenuItem.Click += new System.EventHandler(this.RacerEditorToolStripMenuItem_Click);
            // 
            // courseImporterToolStripMenuItem
            // 
            this.courseImporterToolStripMenuItem.Name = "courseImporterToolStripMenuItem";
            this.courseImporterToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.courseImporterToolStripMenuItem.Text = "Segment Compiler";
            this.courseImporterToolStripMenuItem.Click += new System.EventHandler(this.courseImporterToolStripMenuItem_Click);
            // 
            // vertConverterToolStripMenuItem
            // 
            this.vertConverterToolStripMenuItem.Name = "vertConverterToolStripMenuItem";
            this.vertConverterToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.vertConverterToolStripMenuItem.Text = "Segment Exporter";
            this.vertConverterToolStripMenuItem.Click += new System.EventHandler(this.VertConverterToolStripMenuItem_Click);
            // 
            // itemEditorToolStripMenuItem
            // 
            this.itemEditorToolStripMenuItem.Name = "itemEditorToolStripMenuItem";
            this.itemEditorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.itemEditorToolStripMenuItem.Text = "Object Tools";
            this.itemEditorToolStripMenuItem.Click += new System.EventHandler(this.ItemEditorToolStripMenuItem_Click);
            // 
            // pathEditorToolStripMenuItem
            // 
            this.pathEditorToolStripMenuItem.Name = "pathEditorToolStripMenuItem";
            this.pathEditorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.pathEditorToolStripMenuItem.Text = "Path Tools";
            this.pathEditorToolStripMenuItem.Click += new System.EventHandler(this.PathEditorToolStripMenuItem_Click);
            // 
            // debugKitToolStripMenuItem
            // 
            this.debugKitToolStripMenuItem.Name = "debugKitToolStripMenuItem";
            this.debugKitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.debugKitToolStripMenuItem.Text = "DebugKit";
            this.debugKitToolStripMenuItem.Click += new System.EventHandler(this.debugKitToolStripMenuItem_Click);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.Location = new System.Drawing.Point(216, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(198, 104);
            this.label5.TabIndex = 32;
            this.label5.Text = "Triclon\r\nqueueRAM\r\nRain\r\nshygoo\r\nHootHoot\r\nDaniel McCarthy\r\ndwmkerr";
            // 
            // TarmacAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 257);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TarmacAbout";
            this.Text = "About";
            this.Load += new System.EventHandler(this.OKAbout_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolsStripMenu;
        private System.Windows.Forms.ToolStripMenuItem textureListsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem modelCompilerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textureCompilerToolStripMenuItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem DebugStripMenu;
        private System.Windows.Forms.ToolStripMenuItem racerEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem courseImporterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vertConverterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pathEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugKitToolStripMenuItem;
    }
}
