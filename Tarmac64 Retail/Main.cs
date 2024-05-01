using Assimp;
using F3DSharp;
using Fluent;
using Microsoft.WindowsAPICodePack.Dialogs;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Tarmac64_Library;
using static Tarmac64_Library.TM64_Course;

namespace Tarmac64_Retail
{


    public partial class Main : Form
    {

        TM64 Tarmac = new TM64();
        TM64_Course TarmacCourse = new TM64_Course();
        TM64_GL TarmacGL = new TM64_GL();
        CommonOpenFileDialog fileOpen = new CommonOpenFileDialog();
        
        TM64.OK64Settings okSettings = new TM64.OK64Settings();
        List<TM64_Course.OKObject> OKObjectList = new List<TM64_Course.OKObject>();
        List<TM64_Course.OKObjectType> OKObjectTypeList = new List<TM64_Course.OKObjectType>();
        F3DEX095 F3D = new F3DEX095();
        bool updateBool = false;

        int sectionCount = 0;
        int levelFormat = 0;

        int LastSelectedSection, LastSelectedView = 0;



        public Main()
        {
            InitializeComponent();
        }

        public void CreateColors()
        {
            mapData.MapColor = new TM64_Geometry.OK64Color();
            mapData.MapColor.R = 0;
            mapData.MapColor.G = 0;
            mapData.MapColor.B = 0;
            mapData.MapColor.A = 0;


            skyData.TopColor = new TM64_Geometry.OK64Color();
            skyData.TopColor.R = 0;
            skyData.TopColor.G = 0;
            skyData.TopColor.B = 0;
            skyData.TopColor.A = 0;


            skyData.BotColor = new TM64_Geometry.OK64Color();
            skyData.BotColor.R = 0;
            skyData.BotColor.G = 0;
            skyData.BotColor.B = 0;
            skyData.BotColor.A = 0;
        }
        private void FormLoad(object sender, EventArgs e)
        {
            
            
            CreateColors();

            okSettings.LoadSettings();


            localCamera.rotation[0] = 0;
            //localCamera.rotation = 90;
            gl = GLControl.GLWindow.OpenGL;
            GLControl.OKObjectIndex = 0;

            GLControl.UpdateParent += GLRequestUpdate;
            ObjectControl.UpdateParent += ObjectRequestUpdate;
            ObjectControl.UpdateZoomToTarget += ZoomToObject;
            TextureControl.UpdateParent += TextureRequestUpdate;
            SettingsControl.UpdateParent += SettingsRequestUpdate;

            tabControl1.SelectedIndex = 0;
            tabControl1.SelectedIndex = 1;
            tabControl1.SelectedIndex = 2;
            tabControl1.SelectedIndex = 3;
            tabControl1.SelectedIndex = 4;
            tabControl1.SelectedIndex = 0;
            ObjectRequestUpdate(sender,e);
            
        }

        TM64_Geometry TarmacGeometry = new TM64_Geometry();
        TM64_Paths tm64Path = new TM64_Paths();
        TM64 tm64 = new TM64();

        TM64_Geometry.OK64SectionList[] sectionList = new TM64_Geometry.OK64SectionList[0];
        TM64_Geometry.OK64SectionList[] XLUSectionList = new TM64_Geometry.OK64SectionList[0];
        TM64_Geometry.OK64SectionList[] surfaceList = new TM64_Geometry.OK64SectionList[0];


        string[] viewString = new string[] { "North", "East", "South", "West" };

        string[] surfaceType = new string[] { "Solid", "Dirt", "Dirt Track", "Cement", "Snow Track", 
            "Wood", "Dirt Off-Road", "Grass", "Ice", "Beach Sand", "Snow Off-Road", "Rock Walls", 
            "Dirt Off-Road", "Train Tracks", "Cave Interior", "Rickety Wood Bridge", "Solid Wood Bridge",
            "C-FastOOB", "C-Water", "C-Mushroom Boost","C-Feather Jump", "C-Tornado Jump","C-SpinOut Saveable",
            "C-SpinOut", "C-Failed Start", "C-GreenShell Hit", "C-RedShell Hit", "C-Object Hit", "C-Shrunken",
            "C-Star Power", "C-Boo", "C-Get Item", "C-Trick Jump", "C-Gap Jump", "C-Lava", "C-ForceJump", "DK Parkyway Boost", "Out-Of-Bounds", "Royal Raceway Boost", "Walls" };

        Byte[] surfaceTypeID = new Byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 
            0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 
            0x10, 0x11, 0xFB, 0xFA, 0xF9, 0xF8, 0xF7, 0xF6, 0xF5, 0xF4, 
            0xF3, 0xF2, 0xF1, 0xF0, 0xEF, 0xEE, 0xED, 0xEC, 0xEB, 0xEA, 0xE9, 0xFC, 0xFD, 0xFE, 0xFF };

        

        bool loaded,UpdateDraw = false;

        TM64_Paths.Pathgroup[] pathGroups = new TM64_Paths.Pathgroup[0];

        TM64_Geometry.OK64F3DObject[] masterObjects = new TM64_Geometry.OK64F3DObject[0];
        TM64_Geometry.OK64F3DGroup[] masterGroups = new TM64_Geometry.OK64F3DGroup[0];
        int moveDistance = 50;


        List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>();

        TM64_Geometry.OK64F3DObject[] surfaceObjects = new TM64_Geometry.OK64F3DObject[0];
    

        TM64_Geometry.OK64Texture[] textureArray = new TM64_Geometry.OK64Texture[0];
        Bitmap[] TextureBitmaps = new Bitmap[0];
        

        TM64_Course.MiniMap mapData = new TM64_Course.MiniMap();
        TM64_Course.Sky skyData = new TM64_Course.Sky();
        int[] gameSpeed = new int[0];
        int materialCount;

        TM64_GL.TMCamera localCamera = new TM64_GL.TMCamera();
        OpenGL gl = new OpenGL();




        TM64_Course.Course CourseData;

        private void CompileModel()
        {
            
            fileOpen.InitialDirectory = okSettings.ProjectDirectory;
            fileOpen.IsFolderPicker = false;


            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            CourseData = SettingsControl.CourseData;
            CourseData = (PathControl.UpdateCourse(CourseData));
            textureArray = TextureControl.textureArray;

            List<byte[]> Segments = new List<byte[]>();

            byte[] segment4 = new byte[0];
            byte[] segment6 = new byte[0];
            byte[] segment7 = new byte[0];
            byte[] segment9 = new byte[0];
            byte[] PathListData = new byte[0];
            byte[][] ObjectListData = new byte[5][];
            byte[] ListData = new byte[0];
            byte[] PathData = new byte[0];
            byte[] collisionList = new byte[0];
            byte[] renderList = new byte[0];
            byte[] XLUList = new byte[0];
            byte[] textureList = new byte[0];


            byte[] surfaceTable = new byte[0];
            byte[] displayTable = new byte[0];
            byte[] XLUTable = new byte[0];

            int magic = 0;

            int vertMagic = 0;
            
            // Game speed multiplier. Default is 2
            gameSpeed = new int[4];


            
            

            TarmacGeometry.BuildTextures(textureArray);
                    

            //build segment 7 out of the main course objects and surface geometry
            //build segment 4 out of the same objects.

            TM64_Geometry.OK64F3DObject[] textureObjects = masterObjects;
            byte[] tempBytes = new byte[0];
            if (levelFormat == 0)
            {
                //Race Level

                // build various segment data
                        




                //items trees and piranhas
                int ObjectCount = 0;
                TM64_Paths.Pathlist[] ThisList = new TM64_Paths.Pathlist[5];
                for (int ThisType = 0; ThisType < 3; ThisType++)
                {
                    ThisList[ThisType] = new TM64_Paths.Pathlist();
                    ThisList[ThisType].pathmarker = new List<TM64_Paths.Marker>();
                    for (int ThisObject = 0; ThisObject < OKObjectList.Count; ThisObject++)
                    {
                        if (OKObjectList[ThisObject].ObjectIndex == ThisType)
                        {
                            ThisList[ThisType].Add(OKObjectList[ThisObject]);
                            
                            if (ThisList[ThisType].pathmarker.Count > 63)
                            {
                                MessageBox.Show("FATAL ERROR - " + OKObjectTypeList[ThisType].Name + " Too many objects! Max count 64");
                                return;
                            }
                        }
                    }
                    ObjectCount += ThisList[ThisType].pathmarker.Count;
                }

                ThisList[3] = new TM64_Paths.Pathlist();
                ThisList[3].pathmarker = new List<TM64_Paths.Marker>();
                ThisList[4] = new TM64_Paths.Pathlist();
                ThisList[4].pathmarker = new List<TM64_Paths.Marker>();
                for (int ThisObject = 0; ThisObject < OKObjectList.Count; ThisObject++)
                {
                    if (OKObjectList[ThisObject].ObjectIndex == 3)
                    {
                        ThisList[3].Add(OKObjectList[ThisObject].OriginPosition);
                        if (ThisList[3].pathmarker.Count > 8)
                        {
                            MessageBox.Show("FATAL ERROR - " + OKObjectTypeList[3].Name + " Too many objects! Max count 8");
                            return;
                        }
                    }
                    if (OKObjectList[ThisObject].ObjectIndex == 4)
                    {
                        ThisList[4].Add(OKObjectList[ThisObject].OriginPosition);
                        if (ThisList[4].pathmarker.Count > 8)
                        {
                            MessageBox.Show("FATAL ERROR - " + OKObjectTypeList[4].Name + " Too many objects! Max count 8");
                            return;
                        }
                    }
                }

                ObjectCount += ThisList[3].pathmarker.Count;
                ObjectCount += ThisList[4].pathmarker.Count;

                if (ObjectCount >= 100)
                {
                    MessageBox.Show("FATAL ERROR - OVER 100 STANDARD OBJECTS. BUFFER FILLED");
                    return;
                }
                else if (ObjectCount > 70)
                {
                    MessageBox.Show("Warning! Over 70 Objects. Potential to fill Buffer.");
                }



                MemoryStream ListStream = new MemoryStream();
                for (int This = 0; This < 3; This++)
                {
                    ObjectListData[This] = tm64Path.popMarker(ThisList[This], 64);
                    ListStream.Write(ObjectListData[This], 0, ObjectListData[This].Length);
                }

                ObjectListData[3] = tm64Path.popMarker(ThisList[3], 8);
                ListStream.Write(ObjectListData[3], 0, ObjectListData[3].Length);
                ObjectListData[4] = tm64Path.popMarker(ThisList[4], 8);
                ListStream.Write(ObjectListData[4], 0, ObjectListData[4].Length);
                ListData = ListStream.ToArray();


                ListStream = new MemoryStream();


                CourseData.PathSettings.PathOffsets = new UInt32[4] { 0x800DC778, 0x800DC778, 0x800DC778, 0x800DC778 };
                          
                CourseData.PathSettings.PathOffsets[0] = Convert.ToUInt32(0x06000000 + ListData.Length + 8);
                PathListData = tm64Path.popMarker(pathGroups[0].pathList[0], 0);
                ListStream.Write(PathListData, 0, PathListData.Length);

                PathListData = tm64Path.popMarkerFlat(pathGroups[0].pathList[0], 0);
                ListStream.Write(PathListData, 0, PathListData.Length);
                for (int ThisPath = 1; ThisPath < pathGroups[0].pathList.Length;ThisPath++)
                {
                    CourseData.PathSettings.PathOffsets[ThisPath] = Convert.ToUInt32(0x06000000 + ListData.Length + 8 + ListStream.Position);
                    PathListData = tm64Path.popMarker(pathGroups[0].pathList[ThisPath], 0);
                    ListStream.Write(PathListData, 0, PathListData.Length);
                }

                PathData = ListStream.ToArray();


                textureList = TarmacGeometry.compileCourseTexture(segment6, textureArray, (ListData.Length + 8 + PathData.Length),5, Convert.ToBoolean(CourseData.Fog.FogToggle) );
                if (!TarmacGeometry.CompileCourseObjects(ref vertMagic, ref segment4, ref segment7, segment4, segment7, masterObjects, textureArray, vertMagic, true))
                {
                    return;
                }
                if (!TarmacGeometry.CompileCourseObjects(ref vertMagic, ref segment4, ref segment7, segment4, segment7, surfaceObjects, textureArray, vertMagic, false))
                {
                    return;
                }
                    

                renderList = TarmacGeometry.CompileF3DList(ref sectionList, masterObjects, sectionList, textureArray);
                XLUList = TarmacGeometry.CompileXLUList(ref XLUSectionList, masterObjects, XLUSectionList, textureArray);

                surfaceTable = TarmacGeometry.CompileSurfaceTable(surfaceObjects);

                magic = textureList.Length + ListData.Length + PathData.Length + 8 + surfaceTable.Length + (528 * 2);
                
                // Build the display table with the above magic value
                // 8 bytes for header
                // 8040 bytes for the POP data
                // 8 bytes for Surface Table Footer
                // 528 bytes for the Display Table itself. Another 528 for the XLU table.
                // The surface table is 8 bytes per object.
                // We tracked the number of surface meshes while loading into surfaceObjects.
                // magic is the size of data written before the display lists.
                // it's needed to properly calculate the offsets.
                // We're calculating hardcoded offsets before writing them.
                // So we need to use magic to do it.

                // Build the display table with the above magic value

                displayTable = TarmacGeometry.CompilesectionviewTable(sectionList, magic);
                XLUTable = TarmacGeometry.CompilesectionviewTable(XLUSectionList, magic + renderList.Length);




                memoryStream = new MemoryStream();
                binaryWriter = new BinaryWriter(memoryStream);
                byte[] byteArray = new byte[0];

                binaryWriter.Write(F3D.gsSPEndDisplayList());

                binaryWriter.Write(ListData);
                binaryWriter.Write(PathData);
                binaryWriter.Write(textureList);
                CourseData.OK64HeaderData.SectionViewPosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(displayTable);
                CourseData.OK64HeaderData.XLUViewPosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(XLUTable);
                CourseData.OK64HeaderData.SurfaceMapPosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(surfaceTable);
                binaryWriter.Write(renderList);
                binaryWriter.Write(XLUList);
                segment6 = memoryStream.ToArray();
            }
            else
            {

                //Battle Level





                CourseData.PathSettings.PathOffsets = new UInt32[4] { 0x800DC778, 0x800DC778, 0x800DC778, 0x800DC778 };

                
                TM64_Paths.Pathlist ThisList = new TM64_Paths.Pathlist();

                ThisList = new TM64_Paths.Pathlist();
                ThisList.pathmarker = new List<TM64_Paths.Marker>();
                for (int ThisObject = 0; ThisObject < OKObjectList.Count; ThisObject++)
                {
                    if (OKObjectList[ThisObject].ObjectIndex == 0)
                    {
                        ThisList.Add(OKObjectList[ThisObject].OriginPosition);
                        if (ThisList.pathmarker.Count > 63)
                        {
                            MessageBox.Show("FATAL ERROR - " + OKObjectTypeList[0].Name + " Too many objects! Max count 64");
                            return;
                        }
                    }
                }


                int BattleCount = 0;
                List<TM64_Paths.BattleMarker> ObjectiveList = new List<TM64_Paths.BattleMarker>();
                for (int ThisObject = 0; ThisObject < OKObjectList.Count; ThisObject++)
                {
                    if (OKObjectList[ThisObject].ObjectIndex == 5)
                    {
                        TM64_Paths.BattleMarker ThisSpot = new TM64_Paths.BattleMarker();
                        ThisSpot.xval = OKObjectList[ThisObject].OriginPosition[0];
                        ThisSpot.yval = OKObjectList[ThisObject].OriginPosition[1];
                        ThisSpot.zval = OKObjectList[ThisObject].OriginPosition[2];
                        ThisSpot.flag = OKObjectList[ThisObject].GameMode;
                        ThisSpot.Player = OKObjectList[ThisObject].BattlePlayer;
                        ThisSpot.Type = OKObjectList[ThisObject].ObjectiveClass;

                        ObjectiveList.Add(ThisSpot);
                        BattleCount++;
                        if (BattleCount > 64)
                        {
                            MessageBox.Show("FATAL ERROR - Too many Battle Objectives! Max count 64");
                            return;
                        }
                    }
                }

                MemoryStream ListStream = new MemoryStream();

                byte[] ItemBoxData = tm64Path.popMarker(ThisList, 64);
                byte[] BattleObjectiveData = tm64Path.popMarkerBattleObjective(ObjectiveList, 64);
                ListStream.Write(ItemBoxData, 0, ItemBoxData.Length);
                ListStream.Write(BattleObjectiveData, 0, BattleObjectiveData.Length);


                ListData = ListStream.ToArray();


                textureList = TarmacGeometry.compileCourseTexture(segment6, textureArray, 8 + ListData.Length, 5, Convert.ToBoolean(CourseData.Fog.FogToggle));
                if (!TarmacGeometry.CompileCourseObjects(ref vertMagic, ref segment4, ref segment7, segment4, segment7, masterObjects, textureArray, vertMagic))
                {
                    return;
                }
                if (!TarmacGeometry.CompileCourseObjects(ref vertMagic, ref segment4, ref segment7, segment4, segment7, surfaceObjects, textureArray, vertMagic))
                {
                    return;
                }

                renderList = TarmacGeometry.CompileBattleList(masterObjects, textureArray);
                XLUList = TarmacGeometry.CompileBattleXLU(masterObjects, textureArray);
                surfaceTable = TarmacGeometry.CompileSurfaceTable(surfaceObjects);


                memoryStream = new MemoryStream();
                binaryWriter = new BinaryWriter(memoryStream);
                byte[] byteArray = new byte[0];

                binaryWriter.Write(F3D.gsSPEndDisplayList());
                binaryWriter.Write(ListData);

                binaryWriter.Write(textureList);
                CourseData.OK64HeaderData.SectionViewPosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(renderList);
                CourseData.OK64HeaderData.XLUViewPosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(XLUList); 
                CourseData.OK64HeaderData.SurfaceMapPosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(surfaceTable);

                segment6 = memoryStream.ToArray();
            }




            
            CourseData.ModelData.TextureObjects = TextureControl.textureArray;
            CourseData.Segment4 = segment4;
            CourseData.Segment6 = segment6;
            CourseData.Segment7 = segment7;            
            CourseData.ModelData.RenderObjects = masterObjects;
            CourseData.ModelData.SurfaceObjects = surfaceObjects;



            //Read Ghost Data from the GhostPath and 
            //Set the character. Compressed INput data in the OK64Ghost
            //is padded and needs to be cleaned via recompression.

            if (CourseData.GhostPath != "")
            {
                byte[] TempGhostData = File.ReadAllBytes(CourseData.GhostPath);
                MemoryStream GhostStream = new MemoryStream(TempGhostData);
                BinaryReader GhostReader = new BinaryReader(GhostStream);
                GhostReader.BaseStream.Position = 0;
                CourseData.GhostCharacter = GhostReader.ReadInt32();
                byte[] CleanGhostData = Tarmac.DecompressMIO0(GhostReader.ReadBytes(0x3C00));// Bad MIO0 with Padding
                CourseData.GhostData = Tarmac.CompressMIO0(CleanGhostData); //Make clean
            }
            else
            {
                CourseData.GhostData = new byte[0];
                CourseData.GhostCharacter = -1;
            }


            CourseData.OK64HeaderData.PathLength = new short[4];
            for (int ThisPath = 0; ThisPath < pathGroups[0].pathList.Length; ThisPath++)
            {
                CourseData.OK64HeaderData.PathLength[ThisPath] = Convert.ToInt16(pathGroups[0].pathList[ThisPath].pathmarker.Count);
            }
            for (int ThisPath = pathGroups[0].pathList.Length; ThisPath < 4; ThisPath++)
            {
                CourseData.OK64HeaderData.PathLength[ThisPath] = Convert.ToInt16(1);
            }
            CourseData.Gametype = levelFormat;
            CourseData.SerialNumber = TarmacCourse.OK64Serial(CourseData);


            
            List<TM64_Course.OKObjectType> TypeList = new List<TM64_Course.OKObjectType>();
            for (int This = 6; This < OKObjectTypeList.Count; This++)
            {
                TypeList.Add(OKObjectTypeList[This]);
            }
            List<TM64_Course.OKObject> CustomObjectList = new List<TM64_Course.OKObject>();
            for (int This = 0; This < OKObjectList.Count; This++)
            {
                if (OKObjectList[This].ObjectIndex >= 6)
                {
                    CustomObjectList.Add(OKObjectList[This]);
                }
            }
            
            
            CourseData.ObjectModelData = TarmacCourse.CompileObjectModels(TypeList.ToArray(), Convert.ToBoolean(CourseData.Fog.FogToggle));
            uint Magic = Convert.ToUInt32(CourseData.ObjectModelData.Length);
            CourseData.ObjectAnimationData = TarmacCourse.CompileObjectAnimation(TypeList.ToArray(), Magic);
            Magic += Convert.ToUInt32(CourseData.ObjectAnimationData.Length);
            CourseData.ObjectHitboxData = TarmacCourse.CompileObjectHitbox(TypeList.ToArray(), Magic);
            CourseData.ObjectTypeData = TarmacCourse.SaveObjectTypeRaw(TypeList.ToArray());
            CourseData.ObjectListData = TarmacCourse.SaveOKObjectListRaw(CustomObjectList.ToArray());
            
            if (CourseData.OK64SongPath.Length > 0)
            {
                TM64_Sound TarmacSound = new TM64_Sound();
                CourseData.SongData = TarmacSound.LoadSong(CourseData.OK64SongPath);
            }
            else
            {
                CourseData.SongData = new TM64_Sound.OK64Song();
                CourseData.SongData.SequenceData = new byte[0];
                CourseData.SongData.InstrumentData = new byte[0];

            }




            //WaterVertex (translucency) and Map Scrolling




            //scroll data
            int scrollCount = 0;
            foreach (var textureObject in CourseData.ModelData.TextureObjects)
            {
                if (textureObject.textureScrollS != 0 || textureObject.textureScrollT != 0)
                {
                    scrollCount++;
                }
            }
            foreach (var ObjectType in TypeList)
            {
                foreach (var textureObject in ObjectType.TextureData)
                {
                    if (textureObject.textureScrollS != 0 || textureObject.textureScrollT != 0)
                    {
                        scrollCount++;
                    }
                }
            }



            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);


            byte[] flip = BitConverter.GetBytes(Convert.ToInt32(scrollCount));
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            if (scrollCount > 0)
            {

                foreach (var textureObject in CourseData.ModelData.TextureObjects)
                {
                    if (textureObject.textureScrollS != 0 || textureObject.textureScrollT != 0)
                    {
                        
                        binaryWriter.Write(F3D.BigEndian(Convert.ToInt32(textureObject.RawTexture.f3dexPosition | 0x06000000)));
                        binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(textureObject.textureScrollS)));
                        binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(textureObject.textureScrollT)));
                    }
                }

                foreach (var ObjectType in TypeList)
                {
                    foreach (var textureObject in ObjectType.TextureData)
                    {
                        if (textureObject.textureScrollS != 0 || textureObject.textureScrollT != 0)
                        {

                            binaryWriter.Write(F3D.BigEndian(Convert.ToInt32(textureObject.RawTexture.f3dexPosition | 0x0A000000)));
                            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(textureObject.textureScrollS)));
                            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(textureObject.textureScrollT)));
                        }
                    }
                }
            }
            CourseData.ScrollData = memoryStream.ToArray();
            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);

            //watervertex
            int waterCount = 0;

            foreach (var OBJ in masterObjects)
            {
                if (OBJ.WaveObject)
                {
                    waterCount++;
                }
            }



            flip = BitConverter.GetBytes(Convert.ToInt32(waterCount));
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            if (waterCount > 0)
            {
                foreach (var ThisObject in masterObjects)
                {
                    if (ThisObject.WaveObject)
                    {
                        binaryWriter.Write(F3D.BigEndian(ThisObject.faceCount * 3));
                        binaryWriter.Write(F3D.BigEndian(Convert.ToInt32(ThisObject.VertCachePosition / 0.875)));
                    }
                }
            }

            CourseData.WaterData = memoryStream.ToArray();
            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);


            //screendata
            int screenCount = 0;
            foreach (var textureObject in CourseData.ModelData.TextureObjects)
            {
                if (textureObject.textureScreen > 0)
                {
                    screenCount++;
                }
            }

            foreach (var ObjectType in TypeList)
            {
                foreach (var textureObject in ObjectType.TextureData)
                {
                    if (textureObject.textureScrollS != 0 || textureObject.textureScrollT != 0)
                    {
                        screenCount++;
                    }
                }
            }

            flip = BitConverter.GetBytes(Convert.ToInt32(screenCount));
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            if (screenCount > 0)
            {
                for (int CurrentScreen = 0; CurrentScreen < screenCount; CurrentScreen++)
                {
                    foreach (var textureObject in CourseData.ModelData.TextureObjects)
                    {
                        if (textureObject.textureScreen == (CurrentScreen + 1))
                        {
                            flip = BitConverter.GetBytes(Convert.ToInt32(textureObject.RawTexture.segmentPosition | 0x05000000));
                            Array.Reverse(flip);
                            binaryWriter.Write(flip);
                        }
                    }

                    foreach (var ObjectType in TypeList)
                    {
                        foreach (var textureObject in ObjectType.TextureData)
                        {
                            if (textureObject.textureScreen == (CurrentScreen + 1))
                            {

                                binaryWriter.Write(F3D.BigEndian(Convert.ToInt32(textureObject.RawTexture.segmentPosition | 0x0A000000)));
                            }
                        }
                    }
                }
            }

            CourseData.ScreenData = memoryStream.ToArray();
            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);



            //KillDisplaydata
            int KDCount = 0;
            foreach (var CObj in CourseData.ModelData.RenderObjects)
            {
                for (int ThisBool = 0; ThisBool < 8; ThisBool++)
                {
                    if (!CObj.KillDisplayList[ThisBool])
                    {
                        KDCount++;
                        break;
                    }
                }
            }
            foreach (var CObj in CourseData.ModelData.SurfaceObjects)
            {
                for (int ThisBool = 0; ThisBool < 8; ThisBool++)
                {
                    if (!CObj.KillDisplayList[ThisBool])
                    {
                        KDCount++;
                        break;
                    }
                }
            }

            
            binaryWriter.Write(F3D.BigEndian(KDCount));
            if (KDCount > 0)
            {
                foreach (var CObj in CourseData.ModelData.RenderObjects)
                {
                    for (int ThisSweep = 0; ThisSweep < 8; ThisSweep++)
                    {
                        if (!CObj.KillDisplayList[ThisSweep])
                        {
                            for (int ThisBool = 0; ThisBool < 8; ThisBool++)
                            {
                                binaryWriter.Write(Convert.ToByte(CObj.KillDisplayList[ThisBool]));
                            }
                            binaryWriter.Write(F3D.BigEndian(CObj.meshPosition.Length));
                            foreach (var Mesh in CObj.meshPosition)
                            {
                                binaryWriter.Write(F3D.BigEndian(Mesh));
                            }
                            break;
                        }
                        
                    }
                }
                foreach (var CObj in CourseData.ModelData.SurfaceObjects)
                {
                    for (int ThisSweep = 0; ThisSweep < 8; ThisSweep++)
                    {
                        if (!CObj.KillDisplayList[ThisSweep])
                        {
                            for (int ThisBool = 0; ThisBool < 8; ThisBool++)
                            {
                                binaryWriter.Write(Convert.ToByte(CObj.KillDisplayList[ThisBool]));
                            }
                            binaryWriter.Write(F3D.BigEndian(CObj.meshPosition.Length));
                            foreach (var Mesh in CObj.meshPosition)
                            {
                                binaryWriter.Write(F3D.BigEndian(Mesh));
                            }
                            break;
                        }
                        
                    }
                }

            }

            CourseData.KillDisplayData = memoryStream.ToArray();
            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);




            MessageBox.Show("Save .OK64.Course");
            SaveFileDialog FileSave = new SaveFileDialog();
            FileSave.InitialDirectory = okSettings.ProjectDirectory;
            FileSave.Filter = "Tarmac Course|*.ok64.Course|All Files (*.*)|*.*";
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                string SavePath = FileSave.FileName;                    
                File.WriteAllBytes(SavePath, TarmacCourse.SaveOK64Course(CourseData));                    
            }

            

                    





            MessageBox.Show("Finished");
        
        }


        private void ReplacePaths()
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            MessageBox.Show("Select OK64.POP File");
            OpenFile.Title = "POP File";
            OpenFile.InitialDirectory = okSettings.ProjectDirectory;
            OpenFile.Filter = "Tarmac Path and Object Positions|*.OK64.POP|All Files (*.*)|*.*";
            OpenFile.FileName = null;
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(OpenFile.FileName))
                {
                    string popFile = OpenFile.FileName;

                    if (levelFormat == 0)
                    {
                        pathGroups = tm64Path.loadPOP(popFile, surfaceObjects);
                        if (pathGroups[1].pathList.Length != 0)
                        {
                            foreach (var ItemBox in pathGroups[1].pathList[0].pathmarker)
                            {
                                TM64_Course.OKObject NewObject = TarmacCourse.NewOKObject();

                                NewObject.ObjectIndex = 0;
                                NewObject.OriginPosition[0] = Convert.ToInt16(ItemBox.xval);
                                NewObject.OriginPosition[1] = Convert.ToInt16(ItemBox.yval);
                                NewObject.OriginPosition[2] = Convert.ToInt16(ItemBox.zval);

                                OKObjectList.Add(NewObject);
                                int NewIndex = ObjectControl.ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.ObjectIndex].Name + " " + ObjectControl.ObjectListBox.Items.Count.ToString());

                            }
                        }
                        if (pathGroups[2].pathList.Length != 0)
                        {
                            foreach (var Tree in pathGroups[2].pathList[0].pathmarker)
                            {
                                TM64_Course.OKObject NewObject = TarmacCourse.NewOKObject();

                                NewObject.ObjectIndex = 1;
                                NewObject.OriginPosition[0] = Convert.ToInt16(Tree.xval);
                                NewObject.OriginPosition[1] = Convert.ToInt16(Tree.yval);
                                NewObject.OriginPosition[2] = Convert.ToInt16(Tree.zval);

                                OKObjectList.Add(NewObject);
                                ObjectControl.ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.ObjectIndex].Name + " " + ObjectControl.ObjectListBox.Items.Count.ToString());

                            }
                        }
                        if (pathGroups[3].pathList.Length != 0)
                        {
                            foreach (var Plant in pathGroups[3].pathList[0].pathmarker)
                            {
                                TM64_Course.OKObject NewObject = TarmacCourse.NewOKObject();

                                NewObject.ObjectIndex = 2;
                                NewObject.OriginPosition[0] = Convert.ToInt16(Plant.xval);
                                NewObject.OriginPosition[1] = Convert.ToInt16(Plant.yval);
                                NewObject.OriginPosition[2] = Convert.ToInt16(Plant.zval);

                                OKObjectList.Add(NewObject);
                                ObjectControl.ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.ObjectIndex].Name + " " + ObjectControl.ObjectListBox.Items.Count.ToString());
                            }
                        }
                        if (pathGroups[4].pathList.Length != 0)
                        {
                            foreach (var Coin in pathGroups[4].pathList[0].pathmarker)
                            {
                                TM64_Course.OKObject NewObject = TarmacCourse.NewOKObject();

                                NewObject.ObjectIndex = 3;
                                NewObject.OriginPosition[0] = Convert.ToInt16(Coin.xval);
                                NewObject.OriginPosition[1] = Convert.ToInt16(Coin.yval);
                                NewObject.OriginPosition[2] = Convert.ToInt16(Coin.zval);

                                OKObjectList.Add(NewObject);
                                ObjectControl.ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.ObjectIndex].Name + " " + ObjectControl.ObjectListBox.Items.Count.ToString());

                            }
                        }


                    }
                    else
                    {
                        pathGroups = tm64Path.loadPOP(popFile, surfaceObjects);
                        if (pathGroups[1].pathList.Length != 0)
                        {
                            foreach (var ItemBox in pathGroups[1].pathList[0].pathmarker)
                            {
                                TM64_Course.OKObject NewObject = TarmacCourse.NewOKObject();

                                NewObject.ObjectIndex = 0;
                                NewObject.OriginPosition[0] = Convert.ToInt16(ItemBox.xval);
                                NewObject.OriginPosition[1] = Convert.ToInt16(ItemBox.yval);
                                NewObject.OriginPosition[2] = Convert.ToInt16(ItemBox.zval);

                                OKObjectList.Add(NewObject);
                                int NewIndex = ObjectControl.ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.ObjectIndex].Name + " " + ObjectControl.ObjectListBox.Items.Count.ToString());

                            }
                        }
                    }
                }

            }



            UpdateGLView();
            GLControl.UpdateDraw = true;

            float RadTurn = Convert.ToSingle(90.0f * (Math.PI / 180.0f));
            GLControl.LocalCamera.rotation[0] = RadTurn;

            
        }


        private void ReplaceModel(bool Replace = false)
        {

            MessageBox.Show("Select .FBX or .OBJ File");

            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.InitialDirectory = okSettings.ProjectDirectory;
            OpenFile.Title = "FBX | OBJ File Loading";
            OpenFile.Filter = "FBX Model|*.FBX|OBJ Model|*.OBJ|All Files (*.*)|*.*";
            OpenFile.FileName = null;

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                if (File.Exists(OpenFile.FileName))
                {
                    string FBXfilePath = OpenFile.FileName;
                    levelFormat = 0;

                    AssimpContext importer = new AssimpContext();

                    Scene fbx = importer.ImportFile(FBXfilePath, PostProcessPreset.TargetRealTimeMaximumQuality);

                    materialCount = fbx.MaterialCount;

                    
                    int modelFormat = TarmacGeometry.GetModelFormat(fbx);
                    if (modelFormat == 0)
                    {
                        sectionCount = TarmacGeometry.GetSectionCount(fbx); ;
                    }
                    else
                    {
                        sectionCount = 1;
                    }
                    




                    //
                    // Textures
                    //

                    


                    TM64_Geometry.OK64Texture[] OldTextures = textureArray;
                    int oldMatCount = textureArray.Length;


                    textureArray = TarmacGeometry.loadTextures(fbx, FBXfilePath);
                    materialCount = textureArray.Length;
                    TextureBitmaps = new Bitmap[textureArray.Length];



                    if (Replace)
                    {
                        //Check if reloading data - replace existing textures
                        for (int ThisOld = 0; ThisOld < oldMatCount; ThisOld++)
                        {
                            for (int ThisNew = 0;ThisNew < textureArray.Length; ThisNew++)
                            {
                                if (textureArray[ThisNew].texturePath == OldTextures[ThisOld].texturePath)
                                {
                                    textureArray[ThisNew] = OldTextures[ThisOld];
                                }
                            }
                        }
                    }


                    for (int ThisTex = 0; ThisTex < textureArray.Length; ThisTex++)
                    {

                        //Update the GL Bitmap Cache
                        if (textureArray[ThisTex].texturePath != null)
                        {
                            if (File.Exists(textureArray[ThisTex].texturePath))
                            {
                                try
                                {
                                    TextureBitmaps[ThisTex] = new Bitmap(textureArray[ThisTex].texturePath);
                                }
                                catch
                                {
                                    TextureBitmaps[ThisTex] = new Bitmap(Tarmac64_Library.Properties.Resources.TextureNotFound);
                                }
                            }
                            else
                            {
                                TextureBitmaps[ThisTex] = new Bitmap(Tarmac64_Library.Properties.Resources.TextureNotFound);
                            }
                        }
                    }



                    //
                    // Course Objects
                    // Surface Map
                    //


                    if (levelFormat == 0)
                    {
                        //Race
                        switch (modelFormat)
                        {
                            case 0:
                                {
                                    //normal
                                    masterObjects = TarmacGeometry.LoadMaster(ref masterGroups, fbx, textureArray, okSettings.AlphaCH2);
                                    surfaceObjects = TarmacGeometry.LoadCollisions(fbx, sectionCount, modelFormat, textureArray);
                                    TM64_Geometry.PathfindingObject[] surfaceBoundaries = TarmacGeometry.SurfaceBounds(surfaceObjects, sectionCount);
                                    sectionList = TarmacGeometry.AutomateSection(sectionCount, surfaceObjects, masterObjects, surfaceBoundaries, fbx, 0);
                                    XLUSectionList = TarmacGeometry.AutomateSection(sectionCount, surfaceObjects, masterObjects, surfaceBoundaries, fbx, 0);
                                    break;
                                }
                            case 1:
                                {
                                    //magical mystery mode
                                    masterObjects = TarmacGeometry.CreateMasterNoHeader(fbx, textureArray);
                                    surfaceObjects = TarmacGeometry.CreateCollisionsNoHeader(fbx, textureArray);
                                    surfaceObjects = TarmacGeometry.UpdateSectionIndexNoHeader(surfaceObjects, ref sectionCount);
                                    TM64_Geometry.PathfindingObject[] surfaceBoundaries = TarmacGeometry.SurfaceBounds(surfaceObjects, sectionCount);
                                    sectionList = TarmacGeometry.AutomateSection(sectionCount, surfaceObjects, masterObjects, surfaceBoundaries, fbx, 0);
                                    XLUSectionList = TarmacGeometry.AutomateSection(sectionCount, surfaceObjects, masterObjects, surfaceBoundaries, fbx, 0);
                                    break;
                                }
                        }
                    }
                    else
                    {
                        //Battle
                        masterObjects = TarmacGeometry.LoadMaster(ref masterGroups, fbx, textureArray);
                        surfaceObjects = TarmacGeometry.LoadCollisions(fbx, sectionCount, modelFormat, textureArray);
                        sectionList = new TM64_Geometry.OK64SectionList[0];
                        XLUSectionList = new TM64_Geometry.OK64SectionList[0];
                    }




                    //Prepare User Interface UI
                    masterBox.Nodes.Clear();
                    List<int> listedObjects = new List<int>();


                    for (int currentGroup = 0; currentGroup < masterGroups.Length; currentGroup++)
                    {
                        masterBox.Nodes.Add(masterGroups[currentGroup].groupName, masterGroups[currentGroup].groupName);
                        for (int currentGrandchild = 0; currentGrandchild < masterGroups[currentGroup].subIndexes.Length; currentGrandchild++)
                        {
                            masterBox.Nodes[currentGroup].Nodes.Add(masterObjects[masterGroups[currentGroup].subIndexes[currentGrandchild]].objectName, masterObjects[masterGroups[currentGroup].subIndexes[currentGrandchild]].objectName);
                            listedObjects.Add(masterGroups[currentGroup].subIndexes[currentGrandchild]);
                        }
                    }

                    for (int currentMaster = 0; currentMaster < masterObjects.Length; currentMaster++)
                    {
                        if (listedObjects.IndexOf(currentMaster) == -1)
                        {
                            masterBox.Nodes.Add(masterObjects[currentMaster].objectName, masterObjects[currentMaster].objectName);
                        }
                    }


                    if (levelFormat == 0)
                    {
                        SurfaceMeshListBox.Items.Clear();
                        for (int currentIndex = 0; currentIndex < surfaceObjects.Length; currentIndex++)
                        {
                            SurfaceMeshListBox.Items.Add(surfaceObjects[currentIndex].objectName);
                        }

                        RenderMeshListBox.Items.Clear();
                        for (int currentIndex = 0; currentIndex < masterObjects.Length; currentIndex++)
                        {
                            RenderMeshListBox.Items.Add(masterObjects[currentIndex].objectName);
                        }

                        surfsectionBox.Items.Clear();
                        sectionBox.Items.Clear();
                        CopySectionIndexBox.Items.Clear();
                        for (int currentSection = 0; currentSection < sectionCount; currentSection++)
                        {
                            CopySectionIndexBox.Items.Add("Section " + (currentSection + 1).ToString());
                            surfsectionBox.Items.Add("Section " + (currentSection + 1).ToString());
                            sectionBox.Items.Add("Section " + (currentSection + 1).ToString());

                        }
                        surfmaterialBox.Items.Clear();
                        for (int surfacematerialIndex = 0; surfacematerialIndex < surfaceType.Length; surfacematerialIndex++)
                        {
                            surfmaterialBox.Items.Add(surfaceTypeID[surfacematerialIndex].ToString() + "- " + surfaceType[surfacematerialIndex]);
                        }
                    }
                    TextureControl.Loaded = true;
                    TextureControl.textureArray = textureArray;
                    TextureControl.AddNewTextures(materialCount);

                    for (int ThisTexture = 0; ThisTexture < materialCount; ThisTexture++)
                    {
                        RenderMaterialBox.Items.Add(textureArray[ThisTexture].textureName);
                    }

                    if (levelFormat == 0)
                    {
                        sectionBox.SelectedIndex = 0;
                        TextureControl.textureBox.SelectedIndex = 0;
                        
                    }
                    loaded = true;



                    UpdateGLView();
                    GLControl.UpdateDraw = true;
                    GLControl.CacheTextures();
                    

                    MessageBox.Show("Finished");
                }
            }
            
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            if (loaded)
            {
                CompileModel();
            }
            else
            {
                ReplaceModel();
            }
        }

        private void SectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (loaded == true)
            {
                GLControl.UpdateDraw = true;
                sectionList[LastSelectedSection].viewList[LastSelectedView].objectList = GLControl.SectionList;
                XLUSectionList[LastSelectedSection].viewList[LastSelectedView].objectList = GLControl.SectionList;
                LastSelectedSection = sectionBox.SelectedIndex;
                UpdateSVDisplay();
                UpdateGLView();
            }
        }

        private void ViewBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (loaded == true)
            {
                GLControl.UpdateDraw = true;
                sectionList[LastSelectedSection].viewList[LastSelectedView].objectList = GLControl.SectionList;
                XLUSectionList[LastSelectedSection].viewList[LastSelectedView].objectList = GLControl.SectionList;
                LastSelectedSection = sectionBox.SelectedIndex;
                UpdateSVDisplay();
                UpdateGLView();
            }
        }

        public void ZoomToObject(object sender, EventArgs e)            
        {
            int ObjectIndex = ObjectControl.ZoomIndex;
            float[] TargetOrigin = new float[3] {
                Convert.ToSingle(OKObjectList[ObjectIndex].OriginPosition[0]),
                Convert.ToSingle(OKObjectList[ObjectIndex].OriginPosition[1]),
                Convert.ToSingle(OKObjectList[ObjectIndex].OriginPosition[2]),
            };
            TarmacGL.ZoomCameraTarget(TargetOrigin, GLControl.LocalCamera);
            GLControl.UpdateDraw = true;
        }

        public void TextureRequestUpdate(object sender, EventArgs e)
        {
            textureArray = TextureControl.textureArray;
            GLControl.TextureObjects = textureArray;

            if (TextureControl.UpdateTextureCache)
            {
                int x = 0;
                for (int ThisTex = 0; ThisTex < textureArray.Length; ThisTex++)
                {

                    //Update the GL Bitmap Cache
                    if (textureArray[ThisTex].texturePath != null)
                    {
                        if (File.Exists(textureArray[ThisTex].texturePath))
                        {
                            try
                            {
                                TextureBitmaps[ThisTex] = new Bitmap(textureArray[ThisTex].texturePath);
                            }
                            catch
                            {
                                TextureBitmaps[ThisTex] = new Bitmap(Tarmac64_Library.Properties.Resources.TextureNotFound);
                            }
                        }
                        else
                        {
                            TextureBitmaps[ThisTex] = new Bitmap(Tarmac64_Library.Properties.Resources.TextureNotFound);
                        }
                    }
                }
                GLControl.CacheTextures();
            }
            


            GLControl.BitmapData = TextureBitmaps;
            GLControl.UpdateDraw = true;
        }

        public void SettingsRequestUpdate(object sender, EventArgs e)
        {
            
            GLControl.SkyColors = new float[3, 3]
            {
                    { Convert.ToSingle(SettingsControl.CourseData.SkyColors.TopColor.R/255.0), Convert.ToSingle(SettingsControl.CourseData.SkyColors.TopColor.G / 255.0), Convert.ToSingle(SettingsControl.CourseData.SkyColors.TopColor.B / 255.0) },
                    { Convert.ToSingle(SettingsControl.CourseData.SkyColors.MidColor.R/255.0), Convert.ToSingle(SettingsControl.CourseData.SkyColors.MidColor.G / 255.0), Convert.ToSingle(SettingsControl.CourseData.SkyColors.MidColor.B / 255.0) },
                    { Convert.ToSingle(SettingsControl.CourseData.SkyColors.BotColor.R/255.0), Convert.ToSingle(SettingsControl.CourseData.SkyColors.BotColor.G / 255.0), Convert.ToSingle(SettingsControl.CourseData.SkyColors.BotColor.B / 255.0) },
            };
            GLControl.UpdateDraw = true;
            GLControl.DrawSky = Convert.ToBoolean(SettingsControl.CourseData.SkyboxBool);
        }
        public void ObjectRequestUpdate(object sender, EventArgs e)
        {
            OKObjectList = ObjectControl.OKObjectList;
            OKObjectTypeList = ObjectControl.OKObjectTypeList;
            GLControl.OKObjectIndex = ObjectControl.ObjectTypeIndexBox.SelectedIndex;
            GLControl.OKSelectedObject = ObjectControl.ObjectListBox.SelectedIndex;
            GLControl.ObjectTypes = ObjectControl.OKObjectTypeList.ToArray();
            GLControl.UpdateDraw = true;
        }
        public void GLRequestUpdate(object sender, EventArgs e)
        {
            switch (GLControl.TargetingMode)
            {
                default:
                    {
                        break;
                    }
                case GLViewer.ControlMode.Render:
                    {
                        //render mesh
                        if (GLControl.SelectedRender != -1)
                        {
                            RenderMeshListBox.SelectedIndex = GLControl.SelectedRender;
                        }
                        UpdateRMDisplay();
                        break;
                    }
                case GLViewer.ControlMode.Section:
                    {
                        //section lists
                        sectionList[sectionBox.SelectedIndex].viewList[0].objectList = GLControl.SectionList;
                        XLUSectionList[sectionBox.SelectedIndex].viewList[0].objectList = GLControl.SectionList;
                        if (GLControl.SelectedSection != -1)
                        {
                            SelectObjectIndex(GLControl.SelectedSection);
                        }
                        UpdateSVDisplay();
                        break;
                    }

                case GLViewer.ControlMode.Surface:
                    {
                        //render mesh
                        if (GLControl.SelectedSurface != -1)
                        {
                            SurfaceMeshListBox.SelectedIndex = GLControl.SelectedSurface;
                        }
                        updateSMDisplay();
                        break;
                    }
                case GLViewer.ControlMode.Objects:
                    {
                        //ok objects
                        OKObjectList = GLControl.CourseObjects;
                        ObjectControl.OKObjectList = OKObjectList;

                        switch (GLControl.RequestMode)
                        {
                            default:
                                {
                                    break;
                                }
                            case 1:
                                {
                                    ObjectControl.CreateObject(OKObjectList[OKObjectList.Count - 1]);
                                    break;
                                }
                            case 3:
                                {
                                    ObjectControl.ObjectListBox.SelectedIndex = GLControl.OKSelectedObject;
                                    break;
                                }
                        }
                        break;
                        
                    }
            }
            GLControl.OKObjectIndex = ObjectControl.ObjectTypeIndexBox.SelectedIndex;
            GLControl.OKSelectedObject = ObjectControl.ObjectListBox.SelectedIndex;
        }


        private void SelectObjectIndex(int MasterIndex)
        {
            for (int currentTree = 0; currentTree < masterBox.Nodes.Count; currentTree++)
            {
                if (masterBox.Nodes[currentTree].Nodes.Count > 0)
                {
                    for (int currentNode = 0; currentNode < masterBox.Nodes[currentTree].Nodes.Count; currentNode++)
                    {
                        if (masterBox.Nodes[currentTree].Nodes[currentNode].Name == masterObjects[MasterIndex].objectName)
                        {
                            masterBox.SelectedNode = masterBox.Nodes[currentTree].Nodes[currentNode];
                        }
                    }
                }
                else
                {
                    if (masterBox.Nodes[currentTree].Name == masterObjects[MasterIndex].objectName)
                    {
                        masterBox.SelectedNode = masterBox.Nodes[currentTree];
                        
                    }
                }
            }
        }


        private void UpdateSVDisplay()
        {
            if (!updateBool)
            {
                updateBool = true;
                if (loaded == true)
                {
                    int vertCount = 0;
                    int faceCount = 0;
                    for (int currentTree = 0; currentTree < masterBox.Nodes.Count; currentTree++)
                    {
                        if (masterBox.Nodes[currentTree].Nodes.Count > 0)
                        {
                            for (int currentNode = 0; currentNode < masterBox.Nodes[currentTree].Nodes.Count; currentNode++)
                            {
                                masterBox.Nodes[currentTree].Nodes[currentNode].Checked = false;
                            }
                        }
                        else
                        {
                            masterBox.Nodes[currentTree].Checked = false;
                        }
                    }
                    foreach (var subObject in sectionList[sectionBox.SelectedIndex].viewList[0].objectList)
                    {

                        TreeNode[] thisNode = masterBox.Nodes.Find(masterObjects[subObject].objectName, true);
                        thisNode[0].Checked = true;
                        vertCount = vertCount + masterObjects[subObject].vertCount;
                        faceCount = faceCount + masterObjects[subObject].faceCount;

                    }
                    updateCounter(faceCount);
                }
                updateBool = false;
            }
            GLControl.SectionList = sectionList[sectionBox.SelectedIndex].viewList[0].objectList;
        }
        //Seperate loading the counters to prevent infinite loop. 
        private void updateCounter(int faceCount)
        {
            int objectCount = 0;
            faceBox.Text = faceCount.ToString();
            for (int currentTree = 0; currentTree < masterBox.Nodes.Count; currentTree++)
            {
                if (masterBox.Nodes[currentTree].Nodes.Count > 0)
                {
                    for (int currentNode = 0; currentNode < masterBox.Nodes[currentTree].Nodes.Count; currentNode++)
                    {
                        if (masterBox.Nodes[currentTree].Nodes[currentNode].Checked == true)
                        {
                            objectCount++;
                        }
                    }
                }
                else
                {
                    if (masterBox.Nodes[currentTree].Checked == true)
                    {
                        objectCount++;
                    }
                }
            }
            objectCountBox.Text = objectCount.ToString();
        }
        //
        private void updateSMDisplay()
        {
            if (loaded == true)
            {
                int objectIndex = SurfaceMeshListBox.SelectedIndex;

                surfsectionBox.SelectedIndex = surfaceObjects[objectIndex].surfaceID - 1;
                int materialIndex = Array.IndexOf(surfaceTypeID, surfaceObjects[objectIndex].surfaceMaterial);
                surfmaterialBox.SelectedIndex = materialIndex;
                surfpropertybox.SelectedIndex = surfaceObjects[objectIndex].surfaceProperty;

                SurfaceVertBox.Text = surfaceObjects[objectIndex].vertCount.ToString();
                SurfaceFaceBox.Text = surfaceObjects[objectIndex].faceCount.ToString();

                CheckStop = false;

                GPBoxC.Checked = surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[0];
                TTBoxC.Checked = surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[1];
                VSBoxC.Checked = surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[2];
                BattleBoxC.Checked = surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[2];
                FiftyBoxC.Checked = surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[4];
                HundredBoxC.Checked = surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[5];
                HundredFiftyBoxC.Checked = surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[6];
                ExtraBoxC.Checked = surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[7];
                CheckStop = true;
            }
        }

        void UpdateRMDisplay()
        {
            if (loaded)
            {

                int objectIndex = RenderMeshListBox.SelectedIndex;

                if ((objectIndex >= 0) && (objectIndex < masterObjects.Length))
                {
                    RenderVertBox.Text = masterObjects[objectIndex].vertCount.ToString();
                    RenderFaceBox.Text = masterObjects[objectIndex].faceCount.ToString();

                    RenderMaterialBox.SelectedIndex = masterObjects[objectIndex].materialID;

                    CheckStop = false;
                    int Index = RenderMeshListBox.SelectedIndex;
                    GPBoxR.Checked = masterObjects[Index].KillDisplayList[0];
                    TTBoxR.Checked = masterObjects[Index].KillDisplayList[1];
                    VSBoxR.Checked = masterObjects[Index].KillDisplayList[2];
                    BattleBoxR.Checked = masterObjects[Index].KillDisplayList[3];
                    CC50BoxR.Checked = masterObjects[Index].KillDisplayList[4];
                    CC100BoxR.Checked = masterObjects[Index].KillDisplayList[5];
                    CC150BoxR.Checked = masterObjects[Index].KillDisplayList[6];
                    CCExtraBoxR.Checked = masterObjects[Index].KillDisplayList[7];

                    CheckStop = true;
                }
                
            }
        }

        private bool UpdateTXDisplay()        
        {
            return (TextureControl.UpdateTextureDisplay());
        }


        private void ObjectBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {

            this.BeginInvoke(new MethodInvoker(CheckList), null);
        }

        private void CheckList()
        {
            if (loaded == true)
            {
                int vertCount = 0;
                int faceCount = 0;
                int currentIndex = 0;
                List<int> checkList = new List<int>();
                for (int currentTree = 0; currentTree < masterBox.Nodes.Count; currentTree++)
                {
                    currentIndex++;
                    if (masterBox.Nodes[currentTree].Nodes.Count > 0)
                    {
                        for (int currentNode = 0; currentNode < masterBox.Nodes[currentTree].Nodes.Count; currentNode++)
                        {
                            currentIndex++;
                            if (masterBox.Nodes[currentTree].Nodes[currentNode].Checked == true)
                            {
                                checkList.Add(currentIndex);
                                vertCount = vertCount + masterObjects[currentIndex].vertCount;
                                faceCount = faceCount + masterObjects[currentIndex].faceCount;
                            }
                        }
                    }
                    else
                    {
                        if (masterBox.Nodes[currentTree].Checked == true)
                        {
                            checkList.Add(currentIndex);
                            vertCount = vertCount + masterObjects[currentIndex].vertCount;
                            faceCount = faceCount + masterObjects[currentIndex].faceCount;
                        }
                    }
                }
                sectionList[sectionBox.SelectedIndex].viewList[0].objectList = checkList.ToArray();
                XLUSectionList[sectionBox.SelectedIndex].viewList[0].objectList = checkList.ToArray();
                updateCounter(faceCount);
            }
        }


        private void SurfaceobjectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateSMDisplay();
        }

        private void SurfsectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SurfaceMeshListBox.SelectedIndex != -1)
                surfaceObjects[SurfaceMeshListBox.SelectedIndex].surfaceID = surfsectionBox.SelectedIndex + 1;
        }

        private void SurfmaterialBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (surfmaterialBox.SelectedIndex != -1)
                surfaceObjects[SurfaceMeshListBox.SelectedIndex].surfaceMaterial = surfaceTypeID[surfmaterialBox.SelectedIndex];
        }


        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void surfacepropertybox_SelectedIndexChanged(object sender, EventArgs e)
        {
            surfaceObjects[SurfaceMeshListBox.SelectedIndex].surfaceProperty = surfpropertybox.SelectedIndex;
        }

        private void updateSectionList(string objectName, int Index)
        {
            List<int> objectList = sectionList[sectionBox.SelectedIndex].viewList[0].objectList.ToList();
            
            if (Index > -1)
            {
                int objectCount = objectList.Count; //this value is dynamic as we add/remove items.
                for (int currentObject = 0; currentObject < objectCount; currentObject++)
                {
                    if (currentObject < objectList.Count) //dynamic
                    {
                        if (objectList[currentObject] == Index)
                        {
                            objectList.RemoveAt(currentObject);
                            break;
                        }
                        else
                        {
                            if (currentObject + 1 == objectList.Count)
                            {
                                objectList.Add(Index);
                            }
                        }
                    }
                }
                sectionList[sectionBox.SelectedIndex].viewList[0].objectList = objectList.ToArray();
                XLUSectionList[sectionBox.SelectedIndex].viewList[0].objectList = objectList.ToArray();
            }
            UpdateSVDisplay();
        }

        private void masterBox_AfterCheck(object sender, TreeViewEventArgs e)
        {
            UpdateDraw = true;
            if (e.Action != TreeViewAction.Unknown)
            {
                List<int> checkList = sectionList[sectionBox.SelectedIndex].viewList[0].objectList.ToList();
                bool checkState = e.Node.Checked;
                if (loaded == true)
                {
                    if (e.Node.Nodes.Count > 0)
                    {                        
                        foreach (TreeNode childNode in e.Node.Nodes)
                        {
                            if (childNode.Checked != checkState)
                            {
                                updateSectionList(childNode.Name, childNode.Index);
                                childNode.Checked = checkState;
                            }
                        }
                    }
                    else
                    {
                        updateSectionList(e.Node.Name, e.Node.Index);
                    }
                    
                    
                }
            }
        }

        private void TextureData_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGLView();
        }


        private void UpdateGLView()
        {
            if (loaded)
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                    case 1:
                    case 2:
                    default:
                        {
                            
                            GLControl.CourseModel = masterObjects;
                            GLControl.SurfaceModel = new TM64_Geometry.OK64F3DObject[0];
                            GLControl.CourseObjects = new List<TM64_Course.OKObject>();
                            GLControl.ObjectTypes = new TM64_Course.OKObjectType[0];
                            GLControl.SectionList = new int[0];
                            GLControl.TargetingMode = GLViewer.ControlMode.Scene;

                            break;
                        }
                    case 3:
                        {
                            //OKObjects
                            GLControl.CourseModel = masterObjects;
                            GLControl.SurfaceModel = new TM64_Geometry.OK64F3DObject[0];
                            GLControl.SectionList = new int[0];

                            GLControl.CourseObjects = OKObjectList;
                            GLControl.ObjectTypes = OKObjectTypeList.ToArray();


                            GLControl.TargetingMode = GLViewer.ControlMode.Objects;
                            break;
                        }
                    case 4:
                        {
                            //Section
                            List<TM64_Geometry.OK64F3DObject> SurfaceList = new List<TM64_Geometry.OK64F3DObject>();
                            for (int ThisSurface = 0; ThisSurface < surfaceObjects.Length; ThisSurface++)
                            {
                                if (surfaceObjects[ThisSurface].surfaceID - 1 == sectionBox.SelectedIndex)
                                {
                                    SurfaceList.Add(surfaceObjects[ThisSurface]);
                                }

                            }

                            GLControl.CourseModel = masterObjects;
                            GLControl.SurfaceModel = SurfaceList.ToArray();
                            GLControl.SectionList = sectionList[sectionBox.SelectedIndex].viewList[0].objectList;
                            GLControl.CourseObjects = new List<TM64_Course.OKObject>();
                            GLControl.ObjectTypes = new TM64_Course.OKObjectType[0];
                            GLControl.TargetingMode = GLViewer.ControlMode.Section;
                            break;
                        }
                    case 5:
                        {
                            //Surface
                            GLControl.CourseModel = new TM64_Geometry.OK64F3DObject[0];
                            GLControl.SurfaceModel = surfaceObjects;
                            GLControl.SectionList = new int[0];
                            GLControl.CourseObjects = new List<TM64_Course.OKObject>();
                            GLControl.ObjectTypes = new TM64_Course.OKObjectType[0];
                            GLControl.TargetingMode = GLViewer.ControlMode.Surface;
                            break;
                        }
                    case 6:
                        {
                            //Render
                            GLControl.CourseModel = masterObjects;
                            GLControl.SurfaceModel = new TM64_Geometry.OK64F3DObject[0];
                            GLControl.SectionList = new int[0];
                            GLControl.CourseObjects = new List<TM64_Course.OKObject>();
                            GLControl.ObjectTypes = new TM64_Course.OKObjectType[0];
                            GLControl.TargetingMode = GLViewer.ControlMode.Render;
                            break;
                        }
                }
                GLControl.UpdateDraw = true;
                GLControl.TextureObjects = textureArray;
                GLControl.BitmapData = TextureBitmaps;
                if ( (levelFormat == 0) && (pathGroups.Length > 0) )
                {                    
                    GLControl.PathMarker = pathGroups[0].pathList;
                }
            }
        }

        private void SettingsControl_Load(object sender, EventArgs e)
        {

        }


        private void ObjectControl_Load(object sender, EventArgs e)
        {
            ObjectRequestUpdate(sender,e);
        }


        private void CourseInfo_Click(object sender, EventArgs e)
        {

        }

        bool CheckStop = true;

        private void GPBoxC_CheckedChanged(object sender, EventArgs e)
        {
            if (SurfaceMeshListBox.SelectedIndex > -1)
            {
                if (CheckStop)
                {
                    surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[0] = GPBoxC.Checked;
                    surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[1] = TTBoxC.Checked;
                    surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[2] = VSBoxC.Checked;
                    surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[3] = BattleBoxC.Checked;
                    surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[4] = FiftyBoxC.Checked;
                    surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[5] = HundredBoxC.Checked;
                    surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[6] = HundredFiftyBoxC.Checked;
                    surfaceObjects[SurfaceMeshListBox.SelectedIndex].KillDisplayList[7] = ExtraBoxC.Checked;
                }
            }
        }

        private void TypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SurfaceMap_Click(object sender, EventArgs e)
        {

        }

        private void TextureControl_Load(object sender, EventArgs e)
        {

        }

        private void GLControl_Load(object sender, EventArgs e)
        {

        }


        private void CopyBtn_Click(object sender, EventArgs e)
        {
            sectionList[sectionBox.SelectedIndex].viewList[0].objectList =
                sectionList[CopySectionIndexBox.SelectedIndex].viewList[0].objectList;
            UpdateSVDisplay();
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            FileOpen.InitialDirectory = okSettings.ProjectDirectory;
            FileOpen.Filter = "Tarmac Course|*.ok64.Save|All Files (*.*)|*.*";
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                string SavePath = FileOpen.FileName;

                MemoryStream memoryStream = new MemoryStream();

                SettingsControl.LoadCourseSettings(memoryStream);
                PathControl.
                binaryWriter.Write(SettingsControl.SaveCourseSettings());
                binaryWriter.Write(PathControl.SavePathSettings());
                binaryWriter.Write(TextureControl.SaveTextureArray());
                binaryWriter.Write(ObjectControl.SaveObjectSettings());

                for (int ThisSection = 0; ThisSection < sectionCount; ThisSection++)
                {
                    binaryWriter.Write(sectionList[ThisSection].SaveData());
                }

                for (int ThisSurface = 0; ThisSurface < surfaceObjects.Length; ThisSurface++)
                {
                    binaryWriter.Write(surfaceObjects[ThisSurface].SaveData());
                }

                for (int ThisMaster = 0; ThisMaster < masterObjects.Length; ThisMaster++)
                {
                    binaryWriter.Write(masterObjects[ThisMaster].SaveData());
                }

            }

        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog FileSave = new SaveFileDialog();
            FileSave.InitialDirectory = okSettings.ProjectDirectory;
            FileSave.Filter = "Tarmac Course|*.ok64.Save|All Files (*.*)|*.*";
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                string SavePath = FileSave.FileName;

                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

                binaryWriter.Write(SettingsControl.SaveCourseSettings());
                binaryWriter.Write(PathControl.SavePathSettings());
                binaryWriter.Write(TextureControl.SaveTextureArray());
                binaryWriter.Write(ObjectControl.SaveObjectSettings());
                
                for (int ThisSection = 0; ThisSection < sectionCount; ThisSection++)
                {
                    binaryWriter.Write(sectionList[ThisSection].SaveData());
                }

                for (int ThisSurface = 0; ThisSurface < surfaceObjects.Length; ThisSurface++)
                {
                    binaryWriter.Write(surfaceObjects[ThisSurface].SaveData());
                }

                for (int ThisMaster = 0; ThisMaster < masterObjects.Length; ThisMaster++)
                {
                    binaryWriter.Write(masterObjects[ThisMaster].SaveData());
                }

                File.WriteAllBytes(SavePath, memoryStream.ToArray());
            }

        }

        private void GoalBannerBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void UpdateUI(object sender, EventArgs e)
        {

        }

        private void UpdateUIHandler(object sender, System.Windows.Forms.KeyEventArgs e)
        {

        }

        private void UpdateUIHandler(object sender, EventArgs e)
        {

        }

        private void EchoPowerBox_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void ColorPickAdjust_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void EchoIndexBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ColorPickBase_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            TarmacAbout About = new TarmacAbout();
            About.Show();
        }

        private void objectTypeCompilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectTypeCompiler ObjComp = new ObjectTypeCompiler();
            ObjComp.Show();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SettingsWindow SetWind = new SettingsWindow();
            SetWind.Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplaceModel(false);
            ReplacePaths();
        }

        private void replaceModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplaceModel(true);
        }

        private void replacePathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacePaths();
        }

        private void RenderMeshListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRMDisplay();
        }

        private void RenderMaterialBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                masterObjects[RenderMeshListBox.SelectedIndex].materialID = RenderMaterialBox.SelectedIndex;
                GLControl.UpdateDraw = true;
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemoryStream SaveStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(SaveStream);
            for (int ThisObj = 0; ThisObj < masterObjects.Length; ThisObj++)
            {
                binaryWriter.Write(masterObjects[ThisObj].SaveData());
            }

            byte[] LoadData = SaveStream.ToArray();
            MemoryStream LoadStream = new MemoryStream();
            LoadStream.Write(LoadData, 0, LoadData.Length);
            LoadStream.Position = 0;
            for (int ThisObj = 0; ThisObj < masterObjects.Length; ThisObj++)
            {
                masterObjects[ThisObj] = new TM64_Geometry.OK64F3DObject(LoadStream);
            }
        }

        private void masterBox_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }



        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (masterBox.SelectedNode != null)
            {
                if (CheckStop)
                {
                    masterObjects[masterBox.SelectedNode.Index].KillDisplayList[0] = GPBoxR.Checked;
                    masterObjects[masterBox.SelectedNode.Index].KillDisplayList[1] = TTBoxR.Checked;
                    masterObjects[masterBox.SelectedNode.Index].KillDisplayList[2] = VSBoxR.Checked;
                    masterObjects[masterBox.SelectedNode.Index].KillDisplayList[3] = BattleBoxR.Checked;
                    masterObjects[masterBox.SelectedNode.Index].KillDisplayList[4] = CC50BoxR.Checked;
                    masterObjects[masterBox.SelectedNode.Index].KillDisplayList[5] = CC100BoxR.Checked;
                    masterObjects[masterBox.SelectedNode.Index].KillDisplayList[6] = CC150BoxR.Checked;
                    masterObjects[masterBox.SelectedNode.Index].KillDisplayList[7] = CC50BoxR.Checked;
                }
            }
            
        }

    }
}





