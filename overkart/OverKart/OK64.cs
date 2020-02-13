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
using AssimpSharp;
using Texture64;
using System.Drawing.Imaging;

namespace OverKart64
{
    public class OK64
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


        public class OK64SectionList
        {
            public OK64ViewList[] viewList { get; set; }
        }

        public class OK64ViewList
        {
            public int[] objectList { get; set; }
            public int segmentPosition { get; set; }
        }




        public class OK64Texture
        {
            public string textureName { get; set; }
            public string texturePath { get; set; }
            public int textureWidth { get; set; }
            public int textureHeight { get; set; }
            public int textureFormat { get; set; }
            public int textureClass { get; set; }
            public byte[] compressedTexture { get; set; }
            public int compressedSize { get; set; }
            public int fileSize { get; set; }
            public int segmentPosition { get; set; }
            public int romPosition { get; set; }

        }





        public class OK64F3DObject
        {
            public string objectName { get; set; }
            public int vertCount { get; set; }
            public int faceCount { get; set; }
            public int materialID { get; set; }
            public int surfaceID { get; set; }
            public int surfaceMaterial { get; set; }
            public int[] meshID { get; set; }
            public int[] meshPosition { get; set; }
            public bool flagA { get; set; }
            public bool flagB { get; set; }
            public bool flagC { get; set; }
        }


        public class Face
        {
            public VertIndex vertindex { get; set; }
            public int material { get; set; }
        }

        public class VertIndex
        {
            public int v0 { get; set; }
            public int v1 { get; set; }
            public int v2 { get; set; }

        }

        public class Vertex
        {
            public Position position { get; set; }
            public Color color { get; set; }
        }

        public class Color
        {
            public Byte r { get; set; }
            public Byte g { get; set; }
            public Byte b { get; set; }
            public Byte a { get; set; }
        }

        public class Position
        {

            public Int16 x { get; set; }
            public Int16 y { get; set; }
            public Int16 z { get; set; }
            public Int16 s { get; set; }
            public Int16 t { get; set; }
            public float u { get; set; }
            public float v { get; set; }

        }

        ///




        /// These are used by the SectionView (.OK64.SVL) Parser
        /// replace these with OK64SectionList TO DO
        public class SectionView
        {
            public ViewList[] viewlist { get; set; }
        }
        public class ViewList
        {
            public string[] objectlist { get; set; }
        }

        /// These are used for parsing through Path Markers and Object Markers

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
        public byte[] compileSegment(byte[] seg4, byte[] seg6, byte[] seg7, byte[] seg9, byte[] rombytes, int cID)
        {


            ///This takes precompiled segments and inserts them into the ROM file. It also updates the course header table to reflect
            /// the new data sizes. This allows for proper loading of the course so long as the segments are properly setup. All segment
            /// data should be precompressed where applicable, this assumes that segment 4 and segment 6 are MIO0 compressed and that
            /// Segment 7 has had it's special compression ran. Segment 9 has no compression. rombytes is the ROM file as a byte array, and CID
            /// is the ID of the course we're looking to replace based on it's location in the course header table. 


            /// This writes all segments to the end of the file for simplicity. If data was larger than original (which it almost always will be for custom courses)
            /// then it cannot fit in the existing space without overwriting other course data. 

            OK64 mk = new OK64();
            bs = new MemoryStream();
            bs.Write(rombytes, 0, rombytes.Length);
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

            for (int align =0; align < addressAlign; align++)
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

            List<byte> vertbyte = decompress_MIO0(0, seg4);
            UInt32 vertcount = Convert.ToUInt32(vertbyte.Count / 14);

            flip = BitConverter.GetBytes(vertcount);
            Array.Reverse(flip);
            vertcount = BitConverter.ToUInt32(flip, 0);
            ///MessageBox.Show(vertbyte.Count.ToString() + "--" + vertcount.ToString());


            ///seg7 size

            byte[] seg7byte = decompress_seg7(seg7);
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

        public byte[] compileHotswap(byte[] seg4, byte[] seg6, byte[] seg7, byte[] seg9, string courseName, string previewImage, string bannerImage, string customASM, byte[]skyColor, byte[] rombytes, int cID, int setID)
        {


            ///This takes precompiled segments and inserts them into the ROM file. It also updates the course header table to reflect
            /// the new data sizes. This allows for proper loading of the course so long as the segments are properly setup. All segment
            /// data should be precompressed where applicable, this assumes that segment 4 and segment 6 are MIO0 compressed and that
            /// Segment 7 has had it's special compression ran. Segment 9 has no compression. rombytes is the ROM file as a byte array, and CID
            /// is the ID of the course we're looking to replace based on it's location in the course header table. 


            /// This writes all segments to the end of the file for simplicity. If data was larger than original (which it almost always will be for custom courses)
            /// then it cannot fit in the existing space without overwriting other course data. 


            byte[] cseg4 = fakeCompress(seg4);
            byte[] cseg6 = fakeCompress(seg6);


            OK64 mk = new OK64();
            bs = new MemoryStream();
            
            bs.Write(rombytes, 0, rombytes.Length);
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

            int previewOffset = 0;
            int bannerOffset = 0;

            int asmOffset = 0;
            int asmLength = 0;



            bw.BaseStream.Position = bw.BaseStream.Length;

            addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                bw.Write(Convert.ToByte(0x00));
            }


            seg6start = Convert.ToUInt32(bw.BaseStream.Position);
            bw.Write(cseg6, 0, cseg6.Length);
            
            ///

            addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                bw.Write(Convert.ToByte(0x00));
            }
            seg6end = Convert.ToUInt32(bw.BaseStream.Position);




            seg9start = Convert.ToUInt32(bw.BaseStream.Position);
            bw.Write(seg9, 0, seg9.Length);
            
            ///

            addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                bw.Write(Convert.ToByte(0x00));
            }
            seg9end = Convert.ToUInt32(bw.BaseStream.Position);






            seg4start = Convert.ToUInt32(bw.BaseStream.Position);
            bw.Write(cseg4, 0, cseg4.Length);
            addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                bw.Write(Convert.ToByte(0x00));
            }

            seg7start = Convert.ToUInt32(bw.BaseStream.Position);
            bw.Write(seg7, 0, seg7.Length);


            addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
             {
                bw.Write(Convert.ToByte(0x00));
            }
            seg7end = Convert.ToUInt32(bw.BaseStream.Position);
            ///
            seg7rsp = Convert.ToUInt32(0x0F000000 | (seg7start - seg4start));







            // This writes the internal Course Header name and stores the offset to write later.
            // OverKart64 will use this in future tools to make it easier for users to identify custom maps.


            addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                bw.Write(Convert.ToByte(0x00));
            }




            int nameOffset = Convert.ToInt32(bw.BaseStream.Position);
            
            bw.Write(courseName);   //using a length-defined as opposed to null terminated setup.
                                    //easier to program for writing, easier to program for reading. 



            //finish Course Name


            
            //Write Course Preview Texture
            if (previewImage.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(previewImage);
                N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);


                addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;

                for (int align = 0; align < addressAlign; align++)
                {
                    bw.Write(Convert.ToByte(0x00));
                }




                previewOffset = Convert.ToInt32(bw.BaseStream.Position);
                bw.Write(imageData);



            }

            //Write Course Banner Texture
            if (bannerImage.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(bannerImage);
                N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);


                addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;

                for (int align = 0; align < addressAlign; align++)
                {
                    bw.Write(Convert.ToByte(0x00));
                }


                bannerOffset = Convert.ToInt32(bw.BaseStream.Position);
                bw.Write(imageData);


            }
            //finish writing course preview/banner textures




            //custom ASM


            if (customASM.Length > 0)
            {

                addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;

                for (int align = 0; align < addressAlign; align++)
                {
                    bw.Write(Convert.ToByte(0x00));
                }

                asmOffset = Convert.ToInt32(bw.BaseStream.Position);

                byte[] asmSequence = File.ReadAllBytes(customASM);

                asmLength = asmSequence.Length;

                bw.Write(asmSequence);
            }
            //end of ASM sequence

            //add sky colors 


            addressAlign = 4 - (Convert.ToInt32(bw.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                bw.Write(Convert.ToByte(0x00));
            }

            int skyOffset = Convert.ToInt32(bw.BaseStream.Position);

            bw.Write(skyColor[0]);
            bw.Write(skyColor[1]);
            bw.Write(skyColor[2]);
            bw.Write(Convert.ToByte(0x00));
            bw.Write(skyColor[3]);
            bw.Write(skyColor[4]);
            bw.Write(skyColor[5]);
            bw.Write(Convert.ToByte(0x00));



            ///
            //start of course Header Table entry
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

            byte[] seg7byte = decompress_seg7(seg7);
            UInt32 seg7size = Convert.ToUInt32(seg7byte.Length);


            ///MessageBox.Show(seg7size.ToString());

            flip = BitConverter.GetBytes(seg7size);
            Array.Reverse(flip);
            seg7size = BitConverter.ToUInt32(flip, 0);

            ///MessageBox.Show(seg7size.ToString());

            bw.BaseStream.Seek(0xBEA700 + (setID * 0x3C0) + (cID * 0x30), SeekOrigin.Begin);

            bw.Write(seg6start);
            bw.Write(seg6end);
            bw.Write(seg4start);
            bw.Write(seg7end);
            bw.Write(seg9start);
            bw.Write(seg9end);

            flip = BitConverter.GetBytes(0x0F000000);
            Array.Reverse(flip);
            bw.Write(flip);

            bw.Write(vertcount);

            bw.Write(seg7rsp);


            bw.Write(seg7size);

            flip = BitConverter.GetBytes(0x09000000);
            Array.Reverse(flip);
            bw.Write(flip);

            flip = BitConverter.GetBytes(0x00000000);
            Array.Reverse(flip);
            bw.Write(flip);

            //
            //end of course Header Table entry
            //




            //Write course name offset
            bw.BaseStream.Seek(0xBEB300 + (setID * 0x50) + (cID * 0x4), SeekOrigin.Begin);
            flip = BitConverter.GetBytes(nameOffset);
            Array.Reverse(flip);
            bw.Write(flip);


            //Write preview image offset
            bw.BaseStream.Seek(0xBEB400 + (setID * 0x50) + (cID * 0x4), SeekOrigin.Begin);
            flip = BitConverter.GetBytes(previewOffset);
            Array.Reverse(flip);
            bw.Write(flip);

            //Write banner image offset
            bw.BaseStream.Seek(0xBEB500 + (setID * 0x50) + (cID * 0x4), SeekOrigin.Begin);
            flip = BitConverter.GetBytes(bannerOffset);
            Array.Reverse(flip);
            bw.Write(flip);

            //Write ASM offset
            bw.BaseStream.Seek(0xBEB600 + (setID * 0x50) + (cID * 0x4), SeekOrigin.Begin);
            flip = BitConverter.GetBytes(asmOffset);
            Array.Reverse(flip);
            bw.Write(flip);

            //Write ASM length
            bw.BaseStream.Seek(0xBEB700 + (setID * 0x50) + (cID * 0x4), SeekOrigin.Begin);
            flip = BitConverter.GetBytes(asmLength);
            Array.Reverse(flip);
            bw.Write(flip);


            //Write sky color offset
            bw.BaseStream.Seek(0xBEB800 + (setID * 0x50) + (cID * 0x4), SeekOrigin.Begin);
            flip = BitConverter.GetBytes(skyOffset);
            Array.Reverse(flip);
            bw.Write(flip);


            byte[] newROM = bs.ToArray();
            return newROM;

        }
        public byte[] dumpseg4(int cID, byte[] rombytes)
        {

            bs = new MemoryStream(rombytes);
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


            Buffer.BlockCopy(rombytes, Convert.ToInt32(seg4_addr[cID]), seg4, 0, (Convert.ToInt32(seg7_romptr[cID]) - Convert.ToInt32(seg4_addr[cID])));

            return seg4;
        }

        public byte[] dumpseg5(int cID, byte[] rombytes)
        {

            bs = new MemoryStream(rombytes);
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

            Buffer.BlockCopy(rombytes, Convert.ToInt32(seg9_addr[cID]), seg9, 0, (Convert.ToInt32(seg9_end[cID]) - Convert.ToInt32(seg9_addr[cID])));

            OK64 mk = new OK64();

            bs = new MemoryStream(seg9);

            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);
            int moffset = new int();

            br.BaseStream.Position = (texture_addr[cID] - 0x09000000);

            for (int i = 0; br.BaseStream.Position < br.BaseStream.Length; i++)
            {
                flip4 = br.ReadBytes(4);
                Array.Reverse(flip4);
                moffset = BitConverter.ToInt32(flip4, 0);
                if (moffset != 0)
                {
                    moffset = (moffset & 0xFFFFFF) + 0x641F70;

                    List<byte> texturelist = decompress_MIO0(moffset, rombytes);
                    offsets.Add(segment5.Count);
                    segment5.AddRange(texturelist);
                    br.BaseStream.Seek(12, SeekOrigin.Current);
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




        public byte[] dumpseg6(int cID, byte[] rombytes)
        {

            bs = new MemoryStream(rombytes);
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


            Buffer.BlockCopy(rombytes, Convert.ToInt32(seg6_addr[cID]), seg6, 0, (Convert.ToInt32(seg6_end[cID]) - Convert.ToInt32(seg6_addr[cID])));

            return seg6;
        }

        public byte[] dumpseg7(int cID, byte[] rombytes)
        {

            bs = new MemoryStream(rombytes);
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


            Buffer.BlockCopy(rombytes, Convert.ToInt32(seg7_romptr[cID]), seg7, 0, (Convert.ToInt32(seg7_end[cID]) - Convert.ToInt32(seg7_romptr[cID])));

            return seg7;
        }

        public byte[] dumpseg9(int cID, byte[] rombytes)
        {

            bs = new MemoryStream(rombytes);
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


            Buffer.BlockCopy(rombytes, Convert.ToInt32(seg9_addr[cID]), seg9, 0, (Convert.ToInt32(seg9_end[cID]) - Convert.ToInt32(seg9_addr[cID])));

            return seg9;
        }

        public byte[] dump_ASM(string filePath)
        {

            /// This is specfically designed for Mario Kart 64 USA 1.0 ROM. It should dump to binary the majority of it's ASM commands.
            /// It uses a list provided by MiB to find the ASM sections, there could be plenty of code I'm missing

            byte[] rombytes = File.ReadAllBytes(filePath);
            byte[] asm = new byte[1081936];

            byte[] buffer = new byte[1];
            buffer[0] = 0xFF;


            Buffer.BlockCopy(rombytes, 4096, asm, 0, 887664);

            for (int i = 0; i < 8; i++)
            {
                Buffer.BlockCopy(buffer, 0, asm, 887664 + i, 1);
            }

            Buffer.BlockCopy(rombytes, 1013008, asm, 887672, 174224);

            for (int i = 0; i < 8; i++)
            {
                Buffer.BlockCopy(buffer, 0, asm, 1061896 + i, 1);
            }

            Buffer.BlockCopy(rombytes, 1193536, asm, 1061904, 20032);
            return asm;

        }

        public void translate_ASM(string savePath, string filePath)
        {

            /// This is specfically designed for Mario Kart 64 USA 1.0 ROM. It should convert to plaintext the majority of it's ASM commands.            
            /// Also, there are a few ASM commands that MK64 uses that I currently haven't defined yet. 



            byte[] asm = dump_ASM(filePath);



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


            long current_offset = 0;

            for (int i = 0; i < asm.Length; i += 4)
            {


                asmr.BaseStream.Seek(current_offset, SeekOrigin.Begin);
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
                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "-Unknown Command -" + compare.ToString() + "-  @0x" + current_offset.ToString("X").PadLeft(2, '0');
                }

                current_offset += 4;
                asmr.BaseStream.Seek(current_offset, SeekOrigin.Begin);


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





        /// https:///github.com/Daniel-McCarthy/Mr-Peeps-Compressor 

        public int[] findAllMatches(ref List<byte> dictionary, byte match)
        {
            List<int> matchPositons = new List<int>();

            for (int i = 0; i < dictionary.Count; i++)
            {
                if (dictionary[i] == match)
                {
                    matchPositons.Add(i);
                }
            }

            return matchPositons.ToArray();
        }

        public int[] findLargestMatch(ref List<byte> dictionary, int[] matchesFound, ref byte[] file, int fileIndex, int maxMatch)
        {
            int[] matchSizes = new int[matchesFound.Length];

            for (int i = 0; i < matchesFound.Length; i++)
            {
                int matchSize = 1;
                bool matchFound = true;

                while (matchFound && matchSize < maxMatch && (fileIndex + matchSize < file.Length) && (matchesFound[i] + matchSize < dictionary.Count)) ///NOTE: This could be relevant to compression issues? I suspect it's more related to writing
                {
                    if (file[fileIndex + matchSize] == dictionary[matchesFound[i] + matchSize])
                    {
                        matchSize++;
                    }
                    else
                    {
                        matchFound = false;
                    }

                }

                matchSizes[i] = matchSize;
            }

            int[] bestMatch = new int[2];

            bestMatch[0] = matchesFound[0];
            bestMatch[1] = matchSizes[0];

            for (int i = 1; i < matchesFound.Length; i++)
            {
                if (matchSizes[i] > bestMatch[1])
                {
                    bestMatch[0] = matchesFound[i];
                    bestMatch[1] = matchSizes[i];
                }
            }

            return bestMatch;

        }

        public List<byte> decompress_MIO0(int offset, string path)
        {

            /// This is Peep's Decompression Algorithim for MIO0 decompression. 
            /// It's pretty much taken verbatim with a couple adjustments to variable names.
            /// Thanks.

            FileStream inputFile = File.Open(path, FileMode.Open);
            BigEndianBinaryReader mio0r = new BigEndianBinaryReader(inputFile);

            byte[] file = mio0r.ReadBytes((int)inputFile.Length);


            List<byte> newFile = new List<byte>();


            mio0r.BaseStream.Position = offset;
            string magicNumber = Encoding.ASCII.GetString(mio0r.ReadBytes(4));

            if (magicNumber == "MIO0")
            {
                int decompressedLength = mio0r.ReadInt32();
                int compressedOffset = mio0r.ReadInt32() + offset;
                int uncompressedOffset = mio0r.ReadInt32() + offset;
                int currentOffset;

                try
                {

                    while (newFile.Count < decompressedLength)
                    {

                        byte bits = mio0r.ReadByte(); ///byte of layout bits
                        BitArray arrayOfBits = new BitArray(new byte[1] { bits });

                        for (int i = 7; i > -1 && (newFile.Count < decompressedLength); i--) ///iterate through layout bits
                        {

                            if (arrayOfBits[i] == true)
                            {
                                ///non-compressed
                                ///add one byte from uncompressedOffset to newFile

                                currentOffset = (int)inputFile.Position;

                                inputFile.Seek(uncompressedOffset, SeekOrigin.Begin);

                                newFile.Add(mio0r.ReadByte());
                                uncompressedOffset++;

                                inputFile.Seek(currentOffset, SeekOrigin.Begin);

                            }
                            else
                            {
                                ///compressed
                                ///read 2 bytes
                                ///4 bits = length
                                ///12 bits = offset

                                currentOffset = (int)inputFile.Position;
                                inputFile.Seek(compressedOffset, SeekOrigin.Begin);

                                byte byte1 = mio0r.ReadByte();
                                byte byte2 = mio0r.ReadByte();
                                compressedOffset += 2;

                                ///Note: For Debugging, binary representations can be printed with:  Convert.ToString(numberVariable, 2);

                                byte byte1Upper = (byte)((byte1 & 0x0F));///offset bits
                                byte byte1Lower = (byte)((byte1 & 0xF0) >> 4); ///length bits

                                int combinedOffset = ((byte1Upper << 8) | byte2);

                                int finalOffset = 1 + combinedOffset;
                                int finalLength = 3 + byte1Lower;

                                for (int k = 0; k < finalLength; k++) ///add data for finalLength iterations
                                {
                                    newFile.Add(newFile[newFile.Count - finalOffset]); ///add byte at offset (fileSize - finalOffset) to file
                                }

                                inputFile.Seek(currentOffset, SeekOrigin.Begin); ///return to layout bits

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

        public List<byte> decompress_MIO0(int offset, byte[] file)
        {

            /// This is Peep's Decompression Algorithim for MIO0 decompression. 
            /// It's pretty much taken verbatim with a couple adjustments to variable names.
            /// Thanks.


            MemoryStream inputFile = new MemoryStream(file);
            BigEndianBinaryReader mio0r = new BigEndianBinaryReader(inputFile);

            List<byte> newFile = new List<byte>();


            mio0r.BaseStream.Position = offset;
            string magicNumber = Encoding.ASCII.GetString(mio0r.ReadBytes(4));

            if (magicNumber == "MIO0")
            {
                int decompressedLength = mio0r.ReadInt32();
                int compressedOffset = mio0r.ReadInt32() + offset;
                int uncompressedOffset = mio0r.ReadInt32() + offset;
                int currentOffset;

                try
                {

                    while (newFile.Count < decompressedLength)
                    {

                        byte bits = mio0r.ReadByte(); ///byte of layout bits
                        BitArray arrayOfBits = new BitArray(new byte[1] { bits });

                        for (int i = 7; i > -1 && (newFile.Count < decompressedLength); i--) ///iterate through layout bits
                        {

                            if (arrayOfBits[i] == true)
                            {
                                ///non-compressed
                                ///add one byte from uncompressedOffset to newFile

                                currentOffset = (int)inputFile.Position;

                                inputFile.Seek(uncompressedOffset, SeekOrigin.Begin);

                                newFile.Add(mio0r.ReadByte());
                                uncompressedOffset++;

                                inputFile.Seek(currentOffset, SeekOrigin.Begin);

                            }
                            else
                            {
                                ///compressed
                                ///read 2 bytes
                                ///4 bits = length
                                ///12 bits = offset

                                currentOffset = (int)inputFile.Position;
                                inputFile.Seek(compressedOffset, SeekOrigin.Begin);

                                byte byte1 = mio0r.ReadByte();
                                byte byte2 = mio0r.ReadByte();
                                compressedOffset += 2;

                                ///Note: For Debugging, binary representations can be printed with:  Convert.ToString(numberVariable, 2);

                                byte byte1Upper = (byte)((byte1 & 0x0F));///offset bits
                                byte byte1Lower = (byte)((byte1 & 0xF0) >> 4); ///length bits

                                int combinedOffset = ((byte1Upper << 8) | byte2);

                                int finalOffset = 1 + combinedOffset;
                                int finalLength = 3 + byte1Lower;

                                for (int k = 0; k < finalLength; k++) ///add data for finalLength iterations
                                {
                                    newFile.Add(newFile[newFile.Count - finalOffset]); ///add byte at offset (fileSize - finalOffset) to file
                                }

                                inputFile.Seek(currentOffset, SeekOrigin.Begin); ///return to layout bits

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

        public byte[] compressInitialization(string path, bool fileInputMode)
        {
            if (fileInputMode)
            {
                FileStream inputFile = File.Open(path, FileMode.Open);
                BinaryReader br = new BinaryReader(inputFile);
                byte[] file = br.ReadBytes((int)inputFile.Length);

                inputFile.Close();

                return compress_MIO0(file, 0);
            }
            else
            {
                byte[] stringToFile = Encoding.ASCII.GetBytes(path);

                return compress_MIO0(stringToFile, 0);
            }
        }

        public byte[] compress_MIO0(byte[] file, int offset)
        {
            List<byte> layoutBits = new List<byte>();
            List<byte> dictionary = new List<byte>();

            List<byte> uncompressedData = new List<byte>();
            List<int[]> compressedData = new List<int[]>();

            int maxDictionarySize = 4096;
            int maxMatchLength = 18;
            int minimumMatchSize = 2;
            int decompressedSize = 0;

            for (int i = 0; i < file.Length; i++)
            {
                if (dictionary.Contains(file[i]))
                {
                    ///check for best match
                    int[] matches = findAllMatches(ref dictionary, file[i]);
                    int[] bestMatch = findLargestMatch(ref dictionary, matches, ref file, i, maxMatchLength);

                    if (bestMatch[1] > minimumMatchSize)
                    {
                        ///add to compressedData
                        layoutBits.Add(0);
                        bestMatch[0] = dictionary.Count - bestMatch[0]; ///sets offset in relation to end of dictionary

                        for (int j = 0; j < bestMatch[1]; j++)
                        {
                            dictionary.Add(file[i + j]);
                        }

                        i = i + bestMatch[1] - 1;

                        compressedData.Add(bestMatch);
                        decompressedSize += bestMatch[1];
                    }
                    else
                    {
                        ///add to uncompressed data
                        layoutBits.Add(1);
                        uncompressedData.Add(file[i]);
                        dictionary.Add(file[i]);
                        decompressedSize++;
                    }
                }
                else
                {
                    ///uncompressed data
                    layoutBits.Add(1);
                    uncompressedData.Add(file[i]);
                    dictionary.Add(file[i]);
                    decompressedSize++;
                }

                if (dictionary.Count > maxDictionarySize)
                {
                    int overflow = dictionary.Count - maxDictionarySize;
                    dictionary.RemoveRange(0, overflow);
                }
            }

            return buildMIO0CompressedBlock(ref layoutBits, ref uncompressedData, ref compressedData, decompressedSize, offset);
        }

        public byte[] buildMIO0CompressedBlock(ref List<byte> layoutBits, ref List<byte> uncompressedData, ref List<int[]> offsetLengthPairs, int decompressedSize, int offset)
        {
            List<byte> finalMIO0Block = new List<byte>();           ///the final compressed file
            List<byte> layoutBytes = new List<byte>();              ///holds the layout bits in byte form
            List<byte> compressedDataBytes = new List<byte>();      ///holds length/offset in 2byte form

            int compressedOffset = 16 + offset; ///header size
            int uncompressedOffset;

            ///added magic number
            finalMIO0Block.AddRange(Encoding.ASCII.GetBytes("MIO0")); ///4 byte magic number

            ///add decompressed data size
            byte[] decompressedSizeArray = BitConverter.GetBytes(decompressedSize);
            Array.Reverse(decompressedSizeArray);
            finalMIO0Block.AddRange(decompressedSizeArray);         ///4 byte decompressed size

            ///assemble layout bits into bytes
            while (layoutBits.Count > 0)                            ///convert layout binary bits to bytes
            {
                ///pad bits to full byte if necessary
                while (layoutBits.Count < 8)                         ///pad last byte if necessary
                {
                    layoutBits.Add(0);
                }

                string layoutBitsString = layoutBits[0].ToString() + layoutBits[1].ToString() + layoutBits[2].ToString() + layoutBits[3].ToString()
                                        + layoutBits[4].ToString() + layoutBits[5].ToString() + layoutBits[6].ToString() + layoutBits[7].ToString();

                byte[] layoutByteArray = new byte[1];
                layoutByteArray[0] = Convert.ToByte(layoutBitsString, 2);
                layoutBytes.Add(layoutByteArray[0]);
                layoutBits.RemoveRange(0, (layoutBits.Count < 8) ? layoutBits.Count : 8);

            }


            foreach (int[] offsetLengthPair in offsetLengthPairs)
            {
                offsetLengthPair[0] -= 1;                           ///removes '1' that is added to offset on decompression
                offsetLengthPair[1] -= 3;                           ///removes '3' that is added to length on decompression

                ///combine offset and length into 16 bit block
                int compressedInt = (offsetLengthPair[1] << 12) | (offsetLengthPair[0]);

                ///split int16 into two bytes to be written
                byte[] compressed2Byte = new byte[2];
                compressed2Byte[0] = (byte)(compressedInt & 0xFF);
                compressed2Byte[1] = (byte)((compressedInt >> 8) & 0xFF);

                compressedDataBytes.Add(compressed2Byte[1]);        ///used to be 0 then 1, but this seems to be correct
                compressedDataBytes.Add(compressed2Byte[0]);

            }

            ///pad layout bits if needed
            while (layoutBytes.Count % 4 != 0)
            {
                layoutBytes.Add(0);
            }

            compressedOffset += layoutBytes.Count;

            ///add final compressed offset
            byte[] compressedOffsetArray = BitConverter.GetBytes(compressedOffset);
            Array.Reverse(compressedOffsetArray);
            finalMIO0Block.AddRange(compressedOffsetArray);

            ///add final uncompressed offset
            uncompressedOffset = compressedOffset + compressedDataBytes.Count;
            byte[] uncompressedOffsetArray = BitConverter.GetBytes(uncompressedOffset);
            Array.Reverse(uncompressedOffsetArray);
            finalMIO0Block.AddRange(uncompressedOffsetArray);

            ///add layout bits
            foreach (byte layoutByte in layoutBytes)                 ///add layout bytes to file
            {
                finalMIO0Block.Add(layoutByte);
            }

            ///add compressed data
            foreach (byte compressedByte in compressedDataBytes)     ///add compressed bytes to file
            {
                finalMIO0Block.Add(compressedByte);
            }

            ///add uncompressed data
            foreach (byte uncompressedByte in uncompressedData)      ///add noncompressed bytes to file
            {
                finalMIO0Block.Add(uncompressedByte);
            }

            return finalMIO0Block.ToArray();
        }

        /// https:///github.com/Daniel-McCarthy/Mr-Peeps-Compressor 

        public byte[] decompress_seg7(byte[] useg7)
        {

            /// This will decompress Segment 7's compressed display lists to regular F3DEX commands.
            /// This is used exclusively by Mario Kart 64's Segment 7.


            int cID = SegmentExporter.cID;

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
            /// seg4 is an uncompressed Segment 4. This contains all the vertices for a Mario Kart 64 course.

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
            /// there is an alternative that will return a direct maxscript command to render the triangle. 

            /// command 0x06 will return the segment and offset of the display lists to run on seperate lines.

            /// command 0xB8 represents the end of a display list and will return "ENDSECTION"

            /// command 0x04 will return the vertoffset described above, which should be updated and maintained by the calling function to be passed again.
            /// translate_F3D needs a proper vertoffset provided every time for either 0xB1 or 0xBF commands, it is not maintained automatically.
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





        public SectionView[] LoadSVL(string inputSVL)
        {

            int current_line = 0; ///used to parse the SVL file.
            string[] SVLfile = File.ReadAllLines(inputSVL);

            int section_count = Convert.ToInt32(SVLfile[current_line]);
            current_line++;

            SectionView[] course = new SectionView[section_count];

            ///MessageBox.Show(current_line.ToString());
            for (int section = 0; section < section_count; section++)
            {
                ///MessageBox.Show(current_line.ToString());
                course[section] = new SectionView();
                course[section].viewlist = new ViewList[4];


                for (int view = 0; view < 4; view++)
                {
                    course[section].viewlist[view] = new ViewList();

                    string section_name_garbage = SVLfile[current_line];
                    current_line = current_line + 1;

                    int object_count = Convert.ToInt32(SVLfile[current_line]);
                    current_line++;
                    course[section].viewlist[view].objectlist = new string[object_count];

                    for (int objectindex = 0; objectindex < object_count; objectindex++)
                    {
                        course[section].viewlist[view].objectlist[objectindex] = SVLfile[current_line];
                        current_line = current_line + 1;
                        ///MessageBox.Show(course[section].viewlist[view].objectlist[objectindex]);
                    }


                }
            }

            return course;
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





        public Pathgroup[] loadPOP(string popFile)
        {
            //load the pathgroups from the external .OK64.POP file provided

            //POP files are much easier to read than 3PL files. 
            //POP files are constrained by specific limits for OverKart64 and cannot hold arbitrary data.
            //POP files are only for custom courses designed with OverKart64. 3PL files can hold any object/path data. 

            List<Pathgroup> pathgroup = new List<Pathgroup>();


            string[] reader = File.ReadAllLines(popFile);
            string[] positions = new string[3];

            int[] markerCount = new int[4] { 800, 64, 64, 64 };
            int current_line = 0;

            for (int group = 0; group < 4; group++)
            {
                pathgroup.Add(new Pathgroup());
                pathgroup[group].pathlist = new List<Pathlist>();

                
                pathgroup[group].pathlist.Add(new Pathlist());
                pathgroup[group].pathlist[0].pathmarker = new List<Marker>();

                for (int marker = 0; marker < markerCount[group]; marker++)
                {
                    pathgroup[group].pathlist[0].pathmarker.Add(new Marker());


                    // input format

                    //[xposition,yposition,zposition]
                    //flag

                    // Flag for Path should correlate with section.
                    // Flag for objects will almost always be 0. Unsure of effect. 

                    string lineRead = reader[current_line].Substring(1, (reader[current_line].Length - 2));
                    // This strips the brackets from the first line

                    string[] markerPosition = lineRead.Split(',');
                    // This creates an array containing the marker positions as strings.

                    current_line++;
                    // Advance forward in the file.


                    pathgroup[group].pathlist[0].pathmarker[marker].xval = Convert.ToInt32(Single.Parse(markerPosition[0]));
                    pathgroup[group].pathlist[0].pathmarker[marker].yval = Convert.ToInt32(Single.Parse(markerPosition[1]));
                    pathgroup[group].pathlist[0].pathmarker[marker].zval = Convert.ToInt32(Single.Parse(markerPosition[2]));
                    
                    //maintain Z/Y axis, we flip it only when writing to the ROM.


                    pathgroup[group].pathlist[0].pathmarker[marker].flag = Convert.ToInt32(reader[current_line]);
                    //Read the next line, convert to int. This is the accompanying Flag for the marker. 

                    current_line++;
                    // Advance forward in the file.
                }


            }
            Pathgroup[] popPath = pathgroup.ToArray();
            return popPath;

        }





        //the following gets used to inject path markers or object markers into an uncompressed seg6 file.


        public byte[] popMarkers(string popFile)
        {


            //InjectMarkers is used by the geometry compiler, not to add objects to existing courses.
            // AddMarkers, down below this command, is used by the Object and Path editors to modify existing course data.



            Pathgroup[] pathgroup = loadPOP(popFile);


            bs = new MemoryStream();
            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);

            int groupCount = pathgroup.Length;
            for (int currentGroup = 0; currentGroup < groupCount; currentGroup++)
            {

                int markerCount = pathgroup[currentGroup].pathlist[0].pathmarker.Count;
                for (int currentMarker = 0; currentMarker < markerCount; currentMarker++)
                {
                    int[] tempint = new int[4];



                    flip2 = BitConverter.GetBytes(Convert.ToInt16(pathgroup[currentGroup].pathlist[0].pathmarker[currentMarker].xval));
                    Array.Reverse(flip2);
                    bw.Write(flip2);  //x

                    flip2 = BitConverter.GetBytes(Convert.ToInt16(pathgroup[currentGroup].pathlist[0].pathmarker[currentMarker].zval));
                    Array.Reverse(flip2);
                    bw.Write(flip2);  //z

                    flip2 = BitConverter.GetBytes(Convert.ToInt16(-1 * pathgroup[currentGroup].pathlist[0].pathmarker[currentMarker].yval));
                    Array.Reverse(flip2);
                    bw.Write(flip2);  //y 

                    flip2 = BitConverter.GetBytes(Convert.ToUInt16(pathgroup[currentGroup].pathlist[0].pathmarker[currentMarker].flag));
                    Array.Reverse(flip2);
                    bw.Write(flip2);  //flag






                }




                flip2 = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                Array.Reverse(flip2);
                bw.Write(flip2);  //x

                if (currentGroup == 0)  //group 0 is course paths, groups 1-3 are objects
                {
                    flip2 = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                    Array.Reverse(flip2);
                    bw.Write(flip2);  //z

                    flip2 = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                    Array.Reverse(flip2);
                    bw.Write(flip2);  //y 
                }
                else
                {

                    flip2 = BitConverter.GetBytes(Convert.ToInt16(0));
                    Array.Reverse(flip2);
                    bw.Write(flip2);  //z

                    flip2 = BitConverter.GetBytes(Convert.ToInt16(0));
                    Array.Reverse(flip2);
                    bw.Write(flip2);  //y 
                }


                flip2 = BitConverter.GetBytes(Convert.ToInt16(0));
                Array.Reverse(flip2);
                bw.Write(flip2);  //flag
            }

            byte[] popBytes = bs.ToArray();
            return popBytes;

        }






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









        //public List<byte[]> CompileGeometry(byte[] rombytes, int cID, string rawmodel, string outputdirectory, string pathmarkers, string objectmarkers)
        //{



        //    ///byte[] pathPositions = File.ReadAllBytes(pathbox.Text);
        //    ///byte[] itemPositions = File.ReadAllBytes(itembox.Text);
        //    ///byte[] seg9in = File.ReadAllBytes(seg9box.Text);



        //    /// "Hamp, what are you doing?"
        //    /// "Writing software to compile raw model data into Segment Data for Mario Kart 64"
        //    /// "It's 4 o'clock in the morning, why on earth are you writing software to compile raw model data into segment data for Mario Kart 64?"
        //    /// "Because I've lost control of my life"
        //    ///  https:///www.youtube.com/watch?v=1vs55Z7t7bk




        //    UInt16 relative_zero = 0;
        //    UInt16 relative_index = 0;

        //    byte[] byte4 = new byte[4];
        //    byte[] byte2 = new byte[2];

        //    byte[] ASMInstructions = new byte[0];
        //    //We'll use this for building ASM Instructions 
        //    // before writing them to the ROM.


        //    string[] section_view = File.ReadAllLines("sectionview");

        //    int section_count = Convert.ToInt32(section_view[0]);

        //    string[] viewstrings = { "North", "East", "South", "West" };
        //    AssimpSharp.Scene fbx = new AssimpSharp.Scene();


        //    int SectionViewOffset = 0;
        //    int SurfaceMapOffset = 0;



        //    byte[] useg4 = new byte[0];
        //    byte[] seg4 = new byte[0];
        //    byte[] useg7 = new byte[0];
        //    byte[] seg7 = new byte[0];
        //    byte[] useg6 = new byte[0];
        //    byte[] seg6 = new byte[0];
        //    byte[] seg9 = new byte[0];




        //    MemoryStream romstream = new MemoryStream();
        //    BinaryReader romreader = new BinaryReader(Stream.Null);
        //    BinaryWriter romwriter = new BinaryWriter(Stream.Null);


        //    MemoryStream seg7stream = new MemoryStream();
        //    BinaryReader seg7reader = new BinaryReader(Stream.Null);
        //    BinaryWriter seg7writer = new BinaryWriter(Stream.Null);

        //    MemoryStream seg4stream = new MemoryStream();
        //    BinaryReader seg4reader = new BinaryReader(Stream.Null);
        //    BinaryWriter seg4writer = new BinaryWriter(Stream.Null);

        //    MemoryStream seg9stream = new MemoryStream();
        //    BinaryReader seg9reader = new BinaryReader(Stream.Null);
        //    BinaryWriter seg9writer = new BinaryWriter(Stream.Null);

        //    MemoryStream seg6stream = new MemoryStream();
        //    BinaryReader seg6reader = new BinaryReader(Stream.Null);
        //    BinaryWriter seg6writer = new BinaryWriter(Stream.Null);








        //    romstream = new MemoryStream();
        //    romreader = new BinaryReader(romstream);
        //    romwriter = new BinaryWriter(romstream);

        //    seg7stream = new MemoryStream();
        //    seg7reader = new BinaryReader(seg7stream);
        //    seg7writer = new BinaryWriter(seg7stream);

        //    seg4stream = new MemoryStream();
        //    seg4reader = new BinaryReader(seg4stream);
        //    seg4writer = new BinaryWriter(seg4stream);


        //    seg6stream = new MemoryStream();
        //    seg6reader = new BinaryReader(seg6stream);
        //    seg6writer = new BinaryWriter(seg6stream);

        //    seg9stream = new MemoryStream();
        //    seg9reader = new BinaryReader(seg9stream);
        //    seg9writer = new BinaryWriter(seg9stream);





        //    //So. Here We Go.








        //    ///So AssimpSharp's FBX parser is what we use to load everything.


        //    ///So this has gone through like 5 revisions bear with me.
        //    ///We have the course broken up into objects all referenced under "Course Master Objects".
        //    ///We go through each 1 at a time, load up it's verts into memory and load it's faces into memory.
        //    ///We write the verts to segment 4 and then the instructions to draw the faces.
        //    ///Then we repeat until we have all the objects written.

        //    ///The surface map uses it's own model. Normally Mario Kart builds it's surface map out of objects as well.
        //    /// A future update could save some data space by doing this. Shouldn't cause any issues however.
        //    /// For now, each section has it's own objects that are marked with the surface ID and material ID.
        //    /// The surface_index (area to render) in inheirted from the parent object we pull it from. 

        //    ///Section views are in a .txt format. This is a nightmare. We have to build F3DEX display list calls
        //    /// from all the objects we wrote to file earlier. So each object needs to have it's offset in Segment 7
        //    /// stored so that it can be referenced later by the object's name alone. This is accomplished via parallel arrays of strings and ints.
        //    /// If you don't like it, spend 8 months learning how this game store's it's data formats and write your own tool. 

        //    /// .txt format lists section count, then section 1 north's object count, then all the objects. Repeat for each view, repeat for each section

        //    /// Section Count
        //    ///      Section 1 North Object Count  (let's say 45)
        //    ///              Object 1 name
        //    ///              Object 2 name
        //    ///              ...
        //    ///              Object 45
        //    ///      Section 1 East Object Count  
        //    ///              Object 1 name
        //    /// 

        //    ///etc. 


        //    /// it's 4AM and IDK what the equation should look like.
        //    /// probably F3DEX VertIndex = MaxScript VertIndex - relative_index, and relative_zero is only used to...
        //    ///...calculate where to load vertices from, relative to the current section. So to load a vert it'd look like;
        //    ///  F3DEX vertoffset = (relative_zero + (Highest/Lowest Index - 4)) * 16 
        //    /// I'll probably need those later.


        //    ///We start Segment 7 and Segment 6 by mass writing 0xB8 commands, to block out any display lists that are hardcoded by ASM
        //    ///Then we overwrite the raw data we need to those addresses
        //    ///TO-DO: rewrite the ASM for the pathmarkers and itemboxes to be at EOF to prevent larger data from overwriting ASM hardcoded areas.




        //    int subsection_count = 0;




        //    UInt32 ImgSize = 0, ImgType = 0, ImgFlag1 = 0, ImgFlag2 = 0, ImgFlag3 = 0;
        //    UInt32[] ImgTypes = { 0, 0, 0, 3, 3, 3, 0 }; ///0=RGBA, 3=IA
        //    UInt32[] STheight = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; ///looks like
        //    UInt32[] STwidth = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 }; ///texture sizes...
        //    byte[] heightex = { 5, 5, 6, 5, 5, 6, 5 };
        //    byte[] widthex = { 5, 6, 5, 5, 6, 5, 5 };




        //    ///We'll set this to true for now, and then disable it after we've written all the model data.
        //    bool fuck_me = true;


        //    ///
        //    ///
        //    ///
        //    ///
        //    ///
        //    ///
        //    ///

        //    List<byte> seg9ref = new List<byte>();



        //    byte[] duseg7 = dumpseg7(cID, rombytes);
        //    byte[] seg7ref = decompress_seg7(duseg7);



        //    SectionList offsetlist = new SectionList();
        //    offsetlist.section = new List<int>();

        //    // We write on 0xB8 command to the start of both Seg6 and Seg7 for blocking ASM routines later.


        //    byte4 = BitConverter.GetBytes(0xB8000000);
        //    Array.Reverse(byte4);
        //    seg6writer.Write(byte4);

        //    byte4 = BitConverter.GetBytes(0x00000000);
        //    Array.Reverse(byte4);
        //    seg6writer.Write(byte4);

        //    //Prep Segment 7 for any hardcoded display lists

        //    byte4 = BitConverter.GetBytes(0xB8000000);
        //    Array.Reverse(byte4);
        //    seg7writer.Write(byte4);

        //    byte4 = BitConverter.GetBytes(0x00000000);
        //    Array.Reverse(byte4);
        //    seg7writer.Write(byte4);










        //    ///
        //    ///
        //    ///
        //    ///
        //    ///

        //    int n = 1;
        //    int local_zero = 0;
        //    int current_section = 0;
        //    int current_view = 0;

        //    var assimpSharpImporter = new AssimpSharp.FBX.FBXImporter();
        //    fbx = new AssimpSharp.Scene();
        //    fbx = assimpSharpImporter.ReadFile(rawmodel);

        //    int Seg5Position = 0;
        //    int TextureCount = fbx.Textures.Count;
        //    OK64Texture[] TextureList = new OK64Texture[TextureCount];






        //    for (int tIndex = 0; tIndex < TextureCount; tIndex++)
        //    {
        //        

        //    }


        //    SectionViewOffset = Convert.ToUInt16(seg9writer.BaseStream.Length);
        //    // After writing all of the texture offsets to segment 9 we'll need to add the 
        //    // master display list. But First we'll need to build it down below. In the meanwhile.
        //    // We need this offset to edit the ASM instructions that call for the Master Display List.
        //    // We'll make that adjustment here;



        //    string section_parent = "Course Master Objects";
        //    var currentnode = fbx.RootNode.FindNode(section_parent);


        //    List<string> object_name = new List<string>();

        //    foreach (var child in currentnode.Children)
        //    {

        //        foreach (var subindex in child.Meshes)
        //        {
        //            var current_subobject = fbx.Meshes[subindex];




        //            int vertcount = current_subobject.Vertices.Length;
        //            ///MessageBox.Show(currentnode.Name+"-"+child.Name + "-Vert Count-"+vertcount.ToString());
        //            int facecount = current_subobject.Faces.Length;
        //            ///MessageBox.Show(currentnode.Name + "-" + child.Name + "-Face Count-" + facecount.ToString());


        //            List<Vertex> RealVerts = new List<Vertex>();
        //            int[] local_index = new int[vertcount];


        //            for (int count_local = 0; count_local < vertcount; count_local++)
        //            {
        //                short local_x = Convert.ToInt16(current_subobject.Vertices[count_local].X);
        //                short local_y = Convert.ToInt16(current_subobject.Vertices[count_local].Y);
        //                short local_z = Convert.ToInt16(current_subobject.Vertices[count_local].Z);
        //                float local_u = Convert.ToSingle(current_subobject.TextureCoords[0][count_local][0]);
        //                float local_v = Convert.ToSingle(current_subobject.TextureCoords[0][count_local][1]);
        //                if (RealVerts.Count == 0)
        //                {
        //                    RealVerts.Add(new Vertex { });
        //                    int count_addedindex = RealVerts.Count - 1;
        //                    RealVerts[count_addedindex].position = new Position { };

        //                    RealVerts[count_addedindex].position.x = local_x;
        //                    RealVerts[count_addedindex].position.y = local_y;
        //                    RealVerts[count_addedindex].position.z = local_z;
        //                    RealVerts[count_addedindex].position.u = local_u;
        //                    RealVerts[count_addedindex].position.v = local_v;

        //                    local_index[count_local] = RealVerts.Count - 1;
        //                    ///MessageBox.Show("No Match");
        //                }
        //                else
        //                {

        //                    bool match = false;
        //                    for (int count_realvert = 0; count_realvert < RealVerts.Count; count_realvert++)
        //                    {
        //                        if ((local_x == RealVerts[count_realvert].position.x) & (local_y == RealVerts[count_realvert].position.y) & (local_z == RealVerts[count_realvert].position.z) & (local_u == RealVerts[count_realvert].position.u) & (local_v == RealVerts[count_realvert].position.v))
        //                        {
        //                            local_index[count_local] = count_realvert;
        //                            match = true;
        //                            ///MessageBox.Show("Match");
        //                        }
        //                    }

        //                    if (match == false)
        //                    {
        //                        RealVerts.Add(new Vertex { });
        //                        int count_addedindex = RealVerts.Count - 1;
        //                        RealVerts[count_addedindex].position = new Position { };

        //                        RealVerts[count_addedindex].position.x = local_x;
        //                        RealVerts[count_addedindex].position.y = local_y;
        //                        RealVerts[count_addedindex].position.z = local_z;
        //                        RealVerts[count_addedindex].position.u = local_u;
        //                        RealVerts[count_addedindex].position.v = local_v;


        //                        local_index[count_local] = RealVerts.Count - 1;
        //                        ///MessageBox.Show("No Match");
        //                    }
        //                }
        //            }




        //            if (facecount > 30)
        //            {
        //                MessageBox.Show("Warning Subobject with more than 30 Faces. - " + currentnode.Name + "-" + child.Name);
        //            }

        //            int materialID = new int();

        //            Face[] face = new Face[facecount];
        //            Vertex[] vert = new Vertex[RealVerts.Count];

        //            string[] split = child.Name.Split('_');

        //            int collision_out = new int();
        //            int.TryParse(split[0], out materialID);
        //            materialID = materialID - 1;




        //            int intread = 0;


        //            for (int f = 0; f < facecount; f++)
        //            {
        //                face[f] = new Face { };
        //                face[f].vertindex = new VertIndex { };

        //                face[f].vertindex.v0 = current_subobject.Faces[f].Indices[0];

        //                face[f].vertindex.v2 = current_subobject.Faces[f].Indices[1];

        //                face[f].vertindex.v1 = current_subobject.Faces[f].Indices[2]; ;

        //                face[f].material = materialID;


        //                face[f].vertindex.v0 = local_index[face[f].vertindex.v0];
        //                face[f].vertindex.v1 = local_index[face[f].vertindex.v1];
        //                face[f].vertindex.v2 = local_index[face[f].vertindex.v2];


        //                vert[face[f].vertindex.v0] = new Vertex { };
        //                vert[face[f].vertindex.v0].position = new Position { };


        //                vert[face[f].vertindex.v0].position.x = Convert.ToInt16(RealVerts[face[f].vertindex.v0].position.x);

        //                vert[face[f].vertindex.v0].position.y = Convert.ToInt16(RealVerts[face[f].vertindex.v0].position.z);
        //                /// Flip YZ Axis 
        //                vert[face[f].vertindex.v0].position.z = Convert.ToInt16(RealVerts[face[f].vertindex.v0].position.y);    /// Flip YZ Axis 
        //                vert[face[f].vertindex.v0].position.z = Convert.ToInt16(vert[face[f].vertindex.v0].position.z * -1); /// Flip Y Axis



        //                vert[face[f].vertindex.v2] = new Vertex { };
        //                vert[face[f].vertindex.v2].position = new Position { };



        //                vert[face[f].vertindex.v2].position.x = Convert.ToInt16(RealVerts[face[f].vertindex.v2].position.x);

        //                vert[face[f].vertindex.v2].position.y = Convert.ToInt16(RealVerts[face[f].vertindex.v2].position.z);

        //                vert[face[f].vertindex.v2].position.z = Convert.ToInt16(RealVerts[face[f].vertindex.v2].position.y);    /// Flip YZ Axis 
        //                vert[face[f].vertindex.v2].position.z = Convert.ToInt16(vert[face[f].vertindex.v2].position.z * -1); /// Flip Y Axis



        //                vert[face[f].vertindex.v1] = new Vertex { };
        //                vert[face[f].vertindex.v1].position = new Position { };


        //                vert[face[f].vertindex.v1].position.x = Convert.ToInt16(RealVerts[face[f].vertindex.v1].position.x);

        //                vert[face[f].vertindex.v1].position.y = Convert.ToInt16(RealVerts[face[f].vertindex.v1].position.z);

        //                vert[face[f].vertindex.v1].position.z = Convert.ToInt16(RealVerts[face[f].vertindex.v1].position.y);    /// Flip YZ Axis 
        //                vert[face[f].vertindex.v1].position.z = Convert.ToInt16(vert[face[f].vertindex.v1].position.z * -1); /// Flip Y Axis

        //            }



        //            ///Ok so now that we've loaded the raw model data, let's start writing some F3DEX. God have mercy.







        //            ///
        //            ///Seg 7 is prepped to prevent hardcoded ASM overwrites
        //            ///0xB8 is an end list F3DEX command, which will return any ASM hardcoded call.
        //            ///F3DEX is all alligned to 32 bit addresses, no calls to weird offset.
        //            ///<3 Micro <3 you mad lad
        //            ///

        //            offsetlist.section.Add(Convert.ToInt32(seg7stream.Position));
        //            object_name.Add(child.Name);

        //            byte4 = BitConverter.GetBytes(0xBB000001);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0xFFFFFFFF);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);


        //            ImgType = ImgTypes[TextureList[materialID].TextureClass];
        //            ImgFlag1 = STheight[TextureList[materialID].TextureClass];
        //            ImgFlag2 = STwidth[TextureList[materialID].TextureClass];
        //            if (TextureList[materialID].TextureClass == 6)
        //            {
        //                ImgFlag3 = 0x100;
        //            }
        //            else
        //            {
        //                ImgFlag3 = 0x00;
        //            }



        //            byte4 = BitConverter.GetBytes(0xE8000000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0x00000000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);



        //            byte4 = BitConverter.GetBytes((((ImgType << 0x15) | 0xF5100000) | ((((ImgFlag2 << 1) + 7) >> 3) << 9)) | ImgFlag3);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(Convert.ToInt32(((heightex[TextureList[materialID].TextureClass] & 0xF) << 0x12) | (((heightex[TextureList[materialID].TextureClass] & 0xF0) >> 4) << 0xE) | ((widthex[TextureList[materialID].TextureClass] & 0xF) << 8) | (((widthex[TextureList[materialID].TextureClass] & 0xF0) >> 4) << 4)));
        //            byte4 = BitConverter.GetBytes(BitConverter.ToInt32(byte4, 0) >> 4);
        //            //IDK why but this makes it into what it's supposed to be.
        //            Array.Reverse(byte4);

        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0xF2000000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes((((ImgFlag2 - 1) << 0xE) | ((ImgFlag1 - 1) << 2)));
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0xFD100000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0x05000000 | TextureList[materialID].Seg5Offset);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0xE8000000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0x00000000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            ///F5100000 07000000 E6000000 00000000 F3000000 073FF100

        //            byte4 = BitConverter.GetBytes(0xF5100000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0x07000000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0xE6000000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0x00000000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0xF3000000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            if (TextureList[materialID].TextureClass == 0)
        //            {
        //                byte4 = BitConverter.GetBytes(0x073FF100);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);
        //            }
        //            else if (TextureList[materialID].TextureClass == 1)
        //            {
        //                byte4 = BitConverter.GetBytes(0x077FF080);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //            }



        //            ///load the first set of verts from the relative_zero position;

        //            byte4 = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);


        //            byte4 = BitConverter.GetBytes(0x04000000 | relative_zero * 16);  ///from segment 4 at offset relative_zero
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);



        //            relative_index = 0;

        //            for (int x = 0; x < facecount;)
        //            {



        //                int v0 = 0;
        //                int v1 = 0;
        //                int v2 = 0;


        //                if (x + 2 <= facecount)
        //                {
        //                    /// draw 2 triangles, check for additional verts in both.
        //                    if (face[x].vertindex.v0 > (relative_index + 31) | face[x].vertindex.v1 > (relative_index + 31) | face[x].vertindex.v2 > (relative_index + 31) | /// OR with next line
        //                        face[x + 1].vertindex.v0 > (relative_index + 31) | face[x + 1].vertindex.v1 > (relative_index + 31) | face[x + 1].vertindex.v2 > (relative_index + 31))
        //                    {

        //                        /// OVER VERT LIMIT, LOAD NEW VERTS
        //                        UInt16 maxvalue = Convert.ToUInt16(GetMax(face[x].vertindex.v0, face[x].vertindex.v1));
        //                        maxvalue = Convert.ToUInt16(GetMax(face[x].vertindex.v2, maxvalue));
        //                        maxvalue = Convert.ToUInt16(GetMax(face[x + 1].vertindex.v0, maxvalue));
        //                        maxvalue = Convert.ToUInt16(GetMax(face[x + 1].vertindex.v1, maxvalue));
        //                        maxvalue = Convert.ToUInt16(GetMax(face[x + 1].vertindex.v2, maxvalue));


        //                        if (maxvalue - 4 < 0)
        //                        {
        //                            maxvalue = 0;
        //                        }
        //                        else
        //                        {
        //                            maxvalue = Convert.ToUInt16(maxvalue - 4);
        //                        }
        //                        byte4 = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);


        //                        byte4 = BitConverter.GetBytes(0x04000000 | ((relative_zero + maxvalue) * 16));  ///from segment 4 at offset relative_zero
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);

        //                        relative_index = maxvalue;
        //                    }

        //                    if ((face[x].vertindex.v0 < relative_index) | (face[x].vertindex.v1 < relative_index) | (face[x].vertindex.v2 < relative_index) |
        //                        (face[x + 1].vertindex.v0 < relative_index) | (face[x + 1].vertindex.v1 < relative_index) | (face[x + 1].vertindex.v2 < relative_index))
        //                    {

        //                        /// UNDER VERT LIMIT, LOAD NEW VERTS
        //                        UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v0, face[x].vertindex.v1));
        //                        minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v2, minvalue));
        //                        minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v0, minvalue));
        //                        minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v1, minvalue));
        //                        minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v2, minvalue));

        //                        if (minvalue - 4 < 0)
        //                        {
        //                            minvalue = 0;
        //                        }
        //                        else
        //                        {
        //                            minvalue = Convert.ToUInt16(minvalue - 4);
        //                        }

        //                        byte4 = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);


        //                        byte4 = BitConverter.GetBytes(0x04000000 | ((relative_zero + minvalue) * 16));  ///from segment 4 at offset relative_zero
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);

        //                        relative_index = minvalue;
        //                    }


        //                    ///end vert check



        //                    v0 = face[x].vertindex.v0 - relative_index;
        //                    v1 = face[x].vertindex.v1 - relative_index;
        //                    v2 = face[x].vertindex.v2 - relative_index;


        //                    byte4 = BitConverter.GetBytes(Convert.ToUInt32(0xB1000000 | (v2 << 17) | (v1 << 9) | v0 << 1));
        //                    Array.Reverse(byte4);
        //                    seg7writer.Write(byte4);

        //                    v0 = face[x + 1].vertindex.v0 - relative_index;
        //                    v1 = face[x + 1].vertindex.v1 - relative_index;
        //                    v2 = face[x + 1].vertindex.v2 - relative_index;

        //                    byte4 = BitConverter.GetBytes(Convert.ToUInt32((v2 << 17) | (v1 << 9) | v0 << 1));
        //                    Array.Reverse(byte4);
        //                    seg7writer.Write(byte4);
        //                    x = x + 2;

        //                }
        //                else
        //                {
        //                    /// draw 1 triangle, only 1 vert check

        //                    if (face[x].vertindex.v0 > (relative_index + 31) | face[x].vertindex.v1 > (relative_index + 31) | face[x].vertindex.v2 > (relative_index + 31))
        //                    {

        //                        /// OVER VERT LIMIT, LOAD NEW VERTS
        //                        UInt16 maxvalue = Convert.ToUInt16(GetMax(face[x].vertindex.v0, face[x].vertindex.v1));
        //                        maxvalue = Convert.ToUInt16(GetMax(face[x].vertindex.v2, maxvalue));


        //                        if (maxvalue - 4 < 0)
        //                        {
        //                            maxvalue = 0;
        //                        }
        //                        else
        //                        {
        //                            maxvalue = Convert.ToUInt16(maxvalue - 4);
        //                        }
        //                        byte4 = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);


        //                        byte4 = BitConverter.GetBytes(0x04000000 | ((relative_zero + maxvalue) * 16));  ///from segment 4 at offset relative_zero
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);

        //                        relative_index = maxvalue;
        //                    }

        //                    if ((face[x].vertindex.v0 < relative_index) | (face[x].vertindex.v1 < relative_index) | (face[x].vertindex.v2 < relative_index))
        //                    {

        //                        /// UNDER VERT LIMIT, LOAD NEW VERTS
        //                        UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v0, face[x].vertindex.v1));
        //                        minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v2, minvalue));

        //                        if (minvalue - 4 < 0)
        //                        {
        //                            minvalue = 0;
        //                        }
        //                        else
        //                        {
        //                            minvalue = Convert.ToUInt16(minvalue - 4);
        //                        }

        //                        byte4 = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);


        //                        byte4 = BitConverter.GetBytes(0x04000000 | ((relative_zero + minvalue) * 16));  ///from segment 4 at offset relative_zero
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);

        //                        relative_index = minvalue;
        //                    }





        //                    ///end vert check
        //                    byte4 = BitConverter.GetBytes(Convert.ToUInt32(0xBF000000));
        //                    Array.Reverse(byte4);
        //                    seg7writer.Write(byte4);

        //                    v0 = face[x].vertindex.v0 - relative_index;
        //                    v1 = face[x].vertindex.v1 - relative_index;
        //                    v2 = face[x].vertindex.v2 - relative_index;


        //                    byte4 = BitConverter.GetBytes(Convert.ToUInt32((v2 << 17) | (v1 << 9) | v0 << 1));
        //                    Array.Reverse(byte4);
        //                    seg7writer.Write(byte4);
        //                    x = x + 1;
        //                }


        //            }


        //            byte4 = BitConverter.GetBytes(0xB8000000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(0x00000000);
        //            Array.Reverse(byte4);
        //            seg7writer.Write(byte4);





        //            for (int v = 0; v < RealVerts.Count; v++)
        //            {



        //                if (vert[v] != null)
        //                {


        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.x));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.y));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.z));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.s));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.t));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);
        //                }
        //                else
        //                {
        //                    /// If there was an unused vert for whatever reason, right null data to preserve the vert indexes.
        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(0));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(0));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(0));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(0));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(0));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);


        //                }
        //                byte RGB = 252;
        //                byte Alpha = 0;
        //                seg4writer.Write(RGB);
        //                seg4writer.Write(RGB);
        //                seg4writer.Write(RGB);
        //                seg4writer.Write(Alpha);
        //            }

        //            relative_zero = Convert.ToUInt16(relative_zero + RealVerts.Count);
        //            local_zero = relative_zero;






        //        }
        //    }

        //    current_view += 1;




        //    /// cool, so now that we've gotten that out of the way we can set this back off
        //    fuck_me = false;

        //    ///LOL JK WE GOTTA DO THE WHOLE THING OVER AGAIN EXCEPT THIS TIME FOR THE COLLISION/SURFACE MAP LOLOLOL
        //    ///YOU THOUGHT YOU WERE FREE.

        //    fuck_me = true;




        //    List<int> collisionIDs = new List<int>();
        //    List<int> areaIDs = new List<int>();
        //    List<SectionList> collisionoffsetlist = new List<SectionList>();
        //    n = 1;

        //    current_section = 0;


        //    for (int i = 0; i < section_count; i++)
        //    {
        //        collisionoffsetlist.Add(new SectionList());
        //        collisionoffsetlist[i].section = new List<int>();
        //        section_parent = "Section " + (i + 1).ToString() + " Surface";
        //        currentnode = fbx.RootNode.FindNode(section_parent);

        //        foreach (var child in currentnode.Children)
        //        {
        //            foreach (var subindex in child.Meshes)
        //            {


        //                ///MessageBox.Show(subsection_count.ToString());



        //                var current_subobject = fbx.Meshes[subindex];



        //                int vertcount = current_subobject.Vertices.Length;
        //                int facecount = current_subobject.Faces.Length;

        //                if (facecount > 25)
        //                {
        //                    MessageBox.Show("Warning Subobject with more than 30 Faces. - " + currentnode.Name + "-" + child.Name);
        //                }

        //                int materialID = new int();




        //                List<Vertex> RealVerts = new List<Vertex>();
        //                int[] local_index = new int[vertcount];


        //                for (int count_local = 0; count_local < vertcount; count_local++)
        //                {
        //                    short local_x = Convert.ToInt16(current_subobject.Vertices[count_local].X);
        //                    short local_y = Convert.ToInt16(current_subobject.Vertices[count_local].Y);
        //                    short local_z = Convert.ToInt16(current_subobject.Vertices[count_local].Z);
        //                    float local_u = Convert.ToSingle(current_subobject.TextureCoords[0][count_local][0]);
        //                    float local_v = Convert.ToSingle(current_subobject.TextureCoords[0][count_local][1]);
        //                    if (RealVerts.Count == 0)
        //                    {
        //                        RealVerts.Add(new Vertex { });
        //                        int count_addedindex = RealVerts.Count - 1;
        //                        RealVerts[count_addedindex].position = new Position { };

        //                        RealVerts[count_addedindex].position.x = local_x;
        //                        RealVerts[count_addedindex].position.y = local_y;
        //                        RealVerts[count_addedindex].position.z = local_z;

        //                        local_index[count_local] = RealVerts.Count - 1;
        //                        ///MessageBox.Show("No Match");
        //                    }
        //                    else
        //                    {

        //                        bool match = false;
        //                        for (int count_realvert = 0; count_realvert < RealVerts.Count; count_realvert++)
        //                        {
        //                            if ((local_x == RealVerts[count_realvert].position.x) & (local_y == RealVerts[count_realvert].position.y) & (local_z == RealVerts[count_realvert].position.z) & (local_u == RealVerts[count_realvert].position.u) & (local_v == RealVerts[count_realvert].position.v))
        //                            {
        //                                local_index[count_local] = count_realvert;
        //                                match = true;
        //                                ///MessageBox.Show("Match");
        //                            }
        //                        }

        //                        if (match == false)
        //                        {
        //                            RealVerts.Add(new Vertex { });
        //                            int count_addedindex = RealVerts.Count - 1;
        //                            RealVerts[count_addedindex].position = new Position { };

        //                            RealVerts[count_addedindex].position.x = local_x;
        //                            RealVerts[count_addedindex].position.y = local_y;
        //                            RealVerts[count_addedindex].position.z = local_z;
        //                            RealVerts[count_addedindex].position.u = local_u;
        //                            RealVerts[count_addedindex].position.v = local_v;


        //                            local_index[count_local] = RealVerts.Count - 1;
        //                            ///MessageBox.Show("No Match");
        //                        }
        //                    }
        //                }






        //                Face[] face = new Face[facecount];
        //                Vertex[] vert = new Vertex[RealVerts.Count];




        //                string[] split = new string[3];
        //                string stringread = "";

        //                int intread = 0;


        //                split = child.Name.Split('_');

        //                int collision_out = new int();
        //                int.TryParse(split[1], out materialID);
        //                materialID = materialID - 1;
        //                int.TryParse(split[0], out collision_out);
        //                collision_out = collision_out;
        //                collisionIDs.Add(collision_out);
        //                areaIDs.Add(i);




        //                for (int f = 0; f < facecount; f++)
        //                {
        //                    face[f] = new Face { };
        //                    face[f].vertindex = new VertIndex { };

        //                    face[f].vertindex.v0 = current_subobject.Faces[f].Indices[0];

        //                    face[f].vertindex.v2 = current_subobject.Faces[f].Indices[1];

        //                    face[f].vertindex.v1 = current_subobject.Faces[f].Indices[2];

        //                    face[f].material = materialID;

        //                    face[f].vertindex.v0 = local_index[face[f].vertindex.v0];
        //                    face[f].vertindex.v1 = local_index[face[f].vertindex.v1];
        //                    face[f].vertindex.v2 = local_index[face[f].vertindex.v2];



        //                    vert[face[f].vertindex.v0] = new Vertex { };
        //                    vert[face[f].vertindex.v0].position = new Position { };



        //                    vert[face[f].vertindex.v0].position.x = Convert.ToInt16(RealVerts[face[f].vertindex.v0].position.x);

        //                    vert[face[f].vertindex.v0].position.y = Convert.ToInt16(RealVerts[face[f].vertindex.v0].position.z);
        //                    /// Flip YZ Axis 
        //                    vert[face[f].vertindex.v0].position.z = Convert.ToInt16(RealVerts[face[f].vertindex.v0].position.y);    /// Flip YZ Axis 
        //                    vert[face[f].vertindex.v0].position.z = Convert.ToInt16(vert[face[f].vertindex.v0].position.z * -1); /// Flip Y Axis



        //                    vert[face[f].vertindex.v2] = new Vertex { };
        //                    vert[face[f].vertindex.v2].position = new Position { };



        //                    vert[face[f].vertindex.v2].position.x = Convert.ToInt16(RealVerts[face[f].vertindex.v2].position.x);

        //                    vert[face[f].vertindex.v2].position.y = Convert.ToInt16(RealVerts[face[f].vertindex.v2].position.z);

        //                    vert[face[f].vertindex.v2].position.z = Convert.ToInt16(RealVerts[face[f].vertindex.v2].position.y);    /// Flip YZ Axis 
        //                    vert[face[f].vertindex.v2].position.z = Convert.ToInt16(vert[face[f].vertindex.v2].position.z * -1); /// Flip Y Axis



        //                    vert[face[f].vertindex.v1] = new Vertex { };
        //                    vert[face[f].vertindex.v1].position = new Position { };


        //                    vert[face[f].vertindex.v1].position.x = Convert.ToInt16(RealVerts[face[f].vertindex.v1].position.x);

        //                    vert[face[f].vertindex.v1].position.y = Convert.ToInt16(RealVerts[face[f].vertindex.v1].position.z);

        //                    vert[face[f].vertindex.v1].position.z = Convert.ToInt16(RealVerts[face[f].vertindex.v1].position.y);    /// Flip YZ Axis 
        //                    vert[face[f].vertindex.v1].position.z = Convert.ToInt16(vert[face[f].vertindex.v1].position.z * -1); /// Flip Y Axis



        //                    vert[face[f].vertindex.v0].position.s = Convert.ToInt16(STwidth[TextureList[materialID].TextureClass]);
        //                    vert[face[f].vertindex.v0].position.t = Convert.ToInt16(-1 * STheight[TextureList[materialID].TextureClass]);

        //                    vert[face[f].vertindex.v2].position.s = Convert.ToInt16(STwidth[TextureList[materialID].TextureClass]);
        //                    vert[face[f].vertindex.v2].position.t = Convert.ToInt16(-1 * STheight[TextureList[materialID].TextureClass]);

        //                    vert[face[f].vertindex.v1].position.s = Convert.ToInt16(STwidth[TextureList[materialID].TextureClass]);
        //                    vert[face[f].vertindex.v1].position.t = Convert.ToInt16(-1 * STheight[TextureList[materialID].TextureClass]);


        //                }



        //                ///Ok so now that we've loaded the raw model data, let's start writing some F3DEX. God have mercy.





        //                ///
        //                ///Seg 7 is prepped to prevent hardcoded ASM overwrites
        //                ///0xB8 is an end list F3DEX command, which will return any ASM hardcoded call.
        //                ///F3DEX is all alligned to 32 bit addresses, no calls to weird offset.
        //                ///<3 Micro <3 you mad lad
        //                ///



        //                collisionoffsetlist[i].section.Add(Convert.ToInt32(seg7stream.Position));



        //                byte4 = BitConverter.GetBytes(0xBB000001);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0xFFFFFFFF);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);



        //                ImgType = ImgTypes[TextureList[materialID].TextureClass];
        //                ImgFlag1 = STheight[TextureList[materialID].TextureClass];
        //                ImgFlag2 = STwidth[TextureList[materialID].TextureClass];
        //                if (TextureList[materialID].TextureClass == 6)
        //                {
        //                    ImgFlag3 = 0x100;
        //                }
        //                else
        //                {
        //                    ImgFlag3 = 0x00;
        //                }



        //                byte4 = BitConverter.GetBytes(0xE8000000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0x00000000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);



        //                byte4 = BitConverter.GetBytes((((ImgType << 0x15) | 0xF5100000) | ((((ImgFlag2 << 1) + 7) >> 3) << 9)) | ImgFlag3);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(Convert.ToInt32(((heightex[TextureList[materialID].TextureClass] & 0xF) << 0x12) | (((heightex[TextureList[materialID].TextureClass] & 0xF0) >> 4) << 0xE) | ((widthex[TextureList[materialID].TextureClass] & 0xF) << 8) | (((widthex[TextureList[materialID].TextureClass] & 0xF0) >> 4) << 4)));
        //                byte4 = BitConverter.GetBytes(BitConverter.ToInt32(byte4, 0) >> 4);
        //                ///IDK why but this makes it into what it's supposed to be.
        //                Array.Reverse(byte4);

        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0xF2000000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes((((ImgFlag2 - 1) << 0xE) | ((ImgFlag1 - 1) << 2)));
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0xFD100000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0x05000000 | TextureList[materialID].Seg5Offset);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0xE8000000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0x00000000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                ///F5100000 07000000 E6000000 00000000 F3000000 073FF100

        //                byte4 = BitConverter.GetBytes(0xF5100000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0x07000000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0xE6000000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0x00000000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0xF3000000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0x073FF100);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);




        //                ///load the first set of verts from the relative_zero position;

        //                byte4 = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);


        //                byte4 = BitConverter.GetBytes(0x04000000 | relative_zero * 16);  ///from segment 4 at offset relative_zero
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);



        //                relative_index = 0;

        //                for (int x = 0; x < facecount;)
        //                {



        //                    int v0 = 0;
        //                    int v1 = 0;
        //                    int v2 = 0;


        //                    if (x + 2 <= facecount)
        //                    {
        //                        /// draw 2 triangles, check for additional verts in both.
        //                        if (face[x].vertindex.v0 > (relative_index + 31) | face[x].vertindex.v1 > (relative_index + 31) | face[x].vertindex.v2 > (relative_index + 31) | /// OR with next line
        //                            face[x + 1].vertindex.v0 > (relative_index + 31) | face[x + 1].vertindex.v1 > (relative_index + 31) | face[x + 1].vertindex.v2 > (relative_index + 31))
        //                        {

        //                            /// OVER VERT LIMIT, LOAD NEW VERTS
        //                            UInt16 maxvalue = Convert.ToUInt16(GetMax(face[x].vertindex.v0, face[x].vertindex.v1));
        //                            maxvalue = Convert.ToUInt16(GetMax(face[x].vertindex.v2, maxvalue));
        //                            maxvalue = Convert.ToUInt16(GetMax(face[x + 1].vertindex.v0, maxvalue));
        //                            maxvalue = Convert.ToUInt16(GetMax(face[x + 1].vertindex.v1, maxvalue));
        //                            maxvalue = Convert.ToUInt16(GetMax(face[x + 1].vertindex.v2, maxvalue));


        //                            if (maxvalue - 4 < 0)
        //                            {
        //                                maxvalue = 0;
        //                            }
        //                            else
        //                            {
        //                                maxvalue = Convert.ToUInt16(maxvalue - 4);
        //                            }
        //                            byte4 = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
        //                            Array.Reverse(byte4);
        //                            seg7writer.Write(byte4);


        //                            byte4 = BitConverter.GetBytes(0x04000000 | ((relative_zero + maxvalue) * 16));  ///from segment 4 at offset relative_zero
        //                            Array.Reverse(byte4);
        //                            seg7writer.Write(byte4);

        //                            relative_index = maxvalue;
        //                        }

        //                        if ((face[x].vertindex.v0 < relative_index) | (face[x].vertindex.v1 < relative_index) | (face[x].vertindex.v2 < relative_index) |
        //                            (face[x + 1].vertindex.v0 < relative_index) | (face[x + 1].vertindex.v1 < relative_index) | (face[x + 1].vertindex.v2 < relative_index))
        //                        {

        //                            /// UNDER VERT LIMIT, LOAD NEW VERTS
        //                            UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v0, face[x].vertindex.v1));
        //                            minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v2, minvalue));
        //                            minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v0, minvalue));
        //                            minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v1, minvalue));
        //                            minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v2, minvalue));

        //                            if (minvalue - 4 < 0)
        //                            {
        //                                minvalue = 0;
        //                            }
        //                            else
        //                            {
        //                                minvalue = Convert.ToUInt16(minvalue - 4);
        //                            }

        //                            byte4 = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
        //                            Array.Reverse(byte4);
        //                            seg7writer.Write(byte4);


        //                            byte4 = BitConverter.GetBytes(0x04000000 | ((relative_zero + minvalue) * 16));  ///from segment 4 at offset relative_zero
        //                            Array.Reverse(byte4);
        //                            seg7writer.Write(byte4);

        //                            relative_index = minvalue;
        //                        }


        //                        ///end vert check



        //                        v0 = face[x].vertindex.v0 - relative_index;
        //                        v1 = face[x].vertindex.v1 - relative_index;
        //                        v2 = face[x].vertindex.v2 - relative_index;


        //                        byte4 = BitConverter.GetBytes(Convert.ToUInt32(0xB1000000 | (v2 << 17) | (v1 << 9) | v0 << 1));
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);

        //                        v0 = face[x + 1].vertindex.v0 - relative_index;
        //                        v1 = face[x + 1].vertindex.v1 - relative_index;
        //                        v2 = face[x + 1].vertindex.v2 - relative_index;


        //                        byte4 = BitConverter.GetBytes(Convert.ToUInt32((v2 << 17) | (v1 << 9) | v0 << 1));
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);
        //                        x = x + 2;

        //                    }
        //                    else
        //                    {
        //                        /// draw 1 triangle, only 1 vert check

        //                        if (face[x].vertindex.v0 > (relative_index + 31) | face[x].vertindex.v1 > (relative_index + 31) | face[x].vertindex.v2 > (relative_index + 31))
        //                        {

        //                            /// OVER VERT LIMIT, LOAD NEW VERTS
        //                            UInt16 maxvalue = Convert.ToUInt16(GetMax(face[x].vertindex.v0, face[x].vertindex.v1));
        //                            maxvalue = Convert.ToUInt16(GetMax(face[x].vertindex.v2, maxvalue));


        //                            if (maxvalue - 4 < 0)
        //                            {
        //                                maxvalue = 0;
        //                            }
        //                            else
        //                            {
        //                                maxvalue = Convert.ToUInt16(maxvalue - 4);
        //                            }
        //                            byte4 = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
        //                            Array.Reverse(byte4);
        //                            seg7writer.Write(byte4);


        //                            byte4 = BitConverter.GetBytes(0x04000000 | ((relative_zero + maxvalue) * 16));  ///from segment 4 at offset relative_zero
        //                            Array.Reverse(byte4);
        //                            seg7writer.Write(byte4);

        //                            relative_index = maxvalue;
        //                        }

        //                        if ((face[x].vertindex.v0 < relative_index) | (face[x].vertindex.v1 < relative_index) | (face[x].vertindex.v2 < relative_index))
        //                        {

        //                            /// UNDER VERT LIMIT, LOAD NEW VERTS
        //                            UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v0, face[x].vertindex.v1));
        //                            minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v2, minvalue));

        //                            if (minvalue - 4 < 0)
        //                            {
        //                                minvalue = 0;
        //                            }
        //                            else
        //                            {
        //                                minvalue = Convert.ToUInt16(minvalue - 4);
        //                            }

        //                            byte4 = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
        //                            Array.Reverse(byte4);
        //                            seg7writer.Write(byte4);


        //                            byte4 = BitConverter.GetBytes(0x04000000 | ((relative_zero + minvalue) * 16));  ///from segment 4 at offset relative_zero
        //                            Array.Reverse(byte4);
        //                            seg7writer.Write(byte4);

        //                            relative_index = minvalue;
        //                        }





        //                        ///end vert check
        //                        byte4 = BitConverter.GetBytes(Convert.ToUInt32(0xBF000000));
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);

        //                        v0 = face[x].vertindex.v0 - relative_index;
        //                        v1 = face[x].vertindex.v1 - relative_index;
        //                        v2 = face[x].vertindex.v2 - relative_index;


        //                        byte4 = BitConverter.GetBytes(Convert.ToUInt32((v2 << 17) | (v1 << 9) | v0 << 1));
        //                        Array.Reverse(byte4);
        //                        seg7writer.Write(byte4);
        //                        x = x + 1;
        //                    }


        //                }



        //                byte4 = BitConverter.GetBytes(0xB8000000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);

        //                byte4 = BitConverter.GetBytes(0x00000000);
        //                Array.Reverse(byte4);
        //                seg7writer.Write(byte4);



        //                relative_zero = Convert.ToUInt16(relative_zero + RealVerts.Count);



        //                /// seg 4 needs no prep, can be written as it should be now that we have all the verts

        //                byte[] flip2 = new byte[2];
        //                for (int v = 0; v < RealVerts.Count; v++)
        //                {
        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.x));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.y));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.z));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.s));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte2 = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.t));
        //                    Array.Reverse(byte2);
        //                    seg4writer.Write(byte2);

        //                    byte RGB = 252;
        //                    byte Alpha = 0;
        //                    seg4writer.Write(RGB);
        //                    seg4writer.Write(RGB);
        //                    seg4writer.Write(RGB);
        //                    seg4writer.Write(Alpha);
        //                }



        //            }
        //        }



        //    }


        //    current_section = current_section + 1;




        //    fuck_me = false;/// ok now we can actually take a pause.


        //    /// Next step is going to be going into section 9 and writing the master display lists.
        //    /// So each course is different, but for Mario's Raceway section 9 contains the master display lists.
        //    /// these represent each section, NESW, and point towards an offset in segment 6. This offset contains
        //    /// a series of commands that draw that section. So, we seperated each collection of faces above by
        //    /// material, each time we drew a new material we recorded an offset, and each section has 4 views.
        //    /// So to draw an entire view, we need to write the offset for each material into Segment 6. Then, the offset
        //    /// where those commands are written in segment 6 is written into the master display list in segment 9.


        //    /// So it goes 
        //    ///            Segment 9 Master Display List (0 North) --> Segment 6 Display List(0 North Material A) -->  Segment 7 F3DEX (0 North Material A)
        //    ///                                                    +-> Segment 6 Display List(0 North Material B) -->  Segment 7 F3DEX (0 North Material B)
        //    ///                                                    +-> Segment 6 Display List(0 North Material C) -->  Segment 7 F3DEX (0 North Material C)
        //    ///                                                              ... etc
        //    ///                                                    +-> 0xB8000000 00000000
        //    ///
        //    ///
        //    ///            Segment 9 Master Display List(0 East)-- > Segment 6 Display List(0 East Material A) --> Segment 7 F3DEX(0 East Material A)
        //    ///                                                    +-> Segment 6 Display List(0 East Material B) -->  Segment 7 F3DEX (0 East Material B)
        //    ///                                                    +-> Segment 6 Display List(0 East Material C) -->  Segment 7 F3DEX (0 East Material C)
        //    ///                                                              ... etc
        //    ///                                                    +-> 0xB8000000 00000000
        //    ///                                                    
        //    seg6 = seg6stream.ToArray();


        //    //Now we want to write the pathmarker and object lists, and add those to Segment 6. We have to mark the offsets
        //    // of each of the path lists and object lists we add. We can't add more or less lists than the game originally had,
        //    // but we can add empty lists or add more items to existing lists to help alleviate that issue.
        //    // Later we'll modify some ASM routines with the new offsets for our paths and objects.

        //    int[] pathaddresses = new int[0];
        //    int[] objaddresses = new int[0];
        //    InjectMarkers(seg6, 0, pathmarkers, false, out seg6, out pathaddresses);
        //    InjectMarkers(seg6, 0, objectmarkers, true, out seg6, out objaddresses);


        //    seg6writer.BaseStream.Position = 0;
        //    seg6writer.Write(seg6);


        //    //Now we'll write all the surface map display lists, using the offsets we stored earlier.
        //    //But first we store the offset to this list, so that we can edit the ASM later.
        //    SurfaceMapOffset = Convert.ToInt32(seg6writer.BaseStream.Position);


        //    int collIDcount = 0;
        //    for (int i = 0; i < collisionoffsetlist.Count; i++)
        //    {

        //        foreach (int offset in collisionoffsetlist[i].section)
        //        {


        //            byte4 = BitConverter.GetBytes(0x07000000 | offset);
        //            Array.Reverse(byte4);
        //            seg6writer.Write(byte4);


        //            byte material = Convert.ToByte(collisionIDs[collIDcount]);
        //            byte area = Convert.ToByte(areaIDs[collIDcount] + 1);


        //            seg6writer.Write(material);
        //            seg6writer.Write(area);

        //            seg6writer.Write(Convert.ToUInt16(0));
        //            collIDcount = collIDcount + 1;
        //        }

        //    }

        //    ///End of Surface Map
        //    byte4 = BitConverter.GetBytes(0x00000000);
        //    Array.Reverse(byte4);
        //    seg6writer.Write(byte4);
        //    byte4 = BitConverter.GetBytes(0x00000000);
        //    Array.Reverse(byte4);
        //    seg6writer.Write(byte4);
        //    ///End of Surface Map



        //    ///now we can write the master display list. Weird order, I know, but we're overwriting the game's existing data.
        //    /// so it's all out of order and referencing it's future self.


        //    List<int> mdisplayoffsets = new List<int>();



        //    for (int sv = 1; sv < section_view.LongLength;)
        //    {
        //        string section_name = section_view[sv];
        //        sv++;
        //        mdisplayoffsets.Add(Convert.ToInt32(seg6writer.BaseStream.Position));
        //        subsection_count = Convert.ToInt32(section_view[sv]);
        //        sv++;
        //        for (int ssc = 0; ssc < subsection_count; ssc++)
        //        {
        //            string search_object = section_view[sv];
        //            sv++;

        //            string[] objname_array = object_name.ToArray();


        //            int object_index = Array.IndexOf(objname_array, search_object);


        //            byte4 = BitConverter.GetBytes(0x06000000);
        //            Array.Reverse(byte4);
        //            seg6writer.Write(byte4);

        //            byte4 = BitConverter.GetBytes(offsetlist.section[object_index] | 0x07000000);
        //            Array.Reverse(byte4);
        //            seg6writer.Write(byte4);





        //        }

        //        byte4 = BitConverter.GetBytes(0xB8000000);
        //        Array.Reverse(byte4);
        //        seg6writer.Write(byte4);

        //        byte4 = BitConverter.GetBytes(0x00000000);
        //        Array.Reverse(byte4);
        //        seg6writer.Write(byte4);


        //    }






        //    // Write the master display list onto the end of Segment 9 that we made earlier.
        //    for (int i = 0; i < mdisplayoffsets.Count; i++)
        //    {
        //        byte4 = BitConverter.GetBytes(0x06000000 | mdisplayoffsets[i]);
        //        Array.Reverse(byte4);
        //        seg9writer.Write(byte4);
        //    }



        //    fuck_me = true;
        //    //Oh yeah this is definitely coming back on for this part you bet your sweet ass.





        //    // We'll now make a series of edits to the game's ASM instructions.
        //    // We will be replacing the original instructions for creating the offsets
        //    // to the course data with the new positions, as well as changing the instruction
        //    // type used to load the data to make it easier to calculate.

        //    // Originally, in a lot of places an LUI/ADDIU combination was used. 
        //    // Because ADDIU uses signed integers you would have to increase the ADDIU by 1 
        //    // and then add the value as a negative. 
        //    // 0x06010000 + 0x9570 (-0x6a90) = 0x06009570
        //    // that's really complicated. I don't want to do that. 
        //    // ORI uses unsigned values. That's easy
        //    // So we overwrite the game to do that instead.




        //    //The first one will edit the location of the SectionView Table in segment 9.
        //    romwriter.BaseStream.Position = 0xFBE0C;
        //    ASMInstructions = BitConverter.GetBytes(0x3C040900);
        //    // Load 0x0900 into the first half of the offset register
        //    romwriter.Write(ASMInstructions);
        //    ASMInstructions = BitConverter.GetBytes(0x34840000 | SectionViewOffset);
        //    // load the SectionView into the second half of the register.




        //    romwriter.BaseStream.Position = 0xFF484;
        //    ASMInstructions = BitConverter.GetBytes(0x3C040600);
        //    // Load 0x0600 into the first half of the offset register
        //    romwriter.Write(ASMInstructions);
        //    ASMInstructions = BitConverter.GetBytes(0x34840000 | SurfaceMapOffset);
        //    // load the SurfaceMap offset into the second half of the register.





        //    //The following takes out the HardCoded Objects for the course.
        //    // Each Course has several objects or routines drawn by ASM commands.
        //    // These must be replaced with a 0xB8 endlist command. This immediately ends the routine
        //    // before it can draw or load anything.
        //    // If we did not do this, the ASM would load arbitrary data and cause a crash.

        //    // Each blanking offset is an ASM routine that either uses a LUI/ADDIU or LUI/ORI combo
        //    // The first line sets the segment, the second the offset into the segment.
        //    // We want them to always be LUI/ORI for the same as above.
        //    // Else if we overwrite a LUI/ADDIU that was over the sign limit we also need
        //    // to change the segment from 0601 / 0701 to 0600 / 0700. That's complex.
        //    // For the same as above, instead we just always use the LUI/ORI combo.

        //    // In order to properly translate the command without knowing what it is, we
        //    // read what registers it was originally going to edit and what segment it was 
        //    // intended to write to (0601 / 0600 = Segment 6). Then, we write our own ASM
        //    // routine to load the offset using an LUI/ORI combo with those same parameters.
        //    // except the offset is always going to be 0x0000.

        //    // Then at the start of Segments 6 and 7 we place a single 0xB8 command.
        //    // This replaces the need to overwrite the entire Segment with 0xB8 commands



        //    //These all come from Micro <3, like everything else in this program

        //    int[] BlankingOffsets = new int[] { 0xFA7F8, 0xFA81C, 0xFA7B4, 0xFBDDC, 0xFBDFC, 0xFBE88, 0xFBEA8, 0xFBECC, 0xFBF00, 0x10C0B4,};


        //    foreach (int blank in BlankingOffsets)
        //    {
        //        //First load the segment ID and set it from 0600/0601 to 0600, or 0700/0701 to 0700.
        //        romwriter.BaseStream.Position = blank;
        //        UInt16 input = romreader.ReadUInt16();
        //        UInt16 registervalues = Convert.ToUInt16(input & 0x03FF);
        //        UInt16 output = Convert.ToUInt16(registervalues & 0x3C00);
        //        romwriter.BaseStream.Position = blank;
        //        romwriter.Write(output);

        //        UInt16 SegID = Convert.ToUInt16(romreader.ReadByte()); // Read only the first byte of the segment address, ignore the 00 / 01
        //        romwriter.BaseStream.Position = blank + 2;
        //        romwriter.Write(SegID);


        //        //Replace with an ORI command and set the address to 0.

        //        romwriter.BaseStream.Position = blank+4;
        //        input = romreader.ReadUInt16();
        //        registervalues = Convert.ToUInt16(input & 0x03FF);
        //        output = Convert.ToUInt16(registervalues & 0x3400);
        //        romwriter.BaseStream.Position = blank;
        //        romwriter.Write(output);
        //        romwriter.Write(Convert.ToUInt16(0));

        //    }


        //    fuck_me = false;





        //    //Now we're going to compile the ROM and spit the segment data out.
        //    //That way, if the game doesnt run the user can quickly load
        //    //and debug their segment data.


        //    string outpath = "";




        //    useg4 = seg4stream.ToArray();

        //    seg4 = compress_MIO0(useg4, 0);


        //    useg7 = seg7stream.ToArray();


        //    seg7 = compress_seg7(useg7);

        //    useg6 = seg6stream.ToArray();

        //    seg6 = compress_MIO0(useg6, 0);

        //    seg9 = seg9stream.ToArray(); 
        //    // create Segment 9 from the memorystream we've been building



        //    List<byte[]> outbyte = new List<byte[]>();

        //    outbyte.Add(seg4);
        //    outbyte.Add(seg6);
        //    outbyte.Add(seg7);
        //    outbyte.Add(seg9);
        //    outbyte.Add(useg4);
        //    outbyte.Add(useg6);
        //    outbyte.Add(useg7);

        //    return outbyte;








        //}



        public OK64Texture textureClass(OK64Texture textureObject)
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


        public byte[] writeTextures(byte[] rom, OK64Texture[] textureObject)
        {
            bs = new MemoryStream();
            br = new BinaryReader(bs);
            bw = new BinaryWriter(bs);

            int segment5Position = 0;

            MemoryStream seg9m = new MemoryStream();
            BinaryReader seg9r = new BinaryReader(seg9m);
            BinaryWriter seg9w = new BinaryWriter(seg9m);

            bw.Write(rom);
            int textureCount = (textureObject.Length);
            for (int currentTexture = 0; currentTexture < textureCount; currentTexture++)
            {

                // Establish codec and convert texture. Compress converted texture data via MIO0 compression

                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(textureObject[currentTexture].texturePath);
                N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].textureFormat], bitmapData);
                byte[] compressedTexture = fakeCompress(imageData);


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

            bw.Write(Convert.ToInt32(0));
            bw.Write(Convert.ToInt32(0));
            bw.Write(Convert.ToInt32(0));
            bw.Write(Convert.ToInt32(0));


            byte[] romOut = bs.ToArray();
            return romOut;


        }


        public byte[] compiletextureTable(OK64Texture[] textureObject)
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





        public byte[] compileF3DObject(ref int outMagic, ref byte[] seg4out, AssimpSharp.Scene fbx, byte[] segment7, OK64F3DObject[] courseObject, OK64Texture[] textureObject, int vertMagic)
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


                    

                    int vertcount = current_subobject.Vertices.Length;
                    ///MessageBox.Show(currentnode.Name+"-"+child.Name + "-Vert Count-"+vertcount.ToString());
                    int facecount = current_subobject.Faces.Length;
                    ///MessageBox.Show(currentnode.Name + "-" + child.Name + "-Face Count-" + facecount.ToString());


                    if (facecount > 30)
                    {
                        MessageBox.Show("Warning Subobject with more than 30 Faces. - " + cObj.objectName);
                    }

                    int materialID = new int();

                    Face[] face = new Face[facecount];
                    Vertex[] vert = new Vertex[current_subobject.Vertices.Length];

                    int collision_out = new int();

                    materialID = cObj.materialID;

                    


                    int intread = 0;


                    for (int f = 0; f < facecount; f++)
                    {
                        


                        face[f] = new Face { };
                        face[f].vertindex = new VertIndex { };

                        face[f].vertindex.v0 = current_subobject.Faces[f].Indices[0];

                        face[f].vertindex.v2 = current_subobject.Faces[f].Indices[1];

                        face[f].vertindex.v1 = current_subobject.Faces[f].Indices[2]; ;

                        face[f].material = materialID;

                        vert[face[f].vertindex.v0] = new Vertex { };
                        vert[face[f].vertindex.v0].position = new Position { };


                        vert[face[f].vertindex.v0].position.x = Convert.ToInt16(current_subobject.Vertices[face[f].vertindex.v0].X);

                        vert[face[f].vertindex.v0].position.y = Convert.ToInt16(current_subobject.Vertices[face[f].vertindex.v0].Y);
                        /// Flip YZ Axis 
                        vert[face[f].vertindex.v0].position.z = Convert.ToInt16(current_subobject.Vertices[face[f].vertindex.v0].Z);    /// Flip YZ Axis 
                        vert[face[f].vertindex.v0].position.z = Convert.ToInt16(vert[face[f].vertindex.v0].position.z); /// Flip Y Axis


                       
                        //
                        


                        vert[face[f].vertindex.v2] = new Vertex { };
                        vert[face[f].vertindex.v2].position = new Position { };

                        vert[face[f].vertindex.v2].position.x = Convert.ToInt16(current_subobject.Vertices[face[f].vertindex.v2].X);

                        vert[face[f].vertindex.v2].position.y = Convert.ToInt16(current_subobject.Vertices[face[f].vertindex.v2].Y);

                        vert[face[f].vertindex.v2].position.z = Convert.ToInt16(current_subobject.Vertices[face[f].vertindex.v2].Z);    /// Flip YZ Axis 
                        vert[face[f].vertindex.v2].position.z = Convert.ToInt16(vert[face[f].vertindex.v2].position.z); /// Flip Y Axis



                        //

                        vert[face[f].vertindex.v1] = new Vertex { };
                        vert[face[f].vertindex.v1].position = new Position { };


                        vert[face[f].vertindex.v1].position.x = Convert.ToInt16(current_subobject.Vertices[face[f].vertindex.v1].X);

                        vert[face[f].vertindex.v1].position.y = Convert.ToInt16(current_subobject.Vertices[face[f].vertindex.v1].Y);

                        vert[face[f].vertindex.v1].position.z = Convert.ToInt16(current_subobject.Vertices[face[f].vertindex.v1].Z);    /// Flip YZ Axis 
                        vert[face[f].vertindex.v1].position.z = Convert.ToInt16(vert[face[f].vertindex.v1].position.z); /// Flip Y Axis



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



                        u_offset[0] = Convert.ToSingle(current_subobject.TextureCoords[0][face[f].vertindex.v0][0]);
                        v_offset[0] = Convert.ToSingle(current_subobject.TextureCoords[0][face[f].vertindex.v0][1]);

                        u_offset[1] = Convert.ToSingle(current_subobject.TextureCoords[0][face[f].vertindex.v1][0]);
                        v_offset[1] = Convert.ToSingle(current_subobject.TextureCoords[0][face[f].vertindex.v1][1]);

                        u_offset[2] = Convert.ToSingle(current_subobject.TextureCoords[0][face[f].vertindex.v2][0]);
                        v_offset[2] = Convert.ToSingle(current_subobject.TextureCoords[0][face[f].vertindex.v2][1]);

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
                            MessageBox.Show(u_offset[0].ToString() + "-" + v_offset[0].ToString() + " - UV 0 Out of Range for Object - " + cObj.objectName);
                        }
                        vert[face[f].vertindex.v0].position.s = Convert.ToInt16(s_coord);
                        vert[face[f].vertindex.v0].position.t = Convert.ToInt16(t_coord);


                        //


                        s_coord = Convert.ToInt32(u_offset[1] * STwidth[textureObject[materialID].textureClass] * 32);
                        t_coord = Convert.ToInt32(v_offset[1] * STheight[textureObject[materialID].textureClass] * -32);


                        if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                        {
                            MessageBox.Show(u_offset[1].ToString() + "-" + v_offset[1].ToString() + " UV 1 Out of Range for Object - " + cObj.objectName);
                        }

                        vert[face[f].vertindex.v1].position.s = Convert.ToInt16(s_coord);
                        vert[face[f].vertindex.v1].position.t = Convert.ToInt16(t_coord);


                        //


                        s_coord = Convert.ToInt32(u_offset[2] * STwidth[textureObject[materialID].textureClass] * 32);
                        t_coord = Convert.ToInt32(v_offset[2] * STheight[textureObject[materialID].textureClass] * -32);



                        if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                        {
                            MessageBox.Show(u_offset[2].ToString() + "-" + v_offset[2].ToString() + " UV 2 Out of Range for Object - " + cObj.objectName);
                        }


                        vert[face[f].vertindex.v2].position.s = Convert.ToInt16(s_coord);
                        vert[face[f].vertindex.v2].position.t = Convert.ToInt16(t_coord);


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
                            if (face[x].vertindex.v0 > (relativeIndex + 31) | face[x].vertindex.v1 > (relativeIndex + 31) | face[x].vertindex.v2 > (relativeIndex + 31) | /// OR with next line
                                                face[x + 1].vertindex.v0 > (relativeIndex + 31) | face[x + 1].vertindex.v1 > (relativeIndex + 31) | face[x + 1].vertindex.v2 > (relativeIndex + 31))
                            {

                                /// OVER VERT LIMIT, LOAD NEW VERTS
                                UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v0, face[x].vertindex.v1));
                                minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v2, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v0, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v1, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v2, minvalue));


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

                            if ((face[x].vertindex.v0 < relativeIndex) | (face[x].vertindex.v1 < relativeIndex) | (face[x].vertindex.v2 < relativeIndex) |
                                (face[x + 1].vertindex.v0 < relativeIndex) | (face[x + 1].vertindex.v1 < relativeIndex) | (face[x + 1].vertindex.v2 < relativeIndex))
                            {

                                /// UNDER VERT LIMIT, LOAD NEW VERTS
                                UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v0, face[x].vertindex.v1));
                                minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v2, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v0, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v1, minvalue));
                                minvalue = Convert.ToUInt16(GetMin(face[x + 1].vertindex.v2, minvalue));

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



                            v0 = face[x].vertindex.v0 - relativeIndex;
                            v1 = face[x].vertindex.v1 - relativeIndex;
                            v2 = face[x].vertindex.v2 - relativeIndex;


                            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0xB1000000 | (v2 << 17) | (v1 << 9) | v0 << 1));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);

                            v0 = face[x + 1].vertindex.v0 - relativeIndex;
                            v1 = face[x + 1].vertindex.v1 - relativeIndex;
                            v2 = face[x + 1].vertindex.v2 - relativeIndex;

                            byteArray = BitConverter.GetBytes(Convert.ToUInt32((v2 << 17) | (v1 << 9) | v0 << 1));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);
                            x = x + 2;

                        }
                        else
                        {
                            /// draw 1 triangle, only 1 vert check

                            if (face[x].vertindex.v0 > (relativeIndex + 31) | face[x].vertindex.v1 > (relativeIndex + 31) | face[x].vertindex.v2 > (relativeIndex + 31))
                            {

                                /// OVER VERT LIMIT, LOAD NEW VERTS
                                UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v0, face[x].vertindex.v1));
                                minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v2, minvalue));


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

                            if ((face[x].vertindex.v0 < relativeIndex) | (face[x].vertindex.v1 < relativeIndex) | (face[x].vertindex.v2 < relativeIndex))
                            {

                                /// UNDER VERT LIMIT, LOAD NEW VERTS
                                UInt16 minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v0, face[x].vertindex.v1));
                                minvalue = Convert.ToUInt16(GetMin(face[x].vertindex.v2, minvalue));

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

                            v0 = face[x].vertindex.v0 - relativeIndex;
                            v1 = face[x].vertindex.v1 - relativeIndex;
                            v2 = face[x].vertindex.v2 - relativeIndex;


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






                    for (int v = 0; v < vert.Length; v++)
                    {
                        byteArray = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.x));
                        Array.Reverse(byteArray);
                        seg4w.Write(byteArray);

                        byteArray = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.y));
                        Array.Reverse(byteArray);
                        seg4w.Write(byteArray);

                        byteArray = BitConverter.GetBytes(Convert.ToInt16(vert[v].position.z));
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
            seg4out = seg4m.ToArray();
            byte[] seg7out = seg7m.ToArray();
            outMagic = relativeZero;

            return seg7out.ToArray();



        }

        public byte[] compileF3DList(ref OK64SectionList[] sectionOut, AssimpSharp.Scene fbx, OK64F3DObject[] courseObject, OK64SectionList[] sectionList)
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



        public byte[] compilesurfaceTable(OK64F3DObject[] surfaceObject)
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


        public byte[] compilesectionviewTable(OK64SectionList[] sectionList, int magic)
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


        public byte[] compilesectionviewTable(byte[] segment9, OK64SectionList[] sectionList)
        {
            MemoryStream seg9m = new MemoryStream();
            BinaryReader seg9r = new BinaryReader(seg9m);
            BinaryWriter seg9w = new BinaryWriter(seg9m);

            byte[] byteArray = new byte[0];



            if (segment9.Length > 0)
            {
                seg9w.Write(segment9);
            }

            for (int currentSection = 0; currentSection < sectionList.Length; currentSection++)
            {
                for (int currentView = 0; currentView < 4; currentView++)
                {
                    byteArray = BitConverter.GetBytes(0x06000000 | sectionList[currentSection].viewList[currentView].segmentPosition);
                    Array.Reverse(byteArray);
                    seg9w.Write(byteArray);


                }
            }
            byte[] seg9Out = seg9m.ToArray();
            return seg9Out;
        }



        private void marioracewayASM()
        {
            //// A series of edits to the ASM instructions for Mario Raceway. 
            //    //The first one will edit the location of the SectionView Table in segment 9.
            //    romwriter.BaseStream.Position = 0xFBE0C;
            //    ASMInstructions = BitConverter.GetBytes(0x3C040900);
            //    // Load 0x0900 into the first half of the offset register
            //    romwriter.Write(ASMInstructions);
            //    ASMInstructions = BitConverter.GetBytes(0x34840000 | SectionViewOffset);
            //    // load the SectionView into the second half of the register.




            //romwriter.BaseStream.Position = 0xFF484;
            //ASMInstructions = BitConverter.GetBytes(0x3C040600);
            //// Load 0x0600 into the first half of the offset register
            //romwriter.Write(ASMInstructions);
            //ASMInstructions = BitConverter.GetBytes(0x34840000 | SurfaceMapOffset);






            // load the SurfaceMap offset into the second half of the register.





            //The following takes out the HardCoded Objects for the course.
            // Each Course has several objects or routines drawn by ASM commands.
            // These must be replaced with a 0xB8 endlist command. This immediately ends the routine
            // before it can draw or load anything.
            // If we did not do this, the ASM would load arbitrary data and cause a crash.

            // Each blanking offset is an ASM routine that either uses a LUI/ADDIU or LUI/ORI combo
            // The first line sets the segment, the second the offset into the segment.
            // We want them to always be LUI/ORI for the same as above.
            // Else if we overwrite a LUI/ADDIU that was over the sign limit we also need
            // to change the segment from 0601 / 0701 to 0600 / 0700. That's complex.
            // For the same as above, instead we just always use the LUI/ORI combo.

            // In order to properly translate the command without knowing what it is, we
            // read what registers it was originally going to edit and what segment it was 
            // intended to write to (0601 / 0600 = Segment 6). Then, we write our own ASM
            // routine to load the offset using an LUI/ORI combo with those same parameters.
            // except the offset is always going to be 0x0000.

            // Then at the start of Segments 6 and 7 we place a single 0xB8 command.
            // This replaces the need to overwrite the entire Segment with 0xB8 commands



            //These all come from Micro <3, like everything else in this program

            int[] BlankingOffsets = new int[] { 0xFA7F8, 0xFA81C, 0xFA7B4, 0xFBDDC, 0xFBDFC, 0xFBE88, 0xFBEA8, 0xFBECC, 0xFBF00, 0x10C0B4, };


            //foreach (int blank in BlankingOffsets)
            //{
            //    //First load the segment ID and set it from 0600/0601 to 0600, or 0700/0701 to 0700.
            //    romwriter.BaseStream.Position = blank;
            //    UInt16 input = romreader.ReadUInt16();
            //    UInt16 registervalues = Convert.ToUInt16(input & 0x03FF);
            //    UInt16 output = Convert.ToUInt16(registervalues & 0x3C00);
            //    romwriter.BaseStream.Position = blank;
            //    romwriter.Write(output);

            //    UInt16 SegID = Convert.ToUInt16(romreader.ReadByte()); // Read only the first byte of the segment address, ignore the 00 / 01
            //    romwriter.BaseStream.Position = blank + 2;
            //    romwriter.Write(SegID);


            //    //Replace with an ORI command and set the address to 0.

            //    romwriter.BaseStream.Position = blank + 4;
            //    input = romreader.ReadUInt16();
            //    registervalues = Convert.ToUInt16(input & 0x03FF);
            //    output = Convert.ToUInt16(registervalues & 0x3400);
            //    romwriter.BaseStream.Position = blank;
            //    romwriter.Write(output);
            //    romwriter.Write(Convert.ToUInt16(0));

            //}

        }


    }

}

// phew! We made it. I keep saying we, but it's me doing all the work!
// maybe try pitching in sometime and updating the program! I'd love the help!

// Thank you so much for-a reading my source!

// OverKart 64 Library
// For Mario Kart 64 1.0 USA ROM
// <3 Hamp











