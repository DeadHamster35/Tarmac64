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

        public bool Loaded, Locked = false;
        int[] PanelXY = new int[] { 5, 415 };
        public bool UpdateTextureDisplay()
        {
            if (Loaded)
            {
                Locked = true;
                if (textureArray[textureBox.SelectedIndex].texturePath != null)
                {

                    bitm.ImageLocation = textureArray[textureBox.SelectedIndex].texturePath;
                    heightBox.Text = textureArray[textureBox.SelectedIndex].textureHeight.ToString();
                    widthBox.Text = textureArray[textureBox.SelectedIndex].textureWidth.ToString();

                    if (!AdvanceBox.Checked)
                    {
                        CombineBoxA.SelectedIndex = textureArray[textureBox.SelectedIndex].CombineModeA;
                        CombineBoxB.SelectedIndex = textureArray[textureBox.SelectedIndex].CombineModeB;
                    }

                    for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
                    {
                        GeoModeBox.SetItemChecked(ThisCheck, textureArray[textureBox.SelectedIndex].GeometryBools[ThisCheck]);
                    }

                    RenderBoxA.SelectedIndex = textureArray[textureBox.SelectedIndex].RenderModeA;
                    RenderBoxB.SelectedIndex = textureArray[textureBox.SelectedIndex].RenderModeB;

                    BitBox.SelectedIndex = textureArray[textureBox.SelectedIndex].BitSize;
                    CodecBox.SelectedIndex = textureArray[textureBox.SelectedIndex].TextureFormat;

                    SFlagBox.SelectedIndex = textureArray[textureBox.SelectedIndex].SFlag;
                    TFlagBox.SelectedIndex = textureArray[textureBox.SelectedIndex].TFlag;

                    textureScrollSBox.Text = textureArray[textureBox.SelectedIndex].textureScrollS.ToString();
                    textureScrollTBox.Text = textureArray[textureBox.SelectedIndex].textureScrollT.ToString();
                    
                    screenBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureScreen;

                    Locked = false;
                    return true;
                }
                else
                {
                    Locked = false;
                    return false;
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
                    textureBox.Items.Add("M-" + materialIndex.ToString() + " " + textureArray[materialIndex].textureName);
                    textureCount++;
                }
                else
                {
                    //MessageBox.Show("Warning! Material " + fbx.Materials[materialIndex].Name + " does not have a diffuse texture and cannot be used.");                    
                    textureBox.Items.Add("UNUSABLE " + materialIndex.ToString() + " - " + textureArray[materialIndex].textureName);
                }
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
            foreach (var ThisName in F3DEX095_Parameters.RenderModeNames)
            {
                RenderBoxA.Items.Add(ThisName);
                RenderBoxB.Items.Add(ThisName);
            }
            foreach (var ThisName in F3DEX095_Parameters.ColorCombineNames)
            {
                CombA1.Items.Add(ThisName);
                CombA2.Items.Add(ThisName);
                CombA3.Items.Add(ThisName);
                CombA4.Items.Add(ThisName);
                CombA1A.Items.Add(ThisName);
                CombA2A.Items.Add(ThisName);
                CombA3A.Items.Add(ThisName);
                CombA4A.Items.Add(ThisName);

                CombB1.Items.Add(ThisName);
                CombB2.Items.Add(ThisName);
                CombB3.Items.Add(ThisName);
                CombB4.Items.Add(ThisName);
                CombB1A.Items.Add(ThisName);
                CombB2A.Items.Add(ThisName);
                CombB3A.Items.Add(ThisName);
                CombB4A.Items.Add(ThisName);
            }

            foreach (var ThisName in F3DEX095_Parameters.GeometryModeNames)
            {
                GeoModeBox.Items.Add(ThisName, false);
            }

            AdvancedPanel.Parent = this;
            AdvancedPanel.Location = new System.Drawing.Point { X = 350, Y = 200 };
            AdvancedPanel.BringToFront();
            //
            StandardPanel.Visible = true;
            StandardPanel.Parent = groupBox5;
            StandardPanel.Location = new System.Drawing.Point { X = PanelXY[0], Y = PanelXY[1] };
            StandardPanel.BringToFront();
            this.Height = 780;
        }

        public int LoadTextureSettings(string[] TextureSettings)
        {
            int ThisLine = 0;
            int Count = Convert.ToInt32(TextureSettings[ThisLine++]);
            textureArray = new TM64_Geometry.OK64Texture[Count];
            for (int This = 0; This < Count; This++)
            {
                textureArray[This] = new TM64_Geometry.OK64Texture();
                textureArray[This].texturePath = TextureSettings[ThisLine++];
                if (textureArray[This].texturePath == "")
                {
                    textureArray[This].texturePath = "NULL";
                    textureArray[This].textureName = "NULL";
                }
                else
                {
                    textureArray[This].textureName = textureArray[This].texturePath;
                }
                textureArray[This].textureWidth = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureHeight = Convert.ToInt32(TextureSettings[ThisLine++]);

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


                textureArray[This].textureScrollS = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureScrollT = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].vertAlpha = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureScreen = Convert.ToInt32(TextureSettings[ThisLine++]);

            }
            textureBox.SelectedIndex = 0;
            UpdateTextureDisplay();

            return ThisLine;
        }

        public string[] SaveTextureSettings()
        {
            List<string> Output = new List<string>();

            Output.Add(textureArray.Length.ToString());

            for (int This = 0; This < textureArray.Length; This++)
            {
                Output.Add(textureArray[This].texturePath);
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

                Output.Add(textureArray[This].textureScrollS.ToString());
                Output.Add(textureArray[This].textureScrollT.ToString());
                Output.Add(textureArray[This].vertAlpha.ToString());
                Output.Add(textureArray[This].textureScreen.ToString());
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

        private void UpdateTextureData()
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
                    textureArray[textureBox.SelectedIndex].GeometryBools[ThisCheck] = GeoModeBox.GetItemChecked(ThisCheck);
                }
                textureArray[textureBox.SelectedIndex].SFlag = SFlagBox.SelectedIndex;
                textureArray[textureBox.SelectedIndex].TFlag = TFlagBox.SelectedIndex;

                textureArray[textureBox.SelectedIndex].BitSize = BitBox.SelectedIndex;
                textureArray[textureBox.SelectedIndex].TextureFormat = CodecBox.SelectedIndex;

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

        private void CombineUICheck()
        {
            if ((AdvanceBox.Checked) && (AdvanceBox.Enabled))
            {
                //StandardPanel.Visible = false;
                StandardPanel.Parent = this;
                StandardPanel.Location = new System.Drawing.Point { X = 350, Y = 200 };
                StandardPanel.BringToFront();
                //
                AdvancedPanel.Parent = groupBox5;
                AdvancedPanel.Location = new System.Drawing.Point { X = PanelXY[0], Y = PanelXY[1] };
                AdvancedPanel.BringToFront();
                this.Height = 1000;
            }
            else
            {
                AdvancedPanel.Parent = this;
                AdvancedPanel.Location = new System.Drawing.Point { X = 350, Y = 200 };
                AdvancedPanel.BringToFront();
                //
                StandardPanel.Visible = true;
                StandardPanel.Parent = groupBox5;
                StandardPanel.Location = new System.Drawing.Point { X = PanelXY[0], Y = PanelXY[1] };
                StandardPanel.BringToFront();
                this.Height = 780;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CombineUICheck();
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
            UpdateTextureData();
        }

        private void textureCodecBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }
    }
}
