using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace OverKart64
{
    public partial class OKAbout : Form

    {

        


        DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;


        public OKAbout()
        {
            InitializeComponent();
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void OKAbout_Load(object sender, EventArgs e)
        {
            
            string outlabel = buildDate.ToString().Replace("/","");
            label3.Text = "OK641."+outlabel;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            
                
        }

        private void VertConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SegmentExporter f2 = new SegmentExporter();
            f2.ShowDialog();
        }

        private void SkyColorEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sky f2 = new Sky();
            f2.ShowDialog();
        }

        private void CupEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CourseSelect f2 = new CourseSelect();
            f2.ShowDialog();
        }

        

        private void NotForRetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebugTools f2 = new DebugTools();
            f2.ShowDialog();
        }

        private void VertexEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VertEditor f2 = new VertEditor();
            f2.ShowDialog();
        }

        private void PathEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PathEditor f2 = new PathEditor();
            f2.ShowDialog();
        }

        private void ItemEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectEditor f2 = new ObjectEditor();
            f2.ShowDialog();
        }

        private void TextureListsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BitmapConverter f2 = new BitmapConverter();
            f2.ShowDialog();
        }

        private void SegmentToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SegmentCompiler f2 = new SegmentCompiler();
            f2.ShowDialog();
        }

        private void GeometryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GeometryCompiler f2 = new GeometryCompiler();
            f2.ShowDialog();
        }

        private void SectionViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SectionViewEditor f2 = new SectionViewEditor();
            f2.ShowDialog();
        }

        private void RacerEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RacerEditor f2 = new RacerEditor();
            f2.ShowDialog();
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
