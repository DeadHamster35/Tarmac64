using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Tarmac64_Retail
{
    public partial class HelpWindow : Form
    {
        private static HelpWindow instance;
        private List<string> history = new List<string>();
        private int historyIndex = -1;
        private bool navigating;
        private string currentTopicId;

        public HelpWindow()
        {
            InitializeComponent();
            BuildTree("");
            UpdateNavButtons();
        }

        public static void ShowTopic(string topicId, string controlName = null)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new HelpWindow();
                instance.FormClosed += delegate { instance = null; };
                instance.Show();
            }
            else
            {
                if (instance.WindowState == FormWindowState.Minimized)
                {
                    instance.WindowState = FormWindowState.Normal;
                }
                instance.BringToFront();
                instance.Activate();
            }
            instance.NavigateTo(topicId, controlName, true);
        }

        public static void ShowForControl(string rootTopicId, Control focused)
        {
            string controlName = null;
            if (focused != null)
            {
                controlName = focused.Name;
            }
            HelpTopic topic;
            HelpField field;
            if (HelpContent.FindField(rootTopicId, controlName, out topic, out field))
            {
                ShowTopic(topic.Id, controlName);
                return;
            }
            ShowTopic(rootTopicId, null);
        }

        private void NavigateTo(string topicId, string controlName, bool addHistory)
        {
            HelpTopic topic = HelpContent.GetTopic(topicId);
            if (topic == null)
            {
                return;
            }

            if (addHistory)
            {
                if (historyIndex >= 0 && historyIndex < history.Count - 1)
                {
                    history.RemoveRange(historyIndex + 1, history.Count - historyIndex - 1);
                }
                if (history.Count == 0 || historyIndex < 0 || history[historyIndex] != topicId)
                {
                    history.Add(topicId);
                    historyIndex = history.Count - 1;
                }
            }

            currentTopicId = topicId;
            navigating = true;
            SelectTreeNode(topicId);
            DisplayTopic(topic, controlName);
            navigating = false;
            UpdateNavButtons();
        }

        private void DisplayTopic(HelpTopic topic, string controlName)
        {
            TitleLabel.Text = topic.Title;
            SummaryLabel.Text = topic.Summary;
            if (string.IsNullOrEmpty(topic.Summary))
            {
                SummaryLabel.Height = 0;
            }
            else
            {
                int innerWidth = SummaryLabel.ClientSize.Width - SummaryLabel.Padding.Horizontal;
                if (innerWidth < 40)
                {
                    innerWidth = 40;
                }
                Size measured = TextRenderer.MeasureText(topic.Summary, SummaryLabel.Font, new Size(innerWidth, int.MaxValue), TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
                SummaryLabel.Height = measured.Height + SummaryLabel.Padding.Vertical + 8;
            }

            FieldList.BeginUpdate();
            FieldList.Items.Clear();
            DescriptionBox.Text = "";
            if (topic.Fields != null)
            {
                for (int i = 0; i < topic.Fields.Count; i++)
                {
                    HelpField field = topic.Fields[i];
                    ListViewItem item = new ListViewItem(field.Name);
                    item.SubItems.Add(field.Format);
                    item.SubItems.Add(FirstLine(field.Usage));
                    item.Tag = field;
                    FieldList.Items.Add(item);
                }
            }
            FieldList.EndUpdate();
            SelectFieldRow(controlName);
            RebuildSeeAlso(topic);
        }

        private void SelectFieldRow(string controlName)
        {
            FieldList.SelectedItems.Clear();
            if (string.IsNullOrEmpty(controlName))
            {
                return;
            }
            for (int i = 0; i < FieldList.Items.Count; i++)
            {
                HelpField field = FieldList.Items[i].Tag as HelpField;
                if (field == null || field.ControlNames == null)
                {
                    continue;
                }
                for (int c = 0; c < field.ControlNames.Length; c++)
                {
                    if (field.ControlNames[c] == controlName)
                    {
                        FieldList.Items[i].Selected = true;
                        FieldList.Items[i].EnsureVisible();
                        return;
                    }
                }
            }
        }

        private void RebuildSeeAlso(HelpTopic topic)
        {
            SeeAlsoPanel.SuspendLayout();
            SeeAlsoPanel.Controls.Clear();
            bool any = false;
            if (topic.SeeAlso != null)
            {
                for (int i = 0; i < topic.SeeAlso.Count; i++)
                {
                    HelpTopic linked = HelpContent.GetTopic(topic.SeeAlso[i]);
                    if (linked == null)
                    {
                        continue;
                    }
                    if (!any)
                    {
                        Label prefix = new Label();
                        prefix.AutoSize = true;
                        prefix.Margin = new Padding(0, 6, 4, 0);
                        prefix.Text = "See also:";
                        SeeAlsoPanel.Controls.Add(prefix);
                        any = true;
                    }
                    LinkLabel link = new LinkLabel();
                    link.AutoSize = true;
                    link.Margin = new Padding(4, 6, 4, 0);
                    link.Text = linked.Title;
                    link.Tag = linked.Id;
                    link.LinkClicked += SeeAlsoLink_LinkClicked;
                    SeeAlsoPanel.Controls.Add(link);
                }
            }
            SeeAlsoPanel.Visible = any;
            SeeAlsoPanel.ResumeLayout();
        }

        private void SeeAlsoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel link = sender as LinkLabel;
            if (link != null && link.Tag is string)
            {
                NavigateTo((string)link.Tag, null, true);
            }
        }

        private void SelectTreeNode(string topicId)
        {
            TreeNode match = FindNode(ContentsTree.Nodes, topicId);
            if (match != null)
            {
                ContentsTree.SelectedNode = match;
                match.EnsureVisible();
            }
        }

        private TreeNode FindNode(TreeNodeCollection nodes, string topicId)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if ((string)nodes[i].Tag == topicId)
                {
                    return nodes[i];
                }
                TreeNode child = FindNode(nodes[i].Nodes, topicId);
                if (child != null)
                {
                    return child;
                }
            }
            return null;
        }

        private bool TopicMatches(HelpTopic topic, string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return true;
            }
            string lower = filter.ToLowerInvariant();
            if (topic.Title != null && topic.Title.ToLowerInvariant().Contains(lower))
            {
                return true;
            }
            if (topic.Summary != null && topic.Summary.ToLowerInvariant().Contains(lower))
            {
                return true;
            }
            if (topic.Fields != null)
            {
                for (int i = 0; i < topic.Fields.Count; i++)
                {
                    HelpField field = topic.Fields[i];
                    if (field.Name != null && field.Name.ToLowerInvariant().Contains(lower))
                    {
                        return true;
                    }
                    if (field.Usage != null && field.Usage.ToLowerInvariant().Contains(lower))
                    {
                        return true;
                    }
                    if (field.Format != null && field.Format.ToLowerInvariant().Contains(lower))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void BuildTree(string filter)
        {
            ContentsTree.BeginUpdate();
            ContentsTree.Nodes.Clear();
            Dictionary<string, TreeNode> nodes = new Dictionary<string, TreeNode>();
            List<string> visible = new List<string>();

            if (string.IsNullOrEmpty(filter))
            {
                for (int i = 0; i < HelpContent.Topics.Count; i++)
                {
                    visible.Add(HelpContent.Topics[i].Id);
                }
            }
            else
            {
                for (int i = 0; i < HelpContent.Topics.Count; i++)
                {
                    HelpTopic topic = HelpContent.Topics[i];
                    if (TopicMatches(topic, filter))
                    {
                        string id = topic.Id;
                        while (!string.IsNullOrEmpty(id) && !visible.Contains(id))
                        {
                            visible.Add(id);
                            HelpTopic walk = HelpContent.GetTopic(id);
                            if (walk == null)
                            {
                                break;
                            }
                            id = walk.ParentId;
                        }
                    }
                }
            }

            for (int i = 0; i < HelpContent.Topics.Count; i++)
            {
                HelpTopic topic = HelpContent.Topics[i];
                if (!visible.Contains(topic.Id))
                {
                    continue;
                }
                TreeNode node = new TreeNode(topic.Title);
                node.Tag = topic.Id;
                nodes[topic.Id] = node;
                if (string.IsNullOrEmpty(topic.ParentId) || !nodes.ContainsKey(topic.ParentId))
                {
                    ContentsTree.Nodes.Add(node);
                }
                else
                {
                    nodes[topic.ParentId].Nodes.Add(node);
                }
            }

            ContentsTree.ExpandAll();
            ContentsTree.EndUpdate();
        }

        private void UpdateNavButtons()
        {
            BackButton.Enabled = historyIndex > 0;
            ForwardButton.Enabled = historyIndex >= 0 && historyIndex < history.Count - 1;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (historyIndex > 0)
            {
                historyIndex--;
                NavigateTo(history[historyIndex], null, false);
            }
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            if (historyIndex >= 0 && historyIndex < history.Count - 1)
            {
                historyIndex++;
                NavigateTo(history[historyIndex], null, false);
            }
        }

        private void ContentsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (navigating || e.Node == null || e.Node.Tag == null)
            {
                return;
            }
            NavigateTo((string)e.Node.Tag, null, true);
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            BuildTree(SearchBox.Text);
            if (!string.IsNullOrEmpty(currentTopicId))
            {
                navigating = true;
                SelectTreeNode(currentTopicId);
                navigating = false;
            }
        }

        private static string FirstLine(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            int n = text.IndexOf('\n');
            if (n < 0)
            {
                return text;
            }
            if (n > 0 && text[n - 1] == '\r')
            {
                n--;
            }
            return text.Substring(0, n);
        }

        private void FieldList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FieldList.SelectedItems.Count == 0)
            {
                DescriptionBox.Text = "";
                return;
            }
            HelpField field = FieldList.SelectedItems[0].Tag as HelpField;
            if (field == null)
            {
                DescriptionBox.Text = "";
                return;
            }
            DescriptionBox.Text = field.Name + Environment.NewLine + Environment.NewLine + field.Usage;
        }
    }
}
