using System;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Media.Imaging;
using Tarmac64_Library;



//  1188752   TRACK HEADER OFFSET US ROM



namespace Tarmac64
{
    public partial class SegmentCompiler : Form
    {
        public SegmentCompiler()
        {
            InitializeComponent();
        }

        FileStream fs = new FileStream("null", FileMode.OpenOrCreate);
        MemoryStream bs = new MemoryStream();
        BinaryReader br = new BinaryReader(Stream.Null);
        BinaryWriter bw = new BinaryWriter(Stream.Null);
        MemoryStream ds = new MemoryStream();
        BinaryReader dr = new BinaryReader(Stream.Null);
        BinaryWriter dw = new BinaryWriter(Stream.Null);
        MemoryStream vs = new MemoryStream();
        BinaryReader vr = new BinaryReader(Stream.Null);

        bool debugmode = false;

        TM64 Tarmac = new TM64();
        TM64_Geometry TarmacGeometry = new TM64_Geometry();

        string filePath = "";
        string savePath = "";

        byte[] flip4 = new byte[4];
        byte[] flip2 = new byte[2];

        int[] img_types = new int[] { 0, 0, 0, 3, 3, 3 };
        int[] img_heights = new int[] { 32, 32, 64, 32, 32, 64 };
        int[] img_widths = new int[] { 32, 64, 32, 32, 64, 32 };

        int v0 = 0;
        int v1 = 0;
        int v2 = 0;


        int current_offset = 0;


        int texwidth = 0;
        int x_mask = 0;
        int x_flags = 0;
        int texheight = 0;
        int y_mask = 0;
        int y_flags = 0;
        int textype = 0;


        UInt16 value16 = new UInt16();
        UInt32 value32 = new UInt32();
        Int16 valuesign16 = new Int16();

        UInt32 seg7_addr = new UInt32();

        public static int cID = 0;

        public static UInt32[] seg6_addr = new UInt32[20];      //(0x00) ROM address at which segment 6 file begins
        public static UInt32[] seg6_end = new UInt32[20];       //(0x04) ROM address at which segment 6 file ends
        public static UInt32[] seg4_addr = new UInt32[20];      //(0x08) ROM address at which segment 4 file begins
        public static UInt32[] seg7_end = new UInt32[20];       //(0x0C) ROM address at which segment 7 (not 4) file ends
        public static UInt32[] seg9_addr = new UInt32[20];      //(0x10) ROM address at which segment 9 file begins
        public static UInt32[] seg9_end = new UInt32[20];       //(0x14) ROM address at which segment 9 file ends
        public static UInt32[] seg47_buf = new UInt32[20];      //(0x18) RSP address of compressed segments 4 and 7
        public static UInt32[] numVtxs = new UInt32[20];        //(0x1C) number of vertices in the vertex file
        public static UInt32[] seg7_ptr = new UInt32[20];       //(0x20) RSP address at which segment 7 data begins
        public static UInt32[] seg7_size = new UInt32[20];      //(0x24) Size of segment 7 data after decompression, minus 8 bytes for some reason
        public static UInt32[] texture_addr = new UInt32[20];   //(0x28) RSP address of texture list
        public static UInt16[] flag = new UInt16[20];           //(0x2C) Unknown
        public static UInt16[] unused = new UInt16[20];         //(0x2E) Padding

        public static UInt32[] seg7_romptr = new UInt32[20];    // math derived from above data.

        OpenFileDialog vertopen = new OpenFileDialog();
        SaveFileDialog vertsave = new SaveFileDialog();
        FolderBrowserDialog textsave = new FolderBrowserDialog();



        private void CloseStreams()
        {
            fs.Close();
            bs.Close();
            br.Close();
            bw.Close();
            ds.Close();
            dr.Close();
            dw.Close();
            vs.Close();
            vr.Close();
        }



        private void Load_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {


                //Get the path of specified file
                filePath = vertopen.FileName;

                //Read the contents of the file into a stream

                string filetype = filePath.Substring(filePath.Length - 3);

                if (filetype == "n64" || filetype == "v64")
                {
                    MessageBox.Show("Only Supports .Z64 format");
                }
                else
                {





                    fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    bw = new BinaryWriter(fs);
                    br = new BinaryReader(fs);
                    {
                        br.BaseStream.Seek(1188752, SeekOrigin.Begin);

                        for (int i = 0; i < 20; i++)
                        {
                            seg6_addr[i] = br.ReadUInt32();
                            seg6_end[i] = br.ReadUInt32();
                            seg4_addr[i] = br.ReadUInt32();
                            seg7_end[i] = br.ReadUInt32();
                            seg9_addr[i] = br.ReadUInt32();
                            seg9_end[i] = br.ReadUInt32();
                            seg47_buf[i] = br.ReadUInt32();
                            numVtxs[i] = br.ReadUInt32();
                            seg7_ptr[i] = br.ReadUInt32();
                            seg7_size[i] = br.ReadUInt32();
                            texture_addr[i] = br.ReadUInt32();
                            flag[i] = br.ReadUInt16();
                            unused[i] = br.ReadUInt16();




                            byte[] flip = new byte[4];

                            flip = BitConverter.GetBytes(seg6_addr[i]);
                            Array.Reverse(flip);
                            seg6_addr[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg6_end[i]);
                            Array.Reverse(flip);
                            seg6_end[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg4_addr[i]);
                            Array.Reverse(flip);
                            seg4_addr[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg7_end[i]);
                            Array.Reverse(flip);
                            seg7_end[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg9_addr[i]);
                            Array.Reverse(flip);
                            seg9_addr[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg9_end[i]);
                            Array.Reverse(flip);
                            seg9_end[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg47_buf[i]);
                            Array.Reverse(flip);
                            seg47_buf[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(numVtxs[i]);
                            Array.Reverse(flip);
                            numVtxs[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg7_ptr[i]);
                            Array.Reverse(flip);
                            seg7_ptr[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg7_size[i]);
                            Array.Reverse(flip);
                            seg7_size[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(texture_addr[i]);
                            Array.Reverse(flip);
                            texture_addr[i] = BitConverter.ToUInt32(flip, 0);

                            byte[] flop = new byte[2];

                            flop = BitConverter.GetBytes(flag[i]);
                            Array.Reverse(flop);
                            flag[i] = BitConverter.ToUInt16(flop, 0);

                            flop = BitConverter.GetBytes(unused[i]);
                            Array.Reverse(flop);
                            unused[i] = BitConverter.ToUInt16(flop, 0);



                            seg7_romptr[i] = seg7_ptr[i] - seg47_buf[i] + seg4_addr[i];

                        }




                        string[] items = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut" };

                        foreach (string item in items)
                        {
                            coursebox.Items.Add(item);

                        }


                        coursebox.SelectedIndex = 0;
                        if (debugmode == false)
                        {
                            MessageBox.Show("ROM Loaded");
                        }
                        else
                        {
                            MessageBox.Show("ROM Loaded in Debug Mode");
                        }
                        seg4box.Enabled = true;
                        seg6box.Enabled = true;
                        seg7box.Enabled = true;
                        seg9box.Enabled = true;
                    }
                }
                CloseStreams();
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateUI();
        }

        private void updateUI()
        {
            seg6a.Text = "0x|   " + seg6_addr[coursebox.SelectedIndex].ToString("X");
            seg6e.Text = "0x|   " + seg6_end[coursebox.SelectedIndex].ToString("X");
            seg4a.Text = "0x|   " + seg4_addr[coursebox.SelectedIndex].ToString("X");
            seg7e.Text = "0x|   " + seg7_end[coursebox.SelectedIndex].ToString("X");
            seg9a.Text = "0x|   " + seg9_addr[coursebox.SelectedIndex].ToString("X");
            seg9e.Text = "0x|   " + seg9_end[coursebox.SelectedIndex].ToString("X");
            seg47ra.Text = "0x|   " + seg47_buf[coursebox.SelectedIndex].ToString("X");
            nv.Text = numVtxs[coursebox.SelectedIndex].ToString();
            seg7ra.Text = "0x|   " + seg7_ptr[coursebox.SelectedIndex].ToString("X");
            seg7s.Text = "0x|   " + seg7_size[coursebox.SelectedIndex].ToString("X");
            texra.Text = "0x|   " + texture_addr[coursebox.SelectedIndex].ToString("X");
            flagbox.Text = "0x|   " + flag[coursebox.SelectedIndex].ToString("X");
            padbox.Text = "0x|   " + unused[coursebox.SelectedIndex].ToString("X");
            seg7rom.Text = "0x|   " + seg7_romptr[coursebox.SelectedIndex].ToString("X");
        }



        //Old Code Above












        private byte[] dump_seg9()
        {


            cID = coursebox.SelectedIndex;

            savePath = vertsave.FileName;
            seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];


            byte[] rombytes = File.ReadAllBytes(filePath);
            byte[] seg9 = new byte[(seg9_end[cID] - seg9_addr[cID])];

            Buffer.BlockCopy(rombytes, Convert.ToInt32(seg9_addr[cID]), seg9, 0, (Convert.ToInt32(seg9_end[cID]) - Convert.ToInt32(seg9_addr[cID])));

            return seg9;
            CloseStreams();

        }





        private byte[] dump_seg7()
        {

            cID = coursebox.SelectedIndex;

            savePath = vertsave.FileName;
            seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];
            MessageBox.Show(seg7_addr.ToString());

            byte[] rombytes = File.ReadAllBytes(filePath);
            byte[] seg7 = new byte[(seg7_end[cID] - seg7_addr)];

            Buffer.BlockCopy(rombytes, Convert.ToInt32(seg7_addr), seg7, 0, (Convert.ToInt32(seg7_end[cID]) - Convert.ToInt32(seg7_addr)));

            return seg7;
            CloseStreams();

        }



        private byte[] dump_seg4()
        {


            cID = coursebox.SelectedIndex;

            savePath = vertsave.FileName;
            seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];
            MessageBox.Show(seg7_addr.ToString());

            byte[] rombytes = File.ReadAllBytes(filePath);
            byte[] seg4 = new byte[(seg7_addr - seg4_addr[cID])];

            return seg4;
            CloseStreams();


        }

        private byte[] dump_seg6()
        {


            cID = coursebox.SelectedIndex;

            savePath = vertsave.FileName;
            seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];
            MessageBox.Show(seg7_addr.ToString());

            byte[] rombytes = File.ReadAllBytes(filePath);
            byte[] seg6 = new byte[(seg6_end[cID] - seg6_addr[cID])];

            Buffer.BlockCopy(rombytes, Convert.ToInt32(seg6_addr[cID]), seg6, 0, (Convert.ToInt32(seg6_end[cID]) - Convert.ToInt32(seg6_addr[cID])));


            return seg6;
            CloseStreams();

        }


        private void textbox_change(object sender, EventArgs e)
        {
            updateUI();
        }


        private void Seg4box_CheckedChanged(object sender, EventArgs e)
        {
            if (seg4box.CheckState == CheckState.Checked)
            {
                seg4text.Enabled = true;
                seg4btn.Enabled = true;
            }
            else
            {
                seg4text.Text = "";
                seg4text.Enabled = false;
                seg4btn.Enabled = false;
            }
        }

        private void Seg6box_CheckedChanged(object sender, EventArgs e)
        {
            if (seg6box.CheckState == CheckState.Checked)
            {
                seg6text.Enabled = true;
                seg6btn.Enabled = true;
            }
            else
            {
                seg6text.Text = "";
                seg6text.Enabled = false;
                seg6btn.Enabled = false;
            }
        }

        private void Seg7box_CheckedChanged(object sender, EventArgs e)
        {
            if (seg7box.CheckState == CheckState.Checked)
            {
                seg7text.Enabled = true;
                seg7btn.Enabled = true;
            }
            else
            {
                seg7text.Text = "";
                seg7text.Enabled = false;
                seg7btn.Enabled = false;
            }
        }

        private void Seg9box_CheckedChanged(object sender, EventArgs e)
        {
            if (seg9box.CheckState == CheckState.Checked)
            {
                seg9text.Enabled = true;
                seg9btn.Enabled = true;
            }
            else
            {
                seg9text.Text = "";
                seg9text.Enabled = false;
                seg9btn.Enabled = false;
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            byte[] rombytes = File.ReadAllBytes(filePath);
            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                byte[] seg4 = new byte[0];
                if (seg4box.CheckState == CheckState.Checked)
                {

                    byte[] tempseg = File.ReadAllBytes(seg4text.Text);
                    Array.Resize(ref seg4, tempseg.Length);
                    Array.Copy(tempseg, 0, seg4, 0, tempseg.Length);
                }
                else
                {
                    byte[] tempseg = Tarmac.Dumpseg4(cID, rombytes);
                    Array.Resize(ref seg4, tempseg.Length);
                    Array.Copy(tempseg, 0, seg4, 0, tempseg.Length);
                }
                //
                byte[] seg6 = new byte[0];
                if (seg6box.CheckState == CheckState.Checked)
                {

                    byte[] tempseg = File.ReadAllBytes(seg6text.Text);
                    Array.Resize(ref seg6, tempseg.Length);
                    Array.Copy(tempseg, 0, seg6, 0, tempseg.Length);
                }
                else
                {
                    byte[] tempseg = Tarmac.Dumpseg6(cID, rombytes);
                    Array.Resize(ref seg6, tempseg.Length);
                    Array.Copy(tempseg, 0, seg6, 0, tempseg.Length);
                }
                //
                byte[] seg7 = new byte[0];
                if (seg7box.CheckState == CheckState.Checked)
                {

                    byte[] tempseg = File.ReadAllBytes(seg7text.Text);
                    Array.Resize(ref seg7, tempseg.Length);
                    Array.Copy(tempseg, 0, seg7, 0, tempseg.Length);
                }
                else
                {
                    byte[] tempseg = Tarmac.Dumpseg7(cID, rombytes);
                    Array.Resize(ref seg7, tempseg.Length);
                    Array.Copy(tempseg, 0, seg7, 0, tempseg.Length);
                }
                //
                byte[] seg9 = new byte[0];
                if (seg9box.CheckState == CheckState.Checked)
                {

                    byte[] tempseg = File.ReadAllBytes(seg9text.Text);
                    Array.Resize(ref seg9, tempseg.Length);
                    Array.Copy(tempseg, 0, seg9, 0, tempseg.Length);
                }
                else
                {
                    byte[] tempseg = Tarmac.Dumpseg9(cID, rombytes);
                    Array.Resize(ref seg9, tempseg.Length);
                    Array.Copy(tempseg, 0, seg9, 0, tempseg.Length);
                }
                string outpath = vertsave.FileName;
                
                byte[] newROM = Tarmac.CompileSegment(seg4, seg6, seg7, seg9, rombytes, coursebox.SelectedIndex);
                File.WriteAllBytes(vertsave.FileName, newROM);
                MessageBox.Show("Finished");
            }
        }

        private void Seg4btn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                seg4text.Text = vertopen.FileName;
            }


        }

        private void Seg6btn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                seg6text.Text = vertopen.FileName;
            }
        }

        private void Seg7btn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                seg7text.Text = vertopen.FileName;
            }
        }

        private void Seg9btn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                seg9text.Text = vertopen.FileName;
            }
        }

        private void CourseImporter_Load(object sender, EventArgs e)
        {

        }
    }
}