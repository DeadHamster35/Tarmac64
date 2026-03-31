using Aspose.ThreeD;
using Aspose.ThreeD.Entities;
using Aspose.ThreeD.Formats;
using Aspose.ThreeD.Render;
using Aspose.ThreeD.Utilities;
using Assimp;  //for handling model data
using Assimp.Unmanaged;
using Cereal64.Common.DataElements;
using Cereal64.Common.Rom;
using Cereal64.Common.Utils.Encoding;
using Cereal64.Microcodes.F3DEX.DataElements;
using Cereal64.Microcodes.F3DEX.DataElements.Commands;
using F3DSharp;
using Microsoft.WindowsAPICodePack.Dialogs;
using SharpDX;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Shaders;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting.Contexts;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xml;
using System.Xml.Linq;
using Tarmac64_Library;
using Texture64;  //for handling texture data
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;
using static Tarmac64_Library.TM64_Geometry;

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
            public UInt32 RotationOffset { get; set; }
            public UInt32 ScalingOffset { get; set; }

            public UInt32 MeshListOffset { get; set; }

        }


        public class OK64Animation
        {
            public string AnimationName { get; set; }
            public short[] TranslationTime { get; set; }
            public short[] RotationTime { get; set; }
            public short[] ScaleTime { get; set; }
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

                string PathfinderHeader = TargetPath + "/Pathfinder";
                pathfindingObject = new PathfindingObject(XMLDoc, PathfinderHeader);
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
                Tarmac.GenerateElement(XMLDoc, ObjectXML, "surfaceMaterial", surfaceMaterial);
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
            public PathfindingObject(XmlDocument XMLDoc, string Parent)
            {
                TM64 Tarmac = new TM64();
                XmlNode Owner = XMLDoc.SelectSingleNode(Parent);

                highX = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, Parent, "highX", "0"));
                highY = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, Parent, "highY", "0"));
                highZ = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, Parent, "highZ", "0"));
                lowX = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, Parent, "lowX", "0"));
                lowY = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, Parent, "lowY", "0"));
                lowZ = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, Parent, "lowZ", "0"));

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
            Skeleton.Animation.RotationTime = new short[DataLength];
            for (int ThisRot = 0; ThisRot < Skeleton.Animation.RotationData.Length; ThisRot++)
            {
                Skeleton.Animation.RotationData[ThisRot] = new short[3];
                for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                {
                    Skeleton.Animation.RotationData[ThisRot][ThisVector] = binaryReader.ReadInt16();
                }
                Skeleton.Animation.RotationTime[ThisRot] = binaryReader.ReadInt16();
            }
            //
            DataLength = binaryReader.ReadInt32();
            Skeleton.Animation.TranslationData = new short[DataLength][];
            Skeleton.Animation.TranslationTime = new short[DataLength];
            for (int ThisRot = 0; ThisRot < Skeleton.Animation.TranslationData.Length; ThisRot++)
            {
                Skeleton.Animation.TranslationData[ThisRot] = new short[3];
                for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                {
                    Skeleton.Animation.TranslationData[ThisRot][ThisVector] = binaryReader.ReadInt16();
                }
                Skeleton.Animation.TranslationTime[ThisRot] = binaryReader.ReadInt16();
            }
            //
            DataLength = binaryReader.ReadInt32();
            Skeleton.Animation.ScalingData = new short[DataLength][];
            Skeleton.Animation.ScaleTime = new short[DataLength];
            for (int ThisRot = 0; ThisRot < Skeleton.Animation.ScalingData.Length; ThisRot++)
            {
                Skeleton.Animation.ScalingData[ThisRot] = new short[3];
                for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                {
                    Skeleton.Animation.ScalingData[ThisRot][ThisVector] = binaryReader.ReadInt16();
                }
                Skeleton.Animation.ScaleTime[ThisRot] = binaryReader.ReadInt16();
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
                        binaryWriter.Write(Skeleton.Animation.RotationTime[ThisRot]);
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
                        binaryWriter.Write(Skeleton.Animation.TranslationTime[ThisRot]);
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
                        binaryWriter.Write(Skeleton.Animation.ScaleTime[ThisRot]);
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
   
        public OK64F3DObject CreateF3DObject (Assimp.Scene fbx, Assimp.Node objectNode, TM64_Texture.OK64Texture[] textureArray, bool ForceFlatUV = false, bool AlphaChannelTwo = false, bool DisregardOrigin = false, float AnimeScale = 1.0f)
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

            BScale[0] *= AnimeScale;
            BScale[1] *= AnimeScale;
            BScale[2] *= AnimeScale;

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


        public bool CheckST(OK64F3DObject Object, TM64_Texture.OK64Texture textureObject)
        {
            bool CheckError = false;
            foreach (var Mesh in Object.modelGeometry)
            {
                foreach (var Vert in Mesh.VertData)
                {
                    for (int ThisTexel = 0; ThisTexel < textureObject.TexelData.Count; ThisTexel++)
                    {
                        if (Vert.position.sBase * textureObject.TexelData[ThisTexel].textureWidth > 32768 | Vert.position.sBase * textureObject.TexelData[ThisTexel].textureWidth < -32768)
                        {
                            return true;
                        }
                        else
                        {
                            if (Vert.position.tBase * textureObject.TexelData[ThisTexel].textureHeight > 32768 | Vert.position.tBase * textureObject.TexelData[ThisTexel].textureHeight < -32768)
                            {
                                return true;
                            }
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

        public OK64F3DObject[] LoadMaster(ref OK64F3DGroup[] groupArray, Assimp.Scene fbx, TM64_Texture.OK64Texture[] textureArray, bool AlphaCH = false)
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
                        masterList.Add(CreateF3DObject(fbx, groupParent.Children[currentGrandchild],textureArray, false, AlphaCH));
                        masterCount++;
                    }
                }
                else
                {
                    masterList.Add(CreateF3DObject(fbx, masterNode.Children[currentChild], textureArray, false, AlphaCH));
                    masterCount++;
                }
            }
            
            groupArray = groupList.ToArray();
            masterObjects = GroupSort(masterList.ToArray(), groupArray);
            return masterObjects;
        }



        public OK64F3DObject[] CreateObjects(Assimp.Scene fbx, TM64_Texture.OK64Texture[] textureArray, bool DisregardOrigin = false)
        {
            List<OK64F3DObject> masterObjects = new List<OK64F3DObject>();
            int currentObject = 0; 
            var BaseNode = fbx.RootNode.FindNode("Render Objects");
            TM64.OK64Settings TarmacSettings = new TM64.OK64Settings();
            TarmacSettings.LoadSettings();
            if (BaseNode == null)
            {


                BaseNode = fbx.RootNode.FindNode("Master Objects");
                if (BaseNode == null)
                {
                    MessageBox.Show("Error - No 'Render Objects' node");
                    return null;
                }
            }
            for (int childObject = 0; childObject < BaseNode.Children.Count; childObject++)
            {
                masterObjects.Add(CreateF3DObject(fbx, BaseNode.Children[childObject], textureArray, false, TarmacSettings.AlphaCH2, DisregardOrigin));
            }
            List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>(masterObjects);
            OK64F3DObject[] outputObjects = NaturalSort(masterObjects).ToArray();
            return outputObjects;
        }

        public OK64F3DObject[] CreateMasters(Assimp.Scene fbx, int sectionCount, TM64_Texture.OK64Texture[] textureArray, bool AlphaCH = false)
        {
            List<OK64F3DObject> masterObjects = new List<OK64F3DObject>();
            int currentObject = 0;
            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                var surfaceNode = fbx.RootNode.FindNode("Section " +(currentSection + 1).ToString());
                
                for (int childObject = 0; childObject < surfaceNode.Children.Count; childObject++)
                {
                    masterObjects.Add(CreateF3DObject(fbx,surfaceNode.Children[childObject], textureArray, false, AlphaCH));
                    currentObject++;
                }
                List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>(masterObjects);
            }

            OK64F3DObject[] outputObjects = NaturalSort(masterObjects).ToArray();

            return outputObjects;
        }

        public OK64F3DObject[] CreateMastersNoHeader(Assimp.Scene fbx, int sectionCount, TM64_Texture.OK64Texture[] textureArray, bool AlphaCH = false)
        {
            List<OK64F3DObject> masterObjects = new List<OK64F3DObject>();
            int currentObject = 0;
            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                var surfaceNode = fbx.RootNode;

                for (int childObject = 0; childObject < surfaceNode.Children.Count; childObject++)
                {
                    masterObjects.Add(CreateF3DObject(fbx, surfaceNode.Children[childObject], textureArray, false, AlphaCH));
                    currentObject++;
                }
                List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>(masterObjects);
            }

            OK64F3DObject[] outputObjects = NaturalSort(masterObjects).ToArray();

            return outputObjects;
        }

        public OK64F3DObject[] CreateMasterNoHeader(Assimp.Scene fbx, TM64_Texture.OK64Texture[] textureArray)
        {
            List<OK64F3DObject> masterObjects = new List<OK64F3DObject>();
            int currentObject = 0;
            for (int TargetOBJ = 0; TargetOBJ < fbx.RootNode.ChildCount; TargetOBJ++)
            {
                masterObjects.Add(CreateF3DObject(fbx, fbx.RootNode.Children[TargetOBJ], textureArray, false, false));
                currentObject++;
            }
            OK64F3DObject[] outputObjects = NaturalSort(masterObjects).ToArray();
            return outputObjects;
        }

        public OK64F3DObject[] LoadCollisions(Assimp.Scene fbx, int sectionCount, TM64_Texture.OK64Texture[] textureArray)
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
                    surfaceObjects.Add(CreateF3DObject(fbx,surfaceNode.Children[currentsubObject], textureArray, true));
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
        public OK64F3DObject[] CreateCollisionsNoHeader(Assimp.Scene fbx, TM64_Texture.OK64Texture[] textureArray)
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
                    surfaceObjects.Add(CreateF3DObject(fbx, surfaceNode.Children[currentsubObject], textureArray, true));
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

        public byte[] CompileF3DObject(byte[] InputData, OK64F3DObject[] MasterObjects, TM64_Texture.OK64Texture[] TextureObjects, int vertMagic, int Segment)
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
                        ThisVert.position.s = Convert.ToInt16(ThisVert.position.sBase * TextureObjects[cObj.materialID].TexelData[0].textureWidth);
                        ThisVert.position.t = Convert.ToInt16(ThisVert.position.tBase * TextureObjects[cObj.materialID].TexelData[0].textureHeight);
                        
                        

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

        public byte[] InflateVertex(byte[] Segment4)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            memoryStream.Write(Segment4, 0, Segment4.Length);
            memoryStream.Position = 0;

            MemoryStream OutputStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(OutputStream);

            for (int This = 0; This < (Segment4.Length / 14); This++)
            {
                
                binaryWriter.Write(binaryReader.ReadInt16());
                binaryWriter.Write(binaryReader.ReadInt16());
                binaryWriter.Write(binaryReader.ReadInt16());
                binaryWriter.Write(Convert.ToInt16(0));
                binaryWriter.Write(binaryReader.ReadInt16());
                binaryWriter.Write(binaryReader.ReadInt16());
                binaryWriter.Write(binaryReader.ReadByte());
                binaryWriter.Write(binaryReader.ReadByte());
                binaryWriter.Write(binaryReader.ReadByte());
                binaryWriter.Write(binaryReader.ReadByte());
            }

            return OutputStream.ToArray();
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
        public bool CompileCourseObjects(ref int outMagic, ref byte[] outseg4, ref byte[] outseg7, byte[] segment4, byte[] segment7, OK64F3DObject[] courseObject, TM64_Texture.OK64Texture[] textureObject, int vertMagic, bool BoundingToggle = false)
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

                        
                        
                        ThisVert.position.s = Convert.ToInt16(ThisVert.position.sBase * textureObject[cObj.materialID].TexelData[0].textureWidth);
                        ThisVert.position.t = Convert.ToInt16(ThisVert.position.tBase * textureObject[cObj.materialID].TexelData[0].textureHeight);
                        

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

        public int ZSort(TM64_Texture.OK64Texture TextureObject)
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
            return Check[TextureObject.ColorCombine.RenderModeA];
        }

        public bool XLUCheck(TM64_Texture.OK64Texture TextureObject)
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
            if (Check[TextureObject.ColorCombine.RenderModeA] && Check[TextureObject.ColorCombine.RenderModeB])
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public byte[] CompileObjectList(byte[] OutputData, OK64F3DObject courseObject, TM64_Texture.OK64Texture[] textureObject, int SegmentID)
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

                            byteArray = BitConverter.GetBytes(textureObject[currentTexture].CCPosition | (SegmentID << 24));
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
            }

            byteArray = BitConverter.GetBytes(0xB8000000);
            Array.Reverse(byteArray);
            seg6w.Write(byteArray);

            byteArray = BitConverter.GetBytes(0x00000000);
            Array.Reverse(byteArray);
            seg6w.Write(byteArray);
              
            return seg6m.ToArray();
            
        }


        public byte[] CompileF3DList(ref OK64SectionList[] sectionOut, OK64F3DObject[] courseObject, OK64SectionList[] sectionList, TM64_Texture.OK64Texture[] textureObject)
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

                                            byteArray = BitConverter.GetBytes(textureObject[currentTexture].CCPosition | 0x06000000);
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



        public byte[] CompileXLUList(ref OK64SectionList[] sectionOut, OK64F3DObject[] courseObject, OK64SectionList[] sectionList, TM64_Texture.OK64Texture[] textureObject)
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

            for (int currentSection = 0; currentSection < sectionList.Length; currentSection++)
            {
                for (int currentView = 0; currentView < 4; currentView++)
                {

                    int objectCount = sectionList[currentSection].objectList.Length;
                    sectionList[currentSection].segmentPosition = Convert.ToInt32(seg6m.Position);




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

                                    int objectIndex = sectionList[currentSection].objectList[currentObject];
                                    if (courseObject[objectIndex].materialID == currentTexture)
                                    {
                                        if (!textureWritten)
                                        {
                                            byteArray = BitConverter.GetBytes(0x06000000);
                                            Array.Reverse(byteArray);
                                            seg6w.Write(byteArray);

                                            byteArray = BitConverter.GetBytes(textureObject[currentTexture].CCPosition | 0x06000000);
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

                    seg6w.Write(F3D.gsSPEndDisplayList());

                    sectionOut = sectionList;

                }

            }


            return seg6m.ToArray();
        }

        public byte[] CompileBattleXLU(OK64F3DObject[] courseObject, TM64_Texture.OK64Texture[] textureObject)
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

                                    byteArray = BitConverter.GetBytes(textureObject[currentTexture].CCPosition | 0x06000000);
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
        public byte[] CompileBattleList(OK64F3DObject[] courseObject, TM64_Texture.OK64Texture[] textureObject)
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

                                    byteArray = BitConverter.GetBytes(textureObject[currentTexture].CCPosition | 0x06000000);
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

        public Matrix4x4 GetTotalTransform(Assimp.Node Base, Assimp.Scene FBX)
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



        public OK64Bone LoadBone(Assimp.Node Base, Assimp.Scene FBX, float ModelScale)
        {
            OK64Bone NewBone = new OK64Bone();
            NewBone.Name = Base.Name;
            NewBone.Children = new OK64Bone[Base.ChildCount];

            //Matrix4x4 OPrime = GetTotalTransform(Base, FBX);

            NewBone.Origin = new short[3];
            NewBone.Origin[0] = Convert.ToInt16(Base.Transform.A4 * 100 * ModelScale);
            NewBone.Origin[1] = Convert.ToInt16(Base.Transform.B4 * 100 * ModelScale);
            NewBone.Origin[2] = Convert.ToInt16(Base.Transform.C4 * 100 * ModelScale);

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


        public Point3D RotatePoint(Point3D Point, float[] ObjectAngles)
        {
            var id = Matrix3D.Identity;
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(1, 0, 0), ObjectAngles[0]));
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(0, 1, 0), ObjectAngles[1]));
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(0, 0, 1), ObjectAngles[2]));
            return id.Transform(Point);
        }

        
        public class MK64SurfaceStruct
        {
            public uint dlist { get; set; }      //display list to parse
            public byte type { get; set; }       //surface type
            public byte area { get; set; }        //area ID to render
            public short flags { get; set; }
        };

        public void RecursiveBullshit(uint Surf, uint Address, BinaryReader Seg4Reader, BinaryReader Seg6Reader, BinaryReader Seg7Reader, Aspose.ThreeD.Scene ExportData, Aspose.ThreeD.Entities.Mesh WorkMesh)
        {
            F3DEX095 F3D = new F3DEX095();
            ///hahahahahahahahahahahahahahahahahahahahahahahahahaha
            bool B8 = false;
            uint VertexCache = 0;
            Seg7Reader.BaseStream.Position = Address -= 0x07000000;
            while (!B8)
            {
                byte CommandByte = Seg7Reader.ReadByte();
                switch (CommandByte)
                {
                    case (0x04):
                        {
                            //Load Vertex
                            Seg7Reader.BaseStream.Position += 3;
                            VertexCache = BitConverter.ToUInt32(F3D.BigEndian(Seg7Reader.ReadUInt32()), 0);
                            VertexCache -= 0x04000000; //Segmented Address
                            break;
                        }
                    case (0x06):
                        {
                            Seg7Reader.BaseStream.Position += 3;
                            uint AddressB = BitConverter.ToUInt32(F3D.BigEndian(Seg7Reader.ReadUInt32()), 0);
                            long SavePoint = Seg7Reader.BaseStream.Position;
                            RecursiveBullshit(Surf, AddressB, Seg4Reader, Seg6Reader, Seg7Reader, ExportData, WorkMesh);
                            Seg7Reader.BaseStream.Position = SavePoint;
                            break;
                        }
                    case (0xB1):
                        {



                            int Index;

                            for (int ThisFace = 0; ThisFace < 2; ThisFace++)
                            {
                                int CurrentVertIndex = WorkMesh.ControlPoints.Count;
                                for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                                {
                                    Index = Seg7Reader.ReadByte() / 2;
                                    Seg4Reader.BaseStream.Position = VertexCache + (Index * 16);
                                    Vertex Vert = new Vertex();
                                    Vert.position = new Position();

                                    Vert.position.x = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                    Vert.position.z = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                    Vert.position.y = Convert.ToInt16(BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0) * -1);
                                    Seg4Reader.ReadInt16();//Padding
                                    Vert.position.sBase = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                    Vert.position.tBase = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);

                                    Vert.color = new OK64Color();
                                    Vert.color.R = Seg4Reader.ReadByte();
                                    Vert.color.G = Seg4Reader.ReadByte();
                                    Vert.color.B = Seg4Reader.ReadByte();
                                    Vert.color.A = Seg4Reader.ReadByte();

                                    Aspose.ThreeD.Utilities.Vector4 Positional = new Aspose.ThreeD.Utilities.Vector4();
                                    Positional.X = Vert.position.x;
                                    Positional.Y = Vert.position.y;
                                    Positional.Z = Vert.position.z;
                                    Positional.W = 1; //w for wut

                                    WorkMesh.ControlPoints.Add(Positional);


                                    float UValue = Vert.position.sBase / 32.0f / 32.0f;
                                    float VValue = Vert.position.tBase / 32.0f / 32.0f;

                                    // Create UVset
                                    //VertexElementUV elementUV = WorkMesh.CreateElementUV(Aspose.ThreeD.Entities.TextureMapping.Diffuse, MappingMode.PolygonVertex, ReferenceMode.IndexToDirect);
                                    // Copy the data to the UV vertex element 

                                    Aspose.ThreeD.Utilities.Vector4 UVWT = new Aspose.ThreeD.Utilities.Vector4
                                    (
                                        UValue,
                                        VValue,
                                        0.0f,
                                        1.0f
                                    );

                                    //elementUV.Data.Add(UVWT);

                                    // Create a vertex color element
                                    //VertexElementVertexColor colorElement = new VertexElementVertexColor();
                                    //colorElement.MappingMode = MappingMode.PolygonVertex;
                                    //colorElement.ReferenceMode = ReferenceMode.Direct;



                                    // Add vertex colors (Vector4 format)

                                    float RValue, GValue, BValue, AValue;
                                    RValue = Vert.color.R / 252.0f;
                                    GValue = Vert.color.G / 252.0f;
                                    BValue = Vert.color.B / 252.0f;
                                    AValue = Vert.color.A / 255.0f;
                                    //colorElement.Data.Add(new Aspose.ThreeD.Utilities.Vector4(RValue, GValue, BValue, AValue)); // Red
                                    //WorkMesh.VertexElements.Add(colorElement);
                                }
                                List<int> IndexList = new List<int>();
                                IndexList.Add(CurrentVertIndex);
                                IndexList.Add(CurrentVertIndex + 1);
                                IndexList.Add(CurrentVertIndex + 2);

                                WorkMesh.CreatePolygon(IndexList.ToArray());

                                if (ThisFace == 0)
                                {
                                    Seg7Reader.BaseStream.Position += 1;
                                }

                            }



                            //Two Triangles
                            break;
                        }

                    case (0xBF):
                        {


                            Seg7Reader.BaseStream.Position += 4;
                            int Index;
                            int CurrentVertIndex = WorkMesh.ControlPoints.Count;

                            for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                            {
                                Index = Seg7Reader.ReadByte() / 2;
                                Seg4Reader.BaseStream.Position = VertexCache + (Index * 16);
                                Vertex Vert = new Vertex();
                                Vert.position = new Position();

                                Vert.position.x = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                Vert.position.z = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                Vert.position.y = Convert.ToInt16(BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0) * -1);
                                Seg4Reader.ReadInt16();//Padding
                                Vert.position.sBase = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                Vert.position.tBase = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);

                                Vert.color = new OK64Color();
                                Vert.color.R = Seg4Reader.ReadByte();
                                Vert.color.G = Seg4Reader.ReadByte();
                                Vert.color.B = Seg4Reader.ReadByte();
                                Vert.color.A = Seg4Reader.ReadByte();

                                Aspose.ThreeD.Utilities.Vector4 Positional = new Aspose.ThreeD.Utilities.Vector4();
                                Positional.X = Vert.position.x;
                                Positional.Y = Vert.position.y;
                                Positional.Z = Vert.position.z;
                                Positional.W = 1; //w for wut

                                WorkMesh.ControlPoints.Add(Positional);


                                // Create UVset
                                //VertexElementUV elementUV = WorkMesh.CreateElementUV(Aspose.ThreeD.Entities.TextureMapping.Diffuse, MappingMode.PolygonVertex, ReferenceMode.IndexToDirect);
                                // Copy the data to the UV vertex element 

                                float UValue = Vert.position.sBase / 32.0f / 32.0f;
                                float VValue = Vert.position.tBase / 32.0f / 32.0f;

                                Aspose.ThreeD.Utilities.Vector4 UVWT = new Aspose.ThreeD.Utilities.Vector4
                                (
                                    UValue,
                                    VValue,
                                    0.0f,
                                    1.0f
                                );

                                //elementUV.Data.Add(UVWT);
                                // Create a vertex color element
                                //VertexElementVertexColor colorElement = new VertexElementVertexColor();
                                //colorElement.MappingMode = MappingMode.PolygonVertex;
                                //colorElement.ReferenceMode = ReferenceMode.Direct;



                                // Add vertex colors (Vector4 format)

                                float RValue, GValue, BValue, AValue;
                                RValue = Vert.color.R / 252.0f;
                                GValue = Vert.color.G / 252.0f;
                                BValue = Vert.color.B / 252.0f;
                                AValue = Vert.color.A / 255.0f;
                                //colorElement.Data.Add(new Aspose.ThreeD.Utilities.Vector4(RValue, GValue, BValue, AValue)); // Red
                                //WorkMesh.VertexElements.Add(colorElement);
                            }
                            List<int> IndexList = new List<int>();
                            IndexList.Add(CurrentVertIndex);
                            IndexList.Add(CurrentVertIndex + 1);
                            IndexList.Add(CurrentVertIndex + 2);

                            WorkMesh.CreatePolygon(IndexList.ToArray());
                            //One Triangle
                            break;
                        }
                    case (0xB8):
                        {

                            Aspose.ThreeD.Node node = ExportData.RootNode.CreateChildNode("Mesh-Sub" + Address.ToString("X").ToString(), WorkMesh);
                            Seg7Reader.BaseStream.Position += 7;
                            //End DL
                            B8 = true;
                            break;
                        }
                    default:
                        {
                            Seg7Reader.BaseStream.Position += 3;
                            //lolwut
                            break;
                        }

                }
            }
        }


        public void ExportSurfaceMap(byte[] Segment4, byte[] Segment6, byte[] Segment7, uint Offset)
        {
            MemoryStream Seg4Stream = new MemoryStream(Segment4);
            MemoryStream Seg6Stream = new MemoryStream(Segment6);
            MemoryStream Seg7Stream = new MemoryStream(Segment7);
            BinaryReader Seg4Reader = new BinaryReader(Seg4Stream);
            BinaryReader Seg6Reader = new BinaryReader(Seg6Stream);
            BinaryReader Seg7Reader = new BinaryReader(Seg7Stream);

            F3DEX095 F3D = new F3DEX095();

            Seg6Reader.BaseStream.Position = Offset;

            Aspose.ThreeD.Scene ExportData = new Aspose.ThreeD.Scene();


            MK64SurfaceStruct Surf = new MK64SurfaceStruct();
            while (true)
            {
                Surf.dlist = BitConverter.ToUInt32(F3D.BigEndian(Seg6Reader.ReadUInt32()), 0);
                Surf.type = Seg6Reader.ReadByte();
                Surf.area = Seg6Reader.ReadByte();
                Surf.flags = BitConverter.ToInt16(F3D.BigEndian(Seg6Reader.ReadInt16()), 0);

                if (Surf.dlist == 0)
                {
                    break;
                }

                uint VertexCache = 0;
                Seg7Reader.BaseStream.Position = Surf.dlist - 0x07000000;
                bool B8 = false;

                

                Aspose.ThreeD.Entities.Mesh WorkMesh = new Aspose.ThreeD.Entities.Mesh("Mesh" + Surf.dlist.ToString("X").ToString());

                while (!B8)
                {
                    byte CommandByte = Seg7Reader.ReadByte();
                    switch (CommandByte)
                    {
                        case (0x04):
                            {
                                //Load Vertex
                                Seg7Reader.BaseStream.Position += 3;
                                VertexCache = BitConverter.ToUInt32(F3D.BigEndian(Seg7Reader.ReadUInt32()), 0);
                                VertexCache -= 0x04000000; //Segmented Address
                                break;
                            }
                        case (0x06):
                            {
                                Seg7Reader.BaseStream.Position += 3;
                                uint Address = BitConverter.ToUInt32(F3D.BigEndian(Seg7Reader.ReadUInt32()), 0);
                                long SavePoint = Seg7Reader.BaseStream.Position;
                                RecursiveBullshit(Surf.dlist, Address, Seg4Reader, Seg6Reader, Seg7Reader, ExportData, WorkMesh);
                                Seg7Reader.BaseStream.Position = SavePoint;
                                break;
                            }
                        case (0xB1):
                            {



                                int Index;

                                for (int ThisFace = 0; ThisFace < 2; ThisFace++)
                                {
                                    int CurrentVertIndex = WorkMesh.ControlPoints.Count;
                                    for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                                    {
                                        Index = Seg7Reader.ReadByte() / 2;
                                        Seg4Reader.BaseStream.Position = VertexCache + (Index * 16);
                                        Vertex Vert = new Vertex();
                                        Vert.position = new Position();

                                        Vert.position.x = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                        Vert.position.z = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                        Vert.position.y = Convert.ToInt16(BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0) * -1);
                                        Seg4Reader.ReadInt16();//Padding
                                        Vert.position.sBase = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                        Vert.position.tBase = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);

                                        Vert.color = new OK64Color();
                                        Vert.color.R = Seg4Reader.ReadByte();
                                        Vert.color.G = Seg4Reader.ReadByte();
                                        Vert.color.B = Seg4Reader.ReadByte();
                                        Vert.color.A = Seg4Reader.ReadByte();

                                        Aspose.ThreeD.Utilities.Vector4 Positional = new Aspose.ThreeD.Utilities.Vector4();
                                        Positional.X = Vert.position.x;
                                        Positional.Y = Vert.position.y;
                                        Positional.Z = Vert.position.z;
                                        Positional.W = 1; //w for wut

                                        WorkMesh.ControlPoints.Add(Positional);


                                        float UValue = Vert.position.sBase / 32.0f / 32.0f;
                                        float VValue = Vert.position.tBase / 32.0f / 32.0f;

                                        // Create UVset
                                        //VertexElementUV elementUV = WorkMesh.CreateElementUV(Aspose.ThreeD.Entities.TextureMapping.Diffuse, MappingMode.PolygonVertex, ReferenceMode.IndexToDirect);
                                        // Copy the data to the UV vertex element 

                                        Aspose.ThreeD.Utilities.Vector4 UVWT = new Aspose.ThreeD.Utilities.Vector4
                                        (
                                            UValue,
                                            VValue,
                                            0.0f,
                                            1.0f
                                        );

                                        //elementUV.Data.Add(UVWT);

                                        // Create a vertex color element
                                        //VertexElementVertexColor colorElement = new VertexElementVertexColor();
                                        //colorElement.MappingMode = MappingMode.PolygonVertex;
                                        //colorElement.ReferenceMode = ReferenceMode.Direct;



                                        // Add vertex colors (Vector4 format)

                                        float RValue, GValue, BValue, AValue;
                                        RValue = Vert.color.R / 252.0f;
                                        GValue = Vert.color.G / 252.0f;
                                        BValue = Vert.color.B / 252.0f;
                                        AValue = Vert.color.A / 255.0f;
                                        //colorElement.Data.Add(new Aspose.ThreeD.Utilities.Vector4(RValue, GValue, BValue, AValue)); // Red
                                        //WorkMesh.VertexElements.Add(colorElement);
                                    }
                                    List<int> IndexList = new List<int>();
                                    IndexList.Add(CurrentVertIndex);
                                    IndexList.Add(CurrentVertIndex + 1);
                                    IndexList.Add(CurrentVertIndex + 2);

                                    WorkMesh.CreatePolygon(IndexList.ToArray());

                                    if (ThisFace == 0)
                                    {
                                        Seg7Reader.BaseStream.Position += 1;
                                    }

                                }



                                //Two Triangles
                                break;
                            }

                        case (0xBF):
                            {


                                Seg7Reader.BaseStream.Position += 4;
                                int Index;
                                int CurrentVertIndex = WorkMesh.ControlPoints.Count;

                                for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                                {
                                    Index = Seg7Reader.ReadByte() / 2;
                                    Seg4Reader.BaseStream.Position = VertexCache + (Index * 16);
                                    Vertex Vert = new Vertex();
                                    Vert.position = new Position();

                                    Vert.position.x = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                    Vert.position.z = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                    Vert.position.y = Convert.ToInt16(BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0) * -1);
                                    Seg4Reader.ReadInt16();//Padding
                                    Vert.position.sBase = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);
                                    Vert.position.tBase = BitConverter.ToInt16(F3D.BigEndian(Seg4Reader.ReadInt16()), 0);

                                    Vert.color = new OK64Color();
                                    Vert.color.R = Seg4Reader.ReadByte();
                                    Vert.color.G = Seg4Reader.ReadByte();
                                    Vert.color.B = Seg4Reader.ReadByte();
                                    Vert.color.A = Seg4Reader.ReadByte();

                                    Aspose.ThreeD.Utilities.Vector4 Positional = new Aspose.ThreeD.Utilities.Vector4();
                                    Positional.X = Vert.position.x;
                                    Positional.Y = Vert.position.y;
                                    Positional.Z = Vert.position.z;
                                    Positional.W = 1; //w for wut

                                    WorkMesh.ControlPoints.Add(Positional);


                                    // Create UVset
                                    //VertexElementUV elementUV = WorkMesh.CreateElementUV(Aspose.ThreeD.Entities.TextureMapping.Diffuse, MappingMode.PolygonVertex, ReferenceMode.IndexToDirect);
                                    // Copy the data to the UV vertex element 

                                    float UValue = Vert.position.sBase / 32.0f / 32.0f;
                                    float VValue = Vert.position.tBase / 32.0f / 32.0f;

                                    Aspose.ThreeD.Utilities.Vector4 UVWT = new Aspose.ThreeD.Utilities.Vector4
                                    (
                                        UValue,
                                        VValue,
                                        0.0f,
                                        1.0f
                                    );

                                    //elementUV.Data.Add(UVWT);
                                    // Create a vertex color element
                                    //VertexElementVertexColor colorElement = new VertexElementVertexColor();
                                    //colorElement.MappingMode = MappingMode.PolygonVertex;
                                    //colorElement.ReferenceMode = ReferenceMode.Direct;



                                    // Add vertex colors (Vector4 format)

                                    float RValue, GValue, BValue, AValue;
                                    RValue = Vert.color.R / 252.0f;
                                    GValue = Vert.color.G / 252.0f;
                                    BValue = Vert.color.B / 252.0f;
                                    AValue = Vert.color.A / 255.0f;
                                    //colorElement.Data.Add(new Aspose.ThreeD.Utilities.Vector4(RValue, GValue, BValue, AValue)); // Red
                                    //WorkMesh.VertexElements.Add(colorElement);
                                }
                                List<int> IndexList = new List<int>();
                                IndexList.Add(CurrentVertIndex);
                                IndexList.Add(CurrentVertIndex + 1);
                                IndexList.Add(CurrentVertIndex + 2);

                                WorkMesh.CreatePolygon(IndexList.ToArray());
                                //One Triangle
                                break;
                            }
                        case (0xB8):
                            {

                                Aspose.ThreeD.Node node = ExportData.RootNode.CreateChildNode("Mesh" + Surf.dlist.ToString("X").ToString(), WorkMesh);
                                Seg7Reader.BaseStream.Position += 7;
                                //End DL
                                B8 = true;
                                break;
                            }
                        default:
                            {
                                Seg7Reader.BaseStream.Position += 3;
                                //lolwut
                                break;
                            }

                    }
                }
            }

            SaveFileDialog SaveFile = new SaveFileDialog();
            if (SaveFile.ShowDialog() == DialogResult.OK)
            {

                try
                {

                    ExportData.Save(SaveFile.FileName, FileFormat.FBX7700ASCII);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Assimp Error: {ex.Message}");
                }

            }
        }

        public void ExportGeometry(byte[] Segment4, byte[] Segment7, byte[] Segment5, int TexWidth, int TexHeight)
        {
            TM64 Tarmac = new TM64();
            F3DEX095 F3D = new F3DEX095();
            byte[] Segment7Raw = Tarmac.Decompress_seg7(Segment7);
            byte[] Segment4Raw = Tarmac.DecompressMIO0(Segment4);
            byte[] VertexData = InflateVertex(Segment4Raw);

            MemoryStream VertexStream = new MemoryStream();
            BinaryReader VertexReader = new BinaryReader(VertexStream);
            VertexStream.Write(VertexData, 0, VertexData.Length);

            MemoryStream ModelStream = new MemoryStream();
            BinaryReader ModelReader = new BinaryReader(ModelStream);
            ModelStream.Write(Segment7Raw, 0, Segment7Raw.Length);

            VertexStream.Position = 0;
            ModelStream.Position = 0;


            uint VertexCache = 0;
            Aspose.ThreeD.Scene ExportData = new Aspose.ThreeD.Scene();


            Aspose.ThreeD.Entities.Mesh WorkMesh = new Aspose.ThreeD.Entities.Mesh();

            ModelReader.BaseStream.Position += 8;

            while (true)
            {
                if (ModelReader.BaseStream.Position >= ModelReader.BaseStream.Length)
                {
                    break;
                }

                byte CommandByte = ModelReader.ReadByte();
                switch(CommandByte)
                {
                    case (0x04):
                        {
                            //Load Vertex
                            ModelReader.BaseStream.Position += 3;
                            VertexCache = BitConverter.ToUInt32(F3D.BigEndian(ModelReader.ReadUInt32()), 0);
                            VertexCache -= 0x04000000; //Segmented Address
                            break;
                        }
                    case (0xB1):
                        {
                            
                            

                            int Index;
                            
                            for (int ThisFace = 0; ThisFace < 2; ThisFace++)
                            {
                                int CurrentVertIndex = WorkMesh.ControlPoints.Count;
                                for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                                {
                                    Index = ModelReader.ReadByte() / 2;
                                    VertexReader.BaseStream.Position = VertexCache + (Index * 16);
                                    Vertex Vert = new Vertex();
                                    Vert.position = new Position();
                                    
                                    Vert.position.x = BitConverter.ToInt16(F3D.BigEndian(VertexReader.ReadInt16()), 0);
                                    Vert.position.z = BitConverter.ToInt16(F3D.BigEndian(VertexReader.ReadInt16()), 0);
                                    Vert.position.y = Convert.ToInt16(BitConverter.ToInt16(F3D.BigEndian(VertexReader.ReadInt16()), 0) * -1);
                                    VertexReader.ReadInt16();//Padding
                                    Vert.position.sBase = BitConverter.ToInt16(F3D.BigEndian(VertexReader.ReadInt16()), 0);
                                    Vert.position.tBase = BitConverter.ToInt16(F3D.BigEndian(VertexReader.ReadInt16()), 0);

                                    Vert.color = new OK64Color();
                                    Vert.color.R = VertexReader.ReadByte();
                                    Vert.color.G = VertexReader.ReadByte();
                                    Vert.color.B = VertexReader.ReadByte();
                                    Vert.color.A = VertexReader.ReadByte();

                                    Aspose.ThreeD.Utilities.Vector4 Positional = new Aspose.ThreeD.Utilities.Vector4();
                                    Positional.X = Vert.position.x;
                                    Positional.Y = Vert.position.y;
                                    Positional.Z = Vert.position.z;
                                    Positional.W = 1; //w for wut

                                    WorkMesh.ControlPoints.Add(Positional);


                                    float UValue = Vert.position.sBase / 32.0f / TexWidth;
                                    float VValue = Vert.position.tBase / 32.0f / TexHeight;

                                    // Create UVset
                                    //VertexElementUV elementUV = WorkMesh.CreateElementUV(Aspose.ThreeD.Entities.TextureMapping.Diffuse, MappingMode.PolygonVertex, ReferenceMode.IndexToDirect);
                                    // Copy the data to the UV vertex element 

                                    Aspose.ThreeD.Utilities.Vector4 UVWT = new Aspose.ThreeD.Utilities.Vector4
                                    (
                                        UValue,
                                        VValue,
                                        0.0f,
                                        1.0f
                                    );

                                    //elementUV.Data.Add(UVWT);

                                    // Create a vertex color element
                                    //VertexElementVertexColor colorElement = new VertexElementVertexColor();
                                    //colorElement.MappingMode = MappingMode.PolygonVertex;
                                    //colorElement.ReferenceMode = ReferenceMode.Direct;



                                    // Add vertex colors (Vector4 format)

                                    float RValue, GValue, BValue, AValue;
                                    RValue = Vert.color.R / 252.0f;
                                    GValue = Vert.color.G / 252.0f;
                                    BValue = Vert.color.B / 252.0f;
                                    AValue = Vert.color.A / 255.0f;
                                    //colorElement.Data.Add(new Aspose.ThreeD.Utilities.Vector4(RValue, GValue, BValue, AValue)); // Red
                                    //WorkMesh.VertexElements.Add(colorElement);
                                }
                                List<int> IndexList = new List<int>();
                                IndexList.Add(CurrentVertIndex);
                                IndexList.Add(CurrentVertIndex + 1);
                                IndexList.Add(CurrentVertIndex + 2);

                                WorkMesh.CreatePolygon(IndexList.ToArray());

                                if (ThisFace == 0)
                                {
                                    ModelReader.BaseStream.Position += 1;
                                }
                                
                            }
                            


                            //Two Triangles
                            break;
                        }

                    case (0xBF):
                        {


                            ModelReader.BaseStream.Position += 4;
                            int Index;
                            int CurrentVertIndex = WorkMesh.ControlPoints.Count;

                            for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                            {
                                Index = ModelReader.ReadByte() / 2;
                                VertexReader.BaseStream.Position = VertexCache + (Index * 16);
                                Vertex Vert = new Vertex();
                                Vert.position = new Position();

                                Vert.position.x = BitConverter.ToInt16(F3D.BigEndian(VertexReader.ReadInt16()), 0);
                                Vert.position.z = BitConverter.ToInt16(F3D.BigEndian(VertexReader.ReadInt16()), 0);
                                Vert.position.y = BitConverter.ToInt16(F3D.BigEndian(VertexReader.ReadInt16() * -1), 0);
                                VertexReader.ReadInt16();//Padding
                                Vert.position.sBase = BitConverter.ToInt16(F3D.BigEndian(VertexReader.ReadInt16()), 0);
                                Vert.position.tBase = BitConverter.ToInt16(F3D.BigEndian(VertexReader.ReadInt16()), 0);

                                Vert.color = new OK64Color();
                                Vert.color.R = VertexReader.ReadByte();
                                Vert.color.G = VertexReader.ReadByte();
                                Vert.color.B = VertexReader.ReadByte();
                                Vert.color.A = VertexReader.ReadByte();

                                Aspose.ThreeD.Utilities.Vector4 Positional = new Aspose.ThreeD.Utilities.Vector4();
                                Positional.X = Vert.position.x;
                                Positional.Y = Vert.position.y;
                                Positional.Z = Vert.position.z;
                                Positional.W = 1; //w for wut

                                WorkMesh.ControlPoints.Add(Positional);


                                // Create UVset
                                //VertexElementUV elementUV = WorkMesh.CreateElementUV(Aspose.ThreeD.Entities.TextureMapping.Diffuse, MappingMode.PolygonVertex, ReferenceMode.IndexToDirect);
                                // Copy the data to the UV vertex element 

                                float UValue = Vert.position.sBase / 32.0f / TexWidth;
                                float VValue = Vert.position.tBase / 32.0f / TexHeight;

                                Aspose.ThreeD.Utilities.Vector4 UVWT = new Aspose.ThreeD.Utilities.Vector4
                                (
                                    UValue,
                                    VValue,
                                    0.0f,
                                    1.0f
                                );

                                //elementUV.Data.Add(UVWT);
                                // Create a vertex color element
                                //VertexElementVertexColor colorElement = new VertexElementVertexColor();
                                //colorElement.MappingMode = MappingMode.PolygonVertex;
                                //colorElement.ReferenceMode = ReferenceMode.Direct;



                                // Add vertex colors (Vector4 format)

                                float RValue, GValue, BValue, AValue;
                                RValue = Vert.color.R / 252.0f;
                                GValue = Vert.color.G / 252.0f;
                                BValue = Vert.color.B / 252.0f;
                                AValue = Vert.color.A / 255.0f;
                                //colorElement.Data.Add(new Aspose.ThreeD.Utilities.Vector4(RValue, GValue, BValue, AValue)); // Red
                                //WorkMesh.VertexElements.Add(colorElement);
                            }
                            List<int> IndexList = new List<int>();
                            IndexList.Add(CurrentVertIndex);
                            IndexList.Add(CurrentVertIndex + 1);
                            IndexList.Add(CurrentVertIndex + 2);

                            WorkMesh.CreatePolygon(IndexList.ToArray());
                            //One Triangle
                            break;
                        }
                    case (0xB8):
                        {
                            Aspose.ThreeD.Node node = ExportData.RootNode.CreateChildNode("Cube", WorkMesh);
                            
                            WorkMesh = new Aspose.ThreeD.Entities.Mesh("Mesh" + (ExportData.RootNode.ChildNodes.Count - 1).ToString());
                            
                            ModelReader.BaseStream.Position += 7;
                            //End DL
                            break;
                        }
                    default:
                        {
                            ModelReader.BaseStream.Position += 7;
                            //lolwut
                            break;
                        }

                }
            }



            SaveFileDialog SaveFile = new SaveFileDialog();
            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportData.Save(SaveFile.FileName, FileFormat.FBX7700ASCII);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Assimp Error: {ex.Message}");
                }

            }
        }


        public void CubeMeDaddyO()
        {
            // Initialize a new scene
            Aspose.ThreeD.Scene scene = new Aspose.ThreeD.Scene();
            Aspose.ThreeD.Node cubeNode = scene.RootNode.CreateChildNode("Cube");


            // Define cube vertices
            Aspose.ThreeD.Utilities.Vector4[] vertices = new Aspose.ThreeD.Utilities.Vector4[]
            {
            new Aspose.ThreeD.Utilities.Vector4(-1, -1,  1), // 0
            new Aspose.ThreeD.Utilities.Vector4( 1, -1,  1), // 1
            new Aspose.ThreeD.Utilities.Vector4( 1,  1,  1), // 2
            new Aspose.ThreeD.Utilities.Vector4(-1,  1,  1), // 3
            new Aspose.ThreeD.Utilities.Vector4(-1, -1, -1), // 4
            new Aspose.ThreeD.Utilities.Vector4( 1, -1, -1), // 5
            new Aspose.ThreeD.Utilities.Vector4( 1,  1, -1), // 6
            new Aspose.ThreeD.Utilities.Vector4(-1,  1, -1)  // 7
            };

            // Define cube faces using triangles (two per quad)
            int[] indices = new int[]
            {
            0, 1, 2,  2, 3, 0,  // Front
            1, 5, 6,  6, 2, 1,  // Right
            5, 4, 7,  7, 6, 5,  // Back
            4, 0, 3,  3, 7, 4,  // Left
            3, 2, 6,  6, 7, 3,  // Top
            4, 5, 1,  1, 0, 4   // Bottom
            };

            // Define UV coordinates
            Aspose.ThreeD.Utilities.Vector4[] uvs = new Aspose.ThreeD.Utilities.Vector4[]
            {
                new Aspose.ThreeD.Utilities.Vector4(1, 0, 0, 1), // Red
                new Aspose.ThreeD.Utilities.Vector4(0, 1, 0, 1), // Green
                new Aspose.ThreeD.Utilities.Vector4(0, 0, 0, 1), // Blue
                new Aspose.ThreeD.Utilities.Vector4(1, 1, 0, 1), // Yellow
                new Aspose.ThreeD.Utilities.Vector4(1, 0, 0, 1), // Red
                new Aspose.ThreeD.Utilities.Vector4(0, 1, 0, 1), // Green
                new Aspose.ThreeD.Utilities.Vector4(0, 0, 0, 1), // Blue
                new Aspose.ThreeD.Utilities.Vector4(1, 1, 0, 1), // Yellow
            };
            // Define vertex colors
            Aspose.ThreeD.Utilities.Vector4[] colors = new Aspose.ThreeD.Utilities.Vector4[]
            {
                new Aspose.ThreeD.Utilities.Vector4(1, 0, 0, 1), // Red
                new Aspose.ThreeD.Utilities.Vector4(0, 1, 0, 1), // Green
                new Aspose.ThreeD.Utilities.Vector4(0, 0, 1, 1), // Blue
                new Aspose.ThreeD.Utilities.Vector4(1, 1, 0, 1), // Yellow
                new Aspose.ThreeD.Utilities.Vector4(1, 0, 1, 1), // Magenta
                new Aspose.ThreeD.Utilities.Vector4(0, 1, 1, 1), // Cyan
                new Aspose.ThreeD.Utilities.Vector4(1, 1, 1, 1), // White
                new Aspose.ThreeD.Utilities.Vector4(0, 0, 0, 1)  // Black
            };

            Aspose.ThreeD.Entities.Mesh mesh = new Aspose.ThreeD.Entities.Mesh();
            mesh.ControlPoints.AddRange(vertices);
            // Add vertex positions

            // Add vertex colors
            VertexElementVertexColor colorElement = mesh.CreateElement(VertexElementType.VertexColor, MappingMode.ControlPoint, ReferenceMode.Direct) as VertexElementVertexColor;
            colorElement.SetData(new Aspose.ThreeD.Utilities.Vector4[]
            {
                new Aspose.ThreeD.Utilities.Vector4(1, 0, 0, 1), // Red
                new Aspose.ThreeD.Utilities.Vector4(0, 1, 0, 1), // Green
                new Aspose.ThreeD.Utilities.Vector4(0, 0, 1, 1), // Blue
                new Aspose.ThreeD.Utilities.Vector4(1, 1, 0, 1), // Yellow
                new Aspose.ThreeD.Utilities.Vector4(1, 0, 1, 1), // Magenta
                new Aspose.ThreeD.Utilities.Vector4(0, 1, 1, 1), // Cyan
                new Aspose.ThreeD.Utilities.Vector4(1, 1, 1, 1), // White
                new Aspose.ThreeD.Utilities.Vector4(0, 0, 0, 1)  // Black
            });

            // Add UV coordinates
            VertexElementUV uvElement = mesh.CreateElement(VertexElementType.UV) as VertexElementUV;
            uvElement.Data.AddRange(uvs);

            // Define triangle faces
            mesh.CreatePolygon(indices);

            // Attach mesh to the node
            cubeNode.Entity = mesh;

            // Save the scene
            SaveFileDialog FileSave = new SaveFileDialog();
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                string outputPath = "CubeWithColorsAndUVs.fbx";
                scene.Save(FileSave.FileName, FileFormat.FBX7500ASCII);
            }
            

        }
        public string[] WriteGeometryRSP(TM64_Geometry.OK64F3DObject TargetObject, TM64_Texture.OK64Texture TextureObject, string GraphPtr)
        {
            List<string> Output = new List<string>();
            F3DSharp.F3DEX095 F3D = new F3DSharp.F3DEX095();

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

