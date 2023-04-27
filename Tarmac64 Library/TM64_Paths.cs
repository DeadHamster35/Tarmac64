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
        public class Pathgroup
        {
            public Pathlist[] pathList { get; set; }
        }
        public class Pathlist
        {
            public List<Marker> pathmarker { get; set; }
            public string pathName { get; set; }
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

        public int[] loadmarkerOffsets(int cID, byte[] fileData)
        {


            memoryStream = new MemoryStream(fileData);
            binaryReader = new BinaryReader(memoryStream);
            int[] markerOffset = new int[4];


            binaryReader.BaseStream.Position = (0xDD4D0 + (0x10 * cID));

            for (int currentPath = 0; currentPath < 4; currentPath++)
            {
                dataBytes = binaryReader.ReadBytes(4);
                Array.Reverse(dataBytes);

                // Check for 0x800DC778 pointer. 
                //dont subtract the Segment offset if this is the empty path RAM pointer. 
                // This value points to an offset in RAM with no path markers.
                // The game uses this as a generic "empty list" for any path that isn't used.
                int debugTest = (BitConverter.ToInt32(dataBytes, 0));
                if (BitConverter.ToInt32(dataBytes, 0) != -2146580616)
                {
                    markerOffset[currentPath] = BitConverter.ToInt32(dataBytes, 0) - 0x06000000;
                }
                else
                {
                    markerOffset[currentPath] = BitConverter.ToInt32(dataBytes, 0);
                }
            }

            return markerOffset;
        }
        public byte[] savemarkerOffsets(int cID, byte[] fileData, int[] markerOffsets)
        {


            memoryStream = new MemoryStream();
            memoryStream.Write(fileData, 0, fileData.Length);
            binaryWriter = new BinaryWriter(memoryStream);


            binaryReader.BaseStream.Position = (0xDD4D0 + (0x10 * cID));

            for (int currentPath = 0; currentPath < 4; currentPath++)
            {
                if (markerOffsets[currentPath] == 0)
                {
                    binaryWriter.Write(0x800DC778);
                }
                dataBytes = BitConverter.GetBytes(markerOffsets[currentPath]);                
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);
                // Check for 0x800DC778 pointer. 
                //dont subtract the Segment offset if this is the empty path RAM pointer. 
                // This value points to an offset in RAM with no path markers.
                // The game uses this as a generic "empty list" for any path that isn't used.
                
            }
            byte[] outputData = memoryStream.ToArray();

            return outputData;
        }
        public int[] loadmarkerCount(int cID, byte[] fileData)
        {


            memoryStream = new MemoryStream(fileData);
            binaryReader = new BinaryReader(memoryStream);

            int[] markerCount = new int[4];
            binaryReader.BaseStream.Position = (0xDE5D0 + (0x10 * cID));

            for (int currentPath = 0; currentPath < 4; currentPath++)
            {
                dataBytes = binaryReader.ReadBytes(2);
                Array.Reverse(dataBytes);
                markerCount[currentPath] = BitConverter.ToInt16(dataBytes, 0);
            }
            return markerCount;
        }
        public byte[] savemarkerCount(int cID, byte[] fileData, int[] markerCount)
        {


            memoryStream = new MemoryStream();
            memoryStream.Write(fileData, 0, fileData.Length);
            binaryWriter = new BinaryWriter(memoryStream);

            binaryReader.BaseStream.Position = (0xDE5D0 + (0x10 * cID));

            for (int currentPath = 0; currentPath < 4; currentPath++)
            {
                dataBytes = BitConverter.GetBytes(markerCount[currentPath]);
                Array.Reverse(dataBytes);
                binaryWriter.Write(dataBytes);
            }

            byte[] outputData = memoryStream.ToArray();

            return outputData;
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
                            pointA[2] = Convert.ToSingle(tempList[pindex].pathmarker[marker].zval + 50);
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
                            if (objectID > 0)
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
        public Pathgroup[] loadPOP(string popFile)
        {
            //load the pathgroups from the external .OK64.POP file provided


            List<Pathgroup> pathgroup = new List<Pathgroup>();
            string[] reader = File.ReadAllLines(popFile);
            string[] positions = new string[3];

            int[] markerCount = new int[5];




            int currentLine = 0;

            for (int group = 0; group < 5; group++)
            {
                pathgroup.Add(new Pathgroup());
                List<Pathlist> tempList = new List<Pathlist>();



                tempList.Add(new Pathlist());
                tempList[0].pathmarker = new List<Marker>();

                string pathType = reader[currentLine];
                currentLine++;

                markerCount[group] = Int32.Parse(reader[currentLine]);
                currentLine++;


                for (int marker = 0; marker < markerCount[group]; marker++)
                {
                    tempList[0].pathmarker.Add(new Marker());


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

                    tempList[0].pathmarker[marker].xval = Convert.ToInt32(Single.Parse(markerPosition[0]));
                    tempList[0].pathmarker[marker].yval = Convert.ToInt32(Single.Parse(markerPosition[1]));
                    tempList[0].pathmarker[marker].zval = Convert.ToInt32(Single.Parse(markerPosition[2]));

                    //maintain Z/Y axis, we flip it only when writing to the ROM.
                    tempList[0].pathmarker[marker].flag = Convert.ToInt32(reader[currentLine]);
                    //Read the next line, convert to int. This is the accompanying Flag for the marker. 

                    currentLine++;
                    // Advance forward in the file.
                }

                pathgroup[group].pathList = tempList.ToArray();
            }
            Pathgroup[] popPath = pathgroup.ToArray();
            return popPath;

        }
        public Pathgroup[] loadBattlePOP(string popFile)
        {
            //load the pathgroups from the external .OK64.POP file provided


            List<Pathgroup> pathgroup = new List<Pathgroup>();
            string[] reader = File.ReadAllLines(popFile);
            string[] positions = new string[3];

            int[] markerCount = new int[2];


            int group = 0;

            int currentLine = 0;

            pathgroup.Add(new Pathgroup());
            List<Pathlist> tempList = new List<Pathlist>();



            tempList.Add(new Pathlist());
            tempList[0].pathmarker = new List<Marker>();

            string pathType = reader[currentLine];
            currentLine++;

            markerCount[group] = Int32.Parse(reader[currentLine]);
            currentLine++;


            for (int marker = 0; marker < markerCount[group]; marker++)
            {
                tempList[0].pathmarker.Add(new Marker());


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

                tempList[0].pathmarker[marker].xval = Convert.ToInt32(Single.Parse(markerPosition[0]));
                tempList[0].pathmarker[marker].yval = Convert.ToInt32(Single.Parse(markerPosition[1]));
                tempList[0].pathmarker[marker].zval = Convert.ToInt32(Single.Parse(markerPosition[2]));

                //maintain Z/Y axis, we flip it only when writing to the ROM.
                tempList[0].pathmarker[marker].flag = Convert.ToInt32(reader[currentLine]);
                //Read the next line, convert to int. This is the accompanying Flag for the marker. 
                tempList[0].pathmarker[marker].Color = new float[3] { 1,1,0};
                currentLine++;
                // Advance forward in the file.
            }

            pathgroup[group].pathList = tempList.ToArray();
            Pathgroup[] popPath = pathgroup.ToArray();
            return popPath;

        }
        public byte[] savePOP(Pathgroup[] coursePaths)
        {

            //This will take a Pathgroup array and write it out to a POP file. 
            
            
            string[] positions = new string[3];
            List<string> outputFile = new List<string>();

            
            int[] markerCount = new int[4];

            memoryStream = new MemoryStream();
            TextWriter textOutput = new StreamWriter(memoryStream);


            for (int currentGroup = 0; currentGroup < 4; currentGroup++)
            {
                for (int currentPath = 0; currentPath < coursePaths[currentGroup].pathList.Length; currentPath++)
                {
                    textOutput.Write(coursePaths[currentGroup].pathList[currentPath].pathName);
                    textOutput.Write(Environment.NewLine);
                    textOutput.Write(coursePaths[currentGroup].pathList[currentPath].pathmarker.Count.ToString());
                    textOutput.Write(Environment.NewLine);

                    for (int currentMarker = 0; currentMarker < coursePaths[currentGroup].pathList[currentPath].pathmarker.Count; currentMarker++)
                    {

                        //
                        //
                        textOutput.Write("[" + coursePaths[currentGroup].pathList[currentPath].pathmarker[currentMarker].xval.ToString()
                            + "," + coursePaths[currentGroup].pathList[currentPath].pathmarker[currentMarker].yval.ToString()
                            + "," + coursePaths[currentGroup].pathList[currentPath].pathmarker[currentMarker].zval.ToString() + "]");
                        textOutput.Write(Environment.NewLine);
                        //
                        //
                        //


                        textOutput.Write(coursePaths[currentGroup].pathList[currentPath].pathmarker[currentMarker].flag.ToString());
                        textOutput.Write(Environment.NewLine);
                    } 
                }

            }

            return memoryStream.ToArray();

        }
        //This returns a byte array with the OK64.POP file's paths and objects in their proper format.
        //This needs to be placed at a specific location and that location needs to be updated in ASM.
        public byte[] popMarkers(string popFile)
        {


            //popMarkers is used by the geometry compiler, not to add objects to existing courses.
            Pathgroup[] pathgroup = loadPOP(popFile);


            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);

            int groupCount = pathgroup.Length;
            if (groupCount == 5)
            {
                int[] paddingLength = new int[5] { pathgroup[0].pathList[0].pathmarker.Count, 64, 64, 64, 8 };

                for (int currentGroup = 5; currentGroup < groupCount; currentGroup--)
                {

                    int markerCount = pathgroup[currentGroup].pathList[0].pathmarker.Count;

                    int DoubleTake = 1;
                    if (currentGroup == 0)
                    {
                        DoubleTake = 2; //If path markers, do two passes to write the second set of path data for shells. 
                    }
                    for (int ThisPass = 0; ThisPass < DoubleTake; ThisPass++)
                    {
                        for (int currentMarker = 0; currentMarker < markerCount; currentMarker++)
                        {

                            dataBytes = BitConverter.GetBytes(Convert.ToInt16(pathgroup[currentGroup].pathList[0].pathmarker[currentMarker].xval));
                            Array.Reverse(dataBytes);
                            binaryWriter.Write(dataBytes);  //x


                            dataBytes = BitConverter.GetBytes(Convert.ToInt16(pathgroup[currentGroup].pathList[0].pathmarker[currentMarker].zval));
                            Array.Reverse(dataBytes);
                            binaryWriter.Write(dataBytes);  //z

                            dataBytes = BitConverter.GetBytes(Convert.ToInt16(-1 * pathgroup[currentGroup].pathList[0].pathmarker[currentMarker].yval));
                            Array.Reverse(dataBytes);
                            binaryWriter.Write(dataBytes);  //y 

                            dataBytes = BitConverter.GetBytes(Convert.ToUInt16(pathgroup[currentGroup].pathList[0].pathmarker[currentMarker].flag));
                            Array.Reverse(dataBytes);
                            binaryWriter.Write(dataBytes);  //flag

                        }

                        dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                        Array.Reverse(dataBytes);
                        binaryWriter.Write(dataBytes);  //end list

                        if (currentGroup == 0)  //group 0 is course paths, groups 1-3 are objects
                        {
                            dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                            Array.Reverse(dataBytes);
                            binaryWriter.Write(dataBytes);  //end path

                            dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                            Array.Reverse(dataBytes);
                            binaryWriter.Write(dataBytes);  //end path
                        }
                        else
                        {

                            dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                            Array.Reverse(dataBytes);
                            binaryWriter.Write(dataBytes);  //end object

                            dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                            Array.Reverse(dataBytes);
                            binaryWriter.Write(dataBytes);  //end object
                        }


                        dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                        Array.Reverse(dataBytes);
                        binaryWriter.Write(dataBytes);  //end flag



                        int localPad = paddingLength[currentGroup] - markerCount;
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
                    }
                }
            }
            else
            {
                MessageBox.Show("ERROR - NOT 5 POP GROUPS");
            }
            byte[] popBytes = memoryStream.ToArray();
            return popBytes;

        }
        public byte[] popMarkers(string popFile, TM64_Geometry.OK64F3DObject[] surfaceObjects)
        {


            //popMarkers is used by the geometry compiler, not to add objects to existing courses.
            Pathgroup[] pathgroup = loadPOP(popFile, surfaceObjects);


            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);

            int groupCount = pathgroup.Length;
            if (groupCount == 5)
            {
                int[] paddingLength = new int[5] { 800, 64, 64, 64, 8 };

                for (int currentGroup = 0; currentGroup < groupCount; currentGroup++)
                {

                    int markerCount = pathgroup[currentGroup].pathList[0].pathmarker.Count;


                    for (int currentMarker = 0; currentMarker < markerCount; currentMarker++)
                    {

                        dataBytes = BitConverter.GetBytes(Convert.ToInt16(pathgroup[currentGroup].pathList[0].pathmarker[currentMarker].xval));
                        Array.Reverse(dataBytes);
                        binaryWriter.Write(dataBytes);  //x

                        dataBytes = BitConverter.GetBytes(Convert.ToInt16(pathgroup[currentGroup].pathList[0].pathmarker[currentMarker].zval));
                        Array.Reverse(dataBytes);
                        binaryWriter.Write(dataBytes);  //z

                        dataBytes = BitConverter.GetBytes(Convert.ToInt16(-1 * pathgroup[currentGroup].pathList[0].pathmarker[currentMarker].yval));
                        Array.Reverse(dataBytes);
                        binaryWriter.Write(dataBytes);  //y 

                        dataBytes = BitConverter.GetBytes(Convert.ToUInt16(pathgroup[currentGroup].pathList[0].pathmarker[currentMarker].flag));
                        Array.Reverse(dataBytes);
                        binaryWriter.Write(dataBytes);  //flag

                    }

                    dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                    Array.Reverse(dataBytes);
                    binaryWriter.Write(dataBytes);  //end list

                    if (currentGroup == 0)  //group 0 is course paths, groups 1-3 are objects
                    {
                        dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                        Array.Reverse(dataBytes);
                        binaryWriter.Write(dataBytes);  //end path

                        dataBytes = BitConverter.GetBytes(Convert.ToUInt16(0x8000));
                        Array.Reverse(dataBytes);
                        binaryWriter.Write(dataBytes);  //end path
                    }
                    else
                    {

                        dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                        Array.Reverse(dataBytes);
                        binaryWriter.Write(dataBytes);  //end object

                        dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                        Array.Reverse(dataBytes);
                        binaryWriter.Write(dataBytes);  //end object
                    }


                    dataBytes = BitConverter.GetBytes(Convert.ToInt16(0));
                    Array.Reverse(dataBytes);
                    binaryWriter.Write(dataBytes);  //end flag



                    int localPad = paddingLength[currentGroup] - markerCount;
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
                }
            }
            else
            {
                MessageBox.Show("ERROR - NOT 5 POP GROUPS");
            }
            byte[] popBytes = memoryStream.ToArray();
            return popBytes;

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
