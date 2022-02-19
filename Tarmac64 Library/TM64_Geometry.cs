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
using Assimp;  //for handling model data
using Texture64;  //for handling texture data
using Cereal64.Microcodes.F3DEX.DataElements;
using Cereal64.Common.DataElements;
using Cereal64.Common.Rom;
using Cereal64.Common.Utils.Encoding;
using System.Text.RegularExpressions;
using System.Security.Permissions;
using SharpDX;
using System.Windows;
using Tarmac64_Library;
using F3DSharp;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Tarmac64_Library
{
    public class TM64_Geometry
    {
        
        /// These are various functions for decompressing and handling the segment data for Mario Kart 64.

        string[] viewString = new string[4] { "North", "East", "South", "West" };

        public static int newint = 4;
        Random rValue = new Random();

        TM64 Tarmac = new TM64();
        F3DEX095 F3D = new F3DEX095();

        public static UInt32[] seg7_romptr = new UInt32[20];

        byte[] flip2 = new byte[2];
        UInt32 value32 = new UInt32();
        public class Face
        {
            public VertIndex VertIndex { get; set; }
            public int Material { get; set; }
            public Vertex[] VertData { get; set; }
            public Assimp.Vector3D CenterPosition { get; set; }
            public float HighX { get; set; }
            public float HighY { get; set; }
            public float LowX { get; set; }
            public float LowY { get; set; }
        }

        public class VertIndex
        {
            public int IndexA { get; set; }
            public int IndexB { get; set; }
            public int IndexC { get; set; }

        }

        public class OK64JRSet
        {
            public OK64JRBlock[] BlockObject { get; set; }
            public TM64_Paths.Pathgroup PathGroup { get; set; }
        }

        public class OK64JRSpace
        {
            public int BlockID { get; set; }
            public int XIndex { get; set; }
            public int YIndex { get; set; }
            public int ZIndex { get; set; }
        }

        public class OK64JRBlock
        {
            public string BlockName { get; set; }
            public OK64F3DObject[] ObjectList { get; set; }
            public OK64F3DObject[] SurfaceList { get; set; }
            public TM64_Paths.Pathlist[] PathList { get; set; }
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
            public bool AdvancedSettings { get; set; }
            
            public UInt32[] CombineValuesA {get;set;}
            public UInt32[] CombineValuesB { get; set; }
            public int CombineModeA { get; set; }
            public int CombineModeB { get; set; }
            public int RenderModeA { get; set; }
            public int RenderModeB { get; set; }
            public UInt32 GeometryModes { get; set; }
            public bool[] GeometryBools { get; set; }
            public int BitSize { get; set; }
            public int TextureFormat { get; set; }
            public int SFlag { get; set; }
            public int TFlag { get; set; }
            public int textureScrollS { get; set; }
            public int textureScrollT { get; set; }
            public int textureScreen { get; set; }
            public Image textureBitmap { get; set; }
            public byte[] compressedTexture { get; set; }
            public byte[] PaletteData { get; set; }
            public byte[] rawTexture { get; set; }
            public int compressedSize { get; set; }
            public int fileSize { get; set; }
            public int segmentPosition { get; set; }
            public int palettePosition { get; set; }
            public int paletteSize { get; set; }
            public int romPosition { get; set; }
            public int[] f3dexPosition { get; set; }
            public int vertAlpha { get; set; }

        }
        public class OK64F3DGroup
        {
            public int[] subIndexes { get; set; }
            public string groupName { get; set; }
            public int sectionID { get; set; }

        }


        public class OK64Bone
        {
            public string Name { get; set; }
            public int FrameCount { get; set; }
            public short[] Origin { get; set; }
            public OK64Bone[] Children { get; set; }
            public int MeshCount { get; set; }       
            public OK64Animation Animation { get; set; }
            public int TranslationOffset { get; set; }
            public int MeshListOffset { get; set; }

        }


        public class OK64Animation
        {
            public string AnimationName { get; set; }
            public short[][] TranslationData { get; set; }
            public short[][] RotationData { get; set; }
            public short[][] ScalingData { get; set; }
        }

        public class OK64F3DObject
        {
            public string objectName { get; set; }
            public int vertCount { get; set; }
            public int faceCount { get; set; }
            public int materialID { get; set; }
            public int surfaceID { get; set; }
            public Byte surfaceMaterial { get; set; }
            public int[] meshID { get; set; }
            public int[] meshPosition { get; set; }
            public bool flagA { get; set; }
            public bool flagB { get; set; }
            public bool flagC { get; set; }
            public Face[] modelGeometry { get; set; }
            public float[] objectColor { get; set; }
            public int surfaceProperty { get; set; }
            public PathfindingObject pathfindingObject { get; set; }
            public string BoneName { get; set; }
            public int ListPosition { get; set; }

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
            public OK64Color color { get; set; }
            
        }

        public class OK64Color
        {
            public Byte R { get; set; }
            public Byte G { get; set; }
            public Byte B { get; set; }
            public Byte A { get; set; }
            public float RFloat { get; set; }
            public float GFloat { get; set; }
            public float BFloat { get; set; }
            public float AFloat { get; set; }
        }

        public class Position
        {

            public Int16 x { get; set; }
            public Int16 y { get; set; }
            public Int16 z { get; set; }
            public Int16 s { get; set; }
            public Int16 t { get; set; }
            public float sBase { get; set; }
            public float tBase { get; set; }
            public float sPure { get; set; }
            public float tPure { get; set; }
            public float u { get; set; }
            public float v { get; set; }

        }

        ///





        ///
        ///
        ///
        ///End of classes




        public int GetMax(int first, int second)
        {
            return first > second ? first : second; /// It will take care of all the 3 scenarios
        }

        public int GetMin(int first, int second)
        {
            return first < second ? first : second; /// It will take care of all the 3 scenarios
        }


        private const double Epsilon = 0.000001d;



        public TM64_Geometry.Face[] CreateStandard(float Size = 5.0f)
        {
            TM64_Geometry.Face[] StandardGeometry = new TM64_Geometry.Face[4];
            StandardGeometry[0] = new TM64_Geometry.Face();

            StandardGeometry[0].VertData = new TM64_Geometry.Vertex[3];

            StandardGeometry[0].VertData[0] = new TM64_Geometry.Vertex();
            StandardGeometry[0].VertData[0].position = new TM64_Geometry.Position();
            StandardGeometry[0].VertData[0].position.x = Convert.ToInt16(-1 * Size);
            StandardGeometry[0].VertData[0].position.y = Convert.ToInt16(0);
            StandardGeometry[0].VertData[0].position.z = Convert.ToInt16(0);

            StandardGeometry[0].VertData[1] = new TM64_Geometry.Vertex();
            StandardGeometry[0].VertData[1].position = new TM64_Geometry.Position();
            StandardGeometry[0].VertData[1].position.x = Convert.ToInt16(Size);
            StandardGeometry[0].VertData[1].position.y = Convert.ToInt16(0);
            StandardGeometry[0].VertData[1].position.z = Convert.ToInt16(0);

            StandardGeometry[0].VertData[2] = new TM64_Geometry.Vertex();
            StandardGeometry[0].VertData[2].position = new TM64_Geometry.Position();
            StandardGeometry[0].VertData[2].position.x = Convert.ToInt16(Size);
            StandardGeometry[0].VertData[2].position.y = Convert.ToInt16(0);
            StandardGeometry[0].VertData[2].position.z = Convert.ToInt16(Size * 2);

            
            StandardGeometry[1] = new TM64_Geometry.Face();
            StandardGeometry[1].VertData = new TM64_Geometry.Vertex[3];

            StandardGeometry[1].VertData[0] = new TM64_Geometry.Vertex();
            StandardGeometry[1].VertData[0].position = new TM64_Geometry.Position();
            StandardGeometry[1].VertData[0].position.x = Convert.ToInt16(0);
            StandardGeometry[1].VertData[0].position.y = Convert.ToInt16(-1 * Size);
            StandardGeometry[1].VertData[0].position.z = Convert.ToInt16(0);

            StandardGeometry[1].VertData[1] = new TM64_Geometry.Vertex();
            StandardGeometry[1].VertData[1].position = new TM64_Geometry.Position();
            StandardGeometry[1].VertData[1].position.x = Convert.ToInt16(0);
            StandardGeometry[1].VertData[1].position.y = Convert.ToInt16(Size);
            StandardGeometry[1].VertData[1].position.z = Convert.ToInt16(0);

            StandardGeometry[1].VertData[2] = new TM64_Geometry.Vertex();
            StandardGeometry[1].VertData[2].position = new TM64_Geometry.Position();
            StandardGeometry[1].VertData[2].position.x = Convert.ToInt16(0);
            StandardGeometry[1].VertData[2].position.y = Convert.ToInt16(Size);
            StandardGeometry[1].VertData[2].position.z = Convert.ToInt16(Size * 2);

            StandardGeometry[2] = new TM64_Geometry.Face();
            StandardGeometry[2].VertData = new TM64_Geometry.Vertex[3];

            StandardGeometry[2].VertData[0] = new TM64_Geometry.Vertex();
            StandardGeometry[2].VertData[0].position = new TM64_Geometry.Position();
            StandardGeometry[2].VertData[0].position.x = Convert.ToInt16(-1 * Size);
            StandardGeometry[2].VertData[0].position.y = Convert.ToInt16(0);
            StandardGeometry[2].VertData[0].position.z = Convert.ToInt16(0);

            StandardGeometry[2].VertData[1] = new TM64_Geometry.Vertex();
            StandardGeometry[2].VertData[1].position = new TM64_Geometry.Position();
            StandardGeometry[2].VertData[1].position.x = Convert.ToInt16(Size);
            StandardGeometry[2].VertData[1].position.y = Convert.ToInt16(0);
            StandardGeometry[2].VertData[1].position.z = Convert.ToInt16(Size * 2);

            StandardGeometry[2].VertData[2] = new TM64_Geometry.Vertex();
            StandardGeometry[2].VertData[2].position = new TM64_Geometry.Position();
            StandardGeometry[2].VertData[2].position.x = Convert.ToInt16(-1 * Size);
            StandardGeometry[2].VertData[2].position.y = Convert.ToInt16(0);
            StandardGeometry[2].VertData[2].position.z = Convert.ToInt16(Size* 2);


            StandardGeometry[3] = new TM64_Geometry.Face();
            StandardGeometry[3].VertData = new TM64_Geometry.Vertex[3];

            StandardGeometry[3].VertData[0] = new TM64_Geometry.Vertex();
            StandardGeometry[3].VertData[0].position = new TM64_Geometry.Position();
            StandardGeometry[3].VertData[0].position.x = Convert.ToInt16(0);
            StandardGeometry[3].VertData[0].position.y = Convert.ToInt16(-1 * Size);
            StandardGeometry[3].VertData[0].position.z = Convert.ToInt16(0);

            StandardGeometry[3].VertData[1] = new TM64_Geometry.Vertex();
            StandardGeometry[3].VertData[1].position = new TM64_Geometry.Position();
            StandardGeometry[3].VertData[1].position.x = Convert.ToInt16(0);
            StandardGeometry[3].VertData[1].position.y = Convert.ToInt16(Size);
            StandardGeometry[3].VertData[1].position.z = Convert.ToInt16(Size * 2);

            StandardGeometry[3].VertData[2] = new TM64_Geometry.Vertex();
            StandardGeometry[3].VertData[2].position = new TM64_Geometry.Position();
            StandardGeometry[3].VertData[2].position.x = Convert.ToInt16(0);
            StandardGeometry[3].VertData[2].position.y = Convert.ToInt16(-1 * Size);
            StandardGeometry[3].VertData[2].position.z = Convert.ToInt16(Size * 2);
            return StandardGeometry;
        }

        public Assimp.Vector3D testIntersect(Assimp.Vector3D rayOrigin, Assimp.Vector3D rayDirection, Vertex vertA, Vertex vertB, Vertex vertC)
        {

            Assimp.Vector3D vert0, vert1, vert2;

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
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var invDet = 1d / det;

            var tvec = rayOrigin - vert0;

            var u = Dot(tvec, pvec) * invDet;

            if (u < 0 || u > 1)
            {
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var qvec = Cross(tvec, edge1);

            var v = Dot(rayDirection, qvec) * invDet;

            if (v < 0 || u + v > 1)
            {
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var t = Dot(edge2, qvec) * invDet;

            return new Assimp.Vector3D((float)t, (float)u, (float)v);
        }


        public Assimp.Vector3D testIntersect(Assimp.Vector3D rayOrigin, Assimp.Vector3D rayDirection, Vertex vertA, Vertex vertB, Vertex vertC, float[] Origin)
        {

            Assimp.Vector3D vert0, vert1, vert2;

            vert0.X = vertA.position.x + Origin[0];
            vert0.Y = vertA.position.y + Origin[1];
            vert0.Z = vertA.position.z + Origin[2];

            vert1.X = vertB.position.x + Origin[0];
            vert1.Y = vertB.position.y + Origin[1];
            vert1.Z = vertB.position.z + Origin[2];

            vert2.X = vertC.position.x + Origin[0];
            vert2.Y = vertC.position.y + Origin[1];
            vert2.Z = vertC.position.z + Origin[2];

            var edge1 = vert1 - vert0;
            var edge2 = vert2 - vert0;

            var pvec = Cross(rayDirection, edge2);

            var det = Dot(edge1, pvec);

            if (det > -Epsilon && det < Epsilon)
            {
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var invDet = 1d / det;

            var tvec = rayOrigin - vert0;

            var u = Dot(tvec, pvec) * invDet;

            if (u < 0 || u > 1)
            {
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var qvec = Cross(tvec, edge1);

            var v = Dot(rayDirection, qvec) * invDet;

            if (v < 0 || u + v > 1)
            {
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var t = Dot(edge2, qvec) * invDet;

            return new Assimp.Vector3D((float)t, (float)u, (float)v);
        }

        public Assimp.Vector3D testIntersectScale(Assimp.Vector3D rayOrigin, Assimp.Vector3D rayDirection, Vertex vertA, Vertex vertB, Vertex vertC, float[] Origin, float[] Scale)
        {

            Assimp.Vector3D vert0, vert1, vert2;

            vert0.X = (vertA.position.x * Scale[0]) + Origin[0];
            vert0.Y = (vertA.position.y * Scale[1]) + Origin[1];
            vert0.Z = (vertA.position.z * Scale[2]) + Origin[2];

            vert1.X = (vertB.position.x * Scale[0]) + Origin[0];
            vert1.Y = (vertB.position.y * Scale[1]) + Origin[1];
            vert1.Z = (vertB.position.z * Scale[2]) + Origin[2];
            
            vert2.X = (vertC.position.x * Scale[0]) + Origin[0];
            vert2.Y = (vertC.position.y * Scale[1]) + Origin[1];
            vert2.Z = (vertC.position.z * Scale[2]) + Origin[2];

            var edge1 = vert1 - vert0;
            var edge2 = vert2 - vert0;

            var pvec = Cross(rayDirection, edge2);

            var det = Dot(edge1, pvec);

            if (det > -Epsilon && det < Epsilon)
            {
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var invDet = 1d / det;

            var tvec = rayOrigin - vert0;

            var u = Dot(tvec, pvec) * invDet;

            if (u < 0 || u > 1)
            {
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var qvec = Cross(tvec, edge1);

            var v = Dot(rayDirection, qvec) * invDet;

            if (v < 0 || u + v > 1)
            {
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var t = Dot(edge2, qvec) * invDet;

            return new Assimp.Vector3D((float)t, (float)u, (float)v);
        }

        public Assimp.Vector3D testIntersect(Assimp.Vector3D rayOrigin, Assimp.Vector3D rayDirection, Vertex vertA, Vertex vertB, Vertex vertC, int[] ZoneIndex)
        {

            Assimp.Vector3D vert0, vert1, vert2;

            vert0.X = vertA.position.x + (ZoneIndex[0] * 500);
            vert0.Y = vertA.position.y + (ZoneIndex[1] * 500);
            vert0.Z = vertA.position.z + (ZoneIndex[2] * 250);

            vert1.X = vertB.position.x + (ZoneIndex[0] * 500);
            vert1.Y = vertB.position.y + (ZoneIndex[1] * 500);
            vert1.Z = vertB.position.z + (ZoneIndex[2] * 250);

            vert2.X = vertC.position.x + (ZoneIndex[0] * 500);
            vert2.Y = vertC.position.y + (ZoneIndex[1] * 500);
            vert2.Z = vertC.position.z + (ZoneIndex[2] * 250);

            var edge1 = vert1 - vert0;
            var edge2 = vert2 - vert0;

            var pvec = Cross(rayDirection, edge2);

            var det = Dot(edge1, pvec);

            if (det > -Epsilon && det < Epsilon)
            {
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var invDet = 1d / det;

            var tvec = rayOrigin - vert0;

            var u = Dot(tvec, pvec) * invDet;

            if (u < 0 || u > 1)
            {
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var qvec = Cross(tvec, edge1);

            var v = Dot(rayDirection, qvec) * invDet;

            if (v < 0 || u + v > 1)
            {
                Assimp.Vector3D returnVector = new Assimp.Vector3D();
                return returnVector;
            }

            var t = Dot(edge2, qvec) * invDet;

            return new Assimp.Vector3D((float)t, (float)u, (float)v);
        }


        private static double Dot(Assimp.Vector3D v1, Assimp.Vector3D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        private static Assimp.Vector3D Cross(Assimp.Vector3D v1, Assimp.Vector3D v2)
        {
            Assimp.Vector3D dest;

            dest.X = v1.Y * v2.Z - v1.Z * v2.Y;
            dest.Y = v1.Z * v2.X - v1.X * v2.Z;
            dest.Z = v1.X * v2.Y - v1.Y * v2.X;

            return dest;
        }

        public static Assimp.Vector3D GetTrilinearCoordinateOfTheHit(float t, Assimp.Vector3D rayOrigin, Assimp.Vector3D rayDirection)
        {
            return rayDirection * t + rayOrigin;
        }


        public string F3DEX_Model(out Vertex[] vertOutput, out int outClass, byte commandbyte, byte[] segment, byte[] seg4, int vertoffset, int segmentoffset, Vertex[] vertCache, int texClass, bool returnBad = true)
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
            float[] uval = new float[3];
            float[] vval = new float[3];
            float[] height = new float[] { 32, 32, 64, 32, 32, 64 };
            float[] width = new float[] { 32, 64, 32, 32, 64, 32 };

            mainsegr.BaseStream.Position = segmentoffset;


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



                    xval[0] = vertCache[indexA].position.x;
                    zval[0] = vertCache[indexA].position.y;
                    yval[0] = vertCache[indexA].position.z * -1;
                    uval[0] = Convert.ToSingle(vertCache[indexA].position.s / 32.0 / width[texClass]);
                    vval[0] = Convert.ToSingle(vertCache[indexA].position.t / -32.0 / height[texClass]);


                    xval[1] = vertCache[indexB].position.x;
                    zval[1] = vertCache[indexB].position.y;
                    yval[1] = vertCache[indexB].position.z * -1;
                    uval[1] = Convert.ToSingle(vertCache[indexB].position.s / 32.0 / width[texClass]);
                    vval[1] = Convert.ToSingle(vertCache[indexB].position.t / -32.0 / height[texClass]);

                    xval[2] = vertCache[indexC].position.x;
                    zval[2] = vertCache[indexC].position.y;
                    yval[2] = vertCache[indexC].position.z * -1;
                    uval[2] = Convert.ToSingle(vertCache[indexC].position.s / 32.0 / width[texClass]);
                    vval[2] = Convert.ToSingle(vertCache[indexC].position.t / -32.0 / height[texClass]);

                    ///outputstring = outputstring + "vertbox = mesh vertices:#([" + xval[0].ToString() + ",(" + (yval[0]).ToString() + ")," + zval[0].ToString() + "],[" + xval[1].ToString() + ",(" + (yval[1]).ToString() + ")," + zval[1].ToString() + "],[" + xval[2].ToString() + ",(" + (yval[2]).ToString() + ")," + zval[2].ToString() + "]) faces:#([1,2,3]) MaterialIDS:#(1) " + Environment.NewLine;
                    outputstring = outputstring + xval[0].ToString() + "," + yval[0].ToString() + "," + zval[0].ToString() + ";" + xval[1].ToString() + "," + yval[1].ToString() + "," + zval[1].ToString() + ";" + xval[2].ToString() + "," + yval[2].ToString() + "," + zval[2].ToString() + "," + Environment.NewLine;
                    outputstring = outputstring + uval[0].ToString() + "," + vval[0].ToString() + ";" + uval[1].ToString() + "," + vval[1].ToString() + ";" + uval[2].ToString() + "," + vval[2].ToString() + Environment.NewLine;
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


                mainsegr.BaseStream.Seek(4, SeekOrigin.Current);







                indexA = mainsegr.ReadByte() / 2;
                indexB = mainsegr.ReadByte() / 2;
                indexC = mainsegr.ReadByte() / 2;
                ///outputstring = outputstring + indexA.ToString() + "-" + indexB.ToString() + "-" + indexC.ToString() + "-" + vertoffset.ToString() + "-" + mainsegr.BaseStream.Position.ToString() + Environment.NewLine;
                ////// outputs the 3 vert indexes as well as the vertoffset and offset into the segment that called this command.

                xval[0] = vertCache[indexA].position.x;
                zval[0] = vertCache[indexA].position.y;
                yval[0] = vertCache[indexA].position.z * -1;
                uval[0] = Convert.ToSingle(vertCache[indexA].position.s / 32.0 / width[texClass]);
                vval[0] = Convert.ToSingle(vertCache[indexA].position.t / -32.0 / height[texClass]);


                xval[1] = vertCache[indexB].position.x;
                zval[1] = vertCache[indexB].position.y;
                yval[1] = vertCache[indexB].position.z * -1;
                uval[1] = Convert.ToSingle(vertCache[indexB].position.s / 32.0 / width[texClass]);
                vval[1] = Convert.ToSingle(vertCache[indexB].position.t / -32.0 / height[texClass]);

                xval[2] = vertCache[indexC].position.x;
                zval[2] = vertCache[indexC].position.y;
                yval[2] = vertCache[indexC].position.z * -1;
                uval[2] = Convert.ToSingle(vertCache[indexC].position.s / 32.0 / width[texClass]);
                vval[2] = Convert.ToSingle(vertCache[indexC].position.t / -32.0 / height[texClass]);

                ///outputstring = outputstring + "vertbox = mesh vertices:#([" + xval[0].ToString() + ",(" + (yval[0]).ToString() + ")," + zval[0].ToString() + "],[" + xval[1].ToString() + ",(" + (yval[1]).ToString() + ")," + zval[1].ToString() + "],[" + xval[2].ToString() + ",(" + (yval[2]).ToString() + ")," + zval[2].ToString() + "]) faces:#([1,2,3]) MaterialIDS:#(1) " + Environment.NewLine;
                outputstring = outputstring + xval[0].ToString() + "," + yval[0].ToString() + "," + zval[0].ToString() + ";" + xval[1].ToString() + "," + yval[1].ToString() + "," + zval[1].ToString() + ";" + xval[2].ToString() + "," + yval[2].ToString() + "," + zval[2].ToString() + "," + Environment.NewLine;
                outputstring = outputstring + uval[0].ToString() + "," + vval[0].ToString() + ";" + uval[1].ToString() + "," + vval[1].ToString() + ";" + uval[2].ToString() + "," + vval[2].ToString() + Environment.NewLine;
                ///



            }
            if (commandbyte == 0xFD)
            {
                mainsegr.BaseStream.Seek(3, SeekOrigin.Current);

                byte[] rsp_add = mainsegr.ReadBytes(4);
                Array.Reverse(rsp_add);


                int Value = BitConverter.ToInt32(rsp_add, 0);
                String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                int segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                uint location = Convert.ToUInt32(Binary.Substring(8, 24), 2);
                outputstring = "NEWMATERIAL" + Environment.NewLine + location.ToString("X") + Environment.NewLine;
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
                    texClass = 6;
                    ///MessageBox.Show("6");
                }
                if (compar == "F51010000007C07C")
                {

                    texClass = 0;
                    ///MessageBox.Show("0");
                }
                if (compar == "F5102000000FC07C")
                {

                    texClass = 1;
                    ///MessageBox.Show("1");
                }
                if (compar == "F51010000007C0FC")
                {
                    texClass = 2;
                    ///MessageBox.Show("2");
                }
                if (compar == "F57010000007C07C")
                {
                    texClass = 3;
                    ///MessageBox.Show("3");
                }
                if (compar == "F5702000000FC07C")
                {
                    texClass = 4;
                    ///MessageBox.Show("4");
                }
                if (compar == "F57010000007C0FC")
                {
                    texClass = 5;
                    ///MessageBox.Show("5");
                }

                outputstring = "TEXCLASS" + Environment.NewLine + texClass.ToString() + Environment.NewLine;
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

                        

                        flip2 = seg4r.ReadBytes(2);
                        Array.Reverse(flip2);
                        vertCache[vertIndex + currentVert].position.s = BitConverter.ToInt16(flip2, 0);

                        flip2 = seg4r.ReadBytes(2);
                        Array.Reverse(flip2);
                        vertCache[vertIndex + currentVert].position.t = BitConverter.ToInt16(flip2, 0);


                        seg4r.BaseStream.Seek(0x2, SeekOrigin.Current);

                        vertCache[vertIndex + currentVert].color.R = seg4r.ReadByte();
                        vertCache[vertIndex + currentVert].color.G = seg4r.ReadByte();
                        vertCache[vertIndex + currentVert].color.B = seg4r.ReadByte();
                        vertCache[vertIndex + currentVert].color.A = seg4r.ReadByte();

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
                        //MessageBox.Show(outputstring + "-" + mainsegr.BaseStream.Position.ToString());
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


            outClass = texClass;
            vertOutput = vertCache;
            return outputstring;

        }


        public OK64Texture[] loadTextures(Assimp.Scene fbx, string filePath)
        {

            int materialCount = fbx.Materials.Count;
            OK64Texture[] textureArray = new OK64Texture[materialCount];
            
            for (int materialIndex = 0; materialIndex < materialCount; materialIndex++)
            {
                textureArray[materialIndex] = new TM64_Geometry.OK64Texture();
                textureArray[materialIndex].GeometryBools = new bool[F3DEX095_Parameters.GeometryModes.Length];
                if (fbx.Materials[materialIndex].TextureDiffuse.FilePath != null) 
                {   
                    textureArray[materialIndex].texturePath = fbx.Materials[materialIndex].TextureDiffuse.FilePath;
                    textureArray[materialIndex].textureName = Path.GetFileNameWithoutExtension(textureArray[materialIndex].texturePath);
                    
                    
                    textureArray[materialIndex].textureScrollS = 0;
                    textureArray[materialIndex].textureScrollT = 0;
                    textureArray[materialIndex].textureScreen = 0;
                    textureArray[materialIndex].SFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_WRAP);
                    textureArray[materialIndex].TFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_WRAP);
                    textureArray[materialIndex].vertAlpha = 255;

                    
                    textureArray[materialIndex].GeometryModes = 0;

                    textureArray[materialIndex].GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_ZBUFFER)] = true;
                    textureArray[materialIndex].GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_SHADE)] = true;
                    textureArray[materialIndex].GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_SHADING_SMOOTH)] = true;
                    textureArray[materialIndex].GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_CULL_BACK)] = true;
                    textureArray[materialIndex].GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_CLIPPING)] = true;

                    textureArray[materialIndex].CombineModeA = 6; //F3DEX095_Parameters.G_CC_MODULATERGBA;
                    textureArray[materialIndex].CombineModeB = 6;

                    textureArray[materialIndex].RenderModeA = Array.IndexOf(F3DEX095_Parameters.RenderModes, F3DEX095_Parameters.G_RM_AA_ZB_OPA_SURF);
                    textureArray[materialIndex].RenderModeB = Array.IndexOf(F3DEX095_Parameters.RenderModes, F3DEX095_Parameters.G_RM_AA_ZB_OPA_SURF2);

                    textureArray[materialIndex].TextureFormat = Array.IndexOf(F3DEX095_Parameters.TextureFormats, F3DEX095_Parameters.G_IM_FMT_RGBA);
                    textureArray[materialIndex].BitSize = Array.IndexOf(F3DEX095_Parameters.BitSizes, F3DEX095_Parameters.G_IM_SIZ_16b);


                    string mainDirectory = Path.GetDirectoryName(filePath);
                    textureArray[materialIndex].texturePath = Path.Combine(mainDirectory, textureArray[materialIndex].texturePath);
                    textureArray[materialIndex].texturePath = Path.GetFullPath(textureArray[materialIndex].texturePath);


                    if (File.Exists(textureArray[materialIndex].texturePath))
                    {
                        using (var fs = new FileStream(textureArray[materialIndex].texturePath, FileMode.Open, FileAccess.Read))
                        {
                            textureArray[materialIndex].textureBitmap = Image.FromStream(fs);                            
                        }
                        textureArray[materialIndex].textureHeight = textureArray[materialIndex].textureBitmap.Height;
                        textureArray[materialIndex].textureWidth = textureArray[materialIndex].textureBitmap.Width;

                        int TextureMass = (textureArray[materialIndex].textureHeight * textureArray[materialIndex].textureWidth);

                        
                        if (TextureMass > 2048)
                        {
                            
                        }
                        
                    }
                    else
                    {
                        
                        while (!(File.Exists(textureArray[materialIndex].texturePath)))
                        {
                            /*MessageBox.Show(textureArray[materialIndex].texturePath + " not found, browse to file!");
                            OpenFileDialog fileOpen = new OpenFileDialog();
                            if (fileOpen.ShowDialog() == DialogResult.OK)
                            {
                                textureArray[materialIndex].texturePath = fileOpen.FileName;
                                using (var fs = new FileStream(textureArray[materialIndex].texturePath, FileMode.Open, FileAccess.Read))
                                {
                                    textureArray[materialIndex].textureBitmap = Image.FromStream(fs);
                                }


                                textureArray[materialIndex].textureHeight = textureArray[materialIndex].textureBitmap.Height;
                                textureArray[materialIndex].textureWidth = textureArray[materialIndex].textureBitmap.Width;
                            }
                            else
                            {
                                MessageBox.Show("ERROR FILE NOT SELECTED");
                            */
                                textureArray[materialIndex].textureHeight = 32;
                                textureArray[materialIndex].textureWidth = 32;
                                break;
                            //}
                        }
                    }
                    

                }
            }

            return textureArray;
        }




        public static IEnumerable<OK64F3DObject> NaturalSort(IEnumerable<OK64F3DObject> list)
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
        public OK64Bone LoadAnimationObject(out int Position, byte[] Data)
        {
            MemoryStream memoryStream = new MemoryStream(Data);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            int CurrentPosition = 0;
            int DataLength = 0;

            OK64Bone Skeleton = new OK64Bone();
            Skeleton.Name = binaryReader.ReadString();
            Skeleton.FrameCount = binaryReader.ReadInt32();

            Skeleton.Origin = new short[3];
            for (int ThisVector = 0; ThisVector < 3; ThisVector++)
            {
                Skeleton.Origin[ThisVector] = binaryReader.ReadInt16();
            }


            Skeleton.Animation = new OK64Animation();
            //
            DataLength = binaryReader.ReadInt32();
            Skeleton.Animation.RotationData = new short[DataLength][];
            for (int ThisRot = 0; ThisRot < Skeleton.Animation.RotationData.Length; ThisRot++)
            {
                Skeleton.Animation.RotationData[ThisRot] = new short[3];
                for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                {
                    Skeleton.Animation.RotationData[ThisRot][ThisVector] = binaryReader.ReadInt16();
                }
            }
            //
            DataLength = binaryReader.ReadInt32();
            Skeleton.Animation.TranslationData = new short[DataLength][];
            for (int ThisRot = 0; ThisRot < Skeleton.Animation.TranslationData.Length; ThisRot++)
            {
                Skeleton.Animation.TranslationData[ThisRot] = new short[3];
                for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                {
                    Skeleton.Animation.TranslationData[ThisRot][ThisVector] = binaryReader.ReadInt16();
                }
            }
            //
            DataLength = binaryReader.ReadInt32();
            Skeleton.Animation.ScalingData = new short[DataLength][];
            for (int ThisRot = 0; ThisRot < Skeleton.Animation.ScalingData.Length; ThisRot++)
            {
                Skeleton.Animation.ScalingData[ThisRot] = new short[3];
                for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                {
                    Skeleton.Animation.ScalingData[ThisRot][ThisVector] = binaryReader.ReadInt16();
                }
            }

            DataLength = binaryReader.ReadInt32();
            Skeleton.Children = new OK64Bone[DataLength];
            Position = Convert.ToInt32(binaryReader.BaseStream.Position);
            int DataRead = 0;
            int ThisRead = 0;
            for(int ThisChild = 0; ThisChild < DataLength; ThisChild++)
            {
                binaryReader.BaseStream.Position = Position + DataRead;
                byte[] NewData = binaryReader.ReadBytes(Convert.ToInt32(memoryStream.Length - binaryReader.BaseStream.Position));
                Skeleton.Children[ThisChild] = LoadAnimationObject(out ThisRead, NewData);
                DataRead += ThisRead;
            }
            Position += DataRead;
            return Skeleton;
        }
        public byte[] WriteAnimationObjects(OK64Bone Skeleton)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(Skeleton.Name);
            binaryWriter.Write(Skeleton.FrameCount);

            for (int ThisVector = 0; ThisVector < 3; ThisVector++)
            {
                binaryWriter.Write(Skeleton.Origin[ThisVector]);
            }
            if (Skeleton.Animation == null)
            {
                binaryWriter.Write(0);
                binaryWriter.Write(0);
                binaryWriter.Write(0);
            }
            else
            {

                if (Skeleton.Animation.RotationData != null)
                {
                    binaryWriter.Write(Skeleton.Animation.RotationData.Length);


                    for (int ThisRot = 0; ThisRot < Skeleton.Animation.RotationData.Length; ThisRot++)
                    {
                        for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                        {
                            binaryWriter.Write(Skeleton.Animation.RotationData[ThisRot][ThisVector]);
                        }
                    }
                }
                else
                {
                    binaryWriter.Write(0);
                }


                if (Skeleton.Animation.TranslationData != null)
                {

                    binaryWriter.Write(Skeleton.Animation.TranslationData.Length);

                    for (int ThisRot = 0; ThisRot < Skeleton.Animation.TranslationData.Length; ThisRot++)
                    {
                        for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                        {
                            binaryWriter.Write(Skeleton.Animation.TranslationData[ThisRot][ThisVector]);
                        }
                    }
                }
                else
                {
                    binaryWriter.Write(0);
                }

                if (Skeleton.Animation.ScalingData != null)
                {
                    binaryWriter.Write(Skeleton.Animation.ScalingData.Length);

                    for (int ThisRot = 0; ThisRot < Skeleton.Animation.ScalingData.Length; ThisRot++)
                    {
                        for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                        {
                            binaryWriter.Write(Skeleton.Animation.ScalingData[ThisRot][ThisVector]);
                        }
                    }
                }
                else
                {
                    binaryWriter.Write(0);
                }

            }


            binaryWriter.Write(Skeleton.Children.Length);


            foreach (var Child in Skeleton.Children)
            {
                binaryWriter.Write(WriteAnimationObjects(Child));
            }
            return memoryStream.ToArray();
        }

        public byte[] WriteTextureObjects(OK64Texture[] TextureData)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);


            binaryWriter.Write(TextureData.Length);
            for (int ThisTexture = 0; ThisTexture < TextureData.Length; ThisTexture++)
            {
                if (TextureData[ThisTexture].texturePath != null)
                {
                    binaryWriter.Write(TextureData[ThisTexture].texturePath);
                    binaryWriter.Write(TextureData[ThisTexture].textureName);
                    
                    binaryWriter.Write(TextureData[ThisTexture].textureScrollS);
                    binaryWriter.Write(TextureData[ThisTexture].textureScrollT);
                    binaryWriter.Write(TextureData[ThisTexture].textureScreen);

                    binaryWriter.Write(TextureData[ThisTexture].SFlag);
                    binaryWriter.Write(TextureData[ThisTexture].TFlag);

                    binaryWriter.Write(TextureData[ThisTexture].CombineModeA);
                    binaryWriter.Write(TextureData[ThisTexture].CombineModeB);

                    for (int ThisBool = 0; ThisBool < F3DEX095_Parameters.GeometryModes.Length; ThisBool++)
                    {
                        binaryWriter.Write(TextureData[ThisTexture].GeometryBools[ThisBool]);
                    }

                    binaryWriter.Write(TextureData[ThisTexture].RenderModeA);
                    binaryWriter.Write(TextureData[ThisTexture].RenderModeB);

                    binaryWriter.Write(TextureData[ThisTexture].TextureFormat);
                    binaryWriter.Write(TextureData[ThisTexture].BitSize);


                    binaryWriter.Write(TextureData[ThisTexture].vertAlpha);
                    
                    binaryWriter.Write(TextureData[ThisTexture].textureWidth);
                    binaryWriter.Write(TextureData[ThisTexture].textureHeight);
                    
                }
                else
                {
                    binaryWriter.Write("NULL");
                }
                    
            }

            return memoryStream.ToArray();
        }
        public byte[] WriteMasterObjects (OK64F3DObject[] ModelData)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(ModelData.Length);
            for (int ThisModel = 0; ThisModel < ModelData.Length; ThisModel++)
            {
                binaryWriter.Write(ModelData[ThisModel].objectName);
                binaryWriter.Write(ModelData[ThisModel].BoneName);
                binaryWriter.Write(ModelData[ThisModel].materialID);
                binaryWriter.Write(ModelData[ThisModel].vertCount);
                binaryWriter.Write(ModelData[ThisModel].faceCount);

                binaryWriter.Write(ModelData[ThisModel].meshID.Length);
                for (int ThisMesh = 0; ThisMesh < ModelData[ThisModel].meshID.Length; ThisMesh++)
                {
                    binaryWriter.Write(ModelData[ThisModel].meshID[ThisMesh]);
                }

                binaryWriter.Write(ModelData[ThisModel].modelGeometry.Length);
                for (int ThisGeo = 0; ThisGeo < ModelData[ThisModel].modelGeometry.Length; ThisGeo++)
                {
                    binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].LowX);
                    binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].HighX);
                    binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].LowY);
                    binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].HighY);
                    binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].CenterPosition.X);
                    binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].CenterPosition.Y);
                    binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].CenterPosition.Z);
                    binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertIndex.IndexA);
                    binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertIndex.IndexB);
                    binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertIndex.IndexC);

                    for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                    {
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.x);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.y);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.z);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.u);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.v);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.sBase);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.tBase);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.sPure);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.tPure);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].color.R);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].color.G);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].color.B);
                        binaryWriter.Write(ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].color.A);
                    }


                }
            }
            return memoryStream.ToArray();
        }

        public OK64F3DObject createObject (Assimp.Scene fbx, Assimp.Node objectNode, OK64Texture[] textureArray, bool ForceFlatUV = false)
        {
            OK64F3DObject newObject = new OK64F3DObject();


            newObject.objectColor = new float[3];
            newObject.objectColor[0] = rValue.NextFloat(0.3f, 1);
            newObject.objectColor[1] = rValue.NextFloat(0.3f, 1);
            newObject.objectColor[2] = rValue.NextFloat(0.3f, 1);
            newObject.objectName = objectNode.Name;
            newObject.meshID = objectNode.MeshIndices.ToArray();            

            if (newObject.meshID.Length == 0)
            {
                MessageBox.Show("Empty Course Object! -" + newObject.objectName);
            }
            newObject.materialID = fbx.Meshes[newObject.meshID[0]].MaterialIndex;
            int vertCount = 0;
            int faceCount = 0;

            



            List<int> xValues = new List<int>();
            List<int> yValues = new List<int>();

            

            foreach (var childMesh in objectNode.MeshIndices)
            {

                vertCount = vertCount + fbx.Meshes[childMesh].VertexCount;
                faceCount = faceCount + fbx.Meshes[childMesh].FaceCount;
                if (fbx.Meshes[childMesh].Bones.Count > 0)
                {
                    newObject.BoneName = fbx.Meshes[childMesh].Bones[0].Name;
                }
                else
                {
                    newObject.BoneName = "NULL";
                }
                
            }
            
            newObject.vertCount = vertCount;
            newObject.faceCount = faceCount;
            newObject.modelGeometry = new Face[faceCount];
            int currentFace = 0;
            //newObject.modelGeometry[currentFace];

            foreach (var childMesh in objectNode.MeshIndices)
            {

                foreach (var childPoly in fbx.Meshes[childMesh].Faces)
                {


                    List<int> l_xValues = new List<int>();
                    List<int> l_yValues = new List<int>();
                    List<int> l_zValues = new List<int>();

                    newObject.modelGeometry[currentFace] = new Face();
                    newObject.modelGeometry[currentFace].VertData = new Vertex[3];

                    newObject.modelGeometry[currentFace].VertIndex = new VertIndex();

                    if (childPoly.IndexCount != 3)
                    {
                        MessageBox.Show("FATAL ERROR - INDEX COUNT " + childPoly.IndexCount + "-" + newObject.objectName);
                    }
                    newObject.modelGeometry[currentFace].VertIndex.IndexA = Convert.ToInt16(childPoly.Indices[0]);
                    newObject.modelGeometry[currentFace].VertIndex.IndexB = Convert.ToInt16(childPoly.Indices[1]);
                    newObject.modelGeometry[currentFace].VertIndex.IndexC = Convert.ToInt16(childPoly.Indices[2]);

                    for (int currentVert = 0; currentVert < 3; currentVert++)
                    { 
                        newObject.modelGeometry[currentFace].VertData[currentVert] = new Vertex();
                        newObject.modelGeometry[currentFace].VertData[currentVert].position = new Position();
                        newObject.modelGeometry[currentFace].VertData[currentVert].position.x = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].X);
                        newObject.modelGeometry[currentFace].VertData[currentVert].position.y = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].Y);
                        newObject.modelGeometry[currentFace].VertData[currentVert].position.z = Convert.ToInt16(fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].Z);



                        xValues.Add(newObject.modelGeometry[currentFace].VertData[currentVert].position.x);
                        yValues.Add(newObject.modelGeometry[currentFace].VertData[currentVert].position.y);

                        l_xValues.Add(newObject.modelGeometry[currentFace].VertData[currentVert].position.x);
                        l_yValues.Add(newObject.modelGeometry[currentFace].VertData[currentVert].position.y);
                        l_zValues.Add(newObject.modelGeometry[currentFace].VertData[currentVert].position.z);



                        if (newObject.modelGeometry[currentFace].VertData[currentVert].position.x > newObject.modelGeometry[currentFace].HighX)
                        {
                            newObject.modelGeometry[currentFace].HighX = newObject.modelGeometry[currentFace].VertData[currentVert].position.x;
                        }
                        if (newObject.modelGeometry[currentFace].VertData[currentVert].position.x < newObject.modelGeometry[currentFace].LowX)
                        {
                            newObject.modelGeometry[currentFace].LowX = newObject.modelGeometry[currentFace].VertData[currentVert].position.x;
                        }
                        if (newObject.modelGeometry[currentFace].VertData[currentVert].position.y > newObject.modelGeometry[currentFace].HighY)
                        {
                            newObject.modelGeometry[currentFace].HighY = newObject.modelGeometry[currentFace].VertData[currentVert].position.y;
                        }
                        if (newObject.modelGeometry[currentFace].VertData[currentVert].position.y < newObject.modelGeometry[currentFace].LowY)
                        {
                            newObject.modelGeometry[currentFace].LowY = newObject.modelGeometry[currentFace].VertData[currentVert].position.y;
                        }


                        newObject.modelGeometry[currentFace].VertData[currentVert].color = new OK64Color();
                        if (fbx.Meshes[childMesh].VertexColorChannels[0].Count > 0)
                        {
                            newObject.modelGeometry[currentFace].VertData[currentVert].color.R = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].R * 255));
                            newObject.modelGeometry[currentFace].VertData[currentVert].color.G = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].G * 255));
                            newObject.modelGeometry[currentFace].VertData[currentVert].color.B = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].B * 255));
                            newObject.modelGeometry[currentFace].VertData[currentVert].color.A = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].A * 255));
                        }
                        else
                        {
                            newObject.modelGeometry[currentFace].VertData[currentVert].color.R = 252;
                            newObject.modelGeometry[currentFace].VertData[currentVert].color.G = 252;
                            newObject.modelGeometry[currentFace].VertData[currentVert].color.B = 252;
                            newObject.modelGeometry[currentFace].VertData[currentVert].color.A = 255;
                        }
                        newObject.modelGeometry[currentFace].VertData[currentVert].color.RFloat = Convert.ToSingle(newObject.modelGeometry[currentFace].VertData[currentVert].color.R) / 255;
                        newObject.modelGeometry[currentFace].VertData[currentVert].color.GFloat = Convert.ToSingle(newObject.modelGeometry[currentFace].VertData[currentVert].color.G) / 255;
                        newObject.modelGeometry[currentFace].VertData[currentVert].color.BFloat = Convert.ToSingle(newObject.modelGeometry[currentFace].VertData[currentVert].color.B) / 255;
                        newObject.modelGeometry[currentFace].VertData[currentVert].color.AFloat = Convert.ToSingle(newObject.modelGeometry[currentFace].VertData[currentVert].color.A) / 255;                        
                        if (newObject.modelGeometry[currentFace].VertData[currentVert].color.AFloat != 1.0)
                        {
                            int BreakPoint = 0;
                        }
                    }


                    float centerX = (l_xValues[0] + l_xValues[1] + l_xValues[2]) / 3;
                    float centerY = (l_yValues[0] + l_yValues[1] + l_yValues[2]) / 3;
                    float centerZ = (l_zValues[0] + l_zValues[1] + l_zValues[2]) / 3;

                    newObject.modelGeometry[currentFace].CenterPosition = new Assimp.Vector3D(centerX, centerY, centerZ);




                    //UV coords

                    if (ForceFlatUV)
                    {
                        for (int currentVert = 0; currentVert < 3; currentVert++)
                        {
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.sBase = 0;
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.tBase = 0;
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.sPure = 0;
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.tPure = 0;
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.u = 0;
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.v = 0;
                        }
                    }
                    else
                    {
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



                        if (fbx.Meshes[childMesh].TextureCoordinateChannels[0].Count == 0)
                        {
                            u_offset[0] = 0;
                            v_offset[0] = 0;

                            u_offset[1] = 0;
                            v_offset[1] = 0;

                            u_offset[2] = 0;
                            v_offset[2] = 0;
                        }
                        else
                        {
                            u_offset[0] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[0]][0]);
                            v_offset[0] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[0]][1]);

                            u_offset[1] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[1]][0]);
                            v_offset[1] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[1]][1]);

                            u_offset[2] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[2]][0]);
                            v_offset[2] = Convert.ToSingle(fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[2]][1]);
                        }


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

                        for (int currentVert = 0; currentVert < 3; currentVert++)
                        {

                            newObject.modelGeometry[currentFace].VertData[currentVert].position.sBase = u_offset[currentVert] * 32;
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.tBase = v_offset[currentVert] * -32;
                            if (fbx.Meshes[childMesh].TextureCoordinateChannels[0].Count == 0)
                            {

                                newObject.modelGeometry[currentFace].VertData[currentVert].position.sPure = 0;
                                newObject.modelGeometry[currentFace].VertData[currentVert].position.tPure = 0;

                            }
                            else
                            {
                                newObject.modelGeometry[currentFace].VertData[currentVert].position.sPure = fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[currentVert]][0] * 32;
                                newObject.modelGeometry[currentFace].VertData[currentVert].position.tPure = (fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[currentVert]][1]) * -32;
                            }
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.u = u_offset[currentVert];
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.v = v_offset[currentVert] * -1;
                        }
                    }




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

                newObject.pathfindingObject = new PathfindingObject();
                newObject.pathfindingObject.highX = localMax[0];
                newObject.pathfindingObject.lowX = localMax[1];
                newObject.pathfindingObject.highY = localMax[2];
                newObject.pathfindingObject.lowY = localMax[3];



            }
            return newObject;
        }


        public bool CheckST(OK64F3DObject Object, OK64Texture textureObject)
        {
            bool CheckError = false;
            foreach (var Mesh in Object.modelGeometry)
            {
                foreach (var Vert in Mesh.VertData)
                {
                    
                    if (Vert.position.sBase * textureObject.textureWidth > 32768 | Vert.position.sBase * textureObject.textureWidth < -32768)
                    {
                        CheckError = true;
                    }
                    else
                    {
                        if (Vert.position.tBase * textureObject.textureHeight > 32768 | Vert.position.tBase * textureObject.textureHeight < -32768)
                        {
                            CheckError = true;
                        }
                    }
                }
            }
            return CheckError;
        }

        OK64F3DObject[] GroupSort(OK64F3DObject[] masterObjects, OK64F3DGroup[] groupArray)
        {
            List<OK64F3DObject> groupObjects = new List<OK64F3DObject>();
            List<OK64F3DObject> ungroupedObjects = new List<OK64F3DObject>();
            List<int> listedObjects = new List<int>();
            int newIndex = 0;
            for (int currentGroup = 0; currentGroup < groupArray.Length; currentGroup++)
            {
                for (int currentChild = 0; currentChild < groupArray[currentGroup].subIndexes.Length; currentChild++)
                {
                    groupObjects.Add(masterObjects[groupArray[currentGroup].subIndexes[currentChild]]);
                    listedObjects.Add(groupArray[currentGroup].subIndexes[currentChild]);
                    groupArray[currentGroup].subIndexes[currentChild] = newIndex;
                    newIndex++;
                }
            }
            for (int currentMaster = 0; currentMaster < masterObjects.Length; currentMaster++)
            {
                if (listedObjects.IndexOf(currentMaster) == -1)
                {
                    ungroupedObjects.Add(masterObjects[currentMaster]);

                }
            }

            List<OK64F3DObject> masterList = new List<OK64F3DObject>();
            masterList.AddRange(groupObjects);
            masterList.AddRange(ungroupedObjects);
            return masterList.ToArray();
        }

        public OK64F3DObject[] loadMaster(ref OK64F3DGroup[] groupArray, Assimp.Scene fbx, OK64Texture[] textureArray)
        {
            
            var masterNode = fbx.RootNode.FindNode("Course Master Objects");
            int childCount = masterNode.Children.Count;
            List<OK64F3DObject> masterList = new List<OK64F3DObject>();
            OK64F3DObject[] masterObjects = new OK64F3DObject[0];
            List<OK64F3DGroup> groupList = new List<OK64F3DGroup>();
            int masterCount = 0;
            
            for (int currentChild = 0; currentChild < childCount; currentChild++)
            {
                if (masterNode.Children[currentChild].Children.Count > 0)
                {
                    var groupParent = masterNode.Children[currentChild];
                    int grandparentCount = groupParent.Children.Count;
                    groupList.Add(new OK64F3DGroup());
                    int groupCount = groupList.Count - 1;
                    groupList[groupCount].groupName = groupParent.Name;
                    groupList[groupCount].subIndexes = new int[groupParent.Children.Count];
                    for (int currentGrandchild = 0; currentGrandchild < grandparentCount; currentGrandchild++)
                    {
                        groupList[groupCount].subIndexes[currentGrandchild] = masterCount;
                        masterList.Add(createObject(fbx, groupParent.Children[currentGrandchild],textureArray));
                        masterCount++;
                    }
                }
                else
                {
                    masterList.Add(createObject(fbx, masterNode.Children[currentChild], textureArray));
                    masterCount++;
                }
            }
            
            groupArray = groupList.ToArray();
            masterObjects = GroupSort(masterList.ToArray(), groupArray);
            return masterObjects;
        }



        public OK64F3DObject[] createObjects(Assimp.Scene fbx, OK64Texture[] textureArray)
        {
            List<OK64F3DObject> masterObjects = new List<OK64F3DObject>();
            int currentObject = 0; 
            var BaseNode = fbx.RootNode.FindNode("Master Objects");

            for (int childObject = 0; childObject < BaseNode.Children.Count; childObject++)
            {
                masterObjects.Add(createObject(fbx, BaseNode.Children[childObject], textureArray));
            }
            List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>(masterObjects);
            OK64F3DObject[] outputObjects = NaturalSort(masterObjects).ToArray();
            return outputObjects;
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
                    masterObjects.Add(createObject(fbx,surfaceNode.Children[childObject], textureArray));
                    currentObject++;
                }
                List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>(masterObjects);
            }

            OK64F3DObject[] outputObjects = NaturalSort(masterObjects).ToArray();

            return outputObjects;
        }



        public OK64F3DObject[] loadCollision (Assimp.Scene fbx, int sectionCount, int simpleFormat, OK64Texture[] textureArray)
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
                    surfaceObjects.Add(createObject(fbx,surfaceNode.Children[currentsubObject], textureArray, true));
                    int currentObject = surfaceObjects.Count - 1;
                    surfaceObjects[currentObject].surfaceID = currentSection + 1;
                    string[] surfaceID = surfaceObjects[currentObject].objectName.Split('_');
                    if (surfaceID[0].Length != 0)
                    {
                        surfaceObjects[currentObject].surfaceMaterial = Convert.ToByte(surfaceID[0]);
                    }
                    else
                    {
                        surfaceObjects[currentObject].surfaceMaterial = Convert.ToByte(surfaceID[1]);
                    }
                    surfaceObjects[currentObject].materialID = 1;
                    totalIndex++;
                }
            }
            return surfaceObjects.ToArray();
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
            
            File.WriteAllText(filePath, "SVL2" + Environment.NewLine);
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

                        string[] masterNames = new string[masterObjects.Length];

                        for (int currentName = 0; currentName < masterObjects.Length; currentName++)
                        {
                            masterNames[currentName] = masterObjects[currentName].objectName;
                        }

                        for (int currentObject = 0; currentObject < objectCount; currentObject++)
                        {
                            sectionList[currentSection].viewList[currentView].objectList[currentObject] = Array.IndexOf(masterNames, fileText[currentLine]);
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

        public PathfindingObject[] SurfaceBounds(OK64F3DObject[] surfaceObjects, int sectionCount)
        {
            PathfindingObject[] surfaceBoundaries = new PathfindingObject[sectionCount];

            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                surfaceBoundaries[currentSection] = new PathfindingObject();
                surfaceBoundaries[currentSection].highX = -9999999;
                surfaceBoundaries[currentSection].highY = -9999999;
                surfaceBoundaries[currentSection].lowX = 9999999;
                surfaceBoundaries[currentSection].lowY = 9999999;
            }
                
            for (int currentObject = 0; currentObject < surfaceObjects.Length; currentObject++)
            {
                var thisObject = surfaceObjects[currentObject];
                var thisBoundary = surfaceBoundaries[thisObject.surfaceID -1];

                if (thisBoundary.highX < thisObject.pathfindingObject.highX)
                    thisBoundary.highX = thisObject.pathfindingObject.highX;
                if (thisBoundary.highY < thisObject.pathfindingObject.highY)
                    thisBoundary.highY = thisObject.pathfindingObject.highY;
                if (thisBoundary.lowX > thisObject.pathfindingObject.lowX)
                    thisBoundary.lowX = thisObject.pathfindingObject.lowX;
                if (thisBoundary.lowY > thisObject.pathfindingObject.lowY)
                    thisBoundary.lowY = thisObject.pathfindingObject.lowY;

            }

            return surfaceBoundaries;
        }

        public OK64SectionList[] AutomateSection(int sectionCount, OK64F3DObject[] surfaceObjects, OK64F3DObject[] masterObjects, PathfindingObject[] surfaceBoundaries, Assimp.Scene fbx, int raycastBoolean)
        {
            OK64SectionList[] sectionList = new OK64SectionList[sectionCount];


            List<int> searchList = new List<int>();


            //DEBUG
            //sectionCount = 1;
            //DEBUG

            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                sectionList[currentSection] = new OK64SectionList();
                sectionList[currentSection].viewList = new OK64ViewList[4];






                for (int currentView = 0; currentView < 4; currentView++)
                {
                    sectionList[currentSection].viewList[currentView] = new OK64ViewList();
                    List<int> tempList = new List<int>();


                    searchList = new List<int>();

                    for (int currentMaster = 0; currentMaster < masterObjects.Length; currentMaster++)
                    {
                        switch (currentView)
                        {
                            case 0:
                                {
                                    if (masterObjects[currentMaster].pathfindingObject.highY >= surfaceBoundaries[currentSection].lowY- 10)
                                    {
                                        searchList.Add(currentMaster);
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    if (masterObjects[currentMaster].pathfindingObject.highX >= surfaceBoundaries[currentSection].lowX - 10)
                                    {
                                        searchList.Add(currentMaster);
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    if (masterObjects[currentMaster].pathfindingObject.lowY <= surfaceBoundaries[currentSection].highY + 10)
                                    {
                                        searchList.Add(currentMaster);
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    if (masterObjects[currentMaster].pathfindingObject.lowX <= surfaceBoundaries[currentSection].highX + 10)
                                    {
                                        searchList.Add(currentMaster);
                                    }
                                    break;
                                }
                        }
                    }


                    if (raycastBoolean > 0)
                    {
                        List<int> raycastList = new List<int>();

                        raycastList = RayTest(currentSection, searchList, surfaceObjects, masterObjects);
                        sectionList[currentSection].viewList[currentView].objectList = raycastList.ToArray();
                    }
                    else
                    {
                        sectionList[currentSection].viewList[currentView].objectList = searchList.ToArray();
                    }
                }

            }


            return sectionList;
        }


        public List<int> RayTest (int resolution, int currentView, List<int> raycastList, List<int> searchList, Assimp.Vector3D raycastOrigin, OK64F3DObject[] masterObjects)
        {
            int screenWidth = 180;
            int screenHeight = 160;
            int rayDepth = 30000;
            List<int> tempList = raycastList;


            int closestMaster = 0;
            float closestDistance = 0;
            int masterIndex = 0;

            for (int vPixel = 0; vPixel < screenHeight;)
            {


                for (int hPixel = 0; hPixel < screenWidth;)
                {

                    Assimp.Vector3D raycastVector = new Assimp.Vector3D();
                    float hAngle = new float();
                    int tempV = vPixel - 90;
                    if (tempV < 0)
                        tempV += 360;
                    float vAngle = Convert.ToSingle((vPixel - 90) * (Math.PI / 180));
                    
                    switch (currentView)
                    {
                        case 0:
                            {
                                int tempH = hPixel - 90;
                                if (tempH < 0)
                                    tempH += 360;
                                hAngle = Convert.ToSingle((tempH) * (Math.PI / 180));
                                break;
                            }
                        case 1:
                            {
                                hAngle = Convert.ToSingle(hPixel * (Math.PI / 180));
                                break;
                            }
                        case 2:
                            {
                                hAngle = Convert.ToSingle((hPixel + 90) * (Math.PI / 180));                                
                                break;
                            }
                        case 3:
                            {
                                int tempH = hPixel + 180;
                                if (tempH >= 360)
                                    tempH -= 360;

                                hAngle = Convert.ToSingle((tempH) * (Math.PI / 180));                             
                                break;
                            }
                    }

                    raycastVector.X = Convert.ToSingle(raycastOrigin.X + rayDepth * Math.Cos(hAngle) * Math.Sin(vAngle));
                    raycastVector.Y = Convert.ToSingle(raycastOrigin.Y + rayDepth * Math.Sin(hAngle) * Math.Sin(vAngle));
                    raycastVector.Z = Convert.ToSingle(raycastOrigin.Z + rayDepth * Math.Cos(vAngle));

                    for (int currentSearch = 0; currentSearch < searchList.Count; currentSearch++)
                    {

                        foreach (var searchFace in masterObjects[searchList[currentSearch]].modelGeometry)
                        {
                            switch (currentView)
                            {
                                case 0:
                                    {
                                        if (masterObjects[currentSearch].pathfindingObject.highY > raycastOrigin.Y)
                                        {
                                            Assimp.Vector3D intersectionPoint = testIntersect(raycastOrigin, raycastVector, searchFace.VertData[0], searchFace.VertData[1], searchFace.VertData[2]);

                                            if (intersectionPoint.X < closestDistance & intersectionPoint.X != -1)
                                            {
                                                closestDistance = intersectionPoint.X;
                                                closestMaster = currentSearch;
                                            }
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        if (masterObjects[currentSearch].pathfindingObject.highX > raycastOrigin.X)
                                        {
                                            Assimp.Vector3D intersectionPoint = testIntersect(raycastOrigin, raycastVector, searchFace.VertData[0], searchFace.VertData[1], searchFace.VertData[2]);

                                            if (intersectionPoint.X < closestDistance & intersectionPoint.X != -1)
                                            {
                                                closestDistance = intersectionPoint.X;
                                                closestMaster = currentSearch;
                                            }
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        if (masterObjects[currentSearch].pathfindingObject.lowY < raycastOrigin.Y)
                                        {
                                            Assimp.Vector3D intersectionPoint = testIntersect(raycastOrigin, raycastVector, searchFace.VertData[0], searchFace.VertData[1], searchFace.VertData[2]);

                                            if (intersectionPoint.X < closestDistance & intersectionPoint.X != -1)
                                            {
                                                closestDistance = intersectionPoint.X;
                                                closestMaster = currentSearch;
                                            }
                                        }
                                        break;
                                    }
                                case 3:
                                    {
                                        if (masterObjects[currentSearch].pathfindingObject.lowX < raycastOrigin.X)
                                        {
                                            Assimp.Vector3D intersectionPoint = testIntersect(raycastOrigin, raycastVector, searchFace.VertData[0], searchFace.VertData[1], searchFace.VertData[2]);

                                            if (intersectionPoint.X < closestDistance & intersectionPoint.X != -1)
                                            {
                                                closestDistance = intersectionPoint.X;
                                                closestMaster = currentSearch;
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
                    hPixel = hPixel + resolution;
                }
                vPixel = vPixel + resolution;
            }
            return tempList;            
        }

        public List<int> RayTest(int currentSection, List<int> searchList, OK64F3DObject[] surfaceObjects, OK64F3DObject[] masterObjects)
        {
            int screenWidth = 180;
            int screenHeight = 160;
            int rayDepth = 30000;
            List<int> tempList = new List<int>();
            Assimp.Vector3D raycastOrigin = new Assimp.Vector3D();
            Assimp.Vector3D raycastVector = new Assimp.Vector3D();

            List<int> checkList = searchList;
            int closestMaster = 0;
            
            int masterIndex = 0;


            for (int currentObject = 0; currentObject < surfaceObjects.Length; currentObject++)
            {
                if (surfaceObjects[currentObject].surfaceID == (currentSection + 1))
                {
                    for (int currentFace = 0; currentFace < surfaceObjects[currentObject].modelGeometry.Length; currentFace++)
                    {
                        
                        raycastOrigin = surfaceObjects[currentObject].modelGeometry[currentFace].CenterPosition;
                            
                        raycastOrigin.Z += 25;


                        for (int currentSearch = 0; (currentSearch < searchList.Count); currentSearch++)
                        {
                            foreach (var targetFace in masterObjects[searchList[currentSearch]].modelGeometry)
                            {
                                raycastVector = targetFace.CenterPosition;
                                Assimp.Vector3D targetPoint = testIntersect(raycastOrigin, raycastVector, targetFace.VertData[0], targetFace.VertData[1], targetFace.VertData[2]);
                                if (masterObjects[searchList[currentSearch]].objectName == "CourseObject_part63");
                                {
                                    int x = 0;
                                }
                                if (Math.Abs(targetPoint.X) > 0)
                                {
                                    float targetDistance = Math.Abs(targetPoint.X);

                                    closestMaster = searchList[currentSearch];
                                    for (int currentCheck = 0; currentCheck < checkList.Count; currentCheck++)
                                    {
                                        foreach (var searchFace in masterObjects[checkList[currentCheck]].modelGeometry)
                                        {
                                            Assimp.Vector3D intersectionPoint = testIntersect(raycastOrigin, raycastVector, searchFace.VertData[0], searchFace.VertData[1], searchFace.VertData[2]);

                                            if (Math.Abs(intersectionPoint.X) > 0)
                                            {
                                                if (Math.Abs(intersectionPoint.X) < targetDistance)
                                                {
                                                    closestMaster = checkList[currentCheck];
                                                    targetDistance = Math.Abs(intersectionPoint.X);
                                                }
                                            }
                                        }
                                    }
                                }

                                for (int TargetVert = 0; TargetVert < 3; TargetVert++)
                                {
                                    raycastVector = new Assimp.Vector3D(targetFace.VertData[TargetVert].position.x, targetFace.VertData[TargetVert].position.y, targetFace.VertData[TargetVert].position.z);
                                    targetPoint = testIntersect(raycastOrigin, raycastVector, targetFace.VertData[0], targetFace.VertData[1], targetFace.VertData[2]);

                                    if (Math.Abs(targetPoint.X) > 0)
                                    {
                                        float targetDistance = Math.Abs(targetPoint.X);

                                        closestMaster = searchList[currentSearch];
                                        for (int currentCheck = 0; currentCheck < checkList.Count; currentCheck++)
                                        {
                                            foreach (var searchFace in masterObjects[checkList[currentCheck]].modelGeometry)
                                            {
                                                Assimp.Vector3D intersectionPoint = testIntersect(raycastOrigin, raycastVector, searchFace.VertData[0], searchFace.VertData[1], searchFace.VertData[2]);

                                                if (Math.Abs(intersectionPoint.X) > 0)
                                                {
                                                    if (Math.Abs(intersectionPoint.X) < targetDistance)
                                                    {
                                                        closestMaster = checkList[currentCheck];
                                                        targetDistance = Math.Abs(intersectionPoint.X);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }

                            

                        }
                        masterIndex = Array.IndexOf(tempList.ToArray(), closestMaster);
                        if (masterIndex == -1)
                        {
                            tempList.Add(closestMaster);
                            searchList.Remove(closestMaster);
                        }
                        
                    }
                }
            }
            return tempList;
        }


        public byte[] writeRawTextures(byte[] SegmentData, OK64Texture[] textureObject, int DataLength)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            int TextureSize = SegmentData.Length;
            

            binaryWriter.Write(SegmentData);
            int textureCount = (textureObject.Length);
            for (int currentTexture = 0; currentTexture < textureCount; currentTexture++)
            {
                if ((textureObject[currentTexture].texturePath != null) && (textureObject[currentTexture].texturePath != "NULL"))
                {
                    // Establish codec and convert texture. Compress converted texture data via MIO0 compression

                    N64Codec[][] n64Codec = new N64Codec[][] {
                        new N64Codec[]{ N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA32 },
                        new N64Codec[]{ N64Codec.ONEBPP, N64Codec.ONEBPP , N64Codec.ONEBPP , N64Codec.ONEBPP },
                        new N64Codec[]{ N64Codec.CI4, N64Codec.CI8, N64Codec.CI8, N64Codec.CI8 },
                        new N64Codec[]{ N64Codec.IA4, N64Codec.IA8, N64Codec.IA8, N64Codec.IA8 },
                        new N64Codec[]{ N64Codec.I4, N64Codec.I8, N64Codec.I8, N64Codec.I8 }
                    };

                    byte[] imageData = null;
                    byte[] paletteData = null;
                    Bitmap bitmapData = new Bitmap(textureObject[currentTexture].texturePath);
                    
                    N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], bitmapData);
                    


                    // finish setting texture parameters based on new texture and compressed data.

                    textureObject[currentTexture].compressedSize = imageData.Length;
                    textureObject[currentTexture].fileSize = imageData.Length;
                    textureObject[currentTexture].segmentPosition = TextureSize;  // we need this to build out F3DEX commands later. 
                    TextureSize = TextureSize + textureObject[currentTexture].fileSize;


                    //adjust the MIO0 offset to an 8-byte address as required for N64.
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                    if (addressAlign == 16)
                        addressAlign = 0;


                    for (int align = 0; align < addressAlign; align++)
                    {
                        binaryWriter.Write(Convert.ToByte(0x00));
                    }



                    // write compressed MIO0 texture to end of ROM.

                    textureObject[currentTexture].romPosition = Convert.ToInt32(binaryWriter.BaseStream.Length) + DataLength;
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    binaryWriter.Write(imageData);
                }
            }

            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));


            byte[] romOut = memoryStream.ToArray();
            
            return romOut;


        }


        public byte[] writeModelTextures(byte[] SegmentData, OK64Texture[] textureObject, int DataLength)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            int TextureSize = SegmentData.Length;


            binaryWriter.Write(SegmentData);
            int textureCount = (textureObject.Length);
            for (int currentTexture = 0; currentTexture < textureCount; currentTexture++)
            {
                if ((textureObject[currentTexture].texturePath != null) && (textureObject[currentTexture].texturePath != "NULL"))
                {
                    // Establish codec and convert texture. Compress converted texture data via MIO0 compression

                    N64Codec[][] n64Codec = new N64Codec[][] {
                        new N64Codec[]{ N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA32 },
                        new N64Codec[]{ N64Codec.ONEBPP, N64Codec.ONEBPP , N64Codec.ONEBPP , N64Codec.ONEBPP },
                        new N64Codec[]{ N64Codec.CI4, N64Codec.CI8, N64Codec.CI8, N64Codec.CI8 },
                        new N64Codec[]{ N64Codec.IA4, N64Codec.IA8, N64Codec.IA8, N64Codec.IA8 },
                        new N64Codec[]{ N64Codec.I4, N64Codec.I8, N64Codec.I8, N64Codec.I8 }
                    };
                    byte[] imageData = null;
                    byte[] paletteData = null;
                    Bitmap bitmapData = new Bitmap(textureObject[currentTexture].texturePath);
                    N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], bitmapData);



                    // finish setting texture parameters based on new texture and compressed data.

                    textureObject[currentTexture].compressedSize = imageData.Length;
                    textureObject[currentTexture].fileSize = imageData.Length;
                    textureObject[currentTexture].segmentPosition = Convert.ToInt32(binaryWriter.BaseStream.Position + DataLength);


                    //adjust the MIO0 offset to an 8-byte address as required for N64.
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                    if (addressAlign == 16)
                        addressAlign = 0;


                    for (int align = 0; align < addressAlign; align++)
                    {
                        binaryWriter.Write(Convert.ToByte(0x00));
                    }



                    // write compressed MIO0 texture to end of ROM.

                    textureObject[currentTexture].romPosition = Convert.ToInt32(binaryWriter.BaseStream.Length) + DataLength;
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    binaryWriter.Write(imageData);
                }
            }

            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));


            byte[] romOut = memoryStream.ToArray();

            return romOut;


        }


        public void buildTextures(OK64Texture[] TextureArray)
        {
            int segment5Position = 0;
            for (int currentTexture = 0; currentTexture < TextureArray.Length; currentTexture++)
            {
                if ((TextureArray[currentTexture].texturePath != null) && (TextureArray[currentTexture].texturePath != "NULL"))
                {
                    // Establish codec and convert texture. Compress converted texture data via MIO0 compression


                    N64Codec[][] n64Codec = new N64Codec[][] {
                        new N64Codec[]{ N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA32 },
                        new N64Codec[]{ N64Codec.ONEBPP, N64Codec.ONEBPP , N64Codec.ONEBPP , N64Codec.ONEBPP },
                        new N64Codec[]{ N64Codec.CI4, N64Codec.CI8, N64Codec.CI8, N64Codec.CI8 },
                        new N64Codec[]{ N64Codec.IA4, N64Codec.IA8, N64Codec.IA8, N64Codec.IA8 },
                        new N64Codec[]{ N64Codec.I4, N64Codec.I8, N64Codec.I8, N64Codec.I8 }
                    };
                    byte[] imageData = null;
                    byte[] paletteData = null;
                    Bitmap bitmapData = new Bitmap(TextureArray[currentTexture].texturePath);
                    
                    N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[TextureArray[currentTexture].TextureFormat][TextureArray[currentTexture].BitSize], bitmapData);
                    TextureArray[currentTexture].compressedTexture = Tarmac.CompressMIO0(imageData);
                    TextureArray[currentTexture].rawTexture = imageData;
                    TextureArray[currentTexture].PaletteData = paletteData;

                    // finish setting texture parameters based on new texture and compressed data.

                    TextureArray[currentTexture].compressedSize = TextureArray[currentTexture].compressedTexture.Length;
                    TextureArray[currentTexture].fileSize = imageData.Length;
                    TextureArray[currentTexture].segmentPosition = segment5Position;  // we need this to build out F3DEX commands later.                     
                    segment5Position += TextureArray[currentTexture].fileSize;
                    if (paletteData != null)
                    {
                        TextureArray[currentTexture].paletteSize = paletteData.Length;
                        TextureArray[currentTexture].palettePosition = segment5Position;
                        segment5Position += TextureArray[currentTexture].paletteSize;
                        int addressAlign = 0x1000 - (segment5Position % 0x1000);
                        if (addressAlign == 0x1000)
                            addressAlign = 0;
                        segment5Position += addressAlign;
                    }
                }
            }
        }

        public byte[] WriteTextures(byte[] FileData, TM64_Course.Course Course)
        {
            OK64Texture[] TextureArray = Course.TextureObjects;
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            TM64 Tarmac = new TM64();
            

            

            for (int CurrentTexture = 0; CurrentTexture < TextureArray.Length; CurrentTexture++)
            {
                if ((TextureArray[CurrentTexture].texturePath != null) && (TextureArray[CurrentTexture].texturePath != "NULL"))
                {
                    //adjust the MIO0 offset to an 8-byte address as required for N64.
                    /*
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                    if (addressAlign == 16)
                        addressAlign = 0;


                    for (int align = 0; align < addressAlign; align++)
                    {
                        binaryWriter.Write(Convert.ToByte(0x00));
                    }

                    */

                    // write compressed MIO0 texture to end of ROM.

                    TextureArray[CurrentTexture].romPosition = Convert.ToInt32(binaryWriter.BaseStream.Length);
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    binaryWriter.Write(TextureArray[CurrentTexture].rawTexture);
                    if (TextureArray[CurrentTexture].PaletteData != null)
                    {
                        binaryWriter.Write(TextureArray[CurrentTexture].PaletteData);
                        int addressAlign = 0x1000 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 0x1000);
                        if (addressAlign == 0x1000)
                            addressAlign = 0;
                        for (int align = 0; align < addressAlign; align++)
                        {
                            binaryWriter.Write(Convert.ToByte(0x00));
                        }
                    }
                }
            }
            byte[] UncompressedData = memoryStream.ToArray();
            byte[] CompressedData = Tarmac.CompressMIO0(UncompressedData);

            memoryStream = new MemoryStream();
            binaryReader = new BinaryReader(memoryStream);
            binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(FileData);
            Course.Segment5ROM = Convert.ToInt32(binaryWriter.BaseStream.Position);
            Course.Segment5Length = UncompressedData.Length;
            Course.Segment5CompressedLength = CompressedData.Length;
            binaryWriter.Write(CompressedData);


            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            byte[] FileOut = memoryStream.ToArray();
            return FileOut;
        }


        public byte[] compiletextureTable(TM64_Course.Course CourseData)
        {
            OK64Texture[] textureObject = CourseData.TextureObjects;
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            byte[] byteArray = new byte[0];



            MemoryStream seg9m = new MemoryStream();
            BinaryReader seg9r = new BinaryReader(seg9m);
            BinaryWriter seg9w = new BinaryWriter(seg9m);

            int textureCount = (textureObject.Length);
            /*
            for (int currentTexture = 0; currentTexture < textureCount; currentTexture++) 
            {
                // write out segment 9 texture reference.
                if ((textureObject[currentTexture].texturePath != null) && (textureObject[currentTexture].texturePath != "NULL"))
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
            */
            byteArray = BitConverter.GetBytes(Convert.ToInt32(CourseData.Segment5ROM - 0x641F70));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);

            byteArray = BitConverter.GetBytes(Convert.ToInt32(CourseData.Segment5CompressedLength));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);

            byteArray = BitConverter.GetBytes(Convert.ToInt32(CourseData.Segment5Length));
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
            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);
            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);


            byte[] seg9Out = seg9m.ToArray();
            return seg9Out;
        }


        public byte[] compileF3DObject(byte[] ModelData, OK64F3DObject[] MasterObjects, OK64Texture[] TextureObjects, int vertMagic, int SegmentID)
        {


            //this function is used to create model data with vert data stored inside the same segment.
            //this is for "custom models" and is not intended for Mario Kart 64's traditional level format.

            List<string> object_name = new List<string>();




            byte[] byteArray = new byte[0];


            UInt32 ImgSize = 0, ImgType = 0, ImgFlag1 = 0, ImgFlag2 = 0, ImgFlag3 = 0;
            UInt32[] ImgTypes = { 0, 0, 0, 3, 3, 3, 0 }; ///0=RGBA, 3=IA
            UInt32[] STheight = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; ///looks like
            UInt32[] STwidth = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 }; ///texture sizes...
            byte[] heightex = { 5, 5, 6, 5, 5, 6, 5 };
            byte[] widthex = { 5, 6, 5, 5, 6, 5, 5 };

            byte[] SegmentByte = BitConverter.GetBytes(SegmentID);
            Array.Reverse(SegmentByte);

            int relativeZero = ModelData.Length + vertMagic;
            int relativeIndex = 0;


            MemoryStream seg7m = new MemoryStream();
            BinaryReader seg7r = new BinaryReader(seg7m);
            BinaryWriter seg7w = new BinaryWriter(seg7m);


            MemoryStream seg4m = new MemoryStream();
            BinaryReader seg4r = new BinaryReader(seg4m);
            BinaryWriter seg4w = new BinaryWriter(seg4m);

            foreach (var cObj in MasterObjects)
            {
                cObj.meshPosition = new int[cObj.meshID.Length];
                for (int subIndex = 0; subIndex < cObj.meshID.Length; subIndex++)
                {





                    int facecount = cObj.modelGeometry.Length;


                    int materialID = new int();

                    materialID = cObj.materialID;
                    int indexA;
                    int indexB;
                    int indexC;


                    for (int faceIndex = 0; faceIndex < facecount;)
                    {
                        if (faceIndex + 2 <= facecount)
                        {


                            for (int f = 0; f < 2; f++)
                            {
                                for (int v = 0; v < 3; v++)
                                {
                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.x));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.z));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(-1 * cObj.modelGeometry[f + faceIndex].VertData[v].position.y));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);


                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(0));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    if (CheckST(cObj, TextureObjects[cObj.materialID]))
                                    {
                                        //MessageBox.Show("Fatal UV Error " + cObj.objectName);
                                    }

                                    if ((F3DEX095_Parameters.TextureModes[TextureObjects[cObj.materialID].SFlag] != 0) || (F3DEX095_Parameters.TextureModes[TextureObjects[cObj.materialID].TFlag] != 0))
                                    {
                                        cObj.modelGeometry[f + faceIndex].VertData[v].position.s = Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.sPure * TextureObjects[cObj.materialID].textureWidth);
                                        cObj.modelGeometry[f + faceIndex].VertData[v].position.t = Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.tPure * TextureObjects[cObj.materialID].textureHeight);
                                    }
                                    else
                                    {
                                        cObj.modelGeometry[f + faceIndex].VertData[v].position.s = Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.sBase * TextureObjects[cObj.materialID].textureWidth);
                                        cObj.modelGeometry[f + faceIndex].VertData[v].position.t = Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.tBase * TextureObjects[cObj.materialID].textureHeight);
                                    }


                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.s));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.t));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    


                                    
                                    seg4w.Write(cObj.modelGeometry[f + faceIndex].VertData[v].color.R);
                                    seg4w.Write(cObj.modelGeometry[f + faceIndex].VertData[v].color.G);
                                    seg4w.Write(cObj.modelGeometry[f + faceIndex].VertData[v].color.B);
                                    seg4w.Write(cObj.modelGeometry[f + faceIndex].VertData[v].color.A);                                        
                                }
                            }


                            faceIndex += 2;
                            relativeIndex += 6;

                        }
                        else
                        {


                            //create the vert array for the current face, write it to Segment 4. 




                            for (int v = 0; v < 3; v++)
                            {
                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.x));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.z));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(-1 * cObj.modelGeometry[faceIndex].VertData[v].position.y));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);


                                byteArray = BitConverter.GetBytes(Convert.ToInt16(0));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                if (CheckST(cObj, TextureObjects[cObj.materialID]))
                                {
                                    MessageBox.Show("Fatal UV Error " + cObj.objectName);
                                }

                                if ((F3DEX095_Parameters.TextureModes[TextureObjects[cObj.materialID].SFlag] != 0) || (F3DEX095_Parameters.TextureModes[TextureObjects[cObj.materialID].TFlag] != 0))
                                {
                                    cObj.modelGeometry[faceIndex].VertData[v].position.s = Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.sPure * TextureObjects[cObj.materialID].textureWidth);
                                    cObj.modelGeometry[faceIndex].VertData[v].position.t = Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.tPure * TextureObjects[cObj.materialID].textureHeight);
                                }
                                else
                                {
                                    cObj.modelGeometry[faceIndex].VertData[v].position.s = Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.sBase * TextureObjects[cObj.materialID].textureWidth);
                                    cObj.modelGeometry[faceIndex].VertData[v].position.t = Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.tBase * TextureObjects[cObj.materialID].textureHeight);
                                }


                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.s));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.t));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);





                                seg4w.Write(cObj.modelGeometry[faceIndex].VertData[v].color.R);
                                seg4w.Write(cObj.modelGeometry[faceIndex].VertData[v].color.G);
                                seg4w.Write(cObj.modelGeometry[faceIndex].VertData[v].color.B);
                                seg4w.Write(cObj.modelGeometry[faceIndex].VertData[v].color.A);


                            }

                            faceIndex++;
                            relativeIndex += 3;

                        }



                    }



                    relativeZero += relativeIndex * 16;
                    relativeIndex = 0;
                }





            }

            int newMagic = relativeZero;
            relativeZero = ModelData.Length + vertMagic;
            UInt32 Segment = BitConverter.ToUInt32(SegmentByte, 0);
            UInt32 SAddress = 0;
            uint[] VertIndexesA, VertIndexesB = new uint[0];

            foreach (var cObj in MasterObjects)
            {
                cObj.meshPosition = new int[cObj.meshID.Length];
                for (int subIndex = 0; subIndex < cObj.meshID.Length; subIndex++)
                {
                    int facecount = cObj.modelGeometry.Length;
                    int materialID = cObj.materialID;

                    //Ok so now that we've loaded the raw model data, let's start writing some F3DEX. God have mercy.

                    cObj.meshPosition[subIndex] = Convert.ToInt32(seg7m.Position + newMagic);

                    //load the first set of verts from the relativeZero position;
                    

                    SAddress = Convert.ToUInt32( Segment | relativeZero );
                    seg7w.Write(F3D.gsSPVertex(SAddress, 32, 0));
                    



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
                                relativeZero += relativeIndex * 16;
                                relativeIndex = 0;

                                SAddress = Convert.ToUInt32(Segment | relativeZero);
                                seg7w.Write(F3D.gsSPVertex(SAddress, 32, 0));


                            }

                            ///end vert check

                            indexA = relativeIndex;
                            indexB = relativeIndex + 2;
                            indexC = relativeIndex + 1;
                            VertIndexesA = F3D.GetIndexes(indexA, indexB, indexC);
                            VertIndexesB = F3D.GetIndexes(indexA + 3, indexB + 3, indexC + 3);

                            seg7w.Write(F3D.gsSP2Triangles(VertIndexesA, VertIndexesB));


                            faceIndex += 2;
                            relativeIndex += 6;

                        }
                        else
                        {
                            /// draw 1 triangle, check for additional verts
                            if (relativeIndex >= 29)
                            {
                                relativeZero += relativeIndex * 16;
                                relativeIndex = 0;

                                SAddress = Convert.ToUInt32(Segment | relativeZero);
                                seg7w.Write(F3D.gsSPVertex(SAddress, 0, 32));


                            }

                            indexA = relativeIndex;
                            indexB = relativeIndex + 2;
                            indexC = relativeIndex + 1;

                            seg7w.Write(F3D.gsSP1Triangle(F3D.GetIndexes(indexA,indexB,indexC)));


                            //create the vert array for the current face, write it to Segment 4. 

                            faceIndex++;
                            relativeIndex += 3;

                        }



                    }

                    seg7w.Write(F3D.gsSPEndDisplayList());

                    relativeZero += relativeIndex * 16;
                    relativeIndex = 0;
                }





            }

            byte[] outseg4 = seg4m.ToArray();
            byte[] outseg7 = seg7m.ToArray();

            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outWriter.Write(ModelData);
            outWriter.Write(outseg4);
            outWriter.Write(outseg7);
            return outStream.ToArray();

            
        }

        public byte[] compileF3DHeader(int Position, byte[] HeaderData)
        {
            MemoryStream memoryStream = new MemoryStream();
            
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            memoryStream.Write(HeaderData,0,HeaderData.Length);

            byte[] flip = BitConverter.GetBytes(0x06000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            flip = BitConverter.GetBytes(Position);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            flip = BitConverter.GetBytes(0xB8000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            flip = BitConverter.GetBytes(0);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            return memoryStream.ToArray();
        }

        public void compileCourseObject(ref int outMagic, ref byte[] outseg4, ref byte[] outseg7, byte[] segment4, byte[] segment7, OK64F3DObject[] courseObject, OK64Texture[] textureObject, int vertMagic)
        {




            List<string> object_name = new List<string>();




            byte[] byteArray = new byte[0];



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



                    byte[] SegmentByte = BitConverter.GetBytes(4);
                    Array.Reverse(SegmentByte);
                    uint Segment = BitConverter.ToUInt32(SegmentByte,0);
                    ///load the first set of verts from the relativeZero position;


                    uint SAddress = Convert.ToUInt32(Segment | relativeZero);
                    seg7w.Write(F3D.gsSPVertex(SAddress, 32, 0));




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
                                relativeZero += relativeIndex * 16;
                                relativeIndex = 0;


                                SAddress = Convert.ToUInt32(Segment | relativeZero);
                                seg7w.Write(F3D.gsSPVertex(SAddress, 32, 0));

                            }



                            ///end vert check




                            indexA = relativeIndex;
                            indexB = relativeIndex + 2;
                            indexC = relativeIndex + 1;
                            uint[] VertIndexesA = F3D.GetIndexes(indexA, indexB, indexC);
                            uint[] VertIndexesB = F3D.GetIndexes(indexA + 3, indexB + 3, indexC + 3);

                            seg7w.Write(F3D.gsSP2Triangles(VertIndexesA, VertIndexesB));


                            for (int f = 0; f < 2; f++)
                            {
                                for (int v = 0; v < 3; v++)
                                {
                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.x));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.z));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(-1 * cObj.modelGeometry[f + faceIndex].VertData[v].position.y));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);



                                    if (CheckST(cObj, textureObject[cObj.materialID]))
                                    {
                                        MessageBox.Show("Fatal UV Error " + cObj.objectName);
                                    }

                                    if ((F3DEX095_Parameters.TextureModes[textureObject[cObj.materialID].SFlag] != 0) || (F3DEX095_Parameters.TextureModes[textureObject[cObj.materialID].TFlag] != 0))
                                    {
                                        cObj.modelGeometry[f + faceIndex].VertData[v].position.s = Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.sPure * textureObject[cObj.materialID].textureWidth);
                                        cObj.modelGeometry[f + faceIndex].VertData[v].position.t = Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.tPure * textureObject[cObj.materialID].textureHeight);
                                    }
                                    else
                                    {
                                        cObj.modelGeometry[f + faceIndex].VertData[v].position.s = Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.sBase * textureObject[cObj.materialID].textureWidth);
                                        cObj.modelGeometry[f + faceIndex].VertData[v].position.t = Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.tBase * textureObject[cObj.materialID].textureHeight);
                                    }


                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.s));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[f + faceIndex].VertData[v].position.t));
                                    Array.Reverse(byteArray);
                                    seg4w.Write(byteArray);



                                    switch (cObj.surfaceProperty)
                                    {
                                        case 1:
                                            {
                                                seg4w.Write(Convert.ToByte(153));
                                                seg4w.Write(Convert.ToByte(0));
                                                seg4w.Write(Convert.ToByte(153));
                                                seg4w.Write(Convert.ToByte(0));
                                                break;
                                            }
                                        case 2:
                                            {
                                                seg4w.Write(Convert.ToByte(0));
                                                seg4w.Write(Convert.ToByte(153));
                                                seg4w.Write(Convert.ToByte(153));
                                                seg4w.Write(Convert.ToByte(0));
                                                break;
                                            }
                                        case 3:
                                            {
                                                seg4w.Write(Convert.ToByte(255));
                                                seg4w.Write(Convert.ToByte(0));
                                                seg4w.Write(Convert.ToByte(0));
                                                seg4w.Write(Convert.ToByte(0));
                                                break;
                                            }
                                        case 4:
                                            {
                                                seg4w.Write(Convert.ToByte(230));
                                                seg4w.Write(Convert.ToByte(204));
                                                seg4w.Write(Convert.ToByte(0));
                                                seg4w.Write(Convert.ToByte(0));
                                                break;
                                            }
                                        case 0:
                                        default:
                                            {
                                                seg4w.Write(cObj.modelGeometry[f + faceIndex].VertData[v].color.R);
                                                seg4w.Write(cObj.modelGeometry[f + faceIndex].VertData[v].color.G);
                                                seg4w.Write(cObj.modelGeometry[f + faceIndex].VertData[v].color.B);
                                                seg4w.Write(cObj.modelGeometry[f + faceIndex].VertData[v].color.A);
                                                break;
                                            }


                                    }
                                }
                            }


                            faceIndex += 2;
                            relativeIndex += 6;

                        }
                        else
                        {

                            if (relativeIndex >= 26)
                            {
                                relativeZero += relativeIndex * 16;
                                relativeIndex = 0;


                                SAddress = Convert.ToUInt32(Segment | relativeZero);
                                seg7w.Write(F3D.gsSPVertex(SAddress, 32, 0));




                            }


                            indexA = relativeIndex;
                            indexB = relativeIndex + 2;
                            indexC = relativeIndex + 1;
                            uint[] VertIndexesA = F3D.GetIndexes(indexA, indexB, indexC);

                            seg7w.Write(F3D.gsSP1Triangle(VertIndexesA));


                            /*
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

                            */
                            //create the vert array for the current face, write it to Segment 4. 




                            for (int v = 0; v < 3; v++)
                            {
                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.x));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.z));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(-1 * cObj.modelGeometry[faceIndex].VertData[v].position.y));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);



                                if (CheckST(cObj, textureObject[cObj.materialID]))
                                {
                                    MessageBox.Show("Fatal UV Error " + cObj.objectName);
                                }

                                if ((F3DEX095_Parameters.TextureModes[textureObject[cObj.materialID].SFlag] != 0) || (F3DEX095_Parameters.TextureModes[textureObject[cObj.materialID].TFlag] != 0))
                                {
                                    cObj.modelGeometry[ faceIndex].VertData[v].position.s = Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.sPure * textureObject[cObj.materialID].textureWidth);
                                    cObj.modelGeometry[faceIndex].VertData[v].position.t = Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.tPure * textureObject[cObj.materialID].textureHeight);
                                }
                                else
                                {
                                    cObj.modelGeometry[faceIndex].VertData[v].position.s = Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.sBase * textureObject[cObj.materialID].textureWidth);
                                    cObj.modelGeometry[faceIndex].VertData[v].position.t = Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.tBase * textureObject[cObj.materialID].textureHeight);
                                }


                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.s));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);

                                byteArray = BitConverter.GetBytes(Convert.ToInt16(cObj.modelGeometry[faceIndex].VertData[v].position.t));
                                Array.Reverse(byteArray);
                                seg4w.Write(byteArray);



                                switch (cObj.surfaceProperty)
                                {
                                    case 1:
                                        {
                                            seg4w.Write(Convert.ToByte(153));
                                            seg4w.Write(Convert.ToByte(0));
                                            seg4w.Write(Convert.ToByte(153));
                                            seg4w.Write(Convert.ToByte(0));
                                            break;
                                        }
                                    case 2:
                                        {
                                            seg4w.Write(Convert.ToByte(0));
                                            seg4w.Write(Convert.ToByte(153));
                                            seg4w.Write(Convert.ToByte(153));
                                            seg4w.Write(Convert.ToByte(0));
                                            break;
                                        }
                                    case 3:
                                        {
                                            seg4w.Write(Convert.ToByte(255));
                                            seg4w.Write(Convert.ToByte(0));
                                            seg4w.Write(Convert.ToByte(0));
                                            seg4w.Write(Convert.ToByte(0));
                                            break;
                                        }
                                    case 4:
                                        {
                                            seg4w.Write(Convert.ToByte(230));
                                            seg4w.Write(Convert.ToByte(204));
                                            seg4w.Write(Convert.ToByte(0));
                                            seg4w.Write(Convert.ToByte(0));
                                            break;
                                        }
                                    case 0:
                                    default:
                                        {
                                            seg4w.Write(cObj.modelGeometry[faceIndex].VertData[v].color.R);
                                            seg4w.Write(cObj.modelGeometry[faceIndex].VertData[v].color.G);
                                            seg4w.Write(cObj.modelGeometry[faceIndex].VertData[v].color.B);
                                            seg4w.Write(cObj.modelGeometry[faceIndex].VertData[v].color.A);
                                            break;
                                        }


                                }
                            }

                            faceIndex++;
                            relativeIndex += 3;

                        }



                    }


                    byteArray = BitConverter.GetBytes(0xB8000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00000000);
                    Array.Reverse(byteArray);
                    seg7w.Write(byteArray);

                    relativeZero += relativeIndex * 16;
                    relativeIndex = 0;
                }
            }

            outseg4 = seg4m.ToArray();
            outseg7 = seg7m.ToArray();

            outMagic = relativeZero;
        }

        public byte[] RGBADemo(OK64Texture TextureObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            F3DEX095 F3D = new F3DEX095();


            binaryWriter.Write(F3D.gsSPTexture(65535, 65535, 0, 0, 1));
            binaryWriter.Write(F3D.gsDPPipeSync());
            binaryWriter.Write(F3D.gsDPSetCombineMode(F3DEX095_Parameters.G_CC_MODULATERGBA, F3DEX095_Parameters.G_CC_MODULATERGBA));
            binaryWriter.Write(F3D.gsDPSetRenderMode(F3DEX095_Parameters.G_RM_AA_ZB_OPA_SURF, F3DEX095_Parameters.G_RM_AA_ZB_OPA_SURF2));
            
            binaryWriter.Write(F3D.gsNinSetupTileDescription(F3DEX095_Parameters.G_IM_FMT_RGBA,
                F3DEX095_Parameters.G_IM_SIZ_16b, Convert.ToUInt32(TextureObject.textureWidth), Convert.ToUInt32(TextureObject.textureHeight), 0, 0, F3DEX095_Parameters.G_TX_NOMIRROR, 5, 0, F3DEX095_Parameters.G_TX_NOMIRROR, 5, 0)
                );
            
            binaryWriter.Write(F3D.gsNinLoadTextureImage(Convert.ToUInt32(TextureObject.segmentPosition), F3DEX095_Parameters.G_IM_FMT_RGBA,
                F3DEX095_Parameters.G_IM_SIZ_16b, Convert.ToUInt32(TextureObject.textureWidth), Convert.ToUInt32(TextureObject.textureHeight), 0, 7)
                );

            return memoryStream.ToArray();
        }

        public int ZSort(OK64Texture TextureObject)
        {
            int[] Check = new int[]
            {
                0,
                0,
                5,
                5,
                2,
                2,
                7,
                7,
                1,
                1,
                6,
                6,
                5,
                5,
                0,
                0,
                3,
                3,
                4,
                4,
                0,
                0,
                0,
                0,
                0,
                0,
                3,
                3,
                0,
                0,
            };
            return Check[TextureObject.RenderModeA];
        }

        public bool XLUCheck(OK64Texture TextureObject)
        {
            bool[] Check = new bool[]
            {
                true,
                true,
                false,
                false,
                true,
                true,
                false,
                false,
                true,
                true,
                false,
                false,
                false,
                false,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
            };
            if (Check[TextureObject.RenderModeA] && Check[TextureObject.RenderModeB])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte[] RGBA(OK64Texture TextureObject, UInt32 Segment, bool GeometryToggle = true)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];
            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));



            if (GeometryToggle)
            {
                //First setup the Geometry Mode parameter
                TextureObject.GeometryModes = 0;
                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    if (TextureObject.GeometryBools[ThisCheck])
                    {
                        TextureObject.GeometryModes |= F3DEX095_Parameters.GeometryModes[ThisCheck];
                    }
                }

                binaryWriter.Write(F3D.gsSPClearGeometryMode(F3DEX095_Parameters.AllGeometryModes));    //clear existing modes
                binaryWriter.Write(F3D.gsSPSetGeometryMode(TextureObject.GeometryModes));               //set the mode we made above.
            }
            




            //set MIP levels to 0.
            binaryWriter.Write(
                F3D.gsSPTexture(
                    65535,
                    65535,
                    0,
                    0,
                    1
                )
            );

            //pipe sync.
            binaryWriter.Write(
                F3D.gsDPPipeSync()
            );



            if (GeometryToggle)
            {
                binaryWriter.Write(
                    F3D.gsDPSetCombineMode(
                        F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                        F3DEX095_Parameters.GCCModes[TextureObject.CombineModeB]
                    )
                );
            }
            /*
            if (!TextureObject.AdvancedSettings)
            {
                //set combine mode (simple)
                binaryWriter.Write(
                    F3D.gsDPSetCombineMode(
                        F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                        F3DEX095_Parameters.GCCModes[TextureObject.CombineModeB]
                    )
                );

            }
            else
            {
                //set combine mode (advanced)
                binaryWriter.Write(
                    F3D.gsDPSetCombineMode(
                        TextureObject.CombineValuesA,
                        TextureObject.CombineValuesB
                    )
                );

            }
            */





            //set render mode
            if (GeometryToggle)
            {
                binaryWriter.Write(
                    F3D.gsDPSetRenderMode(
                        F3DEX095_Parameters.RenderModes[TextureObject.RenderModeA],
                        F3DEX095_Parameters.RenderModes[TextureObject.RenderModeB]
                    )
                );
            }

            //Load Texture Settings
            binaryWriter.Write(
                F3D.gsNinSetupTileDescription(
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                    Convert.ToUInt32(TextureObject.textureWidth),
                    Convert.ToUInt32(TextureObject.textureHeight),
                    0,
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.SFlag],
                    widthex,
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.TFlag],
                    heightex,
                    0
                )
            );





            //Load Texture Data
            binaryWriter.Write(
                F3D.gsNinLoadTextureImage(
                    Convert.ToUInt32(TextureObject.segmentPosition | Convert.ToUInt32(Segment << 24)),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                    Convert.ToUInt32(TextureObject.textureWidth),
                    Convert.ToUInt32(TextureObject.textureHeight),
                    0,
                    7
                )
            );





            binaryWriter.Write(F3D.gsSPEndDisplayList());                                             //End the Display List





            return memoryStream.ToArray();

        }



        public byte[] compileCourseTexture(byte[] SegmentData, OK64Texture[] textureObject, int vertMagic, int SegmentID = 5)
        {
            byte[] byteArray = new byte[0];



            int relativeZero = vertMagic;


            MemoryStream seg7m = new MemoryStream();
            BinaryReader seg7r = new BinaryReader(seg7m);
            BinaryWriter seg7w = new BinaryWriter(seg7m);


            MemoryStream seg4m = new MemoryStream();
            BinaryReader seg4r = new BinaryReader(seg4m);
            BinaryWriter seg4w = new BinaryWriter(seg4m);

            //prewrite existing Segment 7 data, OR, prefix Segment 7 with a 0xB8 Command. 
            if (SegmentData.Length > 0)
            {
                seg7w.Write(SegmentData);
            }


            for (int materialID = 0; materialID < textureObject.Length; materialID++)
            {
                if ((textureObject[materialID].textureName != null) && (textureObject[materialID].textureName != "NULL"))
                {
                    textureObject[materialID].f3dexPosition = new int[2];
                    textureObject[materialID].f3dexPosition[0] = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;
                    seg7w.Write(RGBA(textureObject[materialID], Convert.ToUInt32(SegmentID), true));
                }
            }
            return seg7m.ToArray();
        }



        public byte[] compileTextureObject(byte[] SegmentData, OK64Texture[] textureObject, int vertMagic, int SegmentID = 5, bool GeometryMode = true)
        {
            byte[] byteArray = new byte[0];


            UInt32 ImgSize = 0, ImgType = 0, ImgFlag1 = 0, ImgFlag2 = 0, ImgFlag3 = 0;
            UInt32[] ImgTypes = { 0, 0, 0, 3, 3, 3, 0 }; ///0=RGBA, 3=IA
            UInt32[] STheight = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; ///looks like
            UInt32[] STwidth = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 }; ///texture sizes...
            byte[] heightex = { 5, 5, 6, 5, 5, 6, 5 };
            byte[] widthex = { 5, 6, 5, 5, 6, 5, 5 };

            byte[] SegmentByte = BitConverter.GetBytes(SegmentID);
            Array.Reverse(SegmentByte);

            int relativeZero = vertMagic;
            int relativeIndex = 0;


            MemoryStream seg7m = new MemoryStream();
            BinaryReader seg7r = new BinaryReader(seg7m);
            BinaryWriter seg7w = new BinaryWriter(seg7m);


            MemoryStream seg4m = new MemoryStream();
            BinaryReader seg4r = new BinaryReader(seg4m);
            BinaryWriter seg4w = new BinaryWriter(seg4m);

            //prewrite existing Segment 7 data, OR, prefix Segment 7 with a 0xB8 Command. 
            if (SegmentData.Length > 0)
            {
                seg7w.Write(SegmentData);
            }

            for (int materialID = 0; materialID < textureObject.Length; materialID++)
            {
                if ((textureObject[materialID].textureName != null) && (textureObject[materialID].textureName != "NULL"))
                {
                    textureObject[materialID].f3dexPosition = new int[2];
                    textureObject[materialID].f3dexPosition[0] = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;
                    seg7w.Write(RGBA(textureObject[materialID], Convert.ToUInt32(SegmentID), GeometryMode));
                }                    
            }
            return seg7m.ToArray();
        }

        public byte[] compileObjectList(byte[] OutputData, OK64F3DObject[] courseObject, OK64Texture[] textureObject, int SegmentID)
        {
            //this function will create display lists for each of the section views based on the OK64F3DObject array.
            //this array had been previously written to segment 7 and the offsets to each of those objects' meshes...
            // were stored into courseObject[index].meshPosition[] for this process.


            //magic is the offset of the data preceding this in the segment based on the current organization method,


            byte[] SegmentByte = BitConverter.GetBytes(SegmentID);
            Array.Reverse(SegmentByte);

            MemoryStream seg6m = new MemoryStream();
            BinaryReader seg6r = new BinaryReader(seg6m);
            BinaryWriter seg6w = new BinaryWriter(seg6m);

            seg6w.Write(OutputData);

            //BB00000107C007C0B900031D00552078FCFFFFFFFFFCF87C
            byte[] byteArray = new byte[0];

                
            for (int currentTexture = 0; currentTexture < textureObject.Length; currentTexture++)
            {
                bool textureWritten = false;
                for (int currentObject = 0; currentObject < courseObject.Length; currentObject++)
                {
                    if (courseObject[currentObject].materialID == currentTexture)
                    {
                        if (!textureWritten)
                        {
                            byteArray = BitConverter.GetBytes(0x06000000);
                            Array.Reverse(byteArray);
                            seg6w.Write(byteArray);

                            byteArray = BitConverter.GetBytes(textureObject[currentTexture].f3dexPosition[0] | (SegmentID << 24));
                            Array.Reverse(byteArray);
                            seg6w.Write(byteArray);

                            textureWritten = true;
                        }

                        for (int subObject = 0; subObject < courseObject[currentObject].meshPosition.Length; subObject++)
                        {
                            byteArray = BitConverter.GetBytes(0x06000000);
                            Array.Reverse(byteArray);
                            seg6w.Write(byteArray);

                            byteArray = BitConverter.GetBytes(courseObject[currentObject].meshPosition[subObject] | (SegmentID << 24));
                            Array.Reverse(byteArray);
                            seg6w.Write(byteArray);
                        }
                    }
                }

            }

            byteArray = BitConverter.GetBytes(0xB8000000);
            Array.Reverse(byteArray);
            seg6w.Write(byteArray);

            byteArray = BitConverter.GetBytes(0x00000000);
            Array.Reverse(byteArray);
            seg6w.Write(byteArray);

            return seg6m.ToArray();
            
        }


        public byte[] compileF3DList(ref OK64SectionList[] sectionOut, OK64F3DObject[] courseObject, OK64SectionList[] sectionList, OK64Texture[] textureObject)
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



                    //opaque 
                    bool textureWritten = false;

                    for (int ThisZSort = 0; ThisZSort < 5; ThisZSort++)
                    {
                        for (int currentTexture = 0; currentTexture < textureObject.Length; currentTexture++)
                        {
                            textureWritten = false;
                            if (ThisZSort == ZSort(textureObject[currentTexture]))
                            {


                                for (int currentObject = 0; currentObject < objectCount; currentObject++)
                                {

                                    int objectIndex = sectionList[currentSection].viewList[currentView].objectList[currentObject];
                                    if (courseObject[objectIndex].materialID == currentTexture)
                                    {
                                        if (!textureWritten)
                                        {
                                            byteArray = BitConverter.GetBytes(0x06000000);
                                            Array.Reverse(byteArray);
                                            seg6w.Write(byteArray);

                                            byteArray = BitConverter.GetBytes(textureObject[currentTexture].f3dexPosition[0] | 0x06000000);
                                            Array.Reverse(byteArray);
                                            seg6w.Write(byteArray);

                                            textureWritten = true;
                                        }

                                        for (int subObject = 0; subObject < courseObject[objectIndex].meshPosition.Length; subObject++)
                                        {
                                            byteArray = BitConverter.GetBytes(0x06000000);
                                            Array.Reverse(byteArray);
                                            seg6w.Write(byteArray);

                                            byteArray = BitConverter.GetBytes(courseObject[objectIndex].meshPosition[subObject] | 0x07000000);
                                            Array.Reverse(byteArray);
                                            seg6w.Write(byteArray);
                                        }
                                    }
                                }
                            }

                        }
                        //

                    }

                    byteArray = BitConverter.GetBytes(0xB8000000);
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00000000);
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);
                    sectionOut = sectionList;
                    
                }

            }


            return seg6m.ToArray();
        }



        public byte[] compileXLUList(ref OK64SectionList[] sectionOut, OK64F3DObject[] courseObject, OK64SectionList[] sectionList, OK64Texture[] textureObject)
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



                    //opaque 
                    bool textureWritten = false;

                    for (int ThisZSort = 5; ThisZSort < 8; ThisZSort++)
                    {
                        for (int currentTexture = 0; currentTexture < textureObject.Length; currentTexture++)
                        {
                            textureWritten = false;
                            if (ThisZSort == ZSort(textureObject[currentTexture]))
                            {


                                for (int currentObject = 0; currentObject < objectCount; currentObject++)
                                {

                                    int objectIndex = sectionList[currentSection].viewList[currentView].objectList[currentObject];
                                    if (courseObject[objectIndex].materialID == currentTexture)
                                    {
                                        if (!textureWritten)
                                        {
                                            byteArray = BitConverter.GetBytes(0x06000000);
                                            Array.Reverse(byteArray);
                                            seg6w.Write(byteArray);

                                            byteArray = BitConverter.GetBytes(textureObject[currentTexture].f3dexPosition[0] | 0x06000000);
                                            Array.Reverse(byteArray);
                                            seg6w.Write(byteArray);

                                            textureWritten = true;
                                        }

                                        for (int subObject = 0; subObject < courseObject[objectIndex].meshPosition.Length; subObject++)
                                        {
                                            byteArray = BitConverter.GetBytes(0x06000000);
                                            Array.Reverse(byteArray);
                                            seg6w.Write(byteArray);

                                            byteArray = BitConverter.GetBytes(courseObject[objectIndex].meshPosition[subObject] | 0x07000000);
                                            Array.Reverse(byteArray);
                                            seg6w.Write(byteArray);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    byteArray = BitConverter.GetBytes(0xB8000000);
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00000000);
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);
                    sectionOut = sectionList;

                }

            }


            return seg6m.ToArray();
        }




        public byte[] CompileBattleXLU(OK64F3DObject[] courseObject, OK64Texture[] textureObject)
        {


            MemoryStream seg6m = new MemoryStream();
            BinaryReader seg6r = new BinaryReader(seg6m);
            BinaryWriter seg6w = new BinaryWriter(seg6m);
            byte[] byteArray = new byte[0];
            bool textureWritten = false;
            int objectCount = courseObject.Length;
            for (int ThisZSort = 5; ThisZSort < 8; ThisZSort++)
            {
                for (int currentTexture = 0; currentTexture < textureObject.Length; currentTexture++)
                {
                    textureWritten = false;
                    if (ThisZSort == ZSort(textureObject[currentTexture]))
                    {


                        for (int objectIndex = 0; objectIndex < objectCount; objectIndex++)
                        {
                            if (courseObject[objectIndex].materialID == currentTexture)
                            {
                                if (!textureWritten)
                                {
                                    byteArray = BitConverter.GetBytes(0x06000000);
                                    Array.Reverse(byteArray);
                                    seg6w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(textureObject[currentTexture].f3dexPosition[0] | 0x06000000);
                                    Array.Reverse(byteArray);
                                    seg6w.Write(byteArray);

                                    textureWritten = true;
                                }

                                for (int subObject = 0; subObject < courseObject[objectIndex].meshPosition.Length; subObject++)
                                {
                                    byteArray = BitConverter.GetBytes(0x06000000);
                                    Array.Reverse(byteArray);
                                    seg6w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(courseObject[objectIndex].meshPosition[subObject] | 0x07000000);
                                    Array.Reverse(byteArray);
                                    seg6w.Write(byteArray);
                                }
                            }
                        }
                    }

                }
            }

            byteArray = BitConverter.GetBytes(0xB8000000);
            Array.Reverse(byteArray);
            seg6w.Write(byteArray);

            byteArray = BitConverter.GetBytes(0x00000000);
            Array.Reverse(byteArray);
            seg6w.Write(byteArray);
            return seg6m.ToArray();
        }
        public byte[] CompileBattleList(OK64F3DObject[] courseObject, OK64Texture[] textureObject)
        {


            MemoryStream seg6m = new MemoryStream();
            BinaryReader seg6r = new BinaryReader(seg6m);
            BinaryWriter seg6w = new BinaryWriter(seg6m);
            byte[] byteArray = new byte[0];
            bool textureWritten = false;
            int objectCount = courseObject.Length;
            for (int ThisZSort = 0; ThisZSort < 5; ThisZSort++)
            {
                for (int currentTexture = 0; currentTexture < textureObject.Length; currentTexture++)
                {
                    textureWritten = false;
                    if (ThisZSort == ZSort(textureObject[currentTexture]))
                    {


                        for (int objectIndex = 0; objectIndex < objectCount; objectIndex++)
                        {
                            if (courseObject[objectIndex].materialID == currentTexture)
                            {
                                if (!textureWritten)
                                {
                                    byteArray = BitConverter.GetBytes(0x06000000);
                                    Array.Reverse(byteArray);
                                    seg6w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(textureObject[currentTexture].f3dexPosition[0] | 0x06000000);
                                    Array.Reverse(byteArray);
                                    seg6w.Write(byteArray);

                                    textureWritten = true;
                                }

                                for (int subObject = 0; subObject < courseObject[objectIndex].meshPosition.Length; subObject++)
                                {
                                    byteArray = BitConverter.GetBytes(0x06000000);
                                    Array.Reverse(byteArray);
                                    seg6w.Write(byteArray);

                                    byteArray = BitConverter.GetBytes(courseObject[objectIndex].meshPosition[subObject] | 0x07000000);
                                    Array.Reverse(byteArray);
                                    seg6w.Write(byteArray);
                                }
                            }
                        }
                    }

                }
            }

            byteArray = BitConverter.GetBytes(0xB8000000);
            Array.Reverse(byteArray);
            seg6w.Write(byteArray);

            byteArray = BitConverter.GetBytes(0x00000000);
            Array.Reverse(byteArray);
            seg6w.Write(byteArray);
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

        


        public int GetModelFormat(Scene fbx)
        {
            int modelFormat;
            Assimp.Node masterNode = fbx.RootNode.FindNode("Course Master Objects");
            if (masterNode == null)
            {
                modelFormat = 0;
            }
            else
            {
                Assimp.Node searchNode = fbx.RootNode.FindNode("Section 1 North");
                if (searchNode == null)
                {
                    modelFormat = 1;
                }
                else
                {
                    modelFormat = 2;
                }
            }
            return modelFormat;
        }

        public int GetSectionCount(Scene fbx)
        {
            Assimp.Node pathNode = fbx.RootNode.FindNode("Course Paths");

            int sectionCount = 0;
            for (int searchSection = 1; ; searchSection++)
            {
                Assimp.Node searchNode = fbx.RootNode.FindNode("Section " + searchSection.ToString());
                if (searchNode != null)
                {
                    sectionCount++;
                }
                else
                {
                    break;
                }
            }
            return sectionCount;
        }

        public Matrix4x4 GetTotalTransform(Node Base, Scene FBX)
        {
            Matrix4x4 OutTransform = Base.Transform;
            while (Base.Parent != FBX.RootNode)
            {
                Base = Base.Parent;
                OutTransform *= Base.Transform;
            }
            OutTransform *= FBX.RootNode.Transform;
            return OutTransform;
        }



        public OK64Bone LoadBone(Node Base, Scene FBX)
        {
            OK64Bone NewBone = new OK64Bone();
            NewBone.Name = Base.Name;
            NewBone.Children = new OK64Bone[Base.ChildCount];

            Matrix4x4 OPrime = GetTotalTransform(Base, FBX);
            NewBone.Origin = new short[3];
            NewBone.Origin[0] = Convert.ToInt16(OPrime.A4 * 100);
            NewBone.Origin[1] = Convert.ToInt16(OPrime.C4 * 100);
            NewBone.Origin[2] = Convert.ToInt16(OPrime.B4 * 100);

            //Base.Transform.
            for (int ThisChild = 0; ThisChild < Base.ChildCount; ThisChild++)
            {
                NewBone.Children[ThisChild] = LoadBone(Base.Children[ThisChild], FBX);
            }
            return NewBone;
        }

        public float[] ConvertEuler(Assimp.Quaternion Quat)
        {
            float[] Angle = new float[3];

            // roll (x-axis rotation)
            float sinr_cosp = 2 * (Quat.W * Quat.X + Quat.Y * Quat.Z);
            float cosr_cosp = 1 - 2 * (Quat.X * Quat.X + Quat.Y * Quat.Y);
            Angle[0] = Convert.ToSingle((Math.Atan2(sinr_cosp, cosr_cosp)));

            // pitch (y-axis rotation)
            float sinp = 2 * (Quat.W * Quat.Y - Quat.Z * Quat.X);
            if (Math.Abs(sinp) >= 1)
            {
                if (sinp > 0)
                {
                    Angle[1] = Convert.ToSingle((Math.PI / 2)); // use 90 degrees if out of range
                }
                else if (sinp < 0)
                {
                    Angle[1] = Convert.ToSingle((Math.PI / -2)); // use 90 degrees if out of range
                }
            }
            else
            {
                Angle[1] = Convert.ToSingle((Math.Asin(sinp)));
            }


            // yaw (z-axis rotation)
            float siny_cosp = 2 * (Quat.W * Quat.Z + Quat.X * Quat.Y);
            float cosy_cosp = 1 - 2 * (Quat.Y * Quat.Y + Quat.Z * Quat.Z);
            Angle[2] = Convert.ToSingle((Math.Atan2(siny_cosp, cosy_cosp)));

            

            return Angle;
        }




        public byte[] WriteAnimationModels(OK64Bone Skeleton, TM64_Course.OKObjectType SaveObject, int Magic)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] flip2 = new byte[2];
            List<byte> AnimationData = new List<byte>();

            Skeleton.MeshListOffset = Convert.ToInt32(binaryWriter.BaseStream.Position + Magic);
            int MeshCount = 0;
            for (int ThisObject = 0; ThisObject < SaveObject.ModelData.Length; ThisObject++)
            {
                if (SaveObject.ModelData[ThisObject].BoneName == Skeleton.Name)
                {
                    MeshCount++;
                }
            }

            Skeleton.MeshCount = MeshCount;

            for (int ThisObject = 0; ThisObject < SaveObject.ModelData.Length; ThisObject++)
            {
                if (SaveObject.ModelData[ThisObject].BoneName == Skeleton.Name)
                {
                    flip2 = BitConverter.GetBytes(SaveObject.TextureData[SaveObject.ModelData[ThisObject].materialID].f3dexPosition[0]);
                    Array.Reverse(flip2);
                    binaryWriter.Write(flip2);

                    flip2 = BitConverter.GetBytes(Convert.ToInt32(SaveObject.ModelData[ThisObject].meshPosition.Length));
                    Array.Reverse(flip2);
                    binaryWriter.Write(flip2);

                    flip2 = BitConverter.GetBytes(Convert.ToInt32(SaveObject.ModelData[ThisObject].ListPosition | 0x0A000000));
                    Array.Reverse(flip2);
                    binaryWriter.Write(flip2);
                }
            }

            for (int ThisChild = 0; ThisChild < Skeleton.Children.Length; ThisChild++)
            {
                binaryWriter.Write(WriteAnimationModels(Skeleton.Children[ThisChild], SaveObject, Convert.ToInt32(binaryWriter.BaseStream.Length + Magic)));
            }

            return memoryStream.ToArray();
        }

        public byte[] WriteAnimationData(OK64Bone Skeleton, int Magic)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] flip2 = new byte[2];
            List<byte> AnimationData = new List<byte>();

            flip2 = BitConverter.GetBytes(Skeleton.Origin[0]);
            Array.Reverse(flip2);
            binaryWriter.Write(flip2);

            flip2 = BitConverter.GetBytes(Skeleton.Origin[2]);
            Array.Reverse(flip2);
            binaryWriter.Write(flip2);

            flip2 = BitConverter.GetBytes(Convert.ToInt16(Skeleton.Origin[1] * -1));
            Array.Reverse(flip2);
            binaryWriter.Write(flip2);

            binaryWriter.Write(Convert.ToInt16(0));

            for (int ThisFrame = 0; ThisFrame < Skeleton.Animation.RotationData.Length; ThisFrame++)
            {
                flip2 = BitConverter.GetBytes(Convert.ToInt16(Skeleton.Animation.RotationData[ThisFrame][0]));
                Array.Reverse(flip2);
                binaryWriter.Write(flip2);

                flip2 = BitConverter.GetBytes(Convert.ToInt16(Skeleton.Animation.RotationData[ThisFrame][2]));
                Array.Reverse(flip2);
                binaryWriter.Write(flip2);

                flip2 = BitConverter.GetBytes(Convert.ToInt16(Skeleton.Animation.RotationData[ThisFrame][1]));
                Array.Reverse(flip2);
                binaryWriter.Write(flip2);
            }


            if (Skeleton.FrameCount % 2 == 1)
            {
                binaryWriter.Write(Convert.ToInt16(0));
            }


            for (int ThisFrame = 0; ThisFrame < Skeleton.Animation.TranslationData.Length; ThisFrame++)
            {
                flip2 = BitConverter.GetBytes(Convert.ToInt16(Skeleton.Animation.TranslationData[ThisFrame][0]));
                Array.Reverse(flip2);
                binaryWriter.Write(flip2);

                flip2 = BitConverter.GetBytes(Convert.ToInt16(Skeleton.Animation.TranslationData[ThisFrame][2]));
                Array.Reverse(flip2);
                binaryWriter.Write(flip2);

                flip2 = BitConverter.GetBytes(Convert.ToInt16(Skeleton.Animation.TranslationData[ThisFrame][1] * -1));
                Array.Reverse(flip2);
                binaryWriter.Write(flip2);
            }


            if (Skeleton.FrameCount % 2 == 1)
            {
                binaryWriter.Write(Convert.ToInt16(0));
            }


            for (int ThisFrame = 0; ThisFrame < Skeleton.Animation.ScalingData.Length; ThisFrame++)
            {
                flip2 = BitConverter.GetBytes(Convert.ToInt16(Skeleton.Animation.ScalingData[ThisFrame][0]));
                Array.Reverse(flip2);
                binaryWriter.Write(flip2);

                flip2 = BitConverter.GetBytes(Convert.ToInt16(Skeleton.Animation.ScalingData[ThisFrame][2]));
                Array.Reverse(flip2);
                binaryWriter.Write(flip2);

                flip2 = BitConverter.GetBytes(Convert.ToInt16(Skeleton.Animation.ScalingData[ThisFrame][1]));
                Array.Reverse(flip2);
                binaryWriter.Write(flip2);
            }


            if (Skeleton.FrameCount % 2 == 1)
            {
                binaryWriter.Write(Convert.ToInt16(0));
            }


            foreach (var ChildBone in Skeleton.Children)
            {

                ChildBone.TranslationOffset = Convert.ToInt32(Magic + binaryWriter.BaseStream.Position + Skeleton.TranslationOffset);
                binaryWriter.Write(WriteAnimationData(ChildBone, ChildBone.TranslationOffset));
            }
            return memoryStream.ToArray();
        }

        public byte[] BuildAnimationData(OK64Bone Skeleton, int Magic)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] flip2 = new byte[2];
            List<byte> AnimationData = new List<byte>();

            Skeleton.TranslationOffset = Magic;

            binaryWriter.Write(WriteAnimationData(Skeleton, Magic));

            foreach (var ChildBone in Skeleton.Children)
            {
                ChildBone.TranslationOffset = Convert.ToInt32(Magic + binaryWriter.BaseStream.Position);
                binaryWriter.Write(WriteAnimationData(ChildBone, ChildBone.TranslationOffset));
            }

            return memoryStream.ToArray();
        }

        public byte[] WriteAnimationTableData(OK64Bone Skeleton, TM64_Course.OKObjectType SaveObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            flip2 = BitConverter.GetBytes(Skeleton.TranslationOffset);
            Array.Reverse(flip2);
            binaryWriter.Write(flip2);

            flip2 = BitConverter.GetBytes(Skeleton.MeshCount);
            Array.Reverse(flip2);
            binaryWriter.Write(flip2);

            flip2 = BitConverter.GetBytes(SaveObject.ModelScale);
            Array.Reverse(flip2);
            binaryWriter.Write(flip2);

            flip2 = BitConverter.GetBytes(Skeleton.MeshListOffset);
            Array.Reverse(flip2);
            binaryWriter.Write(flip2);

            flip2 = BitConverter.GetBytes(Skeleton.Children.Length);
            Array.Reverse(flip2);
            binaryWriter.Write(flip2);

            foreach (var ChildBone in Skeleton.Children)
            {
                binaryWriter.Write(WriteAnimationTableData(ChildBone, SaveObject));
            }

            return memoryStream.ToArray();
        }


        public byte[] BuildAnimationTable(OK64Bone Skeleton, TM64_Course.OKObjectType SaveData)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            List<byte> AnimationData = new List<byte>();





            flip2 = BitConverter.GetBytes(Skeleton.FrameCount);
            Array.Reverse(flip2);
            binaryWriter.Write(flip2);

            binaryWriter.Write(WriteAnimationTableData(Skeleton, SaveData));           
            

            return memoryStream.ToArray();
        }

        //
        int hms_joint = 0;
        int hms_begin = 1;
        int hms_end = 2;
        int hms_exit = 3;
        /*
            #define	hmsBegin()						hms_begin,	2,
            #define	hmsEnd()						hms_end,	2,
            #define	hmsExit()						hms_exit,	2,
            #define	hmsJoint(mode,addr,ox,oy,oz)	hms_joint,	7,0,(unsigned long)addr,ox,oy,oz,
        */

        public byte[] HMSBegin()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(hms_begin)));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(2)));
            return memoryStream.ToArray();
        }
        public byte[] HMSEnd()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(hms_end)));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(2)));
            return memoryStream.ToArray();
        }
        public byte[] HMSExit()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(hms_exit)));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(2)));
            return memoryStream.ToArray();
        }
        public byte[] HMSJoint(int Mode, int ModelAddress, short[] Origin)
        {
            //hms_joint,    7,0,(unsigned long)addr,ox,oy,oz,
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(hms_joint)));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(7)));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Mode)));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(0x0A000000 | ModelAddress)));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToInt32(Origin[0]))));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToInt32(Origin[1]))));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToInt32(Origin[2]))));
            return memoryStream.ToArray();
        }

        public byte[] WriteBoneHeader(OK64Bone Skeleton, TM64_Course.OKObjectType SaveObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);


            binaryWriter.Write(HMSBegin());

            int MeshCount = 0;
            for (int ThisObject = 0; ThisObject < SaveObject.ModelData.Length; ThisObject++)
            {
                if (SaveObject.ModelData[ThisObject].BoneName == Skeleton.Name)
                {
                    binaryWriter.Write(HMSJoint(0, Convert.ToInt32(SaveObject.ModelData[ThisObject].ListPosition | 0x0A000000), Skeleton.Origin));
                    MeshCount++;
                }
            }
            if (MeshCount == 0)
            {
                binaryWriter.Write(HMSJoint(0, 0, Skeleton.Origin));
            }

            foreach(var SubChild in Skeleton.Children)
            {
                binaryWriter.Write(WriteBoneHeader(SubChild, SaveObject));
            }

            binaryWriter.Write(HMSEnd());

            return memoryStream.ToArray();
        }

        public int GetCount(OK64Bone Skeleton, int Count)
        {
            Count++;
            foreach (var Child in Skeleton.Children)
            {
                Count = GetCount(Child, Count);
            }
            return Count;
        }
        public byte[] BuildAnimationRecord(OK64Bone Skeleton, int AnimationData, int AnimationTable)
        {

            //          AnimeRecord chomp_anim_anm ={
            //MAP_ANIM_NORMAL,    /* attribute                */
            //0,                    /* syncro                 */
            //0,                    /* start frame              */
            //0,                    /* loop frame              */
            //20,                    /* number of frames          */
            //7,                    /* number of joints          */
            //chomp_anim_prm,        /* anim param pointer      */
            //chomp_anim_tbl        /* anim table pointer      */
            //}

            int TotalChildCount = GetCount(Skeleton, 0);
            
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToInt16(1))));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToInt16(0))));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToInt16(0))));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToInt16(0))));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToInt16(Skeleton.FrameCount))));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToInt16(TotalChildCount))));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(0x0A000000 | AnimationData)));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(0x0A000000 | AnimationTable)));

            return memoryStream.ToArray();
        }



        public OK64Animation LoadAnimation(NodeAnimationChannel AnimeChannel, OK64Bone Bone, int FrameCount)
        {
            OK64Animation NewAnime = new OK64Animation();

            
            
            NewAnime.AnimationName = AnimeChannel.NodeName + "_anime";
            NewAnime.TranslationData = new short[FrameCount][];
            for (int ThisFrame = 0; ThisFrame < FrameCount; ThisFrame++)
            {
                if (ThisFrame < AnimeChannel.PositionKeyCount)
                {
                    NewAnime.TranslationData[ThisFrame] = new short[3];
                    for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                    {
                        NewAnime.TranslationData[ThisFrame][ThisVector] = Convert.ToInt16(AnimeChannel.PositionKeys[ThisFrame].Value[ThisVector] * 100);
                    }
                }
                else
                {
                    NewAnime.TranslationData[ThisFrame] = new short[3];
                    for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                    {
                        NewAnime.TranslationData[ThisFrame][ThisVector] = NewAnime.TranslationData[ThisFrame - 1][ThisVector];
                    }
                }
            }


            NewAnime.RotationData = new short[FrameCount][];
            for (int ThisFrame = 0; ThisFrame < FrameCount; ThisFrame++)
            {

                if (ThisFrame < AnimeChannel.RotationKeyCount)
                {
                    NewAnime.RotationData[ThisFrame] = new short[3];


                    float[] RotationTemp = ConvertEuler(AnimeChannel.RotationKeys[ThisFrame].Value);

                    for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                    {
                        NewAnime.RotationData[ThisFrame][ThisVector] = Convert.ToInt16((RotationTemp[ThisVector] / 0.01745329252) * 0xB6);
                    }
                }
                else
                {
                    NewAnime.RotationData[ThisFrame] = new short[3];
                    for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                    {
                        NewAnime.RotationData[ThisFrame][ThisVector] = NewAnime.RotationData[ThisFrame - 1][ThisVector];
                    }
                }

                
            }


            NewAnime.ScalingData = new short[FrameCount][];
            for (int ThisFrame = 0; ThisFrame < FrameCount; ThisFrame++)
            {
                if (ThisFrame < AnimeChannel.ScalingKeyCount)
                {
                    NewAnime.ScalingData[ThisFrame] = new short[3];
                    for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                    {
                        NewAnime.ScalingData[ThisFrame][ThisVector] = Convert.ToInt16(AnimeChannel.ScalingKeys[ThisFrame].Value[ThisVector] * 100);
                    }
                }
                else
                {
                    NewAnime.ScalingData[ThisFrame] = new short[3];
                    for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                    {
                        NewAnime.ScalingData[ThisFrame][ThisVector] = NewAnime.ScalingData[ThisFrame -1][ThisVector];
                    }
                }

            }


            return NewAnime;
        }


        public Point3D RotatePoint(Point3D Point, float[] ObjectAngles)
        {
            var id = Matrix3D.Identity;
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(1, 0, 0), ObjectAngles[0]));
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(0, 1, 0), ObjectAngles[1]));
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(0, 0, 1), ObjectAngles[2]));
            return id.Transform(Point);
        }

        public OK64Bone TransformBone(OK64Bone Bone, OK64Bone Parent, int FrameCount)
        {
            
            for (int ThisFrame = 0; ThisFrame < FrameCount; ThisFrame++)
            {
                Point3D Root = new Point3D() { X = Bone.Origin[0] - Parent.Origin[0], Y = Bone.Origin[1] - Parent.Origin[1], Z = Bone.Origin[2] - Parent.Origin[2] };
                float[] Angle = new float[3]{
                    Convert.ToSingle(Parent.Animation.RotationData[ThisFrame][0]),
                    Convert.ToSingle(Parent.Animation.RotationData[ThisFrame][1]),
                    Convert.ToSingle(Parent.Animation.RotationData[ThisFrame][2]),
                };
                Point3D Branch = RotatePoint(Root, Angle);
                
                Bone.Animation.TranslationData[ThisFrame][0] += Convert.ToInt16((Branch.X - Root.X) + Parent.Animation.TranslationData[ThisFrame][0]);
                Bone.Animation.TranslationData[ThisFrame][1] += Convert.ToInt16((Branch.Y - Root.Y) + Parent.Animation.TranslationData[ThisFrame][1]);
                Bone.Animation.TranslationData[ThisFrame][2] += Convert.ToInt16((Branch.Z - Root.Z) + Parent.Animation.TranslationData[ThisFrame][2]);

            }


            Bone.Origin[0] += Parent.Origin[0];
            Bone.Origin[1] += Parent.Origin[1];
            Bone.Origin[2] += Parent.Origin[2];
            return Bone;
        }
        public OK64Bone GetTransforms(OK64Bone Skeleton, int FrameCount)
        {
            foreach (var Child in Skeleton.Children)
            {
                TransformBone(Child, Skeleton, FrameCount);
            }

            foreach (var Child in Skeleton.Children)
            {
                GetTransforms(Child, FrameCount);
            }
            return Skeleton;
        }

        public OK64Bone ParseAnimation(Scene FBX, NodeAnimationChannel AnimeChannel, OK64Bone Bone, int FrameCount)
        {

            if (Bone.Name == AnimeChannel.NodeName)
            {
                Bone.Animation = LoadAnimation(AnimeChannel, Bone, FrameCount);

            }
            foreach (var Child in Bone.Children)
            {
                ParseAnimation(FBX, AnimeChannel, Child, FrameCount);
            }
            return Bone;
        }
        public OK64Bone LoadSkeleton (Scene FBX)
        {
            
            Node Base = FBX.RootNode.FindNode("BodyBone");
            OK64Bone Skeleton = LoadBone(Base, FBX);

            Animation Anime = FBX.Animations[0];
            Skeleton.FrameCount = Convert.ToInt32(Anime.DurationInTicks);
            for (int ThisNode = 0; ThisNode < Anime.NodeAnimationChannelCount; ThisNode++)
            {
                ParseAnimation(FBX, Anime.NodeAnimationChannels[ThisNode], Skeleton, Skeleton.FrameCount);
            }
            GetTransforms(Skeleton, Skeleton.FrameCount);
            return Skeleton;
        }
        
        


    }

    

}

// phew! We made it. I keep saying we, but it's me doing all the work!
// maybe try pitching in sometime and updating the program! I'd love the help!

// Thank you so much for-a reading my source!

// OverKart 64 Library
// For Mario Kart 64 1.0 USA ROM
// <3 Hamp
          
