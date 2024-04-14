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

        private void PathSettings_Load(object sender, EventArgs e)
        {


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

            if (int.TryParse(BombPointBox.Text, out ParseInt))
            {
                CourseData.BombArray[BombIndexBox.SelectedIndex].Point = Convert.ToInt16(ParseInt);
            }
            CourseData.BombArray[BombIndexBox.SelectedIndex].Type = Convert.ToInt16(BombTypeBox.SelectedIndex);

            CourseData.BombArray[BombIndexBox.SelectedIndex].Type = Convert.ToInt16(BombTypeBox.SelectedIndex);
            CourseData.BombArray[BombIndexBox.SelectedIndex].Point = Convert.ToInt16(BombPointBox.Text);
        }
        public void UpdateUI()
        {

            BombTypeBox.SelectedIndex = CourseData.BombArray[BombIndexBox.SelectedIndex].Type;
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

        private void ColorUpdate()
        {

            System.Drawing.Color buttonColor = System.Drawing.Color.FromArgb(0, 0, 0);
            int[] colorInt = new int[3];



        }
    }

}
