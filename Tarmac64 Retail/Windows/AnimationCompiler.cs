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

namespace Tarmac64_Library
{
    public partial class AnimationCompiler : Form
    {
        public AnimationCompiler()
        {
            InitializeComponent();
        }

        OpenFileDialog FileOpen = new OpenFileDialog();
        SaveFileDialog FileSave = new SaveFileDialog();
        TM64 Tarmac = new TM64();
        TM64_Geometry TarmacGeo = new TM64_Geometry();
        TM64_Geometry.OK64F3DObject[][] MasterObjects = new TM64_Geometry.OK64F3DObject[0][];
        TM64_Geometry.OK64Texture[][] TextureObjects = new TM64_Geometry.OK64Texture[0][];
        int lastMaterial = 0;
        int materialCount = 0;
        int LastSelectedIndex = -1;

        private void ModelCompiler_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                SegmentBox.Items.Add(i.ToString("X"));
            }
            SegmentBox.SelectedIndex = 8;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] OutputData = new byte[0];
            string outputText = "";
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = false;

            MasterObjects = new TM64_Geometry.OK64F3DObject[1][];
            TextureObjects = new TM64_Geometry.OK64Texture[1][];
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {



                string FBXFilePath = dialog.FileName;
                AssimpContext importer = new AssimpContext();



                Scene ModelData = importer.ImportFile(FBXFilePath, PostProcessPreset.TargetRealTimeMaximumQuality);
                string filename = Path.GetFileNameWithoutExtension(FBXFilePath);
                int materialCount = ModelData.MaterialCount;
                int SegmentID = SegmentBox.SelectedIndex;

                
                var ObjectAnimations = new TM64_Course.OKObjectAnimations();
                ObjectAnimations.WalkAnimation = TarmacGeo.LoadSkeleton(ModelData);

                Assimp.Node masterNode = ModelData.RootNode.FindNode("Master Objects");
                if (masterNode != null)
                {
                    TextureObjects[0] = TarmacGeo.loadTextures(ModelData, FBXFilePath);
                    MasterObjects[0] = TarmacGeo.createObjects(ModelData, TextureObjects[0]);
                }
                else
                {
                    MessageBox.Show("Error - " + FBXFilePath);
                }
                TextureControl.Loaded = true;

            }
        }


        private void WriteData()
        {
            byte[] OutputData = new byte[0];
            byte[] HeaderData = new byte[0];
            string asmText = "";
            string hText = "";
            string cText = "";
            string filename = "";

            int SegmentID = Convert.ToInt32(SegmentBox.SelectedIndex);
            int DataLength = Convert.ToInt32(MagicBox.Text, 16) + (MasterObjects.Length * 16);
            int[] RootPositions = new int[MasterObjects.Length];

            for (int currentItem = 0; currentItem < MasterObjects.Length; currentItem++)
            {
                if (MCheckBox.Checked)
                {
                    OutputData = TarmacGeo.writeModelTextures(OutputData, TextureObjects[currentItem], DataLength);
                    OutputData = TarmacGeo.compileTextureObject(OutputData, TextureObjects[currentItem], DataLength, SegmentID);
                    OutputData = TarmacGeo.compileF3DObject(OutputData, MasterObjects[currentItem], TextureObjects[currentItem], DataLength, SegmentID);



                    int SegmentPosition = OutputData.Length + (SegmentID * 0x01000000) + DataLength;

                    HeaderData = TarmacGeo.compileF3DHeader(SegmentPosition, HeaderData);

                    asmText += ".definelabel " + MasterObjects[currentItem][0].objectName + ", 0x" + SegmentPosition.ToString("X").PadLeft(8, '0') + Environment.NewLine;
                    hText += "extern const int " + MasterObjects[currentItem][0].objectName + "; //0x" + SegmentPosition.ToString("X").PadLeft(8, '0') + Environment.NewLine;
                    cText += "const int " + MasterObjects[currentItem][0].objectName + "= 0x" + SegmentPosition.ToString("X").PadLeft(8, '0') + ";" + Environment.NewLine;
                    OutputData = TarmacGeo.compileObjectList(OutputData, MasterObjects[currentItem], TextureObjects[currentItem], SegmentID);
                }
                TM64_Course TarmacCourse = new TM64_Course();
                //byte[] AnimationData = TarmacCourse.CompileObjectAnimation(TypeList.ToArray(), OutputData.Length);


            }
            
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
                binaryWriter.Write(HeaderData);
                binaryWriter.Write(OutputData);
                byte[] FinalData = memoryStream.ToArray();
                byte[] CompressedData = Tarmac.CompressMIO0(FinalData);
                string savePath = Path.GetDirectoryName(FileSave.FileName);
                string fileName = Path.GetFileNameWithoutExtension(FileSave.FileName);
                hText += "#define " + fileName + "_RawDataSize 0x" + OutputData.Length.ToString("X")+Environment.NewLine;
                hText += "#define " + fileName + "_CompressedSize 0x" + CompressedData.Length.ToString("X") + Environment.NewLine;

                File.WriteAllBytes(FileSave.FileName, CompressedData);
                File.WriteAllBytes(Path.Combine(savePath, fileName + ".raw"), FinalData);                
                File.WriteAllText(Path.Combine(savePath, fileName + ".asm"), asmText);
                File.WriteAllText(Path.Combine(savePath, fileName + ".h"), hText);
                File.WriteAllText(Path.Combine(savePath, fileName + ".c"), cText);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            WriteData();
        }
    }
}


/*
 * 
 * 



                
                

                

*/