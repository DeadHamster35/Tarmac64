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

namespace Tarmac64_Retail
{
    public partial class ObjectEditor : UserControl
    {
        public ObjectEditor()
        {
            InitializeComponent();
        }

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

            if (UpdateParent != null)
            {
                UpdateParent(this, EventArgs.Empty);
            }
        }

        public void CreateObject(TM64_Course.OKObject NewObject)
        {
            int NewIndex = ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.ObjectIndex].Name + ObjectListBox.Items.Count.ToString());
            ObjectListBox.SelectedIndex = NewIndex;
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
            ObjectIndexBox.Items.Add(NewItem.Name);

            NewItem = new TM64_Course.OKObjectType();
            NewItem.Name = "Tree";
            NewItem.ModelData = new TM64_Geometry.OK64F3DObject[1];
            NewItem.ModelData[0] = new TM64_Geometry.OK64F3DObject();
            NewItem.ModelData[0].modelGeometry = TarmacGeometry.CreateStandard(8.0f);
            NewItem.ModelData[0].objectColor = new float[] { 0f, 1.0f, 0f };
            NewItem.ModelScale = 1.0f;
            OKObjectTypeList.Add(NewItem);
            ObjectIndexBox.Items.Add(NewItem.Name);
            ObjectIndexBox.SelectedIndex = 0;

            NewItem = new TM64_Course.OKObjectType();
            NewItem.Name = "Piranha";
            NewItem.ModelData = new TM64_Geometry.OK64F3DObject[1];
            NewItem.ModelData[0] = new TM64_Geometry.OK64F3DObject();
            NewItem.ModelData[0].modelGeometry = TarmacGeometry.CreateStandard(6.0f);
            NewItem.ModelData[0].objectColor = new float[] { 1.0f, 0.45f, 0f };
            NewItem.ModelScale = 1.0f;
            OKObjectTypeList.Add(NewItem);
            ObjectIndexBox.Items.Add(NewItem.Name);
            ObjectIndexBox.SelectedIndex = 0;


            NewItem = new TM64_Course.OKObjectType();
            NewItem.Name = "Red Coin";
            NewItem.ModelData = new TM64_Geometry.OK64F3DObject[1];
            NewItem.ModelData[0] = new TM64_Geometry.OK64F3DObject();
            NewItem.ModelData[0].modelGeometry = TarmacGeometry.CreateStandard(2.0f);
            NewItem.ModelData[0].objectColor = new float[] { 1.0f, 0f, 0f };
            NewItem.ModelScale = 1.0f;
            OKObjectTypeList.Add(NewItem);
            ObjectIndexBox.Items.Add(NewItem.Name);
            ObjectIndexBox.SelectedIndex = 0;

            NewItem = new TM64_Course.OKObjectType();
            NewItem.Name = "Battle Objective";
            NewItem.ModelData = new TM64_Geometry.OK64F3DObject[1];
            NewItem.ModelData[0] = new TM64_Geometry.OK64F3DObject();
            NewItem.ModelData[0].modelGeometry = TarmacGeometry.CreateStandard(4.0f);
            NewItem.ModelData[0].objectColor = new float[] { 0f, 1.0f, 0.75f };
            NewItem.ModelScale = 1.0f;
            OKObjectTypeList.Add(NewItem);
            ObjectIndexBox.Items.Add(NewItem.Name);
            ObjectIndexBox.SelectedIndex = 0;
        }
        public void UpdateObjectUI()
        {
            if ((ObjectListBox.Items.Count > 0) && (ObjectListBox.SelectedIndex != -1) && (!Loading))
            {
                ObjectIndexBox.SelectedIndex = OKObjectList[ObjectListBox.SelectedIndex].ObjectIndex;

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

                FlagBox.Text = OKObjectList[ObjectListBox.SelectedIndex].ObjectFlag.ToString();
                BTypeBox.Text = OKObjectList[ObjectListBox.SelectedIndex].BattleType.ToString();
                BPlayerBox.Text = OKObjectList[ObjectListBox.SelectedIndex].BattlePlayer.ToString();
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
                BTypeBox.Text = "";
                BPlayerBox.Text = "";
            }

            if (UpdateParent != null)
            {
                UpdateParent(this, EventArgs.Empty);
            }
        }

        public int LoadSettings(string[] ObjectSettings)
        {
            Loading = true;
            int ThisLine = 0;
            int Count = Convert.ToInt32(ObjectSettings[ThisLine++]);
            OKObjectTypeList.Clear();
            ObjectIndexBox.Items.Clear();
            DefaultOKObjects();
            for (int This = 0; This < Count; This++)
            {                
                TM64_Course.OKObjectType NewType = TarmacCourse.LoadObjectType(ObjectSettings[ThisLine++]);
                OKObjectTypeList.Add(NewType);
                ObjectIndexBox.Items.Add(NewType.Name);
            }
            OKObjectList.Clear();
            ObjectListBox.Items.Clear();
            Count = Convert.ToInt32(ObjectSettings[ThisLine++]);
            
            for (int This = 0; This < Count; This++)
            {
                TM64_Course.OKObject NewObject = new TM64_Course.OKObject();
                NewObject.ObjectIndex = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.ObjectFlag = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.BattlePlayer = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.BattleType = Convert.ToInt16(ObjectSettings[ThisLine++]);
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
            ObjectListBox.SelectedIndex = 0;
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
                Output.Add(OKObjectList[This].ObjectFlag.ToString());
                Output.Add(OKObjectList[This].BattlePlayer.ToString());
                Output.Add(OKObjectList[This].BattleType.ToString());
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
                    OKObjectList[ObjectListBox.SelectedIndex].ObjectFlag = Parse;
                }
                if (short.TryParse(BPlayerBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].BattlePlayer = Parse;
                }
                if (short.TryParse(BTypeBox.Text, out Parse))
                {
                    OKObjectList[ObjectListBox.SelectedIndex].BattleType = Parse;
                }
            }
        }











        

        private void ObjectIndexBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((OKObjectList.Count > 0) && (ObjectListBox.SelectedIndex != -1))
            {
                OKObjectList[ObjectListBox.SelectedIndex].ObjectIndex = Convert.ToInt16(ObjectIndexBox.SelectedIndex);
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
            OpenFileDialog FileOpen = new OpenFileDialog();
            FileOpen.Filter = "Tarmac Object (*.ok64.OBJECT)|*.ok64.OBJECT|All Files (*.*)|*.*";
            FileOpen.DefaultExt = ".ok64.OBJECT";
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {                
                TM64_Course.OKObjectType NewType = TarmacCourse.LoadObjectType(FileOpen.FileName);
                OKObjectTypeList.Add(NewType);
                ObjectIndexBox.Items.Add(NewType.Name);
            }
                    

            UpdateObjectUI();
        }

        private void ObjectListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateObjectUI();
        }

        private void KillTypeBtn_Click(object sender, EventArgs e)
        {
            if (ObjectIndexBox.SelectedIndex > 3)
            {
                OKObjectTypeList.RemoveAt(ObjectIndexBox.SelectedIndex);
                ObjectIndexBox.Items.RemoveAt(ObjectIndexBox.SelectedIndex);
                ObjectIndexBox.SelectedIndex = ObjectIndexBox.Items.Count - 1;
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
    }
}
