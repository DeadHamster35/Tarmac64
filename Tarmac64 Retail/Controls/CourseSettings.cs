using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
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
            CourseData.Gametype = 0;

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
            CourseData.PathSettings.PathEffects = new TM64_Course.PathEffect[0];
            CourseData.PathSettings.PathSurface = new int[4];
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
            GameTypeBox.SelectedIndex = 0;


            for (int songIndex = 0; songIndex < songNames.Length; songIndex++)
            {
                songBox.Items.Add(songNames[songIndex]);
            }


            for (int waterIndex = 0; waterIndex < waterTypes.Length; waterIndex++)
            {
                WaterTypeBox.Items.Add(waterTypes[waterIndex]);
            }
            WaterTypeBox.SelectedIndex = 0;

            songBox.SelectedIndex = 3;


            waterBox.Text = "-80";
            loaded = true;
            UpdateCourse();

        }

        public TM64_Course.Course UpdateCourse(TM64_Course.Course ThisCourse)
        {
            ThisCourse.Settings.Credits = CourseData.Settings.Credits;
            ThisCourse.Settings.Name = CourseData.Settings.Name;
            ThisCourse.Settings.PreviewPath = CourseData.Settings.PreviewPath;
            ThisCourse.Settings.BannerPath = CourseData.Settings.BannerPath;
            ThisCourse.GhostPath = CourseData.GhostPath;
            

            ThisCourse.OK64HeaderData.WaterType = CourseData.OK64HeaderData.WaterType;
            ThisCourse.OK64HeaderData.WaterLevel = CourseData.OK64HeaderData.WaterLevel;

            ThisCourse.ManualTempo = CourseData.ManualTempo;


            ThisCourse.Fog.FogToggle = CourseData.Fog.FogToggle;

            ThisCourse.Fog.FogColor = new TM64_Geometry.OK64Color();

            ThisCourse.Fog.FogColor.R = CourseData.Fog.FogColor.R;
            ThisCourse.Fog.FogColor.G = CourseData.Fog.FogColor.G;
            ThisCourse.Fog.FogColor.B = CourseData.Fog.FogColor.B;
            ThisCourse.Fog.FogColor.A = CourseData.Fog.FogColor.A;
            ThisCourse.Fog.StartDistance = CourseData.Fog.StartDistance;
            ThisCourse.Fog.StopDistance = CourseData.Fog.StopDistance;

            ThisCourse.MapData.MapCoord = new Assimp.Vector2D
            (
                CourseData.MapData.MapCoord[0],
                CourseData.MapData.MapCoord[1]
            );

            ThisCourse.MapData.StartCoord = new Assimp.Vector2D
            (
                CourseData.MapData.StartCoord[0],
                CourseData.MapData.StartCoord[1]
            );

            ThisCourse.MapData.LineCoord = new Assimp.Vector2D
            (
                CourseData.MapData.LineCoord[0],
                CourseData.MapData.LineCoord[1]
            );


            ThisCourse.MapData.MapScale = CourseData.MapData.MapScale;

            ThisCourse.MapData.MapColor.R = CourseData.MapData.MapColor.R;
            ThisCourse.MapData.MapColor.G = CourseData.MapData.MapColor.G;
            ThisCourse.MapData.MapColor.B = CourseData.MapData.MapColor.B;

            ThisCourse.MapData.MinimapPath = CourseData.MapData.MinimapPath;


            ThisCourse.SkyboxBool = CourseData.SkyboxBool;

            ThisCourse.SkyColors.TopColor.R = CourseData.SkyColors.TopColor.R;
            ThisCourse.SkyColors.TopColor.G = CourseData.SkyColors.TopColor.G;
            ThisCourse.SkyColors.TopColor.B = CourseData.SkyColors.TopColor.B;

            ThisCourse.SkyColors.MidColor.R = CourseData.SkyColors.MidColor.R;
            ThisCourse.SkyColors.MidColor.G = CourseData.SkyColors.MidColor.G;
            ThisCourse.SkyColors.MidColor.B = CourseData.SkyColors.MidColor.B;

            ThisCourse.SkyColors.BotColor.R = CourseData.SkyColors.BotColor.R;
            ThisCourse.SkyColors.BotColor.G = CourseData.SkyColors.BotColor.G;
            ThisCourse.SkyColors.BotColor.B = CourseData.SkyColors.BotColor.B;

            ThisCourse.SkyColors.SkyType = CourseData.SkyColors.SkyType;
            ThisCourse.SkyColors.WeatherType = CourseData.SkyColors.WeatherType;

            ThisCourse.MusicID = CourseData.MusicID;
            ThisCourse.OK64SongPath = CourseData.OK64SongPath;

            return ThisCourse;
        }


        public void LoadCourseXML(XmlDocument XMLDoc)
        {
            string ParentPath = "/SaveData/CourseSettings";
            TM64 Tarmac = new TM64();
            CourseData.Settings.Credits = Tarmac.LoadElement(XMLDoc, ParentPath, "Credits");
            CourseData.Settings.Name = Tarmac.LoadElement(XMLDoc, ParentPath, "CourseName");
            CourseData.Settings.PreviewPath = Tarmac.LoadElement(XMLDoc, ParentPath, "PreviewPath");
            CourseData.Settings.BannerPath = Tarmac.LoadElement(XMLDoc, ParentPath, "BannerPath");
            CourseData.GhostPath = Tarmac.LoadElement(XMLDoc, ParentPath, "GhostPath");
            CourseData.Gametype = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "GameType", "0"));
            CourseData.OK64HeaderData.WaterType = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "WaterType", "0"));
            CourseData.OK64HeaderData.WaterLevel = Convert.ToSingle(Tarmac.LoadElement(XMLDoc, ParentPath, "WaterLevel", "-80"));
            CourseData.ManualTempo = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "ManualTempo", "0"));

            CourseData.GoalBannerBool = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, ParentPath, "GoalBannerBool", "1"));
            CourseData.SkyboxBool = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, ParentPath, "SkyboxBool", "1"));

            CourseData.MapData.MinimapPath = Tarmac.LoadElement(XMLDoc, ParentPath, "MinimapPath");

            CourseData.MapData.MapCoord = new Assimp.Vector2D
                (Convert.ToSingle(Tarmac.LoadElement(XMLDoc, ParentPath, "MapCoordX", "260")),
                Convert.ToSingle(Tarmac.LoadElement(XMLDoc, ParentPath, "MapCoordY", "170"))
                );

            CourseData.MapData.StartCoord = new Assimp.Vector2D
                (Convert.ToSingle(Tarmac.LoadElement(XMLDoc, ParentPath, "StartCoordX", "6")),
                Convert.ToSingle(Tarmac.LoadElement(XMLDoc, ParentPath, "StartCoordY", "28"))
                );

            CourseData.MapData.LineCoord = new Assimp.Vector2D
                (Convert.ToSingle(Tarmac.LoadElement(XMLDoc, ParentPath, "LineCoordX", "0")),
                Convert.ToSingle(Tarmac.LoadElement(XMLDoc, ParentPath, "LineCoordY", "0"))
                );

            CourseData.MapData.MapScale = Convert.ToSingle(Tarmac.LoadElement(XMLDoc, ParentPath, "MapScale", "1.55"));

            CourseData.MapData.MapColor = new TM64_Geometry.OK64Color();
            CourseData.MapData.MapColor.R = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "MapColorR", "255"));
            CourseData.MapData.MapColor.G = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "MapColorG", "255"));
            CourseData.MapData.MapColor.B = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "MapColorB", "255"));

            CourseData.SkyColors.TopColor = new TM64_Geometry.OK64Color();
            CourseData.SkyColors.TopColor.R = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "SkyTopColorR", "128"));
            CourseData.SkyColors.TopColor.G = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "SkyTopColorG", "184"));
            CourseData.SkyColors.TopColor.B = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "SkyTopColorB", "248"));

            CourseData.SkyColors.MidColor = new TM64_Geometry.OK64Color();
            CourseData.SkyColors.MidColor.R = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "SkyMidColorR", "216"));
            CourseData.SkyColors.MidColor.G = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "SkyMidColorG", "232"));
            CourseData.SkyColors.MidColor.B = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "SkyMidColorB", "248"));

            CourseData.SkyColors.BotColor = new TM64_Geometry.OK64Color();
            CourseData.SkyColors.BotColor.R = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "SkyBotColorR", "0"));
            CourseData.SkyColors.BotColor.G = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "SkyBotColorG", "0"));
            CourseData.SkyColors.BotColor.B = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "SkyBotColorB", "0"));

            CourseData.SkyColors.SkyType = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "SkyType", "0"));
            CourseData.SkyColors.WeatherType = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "WeatherType", "0"));

            CourseData.MusicID = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "MusicID", "0"));
            CourseData.OK64SongPath = Tarmac.LoadElement(XMLDoc, ParentPath, "OK64SongPath");

            CourseData.Fog = new TM64_Course.OKFog();
            CourseData.Fog.FogToggle = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, ParentPath, "FogToggle", "0"));
            CourseData.Fog.FogColor = new TM64_Geometry.OK64Color();
            CourseData.Fog.FogColor.R = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "FogColorR", "240"));
            CourseData.Fog.FogColor.G = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "FogColorG", "240"));
            CourseData.Fog.FogColor.B = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "FogColorB", "240"));
            CourseData.Fog.FogColor.A = Convert.ToByte(Tarmac.LoadElement(XMLDoc, ParentPath, "FogColorA", "255"));

            CourseData.Fog.StartDistance = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, ParentPath, "FogStart", "900"));
            CourseData.Fog.StopDistance = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, ParentPath, "FogStop", "1000"));


            UpdateUI();
        }

        public void SaveCourseXML(XmlDocument XMLDoc, XmlElement Parent)
        {
            XmlElement CourseXML = XMLDoc.CreateElement("CourseSettings");
            Parent.AppendChild(CourseXML);
            TM64 Tarmac = new TM64();

            Tarmac.GenerateElement(XMLDoc, CourseXML, "Credits", CourseData.Settings.Credits);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "CourseName", CourseData.Settings.Name);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "PreviewPath", CourseData.Settings.PreviewPath);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "BannerPath", CourseData.Settings.BannerPath);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "GhostPath", CourseData.GhostPath);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "GameType", CourseData.Gametype);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "WaterType", CourseData.OK64HeaderData.WaterType);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "WaterLevel", CourseData.OK64HeaderData.WaterLevel);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "ManualTempo", CourseData.ManualTempo);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "GoalBannerBool", CourseData.GoalBannerBool);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "SkyboxBool", CourseData.SkyboxBool);

            Tarmac.GenerateElement(XMLDoc, CourseXML, "MinimapPath", CourseData.MapData.MinimapPath);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "MapCoordX", CourseData.MapData.MapCoord[0]);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "MapCoordY", CourseData.MapData.MapCoord[1]);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "StartCoordX", CourseData.MapData.StartCoord[0]);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "StartCoordY", CourseData.MapData.StartCoord[1]);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "LineCoordX", CourseData.MapData.LineCoord[0]);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "LineCoordY", CourseData.MapData.LineCoord[1]);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "MapScale", CourseData.MapData.MapScale);

            Tarmac.GenerateElement(XMLDoc, CourseXML, "MapColorR", CourseData.MapData.MapColor.R);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "MapColorG", CourseData.MapData.MapColor.G);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "MapColorB", CourseData.MapData.MapColor.B);

            Tarmac.GenerateElement(XMLDoc, CourseXML, "SkyTopColorR", CourseData.SkyColors.TopColor.R);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "SkyTopColorG", CourseData.SkyColors.TopColor.G);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "SkyTopColorB", CourseData.SkyColors.TopColor.B);

            Tarmac.GenerateElement(XMLDoc, CourseXML, "SkyMidColorR", CourseData.SkyColors.MidColor.R);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "SkyMidColorG", CourseData.SkyColors.MidColor.G);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "SkyMidColorB", CourseData.SkyColors.MidColor.B);

            Tarmac.GenerateElement(XMLDoc, CourseXML, "SkyBotColorR", CourseData.SkyColors.BotColor.R);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "SkyBotColorG", CourseData.SkyColors.BotColor.G);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "SkyBotColorB", CourseData.SkyColors.BotColor.B);

            Tarmac.GenerateElement(XMLDoc, CourseXML, "SkyType", CourseData.SkyColors.SkyType);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "WeatherType", CourseData.SkyColors.WeatherType);

            Tarmac.GenerateElement(XMLDoc, CourseXML, "MusicID", CourseData.MusicID);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "OK64SongPath", CourseData.OK64SongPath);

            Tarmac.GenerateElement(XMLDoc, CourseXML, "FogToggle", CourseData.Fog.FogToggle);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "FogColorR", CourseData.Fog.FogColor.R);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "FogColorG", CourseData.Fog.FogColor.G);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "FogColorB", CourseData.Fog.FogColor.B);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "FogColorA", CourseData.Fog.FogColor.A);

            Tarmac.GenerateElement(XMLDoc, CourseXML, "FogStart", CourseData.Fog.StartDistance);
            Tarmac.GenerateElement(XMLDoc, CourseXML, "FogStop", CourseData.Fog.StopDistance);

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


            CourseData.Settings.Credits = CreditsBox.Text;
            CourseData.Settings.Name = CourseNameBox.Text;
            CourseData.Settings.PreviewPath = previewBox.Text;
            CourseData.Settings.BannerPath = bannerBox.Text;
            CourseData.GhostPath = ghostBox.Text;            
            CourseData.Gametype = GameTypeBox.SelectedIndex;

            CourseData.Fog.FogToggle = Convert.ToInt16(FogToggleBox.Checked);

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

            if (float.TryParse(waterBox.Text, out ParseFloat))
            {
                CourseData.OK64HeaderData.WaterLevel = ParseFloat;
            }

            if (int.TryParse(TempoTTBox.Text, out ParseInt))
            {
                CourseData.ManualTempo = ParseInt;
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

            byte ParseByte;

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
            CourseData.PathSettings.PathEffects = PathFX.ToArray();

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

            PathFX = CourseData.PathSettings.PathEffects.ToList();
            int ParseInt, ParseInt2 = 0;
            float ParseFloat, ParseFloat2 = 0;

            CreditsBox.Text = CourseData.Settings.Credits;
            CourseNameBox.Text = CourseData.Settings.Name;
            previewBox.Text = CourseData.Settings.PreviewPath;
            bannerBox.Text = CourseData.Settings.BannerPath;
            ghostBox.Text = CourseData.GhostPath;
            GameTypeBox.SelectedIndex = CourseData.Gametype;
            WaterTypeBox.SelectedIndex = CourseData.OK64HeaderData.WaterType;
            waterBox.Text = CourseData.OK64HeaderData.WaterLevel.ToString();

            FogToggleBox.Checked = Convert.ToBoolean(CourseData.Fog.FogToggle);
            FogStartBox.Text = CourseData.Fog.StartDistance.ToString();
            FogEndBox.Text = CourseData.Fog.StopDistance.ToString();
            FogRBox.Text = CourseData.Fog.FogColor.R.ToString();
            FogGBox.Text = CourseData.Fog.FogColor.G.ToString();
            FogBBox.Text = CourseData.Fog.FogColor.B.ToString();
            FogABox.Text = CourseData.Fog.FogColor.A.ToString();




            SkyBoxCheckBox.Checked = Convert.ToBoolean(CourseData.SkyboxBool);


            
            
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
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void EchoIndexBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Backup = loaded;
            loaded = false;


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

            }
            UpdateCourse();
        }

        private void ColorPickAdjust_Click(object sender, EventArgs e)
        {
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
            }
            UpdateCourse();
        }

        private void EchoPowerBox_TextChanged_1(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void BombSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            weatherBox.Items.Clear();
            
            

            if (skyBox.SelectedIndex == 1)
            {
                weatherBox.Enabled = true;
                weatherBox.Items.Clear();

                weatherBox.Items.Add("Mario R.");
                weatherBox.Items.Add("Koopa B.");
                weatherBox.Items.Add("Moo Moo");
                weatherBox.Items.Add("Luigi R.");
                weatherBox.Items.Add("Royal R.");
                weatherBox.Items.Add("Sherbet");

                if (CourseData.SkyColors.WeatherType == -1)
                {
                    weatherBox.SelectedIndex = 0;
                }
                else
                {

                    if (CourseData.SkyColors.WeatherType < 6)
                    {
                        weatherBox.SelectedIndex = CourseData.SkyColors.WeatherType;
                    }
                    else
                    {
                        weatherBox.SelectedIndex = 0;
                    }
                }
            }
            else if (skyBox.SelectedIndex == 3)
            {
                weatherBox.Enabled = true;

                weatherBox.Items.Clear();

                weatherBox.Items.Add("Snow");
                weatherBox.Items.Add("Rain");

                if (CourseData.SkyColors.WeatherType == -1)
                {
                    weatherBox.SelectedIndex = 0;
                }
                else
                {

                    if (CourseData.SkyColors.WeatherType < 2)
                    {
                        weatherBox.SelectedIndex = CourseData.SkyColors.WeatherType;
                    }
                    else
                    {
                        weatherBox.SelectedIndex = 0;
                    }
                }

            }
            else
            {
                weatherBox.Enabled = true;

                weatherBox.Items.Clear();

                weatherBox.Items.Add("N/A");
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

        private void GameTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void WaterTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }
    }

}
