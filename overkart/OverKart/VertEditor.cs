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
using PeepsCompress;

namespace OverKart64
{
    public partial class VertEditor : Form
    {
        public VertEditor()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        int cID = new int();
        bool loaded = false;
        int vertcount = new int();

        // all need to be read and written as 16bit unsigned integers
        int[] xcor = new int[0];
        int[] ycor = new int[0];
        int[] zcor = new int[0];
        int[] scor = new int[0];
        int[] tcor = new int[0];
        // all need to be read and written as 16bit unsigned integers


        // all need to be read and written as 8bit unsigned bytes
        int[] rcol = new int[0];
        int[] gcol = new int[0];
        int[] bcol = new int[0];
        int[] acol = new int[0];
        // all need to be read and written as 8bit unsigned bytes




        FileStream fs = new FileStream("null", FileMode.OpenOrCreate);
        MemoryStream bs = new MemoryStream();
        BinaryReader br = new BinaryReader(Stream.Null);
        BinaryWriter bw = new BinaryWriter(Stream.Null);
        MemoryStream ds = new MemoryStream();
        BinaryReader dr = new BinaryReader(Stream.Null);
        BinaryWriter dw = new BinaryWriter(Stream.Null);
        MemoryStream vs = new MemoryStream();
        BinaryReader vr = new BinaryReader(Stream.Null);


        OK64 mk = new OK64();

        string filePath = "";
        string savePath = "";

        Int16 valuesign16 = new Int16();

        byte[] flip4 = new byte[4];
        byte[] flip2 = new byte[2];

        int[] img_types = new int[] { 0, 0, 0, 3, 3, 3 };
        int[] img_heights = new int[] { 32, 32, 64, 32, 32, 64 };
        int[] img_widths = new int[] { 32, 64, 32, 32, 64, 32 };
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



        public void DumpVerts()
        {

            cID = coursebox.SelectedIndex;
            
            vertselect.Items.Clear();

            int seg4Size = Convert.ToInt32(seg7_romptr[cID] - seg4_addr[cID]);

            byte[] romBytes = File.ReadAllBytes(filePath);
            byte[] segment4 = new byte[seg4Size];
                
            Array.Copy(romBytes, seg4_addr[cID], segment4, 0, seg4Size);

            byte[] vertBytes = mk.decompressMIO0(segment4);
            
            bool VertEnd = true;

            // all need to be read and written as 16bit unsigned integers

            vertcount = vertBytes.Length / 14;


            Array.Resize(ref xcor, vertcount);
            Array.Resize(ref ycor, vertcount);
            Array.Resize(ref zcor, vertcount);
            Array.Resize(ref scor, vertcount);
            Array.Resize(ref tcor, vertcount);

            Array.Resize(ref rcol, vertcount);
            Array.Resize(ref gcol, vertcount);
            Array.Resize(ref bcol, vertcount);
            Array.Resize(ref acol, vertcount);



            ds = new MemoryStream(vertBytes);
            dr = new BinaryReader(ds);
            {
                dr.BaseStream.Position = 0;
                for (int i = 0; VertEnd; i++)
                {
                    flip2 = dr.ReadBytes(2);
                    Array.Reverse(flip2);
                    xcor[i] = BitConverter.ToInt16(flip2, 0); //x   <-- this really is the X axis. No tricks.
                        
                    flip2 = dr.ReadBytes(2);
                    Array.Reverse(flip2);
                    zcor[i] = BitConverter.ToInt16(flip2, 0); //z    <-- this is actually the Y axis, but the game (like early 3D) treats the Y axis as height
                        
                    flip2 = dr.ReadBytes(2);
                    Array.Reverse(flip2);
                    ycor[i] = BitConverter.ToInt16(flip2, 0); //y    <-- this is actually the Z axis, but the game (like early 3D) treats the Z axis as depth

                    flip2 = dr.ReadBytes(2);
                    Array.Reverse(flip2);
                    scor[i] = BitConverter.ToInt16(flip2, 0); //S

                    flip2 = dr.ReadBytes(2);
                    Array.Reverse(flip2);
                    tcor[i] = BitConverter.ToInt16(flip2, 0); //T

                    rcol[i] = dr.ReadByte();
                    gcol[i] = dr.ReadByte();
                    bcol[i] = dr.ReadByte();
                    acol[i] = dr.ReadByte();

                    vertselect.Items.Add("Vertex-" + i.ToString());

                    if(dr.BaseStream.Position >= dr.BaseStream.Length)
                    {
                        VertEnd = false;
                    }
                }
                    
                CloseStreams();
            }
            
        }






        private void Button2_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
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
                            Array.Reverse(flip);
                            flag[i] = BitConverter.ToUInt16(flip, 0);

                            flop = BitConverter.GetBytes(unused[i]);
                            Array.Reverse(flip);
                            unused[i] = BitConverter.ToUInt16(flip, 0);



                            seg7_romptr[i] = seg7_ptr[i] - seg47_buf[i] + seg4_addr[i];

                        }



                        CloseStreams();
                        string[] items = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut" };

                        foreach (string item in items)
                        {
                            coursebox.Items.Add(item);

                        }

                        impbtn.Enabled = true;
                        expbtn.Enabled = true;
                        coursebox.SelectedIndex = 0;
                        coursebox.Enabled = true;
                        loaded = true;
                        
                        MessageBox.Show("ROM Loaded");
                       
                            
                    }
                }
                CloseStreams();
            }
            
        }


        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            xbox.Text = xcor[vertselect.SelectedIndex].ToString();
            ybox.Text = ycor[vertselect.SelectedIndex].ToString();
            zbox.Text = zcor[vertselect.SelectedIndex].ToString();
            sbox.Text = scor[vertselect.SelectedIndex].ToString();
            tbox.Text = tcor[vertselect.SelectedIndex].ToString();

            rbox.Text = rcol[vertselect.SelectedIndex].ToString();
            gbox.Text = gcol[vertselect.SelectedIndex].ToString();
            bbox.Text = bcol[vertselect.SelectedIndex].ToString();
            abox.Text = acol[vertselect.SelectedIndex].ToString();
        }

        private void Coursebox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DumpVerts();
        }

        private void Xbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (loaded)
            {
                int.TryParse(xbox.Text, out xcor[vertselect.SelectedIndex]);
            }
        }

        private void Ybox_KeyUp(object sender, KeyEventArgs e)
        {
            if (loaded)
            {
                int.TryParse(ybox.Text, out ycor[vertselect.SelectedIndex]);
            }
        }

        private void Zbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (loaded)
            {
                int.TryParse(zbox.Text, out zcor[vertselect.SelectedIndex]);
            }
            
        }

        private void Sbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (loaded)
            {
                int.TryParse(sbox.Text, out scor[vertselect.SelectedIndex]);
            }
        }

        private void Tbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (loaded)
            {
                int.TryParse(tbox.Text, out tcor[vertselect.SelectedIndex]);
            }
        }

        private void Rbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (loaded)
            {
                int.TryParse(rbox.Text, out rcol[vertselect.SelectedIndex]);
            }
        }
        private void Gbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (loaded)
            {
                int.TryParse(gbox.Text, out gcol[vertselect.SelectedIndex]);
            }
        }
        private void Bbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (loaded)
            {
                int.TryParse(bbox.Text, out bcol[vertselect.SelectedIndex]);
            }
        }

        private void Abox_KeyUp(object sender, KeyEventArgs e)
        {
            if (loaded)
            {
                int.TryParse(abox.Text, out acol[vertselect.SelectedIndex]);
            }
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            if (loaded)
            {
                if (vertsave.ShowDialog() == DialogResult.OK)
                {

                    savePath = vertsave.FileName;

                    ds = new MemoryStream();
                    dw = new BinaryWriter(ds);
                    for (int i = 0; i < vertcount; i++)
                    {
                        flip2 = BitConverter.GetBytes(Convert.ToInt16(xcor[i]));
                        Array.Reverse(flip2);
                        dw.Write(BitConverter.ToInt16(flip2, 0));

                        flip2 = BitConverter.GetBytes(Convert.ToInt16(zcor[i]));
                        Array.Reverse(flip2);
                        dw.Write(BitConverter.ToInt16(flip2, 0));

                        flip2 = BitConverter.GetBytes(Convert.ToInt16(ycor[i]));
                        Array.Reverse(flip2);
                        dw.Write(BitConverter.ToInt16(flip2, 0));

                        flip2 = BitConverter.GetBytes(Convert.ToInt16(scor[i]));
                        Array.Reverse(flip2);
                        dw.Write(BitConverter.ToInt16(flip2, 0));

                        flip2 = BitConverter.GetBytes(Convert.ToInt16(tcor[i]));
                        Array.Reverse(flip2);
                        dw.Write(BitConverter.ToInt16(flip2, 0));


                        dw.Write(Convert.ToByte(rcol[i]));
                        dw.Write(Convert.ToByte(gcol[i]));
                        dw.Write(Convert.ToByte(bcol[i]));
                        dw.Write(Convert.ToByte(acol[i]));                      
                        
                    }
                    byte[] seg4 = ds.ToArray();
                    seg4 = mk.compressMIO0(seg4);
                    File.WriteAllBytes(savePath, seg4);

                    MessageBox.Show("Finished");

                }
            }
        }

        private void Impbtn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {

                string importpath = vertopen.FileName;

                string[] reader = File.ReadAllLines(importpath);
                string[] positions = new string[3];

                for (int i = 0; i < reader.Length; i++)
                {

                    positions = reader[i].Split(',').ToArray();
                    xcor[i] = Convert.ToInt32(Convert.ToDouble(positions[0]));
                    ycor[i] = Convert.ToInt32(Convert.ToDouble(positions[1]));
                    zcor[i] = Convert.ToInt32(Convert.ToDouble(positions[2]));



                }




            }
        }

        private void expbtn_Click(object sender, EventArgs e)
        {
            cID = coursebox.SelectedIndex;
            if (vertsave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertsave.FileName;
                for (int i = 0; i < vertcount; i++)
                {
                    
                    System.IO.File.AppendAllText(savePath, xcor[i].ToString() + Environment.NewLine);                   
                    System.IO.File.AppendAllText(savePath, zcor[i].ToString() + Environment.NewLine);                    
                    System.IO.File.AppendAllText(savePath, ycor[i].ToString() + Environment.NewLine);
                    
                    
                }
                MessageBox.Show("Finished");
                CloseStreams();
                
            }
        }
    }
}
