using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using Assimp;
using Tarmac64_Library;
using System.Text.RegularExpressions;
using Tarmac64_Library.Properties;
using SharpGL;
using SharpGL.SceneGraph.Core;
using System.Drawing.Design;
using System.Xml;
using System.Windows.Input;
using System.Drawing.Imaging;
using Cereal64.Microcodes.F3DEX.DataElements;
using Microsoft.WindowsAPICodePack.Dialogs;
using F3DSharp;
using System.Drawing.Drawing2D;
using Texture64;
using Fluent;
using System.Windows.Forms.VisualStyles;
using OverKart64_Retail.Windows;

namespace Tarmac64_Retail
{




    public partial class TextureEditor : UserControl
    {

        public TextureEditor()
        {
            InitializeComponent();
        }
        F3DEX095_Parameters F3DParam = new F3DEX095_Parameters();

        int lastMaterial = 0;
        public TM64_Geometry.OK64Texture[] textureArray = new TM64_Geometry.OK64Texture[0];
        //ColorCombineEditor
        ColorCombineEditor CCEdit = new ColorCombineEditor();
        

        public event EventHandler UpdateParent;
        public bool UpdateTextureCache = false;

        public bool Loaded = false, Locked = false;
        int[] PanelXY = new int[] { 5, 415 };
        public bool UpdateTextureDisplay()
        {
            if (Loaded)
            {

                
                bitm.Invalidate();
                bitm.Update();
                bitm.Refresh();
                Locked = true;
                if (textureArray[MaterialSelect.SelectedIndex].texturePath != null)
                {

                    alphaMaskBox.Text = textureArray[MaterialSelect.SelectedIndex].alphaPath;

                    heightBox.Text = textureArray[MaterialSelect.SelectedIndex].textureHeight.ToString();
                    widthBox.Text = textureArray[MaterialSelect.SelectedIndex].textureWidth.ToString();

                    SFlagBox.SelectedIndex = textureArray[MaterialSelect.SelectedIndex].SFlag;
                    TFlagBox.SelectedIndex = textureArray[MaterialSelect.SelectedIndex].TFlag;

                    textureScrollSBox.Text = textureArray[MaterialSelect.SelectedIndex].textureScrollS.ToString();
                    textureScrollTBox.Text = textureArray[MaterialSelect.SelectedIndex].textureScrollT.ToString();

                    CodecBox.SelectedIndex = textureArray[MaterialSelect.SelectedIndex].TextureFormat;
                    BitBox.SelectedIndex = textureArray[MaterialSelect.SelectedIndex].BitSize;
                }
                else
                {
                    alphaMaskBox.Text = "";
                    heightBox.Text = "";
                    widthBox.Text = "";
                    SFlagBox.SelectedIndex = -1;
                    TFlagBox.SelectedIndex = -1;
                    textureScrollSBox.Text = "";
                    textureScrollTBox.Text = "";
                    CodecBox.SelectedIndex = -1;
                    BitBox.SelectedIndex = -1;
                }

                CycleBox.SelectedIndex = textureArray[MaterialSelect.SelectedIndex].ColorCombine.CycleMode;
                FilterBox.SelectedIndex = textureArray[MaterialSelect.SelectedIndex].ColorCombine.TextureFilter;
                screenBox.SelectedIndex = textureArray[MaterialSelect.SelectedIndex].textureScreen;

                if (!textureArray[MaterialSelect.SelectedIndex].ColorCombine.AdvancedModeA)
                {
                    CombineBoxA.SelectedIndex = textureArray[MaterialSelect.SelectedIndex].ColorCombine.CombineModeA;
                }
                else
                {
                    CombineBoxA.SelectedIndex = -1;
                }

                if (!textureArray[MaterialSelect.SelectedIndex].ColorCombine.AdvancedModeB)
                {
                    CombineBoxB.SelectedIndex = textureArray[MaterialSelect.SelectedIndex].ColorCombine.CombineModeB;
                }
                else
                {
                    CombineBoxB.SelectedIndex = -1;
                }

                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    GeoModeBox.SetItemChecked(ThisCheck, textureArray[MaterialSelect.SelectedIndex].ColorCombine.GeometryBools[ThisCheck]);
                }

                RenderBoxA.SelectedIndex = textureArray[MaterialSelect.SelectedIndex].ColorCombine.RenderModeA;
                RenderBoxB.SelectedIndex = textureArray[MaterialSelect.SelectedIndex].ColorCombine.RenderModeB;


                EnvColorBTN.BackColor = textureArray[MaterialSelect.SelectedIndex].ColorCombine.Environment;
                PrimColorBTN.BackColor = textureArray[MaterialSelect.SelectedIndex].ColorCombine.Primary;
                EAlphaBox.Text = textureArray[MaterialSelect.SelectedIndex].ColorCombine.EnvironmentAlpha.ToString();
                PAlphaBox.Text = textureArray[MaterialSelect.SelectedIndex].ColorCombine.PrimaryAlpha.ToString();

                Locked = false;
                return true;

            }
            else
            {
                
                return false;
            }
            
        }

        public int AddNewTextures(int MaterialCount)
        {
            int textureCount = 0;
            MaterialSelect.Items.Clear();
            for (int materialIndex = 0; materialIndex < MaterialCount; materialIndex++)
            {
                if (textureArray[materialIndex].texturePath != null)
                {
                    MaterialSelect.Items.Add("Texture-" + materialIndex.ToString() + " " + textureArray[materialIndex].TexelData.textureName);                    
                    textureCount++;
                }
                else
                {
                    //MessageBox.Show("Warning! Material " + fbx.Materials[materialIndex].Name + " does not have a diffuse texture and cannot be used.");                    
                    MaterialSelect.Items.Add("Shaded- " + materialIndex.ToString() + " - " + textureArray[materialIndex].TexelData.textureName);                    
                }
                
            }
            MaterialSelect.SelectedIndex = 0;
            return textureCount;
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {
             
        }

        private void TextureEditor_Load(object sender, EventArgs e)
        {

            

            foreach (var ThisName in F3DEX095_Parameters.GCCModeNames)
            {                
                CombineBoxA.Items.Add(ThisName);
                CombineBoxB.Items.Add(ThisName);
            }
            foreach (var ThisName in F3DEX095_Parameters.BitSizeNames)
            {
                BitBox.Items.Add(ThisName);
            }
            foreach (var ThisName in F3DEX095_Parameters.TextureFormatNames)
            {
                CodecBox.Items.Add(ThisName);
            }
            foreach (var ThisName in F3DEX095_Parameters.TextureFilterNames)
            {
                FilterBox.Items.Add(ThisName);
            }
            foreach (var ThisName in F3DEX095_Parameters.RenderModeNamesSimple)
            {
                RenderBoxA.Items.Add(ThisName);
                RenderBoxB.Items.Add(ThisName);
            }

            foreach (var ThisName in F3DEX095_Parameters.GeometryModeNames)
            {
                GeoModeBox.Items.Add(ThisName, false);
            }
            
        }
        public void LoadTextureXML(XmlDocument XMLDoc)
        {
            string ParentPath = "/SaveFile/TextureArray";
            TM64 Tarmac = new TM64();
            int Count = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, ParentPath, "Count"));
            textureArray = new TM64_Geometry.OK64Texture[Count];
            for (int ThisTex = 0; ThisTex < textureArray.Length; ThisTex++)
            {
                textureArray[ThisTex] = new TM64_Geometry.OK64Texture(XMLDoc, ParentPath, (ThisTex));
            }
                
        }

        public void SaveTextureXML(XmlDocument XMLDoc, XmlElement Parent)
        {
            XmlElement TextureXML = XMLDoc.CreateElement("TextureArray");
            Parent.AppendChild(TextureXML);
            TM64 Tarmac = new TM64();
            Tarmac.GenerateElement(XMLDoc, TextureXML, "Count", textureArray.Length);
            for (int ThisTexture = 0; ThisTexture < textureArray.Length; ThisTexture++)
            {
                textureArray[ThisTexture].SaveXML(XMLDoc, TextureXML, ThisTexture);
            }
        }


        private void textureBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (UpdateTextureDisplay())
            {
                lastMaterial = MaterialSelect.SelectedIndex;
            }
            else
            {
                MaterialSelect.SelectedIndex = lastMaterial;
                MessageBox.Show("Selected Material Unavailable!");
            }
            
        }

        private void widthBox_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void textureScrollSBox_TextChanged(object sender, EventArgs e)
        {
            UpdateTextureData();          
        }

        private void textureScrollTBox_TextChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }

        private void screenBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }


        private void UpdateTextureData(bool NewItem = false, int NewIndex = -1)
        {
            if ((Loaded) && (!Locked))
            {
                int Parse;
                if (int.TryParse(textureScrollTBox.Text, out Parse))
                {
                    textureArray[MaterialSelect.SelectedIndex].textureScrollT = Parse;
                }
                if (int.TryParse(textureScrollSBox.Text, out Parse))
                {
                    textureArray[MaterialSelect.SelectedIndex].textureScrollS = Parse;
                }

                textureArray[MaterialSelect.SelectedIndex].textureScreen = screenBox.SelectedIndex;

                textureArray[MaterialSelect.SelectedIndex].ColorCombine.CycleMode = CycleBox.SelectedIndex;
                textureArray[MaterialSelect.SelectedIndex].ColorCombine.RenderModeA = RenderBoxA.SelectedIndex;
                textureArray[MaterialSelect.SelectedIndex].ColorCombine.RenderModeB = RenderBoxB.SelectedIndex;

                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    if (NewIndex == ThisCheck)
                    {
                        textureArray[MaterialSelect.SelectedIndex].ColorCombine.GeometryBools[ThisCheck] = NewItem;
                    }
                    else
                    {
                        textureArray[MaterialSelect.SelectedIndex].ColorCombine.GeometryBools[ThisCheck] = GeoModeBox.GetItemChecked(ThisCheck);
                    }
                }
                textureArray[MaterialSelect.SelectedIndex].SFlag = SFlagBox.SelectedIndex;
                textureArray[MaterialSelect.SelectedIndex].TFlag = TFlagBox.SelectedIndex;

                textureArray[MaterialSelect.SelectedIndex].BitSize = BitBox.SelectedIndex;
                textureArray[MaterialSelect.SelectedIndex].TextureFormat = CodecBox.SelectedIndex;
                textureArray[MaterialSelect.SelectedIndex].ColorCombine.TextureFilter = FilterBox.SelectedIndex;



                if (!textureArray[MaterialSelect.SelectedIndex].ColorCombine.AdvancedModeA)
                {
                    textureArray[MaterialSelect.SelectedIndex].ColorCombine.CombineModeA = CombineBoxA.SelectedIndex;
                }
                if (!textureArray[MaterialSelect.SelectedIndex].ColorCombine.AdvancedModeB)
                {
                    textureArray[MaterialSelect.SelectedIndex].ColorCombine.CombineModeB = CombineBoxB.SelectedIndex;
                }

            }
            
            if (UpdateParent != null)
            {
                UpdateTextureCache = false;
                UpdateParent(this, EventArgs.Empty);
            }
        }
        private void CombineBoxA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Loaded && (!Locked))
            {
                textureArray[MaterialSelect.SelectedIndex].ColorCombine.AdvancedModeA = false;
                UpdateTextureData();
            }
            
        }

        private void CombineBoxB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Loaded &&(!Locked))
            {
                textureArray[MaterialSelect.SelectedIndex].ColorCombine.AdvancedModeB = false;
                UpdateTextureData();
            }
        }

        private void RenderBoxA_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }

        private void RenderBoxB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }

        private void BitBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }

        private void SFlagBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }

        private void TFlagBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData(); 
        }


        private void GeoModeBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bool NewState = false;
            if (e.NewValue == CheckState.Checked)
            {
                NewState = true;
            }
            UpdateTextureData(NewState, e.Index);
        }


        private void OverWriteBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }

        private void bitm_Paint(object sender, PaintEventArgs e)
        {
            if ((Loaded) && (MaterialSelect.SelectedIndex >= 0))
            {
                if (!File.Exists(textureArray[MaterialSelect.SelectedIndex].texturePath))
                {
                    return;
                }
                if (textureArray[MaterialSelect.SelectedIndex].textureHeight == 0)
                {
                    return;
                }


                int THeight = textureArray[MaterialSelect.SelectedIndex].textureHeight;
                int TWidth = textureArray[MaterialSelect.SelectedIndex].textureWidth;

                if (THeight > TWidth)
                {
                    TWidth = Convert.ToInt32(bitm.Height * ((Convert.ToSingle(TWidth) / (Convert.ToSingle(THeight)))));
                    THeight = bitm.Height;
                }
                else
                {
                    THeight = Convert.ToInt32(bitm.Height * ((Convert.ToSingle(THeight) / (Convert.ToSingle(TWidth)))));
                    TWidth = bitm.Height;
                }
                int XOff = Convert.ToInt32((bitm.Width - TWidth) / 2.0f);
                int YOff = Convert.ToInt32((bitm.Height - THeight) / 2.0f);
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                Bitmap Draw = new Bitmap(textureArray[MaterialSelect.SelectedIndex].texturePath);
                e.Graphics.DrawImage(
                   Draw,
                    new Rectangle(XOff, YOff, TWidth, THeight),
                    // destination rectangle 
                    0,
                    0,           // upper-left corner of source rectangle
                    textureArray[MaterialSelect.SelectedIndex].textureWidth,       // width of source rectangle
                    textureArray[MaterialSelect.SelectedIndex].textureHeight,      // height of source rectangle
                    GraphicsUnit.Pixel);
            }
            else
            {
                
            }
        }


        private void AlphaMaskCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if ((Loaded) && (!Locked))
            {
                Locked = true;

                if (!AlphaMaskCheckbox.Checked)
                {
                    textureArray[MaterialSelect.SelectedIndex].alphaPath = "";
                }
                else
                {
                    OpenFileDialog FileOpen = new OpenFileDialog();
                    MessageBox.Show("Select Alpha Mask Texture");
                    if (FileOpen.ShowDialog()==DialogResult.OK)
                    {
                        if (File.Exists(FileOpen.FileName))
                        {
                            textureArray[MaterialSelect.SelectedIndex].alphaPath = FileOpen.FileName;
                        }
                    }

                }
                alphaMaskBox.Text = textureArray[MaterialSelect.SelectedIndex].alphaPath;
                Locked = false;
            }

            
        }

        private void FilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            TM64.OK64Settings okSettings = new TM64.OK64Settings();
            okSettings.LoadSettings();

            SaveFileDialog FileSave = new SaveFileDialog();
            FileSave.InitialDirectory = okSettings.ProjectDirectory;
            FileSave.Filter = "Tarmac Texture|*.ok64.Texture|All Files (*.*)|*.*";

            TM64_Geometry.OK64Texture Local = textureArray[MaterialSelect.SelectedIndex];
            
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                List<string> Output = new List<string>();

                Output.Add(Local.TexelData.textureName);
                Output.Add(Local.texturePath);
                Output.Add(Local.alphaPath);
                Output.Add(Local.ColorCombine.CombineModeA.ToString());
                Output.Add(Local.ColorCombine.CombineModeB.ToString());
                Output.Add(Local.ColorCombine.RenderModeA.ToString());
                Output.Add(Local.ColorCombine.RenderModeB.ToString());
                Output.Add(Local.ColorCombine.GeometryModes.ToString());
                Output.Add(Local.BitSize.ToString());
                Output.Add(Local.ColorCombine.TextureFilter.ToString());
                Output.Add(Local.TextureFormat.ToString());
                Output.Add(Local.SFlag.ToString());
                Output.Add(Local.TFlag.ToString());
                Output.Add(Local.textureScrollS.ToString());
                Output.Add(Local.textureScrollT.ToString());
                Output.Add(Local.textureScreen.ToString());
                Output.Add(Local.GLShiftS.ToString());
                Output.Add(Local.GLShiftT.ToString());


                File.WriteAllLines(FileSave.FileName, Output.ToArray());
            }
    
        }

        private void button4_Click(object sender, EventArgs e)
        {

            TM64.OK64Settings okSettings = new TM64.OK64Settings();
            okSettings.LoadSettings();

            OpenFileDialog FileOpen = new OpenFileDialog();
            FileOpen.InitialDirectory = okSettings.ProjectDirectory;
            FileOpen.Filter = "Tarmac Texture|*.ok64.Texture|All Files (*.*)|*.*";

            TM64_Geometry.OK64Texture Local = textureArray[MaterialSelect.SelectedIndex];
            
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                string[] Input = File.ReadAllLines(FileOpen.FileName);
                int ThisLine = 0;
                Local.TexelData.textureName = Input[ThisLine++];
                Local.texturePath = Input[ThisLine++];
                Local.alphaPath = Input[ThisLine++];
                Local.ColorCombine.CombineModeA = Convert.ToInt32(Input[ThisLine++]);
                Local.ColorCombine.CombineModeB = Convert.ToInt32(Input[ThisLine++]);

                Local.ColorCombine.RenderModeA = Convert.ToInt32(Input[ThisLine++]);
                Local.ColorCombine.RenderModeB = Convert.ToInt32(Input[ThisLine++]);

                Local.ColorCombine.GeometryModes = Convert.ToUInt32(Input[ThisLine++]);
                Local.BitSize = Convert.ToInt32(Input[ThisLine++]);
                Local.ColorCombine.TextureFilter = Convert.ToInt32(Input[ThisLine++]);
                Local.TextureFormat = Convert.ToInt32(Input[ThisLine++]);
                Local.SFlag = Convert.ToInt32(Input[ThisLine++]);
                Local.TFlag = Convert.ToInt32(Input[ThisLine++]);
                Local.textureScrollS = Convert.ToInt32(Input[ThisLine++]);
                Local.textureScrollT = Convert.ToInt32(Input[ThisLine++]);
                Local.textureScreen = Convert.ToInt32(Input[ThisLine++]);
                Local.GLShiftS = Convert.ToInt32(Input[ThisLine++]);
                Local.GLShiftT = Convert.ToInt32(Input[ThisLine++]);

                if (File.Exists(Local.texturePath))
                {
                    using (var fs = new FileStream(Local.texturePath, FileMode.Open, FileAccess.Read))
                    {
                        Local.TexelData.textureBitmap = Image.FromStream(fs);
                    }
                }
                Local.textureWidth = Local.TexelData.textureBitmap.Width;
                Local.textureHeight = Local.TexelData.textureBitmap.Height;

                int materialIndex = MaterialSelect.SelectedIndex;


                if (Local.texturePath != null)
                {
                    MaterialSelect.Items[materialIndex] = ("Texture-" + materialIndex.ToString() + " " + Local.TexelData.textureName);
                }
                else
                {
                    //MessageBox.Show("Warning! Material " + fbx.Materials[materialIndex].Name + " does not have a diffuse texture and cannot be used.");                    
                    MaterialSelect.Items[materialIndex] = ("Shaded- " + materialIndex.ToString() + " - " + Local.TexelData.textureName);
                }


                textureArray[materialIndex] = Local;
                if (UpdateParent != null)
                {
                    UpdateTextureCache = true;
                    UpdateParent(this, EventArgs.Empty);
                }
            }
        }

        private void TexelReplaceBTN_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(FileOpen.FileName))
                {
                    TM64_Geometry.OK64Texture Local = textureArray[MaterialSelect.SelectedIndex];
                    Local.texturePath = FileOpen.FileName;

                    if (File.Exists(Local.texturePath))
                    {
                        using (var fs = new FileStream(Local.texturePath, FileMode.Open, FileAccess.Read))
                        {
                            Local.TexelData.textureBitmap = Image.FromStream(fs);
                        }
                    }
                    Local.textureWidth = Local.TexelData.textureBitmap.Width;
                    Local.textureHeight = Local.TexelData.textureBitmap.Height;

                    textureArray[MaterialSelect.SelectedIndex] = Local;
                    if (UpdateParent != null)
                    {
                        UpdateTextureCache = true;
                        UpdateParent(this, EventArgs.Empty);
                    }

                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Loaded)
            {
                return;
            }

            ColorDialog ColorPick = new ColorDialog();
            if (ColorPick.ShowDialog() == DialogResult.OK) 
            {
                textureArray[MaterialSelect.SelectedIndex].ColorCombine.Environment = ColorPick.Color;                
            }
            UpdateTextureDisplay();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!Loaded)
            {
                return;
            }

            ColorDialog ColorPick = new ColorDialog();
            if (ColorPick.ShowDialog() == DialogResult.OK)
            {
                textureArray[MaterialSelect.SelectedIndex].ColorCombine.Primary = ColorPick.Color;                
            }
            UpdateTextureDisplay();
        }

        private void UpdateCCEditor(object sender, EventArgs e)
        {
            if (CCEdit.CCMode == 0)
            {
                textureArray[MaterialSelect.SelectedIndex].ColorCombine.AdvancedModeA = true;
                textureArray[MaterialSelect.SelectedIndex].ColorCombine.CombineValuesA = CCEdit.ValueArray;
            }
            else
            {
                textureArray[MaterialSelect.SelectedIndex].ColorCombine.AdvancedModeB = true;
                textureArray[MaterialSelect.SelectedIndex].ColorCombine.CombineValuesB = CCEdit.ValueArray;
            }
            UpdateTextureDisplay();
        }

        private void AdvanceABtn_Click(object sender, EventArgs e)
        {
            CCEdit = new ColorCombineEditor();
            CCEdit.UpdateParent += UpdateCCEditor;
            CCEdit.CCMode = 0;
            CCEdit.ValueArray = textureArray[MaterialSelect.SelectedIndex].ColorCombine.CombineValuesA;
            textureArray[MaterialSelect.SelectedIndex].ColorCombine.AdvancedModeA = true;
            CCEdit.Show();
            CCEdit.UpdateUI();
            
        }

        private void AdvanceBBtn_Click(object sender, EventArgs e)
        {
            CCEdit = new ColorCombineEditor();
            CCEdit.UpdateParent += UpdateCCEditor;
            CCEdit.CCMode = 1;
            CCEdit.ValueArray = textureArray[MaterialSelect.SelectedIndex].ColorCombine.CombineValuesB;
            textureArray[MaterialSelect.SelectedIndex].ColorCombine.AdvancedModeB = true;
            CCEdit.Show();
            CCEdit.UpdateUI();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }

        private void textureCodecBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }
    }
}
