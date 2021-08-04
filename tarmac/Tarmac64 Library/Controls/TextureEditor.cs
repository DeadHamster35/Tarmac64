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

namespace Tarmac64_Library.Controls
{
    public partial class TextureEditor : UserControl
    {
        public TextureEditor()
        {
            InitializeComponent();
        }

        int[][] BitSizes = new int[3][]
        {
            new int[]{16},
            new int[]{4,8 },
            new int[]{16}
        };

        int lastMaterial = 0;
        public TM64_Geometry.OK64Texture[] textureArray = new TM64_Geometry.OK64Texture[0];

        public bool UpdateTextureDisplay()
        {
            if (textureArray[textureBox.SelectedIndex].texturePath != null)
            {
                
                bitm.ImageLocation = textureArray[textureBox.SelectedIndex].texturePath;
                heightBox.Text = textureArray[textureBox.SelectedIndex].textureHeight.ToString();
                widthBox.Text = textureArray[textureBox.SelectedIndex].textureWidth.ToString();
                textureModeSBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureModeS;
                textureModeTBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureModeT;
                textureAlphaBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureTransparent;
                textureCodecBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureCodec;
                textureScrollSBox.Text = textureArray[textureBox.SelectedIndex].textureScrollS.ToString();
                textureScrollTBox.Text = textureArray[textureBox.SelectedIndex].textureScrollT.ToString();
                vertAlphaBox.Text = textureArray[textureBox.SelectedIndex].vertAlpha.ToString();
                DoubleBox.Checked = textureArray[textureBox.SelectedIndex].textureDoubleSide;
                screenBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureScreen;

                BitBox.Items.Clear();
                foreach (var Bit in BitSizes[textureArray[textureBox.SelectedIndex].textureCodec])
                {
                    BitBox.Items.Add(Bit.ToString());
                }
                BitBox.SelectedIndex = textureArray[textureBox.SelectedIndex].bitSize;
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
            textureBox.Items.Clear();
            for (int materialIndex = 0; materialIndex < MaterialCount; materialIndex++)
            {
                if (textureArray[materialIndex].texturePath != null)
                {
                    if (textureArray[materialIndex].textureClass == -1)
                    {
                        MessageBox.Show("Warning! Texture wrong dimensions -" + textureArray[materialIndex].textureName + "- Height: " + textureArray[materialIndex].textureHeight + "   Width: " + textureArray[materialIndex].textureWidth);
                        textureBox.Items.Add("UNUSABLE " + materialIndex.ToString() + " - " + textureArray[materialIndex].textureName);
                    }
                    else
                    {
                        textureBox.Items.Add("M-" + materialIndex.ToString() + " " + textureArray[materialIndex].textureName);
                    }
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
            textureCodecBox.SelectedIndex = 0;
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
                textureArray[This].textureWidth = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureHeight = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureModeS = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureModeT = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureScrollS = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureScrollT = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureTransparent = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureDoubleSide = Convert.ToBoolean(TextureSettings[ThisLine++]);
                textureArray[This].vertAlpha = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].textureCodec = Convert.ToInt32(TextureSettings[ThisLine++]);
                textureArray[This].bitSize = Convert.ToInt32(TextureSettings[ThisLine++]);
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
                Output.Add(textureArray[This].textureModeS.ToString());
                Output.Add(textureArray[This].textureModeT.ToString());
                Output.Add(textureArray[This].textureScrollS.ToString());
                Output.Add(textureArray[This].textureScrollT.ToString());
                Output.Add(textureArray[This].textureTransparent.ToString());
                Output.Add(textureArray[This].textureDoubleSide.ToString());
                Output.Add(textureArray[This].vertAlpha.ToString());
                Output.Add(textureArray[This].textureCodec.ToString());
                Output.Add(textureArray[This].bitSize.ToString());
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

        private void textureCodecBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //textureCodecBox.SelectedIndex = 0;
            
            if (textureArray.Length > 0)
            {
                TM64_Geometry TarmacGeometry = new TM64_Geometry();
                int lastCodec = textureArray[textureBox.SelectedIndex].textureCodec;
                textureArray[textureBox.SelectedIndex].textureCodec = textureCodecBox.SelectedIndex;
                
                if (!TarmacGeometry.GetTextureClass(textureArray[textureBox.SelectedIndex]))
                {
                    MessageBox.Show("Error - Cannot select this Codec with current Texture Size");
                    textureArray[textureBox.SelectedIndex].textureCodec = lastCodec;
                }
                else
                {
                    textureArray[textureBox.SelectedIndex].bitSize = 0;
                }
                
                UpdateTextureDisplay();
            }
        }

        private void widthBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textureModeSBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureModeS = textureModeSBox.SelectedIndex;
        }

        private void textureModeTBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureModeT = textureModeTBox.SelectedIndex;
        }

        private void textureScrollSBox_TextChanged(object sender, EventArgs e)
        {
            int sScroll;
            if (int.TryParse(textureScrollSBox.Text, out sScroll))
            {
                textureArray[textureBox.SelectedIndex].textureScrollS = sScroll;
            }
        }

        private void textureScrollTBox_TextChanged(object sender, EventArgs e)
        {
            int tScroll;
            if (int.TryParse(textureScrollTBox.Text, out tScroll))
            {
                textureArray[textureBox.SelectedIndex].textureScrollT = tScroll;
            }
        }

        private void textureAlphaBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureTransparent = textureAlphaBox.SelectedIndex;
        }

        private void screenBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureScreen = screenBox.SelectedIndex;
        }

        private void vertAlphaBox_TextChanged(object sender, EventArgs e)
        {
            int vAlpha;
            if (int.TryParse(vertAlphaBox.Text, out vAlpha))
            {
                textureArray[textureBox.SelectedIndex].vertAlpha = vAlpha;
            }
        }

        private void DoubleBox_CheckedChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureDoubleSide = DoubleBox.Checked;
        }

        private void BitBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].bitSize = BitBox.SelectedIndex;
        }
    }
}
