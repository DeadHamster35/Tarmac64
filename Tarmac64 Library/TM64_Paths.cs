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
            public List<Marker> pathmarker { get; set; }

            public byte[] SaveData()
            {
                MemoryStream Data = new MemoryStream();
                BinaryWriter BWrite = new BinaryWriter(Data);

                BWrite.Write(pathmarker.Count);
                for (int ThisMarker = 0; ThisMarker < pathmarker.Count; ThisMarker++)
                {

                }
                return Data.ToArray();
            }
            public void Add(short[] PositionArray)
            {
                pathmarker.Add(new Marker(PositionArray));
            }
            public void Add(TM64_Course.OKObject Object)
            {
                Marker ThisMark = new Marker(Object.OriginPosition);
                ThisMark.flag = Object.Flag;
                pathmarker.Add(ThisMark);
            }
        }
        public class Marker
        {

            public int xval { get; set; }
            public int yval { get; set; }
            public int zval { get; set; }
            public int flag { get; set; }
            public float[] Color { get; set; }
            public Marker()
            {

            }
            public Marker(short[] PositionArray)
            {
                xval = PositionArray[0];
                yval = PositionArray[1];
                zval = PositionArray[2];
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

        public Pathgroup[] loadPOP(string popFile, TM64_Geometry.OK64F3DObject[] surfaceObjects)
        {
            //load the pathgroups from the external .OK64.POP file provided

            TM64_Geometry tm64Geo = new TM64_Geometry();

            List<Pathgroup> pathgroup = new List<Pathgroup>();
            
            string[] reader = File.ReadAllLines(popFile);
            string[] positions = new string[3];

            int[] markerCount = new int[5];




            int currentLine = 0;

            for (int group = 0; group < 5; group++)
            {
                pathgroup.Add(new Pathgroup());
                List<Pathlist> tempList = new List<Pathlist>();
                   
                if (currentLine < reader.Length)
                {
                    string pathType = reader[currentLine];
                    currentLine++;

                    int Pmax = Convert.ToInt32(reader[currentLine]);
                    currentLine++;

                    for (int pindex = 0; pindex < Pmax; pindex++)
                    {
                        tempList.Add(new Pathlist());
                        tempList[pindex].pathmarker = new List<Marker>();

                        markerCount[group] = Int32.Parse(reader[currentLine]);
                        currentLine++;




                        int currentSection = 0;
                        float[] MColor = { Convert.ToSingle(1.0), Convert.ToSingle(1.0), Convert.ToSingle(0.0) };
                        Random Rando = new Random();
                        for (int marker = 0; marker < markerCount[group]; marker++)
                        {
                            tempList[pindex].pathmarker.Add(new Marker());
                            // input format

                            //[xposition,yposition,zposition]
                            //flag

                            // Flag for Path should correlate with section.
                            // Flag for objects will almost always be 0. Unsure of effect. 

                            string lineRead = reader[currentLine].Substring(1, (reader[currentLine].Length - 2));
                            // This strips the brackets from the first line

                            string[] markerPosition = lineRead.Split(',');
                            // This creates an array containing the marker positions as strings.

                            currentLine++;
                            // Advance forward in the file.

                            tempList[pindex].pathmarker[marker].xval = Convert.ToInt32(Single.Parse(markerPosition[0]));
                            tempList[pindex].pathmarker[marker].yval = Convert.ToInt32(Single.Parse(markerPosition[1]));
                            tempList[pindex].pathmarker[marker].zval = Convert.ToInt32(Single.Parse(markerPosition[2]));

                            //maintain Z/Y axis, we flip it only when writing to the ROM.
                            tempList[pindex].pathmarker[marker].flag = Convert.ToInt32(reader[currentLine]);
                            //Read the next line, convert to int. This is the accompanying Flag for the marker. 


                            currentLine++;
                            // Advance forward in the file.

                            float[] pointA = new float[3];
                            pointA[0] = Convert.ToSingle(tempList[pindex].pathmarker[marker].xval);
                            pointA[1] = Convert.ToSingle(tempList[pindex].pathmarker[marker].yval);
                            pointA[2] = Convert.ToSingle(tempList[pindex].pathmarker[marker].zval + 20);
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
                                tempList[pindex].pathmarker[marker].flag = Convert.ToInt32(surfaceObjects[objectID].surfaceID);
                            }
                            else
                            {
                                if (marker > 0)
                                {
                                    tempList[pindex].pathmarker[marker].flag = tempList[pindex].pathmarker[marker - 1].flag;
                                }
                                else
                                {
                                    tempList[pindex].pathmarker[marker].flag = 1;
                                }
                            }

                            if (tempList[pindex].pathmarker[marker].flag != currentSection)
                            {
                                currentSection = tempList[pindex].pathmarker[marker].flag;
                                MColor = new float[] { Convert.ToSingle(Rando.NextDouble()), Convert.ToSingle(Rando.NextDouble()), Convert.ToSingle(Rando.NextDouble()) };
                            }

                            tempList[pindex].pathmarker[marker].Color = MColor;
                        }
                    }
                }

                pathgroup[group].pathList = tempList.ToArray();
            }
            Pathgroup[] popPath = pathgroup.ToArray();
            return popPath;

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

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(ThisList.pathmarker[currentMarker].xval));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //x

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(ThisList.pathmarker[currentMarker].zval));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //z

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(-1 * ThisList.pathmarker[currentMarker].yval));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //y 

                dataBytes = BitConverter.GetBytes(Convert.ToUInt16(ThisList.pathmarker[currentMarker].flag));
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

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(ThisList.pathmarker[currentMarker].xval));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //x

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //z

                dataBytes = BitConverter.GetBytes(Convert.ToInt16(-1 * ThisList.pathmarker[currentMarker].yval));
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);  //y 

                dataBytes = BitConverter.GetBytes(Convert.ToUInt16(ThisList.pathmarker[currentMarker].flag));
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
