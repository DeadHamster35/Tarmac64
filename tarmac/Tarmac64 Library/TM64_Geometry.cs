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


//custom libraries

using Assimp;  //for handling model data
using Texture64;  //for handling texture data


using Cereal64.Microcodes.F3DEX.DataElements;
using Cereal64.Microcodes.F3DEX.DataElements;
using Cereal64.Common.DataElements;
using Cereal64.Common.Rom;
using Cereal64.Common.Utils.Encoding;
using System.Text.RegularExpressions;
using System.Security.Permissions;
using SharpDX;
using System.Windows;
using Tarmac64;
using System.Windows.Media;

namespace Tarmac64_Geometry
{
    public class TM64_Geometry
    {

        /// These are various functions for decompressing and handling the segment data for Mario Kart 64.

        string[] viewString = new string[4] { "North", "East", "South", "West" };

        public static int newint = 4;
        Random rValue = new Random();

        MemoryStream memoryStream = new MemoryStream();
        BinaryReader binaryReader = new BinaryReader(Stream.Null);
        BinaryWriter binaryWriter = new BinaryWriter(Stream.Null);


        

        public static UInt32[] seg7_romptr = new UInt32[20];

        byte[] flip2 = new byte[2];
        byte[] flip4 = new byte[4];

        UInt16 value16 = new UInt16();
        Int16 valuesign16 = new Int16();

        UInt32 value32 = new UInt32();


        /// These classes are used by the underlying functions.



        /// These are used by the Geometry Builder
        /// 


        public class Header
        {
            public byte[] s6Start { get; set; }
            public byte[] s6End { get; set; }
            public byte[] s47Start { get; set; }
            public byte[] s47End { get; set; }
            public byte[] s9Start { get; set; }
            public byte[] s9End { get; set; }
            public byte[] s47Buffer { get; set; }
            public byte[] vertCount { get; set; }
            public byte[] s7Pointer { get; set; }
            public byte[] s7Size { get; set; }
            public byte[] texturePointer { get; set; }
            public byte[] flagPadding { get; set; }
        }

        public class Face
        {
            public VertIndex vertIndex { get; set; }
            public int material { get; set; }
            public Vertex[] vertData { get; set; }
            public Vector3D centerPosition { get; set; }
            public float highX { get; set; }
            public float highY { get; set; }
            public float lowX { get; set; }
            public float lowY { get; set; }
        }

        public class VertIndex
        {
            public int indexA { get; set; }
            public int indexB { get; set; }
            public int indexC { get; set; }

        }


        public class TMCamera
        {
            public Vector3D position { get; set; }
            public Vector3D target { get; set; }
            public Face[] targetPylon { get; set; }
            public double rotation { get; set; }
            
        }




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
            public bool textureTransparent { get; set; }
            public Image textureBitmap { get; set; }
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
            public Face[] modelGeometry { get; set; }
            public float[] objectColor { get; set; }
            public PathfindingObject pathfindingObject { get; set; }
        }
        public class PathfindingObject
        {
            public float highX { get; set; }
            public float highY { get; set; }
            public float lowX { get; set; }
            public float lowY { get; set; }

            public bool surfaceBoolean { get; set; }

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





        ///
        ///
        ///
        ///End of classes

        ///
        ///
        ///


        public Header[] loadHeader(byte[] fileData)
        {
            

            Header[] courseHeader = new Header[20];

            memoryStream = new MemoryStream(fileData);
            binaryReader = new BinaryReader(memoryStream);

            binaryReader.BaseStream.Seek(0x122390, SeekOrigin.Begin);
            for (int i = 0; i < 20; i++)
            {
                courseHeader[i] = new Header();

                courseHeader[i].s6Start = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s6Start);

                courseHeader[i].s6End = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s6End);

                courseHeader[i].s47Start = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s47Start);

                courseHeader[i].s47End = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s47End);

                courseHeader[i].s9Start = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s9Start);

                courseHeader[i].s9End = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s9End);

                courseHeader[i].s47Buffer = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s47Buffer);

                courseHeader[i].vertCount = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].vertCount);

                courseHeader[i].s7Pointer = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s7Pointer);

                courseHeader[i].s7Size = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s7Size);

                courseHeader[i].texturePointer = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].texturePointer);

                courseHeader[i].flagPadding = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].flagPadding);

            }


            return courseHeader;
        }




        public byte[] compileSegment(byte[] seg4, byte[] seg6, byte[] seg7, byte[] seg9, byte[] fileData, int cID)
        {


            ///This takes precompiled segments and inserts them into the ROM file. It also updates the course header table to reflect
            /// the new data sizes. This allows for proper loading of the course so long as the segments are properly setup. All segment
            /// data should be precompressed where applicable, this assumes that segment 4 and segment 6 are MIO0 compressed and that
            /// Segment 7 has had it's special compression ran. Segment 9 has no compression. fileData is the ROM file as a byte array, and CID
            /// is the ID of the course we're looking to replace based on it's location in the course header table. 


            /// This writes all segments to the end of the file for simplicity. If data was larger than original (which it almost always will be for custom courses)
            /// then it cannot fit in the existing space without overwriting other course data. 

            TM64_Geometry mk = new TM64_Geometry();
            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);
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

            byte[] compseg6 = compressMIO0(seg6);
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
            byte[] compseg4 = compressMIO0(seg4);
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

        public byte[] compileHotswap(byte[] useg4, byte[] useg6, byte[] useg7, byte[] seg9, string courseName, string previewImage, string bannerImage, string mapImage, Int16[] mapCoords, string customASM, string ghostData, byte[] skyColor, byte songID, byte[] gameSpeed, byte[] fileData, int cID, int setID)
        {


            ///This takes precompiled segments and inserts them into the ROM file. It also updates the course header table to reflect
            /// the new data sizes. This allows for proper loading of the course so long as the segments are properly setup. All segment
            /// data should be precompressed where applicable, this assumes that segment 4 and segment 6 are MIO0 compressed and that
            /// Segment 7 has had it's special compression ran. Segment 9 has no compression. fileData is the ROM file as a byte array, and CID
            /// is the ID of the course we're looking to replace based on it's location in the course header table. 


            /// This writes all segments to the end of the file for simplicity. If data was larger than original (which it almost always will be for custom courses)
            /// then it cannot fit in the existing space without overwriting other course data. 
            /// 

            byte[] seg6 = compressMIO0(useg6);
            byte[] seg4 = compressMIO0(useg4);
            byte[] seg7 = compress_seg7(useg7);


            byte[] flip = new byte[0];

            TM64_Geometry mk = new TM64_Geometry();
            memoryStream = new MemoryStream();
            memoryStream.Write(fileData, 0, fileData.Length);
            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);

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
            int mapOffset = 0;
            int coordOffset = 0;
            int nameOffset = 0;
            int ghostOffset = 0;
            int skyOffset = 0;

            int previewEnd = 0;
            int bannerEnd = 0;
            int mapEnd = 0;
            int coordEnd = 0;
            int nameEnd = 0;
            int ghostEnd = 0;
            int skyEnd = 0;



            int asmOffset = 0;
            int asmEnd = 0;

            binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;



            //allignment

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            //







            //Internal Name
            if (courseName.Length > 0)
            {
                nameOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);

                binaryWriter.Write(courseName);   //using a length-defined as opposed to null terminated setup.
                                        //easier to program for writing, easier to program for reading. 
                

                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                nameEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            }
            //


            //Staff Ghost
            if (ghostData.Length > 0)
            {
                ghostOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);

                binaryWriter.Write(courseName);   //using a length-defined as opposed to null terminated setup.
                                        //easier to program for writing, easier to program for reading. 
                

                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                ghostEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            }
            //


            //Write Course Map Texture
            if (mapImage.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(mapImage);
                N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                byte[] compressedData = compressMIO0(imageData);


                mapOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(compressedData);
                



                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                mapEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            }
            //
            //map coords

            coordOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);

            flip = BitConverter.GetBytes(mapCoords[0]);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            flip = BitConverter.GetBytes(mapCoords[1]);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            coordEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);

            //




            //add sky colors


            skyOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[0]);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[1]);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[2]);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[3]);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[4]);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[5]);

            

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            skyEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            //









            //custom ASM
            if (customASM.Length > 0)
            {


                byte[] asmSequence = File.ReadAllBytes(customASM);

                asmOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(asmSequence);
                




                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                asmEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            }
            //







            //Course Preview Texture
            if (previewImage.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(previewImage);
                N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                byte[] compressedData = compressMIO0(imageData);


                previewOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(compressedData);
                



                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                previewEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            }
            //



            //Write Course Banner Texture
            if (bannerImage.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(bannerImage);
                N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                byte[] compressedData = compressMIO0(imageData);


                bannerOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(compressedData);
                


                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                bannerEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            }
            //




            // Segment 6

            seg6start = Convert.ToUInt32(binaryWriter.BaseStream.Position);


            binaryWriter.Write(seg6, 0, seg6.Length);

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            seg6end = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            //


            // Segment 9
            seg9start = Convert.ToUInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(seg9, 0, seg9.Length);

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            seg9end = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            //




            // Segment 4/7
            seg4start = Convert.ToUInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(seg4, 0, seg4.Length);

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            seg7start = Convert.ToUInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(seg7, 0, seg7.Length);


            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            seg7end = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            seg7rsp = Convert.ToUInt32(0x0F000000 | (seg7start - seg4start));
            //






            // Flip Endian on Course Header offsets.

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
            //


            //calculate # verts

            UInt32 vertcount = Convert.ToUInt32(useg4.Length / 14);
            flip = BitConverter.GetBytes(vertcount);
            Array.Reverse(flip);
            vertcount = BitConverter.ToUInt32(flip, 0);
            //



            //seg7 size

            UInt32 seg7size = Convert.ToUInt32(useg7.Length);
            flip = BitConverter.GetBytes(seg7size);
            Array.Reverse(flip);
            seg7size = BitConverter.ToUInt32(flip, 0);
            //


            /// After Calculating the offsets and values above we now write them to the empty space near the end of the ROM.






            binaryWriter.BaseStream.Seek(0xBFDA80 + (setID * 0x7D0) + (cID * 0x64), SeekOrigin.Begin);




            //Internal Course Names
            
            flip = BitConverter.GetBytes(nameOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(nameEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //


            //Write staff ghost offset
            flip = BitConverter.GetBytes(ghostOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(ghostEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //

            //Speed and Song
            //WRITTEN DIRECTLY TO HEADER.

            binaryWriter.Write(songID);
            binaryWriter.Write(gameSpeed[0]);
            binaryWriter.Write(gameSpeed[1]);
            binaryWriter.Write(gameSpeed[2]);
            //



            //Write course minimap offset
            
            flip = BitConverter.GetBytes(mapOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(mapEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //



            //Write course minimap positions

            flip = BitConverter.GetBytes(coordOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(coordEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //



            //Write sky color offset

            flip = BitConverter.GetBytes(skyOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(skyEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //


            //Write ASM offset

            flip = BitConverter.GetBytes(asmOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(asmEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //

            //OK64 Course Header Table 
            

            binaryWriter.Write(seg6start);
            binaryWriter.Write(seg6end);
            binaryWriter.Write(seg4start);
            binaryWriter.Write(seg7end);
            binaryWriter.Write(seg9start);
            binaryWriter.Write(seg9end);

            flip = BitConverter.GetBytes(0x0F000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            binaryWriter.Write(vertcount);

            binaryWriter.Write(seg7rsp);


            binaryWriter.Write(seg7size);

            flip = BitConverter.GetBytes(0x09000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            flip = BitConverter.GetBytes(0x00000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //








            binaryWriter.BaseStream.Seek(0xBFF9C0 + (setID * 0x140) + (cID * 0x10), SeekOrigin.Begin);



            //Write preview image offset

            flip = BitConverter.GetBytes(previewOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(previewEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //



            //Write banner image offset

            flip = BitConverter.GetBytes(bannerOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(bannerEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //






            byte[] newROM = memoryStream.ToArray();
            return newROM;

        }

        public byte[] compileBattle(byte[] useg4, byte[] useg6, byte[] useg7, byte[] seg9, string courseName, string previewImage, string bannerImage, string mapImage, Int16[] mapCoords, string customASM, byte[] skyColor, byte songID, byte[] gameSpeed, byte[] fileData, int cID, int setID)
        {

            /*
            And then the shrill, whining voice began, "Oh, bless his heart, his
            dear little Majesty needn't mind about the White Lady — that's what
            we call her — being dead. The Worshipful Master Doctor is only
            making game of a poor old woman like me when he says that. Sweet
            Mastery Doctor, learned Master Doctor, who ever heard of a witch that
            really died? You can always get them back."
            "Call her up," said the grey voice. "We are all ready. Draw the circle.
            Prepare the blue fire..."
            */

            bool blueFire = true; //needed for any black magic sorcery.

            byte[] seg6 = compressMIO0(useg6);
            byte[] seg4 = compressMIO0(useg4);
            byte[] seg7 = compress_seg7(useg7);


            byte[] flip = new byte[0];

            TM64_Geometry mk = new TM64_Geometry();
            memoryStream = new MemoryStream();
            memoryStream.Write(fileData, 0, fileData.Length);
            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);

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
            int mapOffset = 0;
            int coordOffset = 0;
            int nameOffset = 0;
            int spawnOffset = 0;
            int skyOffset = 0;

            int previewEnd = 0;
            int bannerEnd = 0;
            int mapEnd = 0;
            int coordEnd = 0;
            int nameEnd = 0;
            int spawnEnd = 0;
            int skyEnd = 0;



            int asmOffset = 0;
            int asmEnd = 0;

            binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;



            //allignment

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            //







            //Internal Name
            if (courseName.Length > 0)
            {
                nameOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);

                binaryWriter.Write(courseName);   //using a length-defined as opposed to null terminated setup.
                                                  //easier to program for writing, easier to program for reading. 


                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                nameEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            }
            //




            //Write Course Map Texture
            if (mapImage.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(mapImage);
                N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                byte[] compressedData = compressMIO0(imageData);


                mapOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(compressedData);




                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                mapEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            }
            //
            //map coords

            coordOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);

            flip = BitConverter.GetBytes(mapCoords[0]);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            flip = BitConverter.GetBytes(mapCoords[1]);
            Array.Reverse(flip);
            binaryWriter.Write(flip);



            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            coordEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);

            //




            //add sky colors


            skyOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[0]);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[1]);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[2]);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[3]);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[4]);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(skyColor[5]);



            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            skyEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            //


            //add spawnpoints

            /*
            spawnOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);
            for (int currentPoint = 0; currentPoint < 4; currentPoint++)
            {
                binaryWriter.Write(Convert.ToInt16(spawnPoints[currentPoint].x));
                binaryWriter.Write(Convert.ToInt16(spawnPoints[currentPoint].y));
                binaryWriter.Write(Convert.ToInt16(spawnPoints[currentPoint].z));
            }
            */
            spawnOffset = 0;






            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            spawnEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            //






            //custom ASM
            if (customASM.Length > 0)
            {


                byte[] asmSequence = File.ReadAllBytes(customASM);

                asmOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(asmSequence);





                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                asmEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            }
            //







            //Course Preview Texture
            if (previewImage.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(previewImage);
                N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                byte[] compressedData = compressMIO0(imageData);


                previewOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(compressedData);




                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                previewEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            }
            //



            //Write Course Banner Texture
            if (bannerImage.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(bannerImage);
                N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                byte[] compressedData = compressMIO0(imageData);


                bannerOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(compressedData);



                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                bannerEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            }
            //




            // Segment 6

            seg6start = Convert.ToUInt32(binaryWriter.BaseStream.Position);


            binaryWriter.Write(seg6, 0, seg6.Length);

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            seg6end = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            //


            // Segment 9
            seg9start = Convert.ToUInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(seg9, 0, seg9.Length);

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            seg9end = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            //




            // Segment 4/7
            seg4start = Convert.ToUInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(seg4, 0, seg4.Length);

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            seg7start = Convert.ToUInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(seg7, 0, seg7.Length);


            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            seg7end = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            seg7rsp = Convert.ToUInt32(0x0F000000 | (seg7start - seg4start));
            //






            // Flip Endian on Course Header offsets.

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
            //


            //calculate # verts

            UInt32 vertcount = Convert.ToUInt32(useg4.Length / 14);
            flip = BitConverter.GetBytes(vertcount);
            Array.Reverse(flip);
            vertcount = BitConverter.ToUInt32(flip, 0);
            //



            //seg7 size

            UInt32 seg7size = Convert.ToUInt32(useg7.Length);
            flip = BitConverter.GetBytes(seg7size);
            Array.Reverse(flip);
            seg7size = BitConverter.ToUInt32(flip, 0);
            //


            /// After Calculating the offsets and values above we now write them to the empty space near the end of the ROM.






            binaryWriter.BaseStream.Seek(0xBFDA80 + (setID * 0x7D0) + (cID * 0x64), SeekOrigin.Begin);




            //Internal Course Names

            flip = BitConverter.GetBytes(nameOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(nameEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //

            /*
            //Write staff ghost offset
            flip = BitConverter.GetBytes(ghostOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(ghostEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //
            */

            //Write spawn point offset
            flip = BitConverter.GetBytes(spawnOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(spawnEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);


            //Speed and Song
            //WRITTEN DIRECTLY TO HEADER.

            binaryWriter.Write(songID);
            binaryWriter.Write(gameSpeed[0]);
            binaryWriter.Write(gameSpeed[1]);
            binaryWriter.Write(gameSpeed[2]);
            //



            //Write course minimap offset

            flip = BitConverter.GetBytes(mapOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(mapEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //



            //Write course minimap positions

            flip = BitConverter.GetBytes(coordOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(coordEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //



            //Write sky color offset

            flip = BitConverter.GetBytes(skyOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(skyEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //


            //Write ASM offset

            flip = BitConverter.GetBytes(asmOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(asmEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //

            //OK64 Course Header Table 


            binaryWriter.Write(seg6start);
            binaryWriter.Write(seg6end);
            binaryWriter.Write(seg4start);
            binaryWriter.Write(seg7end);
            binaryWriter.Write(seg9start);
            binaryWriter.Write(seg9end);

            flip = BitConverter.GetBytes(0x0F000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            binaryWriter.Write(vertcount);

            binaryWriter.Write(seg7rsp);


            binaryWriter.Write(seg7size);

            flip = BitConverter.GetBytes(0x09000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            flip = BitConverter.GetBytes(0x00000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //








            binaryWriter.BaseStream.Seek(0xBFF9C0 + (setID * 0x140) + (cID * 0x10), SeekOrigin.Begin);



            //Write preview image offset

            flip = BitConverter.GetBytes(previewOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(previewEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //



            //Write banner image offset

            flip = BitConverter.GetBytes(bannerOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(bannerEnd);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //






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


            

            byte[] fileData = File.ReadAllBytes(filePath);


            Header[] courseHeaders = loadHeader(fileData);

            int s9Start = BitConverter.ToInt32(courseHeaders[cID].s9Start, 0);
            int s9End = BitConverter.ToInt32(courseHeaders[cID].s9Start, 0);

            byte[] seg9 = new byte[(s9End - s9Start)];

            Buffer.BlockCopy(fileData, s9Start, seg9, 0, (s9End - s9Start));



            string[] coursename = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut" };


            memoryStream = new MemoryStream(seg9);

            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);
            
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

                    byte[] decompressedTexture = decompressMIO0(textureFile);
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

                        string texturePath = Path.Combine(outputDir, coursename[cID] + i.ToString() + ".png");

                        exportBitmap.Save(texturePath, ImageFormat.Png);

                          
                    }
                    else
                    {
                        width = 32;
                        height = 64;

                        Bitmap exportBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        Graphics graphicsBitmap = Graphics.FromImage(exportBitmap);
                        N64Graphics.RenderTexture(graphicsBitmap, decompressedTexture, voidBytes, 0, width, height, 1, N64Codec.RGBA16, N64IMode.AlphaCopyIntensity);

                        string texturePath = Path.Combine(outputDir, coursename[cID] + i.ToString() + ".32x64.png");

                        exportBitmap.Save(texturePath, ImageFormat.Png);

                        width = 64;
                        height = 32;

                        exportBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        graphicsBitmap = Graphics.FromImage(exportBitmap);
                        N64Graphics.RenderTexture(graphicsBitmap, decompressedTexture, voidBytes, 0, width, height, 1, N64Codec.RGBA16, N64IMode.AlphaCopyIntensity);

                        texturePath = Path.Combine(outputDir, coursename[cID] + i.ToString() + ".64x32.png");

                        exportBitmap.Save(texturePath, ImageFormat.Png);
                    }
                   


                }
                else
                {
                    binaryReader.BaseStream.Seek(binaryReader.BaseStream.Length, SeekOrigin.Begin);
                }
            }
            MessageBox.Show("Finished");


            


        }








        public byte[] dumpseg4(int cID, byte[] fileData)
        {

            Header[] courseHeader = loadHeader(fileData);

            int s4Start = BitConverter.ToInt32(courseHeader[cID].s47Start, 0);
            int s7Start = BitConverter.ToInt32(courseHeader[cID].s7Pointer, 0);

            byte[] seg4 = new byte[(BitConverter.ToInt32(courseHeader[cID].s7Pointer, 0) - BitConverter.ToInt32(courseHeader[cID].s47Start, 0))];


            Buffer.BlockCopy(fileData, s4Start, seg4, 0, s7Start - s4Start);

            return seg4;
        }

        public byte[] dumpseg5(int cID, byte[] fileData)
        {

            Header[] courseHeader = loadHeader(fileData);

            

            List<int> offsets = new List<int>();


            int segment9Start = BitConverter.ToInt32(courseHeader[cID].s9Start, 0);
            int segment9End = BitConverter.ToInt32(courseHeader[cID].s9End, 0);



            byte[] seg9 = new byte[(segment9End - segment9Start)];
            List<byte> segment5 = new List<byte>();

            Buffer.BlockCopy(fileData, segment9Start, seg9, 0, segment9End - segment9Start);

            TM64_Geometry mk = new TM64_Geometry();

            memoryStream = new MemoryStream(seg9);
            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);
            
            int textureOffset = new int();
            int compressSize = new int();

            binaryReader.BaseStream.Position = (BitConverter.ToInt32(courseHeader[cID].texturePointer, 0) - 0x09000000);

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

                    byte[] decompressedTexture = decompressMIO0(textureFile);
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




        public byte[] dumpseg6(int cID, byte[] fileData)
        {

            Header[] courseHeader = loadHeader(fileData);

            int s6Start = BitConverter.ToInt32(courseHeader[cID].s6Start, 0);
            int s6End = BitConverter.ToInt32(courseHeader[cID].s6End, 0);

            byte[] seg6 = new byte[s6End - s6Start];


            Buffer.BlockCopy(fileData, s6Start, seg6, 0, s6End - s6Start);

            return seg6;
        }

        public byte[] dumpseg7(int cID, byte[] fileData)
        {

            Header[] courseHeaders = loadHeader(fileData);

            int s4Start = BitConverter.ToInt32(courseHeaders[cID].s47Start, 0);
            int s7Start = BitConverter.ToInt32(courseHeaders[cID].s7Pointer, 0);
            int s7End = BitConverter.ToInt32(courseHeaders[cID].s47End, 0);



            s7Start = s4Start + s7Start - 0x0F000000;
            byte[] seg7 = new byte[s7End - s7Start];


            Buffer.BlockCopy(fileData, s7Start, seg7, 0, s7End - s7Start);

            return seg7;
        }

        public byte[] dumpseg9(int cID, byte[] fileData)
        {

            Header[] courseHeaders = loadHeader(fileData);

            int s9Start = BitConverter.ToInt32(courseHeaders[cID].s9Start, 0);
            int s9End = BitConverter.ToInt32(courseHeaders[cID].s9End, 0);


            byte[] seg9 = new byte[s9End - s9Start];


            Buffer.BlockCopy(fileData, s9Start, seg9, 0, s9End - s9Start);

            return seg9;
        }

        public byte[] dump_ASM(string filePath)
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







        public byte[] decompress_seg7(byte[] useg7)
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







            int indexA = 0;
            int indexB = 0;
            int indexC = 0;











            MemoryStream romm = new MemoryStream(segment7);
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

                    if (Math.Abs(indexA - indexB) > 31 || Math.Abs(indexB - indexC) > 31 || Math.Abs(indexC - indexB) > 31)
                    {
                        ///MessageBox.Show("Vert Cache Error-" +Environment.NewLine+ "Face Composed from vertices outside 32 vert cache. Cannot create face. OverKart64 will now crash.");
                    }

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







            int indexA = 0;
            int indexB = 0;
            int indexC = 0;








            byte[] ROM = File.ReadAllBytes(filePath);




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


            MessageBox.Show("Compressed");
            byte[] seg7 = seg7m.ToArray();
            return (seg7);





        }


        private const double Epsilon = 0.000001d;

        public Vector3D testIntersect(Vector3D rayOrigin, Vector3D rayDirection, Vertex vertA, Vertex vertB, Vertex vertC)
        {
            Vector3D vert0, vert1, vert2;

            vert0.X = vertA.position.x;
            vert0.Y = vertA.position.y;
            vert0.Z = vertA.position.z;

            vert1.X = vertB.position.x;
            vert1.Y = vertB.position.y;
            vert1.Z = vertB.position.z;

            vert2.X = vertC.position.x;
            vert2.Y = vertC.position.y;
            vert2.Z = vertC.position.z;

            var edge1 = vert1 - vert0;
            var edge2 = vert2 - vert0;

            var pvec = Cross(rayDirection, edge2);

            var det = Dot(edge1, pvec);

            if (det > -Epsilon && det < Epsilon)
            {
                Vector3D returnVector = new Vector3D();
                return returnVector;
            }

            var invDet = 1d / det;

            var tvec = rayOrigin - vert0;

            var u = Dot(tvec, pvec) * invDet;

            if (u < 0 || u > 1)
            {
                Vector3D returnVector = new Vector3D();
                return returnVector;
            }

            var qvec = Cross(tvec, edge1);

            var v = Dot(rayDirection, qvec) * invDet;

            if (v < 0 || u + v > 1)
            {
                Vector3D returnVector = new Vector3D();
                return returnVector;
            }

            var t = Dot(edge2, qvec) * invDet;

            return new Vector3D((float)t, (float)u, (float)v);
        }

        private static double Dot(Vector3D v1, Vector3D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        private static Vector3D Cross(Vector3D v1, Vector3D v2)
        {
            Vector3D dest;

            dest.X = v1.Y * v2.Z - v1.Z * v2.Y;
            dest.Y = v1.Z * v2.X - v1.X * v2.Z;
            dest.Z = v1.X * v2.Y - v1.Y * v2.X;

            return dest;
        }

        public static Vector3D GetTrilinearCoordinateOfTheHit(float t, Vector3D rayOrigin, Vector3D rayDirection)
        {
            return rayDirection * t + rayOrigin;
        }


        public string F3DEX_Model(out Vertex[] vertOutput, byte commandbyte, byte[] segment, byte[] seg4, int vertoffset, int segmentoffset, Vertex[] vertCache)
        {
            //  new code
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

            int indexA = new int();
            int indexB = new int();
            int indexC = new int();

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





                    indexA = mainsegr.ReadByte() / 2;
                    indexB = mainsegr.ReadByte() / 2;
                    indexC = mainsegr.ReadByte() / 2;

                    ///outputstring = outputstring + indexA.ToString() + "-" + indexB.ToString() + "-" + indexC.ToString() + "-" + vertoffset.ToString() +"-"+ mainsegr.BaseStream.Position.ToString() + Environment.NewLine;
                    /// outputs the 3 vert indexes as well as the vertoffset and offset into the segment that called this command.


                    ///
                    bool breakError = false;

                    xval[0] = vertCache[indexA].position.x;
                    zval[0] = vertCache[indexA].position.y;
                    yval[0] = vertCache[indexA].position.z * -1;

                    xval[1] = vertCache[indexB].position.x;
                    zval[1] = vertCache[indexB].position.y;
                    yval[1] = vertCache[indexB].position.z * -1;

                    xval[2] = vertCache[indexC].position.x;
                    zval[2] = vertCache[indexC].position.y;
                    yval[2] = vertCache[indexC].position.z * -1;

                    if ((xval[0] == 0 & yval[0] == 0 & zval[0] == 0) | (xval[1] == 0 & yval[1] == 0 & zval[1] == 0) | (xval[2] == 0 & yval[2] == 0 & zval[2] == 0))
                    {
                        breakError = true;
                    }


                    if (!breakError)
                    {
                        ///outputstring = outputstring + "vertbox = mesh vertices:#([" + xval[0].ToString() + ",(" + (yval[0]).ToString() + ")," + zval[0].ToString() + "],[" + xval[1].ToString() + ",(" + (yval[1]).ToString() + ")," + zval[1].ToString() + "],[" + xval[2].ToString() + ",(" + (yval[2]).ToString() + ")," + zval[2].ToString() + "]) faces:#([1,2,3]) MaterialIDS:#(1) " + Environment.NewLine;
                        outputstring = outputstring + xval[0].ToString() + "," + yval[0].ToString() + "," + zval[0].ToString() + ";" + xval[1].ToString() + "," + yval[1].ToString() + "," + zval[1].ToString() + ";" + xval[2].ToString() + "," + yval[2].ToString() + "," + zval[2].ToString() + "," + Environment.NewLine;
                        ///
                        
                    }
                    if (i == 0)
                    {
                        mainsegr.BaseStream.Seek(1, SeekOrigin.Current);
                    }
                }
            }
            if (commandbyte == 0xBF)
            {


                ///Draw 1 Triangle
                ///Returns Vert Positions of 3 Verts that make 1 triangle.


                bool breakError = false;
                mainsegr.BaseStream.Seek(4, SeekOrigin.Current);







                indexA = mainsegr.ReadByte() / 2;
                indexB = mainsegr.ReadByte() / 2;
                indexC = mainsegr.ReadByte() / 2;
                ///outputstring = outputstring + indexA.ToString() + "-" + indexB.ToString() + "-" + indexC.ToString() + "-" + vertoffset.ToString() + "-" + mainsegr.BaseStream.Position.ToString() + Environment.NewLine;
                ////// outputs the 3 vert indexes as well as the vertoffset and offset into the segment that called this command.



                xval[0] = vertCache[indexA].position.x;
                zval[0] = vertCache[indexA].position.y;
                yval[0] = vertCache[indexA].position.z * -1;

                xval[1] = vertCache[indexB].position.x;
                zval[1] = vertCache[indexB].position.y;
                yval[1] = vertCache[indexB].position.z * -1;

                xval[2] = vertCache[indexC].position.x;
                zval[2] = vertCache[indexC].position.y;
                yval[2] = vertCache[indexC].position.z * -1;

                if ((xval[0] == 0 & yval[0] == 0 & zval[0] == 0) | (xval[1] == 0 & yval[1] == 0 & zval[1] == 0) | (xval[2] == 0 & yval[2] == 0 & zval[2] == 0))
                {
                    breakError = true;
                }


                if (!breakError)
                {

                    ///outputstring = outputstring + "vertbox = mesh vertices:#([" + xval[0].ToString() + ",(" + (yval[0]).ToString() + ")," + zval[0].ToString() + "],[" + xval[1].ToString() + ",(" + (yval[1]).ToString() + ")," + zval[1].ToString() + "],[" + xval[2].ToString() + ",(" + (yval[2]).ToString() + ")," + zval[2].ToString() + "]) faces:#([1,2,3]) MaterialIDS:#(1) " + Environment.NewLine;
                    outputstring = outputstring + xval[0].ToString() + "," + yval[0].ToString() + "," + zval[0].ToString() + ";" + xval[1].ToString() + "," + yval[1].ToString() + "," + zval[1].ToString() + ";" + xval[2].ToString() + "," + yval[2].ToString() + "," + zval[2].ToString() + "," + Environment.NewLine;
                    ///
                    
                }



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
                int vertIndex = Convert.ToInt32(mainsegr.ReadByte() / 2);
                
                flip2 = mainsegr.ReadBytes(2);
                Array.Reverse(flip2);
                uint vertTotal = BitConverter.ToUInt16(flip2, 0) ;
                vertTotal = vertTotal / 0x40F;



                
                uint vertCount = Convert.ToUInt32(32 - vertIndex);
                

                //MessageBox.Show(segmentoffset.ToString() + "-" + vertIndex.ToString()+"-"+vertCount.ToString());

                byte[] rsp_add = mainsegr.ReadBytes(4);

                Array.Reverse(rsp_add);
                int segid = Convert.ToInt32(rsp_add[3]);
                rsp_add[3] = 0x00;
                int location = BitConverter.ToInt32(rsp_add, 0);
                if (location < seg4r.BaseStream.Length)
                {
                    

                    seg4r.BaseStream.Position = location;
                    for (int currentVert = 0; seg4r.BaseStream.Position < seg4r.BaseStream.Length & currentVert < vertCount; currentVert++)
                    {
                        flip2 = seg4r.ReadBytes(2);
                        Array.Reverse(flip2);
                        vertCache[vertIndex + currentVert].position.x = BitConverter.ToInt16(flip2, 0);

                        flip2 = seg4r.ReadBytes(2);
                        Array.Reverse(flip2);
                        vertCache[vertIndex + currentVert].position.y = BitConverter.ToInt16(flip2, 0);

                        flip2 = seg4r.ReadBytes(2);
                        Array.Reverse(flip2);
                        vertCache[vertIndex + currentVert].position.z = BitConverter.ToInt16(flip2, 0);
                        seg4r.BaseStream.Seek(0xA, SeekOrigin.Current);
                    }



                    if (segid == 4)
                    {
                        outputstring = location.ToString();
                        ///MessageBox.Show(outputstring +"-"+ mainsegr.BaseStream.Position.ToString());
                    }
                    else
                    {
                        outputstring = location.ToString();
                        //MessageBox.Show("WARNING D35-01 :: VERTS LOADED FROM OUTSIDE SEGMENT 4"+Environment.NewLine+mainsegr.BaseStream.Position.ToString("X"));
                        MessageBox.Show(outputstring + "-" + mainsegr.BaseStream.Position.ToString());
                    }

                }
                else
                {

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



            vertOutput = vertCache;
            return outputstring;

        }

        string CheckFilePath(string inputPath, string basePath)
        {
            string outputString = "";
            string[] pathStrings = inputPath.Split('\\');
            if (pathStrings[0] == "..")
            {
                string[] baseStrings = basePath.Split('\\');

                for (int currentString = 0; currentString < pathStrings.Length; currentString++)
                {
                    if (pathStrings[currentString] == "..")
                    {
                        pathStrings[currentString] = baseStrings[currentString + 1];
                    }
                }

                outputString = Path.Combine(pathStrings);
                outputString = baseStrings[0] + Path.DirectorySeparatorChar + outputString;
                
            }
            else
            {
                outputString = inputPath;
            }


            return outputString;
        }

        public OK64Texture[] loadTextures(Assimp.Scene fbx, string filePath)
        {

            int materialCount = fbx.Materials.Count;
            OK64Texture[] textureArray = new OK64Texture[materialCount];
            
            for (int materialIndex = 0; materialIndex < materialCount; materialIndex++)
            {
                textureArray[materialIndex] = new TM64_Geometry.OK64Texture();
                if (fbx.Materials[materialIndex].TextureDiffuse.FilePath != null) 
                {   
                    textureArray[materialIndex].texturePath = fbx.Materials[materialIndex].TextureDiffuse.FilePath;
                    textureArray[materialIndex].textureName = Path.GetFileName(textureArray[materialIndex].texturePath);
                    textureArray[materialIndex].textureFormat = 0;

                    textureArray[materialIndex].texturePath = CheckFilePath(textureArray[materialIndex].texturePath, filePath);
                    
                    if (File.Exists(textureArray[materialIndex].texturePath))
                    {
                        textureArray[materialIndex].textureBitmap = Image.FromFile(textureArray[materialIndex].texturePath);
                        textureArray[materialIndex].textureHeight = textureArray[materialIndex].textureBitmap.Height;
                        textureArray[materialIndex].textureWidth = textureArray[materialIndex].textureBitmap.Width;

                    }
                    else
                    {
                        while (!(File.Exists(textureArray[materialIndex].texturePath)))
                        {
                            MessageBox.Show(textureArray[materialIndex].texturePath + " not found, browse to file!");
                            OpenFileDialog fileOpen = new OpenFileDialog();
                            if (fileOpen.ShowDialog() == DialogResult.OK)
                            {
                                textureArray[materialIndex].texturePath = fileOpen.FileName;
                                textureArray[materialIndex].textureBitmap = Image.FromFile(textureArray[materialIndex].texturePath);
                                textureArray[materialIndex].textureHeight = textureArray[materialIndex].textureBitmap.Height;
                                textureArray[materialIndex].textureWidth = textureArray[materialIndex].textureBitmap.Width;
                            }
                            else
                            {
                                MessageBox.Show("ERROR FILE NOT SELECTED");
                                textureArray[materialIndex].textureHeight = 32;
                                textureArray[materialIndex].textureWidth = 32;
                                break;
                            }
                        }

                    }
                    


                    textureClass(textureArray[materialIndex]);

                }
            }

            return textureArray;
        }




        public static IEnumerable<TM64_Geometry.OK64F3DObject> NaturalSort(IEnumerable<TM64_Geometry.OK64F3DObject> list)
        {
            int maxLen = list.Select(s => s.objectName.Length).Max();
            Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

            return list
                    .Select(s =>
                        new
                        {
                            OrgStr = s,
                            SortStr = Regex.Replace(s.objectName, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                        })
                    .OrderBy(x => x.SortStr)
                    .Select(x => x.OrgStr);
        }
         
        public OK64F3DObject[] loadMaster(Assimp.Scene fbx, OK64Texture[] textureArray)
        {
            
            var masterNode = fbx.RootNode.FindNode("Course Master Objects");
            int masterCount = masterNode.Children.Count;
            OK64F3DObject[] masterObjects = new OK64F3DObject[masterCount];


            for (int currentChild = 0; currentChild < masterNode.Children.Count; currentChild++)
            {
                masterObjects[currentChild] = new TM64_Geometry.OK64F3DObject();


                masterObjects[currentChild].objectColor = new float[3];
                masterObjects[currentChild].objectColor[0] = rValue.NextFloat(0.3f, 1);
                masterObjects[currentChild].objectColor[1] = rValue.NextFloat(0.3f, 1);
                masterObjects[currentChild].objectColor[2] = rValue.NextFloat(0.3f, 1);



                masterObjects[currentChild].objectName = masterNode.Children[currentChild].Name;
                masterObjects[currentChild].meshID = masterNode.Children[currentChild].MeshIndices.ToArray();
                masterObjects[currentChild].materialID = fbx.Meshes[masterObjects[currentChild].meshID[0]].MaterialIndex;
                
                int vertCount = 0;
                int faceCount = 0;

                if (masterObjects[currentChild].meshID.Length == 0)
                {
                    MessageBox.Show("Empty Course Object! -" + masterObjects[currentChild].objectName);
                }



                List<int> xValues = new List<int>();
                List<int> yValues = new List<int>();

                foreach (var childMesh in masterNode.Children[currentChild].MeshIndices)
                {

                    vertCount = vertCount + fbx.Meshes[childMesh].VertexCount;
                    faceCount = faceCount + fbx.Meshes[childMesh].FaceCount;

                }
                masterObjects[currentChild].vertCount = vertCount;
                masterObjects[currentChild].faceCount = faceCount;
                masterObjects[currentChild].modelGeometry = new Face[faceCount];
                int currentFace = 0;
                //masterObjects[currentChild].modelGeometry[currentFace];
               
                foreach (var childMesh in masterNode.Children[currentChild].MeshIndices)
                {
                    
                    foreach (var childPoly in fbx.Meshes[childMesh].Faces)
                    {


                        List<int> l_xValues = new List<int>();
                        List<int> l_yValues = new List<int>();
                        List<int> l_zValues = new List<int>();

                        masterObjects[currentChild].modelGeometry[currentFace] = new Face();
                        masterObjects[currentChild].modelGeometry[currentFace].vertData = new Vertex[3];

                        for (int currentVert = 0; currentVert < 3; currentVert++)
                        {
                            masterObjects[currentChild].modelGeometry[currentFace].vertIndex = new VertIndex();
                            masterObjects[currentChild].modelGeometry[currentFace].vertIndex.indexA = Convert.ToInt16(childPoly.Indices[0]);
                            masterObjects[currentChild].modelGeometry[currentFace].vertIndex.indexB = Convert.ToInt16(childPoly.Indices[1]);
                            masterObjects[currentChild].modelGeometry[currentFace].vertIndex.indexC = Convert.ToInt16(childPoly.Indices[2]);

                            masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert] = new Vertex();
                            masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position = new Position();
                            masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.x = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].X);
                            masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.y = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].Y);
                            masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.z = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].Z);



                            xValues.Add(masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.x);
                            yValues.Add(masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.y);

                            l_xValues.Add(masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.x);
                            l_yValues.Add(masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.y);
                            l_zValues.Add(masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.z);



                            if (masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.x > masterObjects[currentChild].modelGeometry[currentFace].highX)
                            {
                                masterObjects[currentChild].modelGeometry[currentFace].highX = masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.x;
                            }
                            if (masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.x < masterObjects[currentChild].modelGeometry[currentFace].lowX)
                            {
                                masterObjects[currentChild].modelGeometry[currentFace].lowX = masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.x;
                            }
                            if (masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.y > masterObjects[currentChild].modelGeometry[currentFace].highY)
                            {
                                masterObjects[currentChild].modelGeometry[currentFace].highY = masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.y;
                            }
                            if (masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.y < masterObjects[currentChild].modelGeometry[currentFace].lowY)
                            {
                                masterObjects[currentChild].modelGeometry[currentFace].lowY = masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].position.y;
                            }


                            masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].color = new Color();
                            masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].color = new Color();
                            if (fbx.Meshes[childMesh].VertexColorChannels[0].Count > 0)
                            {
                                masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].color.r = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].R * 255));
                                masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].color.g = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].G * 255));
                                masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].color.b = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].B * 255));
                            }
                            else
                            {
                                masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].color.r = 252;
                                masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].color.g = 252;
                                masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].color.b = 252;
                            }
                            masterObjects[currentChild].modelGeometry[currentFace].vertData[currentVert].color.a = 0;
                        }




                        float centerX = (l_xValues[0] + l_xValues[1] + l_xValues[2]) / 3;
                        float centerY = (l_yValues[0] + l_yValues[1] + l_yValues[2]) / 3;
                        float centerZ = (l_zValues[0] + l_zValues[1] + l_zValues[2]) / 3;

                        masterObjects[currentChild].modelGeometry[currentFace].centerPosition = new Vector3D(centerX, centerY, centerZ);




                        //UV coords


                        UInt32[] STheight = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; ///looks like
                        UInt32[] STwidth = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 };


                        //
                        //
                        float u_base = 0;
                        float v_base = 0;

                        float[] u_shift = { 0, 0, 0 };
                        float[] v_shift = { 0, 0, 0 };
                        float[] u_offset = { 0, 0, 0 };
                        float[] v_offset = { 0, 0, 0 };




                        u_offset[0] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[0]][0]);
                        v_offset[0] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[0]][1]);

                        u_offset[1] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[1]][0]);
                        v_offset[1] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[1]][1]);

                        u_offset[2] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[2]][0]);
                        v_offset[2] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[2]][1]);

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
                        int materialID = fbx.Meshes[childMesh].MaterialIndex;

                        s_coord = Convert.ToInt32(u_offset[0] * STwidth[textureArray[materialID].textureClass] * 32);
                        t_coord = Convert.ToInt32(v_offset[0] * STheight[textureArray[materialID].textureClass] * -32);


                        if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                        {
                            MessageBox.Show("FATAL ERROR! " + u_offset[0].ToString() + "-" + v_offset[0].ToString() + " - UV 0 Out of Range for Object - " + fbx.Meshes[childMesh].Name);
                        }
                        masterObjects[currentChild].modelGeometry[currentFace].vertData[0].position.s = Convert.ToInt16(s_coord);
                        masterObjects[currentChild].modelGeometry[currentFace].vertData[0].position.t = Convert.ToInt16(t_coord);



                        s_coord = Convert.ToInt32(u_offset[1] * STwidth[textureArray[materialID].textureClass] * 32);
                        t_coord = Convert.ToInt32(v_offset[1] * STheight[textureArray[materialID].textureClass] * -32);


                        if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                        {
                            MessageBox.Show("FATAL ERROR! " + u_offset[1].ToString() + "-" + v_offset[1].ToString() + " UV 1 Out of Range for Object - " + fbx.Meshes[childMesh].Name);
                        }

                        masterObjects[currentChild].modelGeometry[currentFace].vertData[1].position.s = Convert.ToInt16(s_coord);
                        masterObjects[currentChild].modelGeometry[currentFace].vertData[1].position.t = Convert.ToInt16(t_coord);


                        //


                        s_coord = Convert.ToInt32(u_offset[2] * STwidth[textureArray[materialID].textureClass] * 32);
                        t_coord = Convert.ToInt32(v_offset[2] * STheight[textureArray[materialID].textureClass] * -32);



                        if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                        {
                            MessageBox.Show("FATAL ERROR! " + u_offset[2].ToString() + "-" + v_offset[2].ToString() + " UV 2 Out of Range for Object - " + fbx.Meshes[childMesh].Name);

                        }


                        masterObjects[currentChild].modelGeometry[currentFace].vertData[2].position.s = Convert.ToInt16(s_coord);
                        masterObjects[currentChild].modelGeometry[currentFace].vertData[2].position.t = Convert.ToInt16(t_coord);


                        currentFace++;

                    }

                    int[] localMax = new int[4];
                    localMax[0] = -9999999;
                    localMax[1] = 9999999;
                    localMax[2] = -9999999;
                    localMax[3] = 9999999;

                    for (int currentValue = 0; currentValue < xValues.Count; currentValue++)
                    {
                        if (xValues[currentValue] > localMax[0])
                        {
                            localMax[0] = xValues[currentValue];
                        }
                        if (xValues[currentValue] < localMax[1])
                        {
                            localMax[1] = xValues[currentValue];
                        }
                        if (yValues[currentValue] > localMax[2])
                        {
                            localMax[2] = yValues[currentValue];
                        }
                        if (yValues[currentValue] < localMax[3])
                        {
                            localMax[3] = yValues[currentValue];
                        }
                    }

                    masterObjects[currentChild].pathfindingObject = new PathfindingObject();
                    masterObjects[currentChild].pathfindingObject.highX = localMax[0];
                    masterObjects[currentChild].pathfindingObject.lowX = localMax[1];
                    masterObjects[currentChild].pathfindingObject.highY = localMax[2];
                    masterObjects[currentChild].pathfindingObject.lowY = localMax[3];



                }

            }
            List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>(masterObjects);

            masterObjects = NaturalSort(masterList).ToArray();

            return masterObjects;
        }


        public OK64F3DObject[] createMaster(Assimp.Scene fbx, int sectionCount, OK64Texture[] textureArray)
        {
            List<OK64F3DObject> masterObjects = new List<OK64F3DObject>();
            int currentObject = 0;
            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                var surfaceNode = fbx.RootNode.FindNode("Section " +(currentSection + 1).ToString());
                
                for (int childObject = 0; childObject < surfaceNode.Children.Count; childObject++)
                {
                    masterObjects.Add(new OK64F3DObject());

                    masterObjects[currentObject].objectColor = new float[3];
                    masterObjects[currentObject].objectColor[0] = rValue.NextFloat(0.3f, 1);
                    masterObjects[currentObject].objectColor[1] = rValue.NextFloat(0.3f, 1);
                    masterObjects[currentObject].objectColor[2] = rValue.NextFloat(0.3f, 1);

                    

                    masterObjects[currentObject].objectName = surfaceNode.Children[childObject].Name;
                    masterObjects[currentObject].meshID = surfaceNode.Children[childObject].MeshIndices.ToArray();
                    masterObjects[currentObject].materialID = fbx.Meshes[masterObjects[currentObject].meshID[0]].MaterialIndex;
                    int vertCount = 0;
                    int faceCount = 0;

                    if (masterObjects[currentObject].meshID.Length == 0)
                    {
                        MessageBox.Show("Empty Course Object! -" + masterObjects[currentObject].objectName);
                    }

                    foreach (var childMesh in surfaceNode.Children[childObject].MeshIndices)
                    {

                        vertCount = vertCount + fbx.Meshes[childMesh].VertexCount;
                        faceCount = faceCount + fbx.Meshes[childMesh].FaceCount;

                    }
                    masterObjects[currentObject].vertCount = vertCount;
                    masterObjects[currentObject].faceCount = faceCount;
                    masterObjects[currentObject].modelGeometry = new Face[faceCount];
                    
                    int currentFace = 0;


                    List<int> xValues = new List<int>();
                    List<int> yValues = new List<int>();

                    foreach (var childMesh in surfaceNode.Children[childObject].MeshIndices)
                    {
                        foreach (var childPoly in fbx.Meshes[childMesh].Faces)
                        {


                            masterObjects[currentObject].modelGeometry[currentFace] = new Face();
                            masterObjects[currentObject].modelGeometry[currentFace].vertData = new Vertex[3];

                            masterObjects[currentObject].modelGeometry[currentFace].highX = -99999999;
                            masterObjects[currentObject].modelGeometry[currentFace].highY = -99999999;
                            masterObjects[currentObject].modelGeometry[currentFace].lowX = 99999999;
                            masterObjects[currentObject].modelGeometry[currentFace].lowX = 99999999;

                            if (childPoly.IndexCount != 3)
                            {
                                MessageBox.Show("FATAL ERROR- OBJECT -" + surfaceNode.Children[childObject].Name + " - has invalid geometry");
                            }
                            else
                            {
                                List<int> l_xValues = new List<int>();
                                List<int> l_yValues = new List<int>();
                                List<int> l_zValues = new List<int>();

                                
                                for (int currentVert = 0; currentVert < 3; currentVert++)
                                {
                                    masterObjects[currentObject].modelGeometry[currentFace].vertIndex = new VertIndex();
                                    masterObjects[currentObject].modelGeometry[currentFace].vertIndex.indexA = Convert.ToInt16(childPoly.Indices[0]);
                                    masterObjects[currentObject].modelGeometry[currentFace].vertIndex.indexB = Convert.ToInt16(childPoly.Indices[1]);
                                    masterObjects[currentObject].modelGeometry[currentFace].vertIndex.indexC = Convert.ToInt16(childPoly.Indices[2]);

                                    masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert] = new Vertex();
                                    masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position = new Position();
                                    masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.x = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].X);
                                    masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.y = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].Y);
                                    masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.z = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].Z);




                                    xValues.Add(masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.x);
                                    yValues.Add(masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.y);

                                    l_xValues.Add(masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.x);
                                    l_yValues.Add(masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.y);
                                    l_zValues.Add(masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.z);



                                    if (masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.x > masterObjects[currentObject].modelGeometry[currentFace].highX)
                                    {
                                        masterObjects[currentObject].modelGeometry[currentFace].highX = masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.x;
                                    }
                                    if (masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.x < masterObjects[currentObject].modelGeometry[currentFace].lowX)
                                    {
                                        masterObjects[currentObject].modelGeometry[currentFace].lowX = masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.x;
                                    }
                                    if (masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.y > masterObjects[currentObject].modelGeometry[currentFace].highY)
                                    {
                                        masterObjects[currentObject].modelGeometry[currentFace].highY = masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.y;
                                    }
                                    if (masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.y < masterObjects[currentObject].modelGeometry[currentFace].lowY)
                                    {
                                        masterObjects[currentObject].modelGeometry[currentFace].lowY = masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].position.y;
                                    }

                                    masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].color = new Color();
                                    if (fbx.Meshes[childMesh].VertexColorChannels[0].Count > 0)
                                    {
                                        masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].color.r = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].R * 255));
                                        masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].color.g = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].G * 255));
                                        masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].color.b = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].B * 255));
                                    }
                                    else
                                    {
                                        masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].color.r = 252;
                                        masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].color.g = 252;
                                        masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].color.b = 252;
                                    }
                                    masterObjects[currentObject].modelGeometry[currentFace].vertData[currentVert].color.a = 0;
                                }


                                float centerX = (l_xValues[0] + l_xValues[1] + l_xValues[2]) / 3;
                                float centerY = (l_yValues[0] + l_yValues[1] + l_yValues[2]) / 3;
                                float centerZ = (l_zValues[0] + l_zValues[1] + l_zValues[2]) / 3;
                                
                                masterObjects[currentObject].modelGeometry[currentFace].centerPosition = new Vector3D(centerX, centerY, centerZ);



                                //UV coords


                                UInt32[] STheight = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; ///looks like
                                UInt32[] STwidth = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 };


                                //
                                //
                                float u_base = 0;
                                float v_base = 0;

                                float[] u_shift = { 0, 0, 0 };
                                float[] v_shift = { 0, 0, 0 };
                                float[] u_offset = { 0, 0, 0 };
                                float[] v_offset = { 0, 0, 0 };


                                u_offset[0] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[0]][0]);
                                v_offset[0] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[0]][1]);

                                u_offset[1] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[1]][0]);
                                v_offset[1] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[1]][1]);

                                u_offset[2] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[2]][0]);
                                v_offset[2] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[2]][1]);

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
                                int materialID = fbx.Meshes[childMesh].MaterialIndex;

                                s_coord = Convert.ToInt32(u_offset[0] * STwidth[textureArray[materialID].textureClass] * 32);
                                t_coord = Convert.ToInt32(v_offset[0] * STheight[textureArray[materialID].textureClass] * -32);


                                if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                                {
                                    MessageBox.Show("FATAL ERROR! " + u_offset[0].ToString() + "-" + v_offset[0].ToString() + " - UV 0 Out of Range for Object - " + masterObjects[currentObject].objectName);
                                }
                                masterObjects[currentObject].modelGeometry[currentFace].vertData[0].position.s = Convert.ToInt16(s_coord);
                                masterObjects[currentObject].modelGeometry[currentFace].vertData[0].position.t = Convert.ToInt16(t_coord);



                                s_coord = Convert.ToInt32(u_offset[1] * STwidth[textureArray[materialID].textureClass] * 32);
                                t_coord = Convert.ToInt32(v_offset[1] * STheight[textureArray[materialID].textureClass] * -32);


                                if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                                {
                                    MessageBox.Show("FATAL ERROR! " + u_offset[1].ToString() + "-" + v_offset[1].ToString() + " UV 1 Out of Range for Object - " + masterObjects[currentObject].objectName);
                                }

                                masterObjects[currentObject].modelGeometry[currentFace].vertData[1].position.s = Convert.ToInt16(s_coord);
                                masterObjects[currentObject].modelGeometry[currentFace].vertData[1].position.t = Convert.ToInt16(t_coord);


                                //


                                s_coord = Convert.ToInt32(u_offset[2] * STwidth[textureArray[materialID].textureClass] * 32);
                                t_coord = Convert.ToInt32(v_offset[2] * STheight[textureArray[materialID].textureClass] * -32);



                                if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                                {
                                    MessageBox.Show("FATAL ERROR! " + u_offset[2].ToString() + "-" + v_offset[2].ToString() + " UV 2 Out of Range for Object - " + masterObjects[currentObject].objectName);

                                }


                                masterObjects[currentObject].modelGeometry[currentFace].vertData[2].position.s = Convert.ToInt16(s_coord);
                                masterObjects[currentObject].modelGeometry[currentFace].vertData[2].position.t = Convert.ToInt16(t_coord);

                            }
                            currentFace++;

                        }



                    }





                    int[] localMax = new int[4];
                    localMax[0] = -9999999;
                    localMax[1] = 9999999;
                    localMax[2] = -9999999;
                    localMax[3] = 9999999;

                    for (int currentValue = 0; currentValue < xValues.Count; currentValue++)
                    {
                        if (xValues[currentValue] > localMax[0])
                        {
                            localMax[0] = xValues[currentValue];
                        }
                        if (xValues[currentValue] < localMax[1])
                        {
                            localMax[1] = xValues[currentValue];
                        }
                        if (yValues[currentValue] > localMax[2])
                        {
                            localMax[2] = yValues[currentValue];
                        }
                        if (yValues[currentValue] < localMax[3])
                        {
                            localMax[3] = yValues[currentValue];
                        }
                    }

                    masterObjects[currentObject].pathfindingObject = new PathfindingObject();
                    masterObjects[currentObject].pathfindingObject.highX = localMax[0];
                    masterObjects[currentObject].pathfindingObject.lowX = localMax[1];
                    masterObjects[currentObject].pathfindingObject.highY = localMax[2];
                    masterObjects[currentObject].pathfindingObject.lowY = localMax[3];
                    





                    currentObject++;

                }
                List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>(masterObjects);

              
            }

            OK64F3DObject[] outputObjects = NaturalSort(masterObjects).ToArray();

            return outputObjects;
        }



        public OK64F3DObject[] loadCollision (Assimp.Scene fbx, int sectionCount, OK64Texture[] textureArray, int simpleFormat)
        {   
            int totalIndexCount = 0;
            int totalIndex = 0;
            var surfaceNode = fbx.RootNode;
            List<OK64F3DObject> surfaceObjects = new List<OK64F3DObject>();
            float[] colorValues = new float[3];
            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                if (simpleFormat == 2)
                {
                    surfaceNode = fbx.RootNode.FindNode("Section " + (currentSection + 1).ToString() + " Surface");
                }
                else
                {
                    surfaceNode = fbx.RootNode.FindNode("Section " + (currentSection + 1).ToString());
                }

                colorValues[0] = rValue.NextFloat(0, 1);
                colorValues[1] = rValue.NextFloat(0, 1);
                colorValues[2] = rValue.NextFloat(0, 1);

                int subobjectCount = surfaceNode.Children.Count;
                totalIndexCount = totalIndexCount + surfaceNode.Children.Count;
                for (int currentsubObject = 0; currentsubObject < subobjectCount; currentsubObject++)
                {
                    surfaceObjects.Add(new OK64F3DObject());

                    surfaceObjects[totalIndex].objectColor = new float[3];
                    surfaceObjects[totalIndex].objectColor[0] = colorValues[0];
                    surfaceObjects[totalIndex].objectColor[1] = colorValues[1];
                    surfaceObjects[totalIndex].objectColor[2] = colorValues[2];


                    surfaceObjects[totalIndex].objectName = surfaceNode.Children[currentsubObject].Name;
                    int vertCount = 0;
                    int faceCount = 0;
                    foreach (var childMesh in surfaceNode.Children[currentsubObject].MeshIndices)
                    {

                        vertCount = vertCount + fbx.Meshes[childMesh].VertexCount;
                        faceCount = faceCount + fbx.Meshes[childMesh].FaceCount;

                    }

                    surfaceObjects[totalIndex].faceCount = faceCount;
                    surfaceObjects[totalIndex].vertCount = vertCount;
                    surfaceObjects[totalIndex].meshID = surfaceNode.Children[currentsubObject].MeshIndices.ToArray();

                    if (surfaceObjects[totalIndex].meshID.Length == 0)
                    {
                        MessageBox.Show("Empty Surface Object! -" + surfaceObjects[totalIndex].objectName);
                    }

                    surfaceObjects[totalIndex].surfaceID = currentSection + 1;

                    string[] nameSplit = surfaceObjects[totalIndex].objectName.Split('_');

                    surfaceObjects[totalIndex].surfaceMaterial = Convert.ToInt32(nameSplit[0]);
                    surfaceObjects[totalIndex].pathfindingObject = new PathfindingObject();
                    if (surfaceObjects[totalIndex].surfaceMaterial > 100 & surfaceObjects[totalIndex].surfaceMaterial < 200)
                    {

                        
                        surfaceObjects[totalIndex].pathfindingObject.surfaceBoolean = false;
                        if (surfaceObjects[totalIndex].surfaceMaterial > 117)
                        {
                            surfaceObjects[totalIndex].surfaceMaterial = surfaceObjects[totalIndex].surfaceMaterial + 100;
                        }
                        else
                        {
                            surfaceObjects[totalIndex].surfaceMaterial = surfaceObjects[totalIndex].surfaceMaterial - 100;
                        }
                    }
                    else
                    {
                        surfaceObjects[totalIndex].pathfindingObject.surfaceBoolean = true;
                    }




                    surfaceObjects[totalIndex].materialID = fbx.Meshes[surfaceObjects[totalIndex].meshID[0]].MaterialIndex;
                    surfaceObjects[totalIndex].flagA = false;
                    surfaceObjects[totalIndex].flagB = false;
                    surfaceObjects[totalIndex].flagC = false;

                    int currentFace = 0;

                    surfaceObjects[totalIndex].modelGeometry = new Face[faceCount];


                    foreach (var childMesh in surfaceNode.Children[currentsubObject].MeshIndices)
                    {
                        foreach (var childPoly in fbx.Meshes[childMesh].Faces)
                        {
                            List<int> l_xValues = new List<int>();
                            List<int> l_yValues = new List<int>();
                            List<int> l_zValues = new List<int>();


                            surfaceObjects[totalIndex].modelGeometry[currentFace] = new Face();
                            surfaceObjects[totalIndex].modelGeometry[currentFace].vertData = new Vertex[3];
                            if (childPoly.IndexCount != 3)
                            {
                                MessageBox.Show("FATAL ERROR- OBJECT -" + surfaceNode.Children[currentsubObject].Name + " - has invalid geometry");
                            }
                            else
                            {
                                for (int currentVert = 0; currentVert < 3; currentVert++)
                                {

                                    surfaceObjects[totalIndex].modelGeometry[currentFace].vertIndex = new VertIndex();
                                    surfaceObjects[totalIndex].modelGeometry[currentFace].vertIndex.indexA = Convert.ToInt16(childPoly.Indices[0]);
                                    surfaceObjects[totalIndex].modelGeometry[currentFace].vertIndex.indexB = Convert.ToInt16(childPoly.Indices[1]);
                                    surfaceObjects[totalIndex].modelGeometry[currentFace].vertIndex.indexC = Convert.ToInt16(childPoly.Indices[2]);


                                    surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert] = new Vertex();
                                    surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].position = new Position();
                                    surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].position.x = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].X);
                                    surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].position.y = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].Y);
                                    surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].position.z = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].Z);



                                    l_xValues.Add(surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].position.x);
                                    l_yValues.Add(surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].position.y);
                                    l_zValues.Add(surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].position.z);





                                    surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].color = new Color();
                                    if (fbx.Meshes[childMesh].VertexColorChannels[0].Count > 0)
                                    {
                                        surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].color.r = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].R * 255));
                                        surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].color.g = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].G * 255));
                                        surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].color.b = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].B * 255));
                                    }
                                    else
                                    {
                                        surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].color.r = 252;
                                        surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].color.g = 252;
                                        surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].color.b = 252;
                                    }
                                    surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[currentVert].color.a = 0;
                                }


                                float centerX = (l_xValues[0] + l_xValues[1] + l_xValues[2]) / 3;
                                float centerY = (l_yValues[0] + l_yValues[1] + l_yValues[2]) / 3;
                                float centerZ = (l_zValues[0] + l_zValues[1] + l_zValues[2]) / 3;

                                surfaceObjects[totalIndex].modelGeometry[currentFace].centerPosition = new Vector3D(centerX, centerY, centerZ);

                                //UV coords


                                UInt32[] STheight = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; ///looks like
                                UInt32[] STwidth = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 };


                                //
                                //
                                float u_base = 0;
                                float v_base = 0;

                                float[] u_shift = { 0, 0, 0 };
                                float[] v_shift = { 0, 0, 0 };
                                float[] u_offset = { 0, 0, 0 };
                                float[] v_offset = { 0, 0, 0 };


                                u_offset[0] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[0]][0]);
                                v_offset[0] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[0]][1]);

                                u_offset[1] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[1]][0]);
                                v_offset[1] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[1]][1]);

                                u_offset[2] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[2]][0]);
                                v_offset[2] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[2]][1]);

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
                                int materialID = fbx.Meshes[childMesh].MaterialIndex;

                                s_coord = Convert.ToInt32(u_offset[0] * STwidth[textureArray[materialID].textureClass] * 32);
                                t_coord = Convert.ToInt32(v_offset[0] * STheight[textureArray[materialID].textureClass] * -32);


                                if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                                {
                                    MessageBox.Show("FATAL ERROR! " + u_offset[0].ToString() + "-" + v_offset[0].ToString() + " - UV 0 Out of Range for Object - " + surfaceObjects[totalIndex].objectName);
                                }
                                surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[0].position.s = Convert.ToInt16(s_coord);
                                surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[0].position.t = Convert.ToInt16(t_coord);



                                s_coord = Convert.ToInt32(u_offset[1] * STwidth[textureArray[materialID].textureClass] * 32);
                                t_coord = Convert.ToInt32(v_offset[1] * STheight[textureArray[materialID].textureClass] * -32);


                                if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                                {
                                    MessageBox.Show("FATAL ERROR! " + u_offset[1].ToString() + "-" + v_offset[1].ToString() + " UV 1 Out of Range for Object - " + surfaceObjects[totalIndex].objectName);
                                }

                                surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[1].position.s = Convert.ToInt16(s_coord);
                                surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[1].position.t = Convert.ToInt16(t_coord);


                                //


                                s_coord = Convert.ToInt32(u_offset[2] * STwidth[textureArray[materialID].textureClass] * 32);
                                t_coord = Convert.ToInt32(v_offset[2] * STheight[textureArray[materialID].textureClass] * -32);



                                if (s_coord > 32767 || s_coord < -32768 || t_coord > 32767 || t_coord < -32768)
                                {
                                    MessageBox.Show("FATAL ERROR! " + u_offset[2].ToString() + "-" + v_offset[2].ToString() + " UV 2 Out of Range for Object - " + surfaceObjects[totalIndex].objectName);

                                }


                                surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[2].position.s = Convert.ToInt16(s_coord);
                                surfaceObjects[totalIndex].modelGeometry[currentFace].vertData[2].position.t = Convert.ToInt16(t_coord);

                            }
                            currentFace++;

                        }



                    }



                    totalIndex++;
                }
            }
            return surfaceObjects.ToArray();
        }


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

        public OK64SectionList[] loadSection(Assimp.Scene fbx, int sectionCount, OK64F3DObject[] masterObjects)
        {
            OK64SectionList[] sectionList = new OK64SectionList[sectionCount];
            List<OK64F3DObject> masterList = new List<OK64F3DObject>(masterObjects);
            
            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                
                sectionList[currentSection] = new TM64_Geometry.OK64SectionList();
                sectionList[currentSection].viewList = new TM64_Geometry.OK64ViewList[4];
                for (int view = 0; view < 4; view++)
                {
                    sectionList[currentSection].viewList[view] = new TM64_Geometry.OK64ViewList();

                    var parentNode = fbx.RootNode.FindNode("Section " + (currentSection + 1).ToString() + " " + viewString[view]);
                    sectionList[currentSection].viewList[view].objectList = new int[parentNode.Children.Count];

                    

                    for (int currentObject = 0; currentObject < parentNode.Children.Count; currentObject++)
                    {
                        string searchObject = parentNode.Children[currentObject].Name;
                        var foundObject = masterObjects.FirstOrDefault(b => b.objectName == searchObject);
                        int masterIndex = Array.IndexOf(masterObjects, foundObject);
                        if (masterIndex == -1)
                        {
                            MessageBox.Show("Error- Object not Found- :" + searchObject + ": - Section " + (currentSection + 1).ToString() + "- View " + viewString[view]);
                        }
                        else
                        {
                            sectionList[currentSection].viewList[view].objectList[currentObject] = masterIndex;
                        }
                    }
                }

            }
            return sectionList;
        }

        public void ExportSVL(string filePath, int masterLength, OK64SectionList[] sectionList, OK64F3DObject[] masterObjects)
        {

            File.AppendAllText(filePath, "SVL2" + Environment.NewLine);
            File.AppendAllText(filePath, masterLength.ToString() + Environment.NewLine);
            File.AppendAllText(filePath, sectionList.Length.ToString() + Environment.NewLine);
            foreach (var section in sectionList)
            {
                foreach (var view in section.viewList)
                {
                    File.AppendAllText(filePath, view.objectList.Length.ToString() + Environment.NewLine);
                    foreach (var obj in view.objectList)
                    {
                        File.AppendAllText(filePath, masterObjects[obj].objectName + Environment.NewLine);
                    }
                }


            }
        }

        public OK64SectionList[] ImportSVL(string filePath, int masterCount, OK64F3DObject[] masterObjects)
        {
            string[] fileText = File.ReadAllLines(filePath);
            OK64SectionList[] sectionList = new OK64SectionList[0];
            if (fileText[0] == "SVL2")
            {
                int sectionCount = Convert.ToInt32(fileText[2]); 
                sectionList = new OK64SectionList[sectionCount];
                int currentLine = 3;
                for (int currentSection = 0; currentSection < sectionCount; currentSection++)
                {
                    sectionList[currentSection] = new OK64SectionList();
                    sectionList[currentSection].viewList = new OK64ViewList[4];
                    for (int currentView = 0; currentView < 4; currentView++)
                    {
                        sectionList[currentSection].viewList[currentView] = new OK64ViewList();
                        int objectCount = Convert.ToInt32(fileText[currentLine]);
                        currentLine++;
                        sectionList[currentSection].viewList[currentView].objectList = new int[objectCount];

                        for (int currentObject = 0; currentObject < objectCount; currentObject++)
                        {
                            sectionList[currentSection].viewList[currentView].objectList[currentObject] = Array.IndexOf(masterObjects,fileText[currentLine]);
                            currentLine++;
                        }
                    }
                }
            }
            else
            {
                if (fileText[0] != masterCount.ToString())
                {

                }
                else
                {

                    int sectionCount = Convert.ToInt32(fileText[1]);
                    sectionList = new OK64SectionList[sectionCount];
                    int currentLine = 2;
                    for (int currentSection = 0; currentSection < sectionCount; currentSection++)
                    {
                        sectionList[currentSection] = new OK64SectionList();
                        sectionList[currentSection].viewList = new OK64ViewList[4];
                        for (int currentView = 0; currentView < 4; currentView++)
                        {
                            sectionList[currentSection].viewList[currentView] = new OK64ViewList();
                            int objectCount = Convert.ToInt32(fileText[currentLine]);
                            currentLine++;
                            sectionList[currentSection].viewList[currentView].objectList = new int[objectCount];

                            for (int currentObject = 0; currentObject < objectCount; currentObject++)
                            {
                                sectionList[currentSection].viewList[currentView].objectList[currentObject] = Convert.ToInt32(fileText[currentLine]);
                                currentLine++;
                            }
                        }
                    }


                }
            }
            return sectionList;
        }


        public OK64SectionList[] automateSection(int sectionCount, OK64F3DObject[] surfaceObjects, OK64F3DObject[] masterObjects, Assimp.Scene fbx, bool raycastBoolean)
        {
            OK64SectionList[] sectionList = new OK64SectionList[sectionCount];


            List<int> searchList = new List<int>();


            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                sectionList[currentSection] = new OK64SectionList();
                sectionList[currentSection].viewList = new OK64ViewList[4];

                for (int currentView = 0; currentView < 4; currentView++)
                {
                    sectionList[currentSection].viewList[currentView] = new OK64ViewList();
                    List<int> tempList = new List<int>();

                    GeometryCompiler geometryCompiler;
                    if (System.Windows.Forms.Application.OpenForms["GeometryCompiler"] != null)
                    {
                        geometryCompiler = System.Windows.Forms.Application.OpenForms["GeometryCompiler"] as GeometryCompiler;
                        
                        geometryCompiler.updateOutput("Section " + (currentSection + 1).ToString() + "/" + sectionCount.ToString() + "  " + viewString[currentView]);
                    }




                    for (int currentObject = 0; currentObject < surfaceObjects.Length; currentObject++)
                    {
                        if (surfaceObjects[currentObject].surfaceID == (currentSection + 1) & surfaceObjects[currentObject].pathfindingObject.surfaceBoolean)
                        {
                            for (int currentFace = 0; currentFace < surfaceObjects[currentObject].modelGeometry.Length; currentFace++)
                            {
                                Vector3D raycastOrigin = surfaceObjects[currentObject].modelGeometry[currentFace].centerPosition;

                                switch (currentView)
                                {
                                    case 0:
                                        {
                                            raycastOrigin.Y -= 10;
                                            break;
                                        }
                                    case 1:
                                        {
                                            raycastOrigin.X -= 10;
                                            break;
                                        }
                                    case 2:
                                        {
                                            raycastOrigin.Y += 10;
                                            break;
                                        }
                                    case 3:
                                        {
                                            raycastOrigin.X += 10;
                                            break;
                                        }
                                }




                                raycastOrigin.Z += 8;
                                int screenWidth = 180;
                                int screenHeight = 160;
                                int resolution = 3;
                                int rayDepth = 30000;
                                

                                searchList = new List<int>();

                                for (int currentMaster = 0; currentMaster < masterObjects.Length; currentMaster++)
                                {
                                    switch (currentView)
                                    {
                                        case 0:
                                            {
                                                if (masterObjects[currentMaster].pathfindingObject.highY >= raycastOrigin.Y)
                                                {
                                                    searchList.Add(currentMaster);
                                                    string tempString = masterObjects[currentMaster].objectName;
                                                }
                                                else
                                                {
                                                    string tempString = masterObjects[currentMaster].objectName;
                                                }
                                                break;
                                            }
                                        case 1:
                                            {
                                                if (masterObjects[currentMaster].pathfindingObject.highX >= raycastOrigin.X)
                                                {
                                                    searchList.Add(currentMaster);
                                                    string tempString = masterObjects[currentMaster].objectName;
                                                    if (tempString == "11_part56")
                                                        tempString = masterObjects[currentMaster].objectName; ;
                                                }
                                                else
                                                {
                                                    string tempString = masterObjects[currentMaster].objectName;
                                                    if (tempString == "11_part56")
                                                        tempString = masterObjects[currentMaster].objectName; ;
                                                }
                                                break;
                                            }
                                        case 2:
                                            {
                                                if (masterObjects[currentMaster].pathfindingObject.lowY <= raycastOrigin.Y)
                                                {
                                                    searchList.Add(currentMaster);
                                                }
                                                break;
                                            }
                                        case 3:
                                            {
                                                if (masterObjects[currentMaster].pathfindingObject.lowX <= raycastOrigin.X)
                                                {
                                                    searchList.Add(currentMaster);
                                                    string tempString = masterObjects[currentMaster].objectName;
                                                    if (tempString == "11_part56")
                                                        tempString = surfaceObjects[currentObject].objectName; ;
                                                }
                                                else
                                                {
                                                    string tempString = masterObjects[currentMaster].objectName;
                                                    if (tempString == "11_part56")
                                                        tempString = surfaceObjects[currentObject].objectName; ;
                                                }
                                                break;
                                            }
                                    }
                                }


                                int[] searchObjects = searchList.ToArray();


                                if (raycastBoolean)
                                {
                                    for (int vPixel = 0; vPixel < screenHeight;)
                                    {
                                        int arcWidth = vPixel * 5 + 1;
                                        int horizontalPass = 0;
                                        if ((screenWidth / arcWidth) < resolution)
                                        {
                                            horizontalPass = resolution;
                                        }
                                        else
                                        {
                                            horizontalPass = (screenWidth / arcWidth);
                                        }



                                        for (int hPixel = 0; hPixel < screenWidth;)
                                        {

                                            Vector3D raycastVector = new Vector3D();
                                            switch (currentView)
                                            {
                                                case 0:
                                                    {
                                                        float hAngle = Convert.ToSingle(hPixel * (Math.PI / 180));
                                                        float vAngle = Convert.ToSingle((vPixel - 90) * (Math.PI / 180));

                                                        raycastVector.X = Convert.ToSingle(raycastOrigin.X + rayDepth * Math.Cos(hAngle) * Math.Sin(vAngle));
                                                        raycastVector.Y = Convert.ToSingle(raycastOrigin.Y + rayDepth * Math.Sin(hAngle) * Math.Sin(vAngle));
                                                        raycastVector.Z = Convert.ToSingle(raycastOrigin.Z + rayDepth * Math.Cos(vAngle));

                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        float hAngle = Convert.ToSingle((hPixel + 90) * (Math.PI / 180));
                                                        float vAngle = Convert.ToSingle((vPixel - 90) * (Math.PI / 180));
                                                        raycastVector.X = Convert.ToSingle(raycastOrigin.X + rayDepth * Math.Cos(hAngle) * Math.Sin(vAngle));
                                                        raycastVector.Y = Convert.ToSingle(raycastOrigin.Y + rayDepth * Math.Sin(hAngle) * Math.Sin(vAngle));
                                                        raycastVector.Z = Convert.ToSingle(raycastOrigin.Z + rayDepth * Math.Cos(vAngle));
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        float hAngle = Convert.ToSingle((hPixel + 180) * (Math.PI / 180));
                                                        float vAngle = Convert.ToSingle((vPixel - 90) * (Math.PI / 180));
                                                        raycastVector.X = Convert.ToSingle(raycastOrigin.X + rayDepth * Math.Cos(hAngle) * Math.Sin(vAngle));
                                                        raycastVector.Y = Convert.ToSingle(raycastOrigin.Y + rayDepth * Math.Sin(hAngle) * Math.Sin(vAngle));
                                                        raycastVector.Z = Convert.ToSingle(raycastOrigin.Z + rayDepth * Math.Cos(vAngle));
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        int tempH = hPixel + 270;
                                                        if (tempH > 360)
                                                            tempH -= 360;

                                                        float hAngle = Convert.ToSingle((tempH) * (Math.PI / 180));
                                                        float vAngle = Convert.ToSingle((vPixel - 90) * (Math.PI / 180));
                                                        raycastVector.X = Convert.ToSingle(raycastOrigin.X + rayDepth * Math.Cos(hAngle) * Math.Sin(vAngle));
                                                        raycastVector.Y = Convert.ToSingle(raycastOrigin.Y + rayDepth * Math.Sin(hAngle) * Math.Sin(vAngle));
                                                        raycastVector.Z = Convert.ToSingle(raycastOrigin.Z + rayDepth * Math.Cos(vAngle));
                                                        break;
                                                    }


                                            }


                                            int closestMaster = 0;
                                            int closestDistance = 0;
                                            int masterIndex = 0;







                                            for (int currentSearch = 0; currentSearch < searchObjects.Length; currentSearch++)
                                            {
                                                foreach (var searchFace in masterObjects[searchObjects[currentSearch]].modelGeometry)
                                                {



                                                    switch (currentView)
                                                    {
                                                        case 0:
                                                            {
                                                                if (searchFace.highY > raycastOrigin.Y)
                                                                {
                                                                    Vector3D intersectionPoint = testIntersect(raycastOrigin, raycastVector, searchFace.vertData[0], searchFace.vertData[1], searchFace.vertData[2]);

                                                                    if (intersectionPoint.X < closestDistance & intersectionPoint.X != -1)
                                                                    {
                                                                        closestMaster = searchObjects[currentSearch];
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                        case 1:
                                                            {
                                                                if (searchFace.highX > raycastOrigin.X)
                                                                {
                                                                    Vector3D intersectionPoint = testIntersect(raycastOrigin, raycastVector, searchFace.vertData[0], searchFace.vertData[1], searchFace.vertData[2]);

                                                                    if (intersectionPoint.X < closestDistance & intersectionPoint.X != -1)
                                                                    {
                                                                        closestMaster = searchObjects[currentSearch];
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                        case 2:
                                                            {
                                                                if (searchFace.lowY < raycastOrigin.Y)
                                                                {
                                                                    Vector3D intersectionPoint = testIntersect(raycastOrigin, raycastVector, searchFace.vertData[0], searchFace.vertData[1], searchFace.vertData[2]);

                                                                    if (intersectionPoint.X < closestDistance & intersectionPoint.X != -1)
                                                                    {
                                                                        closestMaster = searchObjects[currentSearch];
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                        case 3:
                                                            {
                                                                if (searchFace.lowX < raycastOrigin.X)
                                                                {
                                                                    Vector3D intersectionPoint = testIntersect(raycastOrigin, raycastVector, searchFace.vertData[0], searchFace.vertData[1], searchFace.vertData[2]);

                                                                    if (intersectionPoint.X < closestDistance & intersectionPoint.X != -1)
                                                                    {
                                                                        closestMaster = searchObjects[currentSearch];
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                    }
                                                }
                                            }

                                            masterIndex = Array.IndexOf(tempList.ToArray(), closestMaster);
                                            if (masterIndex == -1)
                                            {
                                                tempList.Add(closestMaster);
                                            }



                                            hPixel = hPixel + horizontalPass;
                                        }
                                        vPixel = vPixel + resolution;
                                    }

                                }
                                else
                                {

                                    foreach (var currentIndex in searchList)
                                    {
                                        int masterIndex = Array.IndexOf(tempList.ToArray(), currentIndex);
                                        if (masterIndex == -1)
                                        {
                                            tempList.Add(currentIndex);
                                        }


                                    }
                                }
                            }

                        }

                    }

                    sectionList[currentSection].viewList[currentView].objectList = tempList.ToArray();
                    
                }

            }


            return sectionList;
        }

        public byte[] writeTextures(byte[] rom, OK64Texture[] textureObject)
        {
            memoryStream = new MemoryStream();
            binaryReader = new BinaryReader(memoryStream);
            binaryWriter = new BinaryWriter(memoryStream);

            int segment5Position = 0;


            binaryWriter.Write(rom);
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
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    int addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                    if (addressAlign == 4)
                        addressAlign = 0;


                    for (int align = 0; align < addressAlign; align++)
                    {
                        binaryWriter.Write(Convert.ToByte(0x00));
                    }



                    // write compressed MIO0 texture to end of ROM.

                    textureObject[currentTexture].romPosition = Convert.ToInt32(binaryWriter.BaseStream.Length);
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    binaryWriter.Write(compressedTexture);
                }
            }
            
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));


            byte[] romOut = memoryStream.ToArray();
            return romOut;


        }


        public byte[] compiletextureTable(OK64Texture[] textureObject)
        {
            memoryStream = new MemoryStream();
            binaryReader = new BinaryReader(memoryStream);
            binaryWriter = new BinaryWriter(memoryStream);

            byte[] byteArray = new byte[0];



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


        public void compileF3DObject(ref int outMagic, ref byte[] outseg4, ref byte[] outseg7, byte[] segment4, byte[] segment7, OK64F3DObject[] courseObject, OK64Texture[] textureObject, int vertMagic)
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
                for (int subIndex = 0; subIndex < cObj.meshID.Length; subIndex++)
                {




                    
                    int facecount = cObj.modelGeometry.Length;


                    int materialID = new int();

                    materialID = cObj.materialID;

                    ///Ok so now that we've loaded the raw model data, let's start writing some F3DEX. God have mercy.

                    cObj.meshPosition[subIndex] = Convert.ToInt32(seg7m.Position);


                    byteArray = BitConverter.GetBytes(0xBB000001);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0xFFFFFFFF);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);


                    if (textureObject[cObj.materialID].textureTransparent == true)
                    {
                        //B900031D00553078

                        byteArray = BitConverter.GetBytes(0xB900031D);
                        Array.Reverse(byteArray);
                        seg7w.Write(byteArray);

                        byteArray = BitConverter.GetBytes(0x00553078);
                        Array.Reverse(byteArray);
                        seg7w.Write(byteArray);

                    }


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


                    byteArray = BitConverter.GetBytes(0x04000000 | (relativeZero) * 16);  ///from segment 4 at offset relativeZero
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);


                    int indexA;
                    int indexB;
                    int indexC;
                    

                    for (int faceIndex = 0; faceIndex < facecount;)
                    {



                        if (faceIndex + 2 <= facecount)
                        {


                            /// draw 2 triangles, check for additional verts in both.
                            if (relativeIndex >= 26)
                            {
                                relativeZero += relativeIndex;
                                relativeIndex = 0;

                                byteArray = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);


                                byteArray = BitConverter.GetBytes(0x04000000 | ((relativeZero) * 16));  ///from segment 4 at offset relativeZero
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);


                            }



                            ///end vert check




                            indexA = relativeIndex;
                            indexB = relativeIndex + 2;
                            indexC = relativeIndex + 1;

                            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0xB1000000 | (indexC << 17) | (indexB << 9) | indexA << 1));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);

                            indexA = relativeIndex + 3;
                            indexB = relativeIndex + 5;
                            indexC = relativeIndex + 4;

                            byteArray = BitConverter.GetBytes(Convert.ToUInt32((indexC << 17) | (indexB << 9) | indexA << 1));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);


                            for (int f = 0; f < 2; f++)
                            {
                                for (int v = 0; v < 3; v++)
                                {
                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].vertData[v].position.x));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].vertData[v].position.z));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(-1 * cObj.modelGeometry[f + faceIndex].vertData[v].position.y));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);


                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].vertData[v].position.s));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].vertData[v].position.t));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);


                                    seg4w.Write(cObj.modelGeometry[f + faceIndex].vertData[v].color.r);
                                    seg4w.Write(cObj.modelGeometry[f + faceIndex].vertData[v].color.g);
                                    seg4w.Write(cObj.modelGeometry[f + faceIndex].vertData[v].color.b);
                                    seg4w.Write(Convert.ToByte(0));
                                }
                            }


                            faceIndex += 2;
                            relativeIndex += 6;

                        }
                        else
                        {
                            
                            if (relativeIndex >= 26)
                            {
                                relativeZero += relativeIndex;
                                relativeIndex = 0;

                                byteArray = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);


                                byteArray = BitConverter.GetBytes(0x04000000 | ((relativeZero) * 16));  ///from segment 4 at offset relativeZero
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);

                                
                            }






                            ///end vert check
                            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0xBF000000));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);

                            indexA = relativeIndex;
                            indexB = relativeIndex + 2;
                            indexC = relativeIndex + 1;


                            byteArray = BitConverter.GetBytes(Convert.ToUInt32((indexC << 17) | (indexB << 9) | indexA << 1));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);


                            //create the vert array for the current face, write it to Segment 4. 
                            

                            

                            for (int v = 0; v < 3; v++)
                            {
                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].vertData[v].position.x));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].vertData[v].position.z));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(-1 * cObj.modelGeometry[faceIndex].vertData[v].position.y));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);


                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].vertData[v].position.s));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].vertData[v].position.t));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);


                                seg4w.Write(cObj.modelGeometry[faceIndex].vertData[v].color.r);
                                seg4w.Write(cObj.modelGeometry[faceIndex].vertData[v].color.g);
                                seg4w.Write(cObj.modelGeometry[faceIndex].vertData[v].color.b);
                                seg4w.Write(Convert.ToByte(0));
                            }

                            faceIndex++;
                            relativeIndex += 3;

                        }



                    }





                    if (textureObject[cObj.materialID].textureTransparent == true)
                    {
                        //B900031D00552078
                        //disable transparency.
                        byteArray = BitConverter.GetBytes(0xB900031D);
                        Array.Reverse(byteArray);
                        seg7w.Write(byteArray);

                        byteArray = BitConverter.GetBytes(0x00552078);
                        Array.Reverse(byteArray);
                        seg7w.Write(byteArray);

                    }


                    byteArray = BitConverter.GetBytes(0xB8000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    relativeZero += relativeIndex;
                    relativeIndex = 0;
                }





            }
            
            outseg4 = seg4m.ToArray();
            outseg7 = seg7m.ToArray();

            outMagic = relativeZero;
        }


        public void compileBattleObject(ref int outDLOffset, ref int outMagic, ref byte[] outseg4, ref byte[] outseg7, byte[] segment4, byte[] segment7, OK64F3DObject[] courseObject, OK64Texture[] textureObject, int vertMagic)
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
                for (int subIndex = 0; subIndex < cObj.meshID.Length; subIndex++)
                {





                    int facecount = cObj.modelGeometry.Length;


                    int materialID = new int();

                    materialID = cObj.materialID;

                    ///Ok so now that we've loaded the raw model data, let's start writing some F3DEX. God have mercy.

                    cObj.meshPosition[subIndex] = Convert.ToInt32(seg7m.Position);


                    byteArray = BitConverter.GetBytes(0xBB000001);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0xFFFFFFFF);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    // FC121824 FF33FFFF B900031D 00552078

                    byteArray = BitConverter.GetBytes(0xFC121824);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0xFF33FFFF);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0xB900031D);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00552078);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    if (textureObject[cObj.materialID].textureTransparent == true)
                    {
                        //B900031D00553078

                        byteArray = BitConverter.GetBytes(0xB900031D);
                        Array.Reverse(byteArray);
                        seg7w.Write(byteArray);

                        byteArray = BitConverter.GetBytes(0x00553078);
                        Array.Reverse(byteArray);
                        seg7w.Write(byteArray);

                    }


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


                    byteArray = BitConverter.GetBytes(0x04000000 | (relativeZero) * 16);  ///from segment 4 at offset relativeZero
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);


                    int indexA;
                    int indexB;
                    int indexC;


                    for (int faceIndex = 0; faceIndex < facecount;)
                    {



                        if (faceIndex + 2 <= facecount)
                        {


                            /// draw 2 triangles, check for additional verts in both.
                            if (relativeIndex >= 26)
                            {
                                relativeZero += relativeIndex;
                                relativeIndex = 0;

                                byteArray = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);


                                byteArray = BitConverter.GetBytes(0x04000000 | ((relativeZero) * 16));  ///from segment 4 at offset relativeZero
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);


                            }



                            ///end vert check




                            indexA = relativeIndex;
                            indexB = relativeIndex + 2;
                            indexC = relativeIndex + 1;

                            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0xB1000000 | (indexC << 17) | (indexB << 9) | indexA << 1));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);

                            indexA = relativeIndex + 3;
                            indexB = relativeIndex + 5;
                            indexC = relativeIndex + 4;

                            byteArray = BitConverter.GetBytes(Convert.ToUInt32((indexC << 17) | (indexB << 9) | indexA << 1));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);


                            for (int f = 0; f < 2; f++)
                            {
                                for (int v = 0; v < 3; v++)
                                {
                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].vertData[v].position.x));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].vertData[v].position.z));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(-1 * cObj.modelGeometry[f + faceIndex].vertData[v].position.y));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);


                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].vertData[v].position.s));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].vertData[v].position.t));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);


                                    seg4w.Write(cObj.modelGeometry[f + faceIndex].vertData[v].color.r);
                                    seg4w.Write(cObj.modelGeometry[f + faceIndex].vertData[v].color.g);
                                    seg4w.Write(cObj.modelGeometry[f + faceIndex].vertData[v].color.b);
                                    seg4w.Write(Convert.ToByte(0));
                                }
                            }


                            faceIndex += 2;
                            relativeIndex += 6;

                        }
                        else
                        {

                            if (relativeIndex >= 26)
                            {
                                relativeZero += relativeIndex;
                                relativeIndex = 0;

                                byteArray = BitConverter.GetBytes(0x040081FF);  ///load 32 vertices at index 0
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);


                                byteArray = BitConverter.GetBytes(0x04000000 | ((relativeZero) * 16));  ///from segment 4 at offset relativeZero
                                Array.Reverse(byteArray);
                                seg7w.Write(byteArray);


                            }






                            ///end vert check
                            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0xBF000000));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);

                            indexA = relativeIndex;
                            indexB = relativeIndex + 2;
                            indexC = relativeIndex + 1;


                            byteArray = BitConverter.GetBytes(Convert.ToUInt32((indexC << 17) | (indexB << 9) | indexA << 1));
                            Array.Reverse(byteArray);
                            seg7w.Write(byteArray);


                            //create the vert array for the current face, write it to Segment 4. 




                            for (int v = 0; v < 3; v++)
                            {
                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].vertData[v].position.x));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].vertData[v].position.z));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(-1 * cObj.modelGeometry[faceIndex].vertData[v].position.y));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);


                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].vertData[v].position.s));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].vertData[v].position.t));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);


                                seg4w.Write(cObj.modelGeometry[faceIndex].vertData[v].color.r);
                                seg4w.Write(cObj.modelGeometry[faceIndex].vertData[v].color.g);
                                seg4w.Write(cObj.modelGeometry[faceIndex].vertData[v].color.b);
                                seg4w.Write(Convert.ToByte(0));
                            }

                            faceIndex++;
                            relativeIndex += 3;

                        }



                    }





                    if (textureObject[cObj.materialID].textureTransparent == true)
                    {
                        //B900031D00552078
                        //disable transparency.
                        byteArray = BitConverter.GetBytes(0xB900031D);
                        Array.Reverse(byteArray);
                        seg7w.Write(byteArray);

                        byteArray = BitConverter.GetBytes(0x00552078);
                        Array.Reverse(byteArray);
                        seg7w.Write(byteArray);

                    }


                    byteArray = BitConverter.GetBytes(0xB8000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    relativeZero += relativeIndex;
                    relativeIndex = 0;
                }





            }

            outDLOffset = Convert.ToInt32(seg7w.BaseStream.Position);

            foreach (var cObj in courseObject)
            {
                foreach (var objposition in cObj.meshPosition)
                {
                    
                    byteArray = BitConverter.GetBytes(0x06000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(objposition | 0x07000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);
                    
                }
            }

            outseg4 = seg4m.ToArray();
            outseg7 = seg7m.ToArray();

            outMagic = relativeZero;
        }



        public byte[] compileF3DList(ref OK64SectionList[] sectionOut, Assimp.Scene fbx, OK64F3DObject[] courseObject, OK64SectionList[] sectionList)
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

        public byte[] compilesectionviewTable(OK64SectionList[] sectionList, int magic)
        {
            MemoryStream seg6m = new MemoryStream();
            BinaryReader seg6r = new BinaryReader(seg6m);
            BinaryWriter seg6w = new BinaryWriter(seg6m);

            byte[] byteArray = new byte[0];



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