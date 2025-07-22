using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.IO;
using Tarmac64_Library;
using System.Runtime;
using static Tarmac64_Library.TM64_Course;
using F3DSharp;
using Aspose.ThreeD;

namespace Tarmac64_Retail.Windows
{
    public partial class DebugTools : Form
    {
        /*
            0x00 Mario Raceway 	0x9650
            0x01 Choco Mountain 	0x72D0
            0x02 Bowser's Castle 	0x93D8
            0x03 Banshee Boardwalk 	0xB458
            0x04 Yoshi Valley 	0x18240
            0x05 Frappe Snowland 	0x79A0
            0x06 Koopa Troopa Beach 	0x18FD8
            0x07 Royal Raceway 	0xDC28
            0x08 Luigi Raceway 	0xFF28
            0x09 Moo Moo Farm 	0x144B8
            0x0A Toad's Turnpike 	0x23B68
            0x0B Kalimari Desert 	0x23070
            0x0C Sherbet Land 	0x9C20
            0x0D Rainbow Road 	0x16440
            0x0E Wario Stadium 	0xCC38
        */
        string[] CourseNames =
        {
            "Mario Raceway",
            "Choco Mountain",
            "Bowser's Castle",
            "Banshee Boardwalk",
            "Yoshi Valley",
            "Frappe Snowlan",
            "Koopa Troopa Beach",
            "Royal Raceway",
            "Luigi Raceway",
            "Moo Moo Farm",
            "Toad's Turnpike",
            "Kalimari Desert",
            "Sherbet Land",
            "Rainbow Road",
            "Wario Stadium",
            "Block Fort",
            "Double Decker",
            "DK's Jungle Parkway",
            "Big Donut",                
        };
        uint[] SegmentTable =
        {
                0x122390,
        };
        uint[] SurfaceMapOffsets =
        {
            0x9650,
            0x72D0,
            0x93D8,
            0xB458,
            0x18240,
            0x79A0,
            0x18FD8,
            0xDC28,
            0xFF28,
            0x144B8,
            0x23B68,
            0x23070,
            0x9C20,
            0x16440,
            0xCC38,
            0x0,
            0x0,
            0x0,
            0x14338,
            0x0,
        };

        public DebugTools()
        {
            InitializeComponent();
        }

        private void DebugTools_Load(object sender, EventArgs e)
        {
            foreach (var Name in CourseNames)
            {
                CourseBox.Items.Add(Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uint Surf = SurfaceMapOffsets[CourseBox.SelectedIndex];

            TM64 Tarmac = new TM64();
            TM64_Geometry TarmacGeo = new TM64_Geometry();
            F3DEX095 F3D = new F3DEX095();

            OpenFileDialog FileOpen = new OpenFileDialog();
            MessageBox.Show("Open ROM");

            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(FileOpen.FileName))
                {
                    string FileName = FileOpen.FileName;
                    byte[] Data = File.ReadAllBytes(FileName);
                    uint TableAddress = SegmentTable[0];

                    MemoryStream memoryStream = new MemoryStream();
                    BinaryReader binaryReader = new BinaryReader(memoryStream);

                    memoryStream.Write(Data, 0, Data.Length);
                    memoryStream.Position = 0;

                    binaryReader.BaseStream.Position = TableAddress;
                    TM64_Course.CourseHeader Header = new TM64_Course.CourseHeader();
                    binaryReader.BaseStream.Position += (CourseBox.SelectedIndex * 0x30);

                    Header.s6Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    Header.s6End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    Header.s47Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    Header.s47End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    Header.s9Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    Header.s9End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    binaryReader.BaseStream.Position += 4; //0x0F000000
                    Header.VertCount = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    Header.s7Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    Header.S7Size = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    Header.TexturePointer = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);

                    binaryReader.BaseStream.Position = Header.s6Start;
                    int S6Length = Convert.ToInt32(Header.s6End - Header.s6Start);
                    byte[] Segment6 = Tarmac.DecompressMIO0(binaryReader.ReadBytes((int)S6Length));

                    binaryReader.BaseStream.Position = Header.s47Start;
                    int S4Length = Convert.ToInt32(Header.s7Start - 0x0F000000);

                    byte[] Segment4Data = binaryReader.ReadBytes(S4Length);
                    byte[] Segment4 = TarmacGeo.InflateVertex(Tarmac.DecompressMIO0(Segment4Data));

                    binaryReader.BaseStream.Position = Header.s47Start + Convert.ToInt32(Header.s7Start - 0x0F000000);
                    int S7Length = Convert.ToInt32(Header.S7Size);
                    byte[] Segment7Data = binaryReader.ReadBytes(S7Length);
                    byte[] Segment7 = Tarmac.Decompress_seg7(Segment7Data);


                    TarmacGeo.ExportSurfaceMap(Segment4, Segment6, Segment7, SurfaceMapOffsets[CourseBox.SelectedIndex]);

                }
            }
        }
    }
}
