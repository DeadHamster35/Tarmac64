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

namespace Tarmac64_Library
{


    public partial class ObjectCompiler: Form
    {

        TM64 Tarmac = new TM64();
        TM64_Course TarmacCourse = new TM64_Course();
        TM64_GL TarmacGL = new TM64_GL();

        TM64.OK64Settings okSettings = new TM64.OK64Settings();

        bool updateBool = false;

        int raycastBoolean = 0;
        int sectionCount = 0;
        public int programFormat;
        int levelFormat = 0;
        


        public ObjectCompiler()
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

            skyData.MidColor = new TM64_Geometry.OK64Color();
            skyData.MidColor.R = 0;
            skyData.MidColor.G = 0;
            skyData.MidColor.B = 0;
            skyData.MidColor.A = 0;

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

            ColorUpdate();

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


        }





        private void CompileModel()
        {

            
        }


        private void LoadModel()
        {
            MessageBox.Show("Select .FBX File");
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                FBXfilePath = fileOpen.FileName;
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
                    if (TarmacGeometry.CheckST(masterObjects[currentObject], textureArray[masterObjects[currentObject].materialID]))
                    {
                        MessageBox.Show("Fatal UV Error " + masterObjects[currentObject].objectName);
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
                    
                    mcountBox.Text = materialCount.ToString();
                    tcountBox.Text = textureCount.ToString();

                    textureBox.SelectedIndex = 0;
                    lastMaterial = 0;

                }
                MessageBox.Show("Finished" );
                loaded = true;
                actionBtn.Text = "Compile";
            }

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
            openGLControl.Visible = true;
            openGLControl.Enabled = true;
            
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
            
        }
        //Seperate loading the counters to prevent infinite loop. 
        private void updateCounter(int faceCount)
        {
           
        }
        //
        private void updateSMDisplay()
        {

        }

        private bool updateTXDisplay()
        {
            if (textureArray[textureBox.SelectedIndex].texturePath != null)
            {
                bitm.ImageLocation = textureArray[textureBox.SelectedIndex].texturePath;
                return true;
            }
            else
            {
                return false;
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
           
        }

        private void SurfmaterialBox_SelectedIndexChanged(object sender, EventArgs e)
        {
 
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


            

        }

        private void Cpbot_Click(object sender, EventArgs e)
        {
            

        }


        private void PreviewBtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
            }
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


        //

        //

        //

        //




        


        private void Button1_Click_1(object sender, EventArgs e)
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
            highlightedObject = objectID;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
    }
}





