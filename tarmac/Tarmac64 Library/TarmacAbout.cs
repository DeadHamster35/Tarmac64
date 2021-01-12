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
            
            f2.Show();
        }

      
        private void modelCompilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tarmac64_Library.ModelCompiler f2 = new Tarmac64_Library.ModelCompiler();
            f2.Show();
        }

        private void textureCompilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tarmac64_Library.TextureCompiler f2 = new Tarmac64_Library.TextureCompiler();
            f2.Show();
        }

    }
}
