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
using System.Windows.Input;
using System.Drawing.Imaging;
using Cereal64.Microcodes.F3DEX.DataElements;
using Microsoft.WindowsAPICodePack.Dialogs;
using F3DSharp;
using System.Drawing.Drawing2D;
using Texture64;

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


        public event EventHandler UpdateParent;

        public bool Loaded = false, Locked = false;
        int[] PanelXY = new int[] { 5, 415 };
        public bool UpdateTextureDisplay()
        {
            if (Loaded)
            {
                Locked = true;
                if (textureArray[textureBox.SelectedIndex].texturePath != null)
                {

                    //bitm.ImageLocation = textureArray[textureBox.SelectedIndex].texturePath;
                    bitm.Invalidate();
                    bitm.Update();
                    bitm.Refresh();

                    heightBox.Text = textureArray[textureBox.SelectedIndex].textureHeight.ToString();
                    widthBox.Text = textureArray[textureBox.SelectedIndex].textureWidth.ToString();

                    BitBox.SelectedIndex = textureArray[textureBox.SelectedIndex].BitSize;
                    CodecBox.SelectedIndex = textureArray[textureBox.SelectedIndex].TextureFormat;
                    FilterBox.SelectedIndex = textureArray[textureBox.SelectedIndex].TextureFilter;

                    SFlagBox.SelectedIndex = textureArray[textureBox.SelectedIndex].SFlag;
                    TFlagBox.SelectedIndex = textureArray[textureBox.SelectedIndex].TFlag;

                    textureScrollSBox.Text = textureArray[textureBox.SelectedIndex].textureScrollS.ToString();
                    textureScrollTBox.Text = textureArray[textureBox.SelectedIndex].textureScrollT.ToString();
                    
                    screenBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureScreen;

                    CombineBoxA.SelectedIndex = textureArray[textureBox.SelectedIndex].CombineModeA;
                    CombineBoxB.SelectedIndex = textureArray[textureBox.SelectedIndex].CombineModeB;
                    

                    for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                    {
                        GeoModeBox.SetItemChecked(ThisCheck, textureArray[textureBox.SelectedIndex].GeometryBools[ThisCheck]);
                    }

                    RenderBoxA.SelectedIndex = textureArray[textureBox.SelectedIndex].RenderModeA;
                    RenderBoxB.SelectedIndex = textureArray[textureBox.SelectedIndex].RenderModeB;

                    OverWriteIndexBox.Items.Clear();
                    OverWriteIndexBox.SelectedIndex = -1;
                    for (int ThisOverWrite = 0; ThisOverWrite < textureArray[textureBox.SelectedIndex].TextureOverWrite.Length; ThisOverWrite++)
                    {
                        OverWriteIndexBox.Items.Add(ThisOverWrite);
                    }
                    
                    if (textureArray[textureBox.SelectedIndex].TextureOverWrite.Length > 0)
                    {
                        OverWriteIndexBox.SelectedIndex = 0;
                        OverWriteBox.SelectedIndex = textureArray[textureBox.SelectedIndex].TextureOverWrite[0];
                    }
                    else
                    {
                        OverWriteIndexBox.SelectedIndex = -1;
                        OverWriteBox.SelectedIndex = -1;
                    }

                    alphaMaskBox.Text = textureArray[textureBox.SelectedIndex].alphaPath;

                    AlphaMaskCheckbox.Checked = (alphaMaskBox.Text != "");
                    

                    Locked = false;
                    return true;
                }
                else
                {
                    Loaded = false;
                    {
                        bitm.Invalidate();
                        bitm.Update();
                        bitm.Refresh();
                    }
                    Loaded = true;
                    BitBox.SelectedIndex = -1;
                    CodecBox.SelectedIndex = -1;
                    FilterBox.SelectedIndex = -1;

                    SFlagBox.SelectedIndex = -1;
                    TFlagBox.SelectedIndex = -1;

                    textureScrollSBox.Text = "";
                    textureScrollTBox.Text = "";

                    screenBox.SelectedIndex = -1;


                    CombineBoxA.SelectedIndex = textureArray[textureBox.SelectedIndex].CombineModeA;
                    CombineBoxB.SelectedIndex = textureArray[textureBox.SelectedIndex].CombineModeB;
                    

                    for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                    {
                        GeoModeBox.SetItemChecked(ThisCheck, textureArray[textureBox.SelectedIndex].GeometryBools[ThisCheck]);
                    }

                    RenderBoxA.SelectedIndex = textureArray[textureBox.SelectedIndex].RenderModeA;
                    RenderBoxB.SelectedIndex = textureArray[textureBox.SelectedIndex].RenderModeB;

                    alphaMaskBox.Text = "";

                    Locked = false;
                    return true;
                }
            }
            else
            {
                
                return false;
            }
            
        }

        public int AddNewTextures(int MaterialCount)
        {
            int textureCount = 0;
            textureBox.Items.Clear();
            for (int materialIndex = 0; materialIndex < MaterialCount; materialIndex++)
            {
                if (textureArray[materialIndex].texturePath != null)
                {
                    textureBox.Items.Add("Texture-" + materialIndex.ToString() + " " + textureArray[materialIndex].textureName);                    
                    textureCount++;
                }
                else
                {
                    //MessageBox.Show("Warning! Material " + fbx.Materials[materialIndex].Name + " does not have a diffuse texture and cannot be used.");                    
                    textureBox.Items.Add("Shaded- " + materialIndex.ToString() + " - " + textureArray[materialIndex].textureName);                    
                }
                OverWriteBox.Items.Add(materialIndex.ToString() + " " + textureArray[materialIndex].textureName);
            }
            textureBox.SelectedIndex = 0;
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
            foreach (var ThisName in F3DEX095_Parameters.RenderModeNames)
            {
                RenderBoxA.Items.Add(ThisName);
                RenderBoxB.Items.Add(ThisName);
            }

            foreach (var ThisName in F3DEX095_Parameters.GeometryModeNames)
            {
                GeoModeBox.Items.Add(ThisName, false);
            }
            
        }

        public int LoadTextureSettings(string[] TextureSettings, TM64_Geometry.OK64Texture[] CurrentArray, int Version = 5)
        {
            int ThisLine = 0;
            int Count = Convert.ToInt32(TextureSettings[ThisLine++]);
            textureArray = new TM64_Geometry.OK64Texture[Count];
            if (Count != CurrentArray.Length)
            {
                MessageBox.Show("Warning! Texture Counts do not match. Attempting backup.");
            }
            for (int This = 0; This < Count; This++)
            {
                textureArray[This] = new TM64_Geometry.OK64Texture();
                textureArray[This].textureName = TextureSettings[ThisLine++];
                textureArray[This].texturePath = CurrentArray[This].texturePath;
                textureArray[This].textureBitmap = CurrentArray[This].textureBitmap;
                textureArray[This].textureBitmap = CurrentArray[This].textureBitmap;
                textureArray[This].alphaPath = CurrentArray[This].alphaPath;
                textureArray[This].textureWidth = CurrentArray[This].textureWidth;
                textureArray[This].textureHeight = CurrentArray[This].textureHeight;
                ThisLine += 2; //skip height width 

                textureArray[This].SFlag = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].TFlag = Convert.ToInt32(TextureSettings[ThisLine++]);

                textureArray[This].GeometryBools = new bool[F3DEX095_Parameters.GeometryModes.Length];
                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    textureArray[This].GeometryBools[ThisCheck] = Convert.ToBoolean(TextureSettings[ThisLine++]);
                }
                textureArray[This].CombineValuesA = new uint[8];
                textureArray[This].CombineValuesB = new uint[8];
                for (int ThisValue = 0; ThisValue < 8; ThisValue++)
                {
                    textureArray[This].CombineValuesA[ThisValue] = Convert.ToUInt32(TextureSettings[ThisLine++]);
                    textureArray[This].CombineValuesB[ThisValue] = Convert.ToUInt32(TextureSettings[ThisLine++]);
                }

                textureArray[This].CombineModeA = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].CombineModeB = Convert.ToInt32(TextureSettings[ThisLine++]);

                textureArray[This].RenderModeA = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].RenderModeB = Convert.ToInt32(TextureSettings[ThisLine++]);

                textureArray[This].BitSize = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].TextureFormat = Convert.ToInt32(TextureSettings[ThisLine++]);

                if (Version > 6)
                {
                    textureArray[This].TextureFormat = Convert.ToInt32(TextureSettings[ThisLine++]);
                }
                


                textureArray[This].textureScrollS = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureScrollT = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].vertAlpha = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureScreen = Convert.ToInt32(TextureSettings[ThisLine++]);

                int OverWriteCount = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].TextureOverWrite = new int[OverWriteCount];
                for (int ThisOW = 0; ThisOW < OverWriteCount; ThisOW++)
                {
                    textureArray[This].TextureOverWrite[ThisOW] = Convert.ToInt32(TextureSettings[ThisLine++]);
                }
                

            }
            textureBox.SelectedIndex = 0;
            UpdateTextureDisplay();

            return ThisLine;
        }

        public string[] SaveTextureSettings(int Version)
        {
            List<string> Output = new List<string>();

            Output.Add(textureArray.Length.ToString());

            for (int This = 0; This < textureArray.Length; This++)
            {
                Output.Add(textureArray[This].textureName);
                Output.Add(textureArray[This].textureWidth.ToString());
                Output.Add(textureArray[This].textureHeight.ToString());

                Output.Add(textureArray[This].SFlag.ToString());
                Output.Add(textureArray[This].TFlag.ToString());

                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    Output.Add(textureArray[This].GeometryBools[ThisCheck].ToString());
                }
                textureArray[This].CombineValuesA = new uint[8];
                textureArray[This].CombineValuesB = new uint[8];
                for (int ThisValue = 0; ThisValue < 8; ThisValue++)
                {
                    Output.Add(textureArray[This].CombineValuesA[ThisValue].ToString());
                    Output.Add(textureArray[This].CombineValuesB[ThisValue].ToString());
                }
                
                Output.Add(textureArray[This].CombineModeA.ToString());
                Output.Add(textureArray[This].CombineModeB.ToString());

                Output.Add(textureArray[This].RenderModeA.ToString());
                Output.Add(textureArray[This].RenderModeB.ToString());

                Output.Add(textureArray[This].BitSize.ToString());
                Output.Add(textureArray[This].TextureFormat.ToString());
                if (Version > 6)
                {
                    Output.Add(textureArray[This].TextureFilter.ToString());
                }


                Output.Add(textureArray[This].textureScrollS.ToString());
                Output.Add(textureArray[This].textureScrollT.ToString());
                Output.Add(textureArray[This].vertAlpha.ToString());
                Output.Add(textureArray[This].textureScreen.ToString());

                Output.Add(textureArray[This].TextureOverWrite.Length.ToString());                
                foreach (var OverWrite in textureArray[This].TextureOverWrite)
                {
                    Output.Add(OverWrite.ToString());
                }
            }
            return Output.ToArray();
        }

        private void textureBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (UpdateTextureDisplay())
            {
                lastMaterial = textureBox.SelectedIndex;
            }
            else
            {
                textureBox.SelectedIndex = lastMaterial;
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

        private void vertAlphaBox_TextChanged(object sender, EventArgs e)
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
                    textureArray[textureBox.SelectedIndex].textureScrollT = Parse;
                }
                if (int.TryParse(textureScrollSBox.Text, out Parse))
                {
                    textureArray[textureBox.SelectedIndex].textureScrollS = Parse;
                }

                textureArray[textureBox.SelectedIndex].textureScreen = screenBox.SelectedIndex;


                textureArray[textureBox.SelectedIndex].RenderModeA = RenderBoxA.SelectedIndex;
                textureArray[textureBox.SelectedIndex].RenderModeB = RenderBoxB.SelectedIndex;

                for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                {
                    if (NewIndex == ThisCheck)
                    {
                        textureArray[textureBox.SelectedIndex].GeometryBools[ThisCheck] = NewItem;
                    }
                    else
                    {
                        textureArray[textureBox.SelectedIndex].GeometryBools[ThisCheck] = GeoModeBox.GetItemChecked(ThisCheck);
                    }
                }
                textureArray[textureBox.SelectedIndex].SFlag = SFlagBox.SelectedIndex;
                textureArray[textureBox.SelectedIndex].TFlag = TFlagBox.SelectedIndex;

                textureArray[textureBox.SelectedIndex].BitSize = BitBox.SelectedIndex;
                textureArray[textureBox.SelectedIndex].TextureFormat = CodecBox.SelectedIndex;
                textureArray[textureBox.SelectedIndex].TextureFilter = FilterBox.SelectedIndex;

                if (OverWriteIndexBox.SelectedIndex != -1)
                {
                    textureArray[textureBox.SelectedIndex].TextureOverWrite[OverWriteIndexBox.SelectedIndex] = OverWriteBox.SelectedIndex;
                }
                

                //textureArray[textureBox.SelectedIndex].AdvancedSettings = AdvanceBox.Checked;
                textureArray[textureBox.SelectedIndex].AdvancedSettings = false;

                if (!textureArray[textureBox.SelectedIndex].AdvancedSettings)
                {
                    textureArray[textureBox.SelectedIndex].CombineModeA = CombineBoxA.SelectedIndex;
                    textureArray[textureBox.SelectedIndex].CombineModeB = CombineBoxB.SelectedIndex;
                }
            }
            
            if (UpdateParent != null)
            {
                UpdateParent(this, EventArgs.Empty);
            }
        }
        private void CombineBoxA_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }

        private void CombineBoxB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
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

        private void AdvancedPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GeoModeBox_SelectedIndexChanged(object sender, EventArgs e)
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

        private void GeoModeBox_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            UpdateTextureData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Locked = true;
            OverWriteBox.SelectedIndex = textureArray[textureBox.SelectedIndex].TextureOverWrite[OverWriteIndexBox.SelectedIndex];
            Locked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Locked = true;
            OverWriteIndexBox.Items.Add(OverWriteIndexBox.Items.Count);
            List<int> NewArray = textureArray[textureBox.SelectedIndex].TextureOverWrite.ToList();
            NewArray.Add(0);
            textureArray[textureBox.SelectedIndex].TextureOverWrite = NewArray.ToArray();
            OverWriteIndexBox.SelectedIndex = OverWriteIndexBox.Items.Count - 1;
            OverWriteBox.SelectedIndex = 0;
            Locked = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Locked = true;            
            int Index = OverWriteIndexBox.SelectedIndex;
            if (Index == -1)
            {
                return;
            }
            OverWriteIndexBox.Items.RemoveAt(Index);


            List<int> NewArray = textureArray[textureBox.SelectedIndex].TextureOverWrite.ToList();
            NewArray.RemoveAt(Index);
            textureArray[textureBox.SelectedIndex].TextureOverWrite = NewArray.ToArray();
            if (Index > textureArray.Length)
            {
                OverWriteBox.SelectedIndex = textureArray.Length;
            }
            else
            {
                OverWriteBox.SelectedIndex = Index;
            }
            
            Locked = false;
        }

        private void OverWriteBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }

        private void bitm_Paint(object sender, PaintEventArgs e)
        {
            if ((Loaded) && (textureBox.SelectedIndex >= 0))
            {
                if (!File.Exists(textureArray[textureBox.SelectedIndex].texturePath))
                {
                    return;
                }
                if (textureArray[textureBox.SelectedIndex].textureHeight == 0)
                {
                    return;
                }


                int THeight = textureArray[textureBox.SelectedIndex].textureHeight;
                int TWidth = textureArray[textureBox.SelectedIndex].textureWidth;

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
                Bitmap Draw = new Bitmap(textureArray[textureBox.SelectedIndex].texturePath);
                e.Graphics.DrawImage(
                   Draw,
                    new Rectangle(XOff, YOff, TWidth, THeight),
                    // destination rectangle 
                    0,
                    0,           // upper-left corner of source rectangle
                    textureArray[textureBox.SelectedIndex].textureWidth,       // width of source rectangle
                    textureArray[textureBox.SelectedIndex].textureHeight,      // height of source rectangle
                    GraphicsUnit.Pixel);
            }
            else
            {
                
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void AlphaMaskCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if ((Loaded) && (!Locked))
            {
                Locked = true;

                if (!AlphaMaskCheckbox.Checked)
                {
                    textureArray[textureBox.SelectedIndex].alphaPath = "";
                }
                else
                {
                    OpenFileDialog FileOpen = new OpenFileDialog();
                    MessageBox.Show("Select Alpha Mask Texture");
                    if (FileOpen.ShowDialog()==DialogResult.OK)
                    {
                        if (File.Exists(FileOpen.FileName))
                        {
                            textureArray[textureBox.SelectedIndex].alphaPath = FileOpen.FileName;
                        }
                    }

                }
                alphaMaskBox.Text = textureArray[textureBox.SelectedIndex].alphaPath;
                Locked = false;
            }

            
        }

        private void FilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }

        private void textureCodecBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }
    }
}
