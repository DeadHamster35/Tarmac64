using System;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Be.IO;
using System.Diagnostics;

namespace OverKart64
{
    public partial class VertConvert : Form
    {
        public VertConvert()
        {
            InitializeComponent();
        }


        OpenFileDialog vertopen = new OpenFileDialog();
        SaveFileDialog vertsave = new SaveFileDialog();

        private void Button1_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                string filePath = vertopen.FileName;
                Int16 value16 = new Int16();
                Int16 value8 = new Int16();
                if (vertsave.ShowDialog() == DialogResult.OK)
                {
                    

                    

                    using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var ms = new MemoryStream())
                    using (var ds = new MemoryStream())
                    using (var bw = new BeBinaryWriter(ds))
                    using (var br = new BeBinaryReader(ms))
                    {

                        ds.Position = 0;

                        
                        fs.CopyTo(ms);
                        ms.Position = 0;

                        for (int i = 0; i != fs.Length; i = i + 14)
                        {
                            
                            value16 = br.ReadInt16();
                            System.IO.File.AppendAllText(vertsave.FileName, value16.ToString() + Environment.NewLine);                            
                            value16 = br.ReadInt16();
                            System.IO.File.AppendAllText(vertsave.FileName, value16.ToString() + Environment.NewLine);
                            value16 = br.ReadInt16();
                            System.IO.File.AppendAllText(vertsave.FileName, value16.ToString() + Environment.NewLine);
                            value16 = br.ReadInt16();
                            value16 = br.ReadInt16();
                            value8 = br.ReadByte();
                            value8 = br.ReadByte();
                            value8 = br.ReadByte();
                            value8 = br.ReadByte();

                        }
                        
                    }

                    
                }
            }


        }
    }
}
