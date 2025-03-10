using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Tarmac64_Library;
using System.Globalization;
using System.Xml;
using SharpGL.SceneGraph;

namespace Tarmac64_Retail
{
    public partial class PathSettings : UserControl
    {
        public PathSettings()
        {
            InitializeComponent();
        }

        public event EventHandler UpdateParent;

        public string[] songNames = new string[] { "None", "Title", "Menu", "Raceways", "Moo Moo Farm", "Choco Mountain", "Koopa Troopa Beach", "Banshee Boardwalk", "Snowland", "Bowser's Castle", "Kalimari Desert", "#- GP Startup", "#- Final Lap", "#- Final Lap (1st)", "#- Final Lap 2-4", "#- You Lose", "#- Race Results", "Star Music", "Rainbow Road", "DK Parkway", "#- Credits Failure", "Toad's Turnpike", "#- VS/Battle Start", "#- VS/Battle Results", "#- Retry/Quit", "Big Donut / Skyscraper", "#- Trophy A", "#- Trophy B1 (Win)", "Credits", "#- Trophy B2 (Lose)" };
        public string[] pathTypes = new string[] { "Echo", "Color", "Camera", "AirControl", "Long Jump", "AI Area", "3D Weather" };
        public string[] waterTypes = new string[] { "Water", "Void", "Lava", "Ice", "Fire" };
        public int[] bombPoints = new int[] { 40, 100, 265, 285, 420, 0, 0 };
        public int[] bompTypeIDs = new int[] { 3, 3, 3, 1, 1, 0, 0 };
        public string[] bombTypes = new string[] { "Null", "Rotate L", "Rotate R", "Stop" };
        List<TM64_Course.PathEffect> PathFX = new List<TM64_Course.PathEffect>();
        OpenFileDialog FileOpen = new OpenFileDialog();
        ColorDialog ColorPick = new ColorDialog();
        public bool loaded, blocked = false;

        TM64 Tarmac = new TM64();
        TM64_Course TarmacCourse = new TM64_Course();
        TM64.OK64Settings okSettings = new TM64.OK64Settings();
        public TM64_Course.Course CourseData = new TM64_Course.Course();

        public void LoadPathSettings(MemoryStream memoryStream)
        {
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            CourseData.PathSettings.PathSurface[0] = binaryReader.ReadInt32();
            CourseData.PathSettings.PathSurface[1] = binaryReader.ReadInt32();
            CourseData.PathSettings.PathSurface[2] = binaryReader.ReadInt32();
            CourseData.PathSettings.PathSurface[3] = binaryReader.ReadInt32();
            CourseData.PathCount = binaryReader.ReadInt16();
            CourseData.DistributeBool = binaryReader.ReadInt16();

            int FXCount = binaryReader.ReadInt32();
            CourseData.PathSettings.PathEffects = new TM64_Course.PathEffect[FXCount];

            for (int ThisPath = 0; ThisPath < CourseData.PathSettings.PathEffects.Length; ThisPath++)
            {
                CourseData.PathSettings.PathEffects[ThisPath].Type = binaryReader.ReadInt32();
                CourseData.PathSettings.PathEffects[ThisPath].StartIndex = binaryReader.ReadInt32();
                CourseData.PathSettings.PathEffects[ThisPath].EndIndex = binaryReader.ReadInt32();
                CourseData.PathSettings.PathEffects[ThisPath].Power = binaryReader.ReadInt32();
                CourseData.PathSettings.PathEffects[ThisPath].AdjColor.R = binaryReader.ReadByte();
                CourseData.PathSettings.PathEffects[ThisPath].AdjColor.G = binaryReader.ReadByte();
                CourseData.PathSettings.PathEffects[ThisPath].AdjColor.B = binaryReader.ReadByte();
                CourseData.PathSettings.PathEffects[ThisPath].BodyColor.R = binaryReader.ReadByte();
                CourseData.PathSettings.PathEffects[ThisPath].BodyColor.G = binaryReader.ReadByte();
                CourseData.PathSettings.PathEffects[ThisPath].BodyColor.B = binaryReader.ReadByte();
            }

            for (int ThisBomb = 0; ThisBomb < 7; ThisBomb++)
            {
                CourseData.BombArray[ThisBomb].Point = binaryReader.ReadInt16();
                CourseData.BombArray[ThisBomb].Type = binaryReader.ReadInt16();
            }

        }

        public void LoadPathXML(XmlDocument XMLDoc)
        {
            string ParentPath = "/SaveFile/PathSettings";
            TM64 Tarmac = new TM64();

            CourseData.PathSettings.PathSurface[0] = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "PathSurface0", "0"));
            CourseData.PathSettings.PathSurface[1] = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "PathSurface1", "0"));
            CourseData.PathSettings.PathSurface[2] = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "PathSurface2", "0"));
            CourseData.PathSettings.PathSurface[3] = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "PathSurface3", "0"));
            CourseData.PathCount = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, ParentPath, "PathCount", "1"));
            CourseData.DistributeBool = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, ParentPath, "DistributeBool", "0"));
            CourseData.LapCount = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, ParentPath, "LapCount", "3"));

            int FXCount = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "EffectCount", "0"));
            PathFX = new List<TM64_Course.PathEffect>();

            ParentPath = "/SaveFile/PathSettings/PathEffects";
            for (int ThisFX = 0; ThisFX < FXCount; ThisFX++)
            {
                string SubPath = ParentPath + "/FX_" + ThisFX.ToString();
                TM64_Course.PathEffect NewFX = new TM64_Course.PathEffect();
                NewFX.Type = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, SubPath, "Type", "0"));
                NewFX.StartIndex = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, SubPath, "StartIndex", "0"));
                NewFX.EndIndex = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, SubPath, "EndIndex", "0"));
                NewFX.Power = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, SubPath, "Power", "0"));

                NewFX.AdjColor = new TM64_Geometry.OK64Color();
                NewFX.AdjColor.R = Convert.ToByte(Tarmac.LoadElement(XMLDoc, SubPath, "AdjColor.R", "255"));
                NewFX.AdjColor.G = Convert.ToByte(Tarmac.LoadElement(XMLDoc, SubPath, "AdjColor.G", "255"));
                NewFX.AdjColor.B = Convert.ToByte(Tarmac.LoadElement(XMLDoc, SubPath, "AdjColor.B", "255"));

                NewFX.BodyColor = new TM64_Geometry.OK64Color();
                NewFX.BodyColor.R = Convert.ToByte(Tarmac.LoadElement(XMLDoc, SubPath, "BodyColor.R", "255"));
                NewFX.BodyColor.G = Convert.ToByte(Tarmac.LoadElement(XMLDoc, SubPath, "BodyColor.G", "255"));
                NewFX.BodyColor.B = Convert.ToByte(Tarmac.LoadElement(XMLDoc, SubPath, "BodyColor.B", "255"));

                PathIndexBox.Items.Add(ThisFX);
                PathFX.Add(NewFX);                
            }
        }

        public void SavePathXML(XmlDocument XMLDoc, XmlElement Parent)
        {
            UpdatePaths();
            CourseData.PathSettings.PathEffects = PathFX.ToArray();
            XmlElement PathXML = XMLDoc.CreateElement("PathSettings");
            Parent.AppendChild(PathXML);
            TM64 Tarmac = new TM64();

            Tarmac.GenerateElement(XMLDoc, PathXML, "PathSurface0", CourseData.PathSettings.PathSurface[0]);
            Tarmac.GenerateElement(XMLDoc, PathXML, "PathSurface1", CourseData.PathSettings.PathSurface[1]);
            Tarmac.GenerateElement(XMLDoc, PathXML, "PathSurface2", CourseData.PathSettings.PathSurface[2]);
            Tarmac.GenerateElement(XMLDoc, PathXML, "PathSurface3", CourseData.PathSettings.PathSurface[3]);
            Tarmac.GenerateElement(XMLDoc, PathXML, "PathCount", CourseData.PathCount);
            Tarmac.GenerateElement(XMLDoc, PathXML, "DistributeBool", CourseData.DistributeBool);
            Tarmac.GenerateElement(XMLDoc, PathXML, "LapCount", CourseData.LapCount);

            Tarmac.GenerateElement(XMLDoc, PathXML, "EffectCount", CourseData.PathSettings.PathEffects.Length);
            XmlElement FXXML = XMLDoc.CreateElement("PathEffects");
            PathXML.AppendChild(FXXML);
            for (int ThisPath = 0; ThisPath < CourseData.PathSettings.PathEffects.Length; ThisPath++)
            {
                XmlElement LocalXML = XMLDoc.CreateElement("FX_"+ThisPath.ToString());
                FXXML.AppendChild(LocalXML);
                Tarmac.GenerateElement(XMLDoc, LocalXML, "Type", CourseData.PathSettings.PathEffects[ThisPath].Type);
                Tarmac.GenerateElement(XMLDoc, LocalXML, "StartIndex", CourseData.PathSettings.PathEffects[ThisPath].StartIndex);
                Tarmac.GenerateElement(XMLDoc, LocalXML, "EndIndex", CourseData.PathSettings.PathEffects[ThisPath].EndIndex);
                Tarmac.GenerateElement(XMLDoc, LocalXML, "Power", CourseData.PathSettings.PathEffects[ThisPath].Power);
                Tarmac.GenerateElement(XMLDoc, LocalXML, "AdjColor.R", CourseData.PathSettings.PathEffects[ThisPath].AdjColor.R);
                Tarmac.GenerateElement(XMLDoc, LocalXML, "AdjColor.G", CourseData.PathSettings.PathEffects[ThisPath].AdjColor.G);
                Tarmac.GenerateElement(XMLDoc, LocalXML, "AdjColor.B", CourseData.PathSettings.PathEffects[ThisPath].AdjColor.B);
                Tarmac.GenerateElement(XMLDoc, LocalXML, "BodyColor.R", CourseData.PathSettings.PathEffects[ThisPath].BodyColor.R);
                Tarmac.GenerateElement(XMLDoc, LocalXML, "BodyColor.G", CourseData.PathSettings.PathEffects[ThisPath].BodyColor.G);
                Tarmac.GenerateElement(XMLDoc, LocalXML, "BodyColor.B", CourseData.PathSettings.PathEffects[ThisPath].BodyColor.B);
            }
        }
        public byte[] SavePathSettings()
        {
            UpdatePaths();
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(CourseData.PathSettings.PathSurface[0]);
            binaryWriter.Write(CourseData.PathSettings.PathSurface[1]);
            binaryWriter.Write(CourseData.PathSettings.PathSurface[2]);
            binaryWriter.Write(CourseData.PathSettings.PathSurface[3]);
            binaryWriter.Write(CourseData.PathCount);
            binaryWriter.Write(CourseData.DistributeBool);

            binaryWriter.Write(CourseData.PathSettings.PathEffects.Length);
            for (int ThisPath = 0; ThisPath < CourseData.PathSettings.PathEffects.Length; ThisPath++)
            {
                binaryWriter.Write(CourseData.PathSettings.PathEffects[ThisPath].Type);
                binaryWriter.Write(CourseData.PathSettings.PathEffects[ThisPath].StartIndex);
                binaryWriter.Write(CourseData.PathSettings.PathEffects[ThisPath].EndIndex);
                binaryWriter.Write(CourseData.PathSettings.PathEffects[ThisPath].Power);
                binaryWriter.Write(CourseData.PathSettings.PathEffects[ThisPath].AdjColor.R);
                binaryWriter.Write(CourseData.PathSettings.PathEffects[ThisPath].AdjColor.G);
                binaryWriter.Write(CourseData.PathSettings.PathEffects[ThisPath].AdjColor.B);
                binaryWriter.Write(CourseData.PathSettings.PathEffects[ThisPath].BodyColor.R);
                binaryWriter.Write(CourseData.PathSettings.PathEffects[ThisPath].BodyColor.G);
                binaryWriter.Write(CourseData.PathSettings.PathEffects[ThisPath].BodyColor.B);
            }

            for (int ThisBomb = 0; ThisBomb < 7; ThisBomb++)
            {
                binaryWriter.Write(CourseData.BombArray[ThisBomb].Point);
                binaryWriter.Write(CourseData.BombArray[ThisBomb].Type);
            }

            return memoryStream.ToArray();
        }


        public TM64_Course.Course UpdateCourse(TM64_Course.Course ThisCourse)
        {
            ThisCourse.GoalBannerBool = CourseData.GoalBannerBool;
            ThisCourse.PathSettings.PathEffects = CourseData.PathSettings.PathEffects;
            ThisCourse.PathSettings.PathSurface = CourseData.PathSettings.PathSurface;
            ThisCourse.PathCount = CourseData.PathCount;
            ThisCourse.DistributeBool = CourseData.DistributeBool;
            ThisCourse.BombArray = CourseData.BombArray;

            return ThisCourse;
        }

        private void PathSettings_Load(object sender, EventArgs e)
        {

            foreach (var PathType in pathTypes)
            {
                PathTypeBox.Items.Add(PathType);
            }
            foreach (var BombType in bombTypes)
            {
                BombTypeBox.Items.Add(BombType);
            }

            CourseData.BombArray = new TM64_Course.VSBomb[7];
            for (int currentBomb = 0; currentBomb < 7; currentBomb++)
            {
                BombIndexBox.Items.Add("Bomb " + currentBomb.ToString());
                CourseData.BombArray[currentBomb] = new TM64_Course.VSBomb();
                CourseData.BombArray[currentBomb].Point = Convert.ToInt16(bombPoints[currentBomb]);
                CourseData.BombArray[currentBomb].Type = Convert.ToInt16(bompTypeIDs[currentBomb]);
            }
            CourseData.PathSettings.PathEffects = new TM64_Course.PathEffect[0];
            CourseData.PathSettings.PathSurface = new int[4];
            
            BombIndexBox.SelectedIndex = 0;
            PathSurfaceSelect.Items.Add("Path 0");
            PathSurfaceSelect.Items.Add("Path 1");
            PathSurfaceSelect.Items.Add("Path 2");
            PathSurfaceSelect.Items.Add("Path 3");
            PathSurfaceSelect.SelectedIndex = 0;

            CircuitRadio.Checked = true;
            GoalBannerBox.Checked = true;
        }

        public void UpdatePaths()
        {

            int ParseInt;
            byte ParseByte;

            CourseData.GoalBannerBool = Convert.ToInt16(GoalBannerBox.Checked);

            //PathFX
            int PFXID = PathIndexBox.SelectedIndex;
            if (PFXID >= 0)
            {
                PathFX[PFXID].Type = PathTypeBox.SelectedIndex;
                if (int.TryParse(EchoStartBox.Text, out ParseInt))
                {
                    PathFX[PFXID].StartIndex = ParseInt;
                }
                if (int.TryParse(EchoStopBox.Text, out ParseInt))
                {
                    PathFX[PFXID].EndIndex = ParseInt;
                }
                if (int.TryParse(EchoPowerBox.Text, out ParseInt))
                {
                    PathFX[PFXID].Power = ParseInt;
                }

                if (byte.TryParse(BaseR.Text, out ParseByte))
                {
                    PathFX[PFXID].BodyColor.R = ParseByte;
                }
                if (byte.TryParse(BaseG.Text, out ParseByte))
                {
                    PathFX[PFXID].BodyColor.G = ParseByte;
                }
                if (byte.TryParse(BaseB.Text, out ParseByte))
                {
                    PathFX[PFXID].BodyColor.B = ParseByte;
                }

                if (byte.TryParse(AdjR.Text, out ParseByte))
                {
                    PathFX[PFXID].AdjColor.R = ParseByte;
                }
                if (byte.TryParse(AdjG.Text, out ParseByte))
                {
                    PathFX[PFXID].AdjColor.G = ParseByte;
                }
                if (byte.TryParse(AdjB.Text, out ParseByte))
                {
                    PathFX[PFXID].AdjColor.B = ParseByte;
                }
            }

            



            if (int.TryParse(PathSurfaceBox.Text, out ParseInt))
            {
                CourseData.PathSettings.PathSurface[PathSurfaceSelect.SelectedIndex] = ParseInt;
            }


            if (int.TryParse(PathSurfaceBox.Text, out ParseInt))
            {
                CourseData.PathSettings.PathSurface[PathSurfaceSelect.SelectedIndex] = ParseInt;
            }

            if (int.TryParse(PathCountBox.Text, out ParseInt))
            {
                CourseData.PathCount = Convert.ToInt16(ParseInt);
            }

            DistributeBox.Checked = Convert.ToBoolean(CourseData.DistributeBool);

            if (int.TryParse(BombPointBox.Text, out ParseInt))
            {
                CourseData.BombArray[BombIndexBox.SelectedIndex].Point = Convert.ToInt16(ParseInt);
            }

            CourseData.BombArray[BombIndexBox.SelectedIndex].Type = Convert.ToInt16(BombTypeBox.SelectedIndex);
        }
        public void UpdateUI()
        {
            if (loaded)
            {
                GoalBannerBox.Checked = Convert.ToBoolean(CourseData.GoalBannerBool);


                //PathFX
                int PFXID = PathIndexBox.SelectedIndex;
                PathTypeBox.SelectedIndex = PathFX[PFXID].Type;

                EchoStartBox.Text = PathFX[PFXID].StartIndex.ToString();
                EchoStopBox.Text = PathFX[PFXID].EndIndex.ToString();
                EchoPowerBox.Text = PathFX[PFXID].Power.ToString();

                BaseR.Text = PathFX[PFXID].BodyColor.R.ToString();
                BaseG.Text = PathFX[PFXID].BodyColor.G.ToString();
                BaseB.Text = PathFX[PFXID].BodyColor.B.ToString();

                AdjR.Text = PathFX[PFXID].AdjColor.R.ToString();
                AdjG.Text = PathFX[PFXID].AdjColor.G.ToString();
                AdjB.Text = PathFX[PFXID].AdjColor.B.ToString();

                int ParseInt;
                if (int.TryParse(LapCountBox.Text, out ParseInt))
                {
                    CourseData.LapCount = ParseInt;
                }


                PathSurfaceBox.Text = CourseData.PathSettings.PathSurface[PathSurfaceSelect.SelectedIndex].ToString();
                PathCountBox.Text = CourseData.PathCount.ToString();
                BombPointBox.Text = CourseData.BombArray[BombIndexBox.SelectedIndex].Point.ToString();
                BombTypeBox.SelectedIndex = CourseData.BombArray[BombIndexBox.SelectedIndex].Type;
            }
            
        }

        private void BombIndexBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Backup = loaded;
            loaded = false;
            BombTypeBox.SelectedIndex = CourseData.BombArray[BombIndexBox.SelectedIndex].Type;
            BombPointBox.Text = CourseData.BombArray[BombIndexBox.SelectedIndex].Point.ToString();
            loaded = Backup;
        }

        private void BombTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loaded)
            {
                return;
            }
            UpdatePaths();
        }

        private void BombPointBox_TextChanged(object sender, EventArgs e)
        {
            if (!loaded)
            {
                return;
            }
            UpdatePaths();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool Backup = loaded;
            loaded = false;

            PathFX.Add(new TM64_Course.PathEffect());
            int Index = PathFX.Count - 1;            
            PathFX[Index].AdjColor = new TM64_Geometry.OK64Color();
            PathFX[Index].BodyColor = new TM64_Geometry.OK64Color();
            PathIndexBox.Items.Add(Index);
            PathTypeBox.SelectedIndex = 0;

            UpdatePaths();
            loaded = Backup;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool Backup = loaded;
            loaded = false;
            PathFX.RemoveAt(PathIndexBox.SelectedIndex);

            UpdatePaths();
            loaded = Backup;
        }

        private void PathIndexBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void PathFXUpdate(object sender, KeyEventArgs e)
        {
            UpdatePaths();
        }

        private void LapFinishLine_Enter(object sender, EventArgs e)
        {

        }

        private void SprintRadio_CheckedChanged(object sender, EventArgs e)
        {
            LapCountBox.Enabled = true;
            if (SprintRadio.Checked)
            {
                LapCountBox.Enabled = false;
            }
        }

        private void CircuitRadio_CheckedChanged(object sender, EventArgs e)
        {
            LapCountBox.Enabled = true;
            if (SprintRadio.Checked)
            {
                LapCountBox.Enabled = false;
            }
        }

        private void UpdateUIHandler(object sender, KeyEventArgs e)
        {
            UpdateUI();
        }

        private void ColorUpdate()
        {

            System.Drawing.Color buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
            int[] colorInt = new int[3];



        }
    }

}
