using PeepsCompress;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using Tarmac64;
using Tarmac64_Library;


//custom libraries

using Texture64;  //for handling texture data


using Cereal64.Microcodes.F3DEX.DataElements;
using Cereal64.Common.DataElements;
using Cereal64.Common.Rom;
using Cereal64.Common.Utils.Encoding;
using System.Text.RegularExpressions;
using System.Security.Permissions;
using SharpDX;
using System.Globalization;
using System.Runtime.CompilerServices;

using Microsoft.WindowsAPICodePack.Dialogs;
using Assimp;

namespace Tarmac64
{
    public partial class DebugTools : Form
    { 


        public DebugTools()
        {
            InitializeComponent();
        }
        string[] items = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut" };
        int[] raceOrder = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 18 };

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
        public static UInt32[] seg7_romptr = new UInt32[20];
        string outputstring = "";

        int opint = new int();
        byte[] byte16 = new byte[2];
        byte[] byte32 = new byte[4];

        Int16 value16 = new Int16();
        
        Int32 value32 = new Int32();

        TM64 Tarmac = new TM64();
        TM64_Geometry.Vertex[] vertCache = new TM64_Geometry.Vertex[32];

        OpenFileDialog fileOpen = new OpenFileDialog();
        SaveFileDialog fileSave = new SaveFileDialog();


        private void Leftshift_Click(object sender, EventArgs e)
        {
            OutputBox.Text = leftshift(input.Text);
        }


        private void Rightshiftbtn_Click(object sender, EventArgs e)
        {
            OutputBox.Text = rightshift(input.Text);
        }


        private void andbtn_Click(object sender, EventArgs e)
        {
            logicalAND(input.Text);
        }

        private void Orbtn_Click(object sender, EventArgs e)
        {
            logicalOR(input.Text);
        }


        private void logicalAND(string inputstring)
        {
            value32 = Convert.ToInt32(inputstring, 16);
            
            opint = value32 & Convert.ToInt32(logicbox.Text,16);
            byte32 = BitConverter.GetBytes(opint);
            outputstring = BitConverter.ToString(byte32).Replace("-", "");

            MessageBox.Show(outputstring + Environment.NewLine + opint.ToString());

            

        }

        private void logicalOR(string inputstring)
        {
            value32 = Convert.ToInt32(inputstring, 16);

            opint = value32 | Convert.ToInt32(logicbox.Text, 16);
            byte32 = BitConverter.GetBytes(opint);
            outputstring = BitConverter.ToString(byte32).Replace("-", "");

            MessageBox.Show(outputstring + Environment.NewLine + opint.ToString());



        }



        private string leftshift(string inputstring)
        {

            string output = "";
            if (inputstring.Length == 8)
            {
                value32 = Convert.ToInt32(inputstring,16);
                
                opint = value32 << (Convert.ToInt32(shiftbox.Text));
                value32 = Convert.ToInt32(opint);
                byte32 = BitConverter.GetBytes(value32);
                Array.Reverse(byte32);
                output= (BitConverter.ToString(byte32).Replace("-", ""));

            }
            else if (inputstring.Length == 4)
            {
                value16 = Convert.ToInt16(inputstring,16);
                opint = Convert.ToInt32(value16) << (Convert.ToInt32(shiftbox.Text));
                value16 = Convert.ToInt16(opint);
                byte16 = BitConverter.GetBytes(value16);
                Array.Reverse(byte16);
                output= (BitConverter.ToString(byte16).Replace("-", ""));
            }

            return output;
        }

        private string rightshift(string inputstring)
        {
            string output = "";
            if (inputstring.Length == 8)
            {
                value32 = Convert.ToInt32(inputstring,16);           
                opint = value32 >> (Convert.ToInt32(shiftbox.Text));
                //opint = opint & 0xFF000000;
                value32 = Convert.ToInt32(opint);
                //MessageBox.Show(value32.ToString("x"));
                byte32 = BitConverter.GetBytes(value32);
                Array.Reverse(byte32);
                output = (BitConverter.ToString(byte16).Replace("-", ""));

            }
            else if (inputstring.Length == 4)
            {
                value16 = Convert.ToInt16(inputstring,16);
                opint = Convert.ToInt32(value16) >> (Convert.ToInt32(shiftbox.Text));
                value16 = Convert.ToInt16(opint);
                byte16 = BitConverter.GetBytes(value16);
                Array.Reverse(byte16);
                output = (BitConverter.ToString(byte16).Replace("-", ""));
            }
            return output;
        }

        private void Custom_Click(object sender, EventArgs e)
        {

            int v0 = Convert.ToInt32(v0box.Text);
            int v1 = Convert.ToInt32(v1box.Text);
            int v2 = Convert.ToInt32(v2box.Text);


           
            MessageBox.Show(value16.ToString()+"::"+v0.ToString() + "-" + v1.ToString() + "-" + v2.ToString() + "-");
            byte[] flip4 = new byte[4];


            flip4 = BitConverter.GetBytes(Convert.ToInt16((v2) | (v1 << 5) | v0 << 10));

            MessageBox.Show(BitConverter.ToString(flip4).Replace("-", ""));
           
        }

        private void DebugTools_Load(object sender, EventArgs e)
        {

        }





        private void export_Click(object sender, EventArgs e)
        {
            TM64 Tarmac = new TM64();
            
            int targetOffset = 0;
            int.TryParse(offsetbox.Text, out targetOffset);
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                byte[] inputFile = File.ReadAllBytes(fileOpen.FileName);

                byte[] decompressedFile =Tarmac.DecompressMIO0(inputFile);
                

                if (fileSave.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(fileSave.FileName, decompressedFile);

                }
            }
        }


        private void imgclick(object sender, EventArgs e)
        {
            Int32 ImgSize = 0, ImgType = 0, ImgFlag1 = 0, ImgFlag2 = 0, ImgFlag3 = 0, testflag = 0;
            Int32[] ImgTypes = { 0, 0, 0, 3, 3, 3, 0 }; //0=RGBA, 3=IA
            Int32[] ImgFlag1s = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; //texture sizes
            Int32[] ImgFlag2s = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 };
            byte[] flip4 = new byte[4];
            byte[] Param = new byte[2];
            value16 = Convert.ToInt16(imgbox2.Text, 16);
            Param = BitConverter.GetBytes(value16);
            
            Array.Reverse(Param);
            

            ImgType = ImgTypes[Convert.ToInt32(imgbox1.Text, 16) - 0x1A];
            ImgFlag1 = ImgFlag1s[Convert.ToInt32(imgbox1.Text, 16) - 0x1A];
            ImgFlag2 = ImgFlag2s[Convert.ToInt32(imgbox1.Text, 16) - 0x1A];
            ImgFlag3 = 0x100;

            //MessageBox.Show(ImgType.ToString() + "-" + ImgFlag1.ToString() + "-" + ImgFlag2.ToString() + "-" + ImgFlag3.ToString() + "-");
            testflag = (((ImgFlag2 << 1) + 7) >> 3) << 9;

            //MessageBox.Show(testflag.ToString());



            flip4 = BitConverter.GetBytes(Convert.ToUInt32((((ImgType << 0x15) | 0xF5100000) | ((((ImgFlag2 << 1) + 7) >> 3) << 9)) | ImgFlag3));
            Array.Reverse(flip4);
            //MessageBox.Show("F5 String--" + BitConverter.ToString(flip4).Replace("-", ""));
            f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");

            
            flip4 = BitConverter.GetBytes(Convert.ToInt32((((((Param[1] & 0xF) << 18) | (((Param[1] & 0xF0) >> 4) << 14)) | ((Param[0] & 0xF) << 8)) | (((Param[0] & 0xF0) >> 4) << 4))));
            Array.Reverse(flip4);
            MessageBox.Show("Parameter String--"+BitConverter.ToString(flip4).Replace("-", ""));
            f5out.Text = f5out.Text + "--";
            //MessageBox.Show("F2000000");
            
            flip4 = BitConverter.GetBytes(Convert.ToInt32((((ImgFlag2 - 1) << 0xE) | ((ImgFlag1 - 1) << 2))));
            Array.Reverse(flip4);
            //MessageBox.Show("Closing String--" + BitConverter.ToString(flip4).Replace("-", ""));
            f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int tempint = new int();
            byte tempbyte = new byte();

            tempint = Convert.ToInt32(parabox.Text, 16);
//            MessageBox.Show(tempint.ToString());

            
            opint = tempint >> (Convert.ToInt32(shiftbox.Text));

            tempbyte = Convert.ToByte((opint & 0xF0) >> 4 | (opint & 0xF) << 4);
            MessageBox.Show(tempbyte.ToString("X"));
            //opint = opint & 0xFF000000;
            tempint = Convert.ToInt32(opint);
            //MessageBox.Show(value32.ToString("x"));
            byte32 = BitConverter.GetBytes(tempint);
            Array.Reverse(byte32);
            
            
            MessageBox.Show(BitConverter.ToString(byte32).Replace("-", "") + "  " + opint.ToString("X") + Environment.NewLine + Convert.ToString(Convert.ToInt32(BitConverter.ToString(byte32).Replace("-", ""), 16), 2).PadLeft(32, '0'));

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Int32 ImgSize = 0, ImgType = 0, ImgFlag1 = 0, ImgFlag2 = 0, ImgFlag3 = 0, testflag = 0;
            Int32[] ImgTypes = { 0, 0, 0, 3, 3, 3, 0 }; //0=RGBA, 3=IA
            Int32[] ImgFlag1s = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; //texture sizes
            Int32[] ImgFlag2s = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 };
            byte[] flip4 = new byte[4];
            byte[] Param = new byte[3];
            
            Param[2] = Convert.ToByte(imgbox2.Text.Substring(0, 2), 16);
            Param[1] = Convert.ToByte(imgbox2.Text.Substring(2, 2), 16);
            Param[0] = Convert.ToByte(imgbox2.Text.Substring(4, 2), 16);

            Array.Reverse(Param);


            ImgType = ImgTypes[Convert.ToInt32(imgbox1.Text, 16) - 0x20];
            ImgFlag1 = ImgFlag1s[Convert.ToInt32(imgbox1.Text, 16) - 0x20];
            ImgFlag2 = ImgFlag2s[Convert.ToInt32(imgbox1.Text, 16) - 0x20];
            ImgFlag3 = 0x100;

            MessageBox.Show(ImgType.ToString() + "-" + ImgFlag1.ToString() + "-" + ImgFlag2.ToString() + "-" + ImgFlag3.ToString() + "-");


            flip4 = BitConverter.GetBytes(Convert.ToUInt32((ImgType | 0xFD000000) | 0x100000));
            Array.Reverse(flip4);
            //MessageBox.Show(BitConverter.ToString(flip4).Replace("-", ""));
            f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");

            flip4 = BitConverter.GetBytes(Convert.ToUInt32((Param[0] << 0xB) + 0x05000000));
            Array.Reverse(flip4);
            //MessageBox.Show(BitConverter.ToString(flip4).Replace("-", ""));
            //f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");

            flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xE8000000));
            Array.Reverse(flip4);
            //MessageBox.Show(BitConverter.ToString(flip4).Replace("-", ""));
            //f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");

            flip4 = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(flip4);
            //MessageBox.Show(BitConverter.ToString(flip4).Replace("-", ""));
            //f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");

            flip4 = BitConverter.GetBytes(Convert.ToUInt32((((ImgType << 0x15) | 0xF5000000) | 0x100000) | (Param[2] & 0xF)));
            Array.Reverse(flip4);
            //MessageBox.Show(BitConverter.ToString(flip4).Replace("-", ""));
            //f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");

            flip4 = BitConverter.GetBytes(Convert.ToUInt32((((Param[2] & 0xF0) >> 4) << 0x18)));
            Array.Reverse(flip4);
            //MessageBox.Show(BitConverter.ToString(flip4).Replace("-", ""));
            //f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");

            flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xE6000000));
            Array.Reverse(flip4);
            //MessageBox.Show(BitConverter.ToString(flip4).Replace("-", ""));
            //f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");

            flip4 = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(flip4);
            //MessageBox.Show(BitConverter.ToString(flip4).Replace("-", ""));
            //f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");

            ImgSize = (ImgFlag2 * ImgFlag1) - 1;
            if (ImgSize > 0x7FF) ImgSize = 0x7FF;

            Int32 Unknown2x = new Int32();

            Unknown2x = 1;
            Unknown2x = (ImgFlag2 << 1) >> 3; //purpose of this value is unknown

            flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xF3000000));
            Array.Reverse(flip4);
            //MessageBox.Show(BitConverter.ToString(flip4).Replace("-", ""));
            //f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");
            flip4 = BitConverter.GetBytes(Convert.ToUInt32((((Unknown2x + 0x7FF) / Unknown2x) | (((Param[2] & 0xF0) >> 4) << 0x18)) | (ImgSize << 0xC)));
            Array.Reverse(flip4);
            MessageBox.Show(BitConverter.ToString(flip4).Replace("-", ""));
            f5out.Text = f5out.Text + BitConverter.ToString(flip4).Replace("-", "");



        }

        private void Button5_Click(object sender, EventArgs e)
        {

            byte[] value = BitConverter.GetBytes(Convert.ToInt64(pitbox.Text, 16));

            byte tempByte = value[0];

            int Line = BitConverter.ToUInt16(value, 1);
            Line = Line >> 1 & 0x01FF;
            int TMem = BitConverter.ToUInt16(value, 2);
            TMem = TMem & 0x01FF;

            int Tile = value[4] & 0x0F;


            int Palette = (value[5] >> 4) & 0x0F;


            ushort temp = BitConverter.ToUInt16(value, 5);
            tempByte = Convert.ToByte((temp >> 10) & 0x03);
            //int CMTMirror = (TextureMirrorSetting)(tempByte & 0x01);
            //int CMTWrap = (TextureWrapSetting)(tempByte & 0x02);
            byte MaskT = Convert.ToByte((temp >> 6) & 0x0F);
            byte ShiftT = Convert.ToByte((temp >> 2) & 0x0F);
            tempByte = Convert.ToByte(temp & 0x03);
            //CMSMirror = (TextureMirrorSetting)(tempByte & 0x01);
            //CMSWrap = (TextureWrapSetting)(tempByte & 0x02);
            tempByte = value[7];
            byte MaskS = Convert.ToByte((tempByte >> 4) & 0x0F);
            byte ShiftS = Convert.ToByte(tempByte & 0x0F);

            byte ShiftU = Convert.ToByte(Tile >> 10 & 0xF);
            byte ShiftV = Convert.ToByte(Tile >> 10 & 0xF);
            MessageBox.Show(ShiftS.ToString()+"-"+ShiftT.ToString() + "-" + ShiftU.ToString() + "-" + ShiftV.ToString());
        }


        protected string filename;
        protected AssimpSharp.Scene fbx;
        



        private void Button7_Click(object sender, EventArgs e)
        {
            TM64_Geometry Tarmac = new TM64_Geometry();
            int x1 = Convert.ToInt32(n1.Text);
            int x2 = Convert.ToInt32(n2.Text);
            MessageBox.Show(Tarmac.GetMax(x1, x2).ToString());
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            TM64_Geometry Tarmac = new TM64_Geometry();
            int x1 = Convert.ToInt32(n1.Text);
            int x2 = Convert.ToInt32(n2.Text);
            MessageBox.Show(Tarmac.GetMin(x1, x2).ToString());
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileOpen = new OpenFileDialog();
            SaveFileDialog fileSave = new SaveFileDialog();


            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                if (fileSave.ShowDialog() == DialogResult.OK)
                {
                    MemoryStream bs = new MemoryStream();
                    BinaryReader br = new BinaryReader(bs);
                    BinaryWriter bw = new BinaryWriter(bs);

                    byte[] inputSegment4 = File.ReadAllBytes(fileOpen.FileName);

                    for (int currentPosition = 0; currentPosition < inputSegment4.Length; )
                    {
                        bw.Write(inputSegment4, currentPosition, 6);
                        currentPosition = currentPosition + 6;
                        bw.Write(Convert.ToUInt16(0));
                        bw.Write(inputSegment4, currentPosition, 8);
                        currentPosition = currentPosition + 8;
                    }

                    File.WriteAllBytes(fileSave.FileName, bs.ToArray());

                }
            }
        }

        private void f3dBtn_Click(object sender, EventArgs e)
        {
            int targetOffset = new int();
            targetOffset = int.Parse(f3dBox.Text, NumberStyles.HexNumber);


            int vaddress = new int();


            byte commandbyte = new byte();
            byte[] byte29 = new byte[2];

            string output = "";
            byte[] voffset = new byte[2];

            //DEBUG
            //targetOffset = 0;
            //DEBUG

            int texClass = 0;
            int current_offset = 0;
            TM64_Geometry TarmacGeometry = new TM64_Geometry();

            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                if (fileSave.ShowDialog() == DialogResult.OK)
                {
                    string savePath = fileSave.FileName;
                    byte[] segment = File.ReadAllBytes(fileOpen.FileName);

                    MemoryStream seg7m = new MemoryStream(segment);
                    MemoryStream seg4m = new MemoryStream(segment);
                    BinaryReader seg7r = new BinaryReader(seg7m);
                    BinaryReader seg4r = new BinaryReader(seg4m);
                    MemoryStream seg6m = new MemoryStream(segment);
                    BinaryReader seg6r = new BinaryReader(seg6m);


                    for (int currentVert = 0; currentVert < 32; currentVert++)
                    {
                        vertCache[currentVert] = new TM64_Geometry.Vertex();
                        vertCache[currentVert].position = new TM64_Geometry.Position();
                        vertCache[currentVert].color = new TM64_Geometry.OK64Color();
                    }

                    for (bool breakBool = false; breakBool == false;)
                    {
                        seg6r.BaseStream.Seek(targetOffset, SeekOrigin.Begin);
                        byte[] rsp_add = seg6r.ReadBytes(4);
                        Array.Reverse(rsp_add);
                        int Value = BitConverter.ToInt32(rsp_add, 0);
                        if (Value != 0)
                        {
                            String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');
                            int segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                            //MessageBox.Show(segid.ToString() + " " + seg6r.BaseStream.Position.ToString());
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
                                    //subBreak = true;
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
                                    output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, commandbyte, segment, segment, vaddress, current_offset + 1, vertCache, texClass);
                                    vaddress = Convert.ToInt32(output);
                                    output = "";
                                }
                                if (commandbyte == 0xB1)
                                {
                                    output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, commandbyte, segment, segment, vaddress, current_offset + 1, vertCache, texClass);
                                }
                                if (commandbyte == 0xBF)
                                {
                                    output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, commandbyte, segment, segment, vaddress, current_offset + 1, vertCache, texClass);
                                }
                                if (commandbyte == 0x06)
                                {
                                    output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, commandbyte, segment, segment, vaddress, current_offset + 1, vertCache, texClass);

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
                                                output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, segment, segment, vaddress, caddress + 1, vertCache, texClass);
                                                vaddress = Convert.ToInt32(output);
                                                output = "";
                                            }
                                            if (recursivecommand == 0xE4)
                                            {
                                                caddress += 4;
                                            }
                                            if (recursivecommand == 0xB1)
                                            {
                                                output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, segment, segment, vaddress, caddress + 1, vertCache, texClass);
                                            }
                                            if (recursivecommand == 0xBF)
                                            {
                                                output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, segment, segment, vaddress, caddress + 1, vertCache, texClass);
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
                                                output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, segment, segment, vaddress, caddress + 1, vertCache, texClass);
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
                                                output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, segment, segment, vaddress, caddress + 1, vertCache, texClass);
                                            }
                                            if (recursivecommand == 0xBF)
                                            {
                                                output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, segment, segment, vaddress, caddress + 1, vertCache, texClass);
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
                        targetOffset = targetOffset + 4;
                    }

                    MessageBox.Show("Finished");
                }
            }
        }




        private void button12_Click(object sender, EventArgs e)
        {
            TM64 Tarmac = new TM64();

            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                byte[] vertData = File.ReadAllBytes(openFile.FileName);

                byte[] outData = Tarmac.Decompress_seg7(vertData);
                

                SaveFileDialog saveFile = new SaveFileDialog();
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFile.FileName,outData);
                }

            }
        }





        
       

        private void button14_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileOpen = new OpenFileDialog();

            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                byte[] segment4 = File.ReadAllBytes(fileOpen.FileName);



                List<byte> insert = new List<byte> { 0x00, 0x00 };
                int vertcount = segment4.Length / 14;

                List<byte> vertList = segment4.ToList();
                for (int i = 0; i < vertcount; i++)
                {
                    vertList.InsertRange((i + 1) * 10 + (i * 2) + (i * 4), insert);
                }
                byte[] seg4 = vertList.ToArray();

                SaveFileDialog fileSave = new SaveFileDialog();
                if (fileSave.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(fileSave.FileName, seg4);
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderOpen = new FolderBrowserDialog();
            if (folderOpen.ShowDialog() == DialogResult.OK)
            {

                foreach (string file in Directory.GetFiles(folderOpen.SelectedPath, "*.c"))
                {
                    string[] fileData = File.ReadAllLines(file);
                    
                    List<byte> byteList = new List<byte>();

                    MemoryStream memoryStream = new MemoryStream();
                    BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

                    string outPath = Path.Combine(Path.GetDirectoryName(file), "Output");

                    Directory.CreateDirectory(outPath);
                    outPath = Path.Combine(outPath, Path.GetFileName(file));

                    //past the intro start at 1

                    int currentLine = 0;
                    int dataType = 0;


                    for (; currentLine < fileData.Count();)
                    {
                        bool newObject = false;
                        bool dataStart = false;

                        do
                        {
                            string firstLine = fileData[currentLine];
                            currentLine++;
                            string[] splitFirst = firstLine.Split(' ');
                            if (splitFirst[1] == "short")
                            {
                                dataType = 1;
                            }
                        } while (dataStart == false);
                        

                        do
                        {
                            string thisLine = fileData[currentLine];
                            currentLine++;
                            if (thisLine != "};")
                            {
                                if (thisLine != "")
                                {
                                    string[] itemArray = thisLine.Split(',');

                                    foreach (var currentItem in itemArray)
                                    {
                                        if (currentItem.Length > 0)
                                        {
                                            char tab = '\u0009';
                                            string editedItem = currentItem.Replace("0x", "");
                                            editedItem = editedItem.Replace(tab.ToString(), "");
                                            editedItem = editedItem.Replace("\"", "");
                                            if (dataType == 1)
                                            {
                                                binaryWriter.Write(Convert.ToInt16(editedItem, 16));
                                            }
                                            else
                                            {
                                                byteList.Add(Convert.ToByte(editedItem, 16));
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                newObject = true;
                            }
                        } while (newObject == false);
                    }



                    


                    


                    int addressAlign = 4 - (byteList.Count % 4);
                    if (addressAlign == 4)
                        addressAlign = 0;
                    for (int align = 0; align < addressAlign; align++)
                    {
                        byteList.Add(0xFF);
                    }
                    byte[] byteArray = new byte[0];
                    if (dataType == 0)
                    {
                        byteArray = byteList.ToArray();
                    }
                    else
                    {
                        byteArray = memoryStream.ToArray();
                    }

                    TM64 Tarmac = new TM64();
                    File.WriteAllBytes(outPath + ".data.bin", byteArray);

                    try
                    {
                        byte[] textureData = Tarmac.DecompressMIO0(byteArray);


                        int width, height = 0;
                        byte[] voidBytes = new byte[0];

                        if (textureData.Length == 0x800)
                        {
                            width = 32;
                            height = 32;

                            Bitmap exportBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                            Graphics graphicsBitmap = Graphics.FromImage(exportBitmap);
                            N64Graphics.RenderTexture(graphicsBitmap, textureData, voidBytes, 0, width, height, 1, N64Codec.RGBA16, N64IMode.AlphaCopyIntensity);

                            string texturePath = (outPath + ".png");

                            exportBitmap.Save(texturePath, ImageFormat.Png);


                        }
                        else
                        {
                            width = 32;
                            height = 64;

                            Bitmap exportBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                            Graphics graphicsBitmap = Graphics.FromImage(exportBitmap);
                            N64Graphics.RenderTexture(graphicsBitmap, textureData, voidBytes, 0, width, height, 1, N64Codec.RGBA16, N64IMode.AlphaCopyIntensity);

                            string texturePath = (outPath + ".32x64.png");

                            exportBitmap.Save(texturePath, ImageFormat.Png);

                            width = 64;
                            height = 32;

                            exportBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                            graphicsBitmap = Graphics.FromImage(exportBitmap);
                            N64Graphics.RenderTexture(graphicsBitmap, textureData, voidBytes, 0, width, height, 1, N64Codec.RGBA16, N64IMode.AlphaCopyIntensity);

                            texturePath = (outPath + ".64x32.png");

                            exportBitmap.Save(texturePath, ImageFormat.Png);
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                    


                }
                

            }    
        }


        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

            int currentIndex = 0;

            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                string[] vertText = File.ReadAllLines(fileOpen.FileName);
                if (fileOpen.ShowDialog() == DialogResult.OK)
                {
                    if (fileSave.ShowDialog() == DialogResult.OK)
                    {

                        string savePath = fileSave.FileName;
                        string[] faceText = File.ReadAllLines(fileOpen.FileName);



                        TM64_Geometry.Vertex[] vertData = new TM64_Geometry.Vertex[vertText.Length];

                        for (int currentVert = 0; currentVert < vertData.Length; currentVert++)
                        {
                            string currentLine = vertText[currentVert];
                            string[] splitString = currentLine.Split(',');
                            vertData[currentVert] = new TM64_Geometry.Vertex();
                            vertData[currentVert].position = new TM64_Geometry.Position();
                            vertData[currentVert].position.x = Convert.ToInt16(splitString[0]);
                            vertData[currentVert].position.y = Convert.ToInt16(splitString[1]);
                            vertData[currentVert].position.z = Convert.ToInt16(splitString[2]);

                            vertData[currentVert].position.s = Convert.ToInt16(splitString[4]);
                            vertData[currentVert].position.t = Convert.ToInt16(splitString[5]);
                            /*
                            vertData[currentVert].color = new TM64_Geometry.Color();
                            vertData[currentVert].color.r = Convert.ToByte(splitString[6]);
                            vertData[currentVert].color.g = Convert.ToByte(splitString[7]);
                            vertData[currentVert].color.b = Convert.ToByte(splitString[8]);
                            vertData[currentVert].color.a = Convert.ToByte(splitString[9]);
                            */

                        }
                        
                        for (int currentLine = 0; currentLine < faceText.Length;)
                        {
                            string thisLine = faceText[currentLine];
                            currentLine++;
                            string[] splitString = thisLine.Split(' ');
                            if (splitString[0] == "static")
                            {
                                System.IO.File.AppendAllText(savePath, "NEWOBJECT" + Environment.NewLine);
                                System.IO.File.AppendAllText(savePath, "NEWOBJECT" + Environment.NewLine);
                                System.IO.File.AppendAllText(savePath, splitString[1] + Environment.NewLine);

                                bool breakBool = false;
                                do
                                {
                                    string childLine = faceText[currentLine];
                                    currentLine++;
                                    string[] childSplit = childLine.Split('(');
                                    if (childSplit[0] == "gsSPVertex")
                                    {
                                        string[] functionSplit = childSplit[1].Split('[');
                                        functionSplit = functionSplit[1].Split(']');
                                        currentIndex = Convert.ToInt32(functionSplit[0]);
                                    }
                                    else if (childSplit[0] == "gsSP1Triangle")
                                    {
                                        string[] functionSplit = childSplit[1].Split(',');
                                        int indexA = Convert.ToInt32(functionSplit[0]);
                                        int indexB = Convert.ToInt32(functionSplit[1]);
                                        int indexC = Convert.ToInt32(functionSplit[2]);
                                        System.IO.File.AppendAllText(savePath, vertData[indexA+currentIndex].position.x.ToString() + ",");
                                        System.IO.File.AppendAllText(savePath, vertData[indexA + currentIndex].position.z.ToString() + ",");
                                        System.IO.File.AppendAllText(savePath, vertData[indexA + currentIndex].position.y.ToString() + ";");
                                        System.IO.File.AppendAllText(savePath, vertData[indexB + currentIndex].position.x.ToString() + ",");
                                        System.IO.File.AppendAllText(savePath, vertData[indexB + currentIndex].position.z.ToString() + ",");
                                        System.IO.File.AppendAllText(savePath, vertData[indexB + currentIndex].position.y.ToString() + ";");
                                        System.IO.File.AppendAllText(savePath, vertData[indexC + currentIndex].position.x.ToString() + ",");
                                        System.IO.File.AppendAllText(savePath, vertData[indexC + currentIndex].position.z.ToString() + ",");
                                        System.IO.File.AppendAllText(savePath, vertData[indexC + currentIndex].position.y.ToString() + ";");
                                        System.IO.File.AppendAllText(savePath, Environment.NewLine);

                                    }
                                    else if (childSplit[0] == "gsSPEndDisplayList")
                                    {
                                        breakBool = true;
                                        System.IO.File.AppendAllText(savePath, "ENDOBJECT" + Environment.NewLine);
                                        System.IO.File.AppendAllText(savePath, splitString[1] + Environment.NewLine);
                                        System.IO.File.AppendAllText(savePath, Environment.NewLine);
                                    }


                                } while (breakBool == false);

                            }


                        }
                        


                    }
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            int segment = 13;
            byte[] printByte = BitConverter.GetBytes(segment << 24);
            Array.Reverse(printByte);
            MessageBox.Show(printByte[0].ToString("X")+ printByte[1].ToString("X") + printByte[2].ToString("X") + printByte[3].ToString("X"));
        }

        private void button18_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vertSave = new FolderBrowserDialog();
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader br = new BinaryReader(memoryStream);
            OpenFileDialog openFile = new OpenFileDialog();
            for (int currentVert = 0; currentVert < 32; currentVert++)
            {
                vertCache[currentVert] = new TM64_Geometry.Vertex();
                vertCache[currentVert].position = new TM64_Geometry.Position();
                vertCache[currentVert].color = new TM64_Geometry.OK64Color();
            }
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFile.FileName;
                byte[] fileData = File.ReadAllBytes(filePath);

                memoryStream.Write(fileData,0,fileData.Length);

                if (vertSave.ShowDialog() == DialogResult.OK)
                {
                    br.BaseStream.Seek(0x122390, SeekOrigin.Begin);
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
                    int currentOffset = 0;
                    TM64 Tarmac = new TM64();
                    TM64_Geometry TarmacGeometry = new TM64_Geometry();
                    for (int cID = 0; cID < 16; cID++)
                    {


                        string savePath = Path.Combine(vertSave.SelectedPath,items[raceOrder[cID]]+".model.txt");



                        int targetOffset = Convert.ToInt32(seg6_addr[raceOrder[cID]]);
                        byte[] romBytes = File.ReadAllBytes(filePath);

                        byte[] compressedFile = new byte[romBytes.Length - targetOffset];
                        Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);


                        byte[] decompressedverts = Tarmac.DecompressMIO0(compressedFile);
                        byte[] seg6 = decompressedverts.ToArray();

                        uint seg7_addr = (seg7_ptr[raceOrder[cID]] - seg47_buf[raceOrder[cID]]) + seg4_addr[raceOrder[cID]];




                        targetOffset = Convert.ToInt32(seg4_addr[raceOrder[cID]]);
                        romBytes = File.ReadAllBytes(filePath);

                        compressedFile = new byte[romBytes.Length - targetOffset];
                        Array.Copy(romBytes, targetOffset, compressedFile, 0, romBytes.Length - targetOffset);

                        decompressedverts = Tarmac.DecompressMIO0(compressedFile);
                        List<byte> vertList = decompressedverts.ToList();
                        List<byte> insert = new List<byte> { 0x00, 0x00 };
                        int vertcount = (vertList.Count / 14);

                        for (int i = 0; i < vertcount; i++)
                        {
                            vertList.InsertRange((i + 1) * 10 + (i * 2) + (i * 4), insert);
                        }
                        byte[] seg4 = vertList.ToArray();

                        byte[] ROM = File.ReadAllBytes(filePath);
                        byte[] useg7 = Tarmac.Dumpseg7(raceOrder[cID], ROM);
                        byte[] seg7 = Tarmac.Decompress_seg7(useg7);

                        int texClass = 0;

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
                        for (int i = 0; i < 64; i++)
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

                        int current_offset = surfaceoffset[raceOrder[cID]];
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
                                System.IO.File.AppendAllText(savePath, "Section " + sectionID.ToString() + "- Surface " + surfaceID.ToString("X"
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
                                            output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, seg6, seg4, vaddress, caddress + 1, vertCache, texClass);
                                            vaddress = Convert.ToInt32(output);
                                            output = "";
                                        }
                                        if (recursivecommand == 0xE4)
                                        {
                                            caddress += 4;
                                        }
                                        if (recursivecommand == 0xB1)
                                        {
                                            output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, seg6, seg4, vaddress, caddress + 1, vertCache, texClass);
                                        }
                                        if (recursivecommand == 0xBF)
                                        {
                                            output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, seg6, seg4, vaddress, caddress + 1, vertCache, texClass);
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
                                            output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, seg7, seg4, vaddress, caddress + 1, vertCache, texClass);
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
                                            output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, seg7, seg4, vaddress, caddress + 1, vertCache, texClass);
                                        }
                                        if (recursivecommand == 0xBF)
                                        {
                                            output = TarmacGeometry.F3DEX_Model(out vertCache, out texClass, recursivecommand, seg7, seg4, vaddress, caddress + 1, vertCache, texClass);
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




                        

                    }
                }
                MessageBox.Show("Finished");
            }
            

        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                if (fileSave.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(fileSave.FileName, Tarmac.CompressMIO0(File.ReadAllBytes(fileOpen.FileName)));
                }
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderSave = new FolderBrowserDialog();
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                string filePath = fileOpen.FileName;
                if (folderSave.ShowDialog() == DialogResult.OK)
                {
                    string outputDir = folderSave.SelectedPath;

                    int startOffset = Convert.ToInt32(texturedumpbox.Text);
                    int endOffset = Convert.ToInt32(texturedumpend.Text);
                    Tarmac.DumpTexturesOffset(startOffset, endOffset, outputDir, filePath);
                }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Select .FBX File");

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            
            dialog.Title = "FBX File";
            dialog.IsFolderPicker = false;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Get the path of specified file
                string FBXfilePath = dialog.FileName;

                Scene fbx = new Scene();
                AssimpContext importer = new AssimpContext();

                fbx = importer.ImportFile(FBXfilePath, PostProcessPreset.TargetRealTimeMaximumQuality);
                int x = 5;
                x = 5 + 2;
            }

        }

        private void button22_Click(object sender, EventArgs e)
        {
            TM64_Geometry.OK64Texture TextureObject = new TM64_Geometry.OK64Texture();
            TextureObject.segmentPosition = Convert.ToInt32(BitmapAddress.Text,16);
            TextureObject.palettePosition = Convert.ToInt32(PaletteAddress.Text, 16);
            TM64_Geometry TarmacGeometry = new TM64_Geometry();
            byte[] SegmentByte = BitConverter.GetBytes(6);
            SaveFileDialog FileSave = new SaveFileDialog();
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(FileSave.FileName, TarmacGeometry.RGBA(TextureObject, 0, SegmentByte));
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            TM64_Geometry.OK64Texture TextureObject = new TM64_Geometry.OK64Texture();
            TextureObject.textureWidth = 64;
            TextureObject.textureHeight = 64;
            TextureObject.bitSize = 0;
            
            TextureObject.segmentPosition = 0x2000;
            TextureObject.palettePosition = 0x4000;
            TM64_Geometry TarmacGeometry = new TM64_Geometry();

            byte[] SegmentByte = BitConverter.GetBytes(6);
            Array.Reverse(SegmentByte);
            SaveFileDialog FileSave = new SaveFileDialog();
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(FileSave.FileName, TarmacGeometry.CI(TextureObject, 0, SegmentByte));
            }
        }
    }
}
