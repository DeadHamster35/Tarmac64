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



//  1188752   TRACK HEADER OFFSET US ROM



namespace OverKart64
{
    public partial class VertConvert : Form
    {
        public VertConvert()
        {
            InitializeComponent();
        }


        string filePath = "";
        string savePath = "";

        byte[] flip4 = new byte[4];
        byte[] flip2 = new byte[2];

        int[] img_types = new int[] { 0, 0, 0, 3, 3, 3 };
        int[] img_heights = new int[] { 32, 32, 64, 32, 32, 64};
        int[] img_widths = new int[] { 32, 64, 32, 32, 64, 32 };

        int v0 = 0;
        int v1 = 0;
        int v2 = 0;
        int cID = 0;

        int texwidth = 0;
        int x_mask = 0;
        int x_flags = 0;
        int texheight = 0;
        int y_mask = 0;
        int y_flags = 0;
        int textype = 0;

        int seg7_addr = new int();

        UInt16 value16 = new UInt16();
        Int16 valuesign16 = new Int16();
        


        Int32[] seg6_addr = new Int32[20];      //(0x00) ROM address at which segment 6 file begins
        Int32[] seg6_end = new Int32[20];       //(0x04) ROM address at which segment 6 file ends
        Int32[] seg4_addr = new Int32[20];      //(0x08) ROM address at which segment 4 file begins
        Int32[] seg7_end = new Int32[20];       //(0x0C) ROM address at which segment 7 (not 4) file ends
        Int32[] seg9_addr = new Int32[20];      //(0x10) ROM address at which segment 9 file begins
        Int32[] seg9_end = new Int32[20];       //(0x14) ROM address at which segment 9 file ends
        Int32[] seg47_buf = new Int32[20];      //(0x18) RSP address of compressed segments 4 and 7
        Int32[] numVtxs = new Int32[20];        //(0x1C) number of vertices in the vertex file
        Int32[] seg7_ptr = new Int32[20];       //(0x20) RSP address at which segment 7 data begins
        Int32[] seg7_size = new Int32[20];      //(0x24) Size of segment 7 data after decompression, minus 8 bytes for some reason
        Int32[] texture_addr = new Int32[20];   //(0x28) RSP address of texture list
        Int16[] flag = new Int16[20];           //(0x2C) Unknown
        Int16[] unused = new Int16[20];         //(0x2E) Padding

        OpenFileDialog vertopen = new OpenFileDialog();
        SaveFileDialog vertsave = new SaveFileDialog();

        private void DumpFaces(object sender, EventArgs e)
        {
            
               
            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertsave.FileName;









                cID = coursebox.SelectedIndex;

                int miooffset = new int();
                miooffset = seg4_addr[cID];


                List<byte> decompressedverts = decompress(miooffset, filePath);
                byte[] Verts = decompressedverts.ToArray();


                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];


                MessageBox.Show(seg7_addr.ToString());


                using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                using (var ds = new MemoryStream(Verts))
                using (var br = new BinaryReader(fs))
                using (var dr = new BinaryReader(ds))

                {


                    byte commandbyte = new byte();
                    byte[] byte29 = new byte[2];

                   

                    br.BaseStream.Seek(seg7_addr, SeekOrigin.Begin);

                    int vertoffset = 0;
                    byte[] voffset = new byte[2];

                    bool DispEnd = true;

                    for (; DispEnd ;)
                    {


                        commandbyte = br.ReadByte();

                        if (commandbyte == 0xFF) 
                        {
                            
                            DispEnd = false;
                        }
                        if ((commandbyte >= 0x1A && commandbyte <= 0x1F) )
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
                            System.IO.File.AppendAllText(savePath, "." + Environment.NewLine);
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
                            valuesign16 = Convert.ToInt16((valuesign16 *-1 / texheight) / 32);
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



                }


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




                    

                    using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var ms = new MemoryStream())
                    using (var bw = new BinaryWriter(ms))
                    using (var br = new BinaryReader(ms))
                    {


                        fs.CopyTo(ms);
                        ms.Position = 0;


                        br.BaseStream.Seek(1188752, SeekOrigin.Begin);
                       
                        for (int i = 0; i < 20; i++)
                        {
                            seg6_addr[i] = br.ReadInt32();
                            seg6_end[i] = br.ReadInt32();
                            seg4_addr[i] = br.ReadInt32();
                            seg7_end[i] = br.ReadInt32();
                            seg9_addr[i] = br.ReadInt32();
                            seg9_end[i] = br.ReadInt32();
                            seg47_buf[i] = br.ReadInt32();
                            numVtxs[i] = br.ReadInt32();
                            seg7_ptr[i] = br.ReadInt32();
                            seg7_size[i] = br.ReadInt32();
                            texture_addr[i] = br.ReadInt32();
                            flag[i] = br.ReadInt16();
                            unused[i] = br.ReadInt16();

                            byte[] flip = new byte[4];

                            flip = BitConverter.GetBytes(seg6_addr[i]);
                            Array.Reverse(flip);
                            seg6_addr[i] = BitConverter.ToInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg6_end[i]);
                            Array.Reverse(flip);
                            seg6_end[i] = BitConverter.ToInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg4_addr[i]);
                            Array.Reverse(flip);
                            seg4_addr[i] = BitConverter.ToInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg7_end[i]);
                            Array.Reverse(flip);
                            seg7_end[i] = BitConverter.ToInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg9_addr[i]);
                            Array.Reverse(flip);
                            seg9_addr[i] = BitConverter.ToInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg9_end[i]);
                            Array.Reverse(flip);
                            seg9_end[i] = BitConverter.ToInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg47_buf[i]);
                            Array.Reverse(flip);
                            seg47_buf[i] = BitConverter.ToInt32(flip, 0);

                            flip = BitConverter.GetBytes(numVtxs[i]);
                            Array.Reverse(flip);
                            numVtxs[i] = BitConverter.ToInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg7_ptr[i]);
                            Array.Reverse(flip);
                            seg7_ptr[i] = BitConverter.ToInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg7_size[i]);
                            Array.Reverse(flip);
                            seg7_size[i] = BitConverter.ToInt32(flip, 0);
                           
                            flip = BitConverter.GetBytes(texture_addr[i]);
                            Array.Reverse(flip);
                            texture_addr[i] = BitConverter.ToInt32(flip, 0);

                            byte[] flop = new byte[2];

                            flop = BitConverter.GetBytes(flag[i]);
                            Array.Reverse(flip);
                            flag[i] = BitConverter.ToInt16(flip, 0);

                            flop = BitConverter.GetBytes(unused[i]);
                            Array.Reverse(flip);
                            unused[i] = BitConverter.ToInt16(flip, 0);

                        }

                        


                        string[] items = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut"};
                        
                        foreach (string item in items)
                        {
                            coursebox.Items.Add(item);
                            
                        }

                        
                        coursebox.SelectedIndex = 0;
                        MessageBox.Show("ROM Loaded");
                    }
                }

            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
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
        }


        private List<byte>  decompress(int offset, string path)
        {

            FileStream inputFile = File.Open(path, FileMode.Open);
            BigEndianBinaryReader br = new BigEndianBinaryReader(inputFile);

            byte[] file = br.ReadBytes((int)inputFile.Length);


            List<byte> newFile = new List<byte>();


            br.BaseStream.Position = offset;
            string magicNumber = Encoding.ASCII.GetString(br.ReadBytes(4));

            if (magicNumber == "MIO0")
            {
                int decompressedLength = br.ReadInt32();
                int compressedOffset = br.ReadInt32() + offset;
                int uncompressedOffset = br.ReadInt32() + offset;
                int currentOffset;

                try
                {

                    while (newFile.Count < decompressedLength)
                    {

                        byte bits = br.ReadByte(); //byte of layout bits
                        BitArray arrayOfBits = new BitArray(new byte[1] { bits });

                        for (int i = 7; i > -1 && (newFile.Count < decompressedLength); i--) //iterate through layout bits
                        {

                            if (arrayOfBits[i] == true)
                            {
                                //non-compressed
                                //add one byte from uncompressedOffset to newFile

                                currentOffset = (int)inputFile.Position;

                                inputFile.Seek(uncompressedOffset, SeekOrigin.Begin);

                                newFile.Add(br.ReadByte());
                                uncompressedOffset++;

                                inputFile.Seek(currentOffset, SeekOrigin.Begin);

                            }
                            else
                            {
                                //compressed
                                //read 2 bytes
                                //4 bits = length
                                //12 bits = offset

                                currentOffset = (int)inputFile.Position;
                                inputFile.Seek(compressedOffset, SeekOrigin.Begin);

                                byte byte1 = br.ReadByte();
                                byte byte2 = br.ReadByte();
                                compressedOffset += 2;

                                //Note: For Debugging, binary representations can be printed with:  Convert.ToString(numberVariable, 2);

                                byte byte1Upper = (byte)((byte1 & 0x0F));//offset bits
                                byte byte1Lower = (byte)((byte1 & 0xF0) >> 4); //length bits

                                int combinedOffset = ((byte1Upper << 8) | byte2);

                                int finalOffset = 1 + combinedOffset;
                                int finalLength = 3 + byte1Lower;

                                for (int k = 0; k < finalLength; k++) //add data for finalLength iterations
                                {
                                    newFile.Add(newFile[newFile.Count - finalOffset]); //add byte at offset (fileSize - finalOffset) to file
                                }

                                inputFile.Seek(currentOffset, SeekOrigin.Begin); //return to layout bits

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                

                
            }
            inputFile.Close();
            return newFile;

        }

        private void DumpVerts(object sender, EventArgs e)
        {

            cID = coursebox.SelectedIndex;
            if (vertsave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertsave.FileName;
                List<byte> decompressedverts = decompress(seg4_addr[cID], filePath);
                byte[] Verts = decompressedverts.ToArray();
                bool VertEnd = true;

                using (var ds = new MemoryStream(Verts))
                using (var dr = new BinaryReader(ds))
                {
                    dr.BaseStream.Position = 0;
                    for (int i = 0; VertEnd ; i++)
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
                }
            }
        }

        private void Segment6_decompress(object sender, EventArgs e)
        {
            cID = coursebox.SelectedIndex;
            if (vertsave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertsave.FileName;
                List<byte> decompressedverts = decompress(seg6_addr[cID], filePath);
                byte[] seg6 = decompressedverts.ToArray();
                

                using (var fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                using (var ds = new MemoryStream(seg6))
                
                {
                    ds.CopyTo(fs);
                    MessageBox.Show("Finished");
                }
            }
        }

        private void segment7_dump(object sender, EventArgs e)
        {


            cID = coursebox.SelectedIndex;
            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertsave.FileName;
                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];
                MessageBox.Show(seg7_addr.ToString());

                byte[] rombytes = File.ReadAllBytes(filePath);
                byte[] seg7 = new byte[(seg7_end[cID] - seg7_addr)];

                Buffer.BlockCopy(rombytes, seg7_addr, seg7, 0, (seg7_end[cID] - seg7_addr));

                File.WriteAllBytes(savePath, seg7);

                MessageBox.Show("Finished");
            }
        }

        private void segment4_decompress(object sender, EventArgs e)
        {
            cID = coursebox.SelectedIndex;
            if (vertsave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertsave.FileName;
                List<byte> decompressedverts = decompress(seg4_addr[cID], filePath);
                byte[] seg4 = decompressedverts.ToArray();


                using (var fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                using (var ds = new MemoryStream(seg4))

                {
                    ds.CopyTo(fs);
                    MessageBox.Show("Finished");
                }
            }
        }

        private void dumpdlists(object sender, EventArgs e)
        {

            cID = coursebox.SelectedIndex;
            if (vertsave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertsave.FileName;
                List<byte> decompressedverts = decompress(seg6_addr[cID], filePath);
                byte[] Seg6 = decompressedverts.ToArray();

                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];

                decompressedverts = decompress(seg4_addr[cID], filePath);
                byte[] Verts = decompressedverts.ToArray();


                MessageBox.Show(seg7_addr.ToString());









                using (var bs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                using (var br = new BinaryReader(bs))
                using (var ds = new MemoryStream(Seg6))                
                using (var dr = new BinaryReader(ds))
                using (var vs = new MemoryStream(Verts))
                using (var vr = new BinaryReader(vs))
                {
                    byte commandbyte = new byte();
                    byte[] byte29 = new byte[2];



                    br.BaseStream.Seek(seg7_addr, SeekOrigin.Begin);

                    long current_offset = 0;
                    byte[] voffset = new byte[2];

                    bool DispEnd = true;

                    for (; DispEnd;)
                    {

                        current_offset = br.BaseStream.Position;
                        commandbyte = br.ReadByte();

                        




                    }


                }






                            MessageBox.Show("Finished");
                
            }


        }

        private void Button2_Click(object sender, EventArgs e)
        {


            cID = coursebox.SelectedIndex;
            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                savePath = vertsave.FileName;
                seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];
                MessageBox.Show(seg7_addr.ToString());

                byte[] rombytes = File.ReadAllBytes(filePath);
                byte[] seg9 = new byte[(seg9_end[cID] - seg9_addr[cID])];

                Buffer.BlockCopy(rombytes, seg9_addr[cID], seg9, 0, (seg9_end[cID] - seg9_addr[cID]));

                File.WriteAllBytes(savePath, seg9);

                MessageBox.Show("Finished");
            }
        }
    }
}
