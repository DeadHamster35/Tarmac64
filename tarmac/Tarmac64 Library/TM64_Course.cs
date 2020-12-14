using Assimp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarmac64_Library;
using Texture64;

namespace Tarmac64_Library
{
    class TM64_Course
    {


        TM64 Tarmac = new TM64();


        public class Course
        {
            public TM64_Geometry.OK64F3DObject[] MasterObjects { get; set; }
            public TM64_Geometry.OK64F3DObject[] SurfaceObjects { get; set; }
            public TM64_Geometry.OK64Texture[] TextureObjects { get; set; }
            public TM64_Geometry.OK64SectionList[] SectionList { get; set; }

            public byte[] Segment4 { get; set; }
            public byte[] Segment6 { get; set; }
            public byte[] Segment7 { get; set; }
            public byte[] Segment9 { get; set; }

            public string Credits { get; set; }
            public string PreviewPath { get; set; }
            public string BannerPath { get; set; }
            public string AssmeblyPath { get; set; }
            public string GhostPath { get; set; }
            public int[] GameTempos { get; set; }
            public int[] EchoValues { get; set; }
            public int MusicID { get; set; }
            public MiniMap MapData { get; set; }
            public Sky SkyColors { get; set; }
            public MenuHeader MenuHeaderData { get; set; }
            public OK64Header OK64HeaderData { get; set; }
            public CourseHeader HeaderData { get; set; }
            public int PathLength { get; set; }
            public float WaterLevel { get; set; }
        }
        public class MiniMap
        {
            public string MinimapPath { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public Vector2D MapCoord { get; set; }
            public Vector2D StartCoord { get; set; }
            public TM64_Geometry.OK64Color MapColor { get; set; }
            public float MapScale { get; set; }

        }
        public class MenuHeader
        {
            public int Preview { get; set; }
            public int Banner { get; set; }
        }
        public class OK64Header
        {
            public int Version { get; set; }  //version 4
            public int Sky { get; set; }
            public int Credits { get; set; }
            public int Ghost { get; set; }
            public int Assembly { get; set; }
            public int Mods { get; set; }
            public int Maps { get; set; }
            public int Objects { get; set; }
            public byte[] MapX { get; set; }
            public byte[] MapY { get; set; }
            public byte[] EchoStart { get; set; }
            public byte[] EchoStop { get; set; }
            public byte[] Tempo { get; set; }
            public int MusicID { get; set; }

        }

        public class CourseHeader
        {
            public UInt32 s6Start { get; set; }
            public UInt32 s6End { get; set; }
            public UInt32 s47Start { get; set; }
            public UInt32 s47End { get; set; }
            public UInt32 s7Start { get; set; }
            public UInt32 s9Start { get; set; }
            public UInt32 s9End { get; set; }
            public UInt32 VertCount { get; set; }
            public UInt32 S7Size { get; set; }
            public UInt32 TexturePointer { get; set; }
        }

        public class Sky
        {
            public TM64_Geometry.OK64Color TopColor { get; set; }
            public TM64_Geometry.OK64Color MidTopColor { get; set; }
            public TM64_Geometry.OK64Color MidBotColor { get; set; }
            public TM64_Geometry.OK64Color BotColor { get; set; }
        }

        public class Header
        {
            public byte[] s6Start { get; set; }
            public byte[] s6End { get; set; }
            public byte[] s47Start { get; set; }
            public byte[] s47End { get; set; }
            public byte[] s9Start { get; set; }
            public byte[] s9End { get; set; }
            public byte[] S47Buffer { get; set; }
            public byte[] VertCount { get; set; }
            public byte[] S7Pointer { get; set; }
            public byte[] S7Size { get; set; }
            public byte[] TexturePointer { get; set; }
            public byte[] FlagPadding { get; set; }
        }


        public Header[] loadHeader(byte[] fileData)
        {


            Header[] courseHeader = new Header[20];

            MemoryStream memoryStream = new MemoryStream(fileData);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

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

                courseHeader[i].S47Buffer = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].S47Buffer);

                courseHeader[i].VertCount = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].VertCount);

                courseHeader[i].S7Pointer = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].S7Pointer);

                courseHeader[i].S7Size = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].S7Size);

                courseHeader[i].TexturePointer = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].TexturePointer);

                courseHeader[i].FlagPadding = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].FlagPadding);

            }


            return courseHeader;
        }







        //








        public byte[] CompileOverKart(Course courseData, byte[] fileData, int cID, int setID)
        {
            //HOTSWAP

            ///This takes precompiled segments and inserts them into the ROM file. It also updates the course header table to reflect
            /// the new data sizes. This allows for proper loading of the course so long as the segments are properly setup. All segment
            /// data should be precompressed where applicable, this assumes that segment 4 and segment 6 are MIO0 compressed and that
            /// Segment 7 has had it's special compression ran. Segment 9 has no compression. fileData is the ROM file as a byte array, and CID
            /// is the ID of the course we're looking to replace based on it's location in the course header table. 


            /// This writes all segments to the end of the file for simplicity. If data was larger than original (which it almost always will be for custom courses)
            /// then it cannot fit in the existing space without overwriting other course data. 
            /// 

            byte[] seg6 = Tarmac.CompressMIO0(courseData.Segment6);
            byte[] seg4 = Tarmac.CompressMIO0(courseData.Segment4);
            byte[] seg7 = Tarmac.compress_seg7(courseData.Segment7);


            byte[] flip = new byte[0];

            TM64_Geometry mk = new TM64_Geometry();
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(fileData, 0, fileData.Length);
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            BinaryReader binaryReader = new BinaryReader(memoryStream);






            int addressAlign = 0;




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


            courseData.MenuHeaderData = new MenuHeader();


            //Write Course Banner Texture
            if (courseData.BannerPath.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(courseData.BannerPath);
                N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                byte[] compressedData = Tarmac.CompressMIO0(imageData);


                courseData.MenuHeaderData.Banner = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(compressedData);



                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
            }
            else
            {
                courseData.MenuHeaderData.Banner = Convert.ToInt32(0);
            }
            //


            //Course Preview Texture
            if (courseData.PreviewPath.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(courseData.PreviewPath);
                N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                byte[] compressedData = Tarmac.CompressMIO0(imageData);


                courseData.MenuHeaderData.Preview = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(compressedData);




                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
            }
            else
            {
                if (courseData.BannerPath.Length > 0)
                {
                    courseData.MenuHeaderData.Preview = Convert.ToInt32(binaryWriter.BaseStream.Position);
                }
                else
                {
                    courseData.MenuHeaderData.Preview = Convert.ToInt32(0);
                }
            }
            //








            //begin writing header info

            courseData.OK64HeaderData = new OK64Header();
            courseData.OK64HeaderData.Version = 4;

            //add sky colors


            courseData.OK64HeaderData.Sky = Convert.ToInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.TopColor.R);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.TopColor.G);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.TopColor.B);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidTopColor.R);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidTopColor.G);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidTopColor.B);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidBotColor.R);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidBotColor.G);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidBotColor.B);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.BotColor.R);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.BotColor.G);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.BotColor.B);



            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            //Credits
            if (courseData.Credits.Length > 0)
            {
                courseData.OK64HeaderData.Credits = Convert.ToInt32(binaryWriter.BaseStream.Position);

                flip = BitConverter.GetBytes(Convert.ToInt32(courseData.Credits.Length));
                Array.Reverse(flip);
                binaryWriter.Write(flip);
                binaryWriter.Write(Encoding.UTF8.GetBytes(courseData.Credits));
                binaryWriter.Write(0x00);

                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
            }
            else
            {
                courseData.OK64HeaderData.Credits = Convert.ToInt32(0);
            }
            //


            //Staff Ghost
            if (courseData.GhostPath.Length > 0)
            {
                courseData.OK64HeaderData.Ghost = Convert.ToInt32(binaryWriter.BaseStream.Position);
                byte[] ghostData = File.ReadAllBytes(courseData.GhostPath);
                binaryWriter.Write(ghostData);


                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
            }
            else
            {
                courseData.OK64HeaderData.Ghost = Convert.ToInt32(0);
            }
            //



            //custom ASM
            if (courseData.AssmeblyPath.Length > 0)
            {


                byte[] asmSequence = File.ReadAllBytes(courseData.AssmeblyPath);

                courseData.OK64HeaderData.Assembly = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(asmSequence);





                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
            }
            else
            {
                courseData.OK64HeaderData.Assembly = Convert.ToInt32(0);
            }
            //MOD INFO
            courseData.OK64HeaderData.Mods = Convert.ToInt32(0);


            //Write Course Map Texture
            if (courseData.MapData.MinimapPath.Length > 0)
            {
                courseData.OK64HeaderData.Maps = Convert.ToInt32(binaryWriter.BaseStream.Position);
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(courseData.MapData.MinimapPath);
                N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.I4, bitmapData);
                byte[] compressedData = Tarmac.CompressMIO0(imageData);

                courseData.MapData.Width = bitmapData.Width;
                courseData.MapData.Height = bitmapData.Height;





                addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }

                flip = BitConverter.GetBytes(Convert.ToInt16(courseData.MapData.MapCoord.X));
                Array.Reverse(flip);
                binaryWriter.Write(flip);

                flip = BitConverter.GetBytes(Convert.ToInt16(courseData.MapData.MapCoord.Y));
                Array.Reverse(flip);
                binaryWriter.Write(flip);

                flip = BitConverter.GetBytes(Convert.ToInt16(courseData.MapData.StartCoord.X));
                Array.Reverse(flip);
                binaryWriter.Write(flip);

                flip = BitConverter.GetBytes(Convert.ToInt16(courseData.MapData.StartCoord.Y));
                Array.Reverse(flip);
                binaryWriter.Write(flip);

                flip = BitConverter.GetBytes(Convert.ToInt16(courseData.MapData.Height));
                Array.Reverse(flip);
                binaryWriter.Write(flip);

                flip = BitConverter.GetBytes(Convert.ToInt16(courseData.MapData.Width));
                Array.Reverse(flip);
                binaryWriter.Write(flip);

                flip = BitConverter.GetBytes(Convert.ToInt16(courseData.MapData.MapColor.R));
                Array.Reverse(flip);
                binaryWriter.Write(flip);

                flip = BitConverter.GetBytes(Convert.ToInt16(courseData.MapData.MapColor.G));
                Array.Reverse(flip);
                binaryWriter.Write(flip);

                flip = BitConverter.GetBytes(Convert.ToInt16(courseData.MapData.MapColor.B));
                Array.Reverse(flip);
                binaryWriter.Write(flip);

                binaryWriter.Write(Convert.ToInt16(0));

                flip = BitConverter.GetBytes(courseData.MapData.MapScale);
                Array.Reverse(flip);
                binaryWriter.Write(flip);


                binaryWriter.Write(compressedData);
            }



            //OBJECTS
            courseData.OK64HeaderData.Objects = Convert.ToInt32(0);




            //

            //echo





            courseData.OK64HeaderData.Tempo = new byte[4];
            courseData.OK64HeaderData.Tempo[0] = Convert.ToByte(courseData.GameTempos[0]);
            courseData.OK64HeaderData.Tempo[1] = Convert.ToByte(courseData.GameTempos[1]);
            courseData.OK64HeaderData.Tempo[2] = Convert.ToByte(courseData.GameTempos[2]);
            courseData.OK64HeaderData.Tempo[3] = Convert.ToByte(courseData.GameTempos[3]);

            courseData.OK64HeaderData.MusicID = courseData.MusicID;




            courseData.HeaderData = new CourseHeader();
            // Segment 6

            courseData.HeaderData.s6Start = Convert.ToUInt32(binaryWriter.BaseStream.Position);


            binaryWriter.Write(seg6, 0, seg6.Length);

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            courseData.HeaderData.s6End = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            //


            // Segment 9
            courseData.HeaderData.s9Start = Convert.ToUInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(courseData.Segment9, 0, courseData.Segment9.Length);

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            courseData.HeaderData.s9End = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            //




            // Segment 4/7
            courseData.HeaderData.s47Start = Convert.ToUInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(seg4, 0, seg4.Length);

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            courseData.HeaderData.s7Start = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(seg7, 0, seg7.Length);


            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            courseData.HeaderData.s47End = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            UInt32 seg7RSP = Convert.ToUInt32(0x0F000000 | (courseData.HeaderData.s7Start - courseData.HeaderData.s47Start));

            //






            // Flip Endian on Course Header offsets.

            flip = BitConverter.GetBytes(courseData.HeaderData.s6Start);
            Array.Reverse(flip);
            courseData.HeaderData.s6Start = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(courseData.HeaderData.s6End);
            Array.Reverse(flip);
            courseData.HeaderData.s6End = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(courseData.HeaderData.s47Start);
            Array.Reverse(flip);
            courseData.HeaderData.s47Start = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(courseData.HeaderData.s47End);
            Array.Reverse(flip);
            courseData.HeaderData.s47End = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(courseData.HeaderData.s9Start);
            Array.Reverse(flip);
            courseData.HeaderData.s9Start = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(courseData.HeaderData.s9End);
            Array.Reverse(flip);
            courseData.HeaderData.s9End = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(seg7RSP);
            Array.Reverse(flip);
            seg7RSP = BitConverter.ToUInt32(flip, 0);
            //


            //calculate # verts

            courseData.HeaderData.VertCount = Convert.ToUInt32(courseData.Segment4.Length / 14);
            flip = BitConverter.GetBytes(courseData.HeaderData.VertCount);
            Array.Reverse(flip);
            courseData.HeaderData.VertCount = BitConverter.ToUInt32(flip, 0);
            //



            //seg7 size

            UInt32 seg7size = Convert.ToUInt32(courseData.Segment7.Length);
            flip = BitConverter.GetBytes(seg7size);
            Array.Reverse(flip);
            seg7size = BitConverter.ToUInt32(flip, 0);
            //


            /// After Calculating the offsets and values above we now write them past the end of the ROM.








            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            uint headerOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position);


            // Version 4

            flip = BitConverter.GetBytes(courseData.OK64HeaderData.Version);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //
            //



            //courseheader


            binaryWriter.Write(courseData.HeaderData.s6Start);
            binaryWriter.Write(courseData.HeaderData.s6End);
            binaryWriter.Write(courseData.HeaderData.s47Start);
            binaryWriter.Write(courseData.HeaderData.s47End);
            binaryWriter.Write(courseData.HeaderData.s9Start);
            binaryWriter.Write(courseData.HeaderData.s9End);

            flip = BitConverter.GetBytes(0x0F000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            binaryWriter.Write(courseData.HeaderData.VertCount);

            binaryWriter.Write(seg7RSP);


            binaryWriter.Write(seg7size);

            flip = BitConverter.GetBytes(0x09000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            flip = BitConverter.GetBytes(0x00000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //


            //ok64header 13 pointers in
            flip = BitConverter.GetBytes(courseData.OK64HeaderData.Sky);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(courseData.OK64HeaderData.Credits);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(courseData.OK64HeaderData.Ghost);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(courseData.OK64HeaderData.Assembly);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(courseData.OK64HeaderData.Mods);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(courseData.OK64HeaderData.Maps);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(courseData.OK64HeaderData.Objects);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(Convert.ToInt16(courseData.EchoValues[0]));
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(Convert.ToInt16(courseData.EchoValues[1]));
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            binaryWriter.Write(courseData.OK64HeaderData.Tempo[0]);
            binaryWriter.Write(courseData.OK64HeaderData.Tempo[1]);
            binaryWriter.Write(courseData.OK64HeaderData.Tempo[2]);
            binaryWriter.Write(courseData.OK64HeaderData.Tempo[3]);

            flip = BitConverter.GetBytes(courseData.OK64HeaderData.MusicID);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            flip = BitConverter.GetBytes(courseData.PathLength);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            flip = BitConverter.GetBytes(courseData.WaterLevel);
            Array.Reverse(flip);
            binaryWriter.Write(flip);


            for (int currentPad = 0; currentPad < 16; currentPad++)
            {
                flip = BitConverter.GetBytes(0xFFFFFFFF);
                Array.Reverse(flip);
                binaryWriter.Write(flip);
            }

            addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
            if (addressAlign == 4)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            //




            binaryWriter.BaseStream.Position = (0xBE9178 + (setID * 0x50) + (cID * 4));
            flip = BitConverter.GetBytes(headerOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            binaryWriter.BaseStream.Position = (0xBEA578 + (setID * 0x50) + (cID * 8));

            flip = BitConverter.GetBytes(courseData.MenuHeaderData.Banner);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(courseData.MenuHeaderData.Preview);
            Array.Reverse(flip);
            binaryWriter.Write(flip);








            byte[] newROM = memoryStream.ToArray();
            return newROM;

        }

    }
}
