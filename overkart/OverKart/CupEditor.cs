using System;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PeepsCompress;
using System.Diagnostics;

namespace OverKart64
{
    public partial class CourseSelect : Form
    {
        public CourseSelect()
        {
            InitializeComponent();
        }

        Int16[,] courselist = new Int16[5,4];
        string[] courses = new string[20] { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut",};
        string filePath = "";
        

        OpenFileDialog romopen = new OpenFileDialog();
        SaveFileDialog romsave = new SaveFileDialog();
         
        private void Button1_Click(object sender, EventArgs e)
        {
            if (romopen.ShowDialog() == DialogResult.OK)
            {




                //Get the path of specified file
                filePath = romopen.FileName;





                string filetype = filePath.Substring(filePath.Length - 3);

                

                foreach (string item in courses)
                {
                    r0.Items.Add(item);
                    r1.Items.Add(item);
                    r2.Items.Add(item);
                    r3.Items.Add(item);
                }

                string[] cups = { "Mushroom Cup", "Flower Cup", "Star Cup", "Special Cup", "Battle Mode" };

                foreach (string item in cups)
                {
                    c1.Items.Add(item);
                }

                c1.SelectedIndex = 0;

                if (filetype == "n64" || filetype == "v64")
                {
                    MessageBox.Show("Only Supports .Z64 format");
                }
                else
                {



                    using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var ms = new MemoryStream())
                    using (var bw = new BigEndianBinaryWriter(ms))
                    using (var br = new BigEndianBinaryReader(ms))

                    {
                        fs.CopyTo(ms);
                        ms.Position = 0;





                        br.BaseStream.Seek(997300, SeekOrigin.Begin);

                        for (int n=0; n != 5; n++)
                        {
                            
                            courselist[n, 0] = br.ReadInt16();
                            courselist[n, 1] = br.ReadInt16();
                            courselist[n, 2] = br.ReadInt16();
                            courselist[n, 3] = br.ReadInt16();
                        }




                        c1.Enabled = true;
                        r0.Enabled = true;
                        r1.Enabled = true;
                        r2.Enabled = true;
                        r3.Enabled = true;

                        
                        c1.SelectedIndex = 0;
                        r0.SelectedIndex = courselist[0,0];
                        r1.SelectedIndex = courselist[0,1];
                        r2.SelectedIndex = courselist[0,2];
                        r3.SelectedIndex = courselist[0,3];



                        savebtn.Enabled = true;
                        br.Close();
                        bw.Close();
                        ms.Close();
                        fs.Close();


                    }
                }
            }
        }

        private void C1_SelectedIndexChanged(object sender, EventArgs e)
        {
            r0.SelectedIndex = courselist[c1.SelectedIndex, 0];
            r1.SelectedIndex = courselist[c1.SelectedIndex, 1];
            r2.SelectedIndex = courselist[c1.SelectedIndex, 2];
            r3.SelectedIndex = courselist[c1.SelectedIndex, 3];
        }

        private void R1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            courselist[c1.SelectedIndex, 0] = (Int16)r0.SelectedIndex;
        }

        private void R2_SelectedIndexChanged(object sender, EventArgs e)
        {
            courselist[c1.SelectedIndex, 1] = (Int16)r1.SelectedIndex;
        }

        private void R3_SelectedIndexChanged(object sender, EventArgs e)
        {
            courselist[c1.SelectedIndex, 2] = (Int16)r2.SelectedIndex;
        }

        private void R4_SelectedIndexChanged(object sender, EventArgs e)
        {
            courselist[c1.SelectedIndex, 3] = (Int16)r3.SelectedIndex;
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {


            if (romsave.ShowDialog() == DialogResult.OK)
            {
                bool overwrite = new bool();


                string rompath = romsave.FileName;
                if (rompath == filePath)
                {
                    overwrite = true;
                    rompath = "null";
                }


                string filetype = filePath.Substring(filePath.Length - 3);
                if (filetype == "n64" || filetype == "v64")
                {


                    MessageBox.Show("Only Supports .Z64 format");

                }
                else
                {
                    byte nullbyte = 0;
                    using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var ds = new FileStream(rompath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var ms = new MemoryStream())
                    using (var bw = new BigEndianBinaryWriter(ms))
                    using (var br = new BigEndianBinaryReader(ms))

                    {

                        fs.CopyTo(ds);
                        ds.Position = 0;
                        ds.CopyTo(ms);

                        ms.Position = 0;
                       

                        bw.BaseStream.Seek(997300, SeekOrigin.Begin);




                        for (int n = 0; n != 5; n++)
                        {
                            bw.Write(courselist[n, 0]);
                            bw.Write(courselist[n, 1]);
                            bw.Write(courselist[n, 2]);
                            bw.Write(courselist[n, 3]);
                        }
                        
                        ms.Position = 0;
                        ds.Position = 0;

                        ms.CopyTo(ds);
                        ds.Close();
                        ms.Close();
                        fs.Close();
                        bw.Close();
                        br.Close();


                        string command = @"rn64crc.exe";
                        string args = "";
                        if (overwrite == true)
                        {
                            args = "-u \"" + filePath + "\"";
                        }
                        else
                        {
                            args = "-u \"" + rompath + "\"";
                        }


                        Process process = new Process();
                        process.StartInfo.FileName = command;
                        process.StartInfo.Arguments = args;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();


                        MessageBox.Show("Save Completed");
                    }

                }
            }
        }

        private void CourseSelect_Load(object sender, EventArgs e)
        {
            
        }
    }
}