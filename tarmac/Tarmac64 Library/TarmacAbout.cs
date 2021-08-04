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






namespace Tarmac64_Library
{
    public partial class TarmacAbout : Form

    {

        


        DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;


        public TarmacAbout()
        {
            InitializeComponent();
        }


        private void OKAbout_Load(object sender, EventArgs e)
        {
            
            string outlabel = buildDate.ToString().Replace("/","");
            label3.Text = "OK641."+outlabel;
        }
        

        private void NotForRetailToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void GeometryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CourseCompiler f2 = new CourseCompiler();
            
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

        private void resetFoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TM64 Tarmac = new TM64();

            string message = "Reset your Tarmac Folders?";
            string caption = "Settings Dialog";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            if (MessageBox.Show(message, caption, buttons) == System.Windows.Forms.DialogResult.Yes)
            {
                Tarmac.LoadSettings(true);
            }
            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Tarmac64_Library.SongExtractor f2 = new Tarmac64_Library.SongExtractor();
            f2.ShowDialog();
        }

        private void courseLoaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tarmac64_Library.GameBuilder f2 = new Tarmac64_Library.GameBuilder();
            f2.ShowDialog();
        }

        private void extractSongToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void objectTypeCompilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tarmac64_Library.ObjectTypeCompiler f2 = new ObjectTypeCompiler();
            f2.ShowDialog();
        }
    }
}
