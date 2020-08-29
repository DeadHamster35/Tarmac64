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
using Tarmac64_Geometry;


//custom libraries

using Assimp;  //for handling model data
using Texture64;  //for handling texture data


using Cereal64.Microcodes.F3DEX.DataElements;
using Cereal64.Common.DataElements;
using Cereal64.Common.Rom;
using Cereal64.Common.Utils.Encoding;

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
        public byte[] compileSegment(byte[] seg4, byte[] seg6, byte[] seg7, byte[] seg9, byte[] romBytes, int cID)
        {


            ///This takes precompiled segments and inserts them into the ROM file. It also updates the course header table to reflect
            /// the new data sizes. This allows for proper loading of the course so long as the segments are properly setup. All segment
            /// data should be precompressed where applicable, this assumes that segment 4 and segment 6 are MIO0 compressed and that
            /// Segment 7 has had it's special compression ran. Segment 9 has no compression. romBytes is the ROM file as a byte array, and CID
            /// is the ID of the course we're looking to replace based on it's location in the course header table. 


            /// This writes all segments to the end of the file for simplicity. If data was larger than original (which it almost always will be for custom courses)
            /// then it cannot fit in the existing space without overwriting other course data. 

            TM64 mk = new TM64();
            bs = new MemoryStream();
            bs.Write(romBytes, 0, romBytes.Length);
            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);

            UInt32 seg6start = 0;
            UInt32 seg6end = 0;
            UInt32 seg4start = 0;
            UInt32 seg7end = 0;
            UInt32 seg9start = 0;
            UInt32 seg9end = 0;
            UInt32 seg7start = 0;
            UInt32 seg7rsp = 0;


            int addressAlign = 0;



            bw.BaseStream.Position = bw.BaseStream.Length;

            addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                bw.Write(Convert.ToByte(0x00));
            }


            seg6start = Convert.ToUInt32(bw.BaseStream.Position);
            bw.Write(seg6, 0, seg6.Length);
            seg6end = Convert.ToUInt32(bw.BaseStream.Position);
            ///

            addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                bw.Write(Convert.ToByte(0x00));
            }

            seg9start = Convert.ToUInt32(bw.BaseStream.Position);
            bw.Write(seg9, 0, seg9.Length);
            seg9end = Convert.ToUInt32(bw.BaseStream.Position);
            ///

            addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                bw.Write(Convert.ToByte(0x00));
            }

            seg4start = Convert.ToUInt32(bw.BaseStream.Position);
            bw.Write(seg4, 0, seg4.Length);
            ///


            addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                bw.Write(Convert.ToByte(0x00));
            }

            seg7start = Convert.ToUInt32(bw.BaseStream.Position);
            bw.Write(seg7, 0, seg7.Length);
            seg7end = Convert.ToUInt32(bw.BaseStream.Position);
            ///
            seg7rsp = Convert.ToUInt32(seg7start + seg47_buf[cID] - seg4start);



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

            byte[] vertbyte = decompressMIO0(seg4);
            UInt32 vertcount = Convert.ToUInt32(vertbyte.Length / 14);

            flip = BitConverter.GetBytes(vertcount);
            Array.Reverse(flip);
            vertcount = BitConverter.ToUInt32(flip, 0);
            ///MessageBox.Show(vertbyte.Count.ToString() + "--" + vertcount.ToString());

            TM64_Geometry tmGeo = new TM64_Geometry();
            ///seg7 size

            byte[] seg7byte = tmGeo.decompress_seg7(seg7);
            UInt32 seg7size = Convert.ToUInt32(seg7byte.Length);


            ///MessageBox.Show(seg7size.ToString());

            flip = BitConverter.GetBytes(seg7size);
            Array.Reverse(flip);
            seg7size = BitConverter.ToUInt32(flip, 0);

            ///MessageBox.Show(seg7size.ToString());


            bw.BaseStream.Seek(1188752 + (cID * 48), SeekOrigin.Begin);

            bw.Write(seg6start);
            bw.Write(seg6end);
            bw.Write(seg4start);
            bw.Write(seg7end);
            bw.Write(seg9start);
            bw.Write(seg9end);

            bw.BaseStream.Seek(4, SeekOrigin.Current);



            bw.Write(vertcount);


            bw.Write(seg7rsp);



            bw.Write(seg7size);

            byte[] newROM = bs.ToArray();
            return newROM;

        }

        public void DumpTextures(int cID, string outputDir, string filePath)
        {



            
            byte[] romBytes = File.ReadAllBytes(filePath);
            byte[] seg9 = new byte[(seg9_end[cID] - seg9_addr[cID])];

            Buffer.BlockCopy(romBytes, Convert.ToInt32(seg9_addr[cID]), seg9, 0, (Convert.ToInt32(seg9_end[cID]) - Convert.ToInt32(seg9_addr[cID])));



            string[] coursename = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut" };


            bs = new MemoryStream(seg9);

            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);
            int moffset = new int();
            for (int i = 0; br.BaseStream.Position < br.BaseStream.Length; i++)
            {
                flip4 = br.ReadBytes(4);
                Array.Reverse(flip4);
                int textureOffset = BitConverter.ToInt32(flip4, 0);

                flip4 = br.ReadBytes(4);
                Array.Reverse(flip4);
                int compressSize = BitConverter.ToInt32(flip4, 0);



                if (textureOffset != 0)
                {
                    textureOffset = (textureOffset & 0xFFFFFF) + 0x641F70;

                    byte[] textureFile = new byte[compressSize];

                    Array.Copy(romBytes, textureOffset, textureFile, 0, compressSize);

                    byte[] decompressedTexture = decompressMIO0(textureFile);

                    string texturePath = Path.Combine(outputDir, coursename[cID] + i.ToString());

                    File.WriteAllBytes(texturePath + ".bin", decompressedTexture);

                }
                else
                {
                    br.BaseStream.Seek(br.BaseStream.Length, SeekOrigin.Begin);
                }
            }
            MessageBox.Show("Finished");


            


        }




        public byte[] dumpseg4(int cID, byte[] romBytes)
        {

            bs = new MemoryStream(romBytes);
            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);
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

            }


            byte[] seg4 = new byte[(seg7_romptr[cID] - seg4_addr[cID])];


            Buffer.BlockCopy(romBytes, Convert.ToInt32(seg4_addr[cID]), seg4, 0, (Convert.ToInt32(seg7_romptr[cID]) - Convert.ToInt32(seg4_addr[cID])));

            return seg4;
        }

        public byte[] dumpseg5(int cID, byte[] romBytes)
        {

            bs = new MemoryStream(romBytes);
            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);
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

            }
            List<int> offsets = new List<int>();

            byte[] seg9 = new byte[(seg9_end[cID] - seg9_addr[cID])];
            List<byte> segment5 = new List<byte>();

            Buffer.BlockCopy(romBytes, Convert.ToInt32(seg9_addr[cID]), seg9, 0, (Convert.ToInt32(seg9_end[cID]) - Convert.ToInt32(seg9_addr[cID])));

            TM64 mk = new TM64();

            bs = new MemoryStream(seg9);

            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);
            int textureOffset = new int();
            int compressSize = new int();

            br.BaseStream.Position = (texture_addr[cID] - 0x09000000);

            for (int i = 0; br.BaseStream.Position < br.BaseStream.Length; i++)
            {
                flip4 = br.ReadBytes(4);
                Array.Reverse(flip4);
                textureOffset = BitConverter.ToInt32(flip4, 0);

                flip4 = br.ReadBytes(4);
                Array.Reverse(flip4);
                compressSize = BitConverter.ToInt32(flip4, 0);



                if (textureOffset != 0)
                {
                    textureOffset = (textureOffset & 0xFFFFFF) + 0x641F70;

                    byte[] textureFile = new byte[compressSize];

                    Array.Copy(romBytes, textureOffset, textureFile, 0, compressSize);

                    byte[] decompressedTexture = decompressMIO0(textureFile);
                    offsets.Add(segment5.Count);
                    segment5.AddRange(decompressedTexture);
                    br.BaseStream.Seek(8, SeekOrigin.Current);
                }
                else
                {
                    br.BaseStream.Seek(br.BaseStream.Length, SeekOrigin.Begin);
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




        public byte[] dumpseg6(int cID, byte[] romBytes)
        {

            bs = new MemoryStream(romBytes);
            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);
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

            }


            byte[] seg6 = new byte[(seg6_end[cID] - seg6_addr[cID])];


            Buffer.BlockCopy(romBytes, Convert.ToInt32(seg6_addr[cID]), seg6, 0, (Convert.ToInt32(seg6_end[cID]) - Convert.ToInt32(seg6_addr[cID])));

            return seg6;
        }

        public byte[] dumpseg7(int cID, byte[] romBytes)
        {

            bs = new MemoryStream(romBytes);
            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);
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

            }


            byte[] seg7 = new byte[(seg7_end[cID] - seg7_romptr[cID])];


            Buffer.BlockCopy(romBytes, Convert.ToInt32(seg7_romptr[cID]), seg7, 0, (Convert.ToInt32(seg7_end[cID]) - Convert.ToInt32(seg7_romptr[cID])));

            return seg7;
        }

        public byte[] dumpseg9(int cID, byte[] romBytes)
        {

            bs = new MemoryStream(romBytes);
            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);
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

            }


            byte[] seg9 = new byte[(seg9_end[cID] - seg9_addr[cID])];


            Buffer.BlockCopy(romBytes, Convert.ToInt32(seg9_addr[cID]), seg9, 0, (Convert.ToInt32(seg9_end[cID]) - Convert.ToInt32(seg9_addr[cID])));

            return seg9;
        }

        public byte[] dump_ASM(string filePath)
        {

            /// This is specfically designed for Mario Kart 64 USA 1.0 ROM. It should dump to binary the majority of it's ASM commands.
            /// It uses a list provided by MiB to find the ASM sections, there could be plenty of code I'm missing

            byte[] romBytes = File.ReadAllBytes(filePath);
            byte[] asm = new byte[1081936];

            byte[] buffer = new byte[1];
            buffer[0] = 0xFF;


            Buffer.BlockCopy(romBytes, 4096, asm, 0, 887664);

            for (int i = 0; i < 8; i++)
            {
                Buffer.BlockCopy(buffer, 0, asm, 887664 + i, 1);
            }

            Buffer.BlockCopy(romBytes, 1013008, asm, 887672, 174224);

            for (int i = 0; i < 8; i++)
            {
                Buffer.BlockCopy(buffer, 0, asm, 1061896 + i, 1);
            }

            Buffer.BlockCopy(romBytes, 1193536, asm, 1061904, 20032);
            return asm;

        }

        public void translate_ASM(string savePath, string filePath)
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
            bool combo = true;

            Int16 rt = new Int16();
            Int16 rs = new Int16();
            Int16 rd = new Int16();
            Int16 sa = new Int16();

            Int32 target = new Int32();
            Int32 asmbase = new Int32();

            float fs = new float();
            float ft = new float();
            float fd = new float();

            byte[] immbyte = new byte[2];

            Int16 imm = new Int16();
            Int16 offset = new Int16();


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
                        combo = false;
                    }
                }
            }






        }















        public byte[] decompressMIO0(byte[] inputFile)
        {
            byte[] outputFile = Cereal64.Common.Utils.Encoding.MIO0.Decode(inputFile);
            return outputFile;
        }

        public byte[] compressMIO0(byte[] inputFile)
        {
            byte[] outputFile = Cereal64.Common.Utils.Encoding.MIO0.Encode(inputFile);
            return outputFile;
        }



        //fake MIO0 compression.

        public byte[] fakeCompress (byte[] inputFile)
        {
            
            MemoryStream outputStream = new MemoryStream();

            BinaryReader outputReader = new BinaryReader(outputStream);
            BinaryWriter outputWriter = new BinaryWriter(outputStream);

            byte[] byteArray = new byte[0];

            int uncompressedOffset = (inputFile.Length / 8);

            int addressAlign = 8 - (Convert.ToInt32(uncompressedOffset) % 8);
            if (addressAlign == 8)
                addressAlign = 0;

            uncompressedOffset = uncompressedOffset + addressAlign;


            //start fake compression

            byteArray = BitConverter.GetBytes(0x4D494F30);
            Array.Reverse(byteArray);
            outputWriter.Write(byteArray);

            byteArray = BitConverter.GetBytes(inputFile.Length);
            Array.Reverse(byteArray);
            outputWriter.Write(byteArray);

            byteArray = BitConverter.GetBytes(0x00000000);
            Array.Reverse(byteArray);
            outputWriter.Write(byteArray);

            byteArray = BitConverter.GetBytes(uncompressedOffset + 20); // 16 bytes for header size, 4 bytes padding.
            Array.Reverse(byteArray);
            outputWriter.Write(byteArray);

            

            for (int x = 0; x < uncompressedOffset; x++)
            {
                outputWriter.Write(Convert.ToByte(0xFF));
            }


            //padding
            outputWriter.Write(0xFFFFFFFF);



            outputWriter.Write(inputFile);

            //finished fake compression


            byte[] returnByte = outputStream.ToArray();
            return returnByte;


        }







        public byte[] d_seg7(byte[] useg7)
        {

            /// This will decompress Segment 7's compressed display lists to regular F3DEX commands.
            /// This is used exclusively by Mario Kart 64's Segment 7.

            int v0 = 0;
            int v1 = 0;
            int v2 = 0;











            MemoryStream romm = new MemoryStream(useg7);
            BinaryReader mainseg = new BinaryReader(romm);
            MemoryStream seg7m = new MemoryStream();
            BinaryWriter seg7w = new BinaryWriter(seg7m);

            seg7w.BaseStream.Seek(0, SeekOrigin.Begin);

            int compare = new int();
            byte commandbyte = new byte();
            byte[] byte29 = new byte[2];



            mainseg.BaseStream.Seek(0, SeekOrigin.Begin);

            int vertoffset = 0;
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
                        value16 = mainseg.ReadUInt16();
                        value16 = mainseg.ReadUInt16();
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x040681FF));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x04050500));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                    }
                    if (commandbyte == 0x29)
                    {
                        value16 = mainseg.ReadUInt16();
                        v0 = (value16 >> 10) & 0x1F;
                        v1 = (value16 >> 5) & 0x1F;
                        v2 = value16 & 0x1F;



                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBF000000));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((v2 << 17) | (v1 << 9) | v0 << 1));
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
                        v0 = (value16 >> 10) & 0x1F;
                        v1 = (value16 >> 5) & 0x1F;
                        v2 = value16 & 0x1F;



                        flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB1000000 | (v2 << 17) | (v1 << 9) | v0 << 1));
                        Array.Reverse(flip4);
                        seg7w.Write(flip4);
                        value16 = mainseg.ReadUInt16();
                        v0 = (value16 >> 10) & 0x1F;
                        v1 = (value16 >> 5) & 0x1F;
                        v2 = value16 & 0x1F;

                        flip4 = BitConverter.GetBytes(Convert.ToUInt32((v2 << 17) | (v1 << 9) | v0 << 1));
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

        public byte[] compress_seg7(byte[] segment7)
        {

            /// This will compress compatible F3DEX commands into a compressed Segment 7.
            /// This is used exclusively by Mario Kart 64's Segment 7.
            /// No, I don't know how this works. Don't ask me how it works.
            /// I have no fucking clue how I wrote this. If you find out, let me know!



            ///You may ask yourself, "What is that beautiful house?"
            ///You may ask yourself, "Where does that highway go to?"
            ///And you may ask yourself, "Am I right? Am I wrong?"
            ///And you may say to yourself, "My God! What have I done?"







            int v0 = 0;
            int v1 = 0;
            int v2 = 0;











            MemoryStream romm = new MemoryStream(segment7);
            BinaryReader mainseg = new BinaryReader(romm);
            MemoryStream seg7m = new MemoryStream();
            BinaryWriter seg7w = new BinaryWriter(seg7m);

            seg7w.BaseStream.Seek(0, SeekOrigin.Begin);

            int compare = new int();

            string commandbyte = "";  ///keeping the same name from above decompress process
            byte[] byte29 = new byte[2];
            string compar = "";
            byte F3Dbyte = new byte();
            byte[] parambyte = new byte[2];





            int vertoffset = 0;
            byte[] voffset = new byte[2];

            byte compressbyte = new byte();

            bool DispEnd = true;
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
                    /// 000000 00000000
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
                    opint = value32 >> 14;

                    Param[0] = Convert.ToByte((opint & 0xF0) >> 4 | (opint & 0xF) << 4);

                    opint = value32 >> 4;

                    Param[1] = Convert.ToByte((opint & 0xF0) >> 4 | (opint & 0xF) << 4);

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

                    v2 = mainseg.ReadByte();
                    v1 = mainseg.ReadByte();
                    v0 = mainseg.ReadByte();

                    v0 = v0 / 2;
                    v1 = v1 / 2;
                    v2 = v2 / 2;


                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((v2) | (v1 << 5) | v0 << 10));


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


                    v2 = mainseg.ReadByte();
                    v1 = mainseg.ReadByte();
                    v0 = mainseg.ReadByte();

                    v0 = v0 / 2;
                    v1 = v1 / 2;
                    v2 = v2 / 2;


                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((v2) | (v1 << 5) | v0 << 10));


                    seg7w.Write(flip4);


                    ///twice for second set of verts
                    ///have to move the reader forward by 1 position first

                    mainseg.BaseStream.Seek(1, SeekOrigin.Current);

                    v2 = mainseg.ReadByte();
                    v1 = mainseg.ReadByte();
                    v0 = mainseg.ReadByte();



                    v0 = v0 / 2;
                    v1 = v1 / 2;
                    v2 = v2 / 2;

                    if (Math.Abs(v0 - v1) > 31 || Math.Abs(v1 - v2) > 31 || Math.Abs(v2 - v1) > 31)
                    {
                        ///MessageBox.Show("Vert Cache Error-" +Environment.NewLine+ "Face Composed from vertices outside 32 vert cache. Cannot create face. OverKart64 will now crash.");
                    }

                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((v2) | (v1 << 5) | v0 << 10));


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

            byte[] seg7 = seg7m.ToArray();
            return (seg7);





        }

        public int GetMax(int first, int second)
        {
            return first > second ? first : second; /// It will take care of all the 3 scenarios
        }

        public int GetMin(int first, int second)
        {
            return first < second ? first : second; /// It will take care of all the 3 scenarios
        }

        public byte[] compress_seg7(string filePath)
        {

            /// This will compress compatible F3DEX commands into a compressed Segment 7.
            /// This is used exclusively by Mario Kart 64's Segment 7.
            /// No, I don't know how this works. Don't ask me how it works.
            /// I have no fucking clue how I wrote this. If you find out, let me know!



            ///You may ask yourself, "What is that beautiful house?"
            ///You may ask yourself, "Where does that highway go to?"
            ///And you may ask yourself, "Am I right? Am I wrong?"
            ///And you may say to yourself, "My God! What have I done?"







            int v0 = 0;
            int v1 = 0;
            int v2 = 0;








            byte[] ROM = File.ReadAllBytes(filePath);




            MemoryStream romm = new MemoryStream(ROM);
            BinaryReader mainseg = new BinaryReader(romm);
            MemoryStream seg7m = new MemoryStream();
            BinaryWriter seg7w = new BinaryWriter(seg7m);

            seg7w.BaseStream.Seek(0, SeekOrigin.Begin);

            int compare = new int();

            string commandbyte = "";  ///keeping the same name from above decompress process
            byte[] byte29 = new byte[2];
            string compar = "";
            byte F3Dbyte = new byte();
            byte[] parambyte = new byte[2];





            int vertoffset = 0;
            byte[] voffset = new byte[2];

            byte compressbyte = new byte();

            bool DispEnd = true;
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
                    /// 000000 00000000
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
                    opint = value32 >> 14;

                    Param[0] = Convert.ToByte((opint & 0xF0) >> 4 | (opint & 0xF) << 4);

                    opint = value32 >> 4;

                    Param[1] = Convert.ToByte((opint & 0xF0) >> 4 | (opint & 0xF) << 4);

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

                    v2 = mainseg.ReadByte();
                    v1 = mainseg.ReadByte();
                    v0 = mainseg.ReadByte();

                    v0 = v0 / 2;
                    v1 = v1 / 2;
                    v2 = v2 / 2;


                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((v2) | (v1 << 5) | v0 << 10));


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


                    v2 = mainseg.ReadByte();
                    v1 = mainseg.ReadByte();
                    v0 = mainseg.ReadByte();

                    v0 = v0 / 2;
                    v1 = v1 / 2;
                    v2 = v2 / 2;


                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((v2) | (v1 << 5) | v0 << 10));


                    seg7w.Write(flip4);


                    ///twice for second set of verts
                    ///have to move the reader forward by 1 position first

                    mainseg.BaseStream.Seek(1, SeekOrigin.Current);

                    v2 = mainseg.ReadByte();
                    v1 = mainseg.ReadByte();
                    v0 = mainseg.ReadByte();

                    v0 = v0 / 2;
                    v1 = v1 / 2;
                    v2 = v2 / 2;

                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((v2) | (v1 << 5) | v0 << 10));


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


            MessageBox.Show("Compressed");
            byte[] seg7 = seg7m.ToArray();
            return (seg7);





        }


        public string F3DEX_Model(byte commandbyte, byte[] segment, byte[] seg4, int vertoffset, int segmentoffset)
        {
            /// segment is the segment that contained the F3DEX command. for Mario Kart 64 it will most likely be Seg6 or Seg7.
            /// seg4 is an uncompressed 14-byte vert Array, based on Mario Kart 64's layout. This contains all the vertices for a Mario Kart 64 course.
            /// segmentoffset is the position right after the F3DEX commandbyte. If any parameters are read it needs the right offset to start reading from.
            /// if you don't need to draw triangles, you can pass any value for vertoffset. Otherwise we need to know the last position
            /// a vert was loaded from, and the program will treat that position as the new 0 index. Not the same process but the same result.
            /// The Vert Offset is a manipulation of two specific "quirks" to how Mario Kart 64 loads vertices. 
            /// F3DEX has 32 vert registers, meaning you can only load 32 verts at the same time. Command 0x04 loads them
            /// 0x04 loads a certain number of verts from an offset into segment 4 at a certain index in the current vert register. 
            /// So it can for example load 5 vertices from offset 0x4FF0 into segment 4 starting at vert index 7, replacing verts 7-12. 
            /// HOWEVER, it never does this! it always loads the verts at index 0! 
            /// Because it always loads to index 0 and because we have access to the entire segment 4 vert cache, we can cheat! :)
            /// When we get a vert index we multiply it by the size of the vert structure (14 bytes compressed / 16 bytes uncompressed) and add this 
            /// to the vert offset loaded from 0x04. This becomes an offset directly to that verts data in Segment 4. This is much easier and quicker.
            /// Now for command 0x04 we only set the vertoffset to the value in the F3DEX command. It will ALWAYS be segment 4 for Mario Kart 64....
            /// but if it ever comes across verts outside segment 4 it will throw up an error message to warn the user.
            /// The commands for 0xB1 and 0xBF return the 3 vert positions seperated by , with each vert seperated by ;
            /// there is an alternative commented out that will return a direct maxscript command to render the triangle. 
            /// command 0x06 will return the segment and offset of the display lists to run on seperate lines.
            /// command 0xB8 represents the end of a display list and will return "ENDSECTION"
            /// command 0x04 will return the vertoffset described above, which should be updated and maintained by the calling function to be passed again.
            /// F3DEX_Model needs a proper vertoffset provided every time for either 0xB1 or 0xBF commands, it is not maintained automatically.
            /// 


            MemoryStream mainsegm = new MemoryStream(segment);
            MemoryStream segm4 = new MemoryStream(seg4);
            BinaryReader mainsegr = new BinaryReader(mainsegm);
            BinaryReader seg4r = new BinaryReader(segm4);

            int v0 = new int();
            int v1 = new int();
            int v2 = new int();

            int[] xval = new int[3];
            int[] yval = new int[3];
            int[] zval = new int[3];
            int[] sval = new int[3];
            int[] tval = new int[3];


            mainsegr.BaseStream.Position = segmentoffset;

            int texclass = new int();

            string outputstring = "";

            ///mainsegr Either Seg6 or Seg7 Uncompressed
            ///seg4r Seg4 Uncompressed
            bool breakoff = true;

            if (commandbyte == 0xE4)
            {

            }
            if (commandbyte == 0xB1)
            {
                for (int i = 0; i < 2; i++)
                {

                    ///Draw 2 Triangles
                    ///Returns Vert Positions of 3 Verts that make 1 triangle.
                    ///Returns Vert Positions of 3 Verts that make 1 triangle. (line2)



                    v0 = mainsegr.ReadByte();
                    v1 = mainsegr.ReadByte();
                    v2 = mainsegr.ReadByte();

                    v0 = v0 / 2;
                    v1 = v1 / 2;
                    v2 = v2 / 2;

                    ///outputstring = outputstring + v0.ToString() + "-" + v1.ToString() + "-" + v2.ToString() + "-" + vertoffset.ToString() +"-"+ mainsegr.BaseStream.Position.ToString() + Environment.NewLine;
                    /// outputs the 3 vert indexes as well as the vertoffset and offset into the segment that called this command.


                    ///
                    seg4r.BaseStream.Seek(vertoffset + (v0 * 16), SeekOrigin.Begin);

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    xval[0] = BitConverter.ToInt16(flip2, 0); ///x

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    zval[0] = BitConverter.ToInt16(flip2, 0); ///z

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    yval[0] = BitConverter.ToInt16(flip2, 0); ///y
                                                              ///


                    ///
                    seg4r.BaseStream.Seek(vertoffset + (v1 * 16), SeekOrigin.Begin);

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    xval[1] = BitConverter.ToInt16(flip2, 0); ///x

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    zval[1] = BitConverter.ToInt16(flip2, 0); ///z

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    yval[1] = BitConverter.ToInt16(flip2, 0); ///y
                                                              ///


                    ///
                    seg4r.BaseStream.Seek(vertoffset + (v2 * 16), SeekOrigin.Begin);

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    xval[2] = BitConverter.ToInt16(flip2, 0); ///x

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    zval[2] = BitConverter.ToInt16(flip2, 0); ///z

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    yval[2] = BitConverter.ToInt16(flip2, 0); ///y
                                                              ///
                    yval[0] = yval[0] * -1;
                    yval[1] = yval[1] * -1;
                    yval[2] = yval[2] * -1;

                    ///outputstring = outputstring + "vertbox = mesh vertices:#([" + xval[0].ToString() + ",(" + (yval[0]).ToString() + ")," + zval[0].ToString() + "],[" + xval[1].ToString() + ",(" + (yval[1]).ToString() + ")," + zval[1].ToString() + "],[" + xval[2].ToString() + ",(" + (yval[2]).ToString() + ")," + zval[2].ToString() + "]) faces:#([1,2,3]) MaterialIDS:#(1) " + Environment.NewLine;
                    outputstring = outputstring + xval[0].ToString() + "," + yval[0].ToString() + "," + zval[0].ToString() + ";" + xval[1].ToString() + "," + yval[1].ToString() + "," + zval[1].ToString() + ";" + xval[2].ToString() + "," + yval[2].ToString() + "," + zval[2].ToString() + "," + Environment.NewLine;
                    ///
                    mainsegr.BaseStream.Seek(1, SeekOrigin.Current);
                }
            }
            if (commandbyte == 0xBF)
            {


                ///Draw 1 Triangle
                ///Returns Vert Positions of 3 Verts that make 1 triangle.



                mainsegr.BaseStream.Seek(4, SeekOrigin.Current);


                v0 = mainsegr.ReadByte();
                v1 = mainsegr.ReadByte();
                v2 = mainsegr.ReadByte();

                v0 = v0 / 2;
                v1 = v1 / 2;
                v2 = v2 / 2;

                ///outputstring = outputstring + v0.ToString() + "-" + v1.ToString() + "-" + v2.ToString() + "-" + vertoffset.ToString() + "-" + mainsegr.BaseStream.Position.ToString() + Environment.NewLine;
                ////// outputs the 3 vert indexes as well as the vertoffset and offset into the segment that called this command.


                ///
                seg4r.BaseStream.Seek(vertoffset + (v0 * 16), SeekOrigin.Begin);

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                xval[0] = BitConverter.ToInt16(flip2, 0); ///x

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                zval[0] = BitConverter.ToInt16(flip2, 0); ///z

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                yval[0] = BitConverter.ToInt16(flip2, 0); ///y
                                                          ///


                ///
                seg4r.BaseStream.Seek(vertoffset + (v1 * 16), SeekOrigin.Begin);

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                xval[1] = BitConverter.ToInt16(flip2, 0); ///x

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                zval[1] = BitConverter.ToInt16(flip2, 0); ///z

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                yval[1] = BitConverter.ToInt16(flip2, 0); ///y
                                                          ///


                ///
                seg4r.BaseStream.Seek(vertoffset + (v2 * 16), SeekOrigin.Begin);

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                xval[2] = BitConverter.ToInt16(flip2, 0); ///x

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                zval[2] = BitConverter.ToInt16(flip2, 0); ///z

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                yval[2] = BitConverter.ToInt16(flip2, 0); ///y
                                                          ///



                yval[0] = yval[0] * -1;
                yval[1] = yval[1] * -1;
                yval[2] = yval[2] * -1;

                ///outputstring = outputstring + "vertbox = mesh vertices:#([" + xval[0].ToString() + ",(" + (yval[0]).ToString() + ")," + zval[0].ToString() + "],[" + xval[1].ToString() + ",(" + (yval[1]).ToString() + ")," + zval[1].ToString() + "],[" + xval[2].ToString() + ",(" + (yval[2]).ToString() + ")," + zval[2].ToString() + "]) faces:#([1,2,3]) MaterialIDS:#(1) " + Environment.NewLine;
                outputstring = outputstring + xval[0].ToString() + "," + yval[0].ToString() + "," + zval[0].ToString() + ";" + xval[1].ToString() + "," + yval[1].ToString() + "," + zval[1].ToString() + ";" + xval[2].ToString() + "," + yval[2].ToString() + "," + zval[2].ToString() + "," + Environment.NewLine;
                ///
                mainsegr.BaseStream.Seek(1, SeekOrigin.Current);





            }

            if (commandbyte == 0xF5)

            {
                /// Load texture
                /// Returns the location of a texture
                /// Returns the class of a texture (line 2)



                byte[] byte29 = new byte[2];
                string compar = "";

                mainsegr.BaseStream.Seek(-1, SeekOrigin.Current);
                byte29 = mainsegr.ReadBytes(2);
                compar = BitConverter.ToString(byte29).Replace("-", "");
                byte29 = mainsegr.ReadBytes(2);
                compar = compar + BitConverter.ToString(byte29).Replace("-", "");

                byte[] Param = new byte[2];


                ///don't ask me I don't know
                ///don't ask me I don't know
                byte[] parameters = mainsegr.ReadBytes(4);
                Array.Reverse(parameters);
                value32 = BitConverter.ToUInt32(parameters, 0);
                uint opint = new uint();
                opint = value32 >> 14;

                Param[0] = Convert.ToByte((opint & 0xF0) >> 4 | (opint & 0xF) << 4);

                opint = value32 >> 4;

                Param[1] = Convert.ToByte((opint & 0xF0) >> 4 | (opint & 0xF) << 4);

                Array.Reverse(Param);




                mainsegr.BaseStream.Seek(4, SeekOrigin.Current);
                byte29 = mainsegr.ReadBytes(2);
                compar = compar + BitConverter.ToString(byte29).Replace("-", "");
                byte29 = mainsegr.ReadBytes(2);
                compar = compar + BitConverter.ToString(byte29).Replace("-", "");
                ///MessageBox.Show(compar);
                if (compar == "F51011000007C07C")
                {
                    texclass = 6;
                    ///MessageBox.Show("6");
                }
                if (compar == "F51010000007C07C")
                {

                    texclass = 0;
                    ///MessageBox.Show("0");
                }
                if (compar == "F5102000000FC07C")
                {

                    texclass = 1;
                    ///MessageBox.Show("1");
                }
                if (compar == "F51010000007C0FC")
                {
                    texclass = 2;
                    ///MessageBox.Show("2");
                }
                if (compar == "F57010000007C07C")
                {
                    texclass = 3;
                    ///MessageBox.Show("3");
                }
                if (compar == "F5702000000FC07C")
                {
                    texclass = 4;
                    ///MessageBox.Show("4");
                }
                if (compar == "F57010000007C0FC")
                {
                    texclass = 5;
                    ///MessageBox.Show("5");
                }

                uint location = 0;
                byte fdbyte = mainsegr.ReadByte();
                if (fdbyte == 0xFD)
                {
                    mainsegr.BaseStream.Seek(3, SeekOrigin.Current);

                    byte[] rsp_add = mainsegr.ReadBytes(4);
                    Array.Reverse(rsp_add);


                    int Value = BitConverter.ToInt32(rsp_add, 0);
                    String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                    int segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                    location = Convert.ToUInt32(Binary.Substring(8, 24), 2);

                }
                else
                {
                    location = 0xD00D00;
                    ///MessageBox.Show("D00D00- 0x" + fdbyte.ToString("X")+"---"+mainsegr.BaseStream.Position.ToString());
                }

                outputstring = location.ToString("X") + Environment.NewLine + texclass.ToString();

                ///MessageBox.Show(outputstring);

            }

            if (commandbyte == 0x04)
            {
                /// Load vertices
                /// Returns the vert offset into segment 4

                mainsegr.BaseStream.Seek(3, SeekOrigin.Current);
                byte[] rsp_add = mainsegr.ReadBytes(4);

                Array.Reverse(rsp_add);

                int Value = BitConverter.ToInt32(rsp_add, 0);
                String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                int segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                uint location = Convert.ToUInt32(Binary.Substring(8, 24), 2);




                if (segid == 4)
                {
                    outputstring = location.ToString();
                    ///MessageBox.Show(outputstring +"-"+ mainsegr.BaseStream.Position.ToString());
                }
                else
                {
                    MessageBox.Show("ERROR D35-01 :: VERTS LOADED FROM OUTSIDE SEGMENT 4");
                }


            }


            if (commandbyte == 0x06)
            {
                ///Call a display list
                ///Returns the segmentID to call
                ///Returns the Offset to call (line 2)


                mainsegr.BaseStream.Seek(3, SeekOrigin.Current);

                byte[] rsp_add = mainsegr.ReadBytes(4);

                Array.Reverse(rsp_add);

                int Value = BitConverter.ToInt32(rsp_add, 0);
                String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                int segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                int location = Convert.ToInt32(Binary.Substring(8, 24), 2);



                outputstring = segid + Environment.NewLine + location.ToString() + Environment.NewLine;

            }

            if (commandbyte == 0xB8)
            {
                /// End of section
                /// Returns ENDSECTION

                outputstring = "ENDSECTION" + Environment.NewLine;
            }




            return outputstring;

        }
        public List<Pathgroup> Load_3PL(string file_3PL)
        {
            //load the pathgroups from the external .SVL file provided
            List<Pathgroup> pathgroup = new List<Pathgroup>();


            string[] reader = File.ReadAllLines(file_3PL);
            string[] positions = new string[3];

            int groupcount = Convert.ToInt32(reader[0]);
            int current_line = 1;

            for (int group = 0; group < groupcount; group++)
            {
                pathgroup.Add(new Pathgroup());
                pathgroup[group].pathlist = new List<Pathlist>();

                current_line++;  //groupname garbage

                int pathcount = Convert.ToInt32(reader[current_line]);
                current_line++;
                for (int path = 0; path < pathcount; path++)
                {

                    pathgroup[group].pathlist.Add(new Pathlist());
                    pathgroup[group].pathlist[path].pathmarker = new List<Marker>();

                    current_line++;  //pathname garbage
                    int markercount = Convert.ToInt32(reader[current_line]);
                    current_line++;
                    for (int marker = 0; marker < markercount; marker++)
                    {
                        pathgroup[group].pathlist[path].pathmarker.Add(new Marker());


                        pathgroup[group].pathlist[path].pathmarker[marker].xval = Convert.ToInt32(reader[current_line]);
                        current_line++;
                        pathgroup[group].pathlist[path].pathmarker[marker].yval = Convert.ToInt32(reader[current_line]);
                        current_line++;
                        pathgroup[group].pathlist[path].pathmarker[marker].zval = Convert.ToInt32(reader[current_line]);
                        current_line++;
                        pathgroup[group].pathlist[path].pathmarker[marker].flag = Convert.ToInt32(reader[current_line]);
                        current_line++;
                    }
                }

            }
            return pathgroup;
        }






        //Add to existing course.
        public byte[] AddMarkers(byte[] seg6, int cID, List<Pathgroup> pathgroup, bool loadobjects)
        {


            loadoffsets();
            //if loadobjects is true, we load objects. Otherwise, we load PathMarkers.



            bs = new MemoryStream();
            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);


            bs.Write(seg6, 0, seg6.Length);
            int x = 0;
            foreach (Pathgroup group in pathgroup)
            {
                //this following part is the switch; either we're loading the offsets for objects or for pathmarkers.

                //if the user didn't properly format the  marker counts to match stock that's their problem.

                if (loadobjects == true)
                {
                    br.BaseStream.Position = objOffsets[cID].offset[x];
                }
                else
                {
                    br.BaseStream.Position = pathOffsets[cID].offset[x];
                }
                for (int n = 0; n < pathgroup[x].pathlist.Count; n = n + 1)
                {




                    for (int i = 0; i < pathgroup[x].pathlist[n].pathmarker.Count; i = i + 1)
                    {



                        int[] tempint = new int[4];



                        flip2 = BitConverter.GetBytes(Convert.ToInt16(pathgroup[x].pathlist[n].pathmarker[i].xval));
                        Array.Reverse(flip2);
                        bw.Write(flip2);  //x

                        flip2 = BitConverter.GetBytes(Convert.ToInt16(pathgroup[x].pathlist[n].pathmarker[i].zval));
                        Array.Reverse(flip2);
                        bw.Write(flip2);  //z

                        flip2 = BitConverter.GetBytes(Convert.ToInt16(pathgroup[x].pathlist[n].pathmarker[i].yval));
                        Array.Reverse(flip2);
                        bw.Write(flip2);  //y 

                        flip2 = BitConverter.GetBytes(Convert.ToUInt16(pathgroup[x].pathlist[n].pathmarker[i].flag));
                        Array.Reverse(flip2);
                        bw.Write(flip2);  //flag






                    }




                    flip2 = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                    Array.Reverse(flip2);
                    bw.Write(flip2);  //x

                    if (loadobjects == true || n + 1 < pathgroup[x].pathlist.Count)
                    {

                        flip2 = BitConverter.GetBytes(Convert.ToInt16(0));
                        Array.Reverse(flip2);
                        bw.Write(flip2);  //z

                        flip2 = BitConverter.GetBytes(Convert.ToInt16(0));
                        Array.Reverse(flip2);
                        bw.Write(flip2);  //y 
                    }
                    else
                    {
                        flip2 = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                        Array.Reverse(flip2);
                        bw.Write(flip2);  //z

                        flip2 = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                        Array.Reverse(flip2);
                        bw.Write(flip2);  //y 
                    }


                    flip2 = BitConverter.GetBytes(Convert.ToInt16(0));
                    Array.Reverse(flip2);
                    bw.Write(flip2);  //flag





                }
                x = x + 1;
            }


            seg6 = bs.ToArray();
            return seg6;
        }

        public TM64_Geometry.OK64Texture textureClass(TM64_Geometry.OK64Texture textureObject)
        {

            // series of If/Then statements checking first the Format, then the Height and Width of the texture data to determine it's format.
            // if a texture's width/height are not either 32 or 64 then it returns -1 as an error for bad texture dimensions.
            // if both the height and width are 64, it'll return texture class 0. This should be fixed for future builds with an additional check.


            if ((textureObject.textureWidth == 32 | textureObject.textureWidth == 64) & (textureObject.textureHeight == 32 | textureObject.textureHeight == 64))
            {
                if (textureObject.textureFormat == 0)
                {
                    if (textureObject.textureWidth == 32)
                    {
                        if (textureObject.textureHeight == 32)
                        {
                            textureObject.textureClass = 0;
                        }
                        else
                        {
                            textureObject.textureClass = 2;
                        }

                    }
                    else
                    {
                        textureObject.textureClass = 1;
                    }
                }
                else
                {
                    if (textureObject.textureWidth == 32)
                    {
                        if (textureObject.textureHeight == 32)
                        {
                            textureObject.textureClass = 3;
                        }
                        else
                        {
                            textureObject.textureClass = 5;
                        }

                    }
                    else
                    {
                        textureObject.textureClass = 4;
                    }
                }
            }
            else
            {
                textureObject.textureClass = -1;
                // texture is not proper dimensions, return an invalid value.
            }

            return textureObject;
        }


        public byte[] writeTextures(byte[] rom, TM64_Geometry.OK64Texture[] textureObject)
        {
            bs = new MemoryStream();
            br = new BinaryReader(bs);
            bw = new BinaryWriter(bs);

            int segment5Position = 0;


            bw.Write(rom);
            int textureCount = (textureObject.Length);
            for (int currentTexture = 0; currentTexture < textureCount; currentTexture++)
            {
                if (textureObject[currentTexture].texturePath != null)
                {
                    // Establish codec and convert texture. Compress converted texture data via MIO0 compression

                    N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                    byte[] imageData = null;
                    byte[] paletteData = null;
                    Bitmap bitmapData = new Bitmap(textureObject[currentTexture].texturePath);
                    N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].textureFormat], bitmapData);
                    byte[] compressedTexture = compressMIO0(imageData);


                    // finish setting texture parameters based on new texture and compressed data.

                    textureObject[currentTexture].compressedSize = compressedTexture.Length;
                    textureObject[currentTexture].fileSize = imageData.Length;
                    textureObject[currentTexture].segmentPosition = segment5Position;  // we need this to build out F3DEX commands later. 
                    segment5Position = segment5Position + textureObject[currentTexture].fileSize;


                    //adjust the MIO0 offset to an 8-byte address as required for N64.
                    bw.BaseStream.Position = bw.BaseStream.Length;
                    int addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
                    if (addressAlign == 4)
                        addressAlign = 0;


                    for (int align = 0; align < addressAlign; align++)
                    {
                        bw.Write(Convert.ToByte(0x00));
                    }



                    // write compressed MIO0 texture to end of ROM.

                    textureObject[currentTexture].romPosition = Convert.ToInt32(bw.BaseStream.Length);
                    bw.BaseStream.Position = bw.BaseStream.Length;
                    bw.Write(compressedTexture);
                }
            }

            bw.Write(Convert.ToInt32(0));
            bw.Write(Convert.ToInt32(0));
            bw.Write(Convert.ToInt32(0));
            bw.Write(Convert.ToInt32(0));


            byte[] romOut = bs.ToArray();
            return romOut;


        }


        public byte[] compiletextureTable(TM64_Geometry.OK64Texture[] textureObject)
        {
            bs = new MemoryStream();
            br = new BinaryReader(bs);
            bw = new BinaryWriter(bs);

            byte[] byteArray = new byte[0];

            int segment5Position = 0;

            MemoryStream seg9m = new MemoryStream();
            BinaryReader seg9r = new BinaryReader(seg9m);
            BinaryWriter seg9w = new BinaryWriter(seg9m);

            int textureCount = (textureObject.Length);
            for (int currentTexture = 0; currentTexture < textureCount; currentTexture++) 
            {
                // write out segment 9 texture reference.
                if (textureObject[currentTexture].texturePath != null)
                {
                    byteArray = BitConverter.GetBytes(Convert.ToUInt32(0x0F000000 | textureObject[currentTexture].romPosition - 0x641F70));
                    Array.Reverse(byteArray);
                    seg9w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(Convert.ToUInt32(textureObject[currentTexture].compressedSize));
                    Array.Reverse(byteArray);
                    seg9w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(Convert.ToUInt32(textureObject[currentTexture].fileSize));
                    Array.Reverse(byteArray);
                    seg9w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
                    Array.Reverse(byteArray);
                    seg9w.Write(byteArray);
                }
            }
            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);
            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);
            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);
            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);


            byte[] seg9Out = seg9m.ToArray();
            return seg9Out;
        }
        public void compileF3DObject(ref int outMagic, ref byte[] outseg4, ref byte[] outseg7, Assimp.Scene fbx, byte[] segment4, byte[] segment7, TM64_Geometry.OK64F3DObject[] courseObject, TM64_Geometry.OK64Texture[] textureObject, int vertMagic)
        {



            
            List<string> object_name = new List<string>();




            byte[] byteArray = new byte[0];


            UInt32 ImgSize = 0, ImgType = 0, ImgFlag1 = 0, ImgFlag2 = 0, ImgFlag3 = 0;
            UInt32[] ImgTypes = { 0, 0, 0, 3, 3, 3, 0 }; ///0=RGBA, 3=IA
            UInt32[] STheight = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; ///looks like
            UInt32[] STwidth = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 }; ///texture sizes...
            byte[] heightex = { 5, 5, 6, 5, 5, 6, 5 };
            byte[] widthex = { 5, 6, 5, 5, 6, 5, 5 };



            int relativeZero = vertMagic;
            int relativeIndex = 0;

            int segment5Position = 0;

            MemoryStream seg7m = new MemoryStream();
            BinaryReader seg7r = new BinaryReader(seg7m);
            BinaryWriter seg7w = new BinaryWriter(seg7m);


            MemoryStream seg4m = new MemoryStream();
            BinaryReader seg4r = new BinaryReader(seg4m);
            BinaryWriter seg4w = new BinaryWriter(seg4m);

            //prewrite existing Segment 4 data. 
            seg4w.Write(segment4);

            //prewrite existing Segment 7 data, OR, prefix Segment 7 with a 0xB8 Command. 
            if (segment7.Length > 0)
            {
                seg7w.Write(segment7);
            }
            else
            {
                //Prep Segment 7 for any hardcoded display lists

                byteArray = BitConverter.GetBytes(0xB8000000);
                Array.Reverse(byteArray);
                seg7w.Write(byteArray);

                byteArray = BitConverter.GetBytes(0x00000000);
                Array.Reverse(byteArray);
                seg7w.Write(byteArray);
            }

            foreach (var cObj in courseObject)
            {
                cObj.meshPosition = new int[cObj.meshID.Length];
                for(int subIndex = 0; subIndex < cObj.meshID.Length; subIndex++)
                {
                    var current_subobject = fbx.Meshes[cObj.meshID[subIndex]];


                    

                    int vertcount = current_subobject.Vertices.Count;
                    ///MessageBox.Show(currentnode.Name+"-"+child.Name + "-Vert Count-"+vertcount.ToString());
                    int facecount = current_subobject.Faces.Count;
                    ///MessageBox.Show(currentnode.Name + "-" + child.Name + "-Face Count-" + facecount.ToString());


                    if (facecount > 30)
                    {
                        MessageBox.Show("Warning! Subobject with more than 30 Faces. - " + cObj.objectName);
                    }

                    int materialID = new int();

                    TM64_Geometry.Face[] face = new TM64_Geometry.Face[facecount];
                    TM64_Geometry.Vertex[] vert = new TM64_Geometry.Vertex[current_subobject.Vertices.Count];

                    materialID = cObj.materialID;

                    



                    for (int f = 0; f < facecount; f++)
                    {
                        


                        face[f] = new TM64_Geometry.Face();

                        face[f].vertIndex.indexA = current_subobject.Faces[f].Indices[0];

                        face[f].vertIndex.indexC = current_subobject.Faces[f].Indices[1];

                        face[f].vertIndex.indexB = current_subobject.Faces[f].Indices[2]; ;

                        face[f].material = materialID;

                        vert[face[f].vertIndex.indexA] = new TM64_Geometry.Vertex { };
                        vert[face[f].vertIndex.indexA].position = new TM64_Geometry.Position { };


                        vert[face[f].vertIndex.indexA].position.x = Convert.ToInt16(current_subobject.Vertices[face[f].vertIndex.indexA].X);

                        vert[face[f].vertIndex.indexA].position.y = Convert.ToInt16(current_subobject.Vertices[face[f].vertIndex.indexA].Y);
                        /// Flip YZ Axis 
                        vert[face[f].vertIndex.indexA].position.z = Convert.ToInt16(current_subobject.Vertices[face[f].vertIndex.indexA].Z);    /// Flip YZ Axis 
                        vert[face[f].vertIndex.indexA].position.z = Convert.ToInt16(vert[face[f].vertIndex.indexA].position.z); /// Flip Y Axis


                       
                        //
                        


                        vert[face[f].vertIndex.indexC] = new TM64_Geometry.Vertex { };
                        vert[face[f].vertIndex.indexC].position = new TM64_Geometry.Position { };

                        vert[face[f].vertIndex.indexC].position.x = Convert.ToInt16(current_subobject.Vertices[face[f].vertIndex.indexC].X);

                        vert[face[f].vertIndex.indexC].position.y = Convert.ToInt16(current_subobject.Vertices[face[f].vertIndex.indexC].Y);

                        vert[face[f].vertIndex.indexC].position.z = Convert.ToInt16(current_subobject.Vertices[face[f].vertIndex.indexC].Z);    /// Flip YZ Axis 
                        vert[face[f].vertIndex.indexC].position.z = Convert.ToInt16(vert[face[f].vertIndex.indexC].position.z); /// Flip Y Axis



                        //

                        vert[face[f].vertIndex.indexB] = new TM64_Geometry.Vertex { };
                        vert[face[f].vertIndex.indexB].position = new TM64_Geometry.Position { };


                        vert[face[f].vertIndex.indexB].position.x = Convert.ToInt16(current_subobject.Vertices[face[f].vertIndex.indexB].X);

                        vert[face[f].vertIndex.indexB].position.y = Convert.ToInt16(current_subobject.Vertices[face[f].vertIndex.indexB].Y);

                        vert[face[f].vertIndex.indexB].position.z = Convert.ToInt16(current_subobject.Vertices[face[f].vertIndex.indexB].Z);    /// Flip YZ Axis 
                        vert[face[f].vertIndex.indexB].position.z = Convert.ToInt16(vert[face[f].vertIndex.indexB].position.z); /// Flip Y Axis



                        //normalize UV coordinates per triangle



                        float u_base = 0;
                        float v_base = 0;

                        float[] u_shift = { 0, 0, 0 };
                        float[] v_shift = { 0, 0, 0 };
                        float[] u_offset = { 0, 0, 0 };
                        float[] v_offset = { 0, 0, 0 };
                        float UVdecs = 0;
                        float UVdect = 0;
                        Int16 local_s = 0;
                        Int16 local_t = 0;

                        

                        u_offset[0] = Convert.ToSingle(current_subobject.TextureCoordinateChannels[0][face[f].vertIndex.indexA][0]);
                        v_offset[0] = Convert.ToSingle(current_subobject.TextureCoordinateChannels[0][face[f].vertIndex.indexA][1]);

                        u_offset[1] = Convert.ToSingle(current_subobject.TextureCoordinateChannels[0][face[f].vertIndex.indexB][0]);
                        v_offset[1] = Convert.ToSingle(current_subobject.TextureCoordinateChannels[0][face[f].vertIndex.indexB][1]);

                        u_offset[2] = Convert.ToSingle(current_subobject.TextureCoordinateChannels[0][face[f].vertIndex.indexC][0]);
                        v_offset[2] = Convert.ToSingle(current_subobject.TextureCoordinateChannels[0][face[f].vertIndex.indexC][1]);

                        // So we check the absolute values to find which is the least distance from the origin.
                        // Whether we decide to go positive or negative from that position is fine but we want to start as close as we can to the origin.
                        // When we actually store the value we do not use an absolute and maintain the positive/negative sign of the value.

                        if (Math.Abs(u_offset[0]) < Math.Abs(u_offset[1]))
                        {
                            if (Math.Abs(u_offset[0]) < Math.Abs(u_offset[2]))
                            {
                                u_base = u_offset[0];
                                v_base = v_offset[0];
                            }
                            else
                            {
                                u_base = u_offset[2];
                                v_base = v_offset[2];
                            }
                        }
                        else
                        {
                            if (Math.Abs(u_offset[1]) < Math.Abs(u_offset[2]))
                            {
                                u_base = u_offset[1];
                                v_base = v_offset[1];
                            }
                            else
                            {
                                u_base = u_offset[2];
                                v_base = v_offset[2];
                            }
                        }



                        // Set the shift values for each u/v offset
                        u_shift[0] = u_offset[0] - u_base;
                        u_shift[1] = u_offset[1] - u_base;
                        u_shift[2] = u_offset[2] - u_base;

                        v_shift[0] = v_offset[0] - v_base;
                        v_shift[1] = v_offset[1] - v_base;
                        v_shift[2] = v_offset[2] - v_base;


                        //Now apply a modulus operation to get the u/v_base as a decimal only, removing the whole value and any inherited tiling.

                        u_base = u_base % 1.0f;
                        v_base = v_base % 1.0f;

                        // And now add the offsets to the base to get each vert's actual U/V coordinate, before converting to ST.



                        u_offset[0] = u_base + u_shift[0];
                        u_offset[1] = u_base + u_shift[1];
                        u_offset[2] = u_base + u_shift[2];

                        v_offset[0] = v_base + v_shift[0];
                        v_offset[1] = v_base + v_shift[1];
                        v_offset[2] = v_base + v_shift[2];

                        // and now apply the calculation to make them into ST coords for Mario Kart.


                        //

                        int s_coord = 0;
                        int t_coord = 0;

                        s_coord = Convert.ToInt32(u_offset[0] * STwidth[textureObject[materialID].textureClass] * 32);
                        t_coord = Convert.ToInt32(v_offset[0] * STheight[textureObject[materialID].textureClass] * -32);


                        if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                        {
                            MessageBox.Show("FATAL ERROR! " + u_offset[0].ToString() + "-" + v_offset[0].ToString() + " - UV 0 Out of Range for Object - " + cObj.objectName);
                        }
                        vert[face[f].vertIndex.indexA].position.s = Convert.ToInt16(s_coord);
                        vert[face[f].vertIndex.indexA].position.t = Convert.ToInt16(t_coord);


                        //


                        s_coord = Convert.ToInt32(u_offset[1] * STwidth[textureObject[materialID].textureClass] * 32);
                        t_coord = Convert.ToInt32(v_offset[1] * STheight[textureObject[materialID].textureClass] * -32);


                        if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                        {
                            MessageBox.Show("FATAL ERROR! " + u_offset[1].ToString() + "-" + v_offset[1].ToString() + " UV 1 Out of Range for Object - " + cObj.objectName);
                        }

                        vert[face[f].vertIndex.indexB].position.s = Convert.ToInt16(s_coord);
                        vert[face[f].vertIndex.indexB].position.t = Convert.ToInt16(t_coord);


                        //


                        s_coord = Convert.ToInt32(u_offset[2] * STwidth[textureObject[materialID].textureClass] * 32);
                        t_coord = Convert.ToInt32(v_offset[2] * STheight[textureObject[materialID].textureClass] * -32);



                        if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                        {
                            MessageBox.Show("FATAL ERROR! "+u_offset[2].ToString() + "-" + v_offset[2].ToString() + " UV 2 Out of Range for Object - " + cObj.objectName);
                                
                        }


                        vert[face[f].vertIndex.indexC].position.s = Convert.ToInt16(s_coord);
                        vert[face[f].vertIndex.indexC].position.t = Convert.ToInt16(t_coord);


                        //



                        //finish normalizing








                    }



                    ///Ok so now that we've loaded the raw model data, let's start writing some F3DEX. God have mercy.

                    cObj.meshPosition[subIndex] = Convert.ToInt32(seg7m.Position);


                    byteArray = BitConverter.GetBytes(0xBB000001);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0xFFFFFFFF);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);


                    ImgType = ImgTypes[textureObject[cObj.materialID].textureClass];
                    ImgFlag1 = STheight[textureObject[cObj.materialID].textureClass];
                    ImgFlag2 = STwidth[textureObject[cObj.materialID].textureClass];
                    if (textureObject[cObj.materialID].textureClass == 6)
                    {
                        ImgFlag3 = 0x100;
                    }
                    else
                    {
                        ImgFlag3 = 0x00;
                    }



                    byteArray = BitConverter.GetBytes(0xE8000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);



                    byteArray = BitConverter.GetBytes((((ImgType << 0x15) | 0xF5100000) | ((((ImgFlag2 << 1) + 7) >> 3) << 9)) | ImgFlag3);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(Convert.ToInt32(((heightex[textureObject[materialID].textureClass] & 0xF) << 0x12) | (((heightex[textureObject[materialID].textureClass] & 0xF0) >> 4) << 0xE) | ((widthex[textureObject[materialID].textureClass] & 0xF) << 8) | (((widthex[textureObject[materialID].textureClass] & 0xF0) >> 4) << 4)));

                    //IDK why but this makes it into what it's supposed to be. 
                    byteArray = BitConverter.GetBytes(BitConverter.ToInt32(byteArray, 0) >> 4);
                    //IDK why but this makes it into what it's supposed to be. 



                    Array.Reverse(byteArray);

                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0xF2000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes((((ImgFlag2 - 1) << 0xE) | ((ImgFlag1 - 1) << 2)));
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0xFD100000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x05000000 | textureObject[materialID].segmentPosition);  //told you we would need this later. you didn't believe me </3 :'(
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0xE8000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    ///F5100000 07000000 E6000000 00000000 F3000000 073FF100

                    byteArray = BitConverter.GetBytes(0xF5100000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x07000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0xE6000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0xF3000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    if (textureObject[materialID].textureClass == 0)
                    {
                        byteArray = BitConverter.GetBytes(0x073FF100);
                        Array.Reverse(byteArray);
                        seg7w.Write(byteArray);
                    }
                    else if (textureObject[materialID].textureClass == 1)
                    {
                        byteArray = BitConverter.GetBytes(0x077FF080);
                        Array.Reverse(byteArray);
                        seg7w.Write(byteArray);

                    }
                    else if (textureObject[materialID].textureClass == 2)
                    {
                        byteArray = BitConverter.GetBytes(0x077FF100);
                        Array.Reverse(byteArray);
                        seg7w.Write(byteArray);
                    }



                    ///load the first set of verts from the relativeZero position;

                    byteArray = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);


                    byteArray = BitConverter.GetBytes(0x04000000 | relativeZero * 16);  ///from segment 4 at offset relativeZero
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);



                    relativeIndex = 0;

                    for (int x = 0; x < facecount;)
                    {



                        int v0 = 0;
                        int v1 = 0;
                        int v2 = 0;


                        if (x + 2 <= facecount)
                        {
                            /// draw 2 triangles, check for additional verts in both.
                            if (face[x].vertIndex.indexA > (relativeIndex + 31) | face[x].vertIndex.indexB > (relativeIndex + 31) | face[x].vertIndex.indexC > (relativeIndex + 31) | /// OR with next line
                                                face[x + 1].vertIndex.indexA > (relativeIndex + 31) | face[x + 1].vertIndex.indexB > (relativeIndex + 31) | face[x + 1].vertIndex.indexC > (relativeIndex + 31))
                            {

                                /// OVER VERT LIMIT, LOAD NEW VERTS
                                UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertIndex.indexA, face[x].vertIndex.indexB));
                                minvalue = Convert.ToUInt16(GetMin(face[x].vertIndex.indexC, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertIndex.indexA, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertIndex.indexB, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertIndex.indexC, minvalue));


                                if (minvalue - 4 < 0)
                                {
                                    minvalue = 0;
                                }
                                else
                                {
                                    minvalue = Convert.ToUInt16(minvalue - 4);
                                }
                                byteArray = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);


                                byteArray = BitConverter.GetBytes(0x04000000 | ((relativeZero + minvalue) * 16));  ///from segment 4 at offset relativeZero
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);

                                relativeIndex = minvalue;
                            }

                            if ((face[x].vertIndex.indexA < relativeIndex) | (face[x].vertIndex.indexB < relativeIndex) | (face[x].vertIndex.indexC < relativeIndex) |
                                (face[x + 1].vertIndex.indexA < relativeIndex) | (face[x + 1].vertIndex.indexB < relativeIndex) | (face[x + 1].vertIndex.indexC < relativeIndex))
                            {

                                /// UNDER VERT LIMIT, LOAD NEW VERTS
                                UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertIndex.indexA, face[x].vertIndex.indexB));
                                minvalue = Convert.ToUInt16(GetMin(face[x].vertIndex.indexC, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertIndex.indexA, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertIndex.indexB, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertIndex.indexC, minvalue));

                                if (minvalue - 4 < 0)
                                {
                                    minvalue = 0;
                                }
                                else
                                {
                                    minvalue = Convert.ToUInt16(minvalue - 4);
                                }

                                byteArray = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);


                                byteArray = BitConverter.GetBytes(0x04000000 | ((relativeZero + minvalue) * 16));  ///from segment 4 at offset relativeZero
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);

                                relativeIndex = minvalue;
                            }


                            ///end vert check



                            v0 = face[x].vertIndex.indexA - relativeIndex;
                            v1 = face[x].vertIndex.indexB - relativeIndex;
                            v2 = face[x].vertIndex.indexC - relativeIndex;


                            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0xB1000000 | (v2 << 17) | (v1 << 9) | v0 << 1));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);

                            v0 = face[x + 1].vertIndex.indexA - relativeIndex;
                            v1 = face[x + 1].vertIndex.indexB - relativeIndex;
                            v2 = face[x + 1].vertIndex.indexC - relativeIndex;

                            byteArray = BitConverter.GetBytes(Convert.ToUInt32((v2 << 17) | (v1 << 9) | v0 << 1));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);
                            x = x + 2;

                        }
                        else
                        {
                            /// draw 1 triangle, only 1 vert check

                            if (face[x].vertIndex.indexA > (relativeIndex + 31) | face[x].vertIndex.indexB > (relativeIndex + 31) | face[x].vertIndex.indexC > (relativeIndex + 31))
                            {

                                /// OVER VERT LIMIT, LOAD NEW VERTS
                                UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertIndex.indexA, face[x].vertIndex.indexB));
                                minvalue = Convert.ToUInt16(GetMin(face[x].vertIndex.indexC, minvalue));


                                if (minvalue - 4 < 0)
                                {
                                    minvalue = 0;
                                }
                                else
                                {
                                    minvalue = Convert.ToUInt16(minvalue - 4);
                                }
                                byteArray = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);


                                byteArray = BitConverter.GetBytes(0x04000000 | ((relativeZero + minvalue) * 16));  ///from segment 4 at offset relativeZero
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);

                                relativeIndex = minvalue;
                            }

                            if ((face[x].vertIndex.indexA < relativeIndex) | (face[x].vertIndex.indexB < relativeIndex) | (face[x].vertIndex.indexC < relativeIndex))
                            {

                                /// UNDER VERT LIMIT, LOAD NEW VERTS
                                UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertIndex.indexA, face[x].vertIndex.indexB));
                                minvalue = Convert.ToUInt16(GetMin(face[x].vertIndex.indexC, minvalue));

                                if (minvalue - 4 < 0)
                                {
                                    minvalue = 0;
                                }
                                else
                                {
                                    minvalue = Convert.ToUInt16(minvalue - 4);
                                }

                                byteArray = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);


                                byteArray = BitConverter.GetBytes(0x04000000 | ((relativeZero + minvalue) * 16));  ///from segment 4 at offset relativeZero
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);

                                relativeIndex = minvalue;
                            }





                            ///end vert check
                            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0xBF000000));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);

                            v0 = face[x].vertIndex.indexA - relativeIndex;
                            v1 = face[x].vertIndex.indexB - relativeIndex;
                            v2 = face[x].vertIndex.indexC - relativeIndex;


                            byteArray = BitConverter.GetBytes(Convert.ToUInt32((v2 << 17) | (v1 << 9) | v0 << 1));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);
                            x = x + 1;
                        }


                    }


                    byteArray = BitConverter.GetBytes(0xB8000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);
                    //create the vert array for the current subobject, write it to Segment 4. 
                    for (int v = 0; v < vert.Length; v++)
                    {
                        byteArray = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.x));
                        Array.Reverse(byteArray);
                        seg4w.Write(byteArray);

                        byteArray = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.z));
                        Array.Reverse(byteArray);
                        seg4w.Write(byteArray);

                        byteArray = BitConverter.GetBytes(Convert.ToInt16(-1 * vert[v].position.y));
                        Array.Reverse(byteArray);
                        seg4w.Write(byteArray);
                        

                        byteArray = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.s));
                        Array.Reverse(byteArray);
                        seg4w.Write(byteArray);

                        byteArray = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.t));
                        Array.Reverse(byteArray);
                        seg4w.Write(byteArray);

                        byte RGB = 252;
                        byte Alpha = 0;
                        seg4w.Write(RGB);
                        seg4w.Write(RGB);
                        seg4w.Write(RGB);
                        seg4w.Write(Alpha);
                    }
                    relativeZero = Convert.ToUInt16(relativeZero + vert.Length);


                }
                

               


            }
            outseg4 = seg4m.ToArray();
            outseg7 = seg7m.ToArray();

            outMagic = relativeZero;
        }

        public byte[] compileF3DList(ref TM64_Geometry.OK64SectionList[] sectionOut, Assimp.Scene fbx, TM64_Geometry.OK64F3DObject[] courseObject, TM64_Geometry.OK64SectionList[] sectionList)
        {
            //this function will create display lists for each of the section views based on the OK64F3DObject array.
            //this array had been previously written to segment 7 and the offsets to each of those objects' meshes...
            // were stored into courseObject[index].meshPosition[] for this process.


            //magic is the offset of the data preceding this in the segment based on the current organization method,
            

            MemoryStream seg6m = new MemoryStream();
            BinaryReader seg6r = new BinaryReader(seg6m);
            BinaryWriter seg6w = new BinaryWriter(seg6m);

            byte[] byteArray = new byte[0];


            for (int currentSection = 0; currentSection < sectionList.Length; currentSection++)
            {
                for (int currentView = 0; currentView < 4; currentView++)
                {

                    int objectCount = sectionList[currentSection].viewList[currentView].objectList.Length;
                    sectionList[currentSection].viewList[currentView].segmentPosition = Convert.ToInt32(seg6m.Position);
                    for (int currentObject = 0; currentObject < objectCount; currentObject++)
                    {

                        int objectIndex = sectionList[currentSection].viewList[currentView].objectList[currentObject];

                        for (int subObject = 0; subObject < courseObject[objectIndex].meshPosition.Length; subObject++)
                        {
                            byteArray = BitConverter.GetBytes(0x06000000);
                            Array.Reverse(byteArray);
                            seg6w.Write(byteArray);

                            byteArray = BitConverter.GetBytes(courseObject[objectIndex].meshPosition[subObject]| 0x07000000);
                            Array.Reverse(byteArray);
                            seg6w.Write(byteArray);
                        }
                    }

                    byteArray = BitConverter.GetBytes(0xB8000000);
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00000000);
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);

                }
            }
            sectionOut = sectionList;
            return seg6m.ToArray();
        }

        public byte[] compilesurfaceTable(TM64_Geometry.OK64F3DObject[] surfaceObject)
        {
            MemoryStream seg6m = new MemoryStream();
            BinaryReader seg6r = new BinaryReader(seg6m);
            BinaryWriter seg6w = new BinaryWriter(seg6m);

            byte[] byteArray = new byte[0];
            byte singleByte = new byte();

            int objectCount = surfaceObject.Length;


            for (int currentObject = 0; currentObject < objectCount; currentObject++)
            {
                for (int subObject = 0; subObject < surfaceObject[currentObject].meshPosition.Length; subObject++)
                {
                    if (subObject > 0)
                        MessageBox.Show("FATAL ERROR! Object with more than 1 Material: "+surfaceObject[currentObject].objectName);


                    byteArray = BitConverter.GetBytes(surfaceObject[currentObject].meshPosition[subObject] | 0x07000000);
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);

                    singleByte = Convert.ToByte(surfaceObject[currentObject].surfaceMaterial);
                    seg6w.Write(singleByte);

                    singleByte = Convert.ToByte(surfaceObject[currentObject].surfaceID);
                    seg6w.Write(singleByte);

                    byteArray = BitConverter.GetBytes(Convert.ToInt16(0));  //flag data currently hardset to 0.
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);
                }
            }

            byteArray = BitConverter.GetBytes(Convert.ToInt32(0)); 
            Array.Reverse(byteArray);
            seg6w.Write(byteArray);
            byteArray = BitConverter.GetBytes(Convert.ToInt32(0)); 
            Array.Reverse(byteArray);
            seg6w.Write(byteArray);

            byte[] seg6 = seg6m.ToArray();
            return seg6;
        }

        public byte[] compilesectionviewTable(TM64_Geometry.OK64SectionList[] sectionList, int magic)
        {
            MemoryStream seg6m = new MemoryStream();
            BinaryReader seg6r = new BinaryReader(seg6m);
            BinaryWriter seg6w = new BinaryWriter(seg6m);

            byte[] byteArray = new byte[0];
            byte singleByte = new byte();


            for (int currentSection = 0; currentSection < sectionList.Length; currentSection++)
            {
                for (int currentView = 0; currentView < 4; currentView++)
                {
                    byteArray = BitConverter.GetBytes(0x06000000 | (sectionList[currentSection].viewList[currentView].segmentPosition + magic));
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);
                }

            }
            int bufferGap = 33 - sectionList.Length;
            for (int currentSection = 0; currentSection < bufferGap; currentSection++)
            {
                for (int currentView = 0; currentView < 4; currentView++)
                {
                    byteArray = BitConverter.GetBytes(Convert.ToInt32(0));
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);
                }

            }
            byte[] seg6 = seg6m.ToArray();
            return seg6;
        }
    }

}

// phew! We made it. I keep saying we, but it's me doing all the work!
// maybe try pitching in sometime and updating the program! I'd love the help!

// Thank you so much for-a reading my source!

// OverKart 64 Library
// For Mario Kart 64 1.0 USA ROM
// <3 Hamp