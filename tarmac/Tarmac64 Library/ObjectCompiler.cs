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
using PeepsCompress;
using Assimp;
using Tarmac64_Library;
using Tarmac64_Geometry;
using Tarmac64_Paths;
using System.Text.RegularExpressions;
using Tarmac64_Library.Properties;
using SharpGL;
using SharpGL.SceneGraph.Core;
using System.Drawing.Design;
using System.Windows.Input;
using System.Drawing.Imaging;
using Cereal64.Microcodes.F3DEX.DataElements;

namespace Tarmac64
{


    public partial class ObjectCompiler: Form
    {



        TabPage sectionPage = new TabPage();
        TabPage surfacePage = new TabPage();

        bool updateBool = false;
        bool raycastBoolean = false;
        int sectionCount = 0;
        public int programFormat;
        int levelFormat = 0;


        public ObjectCompiler()
        {
            InitializeComponent();
        }

        Stopwatch clockTime = new Stopwatch();
        bool loadGL = false;



        float[] flashRed = { 1.0f, 0.0f, 0.0f, 1.0f, 0.0f };
        float[] flashYellow = { 1.0f, 1.0f, 1.0f, 1.0f, 0.0f };
        float[] flashWhite = { 1.0f, 1.0f, 1.0f, 0.5f, 0.0f };
        int highlightedObject = -1;


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
        TM64_Paths.Pathgroup[] popData = new TM64_Paths.Pathgroup[0];
        string popFile = "";

        TM64_Geometry.OK64F3DObject[] masterObjects = new TM64_Geometry.OK64F3DObject[0];
        TM64_Geometry.OK64F3DGroup[] masterGroups = new TM64_Geometry.OK64F3DGroup[0];
        int moveDistance = 20;


        List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>();

        TM64_Geometry.OK64F3DObject[] surfaceObjects = new TM64_Geometry.OK64F3DObject[0];
    

        TM64_Geometry.OK64Texture[] textureArray = new TM64_Geometry.OK64Texture[0];

        int lastMaterial = 0;


        byte[] skyBytes = new byte[6];

        AssimpContext AssimpImporter = new AssimpContext();
        Assimp.Scene fbx = new Assimp.Scene();
        int materialCount;

        TM64_Geometry.TMCamera localCamera = new TM64_Geometry.TMCamera();
        OpenGL gl = new OpenGL();



        OpenFileDialog fileOpen = new OpenFileDialog();
        SaveFileDialog fileSave = new SaveFileDialog();
        FolderBrowserDialog folderOpen = new FolderBrowserDialog();
        private void GeometryCompiler_Load(object sender, EventArgs e)
        {
            CreateGeometry();
            
            
            gl = openGLControl.OpenGL;
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Tarmac64 Designer")
            {
                about_button.Visible = true;
            }
            else
            {
                about_button.Visible = false;
            }
            this.MaximumSize = new System.Drawing.Size(575,440);
            this.Width = 575;




        }



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
            treeGeometry[0].VertData[0].position.x = Convert.ToInt16(-40);
            treeGeometry[0].VertData[0].position.y = Convert.ToInt16(0);
            treeGeometry[0].VertData[0].position.z = Convert.ToInt16(0);

            treeGeometry[0].VertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[0].VertData[1].position = new TM64_Geometry.Position();
            treeGeometry[0].VertData[1].position.x = Convert.ToInt16(40);
            treeGeometry[0].VertData[1].position.y = Convert.ToInt16(0);
            treeGeometry[0].VertData[1].position.z = Convert.ToInt16(0);

            treeGeometry[0].VertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[0].VertData[2].position = new TM64_Geometry.Position();
            treeGeometry[0].VertData[2].position.x = Convert.ToInt16(40);
            treeGeometry[0].VertData[2].position.y = Convert.ToInt16(0);
            treeGeometry[0].VertData[2].position.z = Convert.ToInt16(80);


            treeGeometry[1] = new TM64_Geometry.Face();
            treeGeometry[1].VertData = new TM64_Geometry.Vertex[3];

            treeGeometry[1].VertData[0] = new TM64_Geometry.Vertex();
            treeGeometry[1].VertData[0].position = new TM64_Geometry.Position();
            treeGeometry[1].VertData[0].position.x = Convert.ToInt16(0);
            treeGeometry[1].VertData[0].position.y = Convert.ToInt16(-40);
            treeGeometry[1].VertData[0].position.z = Convert.ToInt16(0);

            treeGeometry[1].VertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[1].VertData[1].position = new TM64_Geometry.Position();
            treeGeometry[1].VertData[1].position.x = Convert.ToInt16(0);
            treeGeometry[1].VertData[1].position.y = Convert.ToInt16(40);
            treeGeometry[1].VertData[1].position.z = Convert.ToInt16(0);

            treeGeometry[1].VertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[1].VertData[2].position = new TM64_Geometry.Position();
            treeGeometry[1].VertData[2].position.x = Convert.ToInt16(0);
            treeGeometry[1].VertData[2].position.y = Convert.ToInt16(40);
            treeGeometry[1].VertData[2].position.z = Convert.ToInt16(80);

            treeGeometry[2] = new TM64_Geometry.Face();
            treeGeometry[2].VertData = new TM64_Geometry.Vertex[3];

            treeGeometry[2].VertData[0] = new TM64_Geometry.Vertex();
            treeGeometry[2].VertData[0].position = new TM64_Geometry.Position();
            treeGeometry[2].VertData[0].position.x = Convert.ToInt16(-40);
            treeGeometry[2].VertData[0].position.y = Convert.ToInt16(0);
            treeGeometry[2].VertData[0].position.z = Convert.ToInt16(0);

            treeGeometry[2].VertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[2].VertData[1].position = new TM64_Geometry.Position();
            treeGeometry[2].VertData[1].position.x = Convert.ToInt16(40);
            treeGeometry[2].VertData[1].position.y = Convert.ToInt16(0);
            treeGeometry[2].VertData[1].position.z = Convert.ToInt16(80);

            treeGeometry[2].VertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[2].VertData[2].position = new TM64_Geometry.Position();
            treeGeometry[2].VertData[2].position.x = Convert.ToInt16(-40);
            treeGeometry[2].VertData[2].position.y = Convert.ToInt16(0);
            treeGeometry[2].VertData[2].position.z = Convert.ToInt16(80);


            treeGeometry[3] = new TM64_Geometry.Face();
            treeGeometry[3].VertData = new TM64_Geometry.Vertex[3];

            treeGeometry[3].VertData[0] = new TM64_Geometry.Vertex();
            treeGeometry[3].VertData[0].position = new TM64_Geometry.Position();
            treeGeometry[3].VertData[0].position.x = Convert.ToInt16(0);
            treeGeometry[3].VertData[0].position.y = Convert.ToInt16(-40);
            treeGeometry[3].VertData[0].position.z = Convert.ToInt16(0);

            treeGeometry[3].VertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[3].VertData[1].position = new TM64_Geometry.Position();
            treeGeometry[3].VertData[1].position.x = Convert.ToInt16(0);
            treeGeometry[3].VertData[1].position.y = Convert.ToInt16(40);
            treeGeometry[3].VertData[1].position.z = Convert.ToInt16(80);

            treeGeometry[3].VertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[3].VertData[2].position = new TM64_Geometry.Position();
            treeGeometry[3].VertData[2].position.x = Convert.ToInt16(0);
            treeGeometry[3].VertData[2].position.y = Convert.ToInt16(-40);
            treeGeometry[3].VertData[2].position.z = Convert.ToInt16(80);



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


        private void LoadModel()
        {
            MessageBox.Show("Select .FBX File");
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                FBXfilePath = fileOpen.FileName;
                levelFormat = 0;

                clockTime = new Stopwatch();
                clockTime.Start();



                raycastBoolean = false;
                int modelFormat = 0;  // 

                Scene fbx = new Scene();
                AssimpContext importer = new AssimpContext();

                fbx = importer.ImportFile(FBXfilePath, PostProcessPreset.TargetRealTimeMaximumQuality);

                materialCount = fbx.MaterialCount;
                int textureCount = 0;


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


                Assimp.Node pathNode = fbx.RootNode.FindNode("Course Paths");
                Assimp.Mesh countObj = null;


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

                //
                // Textures
                //




                textureArray = tm64Geo.loadTextures(fbx, FBXfilePath);

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
                                masterObjects = tm64Geo.createMaster(fbx, sectionCount, textureArray);
                                surfaceObjects = tm64Geo.loadCollision(fbx, sectionCount, textureArray, modelFormat);
                                sectionList = tm64Geo.AutomateSection(sectionCount, surfaceObjects, masterObjects, fbx, raycastBoolean);
                                break;
                            }
                        case 1:
                            {
                                masterObjects = tm64Geo.loadMaster(ref masterGroups, fbx, textureArray);
                                surfaceObjects = tm64Geo.loadCollision(fbx, sectionCount, textureArray, modelFormat);
                                sectionList = tm64Geo.AutomateSection(sectionCount, surfaceObjects, masterObjects, fbx, raycastBoolean);
                                break;
                            }
                        case 2:
                            {
                                masterObjects = tm64Geo.loadMaster(ref masterGroups, fbx, textureArray);
                                surfaceObjects = tm64Geo.loadCollision(fbx, sectionCount, textureArray, modelFormat);
                                sectionList = tm64Geo.loadSection(fbx, sectionCount, masterObjects);
                                break;
                            }
                    }
                }
                else
                {

                    masterObjects = tm64Geo.loadMaster(ref masterGroups, fbx, textureArray);

                }



                clockTime.Stop();
                TimeSpan ts = clockTime.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

                MessageBox.Show("Finished in " + elapsedTime);
                loaded = true;
                actionBtn.Text = "Compile";
            }

            MessageBox.Show("Select OK64.POP File");
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                popFile = fileOpen.FileName;
                if (levelFormat == 0)
                {
                    popData = tm64Path.loadPOP(popFile, surfaceObjects);
                }
                else
                {
                    popData = tm64Path.loadBattlePOP(popFile);
                }
            }
            openGLControl.Visible = true;
            openGLControl.Enabled = true;
            
        }





        private void LoadBtn_Click(object sender, EventArgs e)
        {
            
        }



       
       






        private void drawFace(TM64_Geometry.Face subFace)
        {            
            foreach (var subVert in subFace.VertData)
            {
                gl.Color(subVert.color.R, subVert.color.G, subVert.color.B, Convert.ToByte(0x255));
                gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
            }            
        }
        private void drawMarker(TM64_Geometry.Face[] subFace, TM64_Paths.Marker pathMarker, float[] colorArray)
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

        private void drawFace(TM64_Geometry.Face subFace, float[] colorArray)
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

        private void drawObjects()
        {
            /*
             * foreach (var subFace in subObject.modelGeometry)
                            {
                                drawFace(subFace, subObject.objectColor);
                            }
            */
            loadGL = true;
        }



        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            //  Get the OpenGL object. 


            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);


            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(localCamera.target.X + 0.0f, localCamera.target.Y + 2.0f, localCamera.target.Z + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + -2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + 2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + 2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + 2.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(localCamera.target.X + 0.0f, localCamera.target.Y + 2.0f, localCamera.target.Z + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + 2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + 2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + 2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + -2.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(localCamera.target.X + 0.0f, localCamera.target.Y + 2.0f, localCamera.target.Z + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + 2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + -2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + -2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + -2.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(localCamera.target.X + 0.0f, localCamera.target.Y + 2.0f, localCamera.target.Z + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + -2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + -2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + -2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + 2.0f);

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

        private void OpenGLControl_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Tarmac64.OKRetail tarmacAbout = new Tarmac64.OKRetail();
            tarmacAbout.Show();

        }

        private void TBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Matbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Formatbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SurfaceobjectBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}





