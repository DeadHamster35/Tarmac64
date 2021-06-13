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
using System.Text.RegularExpressions;
using Tarmac64_Library.Properties;
using SharpGL;
using SharpGL.SceneGraph.Core;
using System.Drawing.Design;
using System.Windows.Input;
using System.Drawing.Imaging;
using Cereal64.Microcodes.F3DEX.DataElements;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Win32;

namespace Tarmac64_Library
{


    public partial class CourseCompiler : Form
    {

        TM64 Tarmac = new TM64();
        TM64_Course TarmacCourse = new TM64_Course();
        TM64_GL TarmacGL = new TM64_GL();
        CommonOpenFileDialog fileOpen = new CommonOpenFileDialog();

        TM64.OK64Settings okSettings = new TM64.OK64Settings();

        bool updateBool = false;

        int raycastBoolean = 0;
        int sectionCount = 0;
        public int programFormat;
        int levelFormat = 0;



        public CourseCompiler()
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

            CreateGeometry();
            CreateColors();
            okSettings = Tarmac.LoadSettings();


            gl = openGLControl.OpenGL;
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Tarmac64 Designer")
            {
                tabControl1.TabPages.Remove(tabControl1.TabPages[4]);
            }
            courseBox.SelectedIndex = 0;
            cupBox.SelectedIndex = 0;
            setBox.SelectedIndex = 0;

            SkyRM.Text = "216";
            SkyGM.Text = "232";
            SkyBM.Text = "248";

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
        int highlightedObject = -1;


        TM64_Geometry TarmacGeometry = new TM64_Geometry();
        TM64_Paths tm64Path = new TM64_Paths();
        TM64 tm64 = new TM64();

        TM64_Geometry.OK64SectionList[] sectionList = new TM64_Geometry.OK64SectionList[0];
        TM64_Geometry.OK64SectionList[] surfaceList = new TM64_Geometry.OK64SectionList[0];

        TM64_Geometry.Face[] markerGeometry = new TM64_Geometry.Face[2];
        TM64_Geometry.Face[] treeGeometry = new TM64_Geometry.Face[4];
        TM64_Geometry.Face[] piranhaGeometry = new TM64_Geometry.Face[4];
        TM64_Geometry.Face[] redcoinGeometry = new TM64_Geometry.Face[4];
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

        TM64_Geometry.OK64F3DObject[] masterObjects = new TM64_Geometry.OK64F3DObject[0];
        TM64_Geometry.OK64F3DGroup[] masterGroups = new TM64_Geometry.OK64F3DGroup[0];
        int moveDistance = 50;


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


            redcoinGeometry[0] = new TM64_Geometry.Face();

            redcoinGeometry[0].VertData = new TM64_Geometry.Vertex[3];

            redcoinGeometry[0].VertData[0] = new TM64_Geometry.Vertex();
            redcoinGeometry[0].VertData[0].position = new TM64_Geometry.Position();
            redcoinGeometry[0].VertData[0].position.x = Convert.ToInt16(-5);
            redcoinGeometry[0].VertData[0].position.y = Convert.ToInt16(0);
            redcoinGeometry[0].VertData[0].position.z = Convert.ToInt16(0);

            redcoinGeometry[0].VertData[1] = new TM64_Geometry.Vertex();
            redcoinGeometry[0].VertData[1].position = new TM64_Geometry.Position();
            redcoinGeometry[0].VertData[1].position.x = Convert.ToInt16(5);
            redcoinGeometry[0].VertData[1].position.y = Convert.ToInt16(0);
            redcoinGeometry[0].VertData[1].position.z = Convert.ToInt16(0);

            redcoinGeometry[0].VertData[2] = new TM64_Geometry.Vertex();
            redcoinGeometry[0].VertData[2].position = new TM64_Geometry.Position();
            redcoinGeometry[0].VertData[2].position.x = Convert.ToInt16(5);
            redcoinGeometry[0].VertData[2].position.y = Convert.ToInt16(0);
            redcoinGeometry[0].VertData[2].position.z = Convert.ToInt16(10);


            redcoinGeometry[1] = new TM64_Geometry.Face();
            redcoinGeometry[1].VertData = new TM64_Geometry.Vertex[3];

            redcoinGeometry[1].VertData[0] = new TM64_Geometry.Vertex();
            redcoinGeometry[1].VertData[0].position = new TM64_Geometry.Position();
            redcoinGeometry[1].VertData[0].position.x = Convert.ToInt16(0);
            redcoinGeometry[1].VertData[0].position.y = Convert.ToInt16(-5);
            redcoinGeometry[1].VertData[0].position.z = Convert.ToInt16(0);

            redcoinGeometry[1].VertData[1] = new TM64_Geometry.Vertex();
            redcoinGeometry[1].VertData[1].position = new TM64_Geometry.Position();
            redcoinGeometry[1].VertData[1].position.x = Convert.ToInt16(0);
            redcoinGeometry[1].VertData[1].position.y = Convert.ToInt16(5);
            redcoinGeometry[1].VertData[1].position.z = Convert.ToInt16(0);

            redcoinGeometry[1].VertData[2] = new TM64_Geometry.Vertex();
            redcoinGeometry[1].VertData[2].position = new TM64_Geometry.Position();
            redcoinGeometry[1].VertData[2].position.x = Convert.ToInt16(0);
            redcoinGeometry[1].VertData[2].position.y = Convert.ToInt16(5);
            redcoinGeometry[1].VertData[2].position.z = Convert.ToInt16(10);

            redcoinGeometry[2] = new TM64_Geometry.Face();
            redcoinGeometry[2].VertData = new TM64_Geometry.Vertex[3];

            redcoinGeometry[2].VertData[0] = new TM64_Geometry.Vertex();
            redcoinGeometry[2].VertData[0].position = new TM64_Geometry.Position();
            redcoinGeometry[2].VertData[0].position.x = Convert.ToInt16(-5);
            redcoinGeometry[2].VertData[0].position.y = Convert.ToInt16(0);
            redcoinGeometry[2].VertData[0].position.z = Convert.ToInt16(0);

            redcoinGeometry[2].VertData[1] = new TM64_Geometry.Vertex();
            redcoinGeometry[2].VertData[1].position = new TM64_Geometry.Position();
            redcoinGeometry[2].VertData[1].position.x = Convert.ToInt16(5);
            redcoinGeometry[2].VertData[1].position.y = Convert.ToInt16(0);
            redcoinGeometry[2].VertData[1].position.z = Convert.ToInt16(10);

            redcoinGeometry[2].VertData[2] = new TM64_Geometry.Vertex();
            redcoinGeometry[2].VertData[2].position = new TM64_Geometry.Position();
            redcoinGeometry[2].VertData[2].position.x = Convert.ToInt16(-5);
            redcoinGeometry[2].VertData[2].position.y = Convert.ToInt16(0);
            redcoinGeometry[2].VertData[2].position.z = Convert.ToInt16(10);


            redcoinGeometry[3] = new TM64_Geometry.Face();
            redcoinGeometry[3].VertData = new TM64_Geometry.Vertex[3];

            redcoinGeometry[3].VertData[0] = new TM64_Geometry.Vertex();
            redcoinGeometry[3].VertData[0].position = new TM64_Geometry.Position();
            redcoinGeometry[3].VertData[0].position.x = Convert.ToInt16(0);
            redcoinGeometry[3].VertData[0].position.y = Convert.ToInt16(-5);
            redcoinGeometry[3].VertData[0].position.z = Convert.ToInt16(0);

            redcoinGeometry[3].VertData[1] = new TM64_Geometry.Vertex();
            redcoinGeometry[3].VertData[1].position = new TM64_Geometry.Position();
            redcoinGeometry[3].VertData[1].position.x = Convert.ToInt16(0);
            redcoinGeometry[3].VertData[1].position.y = Convert.ToInt16(5);
            redcoinGeometry[3].VertData[1].position.z = Convert.ToInt16(10);

            redcoinGeometry[3].VertData[2] = new TM64_Geometry.Vertex();
            redcoinGeometry[3].VertData[2].position = new TM64_Geometry.Position();
            redcoinGeometry[3].VertData[2].position.x = Convert.ToInt16(0);
            redcoinGeometry[3].VertData[2].position.y = Convert.ToInt16(-5);
            redcoinGeometry[3].VertData[2].position.z = Convert.ToInt16(10);

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
            int.TryParse(SkyRM.Text, out colorInt[0]);
            colorInt[1] = 0;
            int.TryParse(SkyGM.Text, out colorInt[1]);
            colorInt[2] = 0;
            int.TryParse(SkyBM.Text, out colorInt[2]);
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

            skyData.MidColor = new TM64_Geometry.OK64Color();
            Byte.TryParse(SkyRM.Text, out colorValue);
            skyData.MidColor.R = colorValue;
            Byte.TryParse(SkyGM.Text, out colorValue);
            skyData.MidColor.G = colorValue;
            Byte.TryParse(SkyBM.Text, out colorValue);
            skyData.MidColor.B = colorValue;

            skyData.MidColor = new TM64_Geometry.OK64Color();
            Byte.TryParse(SkyRB.Text, out colorValue);
            skyData.MidColor.R = colorValue;
            Byte.TryParse(SkyGB.Text, out colorValue);
            skyData.MidColor.G = colorValue;
            Byte.TryParse(SkyBB.Text, out colorValue);
            skyData.MidColor.B = colorValue;


            mapData.MapColor = new TM64_Geometry.OK64Color();
            Byte.TryParse(MapRBox.Text, out colorValue);
            mapData.MapColor.R = colorValue;
            Byte.TryParse(MapGBox.Text, out colorValue);
            mapData.MapColor.G = colorValue;
            Byte.TryParse(MapBBox.Text, out colorValue);
            mapData.MapColor.B = colorValue;

        }





        private void CompileModel()
        {

            int cID = (cupBox.SelectedIndex * 4) + courseBox.SelectedIndex;
            int setID = setBox.SelectedIndex;

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();

                string outputDirectory = okSettings.CurrentDirectory + "\\out";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
  
                if (File.Exists(okSettings.AppDirectory+"\\overkart.z64"))
                {
                
                    List<byte[]> Segments = new List<byte[]>();
                    byte[] rom = File.ReadAllBytes(okSettings.AppDirectory + "\\overkart.z64");


                    byte[] segment4 = new byte[0];
                    byte[] segment6 = new byte[0];
                    byte[] segment7 = new byte[0];
                    byte[] segment9 = new byte[0];
                    byte[] popList = new byte[0];
                    byte[] collisionList = new byte[0];
                    byte[] renderList = new byte[0];


                    byte[] popData = Resources.popResources;

                    byte[] surfaceTable = new byte[0];
                    byte[] displayTable = new byte[0];

                    int magic = 0;

                    int vertMagic = 0;

                    // Game speed multiplier. Default is 2
                    gameSpeed = new int[4];

                    int.TryParse(sp1Box.Text, out gameSpeed[0]);
                    int.TryParse(sp2Box.Text, out gameSpeed[1]);
                    int.TryParse(sp3Box.Text, out gameSpeed[2]);
                    int.TryParse(sp4Box.Text, out gameSpeed[3]);


                    //Course Music

                    byte songID = Convert.ToByte(songBox.SelectedIndex);


                    // This command writes all the bitmaps to the end of the ROM

                    rom = TarmacGeometry.writeTextures(rom, textureArray);
                    segment9 = TarmacGeometry.compiletextureTable(textureArray);


                    //build segment 7 out of the main course objects and surface geometry
                    //build segment 4 out of the same objects.

                    TM64_Geometry.OK64F3DObject[] textureObjects = masterObjects;
                    byte[] tempBytes = new byte[0];
                    if (levelFormat == 0)
                    {
                        int garbage = 0;
                        TarmacGeometry.compileTextureObject(ref garbage, ref segment7, segment7, textureArray, vertMagic);
                        TarmacGeometry.compileF3DObject(ref vertMagic, ref segment4, ref segment7, segment4, segment7, masterObjects, textureArray, vertMagic);
                        TarmacGeometry.compileF3DObject(ref vertMagic, ref segment4, ref segment7, segment4, segment7, surfaceObjects, textureArray, vertMagic);


                        // build various segment data
                        

                        renderList = TarmacGeometry.compileF3DList(ref sectionList, fbx, masterObjects, sectionList, textureArray);


                        popList = tm64Path.popMarkers(popFile);


                        surfaceTable = TarmacGeometry.compilesurfaceTable(surfaceObjects);

                        magic = (8 + 8040 + 952 + 8 + 528 + (surfaceObjects.Length * 8));
                        // 8 bytes for header
                        // 8040 bytes for the POP data
                        // 952 bytes for the POP resources
                        // 8 bytes for Surface Table Footer
                        // 528 bytes for the Display Table itself.
                        // The surface table is 8 bytes per object.
                        // We tracked the number of surface meshes while loading into surfaceObjects.
                        // magic is the size of data written before the display lists.
                        // it's needed to properly calculate the offsets.
                        // We're calculating hardcoded offsets before writing them.
                        // So we need to use magic to do it.

                        // Build the display table with the above magic value

                        displayTable = TarmacGeometry.compilesectionviewTable(sectionList, magic);





                        bs = new MemoryStream();
                        br = new BinaryReader(bs);
                        bw = new BinaryWriter(bs);
                        byte[] byteArray = new byte[0];

                        byteArray = BitConverter.GetBytes(0xB8000000);
                        Array.Reverse(byteArray);
                        bw.Write(byteArray);
                        byteArray = BitConverter.GetBytes(0x00000000);
                        Array.Reverse(byteArray);
                        bw.Write(byteArray);

                        bw.Write(popList);
                        bw.Write(popData);
                        bw.Write(displayTable);
                        bw.Write(surfaceTable);
                        bw.Write(renderList);

                        segment6 = bs.ToArray();
                    }
                    else
                    {
                        int battleOffset = 0;
                        TarmacGeometry.compileBattleObject(ref battleOffset, ref vertMagic, ref segment4, ref segment7, segment4, segment7, masterObjects, textureArray, vertMagic);



                        popList = tm64Path.popBattle(popFile);





                        bs = new MemoryStream();
                        br = new BinaryReader(bs);
                        bw = new BinaryWriter(bs);
                        byte[] byteArray = new byte[0];
                        //FC121824FF33FFFF
                        byteArray = BitConverter.GetBytes(0xFC121824);
                        Array.Reverse(byteArray);
                        bw.Write(byteArray);
                        byteArray = BitConverter.GetBytes(0xFF33FFFF);
                        Array.Reverse(byteArray);
                        bw.Write(byteArray);
                        //B900031D 00552078
                        byteArray = BitConverter.GetBytes(0xB900031D);
                        Array.Reverse(byteArray);
                        bw.Write(byteArray);
                        byteArray = BitConverter.GetBytes(0x00552078);
                        Array.Reverse(byteArray);
                        bw.Write(byteArray);

                        byteArray = BitConverter.GetBytes(0x06000000);
                        Array.Reverse(byteArray);
                        bw.Write(byteArray);

                        byteArray = BitConverter.GetBytes(battleOffset | 0x07000000);
                        Array.Reverse(byteArray);
                        bw.Write(byteArray);

                        bw.Write(popList);

                        segment6 = bs.ToArray();
                    }

                    //Compress appropriate segment data

                    byte[] cseg7 = Tarmac.compress_seg7(segment7);



                    string courseName = nameBox.Text;
                    string previewImage = previewBox.Text;
                    string bannerImage = bannerBox.Text;
                    string mapImage = mapBox.Text;
                    string customASM = asmBox.Text;
                    string ghostData = ghostBox.Text;

                    


                    Int16[] mapCoords = new Int16[2];
                    Int16[] startCoords = new Int16[2];

                    Int16.TryParse(mapXBox.Text, out mapCoords[0]);
                    Int16.TryParse(mapYBox.Text, out mapCoords[1]);
                    Int16.TryParse(startXBox.Text, out startCoords[0]);
                    Int16.TryParse(startYBox.Text, out startCoords[1]);

                    

                    int[] echoValues = new int[2];

                    int.TryParse(EchoStartBox.Text, out echoValues[0]);
                    int.TryParse(EchoStopBox.Text, out echoValues[1]);


                    byte[] cseg4 = Tarmac.CompressMIO0(segment4);
                    byte[] cseg6 = Tarmac.CompressMIO0(segment6);

                    if (levelFormat == 0)
                    {
                        TM64_Course.Course courseData = new TM64_Course.Course();
                        courseData.Segment4 = segment4;
                        courseData.Segment6 = segment6;
                        courseData.Segment7 = segment7;
                        courseData.Segment9 = segment9;
                        courseData.Credits = courseName;
                        courseData.PreviewPath = previewImage;
                        courseData.BannerPath = bannerImage;
                        courseData.MapData = new TM64_Course.MiniMap();
                        courseData.MapData.MinimapPath = mapImage;
                        courseData.MapData.MapCoord = new Vector2D(mapCoords[0], mapCoords[1]);
                        courseData.MapData.StartCoord = new Vector2D(startCoords[0], startCoords[1]);
                        courseData.MapData.MapColor = mapData.MapColor;

                        courseData.MasterObjects = masterObjects;
                        courseData.SurfaceObjects = surfaceObjects;
                        courseData.TextureObjects = textureArray;
                        
                        float tempfloat = new float();
                        Single.TryParse(MapScaleBox.Text, out tempfloat);
                        courseData.MapData.MapScale = tempfloat;


                        courseData.EchoValues = echoValues;
                        courseData.AssmeblyPath = customASM;
                        courseData.GhostPath = ghostData;
                        courseData.SkyColors = skyData;
                        
                        courseData.MusicID = songID;
                        courseData.GameTempos = gameSpeed;
                        courseData.PathLength = pathGroups[0].pathList[0].pathmarker.Count;

                        Single.TryParse(waterBox.Text, out tempfloat);
                        courseData.WaterLevel = tempfloat;

                        rom = TarmacCourse.CompileOverKart(courseData,rom, cID, setID);
                    }



                    string savepath = "";

                    savepath = Path.Combine(outputDirectory, "Mario Kart 64 (U) [!].z64");
                    File.WriteAllBytes(savepath, rom);
                    savepath = Path.Combine(outputDirectory, "Segment 4.bin");
                    File.WriteAllBytes(savepath, segment4);

                    savepath = Path.Combine(outputDirectory, "Compressed Segment 4.bin");
                    File.WriteAllBytes(savepath, cseg4);

                    savepath = Path.Combine(outputDirectory, "Segment 6.bin");
                    File.WriteAllBytes(savepath, segment6);

                    savepath = Path.Combine(outputDirectory, "Compressed Segment 6.bin");
                    File.WriteAllBytes(savepath, cseg6);

                    savepath = Path.Combine(outputDirectory, "Segment 7.bin");
                    File.WriteAllBytes(savepath, segment7);

                    savepath = Path.Combine(outputDirectory, "Compressed Segment 7.bin");
                    File.WriteAllBytes(savepath, cseg7);

                    savepath = Path.Combine(outputDirectory, "Segment 9.bin");
                    File.WriteAllBytes(savepath, segment9);

                    /*
                     * Export OK64 CONFIG
                     */
                    ExportCourseInfo();

                    MessageBox.Show("Finished");
                } else
                {
                    throw new FileNotFoundException("Could not find overkart.z64 beside Tarmac executable");
                }
        }
        


        private void LoadModel()
        {
            OpenFileDialog wpfDialog = new OpenFileDialog();
            wpfDialog.InitialDirectory = okSettings.ProjectDirectory;
            wpfDialog.Title = "Select .FBX File";
            wpfDialog.FilterIndex = 2;
            wpfDialog.Filter = "fbx files (*.fbx)|*.fbx";
        if (wpfDialog.ShowDialog() ==  DialogResult.OK)
            {
                okSettings.CurrentDirectory = System.IO.Path.GetDirectoryName(wpfDialog.FileName);
                //Get the path of specified file
                FBXfilePath = wpfDialog.FileName;

                levelFormat = 0;

                if (raycastBox.Checked)
                {
                    raycastBoolean = 1;  //used to be int for resolution, using 0/1 as false/true until certain resolution not needed.
                }
                else
                {
                    raycastBoolean = 0;
                }
                int modelFormat = 0;  // 

                Scene fbx = new Scene();
                AssimpContext importer = new AssimpContext();

                fbx = importer.ImportFile(FBXfilePath, PostProcessPreset.TargetRealTimeMaximumQuality);

                materialCount = fbx.MaterialCount;
                int textureCount = 0;


                modelFormat = TarmacGeometry.GetModelFormat(fbx);
                sectionCount = TarmacGeometry.GetSectionCount(fbx);

                //
                // Textures
                //
                textureArray = TarmacGeometry.loadTextures(fbx, FBXfilePath);
                materialCount = textureArray.Length;

                //
                // Course Objects
                // Surface Map
                //

                if (levelFormat == 0)
                {
                    switch (modelFormat)
                    {
                        case 0:
                            {
                                masterObjects = TarmacGeometry.createMaster(fbx, sectionCount);
                                surfaceObjects = TarmacGeometry.loadCollision(fbx, sectionCount, modelFormat);
                                TM64_Geometry.PathfindingObject[] surfaceBoundaries = TarmacGeometry.SurfaceBounds(surfaceObjects, sectionCount);
                                sectionList = TarmacGeometry.AutomateSection(sectionCount, surfaceObjects, masterObjects, surfaceBoundaries, fbx, raycastBoolean);
                                break;
                            }
                        case 1:
                            {
                                masterObjects = TarmacGeometry.loadMaster(ref masterGroups, fbx, textureArray);
                                surfaceObjects = TarmacGeometry.loadCollision(fbx, sectionCount, modelFormat);
                                TM64_Geometry.PathfindingObject[] surfaceBoundaries = TarmacGeometry.SurfaceBounds(surfaceObjects, sectionCount);
                                sectionList = TarmacGeometry.AutomateSection(sectionCount, surfaceObjects, masterObjects, surfaceBoundaries, fbx, raycastBoolean);
                                break;
                            }
                        case 2:
                            {
                                masterObjects = TarmacGeometry.loadMaster(ref masterGroups, fbx, textureArray);
                                surfaceObjects = TarmacGeometry.loadCollision(fbx, sectionCount, modelFormat);
                                sectionList = TarmacGeometry.loadSection(fbx, sectionCount, masterObjects);
                                break;
                            }
                    }
                }
                else
                {

                    masterObjects = TarmacGeometry.loadMaster(ref masterGroups, fbx, textureArray);

                }
                masterBox.Nodes.Clear();
                List<int> listedObjects = new List<int>();



                for (int currentObject = 0; currentObject < masterObjects.Length; currentObject++)
                {
                    if (TarmacGeometry.CheckST(masterObjects[currentObject], textureArray[masterObjects[currentObject].materialID]))
                    {
                        MessageBox.Show("Fatal UV Error " + masterObjects[currentObject].objectName);
                    }
                }
                for (int currentObject = 0; currentObject < surfaceObjects.Length; currentObject++)
                {
                    if (TarmacGeometry.CheckST(surfaceObjects[currentObject], textureArray[surfaceObjects[currentObject].materialID]))
                    {
                        MessageBox.Show("Fatal UV Error " + surfaceObjects[currentObject].objectName);
                    }
                }

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
                    surfaceobjectBox.Items.Clear();
                    for (int currentIndex = 0; currentIndex < surfaceObjects.Length; currentIndex++)
                    {
                        surfaceobjectBox.Items.Add(surfaceObjects[currentIndex].objectName);
                    }
                    sectionBox.Items.Clear();
                    for (int currentSection = 0; currentSection < sectionCount; currentSection++)
                    {
                        surfsectionBox.Items.Add("Section " + (currentSection + 1).ToString());
                        sectionBox.Items.Add("Section " + (currentSection + 1).ToString());

                    }
                    surfmaterialBox.Items.Clear();
                    for (int surfacematerialIndex = 0; surfacematerialIndex < surfaceType.Length; surfacematerialIndex++)
                    {
                        surfmaterialBox.Items.Add(surfaceTypeID[surfacematerialIndex].ToString("X") + "- " + surfaceType[surfacematerialIndex]);
                    }
                    viewBox.Items.Clear();
                    foreach (var viewstring in viewString)
                    {
                        viewBox.Items.Add(viewstring);
                    }
                }

                textureBox.Items.Clear();
                for (int materialIndex = 0; materialIndex < materialCount; materialIndex++)
                {
                    if (textureArray[materialIndex].texturePath != null)
                    {
                        if (textureArray[materialIndex].textureClass == -1)
                        {
                            MessageBox.Show("Warning! Texture wrong dimensions -" + textureArray[materialIndex].textureName + "- Height: " + textureArray[materialIndex].textureHeight + "   Width: " + textureArray[materialIndex].textureWidth);
                            textureBox.Items.Add("UNUSABLE " + materialIndex.ToString() + " - " + textureArray[materialIndex].textureName);
                        }
                        else
                        {
                            textureBox.Items.Add("M-" + materialIndex.ToString() + " " + textureArray[materialIndex].textureName);
                        }
                        textureCount++;
                    }
                    else
                    {
                        MessageBox.Show("Warning! Material " + fbx.Materials[materialIndex].Name + " does not have a diffuse texture and cannot be used.");
                        textureArray[materialIndex].textureName = fbx.Materials[materialIndex].Name;
                        textureBox.Items.Add("UNUSABLE " + materialIndex.ToString() + " - " + textureArray[materialIndex].textureName);
                    }
                }
                if (levelFormat == 0)
                {
                    viewBox.SelectedIndex = 0;
                    sectionBox.SelectedIndex = 0;
                    textureBox.SelectedIndex = 0;
                    lastMaterial = 0;

                }
                MessageBox.Show("Finished" );
                loaded = true;
                actionBtn.Text = "Compile";
            }

            /**
             * Read OK64.POP File
             */
            if (File.Exists(okSettings.CurrentDirectory + "\\POP.OK64"))
            {
                popFile = okSettings.CurrentDirectory + "\\POP.OK64";
                if (levelFormat == 0)
                {
                    pathGroups = tm64Path.loadPOP(popFile, surfaceObjects);
                }
                else
                {
                    pathGroups = tm64Path.loadBattlePOP(popFile);
                }
            }
            else
            {
                throw new FileNotFoundException("Error: Place 'OK64.POP' file beside the FBX file.\nIt must be named 'OK64.POP'");
            }
            openGLControl.Visible = true;
            openGLControl.Enabled = true;


            /*
             * Import OK64 CONFIG
             */
             ImportCourseInfo();
        }





        private void LoadBtn_Click(object sender, EventArgs e)
        {
            if (loaded)
            {
                CompileModel();
            }
            else
            {
                LoadModel();
            }
        }
        private void Matbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updateTXDisplay())
            {
                lastMaterial = textureBox.SelectedIndex;
            }
            else
            {
                textureBox.SelectedIndex = lastMaterial;
                MessageBox.Show("Selected Material Unavailable!");
            }
        }

        private void SectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded == true)
            {
                UpdateSVDisplay();
            }
        }

        private void ViewBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded == true)
            {
                UpdateSVDisplay();
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
                    foreach (var subObject in sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList)
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
            objectBox.Text = objectCount.ToString();
        }
        //
        private void updateSMDisplay()
        {
            if (loaded == true)
            {
                int objectIndex = surfaceobjectBox.SelectedIndex;

                surfsectionBox.SelectedIndex = surfaceObjects[objectIndex].surfaceID - 1;
                int materialIndex = Array.IndexOf(surfaceTypeID, surfaceObjects[objectIndex].surfaceMaterial);
                surfmaterialBox.SelectedIndex = materialIndex;
                surfpropertybox.SelectedIndex = surfaceObjects[objectIndex].surfaceProperty;

                surfvertBox.Text = surfaceObjects[objectIndex].vertCount.ToString();
                surffaceBox.Text = surfaceObjects[objectIndex].faceCount.ToString();
            }
        }

        private bool updateTXDisplay()
        {
            if (textureArray[textureBox.SelectedIndex].texturePath != null)
            {
                bitm.ImageLocation = textureArray[textureBox.SelectedIndex].texturePath;
                heightBox.Text = textureArray[textureBox.SelectedIndex].textureHeight.ToString();
                widthBox.Text = textureArray[textureBox.SelectedIndex].textureWidth.ToString();
                textureModeSBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureModeS;
                textureModeTBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureModeT;
                textureAlphaBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureTransparent;
                textureCodecBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureCodec;
                textureScrollSBox.Text = textureArray[textureBox.SelectedIndex].textureScrollS.ToString();
                textureScrollTBox.Text = textureArray[textureBox.SelectedIndex].textureScrollT.ToString();
                vertAlphaBox.Text = textureArray[textureBox.SelectedIndex].vertAlpha.ToString();
                return true;
            }
            else
            {
                return false;
            }
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
                sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList = checkList.ToArray();

                updateCounter(faceCount);
            }
        }

        private void Formatbox_SelectedIndexChanged(object sender, EventArgs e)
        {

            //this is disabled because I currently don't know how to write the F3DEX to load a CI8 format texture.
            //the code for converting the file to CI8, MIO0 compressing and writing it to ROM is fine...
            // but in the section where Course Objects are written to segment 7 there is no switch for CI8...
            // or the material types 3-5. Only RGBA16 is supported at this time, until that code is written.


            TarmacGeometry.textureClass(textureArray[textureBox.SelectedIndex]);
            updateTXDisplay();
        }

        private void SurfaceobjectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateSMDisplay();
        }

        private void SurfsectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            surfaceObjects[surfaceobjectBox.SelectedIndex].surfaceID = surfsectionBox.SelectedIndex + 1;
        }

        private void SurfmaterialBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] surfacesplit = surfmaterialBox.Items[surfmaterialBox.SelectedIndex].ToString().Split('-');
            int surfaceIndex = Convert.ToInt32(surfacesplit[0], 16);
            surfaceObjects[surfaceobjectBox.SelectedIndex].surfaceMaterial = Convert.ToByte(surfaceIndex);
        }

        private void Rtbox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Gtbox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Btbox_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Rbbox_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Gbbox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Bbox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Cptop_Click(object sender, EventArgs e)
        {
            ColorDialog skyDialog = new ColorDialog();

            skyDialog.AllowFullOpen = true;
            skyDialog.ShowHelp = true;


            int rr = 0;
            int.TryParse(SkyRT.Text, out rr);
            int gg = 0;
            int.TryParse(SkyGT.Text, out gg);
            int bb = 0;
            int.TryParse(SkyBT.Text, out bb);
            System.Drawing.Color buttonColor = System.Drawing.Color.FromArgb(rr, gg, bb);

            skyDialog.Color = buttonColor;

            // Update the text box color if the user clicks OK 
            if (skyDialog.ShowDialog() == DialogResult.OK)
            {
                buttonColor = skyDialog.Color;
                SkyRT.Text = skyDialog.Color.R.ToString();
                SkyGT.Text = skyDialog.Color.G.ToString();
                SkyBT.Text = skyDialog.Color.B.ToString();
            }
            ColorUpdate();

        }

        private void Cpbot_Click(object sender, EventArgs e)
        {
            ColorDialog skyDialog = new ColorDialog();

            skyDialog.AllowFullOpen = true;
            skyDialog.ShowHelp = true;


            int rr = 0;
            int.TryParse(SkyRM.Text, out rr);
            int gg = 0;
            int.TryParse(SkyGM.Text, out gg);
            int bb = 0;
            int.TryParse(SkyBM.Text, out bb);
            System.Drawing.Color buttonColor = System.Drawing.Color.FromArgb(rr, gg, bb);

            skyDialog.Color = buttonColor;

            // Update the text box color if the user clicks OK 
            if (skyDialog.ShowDialog() == DialogResult.OK)
            {
                buttonColor = skyDialog.Color;
                SkyRM.Text = skyDialog.Color.R.ToString();
                SkyGM.Text = skyDialog.Color.G.ToString();
                SkyBM.Text = skyDialog.Color.B.ToString();
            }
            ColorUpdate();

        }


        private void PreviewBtn_Click(object sender, EventArgs e)
        {
            fileOpen.InitialDirectory = okSettings.ProjectDirectory;
            fileOpen.IsFolderPicker = false;
            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Get the path of specified file
                previewBox.Text = fileOpen.FileName;
            }
        }

        private void BannerBtn_Click(object sender, EventArgs e)
        {
            fileOpen.InitialDirectory = okSettings.ProjectDirectory;
            fileOpen.IsFolderPicker = false;
            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Get the path of specified file
                bannerBox.Text = fileOpen.FileName;
            }
        }

        private void MapBtn_Click(object sender, EventArgs e)
        {
            fileOpen.InitialDirectory = okSettings.ProjectDirectory;
            fileOpen.IsFolderPicker = false;
            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Get the path of specified file
                mapBox.Text = fileOpen.FileName;
            }
        }

        private void AsmBtn_Click(object sender, EventArgs e)
        {
            fileOpen.InitialDirectory = okSettings.ProjectDirectory;
            fileOpen.IsFolderPicker = false;
            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Get the path of specified file
                asmBox.Text = fileOpen.FileName;
            }
        }


        private void ghostbtn_Click(object sender, EventArgs e)
        {
            fileOpen.InitialDirectory = okSettings.ProjectDirectory;
            fileOpen.IsFolderPicker = false;
            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Get the path of specified file
                ghostBox.Text = fileOpen.FileName;
            }
        }

        private void tBox_CheckedChanged(object sender, EventArgs e)
        {
            
        }




        private void DrawFace(TM64_Geometry.Face subFace)
        {            
            foreach (var subVert in subFace.VertData)
            {
                gl.Color(subVert.color.R, subVert.color.G, subVert.color.B, 1.0f);
                gl.TexCoord(subVert.position.u, subVert.position.v);
                gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
            }            
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

        private void DrawFace(TM64_Geometry.Face subFace, float[] colorArray)
        {
            if (colorArray.Length > 3)
            {
                foreach (var subVert in subFace.VertData)
                {
                    gl.Color(colorArray[0], colorArray[1], colorArray[2], colorArray[3]);
                    gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
                }
            }
            else
            {
                foreach (var subVert in subFace.VertData)
                {
                    gl.Color(colorArray[0], colorArray[1], colorArray[2], 1.0f);
                    gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
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

        private void drawObjects()
        {

            
            List<string> drawnObjects = new List<string>();
            //draw faces

            switch (tabControl1.SelectedIndex)
            {
                case 2:
                    {



                        for (int subIndex = 0; subIndex < masterObjects.Length; subIndex++)
                        {

                            if (subIndex == highlightedObject)
                            {
                                if (chkHover.Checked)
                                {
                                    gl = TarmacGL.DrawTarget(gl, localCamera, glTexture, masterObjects[subIndex]);
                                }
                                else
                                {
                                    if (Array.IndexOf(sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList, subIndex) != -1)
                                    {
                                        if (CheckboxTextured.Checked)
                                        {
                                            gl = TarmacGL.DrawTextured(gl, textureArray, localCamera, glTexture, masterObjects[subIndex]);
                                        }
                                        else
                                        {
                                            gl = TarmacGL.DrawShaded(gl, textureArray, localCamera, glTexture, masterObjects[subIndex]);
                                        }
                                    }
                                    else if (chkWireframe.Checked)
                                    {
                                        gl = TarmacGL.DrawWire(gl, textureArray, localCamera, glTexture, masterObjects[subIndex]);
                                    }
                                }
                            }
                            else
                            {

                                if (Array.IndexOf(sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList, subIndex) != -1)
                                {
                                    if (CheckboxTextured.Checked)
                                    {
                                        gl = TarmacGL.DrawTextured(gl, textureArray, localCamera, glTexture, masterObjects[subIndex]);
                                    }
                                    else
                                    {
                                        gl = TarmacGL.DrawShaded(gl, textureArray, localCamera, glTexture, masterObjects[subIndex]);
                                    }
                                }
                                else if (chkWireframe.Checked)
                                {
                                    gl = TarmacGL.DrawWire(gl, textureArray, localCamera, glTexture, masterObjects[subIndex]);
                                }
                            }

                        }

                        if (chkSection.Checked)
                        {
                            foreach (var subObject in surfaceObjects)
                            {
                                if (subObject.surfaceID == (sectionBox.SelectedIndex + 1))
                                {
                                    gl = TarmacGL.DrawSection(gl, localCamera, glTexture, subObject);
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        for (int subIndex = 0; subIndex < surfaceObjects.Length; subIndex++)
                        {
                            if (subIndex == highlightedObject)
                            {
                                if (chkHover.Checked)
                                {
                                    gl = TarmacGL.DrawTarget(gl, localCamera, glTexture, surfaceObjects[subIndex]);
                                }
                                else
                                {
                                    if (Array.IndexOf(sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList, subIndex) != -1)
                                    {
                                        if (CheckboxTextured.Checked)
                                        {
                                            gl = TarmacGL.DrawTextured(gl, textureArray, localCamera, glTexture, masterObjects[subIndex]);
                                        }
                                        else
                                        {
                                            gl = TarmacGL.DrawShaded(gl, textureArray, localCamera, glTexture, masterObjects[subIndex]);
                                        }
                                    }
                                    else if (chkWireframe.Checked)
                                    {
                                        gl = TarmacGL.DrawWire(gl, textureArray, localCamera, glTexture, masterObjects[subIndex]);
                                    }
                                }
                            }
                            foreach (var face in surfaceObjects[subIndex].modelGeometry)
                            {
                                gl = TarmacGL.DrawShaded(gl, glTexture, face, surfaceObjects[subIndex].objectColor);
                            }
                        }
                        break;
                    }
                default:
                    {

                        foreach (var subObject in masterObjects)
                        {
                            if (CheckboxTextured.Checked)
                            {
                                gl = TarmacGL.DrawTextured(gl, textureArray, localCamera, glTexture, subObject);
                            }
                            else
                            {
                                gl = TarmacGL.DrawShaded(gl, textureArray, localCamera, glTexture, subObject);
                            }
                        }
                        break;
                    }
            }

            if (levelFormat == 0)
            {
                if (chkPop.Checked)
                {

                    foreach (var list in pathGroups[0].pathList)
                    {
                        float[] surfaceYellow = GetYellowFlash(flashYellow);
                        gl.End();
                        gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                        glTexture.Destroy(gl);
                        gl.Begin(OpenGL.GL_TRIANGLES);
                        foreach (var marker in list.pathmarker)
                        {
                            DrawMarker(markerGeometry, marker, surfaceYellow);
                        }
                    }
                    foreach (var list in pathGroups[1].pathList)
                    {
                        float[] color = { 0.5f, 0f, 1.0f };
                        gl.End();
                        gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                        glTexture.Destroy(gl);
                        gl.Begin(OpenGL.GL_TRIANGLES);
                        foreach (var marker in list.pathmarker)
                        {
                            DrawMarker(itemGeometry, marker, color);
                        }
                    }
                    foreach (var list in pathGroups[2].pathList)
                    {
                        if (CheckboxTextured.Checked)
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
                        else
                        {
                            float[] color = { 1.0f, 0.5f, 0 };
                            gl.End();
                            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                            glTexture.Destroy(gl);
                            gl.Begin(OpenGL.GL_TRIANGLES);
                            foreach (var marker in list.pathmarker)
                            {
                                DrawMarker(piranhaGeometry, marker, color);
                            }
                        }



                    }
                    foreach (var list in pathGroups[3].pathList)
                    {
                        float[] color = { 1.0f, 0.5f, 0 };
                        gl.End();
                        gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                        glTexture.Destroy(gl);
                        gl.Begin(OpenGL.GL_TRIANGLES);
                        foreach (var marker in list.pathmarker)
                        {
                            DrawMarker(piranhaGeometry, marker, color);
                        }
                    }
                    foreach (var list in pathGroups[4].pathList)
                    {
                        float[] color = { 1.0f, 0, 0 };
                        gl.End();
                        gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                        glTexture.Destroy(gl);
                        gl.Begin(OpenGL.GL_TRIANGLES);
                        foreach (var marker in list.pathmarker)
                        {
                            DrawMarker(redcoinGeometry, marker, color);
                        }
                    }
                }
            }
            
            loadGL = true;
        }



        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            //  Get the OpenGL object. 

            localCamera.flashWhite = GetAlphaFlash(flashWhite);
            localCamera.flashRed = GetAlphaFlash(flashRed);
            localCamera.flashYellow = GetAlphaFlash(flashYellow);
            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            gl = TarmacGL.DrawNorth(gl, glTexture, localCamera);

            if (loaded)
            {
                drawObjects();
            }
            gl.End();
            
            gl.Flush();
            
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

        private void ExportBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFile.FileName;

                TM64_Geometry TarmacGeometry = new TM64_Geometry();
                TarmacGeometry.ExportSVL(filePath, masterObjects.Length, sectionList, masterObjects);
            }
        }

        private void ImportBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFile.FileName;

                TM64_Geometry TarmacGeometry = new TM64_Geometry();
                TM64_Geometry.OK64SectionList[] tempList = TarmacGeometry.ImportSVL(filePath, masterObjects.Length, masterObjects);
                if (tempList.Length > 0)
                {
                    sectionList = tempList;
                    UpdateSVDisplay();

                }
                else
                {
                    MessageBox.Show("Error! Incorrect Object Count");
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            foreach (var list in pathGroups[0].pathList)
            {
                for (int currentMarker = 0; currentMarker < list.pathmarker.Count; currentMarker++)
                {
                    localCamera.position = new Vector3D(list.pathmarker[currentMarker].xval, list.pathmarker[currentMarker].yval, list.pathmarker[currentMarker].zval + 10);
                    if (currentMarker < list.pathmarker.Count - 1)
                    {
                        localCamera.target = new Vector3D(list.pathmarker[currentMarker + 1].xval, list.pathmarker[currentMarker + 1].yval, list.pathmarker[currentMarker + 1].zval + 6);
                    }
                    else
                    {
                        localCamera.target = new Vector3D(list.pathmarker[0].xval, list.pathmarker[0].yval, list.pathmarker[0].zval + 6);                    
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }


        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void surfacepropertybox_SelectedIndexChanged(object sender, EventArgs e)
        {
            surfaceObjects[surfaceobjectBox.SelectedIndex].surfaceProperty = surfpropertybox.SelectedIndex;
        }

        private void openGLControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Point mouseClick = new System.Windows.Point(e.Location.X, e.Location.Y);

            double[] pointA = gl.UnProject(e.Location.X, e.Location.Y, 0);
            double[] pointB = gl.UnProject(e.Location.X, e.Location.Y, 1);

            Vector3D rayOrigin = new Vector3D(Convert.ToSingle(pointA[0]), Convert.ToSingle(pointA[1]), Convert.ToSingle(pointA[2]));
            Vector3D rayTarget = new Vector3D(Convert.ToSingle(pointB[0]), Convert.ToSingle(pointB[1]), Convert.ToSingle(pointB[2] * -1));


            float objectDistance = -1;
            TM64_Geometry tmGeo = new TM64_Geometry();
            int objectID = -1;
            cName.Text = "";
            
            switch (tabControl1.SelectedIndex)
            {
                default:
                    {
                        break;
                    }
                case 2:
                    {
                        for (int currentObject = 0; (currentObject < masterObjects.Length); currentObject++)
                        {

                            foreach (var face in masterObjects[currentObject].modelGeometry)
                            {

                                Vector3D intersectPoint = tmGeo.testIntersect(rayOrigin, rayTarget, face.VertData[0], face.VertData[1], face.VertData[2]);
                                if (intersectPoint.X > 0)
                                {
                                    if (objectDistance > intersectPoint.X | objectDistance == -1)
                                    {
                                        objectDistance = intersectPoint.X;
                                        objectID = currentObject;
                                        cName.Text = masterObjects[currentObject].objectName;
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                        if (objectID > -1)
                        {
                            var SelectedNodes = masterBox.Nodes.Find(masterObjects[objectID].objectName, true);
                            
                            masterBox.SelectedNode = SelectedNodes[0];
                            if (e.Button == MouseButtons.Right)
                            {
                                SelectedNodes[0].Checked = !SelectedNodes[0].Checked;
                                updateSectionList(masterObjects[objectID].objectName);
                            }
                            else
                            {
                                masterBox.Focus();
                            }
                            
                        }

                        break;
                    }
                case 3:
                    {
                        for (int currentObject = 0; (currentObject < surfaceObjects.Length); currentObject++)
                        {

                            foreach (var face in surfaceObjects[currentObject].modelGeometry)
                            {

                                Vector3D intersectPoint = tmGeo.testIntersect(rayOrigin, rayTarget, face.VertData[0], face.VertData[1], face.VertData[2]);
                                if (intersectPoint.X > 0)
                                {
                                    if (objectDistance > intersectPoint.X | objectDistance == -1)
                                    {
                                        objectDistance = intersectPoint.X;
                                        objectID = currentObject;
                                        cName.Text = surfaceObjects[currentObject].objectName;
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                        if (objectID > -1)
                        {
                            surfaceobjectBox.SelectedIndex = objectID;
                            if (e.Button == MouseButtons.Left)
                            {
                                surfaceobjectBox.Focus();
                            }
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
            int objectID = -1;
            cName.Text = "";



            switch (tabControl1.SelectedIndex)
            {
                default:
                    {
                        break;
                    }
                case 2:
                    {

                        for (int currentObject = 0; (currentObject < masterObjects.Length); currentObject++)
                        {

                            foreach (var face in masterObjects[currentObject].modelGeometry)
                            {

                                Vector3D intersectPoint = tmGeo.testIntersect(rayOrigin, rayTarget, face.VertData[0], face.VertData[1], face.VertData[2]);
                                if (intersectPoint.X > 0)
                                {
                                    if (objectDistance > intersectPoint.X | objectDistance == -1)
                                    {
                                        objectDistance = intersectPoint.X;
                                        objectID = currentObject;
                                        cName.Text = masterObjects[currentObject].objectName;
                                    }
                                }
                                else
                                {

                                }
                            }
                        }

                        break;
                    }
                case 3:
                    {

                        for (int currentObject = 0; (currentObject < surfaceObjects.Length); currentObject++)
                        {

                            foreach (var face in surfaceObjects[currentObject].modelGeometry)
                            {

                                Vector3D intersectPoint = tmGeo.testIntersect(rayOrigin, rayTarget, face.VertData[0], face.VertData[1], face.VertData[2]);
                                if (intersectPoint.X > 0)
                                {
                                    if (objectDistance > intersectPoint.X | objectDistance == -1)
                                    {
                                        objectDistance = intersectPoint.X;
                                        objectID = currentObject;
                                        cName.Text = surfaceObjects[currentObject].objectName;
                                    }
                                }
                                else
                                {

                                }
                            }
                        }

                        break;
                    }


            }
            highlightedObject = objectID;
        }

        private void updateSectionList(string objectName)
        {
            List<int> objectList = sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList.ToList();
            int objectIndex = -1;
            for (int currentObject = 0; currentObject < masterObjects.Length; currentObject++)
            {                
                if (masterObjects[currentObject].objectName == objectName)
                {
                    objectIndex = currentObject;
                    break;
                }
            }
            if (objectIndex > -1)
            {
                int objectCount = objectList.Count; //this value is dynamic as we add/remove items.
                for (int currentObject = 0; currentObject < objectCount; currentObject++)
                {
                    if (currentObject < objectList.Count) //dynamic
                    {
                        if (objectList[currentObject] == objectIndex)
                        {
                            objectList.RemoveAt(currentObject);
                            break;
                        }
                        else
                        {
                            if (currentObject + 1 == objectList.Count)
                            {
                                objectList.Add(objectIndex);
                            }
                        }
                    }
                }
                sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList = objectList.ToArray();
            }
            UpdateSVDisplay();
        }

        private void masterBox_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                List<int> checkList = new List<int>();
                bool checkState = e.Node.Checked;
                checkList = sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList.ToList();
                int faceCount = 0;
                if (loaded == true)
                {
                    if (e.Node.Nodes.Count > 0)
                    {
                        
                        foreach (TreeNode childNode in e.Node.Nodes)
                        {
                            childNode.Checked = checkState;
                            updateSectionList(childNode.Name);
                        }
                        

                    }
                    else
                    {
                        updateSectionList(e.Node.Name);
                    }
                    
                    
                }
            }
        }


        private void ColorPickT_Click(object sender, EventArgs e)
        {
            ColorDialog ColorPick = new ColorDialog();
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                SkyRT.Text = ColorPick.Color.R.ToString();
                SkyGT.Text = ColorPick.Color.G.ToString();
                SkyBT.Text = ColorPick.Color.B.ToString();
            }
            ColorUpdate();
        }

        private void ColorPickMT_Click(object sender, EventArgs e)
        {
            ColorDialog ColorPick = new ColorDialog();
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                SkyRM.Text = ColorPick.Color.R.ToString();
                SkyGM.Text = ColorPick.Color.G.ToString();
                SkyBM.Text = ColorPick.Color.B.ToString();
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


        private void SkyBB_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }

        private void SkyGB_TextChanged(object sender, EventArgs e)
        {
            ColorUpdate();
        }


        private void SkyRB_TextChanged(object sender, EventArgs e)
        {
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
            fileOpen.InitialDirectory = okSettings.ProjectDirectory;
            fileOpen.IsFolderPicker = false;
            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                previewBox.Text = fileOpen.FileName;
            }
        }

        private void bannerBtn_Click_1(object sender, EventArgs e)
        {
            fileOpen.InitialDirectory = okSettings.ProjectDirectory;
            fileOpen.IsFolderPicker = false;
            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                bannerBox.Text = fileOpen.FileName;
            }
        }

        private void ghostBtn_Click_1(object sender, EventArgs e)
        {
            fileOpen.InitialDirectory = okSettings.ProjectDirectory;
            fileOpen.IsFolderPicker = false;
            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ghostBox.Text = fileOpen.FileName;
            }
        }

        private void asmBtn_Click_1(object sender, EventArgs e)
        {
            fileOpen.InitialDirectory = okSettings.ProjectDirectory;
            fileOpen.IsFolderPicker = false;
            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                asmBox.Text = fileOpen.FileName;
            }
        }

        private void mapBtn_Click_1(object sender, EventArgs e)
        {
            fileOpen.InitialDirectory = okSettings.ProjectDirectory;
            fileOpen.IsFolderPicker = false;
            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
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

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void textureModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureModeS = textureModeSBox.SelectedIndex;
        }

        private void textureAlphaBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureTransparent = textureAlphaBox.SelectedIndex;
        }

        private void textureCodecBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureCodec = textureCodecBox.SelectedIndex;
        }

        private void textureModeTBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureModeT = textureModeTBox.SelectedIndex;
        }

        private void ExportCourseInfo()
        {
            gameSpeed = new int[4];

            int.TryParse(sp1Box.Text, out gameSpeed[0]);
            int.TryParse(sp2Box.Text, out gameSpeed[1]);
            int.TryParse(sp3Box.Text, out gameSpeed[2]);
            int.TryParse(sp4Box.Text, out gameSpeed[3]);


            //Course Music

            byte songID = Convert.ToByte(songBox.SelectedIndex);




            string courseName = nameBox.Text;
            string previewImage = previewBox.Text;
            string bannerImage = bannerBox.Text;
            string mapImage = mapBox.Text;
            string customASM = asmBox.Text;
            string ghostData = ghostBox.Text;




            Int16[] mapCoords = new Int16[2];
            Int16[] startCoords = new Int16[2];

            Int16.TryParse(mapXBox.Text, out mapCoords[0]);
            Int16.TryParse(mapYBox.Text, out mapCoords[1]);
            Int16.TryParse(startXBox.Text, out startCoords[0]);
            Int16.TryParse(startYBox.Text, out startCoords[1]);



            int[] echoValues = new int[2];

            int.TryParse(EchoStartBox.Text, out echoValues[0]);
            int.TryParse(EchoStopBox.Text, out echoValues[1]);

            TM64_Course.Course courseData = new TM64_Course.Course();
            courseData.Credits = courseName;
            courseData.PreviewPath = previewImage;
            courseData.BannerPath = bannerImage;
            courseData.MapData = new TM64_Course.MiniMap();
            courseData.MapData.MinimapPath = mapImage;
            courseData.MapData.MapCoord = new Vector2D(mapCoords[0], mapCoords[1]);
            courseData.MapData.StartCoord = new Vector2D(startCoords[0], startCoords[1]);
            courseData.MapData.MapColor = mapData.MapColor;

            float tempfloat = new float();
            Single.TryParse(MapScaleBox.Text, out tempfloat);
            courseData.MapData.MapScale = tempfloat;


            courseData.EchoValues = echoValues;
            courseData.AssmeblyPath = customASM;
            courseData.GhostPath = ghostData;
            courseData.SkyColors = skyData;

            courseData.MusicID = songID;
            courseData.GameTempos = gameSpeed;
            courseData.PathLength = pathGroups[0].pathList[0].pathmarker.Count;

            Single.TryParse(waterBox.Text, out tempfloat);
            courseData.WaterLevel = tempfloat;

            if (!File.Exists(okSettings.CurrentDirectory + "\\CONFIG.OK64"))
            {
                File.Create(okSettings.CurrentDirectory + "\\CONFIG.OK64");
            }
            if (File.Exists(okSettings.CurrentDirectory + "\\CONFIG.OK64"))
            {
                TM64_Course TarmacCourse = new TM64_Course();
                TarmacCourse.WriteCourseInfo(courseData, okSettings.CurrentDirectory + "\\CONFIG.OK64");
            } else {
                throw new FileNotFoundException("ERROR: Could not write CONFIG.OK64");
            }
        }

        private void ImportCourseInfo()
        {
            if (File.Exists(okSettings.CurrentDirectory + "\\CONFIG.OK64"))
            {
                TM64_Course TarmacCourse = new TM64_Course();
                TM64_Course.Course CourseData = TarmacCourse.ReadCourseInfo(okSettings.CurrentDirectory + "\\CONFIG.OK64");
                if (CourseData != null)
                {
                    nameBox.Text = CourseData.Credits;
                    previewBox.Text = CourseData.PreviewPath;
                    bannerBox.Text = CourseData.BannerPath;
                    ghostBox.Text = CourseData.GhostPath;
                    asmBox.Text = CourseData.AssmeblyPath;
                    mapBox.Text = CourseData.MapData.MinimapPath;
                    mapXBox.Text = CourseData.MapData.MapCoord.X.ToString();
                    mapYBox.Text = CourseData.MapData.MapCoord.Y.ToString();
                    startXBox.Text = CourseData.MapData.StartCoord.X.ToString();
                    startYBox.Text = CourseData.MapData.StartCoord.Y.ToString();
                    MapScaleBox.Text = CourseData.MapData.MapScale.ToString();
                    MapRBox.Text = CourseData.MapData.MapColor.R.ToString();
                    MapRBox.Text = CourseData.MapData.MapColor.G.ToString();
                    MapRBox.Text = CourseData.MapData.MapColor.B.ToString();
                    SkyRT.Text = CourseData.SkyColors.TopColor.R.ToString();
                    SkyGT.Text = CourseData.SkyColors.TopColor.G.ToString();
                    SkyBT.Text = CourseData.SkyColors.TopColor.B.ToString();

                    SkyRM.Text = CourseData.SkyColors.MidColor.R.ToString();
                    SkyGM.Text = CourseData.SkyColors.MidColor.G.ToString();
                    SkyBM.Text = CourseData.SkyColors.MidColor.B.ToString();

                    SkyRB.Text = CourseData.SkyColors.BotColor.R.ToString();
                    SkyGB.Text = CourseData.SkyColors.BotColor.G.ToString();
                    SkyBB.Text = CourseData.SkyColors.BotColor.B.ToString();

                    EchoStartBox.Text = CourseData.EchoValues[0].ToString();
                    EchoStopBox.Text = CourseData.EchoValues[1].ToString();
                    waterBox.Text = CourseData.WaterLevel.ToString();
                    sp1Box.Text = CourseData.GameTempos[0].ToString();
                    sp2Box.Text = CourseData.GameTempos[1].ToString();
                    sp3Box.Text = CourseData.GameTempos[2].ToString();
                    sp4Box.Text = CourseData.GameTempos[3].ToString();
                    songBox.SelectedIndex = CourseData.MusicID;
                }

            }
            else
            {
                MessageBox.Show("CONFIG.OK64 Error! Continuing without loading previous user input.");
            }
        }

        private void textureScrollSBox_TextChanged(object sender, EventArgs e)
        {
            int sScroll;
            if (int.TryParse(textureScrollSBox.Text, out sScroll))
            {
                textureArray[textureBox.SelectedIndex].textureScrollS = sScroll;
            }

        }

        private void textureScrollTBox_TextChanged(object sender, EventArgs e)
        {
            int tScroll;
            if (int.TryParse(textureScrollTBox.Text, out tScroll))
            {
                textureArray[textureBox.SelectedIndex].textureScrollT = tScroll;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            int vAlpha;
            if (int.TryParse(vertAlphaBox.Text, out vAlpha))
            {
                textureArray[textureBox.SelectedIndex].vertAlpha = vAlpha;
            }
        }
    }
}





