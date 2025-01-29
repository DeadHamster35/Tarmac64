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
using System.Windows;
using System.Xml;
using Tarmac64_Library;
using SharpDX;

using F3DSharp;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Collections.Concurrent;
using System.Windows.Media.Imaging;
using System.Data;
using System.Security.Policy;
using SharpGL.SceneGraph;
using static Tarmac64_Library.TM64_Geometry;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Collections.Specialized.BitVector32;
using SharpGL.SceneGraph.Quadrics;
using static System.Windows.Forms.AxHost;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

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
            public Face(XmlDocument XMLDoc, string Parent, int FaceNumber)
            {
                TM64 Tarmac = new TM64();
                XmlNode Owner = XMLDoc.SelectSingleNode(Parent);
                string TargetPath = Parent + "/Face_" + FaceNumber.ToString();


                string VertPath = TargetPath + "/VertArray";
                VertData = new Vertex[3];
                for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                {
                    VertData[ThisVert] = new Vertex(XMLDoc, VertPath, ThisVert);
                }

            }
            public void SaveXML(XmlDocument XMLDoc, XmlElement Parent, int FaceNumber)
            {
                TM64 Tarmac = new TM64();
                XmlElement FaceXML = XMLDoc.CreateElement("Face_" + FaceNumber.ToString());
                Parent.AppendChild(FaceXML);


                XmlElement VertArrayXML = XMLDoc.CreateElement("VertArray");
                FaceXML.AppendChild(VertArrayXML);
                for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                {
                    VertData[ThisVert].SaveXML(XMLDoc, VertArrayXML, ThisVert);
                }

            }
            public Face()
            {

            }

            public Vertex[] VertData { get; set; }
        }



        public class OK64SectionList
        {
            public OK64SectionList(XmlDocument XMLDoc, string Parent, int SectionID)
            {
                TM64 Tarmac = new TM64();
                XmlNode Owner = XMLDoc.SelectSingleNode(Parent);
                string TargetPath = Parent + "/Section_" + SectionID.ToString();
                int Count = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, TargetPath, "ObjectCount", "0"));

                objectList = new int[Count];
                segmentPosition = 0;

                for (int This = 0; This < Count; This++)
                {
                    string HeaderName = TargetPath + "/ObjectList";
                    objectList[This] = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, HeaderName, "Object_" + This.ToString(), "0"));
                }


            }
            public void SaveXML(XmlDocument XMLDoc, XmlElement Parent, int SectionID)
            {
                TM64 Tarmac = new TM64();
                XmlElement SectionXML = XMLDoc.CreateElement("Section_"+SectionID.ToString());
                Parent.AppendChild(SectionXML);

                Tarmac.GenerateElement(XMLDoc, SectionXML, "ObjectCount", objectList.Length);
                XmlElement ObjectListXML = XMLDoc.CreateElement("ObjectList");
                SectionXML.AppendChild(ObjectListXML);
                for (int ThisCount = 0; ThisCount < objectList.Length; ThisCount++)
                {
                    Tarmac.GenerateElement(XMLDoc, ObjectListXML, "Object_"+ThisCount.ToString(), objectList[ThisCount]);
                }

            }
            public OK64SectionList()
            {

            }


            public int[] objectList { get; set; }
            public int segmentPosition { get; set; }
        }


        public class OK64TextureRaw
        {
            public Image textureBitmap { get; set; }

            public byte[] compressedTexture { get; set; }
            public byte[] PaletteData { get; set; }
            public byte[] TextureData { get; set; }
            public int compressedSize { get; set; }
            public int fileSize { get; set; }
            public int segmentPosition { get; set; }
            public int palettePosition { get; set; }
            public int paletteSize { get; set; }
            public int romPosition { get; set; }
            public int f3dexPosition { get; set; }

        }
        public class OK64Texture
        {
            public OK64Texture()
            {
                RawTexture = new OK64TextureRaw();
            }
            public OK64Texture(XmlDocument XMLDoc, string Parent, int ChildIndex)
            {
                TM64 Tarmac = new TM64();
                XmlNode Owner = XMLDoc.SelectSingleNode(Parent);
                XmlNode Target = Owner.ChildNodes[ChildIndex];
                
                textureName = XmlConvert.DecodeName(Target.Name);
                string HeaderName = "Texture_" + ChildIndex.ToString();
                textureName = Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "textureName");
                texturePath = Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "texturePath");
                alphaPath = Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "alphaPath");

                CombineModeA = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "CombineModeA","0"));
                CombineModeB = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "CombineModeB", "0"));

                RenderModeA = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "RenderModeA", "0"));
                RenderModeB = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "RenderModeB", "0"));

                GeometryModes = Convert.ToUInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "GeometryModes", "0"));
                GeometryBools = new bool[12];
                BitSize = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "BitSize", "0"));

                TextureFilter = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "TextureFilter", "0"));
                TextureFormat = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "TextureFormat", "0"));
                TextureOverWrite = new int[0];

                SFlag = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "SFlag", "0"));
                TFlag = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "TFlag", "0"));

                textureScrollS = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "textureScrollS", "0"));
                textureScrollT = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "textureScrollT", "0"));

                textureScreen = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "textureScreen", "0"));

                GLShiftS = Convert.ToDouble(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "GLShiftS", "0"));
                GLShiftT = Convert.ToDouble(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "GLShiftT", "0"));

                RawTexture = new OK64TextureRaw();
                if (File.Exists(texturePath))
                {
                    using (var fs = new FileStream(texturePath, FileMode.Open, FileAccess.Read))
                    {
                        Image Raw = Image.FromStream(fs);
                        RawTexture.textureBitmap = Raw;
                        textureWidth = Raw.Width;
                        textureHeight = Raw.Height;
                        fs.Close();
                    }
                }
            }
            
            public void SaveXML(XmlDocument XMLDoc, XmlElement Parent, int TextureID)
            {
                TM64 Tarmac = new TM64();
                XmlElement TextureXML = XMLDoc.CreateElement("Texture_"+TextureID.ToString());
                Parent.AppendChild(TextureXML);

                Tarmac.GenerateElement(XMLDoc, TextureXML, "textureName", textureName);

                if (texturePath != null)
                {
                    Tarmac.GenerateElement(XMLDoc, TextureXML, "texturePath", texturePath);
                }
                else
                {
                    Tarmac.GenerateElement(XMLDoc, TextureXML, "texturePath", "NULL");
                }

                Tarmac.GenerateElement(XMLDoc, TextureXML, "alphaPath", alphaPath);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "CombineModeA", CombineModeA);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "CombineModeB", CombineModeB);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "RenderModeA", RenderModeA);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "RenderModeB", RenderModeB);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "GeometryModes", GeometryModes);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "BitSize", BitSize);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "TextureFilter", TextureFilter);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "TextureFormat", TextureFormat);

                Tarmac.GenerateElement(XMLDoc, TextureXML, "SFlag", SFlag);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "TFlag", TFlag);

                Tarmac.GenerateElement(XMLDoc, TextureXML, "textureScrollS", textureScrollS);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "textureScrollT", textureScrollT);

                Tarmac.GenerateElement(XMLDoc, TextureXML, "textureScreen", textureScreen);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "GLShiftS", GLShiftS);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "GLShiftT", GLShiftT);

            }

            public OK64TextureRaw RawTexture { get; set; }

            public string textureName { get; set; }
            public string texturePath { get; set; }
            public string alphaPath { get; set; }
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
            public int TextureFilter { get; set; }
            public int TextureFormat { get; set; }
            public int SFlag { get; set; }
            public int TFlag { get; set; }
            public int textureScrollS { get; set; }
            public int textureScrollT { get; set; }
            public int textureScreen { get; set; }
            public double GLShiftS { get; set; }
            public double GLShiftT { get; set; }
            public int[] TextureOverWrite { get; set; }
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
            public UInt32 TranslationOffset { get; set; }
            public UInt32 MeshListOffset { get; set; }

        }


        public class OK64Animation
        {
            public string AnimationName { get; set; }
            public short[][] TranslationData { get; set; }
            public short[][] RotationData { get; set; }
            public short[][] ScalingData { get; set; }

            public float[][] RotationFloat { get; set; }
        }

        public class OK64F3DModel
        {
            public List<Vertex> VertexCache { get; set; }
            public List<int[]> Indexes { get; set; }
            public List<int> IndexKey { get; set; }
            
        }
            

        public class OK64F3DObject
        {
            public OK64F3DObject(XmlDocument XMLDoc, string Parent, int MasterNumber)
            {
                TM64 Tarmac = new TM64();
                XmlNode Owner = XMLDoc.SelectSingleNode(Parent);
                string TargetPath = Parent + "/Object_" + MasterNumber.ToString();

                objectName = Tarmac.LoadElement(XMLDoc, TargetPath, "objectName", "");
                vertCount = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, TargetPath, "vertCount", "0"));
                faceCount = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, TargetPath, "faceCount", "0"));
                materialID = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, TargetPath, "materialID", "0"));

                surfaceID = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, TargetPath, "surfaceID", "0"));
                surfaceMaterial = Convert.ToByte(Tarmac.LoadElement(XMLDoc, TargetPath, "surfaceMaterial", "0"));
                surfaceProperty = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, TargetPath, "surfaceProperty", "0"));
                BoneName = Tarmac.LoadElement(XMLDoc, TargetPath, "BoneName", "NULL");


                Random RNG = new Random();
                objectColor = new float[3]
                {
                    RNG.NextFloat(0,1),
                    RNG.NextFloat(0,1),
                    RNG.NextFloat(0,1)
                };

                KillDisplayList = new bool[8];
                int[] KDL = Tarmac.LoadElements(XMLDoc, TargetPath, "KillDisplayList", "0");
                for (int ThisBool = 0; ThisBool < 8; ThisBool++)
                {
                    KillDisplayList[ThisBool] = Convert.ToBoolean(KDL[ThisBool]);
                }
                
                


                int geometryCount = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, TargetPath, "GeometryCount", "0"));
                
                string GeometryHeader = TargetPath + "/GeometryArray";

                TM64_Geometry TMGeo = new TM64_Geometry();
                modelGeometry = TMGeo.LoadFaceArrayXML(XMLDoc, GeometryHeader);

            }


            public void SaveXML(XmlDocument XMLDoc, XmlElement Parent, int MasterNumber)
            {
                TM64 Tarmac = new TM64();
                XmlElement ObjectXML = XMLDoc.CreateElement(XmlConvert.EncodeName("Object_"+MasterNumber.ToString()));
                Parent.AppendChild(ObjectXML);

                Tarmac.GenerateElement(XMLDoc, ObjectXML, "objectName", objectName);
                Tarmac.GenerateElement(XMLDoc, ObjectXML, "vertCount", vertCount);
                Tarmac.GenerateElement(XMLDoc, ObjectXML, "faceCount", faceCount);
                Tarmac.GenerateElement(XMLDoc, ObjectXML, "materialID", materialID);

                Tarmac.GenerateElement(XMLDoc, ObjectXML, "surfaceID", surfaceID);
                Tarmac.GenerateElement(XMLDoc, ObjectXML, "vertCount", surfaceMaterial);
                Tarmac.GenerateElement(XMLDoc, ObjectXML, "surfaceProperty", surfaceProperty);
                Tarmac.GenerateElement(XMLDoc, ObjectXML, "BoneName", BoneName);
                Tarmac.GenerateElement(XMLDoc, ObjectXML, "KillDisplayList", KillDisplayList);
                


                Tarmac.GenerateElement(XMLDoc, ObjectXML, "GeometryCount", modelGeometry.Length);
                XmlElement ModelXML = XMLDoc.CreateElement("GeometryArray");
                ObjectXML.AppendChild(ModelXML);
                TM64_Geometry TMGeo = new TM64_Geometry();
                TMGeo.SaveFaceArrayXML(modelGeometry, XMLDoc, ModelXML);
                

                pathfindingObject.SaveXML(XMLDoc, ObjectXML);

            }

            public OK64F3DObject()
            {

            }


            //pre-compilation data
            public string objectName { get; set; }
            public int vertCount { get; set; }
            public int faceCount { get; set; }
            public int materialID { get; set; }
            public int surfaceID { get; set; }
            public Byte surfaceMaterial { get; set; }
            public Face[] modelGeometry { get; set; }
            public float[] objectColor { get; set; }
            public int surfaceProperty { get; set; }
            public PathfindingObject pathfindingObject { get; set; }
            public string BoneName { get; set; }
            public bool[] KillDisplayList { get; set; }
            public bool WaveObject { get; set; }

            //post-compilation data

            public int[] meshPosition { get; set; }
            public int VertCachePosition { get; set; }
            public int ListPosition { get; set; }

        }
        public class PathfindingObject
        {
            public void SaveXML(XmlDocument XMLDoc, XmlElement Parent)
            {
                TM64 Tarmac = new TM64();
                XmlElement PathXML = XMLDoc.CreateElement("Pathfinder");
                Parent.AppendChild(PathXML);

                Tarmac.GenerateElement(XMLDoc, PathXML, "highX", highX);
                Tarmac.GenerateElement(XMLDoc, PathXML, "highY", highY);
                Tarmac.GenerateElement(XMLDoc, PathXML, "highZ", highZ);

                Tarmac.GenerateElement(XMLDoc, PathXML, "lowX", lowX);
                Tarmac.GenerateElement(XMLDoc, PathXML, "lowY", lowY);
                Tarmac.GenerateElement(XMLDoc, PathXML, "lowZ", lowZ);


            }
            public PathfindingObject()
            {

            }
            public float highX { get; set; }
            public float highY { get; set; }
            public float highZ { get; set; }
            public float lowX { get; set; }
            public float lowY { get; set; }
            public float lowZ { get; set; }


        }

        public class Vertex
        {
            public void SaveXML(XmlDocument XMLDoc, XmlElement Parent, int ThisVert)
            {
                TM64 Tarmac = new TM64();

                XmlElement VertXML = XMLDoc.CreateElement("Vertex_" + ThisVert.ToString());
                Parent.AppendChild(VertXML);
                position.SaveXML(XMLDoc, VertXML);
                color.SaveXML(XMLDoc, VertXML);               
                
            }

            public Vertex(XmlDocument XMLDoc, string Parent, int ThisVertex)
            {
                TM64 Tarmac = new TM64();
                XmlNode Owner = XMLDoc.SelectSingleNode(Parent);
                

                string TargetPath = Parent + "/Vertex_" + ThisVertex.ToString();
                position = new Position(XMLDoc, TargetPath);
                color = new OK64Color(XMLDoc, TargetPath);
                
            }

            public Vertex()
            {

            }
            public Position position { get; set; }
            public OK64Color color { get; set; }
            
        }

        public class OK64Color
        {
            public OK64Color(XmlDocument XMLDoc, string Parent)
            {
                TM64 Tarmac = new TM64();
                string Target = Parent + "/Color";
                int[] RGBA = Tarmac.LoadElements(XMLDoc, Target, "RGBA", "0");
                R = Convert.ToByte(RGBA[0]);
                G = Convert.ToByte(RGBA[1]);
                B = Convert.ToByte(RGBA[2]);
                A = Convert.ToByte(RGBA[3]);

                RFloat = Convert.ToSingle(255.0f / R);
                GFloat = Convert.ToSingle(255.0f / G);
                BFloat = Convert.ToSingle(255.0f / B);
                AFloat = Convert.ToSingle(255.0f / A);
            }
            public void SaveXML(XmlDocument XMLDoc, XmlElement Parent)
            {
                TM64 Tarmac = new TM64();
                XmlElement ColorXML = XMLDoc.CreateElement("Color");
                Parent.AppendChild(ColorXML);
                int[] RGBA = new int[] { R,G,B,A };

                Tarmac.GenerateElement(XMLDoc, ColorXML, "RGBA", RGBA);
            }
            public OK64Color()
            {

            }

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

            public void SaveXML(XmlDocument XMLDoc, XmlElement Parent)
            {
                TM64 Tarmac = new TM64();
                XmlElement PosXML = XMLDoc.CreateElement("Position");
                Parent.AppendChild(PosXML);


                int[] Pos = new int[3]{ x,y,z};

                Tarmac.GenerateElement(XMLDoc, PosXML, "xyz", Pos);

                float[] UV = new float[] { u, v };
                Tarmac.GenerateElement(XMLDoc, PosXML, "uv", UV);
                float[] ST = new float[] { sPure, tPure };
                Tarmac.GenerateElement(XMLDoc, PosXML, "st", ST);

            }
            public Position(XmlDocument XMLDoc, string Parent)
            {
                TM64 Tarmac = new TM64();
                string Target = Parent + "/Position";
                int[] Pos = Tarmac.LoadElements(XMLDoc, Target, "xyz", "0");

                x = Convert.ToInt16(Pos[0]);
                y = Convert.ToInt16(Pos[1]);
                z = Convert.ToInt16(Pos[2]);


                float[] UV = Tarmac.LoadElementsF(XMLDoc, Target, "uv", "0");

                u = Convert.ToSingle(UV[0]);
                v = Convert.ToSingle(UV[1]);


                float[] ST = Tarmac.LoadElementsF(XMLDoc, Target, "st", "0");

                sPure = Convert.ToSingle(ST[0]);
                tPure = Convert.ToSingle(ST[1]);
                sBase = u * 32;
                tBase = (1 - v) * 32;

            }
            //
            public Position()
            {

            }
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


        public Vertex[] StandardVertex(float Size)
        {
            Vertex[] Standard = new Vertex[8];

            for (int ThisVert = 0; ThisVert < 8; ThisVert++)
            {
                Standard[ThisVert] = new Vertex();
                Standard[ThisVert].position = new Position();
            }

            //
            //Poly 1
            Standard[0].position.x = Convert.ToInt16(Size * -1);
            Standard[0].position.y = Convert.ToInt16(0);
            Standard[0].position.z = Convert.ToInt16(0);

            Standard[1].position.x = Convert.ToInt16(Size);
            Standard[1].position.y = Convert.ToInt16(0);
            Standard[1].position.z = Convert.ToInt16(0);

            Standard[2].position.x = Convert.ToInt16(Size);
            Standard[2].position.y = Convert.ToInt16(0);
            Standard[2].position.z = Convert.ToInt16(Size * 2);

            Standard[3].position.x = Convert.ToInt16(Size * -1);
            Standard[3].position.y = Convert.ToInt16(0);
            Standard[3].position.z = Convert.ToInt16(Size * 2);

            //
            //Poly 2

            Standard[4].position.x = Convert.ToInt16(0);
            Standard[4].position.y = Convert.ToInt16(Size * -1);
            Standard[4].position.z = Convert.ToInt16(0);

            Standard[5].position.x = Convert.ToInt16(0);
            Standard[5].position.y = Convert.ToInt16(Size);
            Standard[5].position.z = Convert.ToInt16(0);

            Standard[6].position.x = Convert.ToInt16(0);
            Standard[6].position.y = Convert.ToInt16(Size);
            Standard[6].position.z = Convert.ToInt16(Size * 2);

            Standard[7].position.x = Convert.ToInt16(0);
            Standard[7].position.y = Convert.ToInt16(Size * -1);
            Standard[7].position.z = Convert.ToInt16(Size * 2);

            return Standard;

        }




        public TM64_Geometry.Face[] CreateStandard(float Size = 5.0f, bool DoubleSided = true)
        {
            int CurrentIndex = 0;
            int MaxIndex = 4;
            if (DoubleSided)
            {
                MaxIndex = 8;
            }
            Vertex[] SourceArray = StandardVertex(Size);

            TM64_Geometry.Face[] StandardGeometry = new TM64_Geometry.Face[MaxIndex];
            

            //Poly  1
            //Face 1
            StandardGeometry[CurrentIndex] = new TM64_Geometry.Face();
            StandardGeometry[CurrentIndex].VertData = new TM64_Geometry.Vertex[3];
            StandardGeometry[CurrentIndex].VertData[0] = SourceArray[0];
            StandardGeometry[CurrentIndex].VertData[1] = SourceArray[1];
            StandardGeometry[CurrentIndex].VertData[2] = SourceArray[2];

            CurrentIndex++;

            if (DoubleSided)
            {
                //Backface 1
                StandardGeometry[CurrentIndex] = new TM64_Geometry.Face();
                StandardGeometry[CurrentIndex].VertData = new TM64_Geometry.Vertex[3];
                StandardGeometry[CurrentIndex].VertData[0] = SourceArray[0];
                StandardGeometry[CurrentIndex].VertData[1] = SourceArray[2];
                StandardGeometry[CurrentIndex].VertData[2] = SourceArray[1];
                CurrentIndex++;
            }

            //Face 2
            StandardGeometry[CurrentIndex] = new TM64_Geometry.Face();
            StandardGeometry[CurrentIndex].VertData = new TM64_Geometry.Vertex[3];
            StandardGeometry[CurrentIndex].VertData[0] = SourceArray[2];
            StandardGeometry[CurrentIndex].VertData[1] = SourceArray[3];
            StandardGeometry[CurrentIndex].VertData[2] = SourceArray[0];
            CurrentIndex++;

            if (DoubleSided)
            {
                //Backface 2
                StandardGeometry[CurrentIndex] = new TM64_Geometry.Face();
                StandardGeometry[CurrentIndex].VertData = new TM64_Geometry.Vertex[3];
                StandardGeometry[CurrentIndex].VertData[0] = SourceArray[2];
                StandardGeometry[CurrentIndex].VertData[1] = SourceArray[0];
                StandardGeometry[CurrentIndex].VertData[2] = SourceArray[3];
                CurrentIndex++;

            }

            //Poly  2
            //Face 1
            StandardGeometry[CurrentIndex] = new TM64_Geometry.Face();
            StandardGeometry[CurrentIndex].VertData = new TM64_Geometry.Vertex[3];
            StandardGeometry[CurrentIndex].VertData[0] = SourceArray[4];
            StandardGeometry[CurrentIndex].VertData[1] = SourceArray[5];
            StandardGeometry[CurrentIndex].VertData[2] = SourceArray[6];

            CurrentIndex++;

            if (DoubleSided)
            {
                //Backface 1
                StandardGeometry[CurrentIndex] = new TM64_Geometry.Face();
                StandardGeometry[CurrentIndex].VertData = new TM64_Geometry.Vertex[3];
                StandardGeometry[CurrentIndex].VertData[0] = SourceArray[4];
                StandardGeometry[CurrentIndex].VertData[1] = SourceArray[6];
                StandardGeometry[CurrentIndex].VertData[2] = SourceArray[5];
                CurrentIndex++;
            }

            //Face 2
            StandardGeometry[CurrentIndex] = new TM64_Geometry.Face();
            StandardGeometry[CurrentIndex].VertData = new TM64_Geometry.Vertex[3];
            StandardGeometry[CurrentIndex].VertData[0] = SourceArray[6];
            StandardGeometry[CurrentIndex].VertData[1] = SourceArray[7];
            StandardGeometry[CurrentIndex].VertData[2] = SourceArray[4];
            CurrentIndex++;

            if (DoubleSided)
            {
                //Backface 2
                StandardGeometry[CurrentIndex] = new TM64_Geometry.Face();
                StandardGeometry[CurrentIndex].VertData = new TM64_Geometry.Vertex[3];
                StandardGeometry[CurrentIndex].VertData[0] = SourceArray[6];
                StandardGeometry[CurrentIndex].VertData[1] = SourceArray[4];
                StandardGeometry[CurrentIndex].VertData[2] = SourceArray[7];
                CurrentIndex++;

            }

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


        public Face[] LoadFaceArrayXML(XmlDocument XMLDoc, string Parent)
        {
            TM64 Tarmac = new TM64();
            XmlNode Owner = XMLDoc.SelectSingleNode(Parent);
            Face[] FaceArray = new Face[0];

            List<int> Elements = new List<int>();
            XmlNode CheckNode = XMLDoc.SelectSingleNode("/" + Parent + "/" + "VertPos");
            if (CheckNode != null)
            {
                string Check = XmlConvert.DecodeName(CheckNode.InnerText);
                string[] Collects = Check.Split(';');


                FaceArray = new Face[(Collects.Length / 3)];
                
                for (int ThisFace = 0; ThisFace < FaceArray.Length; ThisFace++)
                {
                    FaceArray[ThisFace] = new Face();
                    FaceArray[ThisFace].VertData = new Vertex[3];
                    for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                    {
                        FaceArray[ThisFace].VertData[ThisVert] = new Vertex();
                        string[] Items = Collects[(ThisFace * 3) + ThisVert].Split(',');
                        
                        FaceArray[ThisFace].VertData[ThisVert].position = new Position();
                        FaceArray[ThisFace].VertData[ThisVert].position.x = Convert.ToInt16(Items[0]);
                        FaceArray[ThisFace].VertData[ThisVert].position.y = Convert.ToInt16(Items[1]);
                        FaceArray[ThisFace].VertData[ThisVert].position.z = Convert.ToInt16(Items[2]);
                    }
                }
            }

            CheckNode = XMLDoc.SelectSingleNode("/" + Parent + "/" + "VertColor");
            if (CheckNode != null)
            {
                string Check = XmlConvert.DecodeName(CheckNode.InnerText);
                string[] Collects = Check.Split(';');


                for (int ThisFace = 0; ThisFace < FaceArray.Length; ThisFace++)
                {
                    for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                    {
                        string[] Items = Collects[(ThisFace * 3) + ThisVert].Split(',');

                        FaceArray[ThisFace].VertData[ThisVert].color = new OK64Color();
                        FaceArray[ThisFace].VertData[ThisVert].color.R = Convert.ToByte(Items[0]);
                        FaceArray[ThisFace].VertData[ThisVert].color.G = Convert.ToByte(Items[1]);
                        FaceArray[ThisFace].VertData[ThisVert].color.B = Convert.ToByte(Items[2]);
                        FaceArray[ThisFace].VertData[ThisVert].color.A = Convert.ToByte(Items[3]);

                        FaceArray[ThisFace].VertData[ThisVert].color.RFloat = Convert.ToSingle(FaceArray[ThisFace].VertData[ThisVert].color.R / 255.0f);
                        FaceArray[ThisFace].VertData[ThisVert].color.GFloat = Convert.ToSingle(FaceArray[ThisFace].VertData[ThisVert].color.G / 255.0f);
                        FaceArray[ThisFace].VertData[ThisVert].color.BFloat = Convert.ToSingle(FaceArray[ThisFace].VertData[ThisVert].color.B / 255.0f);
                        FaceArray[ThisFace].VertData[ThisVert].color.AFloat = Convert.ToSingle(FaceArray[ThisFace].VertData[ThisVert].color.A / 255.0f);
                    }
                }
            }

            CheckNode = XMLDoc.SelectSingleNode("/" + Parent + "/" + "VertUVs");
            if (CheckNode != null)
            {
                string Check = XmlConvert.DecodeName(CheckNode.InnerText);
                string[] Collects = Check.Split(';');


                for (int ThisFace = 0; ThisFace < FaceArray.Length; ThisFace++)
                {
                    for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                    {
                        string[] Items = Collects[(ThisFace * 3) + ThisVert].Split(',');

                        
                        FaceArray[ThisFace].VertData[ThisVert].position.u = Convert.ToSingle(Items[0]);
                        FaceArray[ThisFace].VertData[ThisVert].position.v = Convert.ToSingle(Items[1]);
                        FaceArray[ThisFace].VertData[ThisVert].position.sPure = Convert.ToSingle(Items[2]);
                        FaceArray[ThisFace].VertData[ThisVert].position.tPure = Convert.ToSingle(Items[3]);
                        FaceArray[ThisFace].VertData[ThisVert].position.sBase = FaceArray[ThisFace].VertData[ThisVert].position.u * 32;
                        FaceArray[ThisFace].VertData[ThisVert].position.tBase = FaceArray[ThisFace].VertData[ThisVert].position.v * 32;
                    }
                }
            }

            return FaceArray;
        }
        public void SaveFaceArrayXML(Face[] FaceArray, XmlDocument XMLDoc, XmlElement Parent)
        {
            TM64 Tarmac = new TM64();
            string VertString = "";
            for (int ThisFace = 0; ThisFace < FaceArray.Length; ThisFace++)
            {
                for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                {
                    VertString += FaceArray[ThisFace].VertData[ThisVert].position.x.ToString();
                    VertString += ",";
                    VertString += FaceArray[ThisFace].VertData[ThisVert].position.y.ToString();
                    VertString += ",";
                    VertString += FaceArray[ThisFace].VertData[ThisVert].position.z.ToString();
                    VertString += ";";
                }
            }
            Tarmac.GenerateElementRaw(XMLDoc, Parent, "VertPos", VertString);

            VertString = "";
            for (int ThisFace = 0; ThisFace < FaceArray.Length; ThisFace++)
            {
                for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                {
                    VertString += FaceArray[ThisFace].VertData[ThisVert].color.R.ToString();
                    VertString += ",";
                    VertString += FaceArray[ThisFace].VertData[ThisVert].color.G.ToString();
                    VertString += ",";
                    VertString += FaceArray[ThisFace].VertData[ThisVert].color.B.ToString();
                    VertString += ",";
                    VertString += FaceArray[ThisFace].VertData[ThisVert].color.A.ToString();
                    VertString += ";";
                }
            }
            Tarmac.GenerateElementRaw(XMLDoc, Parent, "VertColor", VertString);

            VertString = "";
            for (int ThisFace = 0; ThisFace < FaceArray.Length; ThisFace++)
            {
                for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                {
                    VertString += FaceArray[ThisFace].VertData[ThisVert].position.u.ToString();
                    VertString += ",";
                    VertString += FaceArray[ThisFace].VertData[ThisVert].position.v.ToString();
                    VertString += ",";
                    VertString += FaceArray[ThisFace].VertData[ThisVert].position.sPure.ToString();
                    VertString += ",";
                    VertString += FaceArray[ThisFace].VertData[ThisVert].position.sPure.ToString();
                    VertString += ";";
                }
            }
            Tarmac.GenerateElementRaw(XMLDoc, Parent, "VertUVs", VertString);
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

        public OK64Texture[] loadTextures(Assimp.Scene fbx, string filePath)
        {

            int materialCount = fbx.Materials.Count;
            OK64Texture[] textureArray = new OK64Texture[materialCount];
            
            for (int materialIndex = 0; materialIndex < materialCount; materialIndex++)
            {
                textureArray[materialIndex] = new TM64_Geometry.OK64Texture();
                textureArray[materialIndex].GeometryBools = new bool[F3DEX095_Parameters.GeometryModes.Length];
                textureArray[materialIndex].textureName = fbx.Materials[materialIndex].Name;
                textureArray[materialIndex].GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_ZBUFFER)] = true;
                textureArray[materialIndex].GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_SHADE)] = true;
                textureArray[materialIndex].GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_SHADING_SMOOTH)] = true;
                textureArray[materialIndex].GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_CULL_BACK)] = true;
                textureArray[materialIndex].GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_CLIPPING)] = true;
                textureArray[materialIndex].GeometryModes = 0;
                textureArray[materialIndex].TextureFilter = Array.IndexOf(F3DEX095_Parameters.TextureFilters, F3DEX095_Parameters.G_TF_BILERP);
                textureArray[materialIndex].CombineModeA = 1; //F3DEX095_Parameters.G_CC_SHADE;
                textureArray[materialIndex].CombineModeB = 1;
                textureArray[materialIndex].TextureOverWrite = new int[0];
                textureArray[materialIndex].RenderModeA = Array.IndexOf(F3DEX095_Parameters.RenderModes, F3DEX095_Parameters.G_RM_AA_ZB_OPA_SURF);
                textureArray[materialIndex].RenderModeB = Array.IndexOf(F3DEX095_Parameters.RenderModes, F3DEX095_Parameters.G_RM_AA_ZB_OPA_SURF2);
                textureArray[materialIndex].alphaPath = "";
                if ((fbx.Materials[materialIndex].TextureDiffuse.FilePath != null) && (fbx.Materials[materialIndex].TextureDiffuse.FilePath != ""))
                {
                    string mainDirectory = Path.GetDirectoryName(filePath);


                    textureArray[materialIndex].texturePath = fbx.Materials[materialIndex].TextureDiffuse.FilePath;
    
                    textureArray[materialIndex].texturePath = Path.Combine(mainDirectory, textureArray[materialIndex].texturePath);
                    textureArray[materialIndex].texturePath = Path.GetFullPath(textureArray[materialIndex].texturePath);


                    if ((fbx.Materials[materialIndex].TextureOpacity.FilePath != null)&&(fbx.Materials[materialIndex].TextureOpacity.FilePath != ""))
                    {
                        textureArray[materialIndex].alphaPath = fbx.Materials[materialIndex].TextureOpacity.FilePath;

                        textureArray[materialIndex].alphaPath = Path.Combine(mainDirectory, textureArray[materialIndex].alphaPath);
                        textureArray[materialIndex].alphaPath = Path.GetFullPath(textureArray[materialIndex].alphaPath);


                    }

                    textureArray[materialIndex].textureName = Path.GetFileNameWithoutExtension(textureArray[materialIndex].texturePath);

                    textureArray[materialIndex].CombineModeA = 6; //F3DEX095_Parameters.G_CC_MODULATERGBA;
                    textureArray[materialIndex].CombineModeB = 6;
                    textureArray[materialIndex].textureScrollS = 0;
                    textureArray[materialIndex].textureScrollT = 0;
                    textureArray[materialIndex].textureScreen = 0;
                    switch (fbx.Materials[materialIndex].TextureDiffuse.WrapModeU)
                    {
                        case Assimp.TextureWrapMode.Wrap:
                            {
                                textureArray[materialIndex].SFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_WRAP);
                                break;
                            }
                        case Assimp.TextureWrapMode.Mirror:
                            {
                                textureArray[materialIndex].SFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_MIRROR);
                                break;
                            }
                        case Assimp.TextureWrapMode.Clamp:
                            {
                                textureArray[materialIndex].SFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_CLAMP);
                                break;
                            }
                    }
                    switch (fbx.Materials[materialIndex].TextureDiffuse.WrapModeV)
                    {
                        case Assimp.TextureWrapMode.Wrap:
                            {
                                textureArray[materialIndex].TFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_WRAP);
                                break;
                            }
                        case Assimp.TextureWrapMode.Mirror:
                            {
                                textureArray[materialIndex].TFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_MIRROR);
                                break;
                            }
                        case Assimp.TextureWrapMode.Clamp:
                            {
                                textureArray[materialIndex].TFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_CLAMP);
                                break;
                            }
                    }
                    //fbx.Materials[materialIndex].TextureDiffuse.WrapModeU

                    textureArray[materialIndex].TextureFormat = Array.IndexOf(F3DEX095_Parameters.TextureFormats, F3DEX095_Parameters.G_IM_FMT_RGBA);
                    textureArray[materialIndex].BitSize = Array.IndexOf(F3DEX095_Parameters.BitSizes, F3DEX095_Parameters.G_IM_SIZ_16b);


                    if (File.Exists(textureArray[materialIndex].texturePath))
                    {
                        using (var fs = new FileStream(textureArray[materialIndex].texturePath, FileMode.Open, FileAccess.Read))
                        {
                            textureArray[materialIndex].RawTexture.textureBitmap = Image.FromStream(fs);
                            fs.Close();
                        }
                        textureArray[materialIndex].textureHeight = textureArray[materialIndex].RawTexture.textureBitmap.Height;
                        textureArray[materialIndex].textureWidth = textureArray[materialIndex].RawTexture.textureBitmap.Width;

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

        public string[] WriteData(short Data)
        {
            List<string> Output = new List<string>();
            Output.Add(Data.ToString());
            return Output.ToArray();
        }
        public string[] WriteData(string Data)
        {
            List<string> Output = new List<string>();
            Output.Add(Data.ToString());
            return Output.ToArray();
        }

        public string[] WriteData(float Data)
        {
            List<string> Output = new List<string>();
            Output.Add(Data.ToString());
            return Output.ToArray();
        }

        public string[] WriteDebugAnimation(OK64Bone Skeleton)
        {
            List<string> Output = new List<string>();
            

            Output.AddRange(WriteData(Skeleton.Name));
            Output.AddRange(WriteData(Skeleton.FrameCount));

            for (int ThisVector = 0; ThisVector < 3; ThisVector++)
            {
                Output.AddRange(WriteData(Skeleton.Origin[ThisVector]));
            }
            Output.Add(Environment.NewLine);
            if (Skeleton.Animation == null)
            {
                Output.AddRange(WriteData(0));
                Output.AddRange(WriteData(0));
                Output.AddRange(WriteData(0));
            }
            else
            {
                Output.AddRange(WriteData(Skeleton.Name + "Rotations"));
                if (Skeleton.Animation.RotationData != null)
                {
                    Output.AddRange(WriteData(Skeleton.Animation.RotationData.Length));


                    for (int ThisRot = 0; ThisRot < Skeleton.Animation.RotationData.Length; ThisRot++)
                    {
                        for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                        {
                            Output.AddRange(WriteData(Convert.ToSingle(Skeleton.Animation.RotationData[ThisRot][ThisVector] / 182.0f)));
                        }
                        Output.Add(Environment.NewLine);

                    }
                }
                else
                {
                    Output.AddRange(WriteData(0));
                }

                Output.AddRange(WriteData(Skeleton.Name + "Translations"));
                if (Skeleton.Animation.TranslationData != null)
                {

                    Output.AddRange(WriteData(Skeleton.Animation.TranslationData.Length));

                    for (int ThisRot = 0; ThisRot < Skeleton.Animation.TranslationData.Length; ThisRot++)
                    {
                        for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                        {
                            Output.AddRange(WriteData(Skeleton.Animation.TranslationData[ThisRot][ThisVector]));
                        }
                        Output.Add(Environment.NewLine);
                    }
                }
                else
                {
                    Output.AddRange(WriteData(0));
                }

                Output.AddRange(WriteData(Skeleton.Name + "Scales"));
                if (Skeleton.Animation.ScalingData != null)
                {
                    Output.AddRange(WriteData(Skeleton.Animation.ScalingData.Length));

                    for (int ThisRot = 0; ThisRot < Skeleton.Animation.ScalingData.Length; ThisRot++)
                    {
                        for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                        {
                            Output.AddRange(WriteData(Skeleton.Animation.ScalingData[ThisRot][ThisVector]));
                        }
                        Output.Add(Environment.NewLine);
                    }
                }
                else
                {
                    Output.AddRange(WriteData(0));
                }

            }

            Output.AddRange(WriteData("Children"));
            Output.AddRange(WriteData(Skeleton.Children.Length));
            Output.Add(Environment.NewLine);

            foreach (var Child in Skeleton.Children)
            {
                Output.AddRange(WriteDebugAnimation(Child));
            }
            return Output.ToArray();
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


                binaryWriter.Write(TextureData[ThisTexture].textureName);
                binaryWriter.Write(TextureData[ThisTexture].CombineModeA);
                binaryWriter.Write(TextureData[ThisTexture].CombineModeB);

                for (int ThisBool = 0; ThisBool < F3DEX095_Parameters.GeometryModes.Length; ThisBool++)
                {
                    binaryWriter.Write(TextureData[ThisTexture].GeometryBools[ThisBool]);
                }

                binaryWriter.Write(TextureData[ThisTexture].RenderModeA);
                binaryWriter.Write(TextureData[ThisTexture].RenderModeB);

                if (TextureData[ThisTexture].texturePath != null)
                {
                    binaryWriter.Write(TextureData[ThisTexture].texturePath);
                    binaryWriter.Write(TextureData[ThisTexture].textureScrollS);
                    binaryWriter.Write(TextureData[ThisTexture].textureScrollT);
                    binaryWriter.Write(TextureData[ThisTexture].textureScreen);

                    binaryWriter.Write(TextureData[ThisTexture].SFlag);
                    binaryWriter.Write(TextureData[ThisTexture].TFlag);


                    
                    binaryWriter.Write(TextureData[ThisTexture].TextureFormat);
                    binaryWriter.Write(TextureData[ThisTexture].BitSize);



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
                for (int ThisBool = 0; ThisBool < 8; ThisBool++)
                {
                    binaryWriter.Write(ModelData[ThisModel].KillDisplayList[ThisBool]);
                }

                binaryWriter.Write(ModelData[ThisModel].modelGeometry.Length);
                for (int ThisGeo = 0; ThisGeo < ModelData[ThisModel].modelGeometry.Length; ThisGeo++)
                {

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

        public OK64F3DObject createObject (Assimp.Scene fbx, Assimp.Node objectNode, OK64Texture[] textureArray, bool ForceFlatUV = false, bool AlphaChannelTwo = false, bool DisregardOrigin = false)
        {
            OK64F3DObject newObject = new OK64F3DObject();
            TM64.OK64Settings TarmacSettings = new TM64.OK64Settings();
            TarmacSettings.LoadSettings();
            
            newObject.objectColor = new float[3];
            newObject.objectColor[0] = rValue.NextFloat(0.3f, 1);
            newObject.objectColor[1] = rValue.NextFloat(0.3f, 1);
            newObject.objectColor[2] = rValue.NextFloat(0.3f, 1);
            newObject.objectName = objectNode.Name;
            newObject.KillDisplayList = new bool[8] { true, true, true, true, true, true, true, true };

            if (objectNode.MeshIndices.Count == 0)
            {
                MessageBox.Show("Empty Course Object! -" + newObject.objectName);
                newObject.materialID = 0;
                newObject.faceCount = 0;
                newObject.vertCount = 0;
                newObject.pathfindingObject = new PathfindingObject();
                newObject.pathfindingObject.highX = 0;
                newObject.pathfindingObject.lowX = 0;
                newObject.pathfindingObject.highY = 0;
                newObject.pathfindingObject.lowY = 0;
                
                newObject.modelGeometry = CreateStandard(0);
                return newObject;
            } 
            newObject.materialID = fbx.Meshes[objectNode.MeshIndices[0]].MaterialIndex;
            int vertCount = 0;
            int faceCount = 0;



            Assimp.Vector3D BOrigin = new Assimp.Vector3D();
            Assimp.Vector3D BScale = new Assimp.Vector3D();
            Assimp.Quaternion RotQuat = new Assimp.Quaternion();
            float[] BRotation = new float[3];
            if (TarmacSettings.ImportMode > 0)
            {

                Assimp.Matrix4x4 OPrime = GetTotalTransform(objectNode, fbx);

                OPrime.Decompose(out BScale, out RotQuat, out BOrigin);

                if (TarmacSettings.ImportMode == 1)
                {
                    //Blender uses 100.0f scaling
                    //3DS Max uses 1.0f scaling
                    BScale *= 0.01f;
                }

                BRotation = ConvertEuler(RotQuat);

                BRotation[0] /= Convert.ToSingle(0.01745329252);
                BRotation[1] /= Convert.ToSingle(0.01745329252);
                BRotation[2] /= Convert.ToSingle(0.01745329252);
            }
            else
            {
                BOrigin[0] = 0.0f;
                BOrigin[1] = 0.0f;
                BOrigin[2] = 0.0f;

                BScale[0] = 1.0f;
                BScale[1] = 1.0f;
                BScale[2] = 1.0f;
            }

            List<int> xValues = new List<int>();
            List<int> yValues = new List<int>();
            List<int> zValues = new List<int>();

            

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

                    if (childPoly.IndexCount != 3)
                    {
                        MessageBox.Show("FATAL ERROR - INDEX COUNT " + childPoly.IndexCount + "- OBJ:" + newObject.objectName);
                    }

                    for (int currentVert = 0; currentVert < 3; currentVert++)
                    { 
                        newObject.modelGeometry[currentFace].VertData[currentVert] = new Vertex();
                        newObject.modelGeometry[currentFace].VertData[currentVert].position = new Position();
                        
                            Point3D VertPosition = new Point3D
                        (
                            (fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].X) * BScale[0] * TarmacSettings.ImportScale,
                            (fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].Y) * BScale[1] * TarmacSettings.ImportScale,
                            (fbx.Meshes[childMesh].Vertices[childPoly.Indices[currentVert]].Z) * BScale[2] * TarmacSettings.ImportScale
                        );

                        Point3D NewPosition = new Point3D();
                        if (!DisregardOrigin)
                        {
                            NewPosition = RotatePoint(VertPosition, BRotation);

                            NewPosition.X += BOrigin[0];
                            NewPosition.Y += BOrigin[1];
                            NewPosition.Z += BOrigin[2];
                        }
                        else
                        {
                            NewPosition.X = VertPosition.X;
                            NewPosition.Y = VertPosition.Y;
                            NewPosition.Z = VertPosition.Z; 
                        }


                        try
                        {
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.x = Convert.ToInt16(NewPosition.X);
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.y = Convert.ToInt16(NewPosition.Y);
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.z = Convert.ToInt16(NewPosition.Z);
                        }
                        catch (OverflowException) 
                        {
                            MessageBox.Show(newObject.objectName + " - Mesh indices are too high or too low. Check your FBX export settings and Tarmac Import Scale");
                            Environment.Exit(0);
                        }

                        xValues.Add(newObject.modelGeometry[currentFace].VertData[currentVert].position.x);
                        yValues.Add(newObject.modelGeometry[currentFace].VertData[currentVert].position.y);
                        zValues.Add(newObject.modelGeometry[currentFace].VertData[currentVert].position.z);

                        l_xValues.Add(newObject.modelGeometry[currentFace].VertData[currentVert].position.x);
                        l_yValues.Add(newObject.modelGeometry[currentFace].VertData[currentVert].position.y);
                        l_zValues.Add(newObject.modelGeometry[currentFace].VertData[currentVert].position.z);





                        newObject.modelGeometry[currentFace].VertData[currentVert].color = new OK64Color();
                        if (fbx.Meshes[childMesh].VertexColorChannels[0].Count > 0)
                        {
                            
                            newObject.modelGeometry[currentFace].VertData[currentVert].color.R = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].R * 255));
                            newObject.modelGeometry[currentFace].VertData[currentVert].color.G = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].G * 255));
                            newObject.modelGeometry[currentFace].VertData[currentVert].color.B = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].B * 255));

                            if (AlphaChannelTwo && (fbx.Meshes[childMesh].VertexColorChannels.Length > 1))
                            {
                                if (fbx.Meshes[childMesh].VertexColorChannels[1].Count > 0)
                                {
                                    int AlphaValue = GetMax((Convert.ToInt32(fbx.Meshes[childMesh].VertexColorChannels[1][childPoly.Indices[currentVert]].R * 255)), GetMax((Convert.ToInt32(fbx.Meshes[childMesh].VertexColorChannels[1][childPoly.Indices[currentVert]].G * 255)), (Convert.ToInt32(fbx.Meshes[childMesh].VertexColorChannels[1][childPoly.Indices[currentVert]].B * 255))));
                                    newObject.modelGeometry[currentFace].VertData[currentVert].color.A = (Convert.ToByte(AlphaValue));
                                }
                                else
                                {
                                    newObject.modelGeometry[currentFace].VertData[currentVert].color.A = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].A * 255));
                                }
                            }
                            else
                            {
                                newObject.modelGeometry[currentFace].VertData[currentVert].color.A = (Convert.ToByte(fbx.Meshes[childMesh].VertexColorChannels[0][childPoly.Indices[currentVert]].A * 255));
                            }
                            
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
                        
                    }



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
                        float uBase, vBase;
                        float uShift, vShift;
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


                        //Get the center of the triangle for reorienting. 

                        uBase = (u_offset[0] + u_offset[1] + u_offset[2]) / 3;
                        vBase = (v_offset[0] + v_offset[1] + v_offset[2]) / 3;

                        //Now apply a modulus operation to get the u/v_base as a decimal only, removing the whole value and any inherited tiling.
                        //Use 2 for the base to maintain mirroring. 
                        uShift = uBase - (uBase % 2.0f);
                        vShift = vBase - (vBase % 2.0f);

                        // And now add the offsets to the base to get each vert's actual U/V coordinate, before converting to ST.

                        for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                        {
                            u_offset[ThisVector] -= uShift;
                            v_offset[ThisVector] -= vShift;
                        }
                        // and now apply the calculation to make them into ST coords for Mario Kart.
                        //

                        for (int currentVert = 0; currentVert < 3; currentVert++)
                        {

                            newObject.modelGeometry[currentFace].VertData[currentVert].position.sBase = u_offset[currentVert] * 32;
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.tBase = (1 - v_offset[currentVert]) * 32;
                            if (fbx.Meshes[childMesh].TextureCoordinateChannels[0].Count == 0)
                            {

                                newObject.modelGeometry[currentFace].VertData[currentVert].position.sPure = 0;
                                newObject.modelGeometry[currentFace].VertData[currentVert].position.tPure = 0;

                            }
                            else
                            {
                                newObject.modelGeometry[currentFace].VertData[currentVert].position.sPure = fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[currentVert]][0] * 32;
                                newObject.modelGeometry[currentFace].VertData[currentVert].position.tPure = (1 - fbx.Meshes[childMesh].TextureCoordinateChannels[0][childPoly.Indices[currentVert]][1]) * 32;
                            }
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.u = u_offset[currentVert];
                            newObject.modelGeometry[currentFace].VertData[currentVert].position.v = (1 - v_offset[currentVert]);
                        }
                    }




                    currentFace++;


                }

                int[][] localMax = new int[3][];

                localMax[0] = new int[2];
                localMax[1] = new int[2];
                localMax[2] = new int[2];

                localMax[0][0] = -9999999;
                localMax[0][1] = 9999999;
                localMax[1][0] = -9999999;
                localMax[1][1] = 9999999;
                localMax[2][0] = -9999999;
                localMax[2][1] = 9999999;

                for (int currentValue = 0; currentValue < xValues.Count; currentValue++)
                {
                    if (xValues[currentValue] > localMax[0][0])
                    {
                        localMax[0][0] = xValues[currentValue];
                    }
                    if (xValues[currentValue] < localMax[0][1])
                    {
                        localMax[0][1] = xValues[currentValue];
                    }
                    if (yValues[currentValue] > localMax[1][0])
                    {
                        localMax[1][0] = yValues[currentValue];
                    }
                    if (yValues[currentValue] < localMax[1][1])
                    {
                        localMax[1][1] = yValues[currentValue];
                    }
                    if (zValues[currentValue] > localMax[2][0])
                    {
                        localMax[2][0] = zValues[currentValue];
                    }
                    if (zValues[currentValue] < localMax[2][1])
                    {
                        localMax[2][1] = zValues[currentValue];
                    }
                }

                newObject.pathfindingObject = new PathfindingObject();
                newObject.pathfindingObject.highX = localMax[0][0];
                newObject.pathfindingObject.lowX = localMax[0][1];
                newObject.pathfindingObject.highY = localMax[1][0];
                newObject.pathfindingObject.lowY = localMax[1][1];
                newObject.pathfindingObject.highZ = localMax[2][0];
                newObject.pathfindingObject.lowZ = localMax[2][1];

                newObject.WaveObject = false;
                for (int KillCheck = 0; KillCheck < newObject.KillDisplayList.Length; KillCheck++)
                {
                    newObject.KillDisplayList[KillCheck] = true;
                }


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

            OK64F3DObject[] ungroupedArray = ungroupedObjects.ToArray();
            Array.Sort(ungroupedArray, (x, y) => string.Compare(x.objectName, y.objectName));

            List<OK64F3DObject> masterList = new List<OK64F3DObject>();
            masterList.AddRange(groupObjects);
            masterList.AddRange(ungroupedArray);
            return masterList.ToArray();
        }

        public OK64F3DObject[] LoadMaster(ref OK64F3DGroup[] groupArray, Assimp.Scene fbx, OK64Texture[] textureArray, bool AlphaCH = false)
        {
            
            var masterNode = fbx.RootNode.FindNode("Render Objects");
            if (masterNode == null)
            {
                masterNode = fbx.RootNode.FindNode("Course Master Objects");
                if (masterNode == null )
                {
                    return null;
                }
            }
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
                        masterList.Add(createObject(fbx, groupParent.Children[currentGrandchild],textureArray, false, AlphaCH));
                        masterCount++;
                    }
                }
                else
                {
                    masterList.Add(createObject(fbx, masterNode.Children[currentChild], textureArray, false, AlphaCH));
                    masterCount++;
                }
            }
            
            groupArray = groupList.ToArray();
            masterObjects = GroupSort(masterList.ToArray(), groupArray);
            return masterObjects;
        }



        public OK64F3DObject[] CreateObjects(Assimp.Scene fbx, OK64Texture[] textureArray, bool DisregardOrigin = false)
        {
            List<OK64F3DObject> masterObjects = new List<OK64F3DObject>();
            int currentObject = 0; 
            var BaseNode = fbx.RootNode.FindNode("Render Objects");
            TM64.OK64Settings TarmacSettings = new TM64.OK64Settings();
            TarmacSettings.LoadSettings();
            if (BaseNode == null)
            {
                MessageBox.Show("Error - No 'Render Objects' node");
            }
            for (int childObject = 0; childObject < BaseNode.Children.Count; childObject++)
            {
                masterObjects.Add(createObject(fbx, BaseNode.Children[childObject], textureArray, false, TarmacSettings.AlphaCH2, DisregardOrigin));
            }
            List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>(masterObjects);
            OK64F3DObject[] outputObjects = NaturalSort(masterObjects).ToArray();
            return outputObjects;
        }



        public OK64F3DObject[] CreateMasters(Assimp.Scene fbx, int sectionCount, OK64Texture[] textureArray, bool AlphaCH = false)
        {
            List<OK64F3DObject> masterObjects = new List<OK64F3DObject>();
            int currentObject = 0;
            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                var surfaceNode = fbx.RootNode.FindNode("Section " +(currentSection + 1).ToString());
                
                for (int childObject = 0; childObject < surfaceNode.Children.Count; childObject++)
                {
                    masterObjects.Add(createObject(fbx,surfaceNode.Children[childObject], textureArray, false, AlphaCH));
                    currentObject++;
                }
                List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>(masterObjects);
            }

            OK64F3DObject[] outputObjects = NaturalSort(masterObjects).ToArray();

            return outputObjects;
        }

        public OK64F3DObject[] CreateMasterNoHeader(Assimp.Scene fbx, OK64Texture[] textureArray)
        {
            List<OK64F3DObject> masterObjects = new List<OK64F3DObject>();
            int currentObject = 0;
            for (int TargetOBJ = 0; TargetOBJ < fbx.RootNode.ChildCount; TargetOBJ++)
            {
                masterObjects.Add(createObject(fbx, fbx.RootNode.Children[TargetOBJ], textureArray, false, false));
                currentObject++;
            }
            OK64F3DObject[] outputObjects = NaturalSort(masterObjects).ToArray();
            return outputObjects;
        }


        public OK64F3DObject[] LoadCollisions(Assimp.Scene fbx, int sectionCount, OK64Texture[] textureArray)
         {   
            int totalIndexCount = 0;
            int totalIndex = 0;
            var surfaceNode = fbx.RootNode;
            List<OK64F3DObject> surfaceObjects = new List<OK64F3DObject>();
            float[] colorValues = new float[3];
            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                surfaceNode = fbx.RootNode.FindNode("Section " + (currentSection + 1).ToString());
                

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
                    surfaceObjects[currentObject].objectColor = new float[3] { colorValues[0], colorValues[1], colorValues[2] };
                    string[] surfaceID = surfaceObjects[currentObject].objectName.Split('_');
                    byte SurfaceStorageByte = 0;
                    if (surfaceID[0].Length != 0)
                    {
                        bool TestResult = byte.TryParse(surfaceID[0], out SurfaceStorageByte);
                        if (!TestResult)
                        {
                            MessageBox.Show("ERROR- Bad Surface Index - " + surfaceObjects[currentObject].objectName);                        
                        }
                        surfaceObjects[currentObject].surfaceMaterial = SurfaceStorageByte;
                    }
                    else
                    {
                        bool TestResult = byte.TryParse(surfaceID[1], out SurfaceStorageByte);
                        if (!TestResult)
                        {
                            MessageBox.Show("ERROR- Bad Surface Index - " + surfaceObjects[currentObject].objectName);
                        }
                        surfaceObjects[currentObject].surfaceMaterial = SurfaceStorageByte;
                    }
                    surfaceObjects[currentObject].materialID = 0;
                    totalIndex++;
                }
            }
            return surfaceObjects.ToArray();
        }

        public OK64F3DObject[] UpdateSectionIndexNoHeader(OK64F3DObject[] SurfaceObjects, ref int SectionCount)
        {

            SectionCount = 1;
            
            for (int ThisObject = 0;ThisObject < SurfaceObjects.Length; ThisObject++)
            {
                string[] Split = SurfaceObjects[ThisObject].objectName.Split('_');
                if (Split.Length > 2)
                {
                    int Result;
                    if (int.TryParse(Split[1], out Result))
                    {
                        SurfaceObjects[ThisObject].surfaceID = Result;
                        if (Result > SectionCount)
                        {
                            SectionCount = Result;
                        }
                    }
                    else
                    {
                        SurfaceObjects[ThisObject].surfaceID = 1;
                    }
                }
            }




            return SurfaceObjects.ToArray();
        }
        public OK64F3DObject[] CreateCollisionsNoHeader(Assimp.Scene fbx, OK64Texture[] textureArray)
        {
            int totalIndexCount = 0;
            int totalIndex = 0;
            var surfaceNode = fbx.RootNode;
            List<OK64F3DObject> surfaceObjects = new List<OK64F3DObject>();
            float[] colorValues = new float[3];
            for (int currentSection = 0; currentSection < 1; currentSection++)
            {
                int subobjectCount = fbx.RootNode.ChildCount;
                for (int currentsubObject = 0; currentsubObject < subobjectCount; currentsubObject++)
                {
                    surfaceObjects.Add(createObject(fbx, surfaceNode.Children[currentsubObject], textureArray, true));
                    int currentObject = surfaceObjects.Count - 1;
                    surfaceObjects[currentObject].surfaceID = currentSection + 1;
                    surfaceObjects[currentObject].objectColor = colorValues;
                    string[] surfaceID = surfaceObjects[currentObject].objectName.Split('_');
                    byte SurfaceStorageByte = 0;
                    if (surfaceID[0].Length != 0)
                    {
                        bool TestResult = byte.TryParse(surfaceID[0], out SurfaceStorageByte);
                        if (!TestResult)
                        {
                            MessageBox.Show("ERROR- Bad Surface Index - " + surfaceObjects[currentObject].objectName);
                        }
                        surfaceObjects[currentObject].surfaceMaterial = SurfaceStorageByte;
                    }
                    else
                    {
                        bool TestResult = byte.TryParse(surfaceID[1], out SurfaceStorageByte);
                        if (!TestResult)
                        {
                            MessageBox.Show("ERROR- Bad Surface Index - " + surfaceObjects[currentObject].objectName);
                        }
                        surfaceObjects[currentObject].surfaceMaterial = SurfaceStorageByte;
                    }
                    surfaceObjects[currentObject].materialID = 0;
                    totalIndex++;
                }
            }
            return surfaceObjects.ToArray();
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


            for (int ThisSection= 0; ThisSection < sectionCount; ThisSection++)
            {
                sectionList[ThisSection] = new OK64SectionList();
            }


            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                for (int currentView = 0; currentView < 1; currentView++)
                {
                    ConcurrentBag<int> searchList = new ConcurrentBag<int>();

                    for (int currentMaster = 0; currentMaster < masterObjects.Length; currentMaster++)
                    {
                        searchList.Add(currentMaster);
                    }


                    sectionList[currentSection].objectList = searchList.ToArray();
                }

            }


            return sectionList;
        }


        public byte[] WriteRawTextures(byte[] SegmentData, OK64Texture[] textureObject, int DataLength)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            int TextureSize = SegmentData.Length;
            

            binaryWriter.Write(SegmentData);



            int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;


            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

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
                        new N64Codec[]{ N64Codec.IA4, N64Codec.IA8, N64Codec.IA16, N64Codec.IA16 },
                        new N64Codec[]{ N64Codec.I4, N64Codec.I8, N64Codec.I8, N64Codec.I8 }
                    };

                    byte[] imageData = null;
                    byte[] paletteData = null;
                    Bitmap bitmapData = new Bitmap(textureObject[currentTexture].texturePath);
                    
                    N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], bitmapData);
                    


                    // finish setting texture parameters based on new texture and compressed data.

                    textureObject[currentTexture].RawTexture.compressedSize = imageData.Length;
                    textureObject[currentTexture].RawTexture.fileSize = imageData.Length;
                    textureObject[currentTexture].RawTexture.segmentPosition = Convert.ToInt32(binaryWriter.BaseStream.Position + DataLength);  // we need this to build out F3DEX commands later. 
                    TextureSize = TextureSize + textureObject[currentTexture].RawTexture.fileSize;


                    //adjust the MIO0 offset to an 8-byte address as required for N64.
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    
                    addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                    if (addressAlign == 16)
                        addressAlign = 0;


                    for (int align = 0; align < addressAlign; align++)
                    {
                        binaryWriter.Write(Convert.ToByte(0x00));
                    }



                    // write compressed MIO0 texture to end of ROM.

                    textureObject[currentTexture].RawTexture.romPosition = Convert.ToInt32(binaryWriter.BaseStream.Length) + DataLength;
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    binaryWriter.Write(imageData);

                    int SegPosition = Convert.ToInt32(binaryWriter.BaseStream.Length) + DataLength;
                    if (paletteData != null)
                    {
                        textureObject[currentTexture].RawTexture.PaletteData = paletteData;
                        textureObject[currentTexture].RawTexture.paletteSize = paletteData.Length;
                        textureObject[currentTexture].RawTexture.palettePosition = SegPosition;
                        SegPosition += textureObject[currentTexture].RawTexture.paletteSize;
                        addressAlign = 0x1000 - (SegPosition % 0x1000);
                        if (addressAlign == 0x1000)
                            addressAlign = 0;
                        SegPosition += addressAlign;

                        binaryWriter.Write(textureObject[currentTexture].RawTexture.PaletteData);
                        addressAlign = 0x1000 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 0x1000);
                        if (addressAlign == 0x1000)
                            addressAlign = 0;
                        for (int align = 0; align < addressAlign; align++)
                        {
                            binaryWriter.Write(Convert.ToByte(0x00));
                        }
                    }


                }
            }

            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));


            byte[] romOut = memoryStream.ToArray();
            
            return romOut;


        }


        public byte[] WriteModelTextures(byte[] SegmentData, OK64Texture[] textureObject, int DataLength)
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
                        new N64Codec[]{ N64Codec.IA4, N64Codec.IA8, N64Codec.IA16, N64Codec.IA16 },
                        new N64Codec[]{ N64Codec.I4, N64Codec.I8, N64Codec.I8, N64Codec.I8 }
                    };
                    byte[] imageData = null;
                    byte[] paletteData = null;
                    Bitmap bitmapData = new Bitmap(textureObject[currentTexture].texturePath);
                    if (textureObject[currentTexture].alphaPath != "")
                    {
                        if (File.Exists(textureObject[currentTexture].alphaPath))
                        {
                            Bitmap MaskedTexture = new Bitmap(bitmapData.Width, bitmapData.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            Bitmap alphaData = new Bitmap(textureObject[currentTexture].alphaPath);

                            for (int ThisY = 0; ThisY < bitmapData.Height; ThisY++)
                            {
                                for (int ThisX = 0; ThisX < bitmapData.Width; ThisX++)
                                {
                                    System.Drawing.Color AlphaData = alphaData.GetPixel(ThisX, ThisY);
                                    System.Drawing.Color ColorData = bitmapData.GetPixel(ThisX, ThisY);
                                    System.Drawing.Color NewColor = System.Drawing.Color.FromArgb(AlphaData.R, ColorData.R, ColorData.G, ColorData.B);
                                    MaskedTexture.SetPixel(ThisX, ThisY, NewColor);
                                    System.Drawing.Color CheckColor = MaskedTexture.GetPixel(ThisX, ThisY);
                                }
                            }
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], MaskedTexture);
                        }
                        else
                        {
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], bitmapData);
                        }
                    }
                    else
                    {
                        N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], bitmapData);
                    }



                    //adjust the MIO0 offset to an 8-byte address as required for N64.
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                    if (addressAlign == 16)
                        addressAlign = 0;


                    for (int align = 0; align < addressAlign; align++)
                    {
                        binaryWriter.Write(Convert.ToByte(0x00));
                    }



                    // finish setting texture parameters based on new texture and compressed data.

                    textureObject[currentTexture].RawTexture.compressedSize = imageData.Length;
                    textureObject[currentTexture].RawTexture.fileSize = imageData.Length;
                    textureObject[currentTexture].RawTexture.segmentPosition = Convert.ToInt32(binaryWriter.BaseStream.Position + DataLength);


                    // write compressed MIO0 texture to end of ROM.

                    textureObject[currentTexture].RawTexture.romPosition = Convert.ToInt32(binaryWriter.BaseStream.Length) + DataLength;
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    textureObject[currentTexture].RawTexture.TextureData = imageData;                    
                    binaryWriter.Write(imageData);

                    int SegPosition = Convert.ToInt32(binaryWriter.BaseStream.Length) + DataLength;
                    if (paletteData != null)
                    {
                        textureObject[currentTexture].RawTexture.PaletteData = paletteData;
                        textureObject[currentTexture].RawTexture.paletteSize = paletteData.Length;
                        textureObject[currentTexture].RawTexture.segmentPosition = SegPosition;
                        SegPosition += textureObject[currentTexture].RawTexture.paletteSize;
                        addressAlign = 0x1000 - (SegPosition % 0x1000);
                        if (addressAlign == 0x1000)
                            addressAlign = 0;
                        SegPosition += addressAlign;

                        binaryWriter.Write(textureObject[currentTexture].RawTexture.PaletteData);
                        addressAlign = 0x1000 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 0x1000);
                        if (addressAlign == 0x1000)
                            addressAlign = 0;
                        for (int align = 0; align < addressAlign; align++)
                        {
                            binaryWriter.Write(Convert.ToByte(0x00));
                        }
                    }

                }
            }

            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));


            byte[] romOut = memoryStream.ToArray();

            return romOut;


        }


        public void BuildTextures(OK64Texture[] TextureArray)
        {
            int segment5Position = 0;
            List<int> SkipMaterials = new List<int>();
            for (int currentTexture = 0; currentTexture < TextureArray.Length; currentTexture++)
            {
                foreach (var Index in TextureArray[currentTexture].TextureOverWrite)
                {
                    SkipMaterials.Add(Index);
                }
            }

            for (int currentTexture = 0; currentTexture < TextureArray.Length; currentTexture++)
            {
                if (!SkipMaterials.Contains(currentTexture))
                {
                    if ((TextureArray[currentTexture].texturePath != null) && (TextureArray[currentTexture].texturePath != "NULL"))
                    {
                        // Establish codec and convert texture. Compress converted texture data via MIO0 compression


                        N64Codec[][] n64Codec = new N64Codec[][] {
                            new N64Codec[]{ N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA32 },
                            new N64Codec[]{ N64Codec.ONEBPP, N64Codec.ONEBPP , N64Codec.ONEBPP , N64Codec.ONEBPP },
                            new N64Codec[]{ N64Codec.CI4, N64Codec.CI8, N64Codec.CI8, N64Codec.CI8 },
                            new N64Codec[]{ N64Codec.IA4, N64Codec.IA8, N64Codec.IA16, N64Codec.IA16 },
                            new N64Codec[]{ N64Codec.I4, N64Codec.I8, N64Codec.I8, N64Codec.I8 }
                        };
                        byte[] imageData = null;
                        byte[] paletteData = null;
                        Bitmap TextureData;
                        try
                        {
                            TextureData = new Bitmap(TextureArray[currentTexture].texturePath);
                        }
                        catch
                        {
                            TextureData = new Bitmap(Tarmac64_Library.Properties.Resources.TextureNotFound);
                        }

                        
                        if (TextureArray[currentTexture].alphaPath != "")
                        {
                            if (File.Exists(TextureArray[currentTexture].alphaPath))
                            {
                                Bitmap MaskedTexture = new Bitmap(TextureData.Width, TextureData.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                Bitmap alphaData = new Bitmap(TextureArray[currentTexture].alphaPath);

                                for (int ThisY = 0; ThisY < TextureData.Height; ThisY++)
                                {
                                    for (int ThisX = 0; ThisX < TextureData.Width; ThisX++)
                                    {
                                        
                                        System.Drawing.Color AlphaData = alphaData.GetPixel(ThisX, ThisY);
                                        System.Drawing.Color ColorData = TextureData.GetPixel(ThisX, ThisY);
                                        System.Drawing.Color NewColor = System.Drawing.Color.FromArgb(AlphaData.R, ColorData.R, ColorData.G, ColorData.B);
                                        MaskedTexture.SetPixel(ThisX, ThisY, NewColor);
                                        System.Drawing.Color CheckColor = MaskedTexture.GetPixel(ThisX, ThisY);
                                    }
                                }
                                MaskedTexture.Save(TextureArray[currentTexture].textureName);
                                N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[TextureArray[currentTexture].TextureFormat][TextureArray[currentTexture].BitSize], MaskedTexture);
                            }
                            else
                            {
                                N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[TextureArray[currentTexture].TextureFormat][TextureArray[currentTexture].BitSize], TextureData);
                            }
                        }
                        else
                        {
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[TextureArray[currentTexture].TextureFormat][TextureArray[currentTexture].BitSize], TextureData);
                        }

                        TextureArray[currentTexture].RawTexture.compressedTexture = Tarmac.CompressMIO0(imageData);
                        TextureArray[currentTexture].RawTexture.TextureData = imageData;
                        TextureArray[currentTexture].RawTexture.PaletteData = paletteData;

                        // finish setting texture parameters based on new texture and compressed data.

                        TextureArray[currentTexture].RawTexture.compressedSize = TextureArray[currentTexture].RawTexture.compressedTexture.Length;
                        TextureArray[currentTexture].RawTexture.fileSize = imageData.Length;
                        TextureArray[currentTexture].RawTexture.segmentPosition = segment5Position;  // we need this to build out F3DEX commands later.                     
                        segment5Position += TextureArray[currentTexture].RawTexture.fileSize;
                        if (paletteData != null)
                        {
                            TextureArray[currentTexture].RawTexture.paletteSize = paletteData.Length;
                            TextureArray[currentTexture].RawTexture.palettePosition = segment5Position;
                            segment5Position += TextureArray[currentTexture].RawTexture.paletteSize;
                            int addressAlign = 0x1000 - (segment5Position % 0x1000);
                            if (addressAlign == 0x1000)
                                addressAlign = 0;
                            segment5Position += addressAlign;
                        }
                    }
                }
            }
        }

        public byte[] WriteTextures(byte[] FileData, TM64_Course.Course Course)
        {
            OK64Texture[] TextureArray = Course.ModelData.TextureObjects;
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

                    TextureArray[CurrentTexture].RawTexture.romPosition = Convert.ToInt32(binaryWriter.BaseStream.Length);
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    binaryWriter.Write(TextureArray[CurrentTexture].RawTexture.TextureData);
                    if (TextureArray[CurrentTexture].RawTexture.PaletteData != null)
                    {
                        binaryWriter.Write(TextureArray[CurrentTexture].RawTexture.PaletteData);
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


        public byte[] CompileTextureTable(TM64_Course.Course CourseData)
        {
            OK64Texture[] textureObject = CourseData.ModelData.TextureObjects;
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            byte[] byteArray = new byte[0];



            MemoryStream seg9m = new MemoryStream();
            BinaryReader seg9r = new BinaryReader(seg9m);
            BinaryWriter seg9w = new BinaryWriter(seg9m);


            /*
            
            //Old Code for loading textures individually. 
            //Custom levels do not (currently) have capacity for shared textures.
            
            int textureCount = (textureObject.Length);
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


            //Compressing all texture data together increases efficiency
            if (CourseData.Segment5Length > 0)
            {
                //We have texture data


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


        public byte[] CompileF3DObject(byte[] InputData, OK64F3DObject[] MasterObjects, OK64Texture[] TextureObjects, int vertMagic, int Segment)
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

            byte[] SegmentByte = BitConverter.GetBytes(Segment);
            Array.Reverse(SegmentByte);
            uint SegmentBinary = BitConverter.ToUInt32(SegmentByte, 0);
            

            int relativeZero = InputData.Length + vertMagic;
            int relativeIndex = 0;


            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(InputData);

            int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;


            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }


            foreach (var cObj in MasterObjects)
            {
                OK64F3DModel[] CrunchedModel = CrunchF3DModel(cObj);
                cObj.meshPosition = new int[1];


                relativeZero = Convert.ToInt32(memoryStream.Position + vertMagic);
                cObj.VertCachePosition = Convert.ToInt32(SegmentBinary | relativeZero);
                
                for (int ThisDraw = 0; ThisDraw < CrunchedModel.Length; ThisDraw++)
                {
                    foreach (var ThisVert in CrunchedModel[ThisDraw].VertexCache)
                    {

                        if (CheckST(cObj, TextureObjects[cObj.materialID]))
                        {
                            MessageBox.Show("Fatal UV Error " + cObj.objectName);
                        }
                        if (TextureObjects[cObj.materialID].SFlag == -1)
                        {
                            ThisVert.position.s = 0;
                            ThisVert.position.t = 0;
                        }
                        //else if ((TextureObjects[cObj.materialID].SFlag > 0) || (TextureObjects[cObj.materialID].TFlag > 0))
                        {
                          //  ThisVert.position.s = Convert.ToInt16(ThisVert.position.sPure * 32 * TextureObjects[cObj.materialID].textureWidth);
                            //ThisVert.position.t = Convert.ToInt16(ThisVert.position.tPure * 32 * TextureObjects[cObj.materialID].textureHeight);
                        }
                        //else                        
                        {
                            ThisVert.position.s = Convert.ToInt16(ThisVert.position.sBase * TextureObjects[cObj.materialID].textureWidth);
                            ThisVert.position.t = Convert.ToInt16(ThisVert.position.tBase * TextureObjects[cObj.materialID].textureHeight);
                        }
                        

                        binaryWriter.Write(WriteVertexBinary16(ThisVert));

                    }
                }


                relativeZero = Convert.ToInt32(memoryStream.Position + vertMagic);
                cObj.meshPosition[0] = Convert.ToInt32(SegmentBinary | relativeZero);

                uint VOffset = 0;
                for (int ThisDraw = 0; ThisDraw < CrunchedModel.Length; ThisDraw++)
                {
                    uint VertexCount = Convert.ToUInt32(CrunchedModel[ThisDraw].VertexCache.Count);
                    int LocalFaceCount = CrunchedModel[ThisDraw].Indexes.Count;
                    binaryWriter.Write(F3D.gsSPVertex(Convert.ToUInt32(cObj.VertCachePosition + VOffset), VertexCount, 0));
                    VOffset += (VertexCount * 16);

                    for (int TargetIndex = 0; TargetIndex < LocalFaceCount;)
                    {
                        if (TargetIndex + 2 <= LocalFaceCount)
                        {
                            //Tri2
                            int[] Indexes = CrunchedModel[ThisDraw].Indexes[TargetIndex];
                            uint[] VertIndexesA = F3D.GetIndexes(Indexes[0], Indexes[2], Indexes[1]);

                            Indexes = CrunchedModel[ThisDraw].Indexes[TargetIndex + 1];
                            uint[] VertIndexesB = F3D.GetIndexes(Indexes[0], Indexes[2], Indexes[1]);

                            binaryWriter.Write(F3D.gsSP2Triangles(VertIndexesA, VertIndexesB));
                            TargetIndex += 2;
                        }
                        else
                        {
                            //Tri1
                            int[] Indexes = CrunchedModel[ThisDraw].Indexes[TargetIndex];
                            uint[] VertIndexesA = F3D.GetIndexes(Indexes[0], Indexes[2], Indexes[1]);

                            binaryWriter.Write(F3D.gsSP1Triangle(VertIndexesA));
                            TargetIndex += 1;
                        }
                    }

                }

                binaryWriter.Write(F3D.gsSPEndDisplayList());

            }


            return memoryStream.ToArray();

            
        }

        public byte[] CompileF3DHeader(int Position, byte[] HeaderData)
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

        public byte[] WriteVertexBinary14(TM64_Geometry.Vertex ThisVert)
        {
            return WriteVertexBinary14(
                ThisVert.position.x,
                ThisVert.position.y,
                ThisVert.position.z,
                ThisVert.position.s,
                ThisVert.position.t,
                ThisVert.color.R,
                ThisVert.color.G,
                ThisVert.color.B,
                ThisVert.color.A
                );
        
        }


        public byte[] WriteVertexBinary16(TM64_Geometry.Vertex ThisVert)
        {
            return WriteVertexBinary16(
                ThisVert.position.x,
                ThisVert.position.y,
                ThisVert.position.z,
                ThisVert.position.s,
                ThisVert.position.t,
                ThisVert.color.R,
                ThisVert.color.G,
                ThisVert.color.B,
                ThisVert.color.A
                );

        }
        public byte[] WriteVertexBinary14(int X, int Y, int Z, int S, int T, int R, int G, int B, int A)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(X)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Z)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(-1 * Y)));

            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(S)));  //ST Coordinates
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(T)));  //ST Coordinates

            binaryWriter.Write(Convert.ToByte(R));
            binaryWriter.Write(Convert.ToByte(G));
            binaryWriter.Write(Convert.ToByte(B));
            binaryWriter.Write(Convert.ToByte(A));

            return memoryStream.ToArray();
        }

        public byte[] WriteVertexBinary16(int X, int Y, int Z, int S, int T, int R, int G, int B, int A)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(X)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Z)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(-1 * Y)));

            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(0)));  //padding

            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(S)));  //ST Coordinates
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(T)));  //ST Coordinates


            binaryWriter.Write(Convert.ToByte(R));
            binaryWriter.Write(Convert.ToByte(G));
            binaryWriter.Write(Convert.ToByte(B));
            binaryWriter.Write(Convert.ToByte(A));

            return memoryStream.ToArray();
        }
        public bool CompileCourseObjects(ref int outMagic, ref byte[] outseg4, ref byte[] outseg7, byte[] segment4, byte[] segment7, OK64F3DObject[] courseObject, OK64Texture[] textureObject, int vertMagic, bool BoundingToggle = false)
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
            //


            byte[] SegmentByte = BitConverter.GetBytes(4);
            Array.Reverse(SegmentByte);
            uint Segment = BitConverter.ToUInt32(SegmentByte, 0);
            ///load the first set of verts from the relativeZero position;
            ///
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
                OK64F3DModel[] CrunchedModel = CrunchF3DModel(cObj);

                
                cObj.meshPosition = new int[1];

                cObj.meshPosition[0] = Convert.ToInt32(seg7w.BaseStream.Position);

                if (BoundingToggle)
                {
                    //Write bounding box check.

                    //XX YY ZZ SS TT RGBA 14bytes
                    var LocalBounds = cObj.pathfindingObject;
                    seg4w.Write(WriteVertexBinary14(
                        Convert.ToInt16(LocalBounds.highX),
                        Convert.ToInt16(LocalBounds.highY),
                        Convert.ToInt16(LocalBounds.highZ),
                        1, 1,
                        252, 252, 252, 255
                        ));
                    seg4w.Write(WriteVertexBinary14(
                        Convert.ToInt16(LocalBounds.lowX),
                        Convert.ToInt16(LocalBounds.highY),
                        Convert.ToInt16(LocalBounds.highZ),
                        0, 1,
                        252, 252, 252, 255
                        ));
                    seg4w.Write(WriteVertexBinary14(
                        Convert.ToInt16(LocalBounds.highX),
                        Convert.ToInt16(LocalBounds.lowY),
                        Convert.ToInt16(LocalBounds.highZ),
                        1, 1,
                        252, 252, 252, 255
                        ));
                    seg4w.Write(WriteVertexBinary14(
                        Convert.ToInt16(LocalBounds.lowX),
                        Convert.ToInt16(LocalBounds.lowY),
                        Convert.ToInt16(LocalBounds.highZ),
                        0, 1,
                        252, 252, 252, 255
                        ));



                    seg4w.Write(WriteVertexBinary14(
                        Convert.ToInt16(LocalBounds.highX),
                        Convert.ToInt16(LocalBounds.highY),
                        Convert.ToInt16(LocalBounds.lowZ),
                        1, 0,
                        252, 252, 252, 255
                        ));
                    seg4w.Write(WriteVertexBinary14(
                        Convert.ToInt16(LocalBounds.lowX),
                        Convert.ToInt16(LocalBounds.highY),
                        Convert.ToInt16(LocalBounds.lowZ),
                        0, 0,
                        252, 252, 252, 255
                        ));
                    seg4w.Write(WriteVertexBinary14(
                        Convert.ToInt16(LocalBounds.highX),
                        Convert.ToInt16(LocalBounds.lowY),
                        Convert.ToInt16(LocalBounds.lowZ),
                        1, 0,
                        252, 252, 252, 255
                        ));
                    seg4w.Write(WriteVertexBinary14(
                        Convert.ToInt16(LocalBounds.lowX),
                        Convert.ToInt16(LocalBounds.lowY),
                        Convert.ToInt16(LocalBounds.lowZ),
                        0, 0,
                        252,252,252,255
                        ));
                    
                    seg7w.Write(F3D.gsSPVertex(Convert.ToUInt32(Segment | relativeZero), 8, 0));
                    relativeZero += (8 * 16);
                    //accounts for the 8 verts in the bounding box.
                    seg7w.Write(F3D.gsSPCullDisplayList(0, 7));
                }
                
                cObj.VertCachePosition = Convert.ToInt32(Segment | relativeZero);

                for (int ThisDraw = 0; ThisDraw < CrunchedModel.Length; ThisDraw++)
                {

                    
                    uint SAddress = Convert.ToUInt32(Segment | relativeZero);
                    uint VertexCount = Convert.ToUInt32(CrunchedModel[ThisDraw].VertexCache.Count);
                    seg7w.Write(F3D.gsSPVertex(SAddress, VertexCount, 0));

                    relativeZero += Convert.ToInt32((VertexCount * 16));


                    foreach (var ThisVert in CrunchedModel[ThisDraw].VertexCache)
                    {

                        if (CheckST(cObj, textureObject[cObj.materialID]))
                        {
                            MessageBox.Show("Fatal UV Error " + cObj.objectName);
                            return false;
                        }
                        if (textureObject[cObj.materialID].SFlag == -1)
                        {
                            ThisVert.position.s = 0;
                            ThisVert.position.t = 0;
                        }
                        else if ((textureObject[cObj.materialID].SFlag > 0) || (textureObject[cObj.materialID].TFlag > 0))
                        {
                            ThisVert.position.s = Convert.ToInt16(ThisVert.position.sBase * textureObject[cObj.materialID].textureWidth);
                            ThisVert.position.t = Convert.ToInt16(ThisVert.position.tBase * textureObject[cObj.materialID].textureHeight);
                        }
                        else                        
                        {
                            ThisVert.position.s = Convert.ToInt16(ThisVert.position.sBase * textureObject[cObj.materialID].textureWidth);
                            ThisVert.position.t = Convert.ToInt16(ThisVert.position.tBase * textureObject[cObj.materialID].textureHeight);
                        }

                        OK64Color TargetColor = new OK64Color();
                        switch (cObj.surfaceProperty)
                        {
                            case 1:
                                {
                                    TargetColor.R = 153;
                                    TargetColor.G = 0;
                                    TargetColor.B = 153;
                                    TargetColor.A = 0;
                                    break;
                                }
                            case 2:
                                {

                                    TargetColor.R = 0;
                                    TargetColor.G = 153;
                                    TargetColor.B = 153;
                                    TargetColor.A = 0;
                                    break;
                                }
                            case 3:
                                {
                                    TargetColor.R = 255;
                                    TargetColor.G = 0;
                                    TargetColor.B = 0;
                                    TargetColor.A = 0;
                                    break;
                                }
                            case 4:
                                {
                                    TargetColor.R = 230;
                                    TargetColor.G = 204;
                                    TargetColor.B = 0;
                                    TargetColor.A = 0;
                                    break;
                                }
                            case 0:
                            default:
                                {
                                    TargetColor.R = ThisVert.color.R;
                                    TargetColor.G = ThisVert.color.G;
                                    TargetColor.B = ThisVert.color.B;
                                    TargetColor.A = ThisVert.color.A;
                                    break;
                                }
                        }

                        seg4w.Write(WriteVertexBinary14(ThisVert));

                    }


                    int LocalFaceCount = CrunchedModel[ThisDraw].Indexes.Count;
                    for (int TargetIndex = 0; TargetIndex < LocalFaceCount; )
                    {



                        if (TargetIndex + 2 <= LocalFaceCount)
                        {
                            //Tri2
                            int[] Indexes = CrunchedModel[ThisDraw].Indexes[TargetIndex];
                            uint[] VertIndexesA = F3D.GetIndexes(Indexes[0], Indexes[2], Indexes[1]);

                            Indexes = CrunchedModel[ThisDraw].Indexes[TargetIndex + 1];
                            uint[] VertIndexesB = F3D.GetIndexes(Indexes[0], Indexes[2], Indexes[1]);

                            seg7w.Write(F3D.gsSP2Triangles(VertIndexesA, VertIndexesB));
                            TargetIndex += 2;
                        }
                        else
                        {
                            //Tri1
                            int[] Indexes = CrunchedModel[ThisDraw].Indexes[TargetIndex];
                            uint[] VertIndexesA = F3D.GetIndexes(Indexes[0], Indexes[2], Indexes[1]);

                            seg7w.Write(F3D.gsSP1Triangle(VertIndexesA));
                            TargetIndex += 1;
                        }
                    }

                    
                    
                }

                seg7w.Write(F3D.gsSPEndDisplayList());

            }

            outseg4 = seg4m.ToArray();
            outseg7 = seg7m.ToArray();

            outMagic = relativeZero;

            return true;
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
                0,
                0,
                0,
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
                0,
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


        public byte[] UntexturedPolygons(OK64Texture TextureObject, bool GeometryToggle = true, bool FogToggle = false)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            //set MIP levels to 0.


            binaryWriter.Write(
                F3D.gsSPTexture(
                    1,
                    1,
                    0,
                    0,
                    0
                )
            );



            //pipe sync.
            binaryWriter.Write(
                F3D.gsDPPipeSync()
            );


            binaryWriter.Write(F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE));

            if (GeometryToggle)
            {

                if (FogToggle)
                {
                    binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.G_CC_PASS2
                        )
                    );
                }
                else

                {
                    binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeB]
                        )
                    );
                }


                binaryWriter.Write(F3D.gsSPClearGeometryMode(F3DEX095_Parameters.AllGeometryModes));    //clear existing modes

                //setup the Geometry Mode parameter
                TextureObject.GeometryModes = 0;
                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    if (TextureObject.GeometryBools[ThisCheck])
                    {
                        TextureObject.GeometryModes |= F3DEX095_Parameters.GeometryModes[ThisCheck];
                    }
                }


                if (FogToggle)
                {
                    TextureObject.GeometryModes |= F3DEX095_Parameters.G_FOG;
                }

                binaryWriter.Write(F3D.gsSPSetGeometryMode(TextureObject.GeometryModes));               //set the mode we made above.

            }


            binaryWriter.Write(F3D.gsSPEndDisplayList());                                             //End the Display List





            return memoryStream.ToArray();

        }

        public byte[] F3DMaterial(OK64Texture TextureObject, UInt32 Segment, bool GeometryToggle = true, bool FogToggle = false)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];
            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));






            //pipe sync.
            binaryWriter.Write(
                F3D.gsDPPipeSync()
            );



            if (GeometryToggle)
            {

                if (FogToggle)
                {
                    binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.G_CC_PASS2
                        )
                    );
                }
                else

                {
                    binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeB]
                        )
                    );
                }

            }
            //set render mode
            if (GeometryToggle)
            {

                if (FogToggle)
                {
                    binaryWriter.Write(
                        F3D.gsDPSetRenderMode(
                            F3DEX095_Parameters.G_RM_FOG_SHADE_A,
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeB]
                        )
                    );
                }
                else

                {
                    binaryWriter.Write(
                        F3D.gsDPSetRenderMode(
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeA],
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeB]
                        )
                    );
                }

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




            if (GeometryToggle)
            {


                binaryWriter.Write(F3D.gsSPClearGeometryMode(F3DEX095_Parameters.AllGeometryModes));    //clear existing modes

                //setup the Geometry Mode parameter
                TextureObject.GeometryModes = 0;
                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    if (TextureObject.GeometryBools[ThisCheck])
                    {
                        TextureObject.GeometryModes |= F3DEX095_Parameters.GeometryModes[ThisCheck];
                    }
                }


                if (FogToggle)
                {
                    TextureObject.GeometryModes |= F3DEX095_Parameters.G_FOG;
                }

                binaryWriter.Write(F3D.gsSPSetGeometryMode(TextureObject.GeometryModes));               //set the mode we made above.
            }


            binaryWriter.Write(F3D.gsSPEndDisplayList());                                             //End the Display List





            return memoryStream.ToArray();

        }


        public byte[] RGBA(OK64Texture TextureObject, UInt32 Segment, bool GeometryToggle = true, bool FogToggle = false, bool Transparent = false)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];
            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));




            byte[] SegmentByte = BitConverter.GetBytes(4);
            Array.Reverse(SegmentByte);
            int SegmentID = BitConverter.ToInt32(SegmentByte, 0);


            //set MIP levels to 0.
            if (TextureObject.TextureFormat != 0)
            {


                binaryWriter.Write(
                    F3D.gsSPTexture(
                        32768,
                        32768,
                        0,
                        0,
                        1
                    )
                );
            }
            else
            {
                binaryWriter.Write(
                    F3D.gsSPTexture(
                        65535,
                        65535,
                        0,
                        0,
                        1
                    )
                );
            }

            //pipe sync.
            binaryWriter.Write(
                F3D.gsDPPipeSync()
            );

            binaryWriter.Write(F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE));

            if (GeometryToggle)
            {

                if (FogToggle)
                {
                    if (Transparent)
                    {
                        //lol what the actual fuck though
                        binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.G_CC_DECALRGBA,
                            F3DEX095_Parameters.G_CC_PASS2
                            )
                        );
                    }
                    else
                    {
                        binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.G_CC_PASS2
                            )
                        );
                    }
                }
                else

                {
                    binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeB]
                        )
                    );
                }

            }



            binaryWriter.Write(
                F3D.gsDPSetTextureFilter(F3DEX095_Parameters.TextureFilters[TextureObject.TextureFilter])
                );


            /*
            if (!TextureObject.AdvancedSettings)
            {
                //set combine mode (simple)
                binaryWriter.Write(
                    F3D.gsNinLoadTextureImage(
                        Convert.ToUInt32(TextureObject.RawTexture.segmentPosition | Convert.ToUInt32(Segment << 24)),
                        F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                        F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                        Convert.ToUInt32(TextureObject.textureWidth),
                        Convert.ToUInt32(TextureObject.textureHeight),
                        0,
                        7
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

                if (FogToggle)
                {
                    binaryWriter.Write(
                        F3D.gsDPSetRenderMode(
                            F3DEX095_Parameters.G_RM_FOG_SHADE_A,
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeB]
                        )
                    );
                }
                else

                {
                    binaryWriter.Write(
                        F3D.gsDPSetRenderMode(
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeA],
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeB]
                        )
                    );
                }

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




            if (GeometryToggle)
            {


                binaryWriter.Write(F3D.gsSPClearGeometryMode(F3DEX095_Parameters.AllGeometryModes));    //clear existing modes

                //setup the Geometry Mode parameter
                TextureObject.GeometryModes = 0;
                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    if (TextureObject.GeometryBools[ThisCheck])
                    {
                        TextureObject.GeometryModes |= F3DEX095_Parameters.GeometryModes[ThisCheck];
                    }
                }


                if (FogToggle)
                {
                    TextureObject.GeometryModes |= F3DEX095_Parameters.G_FOG;
                }

                binaryWriter.Write(F3D.gsSPSetGeometryMode(TextureObject.GeometryModes));               //set the mode we made above.
            }


            //Load Texture Data
            binaryWriter.Write(
                F3D.gsNinLoadTextureImage(
                    Convert.ToUInt32(TextureObject.RawTexture.segmentPosition | Convert.ToUInt32(Segment << 24)),
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


        public byte[] CI(OK64Texture TextureObject, UInt32 Segment, bool GeometryToggle = true, bool FogToggle = false, bool Transparent = false)
        {

            byte[] SegmentByte = BitConverter.GetBytes(Segment);
            Array.Reverse(SegmentByte);
            int SegmentID = BitConverter.ToInt32(SegmentByte, 0);

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];
            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));

            

            binaryWriter.Write(F3D.gsDPSetTextureLUT(F3DSharp.F3DEX095_Parameters.G_TT_RGBA16));
                
            if (TextureObject.BitSize < 1)
            {
                //Macro 4-bit Texture Load

                binaryWriter.Write(F3D.gsDPLoadTLUT_pal16(0, Convert.ToUInt32(TextureObject.RawTexture.palettePosition | SegmentID)));
                binaryWriter.Write(F3D.gsDPLoadTextureBlock_4b(Convert.ToUInt32(TextureObject.RawTexture.segmentPosition | SegmentID),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat], Convert.ToUInt32(TextureObject.textureWidth), Convert.ToUInt32(TextureObject.textureHeight),
                    0, F3DEX095_Parameters.TextureModes[TextureObject.SFlag], widthex, 0, F3DEX095_Parameters.TextureModes[TextureObject.TFlag], heightex, 0));
            }
            else
            {

                binaryWriter.Write(F3D.gsDPLoadTLUT_pal256(0, Convert.ToUInt32(TextureObject.RawTexture.palettePosition | SegmentID)));

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
                binaryWriter.Write(F3D.gsDPLoadTextureBlock(
                    Convert.ToUInt32(TextureObject.RawTexture.segmentPosition | SegmentID),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                    Convert.ToUInt32(TextureObject.textureWidth),
                    Convert.ToUInt32(TextureObject.textureHeight),
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.SFlag],
                    widthex,
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.TFlag],
                    heightex,
                    0));


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



            //set render mode
            if (GeometryToggle)
            {

                if (FogToggle)
                {
                    binaryWriter.Write(
                        F3D.gsDPSetRenderMode(
                            F3DEX095_Parameters.G_RM_FOG_SHADE_A,
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeB]
                        )
                    );
                }
                else

                {
                    binaryWriter.Write(
                        F3D.gsDPSetRenderMode(
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeA],
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeB]
                        )
                    );
                }

            }



            if (GeometryToggle)
            {

                if (FogToggle)
                {
                    if (Transparent)
                    {
                        //lol what the actual fuck though
                        binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.G_CC_DECALRGBA,
                            F3DEX095_Parameters.G_CC_PASS2
                            )
                        );
                    }
                    else
                    {
                        binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.G_CC_PASS2
                            )
                        );
                    }
                }
                else
                {
                    binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeB]
                        )
                    );
                }

            }

            binaryWriter.Write(
                F3D.gsDPTileSync()
                );


            binaryWriter.Write(
                F3D.gsDPSetTextureFilter(F3DEX095_Parameters.TextureFilters[TextureObject.TextureFilter])
                );



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





            if (GeometryToggle)
            {


                binaryWriter.Write(F3D.gsSPClearGeometryMode(F3DEX095_Parameters.AllGeometryModes));    //clear existing modes

                //setup the Geometry Mode parameter
                TextureObject.GeometryModes = 0;
                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    if (TextureObject.GeometryBools[ThisCheck])
                    {
                        TextureObject.GeometryModes |= F3DEX095_Parameters.GeometryModes[ThisCheck];
                    }
                }


                if (FogToggle)
                {
                    TextureObject.GeometryModes |= F3DEX095_Parameters.G_FOG;
                }

                binaryWriter.Write(F3D.gsSPSetGeometryMode(TextureObject.GeometryModes));               //set the mode we made above.
            }


            binaryWriter.Write(F3D.gsSPEndDisplayList());                                             //End the Display List





            return memoryStream.ToArray();

        }


        public byte[] IA(OK64Texture TextureObject, UInt32 Segment, bool GeometryToggle = true, bool FogToggle = false, bool Transparent = false)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];
            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));

            byte[] SegmentByte = BitConverter.GetBytes(Segment);
            Array.Reverse(SegmentByte);
            int SegmentID = BitConverter.ToInt32(SegmentByte, 0);

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


            binaryWriter.Write(F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE));
            if (TextureObject.BitSize < 1)
            {
                //Macro 4-bit Texture Load
                binaryWriter.Write(F3D.gsDPLoadTextureBlock_4b(Convert.ToUInt32(TextureObject.RawTexture.segmentPosition | SegmentID),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat], Convert.ToUInt32(TextureObject.textureWidth), Convert.ToUInt32(TextureObject.textureHeight),
                    0, F3DEX095_Parameters.TextureModes[TextureObject.SFlag], widthex, 0, F3DEX095_Parameters.TextureModes[TextureObject.TFlag], heightex, 0));
            }
            else
            {
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
                binaryWriter.Write(F3D.gsDPLoadTextureBlock(
                    Convert.ToUInt32(TextureObject.RawTexture.segmentPosition | SegmentID),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                    Convert.ToUInt32(TextureObject.textureWidth),
                    Convert.ToUInt32(TextureObject.textureHeight),
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.SFlag],
                    widthex,
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.TFlag],
                    heightex,
                    0));


            }



            if (GeometryToggle)
            {

                if (FogToggle)
                {
                    if (Transparent)
                    {
                        //lol what the actual fuck though
                        binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.G_CC_DECALRGBA,
                            F3DEX095_Parameters.G_CC_PASS2
                            )
                        );
                    }
                    else
                    {
                        binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.G_CC_PASS2
                            )
                        );
                    }
                    
                }
                else

                {
                    binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeB]
                        )
                    );
                }

            }




            binaryWriter.Write(
                F3D.gsDPSetTextureFilter(F3DEX095_Parameters.TextureFilters[TextureObject.TextureFilter])
                );




            //binaryWriter.Write(F3D.gsDPSetT)


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

                if (FogToggle)
                {
                    binaryWriter.Write(
                        F3D.gsDPSetRenderMode(
                            F3DEX095_Parameters.G_RM_FOG_SHADE_A,
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeB]
                        )
                    );
                }
                else

                {
                    binaryWriter.Write(
                        F3D.gsDPSetRenderMode(
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeA],
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeB]
                        )
                    );
                }

            }
            

            if (GeometryToggle)
            {


                binaryWriter.Write(F3D.gsSPClearGeometryMode(F3DEX095_Parameters.AllGeometryModes));    //clear existing modes

                //setup the Geometry Mode parameter
                TextureObject.GeometryModes = 0;
                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    if (TextureObject.GeometryBools[ThisCheck])
                    {
                        TextureObject.GeometryModes |= F3DEX095_Parameters.GeometryModes[ThisCheck];
                    }
                }


                if (FogToggle)
                {
                    TextureObject.GeometryModes |= F3DEX095_Parameters.G_FOG;
                }

                binaryWriter.Write(F3D.gsSPSetGeometryMode(TextureObject.GeometryModes));               //set the mode we made above.
            }

            
            binaryWriter.Write(F3D.gsSPEndDisplayList());                                             //End the Display List





            return memoryStream.ToArray();

        }


        public byte[] compileCourseTexture(byte[] SegmentData, OK64Texture[] textureObject, int vertMagic, int SegmentID = 5, bool FogToggle = false)
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


            /*
                #define G_IM_FMT_RGBA	0
                #define G_IM_FMT_YUV	1
                #define G_IM_FMT_CI	    2
                #define G_IM_FMT_IA	    3
                #define G_IM_FMT_I	    4
            */
            List<int> SkippedMaterials = new List<int>();
            for (int materialID = 0; materialID < textureObject.Length; materialID++)
            {
                foreach (var Index in textureObject[materialID].TextureOverWrite)
                {
                    SkippedMaterials.Add(Index);
                }
            }
            for (int materialID = 0; materialID < textureObject.Length; materialID++)
            {
                if (!SkippedMaterials.Contains(materialID))
                {
                    if ((textureObject[materialID].texturePath != null) && (textureObject[materialID].texturePath != "NULL"))
                    {
                        //Textured Polygons (Slow)
                        textureObject[materialID].RawTexture.f3dexPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;
                        switch (textureObject[materialID].TextureFormat)
                        {

                            case 0:
                            default:
                                {
                                    seg7w.Write(RGBA(textureObject[materialID], Convert.ToUInt32(SegmentID), true, FogToggle, Transparent));
                                    break;
                                }
                            case 2:
                                {
                                    seg7w.Write(CI(textureObject[materialID], Convert.ToUInt32(SegmentID), true, FogToggle, Transparent));
                                    break;
                                }
                            case 3:
                            case 4:
                                {
                                    seg7w.Write(IA(textureObject[materialID], Convert.ToUInt32(SegmentID), true, FogToggle, Transparent));
                                    break;
                                }
                            case 1:
                                {
                                    MessageBox.Show("ERROR - " + textureObject[materialID].textureName + " - YUV Format not supported.");
                                    break;
                                }
                        }


                    }
                    else
                    {
                        // Gouraud or Flat Shading (Fast)
                        textureObject[materialID].RawTexture.f3dexPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;
                        seg7w.Write(UntexturedPolygons(textureObject[materialID], true, FogToggle));

                    }
                }
                else
                {
                    //SkippedMaterial - No Texutre Load Commands
                    textureObject[materialID].RawTexture.f3dexPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;
                    seg7w.Write(F3DMaterial(textureObject[materialID], Convert.ToUInt32(SegmentID), true, FogToggle));
                }
            }
            return seg7m.ToArray();
        }



        public byte[] CompileTextureObjects(byte[] SegmentData, OK64Texture[] textureObject, int vertMagic, int SegmentID = 5, bool GeometryMode = true, bool FogToggle = false)
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


            int addressAlign = 16 - (Convert.ToInt32(seg7w.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;


            for (int align = 0; align < addressAlign; align++)
            {
                seg7w.Write(Convert.ToByte(0x00));
            }


            for (int materialID = 0; materialID < textureObject.Length; materialID++)
            {
                if ((textureObject[materialID].texturePath != null) && (textureObject[materialID].texturePath != "NULL"))
                {
                    //Textured Polygons (Slow)
                    textureObject[materialID].RawTexture.f3dexPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;
                    switch (textureObject[materialID].TextureFormat)
                    {

                        case 0:
                        default:
                            {
                                seg7w.Write(RGBA(textureObject[materialID], Convert.ToUInt32(SegmentID), GeometryMode, FogToggle, Transparent));
                                break;
                            }
                        case 2:
                            {
                                seg7w.Write(CI(textureObject[materialID], Convert.ToUInt32(SegmentID), GeometryMode, FogToggle, Transparent));
                                break;
                            }
                        case 3:
                        case 4:
                            {
                                seg7w.Write(IA(textureObject[materialID], Convert.ToUInt32(SegmentID), GeometryMode, FogToggle, Transparent));
                                break;
                            }
                        case 1:
                            {
                                MessageBox.Show("ERROR - " + textureObject[materialID].textureName + " - YUV Format not supported.");
                                break;
                            }
                    }


                }
                else
                {
                    // Gouraud or Flat Shading (Fast)
                    textureObject[materialID].RawTexture.f3dexPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;
                    seg7w.Write(UntexturedPolygons(textureObject[materialID], GeometryMode, FogToggle));

                }
            }
            return seg7m.ToArray();
        }

        public byte[] CompileObjectList(byte[] OutputData, OK64F3DObject courseObject, OK64Texture[] textureObject, int SegmentID)
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
                
                if (courseObject.materialID == currentTexture)
                {
                    if (!textureWritten)
                    {
                        byteArray = BitConverter.GetBytes(0x06000000);
                        Array.Reverse(byteArray);
                        seg6w.Write(byteArray);

                            byteArray = BitConverter.GetBytes(textureObject[currentTexture].RawTexture.f3dexPosition | (SegmentID << 24));
                            Array.Reverse(byteArray);
                            seg6w.Write(byteArray);

                        textureWritten = true;
                    }

                    for (int subObject = 0; subObject < courseObject.meshPosition.Length; subObject++)
                    {
                        byteArray = BitConverter.GetBytes(0x06000000);
                        Array.Reverse(byteArray);
                        seg6w.Write(byteArray);

                        byteArray = BitConverter.GetBytes(courseObject.meshPosition[subObject] | (SegmentID << 24));
                        Array.Reverse(byteArray);
                        seg6w.Write(byteArray);
                    }


                    byteArray = BitConverter.GetBytes(0xB8000000);
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(0x00000000);
                    Array.Reverse(byteArray);
                    seg6w.Write(byteArray);

                }
                if (textureWritten && (textureObject[currentTexture].RawTexture.paletteSize > 0))
                {
                    seg6w.Write(F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE));
                    seg6w.Write(
                        F3D.gsSPTexture(
                            65535,
                            65535,
                            0,
                            0,
                            1
                        )
                    );
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


        public byte[] CompileF3DList(ref OK64SectionList[] sectionOut, OK64F3DObject[] courseObject, OK64SectionList[] sectionList, OK64Texture[] textureObject)
        {
            //this function will create display lists for each of the section views based on the OK64F3DObject array.
            //this array had been previously written to segment 7 and the offsets to each of those objects' meshes...
            // were stored into courseObject[index].meshPosition[] for this process.


            //magic is the offset of the data preceding this in the segment based on the current organization method,

            
            MemoryStream seg6m = new MemoryStream();
            BinaryReader seg6r = new BinaryReader(seg6m);
            BinaryWriter seg6w = new BinaryWriter(seg6m);

            byte[] byteArray = new byte[0];

            List<int> SkippedMaterials = new List<int>();
            for (int currentTexture = 0; currentTexture < textureObject.Length; currentTexture++)
            {
                foreach (var Index in textureObject[currentTexture].TextureOverWrite)
                {
                    SkippedMaterials.Add(Index);
                }
            }
            for (int currentSection = 0; currentSection < sectionList.Length; currentSection++)
            {
                for (int currentView = 0; currentView < 1; currentView++)
                {

                    int objectCount = sectionList[currentSection].objectList.Length;
                    sectionList[currentSection].segmentPosition = Convert.ToInt32(seg6m.Position);



                    //opaque 
                    bool textureWritten = false;

                    for (int ThisZSort = 0; ThisZSort < 5; ThisZSort++)
                    {
                        for (int currentTexture = 0; currentTexture < textureObject.Length; currentTexture++)
                        {
                            textureWritten = false;
                            if (!SkippedMaterials.Contains(currentTexture))
                            {


                                if (ThisZSort == ZSort(textureObject[currentTexture]))
                                {
                                    for (int currentObject = 0; currentObject < objectCount; currentObject++)
                                    {

                                        int objectIndex = sectionList[currentSection].objectList[currentObject];
                                        if (courseObject[objectIndex].materialID == currentTexture)
                                        {
                                            if (!textureWritten)
                                            {
                                                byteArray = BitConverter.GetBytes(0x06000000);
                                                Array.Reverse(byteArray);
                                                seg6w.Write(byteArray);

                                                byteArray = BitConverter.GetBytes(textureObject[currentTexture].RawTexture.f3dexPosition | 0x06000000);
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


                                if (textureWritten && (textureObject[currentTexture].RawTexture.paletteSize > 0))
                                {
                                    seg6w.Write(F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE));
                                    //set MIP levels to 0.
                                    seg6w.Write(
                                        F3D.gsSPTexture(
                                            65535,
                                            65535,
                                            0,
                                            0,
                                            1
                                        )
                                    );
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



        public byte[] CompileXLUList(ref OK64SectionList[] sectionOut, OK64F3DObject[] courseObject, OK64SectionList[] sectionList, OK64Texture[] textureObject)
        {
            //this function will create display lists for each of the section views based on the OK64F3DObject array.
            //this array had been previously written to segment 7 and the offsets to each of those objects' meshes...
            // were stored into courseObject[index].meshPosition[] for this process.


            //magic is the offset of the data preceding this in the segment based on the current organization method,


            MemoryStream seg6m = new MemoryStream();
            BinaryReader seg6r = new BinaryReader(seg6m);
            BinaryWriter seg6w = new BinaryWriter(seg6m);

            byte[] byteArray = new byte[0];


            /*
                                    */

            List<int> SkippedMaterials = new List<int>();
            for (int currentTexture = 0; currentTexture < textureObject.Length; currentTexture++)
            {
                if (textureObject[currentTexture].TextureOverWrite.Length > 0)
                {
                    SkippedMaterials.Add(currentTexture);
                }
                    
                foreach (var Index in textureObject[currentTexture].TextureOverWrite)
                {
                    SkippedMaterials.Add(Index);
                }
            }
            for (int currentSection = 0; currentSection < sectionList.Length; currentSection++)
            {
                for (int currentView = 0; currentView < 4; currentView++)
                {

                    int objectCount = sectionList[currentSection].objectList.Length;
                    sectionList[currentSection].segmentPosition = Convert.ToInt32(seg6m.Position);


                    for (int currentTexture = 0; currentTexture < textureObject.Length; currentTexture++)
                    {
                        bool ParentOverwrite = false;
                        for (int ThisOverWrite = 0; ThisOverWrite < textureObject[currentTexture].TextureOverWrite.Length; ThisOverWrite++)
                        {
                            bool ChildOverwrite = false;
                            int TargetTexture = textureObject[currentTexture].TextureOverWrite[ThisOverWrite];
                            for (int currentObject = 0; currentObject < objectCount; currentObject++)
                            {

                                int objectIndex = sectionList[currentSection].objectList[currentObject];
                                if (courseObject[objectIndex].materialID == TargetTexture)
                                {
                                    if (!ChildOverwrite)
                                    {
                                        byteArray = BitConverter.GetBytes(0x06000000);
                                        Array.Reverse(byteArray);
                                        seg6w.Write(byteArray);

                                        byteArray = BitConverter.GetBytes(textureObject[TargetTexture].RawTexture.f3dexPosition | 0x06000000);
                                        Array.Reverse(byteArray);
                                        seg6w.Write(byteArray);

                                        ChildOverwrite = true;
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


                    //opaque 
                    bool textureWritten = false;

                    for (int ThisZSort = 5; ThisZSort < 8; ThisZSort++)
                    {
                        for (int currentTexture = 0; currentTexture < textureObject.Length; currentTexture++)
                        {
                            if (!SkippedMaterials.Contains(currentTexture))
                            {
                                textureWritten = false;
                                if (ThisZSort == ZSort(textureObject[currentTexture]))
                                {


                                    for (int currentObject = 0; currentObject < objectCount; currentObject++)
                                    {

                                        int objectIndex = sectionList[currentSection].objectList[currentObject];
                                        if (courseObject[objectIndex].materialID == currentTexture)
                                        {
                                            if (!textureWritten)
                                            {
                                                byteArray = BitConverter.GetBytes(0x06000000);
                                                Array.Reverse(byteArray);
                                                seg6w.Write(byteArray);

                                                byteArray = BitConverter.GetBytes(textureObject[currentTexture].RawTexture.f3dexPosition | 0x06000000);
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

                                    if (textureWritten && (textureObject[currentTexture].RawTexture.paletteSize > 0))
                                    {
                                        seg6w.Write(F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE));
                                        seg6w.Write(
                                            F3D.gsSPTexture(
                                                65535,
                                                65535,
                                                0,
                                                0,
                                                1
                                            )
                                        );
                                    }
                                }
                            }
                        }
                    }

                    seg6w.Write(F3D.gsSPEndDisplayList());

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

                                    byteArray = BitConverter.GetBytes(textureObject[currentTexture].RawTexture.f3dexPosition | 0x06000000);
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

                                    byteArray = BitConverter.GetBytes(textureObject[currentTexture].RawTexture.f3dexPosition | 0x06000000);
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
                        if (textureWritten && (textureObject[currentTexture].RawTexture.paletteSize > 0))
                        {
                            seg6w.Write(F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE));
                            seg6w.Write(
                                F3D.gsSPTexture(
                                    65535,
                                    65535,
                                    0,
                                    0,
                                    1
                                )
                            );
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

        public byte[] CompileSurfaceTable(OK64F3DObject[] surfaceObject)
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
                    {
                        MessageBox.Show("FATAL ERROR! Object with more than 1 Material: " + surfaceObject[currentObject].objectName);
                    }
                        


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

        public byte[] CompilesectionviewTable(OK64SectionList[] sectionList, int magic)
        {
            MemoryStream seg6m = new MemoryStream();
            BinaryReader seg6r = new BinaryReader(seg6m);
            BinaryWriter seg6w = new BinaryWriter(seg6m);

            byte[] byteArray = new byte[0];



            for (int currentSection = 0; currentSection < sectionList.Length; currentSection++)
            {
                for (int currentView = 0; currentView < 4; currentView++)
                {
                    byteArray = BitConverter.GetBytes(0x06000000 | (sectionList[currentSection].segmentPosition + magic));
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

        


        public int GetModelFormat(Assimp.Scene fbx)
        {
            int modelFormat = -1;
            Assimp.Node masterNode = fbx.RootNode.FindNode("Section 1");
            if (masterNode != null)
            {
                modelFormat = 0;  //normal mode
            }
            else
            {
                modelFormat = 1; //OBJ mode
            }
            return modelFormat;
        }

        public int GetSectionCount(Assimp.Scene fbx)
        {
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

        public Matrix4x4 GetTotalTransform(Node Base, Assimp.Scene FBX)
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



        public OK64Bone LoadBone(Node Base, Assimp.Scene FBX, float ModelScale)
        {
            OK64Bone NewBone = new OK64Bone();
            NewBone.Name = Base.Name;
            NewBone.Children = new OK64Bone[Base.ChildCount];

            Matrix4x4 OPrime = GetTotalTransform(Base, FBX);
            NewBone.Origin = new short[3];
            NewBone.Origin[0] = Convert.ToInt16(OPrime.A4 * 100 * ModelScale);
            NewBone.Origin[1] = Convert.ToInt16(OPrime.B4 * 100 * ModelScale);
            NewBone.Origin[2] = Convert.ToInt16(OPrime.C4 * 100 * ModelScale);

            //Base.Transform.
            for (int ThisChild = 0; ThisChild < Base.ChildCount; ThisChild++)
            {
                NewBone.Children[ThisChild] = LoadBone(Base.Children[ThisChild], FBX, ModelScale);
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

            Skeleton.MeshListOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position + Magic);
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
                    flip2 = BitConverter.GetBytes(SaveObject.TextureData[SaveObject.ModelData[ThisObject].materialID].RawTexture.f3dexPosition);
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

        public byte[] WriteAnimationData(OK64Bone Skeleton, UInt32 Magic)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] flip2 = new byte[2];
            List<byte> AnimationData = new List<byte>();


            binaryWriter.Write(F3D.BigEndian(Skeleton.Origin[0]));
            binaryWriter.Write(F3D.BigEndian(Skeleton.Origin[2]));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Origin[1] * -1)));
            binaryWriter.Write(Convert.ToInt16(0));

            for (int ThisFrame = 0; ThisFrame < Skeleton.Animation.RotationData.Length; ThisFrame++)
            {
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.RotationData[ThisFrame][0])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.RotationData[ThisFrame][2])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.RotationData[ThisFrame][1])));
            }
            if (Skeleton.FrameCount % 2 == 1)
            {
                binaryWriter.Write(Convert.ToInt16(0));
            }


            for (int ThisFrame = 0; ThisFrame < Skeleton.Animation.TranslationData.Length; ThisFrame++)
            {
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.TranslationData[ThisFrame][0])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.TranslationData[ThisFrame][2])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.TranslationData[ThisFrame][1] * -1)));
            }
            if (Skeleton.FrameCount % 2 == 1)
            {
                binaryWriter.Write(Convert.ToInt16(0));
            }


            for (int ThisFrame = 0; ThisFrame < Skeleton.Animation.ScalingData.Length; ThisFrame++)
            {
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.ScalingData[ThisFrame][0])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.ScalingData[ThisFrame][2])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.ScalingData[ThisFrame][1])));
            }
            if (Skeleton.FrameCount % 2 == 1)
            {
                binaryWriter.Write(Convert.ToInt16(0));
            }


            foreach (var ChildBone in Skeleton.Children)
            {
                ChildBone.TranslationOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position + Skeleton.TranslationOffset);
                binaryWriter.Write(WriteAnimationData(ChildBone, ChildBone.TranslationOffset));
            }
            return memoryStream.ToArray();
        }

        public byte[] BuildAnimationData(OK64Bone Skeleton, UInt32 Magic)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] flip2 = new byte[2];
            List<byte> AnimationData = new List<byte>();

            Skeleton.TranslationOffset = Magic;

            binaryWriter.Write(WriteAnimationData(Skeleton, Magic));

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


        class CrunchF3DVertCache:IComparable<CrunchF3DVertCache>
        {
            public int UniqueUsage { get; set; }
            public Vertex VertexData { get; set; }
            public int OriginalIndex { get; set; }

            public int CompareTo(CrunchF3DVertCache other)
            {
                return UniqueUsage.CompareTo(other.UniqueUsage);
            }
        }


        private void UpdateMatchCount(List<int[]> IndexArray, List<CrunchF3DVertCache> VCache)
        {
            for (int CacheVert = 0; CacheVert < VCache.Count; CacheVert++)
            {
                VCache[CacheVert].UniqueUsage = 0;
                for (int ThisFace = 0; ThisFace < IndexArray.Count; ThisFace++)
                {
                    for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                    {

                        if (VCache[CacheVert].OriginalIndex == IndexArray[ThisFace][ThisVert])
                        {
                            VCache[CacheVert].UniqueUsage++;
                        }

                    }
                }

            }

            
        }
        public OK64F3DModel[] CrunchF3DModel(OK64F3DObject TargetObject)
        {
            List<OK64F3DModel> OutputModel = new List<OK64F3DModel>();
            List<int[]> IndexArray = new List<int[]>();
            List<CrunchF3DVertCache> VCache = new List<CrunchF3DVertCache>();
            
            for (int ThisFace = 0; ThisFace < TargetObject.faceCount; ThisFace++)
            {
                IndexArray.Add(new int[3]);
                if (TargetObject.modelGeometry[ThisFace].VertData.Length != 3)
                {
                    MessageBox.Show("Index count error with object " + TargetObject.objectName + "- wrong vert count");
                    return new OK64F3DModel[0];
                }

                for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                {
                    var TargetVert = TargetObject.modelGeometry[ThisFace].VertData[ThisVert];
                    int MatchingIndex = VCache.FindIndex(
                        x => x.VertexData.position.x == TargetVert.position.x &&
                        x.VertexData.position.y == TargetVert.position.y &&
                        x.VertexData.position.z == TargetVert.position.z &&

                        x.VertexData.position.sBase == TargetVert.position.sBase &&
                        x.VertexData.position.tBase == TargetVert.position.tBase &&

                        x.VertexData.color.R == TargetVert.color.R &&
                        x.VertexData.color.G == TargetVert.color.G &&
                        x.VertexData.color.B == TargetVert.color.B &&
                        x.VertexData.color.A == TargetVert.color.A

                        );
                    if (MatchingIndex == -1)
                    {
                        CrunchF3DVertCache NewVCache = new CrunchF3DVertCache();
                        NewVCache.VertexData = TargetVert;
                        NewVCache.UniqueUsage = 1;

                        NewVCache.OriginalIndex = VCache.Count;
                        IndexArray[ThisFace][ThisVert] = VCache.Count;

                        VCache.Add(NewVCache);


                    }
                    else
                    {
                        VCache[MatchingIndex].UniqueUsage++;
                        IndexArray[ThisFace][ThisVert] = MatchingIndex;
                    }
                }

            }




            VCache.Sort();
            VCache.Reverse();

            while (IndexArray.Count > 0)
            {


                int LocalCacheSize = Math.Min(32, VCache.Count);

                OK64F3DModel NewF3DCall = new OK64F3DModel();

                NewF3DCall.VertexCache = new List<Vertex>();
                NewF3DCall.Indexes = new List<int[]>();

                List<int> KeyList = new List<int>();

                int TargetVertIndex = 0;
                NewF3DCall.VertexCache.Add(VCache[TargetVertIndex].VertexData);
                KeyList.Add(VCache[TargetVertIndex].OriginalIndex);


                while (NewF3DCall.VertexCache.Count < LocalCacheSize)
                {
                    int StartCount = NewF3DCall.VertexCache.Count;
                    for (int ThisFace = 0; ThisFace < IndexArray.Count; ThisFace++)
                    {
                        for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                        {
                            if (KeyList.Contains(IndexArray[ThisFace][ThisVert]))
                            {
                                for (int SubVert = 0; SubVert < 3; SubVert++)
                                {
                                    if ((!KeyList.Contains(IndexArray[ThisFace][SubVert])) && (NewF3DCall.VertexCache.Count < LocalCacheSize))
                                    {
                                        KeyList.Add(IndexArray[ThisFace][SubVert]);

                                        int MatchingIndex = VCache.FindIndex(
                                            x => x.OriginalIndex == IndexArray[ThisFace][SubVert]
                                        );

                                        NewF3DCall.VertexCache.Add(VCache[MatchingIndex].VertexData);
                                    }
                                }
                                break;
                            }
                        }
                        if (
                                KeyList.Contains(IndexArray[ThisFace][0]) &&
                                KeyList.Contains(IndexArray[ThisFace][1]) &&
                                KeyList.Contains(IndexArray[ThisFace][2])
                        )
                        {
                            NewF3DCall.Indexes.Add(new int[3]
                            {
                            KeyList.IndexOf(IndexArray[ThisFace][0]),
                            KeyList.IndexOf(IndexArray[ThisFace][1]),
                            KeyList.IndexOf(IndexArray[ThisFace][2])
                            });

                            IndexArray.RemoveAt(ThisFace);
                            ThisFace--;  //deal with removal of current index
                        }
                    }
                    if ((StartCount == NewF3DCall.VertexCache.Count) && (StartCount < LocalCacheSize))
                    {
                        if (IndexArray.Count == 0)
                        {
                            //finished 
                            LocalCacheSize = 0;
                        }
                        UpdateMatchCount(IndexArray, VCache);
                        VCache.Sort();
                        VCache.Reverse();

                        NewF3DCall.VertexCache.Add(VCache[0].VertexData);
                        KeyList.Add(VCache[0].OriginalIndex);
                    }
                }   



                OutputModel.Add(NewF3DCall);


                UpdateMatchCount(IndexArray, VCache);
                VCache.Sort();
                VCache.Reverse();

            }


            return OutputModel.ToArray();


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
                        NewAnime.TranslationData[ThisFrame][ThisVector] = Convert.ToInt16(AnimeChannel.PositionKeys[ThisFrame].Value[ThisVector] * 10);
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
            NewAnime.RotationFloat = new float[FrameCount][];
            for (int ThisFrame = 0; ThisFrame < FrameCount; ThisFrame++)
            {

                if (ThisFrame < AnimeChannel.RotationKeyCount)
                {
                    NewAnime.RotationData[ThisFrame] = new short[3];
                    NewAnime.RotationFloat[ThisFrame] = new float[3];


                    float[] RotationTemp = ConvertEuler(AnimeChannel.RotationKeys[ThisFrame].Value);
                    
                    for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                    {
                        NewAnime.RotationFloat[ThisFrame][ThisVector] = Convert.ToSingle(RotationTemp[ThisVector] / 0.01745329252);
                        if (Math.Abs(NewAnime.RotationFloat[ThisFrame][ThisVector]) < 0.01f)
                        {
                            NewAnime.RotationFloat[ThisFrame][ThisVector] = 0f;
                        }
                        NewAnime.RotationData[ThisFrame][ThisVector] = Convert.ToInt16(NewAnime.RotationFloat[ThisFrame][ThisVector]);
                    }
                }
                else
                {
                    NewAnime.RotationData[ThisFrame] = new short[3];
                    NewAnime.RotationFloat[ThisFrame] = new float[3];

                    for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                    {
                        NewAnime.RotationFloat[ThisFrame][ThisVector] = NewAnime.RotationFloat[ThisFrame - 1][ThisVector];
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


        public OK64Bone TransformBone(OK64Bone Bone, OK64Bone Parent, int FrameCount, float ModelScale)
        {
            


            for (int ThisFrame = 0; ThisFrame < FrameCount; ThisFrame++)
            {
                Point3D Root = new Point3D()
                {
                    X = (Bone.Animation.TranslationData[ThisFrame][0]),
                    Y = (Bone.Animation.TranslationData[ThisFrame][1]),
                    Z = (Bone.Animation.TranslationData[ThisFrame][2])
                };
                float[] Angle = new float[3]{
                    Convert.ToSingle(Parent.Animation.RotationFloat[ThisFrame][0]),
                    Convert.ToSingle(Parent.Animation.RotationFloat[ThisFrame][1]),
                    Convert.ToSingle(Parent.Animation.RotationFloat[ThisFrame][2]),
                };
                Point3D Branch = RotatePoint(Root, Angle);

                Bone.Animation.TranslationData[ThisFrame][0] = Convert.ToInt16(Parent.Animation.TranslationData[ThisFrame][0] + Branch.X);
                Bone.Animation.TranslationData[ThisFrame][1] = Convert.ToInt16(Parent.Animation.TranslationData[ThisFrame][1] + Branch.Y);
                Bone.Animation.TranslationData[ThisFrame][2] = Convert.ToInt16(Parent.Animation.TranslationData[ThisFrame][2] + Branch.Z);

                Bone.Animation.RotationData[ThisFrame][0] += Parent.Animation.RotationData[ThisFrame][0];
                Bone.Animation.RotationData[ThisFrame][1] += Parent.Animation.RotationData[ThisFrame][1];
                Bone.Animation.RotationData[ThisFrame][2] += Parent.Animation.RotationData[ThisFrame][2];


            }

            foreach (var Child in Bone.Children)
            {
                TransformBone(Child, Bone, FrameCount, ModelScale);
            }

            return Bone;
        }
        public OK64Bone GetTransforms(OK64Bone Skeleton, int FrameCount, float ModelScale)
        {
            foreach (var Child in Skeleton.Children)
            {
                TransformBone(Child, Skeleton, FrameCount, ModelScale);
            }

            return Skeleton;
        }

        public OK64Bone ParseAnimation(Assimp.Scene FBX, NodeAnimationChannel AnimeChannel, OK64Bone Bone, int FrameCount)
        {

            if (Bone.Name == AnimeChannel.NodeName)
            {
                Bone.Animation = LoadAnimation(AnimeChannel, Bone, FrameCount);
                Bone.FrameCount = FrameCount;
            }
            foreach (var Child in Bone.Children)
            {
                ParseAnimation(FBX, AnimeChannel, Child, FrameCount);
            }
            return Bone;
        }
        public OK64Bone LoadSkeleton (Assimp.Scene FBX, float ModelScale)
        {
            
            Node Base = FBX.RootNode.FindNode("BodyBone");
            OK64Bone Skeleton = LoadBone(Base, FBX, ModelScale);

            Animation Anime = FBX.Animations[0];
            Skeleton.FrameCount = Convert.ToInt32(Anime.DurationInTicks + 1);
            for (int ThisNode = 0; ThisNode < Anime.NodeAnimationChannelCount; ThisNode++)
            {
                ParseAnimation(FBX, Anime.NodeAnimationChannels[ThisNode], Skeleton, Skeleton.FrameCount);
            }
            GetTransforms(Skeleton, Skeleton.FrameCount, ModelScale);
            return Skeleton;
        }



        public string GetRGBAString(TM64_Geometry.OK64Texture TextureObject, int Height, int Width)
        {
            int R, G, B, A;
            int ThisPixel = (Height * TextureObject.textureWidth) + Width;
            return "0x" + TextureObject.RawTexture.TextureData[ThisPixel * 2].ToString("X").PadLeft(2, '0') + TextureObject.RawTexture.TextureData[1 + (ThisPixel * 2)].ToString("X").PadLeft(2, '0') + ", ";
        }


        public string[] WriteVertDataC(TM64_Geometry.OK64F3DObject F3DObject, TM64_Geometry.OK64Texture TextureObject)
        {
            List<string> Output = new List<string>();

            Output.Add("Vtx " + F3DObject.objectName + "_V[] = {");
            OK64F3DModel[] CrunchedModel = CrunchF3DModel(F3DObject);

            /*{ { 15,30,0}, 0, 	{2048, 	   0},  {255, 255, 254, 255} }*/
            foreach (var Crunch in CrunchedModel)
            {
                foreach (var Vert in Crunch.VertexCache)
                {
                    Vert.position.s = Convert.ToInt16(Vert.position.sBase * TextureObject.textureWidth);
                    Vert.position.t = Convert.ToInt16(Vert.position.tBase * TextureObject.textureHeight);

                    Output.Add(
                        "{ {  { " + Vert.position.x.ToString() + ", " + Vert.position.z.ToString() + ", " + (-1 * Vert.position.y).ToString() +
                        " }, 0,  {" + Vert.position.s.ToString() + ", " + Vert.position.t.ToString() + "}, " +
                        "{" + Vert.color.R.ToString() + ", " + Vert.color.G.ToString() + ", " + Vert.color.B.ToString() + ", " + Vert.color.A.ToString() + "} } },"
                        );
                }
            }

            Output.Add("};");

            return Output.ToArray();
        }
        public List<string> WriteTextureC(TM64_Geometry.OK64Texture TextureObject)
        {
            List<string> TextureData = new List<string>();

            if ((TextureObject.texturePath != null) && (TextureObject.texturePath != "NULL"))
            {
                TextureData.Add("unsigned short " + TextureObject.textureName + "[] = {");

                MemoryStream memoryStream = new MemoryStream(TextureObject.RawTexture.TextureData);
                BinaryReader binaryReader = new BinaryReader(memoryStream);
                binaryReader.BaseStream.Position = 0;
                while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                {
                    string RowString = "";
                    
                    for (int ThisRow = 0; ThisRow < 8; ThisRow++)
                    {
                        RowString += "0x"+BitConverter.ToString(binaryReader.ReadBytes(2)).Replace("-","")+ ", ";
                    }
                    TextureData.Add(RowString);
                }
                TextureData.Add("};");
                TextureData.Add("");

                if (TextureObject.RawTexture.PaletteData != null)
                {
                    TextureData.Add("unsigned short " + TextureObject.textureName + "_PAL[] = {");

                    memoryStream = new MemoryStream(TextureObject.RawTexture.PaletteData);
                    binaryReader = new BinaryReader(memoryStream);
                    binaryReader.BaseStream.Position = 0;
                    while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                    {
                        string RowString = "";

                        for (int ThisRow = 0; ThisRow < 8; ThisRow++)
                        {
                            RowString += "0x" + BitConverter.ToString(binaryReader.ReadBytes(2)).Replace("-", "") + ", ";
                        }
                        TextureData.Add(RowString);
                    }
                    TextureData.Add("};");
                    TextureData.Add("");


                }
            }
            return TextureData;
        }


        public string[] WriteRGBA16_RSP(TM64_Geometry.OK64F3DObject TargetObject, TM64_Geometry.OK64Texture TextureObject, string GraphPtr)
        {
            
            List<string> Output = new List<string>();
            F3DSharp.F3DEX095 F3D = new F3DSharp.F3DEX095();
            string[] CombineNames = F3DSharp.F3DEX095_Parameters.GCCModeNames;
            string[] GeometryNames = F3DSharp.F3DEX095_Parameters.GeometryModeNames;
            string[] RenderNames = F3DSharp.F3DEX095_Parameters.RenderModeNames;
            string[] FormatNames = F3DSharp.F3DEX095_Parameters.TextureFormatNames;
            string[] BitSizeNames = F3DSharp.F3DEX095_Parameters.BitSizeNames;
            string[] ModeNames = F3DSharp.F3DEX095_Parameters.TextureModeNames;
            uint[] BitSizes = F3DSharp.F3DEX095_Parameters.BitSizes;
            uint[] GIMSize = F3DSharp.F3DEX095_Parameters.G_IM_ArrayLineBytes;



            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));


            Output.Add("");
            Output.Add("\t//" + TargetObject.objectName);
            Output.Add("\t//Start Texture Load");
            Output.Add("\t//" + TextureObject.textureName);
            Output.Add("");

            Output.Add("\tgSPTexture( " + GraphPtr + "++, 65535, 65535, 0, 0, 1);");
            Output.Add("\tgDPPipeSync( " + GraphPtr + "++);");
            Output.Add("\tgDPSetCombineMode( " + GraphPtr + "++, " + CombineNames[TextureObject.CombineModeA] + ", " + CombineNames[TextureObject.CombineModeB] + ");");
            Output.Add("\tgDPSetRenderMode( " + GraphPtr + "++, " + RenderNames[TextureObject.RenderModeA] + ", " + RenderNames[TextureObject.RenderModeB] + ");");
            Output.Add("\tgDPTileSync( " + GraphPtr + "++);");
            Output.Add("\tgDPSetTile( " + GraphPtr + "++, " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", ((" + (TextureObject.textureWidth * GIMSize[TextureObject.BitSize]).ToString() + " + 7) >> 3), 0, 0, 0, " + ModeNames[TextureObject.TFlag]
                + ", " + heightex.ToString() + ", 0, " + ModeNames[TextureObject.SFlag] + ", " + widthex.ToString() + ", 0);");
            Output.Add("\tgDPSetTileSize( " + GraphPtr + "++, 0, 0, 0, (" + TextureObject.textureWidth.ToString() + " - 1) << 2, (" + TextureObject.textureHeight.ToString() + " - 1) << 2);");

            Output.Add("\tgDPClearGeometryMode( " + GraphPtr + "++, " + F3DSharp.F3DEX095_Parameters.AllGeometryModes.ToString() + ");");
            Output.Add("\tgDPSetGeometryMode( " + GraphPtr + "++, " + TextureObject.GeometryModes.ToString() + ");");

            Output.Add("\tgDPSetTextureImage( " + GraphPtr + "++, " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", 1, &" + TextureObject.textureName + " );");
            Output.Add("\tgDPTileSync( " + GraphPtr + "++);");
            Output.Add("\tgDPSetTile( " + GraphPtr + "++, " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", 0, 0, 7, 0, 0, 0, 0, 0, 0, 0);");
            Output.Add("\tgDPLoadSync( " + GraphPtr + "++);");
            Output.Add("\tgDPLoadBlock( " + GraphPtr + "++, 7, 0, 0, ((" +
                TextureObject.textureWidth.ToString() + " * " + TextureObject.textureHeight.ToString() + ") - 1), "
                + F3D.CALCDXT(Convert.ToUInt32(TextureObject.textureWidth), BitSizes[TextureObject.BitSize]).ToString() + ");");
            //END RGBA16 DRAW

            Output.Add("");
            Output.Add("\t//End Texture Load");
            Output.Add("\t//Start DrawCalls");
            Output.Add("");

            return Output.ToArray();

        }


        public string[] WriteCI_RSP(TM64_Geometry.OK64F3DObject TargetObject, TM64_Geometry.OK64Texture TextureObject, string GraphPtr)
        {

            List<string> Output = new List<string>();
            F3DSharp.F3DEX095 F3D = new F3DSharp.F3DEX095();
            string[] CombineNames = F3DSharp.F3DEX095_Parameters.GCCModeNames;
            string[] GeometryNames = F3DSharp.F3DEX095_Parameters.GeometryModeNames;
            string[] RenderNames = F3DSharp.F3DEX095_Parameters.RenderModeNames;
            string[] FormatNames = F3DSharp.F3DEX095_Parameters.TextureFormatNames;
            string[] BitSizeNames = F3DSharp.F3DEX095_Parameters.BitSizeNames;
            string[] ModeNames = F3DSharp.F3DEX095_Parameters.TextureModeNames;
            uint[] BitSizes = F3DSharp.F3DEX095_Parameters.BitSizes;
            uint[] GIMSize = F3DSharp.F3DEX095_Parameters.G_IM_ArrayLineBytes;



            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));


            /*
             * 
             * 
             *  if (TextureObject.BitSize < 2)
            {
                //Macro 4-bit Texture Load
                binaryWriter.Write(F3D.gsDPLoadTextureBlock_4b(Convert.ToUInt32(TextureObject.segmentPosition | 0x05000000),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat], Convert.ToUInt32(TextureObject.textureWidth), Convert.ToUInt32(TextureObject.textureHeight),
                    0, F3DEX095_Parameters.TextureModes[TextureObject.SFlag], widthex, 0, F3DEX095_Parameters.TextureModes[TextureObject.TFlag], heightex, 0));
            }
            else
            {
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
                binaryWriter.Write(F3D.gsDPLoadTextureBlock(
                    Convert.ToUInt32(TextureObject.segmentPosition | 0x05000000),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                    Convert.ToUInt32(TextureObject.textureWidth),
                    Convert.ToUInt32(TextureObject.textureHeight),
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.SFlag],
                    widthex,
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.TFlag],
                    heightex,
                    0));
                

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

                if (FogToggle)
                {
                    binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.G_CC_PASS2
                        )
                    );
                }
                else

                {
                    binaryWriter.Write(
                        F3D.gsDPSetCombineMode(
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeA],
                            F3DEX095_Parameters.GCCModes[TextureObject.CombineModeB]
                        )
                    );
                }

            }
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





            //set render mode
            if (GeometryToggle)
            {

                if (FogToggle)
                {
                    binaryWriter.Write(
                        F3D.gsDPSetRenderMode(
                            F3DEX095_Parameters.G_RM_FOG_SHADE_A,
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeB]
                        )
                    );
                }
                else

                {
                    binaryWriter.Write(
                        F3D.gsDPSetRenderMode(
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeA],
                            F3DEX095_Parameters.RenderModes[TextureObject.RenderModeB]
                        )
                    );
                }

            }



            if (GeometryToggle)
            {
                //setup the Geometry Mode parameter
                TextureObject.GeometryModes = 0;
                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    if (TextureObject.GeometryBools[ThisCheck])
                    {
                        TextureObject.GeometryModes |= F3DEX095_Parameters.GeometryModes[ThisCheck];
                    }
                }
                binaryWriter.Write(F3D.gsSPSetGeometryMode(TextureObject.GeometryModes));               //set the mode we made above.
            }




            if (GeometryToggle)
            {
                //binaryWriter.Write(F3D.gsSPClearGeometryMode(TextureObject.GeometryModes));    //clear existing modes
            }



            binaryWriter.Write(F3D.gsSPEndDisplayList());                                             //End the Display List


            */


            Output.Add("");
            Output.Add("\t//" + TargetObject.objectName);
            Output.Add("\t//Start Texture Load");
            Output.Add("\t//" + TextureObject.textureName);
            Output.Add("");

            /*
             * 
             * if (TextureObject.BitSize < 2)
            {
                //Macro 4-bit Texture Load
                binaryWriter.Write(F3D.gsDPLoadTextureBlock_4b(Convert.ToUInt32(TextureObject.segmentPosition | 0x05000000),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat], Convert.ToUInt32(TextureObject.textureWidth), Convert.ToUInt32(TextureObject.textureHeight),
                    0, F3DEX095_Parameters.TextureModes[TextureObject.SFlag], widthex, 0, F3DEX095_Parameters.TextureModes[TextureObject.TFlag], heightex, 0));
            }
            else
            {
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
                binaryWriter.Write(F3D.gsDPLoadTextureBlock(
                    Convert.ToUInt32(TextureObject.segmentPosition | 0x05000000),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                    Convert.ToUInt32(TextureObject.textureWidth),
                    Convert.ToUInt32(TextureObject.textureHeight),
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.SFlag],
                    widthex,
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.TFlag],
                    heightex,
                    0));
                

            }


            (F3D.gsDPLoadTextureBlock_4b(Convert.ToUInt32(TextureObject.segmentPosition | 0x05000000),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat], Convert.ToUInt32(TextureObject.textureWidth),
            Convert.ToUInt32(TextureObject.textureHeight), 0, F3DEX095_Parameters.TextureModes[TextureObject.SFlag], 
            widthex, 0, F3DEX095_Parameters.TextureModes[TextureObject.TFlag], heightex, 0));
            */
            if (TextureObject.BitSize < 1)
            {
                /*
                Output.Add("\tgDPLoadTextureBlock_4b( " + GraphPtr + "++, &" + TextureObject.textureName + FormatNames[TextureObject.TextureFormat] + ", " +
                    TextureObject.textureWidth.ToString(); +", " + TextureObject.textureHeight.ToString(); +", 0, " +
                    ModeNames[TextureObject.SFlag] + ", " + ModeNames[TextureObject.TFlag]; + ", "+ heightex.ToString() + ", 0);");
                */


            }
            else
            {

            }

            Output.Add("\tgSPTexture( " + GraphPtr + "++, 65535, 65535, 0, 0, 1);");
            Output.Add("\tgDPPipeSync( " + GraphPtr + "++);");
            Output.Add("\tgDPSetCombineMode( " + GraphPtr + "++, " + CombineNames[TextureObject.CombineModeA] + ", " + CombineNames[TextureObject.CombineModeB] + ");");
            Output.Add("\tgDPSetRenderMode( " + GraphPtr + "++, " + RenderNames[TextureObject.RenderModeA] + ", " + RenderNames[TextureObject.RenderModeB] + ");");
            Output.Add("\tgDPTileSync( " + GraphPtr + "++);");
            Output.Add("\tgDPSetTile( " + GraphPtr + "++, " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", ((" + (TextureObject.textureWidth * GIMSize[TextureObject.BitSize]).ToString() + " + 7) >> 3), 0, 0, 0, " + ModeNames[TextureObject.TFlag]
                + ", " + heightex.ToString() + ", 0, " + ModeNames[TextureObject.SFlag] + ", " + widthex.ToString() + ", 0);");
            Output.Add("\tgDPSetTileSize( " + GraphPtr + "++, 0, 0, 0, (" + TextureObject.textureWidth.ToString() + " - 1) << 2, (" + TextureObject.textureHeight.ToString() + " - 1) << 2);");

            Output.Add("\tgDPClearGeometryMode( " + GraphPtr + "++, " + F3DSharp.F3DEX095_Parameters.AllGeometryModes.ToString() + ");");
            Output.Add("\tgDPSetGeometryMode( " + GraphPtr + "++, " + TextureObject.GeometryModes.ToString() + ");");

            Output.Add("\tgDPSetTextureImage( " + GraphPtr + "++, " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", 1, &" + TextureObject.textureName + " );");
            Output.Add("\tgDPTileSync( " + GraphPtr + "++);");
            Output.Add("\tgDPSetTile( " + GraphPtr + "++, " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", 0, 0, 7, 0, 0, 0, 0, 0, 0, 0);");
            Output.Add("\tgDPLoadSync( " + GraphPtr + "++);");
            Output.Add("\tgDPLoadBlock( " + GraphPtr + "++, 7, 0, 0, ((" +
                TextureObject.textureWidth.ToString() + " * " + TextureObject.textureHeight.ToString() + ") - 1), "
                + F3D.CALCDXT(Convert.ToUInt32(TextureObject.textureWidth), BitSizes[TextureObject.BitSize]).ToString() + ");");
            //END RGBA16 DRAW

            Output.Add("");
            Output.Add("\t//End Texture Load");
            Output.Add("\t//Start DrawCalls");
            Output.Add("");

            return Output.ToArray();

        }
        public string[] WriteRSPCommands(TM64_Geometry.OK64F3DObject TargetObject, TM64_Geometry.OK64Texture TextureObject, string GraphPtr)
        {
            List<string> Output = new List<string>();
            F3DSharp.F3DEX095 F3D = new F3DSharp.F3DEX095();
            string[] CombineNames = F3DSharp.F3DEX095_Parameters.GCCModeNames;
            string[] GeometryNames = F3DSharp.F3DEX095_Parameters.GeometryModeNames;
            string[] RenderNames = F3DSharp.F3DEX095_Parameters.RenderModeNames;
            string[] FormatNames = F3DSharp.F3DEX095_Parameters.TextureFormatNames;
            string[] BitSizeNames = F3DSharp.F3DEX095_Parameters.BitSizeNames;
            string[] ModeNames = F3DSharp.F3DEX095_Parameters.TextureModeNames;
            uint[] BitSizes = F3DSharp.F3DEX095_Parameters.BitSizes;
            uint[] GIMSize = F3DSharp.F3DEX095_Parameters.G_IM_ArrayLineBytes;

            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));


            TextureObject.GeometryModes = 0;
            for (int ThisCheck = 0; ThisCheck < F3DSharp.F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
            {
                if (TextureObject.GeometryBools[ThisCheck])
                {
                    TextureObject.GeometryModes |= F3DSharp.F3DEX095_Parameters.GeometryModes[ThisCheck];
                }
            }


            Output.Add("void GFX_" + TargetObject.objectName + " ()");
            Output.Add("{");


            //LOAD TEXTURE DATA RGBA16
            switch(TextureObject.TextureFormat)
            {

                case 0:
                default:
                    {
                        Output.AddRange(WriteRGBA16_RSP(TargetObject, TextureObject, GraphPtr));
                        break;
                    }
                case 2:
                    {
                        
                        break;
                    }
                case 3:
                case 4:
                    {
                        
                        break;
                    }
                case 1:
                    {
                        MessageBox.Show("ERROR - " + TextureObject.textureName + " - YUV Format not supported.");
                        break;
                    }
            }
        


            int VertIndex = 0;
            int LocalVertIndex = 0;
            int FaceIndex = 0;
            bool Finished = false;

            while (!Finished)
            {
                Output.Add("");
                Output.Add("\tgSPVertex( " + GraphPtr + "++, &" + TargetObject.objectName + "_V[" + VertIndex.ToString() + "] , 30, 0);");
                LocalVertIndex = 0;
                for (int ThisFace = 0; ThisFace < 5; ThisFace++)
                {
                    if ((FaceIndex + 1) < TargetObject.modelGeometry.Length)
                    {
                        Output.Add("\t\tgSP2Triangles( " + GraphPtr + "++, " +
                            LocalVertIndex.ToString() + ", " +
                            (LocalVertIndex + 1).ToString() + ", " +
                            (LocalVertIndex + 2).ToString() + ", " +
                            "0, " +
                            (LocalVertIndex + 3).ToString() + ", " +
                            (LocalVertIndex + 4).ToString() + ", " +
                            (LocalVertIndex + 5).ToString() + ", " +
                            "0 );"
                            );

                        FaceIndex += 2;
                        VertIndex += 6;
                        LocalVertIndex += 6;
                    }
                    else if (FaceIndex < TargetObject.modelGeometry.Length)
                    {
                        Output.Add("\t\tgSP2Triangles( " + GraphPtr + "++, " +
                            "0, " +
                            "0, " +
                            "0, " +
                            "0, " +
                            VertIndex.ToString() +
                            (VertIndex + 1).ToString() + ", " +
                            (VertIndex + 2).ToString() + ", " +
                            "0 );"
                            );

                        FaceIndex += 1;
                        LocalVertIndex += 3;
                        VertIndex += 3;
                    }
                    else
                    {
                        Finished = true;
                        ThisFace = 5;
                    }


                }

            }

            Output.Add("");
            Output.Add("\t//End DrawCalls");
            Output.Add("\t//End " + TargetObject.objectName);
            Output.Add("");


            Output.Add("}");
            return Output.ToArray();
        }


        public string[] WriteTextureRSP(TM64_Geometry.OK64Texture TextureObject, string GraphPtr)
        {
            List<string> Output = new List<string>();
            F3DSharp.F3DEX2 F3D = new F3DSharp.F3DEX2();
            string[] CombineNames = F3DSharp.F3DEX2_Parameters.GCCModeNames;
            string[] GeometryNames = F3DSharp.F3DEX2_Parameters.GeometryModeNames;
            string[] RenderNames = F3DSharp.F3DEX2_Parameters.RenderModeNames;
            string[] FormatNames = F3DSharp.F3DEX2_Parameters.TextureFormatNames;
            string[] BitSizeNames = F3DSharp.F3DEX2_Parameters.BitSizeNames;
            string[] ModeNames = F3DSharp.F3DEX2_Parameters.TextureModeNames;
            uint[] BitSizes = F3DSharp.F3DEX2_Parameters.BitSizes;
            uint[] GIMSize = F3DSharp.F3DEX2_Parameters.G_IM_ArrayLineBytes;

            UInt32 heightex, widthex;
            if (TextureObject.textureHeight != 0)
            {
                heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
                widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));
            }
            else
            {
                heightex = 0;
                widthex = 0;
            }



            TextureObject.GeometryModes = 0;
            for (int ThisCheck = 0; ThisCheck < F3DSharp.F3DEX2_Parameters.GeometryModes.Length; ThisCheck++)
            {
                if (TextureObject.GeometryBools[ThisCheck])
                {
                    TextureObject.GeometryModes |= F3DSharp.F3DEX2_Parameters.GeometryModes[ThisCheck];
                }
            }


            Output.Add("Gfx Draw_" + TextureObject.textureName + "_T[] = ");
            Output.Add("{");


            //LOAD TEXTURE DATA RGBA16
            switch (TextureObject.TextureFormat)
            {

                case 0:
                default:
                    {
                        Output.AddRange(WriteRGBA16_RSP(TextureObject, GraphPtr));
                        break;
                    }
                case 2:
                    {
                        //ci
                        Output.AddRange(WriteCI_RSP(TextureObject));
                        break;
                    }
                case 3:
                case 4:
                    {
                        //ia
                        MessageBox.Show("ERROR - " + TextureObject.textureName + " - IA Format not supported.");
                        break;
                    }
                case 1:
                    {
                        MessageBox.Show("ERROR - " + TextureObject.textureName + " - YUV Format not supported.");
                        break;
                    }
            }



            Output.Add(Environment.NewLine);
            Output.Add("\tgsSPEndDisplayList(),");
            Output.Add("");


            Output.Add("};");
            return Output.ToArray();
        }


        public string[] WriteRGBA16_RSP(TM64_Geometry.OK64Texture TextureObject, string GraphPtr)
        {

            List<string> Output = new List<string>();
            F3DSharp.F3DEX2 F3D = new F3DSharp.F3DEX2();
            string[] CombineNames = F3DSharp.F3DEX2_Parameters.GCCModeNames;
            string[] GeometryNames = F3DSharp.F3DEX2_Parameters.GeometryModeNames;
            string[] RenderNames = F3DSharp.F3DEX2_Parameters.RenderModeNames;
            string[] FormatNames = F3DSharp.F3DEX2_Parameters.TextureFormatNames;
            string[] BitSizeNames = F3DSharp.F3DEX2_Parameters.BitSizeNames;
            string[] ModeNames = F3DSharp.F3DEX2_Parameters.TextureModeNames;
            uint[] BitSizes = F3DSharp.F3DEX2_Parameters.BitSizes;
            uint[] GIMSize = F3DSharp.F3DEX2_Parameters.G_IM_ArrayLineBytes;


            UInt32 heightex, widthex;
            if (TextureObject.textureHeight != 0)
            {
                heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
                widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));
            }
            else
            {
                heightex = 0;
                widthex = 0;
            }


            Output.Add("");
            Output.Add("\t//Start Texture Load");
            Output.Add("\t//" + TextureObject.textureName);
            Output.Add("");

            Output.Add("\tgsSPTexture( " + " 65535, 65535, 0, 0, 1),");
            Output.Add("\tgsDPPipeSync( " + "),");
            Output.Add("\tgsDPSetCombineMode( " + " " + CombineNames[TextureObject.CombineModeA] + ", " + CombineNames[TextureObject.CombineModeB] + "),");
            Output.Add("\tgsDPSetRenderMode( " + " " + RenderNames[TextureObject.RenderModeA] + ", " + RenderNames[TextureObject.RenderModeB] + "),");
            Output.Add("\tgsDPTileSync( " + "),");
            Output.Add("\tgsDPSetTile( " + " " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", ((" + (TextureObject.textureWidth * GIMSize[TextureObject.BitSize]).ToString() + " + 7) >> 3), 0, 0, 0, " + ModeNames[TextureObject.TFlag]
                + ", " + heightex.ToString() + ", 0, " + ModeNames[TextureObject.SFlag] + ", " + widthex.ToString() + ", 0),");
            Output.Add("\tgsDPSetTileSize( " + " 0, 0, 0, (" + TextureObject.textureWidth.ToString() + " - 1) << 2, (" + TextureObject.textureHeight.ToString() + " - 1) << 2),");

            Output.Add("\tgsSPClearGeometryMode( " + " " + F3DSharp.F3DEX2_Parameters.AllGeometryModes.ToString() + "),");
            Output.Add("\tgsSPSetGeometryMode( " + " " + TextureObject.GeometryModes.ToString() + "),");

            Output.Add("\tgsDPSetTextureImage( " + " " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", 1, &" + TextureObject.textureName + "),");
            Output.Add("\tgsDPTileSync( " + "),");
            Output.Add("\tgsDPSetTile( " + " " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", 0, 0, 7, 0, 0, 0, 0, 0, 0, 0),");
            Output.Add("\tgsDPLoadSync( " + "),");
            Output.Add("\tgsDPLoadBlock( " + " 7, 0, 0, ((" +
                TextureObject.textureWidth.ToString() + " * " + TextureObject.textureHeight.ToString() + ") - 1), "
                + F3D.CALCDXT(Convert.ToUInt32(TextureObject.textureWidth), BitSizes[TextureObject.BitSize]).ToString() + "),");
            //END RGBA16 DRAW

            Output.Add("");
            Output.Add("\t//End Texture Load");
            Output.Add("\t//Start DrawCalls");
            Output.Add("");

            return Output.ToArray();

        }

        public string[] WriteCI_RSP(TM64_Geometry.OK64Texture TextureObject)
        {

            List<string> Output = new List<string>();
            F3DSharp.F3DEX2 F3D = new F3DSharp.F3DEX2();
            string[] CombineNames = F3DSharp.F3DEX2_Parameters.GCCModeNames;
            string[] GeometryNames = F3DSharp.F3DEX2_Parameters.GeometryModeNames;
            string[] RenderNames = F3DSharp.F3DEX2_Parameters.RenderModeNames;
            string[] FormatNames = F3DSharp.F3DEX2_Parameters.TextureFormatNames;
            string[] BitSizeNames = F3DSharp.F3DEX2_Parameters.BitSizeNames;
            string[] ModeNames = F3DSharp.F3DEX2_Parameters.TextureModeNames;
            uint[] BitSizes = F3DSharp.F3DEX2_Parameters.BitSizes;
            uint[] GIMSize = F3DSharp.F3DEX2_Parameters.G_IM_ArrayLineBytes;




            UInt32 heightex, widthex;
            if (TextureObject.textureHeight != 0)
            {
                heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
                widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));
            }
            else
            {
                heightex = 0;
                widthex = 0;
            }

            Output.Add("");
            Output.Add("\t//Start Texture Load");
            Output.Add("\t//" + TextureObject.textureName);
            Output.Add("");


            /*
             * 
             * binaryWriter.Write(F3D.gsDPSetTextureLUT(F3DSharp.F3DEX2_Parameters.G_TT_RGBA16));
            binaryWriter.Write(F3D.gsDPLoadTLUT_pal16(0, Convert.ToUInt32(TextureObject.palettePosition | SegmentID)));

            */

            Output.Add
            (
                "\tgsDPSetTextureLUT( " + Convert.ToString(F3DEX2_Parameters.G_TT_RGBA16) + "),"
            );


            Output.Add("\tgsSPTexture( 65535, 65535, 0, 0, 1),");

            Output.Add("\tgsDPPipeSync( " + "),");
            Output.Add("\tgsDPSetCombineMode( " + " " + CombineNames[TextureObject.CombineModeA] + ", " + CombineNames[TextureObject.CombineModeB] + "),");
            Output.Add("\tgsDPSetRenderMode( " + " " + RenderNames[TextureObject.RenderModeA] + ", " + RenderNames[TextureObject.RenderModeB] + "),");
            Output.Add("\tgsDPTileSync( " + "),");
            Output.Add("\tgsDPSetTile( " + " " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", ((" + (TextureObject.textureWidth * GIMSize[TextureObject.BitSize]).ToString() + " + 7) >> 3), 0, 0, 0, " + ModeNames[TextureObject.TFlag]
                + ", " + heightex.ToString() + ", 0, " + ModeNames[TextureObject.SFlag] + ", " + widthex.ToString() + ", 0),");
            Output.Add("\tgsDPSetTileSize( " + " 0, 0, 0, (" + TextureObject.textureWidth.ToString() + " - 1) << 2, (" + TextureObject.textureHeight.ToString() + " - 1) << 2),");

            Output.Add("\tgsSPClearGeometryMode( " + " " + F3DSharp.F3DEX2_Parameters.AllGeometryModes.ToString() + "),");
            Output.Add("\tgsSPSetGeometryMode( " + " " + TextureObject.GeometryModes.ToString() + "),");



            Output.Add
            (
                "\tgsDPLoadTLUT_pal16( 0, &" + TextureObject.textureName + "_PAL),"
            );
            if (TextureObject.BitSize < 2)
            {
                //Macro 4-bit Texture Load
                Output.Add
                (

                    "\tgsDPLoadTextureBlock_4b( &" +
                    TextureObject.textureName + "_T, " +
                    F3DEX2_Parameters.TextureFormatNames[TextureObject.TextureFormat] + "," +
                    Convert.ToString(TextureObject.textureWidth) + "," +
                    Convert.ToString(TextureObject.textureHeight) + "," +
                   "0," +
                   F3DEX2_Parameters.TextureModeNames[TextureObject.SFlag] + "," +
                   F3DEX2_Parameters.TextureModeNames[TextureObject.TFlag] + "," +
                   Convert.ToString(widthex) + "," +
                   Convert.ToString(heightex) + "," +
                   "0," +
                   "0),"
                );
            }
            else
            {
                //Load Texture Settings

                Output.Add
                (
                    "\tgsNinSetupTileDescription( " +
                    F3DEX2_Parameters.TextureFormatNames[TextureObject.TextureFormat] + "," +
                    F3DEX2_Parameters.BitSizeNames[TextureObject.BitSize] + "," +
                    Convert.ToString(TextureObject.textureWidth) + "," +
                    Convert.ToString(TextureObject.textureHeight) + "," +
                   "0," +
                   "0," +
                   F3DEX2_Parameters.TextureModeNames[TextureObject.SFlag] + "," +
                   Convert.ToString(widthex) + "," +
                   "0," +
                   F3DEX2_Parameters.TextureModeNames[TextureObject.TFlag] + "," +
                   Convert.ToString(heightex) + "," +
                   "0),"
                );
                //Load Texture Data
                Output.Add
                (
                   "\tgsDPLoadTextureBlock( &" +
                    TextureObject.textureName + "_T, " +
                    F3DEX2_Parameters.TextureFormatNames[TextureObject.TextureFormat] + "," +
                    F3DEX2_Parameters.BitSizeNames[TextureObject.BitSize] + "," +
                    Convert.ToString(TextureObject.textureWidth) + "," +
                    Convert.ToString(TextureObject.textureHeight) + "," +
                   "0," +
                   F3DEX2_Parameters.TextureModeNames[TextureObject.SFlag] + "," +
                   F3DEX2_Parameters.TextureModeNames[TextureObject.TFlag] + "," +
                   Convert.ToString(widthex) + "," +
                   Convert.ToString(heightex) + "," +
                   "0," +
                   "0),"
                );

            }
            Output.Add("");
            Output.Add("\t//End Texture Load");
            Output.Add("\t//Start DrawCalls");
            Output.Add("");

            return Output.ToArray();

        }

        public string[] WriteGeometryRSP(TM64_Geometry.OK64F3DObject TargetObject, TM64_Geometry.OK64Texture TextureObject, string GraphPtr)
        {
            List<string> Output = new List<string>();
            F3DSharp.F3DEX2 F3D = new F3DSharp.F3DEX2();

            Output.Add("Gfx Draw_" + TargetObject.objectName + "_M[] = ");
            Output.Add("{");

            OK64F3DModel[] CrunchedModel = CrunchF3DModel(TargetObject);

            int VOffset = 0;

            for (int ThisDraw = 0; ThisDraw < CrunchedModel.Length; ThisDraw++)
            {
                uint VertexCount = Convert.ToUInt32(CrunchedModel[ThisDraw].VertexCache.Count);
                int LocalFaceCount = CrunchedModel[ThisDraw].Indexes.Count;
                Output.Add("\tgsSPVertex(&" + TargetObject.objectName + "_V[" + VOffset.ToString() + "] , " + VertexCount.ToString() + ", 0),");

                VOffset += Convert.ToInt32(VertexCount);

                for (int TargetIndex = 0; TargetIndex < LocalFaceCount;)
                {
                    if (TargetIndex + 2 <= LocalFaceCount)
                    {
                        //Tri2
                        int[] Indexes = CrunchedModel[ThisDraw].Indexes[TargetIndex];
                        uint[] VertIndexesA = F3D.GetIndexes(Indexes[0], Indexes[1], Indexes[2]);

                        Indexes = CrunchedModel[ThisDraw].Indexes[TargetIndex + 1];
                        uint[] VertIndexesB = F3D.GetIndexes(Indexes[0], Indexes[1], Indexes[2]);

                        Output.Add("\t\tgsSP2Triangles(" +
                            VertIndexesA[0].ToString() + ", " +
                            (VertIndexesA[1]).ToString() + ", " +
                            (VertIndexesA[2]).ToString() + ", " +
                            "0, " +
                            (VertIndexesB[0]).ToString() + ", " +
                            (VertIndexesB[1]).ToString() + ", " +
                            (VertIndexesB[2]).ToString() + ", " +
                            "0 ),"
                            );
                        TargetIndex += 2;
                    }
                    else
                    {
                        //Tri1
                        int[] Indexes = CrunchedModel[ThisDraw].Indexes[TargetIndex];
                        uint[] VertIndexesA = F3D.GetIndexes(Indexes[0], Indexes[1], Indexes[2]);

                        Output.Add("\t\tgsSP1Triangle(" +
                            VertIndexesA[0].ToString() + ", " +
                            (VertIndexesA[1]).ToString() + ", " +
                            (VertIndexesA[2]).ToString() + ", " +
                            "0 ),"
                            );
                        TargetIndex += 1;
                    }
                }

            }


            Output.Add(Environment.NewLine);
            Output.Add("\tgsSPEndDisplayList(),");
            Output.Add("");
            Output.Add("\t//End DrawCalls " + TargetObject.objectName);
            Output.Add("");


            Output.Add("};");
            return Output.ToArray();
        }


    }







}

// phew! We made it. I keep saying we, but it's me doing all the work!
// maybe try pitching in sometime and updating the program! I'd love the help!

// Thank you so much for-a reading my source!

// OverKart 64 Library
// For Mario Kart 64 1.0 USA ROM
// <3 Hamp

