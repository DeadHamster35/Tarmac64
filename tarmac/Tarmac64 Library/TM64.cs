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
using Tarmac64_Library;


//custom libraries

using Assimp;  //for handling model data
using Texture64;  //for handling texture data


using Cereal64.Microcodes.F3DEX.DataElements;
using Cereal64.Common.DataElements;
using Cereal64.Common.Rom;
using Cereal64.Common.Utils.Encoding;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Tarmac64_Library
{
    public class TM64
    {

        /// These are various functions for decompressing and handling the segment data for Mario Kart 64.


        public static int newint = 4;




        MemoryStream bs = new MemoryStream();
        BinaryReader br = new BinaryReader(Stream.Null);
        BinaryWriter bw = new BinaryWriter(Stream.Null);

        public static UInt32[] seg6_addr = new UInt32[20];
        public static UInt32[] seg6_end = new UInt32[20];
        public static UInt32[] seg4_addr = new UInt32[20];
        public static UInt32[] seg7_end = new UInt32[20];
        public static UInt32[] seg9_addr = new UInt32[20];
        public static UInt32[] seg9_end = new UInt32[20];
        public static UInt32[] seg47_buf = new UInt32[20];
        public static UInt32[] numVtxs = new UInt32[20];
        public static UInt32[] seg7_ptr = new UInt32[20];
        public static UInt32[] seg7_size = new UInt32[20];
        public static UInt32[] texture_addr = new UInt32[20];
        public static UInt16[] flag = new UInt16[20];
        public static UInt16[] unused = new UInt16[20];

        public static UInt32[] seg7_romptr = new UInt32[20];

        byte[] flip2 = new byte[2];
        byte[] flip4 = new byte[4];

        UInt16 value16 = new UInt16();
        Int16 valuesign16 = new Int16();

        UInt32 value32 = new UInt32();


        /// These classes are used by the underlying functions.



        /// These are used by the Geometry Builder
        /// 



        public class Face
        {
            public vertIndex vertIndex { get; set; }
            public int material { get; set; }
        }

        public class vertIndex
        {
            public int v0 { get; set; }
            public int v1 { get; set; }
            public int v2 { get; set; }

        }


        public class OK64Settings
        {
            public string AppDirectory { get; set; }
            public string CurrentDirectory { get; set; }
            public string ProjectDirectory { get; set; }
            public string JRDirectory { get; set; }
            public bool Valid { get; set; }
        }


        ///

        public class Offset
        {
            public List<int> offset { get; set; }
        }

        public class Pathgroup
        {
            public List<Pathlist> pathlist { get; set; }
        }

        public class Pathlist
        {

            public List<Marker> pathmarker { get; set; }



        }

        public class Marker
        {

            public int xval { get; set; }
            public int yval { get; set; }
            public int zval { get; set; }
            public int flag { get; set; }

        }

        List<Offset> pathOffsets = new List<Offset>();

        int[] pathoffset = { 0x5568, 0x4480, 0x4F90, 0x4578, 0xD780, 0x34A0, 0xADE0, 0xB5B8, 0xA540, 0xEC80, 0x3B80, 0x6AC8, 0x4BF8, 0x1D90, 0x56A0, 0x71F0 };



        public byte[] decompressSMSR(byte[]inputData)
        {
            MemoryStream memoryStream = new MemoryStream(inputData);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            MemoryStream memoryOutput = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryOutput);
            BinaryReader outputReader = new BinaryReader(memoryOutput);


            int srcOffs = 0x10;
            binaryReader.BaseStream.Position = 0x08;

            uint outputSize = binaryReader.ReadUInt32();
            binaryReader.BaseStream.Position = 0x0C;
            uint dataOffs = Convert.ToUInt32(srcOffs + binaryReader.ReadUInt32());

            var outputByte = new byte[outputSize];
            
            var dstOffs = 0;

            var numCtrlBits = 0;
            UInt16 ctrlBits = 0;

            while (dstOffs < outputSize)
            {
                if (numCtrlBits == 0)
                {
                    numCtrlBits = 16;
                    binaryReader.BaseStream.Position = srcOffs;
                    ctrlBits = binaryReader.ReadUInt16();
                    srcOffs += 2;
                }

                if (ctrlBits == 0x8000)
                {
                    outputReader.BaseStream.Position = dataOffs;
                    binaryWriter.BaseStream.Position = dstOffs;
                    binaryWriter.Write(outputReader.ReadByte());
                    dataOffs++;
                    dstOffs++;
                }
                else
                {

                    binaryReader.BaseStream.Position = srcOffs;
                    UInt16 pair = binaryReader.ReadUInt16();
                    srcOffs += 2;

                    UInt16 length = Convert.ToUInt16((pair >> 12) + 3);
                    UInt16 offset = Convert.ToUInt16((pair & 0x0FFF) - 1);
                    uint windowPtr = Convert.ToUInt32((dstOffs - offset) - 2);

                    while (length > 0)
                    {
                        outputReader.BaseStream.Seek(windowPtr, SeekOrigin.Current);
                        binaryWriter.BaseStream.Position = dstOffs;
                        binaryWriter.Write(outputReader.ReadByte());
                        windowPtr++;
                        dstOffs++;
                        length--;
                    }
                }

                ctrlBits <<= 1;
                numCtrlBits--;
            }
            return memoryOutput.ToArray();
            
        }
        List<Offset> objOffsets = new List<Offset>();
        private void loadoffsets()
        {

            objOffsets.Add(new Offset { });

            objOffsets[0].offset = new List<int>();
            objOffsets[0].offset.Add(0x9498);
            objOffsets[0].offset.Add(0x9518);
            objOffsets[0].offset.Add(0x9570);
            objOffsets.Add(new Offset { });
            objOffsets[1].offset = new List<int>();
            objOffsets[1].offset.Add(0x7250);
            objOffsets[1].offset.Add(0x7230);
            objOffsets[1].offset.Add(0x4480);
            objOffsets.Add(new Offset { });
            objOffsets[2].offset = new List<int>();
            objOffsets[2].offset.Add(0x9290);
            objOffsets[2].offset.Add(0x9370);
            objOffsets.Add(new Offset { });
            objOffsets[3].offset = new List<int>();
            objOffsets[3].offset.Add(0xB3D0);
            objOffsets.Add(new Offset { });
            objOffsets[4].offset = new List<int>();
            objOffsets[4].offset.Add(0X180A0);
            objOffsets[4].offset.Add(0x18110);
            objOffsets.Add(new Offset { });
            objOffsets[5].offset = new List<int>();
            objOffsets[5].offset.Add(0x7718);
            objOffsets[5].offset.Add(0x7810);
            objOffsets[5].offset.Add(0x34A0);
            objOffsets.Add(new Offset { });
            objOffsets[6].offset = new List<int>();
            objOffsets[6].offset.Add(0x18E78);
            objOffsets[6].offset.Add(0x187F0);
            objOffsets[6].offset.Add(0xADE0);
            objOffsets.Add(new Offset { });
            objOffsets[7].offset = new List<int>();
            objOffsets[7].offset.Add(0xDA78);
            objOffsets[7].offset.Add(0xDB80);
            objOffsets[7].offset.Add(0xD9F0);
            objOffsets[7].offset.Add(0xB5B8);
            objOffsets.Add(new Offset { });
            objOffsets[8].offset = new List<int>();
            objOffsets[8].offset.Add(0xFE80);
            objOffsets[8].offset.Add(0xFDE8);
            objOffsets.Add(new Offset { });
            objOffsets[9].offset = new List<int>();
            objOffsets[9].offset.Add(0x14330);
            objOffsets[9].offset.Add(0x143E0);
            objOffsets.Add(new Offset { });
            objOffsets[10].offset = new List<int>();
            objOffsets[10].offset.Add(0x22AE0);
            objOffsets.Add(new Offset { });
            objOffsets[11].offset = new List<int>();
            objOffsets[11].offset.Add(0x22F08);
            objOffsets[11].offset.Add(0x22E8);
            objOffsets.Add(new Offset { });
            objOffsets[12].offset = new List<int>();
            objOffsets[12].offset.Add(0x9B80);
            objOffsets[12].offset.Add(0x4BF8);
            objOffsets.Add(new Offset { });
            objOffsets[13].offset = new List<int>();
            objOffsets[13].offset.Add(0x16338);
            objOffsets.Add(new Offset { });
            objOffsets[14].offset = new List<int>();
            objOffsets[14].offset.Add(0xCB40);
            objOffsets.Add(new Offset { });
            objOffsets[15].offset = new List<int>();
            objOffsets[15].offset.Add(0x38);
            objOffsets.Add(new Offset { });
            objOffsets[15].offset = new List<int>();
            objOffsets[15].offset.Add(0X80);
            objOffsets.Add(new Offset { });
            objOffsets[15].offset = new List<int>();
            objOffsets[15].offset.Add(0x28);
            objOffsets.Add(new Offset { });
            objOffsets[15].offset = new List<int>();
            objOffsets[15].offset.Add(0x13EC0);
            objOffsets.Add(new Offset { });
            objOffsets[15].offset = new List<int>();
            objOffsets[15].offset.Add(0x58);


            pathOffsets.Add(new Offset { });

            pathOffsets[0].offset = new List<int>();
            pathOffsets[0].offset.Add(0x5568);
            pathOffsets.Add(new Offset { });
            pathOffsets[1].offset = new List<int>();
            pathOffsets[1].offset.Add(0x4480);
            pathOffsets.Add(new Offset { });
            pathOffsets[2].offset = new List<int>();
            pathOffsets[2].offset.Add(0x4F90);
            pathOffsets.Add(new Offset { });
            pathOffsets[3].offset = new List<int>();
            pathOffsets[3].offset.Add(0x4578);
            pathOffsets.Add(new Offset { });
            pathOffsets[4].offset = new List<int>();
            pathOffsets[4].offset.Add(0xD780);
            pathOffsets[4].offset.Add(0xD9C8);
            pathOffsets[4].offset.Add(0xDC18);
            pathOffsets[4].offset.Add(0xDEA8);
            pathOffsets.Add(new Offset { });
            pathOffsets[5].offset = new List<int>();
            pathOffsets[5].offset.Add(0x34A0);
            pathOffsets.Add(new Offset { });
            pathOffsets[6].offset = new List<int>();
            pathOffsets[6].offset.Add(0xADE0);
            pathOffsets.Add(new Offset { });
            pathOffsets[7].offset = new List<int>();
            pathOffsets[7].offset.Add(0xB5B8);
            pathOffsets.Add(new Offset { });
            pathOffsets[8].offset = new List<int>();
            pathOffsets[8].offset.Add(0xA540);
            pathOffsets.Add(new Offset { });
            pathOffsets[9].offset = new List<int>();
            pathOffsets[9].offset.Add(0xEC80);
            pathOffsets.Add(new Offset { });
            pathOffsets[10].offset = new List<int>();
            pathOffsets[10].offset.Add(0x3B80);
            pathOffsets.Add(new Offset { });
            pathOffsets[11].offset = new List<int>();
            pathOffsets[11].offset.Add(0x6AC8);
            pathOffsets.Add(new Offset { });
            pathOffsets[12].offset = new List<int>();
            pathOffsets[12].offset.Add(0x4BF8);
            pathOffsets.Add(new Offset { });
            pathOffsets[13].offset = new List<int>();
            pathOffsets[13].offset.Add(0x1D90);
            pathOffsets.Add(new Offset { });
            pathOffsets[14].offset = new List<int>();
            pathOffsets[14].offset.Add(0x56A0);
            pathOffsets.Add(new Offset { });
            pathOffsets[15].offset = new List<int>();
            pathOffsets[15].offset.Add(0x71F0);


        }

        ///
        ///
        ///
        ///End of classes

        ///
        ///
        ///

        public OK64Settings LoadSettings(bool Forced = false)
        {
            OK64Settings OkSettings = new OK64Settings();
            
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string SettingsPath = Path.Combine(AppData, "Tarmac.OK64Settings");
            string[] settings = new string[0];
            OkSettings.AppDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            bool corrupt = Forced;
            if (!File.Exists(SettingsPath))
            {
                corrupt = true;
            }
            else
            {
                settings = File.ReadAllLines(SettingsPath);
                if (settings.Length == 2)
                {
                    OkSettings.ProjectDirectory = settings[0];
                    OkSettings.JRDirectory = settings[1];
                    if (OkSettings.ProjectDirectory == null | OkSettings.JRDirectory == null)
                    {
                        corrupt = true;
                    }
                }
                else
                {
                    corrupt = true;
                }
            }
            if (corrupt)
            {
                MessageBox.Show("Error Loading Settings. Please select your Project Folder");
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = "C:\\";
                dialog.IsFolderPicker = true;
                dialog.Title = "Select Project Folder";
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    OkSettings.ProjectDirectory = dialog.FileName;
                }
                MessageBox.Show("Please select your TarmacJR Chunk Folder");
                dialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                dialog.IsFolderPicker = true;
                dialog.Title = "Select TarmacJR Chunk Folder";
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    OkSettings.JRDirectory = dialog.FileName;
                }

                File.AppendAllText(SettingsPath, OkSettings.ProjectDirectory + Environment.NewLine);
                File.AppendAllText(SettingsPath, OkSettings.JRDirectory + Environment.NewLine);
            }
            return OkSettings;
        }

        

        public byte[] CompileSegment(byte[] seg4, byte[] seg6, byte[] seg7, byte[] seg9, byte[] fileData, int cID)
        {


            ///This takes precompiled segments and inserts them into the ROM file. It also updates the course header table to reflect
            /// the new data sizes. This allows for proper loading of the course so long as the segments are properly setup. All segment
            /// data should be precompressed where applicable, this assumes that segment 4 and segment 6 are MIO0 compressed and that
            /// Segment 7 has had it's special compression ran. Segment 9 has no compression. fileData is the ROM file as a byte array, and CID
            /// is the ID of the course we're looking to replace based on it's location in the course header table. 


            /// This writes all segments to the end of the file for simplicity. If data was larger than original (which it almost always will be for custom courses)
            /// then it cannot fit in the existing space without overwriting other course data. 

            TM64_Geometry mk = new TM64_Geometry();
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            binaryWriter.Write(fileData, 0, fileData.Length);
            binaryWriter.BaseStream.Position = 0;

            UInt32 seg6start = 0;
            UInt32 seg6end = 0;
            UInt32 seg4start = 0;
            UInt32 seg7end = 0;
            UInt32 seg9start = 0;
            UInt32 seg9end = 0;
            UInt32 seg7start = 0;
            UInt32 seg7rsp = 0;


            int addressAlign = 0;



            binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            byte[] compseg6 = CompressMIO0(seg6);
            seg6start = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(compseg6, 0, compseg6.Length);
            seg6end = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            ///

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            seg9start = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(seg9, 0, seg9.Length);
            seg9end = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            ///

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            byte[] compseg4 = CompressMIO0(seg4);
            seg4start = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(compseg4, 0, compseg4.Length);
            ///


            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            byte[] compseg7 = compress_seg7(seg7);
            seg7start = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(compseg7, 0, compseg7.Length);
            seg7end = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            seg7rsp = Convert.ToUInt32(0x0F000000 | (seg7start - seg4start));


            ///

            byte[] flip = new byte[4];

            flip = BitConverter.GetBytes(seg6start);
            Array.Reverse(flip);
            seg6start = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(seg6end);
            Array.Reverse(flip);
            seg6end = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(seg4start);
            Array.Reverse(flip);
            seg4start = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(seg7end);
            Array.Reverse(flip);
            seg7end = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(seg9start);
            Array.Reverse(flip);
            seg9start = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(seg9end);
            Array.Reverse(flip);
            seg9end = BitConverter.ToUInt32(flip, 0);




            flip = BitConverter.GetBytes(seg7rsp);
            Array.Reverse(flip);
            seg7rsp = BitConverter.ToUInt32(flip, 0);


            ///calculate # verts

            UInt32 vertcount = Convert.ToUInt32(seg4.Length / 14);

            flip = BitConverter.GetBytes(vertcount);
            Array.Reverse(flip);
            vertcount = BitConverter.ToUInt32(flip, 0);
            ///MessageBox.Show(vertbyte.Count.ToString() + "--" + vertcount.ToString());


            ///seg7 size

            UInt32 seg7size = Convert.ToUInt32(seg7.Length);


            ///MessageBox.Show(seg7size.ToString());

            flip = BitConverter.GetBytes(seg7size);
            Array.Reverse(flip);
            seg7size = BitConverter.ToUInt32(flip, 0);

            ///MessageBox.Show(seg7size.ToString());


            binaryWriter.BaseStream.Seek(0x122390 + (cID * 48), SeekOrigin.Begin);

            binaryWriter.Write(seg6start);
            binaryWriter.Write(seg6end);
            binaryWriter.Write(seg4start);
            binaryWriter.Write(seg7end);
            binaryWriter.Write(seg9start);
            binaryWriter.Write(seg9end);

            binaryWriter.BaseStream.Seek(4, SeekOrigin.Current);



            binaryWriter.Write(vertcount);


            binaryWriter.Write(seg7rsp);



            binaryWriter.Write(seg7size);

            byte[] newROM = memoryStream.ToArray();
            return newROM;

        }
        public string[] DumpVerts(byte[] vertBytes)
        {


            bool vertEnd = true;

            int vertcount = vertBytes.Length / 14;

            string[] output = new string[vertcount];

            int xcor = new int();
            int ycor = new int();
            int zcor = new int();
            int scor = new int();
            int tcor = new int();


            MemoryStream ds = new MemoryStream(vertBytes);
            BinaryReader dr = new BinaryReader(ds);
            {
                dr.BaseStream.Position = 0;
                for (int i = 0; vertEnd; i++)
                {
                    byte[] flip2 = dr.ReadBytes(2);
                    Array.Reverse(flip2);
                    xcor = BitConverter.ToInt16(flip2, 0); //x   <-- this really is the X axis. No tricks.

                    flip2 = dr.ReadBytes(2);
                    Array.Reverse(flip2);
                    zcor = BitConverter.ToInt16(flip2, 0); //z    <-- this is actually the Y axis, but the game (like early 3D) treats the Y axis as height

                    flip2 = dr.ReadBytes(2);
                    Array.Reverse(flip2);
                    ycor = BitConverter.ToInt16(flip2, 0); //y    <-- this is actually the Z axis, but the game (like early 3D) treats the Z axis as depth

                    flip2 = dr.ReadBytes(2);
                    Array.Reverse(flip2);
                    scor = BitConverter.ToInt16(flip2, 0); //S

                    flip2 = dr.ReadBytes(2);
                    Array.Reverse(flip2);
                    tcor = BitConverter.ToInt16(flip2, 0); //T

                    byte rcol = dr.ReadByte();
                    byte gcol = dr.ReadByte();
                    byte bcol = dr.ReadByte();
                    byte acol = dr.ReadByte();

                    output[i] = "[" + xcor.ToString() + "," + zcor.ToString() + "," + ycor.ToString() + "]";

                    if (dr.BaseStream.Position >= dr.BaseStream.Length)
                    {
                        vertEnd = false;
                    }
                }

                return output;
            }

        }
        public void DumpTextures(int cID, string outputDir, string filePath)
        {



            TM64_Course TarmacCourse = new TM64_Course();
            int fileName = 0;
            byte[] fileData = File.ReadAllBytes(filePath);


            TM64_Course.Header[] courseHeaders = TarmacCourse.loadHeader(fileData);

            int s9Start = BitConverter.ToInt32(courseHeaders[cID].s9Start, 0);
            int s9End = BitConverter.ToInt32(courseHeaders[cID].s9End, 0);

            byte[] seg9 = new byte[(s9End - s9Start)];

            Buffer.BlockCopy(fileData, s9Start, seg9, 0, (s9End - s9Start));



            string[] coursename = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut" };


            MemoryStream memoryStream = new MemoryStream(seg9);

            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            for (int i = 0; binaryReader.BaseStream.Position < binaryReader.BaseStream.Length; i++)
            {
                flip4 = binaryReader.ReadBytes(4);
                Array.Reverse(flip4);
                int textureOffset = BitConverter.ToInt32(flip4, 0);

                flip4 = binaryReader.ReadBytes(4);
                Array.Reverse(flip4);
                int compressSize = BitConverter.ToInt32(flip4, 0);



                if (textureOffset != 0)
                {
                    textureOffset = (textureOffset & 0xFFFFFF) + 0x641F70;

                    byte[] textureFile = new byte[compressSize];

                    Array.Copy(fileData, textureOffset, textureFile, 0, compressSize);

                    byte[] decompressedTexture = DecompressMIO0(textureFile);
                    byte[] voidBytes = new byte[0];

                    int width = 0;
                    int height = 0;


                    if (decompressedTexture.Length == 0x800)
                    {
                        width = 32;
                        height = 32;

                        Bitmap exportBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        Graphics graphicsBitmap = Graphics.FromImage(exportBitmap);
                        N64Graphics.RenderTexture(graphicsBitmap, decompressedTexture, voidBytes, 0, width, height, 1, N64Codec.RGBA16, N64IMode.AlphaCopyIntensity);

                        string texturePath = Path.Combine(outputDir, fileName.ToString("X") + ".png");

                        exportBitmap.Save(texturePath, ImageFormat.Png);
                        fileName = fileName + decompressedTexture.Length;

                    }
                    else
                    {
                        width = 32;
                        height = 64;

                        Bitmap exportBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        Graphics graphicsBitmap = Graphics.FromImage(exportBitmap);
                        N64Graphics.RenderTexture(graphicsBitmap, decompressedTexture, voidBytes, 0, width, height, 1, N64Codec.RGBA16, N64IMode.AlphaCopyIntensity);

                        string texturePath = Path.Combine(outputDir, fileName.ToString("X") + ".32x64.png");

                        exportBitmap.Save(texturePath, ImageFormat.Png);

                        width = 64;
                        height = 32;

                        exportBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        graphicsBitmap = Graphics.FromImage(exportBitmap);
                        N64Graphics.RenderTexture(graphicsBitmap, decompressedTexture, voidBytes, 0, width, height, 1, N64Codec.RGBA16, N64IMode.AlphaCopyIntensity);

                        texturePath = Path.Combine(outputDir, fileName.ToString("X") + ".64x32.png");

                        exportBitmap.Save(texturePath, ImageFormat.Png);
                        fileName = fileName + decompressedTexture.Length;
                    }



                }
                else
                {
                    binaryReader.BaseStream.Seek(binaryReader.BaseStream.Length, SeekOrigin.Begin);
                }
                binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + 8;
            }
            MessageBox.Show("Finished");





        }




        public void DumpTexturesOffset(int offset, int endoffset, string outputDir, string filePath)
        {



            TM64_Course TarmacCourse = new TM64_Course();
            int fileName = 0;
            byte[] fileData = File.ReadAllBytes(filePath);


            TM64_Course.Header[] courseHeaders = TarmacCourse.loadHeader(fileData);

            int s9Start = offset;
            int s9End = endoffset;

            byte[] seg9 = new byte[(s9End - s9Start)];

            Buffer.BlockCopy(fileData, s9Start, seg9, 0, (s9End - s9Start));




            MemoryStream memoryStream = new MemoryStream(seg9);

            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            for (int i = 0; binaryReader.BaseStream.Position < binaryReader.BaseStream.Length; i++)
            {
                flip4 = binaryReader.ReadBytes(4);
                Array.Reverse(flip4);
                int textureOffset = BitConverter.ToInt32(flip4, 0);

                flip4 = binaryReader.ReadBytes(4);
                Array.Reverse(flip4);
                int compressSize = BitConverter.ToInt32(flip4, 0);



                if (textureOffset != 0)
                {
                    textureOffset = (textureOffset & 0xFFFFFF) + 0x641F70;

                    byte[] textureFile = new byte[compressSize];

                    Array.Copy(fileData, textureOffset, textureFile, 0, compressSize);

                    byte[] decompressedTexture = DecompressMIO0(textureFile);
                    byte[] voidBytes = new byte[0];

                    int width = 0;
                    int height = 0;


                    if (decompressedTexture.Length == 0x800)
                    {
                        width = 32;
                        height = 32;

                        Bitmap exportBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        Graphics graphicsBitmap = Graphics.FromImage(exportBitmap);
                        N64Graphics.RenderTexture(graphicsBitmap, decompressedTexture, voidBytes, 0, width, height, 1, N64Codec.RGBA16, N64IMode.AlphaCopyIntensity);

                        string texturePath = Path.Combine(outputDir, fileName.ToString("X") + ".png");

                        exportBitmap.Save(texturePath, ImageFormat.Png);
                        fileName = fileName + decompressedTexture.Length;

                    }
                    else
                    {
                        width = 32;
                        height = 64;

                        Bitmap exportBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        Graphics graphicsBitmap = Graphics.FromImage(exportBitmap);
                        N64Graphics.RenderTexture(graphicsBitmap, decompressedTexture, voidBytes, 0, width, height, 1, N64Codec.RGBA16, N64IMode.AlphaCopyIntensity);

                        string texturePath = Path.Combine(outputDir, fileName.ToString("X") + ".32x64.png");

                        exportBitmap.Save(texturePath, ImageFormat.Png);

                        width = 64;
                        height = 32;

                        exportBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        graphicsBitmap = Graphics.FromImage(exportBitmap);
                        N64Graphics.RenderTexture(graphicsBitmap, decompressedTexture, voidBytes, 0, width, height, 1, N64Codec.RGBA16, N64IMode.AlphaCopyIntensity);

                        texturePath = Path.Combine(outputDir, fileName.ToString("X") + ".64x32.png");

                        exportBitmap.Save(texturePath, ImageFormat.Png);
                        fileName = fileName + decompressedTexture.Length;
                    }



                }
                else
                {
                    binaryReader.BaseStream.Seek(binaryReader.BaseStream.Length, SeekOrigin.Begin);
                }
                binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + 8;
            }
            MessageBox.Show("Finished");





        }



        public byte[] Dumpseg4(int cID, byte[] fileData)
        {

            TM64_Course TarmacCourse = new TM64_Course();
            TM64_Course.Header[] courseHeader = TarmacCourse.loadHeader(fileData);

            int s4Start = BitConverter.ToInt32(courseHeader[cID].s47Start, 0);
            int s7Start = BitConverter.ToInt32(courseHeader[cID].S7Pointer, 0);

            byte[] seg4 = new byte[(BitConverter.ToInt32(courseHeader[cID].S7Pointer, 0) - BitConverter.ToInt32(courseHeader[cID].s47Start, 0))];


            Buffer.BlockCopy(fileData, s4Start, seg4, 0, s7Start - s4Start);

            return seg4;
        }
        public byte[] Dumpseg5(int cID, byte[] fileData)
        {

            TM64_Course TarmacCourse = new TM64_Course();
            TM64_Course.Header[] courseHeader = TarmacCourse.loadHeader(fileData);



            List<int> offsets = new List<int>();


            int segment9Start = BitConverter.ToInt32(courseHeader[cID].s9Start, 0);
            int segment9End = BitConverter.ToInt32(courseHeader[cID].s9End, 0);



            byte[] seg9 = new byte[(segment9End - segment9Start)];
            List<byte> segment5 = new List<byte>();

            Buffer.BlockCopy(fileData, segment9Start, seg9, 0, segment9End - segment9Start);

            TM64_Geometry mk = new TM64_Geometry();

            MemoryStream memoryStream = new MemoryStream(seg9);
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            int textureOffset = new int();
            int compressSize = new int();

            binaryReader.BaseStream.Position = (BitConverter.ToInt32(courseHeader[cID].TexturePointer, 0) - 0x09000000);

            for (int i = 0; binaryReader.BaseStream.Position < binaryReader.BaseStream.Length; i++)
            {
                flip4 = binaryReader.ReadBytes(4);
                Array.Reverse(flip4);
                textureOffset = BitConverter.ToInt32(flip4, 0);

                flip4 = binaryReader.ReadBytes(4);
                Array.Reverse(flip4);
                compressSize = BitConverter.ToInt32(flip4, 0);



                if (textureOffset != 0)
                {
                    textureOffset = (textureOffset & 0xFFFFFF) + 0x641F70;

                    byte[] textureFile = new byte[compressSize];

                    Array.Copy(fileData, textureOffset, textureFile, 0, compressSize);

                    byte[] decompressedTexture = DecompressMIO0(textureFile);
                    offsets.Add(segment5.Count);
                    segment5.AddRange(decompressedTexture);
                    binaryReader.BaseStream.Seek(8, SeekOrigin.Current);
                }
                else
                {
                    binaryReader.BaseStream.Seek(binaryReader.BaseStream.Length, SeekOrigin.Begin);
                }
            }

            for (int x = 0; x < offsets.Count; x++)
            {

                byte[] flip2 = BitConverter.GetBytes(x + 1);
                Array.Reverse(flip2);
                segment5.AddRange(flip2);

                flip2 = BitConverter.GetBytes(offsets[x]);
                Array.Reverse(flip2);
                segment5.AddRange(flip2);

            }
            byte[] seg5 = segment5.ToArray();



            return seg5;
        }
        public byte[] Dumpseg6(int cID, byte[] fileData)
        {

            TM64_Course TarmacCourse = new TM64_Course();
            TM64_Course.Header[] courseHeader = TarmacCourse.loadHeader(fileData);

            int s6Start = BitConverter.ToInt32(courseHeader[cID].s6Start, 0);
            int s6End = BitConverter.ToInt32(courseHeader[cID].s6End, 0);

            byte[] seg6 = new byte[s6End - s6Start];


            Buffer.BlockCopy(fileData, s6Start, seg6, 0, s6End - s6Start);

            return seg6;
        }
        public byte[] Dumpseg7(int cID, byte[] fileData)
        {

            TM64_Course TarmacCourse = new TM64_Course();
            TM64_Course.Header[] courseHeaders = TarmacCourse.loadHeader(fileData);

            int s4Start = BitConverter.ToInt32(courseHeaders[cID].s47Start, 0);
            int s7Start = BitConverter.ToInt32(courseHeaders[cID].S7Pointer, 0);
            int s7End = BitConverter.ToInt32(courseHeaders[cID].s47End, 0);



            s7Start = s4Start + s7Start - 0x0F000000;
            byte[] seg7 = new byte[s7End - s7Start];


            Buffer.BlockCopy(fileData, s7Start, seg7, 0, s7End - s7Start);

            return seg7;
        }
        public byte[] Dumpseg9(int cID, byte[] fileData)
        {

            TM64_Course TarmacCourse = new TM64_Course();
            TM64_Course.Header[] courseHeaders = TarmacCourse.loadHeader(fileData);

            int s9Start = BitConverter.ToInt32(courseHeaders[cID].s9Start, 0);
            int s9End = BitConverter.ToInt32(courseHeaders[cID].s9End, 0);


            byte[] seg9 = new byte[s9End - s9Start];


            Buffer.BlockCopy(fileData, s9Start, seg9, 0, s9End - s9Start);

            return seg9;
        }
        public byte[] Dump_ASM(string filePath)
        {

            /// This is specfically designed for Mario Kart 64 USA 1.0 ROM. It should dump to binary the majority of it's ASM commands.
            /// It uses a list provided by MiB to find the ASM sections, there could be plenty of code I'm missing

            byte[] fileData = File.ReadAllBytes(filePath);
            byte[] asm = new byte[1081936];

            byte[] buffer = new byte[1];
            buffer[0] = 0xFF;


            Buffer.BlockCopy(fileData, 4096, asm, 0, 887664);

            for (int i = 0; i < 8; i++)
            {
                Buffer.BlockCopy(buffer, 0, asm, 887664 + i, 1);
            }

            Buffer.BlockCopy(fileData, 1013008, asm, 887672, 174224);

            for (int i = 0; i < 8; i++)
            {
                Buffer.BlockCopy(buffer, 0, asm, 1061896 + i, 1);
            }

            Buffer.BlockCopy(fileData, 1193536, asm, 1061904, 20032);
            return asm;

        }
        public void TranslateASM(string savePath, string filePath)
        {

            /// This is specfically designed for Mario Kart 64 USA 1.0 ROM. It should convert to plaintext the majority of it's ASM commands.            
            /// Also, there are a few ASM commands that MK64 uses that I currently haven't defined yet. 


            byte[] asm = File.ReadAllBytes(filePath);

            MemoryStream asmm = new MemoryStream(asm);
            BinaryReader asmr = new BinaryReader(asmm);
            string output = "";
            byte[] asmbytes = new byte[4];
            int compare = new int();
            byte commandbyte = new byte();
            bool unknown = false;
            asmr.BaseStream.Seek(0, SeekOrigin.Begin);

            bool debug_bool = false;

            Int16 rt = new Int16();
            Int16 rs = new Int16();
            Int16 rd = new Int16();
            Int16 sa = new Int16();




            byte[] immbyte = new byte[2];




            int[] current_offset = new int[] { 0x1000, 0xF7510, 0x123640 };

            int[] dataLength = new int[] { 0xD8B70, 0x2A890, 0x4E40 };

            for (int loop = 0; loop < 3; loop++)
            {
                for (int i = 0; i < dataLength[loop]; i += 4)
                {


                    asmr.BaseStream.Seek(current_offset[loop], SeekOrigin.Begin);
                    asmbytes = asmr.ReadBytes(4);

                    commandbyte = asmbytes[0];

                    debug_bool = false;  ///set FALSE to ONLY print debug commands

                    unknown = true;
                    String CommandBinary = Convert.ToString(commandbyte, 2).PadLeft(8, '0');
                    compare = Convert.ToInt16(CommandBinary.Substring(0, 6), 2);
                    ///MessageBox.Show("Command "+compare.ToString()+"- 0x "+BitConverter.ToString(asmbytes).Replace("-", " "));
                    if (compare == 1)
                    {
                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int Value = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);

                        if (rt == 0)
                        {
                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLTZ} Branch on Less than Zero - If the value at register " + rs.ToString() + " is < 0 then Branch within the 256MB region at 0x" + Value.ToString("X");

                        }
                        if (rt == 1)
                        {
                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BGEZ} Branch on Greater than or Equal to Zero - If the value at register " + rs.ToString() + " is >= 0 then Branch within the 256MB region at 0x" + Value.ToString("X");

                        }
                        if (rt == 2)
                        {
                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLTZL} Branch on Less than Zero Likely - If the value at register " + rs.ToString() + " is < 0 then Branch within the 256MB region at 0x" + Value.ToString("X");

                        }
                        if (rt == 3)
                        {
                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BGEZL} Branch on Greater than or Equal to Zero Likely - If the value at register " + rs.ToString() + " is >= 0 then Branch within the 256MB region at 0x" + Value.ToString("X");

                        }
                        if (rt == 16)
                        {
                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLTZAL} Branch on Less than or Equal to Zero And Link- If the value at register " + rs.ToString() + " is  < 0 then Branch within the 256MB region at 0x" + Value.ToString("X") + " and return address";

                        }
                        if (rt == 17)
                        {
                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BGEZAL} Branch on Greater than or Equal to Zero And Link- If the value at register " + rs.ToString() + " is >= 0 then Branch within the 256MB region at 0x" + Value.ToString("X") + " and return address";

                        }
                        if (rt == 18)
                        {
                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLTZALL} Branch on Less than or Equal to Zero And Link Likely- If the value at register " + rs.ToString() + " is < 0 then Branch within the 256MB region at 0x" + Value.ToString("X") + " and return address";

                        }
                        if (rt == 19)
                        {
                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BGEZALL} Branch on Greater than or Equal to Zero And Link Likely- If the value at register " + rs.ToString() + " is >= 0 then Branch within the 256MB region at 0x" + Value.ToString("X") + " and return address";

                        }







                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 2)
                    {
                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int Value = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                        Value = Convert.ToInt32(Binary.Substring(6, 26), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{J} Jump - Branch within the 256MB region at 0x" + Value.ToString("X");

                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 3)
                    {
                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int Value = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                        Value = Convert.ToInt32(Binary.Substring(6, 26), 2);




                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{JAL} Jump and Link - Procedure Call within the 256MB region at 0x" + Value.ToString("X");

                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 4)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BEQ} Branch on Equal - If the values are equal at register " + rs.ToString() + " and at register " + rt.ToString() + "then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");

                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 5)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);


                        debug_bool = true;


                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BNE} Branch on Not Equal - If the values are not equal at register " + rs.ToString() + " and at register " + rt.ToString() + "then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");

                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 8)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ADDI} ADD Immediate Signed Word - Add the Signed value 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the Signed value at register " + rs.ToString() + " and write it to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 9)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);


                        debug_bool = true;


                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ADDIU} ADD Immediate Unsigned Word - Add the Unsigned value 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the Unsigned value at register " + rs.ToString() + " and write it to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 10)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SLTI} Set on Less Than Immediate - Set a 0/1 True/False value at register " + rt.ToString() + " if the Signed value at register " + rs.ToString() + " is less than the Signed value " + BitConverter.ToString(immbyte).Replace("-", "");
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 11)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SLTIU} Set on Less Than Immediate Unsigned - Set a 0/1 True/False value at register " + rt.ToString() + " if the Unsigned value at register " + rs.ToString() + " is less than the Unsigned value " + BitConverter.ToString(immbyte).Replace("-", "");
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 12)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ANDI} AND Immediate- Perform a Bitwise Logical AND for the value at register " + rs.ToString() + " and the value " + BitConverter.ToString(immbyte).Replace("-", "") + " and write it to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 13)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ORI} OR Immediate- Perform a Bitwise Logical OR for the value at register " + rs.ToString() + " and the value " + BitConverter.ToString(immbyte).Replace("-", "") + " and write it to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 14)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{XORI} XOR Immediate- Perform a Bitwise Logical XOR for the value at register " + rs.ToString() + " and the value " + BitConverter.ToString(immbyte).Replace("-", "") + " and write it to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }

                    if (compare == 15)
                    {

                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        ///MessageBox.Show(rt.ToString());

                        Array.Copy(asmbytes, 2, immbyte, 0, 2);



                        debug_bool = true;



                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LUI} Load Upper Immediate - at register " + rt.ToString() + " load value 0x" + BitConverter.ToString(immbyte).Replace("-", "");
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 20)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BEQL} Branch on Equal Likely- If the values are equal at register " + rs.ToString() + " and at register " + rt.ToString() + "then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 21)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BNEL} Branch on NOT Equal Likely- If the values are not equal at register " + rs.ToString() + " and at register " + rt.ToString() + "then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 22)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);

                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLEZL} Branch on Less Than or Equal to 0 Likely- If the value at register " + rs.ToString() + " is less than or equal to 0 then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 23)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);

                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLEZL} Branch on Greater Than or Equal to 0 Likely- If the value at register " + rs.ToString() + " is greater than or equal to 0 then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 24)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);




                        ///Oh, behave.
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{DADDI} Doubleword ADD Immediate- Add the value at register " + rs.ToString() + " to the value 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " and write it to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 25)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{DADDIU} Doubleword ADD Unsigned Immediate- Add the value at register " + rs.ToString() + " to the value 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " and write it to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 26)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LDL} Load Doubleword Left- Reads the value at register " + rs.ToString() + " and sets the Most-Significant bytes to the value of ( 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " and writes it to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 27)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LDL} Load Doubleword Right- Reads the value at register " + rs.ToString() + " and sets the Least-Significant bytes to the value of ( 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " and writes it to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 32)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);


                        debug_bool = true;


                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LB} Load Byte- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to a signed byte that is written to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 33)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LH} Load Halfword- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to a signed Halfword that is written to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 34)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LWL} Load World Left- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to a signed word whose Most-Significant bytes are added to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 35)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LW} Load Word- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to a signed word that is loaded to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 36)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LBU} Load Unsigned Byte- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to an unsigned byte that is written to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 37)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LHU} Load Halfword- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to an unsigned Halfword that is written to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 38)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LWLU} Load World Left- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to an unsigned word whose Most-Significant bytes are added to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 39)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LWU} Load Word- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to an unsigned word that is loaded to register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 40)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SB} Store Byte- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the byte at register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 41)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SH} Store HalfWord- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the halfword at register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 42)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SWL} Store Word Left- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the Most Significant bytes from the word at register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 43)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SW} Store Word- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the word at register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 44)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SDL} Store Doubleword Left- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the Most-Significant bytes of the Doubleword at register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 45)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SDR} Store Doubleword Right- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the Least-Significant bytes of the Doubleword at register " + rt.ToString();
                        ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 46)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SWR} Store Word Right- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the Least Significant bytes from the word at register " + rt.ToString();                    ///MessageBox.Show(output);
                        unknown = false;
                    }
                    if (compare == 61)
                    {
                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   ///base?
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        Array.Copy(asmbytes, 2, immbyte, 0, 2);





                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SDC1} Store Doubleword from Float- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the doubleword located at register " + rt.ToString();                    ///MessageBox.Show(output);
                        unknown = false;
                    }


                    if (compare == 0xFF)
                    {
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "-BREAK-";
                        unknown = false;
                    }



                    /// If commandbyte is 0x00, there is a secondary commandbyte at the far end
                    if (compare == 0x00)
                    {
                        commandbyte = asmbytes[3];
                        CommandBinary = Convert.ToString(commandbyte, 2).PadLeft(8, '0');
                        compare = Convert.ToInt16(CommandBinary.Substring(2, 6), 2);

                        ///MessageBox.Show(commandbyte.ToString() + "-" + compare.ToString() + "---" + BitConverter.ToString(asmbytes).Replace("-", " "));
                        if (compare == 0)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);
                            sa = Convert.ToInt16(Binary.Substring(21, 5), 2);

                            if (rt != 0 || rd != 0 || sa != 0)  /// If all values are 0 then the hex string was [00 00 00 00] and can be skipped.
                            {


                                output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SLL} Shift Word Left Logical - Left-Shift the word at register " + rt.ToString() + " by " + sa.ToString() + " and write it to register " + rd.ToString();
                                ///MessageBox.Show(output);
                            }
                            else
                            {

                                output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "[00 00 00 00]";
                            }
                        }
                        if (compare == 2)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);
                            sa = Convert.ToInt16(Binary.Substring(21, 5), 2);

                            if (rt != 0 || rd != 0 || sa != 0)  /// If all values are 0 then the hex string was [00 00 00 00] and can be skipped.
                            {


                                output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SRL} Shift Word Right Logical - Left-Shift the word at register " + rt.ToString() + " by " + sa.ToString() + " and write it to register " + rd.ToString();
                                ///MessageBox.Show(output);
                            }
                        }
                        if (compare == 3)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);
                            sa = Convert.ToInt16(Binary.Substring(21, 5), 2);

                            if (rt != 0 || rd != 0 || sa != 0)  /// If all values are 0 then the hex string was [00 00 00 00] and can be skipped.
                            {


                                output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SRL} Shift Word Right Arithmetic - Left-Shift the word at register " + rt.ToString() + " by " + sa.ToString() + " and write it to register " + rd.ToString();
                                ///MessageBox.Show(output);
                            }
                        }
                        if (compare == 7)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);


                            if (rt != 0 || rd != 0 || sa != 0)  /// If all values are 0 then the hex string was [00 00 00 00] and can be skipped.
                            {


                                output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SRL} Shift Word Right Arithmetic - Left-Shift the word at register " + rt.ToString() + " by the amount at register " + rs.ToString() + " and write it to register " + rd.ToString();
                                ///MessageBox.Show(output);
                            }
                        }
                        if (compare == 8)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip2, 0, 2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0);
                            String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);









                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{JR} Jump Register - Jump to Address in Register " + rs.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 16)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 2, flip2, 0, 2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0);
                            String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                            rd = Convert.ToInt16(Binary.Substring(0, 5), 2);







                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{MFHI} Move From HI Register - Move the special HI register to register " + rd.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 18)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 2, flip2, 0, 2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0);
                            String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                            rd = Convert.ToInt16(Binary.Substring(0, 5), 2);









                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{MFLO} Move From LO Register - Move the special HI register to register " + rd.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 24)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 2, flip2, 0, 2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0);
                            String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{MULT} Multiply Word- Multiply Signed 32 Bit Integers at  register " + rs.ToString() + " and register " + rt.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 26)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 2, flip2, 0, 2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0);
                            String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{DIV} Divide Word- Divide Signed 32 Bit Integer at register " + rs.ToString() + " by register " + rt.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 27)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 2, flip2, 0, 2);
                            Array.Reverse(flip2);
                            valuesign16 = BitConverter.ToInt16(flip2, 0);
                            String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{DIV} Divide Word- Divide Unsigned 32 Bit Integer at register " + rs.ToString() + " by register " + rt.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 32)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ADD} Add Word- Add the Signed Word at register " + rs.ToString() + " to register " + rt.ToString() + " and write it to register " + rd.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 33)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ADDU} Add Word- Add the Unsigned Word at register " + rs.ToString() + " to register " + rt.ToString() + " and write it to register " + rd.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 34)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SUB} Subtract Word- Subtract the Signed Word at register " + rt.ToString() + " from the Word at register " + rt.ToString() + " and write it to register " + rd.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 35)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SUBU} Subtract Word- Subtract the Unsigned Word at register " + rt.ToString() + " from the Word at register " + rt.ToString() + " and write it to register " + rd.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 36)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{AND} AND- Perform a Bitwise Logical AND for the value at register " + rs.ToString() + " and the value at register " + rt.ToString() + " and write it to register " + rd.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 37)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{OR} OR- Perform a Bitwise Logical OR for the value at register " + rs.ToString() + " and the value at register " + rt.ToString() + " and write it to register " + rd.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 37)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{OR} OR- Perform a Bitwise Logical OR for the value at register " + rs.ToString() + " and the value at register " + rt.ToString() + " and write it to register " + rd.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 38)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{XOR} XOR- Perform a Bitwise Logical XOR for the value at register " + rs.ToString() + " and the value at register " + rt.ToString() + " and write it to register " + rd.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 42)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SLT} Set on Less Than- Set a 0/1 True/False value at register " + rd.ToString() + " if the Signed value at register " + rs.ToString() + " is less than the Signed value at register " + rt.ToString();
                            ///MessageBox.Show(output);
                        }
                        if (compare == 43)
                        {
                            unknown = false;

                            Array.Copy(asmbytes, 0, flip4, 0, 4);
                            Array.Reverse(flip4);
                            int value32 = BitConverter.ToInt32(flip4, 0);
                            String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                            rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                            rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                            rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SLTU} Set on Less Than- Set a 0/1 True/False value at register " + rd.ToString() + " if the Unsigned value at register " + rs.ToString() + " is less than the Unsigned value at register " + rt.ToString();
                            ///MessageBox.Show(output);
                        }
                    }



                    if (unknown)
                    {
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "";
                        ///MessageBox.Show("-Unknown Command 0x" + compare.ToString("X").PadLeft(2, '0') + "-  @0x" + current_offset.ToString("X").PadLeft(2, '0'));
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "-Unknown Command -" + compare.ToString() + "-  @0x" + current_offset[loop].ToString("X").PadLeft(2, '0');
                    }

                    current_offset[loop] += 4;
                    asmr.BaseStream.Seek(current_offset[loop], SeekOrigin.Begin);


                    if (debug_bool)
                    {



                        output = "0x" + (asmr.BaseStream.Position - 4).ToString("X").PadLeft(8, '0') + output;

                        if (output != "")
                        {
                            System.IO.File.AppendAllText(savePath, output + Environment.NewLine);
                        }
                    }
                    else
                    {

                    }
                }
            }






        }

        public byte[] DecompressMIO0(byte[] inputFile)
        {
            byte[] outputFile = Cereal64.Common.Utils.Encoding.MIO0.Decode(inputFile);
            
            return outputFile;
        }

        public byte[] CompressMIO0(byte[] inputFile)
        {
            byte[] outputFile = Cereal64.Common.Utils.Encoding.MIO0.Encode(inputFile);
            return outputFile;
        }

        public byte[] Decompress_seg7(byte[] useg7)
        {

            /// This will decompress Segment 7's compressed display lists to regular F3DEX commands.
            /// This is used exclusively by Mario Kart 64's Segment 7.

            int indexA = 0;
            int indexB = 0;
            int indexC = 0;











            MemoryStream romm = new MemoryStream(useg7);
            BinaryReader mainseg = new BinaryReader(romm);
            MemoryStream seg7m = new MemoryStream();
            BinaryWriter seg7w = new BinaryWriter(seg7m);

            seg7w.BaseStream.Seek(0, SeekOrigin.Begin);

            byte commandbyte = new byte();
            byte[] byte29 = new byte[2];



            mainseg.BaseStream.Seek(0, SeekOrigin.Begin);


            byte[] voffset = new byte[2];

            bool DispEnd = true;

            for (int i = 0; DispEnd; i++)
            {

                if (mainseg.BaseStream.Position == mainseg.BaseStream.Length)
                {
                    DispEnd = false;
                }
                else
                {
                    commandbyte = mainseg.ReadByte();




                    if (i > 2415)
                    {
                        ///MessageBox.Show(i.ToString()+"-Execute Order 0x" + commandbyte.ToString("X"));
                    }
                    if (commandbyte == 0xFF)
                    {


                        DispEnd = false;
                    }

                    if (commandbyte >= 0x00 && commandbyte <= 0x14)
                    {

                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBC000002));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x80000040));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x03860010));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x09000000 | (commandbyte * 0x18) + 8));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x03880010));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x09000000 | commandbyte * 0x18));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x15)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFC121824));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFF33FFFF));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);

                    }
                    if (commandbyte == 0x16)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFC127E24));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFFFFF3F9));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x17)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFCFFFFFF));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFFFE793C));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x18)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB900031D));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00552078));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x19)
                    {
                        ///
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB900031D));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00553078));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if ((commandbyte >= 0x1A && commandbyte <= 0x1F) | commandbyte == 0x2C)
                    {

                        UInt32 ImgSize = 0, ImgType = 0, ImgFlag1 = 0, ImgFlag2 = 0, ImgFlag3 = 0;
                        UInt32[] ImgTypes = { 0, 0, 0, 3, 3, 3, 0 }; ///0=RGBA, 3=IA
                        UInt32[] STheight = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; ///looks like
                        UInt32[] STwidth = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 };
                        byte[] Param = new byte[2];

                        Param[0] = mainseg.ReadByte();
                        Param[1] = mainseg.ReadByte();


                        if (commandbyte == 0x2C)
                        {
                            ImgType = ImgTypes[6];
                            ImgFlag1 = STheight[6];
                            ImgFlag2 = STwidth[6];
                            ImgFlag3 = 0x100;
                        }
                        else
                        {
                            ImgType = ImgTypes[commandbyte - 0x1A];
                            ImgFlag1 = STheight[commandbyte - 0x1A];
                            ImgFlag2 = STwidth[commandbyte - 0x1A];
                            ImgFlag3 = 0;
                        }
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xE8000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((((ImgType << 0x15) | 0xF5100000) | ((((ImgFlag2 << 1) + 7) >> 3) << 9)) | ImgFlag3));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((((((Param[1] & 0xF) << 0x12) | (((Param[1] & 0xF0) >> 4) << 0xE)) | ((Param[0] & 0xF) << 8)) | (((Param[0] & 0xF0) >> 4) << 4))));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xF2000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((((ImgFlag2 - 1) << 0xE) | ((ImgFlag1 - 1) << 2))));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte >= 0x20 && commandbyte <= 0x25)
                    {
                        UInt32 ImgSize = 0, ImgType = 0, ImgFlag1 = 0, ImgFlag2 = 0, ImgFlag3 = 0;
                        UInt32[] ImgTypes = { 0, 0, 0, 3, 3, 3, 0 }; ///0=RGBA, 3=IA
                        UInt32[] STheight = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; ///looks like
                        UInt32[] STwidth = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 };
                        byte[] Param = new byte[3];

                        Param[0] = mainseg.ReadByte();
                        Param[1] = mainseg.ReadByte();
                        Param[2] = mainseg.ReadByte();


                        ImgType = ImgTypes[commandbyte - 0x20];
                        ImgFlag1 = STheight[commandbyte - 0x20];
                        ImgFlag2 = STwidth[commandbyte - 0x20];
                        ImgFlag3 = 0;

                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((ImgType | 0xFD000000) | 0x100000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((Param[0] << 0xB) + 0x05000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xE8000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((((ImgType << 0x15) | 0xF5000000) | 0x100000) | (Param[2] & 0xF)));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((((Param[2] & 0xF0) >> 4) << 0x18)));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xE6000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);

                        ImgSize = (ImgFlag2 * ImgFlag1) - 1;
                        if (ImgSize > 0x7FF) ImgSize = 0x7FF;

                        UInt32 Unknown2x = new UInt32();

                        Unknown2x = 1;
                        Unknown2x = (ImgFlag2 << 1) >> 3; ///purpose of this value is unknown

                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xF3000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((((Unknown2x + 0x7FF) / Unknown2x) | (((Param[2] & 0xF0) >> 4) << 0x18)) | (ImgSize << 0xC)));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);

                    }
                    if (commandbyte == 0x26)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBB000001));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFFFFFFFF));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x27)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBB000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00010001));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x28)
                    {
                        //flip4 = mainseg.ReadBytes(2);
                        //Array.Reverse(flip4);
                        uint address = mainseg.ReadUInt16();


                        int lvertCount = mainseg.ReadByte() & 0x3F;
                        int lvertIndex = mainseg.ReadByte() & 0x3F;



                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x04000000 | (lvertIndex * 2) << 16 | (lvertCount << 10) + (16 * (lvertCount) - 1)));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x04000000 | address * 16));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x29)
                    {
                        value16 = mainseg.ReadUInt16();
                        indexA = (value16 >> 10) & 0x1F;
                        indexB = (value16 >> 5) & 0x1F;
                        indexC = value16 & 0x1F;



                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBF000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((indexC << 17) | (indexB << 9) | indexA << 1));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x2A)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB8000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x2B)
                    {
                        value16 = mainseg.ReadUInt16();
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x06000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((0x07000000 | (value16 * 8))));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x2D)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBE000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00000140));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x2E)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D002E));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D002E));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x2F)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D002F));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D002F));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x30)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D0030));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D0030));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte >= 0x33 && commandbyte <= 0x52)
                    {

                        value16 = mainseg.ReadUInt16();

                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x04000000 | (((commandbyte - 0x32) * 0x410) - 1)));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x04000000 | (value16 * 16)));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x53)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFCFFFFFF));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFFFCF279));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }

                    if (commandbyte == 0x54)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB900031D));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00442D58));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x55)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB900031D));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00404DD8));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x56)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB7000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00002000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x57)
                    {
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB6000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00002000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x58)
                    {

                        value16 = mainseg.ReadUInt16();
                        indexA = (value16 >> 10) & 0x1F;
                        indexB = (value16 >> 5) & 0x1F;
                        indexC = value16 & 0x1F;



                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB1000000 | (indexC << 17) | (indexB << 9) | indexA << 1));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        value16 = mainseg.ReadUInt16();
                        indexA = (value16 >> 10) & 0x1F;
                        indexB = (value16 >> 5) & 0x1F;
                        indexC = value16 & 0x1F;

                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((indexC << 17) | (indexB << 9) | indexA << 1));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);

                    }
                    if (i > 2415)
                    {
                        ///MessageBox.Show(i.ToString() + "-Finished Order 0x" + commandbyte.ToString("X"));
                    }
                }
            }

            byte[] seg7 = seg7m.ToArray();

            return (seg7);





        }


        public byte[] compress_seg7(byte[] ROM)
        {

            /// This will compress compatible F3DEX commands into a compressed Segment 7.
            /// This is used exclusively by Mario Kart 64's Segment 7.
            /// No, I don't know how this works. Don't ask me how it works.
            /// I have no fucking clue how I wrote this. If you find out, let me know!



            ///You may ask yourself, "What is that beautiful house?"
            ///You may ask yourself, "Where does that highway go to?"
            ///And you may ask yourself, "Am I right? Am I wrong?"
            ///And you may say to yourself, "My God! What have I done?"







            int indexA = 0;
            int indexB = 0;
            int indexC = 0;


            MemoryStream romm = new MemoryStream(ROM);
            BinaryReader mainseg = new BinaryReader(romm);
            MemoryStream seg7m = new MemoryStream();
            BinaryWriter seg7w = new BinaryWriter(seg7m);

            seg7w.BaseStream.Seek(0, SeekOrigin.Begin);


            string commandbyte = "";  ///keeping the same name from above decompress process
            byte[] byte29 = new byte[2];
            string compar = "";
            byte F3Dbyte = new byte();
            byte[] parambyte = new byte[2];






            byte[] voffset = new byte[2];

            byte compressbyte = new byte();

            mainseg.BaseStream.Position = 0;

            for (int i = 0; (mainseg.BaseStream.Position < mainseg.BaseStream.Length); i++)
            {

                F3Dbyte = mainseg.ReadByte();
                commandbyte = F3Dbyte.ToString("x").PadLeft(2, '0').ToUpper();

                ///MessageBox.Show(F3Dbyte.ToString("x").PadLeft(2,'0').ToUpper() + "--" + mainseg.BaseStream.Position.ToString()); ;




                if (commandbyte == "BC")
                {


                    MessageBox.Show("Unsupported Command -BC-");
                    ///0x00 -- 0x14

                    ///curently unsupported, ??not featured in stock MK64 racing tracks?? Can't find in multiple courses.


                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBC000002));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);
                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x80000040));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);
                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x03860010));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);
                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x09000000 | (commandbyte * 0x18) + 8));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);
                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x03880010));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);
                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x09000000 | commandbyte * 0x18));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);


                }
                if (commandbyte == "FC")
                {

                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    if (compar == "1218")
                    {
                        compressbyte = 0x15;
                    }
                    if (compar == "127E")
                    {
                        compressbyte = 0x16;
                    }
                    if (compar == "FFFF")
                    {
                        compressbyte = 0x17;
                    }

                    mainseg.BaseStream.Seek(5, SeekOrigin.Current);
                    seg7w.Write(compressbyte);

                }
                if (commandbyte == "B9")
                {


                    mainseg.BaseStream.Seek(5, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    if (compar == "2078")
                    {
                        compressbyte = 0x18;
                    }
                    if (compar == "3078")
                    {
                        compressbyte = 0x19;
                    }
                    seg7w.Write(compressbyte);

                }
                if (commandbyte == "E8")
                {
                    /// 00000000 00000000
                    ///0x1A -> 0x1F + 0x2C
                    mainseg.BaseStream.Seek(7, SeekOrigin.Current);





                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");
                    byte29 = mainseg.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");

                    byte[] Param = new byte[2];


                    ///don't ask me I don't know
                    ///don't ask me I don't know
                    byte[] parameters = mainseg.ReadBytes(4);
                    Array.Reverse(parameters);
                    value32 = BitConverter.ToUInt32(parameters, 0);
                    uint opint = new uint();
                    byte height = Convert.ToByte((value32 >> 14) & 0xF);
                    byte heightmode = Convert.ToByte((value32 >> 18) & 0xF);
                    byte width = Convert.ToByte((value32 >> 4) & 0xF);
                    byte widthmode = Convert.ToByte((value32 >> 8) & 0xF);

                    Param[0] = Convert.ToByte((height) << 4 | (heightmode));

                    opint = value32 >> 4;

                    Param[1] = Convert.ToByte((width) << 4 | (widthmode));

                    Array.Reverse(Param);

                    mainseg.BaseStream.Seek(4, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");
                    byte29 = mainseg.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");
                    ///MessageBox.Show(compar);
                    if (compar == "F51011000007C07C")
                    {
                        compressbyte = 0x2C;
                    }
                    if (compar == "F51010000007C07C")
                    {

                        compressbyte = 0x1A;
                    }
                    if (compar == "F5102000000FC07C")
                    {

                        compressbyte = 0x1B;
                    }
                    if (compar == "F51010000007C0FC")
                    {
                        compressbyte = 0x1C;
                    }
                    if (compar == "F57010000007C07C")
                    {
                        compressbyte = 0x1D;
                    }
                    if (compar == "F5702000000FC07C")
                    {
                        compressbyte = 0x1E;
                    }
                    if (compar == "F57010000007C0FC")
                    {
                        compressbyte = 0x1F;
                    }
                    ///MessageBox.Show(BitConverter.ToString(Param));
                    seg7w.Write(compressbyte);
                    seg7w.Write(Param);
                    ///don't ask me I don't know
                    ///don't ask me I don't know
                }
                if (commandbyte == "FD")
                {


                    ///0x20  ->  0x25


                    mainseg.BaseStream.Seek(1, SeekOrigin.Current);


                    byte[] Param = new byte[3];


                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    ///dont ask me I don't know
                    ///dont ask me I don't know
                    ///dont ask me I don't know

                    byte[] parambytes = mainseg.ReadBytes(4);
                    Array.Reverse(parambytes);
                    value32 = BitConverter.ToUInt32(parambytes, 0);


                    Param[0] = Convert.ToByte((value32 - 0x05000000) >> 11);
                    Param[1] = 0x00;
                    Param[2] = 0x70;
                    ///dont ask me I don't know
                    ///dont ask me I don't know
                    ///dont ask me I don't know
                    ///MessageBox.Show(value32.ToString("x")+"--"+Param[0].ToString("x"));
                    mainseg.BaseStream.Seek(28, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");
                    byte29 = mainseg.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");




                    if (compar == "0000073FF100")
                    {

                        compressbyte = 0x20;
                    }
                    if (compar == "0000077FF080")
                    {

                        compressbyte = 0x21;
                    }
                    if (compar == "0000077FF100")
                    {
                        compressbyte = 0x22;
                    }
                    if (compar == "0003073FF100")
                    {
                        compressbyte = 0x23;
                    }
                    if (compar == "0003077FF080")
                    {
                        compressbyte = 0x24;
                    }
                    if (compar == "0003077FF100")
                    {
                        compressbyte = 0x25;
                    }

                    seg7w.Write(compressbyte);
                    seg7w.Write(Param);


                }
                if (commandbyte == "BB")
                {

                    ///0x26 000001  FFFFFFFF
                    ///0x27    00010001
                    mainseg.BaseStream.Seek(3, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    if (compar == "0001")
                    {
                        compressbyte = 0x27;
                    }
                    if (compar == "FFFF")
                    {
                        compressbyte = 0x26;
                    }
                    seg7w.Write(compressbyte);

                    mainseg.BaseStream.Seek(2, SeekOrigin.Current);




                }

                if (commandbyte == "BF")
                {



                    compressbyte = 0x29;
                    seg7w.Write(compressbyte);
                    mainseg.BaseStream.Seek(4, SeekOrigin.Current);

                    indexC = mainseg.ReadByte();
                    indexB = mainseg.ReadByte();
                    indexA = mainseg.ReadByte();

                    indexA = indexA / 2;
                    indexB = indexB / 2;
                    indexC = indexC / 2;


                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((indexC) | (indexB << 5) | indexA << 10));


                    seg7w.Write(flip4);
                }
                if (commandbyte == "B8")
                {

                    ///0x2A
                    compressbyte = 0x2A;
                    seg7w.Write(compressbyte);
                    mainseg.BaseStream.Seek(7, SeekOrigin.Current);

                }
                if (commandbyte == "06")
                {

                    ///0x2B
                    compressbyte = 0x2B;


                    mainseg.BaseStream.Seek(3, SeekOrigin.Current);

                    byte[] parambytes = mainseg.ReadBytes(4);
                    Array.Reverse(parambytes);
                    value32 = BitConverter.ToUInt32(parambytes, 0);

                    value32 = value32 & 0x00FFFFFF;



                    seg7w.Write(compressbyte);
                    seg7w.Write(Convert.ToUInt16(value32 / 8));
                }
                if (commandbyte == "BE")
                {

                    ///0x2D
                    compressbyte = 0x2D;
                    mainseg.BaseStream.Seek(7, SeekOrigin.Current);
                    seg7w.Write(compressbyte);
                }
                if (commandbyte == "D0")
                {


                    mainseg.BaseStream.Seek(3, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    if (compar == "002E")
                    {
                        compressbyte = 0x2E;
                    }
                    if (compar == "002F")
                    {
                        compressbyte = 0x2F;
                    }
                    if (compar == "0030")
                    {
                        compressbyte = 0x30;
                    }

                    seg7w.Write(compressbyte);
                    mainseg.BaseStream.Seek(8, SeekOrigin.Current);
                }
                if (commandbyte == "04")
                {

                    ///0x33->0x52
                    mainseg.BaseStream.Seek(1, SeekOrigin.Current);
                    byte[] Param = mainseg.ReadBytes(2);
                    Array.Reverse(Param);
                    value16 = BitConverter.ToUInt16(Param, 0);

                    compressbyte = Convert.ToByte(((value16 + 1) / 0x410) + 0x32);
                    seg7w.Write(compressbyte);

                    byte[] parambytes = mainseg.ReadBytes(4);
                    Array.Reverse(parambytes);
                    value32 = BitConverter.ToUInt32(parambytes, 0);
                    value32 = (value32 - 0x04000000) / 16;

                    value16 = Convert.ToUInt16(value32);
                    seg7w.Write(value16);
                }

                if (commandbyte == "FC")
                {

                    ///0x53
                    compressbyte = 0x53;
                    mainseg.BaseStream.Seek(7, SeekOrigin.Current);
                    seg7w.Write(compressbyte);
                }

                if (commandbyte == "B9")
                {

                    mainseg.BaseStream.Seek(3, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    if (compar == "0044")
                    {
                        compressbyte = 0x54;
                    }
                    if (compar == "0040")
                    {
                        compressbyte = 0x55;
                    }
                    seg7w.Write(compressbyte);
                    mainseg.BaseStream.Seek(2, SeekOrigin.Current);




                }
                if (commandbyte == "B7")
                {

                    ///0x56
                    compressbyte = 0x56;
                    mainseg.BaseStream.Seek(15, SeekOrigin.Current);

                    seg7w.Write(compressbyte);
                }
                if (commandbyte == "B6")
                {

                    ///0x57
                    compressbyte = 0x57;
                    mainseg.BaseStream.Seek(15, SeekOrigin.Current);
                    seg7w.Write(compressbyte);
                }
                if (commandbyte == "B1")
                {

                    ///0x58
                    compressbyte = 0x58;
                    seg7w.Write(compressbyte);


                    indexC = mainseg.ReadByte();
                    indexB = mainseg.ReadByte();
                    indexA = mainseg.ReadByte();

                    indexA = indexA / 2;
                    indexB = indexB / 2;
                    indexC = indexC / 2;


                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((indexC) | (indexB << 5) | indexA << 10));


                    seg7w.Write(flip4);


                    ///twice for second set of verts
                    ///have to move the reader forward by 1 position first

                    mainseg.BaseStream.Seek(1, SeekOrigin.Current);

                    indexC = mainseg.ReadByte();
                    indexB = mainseg.ReadByte();
                    indexA = mainseg.ReadByte();

                    indexA = indexA / 2;
                    indexB = indexB / 2;
                    indexC = indexC / 2;

                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((indexC) | (indexB << 5) | indexA << 10));


                    seg7w.Write(flip4);





                }

            }


            flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFF000000));
            Array.Reverse(flip4);
            seg7w.Write(flip4);
            flip4 = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(flip4);
            seg7w.Write(flip4);

            seg7w.Write(flip4);

            seg7w.Write(flip4);

            ///fin


            //MessageBox.Show("Compressed");
            byte[] seg7 = seg7m.ToArray();
            return (seg7);





        }
    }

}

// phew! We made it. I keep saying we, but it's me doing all the work!
// maybe try pitching in sometime and updating the program! I'd love the help!

// Thank you so much for-a reading my source!

// OverKart 64 Library
// For Mario Kart 64 1.0 USA ROM
// <3 Hamp
