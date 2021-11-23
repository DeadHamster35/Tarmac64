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
using Tarmac64_Library;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Net;

namespace Tarmac64_Retail
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
            string hText = "";

            if (fileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string[] PNGList = Directory.GetFiles(fileOpen.FileName, "*.PNG", SearchOption.AllDirectories);
                string[] BMPList = Directory.GetFiles(fileOpen.FileName, "*.BMP", SearchOption.AllDirectories);
                string[] JPGList = Directory.GetFiles(fileOpen.FileName, "*.JPG", SearchOption.AllDirectories);
                string[] JPEGList = Directory.GetFiles(fileOpen.FileName, "*.JPEG", SearchOption.AllDirectories);
                string[] imageList = PNGList.Concat(BMPList).Concat(JPGList).Concat(JPEGList).ToArray();
                string parentDirectory = Path.Combine(fileOpen.FileName, "output");
                Directory.CreateDirectory(parentDirectory);

                
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

                foreach (var textureAddress in imageList)
                {
                    string fileName = Path.GetFileNameWithoutExtension(textureAddress);

                    byte[] imageData = null;
                    byte[] paletteData = null;
                    Bitmap bitmapData = new Bitmap(textureAddress);
                    N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[codecBox.SelectedIndex], bitmapData);
                    byte[] compressedTexture = Tarmac.CompressMIO0(imageData);

                    int SegmentPosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                    binaryWriter.Write(imageData);
                    int PalettePosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                    if (paletteData != null)
                    {
                        
                        binaryWriter.Write(paletteData);
                    }


                    hText += "#define " + fileName + "_Offset 0x" + SegmentPosition.ToString("X") + Environment.NewLine;
                    hText += "#define " + fileName + "_Size 0x" + imageData.Length.ToString("X") + Environment.NewLine;
                    if (paletteData != null)
                    {
                        hText += "#define " + fileName + "_Offset 0x" + PalettePosition.ToString("X") + Environment.NewLine;
                        hText += "#define " + fileName + "_PaletteSize 0x" + paletteData.Length.ToString("X") + Environment.NewLine;
                    }
                    
                    
                }

                binaryWriter.Close();
                memoryStream.Close();
                string childDirectory = Path.Combine(parentDirectory, "ImageData");
                
                File.WriteAllBytes(childDirectory + ".RAW", memoryStream.ToArray());
                File.WriteAllBytes(childDirectory + ".MIO0", Tarmac.CompressMIO0(memoryStream.ToArray()));
                
                File.WriteAllText(Path.Combine(childDirectory + ".h"), hText);

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


                bitmapData.Dispose();
                MessageBox.Show("Finished");
            }
        }

        private void TextureCompiler_Load(object sender, EventArgs e)
        {
            
            codecBox.SelectedIndex = 0;
        }
    }
}
