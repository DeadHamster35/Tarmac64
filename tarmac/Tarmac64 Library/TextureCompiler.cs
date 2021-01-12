using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Texture64;

namespace Tarmac64_Library
{
    public partial class TextureCompiler : Form
    {
        public TextureCompiler()
        {
            InitializeComponent();
        }
        /*
            RGBA16
            RGBA32
            IA16
            IA8
            IA4
            I8
            I4
            CI8
            CI4
        */
        N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.RGBA32, N64Codec.IA16, N64Codec.IA8, N64Codec.IA4, N64Codec.I8, N64Codec.I4, N64Codec.CI8, N64Codec.CI4 };
        CommonOpenFileDialog fileOpen = new CommonOpenFileDialog();
        TM64 Tarmac = new TM64();
        

        private void button1_Click(object sender, EventArgs e)
        {
            fileOpen.IsFolderPicker = true;
            

            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string[] PNGList = Directory.GetFiles(fileOpen.FileName, "*.PNG", SearchOption.AllDirectories);
                string[] BMPList = Directory.GetFiles(fileOpen.FileName, "*.BMP", SearchOption.AllDirectories);
                string[] JPGList = Directory.GetFiles(fileOpen.FileName, "*.JPG", SearchOption.AllDirectories);
                string[] JPEGList = Directory.GetFiles(fileOpen.FileName, "*.JPEG", SearchOption.AllDirectories);
                string[] imageList = PNGList.Concat(BMPList).Concat(JPGList).Concat(JPEGList).ToArray();
                string parentDirectory = Path.Combine(fileOpen.FileName, "output");
                Directory.CreateDirectory(parentDirectory);
                foreach (var textureAddress in imageList)
                {
                    string fileName = Path.GetFileName(textureAddress);
                    string childDirectory = Path.Combine(parentDirectory, fileName);
                    N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.RGBA32 };
                    byte[] imageData = null;
                    byte[] paletteData = null;
                    Bitmap bitmapData = new Bitmap(textureAddress);
                    N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[codecBox.SelectedIndex], bitmapData);
                    byte[] compressedTexture = Tarmac.CompressMIO0(imageData);
                    
                    File.WriteAllBytes(childDirectory + ".RAW",imageData);
                    File.WriteAllBytes(childDirectory + ".MIO0",compressedTexture);

                }
                MessageBox.Show("Finished");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fileOpen.IsFolderPicker = false;


            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string textureAddress = fileOpen.FileName;                
                string parentDirectory = Path.Combine(Path.GetDirectoryName(textureAddress), "output");
                Directory.CreateDirectory(parentDirectory);
                string fileName = Path.GetFileName(textureAddress);
                string childDirectory = Path.Combine(parentDirectory, fileName);
                
                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(textureAddress);
                N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[codecBox.SelectedIndex], bitmapData);
                byte[] compressedTexture = Tarmac.CompressMIO0(imageData);
                

                File.WriteAllBytes(childDirectory + ".RAW", imageData);
                File.WriteAllBytes(childDirectory + ".RAW.MIO0", compressedTexture);
                if (paletteData != null)
                {
                    byte[] compressedPalette = Tarmac.CompressMIO0(paletteData);
                    File.WriteAllBytes(childDirectory + ".PALETTE", paletteData);
                    File.WriteAllBytes(childDirectory + ".PALETTE.MIO0", compressedTexture);
                }
                

                
                MessageBox.Show("Finished");
            }
        }

        private void TextureCompiler_Load(object sender, EventArgs e)
        {
            codecBox.SelectedIndex = 0;
        }
    }
}
