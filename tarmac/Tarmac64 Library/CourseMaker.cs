using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using Assimp;
using Tarmac64_Library;
using Tarmac64_Library.Properties;
using System.Text.RegularExpressions;
using SharpGL;
using SharpGL.SceneGraph.Core;
using System.Drawing.Design;
using System.Windows.Input;
using System.Drawing.Imaging;
using Cereal64.Microcodes.F3DEX.DataElements;

namespace Tarmac64_Library
{


    public partial class CourseMaker : Form
    {

        TM64 Tarmac = new TM64();
        TM64_Course TarmacCourse = new TM64_Course();
        TM64_GL TarmacGL = new TM64_GL();
        TM64_Geometry TarmacGeometry = new TM64_Geometry();
        

        TM64.OK64Settings okSettings = new TM64.OK64Settings();

        bool updateBool = false;

        int raycastBoolean = 0;
        int sectionCount = 0;
        public int programFormat;
        int levelFormat = 0;



        public CourseMaker()
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

            skyData.MidBotColor = new TM64_Geometry.OK64Color();
            skyData.MidBotColor.R = 0;
            skyData.MidBotColor.G = 0;
            skyData.MidBotColor.B = 0;
            skyData.MidBotColor.A = 0;

            skyData.MidTopColor = new TM64_Geometry.OK64Color();
            skyData.MidTopColor.R = 0;
            skyData.MidTopColor.G = 0;
            skyData.MidTopColor.B = 0;
            skyData.MidTopColor.A = 0;

            skyData.BotColor = new TM64_Geometry.OK64Color();
            skyData.BotColor.R = 0;
            skyData.BotColor.G = 0;
            skyData.BotColor.B = 0;
            skyData.BotColor.A = 0;
        }
        private void FormLoad(object sender, EventArgs e)
        {
            CreateGeometry();
            CreateColors();


            okSettings = Tarmac.LoadSettings();

            gl = openGLControl.OpenGL;
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Tarmac64 Designer")
            {
                about_button.Visible = true;
            }
            else
            {
                about_button.Visible = false;
            }
            courseBox.SelectedIndex = 0;
            cupBox.SelectedIndex = 0;
            setBox.SelectedIndex = 0;

            SkyRMT.Text = "216";
            SkyGMT.Text = "232";
            SkyBMT.Text = "248";

            mapXBox.Text = "260";
            mapYBox.Text = "170";
            startXBox.Text = "6";
            startYBox.Text = "28";
            MapRBox.Text = "255";
            MapGBox.Text = "255";
            MapBBox.Text = "255";
            MapScaleBox.Text = "1.55";

            SkyRT.Text = "128";
            SkyGT.Text = "184";
            SkyBT.Text = "248";
            ColorUpdate();

            for (int songIndex = 0; songIndex < songNames.Length; songIndex++)
            {
                songBox.Items.Add(songNames[songIndex]);
            }
            songBox.SelectedIndex = 3;


            for (int startIndex = -20; startIndex < 20; startIndex++)
            {
                ChunkXBox.Items.Add(startIndex.ToString());
            }
            for (int startIndex = -20; startIndex < 20; startIndex++)
            {
                ChunkYBox.Items.Add(startIndex.ToString());
            }
            for (int startIndex = -4; startIndex < 4; startIndex++)
            {
                ChunkZBox.Items.Add(startIndex.ToString());
            }

            sp1Box.Text = "2";
            sp2Box.Text = "2";
            sp3Box.Text = "2";
            sp4Box.Text = "2";

            EchoStartBox.Text = "411";
            EchoStopBox.Text = "440";

            waterBox.Text = "-80";
        }

        Stopwatch clockTime = new Stopwatch();
        bool loadGL = false;
        SharpGL.SceneGraph.Assets.Texture glTexture = new SharpGL.SceneGraph.Assets.Texture();

        uint[] gtexture = new uint[1];
        Bitmap gImage1;
        float[] flashRed = { 1.0f, 0.0f, 0.0f, 1.0f, 0.0f };
        float[] flashYellow = { 1.0f, 1.0f, 1.0f, 1.0f, 0.0f };
        float[] flashWhite = { 1.0f, 1.0f, 1.0f, 0.5f, 0.0f };
        int[] highlightedObject = new int[] { -50, -50, -50 };
        int moveDistance = 50;


        TM64_Geometry tm64Geo = new TM64_Geometry();
        TM64_Paths tm64Path = new TM64_Paths();
        TM64 tm64 = new TM64();

        TM64_Geometry.OK64SectionList[] sectionList = new TM64_Geometry.OK64SectionList[0];
        TM64_Geometry.OK64SectionList[] surfaceList = new TM64_Geometry.OK64SectionList[0];

        TM64_Geometry.Face[] markerGeometry = new TM64_Geometry.Face[2];
        TM64_Geometry.Face[] treeGeometry = new TM64_Geometry.Face[4];
        TM64_Geometry.Face[] piranhaGeometry = new TM64_Geometry.Face[4];
        TM64_Geometry.Face[] itemGeometry = new TM64_Geometry.Face[4];

        MemoryStream bs = new MemoryStream();
        BinaryReader br = new BinaryReader(Stream.Null);
        BinaryWriter bw = new BinaryWriter(Stream.Null);
        MemoryStream ds = new MemoryStream();
        BinaryReader dr = new BinaryReader(Stream.Null);
        BinaryWriter dw = new BinaryWriter(Stream.Null);
        MemoryStream vs = new MemoryStream();
        BinaryReader vr = new BinaryReader(Stream.Null);

        uint[] gltextureArray = new uint[0];

        string[] viewString = new string[] { "North", "East", "South", "West" };

        string[] surfaceType = new string[] { "Solid", "Dirt", "Dirt Track", "Cement", "Snow Track", "Wood", "Dirt Off-Road", "Grass", "Ice", "Beach Sand", "Snow Off-Road", "Rock Walls", "Dirt Off-Road", "Train Tracks", "Cave Interior", "Rickety Wood Bridge", "Solid Wood Bridge", "DK Parkyway Boost", "Out-Of-Bounds", "Royal Raceway Boost", "Walls" };
        int[] surfaceTypeID = new int[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0xFC, 0xFD, 0xFE, 0xFF };

        public string[] songNames = new string[] { "None", "Title", "Menu", "Raceways", "Moo Moo Farm", "Choco Mountain", "Koopa Troopa Beach", "Banshee Boardwalk", "Snowland", "Bowser's Castle", "Kalimari Desert", "#- GP Startup", "#- Final Lap", "#- Final Lap (1st)", "#- Final Lap 2-4", "#- You Lose", "#- Race Results", "Star Music", "Rainbow Road", "DK Parkway", "#- Credits Failure", "Toad's Turnpike", "#- VS/Battle Start", "#- VS/Battle Results", "#- Retry/Quit", "Big Donut / Skyscraper", "#- Trophy A", "#- Trophy B1 (Win)", "Credits", "#- Trophy B2 (Lose)" };

        bool loaded = false;

        string FBXfilePath = "";
        TM64_Paths.Pathgroup[] pathGroups = new TM64_Paths.Pathgroup[0];
        string popFile = "";

        
        TM64_Geometry.OK64F3DGroup[] masterGroups = new TM64_Geometry.OK64F3DGroup[0];
        TM64_Geometry.OK64JRBlock[] BlockObjects = new TM64_Geometry.OK64JRBlock[0];

        List<TM64_Geometry.OK64JRSpace> CourseObjects = new List<TM64_Geometry.OK64JRSpace>();

        TM64_Geometry.OK64JRBlock EmptyBlock = new TM64_Geometry.OK64JRBlock();
        TM64_Geometry.OK64JRBlock ZoneBlock = new TM64_Geometry.OK64JRBlock();

        List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>();

        TM64_Geometry.OK64F3DObject[] surfaceObjects = new TM64_Geometry.OK64F3DObject[0];


        TM64_Geometry.OK64Texture[] textureArray = new TM64_Geometry.OK64Texture[0];

        int lastMaterial = 0;

        TM64_Course.MiniMap mapData = new TM64_Course.MiniMap();
        TM64_Course.Sky skyData = new TM64_Course.Sky();
        int[] gameSpeed = new int[0];

        AssimpContext AssimpImporter = new AssimpContext();
        Assimp.Scene fbx = new Assimp.Scene();
        int materialCount;

        TM64_GL.TMCamera localCamera = new TM64_GL.TMCamera();
        OpenGL gl = new OpenGL();



        OpenFileDialog fileOpen = new OpenFileDialog();
        SaveFileDialog fileSave = new SaveFileDialog();
        FolderBrowserDialog folderOpen = new FolderBrowserDialog();




        private void CreateGeometry()
        {
            markerGeometry[0] = new TM64_Geometry.Face();
            markerGeometry[0].VertData = new TM64_Geometry.Vertex[3];

            markerGeometry[0].VertData[0] = new TM64_Geometry.Vertex();
            markerGeometry[0].VertData[0].position = new TM64_Geometry.Position();
            markerGeometry[0].VertData[0].position.x = Convert.ToInt16(-5);
            markerGeometry[0].VertData[0].position.y = Convert.ToInt16(0);
            markerGeometry[0].VertData[0].position.z = Convert.ToInt16(0);

            markerGeometry[0].VertData[1] = new TM64_Geometry.Vertex();
            markerGeometry[0].VertData[1].position = new TM64_Geometry.Position();
            markerGeometry[0].VertData[1].position.x = Convert.ToInt16(5);
            markerGeometry[0].VertData[1].position.y = Convert.ToInt16(0);
            markerGeometry[0].VertData[1].position.z = Convert.ToInt16(0);

            markerGeometry[0].VertData[2] = new TM64_Geometry.Vertex();
            markerGeometry[0].VertData[2].position = new TM64_Geometry.Position();
            markerGeometry[0].VertData[2].position.x = Convert.ToInt16(0);
            markerGeometry[0].VertData[2].position.y = Convert.ToInt16(0);
            markerGeometry[0].VertData[2].position.z = Convert.ToInt16(10);


            markerGeometry[1] = new TM64_Geometry.Face();
            markerGeometry[1].VertData = new TM64_Geometry.Vertex[3];

            markerGeometry[1].VertData[0] = new TM64_Geometry.Vertex();
            markerGeometry[1].VertData[0].position = new TM64_Geometry.Position();
            markerGeometry[1].VertData[0].position.x = Convert.ToInt16(0);
            markerGeometry[1].VertData[0].position.y = Convert.ToInt16(-5);
            markerGeometry[1].VertData[0].position.z = Convert.ToInt16(0);

            markerGeometry[1].VertData[1] = new TM64_Geometry.Vertex();
            markerGeometry[1].VertData[1].position = new TM64_Geometry.Position();
            markerGeometry[1].VertData[1].position.x = Convert.ToInt16(0);
            markerGeometry[1].VertData[1].position.y = Convert.ToInt16(5);
            markerGeometry[1].VertData[1].position.z = Convert.ToInt16(0);

            markerGeometry[1].VertData[2] = new TM64_Geometry.Vertex();
            markerGeometry[1].VertData[2].position = new TM64_Geometry.Position();
            markerGeometry[1].VertData[2].position.x = Convert.ToInt16(0);
            markerGeometry[1].VertData[2].position.y = Convert.ToInt16(0);
            markerGeometry[1].VertData[2].position.z = Convert.ToInt16(10);

            treeGeometry[0] = new TM64_Geometry.Face();

            treeGeometry[0].VertData = new TM64_Geometry.Vertex[3];

            treeGeometry[0].VertData[0] = new TM64_Geometry.Vertex();
            treeGeometry[0].VertData[0].position = new TM64_Geometry.Position();
            treeGeometry[0].VertData[0].position.x = Convert.ToInt16(-20);
            treeGeometry[0].VertData[0].position.y = Convert.ToInt16(0);
            treeGeometry[0].VertData[0].position.z = Convert.ToInt16(0);
            treeGeometry[0].VertData[0].position.u = 0.0f;
            treeGeometry[0].VertData[0].position.v = 1.0f;

            treeGeometry[0].VertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[0].VertData[1].position = new TM64_Geometry.Position();
            treeGeometry[0].VertData[1].position.x = Convert.ToInt16(20);
            treeGeometry[0].VertData[1].position.y = Convert.ToInt16(0);
            treeGeometry[0].VertData[1].position.z = Convert.ToInt16(0);
            treeGeometry[0].VertData[1].position.u = 1.0f;
            treeGeometry[0].VertData[1].position.v = 1.0f;

            treeGeometry[0].VertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[0].VertData[2].position = new TM64_Geometry.Position();
            treeGeometry[0].VertData[2].position.x = Convert.ToInt16(20);
            treeGeometry[0].VertData[2].position.y = Convert.ToInt16(0);
            treeGeometry[0].VertData[2].position.z = Convert.ToInt16(80);
            treeGeometry[0].VertData[2].position.u = 1.0f;
            treeGeometry[0].VertData[2].position.v = 0.0f;


            treeGeometry[1] = new TM64_Geometry.Face();
            treeGeometry[1].VertData = new TM64_Geometry.Vertex[3];

            treeGeometry[1].VertData[0] = new TM64_Geometry.Vertex();
            treeGeometry[1].VertData[0].position = new TM64_Geometry.Position();
            treeGeometry[1].VertData[0].position.x = Convert.ToInt16(0);
            treeGeometry[1].VertData[0].position.y = Convert.ToInt16(-20);
            treeGeometry[1].VertData[0].position.z = Convert.ToInt16(0);
            treeGeometry[1].VertData[0].position.u = 0.0f;
            treeGeometry[1].VertData[0].position.v = 1.0f;

            treeGeometry[1].VertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[1].VertData[1].position = new TM64_Geometry.Position();
            treeGeometry[1].VertData[1].position.x = Convert.ToInt16(0);
            treeGeometry[1].VertData[1].position.y = Convert.ToInt16(20);
            treeGeometry[1].VertData[1].position.z = Convert.ToInt16(0);
            treeGeometry[1].VertData[1].position.u = 1.0f;
            treeGeometry[1].VertData[1].position.v = 1.0f;

            treeGeometry[1].VertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[1].VertData[2].position = new TM64_Geometry.Position();
            treeGeometry[1].VertData[2].position.x = Convert.ToInt16(0);
            treeGeometry[1].VertData[2].position.y = Convert.ToInt16(20);
            treeGeometry[1].VertData[2].position.z = Convert.ToInt16(80);
            treeGeometry[1].VertData[2].position.u = 1.0f;
            treeGeometry[1].VertData[2].position.v = 0.0f;

            treeGeometry[2] = new TM64_Geometry.Face();
            treeGeometry[2].VertData = new TM64_Geometry.Vertex[3];

            treeGeometry[2].VertData[0] = new TM64_Geometry.Vertex();
            treeGeometry[2].VertData[0].position = new TM64_Geometry.Position();
            treeGeometry[2].VertData[0].position.x = Convert.ToInt16(-20);
            treeGeometry[2].VertData[0].position.y = Convert.ToInt16(0);
            treeGeometry[2].VertData[0].position.z = Convert.ToInt16(0);
            treeGeometry[2].VertData[0].position.u = 0.0f;
            treeGeometry[2].VertData[0].position.v = 1.0f;

            treeGeometry[2].VertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[2].VertData[1].position = new TM64_Geometry.Position();
            treeGeometry[2].VertData[1].position.x = Convert.ToInt16(20);
            treeGeometry[2].VertData[1].position.y = Convert.ToInt16(0);
            treeGeometry[2].VertData[1].position.z = Convert.ToInt16(80);
            treeGeometry[2].VertData[1].position.u = 1.0f;
            treeGeometry[2].VertData[1].position.v = 0.0f;

            treeGeometry[2].VertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[2].VertData[2].position = new TM64_Geometry.Position();
            treeGeometry[2].VertData[2].position.x = Convert.ToInt16(-20);
            treeGeometry[2].VertData[2].position.y = Convert.ToInt16(0);
            treeGeometry[2].VertData[2].position.z = Convert.ToInt16(80);
            treeGeometry[2].VertData[2].position.u = 0.0f;
            treeGeometry[2].VertData[2].position.v = 0.0f;


            treeGeometry[3] = new TM64_Geometry.Face();
            treeGeometry[3].VertData = new TM64_Geometry.Vertex[3];

            treeGeometry[3].VertData[0] = new TM64_Geometry.Vertex();
            treeGeometry[3].VertData[0].position = new TM64_Geometry.Position();
            treeGeometry[3].VertData[0].position.x = Convert.ToInt16(0);
            treeGeometry[3].VertData[0].position.y = Convert.ToInt16(-20);
            treeGeometry[3].VertData[0].position.z = Convert.ToInt16(0);
            treeGeometry[3].VertData[0].position.u = 0.0f;
            treeGeometry[3].VertData[0].position.v = 1.0f;

            treeGeometry[3].VertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[3].VertData[1].position = new TM64_Geometry.Position();
            treeGeometry[3].VertData[1].position.x = Convert.ToInt16(0);
            treeGeometry[3].VertData[1].position.y = Convert.ToInt16(20);
            treeGeometry[3].VertData[1].position.z = Convert.ToInt16(80);
            treeGeometry[3].VertData[1].position.u = 1.0f;
            treeGeometry[3].VertData[1].position.v = 0.0f;

            treeGeometry[3].VertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[3].VertData[2].position = new TM64_Geometry.Position();
            treeGeometry[3].VertData[2].position.x = Convert.ToInt16(0);
            treeGeometry[3].VertData[2].position.y = Convert.ToInt16(-20);
            treeGeometry[3].VertData[2].position.z = Convert.ToInt16(80);
            treeGeometry[3].VertData[2].position.u = 0.0f;
            treeGeometry[3].VertData[2].position.v = 0.0f;




            piranhaGeometry[0] = new TM64_Geometry.Face();

            piranhaGeometry[0].VertData = new TM64_Geometry.Vertex[3];

            piranhaGeometry[0].VertData[0] = new TM64_Geometry.Vertex();
            piranhaGeometry[0].VertData[0].position = new TM64_Geometry.Position();
            piranhaGeometry[0].VertData[0].position.x = Convert.ToInt16(-30);
            piranhaGeometry[0].VertData[0].position.y = Convert.ToInt16(0);
            piranhaGeometry[0].VertData[0].position.z = Convert.ToInt16(0);

            piranhaGeometry[0].VertData[1] = new TM64_Geometry.Vertex();
            piranhaGeometry[0].VertData[1].position = new TM64_Geometry.Position();
            piranhaGeometry[0].VertData[1].position.x = Convert.ToInt16(30);
            piranhaGeometry[0].VertData[1].position.y = Convert.ToInt16(0);
            piranhaGeometry[0].VertData[1].position.z = Convert.ToInt16(0);

            piranhaGeometry[0].VertData[2] = new TM64_Geometry.Vertex();
            piranhaGeometry[0].VertData[2].position = new TM64_Geometry.Position();
            piranhaGeometry[0].VertData[2].position.x = Convert.ToInt16(30);
            piranhaGeometry[0].VertData[2].position.y = Convert.ToInt16(0);
            piranhaGeometry[0].VertData[2].position.z = Convert.ToInt16(60);


            piranhaGeometry[1] = new TM64_Geometry.Face();
            piranhaGeometry[1].VertData = new TM64_Geometry.Vertex[3];

            piranhaGeometry[1].VertData[0] = new TM64_Geometry.Vertex();
            piranhaGeometry[1].VertData[0].position = new TM64_Geometry.Position();
            piranhaGeometry[1].VertData[0].position.x = Convert.ToInt16(0);
            piranhaGeometry[1].VertData[0].position.y = Convert.ToInt16(-30);
            piranhaGeometry[1].VertData[0].position.z = Convert.ToInt16(0);

            piranhaGeometry[1].VertData[1] = new TM64_Geometry.Vertex();
            piranhaGeometry[1].VertData[1].position = new TM64_Geometry.Position();
            piranhaGeometry[1].VertData[1].position.x = Convert.ToInt16(0);
            piranhaGeometry[1].VertData[1].position.y = Convert.ToInt16(30);
            piranhaGeometry[1].VertData[1].position.z = Convert.ToInt16(0);

            piranhaGeometry[1].VertData[2] = new TM64_Geometry.Vertex();
            piranhaGeometry[1].VertData[2].position = new TM64_Geometry.Position();
            piranhaGeometry[1].VertData[2].position.x = Convert.ToInt16(0);
            piranhaGeometry[1].VertData[2].position.y = Convert.ToInt16(30);
            piranhaGeometry[1].VertData[2].position.z = Convert.ToInt16(60);

            piranhaGeometry[2] = new TM64_Geometry.Face();
            piranhaGeometry[2].VertData = new TM64_Geometry.Vertex[3];

            piranhaGeometry[2].VertData[0] = new TM64_Geometry.Vertex();
            piranhaGeometry[2].VertData[0].position = new TM64_Geometry.Position();
            piranhaGeometry[2].VertData[0].position.x = Convert.ToInt16(-30);
            piranhaGeometry[2].VertData[0].position.y = Convert.ToInt16(0);
            piranhaGeometry[2].VertData[0].position.z = Convert.ToInt16(0);

            piranhaGeometry[2].VertData[1] = new TM64_Geometry.Vertex();
            piranhaGeometry[2].VertData[1].position = new TM64_Geometry.Position();
            piranhaGeometry[2].VertData[1].position.x = Convert.ToInt16(30);
            piranhaGeometry[2].VertData[1].position.y = Convert.ToInt16(0);
            piranhaGeometry[2].VertData[1].position.z = Convert.ToInt16(60);

            piranhaGeometry[2].VertData[2] = new TM64_Geometry.Vertex();
            piranhaGeometry[2].VertData[2].position = new TM64_Geometry.Position();
            piranhaGeometry[2].VertData[2].position.x = Convert.ToInt16(-30);
            piranhaGeometry[2].VertData[2].position.y = Convert.ToInt16(0);
            piranhaGeometry[2].VertData[2].position.z = Convert.ToInt16(60);


            piranhaGeometry[3] = new TM64_Geometry.Face();
            piranhaGeometry[3].VertData = new TM64_Geometry.Vertex[3];

            piranhaGeometry[3].VertData[0] = new TM64_Geometry.Vertex();
            piranhaGeometry[3].VertData[0].position = new TM64_Geometry.Position();
            piranhaGeometry[3].VertData[0].position.x = Convert.ToInt16(0);
            piranhaGeometry[3].VertData[0].position.y = Convert.ToInt16(-30);
            piranhaGeometry[3].VertData[0].position.z = Convert.ToInt16(0);

            piranhaGeometry[3].VertData[1] = new TM64_Geometry.Vertex();
            piranhaGeometry[3].VertData[1].position = new TM64_Geometry.Position();
            piranhaGeometry[3].VertData[1].position.x = Convert.ToInt16(0);
            piranhaGeometry[3].VertData[1].position.y = Convert.ToInt16(30);
            piranhaGeometry[3].VertData[1].position.z = Convert.ToInt16(60);

            piranhaGeometry[3].VertData[2] = new TM64_Geometry.Vertex();
            piranhaGeometry[3].VertData[2].position = new TM64_Geometry.Position();
            piranhaGeometry[3].VertData[2].position.x = Convert.ToInt16(0);
            piranhaGeometry[3].VertData[2].position.y = Convert.ToInt16(-30);
            piranhaGeometry[3].VertData[2].position.z = Convert.ToInt16(60);


            itemGeometry[0] = new TM64_Geometry.Face();

            itemGeometry[0].VertData = new TM64_Geometry.Vertex[3];

            itemGeometry[0].VertData[0] = new TM64_Geometry.Vertex();
            itemGeometry[0].VertData[0].position = new TM64_Geometry.Position();
            itemGeometry[0].VertData[0].position.x = Convert.ToInt16(-8);
            itemGeometry[0].VertData[0].position.y = Convert.ToInt16(0);
            itemGeometry[0].VertData[0].position.z = Convert.ToInt16(0);

            itemGeometry[0].VertData[1] = new TM64_Geometry.Vertex();
            itemGeometry[0].VertData[1].position = new TM64_Geometry.Position();
            itemGeometry[0].VertData[1].position.x = Convert.ToInt16(8);
            itemGeometry[0].VertData[1].position.y = Convert.ToInt16(0);
            itemGeometry[0].VertData[1].position.z = Convert.ToInt16(0);

            itemGeometry[0].VertData[2] = new TM64_Geometry.Vertex();
            itemGeometry[0].VertData[2].position = new TM64_Geometry.Position();
            itemGeometry[0].VertData[2].position.x = Convert.ToInt16(8);
            itemGeometry[0].VertData[2].position.y = Convert.ToInt16(0);
            itemGeometry[0].VertData[2].position.z = Convert.ToInt16(16);


            itemGeometry[1] = new TM64_Geometry.Face();
            itemGeometry[1].VertData = new TM64_Geometry.Vertex[3];

            itemGeometry[1].VertData[0] = new TM64_Geometry.Vertex();
            itemGeometry[1].VertData[0].position = new TM64_Geometry.Position();
            itemGeometry[1].VertData[0].position.x = Convert.ToInt16(0);
            itemGeometry[1].VertData[0].position.y = Convert.ToInt16(-8);
            itemGeometry[1].VertData[0].position.z = Convert.ToInt16(0);

            itemGeometry[1].VertData[1] = new TM64_Geometry.Vertex();
            itemGeometry[1].VertData[1].position = new TM64_Geometry.Position();
            itemGeometry[1].VertData[1].position.x = Convert.ToInt16(0);
            itemGeometry[1].VertData[1].position.y = Convert.ToInt16(8);
            itemGeometry[1].VertData[1].position.z = Convert.ToInt16(0);

            itemGeometry[1].VertData[2] = new TM64_Geometry.Vertex();
            itemGeometry[1].VertData[2].position = new TM64_Geometry.Position();
            itemGeometry[1].VertData[2].position.x = Convert.ToInt16(0);
            itemGeometry[1].VertData[2].position.y = Convert.ToInt16(8);
            itemGeometry[1].VertData[2].position.z = Convert.ToInt16(16);

            itemGeometry[2] = new TM64_Geometry.Face();
            itemGeometry[2].VertData = new TM64_Geometry.Vertex[3];

            itemGeometry[2].VertData[0] = new TM64_Geometry.Vertex();
            itemGeometry[2].VertData[0].position = new TM64_Geometry.Position();
            itemGeometry[2].VertData[0].position.x = Convert.ToInt16(-8);
            itemGeometry[2].VertData[0].position.y = Convert.ToInt16(0);
            itemGeometry[2].VertData[0].position.z = Convert.ToInt16(0);

            itemGeometry[2].VertData[1] = new TM64_Geometry.Vertex();
            itemGeometry[2].VertData[1].position = new TM64_Geometry.Position();
            itemGeometry[2].VertData[1].position.x = Convert.ToInt16(8);
            itemGeometry[2].VertData[1].position.y = Convert.ToInt16(0);
            itemGeometry[2].VertData[1].position.z = Convert.ToInt16(16);

            itemGeometry[2].VertData[2] = new TM64_Geometry.Vertex();
            itemGeometry[2].VertData[2].position = new TM64_Geometry.Position();
            itemGeometry[2].VertData[2].position.x = Convert.ToInt16(-8);
            itemGeometry[2].VertData[2].position.y = Convert.ToInt16(0);
            itemGeometry[2].VertData[2].position.z = Convert.ToInt16(16);


            itemGeometry[3] = new TM64_Geometry.Face();
            itemGeometry[3].VertData = new TM64_Geometry.Vertex[3];

            itemGeometry[3].VertData[0] = new TM64_Geometry.Vertex();
            itemGeometry[3].VertData[0].position = new TM64_Geometry.Position();
            itemGeometry[3].VertData[0].position.x = Convert.ToInt16(0);
            itemGeometry[3].VertData[0].position.y = Convert.ToInt16(-8);
            itemGeometry[3].VertData[0].position.z = Convert.ToInt16(0);

            itemGeometry[3].VertData[1] = new TM64_Geometry.Vertex();
            itemGeometry[3].VertData[1].position = new TM64_Geometry.Position();
            itemGeometry[3].VertData[1].position.x = Convert.ToInt16(0);
            itemGeometry[3].VertData[1].position.y = Convert.ToInt16(8);
            itemGeometry[3].VertData[1].position.z = Convert.ToInt16(16);

            itemGeometry[3].VertData[2] = new TM64_Geometry.Vertex();
            itemGeometry[3].VertData[2].position = new TM64_Geometry.Position();
            itemGeometry[3].VertData[2].position.x = Convert.ToInt16(0);
            itemGeometry[3].VertData[2].position.y = Convert.ToInt16(-8);
            itemGeometry[3].VertData[2].position.z = Convert.ToInt16(16);

        }
        private void ColorUpdate()
        {
            System.Drawing.Color buttonColor = System.Drawing.Color.FromArgb(0,0,0);
            int[] colorInt = new int[3];

            colorInt[0] = 0;
            int.TryParse(SkyRT.Text, out colorInt[0]);
            colorInt[1] = 0;
            int.TryParse(SkyGT.Text, out colorInt[1]);
            colorInt[2] = 0;
            int.TryParse(SkyBT.Text, out colorInt[2]);

            if (colorInt[0] < 256 & colorInt[0] > -1 & colorInt[1] < 256 & colorInt[1] > -1 & colorInt[2] < 256 & colorInt[2] > -1)
            {
                buttonColor = System.Drawing.Color.FromArgb(colorInt[0], colorInt[1], colorInt[2]);
            }
            else
            {
                buttonColor = System.Drawing.Color.FromArgb(0,0,0);
            }
            ColorPickT.BackColor = buttonColor;

            colorInt[0] = 0;
            int.TryParse(SkyRMT.Text, out colorInt[0]);
            colorInt[1] = 0;
            int.TryParse(SkyGMT.Text, out colorInt[1]);
            colorInt[2] = 0;
            int.TryParse(SkyBMT.Text, out colorInt[2]);
            if (colorInt[0] < 256 & colorInt[0] > -1 & colorInt[1] < 256 & colorInt[1] > -1 & colorInt[2] < 256 & colorInt[2] > -1)
            {
                buttonColor = System.Drawing.Color.FromArgb(colorInt[0], colorInt[1], colorInt[2]);
            }
            else
            {
                buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
            }
            ColorPickMT.BackColor = buttonColor;

            colorInt[0] = 0;
            int.TryParse(SkyRMB.Text, out colorInt[0]);
            colorInt[1] = 0;
            int.TryParse(SkyGMB.Text, out colorInt[1]);
            colorInt[2] = 0;
            int.TryParse(SkyBMB.Text, out colorInt[2]);
            if (colorInt[0] < 256 & colorInt[0] > -1 & colorInt[1] < 256 & colorInt[1] > -1 & colorInt[2] < 256 & colorInt[2] > -1)
            {
                buttonColor = System.Drawing.Color.FromArgb(colorInt[0], colorInt[1], colorInt[2]);
            }
            else
            {
                buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
            }
            ColorPickMB.BackColor = buttonColor;

            colorInt[0] = 0;
            int.TryParse(SkyRB.Text, out colorInt[0]);
            colorInt[1] = 0;
            int.TryParse(SkyGB.Text, out colorInt[1]);
            colorInt[2] = 0;
            int.TryParse(SkyBB.Text, out colorInt[2]);
            if (colorInt[0] < 256 & colorInt[0] > -1 & colorInt[1] < 256 & colorInt[1] > -1 & colorInt[2] < 256 & colorInt[2] > -1)
            {
                buttonColor = System.Drawing.Color.FromArgb(colorInt[0], colorInt[1], colorInt[2]);
            }
            else
            {
                buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
            }
            ColorPickB.BackColor = buttonColor;

            colorInt[0] = 0;
            int.TryParse(MapRBox.Text, out colorInt[0]);
            colorInt[1] = 0;
            int.TryParse(MapGBox.Text, out colorInt[1]);
            colorInt[2] = 0;
            int.TryParse(MapBBox.Text, out colorInt[2]);
            if (colorInt[0] < 256 & colorInt[0] > -1 & colorInt[1] < 256 & colorInt[1] > -1 & colorInt[2] < 256 & colorInt[2] > -1)
            {
                buttonColor = System.Drawing.Color.FromArgb(colorInt[0], colorInt[1], colorInt[2]);
            }
            else
            {
                buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
            }
            ColorPickMap.BackColor = buttonColor;





            byte colorValue = 0;
            skyData.TopColor = new TM64_Geometry.OK64Color();
            Byte.TryParse(SkyRT.Text, out colorValue);
            skyData.TopColor.R = colorValue;
            Byte.TryParse(SkyGT.Text, out colorValue);
            skyData.TopColor.G = colorValue;
            Byte.TryParse(SkyBT.Text, out colorValue);
            skyData.TopColor.B = colorValue;

            skyData.MidTopColor = new TM64_Geometry.OK64Color();
            Byte.TryParse(SkyRMT.Text, out colorValue);
            skyData.MidTopColor.R = colorValue;
            Byte.TryParse(SkyGMT.Text, out colorValue);
            skyData.MidTopColor.G = colorValue;
            Byte.TryParse(SkyBMT.Text, out colorValue);
            skyData.MidTopColor.B = colorValue;

            skyData.MidBotColor = new TM64_Geometry.OK64Color();
            Byte.TryParse(SkyRMB.Text, out colorValue);
            skyData.MidBotColor.R = colorValue;
            Byte.TryParse(SkyGMB.Text, out colorValue);
            skyData.MidBotColor.G = colorValue;
            Byte.TryParse(SkyBMB.Text, out colorValue);
            skyData.MidBotColor.B = colorValue;

            mapData.MapColor = new TM64_Geometry.OK64Color();
            Byte.TryParse(MapRBox.Text, out colorValue);
            mapData.MapColor.R = colorValue;
            Byte.TryParse(MapGBox.Text, out colorValue);
            mapData.MapColor.G = colorValue;
            Byte.TryParse(MapBBox.Text, out colorValue);
            mapData.MapColor.B = colorValue;

        }


        private void LoadBtn_Click(object sender, EventArgs e)
        {
            Scene fbx = new Scene();
            AssimpContext importer = new AssimpContext();

            string[] ChunkList = Directory.GetFiles(okSettings.JRDirectory, "*.FBX*", SearchOption.AllDirectories);

            MemoryStream LocalStream = new MemoryStream();

            LocalStream.Write(Resources.NONE_475, 0 , Resources.NONE_475.Length);
            LocalStream.Position = 0;
            
            fbx = importer.ImportFileFromStream(LocalStream, PostProcessPreset.TargetRealTimeMaximumQuality);
            EmptyBlock.ObjectList = TarmacGeometry.createObjects(fbx);

            LocalStream = new MemoryStream();
            LocalStream.Position = 0;
            LocalStream.Write(Resources.ZONE, 0, Resources.ZONE.Length);
            LocalStream.Position = 0;

            fbx = importer.ImportFileFromStream(LocalStream, PostProcessPreset.TargetRealTimeMaximumQuality);
            ZoneBlock.ObjectList = TarmacGeometry.createObjects(fbx);


            foreach (var subObject in EmptyBlock.ObjectList)
            {
                foreach (var face in subObject.modelGeometry)
                {
                    foreach (var vert in face.VertData)
                    {
                        vert.color.R = 255;
                        vert.color.G = 255;
                        vert.color.B = 255;
                    }
                }
            }
            
            BlockObjects = new TM64_Geometry.OK64JRBlock[ChunkList.Length];
            for (int currentFile = 0; currentFile < ChunkList.Length; currentFile++)
            {
                
                fbx = new Scene();
                importer = new AssimpContext();
                BlockObjects[currentFile] = new TM64_Geometry.OK64JRBlock();
                string ParentFolder = Directory.GetParent(ChunkList[currentFile]).Name;
                BlockObjects[currentFile].BlockName = ParentFolder +"-"+ Path.GetFileNameWithoutExtension(ChunkList[currentFile]);
                fbx = importer.ImportFile(ChunkList[currentFile], PostProcessPreset.TargetRealTimeMaximumQuality);
                BlockObjects[currentFile].ObjectList = TarmacGeometry.createObjects(fbx);

                ObjectSelectBox.Items.Add(BlockObjects[currentFile].BlockName);
            }
            loaded = true;
            MessageBox.Show("Finished");
            openGLControl.Visible = true;
            openGLControl.Enabled = true;
        }


        private void loadBlock(string filePath)
        {
            MessageBox.Show("Select OK64.POP File");
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                popFile = fileOpen.FileName;
                if (levelFormat == 0)
                {
                    pathGroups = tm64Path.loadPOP(popFile, surfaceObjects);
                }
                else
                {
                    pathGroups = tm64Path.loadBattlePOP(popFile);
                }
            }
        }


        private void Matbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void tBox_CheckedChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureTransparent = tBox.Checked;
        }




        
        private void DrawMarker(TM64_Geometry.Face[] subFace, TM64_Paths.Marker pathMarker, float[] colorArray)
        {
            foreach (var face in subFace)
            {
                if (colorArray.Length > 3)
                {
                    foreach (var subVert in face.VertData)
                    {
                        gl.Color(colorArray[0], colorArray[1], colorArray[2], colorArray[3]);
                        gl.Vertex(subVert.position.x + pathMarker.xval, subVert.position.y + pathMarker.yval, subVert.position.z + pathMarker.zval);
                    }
                }
                else
                {
                    foreach (var subVert in face.VertData)
                    {
                        gl.Color(colorArray[0], colorArray[1], colorArray[2], 1.0f);
                        gl.Vertex(subVert.position.x + pathMarker.xval, subVert.position.y + pathMarker.yval, subVert.position.z + pathMarker.zval);
                    }
                }
            }
        }

        private void DrawTree(TM64_Geometry.Face[] subFace, TM64_Paths.Marker pathMarker)
        {
            foreach (var face in subFace)
            {
                    foreach (var subVert in face.VertData)
                    {
                        gl.Color(1.0f,1.0f,1.0f,1.0f);
                        gl.TexCoord(subVert.position.u, subVert.position.v);
                        gl.Vertex(subVert.position.x + pathMarker.xval, subVert.position.y + pathMarker.yval, subVert.position.z + pathMarker.zval);
                    }
            }
        }


        public float[] GetAlphaFlash(float[] flashColor)
        {
            if (flashColor[4] == 0.0f)
            {
                if (flashColor[3] < 1.0f)
                {
                    flashColor[3] = Convert.ToSingle(flashColor[3] + 0.1);
                }
                else
                {
                    flashColor[4] = 1.0f;
                }
            }
            else
            {
                if (flashColor[3] > 0.0)
                {
                    flashColor[3] = Convert.ToSingle(flashColor[3] - 0.1);
                }
                else
                {
                    flashColor[4] = 0.0f;
                }
            }


            if (flashColor[3] > 1.0f)
            {
                flashColor[3] = 1.0f;
                flashColor[4] = 1.0f;
            }
            if (flashColor[3] < 0.0f)
            {
                flashColor[3] = 0.0f;
                flashColor[4] = 0.0f;
            }



            float[] outputColor = { flashColor[0], flashColor[1], flashColor[2], flashColor[3] };
            return outputColor;
        }

        public float[] GetYellowFlash(float[] flashColor)
        {
            if (flashColor[4] == 0.0f)
            {
                if (flashColor[2] < 1.0f)
                {
                    flashColor[2] = Convert.ToSingle(flashColor[2] + 0.05);
                }
                else
                {
                    flashColor[4] = 1.0f;
                }
            }
            else
            {
                if (flashColor[2] > -1.0)
                {
                    flashColor[2] = Convert.ToSingle(flashColor[2] - 0.05);
                }
                else
                {
                    flashColor[4] = 0.0f;
                }
            }

            float[] outputColor = { flashColor[0], flashColor[1], flashColor[2], flashColor[3] };
            return outputColor;
        }


        private int CheckZone(int x, int y, int z)
        {
            for (int currentObject = 0; currentObject < CourseObjects.Count; currentObject++)
            {
                if (CourseObjects[currentObject].XIndex == x)
                {
                    if (CourseObjects[currentObject].YIndex == y)
                    {
                        if (CourseObjects[currentObject].ZIndex == z)
                        {
                            return currentObject;
                        }
                    }
                }
            }
            return -1;
        }

        private void drawObjects()
        {

            
            List<string> drawnObjects = new List<string>();
            //draw faces

            for (int xIndex = -10; xIndex < 10; xIndex++)
            {
                for (int yIndex = -10; yIndex < 10; yIndex++)
                {
                    for (int zIndex = -4; zIndex < 4; zIndex++)
                    {
                        int BlockZone = CheckZone(xIndex, yIndex, zIndex);
                        int[] ZoneIndex = new int[] { xIndex, yIndex, zIndex };
                        
                        TM64_Geometry.OK64F3DObject[] ThisBlock = new TM64_Geometry.OK64F3DObject[0];
                        
                        if (BlockZone > 0)
                        {

                            ThisBlock = BlockObjects[CourseObjects[BlockZone].BlockID].ObjectList;                            
                        }
                        else
                        {
                            ThisBlock = EmptyBlock.ObjectList;
                        }
                        
                        if (ZoneIndex[0] == highlightedObject[0] & ZoneIndex[1] == highlightedObject[1] & ZoneIndex[2] == highlightedObject[2])
                        {
                            int x = 0;
                            if (chkHover.Checked)
                            {
                                foreach (var thisObject in ThisBlock)
                                {
                                    gl = TarmacGL.DrawTarget(gl, localCamera, glTexture, thisObject, ZoneIndex);
                                }
                            }
                            else
                            {
                                if (CheckboxTextured.Checked)
                                {
                                    foreach (var thisObject in ThisBlock)
                                    {
                                        gl = TarmacGL.DrawTextured(gl, textureArray, localCamera, glTexture, thisObject, ZoneIndex);
                                    }
                                }
                                else
                                {
                                    foreach (var thisObject in ThisBlock)
                                    {
                                        gl = TarmacGL.DrawShaded(gl, textureArray, localCamera, glTexture, thisObject, ZoneIndex);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (CheckboxTextured.Checked)
                            {
                                foreach (var thisObject in ThisBlock)
                                {
                                    gl = TarmacGL.DrawTextured(gl, textureArray, localCamera, glTexture, thisObject, ZoneIndex);
                                }
                            }
                            else
                            {
                                foreach (var thisObject in ThisBlock)
                                {
                                    gl = TarmacGL.DrawShaded(gl, textureArray, localCamera, glTexture, thisObject, ZoneIndex);
                                }
                            }
                        }
                        

                    }
                }
            }


            /*

            if (levelFormat == 0)
            {
                if (chkPop.Checked)
                {
                    foreach (var list in pathGroups[0].pathList)
                    {
                        float[] surfaceYellow = GetYellowFlash(flashYellow);
                        foreach (var marker in list.pathmarker)
                        {
                            DrawMarker(markerGeometry, marker, surfaceYellow);
                        }
                    }
                    foreach (var list in pathGroups[1].pathList)
                    {
                        float[] surfaceYellow = GetYellowFlash(flashYellow);
                        foreach (var marker in list.pathmarker)
                        {
                            DrawMarker(itemGeometry, marker, surfaceYellow);
                        }
                    }
                    foreach (var list in pathGroups[2].pathList)
                    {

                                gl.End();
                        glTexture.Destroy(gl);
                        gl.Enable(OpenGL.GL_TEXTURE_2D);

                        glTexture.Create(gl, Resources.Tree1);
                        glTexture.Bind(gl);
                        gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                        gl.Begin(OpenGL.GL_TRIANGLES);
                        foreach (var marker in list.pathmarker)
                        {
                            DrawTree(treeGeometry, marker);
                        }
                        gl.End();



                    }
                    foreach (var list in pathGroups[3].pathList)
                    {
                        float[] surfaceYellow = GetYellowFlash(flashYellow);
                        foreach (var marker in list.pathmarker)
                        {
                            DrawMarker(piranhaGeometry, marker, surfaceYellow);
                        }
                    }
                }
            }
            */
            loadGL = true;
            
        }



        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            localCamera.flashWhite = GetAlphaFlash(flashWhite);
            localCamera.flashRed = GetAlphaFlash(flashRed);
            localCamera.flashYellow = GetAlphaFlash(flashYellow);

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            if (loaded)
            {


                TarmacGL.DrawNorth(gl, glTexture, localCamera);
                drawObjects();



                gl.End();
                gl.Flush();
            }
            openGLControl_Resized(null, null);
        }

        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  Get the OpenGL object.

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            gl.Perspective(90.0f, (double)Width / (double)Height, 0.01, 50000.0);

            //  Use the 'look at' helper function to position and aim the camera.
            gl.LookAt(localCamera.position.X, localCamera.position.Y, localCamera.position.Z, localCamera.target.X, localCamera.target.Y, localCamera.target.Z, 0, 0, 1);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }



        private void updateTarget()
        {
            float hAngle = Convert.ToSingle(localCamera.rotation * (Math.PI / 180));
            float[] localCoord = new float[3];
            localCoord[0] = Convert.ToSingle(localCamera.position.X + 50 * Math.Cos(hAngle));
            localCoord[1] = Convert.ToSingle(localCamera.position.Y + 50 * Math.Sin(hAngle));
            localCoord[2] = localCamera.position.Z;
            localCamera.target = new Vector3D(localCoord[0], localCoord[1], localCoord[2]);
        }
        private void moveCamera(int direction)
        {
            switch (direction)
            {
                case 0:
                    {
                        //forward
                        float hAngle = Convert.ToSingle(localCamera.rotation * (Math.PI / 180));
                        float[] localCoord = new float[3];
                        localCoord[0] = Convert.ToSingle(localCamera.position.X + moveDistance * Math.Cos(hAngle));
                        localCoord[1] = Convert.ToSingle(localCamera.position.Y + moveDistance * Math.Sin(hAngle));
                        localCoord[2] = localCamera.position.Z;
                        localCamera.position = new Vector3D(localCoord[0], localCoord[1], localCoord[2]);
                        updateTarget();

                        break;
                    }
                case 1:
                    {
                        //back
                        float hAngle = Convert.ToSingle(localCamera.rotation * (Math.PI / 180));
                        float[] localCoord = new float[3];
                        localCoord[0] = Convert.ToSingle(localCamera.position.X - moveDistance * Math.Cos(hAngle));
                        localCoord[1] = Convert.ToSingle(localCamera.position.Y - moveDistance * Math.Sin(hAngle));
                        localCoord[2] = localCamera.position.Z;
                        localCamera.position = new Vector3D(localCoord[0], localCoord[1], localCoord[2]);
                        updateTarget();

                        break;
                    }
                case 2:
                    {
                        //up
                        float hAngle = Convert.ToSingle(localCamera.rotation * (Math.PI / 180));
                        float[] localCoord = new float[3];
                        localCoord[0] = localCamera.position.X;
                        localCoord[1] = localCamera.position.Y;
                        localCoord[2] = localCamera.position.Z + moveDistance;
                        localCamera.position = new Vector3D(localCoord[0], localCoord[1], localCoord[2]);
                        updateTarget();

                        break;
                    }
                case 3:
                    {
                        //down
                        float hAngle = Convert.ToSingle(localCamera.rotation * (Math.PI / 180));
                        float[] localCoord = new float[3];
                        localCoord[0] = localCamera.position.X;
                        localCoord[1] = localCamera.position.Y;
                        localCoord[2] = localCamera.position.Z - moveDistance;
                        localCamera.position = new Vector3D(localCoord[0], localCoord[1], localCoord[2]);
                        updateTarget();

                        break;
                    }
                case 4:
                    {
                        //strafe
                        float strafeAngle = Convert.ToSingle(localCamera.rotation - 90);
                        if (strafeAngle < 0)
                            strafeAngle += 360;
                        float hAngle = Convert.ToSingle(strafeAngle * (Math.PI / 180));
                        float[] localCoord = new float[3];
                        localCoord[0] = Convert.ToSingle(localCamera.position.X + moveDistance * Math.Cos(hAngle));
                        localCoord[1] = Convert.ToSingle(localCamera.position.Y + moveDistance * Math.Sin(hAngle));
                        localCoord[2] = localCamera.position.Z;
                        localCamera.position = new Vector3D(localCoord[0], localCoord[1], localCoord[2]);
                        updateTarget();

                        break;
                    }
                case 5:
                    {
                        //strafe
                        float strafeAngle = Convert.ToSingle(localCamera.rotation + 90);
                        if (strafeAngle > 360)
                            strafeAngle -= 360;
                        float hAngle = Convert.ToSingle(strafeAngle * (Math.PI / 180));
                        float[] localCoord = new float[3];
                        localCoord[0] = Convert.ToSingle(localCamera.position.X + moveDistance * Math.Cos(hAngle));
                        localCoord[1] = Convert.ToSingle(localCamera.position.Y + moveDistance * Math.Sin(hAngle));
                        localCoord[2] = localCamera.position.Z;
                        localCamera.position = new Vector3D(localCoord[0], localCoord[1], localCoord[2]);
                        updateTarget();

                        break;
                    }

            }

        }
        private void openGLControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (loadGL)
            {

                if (Keyboard.IsKeyDown(Key.W))
                {
                    moveCamera(0);
                }
                if (Keyboard.IsKeyDown(Key.S))
                {
                    moveCamera(1);
                }
                if (Keyboard.IsKeyDown(Key.A))
                {
                    localCamera.rotation += (moveDistance / 5);
                    if (localCamera.rotation < 0)
                        localCamera.rotation += 360;
                    if (localCamera.rotation > 360)
                        localCamera.rotation -= 360;
                    updateTarget();

                }
                if (Keyboard.IsKeyDown(Key.D))
                {

                    localCamera.rotation -= (moveDistance / 5);
                    if (localCamera.rotation < 0)
                        localCamera.rotation += 360;
                    if (localCamera.rotation > 360)
                        localCamera.rotation -= 360;
                    updateTarget();
                }
                if (Keyboard.IsKeyDown(Key.Q))
                {
                    moveCamera(5);
                }
                if (Keyboard.IsKeyDown(Key.E))
                {
                    moveCamera(4);
                }
                if (Keyboard.IsKeyDown(Key.R))
                {
                    moveCamera(2);
                }
                if (Keyboard.IsKeyDown(Key.F))
                {
                    moveCamera(3);
                }
                if (Keyboard.IsKeyDown(Key.T))
                {
                    moveDistance += 5;
                }
                if (Keyboard.IsKeyDown(Key.G))
                {
                    moveDistance -= 5;
                }

                cxBox.Text = localCamera.position.X.ToString();
                cyBox.Text = localCamera.position.Y.ToString();
                czBox.Text = localCamera.position.Z.ToString();
                csBox.Text = moveDistance.ToString();


                openGLControl_Resized(null, null);
            }
        }

        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {

            //  Get the OpenGL object.

            //  Set the clear color.
            gl.ClearColor(.0f, .0f, .0f, 0);

        }

        private void OpenGLControl_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Tarmac64.OKRetail tarmacAbout = new Tarmac64.OKRetail();
            tarmacAbout.Show();

        }

        //

        //

        //

        //




        private void openGLControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
            System.Windows.Point mouseClick = new System.Windows.Point(e.Location.X, e.Location.Y);

            double[] pointA = gl.UnProject(e.Location.X, e.Location.Y, 0);
            double[] pointB = gl.UnProject(e.Location.X, e.Location.Y, 1);

            Vector3D rayOrigin = new Vector3D(Convert.ToSingle(pointA[0]), Convert.ToSingle(pointA[1]), Convert.ToSingle(pointA[2]));
            Vector3D rayTarget = new Vector3D(Convert.ToSingle(pointB[0]), Convert.ToSingle(pointB[1]), Convert.ToSingle(pointB[2] * -1));


            float objectDistance = -1;
            TM64_Geometry tmGeo = new TM64_Geometry();
            int[] objectID = new int[] { -50, -50, -50 };
            cName.Text = "";
            for(int Xindex = -10; Xindex < 10; Xindex++)
            {
                for (int Yindex = -10; Yindex < 10; Yindex++)
                {
                    for (int Zindex = -4; Zindex < 4; Zindex++)
                    {
                        int[] ZoneIndex = new int[] { Xindex, Yindex, Zindex };
                        foreach (var subObject in ZoneBlock.ObjectList)
                        {
                            foreach (var face in subObject.modelGeometry)
                            {
                                Vector3D intersectPoint = tmGeo.testIntersect(rayOrigin, rayTarget, face.VertData[0], face.VertData[1], face.VertData[2], ZoneIndex);
                                if (intersectPoint.X > 0)
                                {
                                    if (objectDistance > intersectPoint.X | objectDistance == -1)
                                    {
                                        objectDistance = intersectPoint.X;
                                        objectID = new int[] { Xindex, Yindex, Zindex };
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
            
            switch (tabControl1.SelectedIndex)
            {
                default:
                    {
                        break;
                    }
                case 2:
                    {
                        if (objectID[0] != -50)
                        {
                            highlightedObject = objectID;
                            ChunkXBox.SelectedIndex = highlightedObject[0] + 20;
                            ChunkYBox.SelectedIndex = highlightedObject[1] + 20;
                            ChunkZBox.SelectedIndex = highlightedObject[2] + 4;

                        }
                        break;
                    }
            }
            
        }


        private void openGLControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Point mouseClick = new System.Windows.Point(e.Location.X, e.Location.Y);

            double[] pointA = gl.UnProject(e.Location.X, e.Location.Y, 0);
            double[] pointB = gl.UnProject(e.Location.X, e.Location.Y, 1);

            Vector3D rayOrigin = new Vector3D(Convert.ToSingle(pointA[0]), Convert.ToSingle(pointA[1]), Convert.ToSingle(pointA[2]));
            Vector3D rayTarget = new Vector3D(Convert.ToSingle(pointB[0]), Convert.ToSingle(pointB[1]), Convert.ToSingle(pointB[2] * -1));


            float objectDistance = -1;
            TM64_Geometry tmGeo = new TM64_Geometry();
            int[] objectID = new int[3];
            cName.Text = "";
            for (int Xindex = -10; Xindex < 10; Xindex++)
            {
                for (int Yindex = -10; Yindex < 10; Yindex++)
                {
                    for (int Zindex = -4; Zindex < 4; Zindex++)
                    {
                        int[] ZoneIndex = new int[] { Xindex, Yindex, Zindex };
                        foreach (var subObject in ZoneBlock.ObjectList)
                        {
                            foreach (var face in subObject.modelGeometry)
                            {
                                Vector3D intersectPoint = tmGeo.testIntersect(rayOrigin, rayTarget, face.VertData[0], face.VertData[1], face.VertData[2], ZoneIndex);
                                if (intersectPoint.X > 0)
                                {
                                    if (objectDistance > intersectPoint.X | objectDistance == -1)
                                    {
                                        objectDistance = intersectPoint.X;
                                        objectID = new int[] { Xindex, Yindex, Zindex };
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
            switch (tabControl1.SelectedIndex)
            {
                
                case 2:
                    {

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }


        private void masterBox_AfterCheck(object sender, TreeViewEventArgs e)
        {
            
        }


        private void ColorPickT_Click(object sender, EventArgs e)
        {
            ColorDialog ColorPick = new ColorDialog();
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                SkyRT.Text = ColorPick.Color.R.ToString("X");
                SkyGT.Text = ColorPick.Color.G.ToString("X");
                SkyBT.Text = ColorPick.Color.B.ToString("X");
            }
            ColorUpdate();
        }

        private void ColorPickMT_Click(object sender, EventArgs e)
        {
            ColorDialog ColorPick = new ColorDialog();
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                SkyRMT.Text = ColorPick.Color.R.ToString("X");
                SkyGMT.Text = ColorPick.Color.G.ToString("X");
                SkyBMT.Text = ColorPick.Color.B.ToString("X");
            }
            ColorUpdate();
        }

        private void SkyGT_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyRT_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyBT_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyRMT_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyBMT_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyGMT_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyBMB_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyBB_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyGB_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyGMB_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyRMB_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyRB_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void CourseMaker_Load(object sender, EventArgs e)
        {
            FormLoad(sender, e);
        }

        private void ColorPickMB_Click(object sender, EventArgs e)
        {
            ColorDialog ColorPick = new ColorDialog();
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                SkyRMB.Text = ColorPick.Color.R.ToString();
                SkyGMB.Text = ColorPick.Color.G.ToString();
                SkyBMB.Text = ColorPick.Color.B.ToString();
            }
            ColorUpdate();
        }

        private void ColorPickB_Click(object sender, EventArgs e)
        {
            ColorDialog ColorPick = new ColorDialog();
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                SkyRB.Text = ColorPick.Color.R.ToString();
                SkyGB.Text = ColorPick.Color.G.ToString();
                SkyBB.Text = ColorPick.Color.B.ToString();
            }
            ColorUpdate();
        }

        private void ColorPickMap_Click(object sender, EventArgs e)
        {
            ColorDialog ColorPick = new ColorDialog();
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                MapRBox.Text = ColorPick.Color.R.ToString();
                MapGBox.Text = ColorPick.Color.G.ToString();
                MapBBox.Text = ColorPick.Color.B.ToString();
            }
            ColorUpdate();
        }

        private void previewBtn_Click_1(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                previewBox.Text = fileOpen.FileName;
            }
        }

        private void bannerBtn_Click_1(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                bannerBox.Text = fileOpen.FileName;
            }
        }

        private void ghostBtn_Click_1(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                ghostBox.Text = fileOpen.FileName;
            }
        }

        private void asmBtn_Click_1(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                asmBox.Text = fileOpen.FileName;
            }
        }

        private void mapBtn_Click_1(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                mapBox.Text = fileOpen.FileName;
            }
        }

        private void MapG_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void MapB_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void MapR_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void CourseMaker_Resize(object sender, EventArgs e)
        {
            /*
            if (this.Size.Height > 720)
            {

                controlGroup.Location = new System.Drawing.Point(controlGroup.Location.X, 186 + ((this.Size.Height - 720) / 2));
                viewGroup.Location = new System.Drawing.Point(viewGroup.Location.X, 504 + (this.Size.Height - 720));
                ChunkBox.Size = new System.Drawing.Size(ChunkBox.Width, 176 + ((this.Size.Height - 720) / 2));

                controlGroup.Size = new System.Drawing.Size(controlGroup.Width, 312 + ((this.Size.Height - 720) / 2));
                SubobjectBox.Size = new System.Drawing.Size(SubobjectBox.Width, 108 + ((this.Size.Height - 720) / 2));
                TextureEditBox.Location = new System.Drawing.Point(TextureEditBox.Location.X, 283 + ((this.Size.Height - 720) / 2));
                TextureEditLabel.Location = new System.Drawing.Point(TextureEditLabel.Location.X, 286 + ((this.Size.Height - 720) / 2));

            }
            else
            {
                controlGroup.Location= new System.Drawing.Point(controlGroup.Location.X, 186);
                viewGroup.Location = new System.Drawing.Point(viewGroup.Location.X, 504);
                ChunkBox.Size = new System.Drawing.Size(ChunkBox.Width, 176);

                controlGroup.Size = new System.Drawing.Size(controlGroup.Width, 312);
                SubobjectBox.Size = new System.Drawing.Size(SubobjectBox.Width, 108);
                TextureEditBox.Location = new System.Drawing.Point(TextureEditBox.Location.X, 283);
                TextureEditLabel.Location = new System.Drawing.Point(TextureEditLabel.Location.X, 286);
            }
            */
        }

        private void CheckZoneBox()
        {
            int checkValue = CheckZone(ChunkXBox.SelectedIndex - 20, ChunkYBox.SelectedIndex - 20, ChunkZBox.SelectedIndex - 20);

            if (checkValue > 0)
            {
                //ChunkBox.SelectedNode. = checkValue;
            }
            
        }
        private void ChunkXBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckZoneBox();
        }
    }
}
