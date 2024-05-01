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

            for (int ThisFX = 0; ThisFX < FXCount; ThisFX++)
            {

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
            BombIndexBox.SelectedIndex = 0;
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

        }

        private void BombPointBox_TextChanged(object sender, EventArgs e)
        {

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

        private void ColorUpdate()
        {

            System.Drawing.Color buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
            int[] colorInt = new int[3];



        }
    }

}
