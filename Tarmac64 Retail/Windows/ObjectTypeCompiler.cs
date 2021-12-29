using Assimp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tarmac64_Library;
using System.IO;

namespace Tarmac64_Retail
{
    public partial class ObjectTypeCompiler : Form
    {
        string[] BehaviorNames = new string[] { "DEAD", "EXIST", "FLOAT", "PATH", "WANDER", "SEARCH", "BOUNCE", "BEHAVIOR 7", "BEHAVIOR 8", "BEHAVIOR 9", 
            "BEHAVIOR 10", "BEHAVIOR 11", "BEHAVIOR 12", "BEHAVIOR 13", "BEHAVIOR 14", "BEHAVIOR 15", "BEHAVIOR 16", "BEHAVIOR 17", "BEHAVIOR 18", "BEHAVIOR 19", 
            "BEHAVIOR 20", "BEHAVIOR 21", "BEHAVIOR 22", "BEHAVIOR 23", "BEHAVIOR 24", "BEHAVIOR 25", "BEHAVIOR 26", "BEHAVIOR 27", "BEHAVIOR 28", "BEHAVIOR 29",
            "BEHAVIOR 30", "BEHAVIOR 31", "BEHAVIOR 32", "BEHAVIOR 33", "BEHAVIOR 34", "BEHAVIOR 35", "BEHAVIOR 36", "BEHAVIOR 37", "BEHAVIOR 38", "BEHAVIOR 39",
            "BEHAVIOR 40", "BEHAVIOR 41", "BEHAVIOR 42", "BEHAVIOR 43", "BEHAVIOR 44", "BEHAVIOR 45", "BEHAVIOR 46", "BEHAVIOR 47", "BEHAVIOR 48", "BEHAVIOR 49",
            "BEHAVIOR 50", "BEHAVIOR 51", "BEHAVIOR 52", "BEHAVIOR 53", "BEHAVIOR 54", "BEHAVIOR 55", "BEHAVIOR 56", "BEHAVIOR 57", "BEHAVIOR 58", "BEHAVIOR 59",
            "BEHAVIOR 60", "BEHAVIOR 61", "BEHAVIOR 62", "BEHAVIOR 63", "BEHAVIOR 64", "BEHAVIOR 65", "BEHAVIOR 66", "BEHAVIOR 67", "BEHAVIOR 68", "BEHAVIOR 69",
            "BEHAVIOR 70", "BEHAVIOR 71", "BEHAVIOR 72", "BEHAVIOR 73", "BEHAVIOR 74", "BEHAVIOR 75", "BEHAVIOR 76", "BEHAVIOR 77", "BEHAVIOR 78", "BEHAVIOR 79",
            "BEHAVIOR 80", "BEHAVIOR 81", "BEHAVIOR 82", "BEHAVIOR 83", "BEHAVIOR 84", "BEHAVIOR 85", "BEHAVIOR 86", "BEHAVIOR 87", "BEHAVIOR 88", "BEHAVIOR 89",
            "BEHAVIOR 90", "BEHAVIOR 91", "BEHAVIOR 92", "BEHAVIOR 93", "BEHAVIOR 94", "BEHAVIOR 95", "BEHAVIOR 96", "BEHAVIOR 97", "BEHAVIOR 98", "BEHAVIOR 99",
        };
        string[] StatusNames = new string[] { "None", "MapObjectHit", "LightningHit", "BooTranslucent", "BecomeBombOn", "BecomeBombOff", "FlattenedOn", "FlattenedOff", "MushroomBoost", "SpinOutSaveable", "SpinOut", "GreenShellHit", "RedShellHit", "Bonk", "StarOn", "GhostOn", "StarOff", "GhostOff" };
        int[] StatusValues = new int[] { -1, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        string[] EffectNames = new string[] { "None", "StateAnimMusicNote", "StateAnimCrash", "StateAnimPoomp", "StateAnimBoing", "StateAnimExplosion", "StateAnimBonkStars", "StateAnimLandingDust" };
        int[] EffectValues = new int[] { -1, 0, 1, 2, 3, 4, 5, 6 };
        string[] SoundTypes = new string[] { "Global", "Local" };
        string[] SoundNames = new string[] { "NONE", "SE_GLOBAL_THUNDER", "SE_GLOBAL_COIN", "SE_GLOBAL_FIREWORKS", "SE_GLOBAL_CHEEPCHEEP_CHARGE", 
            "SE_GLOBAL_CHEEPCHEEP_INTRO", "SE_GLOBAL_CHEEPCHEEP_FIRE", "SE_GLOBAL_PODIUM_JUMP", "SE_GLOBAL_STAR_SHIMMER", "SE_GLOBAL_CROWD_EEEH",
            "SE_KART_JUMP_BOING", "SE_KART_FEATHER_JUMP", "SE_KART_FINISH_RACE", "SE_KART_WATER_SPLASH", 
            "SE_KART_LANDING", "SE_KART_LANDING_BIG", "SE_KART_DASH", "SE_KART_OVER_DRIFT", "SE_KART_EXPLOSION", "SE_KART_EXPLOSION_BIG", 
            "SE_KART_FLATTENED_SWAYING", "SE_KART_SLIPSTREAM", "SE_KART_PUT_BANANA", "SE_KART_ITEM_THROW", "SE_KART_ITEM_THROW_HIGH", 
            "SE_KART_HIT_TREE", "SE_KART_HIT_WALL", "SE_KART_HIT_GUARD_RAIL", "SE_KART_HIT_HAND_RAIL", "SE_KART_HIT_METAL", "SE_KART_HIT_ICE", 
            "SE_KART_HIT_SHRUB", "SE_KART_HIT_PENGUIN", "SE_KART_HIT_THWOMP", "SE_KART_HIT_PIRANHA", "SE_2ND_LAP_FANFARE", "SE_FINAL_LAP_FANFARE", 
            "SE_FREEZING", "SE_ICE_CRUSH", "SE_THWOMP_LAND", "SE_THWOMP_DESTROY", "SE_BALLOON_LOST", "SE_ITEMBOX_DESTROY", "SE_ITEM_BOO_SWOOSH", 
            "SE_WATER_SPLASH", "SE_RAILROAD_CROSSING_DING", "SE_METAL_COFFIN", "SE_THUNDER", "SE_BOWSER_FIRE", "SE_BOWSER_FIRE_SMALL", 
            "SE_ITEM_BANANA_BUNCH_HIT", "SE_ITEM_SHELL_HIT", "SE_ITEM_ROULETTE_FINISH", "SE_UI_CAMERA_OUT", "SE_UI_CAMERA_IN", "SE_UI_ERROR_MISSING", 
            "SE_BOO_LAUGH", "SE_THWOMP_LAUGH", "SE_BAT_SINGLE", "SE_COW", "SE_MONTY_MOLE", "SE_SEAGULL", "SE_PENGUIN_BIG", "SE_PENGUIN_SMALL", 
            "SE_CHAIN_CHOMP", "SE_SEDAN_HORN_SINGLE", "SE_SEDAN_HORN_MULTI", "SE_BUS_TRUCK_HORN_SINGLE", "SE_BUS_TRUCK_HORN_MULTI", 
            "SE_TANKLORRY_HORN_SINGLE", "SE_TANKLORRY_HORN_MULTI", "SE_TRAIN_WHISTLE_SINGLE", "SE_TRAIN_WHISTLE_MULTI", "SE_BOAT_HORN_SINGLE", 
            "SE_BOAT_HORN_MULTI","SE_LAKITU_REVERSE", "SE_ITEM_ROULETTE", "SE_DRIVE_SPIN_OUT", "SE_DRIVE_ROPE_BRIDGE", 
            "SE_DRIVE_AB_SPIN", "SE_DRIVE_WOOD_BRIDGE", "SE_DRIVE_BRICKS", "SE_DRIVE_RAILWAY", "SE_DRIVE_BUBBLES", "SE_LEVEL_ENGINE_TRAIN", 
            "SE_LEVEL_ENGINE_BUS", "SE_LEVEL_ENGINE_TRUCK", "SE_LEVEL_ENGINE_TANKLORRY", "SE_LEVEL_ENGINE_SEDAN", "SE_LEVEL_BATS", 
            "SE_LEVEL_JUNGLE_NOISES", "SE_LEVEL_WATERFALL", "SE_LEVEL_AUDIENCE", "SE_STATE_THUNDERED", "SE_STATE_INVISIBLE", "SE_STATE_INVINCIBLE", 
            "SE_ITEM_BLUE_SHELL" };
        int[] SoundIDs = new int[] { -1, 0x1900f00c, 0x49008017, 0x4900801e, 0x4900801f, 0x49008020, 0x49008021, 0x49008022, 0x49008023, 0x4900802a,
            0x19008000, 0x19008002, 0x1900f103, 0x19008008, 0x1900a209, 0x1900a60a, 0x1900a40b, 0x1900851e, 0x19018010, 0x19009005, 0x0100f024, 
            0x19008011, 0x19008012, 0x19008004, 0x19018014, 0x19007018, 0x19007019, 0x1900701a, 0x1900701b, 0x19008001, 0x1900701d, 0x1900701f, 
            0x1900a046, 0x1900a04c, 0x1900a052, 0x1900f015, 0x1900ff3a, 0x1900a055, 0x1900a056, 0x1900800f, 0x1901a24a, 0x19009051, 0x19008406, 
            0x19009e59, 0x1900801c, 0x19017016, 0x1901904e, 0x1900f013, 0x51038009, 0x5102800a, 0x19019053, 0x19008054, 0x0100fe47, 0x1900904f, 
            0x19009050, 0x1900a058, 0x1900705a, 0x19036045, 0x19017044, 0x1901904d, 0x19018007, 0x19017043, 0x19007017, 0x19007049, 0x19018057, 
            0x1901703b, 0x1901703c, 0x1901703d, 0x1901703e, 0x19017041, 0x19017042, 0x1901800d, 0x1901800e, 0x19018047, 0x19018048, 0x0100fa28, 
            0x0100fe1c, 0x0100f81d, 0x0100f020, 0x0100f822, 0x0100f023, 0x51038007, 0x01008046, 0x0100f025, 0x51018000, 0x51018002, 0x51018003, 
            0x51018004, 0x51018005, 0x51028006, 0x0170802d, 0x51028001, 0x5103700b, 0x5101c00c, 0x0100fa4c, 0x0100ff2c, 0x51018008 };

        AssimpContext importer = new AssimpContext();
        TM64_Course TarmacCourse = new TM64_Course();
        TM64_Geometry TarmacGeometry = new TM64_Geometry();
        TM64 Tarmac = new TM64();
        Scene ModelData = new Scene();
        public ObjectTypeCompiler()
        {
            InitializeComponent();
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                ModelBox.Text = FileOpen.FileName;

                ModelData = importer.ImportFile(ModelBox.Text, PostProcessPreset.TargetRealTimeMaximumQuality);
                TextureControl.textureArray = TarmacGeometry.loadTextures(ModelData, ModelBox.Text);
                TextureControl.AddNewTextures(TextureControl.textureArray.Length);
                TextureControl.Loaded = true;
            }
        }

        private void BuildBtn_Click(object sender, EventArgs e)
        {

            SaveFileDialog FileSave = new SaveFileDialog();
            FileSave.Filter = "OK64Object| *.ok64.OBJECT";
            FileSave.DefaultExt = ".ok64.OBJECT";
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                TM64_Course.OKObjectType NewType = new TM64_Course.OKObjectType();
                NewType.Name = NameBox.Text;

                NewType.TextureData = TextureControl.textureArray;
                NewType.ModelData = TarmacGeometry.createObjects(ModelData, NewType.TextureData);

                if (AToggleBox.Checked)
                {
                    NewType.ObjectAnimations = new TM64_Course.OKObjectAnimations();
                    var WalkData = importer.ImportFile(WalkBox.Text, PostProcessPreset.TargetRealTimeMaximumQuality);
                    NewType.ObjectAnimations.WalkAnimation = TarmacGeometry.LoadSkeleton(WalkData);

                    var TargetData = importer.ImportFile(TargetBox.Text, PostProcessPreset.TargetRealTimeMaximumQuality);
                    NewType.ObjectAnimations.TargetAnimation = TarmacGeometry.LoadSkeleton(TargetData);

                    var DeathData = importer.ImportFile(DeathBox.Text, PostProcessPreset.TargetRealTimeMaximumQuality);
                    NewType.ObjectAnimations.DeathAnimation = TarmacGeometry.LoadSkeleton(DeathData);
                }
                else
                {
                    NewType.ObjectAnimations = null;
                }


                NewType.ModelScale = Convert.ToSingle(ScaleBox.Text);
                NewType.BehaviorClass = Convert.ToInt16(BehaviorBox.SelectedIndex -1);
                NewType.StatusClass = Convert.ToInt16(StatusValues[StatusBox.SelectedIndex]);
                NewType.EffectClass = Convert.ToInt16(EffectValues[EffectBox.SelectedIndex]);
                NewType.Range = Convert.ToInt16(RangeBox.Text);
                NewType.Sight = Convert.ToInt16(SightBox.Text);
                NewType.Viewcone = Convert.ToInt16(Viewconebox.Text);
                NewType.MaxSpeed = Convert.ToSingle(SpeedBox.Text);
                NewType.CollisionRadius = Convert.ToInt16(Convert.ToInt16(CollisionBox.Text) * 100);
                NewType.SoundID = SoundIDs[SoundNameBox.SelectedIndex];
                NewType.SoundRadius = Convert.ToInt16(SoundRangeBox.Text);
                NewType.SoundType = Convert.ToInt16(SoundTypeBox.SelectedIndex);
                NewType.RenderRadius = Convert.ToInt16(RenderBox.Text);
                NewType.Hitbox = Convert.ToInt16(Convert.ToInt16(HitBox.Text) * 100);
                if (GravityBox.Checked)
                {
                    NewType.GravityToggle = 1;
                }
                else 
                {
                    NewType.GravityToggle = 0;
                }
                if (CameraAlignBox.Checked)
                {
                    NewType.CameraAlligned = 1;
                }
                else
                {
                    NewType.CameraAlligned = 0;
                }

                File.WriteAllBytes(FileSave.FileName, Tarmac.CompressMIO0(TarmacCourse.SaveObjectType(NewType)));

            }
            

        }

        private void ObjectTypeCompiler_Load(object sender, EventArgs e)
        {
            foreach(var Behavior in BehaviorNames)
            {
                BehaviorBox.Items.Add(Behavior);
            }
            foreach (var Status in StatusNames)
            {
                StatusBox.Items.Add(Status);
            }
            foreach (var Effect in EffectNames)
            {
                EffectBox.Items.Add(Effect);
            }
            foreach (var Type in SoundTypes)
            {
                SoundTypeBox.Items.Add(Type);
            }
            foreach (var Name in SoundNames)
            {
                SoundNameBox.Items.Add(Name);
            }
            BehaviorBox.SelectedIndex = 1;
            StatusBox.SelectedIndex = 0;
            EffectBox.SelectedIndex = 0;
            SoundTypeBox.SelectedIndex = 0;
            SoundNameBox.SelectedIndex = 0;
            WalkBox.Enabled = false;
            TargetBox.Enabled = false;
            DeathBox.Enabled = false;
        }

        private void SoundNameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SoundNameBox.SelectedIndex < 10)
            {
                SoundTypeBox.SelectedIndex = 0;
                SoundTypeBox.Enabled = false;
            }
            else
            {
                SoundTypeBox.Enabled = true;
            }
        }

        private void BehaviorBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BehaviorBox.SelectedIndex == 3)
            {
                RangeLabel.Text = "Path";
            }
            else
            {
                RangeLabel.Text = "Range";
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                WalkBox.Text = FileOpen.FileName;
            }
        }

        private void AToggleBox_CheckedChanged(object sender, EventArgs e)
        {
            WalkBox.Enabled = AToggleBox.Checked;
            TargetBox.Enabled = AToggleBox.Checked;
            DeathBox.Enabled = AToggleBox.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                TargetBox.Text = FileOpen.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                DeathBox.Text = FileOpen.FileName;
            }
        }
    }
}
