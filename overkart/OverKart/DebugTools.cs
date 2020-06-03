using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AssimpSharp.FBX;
using System.Text;
using System.Windows.Media.Effects;
using System.Globalization;

namespace OverKart64
{
    public partial class DebugTools : Form




    {

        string outputstring = "";

        int opint = new int();
        byte[] byte16 = new byte[2];
        byte[] byte32 = new byte[4];

        Int16 value16 = new Int16();
        
        Int32 value32 = new Int32();


        OpenFileDialog fileOpen = new OpenFileDialog();
        SaveFileDialog fileSave = new SaveFileDialog();

        public DebugTools()
        {
            InitializeComponent();
        }

        private void Leftshift_Click(object sender, EventArgs e)
        {
            leftshift(input.Text);
        }


        private void Rightshiftbtn_Click(object sender, EventArgs e)
        {
            rightshift(input.Text);
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



        private void leftshift(string inputstring)
        {


            if (inputstring.Length == 8)
            {
                value32 = Convert.ToInt32(inputstring,16);
                
                opint = value32 << (Convert.ToInt32(shiftbox.Text));
                value32 = Convert.ToInt32(opint);
                byte32 = BitConverter.GetBytes(value32);
                Array.Reverse(byte32);
                MessageBox.Show(BitConverter.ToString(byte32).Replace("-", "") + "  " + opint.ToString("X") + Environment.NewLine + Convert.ToString(Convert.ToInt32(BitConverter.ToString(byte32).Replace("-", ""), 16), 2).PadLeft(32, '0'));

            }
            else if (inputstring.Length == 4)
            {
                value16 = Convert.ToInt16(inputstring,16);
                opint = Convert.ToInt32(value16) << (Convert.ToInt32(shiftbox.Text));
                value16 = Convert.ToInt16(opint);
                byte16 = BitConverter.GetBytes(value16);
                Array.Reverse(byte16);
                MessageBox.Show(BitConverter.ToString(byte16).Replace("-", "") + "  " + opint.ToString("X") + Environment.NewLine + Convert.ToString(Convert.ToInt32(BitConverter.ToString(byte16).Replace("-", ""), 16), 2).PadLeft(16, '0'));
            }


        }

        private void rightshift(string inputstring)
        {

            if (inputstring.Length == 8)
            {
                value32 = Convert.ToInt32(inputstring,16);           
                opint = value32 >> (Convert.ToInt32(shiftbox.Text));
                //opint = opint & 0xFF000000;
                value32 = Convert.ToInt32(opint);
                //MessageBox.Show(value32.ToString("x"));
                byte32 = BitConverter.GetBytes(value32);
                Array.Reverse(byte32);
                MessageBox.Show(BitConverter.ToString(byte32).Replace("-", "") + "  " + opint.ToString("X") + Environment.NewLine + Convert.ToString(Convert.ToInt32(BitConverter.ToString(byte32).Replace("-", ""), 16), 2).PadLeft(32, '0'));

            }
            else if (inputstring.Length == 4)
            {
                value16 = Convert.ToInt16(inputstring,16);
                opint = Convert.ToInt32(value16) >> (Convert.ToInt32(shiftbox.Text));
                value16 = Convert.ToInt16(opint);
                byte16 = BitConverter.GetBytes(value16);
                Array.Reverse(byte16);
                MessageBox.Show(BitConverter.ToString(byte16).Replace("-", "") + "  " + opint.ToString("X") + Environment.NewLine + Convert.ToString(Convert.ToInt32(BitConverter.ToString(byte16).Replace("-", ""), 16), 2).PadLeft(16, '0'));
            }
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
            OK64 mk = new OK64();
            
            int targetOffset = 0;
            int.TryParse(offsetbox.Text, out targetOffset);
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                byte[] inputFile = File.ReadAllBytes(fileOpen.FileName);

                byte[] compressedFile = new byte[inputFile.Length - targetOffset];

                Array.Copy(inputFile, targetOffset, compressedFile, 0, inputFile.Length - targetOffset);

                byte[] decompressedFile =mk.decompressMIO0(compressedFile);
                

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
        




        private void Button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileOpen = new OpenFileDialog();
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                var assimpSharpImporter = new AssimpSharp.FBX.FBXImporter();
                fbx = new AssimpSharp.Scene();
                fbx = assimpSharpImporter.ReadFile(fileOpen.FileName);
                var searchNode = fbx.RootNode.FindNode("Course Paths");
                var pathNode = searchNode.Children[0];

                var pathObject = fbx.Meshes[pathNode.Meshes[0]];

                MessageBox.Show(pathObject.Vertices[0].X.ToString());


            }
            

        }

        private void Button7_Click(object sender, EventArgs e)
        {
            OK64 mk = new OK64();
            int x1 = Convert.ToInt32(n1.Text);
            int x2 = Convert.ToInt32(n2.Text);
            MessageBox.Show(mk.GetMax(x1, x2).ToString());
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            OK64 mk = new OK64();
            int x1 = Convert.ToInt32(n1.Text);
            int x2 = Convert.ToInt32(n2.Text);
            MessageBox.Show(mk.GetMin(x1, x2).ToString());
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
        }

    }
}
