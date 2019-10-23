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
using System.Windows.Media.Imaging;



//  1188752   TRACK HEADER OFFSET US ROM



namespace OverKart64
{
    public partial class CourseImporter : Form
    {
        public CourseImporter()
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

        OK64 mk = new OK64();

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


        private void DumpTextures()
        {


            if (textsave.ShowDialog() == DialogResult.OK)
            {
                string outputdir = textsave.SelectedPath;

                cID = coursebox.SelectedIndex;


                byte[] rombytes = File.ReadAllBytes(filePath);
                byte[] seg9 = new byte[(seg9_end[cID] - seg9_addr[cID])];

                Buffer.BlockCopy(rombytes, Convert.ToInt32(seg9_addr[cID]), seg9, 0, (Convert.ToInt32(seg9_end[cID]) - Convert.ToInt32(seg9_addr[cID])));



                string[] coursename = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut" };


                bs = new MemoryStream(seg9);

                bw = new BinaryWriter(bs);
                br = new BinaryReader(bs);
                int moffset = new int();
                for (int i = 0; br.BaseStream.Position < br.BaseStream.Length; i++)
                {
                    flip4 = br.ReadBytes(4);
                    Array.Reverse(flip4);
                    moffset = BitConverter.ToInt32(flip4, 0);
                    if (moffset != 0)
                    {
                        moffset = (moffset & 0xFFFFFF) + 0x641F70;
                        List<byte> texturelist = mk.decompress_MIO0(moffset, filePath);
                        byte[] texturebytes = texturelist.ToArray();
                        string texturepath = Path.Combine(outputdir, coursename[cID] + i.ToString());




                        File.WriteAllBytes(texturepath + ".bin", texturebytes);

                        br.BaseStream.Seek(12, SeekOrigin.Current);
                    }
                    else
                    {
                        br.BaseStream.Seek(br.BaseStream.Length, SeekOrigin.Begin);
                    }
                }
                MessageBox.Show("Finished");


            }


        }


        private void dumpseg5()
        {

            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                string outputdir = vertsave.FileName;

                cID = coursebox.SelectedIndex;


                byte[] rombytes = File.ReadAllBytes(filePath);
                byte[] seg9 = new byte[(seg9_end[cID] - seg9_addr[cID])];
                List<byte> segment5 = new List<byte>();

                Buffer.BlockCopy(rombytes, Convert.ToInt32(seg9_addr[cID]), seg9, 0, (Convert.ToInt32(seg9_end[cID]) - Convert.ToInt32(seg9_addr[cID])));



                string[] coursename = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut" };


                bs = new MemoryStream(seg9);

                bw = new BinaryWriter(bs);
                br = new BinaryReader(bs);
                int moffset = new int();
                for (int i = 0; br.BaseStream.Position < br.BaseStream.Length; i++)
                {
                    flip4 = br.ReadBytes(4);
                    Array.Reverse(flip4);
                    moffset = BitConverter.ToInt32(flip4, 0);
                    if (moffset != 0)
                    {
                        moffset = (moffset & 0xFFFFFF) + 0x641F70;
                        List<byte> texturelist = mk.decompress_MIO0(moffset, filePath);
                        segment5.AddRange(texturelist);
                        br.BaseStream.Seek(12, SeekOrigin.Current);
                    }
                    else
                    {
                        br.BaseStream.Seek(br.BaseStream.Length, SeekOrigin.Begin);
                    }
                }

                byte[] seg5 = segment5.ToArray();

                File.WriteAllBytes(outputdir, seg5);

                MessageBox.Show("Finished");


            }
        }



        private void DumpFaces(object sender, EventArgs e)
        {


            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertsave.FileName;









                cID = coursebox.SelectedIndex;

                int miooffset = new int();
                miooffset = Convert.ToInt32(seg4_addr[cID]);


                List<byte> decompressedverts = mk.decompress_MIO0(miooffset, filePath);
                byte[] Verts = decompressedverts.ToArray();
                // we do a sloppy copy and don't convert the verts to their 16byte "proper" form.

                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];


                MessageBox.Show(seg7_addr.ToString());

                fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                ds = new MemoryStream(Verts);
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
                        System.IO.File.AppendAllText(savePath, "ENDSECTION" + Environment.NewLine);
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


                        System.IO.File.AppendAllText(savePath, v0.ToString() + "-" + v1.ToString() + "-" + v2.ToString() + Environment.NewLine);



                        dr.BaseStream.Seek(vertoffset + (v0 * 14), SeekOrigin.Begin);

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


                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0); //s
                        valuesign16 = Convert.ToInt16((valuesign16 / texwidth) / 32);
                        System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);
                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0); //t
                        valuesign16 = Convert.ToInt16((valuesign16 * -1 / texheight) / 32);
                        System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);

                        dr.BaseStream.Seek(vertoffset + (v1 * 14), SeekOrigin.Begin);

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


                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0); //s
                        valuesign16 = Convert.ToInt16((valuesign16 / texwidth) / 32);
                        System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);
                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0); //t
                        valuesign16 = Convert.ToInt16((valuesign16 * -1 / texheight) / 32);
                        System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);



                        dr.BaseStream.Seek(vertoffset + (v2 * 14), SeekOrigin.Begin);

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


                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0); //s
                        valuesign16 = Convert.ToInt16((valuesign16 / texwidth) / 32);
                        System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);
                        flip2 = dr.ReadBytes(2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0); //t
                        valuesign16 = Convert.ToInt16((valuesign16 * -1 / texheight) / 32);
                        System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);



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


                            System.IO.File.AppendAllText(savePath, v0.ToString() + "-" + v1.ToString() + "-" + v2.ToString() + Environment.NewLine);


                            dr.BaseStream.Seek(vertoffset + (v0 * 14), SeekOrigin.Begin);

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
                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0); //s
                            valuesign16 = Convert.ToInt16((valuesign16 / texwidth) / 32);
                            System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);
                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0); //t
                            valuesign16 = Convert.ToInt16((valuesign16 * -1 / texheight) / 32);
                            System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);

                            dr.BaseStream.Seek(vertoffset + (v1 * 14), SeekOrigin.Begin);

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
                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0); //s
                            valuesign16 = Convert.ToInt16((valuesign16 / texwidth) / 32);
                            System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);
                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0); //t
                            valuesign16 = Convert.ToInt16((valuesign16 * -1 / texheight) / 32);
                            System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);



                            dr.BaseStream.Seek(vertoffset + (v2 * 14), SeekOrigin.Begin);

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
                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0); //s
                            valuesign16 = Convert.ToInt16((valuesign16 / texwidth) / 32);
                            System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);
                            flip2 = dr.ReadBytes(2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0); //t
                            valuesign16 = Convert.ToInt16((valuesign16 * -1 / texheight) / 32);
                            System.IO.File.AppendAllText(savePath, valuesign16.ToString() + Environment.NewLine);
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
            seg6a.Text = seg6_addr[coursebox.SelectedIndex].ToString();
            seg6e.Text = seg6_end[coursebox.SelectedIndex].ToString();
            seg4a.Text = seg4_addr[coursebox.SelectedIndex].ToString();
            seg7e.Text = seg7_end[coursebox.SelectedIndex].ToString();
            seg9a.Text = seg9_addr[coursebox.SelectedIndex].ToString();
            seg9e.Text = seg9_end[coursebox.SelectedIndex].ToString();
            seg47ra.Text = seg47_buf[coursebox.SelectedIndex].ToString();
            nv.Text = numVtxs[coursebox.SelectedIndex].ToString();
            seg7ra.Text = seg7_ptr[coursebox.SelectedIndex].ToString();
            seg7s.Text = seg7_size[coursebox.SelectedIndex].ToString();
            texra.Text = texture_addr[coursebox.SelectedIndex].ToString();
            flagbox.Text = flag[coursebox.SelectedIndex].ToString();
            padbox.Text = unused[coursebox.SelectedIndex].ToString();
            seg7rom.Text = seg7_romptr[coursebox.SelectedIndex].ToString();
        }




        private void DumpVerts(object sender, EventArgs e)
        {

            cID = coursebox.SelectedIndex;
            if (vertsave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertsave.FileName;

                List<byte> decompressedverts = mk.decompress_MIO0(Convert.ToInt32(seg4_addr[cID]), filePath);
                byte[] Verts = decompressedverts.ToArray();

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
            if (vertsave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertsave.FileName;
                List<byte> decompressedverts = mk.decompress_MIO0(Convert.ToInt32(seg6_addr[cID]), filePath);
                byte[] seg6 = decompressedverts.ToArray();


                fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                ds = new MemoryStream(seg6);


                ds.CopyTo(fs);
                MessageBox.Show("Finished");
                CloseStreams();

            }
        }

        private void segment7_decomp(object sender, EventArgs e)
        {

            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertsave.FileName;
                MessageBox.Show(savePath);
                byte[] seg7 = mk.decompress_seg7(filePath);

                File.WriteAllBytes(savePath, seg7);
                MessageBox.Show("Finished");
                CloseStreams();
            }
        }





        private void segment4_decompress(object sender, EventArgs e)
        {
            cID = coursebox.SelectedIndex;
            if (vertsave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertsave.FileName;
                List<byte> decompressedverts = mk.decompress_MIO0(Convert.ToInt32(seg4_addr[cID]), filePath);
                List<byte> insert = new List<byte> { 0x00, 0x00 };
                int vertcount = (decompressedverts.Count / 14);

                for (int i = 0; i < vertcount; i++)
                {
                    decompressedverts.InsertRange((i + 1) * 10 + (i * 2) + (i * 4), insert);
                }



                byte[] seg4 = decompressedverts.ToArray();


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
            if (vertsave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertsave.FileName;
                List<byte> decompressedverts = mk.decompress_MIO0(Convert.ToInt32(seg6_addr[cID]), filePath);
                byte[] seg6 = decompressedverts.ToArray();

                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];

                decompressedverts = mk.decompress_MIO0(Convert.ToInt32(seg4_addr[cID]), filePath);
                List<byte> insert = new List<byte> { 0x00, 0x00 };
                int vertcount = (decompressedverts.Count / 14);

                for (int i = 0; i < vertcount; i++)
                {
                    decompressedverts.InsertRange((i + 1) * 10 + (i * 2) + (i * 4), insert);
                }
                byte[] seg4 = decompressedverts.ToArray();

                byte[] seg7 = new byte[seg7_size[cID]];
                seg7 = mk.decompress_seg7(filePath);




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


                current_offset = 0;
                for (int i = 0; i < 1;)
                {

                    seg6r.BaseStream.Seek(current_offset, SeekOrigin.Begin);
                    commandbyte = seg6r.ReadByte();

                    output = "";

                    //MessageBox.Show("-Execute Order 0x" + commandbyte.ToString("X")+"--"+current_offset.ToString());

                    if (commandbyte == 0x00)
                    {
                        //End F3DEX Display Lists....probably? lol
                        i = 2;
                    }

                    //Special Conditional
                    if (commandbyte == 0xE4)
                    {
                        current_offset += 4;
                    }
                    //Special Conditional

                    if (commandbyte == 0x04)
                    {
                        output = mk.translate_F3D(commandbyte, seg6r, vr, vaddress);
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
                        output = mk.translate_F3D(commandbyte, seg6r, vr, vaddress);

                        StringReader sread = new StringReader(output);




                        int segid = Convert.ToInt32(sread.ReadLine());
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
                                    output = mk.translate_F3D(recursivecommand, seg6r, seg4r, vaddress);
                                    vaddress = Convert.ToInt32(output);
                                    output = "";
                                }
                                if (recursivecommand == 0xE4)
                                {
                                    caddress += 4;
                                }
                                if (recursivecommand == 0xB1)
                                {
                                    output = mk.translate_F3D(recursivecommand, seg6r, seg4r, vaddress);
                                }
                                if (recursivecommand == 0xBF)
                                {
                                    output = mk.translate_F3D(recursivecommand, seg6r, seg4r, vaddress);
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
                                    output = mk.translate_F3D(recursivecommand, seg7r, seg4r, vaddress);
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
                                    output = mk.translate_F3D(recursivecommand, seg7r, seg4r, vaddress);
                                }
                                if (recursivecommand == 0xBF)
                                {
                                    output = mk.translate_F3D(recursivecommand, seg7r, seg4r, vaddress);
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
                    if (commandbyte == 0xB8)
                    {
                        output = "ENDSECTION" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    }



                    if (output != "")
                    {
                        System.IO.File.AppendAllText(savePath, output);
                    }

                    //MessageBox.Show("Command-"+commandbyte.ToString("X"));

                    current_offset += 8;
                }




                MessageBox.Show("Finished");
                CloseStreams();

            }

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


        private void dumpasm_btn(object sender, EventArgs e)
        {
            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertsave.FileName;




                byte[] asm = mk.dump_ASM(filePath);

                File.WriteAllBytes(savePath, asm);
                MessageBox.Show("Finished");
                CloseStreams();
            }
        }

        private void procasmbtn(object sender, EventArgs e)
        {

            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertsave.FileName;


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
            if (vertsave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertsave.FileName;
                List<byte> decompressedverts = mk.decompress_MIO0(Convert.ToInt32(seg6_addr[cID]), filePath);
                byte[] seg6 = decompressedverts.ToArray();

                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];

                decompressedverts = mk.decompress_MIO0(Convert.ToInt32(seg4_addr[cID]), filePath);
                List<byte> insert = new List<byte> { 0x00, 0x00 };
                int vertcount = (decompressedverts.Count / 14);

                for (int i = 0; i < vertcount; i++)
                {
                    decompressedverts.InsertRange((i + 1) * 10 + (i * 2) + (i * 4), insert);
                }
                byte[] seg4 = decompressedverts.ToArray();

                byte[] seg7 = new byte[seg7_size[cID]];
                seg7 = mk.decompress_seg7(filePath);




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



                current_offset = surfaceoffset[cID];
                for (int i = 0; i < 1;)
                {

                    seg6r.BaseStream.Seek(current_offset, SeekOrigin.Begin);
                    byte[] rsp_add = seg6r.ReadBytes(4);


                    if (rsp_add[0] == 0x00)
                    {
                        i = 2;
                    }
                    else
                    {


                        Array.Reverse(rsp_add);

                        int Value = BitConverter.ToInt32(rsp_add, 0);
                        String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                        int segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                        int caddress = Convert.ToInt32(Binary.Substring(8, 24), 2);

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
                                    output = mk.translate_F3D(recursivecommand, seg6r, seg4r, vaddress);
                                    vaddress = Convert.ToInt32(output);
                                    output = "";
                                }
                                if (recursivecommand == 0xE4)
                                {
                                    caddress += 4;
                                }
                                if (recursivecommand == 0xB1)
                                {
                                    output = mk.translate_F3D(recursivecommand, seg6r, seg4r, vaddress);
                                }
                                if (recursivecommand == 0xBF)
                                {
                                    output = mk.translate_F3D(recursivecommand, seg6r, seg4r, vaddress);
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
                                    output = mk.translate_F3D(recursivecommand, seg7r, seg4r, vaddress);
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
                                    output = mk.translate_F3D(recursivecommand, seg7r, seg4r, vaddress);
                                }
                                if (recursivecommand == 0xBF)
                                {
                                    output = mk.translate_F3D(recursivecommand, seg7r, seg4r, vaddress);
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


                    //MessageBox.Show("Command-"+commandbyte.ToString("X"));

                    current_offset += 8;
                }




                MessageBox.Show("Finished");
                CloseStreams();

            }
        }

        private void cseg7btn_click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                string compressedfile = vertopen.FileName;
                if (vertsave.ShowDialog() == DialogResult.OK)
                {
                    savePath = vertsave.FileName;
                    MessageBox.Show(savePath);
                    byte[] seg7 = mk.compress_seg7(compressedfile);

                    File.WriteAllBytes(savePath, seg7);
                    MessageBox.Show("Finished");
                    CloseStreams();
                }
            }
        }

        private void Debug_btn_Click(object sender, EventArgs e)
        {
            forceboosts();
        }

        private void forceboosts()
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                string seg6Path = vertopen.FileName;

                byte[] seg6 = File.ReadAllBytes(seg6Path);
                cID = coursebox.SelectedIndex;
                if (vertsave.ShowDialog() == DialogResult.OK)
                {

                    savePath = vertsave.FileName;


                    seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];

                    List<byte> decompressedverts = mk.decompress_MIO0(Convert.ToInt32(seg4_addr[cID]), filePath);
                    List<byte> insert = new List<byte> { 0x00, 0x00 };
                    int vertcount = (decompressedverts.Count / 14);

                    for (int i = 0; i < vertcount; i++)
                    {
                        decompressedverts.InsertRange((i + 1) * 10 + (i * 2) + (i * 4), insert);
                    }
                    byte[] seg4 = decompressedverts.ToArray();

                    byte[] seg7 = new byte[seg7_size[cID]];
                    seg7 = mk.decompress_seg7(filePath);




                    MemoryStream seg7m = new MemoryStream();
                    MemoryStream seg6m = new MemoryStream();
                    MemoryStream seg4m = new MemoryStream();
                    BinaryReader seg7r = new BinaryReader(seg7m);
                    BinaryReader seg6r = new BinaryReader(seg6m);
                    BinaryReader seg4r = new BinaryReader(seg4m);
                    BinaryWriter seg6w = new BinaryWriter(seg6m);
                    int vaddress = new int();

                    seg7m.Write(seg7, 0, seg7.Length);
                    seg6m.Write(seg6, 0, seg6.Length);
                    seg4m.Write(seg4, 0, seg4.Length);

                    byte commandbyte = new byte();
                    byte[] byte29 = new byte[2];

                    string output = "";
                    byte[] voffset = new byte[2];
                    // offsets to surface maps for each course, in course order. battle maps have no surface map and are listed as 0xFF for simplicicty.

                    int[] surfaceoffset = { 0x9650, 0x72d0, 0x93d8, 0xb458, 0x18240, 0x79a0, 0x18fd8, 0xdc28, 0xff28, 0x144b8, 0x23b68, 0x23070, 0x9c20, 0x16440, 0xcc38, 0xff, 0xff, 0xff, 0x14338, 0xff };



                    current_offset = surfaceoffset[cID];
                    for (int i = 0; i < 1;)
                    {

                        seg6r.BaseStream.Seek(current_offset, SeekOrigin.Begin);
                        byte[] rsp_add = seg6r.ReadBytes(4);
                        seg6w.Write(Convert.ToByte(0xFC));

                        if (rsp_add[0] == 0x00)
                        {
                            i = 2;
                        }
                        current_offset += 8;
                    }

                    seg6 = seg6m.ToArray();
                    File.WriteAllBytes(savePath, seg6);

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
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                if (vertsave.ShowDialog() == DialogResult.OK)
                {
                    string savePath = vertsave.FileName;
                    byte[] output = mk.compressInitialization(vertopen.FileName, true);
                    File.WriteAllBytes(savePath, output);
                    MessageBox.Show("Finished");
                    CloseStreams();
                }
            }
        }

        private void Proc9btn_Click(object sender, EventArgs e)
        {
            DumpTextures();
        }

        private void Seg5btn_Click(object sender, EventArgs e)
        {
            dumpseg5();
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

        private void compileROM()
        {
            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertsave.FileName;
                cID = coursebox.SelectedIndex;

                byte[] seg4 = new byte[0];
                if (seg4box.CheckState == CheckState.Checked)
                {

                    byte[] tempseg = File.ReadAllBytes(seg4text.Text);
                    Array.Resize(ref seg4, tempseg.Length);
                    Array.Copy(tempseg, 0, seg4, 0, tempseg.Length);
                }
                else
                {
                    byte[] tempseg = dump_seg4();
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
                    byte[] tempseg = dump_seg6();
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
                    byte[] tempseg = dump_seg7();
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
                    byte[] tempseg = dump_seg9();
                    Array.Resize(ref seg9, tempseg.Length);
                    Array.Copy(tempseg, 0, seg9, 0, tempseg.Length);
                }

                byte[] rombytes = File.ReadAllBytes(filePath);

                bs = new MemoryStream();
                bs.Write(rombytes, 0, rombytes.Length);
                bw = new BinaryWriter(bs);
                br = new BinaryReader(bs);

                int seg6start = 0;
                int seg6end = 0;
                int seg4start = 0;
                int seg7end = 0;
                int seg9start = 0;
                int seg9end = 0;
                int seg7start = 0;
                int seg7rsp = 0;


                bw.BaseStream.Position = bw.BaseStream.Length;

                seg6start = Convert.ToInt32(bw.BaseStream.Position);
                bw.Write(seg6, 0, seg6.Length);
                seg6end = Convert.ToInt32(bw.BaseStream.Position);
                //
                seg9start = Convert.ToInt32(bw.BaseStream.Position);
                bw.Write(seg9, 0, seg9.Length);
                seg9end = Convert.ToInt32(bw.BaseStream.Position);
                //
                seg4start = Convert.ToInt32(bw.BaseStream.Position);
                bw.Write(seg4, 0, seg4.Length);
                seg7start = Convert.ToInt32(bw.BaseStream.Position);
                bw.Write(seg7, 0, seg7.Length);
                seg7end = Convert.ToInt32(bw.BaseStream.Position);
                //
                seg7rsp = Convert.ToInt32(seg7start + seg47_buf[cID] - seg4start);



                //

                byte[] flip = new byte[4];

                flip = BitConverter.GetBytes(seg6start);
                Array.Reverse(flip);
                seg6start = BitConverter.ToInt32(flip, 0);

                flip = BitConverter.GetBytes(seg6end);
                Array.Reverse(flip);
                seg6end = BitConverter.ToInt32(flip, 0);

                flip = BitConverter.GetBytes(seg4start);
                Array.Reverse(flip);
                seg4start = BitConverter.ToInt32(flip, 0);

                flip = BitConverter.GetBytes(seg7end);
                Array.Reverse(flip);
                seg7end = BitConverter.ToInt32(flip, 0);

                flip = BitConverter.GetBytes(seg9start);
                Array.Reverse(flip);
                seg9start = BitConverter.ToInt32(flip, 0);

                flip = BitConverter.GetBytes(seg9end);
                Array.Reverse(flip);
                seg9end = BitConverter.ToInt32(flip, 0);

                flip = BitConverter.GetBytes(seg7rsp);
                Array.Reverse(flip);
                seg7rsp = BitConverter.ToInt32(flip, 0);


                //




                bw.BaseStream.Seek(1188752 + (cID * 48), SeekOrigin.Begin);

                bw.Write(seg6start);
                bw.Write(seg6end);
                bw.Write(seg4start);
                bw.Write(seg7end);
                bw.Write(seg9start);
                bw.Write(seg9end);
                bw.BaseStream.Seek(8, SeekOrigin.Current);
                bw.Write(seg7rsp);


                File.WriteAllBytes(savePath, bs.ToArray());
                MessageBox.Show("Finished");
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            compileROM();
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
    }
}