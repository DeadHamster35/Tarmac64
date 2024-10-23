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
using System.Reflection;
using Fluent;
using System.Windows.Forms.Design.Behavior;
using static Tarmac64_Library.TM64_Objects;
using System.Runtime.CompilerServices;

namespace Tarmac64_Retail
{
    public partial class ObjectTypeCompiler : Form
    {

        TM64_Objects.OK64Behavior[] BehaviorArray = new TM64_Objects.OK64Behavior[0];
        TM64_Objects.OK64Collide[] HitboxArray = new TM64_Objects.OK64Collide[0];


        string[] BoxTypes = new string[] { "Sphere", "Cylinder", "Box" };
        string[] CollisionNames = new string[] { "NONE", "DEAD", "BUMP", "DAMAGE"};
        string[] BehaviorNames = new string[] { "DEAD", "EXIST", "FLOAT", "PATH", "WANDER", "SEARCH", "BOUNCE", "FLEE", "STRAFE", "WATER-BOB"};
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
        TM64_Objects TarmacObject = new TM64_Objects();
        Scene ModelData = new Scene();
        TM64.OK64Settings TarmacSettings = new TM64.OK64Settings();
        OK64Parameter[] ParameterArray = new OK64Parameter[0];
        
        public ObjectTypeCompiler()
        {
            InitializeComponent();
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            FileOpen.InitialDirectory = TarmacSettings.ObjectDirectory;
            FileOpen.Filter = "FBX File|*.fbx|All Files|*.*";          
            
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                ModelBox.Text = FileOpen.FileName;

                ModelData = importer.ImportFile(ModelBox.Text, PostProcessPreset.TargetRealTimeMaximumQuality);
                TextureControl.textureArray = TarmacGeometry.loadTextures(ModelData, ModelBox.Text);
                TextureControl.AddNewTextures(TextureControl.textureArray.Length);
                TextureControl.Loaded = true;
            }
        }


        private void UpdateHitbox()
        {
            if (AddingHB)
            {
                return;
            }

            int Index = IndexBox.SelectedIndex;
            Int16 Parse;
            Single ParseS;
            if (Int16.TryParse(OriginXBox.Text, out Parse))
            {
                HitboxArray[Index].Origin[0] = Parse;
            }
            if (Int16.TryParse(OriginYBox.Text, out Parse))
            {
                HitboxArray[Index].Origin[1] = Parse;
            }
            if (Int16.TryParse(OriginZBox.Text, out Parse))
            {
                HitboxArray[Index].Origin[2] = Parse;
            }
            if (Int16.TryParse(SizeXBox.Text, out Parse))
            {
                HitboxArray[Index].Size[0] = Parse;
            }
            if (Int16.TryParse(SizeYBox.Text, out Parse))
            {
                HitboxArray[Index].Size[1] = Parse;
            }
            if (Int16.TryParse(SizeZBox.Text, out Parse))
            {
                HitboxArray[Index].Size[2] = Parse;
            }
            if (Int16.TryParse(AngleZBox.Text, out Parse))
            {
                HitboxArray[Index].BoxAngle = Parse;
            }
            if (Single.TryParse(ScaleBox.Text, out ParseS))
            {
                HitboxArray[Index].Scale = ParseS;
            }

            HitboxArray[Index].Type = Convert.ToInt16(TypeBox.SelectedIndex);
            HitboxArray[Index].Status = Convert.ToInt16(StatusValues[StatusBox.SelectedIndex]);
            HitboxArray[Index].Effect = Convert.ToInt16(EffectValues[EffectBox.SelectedIndex]);
            HitboxArray[Index].SolidObject = solidBox.Checked;

            switch (HitboxArray[Index].Type)
            {
                case 0:
                    {
                        //sphere
                        SizeYBox.Enabled = false;
                        SizeZBox.Enabled = false;
                        SizeXBox.Text = Convert.ToString(HitboxArray[Index].Size[0]);
                        SizeYBox.Text = "";
                        SizeZBox.Text = "";
                        break;
                    }
                case 1:
                    {
                        //cylinder
                        SizeYBox.Enabled = true;
                        SizeZBox.Enabled = false;
                        SizeXBox.Text = Convert.ToString(HitboxArray[Index].Size[0]);
                        SizeYBox.Text = Convert.ToString(HitboxArray[Index].Size[1]);
                        SizeZBox.Text = "";
                        break;
                    }
                case 2:
                    {
                        //cube
                        SizeYBox.Enabled = true;
                        SizeZBox.Enabled = true;
                        SizeXBox.Text = Convert.ToString(HitboxArray[Index].Size[0]);
                        SizeYBox.Text = Convert.ToString(HitboxArray[Index].Size[1]);
                        SizeZBox.Text = Convert.ToString(HitboxArray[Index].Size[2]);
                        break;
                    }
            }
        }


        private void BuildBtn_Click(object sender, EventArgs e)
        {

            SaveFileDialog FileSave = new SaveFileDialog();
            FileSave.Filter = "OK64Object|*.ok64.OBJECT|All Files(*.*)|*.";
            FileSave.DefaultExt = ".ok64.OBJECT";
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                TM64_Course.OKObjectType NewType = new TM64_Course.OKObjectType();
                NewType.Name = NameBox.Text;
                NewType.TextureData = TextureControl.textureArray;
                NewType.Flag = Convert.ToInt16(FlagBox.Text);
                NewType.ObjectHitbox = HitboxArray;

                float TempFloat;
                if (Single.TryParse(ScaleBox.Text, out TempFloat))
                {
                    NewType.ModelScale = TempFloat;
                }
                else
                {
                    MessageBox.Show("Scale Parsing Error. Check scale value.");
                    return;
                }


                NewType.BehaviorClass = Convert.ToInt16(BehaviorBox.SelectedIndex - 1);
                NewType.Behavior = BehaviorArray[NewType.BehaviorClass];

                List<TM64_Objects.OK64Parameter> ParameterList = (List<TM64_Objects.OK64Parameter>)ParameterView.Items;

                for (int ThisParameter = 0; ThisParameter < NewType.Behavior.Parameters.Length; ThisParameter++)
                {
                    NewType.Behavior.Parameters[ThisParameter].Value = ParameterList[ThisParameter].Value;
                }




                if (AToggleBox.Checked)
                {
                    NewType.ObjectAnimations = new TM64_Course.OKObjectAnimations();
                    var WalkData = importer.ImportFile(WalkBox.Text, PostProcessPreset.TargetRealTimeMaximumQuality);
                    NewType.ObjectAnimations.Animation = TarmacGeometry.LoadSkeleton(WalkData, NewType.ModelScale);
                }
                else
                {
                    NewType.ObjectAnimations = null;
                }

                NewType.ModelData = TarmacGeometry.CreateObjects(ModelData, NewType.TextureData, true);


                
                
                NewType.BumpRadius = Convert.ToInt16(Convert.ToInt16(LevelBump.Text) * 100);
                NewType.SoundID = SoundIDs[SoundNameBox.SelectedIndex];
                NewType.SoundRadius = Convert.ToInt16(SoundRangeBox.Text);
                NewType.SoundType = Convert.ToInt16(SoundTypeBox.SelectedIndex);
                NewType.RenderRadius = Convert.ToInt16(RenderBox.Text);
                if (GravityBox.Checked)
                {
                    NewType.GravityToggle = 1;
                }
                else 
                {
                    NewType.GravityToggle = 0;
                }
                //
                if (CameraAlignBox.Checked)
                {
                    NewType.CameraAlligned = 1;
                }
                else
                {
                    NewType.CameraAlligned = 0;
                }
                //
                if (ZSortBox.Checked)
                {
                    NewType.ZSortToggle = 1;
                }
                else
                {
                    NewType.ZSortToggle = 0;
                }

                File.WriteAllBytes(FileSave.FileName, Tarmac.CompressMIO0(TarmacCourse.SaveObjectType(NewType)));

            }
            

        }

        private void ResetParameterNames(object sender, CellEditEventArgs e)
        {
            if (e.SubItemIndex == 0)
            {
                e.Cancel = true;
            }
        }

        private void ResetParameterView()
        {
            
            ParameterView.Theme = OLVTheme.VistaExplorer;
            ParameterView.ItemFont = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);

            
            // display the the file name as the list item label
            ParameterView.Properties.Name = "Name";
            ParameterView.Properties.ColumnNames = new List<string>
            {
                "Value"
            };
            ParameterView.Properties.Columns = new List<string>
            {
                "Value"
            };
            ParameterView.Properties.Description = "Description";

            ParameterView.Items = new List<TM64_Objects.OK64Parameter>();
            ParameterView.EnableCellEditing = true;

            ParameterView.InnerList.CellEditActivation = Fluent.Lists.AdvancedListView.CellEditActivateMode.DoubleClick;

            for (int ThisPar = 0; ThisPar < BehaviorArray[BehaviorBox.SelectedIndex].Parameters.Length; ThisPar++)
            {
                OK64Parameter NewPar = new OK64Parameter();
                NewPar.Name = BehaviorArray[BehaviorBox.SelectedIndex].Parameters[ThisPar].Name;
                NewPar.Value = BehaviorArray[BehaviorBox.SelectedIndex].Parameters[ThisPar].Value;
                ParameterView.Items.Add(NewPar);
            }

            int NewHeight = 25 + (BehaviorArray[BehaviorBox.SelectedIndex].Parameters.Length * 25);
            int NewWidth = ParameterView.Width;
            ParameterView.Size = new Size(NewWidth,NewHeight);
            
            ParameterView.Redraw();
            ParameterView.InnerList.CellEditFinishing += new CellEditEventHandler(ResetParameterNames);
        }


        private void ObjectTypeCompiler_Load(object sender, EventArgs e)
        {
            
            foreach (var Status in StatusNames)
            {
                StatusBox.Items.Add(Status);
            }
            foreach (var Effect in EffectNames)
            {
                EffectBox.Items.Add(Effect);
            }
            
            foreach (var Types in BoxTypes)
            {
                TypeBox.Items.Add(Types);
            }

            TM64_Objects TM64Obj = new TM64_Objects();
            string BehaviorPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Resources", "Behaviors.XML");
            BehaviorArray = TM64Obj.LoadBehaviorXML(BehaviorPath);
            
            TarmacSettings.LoadSettings();

            for (int ThisBehavior = 0; ThisBehavior < BehaviorArray.Length; ThisBehavior++)
            {
                BehaviorBox.Items.Add(BehaviorArray[ThisBehavior].Name);
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
            SoundTypeBox.SelectedIndex = 0;
            SoundNameBox.SelectedIndex = 0;
            ResetParameterView();

            WalkBox.Enabled = false;
            NameBox.Focus();
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
            ResetParameterView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            FileOpen.InitialDirectory = TarmacSettings.ObjectDirectory;
            FileOpen.Filter = "FBX File|*.FBX|All Files(*.*)|*.*";
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                WalkBox.Text = FileOpen.FileName;
            }
        }

        private void AToggleBox_CheckedChanged(object sender, EventArgs e)
        {
            WalkBox.Enabled = AToggleBox.Checked;
        }


        private void AToggleBox_CheckedChanged_1(object sender, EventArgs e)
        {
            WalkBox.Enabled = AToggleBox.Checked;
        }

        private void HitboxBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            FileOpen.InitialDirectory = TarmacSettings.ObjectDirectory;
            FileOpen.Filter = "Tarmac Hitbox|*.ok64.HITBOX|All Files(*.*)|*.*";
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                //HitboxBox.Text = FileOpen.FileName;
            }
        }

        private void HitboxBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void FListView_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        bool AddingHB = false;
        private void AddBtn_Click(object sender, EventArgs e)
        {
            AddingHB = true;
            List<TM64_Objects.OK64Collide> CurrentList = HitboxArray.ToList();
            CurrentList.Insert(IndexBox.SelectedIndex + 1, new TM64_Objects.OK64Collide("CollisionSphere " + (HitboxArray.Length + 1).ToString()));
            IndexBox.Items.Insert(IndexBox.SelectedIndex + 1, "CollisionSphere " + (HitboxArray.Length + 1).ToString());
            HitboxArray = CurrentList.ToArray();
            IndexBox.SelectedIndex = IndexBox.Items.Count - 1;
            UpdateHBUI();
            AddingHB = false;
        }

        private void UpdateHBUI()
        {
            int Index = IndexBox.SelectedIndex;
            OriginXBox.Text = Convert.ToString(HitboxArray[Index].Origin[0]);
            OriginYBox.Text = Convert.ToString(HitboxArray[Index].Origin[1]);
            OriginZBox.Text = Convert.ToString(HitboxArray[Index].Origin[2]);

            StatusBox.SelectedIndex = Array.IndexOf(StatusValues, HitboxArray[Index].Status);
            EffectBox.SelectedIndex = Array.IndexOf(EffectValues, HitboxArray[Index].Effect);
            TypeBox.SelectedIndex = HitboxArray[Index].Type;
            ScaleBox.Text = Convert.ToString(HitboxArray[Index].Scale);
            solidBox.Checked = HitboxArray[Index].SolidObject;
            switch (HitboxArray[Index].Type)
            {
                case 0:
                    {
                        //sphere
                        SizeYBox.Enabled = false;
                        SizeZBox.Enabled = false;
                        SizeXBox.Text = Convert.ToString(HitboxArray[Index].Size[0]);
                        SizeYBox.Text = "";
                        SizeZBox.Text = "";
                        break;
                    }
                case 1:
                    {
                        //cylinder
                        SizeYBox.Enabled = true;
                        SizeZBox.Enabled = false;
                        SizeXBox.Text = Convert.ToString(HitboxArray[Index].Size[0]);
                        SizeYBox.Text = Convert.ToString(HitboxArray[Index].Size[1]);
                        SizeZBox.Text = "";
                        break;
                    }
                case 2:
                    {
                        //cube
                        SizeYBox.Enabled = true;
                        SizeZBox.Enabled = true;
                        SizeXBox.Text = Convert.ToString(HitboxArray[Index].Size[0]);
                        SizeYBox.Text = Convert.ToString(HitboxArray[Index].Size[1]);
                        SizeZBox.Text = Convert.ToString(HitboxArray[Index].Size[2]);
                        break;
                    }
            }
        }
        private void IndexBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AddingHB)
            {
                return;
            }
            if (HitboxArray.Length > 0)
            {
                UpdateHBUI();
            }
        }

        private void HitboxComboIndexChange(object sender, EventArgs e)
        {
            UpdateHitbox();
        }

        private void HitboxKeyUp(object sender, KeyEventArgs e)
        {
            UpdateHitbox();
        }

        private void solidBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHitbox();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
        private void button3_Click(object sender, EventArgs e)
        {

            SaveFileDialog FileSave = new SaveFileDialog();
            FileSave.Filter = "OK64Object|*.ok64.OBJECT|All Files(*.*)|*.";
            FileSave.DefaultExt = ".ok64.OBJECT";
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                TM64_Course.OKObjectType NewType = new TM64_Course.OKObjectType();
                NewType.Name = NameBox.Text;
                NewType.TextureData = TextureControl.textureArray;
                NewType.Flag = Convert.ToInt16(FlagBox.Text);
                if (HitboxBox.Text != "")
                {
                    NewType.ObjectHitbox = TarmacObject.LoadHitboxFile(File.ReadAllBytes(HitboxBox.Text));
                }
                else
                {
                    NewType.ObjectHitbox = null;
                }



                float TempFloat;
                if (Single.TryParse(ScaleBox.Text, out TempFloat))
                {
                    NewType.ModelScale = TempFloat;
                }
                else
                {
                    MessageBox.Show("Scale Parsing Error. Check scale value.");
                    return;
                }


                if (AToggleBox.Checked)
                {
                    NewType.ObjectAnimations = new TM64_Course.OKObjectAnimations();
                    var WalkData = importer.ImportFile(WalkBox.Text, PostProcessPreset.TargetRealTimeMaximumQuality);
                    NewType.ObjectAnimations.WalkAnimation = TarmacGeometry.LoadSkeleton(WalkData, NewType.ModelScale);

                    var TargetData = importer.ImportFile(TargetBox.Text, PostProcessPreset.TargetRealTimeMaximumQuality);
                    NewType.ObjectAnimations.TargetAnimation = TarmacGeometry.LoadSkeleton(TargetData, NewType.ModelScale);

                    var DeathData = importer.ImportFile(DeathBox.Text, PostProcessPreset.TargetRealTimeMaximumQuality);
                    NewType.ObjectAnimations.DeathAnimation = TarmacGeometry.LoadSkeleton(DeathData, NewType.ModelScale);
                }
                else
                {
                    NewType.ObjectAnimations = null;
                }

                NewType.ModelData = TarmacGeometry.CreateObjects(ModelData, NewType.TextureData, true);



                NewType.BehaviorClass = Convert.ToInt16(BehaviorBox.SelectedIndex - 1);
                NewType.Range = Convert.ToInt16(RangeBox.Text);
                NewType.Sight = Convert.ToInt16(SightBox.Text);
                NewType.Viewcone = Convert.ToInt16(Viewconebox.Text);
                NewType.MaxSpeed = Convert.ToSingle(SpeedBox.Text);
                NewType.BumpRadius = Convert.ToInt16(Convert.ToInt16(LevelBump.Text) * 100);
                NewType.SoundID = SoundIDs[SoundNameBox.SelectedIndex];
                NewType.SoundRadius = Convert.ToInt16(SoundRangeBox.Text);
                NewType.SoundType = Convert.ToInt16(SoundTypeBox.SelectedIndex);
                NewType.RenderRadius = Convert.ToInt16(RenderBox.Text);
                if (GravityBox.Checked)
                {
                    NewType.GravityToggle = 1;
                }
                else
                {
                    NewType.GravityToggle = 0;
                }
                //
                if (CameraAlignBox.Checked)
                {
                    NewType.CameraAlligned = 1;
                }
                else
                {
                    NewType.CameraAlligned = 0;
                }
                //
                if (ZSortBox.Checked)
                {
                    NewType.ZSortToggle = 1;
                }
                else
                {
                    NewType.ZSortToggle = 0;
                }

                File.WriteAllLines(FileSave.FileName, TarmacGeometry.WriteDebugAnimation(NewType.ObjectAnimations.WalkAnimation));

            }

        }
    }
}
