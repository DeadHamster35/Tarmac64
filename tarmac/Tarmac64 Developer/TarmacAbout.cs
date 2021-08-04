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
using Tarmac64_Library;
using Tarmac64;





namespace Tarmac64
{
    public partial class TarmacAbout : Form

    {

        


        DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;


        public TarmacAbout()
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

        

        private void NotForRetailToolStripMenuItem_Click(object sender, EventArgs e)
        {

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


        private void SegmentToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SegmentCompiler f2 = new SegmentCompiler();
            f2.ShowDialog();
        }

        private void GeometryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CourseCompiler f2 = new CourseCompiler();
            
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

        private void debugTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderDebug f2 = new renderDebug();
            f2.ShowDialog();
        }

        private void modelCompilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tarmac64_Library.ModelCompiler f2 = new Tarmac64_Library.ModelCompiler();
            f2.ShowDialog();
        }

        private void textureCompilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tarmac64_Library.TextureCompiler f2 = new Tarmac64_Library.TextureCompiler();
            f2.ShowDialog();
        }

        private void debugKitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebugTools f2 = new DebugTools();
            f2.ShowDialog();
        }

        private void courseImporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SegmentCompiler f2 = new SegmentCompiler();
            f2.ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void extractSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongExtractor f2 = new SongExtractor();
            f2.ShowDialog();
        }

        private void resetFoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TM64 Tarmac = new TM64();

            string message = "Reset your Tarmac Folders?";
            string caption = "Tarmac64";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            if (MessageBox.Show(message, caption, buttons) == System.Windows.Forms.DialogResult.Yes)
            {
                Tarmac.LoadSettings(true);
            }
        }

        private void courseLoaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameBuilder f2 = new GameBuilder();
            f2.ShowDialog();
        }

        private void objectTypeCompilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tarmac64_Library.ObjectTypeCompiler f2 = new ObjectTypeCompiler();
            f2.ShowDialog();
        }
    }
}
