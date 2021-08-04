using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Tarmac64_Library
{
    public partial class SongExtractor : Form
    {
        public SongExtractor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Select Source ROM");
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    TM64_Sound TarmacSound = new TM64_Sound();
                    TM64 Tarmac = new TM64();
                    TM64_Sound.OK64Song ExportSong = TarmacSound.ExtractSong(openFile.FileName, Convert.ToInt32(bankBox.Text), Convert.ToInt32(seqBox.Text));
                    File.WriteAllBytes(saveFile.FileName, Tarmac.CompressMIO0(TarmacSound.SaveSong(TarmacSound.ExtractSong(openFile.FileName, Convert.ToInt32(bankBox.Text), Convert.ToInt32(seqBox.Text)))));
                }
                        
            }
        }
    }
}
