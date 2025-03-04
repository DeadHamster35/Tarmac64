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

namespace Tarmac64_Retail
{
    public partial class CourseSettings : UserControl
    {
        public CourseSettings()
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

        private void CourseSettings_Load(object sender, EventArgs e)
        {
            CourseData.OK64HeaderData = new TM64_Course.OK64Header();


            CourseData.MapData = new TM64_Course.MiniMap();
            CourseData.MapData.MapColor = new TM64_Geometry.OK64Color();
            CourseData.MapData.MapCoord = new Assimp.Vector2D(0, 0);
            CourseData.MapData.StartCoord = new Assimp.Vector2D(0, 0);
            CourseData.MapData.LineCoord = new Assimp.Vector2D(0, 0);
            CourseData.SkyColors = new TM64_Course.Sky();
            CourseData.SkyColors.TopColor = new TM64_Geometry.OK64Color();
            CourseData.SkyColors.MidColor = new TM64_Geometry.OK64Color();
            CourseData.SkyColors.BotColor = new TM64_Geometry.OK64Color();
            CourseData.EchoColor = new TM64_Geometry.OK64Color();
            CourseData.EchoAdjustColor = new TM64_Geometry.OK64Color();
            CourseData.PathEffects = new TM64_Course.PathEffect[0];
            CourseData.BombArray = new TM64_Course.VSBomb[7];
            CourseData.PathSurface = new int[4];
            CourseData.Fog = new TM64_Course.OKFog();
            CourseData.Fog.FogToggle = 0;
            CourseData.Fog.StartDistance = 900;
            CourseData.Fog.StopDistance = 1000;
            CourseData.Fog.FogColor = new TM64_Geometry.OK64Color();
            CourseData.Fog.FogColor.R = 240;
            CourseData.Fog.FogColor.G = 240;
            CourseData.Fog.FogColor.B = 240;
            CourseData.Fog.FogColor.A = 255;
            CourseData.GoalBannerBool = 1;
            CourseData.SkyboxBool = 1;
            CourseData.ManualTempo = 2;

            FogStartBox.Text = "900";
            FogEndBox.Text = "1000";
            FogToggleBox.Checked = false;
            FogRBox.Text = "240";
            FogGBox.Text = "240";
            FogBBox.Text = "240";
            FogABox.Text = "255";


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

            skyBox.SelectedIndex = 0;
            weatherBox.SelectedIndex = 0;

            PathSurfaceSelect.Items.Add("Path 0");
            PathSurfaceSelect.Items.Add("Path 1");
            PathSurfaceSelect.Items.Add("Path 2");
            PathSurfaceSelect.Items.Add("Path 3");
            PathSurfaceSelect.SelectedIndex = 0;

            foreach (var BombType in bombTypes)
            {   
                BombTypeBox.Items.Add(BombType);
            }
            for (int currentBomb = 0; currentBomb < 7; currentBomb++)
            {
                BombIndexBox.Items.Add("Bomb " + currentBomb.ToString());
                CourseData.BombArray[currentBomb] = new TM64_Course.VSBomb();
                CourseData.BombArray[currentBomb].Point = Convert.ToInt16(bombPoints[currentBomb]);
                CourseData.BombArray[currentBomb].Type = Convert.ToInt16(bompTypeIDs[currentBomb]);
            }

            for (int songIndex = 0; songIndex < songNames.Length; songIndex++)
            {
                songBox.Items.Add(songNames[songIndex]);
            }

            for (int pathIndex = 0; pathIndex < pathTypes.Length; pathIndex++)
            {
                PathTypeBox.Items.Add(pathTypes[pathIndex]);
            }

            for (int waterIndex = 0; waterIndex < waterTypes.Length; waterIndex++)
            {
                WaterTypeBox.Items.Add(waterTypes[waterIndex]);
            }
            WaterTypeBox.SelectedIndex = 0;
            PathTypeBox.SelectedIndex = 0;
            BombIndexBox.SelectedIndex = 0;
            songBox.SelectedIndex = 3;


            waterBox.Text = "-80";
            loaded = true;
            UpdateCourse();

        }

        public int LoadCourseSettings(string[] SettingsInfo)
        {
            int ThisLine = 0;
            CourseData.Credits = SettingsInfo[ThisLine++];
            CourseData.Name = SettingsInfo[ThisLine++];
            CourseData.PreviewPath = SettingsInfo[ThisLine++];
            CourseData.BannerPath = SettingsInfo[ThisLine++];
            CourseData.GhostPath = SettingsInfo[ThisLine++];
            CourseData.OK64HeaderData.WaterType = Convert.ToInt32(SettingsInfo[ThisLine++]);
            CourseData.OK64HeaderData.WaterLevel = Convert.ToInt32(SettingsInfo[ThisLine++]);
            CourseData.ManualTempo = Convert.ToInt32(SettingsInfo[ThisLine++]);
            CourseData.GoalBannerBool = Convert.ToInt16(SettingsInfo[ThisLine++]);
            CourseData.SkyboxBool = Convert.ToInt16(SettingsInfo[ThisLine++]);
            CourseData.PathSurface = new int[4];
            CourseData.PathSurface[0] = Convert.ToInt32(SettingsInfo[ThisLine++]);
            CourseData.PathSurface[1] = Convert.ToInt32(SettingsInfo[ThisLine++]);
            CourseData.PathSurface[2] = Convert.ToInt32(SettingsInfo[ThisLine++]);
            CourseData.PathSurface[3] = Convert.ToInt32(SettingsInfo[ThisLine++]);

            CourseData.PathCount = Convert.ToInt16(SettingsInfo[ThisLine++]);
            CourseData.DistributeBool = Convert.ToInt16(SettingsInfo[ThisLine++]);

            int Count = Convert.ToInt32(SettingsInfo[ThisLine++]);
            CourseData.PathEffects = new TM64_Course.PathEffect[Count];
            PathIndexBox.Items.Clear();
            for (int This = 0; This < Count; This++)
            {
                CourseData.PathEffects[This] = new TM64_Course.PathEffect();
                CourseData.PathEffects[This].Type = Convert.ToInt32(SettingsInfo[ThisLine++]);
                CourseData.PathEffects[This].StartIndex = Convert.ToInt32(SettingsInfo[ThisLine++]);
                CourseData.PathEffects[This].EndIndex = Convert.ToInt32(SettingsInfo[ThisLine++]);
                CourseData.PathEffects[This].Power = Convert.ToInt32(SettingsInfo[ThisLine++]);

                CourseData.PathEffects[This].AdjColor = new TM64_Geometry.OK64Color();
                CourseData.PathEffects[This].AdjColor.R = Convert.ToByte(SettingsInfo[ThisLine++]);
                CourseData.PathEffects[This].AdjColor.G = Convert.ToByte(SettingsInfo[ThisLine++]);
                CourseData.PathEffects[This].AdjColor.B = Convert.ToByte(SettingsInfo[ThisLine++]);

                CourseData.PathEffects[This].BodyColor = new TM64_Geometry.OK64Color();
                CourseData.PathEffects[This].BodyColor.R = Convert.ToByte(SettingsInfo[ThisLine++]);
                CourseData.PathEffects[This].BodyColor.G = Convert.ToByte(SettingsInfo[ThisLine++]);
                CourseData.PathEffects[This].BodyColor.B = Convert.ToByte(SettingsInfo[ThisLine++]);
                PathIndexBox.Items.Add("P " + (PathIndexBox.Items.Count).ToString());
            }


            CourseData.MapData.MinimapPath = SettingsInfo[ThisLine++];
            CourseData.MapData.MapCoord = new Assimp.Vector2D
                (
                    Convert.ToInt32(SettingsInfo[ThisLine++]),
                    Convert.ToInt32(SettingsInfo[ThisLine++])
                );
            CourseData.MapData.StartCoord = new Assimp.Vector2D
                (
                    Convert.ToInt32(SettingsInfo[ThisLine++]),
                    Convert.ToInt32(SettingsInfo[ThisLine++])
                );
            CourseData.MapData.LineCoord = new Assimp.Vector2D
                (
                    Convert.ToInt32(SettingsInfo[ThisLine++]),
                    Convert.ToInt32(SettingsInfo[ThisLine++])
                );
            CourseData.MapData.MapScale = Convert.ToSingle(SettingsInfo[ThisLine++]);
            CourseData.MapData.MapColor = new TM64_Geometry.OK64Color();
            CourseData.MapData.MapColor.R = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.MapData.MapColor.G = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.MapData.MapColor.B = Convert.ToByte(SettingsInfo[ThisLine++]);

            CourseData.SkyColors.TopColor = new TM64_Geometry.OK64Color();
            CourseData.SkyColors.TopColor.R = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.SkyColors.TopColor.G = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.SkyColors.TopColor.B = Convert.ToByte(SettingsInfo[ThisLine++]);

            CourseData.SkyColors.MidColor = new TM64_Geometry.OK64Color();
            CourseData.SkyColors.MidColor.R = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.SkyColors.MidColor.G = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.SkyColors.MidColor.B = Convert.ToByte(SettingsInfo[ThisLine++]);

            CourseData.SkyColors.BotColor = new TM64_Geometry.OK64Color();
            CourseData.SkyColors.BotColor.R = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.SkyColors.BotColor.G = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.SkyColors.BotColor.B = Convert.ToByte(SettingsInfo[ThisLine++]);

            CourseData.SkyColors.SkyType = Convert.ToInt32(SettingsInfo[ThisLine++]);
            CourseData.SkyColors.WeatherType = Convert.ToInt32(SettingsInfo[ThisLine++]);

            CourseData.MusicID = Convert.ToInt32(SettingsInfo[ThisLine++]);
            CourseData.OK64SongPath = SettingsInfo[ThisLine++];

            CourseData.BombArray = new TM64_Course.VSBomb[7];
            for (int ThisBomb = 0; ThisBomb < 7; ThisBomb++)
            {
                CourseData.BombArray[ThisBomb] = new TM64_Course.VSBomb();
                CourseData.BombArray[ThisBomb].Point = Convert.ToInt16(SettingsInfo[ThisLine++]);
                CourseData.BombArray[ThisBomb].Type = Convert.ToInt16(SettingsInfo[ThisLine++]);
            }



            CourseData.Fog = new TM64_Course.OKFog();

            CourseData.Fog.FogToggle = Convert.ToInt16(SettingsInfo[ThisLine++]);
            CourseData.Fog.FogColor = new TM64_Geometry.OK64Color();
            CourseData.Fog.FogColor.R = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.Fog.FogColor.G = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.Fog.FogColor.B = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.Fog.FogColor.A = Convert.ToByte(SettingsInfo[ThisLine++]);
            CourseData.Fog.StartDistance = Convert.ToInt16(SettingsInfo[ThisLine++]);
            CourseData.Fog.StopDistance = Convert.ToInt16(SettingsInfo[ThisLine++]);

            UpdateUI();
            return ThisLine;
        }

        public string[] SaveCourseSettings()
        {
            List<string> Output = new List<string>();

            Output.Add(CourseData.Credits);
            Output.Add(CourseData.Name);
            Output.Add(CourseData.PreviewPath);
            Output.Add(CourseData.BannerPath);
            Output.Add(CourseData.GhostPath);
            Output.Add(CourseData.OK64HeaderData.WaterType.ToString());
            Output.Add(CourseData.OK64HeaderData.WaterLevel.ToString());
            Output.Add(CourseData.ManualTempo.ToString());
            Output.Add(CourseData.GoalBannerBool.ToString());
            Output.Add(CourseData.SkyboxBool.ToString());
            Output.Add(CourseData.PathSurface[0].ToString());
            Output.Add(CourseData.PathSurface[1].ToString());
            Output.Add(CourseData.PathSurface[2].ToString());
            Output.Add(CourseData.PathSurface[3].ToString());
            Output.Add(CourseData.PathCount.ToString());
            Output.Add(CourseData.DistributeBool.ToString());

            Output.Add(CourseData.PathEffects.Length.ToString());
            for (int ThisPath = 0; ThisPath < CourseData.PathEffects.Length; ThisPath++)
            {
                Output.Add(CourseData.PathEffects[ThisPath].Type.ToString());
                Output.Add(CourseData.PathEffects[ThisPath].StartIndex.ToString());
                Output.Add(CourseData.PathEffects[ThisPath].EndIndex.ToString());
                Output.Add(CourseData.PathEffects[ThisPath].Power.ToString());
                Output.Add(CourseData.PathEffects[ThisPath].AdjColor.R.ToString());
                Output.Add(CourseData.PathEffects[ThisPath].AdjColor.G.ToString());
                Output.Add(CourseData.PathEffects[ThisPath].AdjColor.B.ToString());
                Output.Add(CourseData.PathEffects[ThisPath].BodyColor.R.ToString());
                Output.Add(CourseData.PathEffects[ThisPath].BodyColor.G.ToString());
                Output.Add(CourseData.PathEffects[ThisPath].BodyColor.B.ToString());
            }

            Output.Add(CourseData.MapData.MinimapPath.ToString());
            Output.Add(CourseData.MapData.MapCoord[0].ToString());
            Output.Add(CourseData.MapData.MapCoord[1].ToString());
            Output.Add(CourseData.MapData.StartCoord[0].ToString());
            Output.Add(CourseData.MapData.StartCoord[1].ToString());
            Output.Add(CourseData.MapData.LineCoord[0].ToString());
            Output.Add(CourseData.MapData.LineCoord[1].ToString());
            Output.Add(CourseData.MapData.MapScale.ToString());

            Output.Add(CourseData.MapData.MapColor.R.ToString());
            Output.Add(CourseData.MapData.MapColor.G.ToString());
            Output.Add(CourseData.MapData.MapColor.B.ToString());

            Output.Add(CourseData.SkyColors.TopColor.R.ToString());
            Output.Add(CourseData.SkyColors.TopColor.G.ToString());
            Output.Add(CourseData.SkyColors.TopColor.B.ToString());

            Output.Add(CourseData.SkyColors.MidColor.R.ToString());
            Output.Add(CourseData.SkyColors.MidColor.G.ToString());
            Output.Add(CourseData.SkyColors.MidColor.B.ToString());

            Output.Add(CourseData.SkyColors.BotColor.R.ToString());
            Output.Add(CourseData.SkyColors.BotColor.G.ToString());
            Output.Add(CourseData.SkyColors.BotColor.B.ToString());

            Output.Add(CourseData.SkyColors.SkyType.ToString());
            Output.Add(CourseData.SkyColors.WeatherType.ToString());


            Output.Add(CourseData.MusicID.ToString());
            Output.Add(CourseData.OK64SongPath);
            for (int ThisBomb = 0; ThisBomb < 7; ThisBomb++)
            {
                Output.Add(CourseData.BombArray[ThisBomb].Point.ToString());
                Output.Add(CourseData.BombArray[ThisBomb].Type.ToString());
            }

            Output.Add(CourseData.Fog.FogToggle.ToString());
            Output.Add(CourseData.Fog.FogColor.R.ToString());
            Output.Add(CourseData.Fog.FogColor.G.ToString());
            Output.Add(CourseData.Fog.FogColor.B.ToString());
            Output.Add(CourseData.Fog.FogColor.A.ToString());
            Output.Add(CourseData.Fog.StartDistance.ToString());
            Output.Add(CourseData.Fog.StopDistance.ToString());

            return Output.ToArray();

        }

        public void UpdateCourse()
        {

            if ((!loaded) || (blocked))
            {
                return;
            }
            ColorUpdate();

            int ParseInt, ParseInt2 = 0;
            float ParseFloat, ParseFloat2 = 0;
            int Index = PathIndexBox.SelectedIndex;


            CourseData.Credits = CreditsBox.Text;
            CourseData.Name = CourseNameBox.Text;
            CourseData.PreviewPath = previewBox.Text;
            CourseData.BannerPath = bannerBox.Text;
            CourseData.GhostPath = ghostBox.Text;



            CourseData.Fog.FogToggle = Convert.ToInt16(FogToggleBox.Checked);

            CourseData.GoalBannerBool = Convert.ToInt16(!GoalBannerBox.Checked);  //Inverse
            CourseData.SkyboxBool = Convert.ToInt16(SkyBoxCheckBox.Checked);  



            if (int.TryParse(FogRBox.Text, out ParseInt))
            {
                CourseData.Fog.FogColor.R = Convert.ToByte(ParseInt);
            }
            if (int.TryParse(FogGBox.Text, out ParseInt))
            {
                CourseData.Fog.FogColor.G = Convert.ToByte(ParseInt);
            }
            if (int.TryParse(FogBBox.Text, out ParseInt))
            {
                CourseData.Fog.FogColor.B = Convert.ToByte(ParseInt);
            }
            if (int.TryParse(FogABox.Text, out ParseInt))
            {
                CourseData.Fog.FogColor.A = Convert.ToByte(ParseInt);
            }

            if (int.TryParse(FogStartBox.Text, out ParseInt))
            {
                CourseData.Fog.StartDistance = Convert.ToInt16(ParseInt);
            }
            if (int.TryParse(FogEndBox.Text, out ParseInt))
            {
                CourseData.Fog.StopDistance = Convert.ToInt16(ParseInt);
            }


            CourseData.OK64HeaderData.WaterType = WaterTypeBox.SelectedIndex;

            if (int.TryParse(PathSurfaceBox.Text, out ParseInt))
            {
                CourseData.PathSurface[PathSurfaceSelect.SelectedIndex] = ParseInt;
            }

            if (int.TryParse(PathCountBox.Text, out ParseInt))
            {
                CourseData.PathCount = Convert.ToInt16(ParseInt);
            }
            CourseData.DistributeBool = Convert.ToInt16(DistributeBox.Checked);

            if (float.TryParse(waterBox.Text, out ParseFloat))
            {
                CourseData.OK64HeaderData.WaterLevel = ParseFloat;
            }

            if (int.TryParse(TempoTTBox.Text, out ParseInt))
            {
                CourseData.ManualTempo = ParseInt;
            }
            if (int.TryParse(LapBox.Text, out ParseInt))
            {
                CourseData.LapCount = ParseInt;
            }



            if (int.TryParse(BombPointBox.Text, out ParseInt))
            {
                CourseData.BombArray[BombIndexBox.SelectedIndex].Point = Convert.ToInt16(ParseInt);
            }
            CourseData.BombArray[BombIndexBox.SelectedIndex].Type = Convert.ToInt16(BombTypeBox.SelectedIndex);


            byte ParseByte = new byte();
            if (Index != -1)
            {
                if (int.TryParse(EchoStartBox.Text, out ParseInt))
                {
                    PathFX[Index].StartIndex = ParseInt;
                }
                if (int.TryParse(EchoStopBox.Text, out ParseInt))
                {
                    PathFX[Index].EndIndex = ParseInt;
                }
                if (int.TryParse(EchoPowerBox.Text, out ParseInt))
                {
                    PathFX[Index].Power = ParseInt;
                }

                

                if (byte.TryParse(BaseR.Text, out ParseByte))
                {
                    PathFX[Index].BodyColor.R = Convert.ToByte(ParseByte);
                }
                if (byte.TryParse(BaseG.Text, out ParseByte))
                {
                    PathFX[Index].BodyColor.G = Convert.ToByte(ParseByte);
                }
                if (byte.TryParse(BaseB.Text, out ParseByte))
                {
                    PathFX[Index].BodyColor.B = Convert.ToByte(ParseByte);
                }
                if (byte.TryParse(AdjR.Text, out ParseByte))
                {
                    PathFX[Index].AdjColor.R = Convert.ToByte(ParseByte);
                }
                if (byte.TryParse(AdjG.Text, out ParseByte))
                {
                    PathFX[Index].AdjColor.G = Convert.ToByte(ParseByte);
                }
                if (byte.TryParse(AdjB.Text, out ParseByte))
                {
                    PathFX[Index].AdjColor.B = Convert.ToByte(ParseByte);
                }
                PathFX[Index].Type = PathTypeBox.SelectedIndex;
            }


            


            if (int.TryParse(mapXBox.Text, out ParseInt))
            {
                if (int.TryParse(mapYBox.Text, out ParseInt2))
                {
                    CourseData.MapData.MapCoord = new Assimp.Vector2D(ParseInt, ParseInt2);
                }
            }


            if (int.TryParse(startXBox.Text, out ParseInt))
            {
                if (int.TryParse(startYBox.Text, out ParseInt2))
                {
                    CourseData.MapData.StartCoord = new Assimp.Vector2D(ParseInt, ParseInt2);
                }
            }

            if (int.TryParse(lineXBox.Text, out ParseInt))
            {
                if (int.TryParse(lineYBox.Text, out ParseInt2))
                {
                    CourseData.MapData.LineCoord = new Assimp.Vector2D(ParseInt, ParseInt2);
                }
            }

            float.TryParse(MapScaleBox.Text, out ParseFloat);
            CourseData.MapData.MapScale = ParseFloat;


            if (byte.TryParse(MapRBox.Text, out ParseByte))
            {
                CourseData.MapData.MapColor.R = Convert.ToByte(ParseByte);
            }
            if (byte.TryParse(MapGBox.Text, out ParseByte))
            {
                CourseData.MapData.MapColor.G = Convert.ToByte(ParseByte);
            }
            if (byte.TryParse(MapBBox.Text, out ParseByte))
            {
                CourseData.MapData.MapColor.B = Convert.ToByte(ParseByte);
            }


            CourseData.MapData.MinimapPath = mapBox.Text;

            //sky

            if (byte.TryParse(SkyRT.Text, out ParseByte))
            {
                CourseData.SkyColors.TopColor.R = Convert.ToByte(ParseByte);
            }
            if (byte.TryParse(SkyGT.Text, out ParseByte))
            {
                CourseData.SkyColors.TopColor.G = Convert.ToByte(ParseByte);
            }
            if (byte.TryParse(SkyBT.Text, out ParseByte))
            {
                CourseData.SkyColors.TopColor.B = Convert.ToByte(ParseByte);
            }

            if (byte.TryParse(SkyRM.Text, out ParseByte))
            {
                CourseData.SkyColors.MidColor.R = Convert.ToByte(ParseByte);
            }
            if (byte.TryParse(SkyGM.Text, out ParseByte))
            {
                CourseData.SkyColors.MidColor.G = Convert.ToByte(ParseByte);
            }
            if (byte.TryParse(SkyBM.Text, out ParseByte))
            {
                CourseData.SkyColors.MidColor.B = Convert.ToByte(ParseByte);
            }

            if (byte.TryParse(SkyRB.Text, out ParseByte))
            {
                CourseData.SkyColors.BotColor.R = Convert.ToByte(ParseByte);
            }
            if (byte.TryParse(SkyGB.Text, out ParseByte))
            {
                CourseData.SkyColors.BotColor.G = Convert.ToByte(ParseByte);
            }
            if (byte.TryParse(SkyBB.Text, out ParseByte))
            {
                CourseData.SkyColors.BotColor.B = Convert.ToByte(ParseByte);
            }

            CourseData.SkyColors.SkyType = skyBox.SelectedIndex;
            CourseData.SkyColors.WeatherType = weatherBox.SelectedIndex;

            CourseData.MusicID = songBox.SelectedIndex;
            CourseData.OK64SongPath = oksongBox.Text;
            CourseData.PathEffects = PathFX.ToArray();

            CourseData.BombArray[BombIndexBox.SelectedIndex].Type = Convert.ToInt16(BombTypeBox.SelectedIndex);
            CourseData.BombArray[BombIndexBox.SelectedIndex].Point = Convert.ToInt16(BombPointBox.Text);
            if (UpdateParent != null)
            {
                UpdateParent(null, null);
            }
            
        }


        public void UpdateUI()
        {

            if (!loaded)
            {
                return;
            }
            
            PathFX = CourseData.PathEffects.ToList();
            int ParseInt, ParseInt2 = 0;
            float ParseFloat, ParseFloat2 = 0;
            int Index = PathIndexBox.SelectedIndex;

            CreditsBox.Text = CourseData.Credits;
            CourseNameBox.Text = CourseData.Name;
            previewBox.Text = CourseData.PreviewPath;
            bannerBox.Text = CourseData.BannerPath;
            ghostBox.Text = ghostBox.Text;
            WaterTypeBox.SelectedIndex = CourseData.OK64HeaderData.WaterType;
            BombTypeBox.SelectedIndex = CourseData.BombArray[BombIndexBox.SelectedIndex].Type;
            waterBox.Text = CourseData.OK64HeaderData.WaterLevel.ToString();

            FogToggleBox.Checked = Convert.ToBoolean(CourseData.Fog.FogToggle);
            FogStartBox.Text = CourseData.Fog.StartDistance.ToString();
            FogEndBox.Text = CourseData.Fog.StopDistance.ToString();
            FogRBox.Text = CourseData.Fog.FogColor.R.ToString();
            FogGBox.Text = CourseData.Fog.FogColor.G.ToString();
            FogBBox.Text = CourseData.Fog.FogColor.B.ToString();
            FogABox.Text = CourseData.Fog.FogColor.A.ToString();


            if (Index != -1)
            {

                EchoStartBox.Text = PathFX[Index].StartIndex.ToString();
                EchoStopBox.Text = PathFX[Index].EndIndex.ToString();
                EchoPowerBox.Text = PathFX[Index].Power.ToString();

                BaseR.Text = PathFX[Index].BodyColor.R.ToString();
                BaseG.Text = PathFX[Index].BodyColor.G.ToString();
                BaseB.Text = PathFX[Index].BodyColor.B.ToString();

                AdjR.Text = PathFX[Index].AdjColor.R.ToString();
                AdjG.Text = PathFX[Index].AdjColor.G.ToString();
                AdjB.Text = PathFX[Index].AdjColor.B.ToString();
                PathTypeBox.SelectedIndex = PathFX[Index].Type;
            }

            PathSurfaceBox.Text = CourseData.PathSurface[PathSurfaceSelect.SelectedIndex].ToString();
            PathCountBox.Text = CourseData.PathCount.ToString();

            GoalBannerBox.Checked = !Convert.ToBoolean(CourseData.GoalBannerBool);
            SkyBoxCheckBox.Checked = Convert.ToBoolean(CourseData.SkyboxBool);


            mapBox.Text = CourseData.MapData.MinimapPath;
            mapXBox.Text = CourseData.MapData.MapCoord[0].ToString();
            mapYBox.Text = CourseData.MapData.MapCoord[1].ToString();

            startXBox.Text = CourseData.MapData.StartCoord[0].ToString();
            startYBox.Text = CourseData.MapData.StartCoord[1].ToString();

            lineXBox.Text = CourseData.MapData.LineCoord[0].ToString();
            lineYBox.Text = CourseData.MapData.LineCoord[1].ToString();

            MapScaleBox.Text = CourseData.MapData.MapScale.ToString();

            MapRBox.Text = CourseData.MapData.MapColor.R.ToString();
            MapGBox.Text = CourseData.MapData.MapColor.G.ToString();
            MapBBox.Text = CourseData.MapData.MapColor.B.ToString();

            mapBox.Text = CourseData.MapData.MinimapPath;

            SkyRT.Text = CourseData.SkyColors.TopColor.R.ToString();
            SkyGT.Text = CourseData.SkyColors.TopColor.G.ToString();
            SkyBT.Text = CourseData.SkyColors.TopColor.B.ToString();
            SkyRM.Text = CourseData.SkyColors.MidColor.R.ToString();
            SkyGM.Text = CourseData.SkyColors.MidColor.G.ToString();
            SkyBM.Text = CourseData.SkyColors.MidColor.B.ToString();
            SkyRB.Text = CourseData.SkyColors.BotColor.R.ToString();
            SkyGB.Text = CourseData.SkyColors.BotColor.G.ToString();
            SkyBB.Text = CourseData.SkyColors.BotColor.B.ToString();
            skyBox.SelectedIndex = CourseData.SkyColors.SkyType;
            weatherBox.SelectedIndex = CourseData.SkyColors.WeatherType;

            ColorUpdate();

            songBox.SelectedIndex = CourseData.MusicID;
            oksongBox.Text = CourseData.OK64SongPath;

            BombTypeBox.SelectedIndex = CourseData.BombArray[BombIndexBox.SelectedIndex].Type;
        }


        private void UpdateUIHandler(object sender, KeyEventArgs e)
        {
            UpdateCourse();
        }


        private void previewBtn_Click(object sender, EventArgs e)
        {
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                previewBox.Text = FileOpen.FileName;
            }
            UpdateCourse();
        }

        private void bannerBtn_Click(object sender, EventArgs e)
        {
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                bannerBox.Text = FileOpen.FileName;
            }
            UpdateCourse();
        }

        private void ghostBtn_Click(object sender, EventArgs e)
        {
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                ghostBox.Text = FileOpen.FileName;
            }
            UpdateCourse();
        }

        private void mapBtn_Click(object sender, EventArgs e)
        {
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                mapBox.Text = FileOpen.FileName;
            }
            UpdateCourse();
        }

        private void songBtn_Click(object sender, EventArgs e)
        {
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                oksongBox.Text = FileOpen.FileName;
            }
            UpdateCourse();
        }


        private void ColorUpdate()
        {

            System.Drawing.Color buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
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
                buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
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


            colorInt[0] = 0;
            int.TryParse(BaseR.Text, out colorInt[0]);
            colorInt[1] = 0;
            int.TryParse(BaseG.Text, out colorInt[1]);
            colorInt[2] = 0;
            int.TryParse(BaseB.Text, out colorInt[2]);
            if (colorInt[0] < 256 & colorInt[0] > -1 & colorInt[1] < 256 & colorInt[1] > -1 & colorInt[2] < 256 & colorInt[2] > -1)
            {
                buttonColor = System.Drawing.Color.FromArgb(colorInt[0], colorInt[1], colorInt[2]);
            }
            else
            {
                buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
            }
            ColorPickBase.BackColor = buttonColor;


            colorInt[0] = 0;
            int.TryParse(AdjR.Text, out colorInt[0]);
            colorInt[1] = 0;
            int.TryParse(AdjG.Text, out colorInt[1]);
            colorInt[2] = 0;
            int.TryParse(AdjB.Text, out colorInt[2]);
            if (colorInt[0] < 256 & colorInt[0] > -1 & colorInt[1] < 256 & colorInt[1] > -1 & colorInt[2] < 256 & colorInt[2] > -1)
            {
                buttonColor = System.Drawing.Color.FromArgb(colorInt[0], colorInt[1], colorInt[2]);
            }
            else
            {
                buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
            }
            ColorPickAdjust.BackColor = buttonColor;




            colorInt[0] = 0;
            int.TryParse(FogRBox.Text, out colorInt[0]);
            colorInt[1] = 0;
            int.TryParse(FogGBox.Text, out colorInt[1]);
            colorInt[2] = 0;
            int.TryParse(FogBBox.Text, out colorInt[2]);

            if (colorInt[0] < 256 & colorInt[0] > -1 & colorInt[1] < 256 & colorInt[1] > -1 & colorInt[2] < 256 & colorInt[2] > -1)
            {
                buttonColor = System.Drawing.Color.FromArgb(colorInt[0], colorInt[1], colorInt[2]);
            }
            else
            {
                buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
            }
            FogCButton.BackColor = buttonColor;


        }


        private void ColorPickT_Click(object sender, EventArgs e)
        {
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                SkyRT.Text = ColorPick.Color.R.ToString();
                SkyGT.Text = ColorPick.Color.G.ToString();
                SkyBT.Text = ColorPick.Color.B.ToString();
            }
            UpdateCourse();
        }

        private void ColorPickMT_Click(object sender, EventArgs e)
        {
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                SkyRM.Text = ColorPick.Color.R.ToString();
                SkyGM.Text = ColorPick.Color.G.ToString();
                SkyBM.Text = ColorPick.Color.B.ToString();
            }
            UpdateCourse();
        }

        private void ColorPickB_Click(object sender, EventArgs e)
        {
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                SkyRB.Text = ColorPick.Color.R.ToString();
                SkyGB.Text = ColorPick.Color.G.ToString();
                SkyBB.Text = ColorPick.Color.B.ToString();
            }
            UpdateCourse();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool Backup = loaded;
            loaded = false;

            PathFX.Add(new TM64_Course.PathEffect());
            int Index = PathFX.Count - 1;
            PathFX[Index].AdjColor = new TM64_Geometry.OK64Color();
            PathFX[Index].BodyColor = new TM64_Geometry.OK64Color();
            PathTypeBox.SelectedIndex = 0;
            EchoStartBox.Text = "0";
            EchoStopBox.Text = "0";
            EchoPowerBox.Text = "0";
            BaseR.Text = "0";
            BaseG.Text = "0";
            BaseB.Text = "0";
            AdjR.Text = "0";
            AdjG.Text = "0";
            AdjB.Text = "0";

            PathIndexBox.SelectedIndex = PathIndexBox.Items.Add("P " + (PathIndexBox.Items.Count).ToString());

            loaded = Backup;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PathIndexBox.Items.Count > 0)
            {
                PathFX.RemoveAt(PathIndexBox.SelectedIndex);
                PathIndexBox.Items.RemoveAt(PathIndexBox.SelectedIndex);
            }
        }

        private void EchoIndexBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Backup = loaded;
            loaded = false;

            int Index = PathIndexBox.SelectedIndex;
            PathTypeBox.SelectedIndex = PathFX[Index].Type;
            EchoStartBox.Text = PathFX[Index].StartIndex.ToString();
            EchoStopBox.Text = PathFX[Index].EndIndex.ToString();
            EchoPowerBox.Text = PathFX[Index].Power.ToString();
            BaseR.Text = PathFX[Index].BodyColor.R.ToString();
            BaseG.Text = PathFX[Index].BodyColor.G.ToString();
            BaseB.Text = PathFX[Index].BodyColor.B.ToString();

            AdjR.Text = PathFX[Index].AdjColor.R.ToString();
            AdjG.Text = PathFX[Index].AdjColor.G.ToString();
            AdjB.Text = PathFX[Index].AdjColor.B.ToString();

            loaded = Backup;
        }

        private void EchoStartBox_TextChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void EchoStopBox_TextChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void EchoPowerBox_TextChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void ColorPickMap_Click_1(object sender, EventArgs e)
        {
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                MapRBox.Text = ColorPick.Color.R.ToString();
                MapGBox.Text = ColorPick.Color.G.ToString();
                MapBBox.Text = ColorPick.Color.B.ToString();
            }
            UpdateCourse();
        }

        private void ColorPickBase_Click(object sender, EventArgs e)
        {
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                BaseR.Text = ColorPick.Color.R.ToString();
                BaseG.Text = ColorPick.Color.G.ToString();
                BaseB.Text = ColorPick.Color.B.ToString();
            }
            UpdateCourse();
        }

        private void ColorPickAdjust_Click(object sender, EventArgs e)
        {
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                AdjR.Text = ColorPick.Color.R.ToString();
                AdjG.Text = ColorPick.Color.G.ToString();
                AdjB.Text = ColorPick.Color.B.ToString();
            }
            UpdateCourse();
        }

        private void EchoPowerBox_TextChanged_1(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void BombSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Backup = loaded;
            loaded = false;
            BombTypeBox.SelectedIndex = CourseData.BombArray[BombIndexBox.SelectedIndex].Type;
            BombPointBox.Text = CourseData.BombArray[BombIndexBox.SelectedIndex].Point.ToString();
            loaded = Backup;
        }

        private void BombPointBox_TextChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void BombTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void BombPointBox_TextChanged_1(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void songBtn_Click_1(object sender, EventArgs e)
        {
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                oksongBox.Text = FileOpen.FileName;
            }
            UpdateCourse();
        }

        private void oksongBox_TextChanged(object sender, EventArgs e)
        {
            if (oksongBox.Text.Length > 0)
            {
                songBox.Enabled = false;
            }
            else
            {
                songBox.Enabled = true;
            }
        }

        private void waterBox_TextChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            weatherBox.Enabled = false;
            if (skyBox.SelectedIndex == 3)
            {
                weatherBox.Enabled = true;
            }
            UpdateCourse();
        }

        private void weatherBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void songBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void UpdateUIHandler(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void ghostBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }



        private void FogCButton_Click(object sender, EventArgs e)
        {
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                FogRBox.Text = ColorPick.Color.R.ToString();
                FogGBox.Text = ColorPick.Color.G.ToString();
                FogBBox.Text = ColorPick.Color.B.ToString();
            }
            UpdateCourse();
        }

        private void UpdateUI(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void GoalBannerBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void WaterTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }
    }

}
