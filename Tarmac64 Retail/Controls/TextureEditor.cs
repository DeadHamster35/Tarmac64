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
using Fluent;
using System.Windows.Forms.VisualStyles;

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
        public bool UpdateTextureCache = false;

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

        public void LoadTextureArray(MemoryStream memoryStream)
        {
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            int tCount = binaryReader.ReadInt32();
            textureArray = new TM64_Geometry.OK64Texture[tCount];
            for (int ThisTex = 0; ThisTex < textureArray.Length; ThisTex++)
            {
                textureArray[ThisTex] = new TM64_Geometry.OK64Texture(memoryStream);
            }
        }

        public byte[] SaveTextureArray()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(textureArray.Length);
            for (int ThisTex = 0; ThisTex < textureArray.Length; ThisTex++)
            {
                binaryWriter.Write(textureArray[ThisTex].SaveData());
            }

            return memoryStream.ToArray();
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
                UpdateTextureCache = false;
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


        private void GeoModeBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bool NewState = false;
            if (e.NewValue == CheckState.Checked)
            {
                NewState = true;
            }
            UpdateTextureData(NewState, e.Index);
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            TM64.OK64Settings okSettings = new TM64.OK64Settings();
            okSettings.LoadSettings();

            SaveFileDialog FileSave = new SaveFileDialog();
            FileSave.InitialDirectory = okSettings.ProjectDirectory;
            FileSave.Filter = "Tarmac Texture|*.ok64.Texture|All Files (*.*)|*.*";

            TM64_Geometry.OK64Texture Local = textureArray[textureBox.SelectedIndex];

            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                List<string> Output = new List<string>();

                Output.Add(Local.textureName);
                Output.Add(Local.texturePath);
                Output.Add(Local.alphaPath);
                Output.Add(Local.CombineModeA.ToString());
                Output.Add(Local.CombineModeB.ToString());
                Output.Add(Local.RenderModeA.ToString());
                Output.Add(Local.RenderModeB.ToString());
                Output.Add(Local.GeometryModes.ToString());
                Output.Add(Local.BitSize.ToString());
                Output.Add(Local.TextureFilter.ToString());
                Output.Add(Local.TextureFormat.ToString());
                Output.Add(Local.SFlag.ToString());
                Output.Add(Local.TFlag.ToString());
                Output.Add(Local.textureScrollS.ToString());
                Output.Add(Local.textureScrollT.ToString());
                Output.Add(Local.textureScreen.ToString());
                Output.Add(Local.GLShiftS.ToString());
                Output.Add(Local.GLShiftT.ToString());

                Output.Add(Local.TextureOverWrite.Length.ToString());
                for (int ThisOver = 0; ThisOver < Local.TextureOverWrite.Length; ThisOver++)
                {
                    Output.Add(Local.TextureOverWrite[ThisOver].ToString());
                }

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

            TM64_Geometry.OK64Texture Local = textureArray[textureBox.SelectedIndex];

            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                string[] Input = File.ReadAllLines(FileOpen.FileName);
                int ThisLine = 0;
                Local.textureName = Input[ThisLine++];
                Local.texturePath = Input[ThisLine++];
                Local.alphaPath = Input[ThisLine++];
                Local.CombineModeA = Convert.ToInt32(Input[ThisLine++]);
                Local.CombineModeB = Convert.ToInt32(Input[ThisLine++]);

                Local.RenderModeA = Convert.ToInt32(Input[ThisLine++]);
                Local.RenderModeB = Convert.ToInt32(Input[ThisLine++]);

                Local.GeometryModes = Convert.ToUInt32(Input[ThisLine++]);
                Local.BitSize = Convert.ToInt32(Input[ThisLine++]);
                Local.TextureFilter = Convert.ToInt32(Input[ThisLine++]);
                Local.TextureFormat = Convert.ToInt32(Input[ThisLine++]);
                Local.SFlag = Convert.ToInt32(Input[ThisLine++]);
                Local.TFlag = Convert.ToInt32(Input[ThisLine++]);
                Local.textureScrollS = Convert.ToInt32(Input[ThisLine++]);
                Local.textureScrollT = Convert.ToInt32(Input[ThisLine++]);
                Local.textureScreen = Convert.ToInt32(Input[ThisLine++]);
                Local.GLShiftS = Convert.ToInt32(Input[ThisLine++]);
                Local.GLShiftT = Convert.ToInt32(Input[ThisLine++]);

                Local.TextureOverWrite = new int[Convert.ToInt32(Input[ThisLine++])];

                for (int ThisOver = 0; ThisOver < Local.TextureOverWrite.Length; ThisOver++)
                {
                    Local.TextureOverWrite[ThisOver] = Convert.ToInt32(Input[ThisLine++]);
                }

                if (File.Exists(Local.texturePath))
                {
                    using (var fs = new FileStream(Local.texturePath, FileMode.Open, FileAccess.Read))
                    {
                        Local.RawTexture.textureBitmap = Image.FromStream(fs);
                    }
                }
                Local.textureWidth = Local.RawTexture.textureBitmap.Width;
                Local.textureHeight = Local.RawTexture.textureBitmap.Height;

                int materialIndex = textureBox.SelectedIndex;


                if (Local.texturePath != null)
                {
                    textureBox.Items[materialIndex] = ("Texture-" + materialIndex.ToString() + " " + Local.textureName);
                }
                else
                {
                    //MessageBox.Show("Warning! Material " + fbx.Materials[materialIndex].Name + " does not have a diffuse texture and cannot be used.");                    
                    textureBox.Items[materialIndex] = ("Shaded- " + materialIndex.ToString() + " - " + Local.textureName);
                }


                textureArray[materialIndex] = Local;
                if (UpdateParent != null)
                {
                    UpdateTextureCache = true;
                    UpdateParent(this, EventArgs.Empty);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(FileOpen.FileName))
                {
                    TM64_Geometry.OK64Texture Local = textureArray[textureBox.SelectedIndex];
                    Local.texturePath = FileOpen.FileName;

                    if (File.Exists(Local.texturePath))
                    {
                        using (var fs = new FileStream(Local.texturePath, FileMode.Open, FileAccess.Read))
                        {
                            Local.RawTexture.textureBitmap = Image.FromStream(fs);
                        }
                    }
                    Local.textureWidth = Local.RawTexture.textureBitmap.Width;
                    Local.textureHeight = Local.RawTexture.textureBitmap.Height;

                    textureArray[textureBox.SelectedIndex] = Local;
                    if (UpdateParent != null)
                    {
                        UpdateTextureCache = true;
                        UpdateParent(this, EventArgs.Empty);
                    }

                }
            }
            
        }

        private void textureCodecBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextureData();
        }
    }
}
