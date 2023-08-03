using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tarmac64_Library;
using System.IO;
using System.Linq;

namespace Tarmac64_Retail
{
    public partial class ObjectEditor : UserControl
    {
        public ObjectEditor()
        {
            InitializeComponent();
        }

        string[] OKGameModeNames = new string[]
        {
            "Battle",
            "Capture the Flag",
            "Soccer"
        };


        string[] OKObjectiveClasses = new string[]
        {
            "Spawn",
            "Flag",
            "Base"
        };

        TM64_Course TarmacCourse = new TM64_Course();
        TM64 Tarmac = new TM64();
        TM64_GL TarmacGL = new TM64_GL();
        TM64_Geometry TarmacGeometry = new TM64_Geometry();
        public List<TM64_Course.OKObject> OKObjectList = new List<TM64_Course.OKObject>();
        public List<TM64_Course.OKObjectType> OKObjectTypeList = new List<TM64_Course.OKObjectType>();
        bool Loading = false;
        public event EventHandler UpdateParent;
        public event EventHandler UpdateZoomToTarget;
        public int ZoomIndex = -1;
        TM64_Geometry.Face[] StandardGeometry = new TM64_Geometry.Face[4];

        private void AddObjBtn_Click(object sender, EventArgs e)
        {
            TM64_Course.OKObject NewObject = TarmacCourse.NewOKObject();

            if ((ObjectListBox.SelectedIndex >= 0) && (ObjectListBox.SelectedIndex <= ObjectListBox.Items.Count))
            {
                NewObject.ObjectIndex = OKObjectList[ObjectListBox.SelectedIndex].ObjectIndex;
            }
            OKObjectList.Add(NewObject);
            int NewIndex = ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.ObjectIndex].Name + ObjectListBox.Items.Count.ToString());
            ObjectListBox.SelectedIndex = NewIndex;
        }

        private void ObjectEditor_Load(object sender, EventArgs e)
        {

            DefaultOKObjects();

            foreach (var GName in OKGameModeNames)
            {
                ModeBox.Items.Add(GName);
            }
            foreach (var GName in OKObjectiveClasses)
            {
                ClassBox.Items.Add(GName);
            }

            if (UpdateParent != null)
            {
                UpdateParent(this, EventArgs.Empty);
            }
        }

        public void CreateObject(TM64_Course.OKObject NewObject)
        {
            if (-1 < NewObject.ObjectIndex && NewObject.ObjectIndex <= OKObjectTypeList.Count)
            {
                if (RandomZBox.Checked)
                {
                    Random RNG = new Random();
                    NewObject.OriginAngle[2] = Convert.ToInt16(RNG.Next(360));
                }
                int NewIndex = ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.ObjectIndex].Name + ObjectListBox.Items.Count.ToString());
                
                ObjectListBox.SelectedIndex = NewIndex;
            }
            
        }


        public void RemoveObject(int ObjectIndex)
        {
            if (ObjectIndex > 0)
            {
                OKObjectList.RemoveAt(ObjectIndex);
                ObjectListBox.Items.RemoveAt(ObjectIndex);
            }
        }

        public void AddType(string OKTypePath)
        {
            
        }

        public void DefaultOKObjects()
        {
            TM64_Course.OKObjectType NewItem = new TM64_Course.OKObjectType();
            NewItem.Name = "Item Box";
            NewItem.ModelData = new TM64_Geometry.OK64F3DObject[1];
            NewItem.ModelData[0] = new TM64_Geometry.OK64F3DObject();
            NewItem.ModelData[0].modelGeometry = TarmacGeometry.CreateStandard(4.0f);
            NewItem.ModelData[0].objectColor = new float[] { 1.0f, 1.0f, 0.5f };
            NewItem.ModelScale = 1.0f;
            OKObjectTypeList.Add(NewItem);
            ObjectTypeIndexBox.Items.Add(NewItem.Name);

            NewItem = new TM64_Course.OKObjectType();
            NewItem.Name = "Tree";
            NewItem.ModelData = new TM64_Geometry.OK64F3DObject[1];
            NewItem.ModelData[0] = new TM64_Geometry.OK64F3DObject();
            NewItem.ModelData[0].modelGeometry = TarmacGeometry.CreateStandard(8.0f);
            NewItem.ModelData[0].objectColor = new float[] { 0f, 1.0f, 0f };
            NewItem.ModelScale = 1.0f;
            OKObjectTypeList.Add(NewItem);
            ObjectTypeIndexBox.Items.Add(NewItem.Name);
            ObjectTypeIndexBox.SelectedIndex = 0;

            NewItem = new TM64_Course.OKObjectType();
            NewItem.Name = "Piranha";
            NewItem.ModelData = new TM64_Geometry.OK64F3DObject[1];
            NewItem.ModelData[0] = new TM64_Geometry.OK64F3DObject();
            NewItem.ModelData[0].modelGeometry = TarmacGeometry.CreateStandard(6.0f);
            NewItem.ModelData[0].objectColor = new float[] { 1.0f, 0.45f, 0f };
            NewItem.ModelScale = 1.0f;
            OKObjectTypeList.Add(NewItem);
            ObjectTypeIndexBox.Items.Add(NewItem.Name);
            ObjectTypeIndexBox.SelectedIndex = 0;


            NewItem = new TM64_Course.OKObjectType();
            NewItem.Name = "Red Coin";
            NewItem.ModelData = new TM64_Geometry.OK64F3DObject[1];
            NewItem.ModelData[0] = new TM64_Geometry.OK64F3DObject();
            NewItem.ModelData[0].modelGeometry = TarmacGeometry.CreateStandard(2.0f);
            NewItem.ModelData[0].objectColor = new float[] { 1.0f, 0f, 0f };
            NewItem.ModelScale = 1.0f;
            OKObjectTypeList.Add(NewItem);
            ObjectTypeIndexBox.Items.Add(NewItem.Name);
            ObjectTypeIndexBox.SelectedIndex = 0;

            NewItem = new TM64_Course.OKObjectType();
            NewItem.Name = "Special Box";
            NewItem.ModelData = new TM64_Geometry.OK64F3DObject[1];
            NewItem.ModelData[0] = new TM64_Geometry.OK64F3DObject();
            NewItem.ModelData[0].modelGeometry = TarmacGeometry.CreateStandard(4.0f);
            NewItem.ModelData[0].objectColor = new float[] { 1.0f, 1.0f, 0.5f };
            NewItem.ModelScale = 1.0f;
            OKObjectTypeList.Add(NewItem);
            ObjectTypeIndexBox.Items.Add(NewItem.Name);

            NewItem = new TM64_Course.OKObjectType();
            NewItem.Name = "Battle Objective";
            NewItem.ModelData = new TM64_Geometry.OK64F3DObject[1];
            NewItem.ModelData[0] = new TM64_Geometry.OK64F3DObject();
            NewItem.ModelData[0].modelGeometry = TarmacGeometry.CreateStandard(4.0f);
            NewItem.ModelData[0].objectColor = new float[] { 0f, 1.0f, 0.75f };
            NewItem.ModelScale = 1.0f;
            OKObjectTypeList.Add(NewItem);
            ObjectTypeIndexBox.Items.Add(NewItem.Name);
            ObjectTypeIndexBox.SelectedIndex = 0;
        }
        public void UpdateObjectUI()
        {
            if ((ObjectListBox.Items.Count > 0) && (ObjectListBox.SelectedIndex != -1) && (!Loading))
            {
                ObjectTypeIndexBox.SelectedIndex = OKObjectList[ObjectListBox.SelectedIndex].ObjectIndex;

                LocationXBox.Text = OKObjectList[ObjectListBox.SelectedIndex].OriginPosition[0].ToString();
                LocationYBox.Text = OKObjectList[ObjectListBox.SelectedIndex].OriginPosition[1].ToString();
                LocationZBox.Text = OKObjectList[ObjectListBox.SelectedIndex].OriginPosition[2].ToString();

                AngleXBox.Text = OKObjectList[ObjectListBox.SelectedIndex].OriginAngle[0].ToString();
                AngleYBox.Text = OKObjectList[ObjectListBox.SelectedIndex].OriginAngle[1].ToString();
                AngleZBox.Text = OKObjectList[ObjectListBox.SelectedIndex].OriginAngle[2].ToString();

                VelocityXBox.Text = OKObjectList[ObjectListBox.SelectedIndex].Velocity[0].ToString();
                VelocityYBox.Text = OKObjectList[ObjectListBox.SelectedIndex].Velocity[1].ToString();
                VelocityZBox.Text = OKObjectList[ObjectListBox.SelectedIndex].Velocity[2].ToString();

                RotationXBox.Text = OKObjectList[ObjectListBox.SelectedIndex].AngularVelocity[0].ToString();
                RotationYBox.Text = OKObjectList[ObjectListBox.SelectedIndex].AngularVelocity[1].ToString();
                RotationZBox.Text = OKObjectList[ObjectListBox.SelectedIndex].AngularVelocity[2].ToString();

                FlagBox.Text = OKObjectList[ObjectListBox.SelectedIndex].Flag.ToString();

                if (OKObjectList[ObjectListBox.SelectedIndex].ObjectIndex == 4)
                {
                    ModeBox.SelectedIndex = OKObjectList[ObjectListBox.SelectedIndex].GameMode;
                    ClassBox.SelectedIndex = OKObjectList[ObjectListBox.SelectedIndex].ObjectiveClass;
                    BPlayerBox.Text = OKObjectList[ObjectListBox.SelectedIndex].BattlePlayer.ToString();
                }
            }
            else
            {
                LocationXBox.Text = "";
                LocationYBox.Text = "";
                LocationZBox.Text = "";

                AngleXBox.Text = "";
                AngleYBox.Text = "";
                AngleZBox.Text = "";

                VelocityXBox.Text = "";
                VelocityYBox.Text = ""; 
                VelocityZBox.Text = "";

                RotationXBox.Text = "";
                RotationYBox.Text = "";
                RotationZBox.Text = "";

                FlagBox.Text = "";
                ModeBox.SelectedIndex = -1;
                ClassBox.SelectedIndex = -1;
                BPlayerBox.Text = "";
            }

            if (UpdateParent != null)
            {
                UpdateParent(this, EventArgs.Empty);
            }
        }

        public int LoadSettings(string[] ObjectSettings)
        {

            TM64.OK64Settings okSettings = new TM64.OK64Settings();
            okSettings.LoadSettings();

            Loading = true;
            int ThisLine = 0;
            int Count = Convert.ToInt32(ObjectSettings[ThisLine++]);
            OKObjectTypeList.Clear();
            ObjectTypeIndexBox.Items.Clear();
            DefaultOKObjects();
            OpenFileDialog FileOpen = new OpenFileDialog();
            FileOpen.InitialDirectory = okSettings.ProjectDirectory;

            for (int This = 0; This < Count; This++)
            {
                string TargetFile = ObjectSettings[ThisLine++];
                if (!File.Exists(TargetFile))
                {
                    MessageBox.Show("Please select replacement OKObject for " + Environment.NewLine + TargetFile);
                    if (FileOpen.ShowDialog() == DialogResult.OK)
                    {
                        TargetFile = FileOpen.FileName;
                    }
                }

                TM64_Course.OKObjectType NewType = TarmacCourse.LoadObjectType(TargetFile);

                for (int ThisTexture = 0; ThisTexture < NewType.TextureData.Length; ThisTexture++)
                {
                    if (
                        (NewType.TextureData[ThisTexture].texturePath == null) ||
                        (!File.Exists(NewType.TextureData[ThisTexture].texturePath))
                    )
                    {
                        MessageBox.Show("Error loading texture " + NewType.TextureData[ThisTexture].textureName + " for " + NewType.Name);
                        if (FileOpen.ShowDialog() == DialogResult.OK)
                        {
                            if (FileOpen.FileName != null)
                            {
                                if (File.Exists(FileOpen.FileName))
                                {
                                    NewType.TextureData[ThisTexture].texturePath = FileOpen.FileName;
                                }
                                else
                                {
                                    NewType.TextureData[ThisTexture].texturePath = null;
                                    MessageBox.Show("File was not found - compiled map may be corrupt");
                                }
                            }
                        }
                    }
                }
                

                OKObjectTypeList.Add(NewType);
                ObjectTypeIndexBox.Items.Add(NewType.Name);
            }


            OKObjectList.Clear();
            ObjectListBox.Items.Clear();
            Count = Convert.ToInt32(ObjectSettings[ThisLine++]);
            
            for (int This = 0; This < Count; This++)
            {
                TM64_Course.OKObject NewObject = new TM64_Course.OKObject();
                NewObject.ObjectIndex = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.GameMode = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.BattlePlayer = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.ObjectiveClass = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.OriginPosition = new short[3];
                NewObject.OriginPosition[0] = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.OriginPosition[1] = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.OriginPosition[2] = Convert.ToInt16(ObjectSettings[ThisLine++]);

                NewObject.OriginAngle = new short[3];
                NewObject.OriginAngle[0] = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.OriginAngle[1] = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.OriginAngle[2] = Convert.ToInt16(ObjectSettings[ThisLine++]);

                NewObject.Velocity = new float[3];
                NewObject.Velocity[0] = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.Velocity[1] = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.Velocity[2] = Convert.ToInt16(ObjectSettings[ThisLine++]);

                NewObject.AngularVelocity = new short[3];
                NewObject.AngularVelocity[0] = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.AngularVelocity[1] = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.AngularVelocity[2] = Convert.ToInt16(ObjectSettings[ThisLine++]);

                OKObjectList.Add(NewObject);
                int NewIndex = ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.ObjectIndex].Name + ObjectListBox.Items.Count.ToString());
            }
            if (ObjectListBox.Items.Count > 0)
            {
                ObjectListBox.SelectedIndex = 0;
            }
            Loading = false;
            UpdateObjectUI();

            return ThisLine;
        }
        public string[] SaveSettings()
        {
            List<string> Output = new List<string>();

            Output.Add((OKObjectTypeList.Count - 6).ToString());
            for (int This = 6; This < OKObjectTypeList.Count; This++)
            {
                Output.Add(OKObjectTypeList[This].Path);
            }
            Output.Add(OKObjectList.Count.ToString());
            for (int This = 0; This < OKObjectList.Count; This++)
            {
                Output.Add(OKObjectList[This].ObjectIndex.ToString());
                Output.Add(OKObjectList[This].GameMode.ToString());
                Output.Add(OKObjectList[This].BattlePlayer.ToString());
                Output.Add(OKObjectList[This].ObjectiveClass.ToString());
                Output.Add(OKObjectList[This].OriginPosition[0].ToString());
                Output.Add(OKObjectList[This].OriginPosition[1].ToString());
                Output.Add(OKObjectList[This].OriginPosition[2].ToString());

                Output.Add(OKObjectList[This].OriginAngle[0].ToString());
                Output.Add(OKObjectList[This].OriginAngle[1].ToString());
                Output.Add(OKObjectList[This].OriginAngle[2].ToString());

                Output.Add(OKObjectList[This].Velocity[0].ToString());
                Output.Add(OKObjectList[This].Velocity[1].ToString());
                Output.Add(OKObjectList[This].Velocity[2].ToString());

                Output.Add(OKObjectList[This].AngularVelocity[0].ToString());
                Output.Add(OKObjectList[This].AngularVelocity[1].ToString());
                Output.Add(OKObjectList[This].AngularVelocity[2].ToString());
            }

            return Output.ToArray();
        }

        private void UpdateLocation()
        {
            if (ObjectListBox.SelectedIndex != -1)
            {
                short Parse = 0;
                if (short.TryParse(LocationXBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].OriginPosition[0] = Parse;
                }

                if (short.TryParse(LocationYBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].OriginPosition[1] = Parse;
                }

                if (short.TryParse(LocationZBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].OriginPosition[2] = Parse;
                }


                if (short.TryParse(AngleXBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].OriginAngle[0] = Parse;
                }

                if (short.TryParse(AngleYBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].OriginAngle[1] = Parse;
                }

                if (short.TryParse(AngleZBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].OriginAngle[2] = Parse;
                }

                float FloatParse = 0;
                if (float.TryParse(VelocityXBox.Text, out FloatParse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].Velocity[0] = FloatParse;
                }

                if (float.TryParse(VelocityYBox.Text, out FloatParse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].Velocity[1] = FloatParse;
                }

                if (float.TryParse(VelocityZBox.Text, out FloatParse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].Velocity[2] = FloatParse;
                }

                if (short.TryParse(RotationXBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].AngularVelocity[0] = Parse;
                }

                if (short.TryParse(RotationYBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].AngularVelocity[1] = Parse;
                }

                if (short.TryParse(RotationZBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].AngularVelocity[2] = Parse;
                }

                if (short.TryParse(FlagBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].Flag = Parse;
                }

                if (OKObjectList[ObjectListBox.SelectedIndex].ObjectIndex == 4)
                {
                    OKObjectList[ObjectListBox.SelectedIndex].GameMode = Convert.ToInt16(ModeBox.SelectedIndex);

                    if (short.TryParse(BPlayerBox.Text, out Parse))
                    {
                        OKObjectList[ObjectListBox.SelectedIndex].BattlePlayer = Parse;
                    }
                    OKObjectList[ObjectListBox.SelectedIndex].ObjectiveClass = Convert.ToInt16(ClassBox.SelectedIndex);
                }
                    
            }
        }











        

        private void ObjectIndexBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((OKObjectList.Count > 0) && (ObjectListBox.SelectedIndex != -1))
            {
                OKObjectList[ObjectListBox.SelectedIndex].ObjectIndex = Convert.ToInt16(ObjectTypeIndexBox.SelectedIndex);
                ObjectListBox.Items[ObjectListBox.SelectedIndex] = ("Object " + OKObjectTypeList[OKObjectList[ObjectListBox.SelectedIndex].ObjectIndex].Name + ObjectListBox.SelectedIndex.ToString());
                
            }
            UpdateObjectUI();
        }

        private void UpdateLocationHandler(object sender, KeyEventArgs e)
        {
            UpdateLocation();
            UpdateObjectUI();
        }

        private void KillObjBtn_Click(object sender, EventArgs e)
        {
            RemoveObject(ObjectListBox.SelectedIndex);
            UpdateObjectUI();
        }

        private void AddTypeBtn_Click(object sender, EventArgs e)
        {
            TM64.OK64Settings TarmacSettings = new TM64.OK64Settings();
            TarmacSettings.LoadSettings();

            OpenFileDialog FileOpen = new OpenFileDialog();
            FileOpen.Filter = "Tarmac Object (*.ok64.OBJECT)|*.ok64.OBJECT|All Files (*.*)|*.*";
            FileOpen.DefaultExt = ".ok64.OBJECT";
            FileOpen.InitialDirectory = TarmacSettings.ObjectDirectory;
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {                
                TM64_Course.OKObjectType NewType = TarmacCourse.LoadObjectType(FileOpen.FileName);
                OKObjectTypeList.Add(NewType);
                ObjectTypeIndexBox.Items.Add(NewType.Name);
            }
                    

            UpdateObjectUI();
        }

        private void ObjectListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateObjectUI();
        }

        private void KillTypeBtn_Click(object sender, EventArgs e)
        {
            if (ObjectTypeIndexBox.SelectedIndex > 5)
            {

                
                var indexes = OKObjectList.Select((item, index) => new { Item = item, Index = index })
                  .Where(o => o.Item.ObjectIndex == ObjectTypeIndexBox.SelectedIndex)
                  .Select(o => o.Index);
                int ListOffset = 0;
                foreach (int i in indexes)
                {
                    ObjectListBox.Items.RemoveAt(i - ListOffset);
                    ListOffset++;
                }


                OKObjectList.RemoveAll(x => x.ObjectIndex == ObjectTypeIndexBox.SelectedIndex);




                indexes = OKObjectList.Select((item, index) => new { Item = item, Index = index })
                  .Where(o => o.Item.ObjectIndex > ObjectTypeIndexBox.SelectedIndex)
                  .Select(o => o.Index);
                foreach (int i in indexes)
                {
                    OKObjectList[i].ObjectIndex--;
                }


                OKObjectTypeList.RemoveAt(ObjectTypeIndexBox.SelectedIndex);
                ObjectTypeIndexBox.Items.RemoveAt(ObjectTypeIndexBox.SelectedIndex);
                RefreshObjectListBox();
                if (ObjectTypeIndexBox.SelectedIndex == -1)
                {
                    ObjectTypeIndexBox.SelectedIndex = ObjectTypeIndexBox.Items.Count -1;
                }
            }

        }

        private void RefreshObjectListBox()
        {
            int j = 0;
            foreach (var obj in OKObjectList)
            {
                ObjectListBox.Items[j] = ("Object " + OKObjectTypeList[obj.ObjectIndex].Name + j.ToString());
                j++;
            }
        }

        private void ObjectListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ZoomIndex = -1;
            int Index = this.ObjectListBox.IndexFromPoint(e.Location);
            if (Index != System.Windows.Forms.ListBox.NoMatches)
            {
                ZoomIndex = Index;
            }
            UpdateZoomToTarget(sender, e);
        }

        private void label75_Click(object sender, EventArgs e)
        {

        }

        private void ClassBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
