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
using System.Xml;

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
                NewObject.TypeIndex = OKObjectList[ObjectListBox.SelectedIndex].TypeIndex;
            }
            OKObjectList.Add(NewObject);
            int NewIndex = ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.TypeIndex].Name + ObjectListBox.Items.Count.ToString());
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
            if (-1 < NewObject.TypeIndex && NewObject.TypeIndex <= OKObjectTypeList.Count)
            {
                if (RandomZBox.Checked)
                {
                    Random RNG = new Random();
                    NewObject.OriginAngle[2] = Convert.ToInt16(RNG.Next(360));
                }
                int NewIndex = ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.TypeIndex].Name + ObjectListBox.Items.Count.ToString());
                
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
                ObjectTypeIndexBox.SelectedIndex = OKObjectList[ObjectListBox.SelectedIndex].TypeIndex;

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

                if (OKObjectList[ObjectListBox.SelectedIndex].TypeIndex == 5)
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

        public int LoadSettings(string[] ObjectSettings, int Version = 5)
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
                NewObject.TypeIndex = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.GameMode = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.BattlePlayer = Convert.ToInt16(ObjectSettings[ThisLine++]);
                NewObject.ObjectiveClass = Convert.ToInt16(ObjectSettings[ThisLine++]);

                if (Version >= 6)
                {
                    NewObject.Flag = Convert.ToInt16(ObjectSettings[ThisLine++]);
                }

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
                int NewIndex = ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.TypeIndex].Name + ObjectListBox.Items.Count.ToString());
            }
            if (ObjectListBox.Items.Count > 0)
            {
                ObjectListBox.SelectedIndex = 0;
            }
            Loading = false;
            UpdateObjectUI();

            return ThisLine;
        }

        public void AddObjectToArray(string TargetFile)
        {
            TM64.OK64Settings okSettings = new TM64.OK64Settings();
            OpenFileDialog FileOpen = new OpenFileDialog();
            FileOpen.InitialDirectory = okSettings.ProjectDirectory;

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

        public void LoadObjectSettings(MemoryStream memoryStream)
        {
            
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            int ObjectTypeCount = binaryReader.ReadInt32();

            OKObjectTypeList.Clear();
            ObjectTypeIndexBox.Items.Clear();
            DefaultOKObjects();


            //error handler
            

            for (int This = 0; This < ObjectTypeCount; This++)
            {
                string TargetFile = binaryReader.ReadString();
                AddObjectToArray(TargetFile);
            }




            OKObjectList.Clear();
            ObjectListBox.Items.Clear();

            int ObjectCount = binaryReader.ReadInt32();
            for (int ThisObj = 0; ThisObj < ObjectCount; ThisObj++)
            {
                TM64_Course.OKObject NewObj = new TM64_Course.OKObject(memoryStream);
                OKObjectList.Add(NewObj);
                ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObj.TypeIndex].Name + ObjectListBox.Items.Count.ToString());
            }
        }
        public void LoadObjectXML(XmlDocument XMLDoc)
        {
            OKObjectTypeList.Clear();
            ObjectTypeIndexBox.Items.Clear();
            DefaultOKObjects();


            string ParentPath = "/SaveFile/ObjectData";
            TM64 Tarmac = new TM64();
            int TypeCount = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "TypeCount", "0"));
            string TypeArrayPath = "/SaveFile/ObjectData/ObjectTypeArray";
            for (int This = 0; This < TypeCount; This++)
            {
                string Target = Tarmac.LoadElement(XMLDoc, TypeArrayPath, "TypePath" + (This).ToString(), "NULL");
                AddObjectToArray(Target);
            }



            OKObjectList.Clear();
            ObjectListBox.Items.Clear();

            int ObjectCount = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "ObjectCount", "0"));
            string ObjectArrayPath = "/SaveFile/ObjectData/ObjectArray";
            for (int This = 0; This < ObjectCount; This++)
            {
                TM64_Course.OKObject NewObj = new TM64_Course.OKObject(XMLDoc, ObjectArrayPath, This);
                OKObjectList.Add(NewObj);
                ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObj.TypeIndex].Name + ObjectListBox.Items.Count.ToString());
            }
        }
        public void SaveObjectXML(XmlDocument XMLDoc, XmlElement Parent)
        {
            XmlElement ObjectXML = XMLDoc.CreateElement("ObjectData");
            Parent.AppendChild(ObjectXML);
            TM64 Tarmac = new TM64();
            Tarmac.GenerateElement(XMLDoc, ObjectXML, "TypeCount", (OKObjectTypeList.Count - 6));

            XmlElement TypeArray = XMLDoc.CreateElement("ObjectTypeArray");
            ObjectXML.AppendChild(TypeArray);
            for (int This = 6; This < OKObjectTypeList.Count; This++)
            {
                Tarmac.GenerateElement(XMLDoc, TypeArray, "TypePath" + (This-6).ToString(), OKObjectTypeList[This].Path);
            }


            XmlElement ObjectArray = XMLDoc.CreateElement("ObjectArray");
            Tarmac.GenerateElement(XMLDoc, ObjectXML, "ObjectCount", OKObjectList.Count);
            ObjectXML.AppendChild(ObjectArray);
            for (int This = 0; This < OKObjectList.Count; This++)
            {
                OKObjectList[This].SaveXML(XMLDoc, ObjectArray, This);
            }
        }
        public byte[] SaveObjectSettings()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);



            binaryWriter.Write((OKObjectTypeList.Count - 6));
            for (int This = 6; This < OKObjectTypeList.Count; This++)
            {   
                binaryWriter.Write(OKObjectTypeList[This].Path);
            }

            binaryWriter.Write(OKObjectList.Count);
            for (int This = 0; This < OKObjectList.Count; This++)
            {
                binaryWriter.Write(OKObjectList[This].SaveObjectType());
            }

            return memoryStream.ToArray();
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

                if (OKObjectList[ObjectListBox.SelectedIndex].TypeIndex == 5)
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
                OKObjectList[ObjectListBox.SelectedIndex].TypeIndex = Convert.ToInt16(ObjectTypeIndexBox.SelectedIndex);
                ObjectListBox.Items[ObjectListBox.SelectedIndex] = ("Object " + OKObjectTypeList[OKObjectList[ObjectListBox.SelectedIndex].TypeIndex].Name + ObjectListBox.SelectedIndex.ToString());
                
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
                  .Where(o => o.Item.TypeIndex == ObjectTypeIndexBox.SelectedIndex)
                  .Select(o => o.Index);
                int ListOffset = 0;
                foreach (int i in indexes)
                {
                    ObjectListBox.Items.RemoveAt(i - ListOffset);
                    ListOffset++;
                }


                OKObjectList.RemoveAll(x => x.TypeIndex == ObjectTypeIndexBox.SelectedIndex);




                indexes = OKObjectList.Select((item, index) => new { Item = item, Index = index })
                  .Where(o => o.Item.TypeIndex > ObjectTypeIndexBox.SelectedIndex)
                  .Select(o => o.Index);
                foreach (int i in indexes)
                {
                    OKObjectList[i].TypeIndex--;
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
                ObjectListBox.Items[j] = ("Object " + OKObjectTypeList[obj.TypeIndex].Name + j.ToString());
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

        
        private void ImpPOPButton_Click(object sender, EventArgs e)
        {
            if ((ObjectTypeIndexBox.SelectedIndex >= 0) && (ObjectTypeIndexBox.SelectedIndex <= ObjectTypeIndexBox.Items.Count))
            {


                TM64_Paths.Pathlist[] PathArray = new TM64_Paths.Pathlist[0];
                TM64.OK64Settings okSettings = new TM64.OK64Settings();
                TM64_Paths tm64Path = new TM64_Paths();

                //blank
                TM64_Geometry.OK64F3DObject[] surfaceObjects = new TM64_Geometry.OK64F3DObject[0];

                okSettings.LoadSettings();

                OpenFileDialog OpenFile = new OpenFileDialog();
                MessageBox.Show("Select OK64.POP File");
                OpenFile.Title = "POP3 File";
                OpenFile.InitialDirectory = okSettings.ProjectDirectory;
                OpenFile.Filter = "Tarmac POP|*.OK64.POP|All Files (*.*)|*.*";
                OpenFile.FileName = null;
                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(OpenFile.FileName))
                    {
                        string popFile = OpenFile.FileName;
                        PathArray = tm64Path.LoadPOP3(popFile, surfaceObjects);
                    }
                }

                for (int ThisLoop = 0; ThisLoop < PathArray.Length; ThisLoop++)
                {
                    for (int ThisPass = 0; ThisPass < PathArray[ThisLoop].pathmarker.Count; ThisPass++)
                    {

                        TM64_Course.OKObject NewObject = TarmacCourse.NewOKObject();

                        NewObject.OriginPosition[0] = Convert.ToInt16(PathArray[ThisLoop].pathmarker[ThisPass].X);
                        NewObject.OriginPosition[1] = Convert.ToInt16(PathArray[ThisLoop].pathmarker[ThisPass].Y);
                        NewObject.OriginPosition[2] = Convert.ToInt16(PathArray[ThisLoop].pathmarker[ThisPass].Z);

                        NewObject.Flag = Convert.ToInt16(PathArray[ThisLoop].pathmarker[ThisPass].Flag);

                        NewObject.TypeIndex = Convert.ToInt16(ObjectTypeIndexBox.SelectedIndex);
                        OKObjectList.Add(NewObject);
                        int NewIndex = ObjectListBox.Items.Add("Object " + OKObjectTypeList[NewObject.TypeIndex].Name + ObjectListBox.Items.Count.ToString());
                        ObjectListBox.SelectedIndex = NewIndex;
                    }
                }
            }

        }
    }
}
