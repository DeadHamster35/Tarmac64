using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Tarmac64_Library;
using System.Windows.Forms;
using System.IO;

namespace Tarmac64_Retail
{
    public partial class HitboxCompiler : Form
    {
        bool Loaded = false;
        AssimpContext importer = new AssimpContext();
        TM64_Course TarmacCourse = new TM64_Course();
        TM64_Geometry TarmacGeometry = new TM64_Geometry();
        TM64_Objects TarmacObject = new TM64_Objects();
        TM64 Tarmac = new TM64();
        TM64_Objects.OK64Collide[] Hitbox = new TM64_Objects.OK64Collide[0];

        string[] BoxTypes = new string[] { "Sphere", "Box"};
        string[] CollisionNames = new string[] { "NONE", "DEAD", "BUMP", "DAMAGE" };
        string[] StatusNames = new string[] { "None", "MapObjectHit", "LightningHit", "BooTranslucent", "BecomeBombOn", "BecomeBomb ", "FlattenedOn", "FlattenedOff", "MushroomBoost", "SpinOutSaveable", "SpinOut", "GreenShellHit", "RedShellHit", "Bonk", "StarOn", "GhostOn", "StarOff", "GhostOff" };
        int[] StatusValues = new int[] { -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        string[] EffectNames = new string[] { "None", "StateAnimMusicNote", "StateAnimCrash", "StateAnimPoomp", "StateAnimBoing", "StateAnimExplosion", "StateAnimBonkStars", "StateAnimLandingDust" };
        int[] EffectValues = new int[] { -1, 0, 1, 2, 3, 4, 5, 6 };
        TM64.OK64Settings TarmacSettings = new TM64.OK64Settings();
        public HitboxCompiler()
        {
            InitializeComponent();
        }

        private void LoadFBXClick(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(FileOpen.FileName))
                {
                    return;
                }

                Hitbox = TarmacObject.LoadHBK(FileOpen.FileName);
                
                foreach (var Hit in Hitbox)
                {
                    IndexBox.Items.Add(Hit.Name);
                }
                Loaded = true;
                IndexBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Error");
                Hitbox = new TM64_Objects.OK64Collide[0];
                IndexBox.Items.Clear();
            }
        }

        private void IndexBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Hitbox.Length > 0)
            {
                Loaded = false;
                int Index = IndexBox.SelectedIndex;
                OriginXBox.Text = Convert.ToString(Hitbox[Index].Origin[0]);
                OriginYBox.Text = Convert.ToString(Hitbox[Index].Origin[1]);
                OriginZBox.Text = Convert.ToString(Hitbox[Index].Origin[2]);

                StatusBox.SelectedIndex = Array.IndexOf(StatusValues,Hitbox[Index].Status);
                EffectBox.SelectedIndex = Array.IndexOf(EffectValues,Hitbox[Index].Effect);
                ColResultBox.SelectedIndex = Hitbox[Index].CollideResult;
                DmgResultBox.SelectedIndex = Hitbox[Index].HitResult;
                TypeBox.SelectedIndex = Hitbox[Index].Type;
                ScaleBox.Text = Convert.ToString(Hitbox[Index].Scale);
                if (Hitbox[Index].Type == 0)
                {
                    
                    SizeYBox.Enabled = false;
                    SizeZBox.Enabled = false;
                    SizeXBox.Text = Convert.ToString(Hitbox[Index].Size[0]);
                    SizeYBox.Text = "";
                    SizeZBox.Text = "";
                }
                else
                {
                    
                    SizeYBox.Enabled = true;
                    SizeZBox.Enabled = true;
                    SizeXBox.Text = Convert.ToString(Hitbox[Index].Size[0]);
                    SizeYBox.Text = Convert.ToString(Hitbox[Index].Size[1]);
                    SizeZBox.Text = Convert.ToString(Hitbox[Index].Size[2]);
                }
                Loaded = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TarmacSettings.LoadSettings();
            foreach (var Type in BoxTypes)
            {
                TypeBox.Items.Add(Type);
            }
            foreach (var Reaction in CollisionNames)
            {
                ColResultBox.Items.Add(Reaction);
                DmgResultBox.Items.Add(Reaction);
            }
            foreach (var Status in StatusNames)
            {
                StatusBox.Items.Add(Status);
            }
            foreach (var Effect in EffectNames)
            {
                EffectBox.Items.Add(Effect);
            }
            ScaleBox.Text = "0.1";
            TypeBox.SelectedIndex = 0;
            ColResultBox.SelectedIndex = 0;
            DmgResultBox.SelectedIndex = 0;
            StatusBox.SelectedIndex = 0;
            EffectBox.SelectedIndex = 0;
        }

        private void CompileClick(object sender, EventArgs e)
        {
            
            byte[] FileData = TarmacObject.SaveHitboxFile(Hitbox);
            SaveFileDialog FileSave = new SaveFileDialog();
            FileSave.InitialDirectory = TarmacSettings.ObjectDirectory;
            FileSave.Filter = "Tarmac Hitbox|*.ok64.HITBOX|All Files(*.*)|*.*";
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(FileSave.FileName, FileData);
            }
        }


        private void AddBtn_Click(object sender, EventArgs e)
        {
            List<TM64_Objects.OK64Collide> CurrentList = Hitbox.ToList();
            CurrentList.Insert(IndexBox.SelectedIndex + 1, new TM64_Objects.OK64Collide("CollisionSphere " + (Hitbox.Length + 1).ToString()));
            IndexBox.Items.Insert(IndexBox.SelectedIndex + 1, "CollisionSphere " + (Hitbox.Length + 1).ToString());
            Hitbox = CurrentList.ToArray();
            IndexBox.SelectedIndex = IndexBox.Items.Count - 1;
            Loaded = true;
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            List<TM64_Objects.OK64Collide> CurrentList = Hitbox.ToList();
            int Index = IndexBox.SelectedIndex;
            if (Index > 0)
            {


                IndexBox.Items.RemoveAt(Index);
                CurrentList.RemoveAt(Index);
                Hitbox = CurrentList.ToArray();

                if (Hitbox.Length == 0)
                {
                    IndexBox.SelectedIndex = -1;
                    IndexBox.Text = "";
                    Loaded = false;
                }
                else
                {
                    while (Index >= IndexBox.Items.Count)
                    {
                        Index--;
                    }
                    IndexBox.SelectedIndex = Index;
                }
            }
        }

        private void UpdateHitboxData()
        {
            if (Loaded)
            {
                int Index = IndexBox.SelectedIndex;
                Int16 Parse;
                Single ParseS;
                if (Int16.TryParse(OriginXBox.Text, out Parse))
                {
                    Hitbox[Index].Origin[0] = Parse;
                }
                if (Int16.TryParse(OriginYBox.Text, out Parse))
                {
                    Hitbox[Index].Origin[1] = Parse;
                }
                if (Int16.TryParse(OriginZBox.Text, out Parse))
                {
                    Hitbox[Index].Origin[2] = Parse;
                }
                if (Int16.TryParse(SizeXBox.Text, out Parse))
                {
                    Hitbox[Index].Size[0] = Parse;
                }
                if (Int16.TryParse(SizeYBox.Text, out Parse))
                {
                    Hitbox[Index].Size[1] = Parse;
                }
                if (Int16.TryParse(SizeZBox.Text, out Parse))
                {
                    Hitbox[Index].Size[2] = Parse;
                }
                if (Int16.TryParse(AngleZBox.Text, out Parse))
                {
                    Hitbox[Index].BoxAngle = Parse;
                }
                if (Single.TryParse(ScaleBox.Text, out ParseS))
                {
                    Hitbox[Index].Scale = ParseS;
                }

                Hitbox[Index].Type = Convert.ToInt16(TypeBox.SelectedIndex);
                Hitbox[Index].Status = Convert.ToInt16(StatusValues[StatusBox.SelectedIndex]);
                Hitbox[Index].Effect = Convert.ToInt16(EffectValues[EffectBox.SelectedIndex]);
                Hitbox[Index].HitResult = Convert.ToInt16(DmgResultBox.SelectedIndex);
                Hitbox[Index].CollideResult = Convert.ToInt16(ColResultBox.SelectedIndex);



                if (Hitbox[Index].Type == 0)
                {

                    SizeYBox.Enabled = false;
                    SizeZBox.Enabled = false;
                    SizeXBox.Text = Convert.ToString(Hitbox[Index].Size[0]);
                    SizeYBox.Text = "";
                    SizeZBox.Text = "";
                }
                else
                {

                    SizeYBox.Enabled = true;
                    SizeZBox.Enabled = true;
                    SizeXBox.Text = Convert.ToString(Hitbox[Index].Size[0]);
                    SizeYBox.Text = Convert.ToString(Hitbox[Index].Size[1]);
                    SizeZBox.Text = Convert.ToString(Hitbox[Index].Size[2]);
                }
            }

        }
        private void TypeBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            UpdateHitboxData();
        }

        private void UpdateHBData(object sender, KeyEventArgs e)
        {
            UpdateHitboxData();
        }
    }
}
