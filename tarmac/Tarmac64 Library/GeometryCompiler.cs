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


    public partial class GeometryCompiler : Form
    {



        TabPage sectionPage = new TabPage();
        TabPage surfacePage = new TabPage();

        bool updateBool = false;
        bool raycastBoolean = false;
        int sectionCount = 0;
        public int programFormat;
        int levelFormat = 0;


        public GeometryCompiler()
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
            sectionPage = tabControl1.TabPages[3];
            surfacePage = tabControl1.TabPages[4];
            
            
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
            courseBox.SelectedIndex = 0;
            cupBox.SelectedIndex = 0;
            setBox.SelectedIndex = 0;

            rbbox.Text = "216";
            gbbox.Text = "232";
            bbbox.Text = "248";


            rtbox.Text = "128";
            gtbox.Text = "184";
            btbox.Text = "248";
            colorUpdate();

            for (int songIndex = 0; songIndex < songNames.Length; songIndex++)
            {
                songBox.Items.Add(songNames[songIndex]);
            }
            songBox.SelectedIndex = 3;

            sp1Box.Text = "2";
            sp2Box.Text = "2";
            sp3Box.Text = "2";



        }



        private void CreateGeometry()
        {
            markerGeometry[0] = new TM64_Geometry.Face();
            markerGeometry[0].vertData = new TM64_Geometry.Vertex[3];

            markerGeometry[0].vertData[0] = new TM64_Geometry.Vertex();
            markerGeometry[0].vertData[0].position = new TM64_Geometry.Position();
            markerGeometry[0].vertData[0].position.x = Convert.ToInt16(-5);
            markerGeometry[0].vertData[0].position.y = Convert.ToInt16(0);
            markerGeometry[0].vertData[0].position.z = Convert.ToInt16(0);

            markerGeometry[0].vertData[1] = new TM64_Geometry.Vertex();
            markerGeometry[0].vertData[1].position = new TM64_Geometry.Position();
            markerGeometry[0].vertData[1].position.x = Convert.ToInt16(5);
            markerGeometry[0].vertData[1].position.y = Convert.ToInt16(0);
            markerGeometry[0].vertData[1].position.z = Convert.ToInt16(0);

            markerGeometry[0].vertData[2] = new TM64_Geometry.Vertex();
            markerGeometry[0].vertData[2].position = new TM64_Geometry.Position();
            markerGeometry[0].vertData[2].position.x = Convert.ToInt16(0);
            markerGeometry[0].vertData[2].position.y = Convert.ToInt16(0);
            markerGeometry[0].vertData[2].position.z = Convert.ToInt16(10);


            markerGeometry[1] = new TM64_Geometry.Face();
            markerGeometry[1].vertData = new TM64_Geometry.Vertex[3];

            markerGeometry[1].vertData[0] = new TM64_Geometry.Vertex();
            markerGeometry[1].vertData[0].position = new TM64_Geometry.Position();
            markerGeometry[1].vertData[0].position.x = Convert.ToInt16(0);
            markerGeometry[1].vertData[0].position.y = Convert.ToInt16(-5);
            markerGeometry[1].vertData[0].position.z = Convert.ToInt16(0);

            markerGeometry[1].vertData[1] = new TM64_Geometry.Vertex();
            markerGeometry[1].vertData[1].position = new TM64_Geometry.Position();
            markerGeometry[1].vertData[1].position.x = Convert.ToInt16(0);
            markerGeometry[1].vertData[1].position.y = Convert.ToInt16(5);
            markerGeometry[1].vertData[1].position.z = Convert.ToInt16(0);

            markerGeometry[1].vertData[2] = new TM64_Geometry.Vertex();
            markerGeometry[1].vertData[2].position = new TM64_Geometry.Position();
            markerGeometry[1].vertData[2].position.x = Convert.ToInt16(0);
            markerGeometry[1].vertData[2].position.y = Convert.ToInt16(0);
            markerGeometry[1].vertData[2].position.z = Convert.ToInt16(10);

            treeGeometry[0] = new TM64_Geometry.Face();

            treeGeometry[0].vertData = new TM64_Geometry.Vertex[3];

            treeGeometry[0].vertData[0] = new TM64_Geometry.Vertex();
            treeGeometry[0].vertData[0].position = new TM64_Geometry.Position();
            treeGeometry[0].vertData[0].position.x = Convert.ToInt16(-40);
            treeGeometry[0].vertData[0].position.y = Convert.ToInt16(0);
            treeGeometry[0].vertData[0].position.z = Convert.ToInt16(0);

            treeGeometry[0].vertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[0].vertData[1].position = new TM64_Geometry.Position();
            treeGeometry[0].vertData[1].position.x = Convert.ToInt16(40);
            treeGeometry[0].vertData[1].position.y = Convert.ToInt16(0);
            treeGeometry[0].vertData[1].position.z = Convert.ToInt16(0);

            treeGeometry[0].vertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[0].vertData[2].position = new TM64_Geometry.Position();
            treeGeometry[0].vertData[2].position.x = Convert.ToInt16(40);
            treeGeometry[0].vertData[2].position.y = Convert.ToInt16(0);
            treeGeometry[0].vertData[2].position.z = Convert.ToInt16(80);


            treeGeometry[1] = new TM64_Geometry.Face();
            treeGeometry[1].vertData = new TM64_Geometry.Vertex[3];

            treeGeometry[1].vertData[0] = new TM64_Geometry.Vertex();
            treeGeometry[1].vertData[0].position = new TM64_Geometry.Position();
            treeGeometry[1].vertData[0].position.x = Convert.ToInt16(0);
            treeGeometry[1].vertData[0].position.y = Convert.ToInt16(-40);
            treeGeometry[1].vertData[0].position.z = Convert.ToInt16(0);

            treeGeometry[1].vertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[1].vertData[1].position = new TM64_Geometry.Position();
            treeGeometry[1].vertData[1].position.x = Convert.ToInt16(0);
            treeGeometry[1].vertData[1].position.y = Convert.ToInt16(40);
            treeGeometry[1].vertData[1].position.z = Convert.ToInt16(0);

            treeGeometry[1].vertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[1].vertData[2].position = new TM64_Geometry.Position();
            treeGeometry[1].vertData[2].position.x = Convert.ToInt16(0);
            treeGeometry[1].vertData[2].position.y = Convert.ToInt16(40);
            treeGeometry[1].vertData[2].position.z = Convert.ToInt16(80);

            treeGeometry[2] = new TM64_Geometry.Face();
            treeGeometry[2].vertData = new TM64_Geometry.Vertex[3];

            treeGeometry[2].vertData[0] = new TM64_Geometry.Vertex();
            treeGeometry[2].vertData[0].position = new TM64_Geometry.Position();
            treeGeometry[2].vertData[0].position.x = Convert.ToInt16(-40);
            treeGeometry[2].vertData[0].position.y = Convert.ToInt16(0);
            treeGeometry[2].vertData[0].position.z = Convert.ToInt16(0);

            treeGeometry[2].vertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[2].vertData[1].position = new TM64_Geometry.Position();
            treeGeometry[2].vertData[1].position.x = Convert.ToInt16(40);
            treeGeometry[2].vertData[1].position.y = Convert.ToInt16(0);
            treeGeometry[2].vertData[1].position.z = Convert.ToInt16(80);

            treeGeometry[2].vertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[2].vertData[2].position = new TM64_Geometry.Position();
            treeGeometry[2].vertData[2].position.x = Convert.ToInt16(-40);
            treeGeometry[2].vertData[2].position.y = Convert.ToInt16(0);
            treeGeometry[2].vertData[2].position.z = Convert.ToInt16(80);


            treeGeometry[3] = new TM64_Geometry.Face();
            treeGeometry[3].vertData = new TM64_Geometry.Vertex[3];

            treeGeometry[3].vertData[0] = new TM64_Geometry.Vertex();
            treeGeometry[3].vertData[0].position = new TM64_Geometry.Position();
            treeGeometry[3].vertData[0].position.x = Convert.ToInt16(0);
            treeGeometry[3].vertData[0].position.y = Convert.ToInt16(-40);
            treeGeometry[3].vertData[0].position.z = Convert.ToInt16(0);

            treeGeometry[3].vertData[1] = new TM64_Geometry.Vertex();
            treeGeometry[3].vertData[1].position = new TM64_Geometry.Position();
            treeGeometry[3].vertData[1].position.x = Convert.ToInt16(0);
            treeGeometry[3].vertData[1].position.y = Convert.ToInt16(40);
            treeGeometry[3].vertData[1].position.z = Convert.ToInt16(80);

            treeGeometry[3].vertData[2] = new TM64_Geometry.Vertex();
            treeGeometry[3].vertData[2].position = new TM64_Geometry.Position();
            treeGeometry[3].vertData[2].position.x = Convert.ToInt16(0);
            treeGeometry[3].vertData[2].position.y = Convert.ToInt16(-40);
            treeGeometry[3].vertData[2].position.z = Convert.ToInt16(80);



            piranhaGeometry[0] = new TM64_Geometry.Face();

            piranhaGeometry[0].vertData = new TM64_Geometry.Vertex[3];

            piranhaGeometry[0].vertData[0] = new TM64_Geometry.Vertex();
            piranhaGeometry[0].vertData[0].position = new TM64_Geometry.Position();
            piranhaGeometry[0].vertData[0].position.x = Convert.ToInt16(-30);
            piranhaGeometry[0].vertData[0].position.y = Convert.ToInt16(0);
            piranhaGeometry[0].vertData[0].position.z = Convert.ToInt16(0);

            piranhaGeometry[0].vertData[1] = new TM64_Geometry.Vertex();
            piranhaGeometry[0].vertData[1].position = new TM64_Geometry.Position();
            piranhaGeometry[0].vertData[1].position.x = Convert.ToInt16(30);
            piranhaGeometry[0].vertData[1].position.y = Convert.ToInt16(0);
            piranhaGeometry[0].vertData[1].position.z = Convert.ToInt16(0);

            piranhaGeometry[0].vertData[2] = new TM64_Geometry.Vertex();
            piranhaGeometry[0].vertData[2].position = new TM64_Geometry.Position();
            piranhaGeometry[0].vertData[2].position.x = Convert.ToInt16(30);
            piranhaGeometry[0].vertData[2].position.y = Convert.ToInt16(0);
            piranhaGeometry[0].vertData[2].position.z = Convert.ToInt16(60);


            piranhaGeometry[1] = new TM64_Geometry.Face();
            piranhaGeometry[1].vertData = new TM64_Geometry.Vertex[3];

            piranhaGeometry[1].vertData[0] = new TM64_Geometry.Vertex();
            piranhaGeometry[1].vertData[0].position = new TM64_Geometry.Position();
            piranhaGeometry[1].vertData[0].position.x = Convert.ToInt16(0);
            piranhaGeometry[1].vertData[0].position.y = Convert.ToInt16(-30);
            piranhaGeometry[1].vertData[0].position.z = Convert.ToInt16(0);

            piranhaGeometry[1].vertData[1] = new TM64_Geometry.Vertex();
            piranhaGeometry[1].vertData[1].position = new TM64_Geometry.Position();
            piranhaGeometry[1].vertData[1].position.x = Convert.ToInt16(0);
            piranhaGeometry[1].vertData[1].position.y = Convert.ToInt16(30);
            piranhaGeometry[1].vertData[1].position.z = Convert.ToInt16(0);

            piranhaGeometry[1].vertData[2] = new TM64_Geometry.Vertex();
            piranhaGeometry[1].vertData[2].position = new TM64_Geometry.Position();
            piranhaGeometry[1].vertData[2].position.x = Convert.ToInt16(0);
            piranhaGeometry[1].vertData[2].position.y = Convert.ToInt16(30);
            piranhaGeometry[1].vertData[2].position.z = Convert.ToInt16(60);

            piranhaGeometry[2] = new TM64_Geometry.Face();
            piranhaGeometry[2].vertData = new TM64_Geometry.Vertex[3];

            piranhaGeometry[2].vertData[0] = new TM64_Geometry.Vertex();
            piranhaGeometry[2].vertData[0].position = new TM64_Geometry.Position();
            piranhaGeometry[2].vertData[0].position.x = Convert.ToInt16(-30);
            piranhaGeometry[2].vertData[0].position.y = Convert.ToInt16(0);
            piranhaGeometry[2].vertData[0].position.z = Convert.ToInt16(0);

            piranhaGeometry[2].vertData[1] = new TM64_Geometry.Vertex();
            piranhaGeometry[2].vertData[1].position = new TM64_Geometry.Position();
            piranhaGeometry[2].vertData[1].position.x = Convert.ToInt16(30);
            piranhaGeometry[2].vertData[1].position.y = Convert.ToInt16(0);
            piranhaGeometry[2].vertData[1].position.z = Convert.ToInt16(60);

            piranhaGeometry[2].vertData[2] = new TM64_Geometry.Vertex();
            piranhaGeometry[2].vertData[2].position = new TM64_Geometry.Position();
            piranhaGeometry[2].vertData[2].position.x = Convert.ToInt16(-30);
            piranhaGeometry[2].vertData[2].position.y = Convert.ToInt16(0);
            piranhaGeometry[2].vertData[2].position.z = Convert.ToInt16(60);


            piranhaGeometry[3] = new TM64_Geometry.Face();
            piranhaGeometry[3].vertData = new TM64_Geometry.Vertex[3];

            piranhaGeometry[3].vertData[0] = new TM64_Geometry.Vertex();
            piranhaGeometry[3].vertData[0].position = new TM64_Geometry.Position();
            piranhaGeometry[3].vertData[0].position.x = Convert.ToInt16(0);
            piranhaGeometry[3].vertData[0].position.y = Convert.ToInt16(-30);
            piranhaGeometry[3].vertData[0].position.z = Convert.ToInt16(0);

            piranhaGeometry[3].vertData[1] = new TM64_Geometry.Vertex();
            piranhaGeometry[3].vertData[1].position = new TM64_Geometry.Position();
            piranhaGeometry[3].vertData[1].position.x = Convert.ToInt16(0);
            piranhaGeometry[3].vertData[1].position.y = Convert.ToInt16(30);
            piranhaGeometry[3].vertData[1].position.z = Convert.ToInt16(60);

            piranhaGeometry[3].vertData[2] = new TM64_Geometry.Vertex();
            piranhaGeometry[3].vertData[2].position = new TM64_Geometry.Position();
            piranhaGeometry[3].vertData[2].position.x = Convert.ToInt16(0);
            piranhaGeometry[3].vertData[2].position.y = Convert.ToInt16(-30);
            piranhaGeometry[3].vertData[2].position.z = Convert.ToInt16(60);


            itemGeometry[0] = new TM64_Geometry.Face();

            itemGeometry[0].vertData = new TM64_Geometry.Vertex[3];

            itemGeometry[0].vertData[0] = new TM64_Geometry.Vertex();
            itemGeometry[0].vertData[0].position = new TM64_Geometry.Position();
            itemGeometry[0].vertData[0].position.x = Convert.ToInt16(-8);
            itemGeometry[0].vertData[0].position.y = Convert.ToInt16(0);
            itemGeometry[0].vertData[0].position.z = Convert.ToInt16(0);

            itemGeometry[0].vertData[1] = new TM64_Geometry.Vertex();
            itemGeometry[0].vertData[1].position = new TM64_Geometry.Position();
            itemGeometry[0].vertData[1].position.x = Convert.ToInt16(8);
            itemGeometry[0].vertData[1].position.y = Convert.ToInt16(0);
            itemGeometry[0].vertData[1].position.z = Convert.ToInt16(0);

            itemGeometry[0].vertData[2] = new TM64_Geometry.Vertex();
            itemGeometry[0].vertData[2].position = new TM64_Geometry.Position();
            itemGeometry[0].vertData[2].position.x = Convert.ToInt16(8);
            itemGeometry[0].vertData[2].position.y = Convert.ToInt16(0);
            itemGeometry[0].vertData[2].position.z = Convert.ToInt16(16);


            itemGeometry[1] = new TM64_Geometry.Face();
            itemGeometry[1].vertData = new TM64_Geometry.Vertex[3];

            itemGeometry[1].vertData[0] = new TM64_Geometry.Vertex();
            itemGeometry[1].vertData[0].position = new TM64_Geometry.Position();
            itemGeometry[1].vertData[0].position.x = Convert.ToInt16(0);
            itemGeometry[1].vertData[0].position.y = Convert.ToInt16(-8);
            itemGeometry[1].vertData[0].position.z = Convert.ToInt16(0);

            itemGeometry[1].vertData[1] = new TM64_Geometry.Vertex();
            itemGeometry[1].vertData[1].position = new TM64_Geometry.Position();
            itemGeometry[1].vertData[1].position.x = Convert.ToInt16(0);
            itemGeometry[1].vertData[1].position.y = Convert.ToInt16(8);
            itemGeometry[1].vertData[1].position.z = Convert.ToInt16(0);

            itemGeometry[1].vertData[2] = new TM64_Geometry.Vertex();
            itemGeometry[1].vertData[2].position = new TM64_Geometry.Position();
            itemGeometry[1].vertData[2].position.x = Convert.ToInt16(0);
            itemGeometry[1].vertData[2].position.y = Convert.ToInt16(8);
            itemGeometry[1].vertData[2].position.z = Convert.ToInt16(16);

            itemGeometry[2] = new TM64_Geometry.Face();
            itemGeometry[2].vertData = new TM64_Geometry.Vertex[3];

            itemGeometry[2].vertData[0] = new TM64_Geometry.Vertex();
            itemGeometry[2].vertData[0].position = new TM64_Geometry.Position();
            itemGeometry[2].vertData[0].position.x = Convert.ToInt16(-8);
            itemGeometry[2].vertData[0].position.y = Convert.ToInt16(0);
            itemGeometry[2].vertData[0].position.z = Convert.ToInt16(0);

            itemGeometry[2].vertData[1] = new TM64_Geometry.Vertex();
            itemGeometry[2].vertData[1].position = new TM64_Geometry.Position();
            itemGeometry[2].vertData[1].position.x = Convert.ToInt16(8);
            itemGeometry[2].vertData[1].position.y = Convert.ToInt16(0);
            itemGeometry[2].vertData[1].position.z = Convert.ToInt16(16);

            itemGeometry[2].vertData[2] = new TM64_Geometry.Vertex();
            itemGeometry[2].vertData[2].position = new TM64_Geometry.Position();
            itemGeometry[2].vertData[2].position.x = Convert.ToInt16(-8);
            itemGeometry[2].vertData[2].position.y = Convert.ToInt16(0);
            itemGeometry[2].vertData[2].position.z = Convert.ToInt16(16);


            itemGeometry[3] = new TM64_Geometry.Face();
            itemGeometry[3].vertData = new TM64_Geometry.Vertex[3];

            itemGeometry[3].vertData[0] = new TM64_Geometry.Vertex();
            itemGeometry[3].vertData[0].position = new TM64_Geometry.Position();
            itemGeometry[3].vertData[0].position.x = Convert.ToInt16(0);
            itemGeometry[3].vertData[0].position.y = Convert.ToInt16(-8);
            itemGeometry[3].vertData[0].position.z = Convert.ToInt16(0);

            itemGeometry[3].vertData[1] = new TM64_Geometry.Vertex();
            itemGeometry[3].vertData[1].position = new TM64_Geometry.Position();
            itemGeometry[3].vertData[1].position.x = Convert.ToInt16(0);
            itemGeometry[3].vertData[1].position.y = Convert.ToInt16(8);
            itemGeometry[3].vertData[1].position.z = Convert.ToInt16(16);

            itemGeometry[3].vertData[2] = new TM64_Geometry.Vertex();
            itemGeometry[3].vertData[2].position = new TM64_Geometry.Position();
            itemGeometry[3].vertData[2].position.x = Convert.ToInt16(0);
            itemGeometry[3].vertData[2].position.y = Convert.ToInt16(-8);
            itemGeometry[3].vertData[2].position.z = Convert.ToInt16(16);

        }
        private void colorUpdate()
        {

            int rr = 0;
            int.TryParse(rtbox.Text, out rr);
            int gg = 0;
            int.TryParse(gtbox.Text, out gg);
            int bb = 0;
            int.TryParse(btbox.Text, out bb);
            System.Drawing.Color tbuttoncolor = System.Drawing.Color.FromArgb(rr, gg, bb);



            rr = 0;
            int.TryParse(rbbox.Text, out rr);
            gg = 0;
            int.TryParse(gbbox.Text, out gg);
            bb = 0;
            int.TryParse(bbbox.Text, out bb);
            System.Drawing.Color bbuttoncolor = System.Drawing.Color.FromArgb(rr, gg, bb);

            cptop.BackColor = tbuttoncolor;
            cpbot.BackColor = bbuttoncolor;

        }





        private void compileModel()
        {

            int cID = (cupBox.SelectedIndex * 4) + courseBox.SelectedIndex;
            int setID = setBox.SelectedIndex;

            MessageBox.Show("Select OverKart Patched ROM");
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                string romPath = fileOpen.FileName;

                MessageBox.Show("Please select an output Directory");
                if (folderOpen.ShowDialog() == DialogResult.OK)
                {
                    string outputDirectory = folderOpen.SelectedPath;

                    List<byte[]> Segments = new List<byte[]>();
                    byte[] rom = File.ReadAllBytes(romPath);


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
                    byte[] gameSpeed = new byte[3];

                    Byte.TryParse(sp1Box.Text, out gameSpeed[0]);
                    Byte.TryParse(sp2Box.Text, out gameSpeed[1]);
                    Byte.TryParse(sp3Box.Text, out gameSpeed[2]);



                    //Course Music

                    byte songID = Convert.ToByte(songBox.SelectedIndex);


                    // This command writes all the bitmaps to the end of the ROM

                    rom = tm64Geo.writeTextures(rom, textureArray);
                    segment9 = tm64Geo.compiletextureTable(textureArray);







                    //build segment 7 out of the main course objects and surface geometry
                    //build segment 4 out of the same objects.

                    TM64_Geometry.OK64F3DObject[] textureObjects = masterObjects;
                    byte[] tempBytes = new byte[0];
                    if (levelFormat == 0)
                    {
                        tm64Geo.compileTextureObject(ref segment7, segment7, textureArray, vertMagic);
                        tm64Geo.compileF3DObject(ref vertMagic, ref segment4, ref segment7, segment4, segment7, masterObjects, textureArray, vertMagic);
                        tm64Geo.compileF3DObject(ref vertMagic, ref segment4, ref segment7, segment4, segment7, surfaceObjects, textureArray, vertMagic);


                        // build various segment data
                        

                        renderList = tm64Geo.compileF3DList(ref sectionList, fbx, masterObjects, sectionList, textureArray);


                        popList = tm64Path.popMarkers(popFile);


                        surfaceTable = tm64Geo.compilesurfaceTable(surfaceObjects);

                        magic = (8 + 7968 + 952 + 8 + 528 + (surfaceObjects.Length * 8));
                        // 8 bytes for header
                        // 7968 bytes for the POP data
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

                        displayTable = tm64Geo.compilesectionviewTable(sectionList, magic);





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
                        tm64Geo.compileBattleObject(ref battleOffset, ref vertMagic, ref segment4, ref segment7, segment4, segment7, masterObjects, textureArray, vertMagic);



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

                    byte[] cseg7 = tm64Geo.compress_seg7(segment7);



                    string courseName = nameBox.Text;
                    string previewImage = previewBox.Text;
                    string bannerImage = bannerBox.Text;
                    string mapImage = mapBox.Text;
                    string customASM = asmBox.Text;
                    string ghostData = ghostBox.Text;

                    byte[] skyColor = skyBytes;


                    Int16[] mapCoords = new Int16[2];

                    Int16.TryParse(xBox.Text, out mapCoords[0]);
                    Int16.TryParse(yBox.Text, out mapCoords[1]);


                    byte[] cseg4 = tm64Geo.compressMIO0(segment4);
                    byte[] cseg6 = tm64Geo.compressMIO0(segment6);

                    if (levelFormat == 0)
                    {
                        rom = tm64Geo.compileHotswap(segment4, segment6, segment7, segment9, courseName, previewImage, bannerImage, mapImage, mapCoords, customASM, ghostData, skyColor, songID, gameSpeed, rom, cID, setID);
                    }
                    else
                    {
                        rom = tm64Geo.compileBattle(segment4, segment6, segment7, segment9, courseName, previewImage, bannerImage, mapImage, mapCoords, customASM, skyColor, songID, gameSpeed, rom, cID, setID);

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


                    MessageBox.Show("Finished");
                }
            }
        }


        private void loadModel()
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
                                sectionList = tm64Geo.automateSection(sectionCount, surfaceObjects, masterObjects, fbx, raycastBoolean);
                                break;
                            }
                        case 1:
                            {
                                masterObjects = tm64Geo.loadMaster(ref masterGroups, fbx, textureArray);
                                surfaceObjects = tm64Geo.loadCollision(fbx, sectionCount, textureArray, modelFormat);
                                sectionList = tm64Geo.automateSection(sectionCount, surfaceObjects, masterObjects, fbx, raycastBoolean);
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
                    mcountBox.Text = materialCount.ToString();
                    tcountBox.Text = textureCount.ToString();

                    textureBox.SelectedIndex = 0;
                    lastMaterial = 0;

                }

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





        private void loadBtn_click(object sender, EventArgs e)
        {
            if (loaded)
            {
                compileModel();
            }
            else
            {
                loadModel();
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
                updateSVDisplay();
            }
        }

        private void ViewBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded == true)
            {
                updateSVDisplay();
            }
        }

        private void updateSVDisplay()
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
                formatBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureFormat;
                classBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureClass;
                tBox.Checked = textureArray[textureBox.SelectedIndex].textureTransparent;
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


            textureArray[textureBox.SelectedIndex].textureFormat = formatBox.SelectedIndex;
            tm64Geo.textureClass(textureArray[textureBox.SelectedIndex]);
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
            Byte.TryParse(rtbox.Text, out skyBytes[0]);
            colorUpdate();
        }

        private void Gtbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(gtbox.Text, out skyBytes[1]);
            colorUpdate();
        }

        private void Btbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(btbox.Text, out skyBytes[2]);
            colorUpdate();
        }

        private void Rbbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(rbbox.Text, out skyBytes[3]);
            colorUpdate();
        }

        private void Gbbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(gbbox.Text, out skyBytes[4]);
            colorUpdate();
        }

        private void Bbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(bbbox.Text, out skyBytes[5]);
            colorUpdate();
        }

        private void Cptop_Click(object sender, EventArgs e)
        {
            ColorDialog skyDialog = new ColorDialog();

            skyDialog.AllowFullOpen = true;
            skyDialog.ShowHelp = true;


            int rr = 0;
            int.TryParse(rtbox.Text, out rr);
            int gg = 0;
            int.TryParse(gtbox.Text, out gg);
            int bb = 0;
            int.TryParse(btbox.Text, out bb);
            System.Drawing.Color buttonColor = System.Drawing.Color.FromArgb(rr, gg, bb);

            skyDialog.Color = buttonColor;

            // Update the text box color if the user clicks OK 
            if (skyDialog.ShowDialog() == DialogResult.OK)
            {
                buttonColor = skyDialog.Color;
                rtbox.Text = skyDialog.Color.R.ToString();
                gtbox.Text = skyDialog.Color.G.ToString();
                btbox.Text = skyDialog.Color.B.ToString();
            }
            colorUpdate();

        }

        private void Cpbot_Click(object sender, EventArgs e)
        {
            ColorDialog skyDialog = new ColorDialog();

            skyDialog.AllowFullOpen = true;
            skyDialog.ShowHelp = true;


            int rr = 0;
            int.TryParse(rbbox.Text, out rr);
            int gg = 0;
            int.TryParse(gbbox.Text, out gg);
            int bb = 0;
            int.TryParse(bbbox.Text, out bb);
            System.Drawing.Color buttonColor = System.Drawing.Color.FromArgb(rr, gg, bb);

            skyDialog.Color = buttonColor;

            // Update the text box color if the user clicks OK 
            if (skyDialog.ShowDialog() == DialogResult.OK)
            {
                buttonColor = skyDialog.Color;
                rbbox.Text = skyDialog.Color.R.ToString();
                gbbox.Text = skyDialog.Color.G.ToString();
                bbbox.Text = skyDialog.Color.B.ToString();
            }
            colorUpdate();

        }


        private void PreviewBtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                previewBox.Text = fileOpen.FileName;
            }
        }

        private void BannerBtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                bannerBox.Text = fileOpen.FileName;
            }
        }

        private void MapBtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                mapBox.Text = fileOpen.FileName;
            }
        }

        private void AsmBtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                asmBox.Text = fileOpen.FileName;
            }
        }


        private void ghostbtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                ghostBox.Text = fileOpen.FileName;
            }
        }

        private void tBox_CheckedChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureTransparent = tBox.Checked;
        }






        public byte[] pngtoByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        public byte[] bmptoByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            return ms.ToArray();
        }



        private void drawFace(TM64_Geometry.Face subFace)
        {            
            foreach (var subVert in subFace.vertData)
            {
                gl.Color(subVert.color.r, subVert.color.g, subVert.color.b, Convert.ToByte(0x255));
                gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
            }            
        }
        private void drawMarker(TM64_Geometry.Face[] subFace, TM64_Paths.Marker pathMarker, float[] colorArray)
        {
            foreach (var face in subFace)
            {
                if (colorArray.Length > 3)
                {
                    foreach (var subVert in face.vertData)
                    {
                        gl.Color(colorArray[0], colorArray[1], colorArray[2], colorArray[3]);
                        gl.Vertex(subVert.position.x + pathMarker.xval, subVert.position.y + pathMarker.yval, subVert.position.z + pathMarker.zval);
                    }
                }
                else
                {
                    foreach (var subVert in face.vertData)
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
                foreach (var subVert in subFace.vertData)
                {
                    gl.Color(colorArray[0], colorArray[1], colorArray[2], colorArray[3]);
                    gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
                }
            }
            else
            {
                foreach (var subVert in subFace.vertData)
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
                    flashColor[3] = Convert.ToSingle(flashColor[3] + 0.05);
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
                    flashColor[3] = Convert.ToSingle(flashColor[3] - 0.05);
                }
                else
                {
                    flashColor[4] = 0.0f;
                }
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
                if (flashColor[2] > 0.0)
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
                        float[] surfaceWhite = GetAlphaFlash(flashWhite);
                        float[] surfaceRed = GetAlphaFlash(flashRed);


                        for (int subIndex = 0; subIndex < masterObjects.Length; subIndex++)
                        {
                            
                            if (subIndex == highlightedObject)
                            {
                                if (chkHover.Checked)
                                {
                                    gl.End();
                                    gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                                    gl.Begin(OpenGL.GL_TRIANGLES);
                                    foreach (var subFace in masterObjects[subIndex].modelGeometry)
                                    {
                                        drawFace(subFace, surfaceWhite);
                                    }
                                }
                                else
                                {
                                    if (Array.IndexOf(sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList, subIndex) != -1)
                                    {
                                        gl.End();
                                        gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                                        gl.Begin(OpenGL.GL_TRIANGLES);
                                        foreach (var subFace in masterObjects[subIndex].modelGeometry)
                                        {
                                            drawFace(subFace, masterObjects[subIndex].objectColor);
                                        }
                                    }
                                    else if (chkWireframe.Checked)
                                    {
                                        gl.End();
                                        gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
                                        gl.Begin(OpenGL.GL_TRIANGLES);
                                        foreach (var subFace in masterObjects[subIndex].modelGeometry)
                                        {
                                            drawFace(subFace, masterObjects[subIndex].objectColor);
                                        }

                                    }
                                }
                            }
                            else
                            {

                                if (Array.IndexOf(sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList, subIndex) != -1)
                                {
                                    gl.End();
                                    gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                                    gl.Begin(OpenGL.GL_TRIANGLES);
                                    foreach (var subFace in masterObjects[subIndex].modelGeometry)
                                    {
                                        drawFace(subFace, masterObjects[subIndex].objectColor);
                                    }
                                }
                                else if (chkWireframe.Checked)
                                {
                                    gl.End();
                                    gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
                                    gl.Begin(OpenGL.GL_TRIANGLES);
                                    foreach (var subFace in masterObjects[subIndex].modelGeometry)
                                    {
                                        drawFace(subFace, masterObjects[subIndex].objectColor);
                                    }

                                }
                            }
                            
                        }


                        gl.End();
                        gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                        gl.Begin(OpenGL.GL_TRIANGLES);

                        if (chkSection.Checked)
                        {
                            foreach (var subObject in surfaceObjects)
                            {
                                if (subObject.surfaceID == (sectionBox.SelectedIndex + 1))
                                {
                                    foreach (var subFace in subObject.modelGeometry)
                                    {
                                        drawFace(subFace, surfaceRed);
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        foreach (var subObject in surfaceObjects)
                        {
                            foreach (var subFace in subObject.modelGeometry)
                            {
                                drawFace(subFace, subObject.objectColor);
                            }
                        }
                        break;
                    }
                default:
                    {
                        foreach (var subObject in masterObjects)
                        {
                            foreach (var subFace in subObject.modelGeometry)
                            {
                                drawFace(subFace, subObject.objectColor);
                            }
                        }
                        break;
                    }
            }

            if (levelFormat == 0)
            {
                foreach (var list in popData[0].pathList)
                {
                    float[] surfaceYellow = GetYellowFlash(flashYellow);
                    foreach (var marker in list.pathmarker)
                    {
                        drawMarker(markerGeometry, marker, surfaceYellow);
                    }
                }
                foreach (var list in popData[1].pathList)
                {
                    float[] surfaceYellow = GetYellowFlash(flashYellow);
                    foreach (var marker in list.pathmarker)
                    {
                        drawMarker(itemGeometry, marker, surfaceYellow);
                    }
                }
                foreach (var list in popData[2].pathList)
                {
                    float[] surfaceYellow = GetYellowFlash(flashYellow);
                    foreach (var marker in list.pathmarker)
                    {
                        drawMarker(treeGeometry, marker, surfaceYellow);
                    }
                }
                foreach (var list in popData[3].pathList)
                {
                    float[] surfaceYellow = GetYellowFlash(flashYellow);
                    foreach (var marker in list.pathmarker)
                    {
                        drawMarker(piranhaGeometry, marker, surfaceYellow);
                    }
                }
            }
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

        private void openGLControl_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
            int objectID = -1;
            cName.Text = "";
            for (int currentObject = 0; (currentObject < masterObjects.Length); currentObject++)
            {

                foreach (var face in masterObjects[currentObject].modelGeometry)
                {

                    Vector3D intersectPoint = tmGeo.testIntersect(rayOrigin, rayTarget, face.vertData[0], face.vertData[1], face.vertData[2]);
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
            switch (tabControl1.SelectedIndex)
            {
                default:
                    {
                        break;
                    }
                case 2:
                    {
                        
                        if (objectID > -1)
                        {
                            var SelectedNodes = masterBox.Nodes.Find(masterObjects[objectID].objectName, true);
                            masterBox.SelectedNode = SelectedNodes[0];
                            if (e.Button == MouseButtons.Right)
                            {
                                SelectedNodes[0].Checked = !SelectedNodes[0].Checked;
                                updateSectionList(masterObjects[objectID].objectName);
                            }
                        }
                        
                        break;
                    }
            }
        }

        


        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFile.FileName;

                TM64_Geometry tm64Geo = new TM64_Geometry();
                tm64Geo.ExportSVL(filePath, masterObjects.Length, sectionList, masterObjects);                
            }
        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFile.FileName;

                TM64_Geometry tm64Geo = new TM64_Geometry();
                TM64_Geometry.OK64SectionList[] tempList = tm64Geo.ImportSVL(filePath, masterObjects.Length, masterObjects);
                if (tempList.Length > 0)
                {
                    sectionList = tempList;
                    updateSVDisplay();

                }
                else
                {
                    MessageBox.Show("Error! Incorrect Object Count");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            foreach (var list in popData[0].pathList)
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

                    Vector3D intersectPoint = tmGeo.testIntersect(rayOrigin, rayTarget, face.vertData[0], face.vertData[1], face.vertData[2]);
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
        }

        private void masterBox_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                List<int> checkList = new List<int>();
                bool checkState = e.Node.Checked;
                checkList = sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList.ToList();
                int vertCount = 0, faceCount = 0;
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
                    
                    updateCounter(faceCount);
                }
            }
        }
    }
}





