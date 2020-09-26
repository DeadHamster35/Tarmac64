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
using Tarmac64_Geometry;



//  1188752   TRACK HEADER OFFSET US ROM



namespace Tarmac64
{
    public partial class SegmentExporter : Form
    {
        public SegmentExporter()
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

        TM64_Geometry mk = new TM64_Geometry();

        TM64_Geometry.Vertex[] vertCache = new TM64_Geometry.Vertex[32];

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

        int[] headerOffset = new int[] { 0x122390, 0xBEA700, 0xBEAAC0, 0xBEAE80, 0xBEB240 };

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

        OpenFileDialog vertOpen = new OpenFileDialog();
        SaveFileDialog vertSave = new SaveFileDialog();
        FolderBrowserDialog textSave = new FolderBrowserDialog();



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



        


        private void DumpFaces(object sender, EventArgs e)
        {


            if (vertSave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertSave.FileName;









                cID = coursebox.SelectedIndex;

                
                int targetOffset = Convert.ToInt32(seg4_addr[cID]);
                byte[] romBytes = File.ReadAllBytes(filePath);

                byte[] compressedFile = new byte[romBytes.Length - targetOffset];
                Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);


                byte[] vertByte = mk.decompressMIO0(compressedFile);
                
                // we do a sloppy copy and don't convert the verts to their 16byte "proper" form.

                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];


                MessageBox.Show(seg7_addr.ToString());

                fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                ds = new MemoryStream(vertByte);
                br = new BinaryReader(fs);
                dr = new BinaryReader(ds);





                byte commandbyte = new byte();
                byte[] byte29 = new byte[2];



                br.BaseStream.Seek(seg7_addr, SeekOrigin.Begin);

                int vertoffset = 0;
                byte[] voffset = new byte[2];

                bool DispEnd = true;

                for (; DispEnd;)
                {


                    commandbyte = br.ReadByte();

                    if (commandbyte == 0xFF)
                    {

                        DispEnd = false;
                    }
                    if ((commandbyte >= 0x1A && commandbyte <= 0x1F))
                    {

                        value16 = br.ReadUInt16();

                        x_mask = value16 >> 4;
                        x_flags = value16 & 0x03;

                        y_mask = value16 >> 4;
                        y_flags = value16 & 0x03;

                        texwidth = img_widths[commandbyte - 0x1A];

                        texheight = img_heights[commandbyte - 0x1A];
                        textype = img_types[commandbyte - 0x1A];
                        //MessageBox.Show(texwidth.ToString() + "-"+texheight.ToString());



                        br.BaseStream.Seek(2, SeekOrigin.Current);
                    }

                    if (commandbyte == 0x2C || commandbyte == 0x2B)
                    {
                        br.BaseStream.Seek(2, SeekOrigin.Current);
                    }
                    if (commandbyte >= 0x20 && commandbyte <= 0x25)
                    {
                        br.BaseStream.Seek(3, SeekOrigin.Current);
                    }
                    if (commandbyte == 0x2A)
                    {
                        System.IO.File.AppendAllText(savePath, "ENDSECTION"+ Environment.NewLine);
                        System.IO.File.AppendAllText(savePath, "Object " + (br.BaseStream.Position - seg7_addr).ToString() + Environment.NewLine);
                    }
                    if (commandbyte == 0x28)
                    {
                        br.BaseStream.Seek(1, SeekOrigin.Current);
                    }
                    if (commandbyte == 0x29)
                    {

                        value16 = br.ReadUInt16();
                        // MessageBox.Show(value16.ToString() + " . " + i.ToString() + " .test1134. "+br.BaseStream.Position.ToString());
                        v0 = (value16 >> 10) & 0x1F;
                        v1 = (value16 >> 5) & 0x1F;
                        v2 = value16 & 0x1F;


                        if (v0 > 32 || v1 > 32 || v2 > 32)
                        {
                            MessageBox.Show("Crap Vert Index");

                        }
                        short[] xval = new short[3];
                        short[] yval = new short[3];
                        short[] zval = new short[3];
                        short[] uval = new short[3];
                        short[] vval = new short[3];
                        string printstring = "";
                           


                        dr.BaseStream.Seek(vertoffset + (v0 * 14), SeekOrigin.Begin);

                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        xval[0] = BitConverter.ToInt16(flip2, 0); //x
                        
                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        zval[0] = BitConverter.ToInt16(flip2, 0); //z
                       
                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        yval[0] = BitConverter.ToInt16(flip2, 0); //y
                        


                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        uval[0] = BitConverter.ToInt16(flip2, 0); //s
                        uval[0] = Convert.ToInt16((uval[0] / texwidth) / 32);
                        
                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        vval[0] = BitConverter.ToInt16(flip2, 0); //s
                        vval[0] = Convert.ToInt16((vval[0] * -1 / texwidth) / 32);

                        //
                        //

                        dr.BaseStream.Seek(vertoffset + (v1 * 14), SeekOrigin.Begin);

                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        xval[1] = BitConverter.ToInt16(flip2, 0); //x

                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        zval[1] = BitConverter.ToInt16(flip2, 0); //z

                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        yval[1] = BitConverter.ToInt16(flip2, 0); //y



                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        uval[1] = BitConverter.ToInt16(flip2, 0); //s
                        uval[1] = Convert.ToInt16((uval[0] / texwidth) / 32);

                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        vval[1] = BitConverter.ToInt16(flip2, 0); //s
                        vval[1] = Convert.ToInt16((vval[0] * -1 / texwidth) / 32);

                        //
                        //



                        dr.BaseStream.Seek(vertoffset + (v2 * 14), SeekOrigin.Begin);

                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        xval[2] = BitConverter.ToInt16(flip2, 0); //x

                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        zval[2] = BitConverter.ToInt16(flip2, 0); //z

                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        yval[2] = BitConverter.ToInt16(flip2, 0); //y



                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        uval[2] = BitConverter.ToInt16(flip2, 0); //s
                        uval[2] = Convert.ToInt16((uval[0] / texwidth) / 32);

                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        vval[2] = BitConverter.ToInt16(flip2, 0); //s
                        vval[2] = Convert.ToInt16((vval[0] * -1 / texwidth) / 32);







                        printstring = xval[0].ToString() + "," + yval[0].ToString() + "," + zval[0].ToString() + ";";
                        printstring = printstring + xval[1].ToString() + "," + yval[1].ToString() + "," + zval[1].ToString() + ";";
                        printstring = printstring + xval[2].ToString() + "," + yval[2].ToString() + "," + zval[2].ToString() + ";";
                        System.IO.File.AppendAllText(savePath, printstring + Environment.NewLine);
                    }

                    if (commandbyte == 0x58)
                    {

                        for (int i = 0; i < 2; i++)
                        {
                            value16 = br.ReadUInt16();
                            // MessageBox.Show(value16.ToString() + " . " + i.ToString() + " .test1134. "+br.BaseStream.Position.ToString());
                            v0 = (value16 >> 10) & 0x1F;
                            v1 = (value16 >> 5) & 0x1F;
                            v2 = value16 & 0x1F;


                            if (v0 > 32 || v1 > 32 || v2 > 32)
                            {
                                MessageBox.Show("Crap Vert Index");

                            }
                            short[] xval = new short[3];
                            short[] yval = new short[3];
                            short[] zval = new short[3];
                            short[] uval = new short[3];
                            short[] vval = new short[3];
                            string printstring = "";



                            dr.BaseStream.Seek(vertoffset + (v0 * 14), SeekOrigin.Begin);

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            xval[0] = BitConverter.ToInt16(flip2, 0); //x

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            zval[0] = BitConverter.ToInt16(flip2, 0); //z

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            yval[0] = BitConverter.ToInt16(flip2, 0); //y



                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            uval[0] = BitConverter.ToInt16(flip2, 0); //s
                            uval[0] = Convert.ToInt16((uval[0] / texwidth) / 32);

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            vval[0] = BitConverter.ToInt16(flip2, 0); //s
                            vval[0] = Convert.ToInt16((vval[0] * -1 / texwidth) / 32);

                            //
                            //

                            dr.BaseStream.Seek(vertoffset + (v1 * 14), SeekOrigin.Begin);

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            xval[1] = BitConverter.ToInt16(flip2, 0); //x

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            zval[1] = BitConverter.ToInt16(flip2, 0); //z

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            yval[1] = BitConverter.ToInt16(flip2, 0); //y



                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            uval[1] = BitConverter.ToInt16(flip2, 0); //s
                            uval[1] = Convert.ToInt16((uval[0] / texwidth) / 32);

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            vval[1] = BitConverter.ToInt16(flip2, 0); //s
                            vval[1] = Convert.ToInt16((vval[0] * -1 / texwidth) / 32);

                            //
                            //



                            dr.BaseStream.Seek(vertoffset + (v2 * 14), SeekOrigin.Begin);

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            xval[2] = BitConverter.ToInt16(flip2, 0); //x

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            zval[2] = BitConverter.ToInt16(flip2, 0); //z

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            yval[2] = BitConverter.ToInt16(flip2, 0); //y



                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            uval[2] = BitConverter.ToInt16(flip2, 0); //s
                            uval[2] = Convert.ToInt16((uval[0] / texwidth) / 32);

                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            vval[2] = BitConverter.ToInt16(flip2, 0); //s
                            vval[2] = Convert.ToInt16((vval[0] * -1 / texwidth) / 32);







                            printstring = xval[0].ToString() + "," + yval[0].ToString() + "," + zval[0].ToString() + ";";
                            printstring = printstring + xval[1].ToString() + "," + yval[1].ToString() + "," + zval[1].ToString() + ";";
                            printstring = printstring + xval[2].ToString() + "," + yval[2].ToString() + "," + zval[2].ToString() + ";";
                            System.IO.File.AppendAllText(savePath, printstring + Environment.NewLine);
                        }


                    }


                    if (commandbyte >= 0x33 && commandbyte <= 0x52)
                    {


                        vertoffset = br.ReadUInt16() * 14;
                        // MessageBox.Show(vertoffset.ToString()+"-voffset test3535");



                    }




                }

                CloseStreams();
                MessageBox.Show("Finished");
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            if (vertOpen.ShowDialog() == DialogResult.OK)
            {


                //Get the path of specified file
                filePath = vertOpen.FileName;

                //Read the contents of the file into a stream

                string filetype = filePath.Substring(filePath.Length - 3);

                if (filetype == "n64" || filetype == "v64")
                {
                    MessageBox.Show("Only Supports .Z64 format");
                }
                else
                {
                    loadCourse(filePath);
                    coursebox.SelectedIndex = 0;
                }
                CloseStreams();
            }
        }



        private void loadCourse(string filePath)
        {
            fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            bw = new BinaryWriter(fs);
            br = new BinaryReader(fs);
            {
                br.BaseStream.Seek(headerOffset[headerBox.SelectedIndex], SeekOrigin.Begin);

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



                if (headerBox.SelectedIndex > 0)
                {
                    for (int courseIndex = 0; courseIndex < 20; courseIndex++)
                    {
                        coursebox.Items.Add("Custom Course " + headerBox.SelectedIndex.ToString() + "-" + courseIndex.ToString() + " ");

                    }

                }
                else
                {
                    string[] items = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut" };
                    coursebox.Items.Clear();
                    foreach (string item in items)
                    {
                        coursebox.Items.Add(item);

                    }
                }


                MessageBox.Show("ROM Loaded");

                proc4btn.Enabled = true;
                proc6btn.Enabled = true;
                proc7btn.Enabled = true;
                proc9btn.Enabled = true;
                surfacemapbtn.Enabled = true;

                seg4btn.Enabled = true;
                seg5btn.Enabled = true;
                seg6btn.Enabled = true;
                seg7btn.Enabled = true;
                asmbtn.Enabled = true;

                dump4btn.Enabled = true;
                dump6btn.Enabled = true;
                dump7btn.Enabled = true;
                dump9btn.Enabled = true;
                dumpasmbtn.Enabled = true;

                cseg4btn.Enabled = true;
                cseg6btn.Enabled = true;
                cseg7btn.Enabled = true;
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




        private void DumpVerts(object sender, EventArgs e)
        {

            cID = coursebox.SelectedIndex;
            if (vertSave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertSave.FileName;




                int targetOffset = Convert.ToInt32(seg4_addr[cID]);
                byte[] romBytes = File.ReadAllBytes(filePath);

                byte[] compressedFile = new byte[0];
                Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);


                byte[] Verts = mk.decompressMIO0(compressedFile);
                

                // we do a sloppy copy and don't convert the verts to their 16byte "proper" form.

                bool VertEnd = true;

                ds = new MemoryStream(Verts);
                dr = new BinaryReader(ds);
                {
                    dr.BaseStream.Position = 0;
                    for (int i = 0; VertEnd; i++)
                    {
                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0); //x
                        System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);
                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0); //z
                        System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);
                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0); //y
                        System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);
                        if (dr.BaseStream.Position + 8 < dr.BaseStream.Length)
                        {
                            dr.BaseStream.Position = dr.BaseStream.Position + 8;
                        }
                        else
                        {
                            VertEnd = false;
                        }
                    }
                    MessageBox.Show("Finished");
                    CloseStreams();
                }
            }
        }

        private void Segment6_decompress(object sender, EventArgs e)
        {
            cID = coursebox.SelectedIndex;
            if (vertSave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertSave.FileName;





                int targetOffset = Convert.ToInt32(seg6_addr[cID]);
                byte[] romBytes = File.ReadAllBytes(filePath);

                byte[] compressedFile = new byte[romBytes.Length - targetOffset];
                Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);

                byte[] seg6 = mk.decompressMIO0(compressedFile);
                


                fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                ds = new MemoryStream(seg6);


                ds.CopyTo(fs);
                MessageBox.Show("Finished");
                CloseStreams();

            }
        }

        private void segment7_decomp(object sender, EventArgs e)
        {

            if (vertSave.ShowDialog() == DialogResult.OK)
            {
                cID = coursebox.SelectedIndex;
                savePath = vertSave.FileName;
                byte[] ROM = File.ReadAllBytes(filePath);
                byte[] useg7 = mk.dumpseg7(cID, ROM);
                byte[] seg7 = mk.decompress_seg7(useg7);

                File.WriteAllBytes(savePath, seg7);
                MessageBox.Show("Finished");
                CloseStreams();
            }
        }





        private void segment4_decompress(object sender, EventArgs e)
        {
            cID = coursebox.SelectedIndex;
            if (vertSave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertSave.FileName;

                int targetOffset = Convert.ToInt32(seg4_addr[cID]);
                byte[] romBytes = File.ReadAllBytes(filePath);

                byte[] compressedFile = new byte[romBytes.Length - targetOffset];
                Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);

                byte[] seg4 = mk.decompressMIO0(compressedFile);
                

                fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                ds = new MemoryStream(seg4);

                {
                    ds.CopyTo(fs);
                    MessageBox.Show("Finished");
                    CloseStreams();
                }
            }
        }

        private void dumpdlists(object sender, EventArgs e)
        {
            cID = coursebox.SelectedIndex;
            if (vertSave.ShowDialog() == DialogResult.OK)
            {
                
                savePath = vertSave.FileName;


                int targetOffset = Convert.ToInt32(seg6_addr[cID]);
                byte[] romBytes = File.ReadAllBytes(filePath);

                byte[] compressedFile = new byte[romBytes.Length - targetOffset];
                Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);

                byte[] seg6 = mk.decompressMIO0(compressedFile);
                

                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];


                targetOffset = Convert.ToInt32(seg4_addr[cID]);    
                compressedFile = new byte[romBytes.Length - targetOffset];
                Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);


                byte[] decompressedVerts = mk.decompressMIO0(compressedFile);
                List<byte> vertList = decompressedVerts.ToList();
                List<byte> insert = new List<byte> { 0x00, 0x00 };
                int vertcount = (vertList.Count / 14);

                for (int i = 0; i < vertcount; i++)
                {
                    vertList.InsertRange((i + 1) * 10 + (i * 2) + (i * 4), insert);
                }
                byte[] seg4 = vertList.ToArray();

                byte[] ROM = File.ReadAllBytes(filePath);
                byte[] seg7 = mk.dumpseg7(cID, ROM);
                byte[] useg7 = mk.decompress_seg7(seg7);




                MemoryStream seg7m = new MemoryStream(useg7);
                MemoryStream seg6m = new MemoryStream(seg6);
                MemoryStream seg4m = new MemoryStream(seg4);
                BinaryReader seg7r = new BinaryReader(seg7m);
                BinaryReader seg6r = new BinaryReader(seg6m);
                BinaryReader seg4r = new BinaryReader(seg4m);

                int vaddress = new int();


                byte commandbyte = new byte();
                byte[] byte29 = new byte[2];

                string output = "";
                byte[] voffset = new byte[2];

                //DEBUG
                int displayOffset = 0;
                //DEBUG


                for (bool breakBool = false; breakBool == false; )
                {
                    seg6r.BaseStream.Seek(displayOffset, SeekOrigin.Begin);
                    byte[] rsp_add = seg6r.ReadBytes(4);
                    Array.Reverse(rsp_add);
                    int Value = BitConverter.ToInt32(rsp_add, 0);
                    if (Value != 0)
                    {
                        String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');
                        int segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                        MessageBox.Show(segid.ToString() + " " + seg6r.BaseStream.Position.ToString());
                        current_offset = Convert.ToInt32(Binary.Substring(8, 24), 2);

                        for (bool subBreak = false; subBreak == false & current_offset < seg6r.BaseStream.Length;)
                        {

                            seg6r.BaseStream.Seek(current_offset, SeekOrigin.Begin);
                            commandbyte = seg6r.ReadByte();

                            output = "";

                            //MessageBox.Show("-Execute Order 0x" + commandbyte.ToString("X")+"--"+current_offset.ToString());

                            if (commandbyte == 0x00)
                            {
                                //End F3DEX Display Lists....probably? lol
                                subBreak = true;
                            }
                            if (commandbyte == 0xB8)
                            {
                                output = "ENDSECTION" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                                subBreak = true;
                            }


                            //Special Conditional
                            if (commandbyte == 0xE4)
                            {
                                current_offset += 4;
                            }
                            //Special Conditional

                            if (commandbyte == 0x04)
                            {
                                output = mk.F3DEX_Model(out vertCache,commandbyte, seg6, seg4, vaddress, current_offset + 1, vertCache);
                                vaddress = Convert.ToInt32(output);
                                output = "";
                            }
                            if (commandbyte == 0xB1)
                            {
                                //output = mk.translate_F3D(commandbyte, seg6r, vr, vaddress);
                            }
                            if (commandbyte == 0xBF)
                            {
                                //output = mk.translate_F3D(commandbyte, seg6r, vr, vaddress);
                            }
                            if (commandbyte == 0x06)
                            {
                                output = mk.F3DEX_Model(out vertCache,commandbyte, seg6, seg4, vaddress, current_offset + 1, vertCache);

                                StringReader sread = new StringReader(output);




                                segid = Convert.ToInt32(sread.ReadLine());
                                int caddress = Convert.ToInt32(sread.ReadLine());
                                System.IO.File.AppendAllText(savePath, "NEWOBJECT" + Environment.NewLine); ;
                                System.IO.File.AppendAllText(savePath, "Command Address 0x" + current_offset.ToString("X") + Environment.NewLine); ;
                                System.IO.File.AppendAllText(savePath, "Segment 0" + segid.ToString() + "00" + caddress.ToString("X") + Environment.NewLine);


                                //MessageBox.Show(segid.ToString() + "-" + caddress.ToString());

                                output = "";
                                byte recursivecommand = new byte();



                                for (int n = 0; n < 1;)
                                {
                                    if (segid == 6)
                                    {

                                        seg6r.BaseStream.Seek(caddress, SeekOrigin.Begin);
                                        recursivecommand = seg6r.ReadByte();

                                        if (recursivecommand == 0x04)
                                        {
                                            output = mk.F3DEX_Model(out vertCache, recursivecommand, seg6, seg4, vaddress, caddress + 1, vertCache);
                                            vaddress = Convert.ToInt32(output);
                                            output = "";
                                        }
                                        if (recursivecommand == 0xE4)
                                        {
                                            caddress += 4;
                                        }
                                        if (recursivecommand == 0xB1)
                                        {
                                            output = mk.F3DEX_Model(out vertCache, recursivecommand, seg6, seg4, vaddress, caddress + 1, vertCache);
                                        }
                                        if (recursivecommand == 0xBF)
                                        {
                                            output = mk.F3DEX_Model(out vertCache, recursivecommand, seg6, seg4, vaddress, caddress + 1, vertCache);
                                        }
                                        if (recursivecommand == 0xB8)
                                        {
                                            output = "";

                                            System.IO.File.AppendAllText(savePath, "ENDOBJECT" + Environment.NewLine);
                                            System.IO.File.AppendAllText(savePath, "Segment 0" + segid.ToString() + "00" + caddress.ToString("X") + Environment.NewLine);
                                            System.IO.File.AppendAllText(savePath, Environment.NewLine);

                                            n = 2;
                                        }
                                        if (output != "")
                                        {
                                            System.IO.File.AppendAllText(savePath, output);
                                        }
                                    }

                                    if (segid == 7)
                                    {
                                        seg7r.BaseStream.Seek(caddress, SeekOrigin.Begin);
                                        recursivecommand = seg7r.ReadByte();



                                        if (recursivecommand == 0x04)
                                        {
                                            output = mk.F3DEX_Model(out vertCache, recursivecommand, useg7, seg4, vaddress, caddress + 1, vertCache);
                                            vaddress = Convert.ToInt32(output);
                                            // MessageBox.Show("Vert Update-" + vaddress.ToString());
                                            output = "";
                                        }
                                        if (recursivecommand == 0xE4)
                                        {
                                            caddress += 4;
                                        }
                                        if (recursivecommand == 0xB1)
                                        {
                                            output = mk.F3DEX_Model(out vertCache, recursivecommand, useg7, seg4, vaddress, caddress + 1, vertCache);
                                        }
                                        if (recursivecommand == 0xBF)
                                        {
                                            output = mk.F3DEX_Model(out vertCache, recursivecommand, useg7, seg4, vaddress, caddress + 1, vertCache);
                                        }
                                        if (recursivecommand == 0xB8)
                                        {
                                            output = "";

                                            System.IO.File.AppendAllText(savePath, "ENDOBJECT" + Environment.NewLine);
                                            System.IO.File.AppendAllText(savePath, "Segment 0" + segid.ToString() + "00" + caddress.ToString("X") + Environment.NewLine);
                                            System.IO.File.AppendAllText(savePath, Environment.NewLine);

                                            n = 2;
                                        }
                                        if (output != "")
                                        {
                                            System.IO.File.AppendAllText(savePath, output);
                                        }
                                    }
                                    caddress += 8;
                                }

                                output = "";

                            }




                            if (output != "")
                            {
                                System.IO.File.AppendAllText(savePath, output);
                            }

                            //MessageBox.Show("Command-"+commandbyte.ToString("X"));

                            current_offset += 8;
                        }


                    }
                    else
                        breakBool = true;
                    displayOffset = displayOffset + 4;
                }

                MessageBox.Show("Finished");
                CloseStreams();

            }

        }





        private void dumpface2(object sender, EventArgs e)
        {
            cID = coursebox.SelectedIndex;
            if (vertSave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertSave.FileName;


                for (int currentVert = 0; currentVert < 32; currentVert++)
                {
                    vertCache[currentVert] = new TM64_Geometry.Vertex();
                    vertCache[currentVert].position = new TM64_Geometry.Position();
                    vertCache[currentVert].color = new TM64_Geometry.OK64Color();
                }

                int targetOffset = Convert.ToInt32(seg6_addr[cID]);
                byte[] romBytes = File.ReadAllBytes(filePath);

                byte[] compressedFile = new byte[romBytes.Length - targetOffset];
                Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);

                byte[] seg6 = mk.decompressMIO0(compressedFile);


                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];


                targetOffset = Convert.ToInt32(seg4_addr[cID]);
                compressedFile = new byte[romBytes.Length - targetOffset];
                Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);


                byte[] decompressedVerts = mk.decompressMIO0(compressedFile);
                List<byte> vertList = decompressedVerts.ToList();
                List<byte> insert = new List<byte> { 0x00, 0x00 };
                int vertcount = (vertList.Count / 14);

                for (int i = 0; i < vertcount; i++)
                {
                    vertList.InsertRange((i + 1) * 10 + (i * 2) + (i * 4), insert);
                }
                byte[] seg4 = vertList.ToArray();

                byte[] ROM = File.ReadAllBytes(filePath);
                byte[] useg7 = mk.dumpseg7(cID, ROM);
                byte[] seg7 = mk.decompress_seg7(useg7);




                MemoryStream seg7m = new MemoryStream(seg7);
                MemoryStream seg4m = new MemoryStream(seg4);
                BinaryReader seg7r = new BinaryReader(seg7m);
                BinaryReader seg4r = new BinaryReader(seg4m);

                int vaddress = new int();


                byte commandbyte = new byte();
                byte[] byte29 = new byte[2];

                string output = "";
                byte[] voffset = new byte[2];

                //DEBUG
                int displayOffset = 0;
                //DEBUG


                while(displayOffset < seg7r.BaseStream.Length)
                {
                    
                    seg7r.BaseStream.Seek(displayOffset, SeekOrigin.Begin);
                    commandbyte = seg7r.ReadByte();

                    output = "";

                    if (commandbyte == 0xB8)
                    {
                        output = "ENDSECTION" + Environment.NewLine + "Object " + seg7r.BaseStream.Position.ToString("X") + Environment.NewLine;                        
                    }


                    //Special Conditional
                    if (commandbyte == 0xE4)
                    {
                        displayOffset += 4;
                    }
                    //Special Conditional

                    if (commandbyte == 0x04)
                    {
                        output = mk.F3DEX_Model(out vertCache, commandbyte, seg7, seg4, vaddress, displayOffset + 1, vertCache);
                        vaddress = Convert.ToInt32(output);
                        output = "";
                    }
                    if (commandbyte == 0xB1)
                    {
                        output = mk.F3DEX_Model(out vertCache, commandbyte, seg7, seg4, vaddress, displayOffset + 1, vertCache);
                    }
                    if (commandbyte == 0xBF)
                    {
                        output = mk.F3DEX_Model(out vertCache, commandbyte, seg7, seg4, vaddress, displayOffset + 1, vertCache);
                    }
                            



                    if (output != "")
                    {
                        System.IO.File.AppendAllText(savePath, output);
                    }

                    //MessageBox.Show("Command-"+commandbyte.ToString("X"));

                    displayOffset += 8;
                }

                MessageBox.Show("Finished");
                CloseStreams();

            }

        }

        //Old Code Above












        private void dump9_btn(object sender, EventArgs e)
        {


            cID = coursebox.SelectedIndex;
            if (vertSave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertSave.FileName;
                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];
                MessageBox.Show(seg7_addr.ToString());

                byte[] rombytes = File.ReadAllBytes(filePath);
                byte[] seg9 = new byte[(seg9_end[cID] - seg9_addr[cID])];

                Buffer.BlockCopy(rombytes, Convert.ToInt32(seg9_addr[cID]), seg9, 0, (Convert.ToInt32(seg9_end[cID]) - Convert.ToInt32(seg9_addr[cID])));

                File.WriteAllBytes(savePath, seg9);

                MessageBox.Show("Finished");
                CloseStreams();
            }
        }


        


        private void dump7_btn(object sender, EventArgs e)
        {

            cID = coursebox.SelectedIndex;
            if (vertSave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertSave.FileName;
                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];
                MessageBox.Show(seg7_addr.ToString());

                byte[] rombytes = File.ReadAllBytes(filePath);
                byte[] seg7 = new byte[(seg7_end[cID] - seg7_addr)];

                Buffer.BlockCopy(rombytes, Convert.ToInt32(seg7_addr), seg7, 0, (Convert.ToInt32(seg7_end[cID]) - Convert.ToInt32(seg7_addr)));

                File.WriteAllBytes(savePath, seg7);

                MessageBox.Show("Finished");
                CloseStreams();
            }
        }


        private void dump4_btn(object sender, EventArgs e)
        {


            cID = coursebox.SelectedIndex;
            if (vertSave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertSave.FileName;
                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];
                MessageBox.Show(seg7_addr.ToString());

                byte[] rombytes = File.ReadAllBytes(filePath);
                byte[] seg4 = new byte[(seg7_addr - seg4_addr[cID])];

                Buffer.BlockCopy(rombytes, Convert.ToInt32(seg4_addr[cID]), seg4, 0, (Convert.ToInt32(seg7_addr) - Convert.ToInt32(seg4_addr[cID])));

                File.WriteAllBytes(savePath, seg4);

                MessageBox.Show("Finished");
                CloseStreams();
            }
        }

        private void dump6_btn(object sender, EventArgs e)
        {


            cID = coursebox.SelectedIndex;
            if (vertSave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertSave.FileName;
                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];
                MessageBox.Show(seg7_addr.ToString());

                byte[] rombytes = File.ReadAllBytes(filePath);
                byte[] seg6 = new byte[(seg6_end[cID] - seg6_addr[cID])];

                Buffer.BlockCopy(rombytes, Convert.ToInt32(seg6_addr[cID]), seg6, 0, (Convert.ToInt32(seg6_end[cID]) - Convert.ToInt32(seg6_addr[cID])));

                File.WriteAllBytes(savePath, seg6);

                MessageBox.Show("Finished");
                CloseStreams();
            }
        }


        private void dumpasm_btn(object sender, EventArgs e)
        {
            if (vertSave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertSave.FileName;




                byte[] asm = mk.dump_ASM(filePath);

                File.WriteAllBytes(savePath, asm);
                MessageBox.Show("Finished");
                CloseStreams();
            }
        }

        private void procasmbtn(object sender, EventArgs e)
        {
            if (vertSave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertSave.FileName;


                mk.translate_ASM(savePath, filePath);

                MessageBox.Show("Finished");
                CloseStreams();
            }
            
        }

        private void textbox_change(object sender, EventArgs e)
        {
            updateUI();
        }

        private void sfcbtn_click(object sender, EventArgs e)
        {
            cID = coursebox.SelectedIndex;
            if (vertSave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertSave.FileName;



                int targetOffset = Convert.ToInt32(seg6_addr[cID]);
                byte[] romBytes = File.ReadAllBytes(filePath);

                byte[] compressedFile = new byte[romBytes.Length - targetOffset];
                Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);


                byte[] decompressedverts = mk.decompressMIO0(compressedFile);
                byte[] seg6 = decompressedverts.ToArray();

                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];




                targetOffset = Convert.ToInt32(seg4_addr[cID]);
                romBytes = File.ReadAllBytes(filePath);

                compressedFile = new byte[romBytes.Length - targetOffset];
                Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);

                decompressedverts = mk.decompressMIO0(compressedFile);
                List<byte> vertList = decompressedverts.ToList();
                List<byte> insert = new List<byte> { 0x00, 0x00 };
                int vertcount = (vertList.Count / 14);

                for (int i = 0; i < vertcount; i++)
                {
                    vertList.InsertRange((i + 1) * 10 + (i * 2) + (i * 4), insert);
                }
                byte[] seg4 = vertList.ToArray();

                byte[] ROM = File.ReadAllBytes(filePath);
                byte[] useg7 = mk.dumpseg7(cID, ROM);
                byte[] seg7 = mk.decompress_seg7(useg7);



                MemoryStream seg7m = new MemoryStream(seg7);
                MemoryStream seg6m = new MemoryStream(seg6);
                MemoryStream seg4m = new MemoryStream(seg4);
                BinaryReader seg7r = new BinaryReader(seg7m);
                BinaryReader seg6r = new BinaryReader(seg6m);
                BinaryReader seg4r = new BinaryReader(seg4m);

                int vaddress = new int();


                byte commandbyte = new byte();
                byte[] byte29 = new byte[2];

                string output = "";
                byte[] voffset = new byte[2];
                // offsets to surface maps for each course, in course order. battle maps have no surface map and are listed as 0xFF for simplicicty.

                int[] surfaceoffset = { 0x9650, 0x72d0, 0x93d8, 0xb458, 0x18240, 0x79a0, 0x18fd8, 0xdc28, 0xff28, 0x144b8, 0x23b68, 0x23070, 0x9c20, 0x16440, 0xcc38, 0xff, 0xff, 0xff, 0x14338, 0xff };

                TM64_Geometry.OK64Color[] segmentColors = new TM64_Geometry.OK64Color[64];
                Random rngValue = new Random();
                for (int i=0; i < 64; i++)
                {
                    segmentColors[i] = new TM64_Geometry.OK64Color();
                    byte r, g, b = new int();
                    r = Convert.ToByte(rngValue.Next(0, 255));
                    g = Convert.ToByte(rngValue.Next(0, 255));
                    b = Convert.ToByte(rngValue.Next(0, 255));
                    segmentColors[i].R = r;
                    segmentColors[i].G = g;
                    segmentColors[i].B = b;
                }

                current_offset = surfaceoffset[cID];
                for (int i = 0; i < 1;)
                {

                    seg6r.BaseStream.Seek(current_offset, SeekOrigin.Begin);
                    byte[] rsp_add = seg6r.ReadBytes(4);
                    int surfaceID = Convert.ToInt32(seg6r.ReadByte());
                    int sectionID = Convert.ToInt32(seg6r.ReadByte());


                    if (rsp_add[0] == 0x00)
                    {
                        i = 2;
                    }
                    else
                    {

                        int newR, newG, newB = new int();

                        if (sectionID != 255)
                        {

                            int colorOffset = rngValue.Next(0, 20);
                            newR = segmentColors[sectionID].R - colorOffset;
                            if (newR < 0)
                            {
                                newR = 0;
                            }

                            newG = segmentColors[sectionID].G - colorOffset;
                            if (newG < 0)
                            {
                                newR = 0;
                            }

                            newB = segmentColors[sectionID].B - colorOffset;
                            if (newB < 0)
                            {
                                newB = 0;
                            }
                        }
                        else
                        {
                            newR = 255;
                            newG = 255;
                            newB = 255;
                        }
                        Array.Reverse(rsp_add);

                        int Value = BitConverter.ToInt32(rsp_add, 0);
                        String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                        int segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                        int caddress = Convert.ToInt32(Binary.Substring(8, 24), 2);

                        System.IO.File.AppendAllText(savePath, "NEWOBJECT" + Environment.NewLine);
                        System.IO.File.AppendAllText(savePath, newR.ToString() + ", " + newG.ToString() + ", " + newB.ToString() + ", " + Environment.NewLine);
                        System.IO.File.AppendAllText(savePath, "Section "+sectionID.ToString()+"- Surface "+surfaceID.ToString("X"
                            ) + "- Segment 0" + segid.ToString() + "00" + caddress.ToString("X") + Environment.NewLine);


                        //MessageBox.Show(segid.ToString() + "-" + caddress.ToString());

                        output = "";
                        byte recursivecommand = new byte();

                        if (segid == 6)
                        {
                            seg6r.BaseStream.Seek(caddress, SeekOrigin.Begin);
                            recursivecommand = seg6r.ReadByte();
                            //MessageBox.Show(recursivecommand.ToString("x") + "-6");
                            if (recursivecommand == 0x06)
                            {
                                seg6r.BaseStream.Seek(3, SeekOrigin.Current);
                                rsp_add = seg6r.ReadBytes(4);
                                Array.Reverse(rsp_add);

                                Value = BitConverter.ToInt32(rsp_add, 0);
                                Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                                segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                                caddress = Convert.ToInt32(Binary.Substring(8, 24), 2);

                                System.IO.File.AppendAllText(savePath, "NEWOBJECT" + Environment.NewLine);
                                System.IO.File.AppendAllText(savePath, newR.ToString() + ", " + newG.ToString() + ", " + newB.ToString() + ", " + Environment.NewLine);
                                System.IO.File.AppendAllText(savePath, "Surface " + surfaceID.ToString() + "- Segment 0" + segid.ToString() + "00" + caddress.ToString("X") + Environment.NewLine);

                            }
                        }
                        if (segid == 7)
                        {   
                            seg7r.BaseStream.Seek(caddress, SeekOrigin.Begin);
                            recursivecommand = seg7r.ReadByte();
                            //MessageBox.Show(recursivecommand.ToString("x")+"-7");
                            if (recursivecommand == 0x06)
                            {
                                seg7r.BaseStream.Seek(3, SeekOrigin.Current);
                                rsp_add = seg7r.ReadBytes(4);
                                Array.Reverse(rsp_add);

                                Value = BitConverter.ToInt32(rsp_add, 0);
                                Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                                segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                                caddress = Convert.ToInt32(Binary.Substring(8, 24), 2);

                                System.IO.File.AppendAllText(savePath, "NEWOBJECT" + Environment.NewLine);
                                System.IO.File.AppendAllText(savePath, newR.ToString() + ", " + newG.ToString() + ", " + newB.ToString() + ", " + Environment.NewLine);
                                System.IO.File.AppendAllText(savePath, "Surface " + surfaceID.ToString() + "- Segment 0" + segid.ToString() + "00" + caddress.ToString("X") + Environment.NewLine);

                            }
                        }
                        
                       


                        for (int n = 0; n < 1;)
                        {
                            if (segid == 6)
                            {
                                seg6r.BaseStream.Seek(caddress, SeekOrigin.Begin);
                                recursivecommand = seg6r.ReadByte();
                                //MessageBox.Show(recursivecommand.ToString("X"));
                                if (recursivecommand == 0x04)
                                {
                                    output = mk.F3DEX_Model(out vertCache, recursivecommand, seg6, seg4, vaddress, caddress + 1,vertCache);
                                    vaddress = Convert.ToInt32(output);
                                    output = "";
                                }
                                if (recursivecommand == 0xE4)
                                {
                                    caddress += 4;
                                }
                                if (recursivecommand == 0xB1)
                                {
                                    output = mk.F3DEX_Model(out vertCache, recursivecommand, seg6, seg4, vaddress, caddress + 1, vertCache);
                                }
                                if (recursivecommand == 0xBF)
                                {
                                    output = mk.F3DEX_Model(out vertCache, recursivecommand, seg6, seg4, vaddress, caddress + 1, vertCache);
                                }
                                if (recursivecommand == 0xB8)
                                {
                                    output = "";

                                    System.IO.File.AppendAllText(savePath, "ENDOBJECT" + Environment.NewLine);
                                    System.IO.File.AppendAllText(savePath, newR.ToString() + ", " + newG.ToString() + ", " + newB.ToString() + ", " + Environment.NewLine);
                                    System.IO.File.AppendAllText(savePath, "Surface " + surfaceID.ToString() + "- Segment 0" + segid.ToString() + "00" + caddress.ToString("X") + Environment.NewLine);

                                    n = 2;
                                }
                                if (output != "")
                                {
                                    System.IO.File.AppendAllText(savePath, output);
                                }
                            }

                            if (segid == 7)
                            {
                                seg7r.BaseStream.Seek(caddress, SeekOrigin.Begin);
                                recursivecommand = seg7r.ReadByte();
                                //MessageBox.Show(recursivecommand.ToString("X"));


                                if (recursivecommand == 0x04)
                                {
                                    output = mk.F3DEX_Model(out vertCache, recursivecommand, seg7, seg4, vaddress, caddress + 1,vertCache);
                                    vaddress = Convert.ToInt32(output);
                                    // MessageBox.Show("Vert Update-" + vaddress.ToString());
                                    output = "";
                                }
                                if (recursivecommand == 0xE4)
                                {
                                    caddress += 4;
                                }
                                if (recursivecommand == 0xB1)
                                {
                                    output = mk.F3DEX_Model(out vertCache, recursivecommand, seg7, seg4, vaddress, caddress + 1, vertCache);
                                }
                                if (recursivecommand == 0xBF)
                                {
                                    output = mk.F3DEX_Model(out vertCache, recursivecommand, seg7, seg4, vaddress, caddress + 1, vertCache);
                                }
                                if (recursivecommand == 0xB8)
                                {
                                    output = "";

                                    System.IO.File.AppendAllText(savePath, "ENDOBJECT" + Environment.NewLine);
                                    System.IO.File.AppendAllText(savePath, newR.ToString() + ", " + newG.ToString() + ", " + newB.ToString() + ", " + Environment.NewLine);
                                    System.IO.File.AppendAllText(savePath, "Surface " + surfaceID.ToString() + "- Segment 0" + segid.ToString() + "00" + caddress.ToString("X") + Environment.NewLine);

                                    n = 2;
                                }
                                if (output != "")
                                {
                                    System.IO.File.AppendAllText(savePath, output);
                                }
                            }
                            caddress += 8;
                        }
                        


                        System.IO.File.AppendAllText(savePath, "ENDSECTION" + Environment.NewLine); ;
                        System.IO.File.AppendAllText(savePath, newR.ToString() + ", " + newG.ToString() + ", " + newB.ToString() + ", " + Environment.NewLine);
                        System.IO.File.AppendAllText(savePath, "Surface " + surfaceID.ToString() + "- Segment 0" + segid.ToString() + "00" + caddress.ToString("X") + Environment.NewLine);
                        output = "";

                    }


                    //MessageBox.Show("Command-"+commandbyte.ToString("X"));

                    current_offset += 8;
                }




                MessageBox.Show("Finished");
                CloseStreams();

            }
        }

        private void cseg7btn_click(object sender, EventArgs e)
        {
            if (vertOpen.ShowDialog() == DialogResult.OK)
            {
                string compressedfile = vertOpen.FileName;
                if (vertSave.ShowDialog() == DialogResult.OK)
                {
                    savePath = vertSave.FileName;
                    MessageBox.Show(savePath);
                    byte[] seg7 = mk.compress_seg7(compressedfile);

                    File.WriteAllBytes(savePath, seg7);
                    MessageBox.Show("Finished");
                    CloseStreams();
                }
            }
        }

        private void cseg6btn_click(object sender, EventArgs e)
        {
            recompress_mio0();
        }

        private void cseg4btn_click(object sender, EventArgs e)
        {
            recompress_mio0();
        }

        private void recompress_mio0()
        {
            if (vertOpen.ShowDialog() == DialogResult.OK)
            {
                if (vertSave.ShowDialog() == DialogResult.OK)
                {
                    string savePath = vertSave.FileName;
                    byte[] inputFile = File.ReadAllBytes(vertOpen.FileName);
                    byte[] output = mk.compressMIO0(inputFile);
                    File.WriteAllBytes(savePath, output);
                    MessageBox.Show("Finished");
                    CloseStreams();
                }
            }
        }

        private void Proc9btn_Click(object sender, EventArgs e)
        {
            if (textSave.ShowDialog() == DialogResult.OK)
            {
                string outputDir = textSave.SelectedPath;
                
                cID = coursebox.SelectedIndex;
                mk.DumpTextures(cID,outputDir,filePath);
            }
            
        }

        private void Seg5btn_Click(object sender, EventArgs e)
        {
            if (vertSave.ShowDialog() == DialogResult.OK)
            {
                cID = coursebox.SelectedIndex;
                savePath = vertSave.FileName;
                byte[] ROM = File.ReadAllBytes(filePath);
                byte[] seg5 = mk.dumpseg5(cID, ROM);


                File.WriteAllBytes(savePath, seg5);
                MessageBox.Show("Finished");
                CloseStreams();
            }
        }

        private void SegmentExporter_Load(object sender, EventArgs e)
        {
            headerBox.SelectedIndex = 0;
        }
    }
}
