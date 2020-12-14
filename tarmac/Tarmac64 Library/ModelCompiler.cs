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
    public partial class ModelCompiler : Form
    {
        public ModelCompiler()
        {
            InitializeComponent();
        }

        OpenFileDialog FileOpen = new OpenFileDialog();
        SaveFileDialog FileSave = new SaveFileDialog();
        TM64 Tarmac = new TM64();
        TM64_Geometry TarmacGeo = new TM64_Geometry();
        TM64_Geometry.OK64F3DObject[] MasterObjects = new TM64_Geometry.OK64F3DObject[0];
        TM64_Geometry.OK64Texture[] TextureObjects = new TM64_Geometry.OK64Texture[0];

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] OutputData = new byte[0];
            string outputText = "";
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {

                

                string[] fileList = Directory.GetFiles(dialog.FileName, "*.FBX*", SearchOption.AllDirectories);
                AssimpContext importer = new AssimpContext();

                foreach (string FBXFilePath in fileList)
                {
                    int DataLength = Convert.ToInt32(MagicBox.Text, 16);
                    

                    Scene ModelData = importer.ImportFile(FBXFilePath, PostProcessPreset.TargetRealTimeMaximumQuality);
                    string filename = Path.GetFileNameWithoutExtension(FBXFilePath);
                    int materialCount = ModelData.MaterialCount;
                    int SegmentID = SegmentBox.SelectedIndex;



                    Assimp.Node masterNode = ModelData.RootNode.FindNode("Master Objects");
                    if (masterNode != null)
                    {
                        TextureObjects = TarmacGeo.loadTextures(ModelData, FBXFilePath);
                        MasterObjects = TarmacGeo.createObjects(ModelData);

                        
                        OutputData = TarmacGeo.writeRawTextures(OutputData, TextureObjects, DataLength);
                        TarmacGeo.compileTextureObject(ref DataLength, ref OutputData, OutputData, TextureObjects, DataLength, SegmentID);
                        TarmacGeo.compileF3DObject(ref DataLength, ref OutputData, OutputData, MasterObjects, TextureObjects, DataLength, SegmentID);

                        outputText += filename + "-" + DataLength.ToString("X") + Environment.NewLine;
                        OutputData = TarmacGeo.compileObjectList(ref DataLength, ModelData, OutputData, MasterObjects, TextureObjects, SegmentID);

                        
                    }
                }

                if (FileSave.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(FileSave.FileName + ".raw.bin", OutputData);
                    File.WriteAllBytes(FileSave.FileName, Tarmac.CompressMIO0(OutputData));
                    File.WriteAllText(FileSave.FileName + ".help.txt", outputText);
                }

            }
        }

        private void ModelCompiler_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                SegmentBox.Items.Add(i.ToString("X"));
            }
            SegmentBox.SelectedIndex = 8;
        }
    }
}
