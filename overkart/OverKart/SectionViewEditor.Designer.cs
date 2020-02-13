namespace OverKart64
{
    partial class SectionViewEditor
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
            this.loadbtn = new System.Windows.Forms.Button();
            this.sectionbox = new System.Windows.Forms.ComboBox();
            this.exportbtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.removebtn = new System.Windows.Forms.Button();
            this.addbtn = new System.Windows.Forms.Button();
            this.sectiondisplay = new System.Windows.Forms.TextBox();
            this.svlload = new System.Windows.Forms.Button();
            this.objectlistbox = new System.Windows.Forms.CheckedListBox();
            this.viewbox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.countdisplay = new System.Windows.Forms.TextBox();
            this.facedisplay = new System.Windows.Forms.TextBox();
            this.vertdisplay = new System.Windows.Forms.TextBox();
            this.updatelistbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loadbtn
            // 
            this.loadbtn.Location = new System.Drawing.Point(12, 12);
            this.loadbtn.Name = "loadbtn";
            this.loadbtn.Size = new System.Drawing.Size(75, 23);
            this.loadbtn.TabIndex = 1;
            this.loadbtn.Text = "Load .FBX";
            this.loadbtn.UseVisualStyleBackColor = true;
            this.loadbtn.Click += new System.EventHandler(this.Loadbtn_Click);
            // 
            // sectionbox
            // 
            this.sectionbox.Enabled = false;
            this.sectionbox.FormattingEnabled = true;
            this.sectionbox.Location = new System.Drawing.Point(186, 116);
            this.sectionbox.Name = "sectionbox";
            this.sectionbox.Size = new System.Drawing.Size(125, 21);
            this.sectionbox.TabIndex = 2;
            this.sectionbox.SelectedIndexChanged += new System.EventHandler(this.Sectionbox_SelectedIndexChanged);
            // 
            // exportbtn
            // 
            this.exportbtn.Enabled = false;
            this.exportbtn.Location = new System.Drawing.Point(236, 12);
            this.exportbtn.Name = "exportbtn";
            this.exportbtn.Size = new System.Drawing.Size(75, 23);
            this.exportbtn.TabIndex = 3;
            this.exportbtn.Text = "Export .SVL";
            this.exportbtn.UseVisualStyleBackColor = true;
            this.exportbtn.Click += new System.EventHandler(this.Exportbtn_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(186, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "Section";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // removebtn
            // 
            this.removebtn.Enabled = false;
            this.removebtn.Location = new System.Drawing.Point(250, 73);
            this.removebtn.Name = "removebtn";
            this.removebtn.Size = new System.Drawing.Size(61, 23);
            this.removebtn.TabIndex = 5;
            this.removebtn.Text = "Remove";
            this.removebtn.UseVisualStyleBackColor = true;
            this.removebtn.Click += new System.EventHandler(this.Removebtn_Click);
            // 
            // addbtn
            // 
            this.addbtn.Enabled = false;
            this.addbtn.Location = new System.Drawing.Point(186, 73);
            this.addbtn.Name = "addbtn";
            this.addbtn.Size = new System.Drawing.Size(61, 23);
            this.addbtn.TabIndex = 6;
            this.addbtn.Text = "Add";
            this.addbtn.UseVisualStyleBackColor = true;
            this.addbtn.Click += new System.EventHandler(this.Addbtn_Click);
            // 
            // sectiondisplay
            // 
            this.sectiondisplay.Enabled = false;
            this.sectiondisplay.Location = new System.Drawing.Point(186, 213);
            this.sectiondisplay.Name = "sectiondisplay";
            this.sectiondisplay.Size = new System.Drawing.Size(125, 20);
            this.sectiondisplay.TabIndex = 7;
            this.sectiondisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // svlload
            // 
            this.svlload.Enabled = false;
            this.svlload.Location = new System.Drawing.Point(105, 12);
            this.svlload.Name = "svlload";
            this.svlload.Size = new System.Drawing.Size(75, 23);
            this.svlload.TabIndex = 8;
            this.svlload.Text = "Load .SVL";
            this.svlload.UseVisualStyleBackColor = true;
            this.svlload.Click += new System.EventHandler(this.Svlload_Click);
            // 
            // objectlistbox
            // 
            this.objectlistbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.objectlistbox.FormattingEnabled = true;
            this.objectlistbox.Location = new System.Drawing.Point(13, 41);
            this.objectlistbox.Name = "objectlistbox";
            this.objectlistbox.Size = new System.Drawing.Size(168, 349);
            this.objectlistbox.TabIndex = 9;
            // 
            // viewbox
            // 
            this.viewbox.Enabled = false;
            this.viewbox.FormattingEnabled = true;
            this.viewbox.Items.AddRange(new object[] {
            "North",
            "East",
            "South",
            "West"});
            this.viewbox.Location = new System.Drawing.Point(186, 157);
            this.viewbox.Name = "viewbox";
            this.viewbox.Size = new System.Drawing.Size(125, 21);
            this.viewbox.TabIndex = 10;
            this.viewbox.SelectedIndexChanged += new System.EventHandler(this.Viewbox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(187, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 14);
            this.label2.TabIndex = 11;
            this.label2.Text = "View";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // countdisplay
            // 
            this.countdisplay.Enabled = false;
            this.countdisplay.Location = new System.Drawing.Point(186, 239);
            this.countdisplay.Name = "countdisplay";
            this.countdisplay.Size = new System.Drawing.Size(125, 20);
            this.countdisplay.TabIndex = 12;
            this.countdisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // facedisplay
            // 
            this.facedisplay.Enabled = false;
            this.facedisplay.Location = new System.Drawing.Point(186, 265);
            this.facedisplay.Name = "facedisplay";
            this.facedisplay.Size = new System.Drawing.Size(125, 20);
            this.facedisplay.TabIndex = 13;
            this.facedisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // vertdisplay
            // 
            this.vertdisplay.Enabled = false;
            this.vertdisplay.Location = new System.Drawing.Point(186, 291);
            this.vertdisplay.Name = "vertdisplay";
            this.vertdisplay.Size = new System.Drawing.Size(125, 20);
            this.vertdisplay.TabIndex = 14;
            this.vertdisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // updatelistbtn
            // 
            this.updatelistbtn.Enabled = false;
            this.updatelistbtn.Location = new System.Drawing.Point(186, 339);
            this.updatelistbtn.Name = "updatelistbtn";
            this.updatelistbtn.Size = new System.Drawing.Size(125, 23);
            this.updatelistbtn.TabIndex = 15;
            this.updatelistbtn.Text = "Update Section-View";
            this.updatelistbtn.UseVisualStyleBackColor = true;
            this.updatelistbtn.Click += new System.EventHandler(this.updatebtn);
            // 
            // SectionViewEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 412);
            this.Controls.Add(this.updatelistbtn);
            this.Controls.Add(this.vertdisplay);
            this.Controls.Add(this.facedisplay);
            this.Controls.Add(this.countdisplay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.viewbox);
            this.Controls.Add(this.objectlistbox);
            this.Controls.Add(this.svlload);
            this.Controls.Add(this.sectiondisplay);
            this.Controls.Add(this.addbtn);
            this.Controls.Add(this.removebtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exportbtn);
            this.Controls.Add(this.sectionbox);
            this.Controls.Add(this.loadbtn);
            this.MaximumSize = new System.Drawing.Size(340, 9999);
            this.MinimumSize = new System.Drawing.Size(340, 210);
            this.Name = "SectionViewEditor";
            this.Text = "Section-View Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button loadbtn;
        private System.Windows.Forms.ComboBox sectionbox;
        private System.Windows.Forms.Button exportbtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button removebtn;
        private System.Windows.Forms.Button addbtn;
        private System.Windows.Forms.TextBox sectiondisplay;
        private System.Windows.Forms.Button svlload;
        private System.Windows.Forms.CheckedListBox objectlistbox;
        private System.Windows.Forms.ComboBox viewbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox countdisplay;
        private System.Windows.Forms.TextBox facedisplay;
        private System.Windows.Forms.TextBox vertdisplay;
        private System.Windows.Forms.Button updatelistbtn;
    }
}