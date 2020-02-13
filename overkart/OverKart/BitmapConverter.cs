using System;
using System.Windows;
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
using System.Windows.Media.Imaging;
using System.Globalization;
using Texture64;


namespace OverKart64
{


    public partial class BitmapConverter : Form
    {

        bool loaded = false;

        FileStream fs = new FileStream("null", FileMode.OpenOrCreate);
        MemoryStream bs = new MemoryStream();
        BinaryReader br = new BinaryReader(Stream.Null);
        BinaryWriter bw = new BinaryWriter(Stream.Null);
        MemoryStream ds = new MemoryStream();
        BinaryReader dr = new BinaryReader(Stream.Null);
        BinaryWriter dw = new BinaryWriter(Stream.Null);
        MemoryStream vs = new MemoryStream();
        BinaryReader vr = new BinaryReader(Stream.Null);


        List<int> ltexclass = new List<int>();
        int[] texclass = new int[0];

        List<UInt32> ltexoffset = new List<UInt32>();
        UInt32[] texoffset = new UInt32[0];


        bool debugmode = false;

        OK64 mk = new OK64();

        string filePath = "";
        string savePath = "";

        byte[] flip4 = new byte[4];
        byte[] flip2 = new byte[2];

        int[] img_types = new int[] { 0, 0, 0, 3, 3, 3, 0 };        
        int[] img_heights = new int[] { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 };
        int[] img_widths = new int[] { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 };

        string[] img_types_string = new string[] { "RGBA16", "RGBA16", "RGBA16", "IA", "IA", "IA", "RGBA16" };

        int v0 = 0;
        int v1 = 0;
        int v2 = 0;


        int current_offset = 0;


        int texwidth = 0;
        int x_mask = 0;
        int x_flags = 0;
        int texheight = 0;
        int y_mask = 0;
        int y_flags = 0;
        int textype = 0;


        UInt16 value16 = new UInt16();
        UInt32 value32 = new UInt32();
        Int16 valuesign16 = new Int16();

        UInt32 seg7_addr = new UInt32();

        public static int cID = 0;

        public static UInt32[] seg6_addr = new UInt32[20];      //(0x00) ROM address at which segment 6 file begins
        public static UInt32[] seg6_end = new UInt32[20];       //(0x04) ROM address at which segment 6 file ends
        public static UInt32[] seg4_addr = new UInt32[20];      //(0x08) ROM address at which segment 4 file begins
        public static UInt32[] seg7_end = new UInt32[20];       //(0x0C) ROM address at which segment 7 (not 4) file ends
        public static UInt32[] seg9_addr = new UInt32[20];      //(0x10) ROM address at which segment 9 file begins
        public static UInt32[] seg9_end = new UInt32[20];       //(0x14) ROM address at which segment 9 file ends
        public static UInt32[] seg47_buf = new UInt32[20];      //(0x18) RSP address of compressed segments 4 and 7
        public static UInt32[] numVtxs = new UInt32[20];        //(0x1C) number of vertices in the vertex file
        public static UInt32[] seg7_ptr = new UInt32[20];       //(0x20) RSP address at which segment 7 data begins
        public static UInt32[] seg7_size = new UInt32[20];      //(0x24) Size of segment 7 data after decompression, minus 8 bytes for some reason
        public static UInt32[] texture_addr = new UInt32[20];   //(0x28) RSP address of texture list
        public static UInt16[] flag = new UInt16[20];           //(0x2C) Unknown
        public static UInt16[] unused = new UInt16[20];         //(0x2E) Padding

        public static UInt32[] seg7_romptr = new UInt32[20];    // math derived from above data.

        OpenFileDialog vertopen = new OpenFileDialog();
        SaveFileDialog vertsave = new SaveFileDialog();
        FolderBrowserDialog textsave = new FolderBrowserDialog();

        OK64 ok64 = new OK64();



        public BitmapConverter()
        {
            InitializeComponent();
        }


        private void convert_click(object sender, EventArgs e)
        {


            N64Codec[] selected_codex = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8};

            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                if (textsave.ShowDialog() == DialogResult.OK)
                {
                    string outpath = textsave.SelectedPath;
                    AssimpSharp.Scene fbx = new AssimpSharp.Scene();
                    savePath = vertopen.FileName;
                    var assimpSharpImporter = new AssimpSharp.FBX.FBXImporter();
                    fbx = new AssimpSharp.Scene();
                    fbx = assimpSharpImporter.ReadFile(savePath);

                    int material_count = fbx.Materials.Count;
                    
                    for (int tIndex = 0; tIndex < material_count; tIndex++)
                    {
                        string filepath = fbx.Materials[tIndex].TextureDiffuse.TextureBase;
                        string filename = (tIndex+1).ToString() + ".bin";

                        Bitmap bm = new Bitmap(filepath);
                        byte[] imageData = null, paletteData = null;

                        N64Graphics.Convert(ref imageData, ref paletteData, selected_codex[codex.SelectedIndex], bm);

                        savePath = Path.Combine(outpath, filename);
                        File.WriteAllBytes(savePath, imageData);

                        
                    }
                    MessageBox.Show("Finished");
                }
            }
        }


        private void TextureLists_Load(object sender, EventArgs e)
        {
            codex.SelectedIndex = 0;
        }

    }
}
