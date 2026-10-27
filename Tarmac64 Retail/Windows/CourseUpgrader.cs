using Cereal64.Microcodes.F3DEX.DataElements;
using F3DSharp;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tarmac64_Library;

namespace Tarmac64_Retail.Windows
{
    public partial class CourseUpgrader : Form
    {
        public CourseUpgrader()
        {
            InitializeComponent();
        }


        public struct V4
        {
            public int Version { get; set; }
            public TM64_Course.CourseHeader HeaderData { get; set; }
            public int Sky { get; set; }
            public int Credits { get; set; }
            public int Ghost { get; set; }
            public int Assembly { get; set; }
            public int AssemblyEnd { get; set; }
            public int Maps { get; set; }
            public int Objects { get; set; }
            public short EchoStart { get; set; }
            public short EchoStop { get; set; }
            public int[] Tempo { get; set; }
            public int MusicID { get; set; }
            public int Unused17 { get; set; }
            public float WaterHeight { get; set; }

            public V4(BinaryReader binaryReader)
            {
                F3DEX095 F3D = new F3DEX095();
                Version = 4;
                HeaderData = new TM64_Course.CourseHeader();
                HeaderData.s6Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.s6End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.s47Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.s47End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.s9Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.s9End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                binaryReader.BaseStream.Position += 4;
                HeaderData.VertCount = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.s7Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.S7Size = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.TexturePointer = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                binaryReader.BaseStream.Position += 4;

                Sky = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Credits = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Ghost = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Assembly = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                AssemblyEnd = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Maps = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Objects = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                EchoStart = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                EchoStop = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                Tempo = new int[4];
                Tempo[0] = binaryReader.ReadByte();
                Tempo[1] = binaryReader.ReadByte();
                Tempo[2] = binaryReader.ReadByte();
                Tempo[3] = binaryReader.ReadByte();
                MusicID = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Unused17 = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                byte[] waterBytes = F3D.BigEndian(binaryReader.ReadInt32());
                int waterBits = BitConverter.ToInt32(waterBytes, 0);
                WaterHeight = BitConverter.ToSingle(BitConverter.GetBytes(waterBits), 0);
            }
        }
        public struct V5
        {
            public int Version { get; set; }
            public TM64_Course.CourseHeader HeaderData { get; set; }
            public int SectionViewPosition { get; set; }
            public int SurfaceMapPosition { get; set; }
            public int SkyPosition { get; set; }
            public short SkyType { get; set; }
            public short WeatherType { get; set; }
            public int Credits { get; set; }
            public int CourseName { get; set; }
            public int SerialKey { get; set; }
            public int Ghost { get; set; }
            public int Maps { get; set; }
            public int ObjectStart { get; set; }
            public int ObjectModelStart { get; set; }
            public int ObjectEnd { get; set; }
            public int BombOffset { get; set; }
            public int EchoOffset { get; set; }
            public int EchoEnd { get; set; }
            public int[] Tempo { get; set; }
            public int MusicID { get; set; }
            public int PathLength { get; set; }
            public short WaterType { get; set; }
            public short WaterLevel { get; set; }
            public int ScrollStart { get; set; }
            public int ScrollEnd { get; set; }
            public int Sky { get; set; }

            public V5(BinaryReader binaryReader)
            {

                F3DEX095 F3D = new F3DEX095();
                Version = 5;

                HeaderData = new TM64_Course.CourseHeader();
                
                    HeaderData.s6Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    HeaderData.s6End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);

                    HeaderData.s47Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    HeaderData.s47End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);

                    

                    HeaderData.s9Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    HeaderData.s9End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);

                    binaryReader.BaseStream.Position += 4; //0x0F000000

                    HeaderData.VertCount = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    HeaderData.s7Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    
                    HeaderData.S7Size = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);

                    HeaderData.TexturePointer = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    binaryReader.BaseStream.Position += 4;


                SectionViewPosition = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                
                SurfaceMapPosition = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                SkyPosition = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                SkyType = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                WeatherType = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                Credits = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                CourseName = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                SerialKey = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Ghost = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Maps = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                ObjectStart = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                ObjectModelStart = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                ObjectEnd = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                BombOffset = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                EchoOffset = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                EchoEnd = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);

                Tempo = new int[4];
                Tempo[0] = binaryReader.ReadSByte();
                Tempo[1] = binaryReader.ReadSByte();
                Tempo[2] = binaryReader.ReadSByte();
                Tempo[3] = binaryReader.ReadSByte();

                MusicID = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                PathLength = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);

                WaterType = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                WaterLevel = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);

                ScrollStart = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                ScrollEnd = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Sky = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);

            }
        }


        public struct V51
        {
            public int Version { get; set; }
            public TM64_Course.CourseHeader HeaderData { get; set; }
            public int SectionViewPosition { get; set; }
            public int XLUViewPosition { get; set; }
            public int SurfaceMapPosition { get; set; }
            public int SkyPosition { get; set; }
            public short SkyType { get; set; }
            public short WeatherType { get; set; }
            public int Credits { get; set; }
            public int CourseName { get; set; }
            public int SerialKey { get; set; }
            public int Ghost { get; set; }
            public int Maps { get; set; }
            public int ObjectStart { get; set; }
            public int ObjectModelStart { get; set; }
            public int ObjectEnd { get; set; }
            public int BombOffset { get; set; }
            public int EchoOffset { get; set; }
            public int EchoEnd { get; set; }
            public int[] Tempo { get; set; }
            public int MusicID { get; set; }
            public int PathLength { get; set; }
            public short[] PathLengths { get; set; }
            public int PathOffset { get; set; }
            public short WaterType { get; set; }
            public short WaterLevel { get; set; }
            public float WaterHeight { get; set; }
            public int ScrollStart { get; set; }
            public int ScrollEnd { get; set; }
            public int Sky { get; set; }

            public uint ROMVersion { get; set; }

            public V51(V4 source, uint version)
            {
                Version = 5;
                ROMVersion = version;
                HeaderData = source.HeaderData;
                SectionViewPosition = 0x2328;
                XLUViewPosition = 0;
                SurfaceMapPosition = 0x2538;
                SkyPosition = source.Sky;
                SkyType = 0;
                WeatherType = 0;
                Credits = source.Credits;
                CourseName = 0;
                SerialKey = 0;
                Ghost = source.Ghost;
                Maps = source.Maps;
                ObjectStart = source.Objects;
                ObjectModelStart = 0;
                ObjectEnd = 0;
                BombOffset = 0;
                EchoOffset = source.EchoStart;
                EchoEnd = source.EchoStop;
                Tempo = source.Tempo;
                MusicID = source.MusicID;
                PathLength = 1;
                PathLengths = new short[4] { 1, 1, 1, 1 };
                PathOffset = 0;
                WaterType = 0;
                WaterLevel = 0;
                WaterHeight = source.WaterHeight;
                ScrollStart = 0;
                ScrollEnd = 0;
                Sky = 0;
            }

            public V51(BinaryReader binaryReader)
            {

                F3DEX095 F3D = new F3DEX095();
                Version = 5;
                ROMVersion = 5;

                HeaderData = new TM64_Course.CourseHeader();

                HeaderData.s6Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.s6End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);

                HeaderData.s47Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.s47End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);



                HeaderData.s9Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.s9End = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);

                binaryReader.BaseStream.Position += 4; //0x0F000000

                HeaderData.VertCount = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                HeaderData.s7Start = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);

                HeaderData.S7Size = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);

                HeaderData.TexturePointer = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);

                binaryReader.BaseStream.Position += 4;

                SectionViewPosition = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                XLUViewPosition = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                SurfaceMapPosition = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                SkyPosition = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                SkyType = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                WeatherType = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                Credits = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                CourseName = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                SerialKey = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Ghost = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Maps = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                ObjectStart = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                ObjectModelStart = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                ObjectEnd = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                BombOffset = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                EchoOffset = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                EchoEnd = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);

                Tempo = new int[4];
                Tempo[0] = binaryReader.ReadSByte();
                Tempo[1] = binaryReader.ReadSByte();
                Tempo[2] = binaryReader.ReadSByte();
                Tempo[3] = binaryReader.ReadSByte();

                MusicID = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                PathLengths = new short[4];
                PathLengths[0] = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                PathLengths[1] = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                PathLengths[2] = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                PathLengths[3] = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                PathLength = PathLengths[0];

                WaterType = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                WaterLevel = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);

                ScrollStart = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                short wvOffset = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                short screenOffset = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                short kdOffset = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                short scrollSize = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                PathOffset = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                ScrollEnd = ScrollStart + scrollSize;
                Sky = 0;
                WaterHeight = WaterLevel;

            }
        }
        private void CourseUpgrader_Load(object sender, EventArgs e)
        {
            ButtonSave.Enabled = false;
            ComboBoxCourses.Items.Clear();
        
        }
        V51[] V51Array = new V51[80];
        V5[] V5Array = new V5[80];
        V4[] V4Array = new V4[80];
        List<int> courseSlots = new List<int>();
        string loadedRomPath = "";
        TM64_Course.Course PreparedCourse = null;
        TM64_Paths.Pathlist[] ExtractedPaths = new TM64_Paths.Pathlist[0];
        
        private void button1_Click(object sender, EventArgs e)
        {
            ButtonSave.Enabled = false;
            ComboBoxCourses.Items.Clear();

            V51Array = new V51[80];
            V5Array = new V5[80];
            V4Array = new V4[80];
            courseSlots = new List<int>();
            PreparedCourse = null;
            ExtractedPaths = new TM64_Paths.Pathlist[0];
            OpenFileDialog FileOpen = new OpenFileDialog();
            if (FileOpen.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string FileName = FileOpen.FileName;
            if (!File.Exists(FileName))
            {
                return;
            }

            loadedRomPath = FileName;
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            F3DEX095 F3D = new F3DEX095();
            byte[] Data = File.ReadAllBytes(FileName);
            memoryStream.Write(Data, 0, Data.Length);
            memoryStream.Position = 0;

            binaryReader.BaseStream.Position = 0xBE9178;
            for (int ThisSet = 0; ThisSet < 5; ThisSet++)
            {
                for (int ThisCourse = 0; ThisCourse < 16; ThisCourse++)
                {
                    int Index = ((ThisSet * 20) + ThisCourse);
                    int OffsetOffset = 0xBE9178 + (Index * 4);
                    binaryReader.BaseStream.Position = OffsetOffset;

                    uint HeaderOffset = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    if (HeaderOffset == Convert.ToUInt32(0xFFFFFFFF))
                    {
                        continue;
                    }

                    binaryReader.BaseStream.Position = HeaderOffset;

                    uint VersionNumber = BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
                    V51 parsed;
                    if (VersionNumber <= 5)
                    {
                        V4 legacy = new V4(binaryReader);
                        parsed = new V51(legacy, VersionNumber);
                    }
                    else
                    {
                        parsed = new V51(binaryReader);
                        parsed.ROMVersion = VersionNumber;
                    }
                    V51Array[Index] = parsed;
                    courseSlots.Add(Index);

                    string Name = "Course " + Index.ToString();
                    int namePointer = parsed.CourseName;
                    if ((namePointer == 0) || (namePointer == unchecked((int)0xDEADBEEF)))
                    {
                        namePointer = parsed.Credits;
                    }
                    if (IsValidPointer(namePointer, Data.Length))
                    {
                        binaryReader.BaseStream.Position = namePointer;
                        int Length = ReadInt32BE(binaryReader, F3D);
                        if ((Length > 0) && (Length < 256) && ((namePointer + 4 + Length) <= Data.Length))
                        {
                            Name = Encoding.UTF8.GetString(binaryReader.ReadBytes(Length));
                        }
                    }
                    ComboBoxCourses.Items.Add(Name);
                }
            }
            binaryReader.Close();
            memoryStream.Close();

            ButtonSave.Enabled = true;
        }

        private void WriteV5()
        {
            int slot = GetSelectedSlot();
            if (slot < 0)
            {
                return;
            }
            V51 Course = V51Array[slot];
            TextBoxItem.Text = "";
            TextBoxValue.Text = "";
            List<string> Items = new List<string>();
            List<string> Values = new List<string>();

            TextBoxItem.Text += nameof(Course.Version) + Environment.NewLine;
            TextBoxValue.Text += Course.Version + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.HeaderData.s6Start) + Environment.NewLine;
            TextBoxValue.Text += Course.HeaderData.s6Start.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.HeaderData.s6End) + Environment.NewLine;
            TextBoxValue.Text += Course.HeaderData.s6End.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.HeaderData.s47Start) + Environment.NewLine;
            TextBoxValue.Text += Course.HeaderData.s47Start.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.HeaderData.s47End) + Environment.NewLine;
            TextBoxValue.Text += Course.HeaderData.s47End.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.HeaderData.s9Start) + Environment.NewLine;
            TextBoxValue.Text += Course.HeaderData.s9Start.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.HeaderData.s9End) + Environment.NewLine;
            TextBoxValue.Text += Course.HeaderData.s9End.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.HeaderData.VertCount) + Environment.NewLine;
            TextBoxValue.Text += Course.HeaderData.VertCount.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.HeaderData.s7Start) + Environment.NewLine;
            TextBoxValue.Text += Course.HeaderData.s7Start.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.HeaderData.S7Size) + Environment.NewLine;
            TextBoxValue.Text += Course.HeaderData.S7Size.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.HeaderData.TexturePointer) + Environment.NewLine;
            TextBoxValue.Text += Course.HeaderData.TexturePointer.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.SectionViewPosition) + Environment.NewLine;
            TextBoxValue.Text += Course.SectionViewPosition.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.XLUViewPosition) + Environment.NewLine;
            TextBoxValue.Text += Course.XLUViewPosition.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.SurfaceMapPosition) + Environment.NewLine;
            TextBoxValue.Text += Course.SurfaceMapPosition.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.SkyPosition) + Environment.NewLine;
            TextBoxValue.Text += Course.SkyPosition.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.SkyType) + Environment.NewLine;
            TextBoxValue.Text += Course.SkyType.ToString() + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.WeatherType) + Environment.NewLine;
            TextBoxValue.Text += Course.WeatherType.ToString() + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.Credits) + Environment.NewLine;
            TextBoxValue.Text += Course.Credits.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.CourseName) + Environment.NewLine;
            TextBoxValue.Text += Course.CourseName.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.SerialKey) + Environment.NewLine;
            TextBoxValue.Text += Course.SerialKey.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.Ghost) + Environment.NewLine;
            TextBoxValue.Text += Course.Ghost.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.Maps) + Environment.NewLine;
            TextBoxValue.Text += Course.Maps.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.ObjectStart) + Environment.NewLine;
            TextBoxValue.Text += Course.ObjectStart.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.ObjectEnd) + Environment.NewLine;
            TextBoxValue.Text += Course.ObjectEnd.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.BombOffset) + Environment.NewLine;
            TextBoxValue.Text += Course.BombOffset.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.EchoOffset) + Environment.NewLine;
            TextBoxValue.Text += Course.EchoOffset.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.EchoEnd) + Environment.NewLine;
            TextBoxValue.Text += Course.EchoEnd.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.Tempo) + Environment.NewLine;
            TextBoxItem.Text += nameof(Course.Tempo) + Environment.NewLine;
            TextBoxItem.Text += nameof(Course.Tempo) + Environment.NewLine;
            TextBoxItem.Text += nameof(Course.Tempo) + Environment.NewLine;
            TextBoxValue.Text += Course.Tempo[0].ToString() + Environment.NewLine;
            TextBoxValue.Text += Course.Tempo[1].ToString() + Environment.NewLine;
            TextBoxValue.Text += Course.Tempo[2].ToString() + Environment.NewLine;
            TextBoxValue.Text += Course.Tempo[3].ToString() + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.MusicID) + Environment.NewLine;
            TextBoxValue.Text += Course.MusicID.ToString() + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.PathLength) + Environment.NewLine;
            TextBoxValue.Text += Course.PathLength.ToString() + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.WaterType) + Environment.NewLine;
            TextBoxValue.Text += Course.WaterType.ToString() + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.WaterLevel) + Environment.NewLine;
            TextBoxValue.Text += Course.WaterLevel.ToString() + Environment.NewLine;
            TextBoxItem.Text += nameof(Course.WaterHeight) + Environment.NewLine;
            TextBoxValue.Text += Course.WaterHeight.ToString() + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.ScrollStart) + Environment.NewLine;
            TextBoxValue.Text += Course.ScrollStart.ToString("X") + Environment.NewLine;
            TextBoxItem.Text += nameof(Course.ScrollEnd) + Environment.NewLine;
            TextBoxValue.Text += Course.ScrollEnd.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.Sky) + Environment.NewLine;
            TextBoxValue.Text += Course.Sky.ToString("X") + Environment.NewLine;
        }
        private void ComboBoxCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            int slot = GetSelectedSlot();
            if (slot < 0)
            {
                return;
            }

            WriteV5();
            DisplaySegmentSizes(null, V51Array[slot]);

            if (string.IsNullOrEmpty(loadedRomPath) || !File.Exists(loadedRomPath))
            {
                return;
            }

            try
            {
                byte[] rom = File.ReadAllBytes(loadedRomPath);
                DisplaySegmentSizes(rom, V51Array[slot]);
                PreparedCourse = PrepareCourseFromROM(rom, V51Array[slot], ComboBoxCourses.Text, slot);
                if (PreparedCourse == null)
                {
                    return;
                }
                ApplyPreparedCourseToUI(PreparedCourse, V51Array[slot]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to prepare course: " + ex.Message);
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if (PreparedCourse == null)
            {
                MessageBox.Show("Select a course first.");
                return;
            }

            courseSettings1.UpdateCourse();
            pathSettings1.UpdatePaths();
            PreparedCourse = courseSettings1.UpdateCourse(PreparedCourse);
            PreparedCourse = pathSettings1.UpdateCourse(PreparedCourse);

            PreparedCourse.OK64HeaderData.Version = 6;
            PreparedCourse.OK64HeaderData.GoalBannerToggle = Convert.ToByte(PreparedCourse.GoalBannerBool);
            PreparedCourse.OK64HeaderData.SkyboxToggle = Convert.ToByte(PreparedCourse.SkyboxBool);
            PreparedCourse.OK64HeaderData.SectionViewPosition = ParseSeg6Position(TextBoxSectionView.Text);
            PreparedCourse.OK64HeaderData.XLUViewPosition = ParseSeg6Position(TextBoxXLUView.Text);
            PreparedCourse.OK64HeaderData.SurfaceMapPosition = ParseSeg6Position(TextBoxSurfaceMap.Text);
            EnsureCourseForSave(PreparedCourse);

            SaveFileDialog FileSave = new SaveFileDialog();
            FileSave.Filter = "Tarmac Course|*.ok64.Course|All Files (*.*)|*.*";
            FileSave.DefaultExt = "ok64.Course";
            FileSave.FileName = PreparedCourse.Settings.Name;
            if (FileSave.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            TM64_Course TarmacCourse = new TM64_Course();
            File.WriteAllBytes(FileSave.FileName, TarmacCourse.SaveOK64Course(PreparedCourse));
            MessageBox.Show("Saved " + FileSave.FileName);
        }

        private void ExportV5(string FolderPath, string ROM)
        {
            TM64 Tarmac = new TM64();
            TM64_Geometry TarmacGeo = new TM64_Geometry();
            
            F3DEX095 F3D = new F3DEX095();
            V51 Course = V51Array[ComboBoxCourses.SelectedIndex];

            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            byte[] FileData = File.ReadAllBytes(ROM);
            memoryStream.Write(FileData, 0, FileData.Length);
            memoryStream.Position = 0;

            string Target;
            int Offset, DataLength;

            binaryReader.BaseStream.Position = Course.HeaderData.s6Start;
            DataLength = Convert.ToInt32(Course.HeaderData.s6End - Course.HeaderData.s6Start);
            Target = Path.Combine(FolderPath, "Segment6.bin");
            File.WriteAllBytes(Target, Tarmac.DecompressMIO0(binaryReader.ReadBytes(DataLength)));

            binaryReader.BaseStream.Position = Course.HeaderData.s9Start;
            DataLength = Convert.ToInt32(Course.HeaderData.s9End - Course.HeaderData.s9Start);
            Target = Path.Combine(FolderPath, "Segment9.bin");
            File.WriteAllBytes(Target, binaryReader.ReadBytes(DataLength));

            

            Offset = 0xFFFF;
            int Loop = 0;
            int addressAlign;
            List<byte> TextureArrayBytes = new List<byte>();
            while (Offset != 0)
            {
                binaryReader.BaseStream.Position = Course.HeaderData.s9Start + (Loop * 16);
                
                Offset = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                if (Offset == 0)
                {
                    continue;
                }
                Offset -= 0x0F000000;
                DataLength = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);

                addressAlign = 16 - (Convert.ToInt32(DataLength) % 16);
                if (addressAlign == 16)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    DataLength++;
                }


                binaryReader.BaseStream.Position = Offset + 0x641F70;
                Target = Path.Combine(FolderPath, "Segment5_" + (Offset - 0x641F70).ToString()+".bin");
                byte[] LocalData = binaryReader.ReadBytes(DataLength);
                TextureArrayBytes.AddRange(Tarmac.DecompressMIO0(LocalData));
                File.WriteAllBytes(Target, Tarmac.DecompressMIO0(LocalData));
                Loop++;
            }
            


            binaryReader.BaseStream.Position = Course.HeaderData.s47Start;
            DataLength = Convert.ToInt32(Course.HeaderData.s7Start - 0x0F000000);

            addressAlign = 16 - (Convert.ToInt32(DataLength) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                DataLength++;
            }
            Target = Path.Combine(FolderPath, "Segment4.bin");
            byte[] Segment4Data = binaryReader.ReadBytes(DataLength);
            File.WriteAllBytes(Target, TarmacGeo.InflateVertex(Tarmac.DecompressMIO0(Segment4Data)));

            binaryReader.BaseStream.Position = Course.HeaderData.s47Start + Convert.ToInt32(Course.HeaderData.s7Start - 0x0F000000);
            DataLength = Convert.ToInt32(Course.HeaderData.S7Size);
            Target = Path.Combine(FolderPath, "Segment7.bin");
            byte[] Segment7Data = binaryReader.ReadBytes(DataLength);
            File.WriteAllBytes(Target, Segment7Data);

            Target = Path.Combine(FolderPath, "RawSegment7.bin");
            File.WriteAllBytes(Target, Tarmac.Decompress_seg7(Segment7Data));

            binaryReader.BaseStream.Position = Course.HeaderData.s47Start;
            DataLength = Convert.ToInt32(Course.HeaderData.s47End - Course.HeaderData.s47Start);
            Target = Path.Combine(FolderPath, "Segment47.bin");
            File.WriteAllBytes(Target, binaryReader.ReadBytes(DataLength));

            Target = Path.Combine(FolderPath, "Offsets.txt");
            string[] Values = TextBoxValue.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            string[] Params = TextBoxItem.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            for (int ThisStr = 0; ThisStr < Values.Length; ThisStr++)
            {
                File.AppendAllText(Target, Params[ThisStr]);
                File.AppendAllText(Target, "\t\t\t\t\t");
                File.AppendAllText(Target, Values[ThisStr]);
                File.AppendAllText(Target, Environment.NewLine);
            }

            TarmacGeo.ExportGeometry(Segment4Data, Segment7Data, 32, 32);

        }

        private byte[] ExtractMIO0Blob(byte[] data)
        {
            if ((data == null) || (data.Length < 16))
            {
                return new byte[0];
            }
            int start = 0;
            int searchEnd = data.Length - 4;
            if (searchEnd > 32)
            {
                searchEnd = 32;
            }
            for (int i = 0; i <= searchEnd; i++)
            {
                if ((data[i] == 0x4D) && (data[i + 1] == 0x49) && (data[i + 2] == 0x4F) && (data[i + 3] == 0x30))
                {
                    start = i;
                    break;
                }
            }
            int length = data.Length - start;
            int maxTexture = 0x1000 - 28;
            if (length > maxTexture)
            {
                length = maxTexture;
            }
            byte[] output = new byte[length];
            Array.Copy(data, start, output, 0, length);
            return output;
        }

        private bool IsCustomMenuPointer(int pointer, int romLength)
        {
            if ((pointer == 0) || (pointer == unchecked((int)0xFFFFFFFF)))
            {
                return false;
            }
            return IsValidPointer(pointer, romLength);
        }

        private int MenuAssetEnd(int start, int preferredEnd, int fallbackEnd, int romLength, int maxLength)
        {
            if (IsCustomMenuPointer(preferredEnd, romLength) && (preferredEnd > start))
            {
                return preferredEnd;
            }
            if (IsCustomMenuPointer(fallbackEnd, romLength) && (fallbackEnd > start))
            {
                return fallbackEnd;
            }
            int capped = start + maxLength;
            if (capped > romLength)
            {
                capped = romLength;
            }
            return capped;
        }

        private void LoadMenuImages(byte[] rom, TM64_Course.Course course, int slot, F3DEX095 F3D)
        {
            int setID = slot / 20;
            int courseID = slot % 20;
            int menuEntry = 0xBE9178 + 0x1400 + (setID * 0xA0) + (courseID * 8);
            if ((menuEntry + 8) > rom.Length)
            {
                return;
            }

            MemoryStream romStream = new MemoryStream(rom);
            BinaryReader binaryReader = new BinaryReader(romStream);
            binaryReader.BaseStream.Position = menuEntry;
            int bannerOffset = ReadInt32BE(binaryReader, F3D);
            int previewOffset = ReadInt32BE(binaryReader, F3D);
            int nextBanner = 0;
            if ((menuEntry + 12) <= rom.Length)
            {
                nextBanner = ReadInt32BE(binaryReader, F3D);
            }

            if (IsCustomMenuPointer(bannerOffset, rom.Length))
            {
                int bannerEnd = MenuAssetEnd(bannerOffset, previewOffset, nextBanner, rom.Length, 0x2000);
                course.BannerData = ReadRange(rom, bannerOffset, bannerEnd - bannerOffset);
                if (course.BannerData.Length > 0)
                {
                    course.Settings.BannerPath = TM64_Course.Course.InternalDataPath;
                }
            }

            if (IsCustomMenuPointer(previewOffset, rom.Length))
            {
                int previewEnd = MenuAssetEnd(previewOffset, nextBanner, 0, rom.Length, 0x4000);
                course.PreviewData = ReadRange(rom, previewOffset, previewEnd - previewOffset);
                if (course.PreviewData.Length > 0)
                {
                    course.Settings.PreviewPath = TM64_Course.Course.InternalDataPath;
                }
            }
        }

        private int GetSelectedSlot()
        {
            if ((ComboBoxCourses.SelectedIndex < 0) || (ComboBoxCourses.SelectedIndex >= courseSlots.Count))
            {
                return -1;
            }
            return courseSlots[ComboBoxCourses.SelectedIndex];
        }

        private bool IsValidPointer(int pointer, int romLength)
        {
            if ((pointer == 0) || (pointer == unchecked((int)0xDEADBEEF)) || (pointer == -1))
            {
                return false;
            }
            return (pointer > 0) && (pointer < romLength);
        }

        private int ReadInt32BE(BinaryReader binaryReader, F3DEX095 F3D)
        {
            return BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
        }

        private uint ReadUInt32BE(BinaryReader binaryReader, F3DEX095 F3D)
        {
            return BitConverter.ToUInt32(F3D.BigEndian(binaryReader.ReadUInt32()), 0);
        }

        private short ReadInt16BE(BinaryReader binaryReader, F3DEX095 F3D)
        {
            return BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
        }

        private float ReadFloatBE(BinaryReader binaryReader, F3DEX095 F3D)
        {
            int bits = ReadInt32BE(binaryReader, F3D);
            return BitConverter.ToSingle(BitConverter.GetBytes(bits), 0);
        }

        private byte[] Int32BEBytes(int value)
        {
            byte[] flip = BitConverter.GetBytes(value);
            Array.Reverse(flip);
            return flip;
        }

        private int ParseSeg6Position(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return unchecked((int)0xFFFFFFFF);
            }
            text = text.Trim();
            if (text.StartsWith("0x") || text.StartsWith("0X"))
            {
                text = text.Substring(2);
            }
            uint parsed;
            if (!uint.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out parsed))
            {
                return unchecked((int)0xFFFFFFFF);
            }
            if (parsed == 0)
            {
                return unchecked((int)0xFFFFFFFF);
            }
            int offset = Seg6Offset(unchecked((int)parsed));
            if (offset == 0)
            {
                return unchecked((int)0xFFFFFFFF);
            }
            return offset;
        }

        private int Seg6Offset(int pointer)
        {
            if ((pointer == 0) || (pointer == unchecked((int)0xFFFFFFFF)))
            {
                return pointer;
            }
            return pointer & 0x00FFFFFF;
        }

        private short PathPointCount(int value)
        {
            if (value < 1)
            {
                return 1;
            }
            if (value > short.MaxValue)
            {
                return short.MaxValue;
            }
            return Convert.ToInt16(value);
        }

        private int Align16(int length)
        {
            int addressAlign = 16 - (length % 16);
            if (addressAlign == 16)
            {
                addressAlign = 0;
            }
            return length + addressAlign;
        }

        private void DisplaySegmentSizes(byte[] rom, V51 source)
        {
            TextBoxS4Size.Text = (source.HeaderData.VertCount * 14u).ToString("X");
            TextBoxS7Size.Text = source.HeaderData.S7Size.ToString("X");

            int s5Size = 0;
            int s6Size = 0;
            if (rom != null)
            {
                s5Size = SumTextureRawSizes(rom, Convert.ToInt32(source.HeaderData.s9Start));
                s6Size = ReadMIO0UncompressedSize(rom, Convert.ToInt32(source.HeaderData.s6Start));
            }
            TextBoxS5Size.Text = s5Size.ToString("X");
            TextBoxS6Size.Text = s6Size.ToString("X");
        }

        private int SumTextureRawSizes(byte[] rom, int s9Start)
        {
            if ((rom == null) || !IsValidPointer(s9Start, rom.Length))
            {
                return 0;
            }

            F3DEX095 F3D = new F3DEX095();
            MemoryStream memoryStream = new MemoryStream(rom);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            int total = 0;
            int loop = 0;
            while (loop < 1024)
            {
                int entry = s9Start + (loop * 16);
                if ((entry + 12) > rom.Length)
                {
                    break;
                }
                binaryReader.BaseStream.Position = entry;
                int offset = ReadInt32BE(binaryReader, F3D);
                if (offset == 0)
                {
                    break;
                }
                ReadInt32BE(binaryReader, F3D);
                int rawSize = ReadInt32BE(binaryReader, F3D);
                if (rawSize > 0)
                {
                    total += rawSize;
                }
                loop++;
            }
            return total;
        }

        private int ReadMIO0UncompressedSize(byte[] rom, int start)
        {
            if ((rom == null) || (start < 0) || ((start + 8) > rom.Length))
            {
                return 0;
            }

            int header = start;
            int searchEnd = start + 32;
            if (searchEnd > (rom.Length - 4))
            {
                searchEnd = rom.Length - 4;
            }
            bool found = false;
            for (int i = start; i <= searchEnd; i++)
            {
                if ((rom[i] == 0x4D) && (rom[i + 1] == 0x49) && (rom[i + 2] == 0x4F) && (rom[i + 3] == 0x30))
                {
                    header = i;
                    found = true;
                    break;
                }
            }
            if (!found || ((header + 8) > rom.Length))
            {
                return 0;
            }

            F3DEX095 F3D = new F3DEX095();
            MemoryStream memoryStream = new MemoryStream(rom);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            binaryReader.BaseStream.Position = header + 4;
            int uncompressed = ReadInt32BE(binaryReader, F3D);
            if (uncompressed < 0)
            {
                return 0;
            }
            return uncompressed;
        }

        private byte[] ReadRange(byte[] rom, int start, int length)
        {
            if ((start < 0) || (length <= 0) || (start >= rom.Length))
            {
                return new byte[0];
            }
            if ((start + length) > rom.Length)
            {
                length = rom.Length - start;
            }
            byte[] output = new byte[length];
            Array.Copy(rom, start, output, 0, length);
            return output;
        }

        private string ReadLengthString(byte[] rom, int pointer, F3DEX095 F3D)
        {
            if (!IsValidPointer(pointer, rom.Length))
            {
                return "";
            }
            MemoryStream memoryStream = new MemoryStream(rom);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            binaryReader.BaseStream.Position = pointer;
            int length = ReadInt32BE(binaryReader, F3D);
            if ((length <= 0) || (length > 1024) || ((pointer + 4 + length) > rom.Length))
            {
                return "";
            }
            return Encoding.UTF8.GetString(binaryReader.ReadBytes(length));
        }

        private byte ReadSkyChannel(BinaryReader binaryReader)
        {
            binaryReader.ReadByte();
            return binaryReader.ReadByte();
        }

        private TM64_Geometry.OK64Color NewColor(byte r, byte g, byte b, byte a)
        {
            TM64_Geometry.OK64Color color = new TM64_Geometry.OK64Color();
            color.R = r;
            color.G = g;
            color.B = b;
            color.A = a;
            return color;
        }

        private void EnsureCourseForSave(TM64_Course.Course course)
        {
            if (course.Settings == null)
            {
                course.Settings = new TM64_Course.CourseSettings();
            }
            if (course.Settings.Credits == null)
            {
                course.Settings.Credits = "";
            }
            if (course.Settings.Name == null)
            {
                course.Settings.Name = "";
            }
            if (course.Settings.PreviewPath == null)
            {
                course.Settings.PreviewPath = "";
            }
            if (course.Settings.BannerPath == null)
            {
                course.Settings.BannerPath = "";
            }
            if (course.GhostPath == null)
            {
                course.GhostPath = "";
            }
            if (course.SerialNumber == null)
            {
                course.SerialNumber = "";
            }
            if (course.MapData == null)
            {
                course.MapData = new TM64_Course.MiniMap();
            }
            if (course.MapData.MinimapPath == null)
            {
                course.MapData.MinimapPath = "";
            }
            if (course.MapData.MapColor == null)
            {
                course.MapData.MapColor = NewColor(255, 255, 255, 255);
            }
            if (course.PathSettings == null)
            {
                course.PathSettings = new TM64_Course.PathSettings();
            }
            if (course.PathSettings.PathEffects == null)
            {
                course.PathSettings.PathEffects = new TM64_Course.PathEffect[0];
            }
            for (int ThisFX = 0; ThisFX < course.PathSettings.PathEffects.Length; ThisFX++)
            {
                if (course.PathSettings.PathEffects[ThisFX].BodyColor == null)
                {
                    course.PathSettings.PathEffects[ThisFX].BodyColor = NewColor(255, 255, 255, 255);
                }
                if (course.PathSettings.PathEffects[ThisFX].AdjColor == null)
                {
                    course.PathSettings.PathEffects[ThisFX].AdjColor = NewColor(255, 255, 255, 255);
                }
            }
            if (course.PathSettings.PathOffsets == null)
            {
                course.PathSettings.PathOffsets = new uint[4] { 0x800DC778, 0x800DC778, 0x800DC778, 0x800DC778 };
            }
            if (course.OK64HeaderData == null)
            {
                course.OK64HeaderData = new TM64_Course.OK64Header();
            }
            if (course.OK64HeaderData.PathLength == null)
            {
                course.OK64HeaderData.PathLength = new short[4] { 1, 1, 1, 1 };
            }
            if (course.SongData == null)
            {
                course.SongData = new TM64_Sound.OK64Song();
            }
            if (course.SongData.SequenceData == null)
            {
                course.SongData.SequenceData = new byte[0];
            }
            if (course.SongData.InstrumentData == null)
            {
                course.SongData.InstrumentData = new byte[0];
            }
            if (course.Segment4 == null)
            {
                course.Segment4 = new byte[0];
            }
            if (course.Segment6 == null)
            {
                course.Segment6 = new byte[0];
            }
            if (course.Segment7 == null)
            {
                course.Segment7 = new byte[0];
            }
            if (course.ScrollData == null)
            {
                course.ScrollData = new byte[0];
            }
            if (course.WaterData == null)
            {
                course.WaterData = Int32BEBytes(0);
            }
            if (course.ScreenData == null)
            {
                course.ScreenData = Int32BEBytes(0);
            }
            if (course.KillDisplayData == null)
            {
                course.KillDisplayData = Int32BEBytes(0);
            }
            if (course.GhostData == null)
            {
                course.GhostData = new byte[0];
            }
            if (course.RadarData == null)
            {
                course.RadarData = new byte[0];
            }
            if (course.ObjectListData == null)
            {
                course.ObjectListData = Int32BEBytes(0);
            }
            if (course.ObjectTypeData == null)
            {
                course.ObjectTypeData = Int32BEBytes(0);
            }
            if (course.ObjectModelData == null)
            {
                course.ObjectModelData = new byte[0];
            }
            if (course.ObjectAnimationData == null)
            {
                course.ObjectAnimationData = new byte[0];
            }
            if (course.ObjectHitboxData == null)
            {
                course.ObjectHitboxData = new byte[0];
            }
            if (course.ParameterData == null)
            {
                course.ParameterData = new byte[0];
            }
            if (course.ModelData == null)
            {
                course.ModelData = new TM64_Course.CourseModelData();
            }
            if (course.ModelData.TextureObjects == null)
            {
                course.ModelData.TextureObjects = new TM64_Texture.OK64Texture[0];
            }
            if (course.BombArray == null)
            {
                course.BombArray = new TM64_Course.VSBomb[7];
                for (int ThisBomb = 0; ThisBomb < 7; ThisBomb++)
                {
                    course.BombArray[ThisBomb] = new TM64_Course.VSBomb();
                }
            }
        }

        private void ApplyPreparedCourseToUI(TM64_Course.Course course, V51 source)
        {
            courseSettings1.LoadCourse(course);
            pathSettings1.LoadCourse(course);
            pathSettings1.ReloadLoadedPaths(ExtractedPaths);

            TextBoxS47.Text = source.HeaderData.s47Start.ToString("X");
            TextBoxS6.Text = source.HeaderData.s6Start.ToString("X");
            TextBoxS9.Text = source.HeaderData.s9Start.ToString("X");
            TextBoxS5.Text = (course.Segment5 != null) ? course.Segment5.Length.ToString("X") : "0";
            textBox6.Text = source.HeaderData.VertCount.ToString();
            textBox5.Text = source.HeaderData.S7Size.ToString("X");
            TextBoxSectionView.Text = course.OK64HeaderData.SectionViewPosition.ToString("X");
            TextBoxXLUView.Text = course.OK64HeaderData.XLUViewPosition.ToString("X");
            TextBoxSurfaceMap.Text = course.OK64HeaderData.SurfaceMapPosition.ToString("X");
        }

        private TM64_Course.Course PrepareCourseFromROM(byte[] rom, V51 source, string displayName, int slot)
        {
            TM64 Tarmac = new TM64();
            F3DEX095 F3D = new F3DEX095();
            TM64_Course.Course course = new TM64_Course.Course();
            course.OK64HeaderData = new TM64_Course.OK64Header();
            course.OK64HeaderData.MapHeader = source.HeaderData;
            course.OK64HeaderData.Version = 6;
            course.GoalBannerBool = 1;
            course.OK64HeaderData.GoalBannerToggle = 1;
            course.SkyboxBool = 1;
            course.OK64HeaderData.SkyboxToggle = 1;
            course.LapCount = 3;
            course.ManualTempo = source.Tempo[0] & 0xFF;
            course.PathCount = 1;
            course.DistributeBool = 0;
            course.Gametype = 0;
            course.OK64HeaderData.PathLength = new short[4] { 1, 0, 0, 0 };
            if ((source.ROMVersion > 5) && (source.PathLengths != null) && (source.PathLengths.Length > 0))
            {
                course.OK64HeaderData.PathLength[0] = PathPointCount(source.PathLengths[0]);
            }
            if (source.ROMVersion <= 4)
            {
                course.OK64HeaderData.SectionViewPosition = 0x2328;
                course.OK64HeaderData.SurfaceMapPosition = 0x2538;
                course.OK64HeaderData.XLUViewPosition = unchecked((int)0xFFFFFFFF);
            }
            else
            {
                course.OK64HeaderData.SectionViewPosition = Seg6Offset(source.SectionViewPosition);
                course.OK64HeaderData.SurfaceMapPosition = Seg6Offset(source.SurfaceMapPosition);
                course.OK64HeaderData.XLUViewPosition = Seg6Offset(source.XLUViewPosition);
                if (course.OK64HeaderData.XLUViewPosition == 0)
                {
                    course.OK64HeaderData.XLUViewPosition = unchecked((int)0xFFFFFFFF);
                }
            }
            course.OK64HeaderData.WaterType = 0;
            course.OK64HeaderData.WaterLevel = source.WaterHeight;
            if (source.ROMVersion > 5)
            {
                course.OK64HeaderData.WaterType = source.WaterType;
                course.OK64HeaderData.WaterLevel = source.WaterLevel;
                if ((source.WaterType < 0) || (source.WaterType > 4))
                {
                    course.OK64HeaderData.WaterLevel = source.WaterType;
                    course.OK64HeaderData.WaterType = 0;
                }
            }
            if (float.IsNaN(course.OK64HeaderData.WaterLevel) || float.IsInfinity(course.OK64HeaderData.WaterLevel))
            {
                course.OK64HeaderData.WaterLevel = -80;
            }

            course.SkyColors = new TM64_Course.Sky();
            course.SkyColors.TopColor = NewColor(128, 184, 248, 255);
            course.SkyColors.MidColor = NewColor(216, 232, 248, 255);
            course.SkyColors.BotColor = NewColor(0, 0, 0, 255);
            course.SkyColors.SkyType = 0;
            course.SkyColors.WeatherType = 0;
            if (source.ROMVersion > 5)
            {
                course.SkyColors.SkyType = (source.SkyType >> 8) & 0xFF;
                course.SkyColors.WeatherType = source.SkyType & 0xFF;
            }

            course.Fog = new TM64_Course.OKFog();
            course.Fog.FogToggle = 0;
            course.Fog.StartDistance = -1;
            course.Fog.StopDistance = -1;
            course.Fog.FogColor = NewColor(0, 0, 0, 255);

            course.Settings.Credits = ReadLengthString(rom, source.Credits, F3D);
            course.Settings.Name = ReadLengthString(rom, source.CourseName, F3D);
            if (string.IsNullOrEmpty(course.Settings.Name))
            {
                course.Settings.Name = displayName;
            }
            course.Settings.PreviewPath = "";
            course.Settings.BannerPath = "";
            course.SerialNumber = ReadLengthString(rom, source.SerialKey, F3D);
            course.GhostPath = "";
            course.OK64SongPath = "";
            course.BannerData = new byte[0];
            course.PreviewData = new byte[0];
            course.GhostData = new byte[0];
            course.GhostCharacter = -1;
            course.RadarData = new byte[0];
            LoadMenuImages(rom, course, slot, F3D);

            MemoryStream romStream = new MemoryStream(rom);
            BinaryReader binaryReader = new BinaryReader(romStream);

            int s6Length = Convert.ToInt32(source.HeaderData.s6End - source.HeaderData.s6Start);
            course.Segment6 = new byte[0];
            if ((s6Length > 0) && IsValidPointer(Convert.ToInt32(source.HeaderData.s6Start), rom.Length))
            {
                binaryReader.BaseStream.Position = source.HeaderData.s6Start;
                course.Segment6 = Tarmac.DecompressMIO0(binaryReader.ReadBytes(s6Length));
            }

            int s9Length = Convert.ToInt32(source.HeaderData.s9End - source.HeaderData.s9Start);
            course.Segment9 = new byte[0];
            if ((s9Length > 0) && IsValidPointer(Convert.ToInt32(source.HeaderData.s9Start), rom.Length))
            {
                binaryReader.BaseStream.Position = source.HeaderData.s9Start;
                course.Segment9 = binaryReader.ReadBytes(s9Length);
            }

            List<byte> textureBytes = new List<byte>();
            if (course.Segment9.Length >= 16)
            {
                int loop = 0;
                while (true)
                {
                    int entry = Convert.ToInt32(source.HeaderData.s9Start) + (loop * 16);
                    if ((entry + 8) > rom.Length)
                    {
                        break;
                    }
                    binaryReader.BaseStream.Position = entry;
                    int offset = ReadInt32BE(binaryReader, F3D);
                    if (offset == 0)
                    {
                        break;
                    }
                    int dataLength = ReadInt32BE(binaryReader, F3D);
                    int address;
                    if ((offset & unchecked((int)0xFF000000)) == 0x0F000000)
                    {
                        address = (offset - 0x0F000000) + 0x641F70;
                    }
                    else
                    {
                        address = offset + 0x641F70;
                    }
                    dataLength = Align16(dataLength);
                    if ((address < 0) || ((address + dataLength) > rom.Length))
                    {
                        loop++;
                        continue;
                    }
                    binaryReader.BaseStream.Position = address;
                    byte[] localData = binaryReader.ReadBytes(dataLength);
                    try
                    {
                        textureBytes.AddRange(Tarmac.DecompressMIO0(localData));
                    }
                    catch
                    {
                    }
                    loop++;
                }
            }
            course.Segment5 = textureBytes.ToArray();
            course.Segment5Length = course.Segment5.Length;

            int seg4Comp = Convert.ToInt32(source.HeaderData.s7Start - 0x0F000000);
            if (seg4Comp < 0)
            {
                seg4Comp = Convert.ToInt32(source.HeaderData.s7Start - source.HeaderData.s47Start);
            }
            //seg4Comp = Align16(seg4Comp);
            course.Segment4 = new byte[0];
            course.Segment7 = new byte[0];
            if (IsValidPointer(Convert.ToInt32(source.HeaderData.s47Start), rom.Length) && (seg4Comp > 0))
            {
                binaryReader.BaseStream.Position = source.HeaderData.s47Start;
                byte[] compressed4 = binaryReader.ReadBytes(seg4Comp);
                try
                {
                    course.Segment4 = Tarmac.DecompressMIO0(compressed4);
                }
                catch
                {
                    course.Segment4 = compressed4;
                }

                int seg7Comp = Convert.ToInt32(source.HeaderData.s47End - source.HeaderData.s47Start - seg4Comp);
                if (seg7Comp < 0)
                {
                    seg7Comp = Convert.ToInt32(source.HeaderData.S7Size);
                }
                if (seg7Comp > 0)
                {
                    binaryReader.BaseStream.Position = source.HeaderData.s47Start + seg4Comp;
                    byte[] compressed7 = binaryReader.ReadBytes(seg7Comp);
                    try
                    {
                        course.Segment7 = Tarmac.Decompress_seg7(compressed7);
                    }
                    catch
                    {
                        course.Segment7 = compressed7;
                    }
                }
            }

            if ((source.ScrollStart != 0) && (source.ScrollEnd > source.ScrollStart) && (source.ScrollStart < rom.Length))
            {
                course.ScrollData = ReadRange(rom, source.ScrollStart, source.ScrollEnd - source.ScrollStart);
            }
            else
            {
                course.ScrollData = new byte[0];
            }
            course.WaterData = Int32BEBytes(0);
            course.ScreenData = Int32BEBytes(0);
            course.KillDisplayData = Int32BEBytes(0);

            course.PathSettings.PathOffsets = new uint[4] { 0x06000008, 0x800DC778, 0x800DC778, 0x800DC778 };
            if (IsValidPointer(source.PathOffset, rom.Length) && ((source.PathOffset + 4) <= rom.Length))
            {
                binaryReader.BaseStream.Position = source.PathOffset;
                uint pathPointer = ReadUInt32BE(binaryReader, F3D);
                if ((pathPointer & 0xFF000000) == 0x06000000)
                {
                    course.PathSettings.PathOffsets[0] = pathPointer;
                }
            }

            TM64_Paths tarmacPath = new TM64_Paths();
            TM64_Paths.Pathlist parsedPath = tarmacPath.LoadPathList(course.Segment6, Convert.ToInt32(course.PathSettings.PathOffsets[0] & 0x00FFFFFF));
            if ((parsedPath.pathmarker == null) || (parsedPath.pathmarker.Count == 0))
            {
                course.PathSettings.PathOffsets[0] = 0x06000008;
                parsedPath = tarmacPath.LoadPathList(course.Segment6, 8);
            }
            if ((parsedPath.pathmarker == null) || (parsedPath.pathmarker.Count == 0))
            {
                parsedPath = tarmacPath.LoadPathList(course.Segment6, 0);
                if ((parsedPath.pathmarker != null) && (parsedPath.pathmarker.Count > 0))
                {
                    course.PathSettings.PathOffsets[0] = 0x06000000;
                }
            }
            if ((parsedPath.pathmarker != null) && (parsedPath.pathmarker.Count > 0))
            {
                course.OK64HeaderData.PathLength[0] = PathPointCount(parsedPath.pathmarker.Count);
            }
            course.OK64HeaderData.PathLength[1] = 0;
            course.OK64HeaderData.PathLength[2] = 0;
            course.OK64HeaderData.PathLength[3] = 0;
            course.PathCount = 1;
            ExtractedPaths = new TM64_Paths.Pathlist[] { parsedPath };

            if (IsValidPointer(source.SkyPosition, rom.Length))
            {
                binaryReader.BaseStream.Position = source.SkyPosition;
                if ((source.SkyPosition + 12) <= rom.Length)
                {
                    course.SkyColors.TopColor.R = ReadSkyChannel(binaryReader);
                    course.SkyColors.TopColor.G = ReadSkyChannel(binaryReader);
                    course.SkyColors.TopColor.B = ReadSkyChannel(binaryReader);
                    course.SkyColors.MidColor.R = ReadSkyChannel(binaryReader);
                    course.SkyColors.MidColor.G = ReadSkyChannel(binaryReader);
                    course.SkyColors.MidColor.B = ReadSkyChannel(binaryReader);
                }
                if (source.ROMVersion <= 5)
                {
                    if ((source.SkyPosition + 24) <= rom.Length)
                    {
                        ReadSkyChannel(binaryReader);
                        ReadSkyChannel(binaryReader);
                        ReadSkyChannel(binaryReader);
                        course.SkyColors.BotColor.R = ReadSkyChannel(binaryReader);
                        course.SkyColors.BotColor.G = ReadSkyChannel(binaryReader);
                        course.SkyColors.BotColor.B = ReadSkyChannel(binaryReader);
                    }
                }
                else if ((source.SkyPosition + 32) <= rom.Length)
                {
                    binaryReader.BaseStream.Position = source.SkyPosition + 16;
                    ReadSkyChannel(binaryReader);
                    ReadSkyChannel(binaryReader);
                    ReadSkyChannel(binaryReader);
                    course.SkyColors.BotColor.R = ReadSkyChannel(binaryReader);
                    course.SkyColors.BotColor.G = ReadSkyChannel(binaryReader);
                    course.SkyColors.BotColor.B = ReadSkyChannel(binaryReader);
                }
            }

            course.MapData = new TM64_Course.MiniMap();
            course.MapData.MapColor = NewColor(255, 255, 255, 255);
            course.MapData.MapCoord = new Assimp.Vector2D(0, 0);
            course.MapData.StartCoord = new Assimp.Vector2D(0, 0);
            course.MapData.LineCoord = new Assimp.Vector2D(0, 0);
            course.MapData.MinimapPath = "";
            course.MapData.MapScale = 1.55f;
            bool newMapHeader = (source.ROMVersion > 5);
            int mapHeaderSize = newMapHeader ? 28 : 24;
            if (IsValidPointer(source.Maps, rom.Length) && ((source.Maps + mapHeaderSize) <= rom.Length))
            {
                binaryReader.BaseStream.Position = source.Maps;
                short mapX = ReadInt16BE(binaryReader, F3D);
                short mapY = ReadInt16BE(binaryReader, F3D);
                short startX = ReadInt16BE(binaryReader, F3D);
                short startY = ReadInt16BE(binaryReader, F3D);
                short lineX = 0;
                short lineY = 0;
                if (newMapHeader)
                {
                    lineX = ReadInt16BE(binaryReader, F3D);
                    lineY = ReadInt16BE(binaryReader, F3D);
                }
                course.MapData.Height = ReadInt16BE(binaryReader, F3D);
                course.MapData.Width = ReadInt16BE(binaryReader, F3D);
                course.MapData.MapColor.R = Convert.ToByte(ReadInt16BE(binaryReader, F3D) & 0xFF);
                course.MapData.MapColor.G = Convert.ToByte(ReadInt16BE(binaryReader, F3D) & 0xFF);
                course.MapData.MapColor.B = Convert.ToByte(ReadInt16BE(binaryReader, F3D) & 0xFF);
                ReadInt16BE(binaryReader, F3D);
                float mapScale = ReadFloatBE(binaryReader, F3D);
                if (!(float.IsNaN(mapScale) || float.IsInfinity(mapScale) || (mapScale == 0)))
                {
                    course.MapData.MapScale = mapScale;
                }
                course.MapData.MapCoord = new Assimp.Vector2D(mapX, mapY);
                course.MapData.StartCoord = new Assimp.Vector2D(startX, startY);
                course.MapData.LineCoord = new Assimp.Vector2D(lineX, lineY);
                int remaining = 0x1000 - mapHeaderSize;
                if ((source.Maps + mapHeaderSize + remaining) > rom.Length)
                {
                    remaining = rom.Length - (source.Maps + mapHeaderSize);
                }
                if (remaining > 0)
                {
                    course.RadarData = ExtractMIO0Blob(binaryReader.ReadBytes(remaining));
                    if (course.RadarData.Length > 0)
                    {
                        course.MapData.MinimapPath = TM64_Course.Course.InternalDataPath;
                    }
                }
            }

            course.BombArray = new TM64_Course.VSBomb[7];
            for (int ThisBomb = 0; ThisBomb < 7; ThisBomb++)
            {
                course.BombArray[ThisBomb] = new TM64_Course.VSBomb();
                course.BombArray[ThisBomb].Point = Convert.ToInt16(new int[] { 40, 100, 265, 285, 420, 0, 0 }[ThisBomb]);
                course.BombArray[ThisBomb].Type = Convert.ToInt16(new int[] { 3, 3, 3, 1, 1, 0, 0 }[ThisBomb]);
            }
            if (IsValidPointer(source.BombOffset, rom.Length))
            {
                binaryReader.BaseStream.Position = source.BombOffset;
                for (int ThisBomb = 0; ThisBomb < 7; ThisBomb++)
                {
                    if ((binaryReader.BaseStream.Position + 4) > rom.Length)
                    {
                        break;
                    }
                    course.BombArray[ThisBomb].Point = ReadInt16BE(binaryReader, F3D);
                    course.BombArray[ThisBomb].Type = ReadInt16BE(binaryReader, F3D);
                    if (course.BombArray[ThisBomb].Type < 0)
                    {
                        course.BombArray[ThisBomb].Type = 0;
                    }
                    binaryReader.BaseStream.Position += 24;
                }
            }

            course.PathSettings.PathEffects = new TM64_Course.PathEffect[0];
            if (source.ROMVersion <= 5)
            {
                if ((source.EchoOffset != 0) || (source.EchoEnd != 0))
                {
                    TM64_Course.PathEffect echo = new TM64_Course.PathEffect();
                    echo.StartIndex = source.EchoOffset;
                    echo.EndIndex = source.EchoEnd;
                    echo.Type = 0;
                    echo.Power = 0;
                    echo.BodyColor = NewColor(255, 255, 255, 255);
                    echo.AdjColor = NewColor(255, 255, 255, 255);
                    course.PathSettings.PathEffects = new TM64_Course.PathEffect[] { echo };
                }
            }
            else if (IsValidPointer(source.EchoOffset, rom.Length) && (source.EchoEnd > source.EchoOffset))
            {
                binaryReader.BaseStream.Position = source.EchoOffset;
                int echoCount = ReadInt32BE(binaryReader, F3D);
                if ((echoCount > 0) && (echoCount < 256))
                {
                    List<TM64_Course.PathEffect> effects = new List<TM64_Course.PathEffect>();
                    for (int ThisEcho = 0; ThisEcho < echoCount; ThisEcho++)
                    {
                        if ((binaryReader.BaseStream.Position + 12) > rom.Length)
                        {
                            break;
                        }
                        TM64_Course.PathEffect effect = new TM64_Course.PathEffect();
                        effect.StartIndex = ReadInt16BE(binaryReader, F3D);
                        effect.EndIndex = ReadInt16BE(binaryReader, F3D);
                        effect.Type = binaryReader.ReadByte();
                        if (effect.Type < 0)
                        {
                            effect.Type = 0;
                        }
                        effect.Power = binaryReader.ReadByte();
                        effect.BodyColor = NewColor(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte(), 255);
                        effect.AdjColor = NewColor(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte(), 255);
                        effects.Add(effect);
                    }
                    course.PathSettings.PathEffects = effects.ToArray();
                }
            }

            if (IsValidPointer(source.Ghost, rom.Length))
            {
                int ghostLength = 0x3C00;
                if ((source.Ghost + ghostLength) > rom.Length)
                {
                    ghostLength = rom.Length - source.Ghost;
                }
                course.GhostData = ReadRange(rom, source.Ghost, ghostLength);
            }

            course.SongData = new TM64_Sound.OK64Song();
            course.SongData.SequenceData = new byte[0];
            course.SongData.InstrumentData = new byte[0];
            if (source.ROMVersion <= 5)
            {
                course.MusicID = unchecked((short)source.MusicID);
            }
            else if ((source.MusicID >= 0) && (source.MusicID < 50))
            {
                course.MusicID = source.MusicID;
            }
            else if (IsValidPointer(source.MusicID, rom.Length) && ((source.MusicID + 16) <= rom.Length))
            {
                binaryReader.BaseStream.Position = source.MusicID;
                int seqPointer = ReadInt32BE(binaryReader, F3D);
                int seqSize = ReadInt32BE(binaryReader, F3D);
                int insPointer = ReadInt32BE(binaryReader, F3D);
                int insSize = ReadInt32BE(binaryReader, F3D);
                course.MusicID = 3;
                if (IsValidPointer(seqPointer, rom.Length) && (seqSize > 0))
                {
                    course.SongData.SequenceData = ReadRange(rom, seqPointer, seqSize);
                }
                if (IsValidPointer(insPointer, rom.Length) && (insSize > 0))
                {
                    course.SongData.InstrumentData = ReadRange(rom, insPointer, insSize);
                }
                if (course.SongData.SequenceData.Length > 0)
                {
                    course.OK64SongPath = TM64_Course.Course.InternalDataPath;
                }
            }
            else
            {
                course.MusicID = 0;
            }

            course.ObjectTypeData = Int32BEBytes(0);
            course.ObjectListData = Int32BEBytes(0);
            course.ObjectModelData = new byte[0];
            course.ObjectAnimationData = new byte[0];
            course.ObjectHitboxData = new byte[0];
            course.ParameterData = new byte[0];

            if ((course.Segment5 != null) && (course.Segment5.Length > 0))
            {
                TM64_Texture.OK64Texture imported = new TM64_Texture.OK64Texture();
                imported.TexelData[0].texturePath = "IMPORTED";
                imported.TexelData[0].TextureData = course.Segment5;
                imported.TexelData[0].fileSize = course.Segment5.Length;
                imported.TexelData[0].compressedTexture = new byte[0];
                imported.TexelData[0].compressedSize = 0;
                course.ModelData.TextureObjects = new TM64_Texture.OK64Texture[] { imported };
            }
            else
            {
                course.ModelData.TextureObjects = new TM64_Texture.OK64Texture[0];
            }

            if (string.IsNullOrEmpty(course.SerialNumber))
            {
                TM64_Course TarmacCourse = new TM64_Course();
                course.SerialNumber = TarmacCourse.OK64Serial(course);
            }

            return course;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TM64_Geometry TarmacGeo = new TM64_Geometry();
            TarmacGeo.CubeMeDaddyO();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Tarmac64_Library.GameBuilderPro GB = new GameBuilderPro();
            GB.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog FS = new OpenFileDialog();
            if (FS.ShowDialog() == DialogResult.OK)
            {
                TM64 Tarmac = new TM64();
                SaveFileDialog FF = new SaveFileDialog();
                if (FF.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(FF.FileName, Tarmac.DecompressMIO0(File.ReadAllBytes(FS.FileName)));
                }
            }
        }
    }
}
