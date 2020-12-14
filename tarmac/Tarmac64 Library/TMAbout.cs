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




namespace Tarmac64
{
    public partial class OKRetail : Form

    {
        public OKRetail()
        {
            InitializeComponent();
        }


        TM64 Tarmac = new TM64();


        DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;


        

        private void GeometryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GeometryCompiler f2 = new GeometryCompiler();
            f2.ShowDialog();
        }


        private void OKAbout_Load(object sender, EventArgs e)
        {
            outLabel.Text = buildDate.ToString().Replace("/", "");
        }

        private void ROMManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void TextureListsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GeometryCompiler f2 = new GeometryCompiler();
            f2.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TM64.OK64Settings okSettings = Tarmac.LoadSettings();
        }
    }
}
