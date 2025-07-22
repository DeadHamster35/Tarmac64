using Cereal64.Microcodes.F3DEX.DataElements;
using F3DSharp;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tarmac64_Library;

namespace OverKart64_Retail.Windows
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
            public int Mods { get; set; }
            public int Maps { get; set; }
            public int Objects { get; set; }
            public short[] Echo {get;set;}
            public short[] Tempo { get; set; }
            public int MusicID { get; set; }
            public short PathLength { get; set; }
            public short WaterHeight { get; set; }

            public int ScrollOffset { get; set; }
            public int LastOffset { get; set; }
                
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
            public short WaterType { get; set; }
            public short WaterLevel { get; set; }
            public int ScrollStart { get; set; }
            public int ScrollEnd { get; set; }
            public int Sky { get; set; }

            public V51(BinaryReader binaryReader)
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
                PathLength = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);

                WaterType = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);
                WaterLevel = BitConverter.ToInt16(F3D.BigEndian(binaryReader.ReadInt16()), 0);

                ScrollStart = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                ScrollEnd = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
                Sky = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);

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
        
        private void button1_Click(object sender, EventArgs e)
        {
            ButtonSave.Enabled = false;
            ComboBoxCourses.Items.Clear();

            V51Array = new V51[80];
            V5Array = new V5[80];
            V4Array = new V4[80];
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


            //file found
                    
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
                    //if (VersionNumber == 5)
                    {
                        V51Array[Index] = new V51(binaryReader);
                    }
                    if (V51Array[Index].CourseName == 0xDEADBEEF)
                    {
                        binaryReader.BaseStream.Position = V51Array[Index].CourseName;
                        int Length = binaryReader.ReadInt32();
                        byte[] CharData = binaryReader.ReadBytes(Length);
                        string Name = Encoding.UTF8.GetString(CharData);
                        ComboBoxCourses.Items.Add(Name);
                    }
                    else
                    {
                        ComboBoxCourses.Items.Add("Course " + Index.ToString());
                    }
                   

                }
            }
            binaryReader.Close();
            memoryStream.Close();

            ButtonSave.Enabled = true;
        }

        private void WriteV5()
        {
            V51 Course = V51Array[ComboBoxCourses.SelectedIndex];
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

            TextBoxItem.Text += nameof(Course.ScrollStart) + Environment.NewLine;
            TextBoxValue.Text += Course.ScrollStart.ToString("X") + Environment.NewLine;
            TextBoxItem.Text += nameof(Course.ScrollEnd) + Environment.NewLine;
            TextBoxValue.Text += Course.ScrollEnd.ToString("X") + Environment.NewLine;

            TextBoxItem.Text += nameof(Course.Sky) + Environment.NewLine;
            TextBoxValue.Text += Course.Sky.ToString("X") + Environment.NewLine;
        }
        private void ComboBoxCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (V51Array.Length > 0)
            {
                WriteV5();
            }
            if (V4Array.Length > 0)
            {
                //WriteV4();
            }


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

            binaryReader.BaseStream.Position = Course.HeaderData.s9Start;

            Offset = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);
            DataLength = BitConverter.ToInt32(F3D.BigEndian(binaryReader.ReadInt32()), 0);

            int addressAlign = 16 - (Convert.ToInt32(DataLength) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                DataLength++;
            }


            Offset += 0x641F70;
            binaryReader.BaseStream.Position = Offset;
            Target = Path.Combine(FolderPath, "Segment5.bin");
            byte[] Segment5Data = binaryReader.ReadBytes(DataLength);
            File.WriteAllBytes(Target, Tarmac.DecompressMIO0(Segment5Data));


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

            TarmacGeo.ExportGeometry(Segment4Data, Segment7Data, Segment5Data, 32, 32);

        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog FileOpen = new CommonOpenFileDialog();
            FileOpen.IsFolderPicker = true;
            MessageBox.Show("Select Output Directory");
            if (FileOpen.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }
            string FolderPath = FileOpen.FileName;
            MessageBox.Show("Select ROM");
            FileOpen.IsFolderPicker = false;
            if (FileOpen.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }
            string ROM = FileOpen.FileName;
            if (!File.Exists(ROM))
            {
                return;
            }
            int TargetOffset, Length;


            if (V51Array.Length > 0)
            {
                ExportV5(FolderPath, ROM);
                return;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            TM64_Geometry TarmacGeo = new TM64_Geometry();
            TarmacGeo.CubeMeDaddyO();
        }
    }
}
