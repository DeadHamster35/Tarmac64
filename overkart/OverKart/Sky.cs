using System;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Be.IO;
using System.Diagnostics;

namespace OverKart64
{
    public partial class Sky : Form
    {
        public Sky()
        {
            InitializeComponent();
        }


        byte[] bluetop = new byte[21];
        byte[] redtop = new byte[21];
        byte[] greentop = new byte[21];
        byte[] bluebot = new byte[21];
        byte[] redbot = new byte[21];
        byte[] greenbot = new byte[21];

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

                string[] items = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut", "Award Ceremony" };

                foreach (string item in items)
                {
                    coursebox.Items.Add(item);
                }
                coursebox.SelectedIndex = 0;

                if (filetype == "n64" || filetype == "v64")
                {
                    MessageBox.Show("Only Supports .Z64 format");
                }
                else
                {
                    using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var ms = new MemoryStream())
                    using (var bw = new BeBinaryWriter(ms))
                    using (var br = new BeBinaryReader(ms))

                    {
                        fs.CopyTo(ms);
                        ms.Position = 0;




                        br.BaseStream.Seek(1188064, SeekOrigin.Begin);

                        for (int n = 0; n < 21; n++)
                        {
                            br.BaseStream.Seek(1, SeekOrigin.Current);
                            redtop[n] = br.ReadByte();
                            br.BaseStream.Seek(1, SeekOrigin.Current);
                            greentop[n] = br.ReadByte();
                            br.BaseStream.Seek(1, SeekOrigin.Current);
                            bluetop[n] = br.ReadByte();
                            br.BaseStream.Seek(1, SeekOrigin.Current);
                            redbot[n] = br.ReadByte();
                            br.BaseStream.Seek(1, SeekOrigin.Current);
                            greenbot[n] = br.ReadByte();
                            br.BaseStream.Seek(1, SeekOrigin.Current);
                            bluebot[n] = br.ReadByte();

                        }


                        rtbox.Enabled = true;
                        gtbox.Enabled = true;
                        btbox.Enabled = true;
                        rbbox.Enabled = true;
                        gbbox.Enabled = true;
                        bbox.Enabled = true;
                        cpbot.Enabled = true;
                        cptop.Enabled = true;

                        rtbox.Text = redtop[0].ToString();
                        gtbox.Text = greentop[0].ToString();
                        btbox.Text = bluetop[0].ToString();
                        rbbox.Text = redbot[0].ToString();
                        gbbox.Text = greenbot[0].ToString();
                        bbox.Text = bluebot[0].ToString();



                        savebtn.Enabled = true;
                        br.Close();
                        bw.Close();
                        ms.Close();
                        fs.Close();


                    }
                }
            }
        }

        private void Sky_Load(object sender, EventArgs e)
        {
            
        }

        private void Button2_Click(object sender, EventArgs e)
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
                    using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var ds = new FileStream(rompath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var ms = new MemoryStream())
                    using (var bw = new BeBinaryWriter(ms))
                    using (var br = new BeBinaryReader(ms))

                    {

                        fs.CopyTo(ds);
                        ds.Position = 0;
                        ds.CopyTo(ms);

                        ms.Position = 0;




                        br.BaseStream.Seek(1188064, SeekOrigin.Begin);

                        for (int n = 0; n < 21; n++)
                        {
                            bw.BaseStream.Seek(1, SeekOrigin.Current);
                            bw.Write(redtop[n]);
                            br.BaseStream.Seek(1, SeekOrigin.Current);
                            bw.Write(greentop[n]);
                            br.BaseStream.Seek(1, SeekOrigin.Current);
                            bw.Write(bluetop[n]);
                            br.BaseStream.Seek(1, SeekOrigin.Current);
                            bw.Write(redbot[n]);
                            br.BaseStream.Seek(1, SeekOrigin.Current);
                            bw.Write(greenbot[n]);
                            br.BaseStream.Seek(1, SeekOrigin.Current);
                            bw.Write(bluebot[n]);

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

        private void Rtbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(rtbox.Text, out redtop[coursebox.SelectedIndex]);
            colorupdate();
        }

        private void Rbbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(rbbox.Text, out redbot[coursebox.SelectedIndex]);
            colorupdate();
        }

        private void Gtbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(gtbox.Text, out greentop[coursebox.SelectedIndex]);
            colorupdate();
        }

        private void Gbbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(gbbox.Text, out greenbot[coursebox.SelectedIndex]);
            colorupdate();
        }

        private void Btbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(btbox.Text, out bluetop[coursebox.SelectedIndex]);
            colorupdate();
        }

        private void Bbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(bbox.Text, out bluebot[coursebox.SelectedIndex]);
            colorupdate();
        }

        private void Coursebox_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtbox.Text = redtop[coursebox.SelectedIndex].ToString();
            gtbox.Text = greentop[coursebox.SelectedIndex].ToString();
            btbox.Text = bluetop[coursebox.SelectedIndex].ToString();
            rbbox.Text = redbot[coursebox.SelectedIndex].ToString();
            gbbox.Text = greenbot[coursebox.SelectedIndex].ToString();
            bbox.Text = bluebot[coursebox.SelectedIndex].ToString();
        }

        private void Cptop_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();

            MyDialog.AllowFullOpen = true;

            MyDialog.ShowHelp = true;



            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                rtbox.Text = MyDialog.Color.R.ToString();
                gtbox.Text = MyDialog.Color.G.ToString();
                btbox.Text = MyDialog.Color.B.ToString();
            }
        }

        private void Cpbot_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();

            MyDialog.AllowFullOpen = true;

            MyDialog.ShowHelp = true;

            byte r = new byte();
            byte g = new byte();
            byte b = new byte();


            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                r = MyDialog.Color.R;
                g = MyDialog.Color.G;
                b = MyDialog.Color.B;
                rbbox.Text = r.ToString();
                gbbox.Text =g.ToString();
                bbox.Text = b.ToString();
            }
        }

        private void Sky_KeyUp(object sender, KeyEventArgs e)
        {
           
        }



        private void colorupdate()
        {
            int rr = new int();
            int.TryParse(rtbox.Text, out rr);
            int gg = new int();
            int.TryParse(gtbox.Text, out gg);
            int bb = new int();
            int.TryParse(btbox.Text, out bb);
            Color tbuttoncolor = Color.FromArgb(rr,gg,bb);

            
            int.TryParse(rbbox.Text, out rr);
            
            int.TryParse(gbbox.Text, out gg);
            
            int.TryParse(bbox.Text, out bb);
            Color bbuttoncolor = Color.FromArgb(rr, gg, bb);

            cptop.BackColor = tbuttoncolor;
            cpbot.BackColor = bbuttoncolor;

        }
    }
}
