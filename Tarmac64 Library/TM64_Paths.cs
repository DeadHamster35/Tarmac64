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
using Assimp;

using F3DSharp;
using SharpDX;
using System.Xml;

namespace Tarmac64_Library
{
    public class TM64_Paths
    {

        byte[] dataBytes = new byte[0];
        MemoryStream memoryStream = new MemoryStream();
        BinaryReader binaryReader = new BinaryReader(Stream.Null);
        BinaryWriter binaryWriter = new BinaryWriter(Stream.Null);
        F3DEX095 F3D = new F3DEX095();

        public class Pathlist
        {
            public Pathlist()
            {

            }
            public Pathlist(XmlDocument XMLDoc, string Parent, int PathIndex)
            {
                TM64 Tarmac = new TM64();
                XmlNode Owner = XMLDoc.SelectSingleNode(Parent);
                string TargetPath = Parent + "/Path_" + PathIndex.ToString();
                Random RNG = new Random();

                float[] Color = new float[3]
                {
                    RNG.NextFloat(0,1),
                    RNG.NextFloat(0,1),
                    RNG.NextFloat(0,1)
                };

                int MarkerCount = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, TargetPath, "MarkerCount", "0"));
                pathmarker = new List<Marker>();
                for (int ThisMarker = 0; ThisMarker < MarkerCount; ThisMarker++)
                {
                    pathmarker.Add(new Marker());
                    int[] Pos = Tarmac.LoadElements(XMLDoc, TargetPath, "MarkerPosition_" + ThisMarker.ToString(), "0");

                    pathmarker[ThisMarker].X = Convert.ToInt16(Pos[0]);
                    pathmarker[ThisMarker].Y = Convert.ToInt16(Pos[1]);
                    pathmarker[ThisMarker].Z = Convert.ToInt16(Pos[2]);

                    pathmarker[ThisMarker].Flag = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, TargetPath, "Flag_" + ThisMarker.ToString(), "0"));

                    pathmarker[ThisMarker].Color = Color;
                }

            }
            public void SaveXML(XmlDocument XMLDoc, XmlElement Parent, int PathIndex)
            {
                TM64 Tarmac = new TM64();
                XmlElement PathXML = XMLDoc.CreateElement("Path_" + PathIndex.ToString());
                Parent.AppendChild(PathXML);

                Tarmac.GenerateElement(XMLDoc, PathXML, "MarkerCount", pathmarker.Count);
                for (int ThisMark = 0; ThisMark < pathmarker.Count; ThisMark++) 
                {
                    int[] PositionArray = new int[3]
                    {
                        pathmarker[ThisMark].X,
                        pathmarker[ThisMark].Y,
                        pathmarker[ThisMark].Z,
                    };
                    Tarmac.GenerateElement(XMLDoc, PathXML, "MarkerPosition_"+ThisMark.ToString(), PositionArray);
                    Tarmac.GenerateElement(XMLDoc, PathXML, "Flag_" + ThisMark.ToString(), pathmarker[ThisMark].Flag);

                }
                

            }



            public List<Marker> pathmarker { get; set; }
            public void Add(short[] PositionArray)
            {
                pathmarker.Add(new Marker(PositionArray));
            }
            public void Add(TM64_Course.OKObject Object)
            {
                Marker ThisMark = new Marker(Object.OriginPosition);
                ThisMark.Flag = Object.Flag;
                pathmarker.Add(ThisMark);
            }
        }
        public class Marker
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int Flag { get; set; }
            public float[] Color { get; set; }
            public Marker()
            {

            }
            public Marker(short[] PositionArray)
            {
                X = PositionArray[0];
                Y = PositionArray[1];
                Z = PositionArray[2];
            }
        }

        public class BattleMarker
        {

            public int xval { get; set; }
            public int yval { get; set; }
            public int zval { get; set; }
            public int flag { get; set; }
            public int Player { get; set; }
            public int Type { get; set; }
            public float[] Color { get; set; }

        }

        public Pathlist[] LoadPOP3(string popFile, TM64_Geometry.OK64F3DObject[] surfaceObjects)
        {
            //load the pathgroups from the external .OK64.POP file provided

            TM64_Geometry tm64Geo = new TM64_Geometry();
            
            string[] FileData = File.ReadAllLines(popFile);

            


            int currentLine = 1;
            string Count = FileData[currentLine++];
            //int PathCount = Convert.ToInt32(FileData[currentLine++]);
            int PathCount = Convert.ToInt32(Count);
            Pathlist[] PathArray = new Pathlist[PathCount];

            for (int PathIndex = 0; PathIndex < PathCount; PathIndex++)
            {
                PathArray[PathIndex] = new Pathlist();

                int MarkerCount = Int32.Parse(FileData[currentLine++]);

                PathArray[PathIndex].pathmarker = new List<Marker>();

                int currentSection = 0;                    
                Random RNG = new Random();
                float[] MColor = new float[3] { 0, 0, 0 };
                    

                for (int CurrentMarker = 0; CurrentMarker < MarkerCount; CurrentMarker++)
                {
                    PathArray[PathIndex].pathmarker.Add(new Marker());

                    string ParseLine = FileData[currentLine++];
                    ParseLine = ParseLine.Replace("[", "");
                    ParseLine = ParseLine.Replace("]", "");
                    
                    // This strips the brackets from the first line

                    string[] PositionArray = ParseLine.Split(',');
                    // This creates an array containing the marker positions as strings.

                    PathArray[PathIndex].pathmarker[CurrentMarker].X = Convert.ToInt32(Single.Parse(PositionArray[0]));
                    PathArray[PathIndex].pathmarker[CurrentMarker].Y = Convert.ToInt32(Single.Parse(PositionArray[1]));
                    PathArray[PathIndex].pathmarker[CurrentMarker].Z = Convert.ToInt32(Single.Parse(PositionArray[2]));

                    //maintain Z/Y axis, we flip it only when writing to the ROM.
                    PathArray[PathIndex].pathmarker[CurrentMarker].Flag = Convert.ToInt32(FileData[currentLine++]);
                    //Read the next line, convert to int. This is the accompanying Flag for the marker. 


                    float[] pointA = new float[3];
                    pointA[0] = Convert.ToSingle(PathArray[PathIndex].pathmarker[CurrentMarker].X);
                    pointA[1] = Convert.ToSingle(PathArray[PathIndex].pathmarker[CurrentMarker].Y);
                    pointA[2] = Convert.ToSingle(PathArray[PathIndex].pathmarker[CurrentMarker].Z + 20);



                    //custom Path Flag routine
                    //uses surfaceObjects and raycasts to determine appropriate surface section.
                    Vector3D rayOrigin = new Vector3D(Convert.ToSingle(pointA[0]), Convert.ToSingle(pointA[1]), Convert.ToSingle(pointA[2]));
                    Vector3D rayTarget = new Vector3D(0, 0, -1);


                    float objectDistance = -1;
                    TM64_Geometry tmGeo = new TM64_Geometry();
                    int objectID = -1;

                    for (int currentObject = 0; (currentObject < surfaceObjects.Length); currentObject++)
                    {

                        foreach (var face in surfaceObjects[currentObject].modelGeometry)
                        {

                            Vector3D intersectPoint = tmGeo.testIntersect(rayOrigin, rayTarget, face.VertData[0], face.VertData[1], face.VertData[2]);
                            if (intersectPoint.X > 0)
                            {
                                if (objectDistance > intersectPoint.X | objectDistance == -1)
                                {
                                    objectDistance = Convert.ToSingle(intersectPoint.X);
                                    objectID = currentObject;
                                }
                            }
                        }
                    }
                    if (objectID >= 0)
                    {
                        PathArray[PathIndex].pathmarker[CurrentMarker].Flag = Convert.ToInt32(surfaceObjects[objectID].surfaceID);
                    }
                    else
                    {
                        if (CurrentMarker > 0)
                        {
                            PathArray[PathIndex].pathmarker[CurrentMarker].Flag = PathArray[PathIndex].pathmarker[CurrentMarker - 1].Flag;
                        }
                        else
                        {
                            PathArray[PathIndex].pathmarker[CurrentMarker].Flag = 1;
                        }
                    }

                    if (PathArray[PathIndex].pathmarker[CurrentMarker].Flag != currentSection)
                    {
                        currentSection = PathArray[PathIndex].pathmarker[CurrentMarker].Flag;
                        MColor = new float[] { RNG.NextFloat(0,1), RNG.NextFloat(0, 1), RNG.NextFloat(0, 1), };
                    }

                    PathArray[PathIndex].pathmarker[CurrentMarker].Color = MColor;
                }
            
            }

            return PathArray;


        }


        public byte[] popMarker(Pathlist ThisList, int PaddingLength)
        {


            //popMarkers is used by the geometry compiler, not to add objects to existing courses.
            
            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);



            int markerCount = ThisList.pathmarker.Count;


            for (int currentMarker = 0; currentMarker < markerCount; currentMarker++)
            {

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(ThisList.pathmarker[currentMarker].X));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //x

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(ThisList.pathmarker[currentMarker].Z));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //z

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(-1 * ThisList.pathmarker[currentMarker].Y));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //y 

                dataBytes = BitConverter.GetBytes(Convert.ToUInt16(ThisList.pathmarker[currentMarker].Flag));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //flag

            }

            dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
            Array.Reverse(dataBytes);
            binaryWriter.Write(dataBytes);  //end list

            dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
            Array.Reverse(dataBytes);
            binaryWriter.Write(dataBytes);  //end path

            dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
            Array.Reverse(dataBytes);
            binaryWriter.Write(dataBytes);  //end path
           
            dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
            Array.Reverse(dataBytes);
            binaryWriter.Write(dataBytes);  //end flag



            int localPad = PaddingLength - markerCount;
            for (int currentMarker = 0; currentMarker < localPad; currentMarker++)
            {

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //pad

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //pad

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //pad

                dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //pad
            }
            
            byte[] popBytes = memoryStream.ToArray();
            return popBytes;

        }

        public byte[] popMarkerBattleObjective(List<BattleMarker> ThisList, int PaddingLength)
        {


            //popMarkers is used by the geometry compiler, not to add objects to existing courses.

            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);



            int markerCount = ThisList.Count;


            for (int currentMarker = 0; currentMarker < markerCount; currentMarker++)
            {
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ThisList[currentMarker].xval)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ThisList[currentMarker].zval)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(-1 * ThisList[currentMarker].yval)));

                binaryWriter.Write(F3D.BigEndian(Convert.ToUInt16(ThisList[currentMarker].flag)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToUInt16(ThisList[currentMarker].Player)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToUInt16(ThisList[currentMarker].Type)));
            }

            binaryWriter.Write(F3D.BigEndian(Convert.ToUInt16(0x8000)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToUInt16(0x8000)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToUInt16(0x8000)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(0)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(0)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(0)));



            int localPad = PaddingLength - markerCount;
            for (int currentMarker = 0; currentMarker < localPad; currentMarker++)
            {
                binaryWriter.Write(F3D.BigEndian(Convert.ToUInt16(0x8000)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToUInt16(0x8000)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToUInt16(0x8000)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(0)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(0)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(0)));
            }

            byte[] popBytes = memoryStream.ToArray();
            return popBytes;

        }


        public byte[] popMarkerFlat(Pathlist ThisList, int PaddingLength)
        {


            //popMarkers is used by the geometry compiler, not to add objects to existing courses.

            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);



            int markerCount = ThisList.pathmarker.Count;


            for (int currentMarker = 0; currentMarker < markerCount; currentMarker++)
            {

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(ThisList.pathmarker[currentMarker].X));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //x

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //z

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(-1 * ThisList.pathmarker[currentMarker].Y));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //y 

                dataBytes = BitConverter.GetBytes(Convert.ToUInt16(ThisList.pathmarker[currentMarker].Flag));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //flag

            }

            dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
            Array.Reverse(dataBytes);
            binaryWriter.Write(dataBytes);  //end list

            dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
            Array.Reverse(dataBytes);
            binaryWriter.Write(dataBytes);  //end path

            dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
            Array.Reverse(dataBytes);
            binaryWriter.Write(dataBytes);  //end path

            dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
            Array.Reverse(dataBytes);
            binaryWriter.Write(dataBytes);  //end flag



            int localPad = PaddingLength - markerCount;
            for (int currentMarker = 0; currentMarker < localPad; currentMarker++)
            {

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //pad

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //pad

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //pad

                dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //pad
            }

            byte[] popBytes = memoryStream.ToArray();
            return popBytes;

        }



    }
}
