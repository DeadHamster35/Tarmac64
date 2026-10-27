namespace Tarmac64_Retail
{
    partial class HelpWindow
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
            this.NavPanel = new System.Windows.Forms.Panel();
            this.ForwardButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.SplitContainer = new System.Windows.Forms.SplitContainer();
            this.ContentsTree = new System.Windows.Forms.TreeView();
            this.SearchBox = new System.Windows.Forms.TextBox();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.SummaryLabel = new System.Windows.Forms.Label();
            this.FieldList = new System.Windows.Forms.ListView();
            this.FieldColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FormatColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UsageColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DescriptionBox = new System.Windows.Forms.TextBox();
            this.SeeAlsoPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.NavPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.Panel1.SuspendLayout();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // NavPanel
            // 
            this.NavPanel.Controls.Add(this.ForwardButton);
            this.NavPanel.Controls.Add(this.BackButton);
            this.NavPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NavPanel.Location = new System.Drawing.Point(0, 0);
            this.NavPanel.Name = "NavPanel";
            this.NavPanel.Size = new System.Drawing.Size(900, 32);
            this.NavPanel.TabIndex = 0;
            // 
            // BackButton
            // 
            this.BackButton.Location = new System.Drawing.Point(8, 4);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(75, 23);
            this.BackButton.TabIndex = 0;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // ForwardButton
            // 
            this.ForwardButton.Location = new System.Drawing.Point(89, 4);
            this.ForwardButton.Name = "ForwardButton";
            this.ForwardButton.Size = new System.Drawing.Size(75, 23);
            this.ForwardButton.TabIndex = 1;
            this.ForwardButton.Text = "Forward";
            this.ForwardButton.UseVisualStyleBackColor = true;
            this.ForwardButton.Click += new System.EventHandler(this.ForwardButton_Click);
            // 
            // SplitContainer
            // 
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.Location = new System.Drawing.Point(0, 32);
            this.SplitContainer.Name = "SplitContainer";
            // 
            // SplitContainer.Panel1
            // 
            this.SplitContainer.Panel1.Controls.Add(this.ContentsTree);
            this.SplitContainer.Panel1.Controls.Add(this.SearchBox);
            this.SplitContainer.Panel1MinSize = 140;
            // 
            // SplitContainer.Panel2
            // 
            this.SplitContainer.Panel2.Controls.Add(this.FieldList);
            this.SplitContainer.Panel2.Controls.Add(this.DescriptionBox);
            this.SplitContainer.Panel2.Controls.Add(this.SeeAlsoPanel);
            this.SplitContainer.Panel2.Controls.Add(this.SummaryLabel);
            this.SplitContainer.Panel2.Controls.Add(this.TitleLabel);
            this.SplitContainer.Panel2MinSize = 280;
            this.SplitContainer.Size = new System.Drawing.Size(900, 528);
            this.SplitContainer.SplitterDistance = 240;
            this.SplitContainer.TabIndex = 1;
            // 
            // ContentsTree
            // 
            this.ContentsTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentsTree.FullRowSelect = true;
            this.ContentsTree.HideSelection = false;
            this.ContentsTree.Location = new System.Drawing.Point(0, 0);
            this.ContentsTree.Name = "ContentsTree";
            this.ContentsTree.Size = new System.Drawing.Size(240, 505);
            this.ContentsTree.TabIndex = 0;
            this.ContentsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ContentsTree_AfterSelect);
            // 
            // SearchBox
            // 
            this.SearchBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SearchBox.Location = new System.Drawing.Point(0, 505);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(240, 20);
            this.SearchBox.TabIndex = 1;
            this.SearchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = false;
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(0, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Padding = new System.Windows.Forms.Padding(8, 6, 8, 0);
            this.TitleLabel.Size = new System.Drawing.Size(656, 28);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Title";
            // 
            // SummaryLabel
            // 
            this.SummaryLabel.AutoSize = false;
            this.SummaryLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SummaryLabel.Location = new System.Drawing.Point(0, 28);
            this.SummaryLabel.Name = "SummaryLabel";
            this.SummaryLabel.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.SummaryLabel.Size = new System.Drawing.Size(656, 80);
            this.SummaryLabel.TabIndex = 1;
            this.SummaryLabel.Text = "Summary";
            // 
            // FieldList
            // 
            this.FieldList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FieldColumn,
            this.FormatColumn,
            this.UsageColumn});
            this.FieldList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FieldList.FullRowSelect = true;
            this.FieldList.GridLines = true;
            this.FieldList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.FieldList.HideSelection = false;
            this.FieldList.Location = new System.Drawing.Point(0, 92);
            this.FieldList.MultiSelect = false;
            this.FieldList.Name = "FieldList";
            this.FieldList.Size = new System.Drawing.Size(656, 328);
            this.FieldList.TabIndex = 2;
            this.FieldList.UseCompatibleStateImageBehavior = false;
            this.FieldList.View = System.Windows.Forms.View.Details;
            this.FieldList.SelectedIndexChanged += new System.EventHandler(this.FieldList_SelectedIndexChanged);
            // 
            // FieldColumn
            // 
            this.FieldColumn.Text = "Field";
            this.FieldColumn.Width = 140;
            // 
            // FormatColumn
            // 
            this.FormatColumn.Text = "Kind";
            this.FormatColumn.Width = 90;
            // 
            // UsageColumn
            // 
            this.UsageColumn.Text = "What it does";
            this.UsageColumn.Width = 470;
            // 
            // DescriptionBox
            // 
            this.DescriptionBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DescriptionBox.Location = new System.Drawing.Point(0, 360);
            this.DescriptionBox.Multiline = true;
            this.DescriptionBox.Name = "DescriptionBox";
            this.DescriptionBox.ReadOnly = true;
            this.DescriptionBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DescriptionBox.Size = new System.Drawing.Size(656, 140);
            this.DescriptionBox.TabIndex = 3;
            // 
            // SeeAlsoPanel
            // 
            this.SeeAlsoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SeeAlsoPanel.Location = new System.Drawing.Point(0, 500);
            this.SeeAlsoPanel.Name = "SeeAlsoPanel";
            this.SeeAlsoPanel.Size = new System.Drawing.Size(656, 28);
            this.SeeAlsoPanel.TabIndex = 4;
            this.SeeAlsoPanel.WrapContents = true;
            // 
            // HelpWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 560);
            this.Controls.Add(this.SplitContainer);
            this.Controls.Add(this.NavPanel);
            this.MinimumSize = new System.Drawing.Size(700, 450);
            this.Name = "HelpWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tarmac 64 Help";
            this.NavPanel.ResumeLayout(false);
            this.SplitContainer.Panel1.ResumeLayout(false);
            this.SplitContainer.Panel1.PerformLayout();
            this.SplitContainer.Panel2.ResumeLayout(false);
            this.SplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel NavPanel;
        private System.Windows.Forms.Button ForwardButton;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.SplitContainer SplitContainer;
        private System.Windows.Forms.TreeView ContentsTree;
        private System.Windows.Forms.TextBox SearchBox;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label SummaryLabel;
        private System.Windows.Forms.ListView FieldList;
        private System.Windows.Forms.ColumnHeader FieldColumn;
        private System.Windows.Forms.ColumnHeader FormatColumn;
        private System.Windows.Forms.ColumnHeader UsageColumn;
        private System.Windows.Forms.TextBox DescriptionBox;
        private System.Windows.Forms.FlowLayoutPanel SeeAlsoPanel;
    }
}
