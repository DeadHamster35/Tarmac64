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
        TM64_Geometry.OK64F3DObject[][] MasterObjects = new TM64_Geometry.OK64F3DObject[0][];
        TM64_Geometry.OK64Texture[][] TextureObjects = new TM64_Geometry.OK64Texture[0][];
        int lastMaterial = 0;
        int materialCount = 0;
        int LastSelectedIndex = -1;

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
                MasterObjects = new TM64_Geometry.OK64F3DObject[fileList.Length][];
                TextureObjects = new TM64_Geometry.OK64Texture[fileList.Length][];

                FBXBox.Items.Clear();

                for (int currentFile = 0; currentFile < fileList.Length; currentFile++)
                {
                    string FBXFilePath = fileList[currentFile];
                    int DataLength = Convert.ToInt32(MagicBox.Text, 16);


                    Scene ModelData = importer.ImportFile(FBXFilePath, PostProcessPreset.TargetRealTimeMaximumQuality);
                    string filename = Path.GetFileNameWithoutExtension(FBXFilePath);
                    int materialCount = ModelData.MaterialCount;
                    int SegmentID = SegmentBox.SelectedIndex;



                    Assimp.Node masterNode = ModelData.RootNode.FindNode("Master Objects");
                    if (masterNode != null)
                    {
                        TextureObjects[currentFile] = TarmacGeo.loadTextures(ModelData, FBXFilePath);
                        MasterObjects[currentFile] = TarmacGeo.createObjects(ModelData, TextureObjects[currentFile]);
                    }
                    else
                    {
                        MessageBox.Show("Error - " + FBXFilePath);
                    }
                    FBXBox.Items.Add(Path.GetFileNameWithoutExtension(FBXFilePath));
                }
                FBXBox.SelectedIndex = 0;
                TextureControl.Loaded = true;
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

                FBXBox.Items.Clear();

                Assimp.Node masterNode = ModelData.RootNode.FindNode("Master Objects");
                if (masterNode != null)
                {
                    //TextureObjects[0] = TarmacGeo.loadTextures(ModelData, FBXFilePath);
                    //MasterObjects[0] = TarmacGeo.createObjects(ModelData, TextureObjects[0]);
                    int a = 0;
                }
                else
                {
                    MessageBox.Show("Error - " + FBXFilePath);
                }
                FBXBox.Items.Add(Path.GetFileNameWithoutExtension(FBXFilePath));
                TextureControl.Loaded = true;

            }
        }



        private void FBXBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LastSelectedIndex != -1)
            {
                TextureObjects[LastSelectedIndex] = TextureControl.textureArray;
            }
            LastSelectedIndex = FBXBox.SelectedIndex;
            TextureControl.textureArray = TextureObjects[FBXBox.SelectedIndex];
            TextureControl.AddNewTextures(TextureObjects[FBXBox.SelectedIndex].Length);
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
                hText += "#define " + fileName + "_RawDataSize 0x" + OutputData.Length.ToString("X") + Environment.NewLine;
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

        public string GetRGBAString(TM64_Geometry.OK64Texture TextureObject, int Height, int Width)
        {
            int R, G, B, A;
            int ThisPixel = (Height * TextureObject.textureWidth) + Width;
            return "0x" + TextureObject.rawTexture[ThisPixel * 2].ToString("X").PadLeft(2, '0') + TextureObject.rawTexture[1 + (ThisPixel * 2)].ToString("X").PadLeft(2, '0') + ", ";
        }


        public string[] WriteVertDataC(TM64_Geometry.OK64F3DObject F3DObject)
        {
            List<string> Output = new List<string>();

            Output.Add("Vtx " + F3DObject.objectName + "_V[] = {");


            /*{ { 15,30,0}, 0, 	{2048, 	   0},  {255, 255, 254, 255} }*/
            foreach (var Face in F3DObject.modelGeometry)
            {
                foreach (var Vert in Face.VertData)
                {
                    Output.Add(
                        "{ {  { " + Vert.position.x.ToString() + ", " + Vert.position.y.ToString() + ", " + Vert.position.z.ToString() +
                        " }, 0,  {" + Vert.position.s.ToString() + ", " + Vert.position.t.ToString() + "}, " +
                        "{" + Vert.color.R.ToString() + ", " + Vert.color.G.ToString() + ", " + Vert.color.B.ToString() + ", " + Vert.color.A.ToString() + "} } },"
                        );
                }
            }

            Output.Add("};");

            return Output.ToArray();
        }
        public List<string> WriteTextureC(TM64_Geometry.OK64Texture TextureObject)
        {
            List<string> TextureData = new List<string>();

            TextureData.Add("unsigned short " + TextureObject.textureName + "[] = {");

            int PixelWidth = 0;
            int PixelHeight = 0;
            bool Finished = false;
            while (!Finished)
            {
                string RowString = "";
                for (int ThisRow = 0; ThisRow < 8; ThisRow++)
                {
                    RowString += GetRGBAString(TextureObject, PixelHeight, PixelWidth);

                    PixelWidth++;
                    if (PixelWidth >= TextureObject.textureWidth)
                    {
                        PixelHeight++;
                        if (PixelHeight == TextureObject.textureHeight)
                        {
                            Finished = true;
                        }
                        else
                        {
                            PixelWidth = 0;
                        }
                    }
                }
                TextureData.Add(RowString);
            }
            TextureData.Add("};");
            TextureData.Add("");
            return TextureData;
        }

        public string[] WriteRSPCommands(TM64_Geometry.OK64F3DObject TargetObject, TM64_Geometry.OK64Texture TextureObject, string GraphPtr)
        {
            List<string> Output = new List<string>();
            F3DSharp.F3DEX095 F3D = new F3DSharp.F3DEX095();
            string[] CombineNames = F3DSharp.F3DEX095_Parameters.GCCModeNames;
            string[] GeometryNames = F3DSharp.F3DEX095_Parameters.GeometryModeNames;
            string[] RenderNames = F3DSharp.F3DEX095_Parameters.RenderModeNames;
            string[] FormatNames = F3DSharp.F3DEX095_Parameters.TextureFormatNames;
            string[] BitSizeNames = F3DSharp.F3DEX095_Parameters.BitSizeNames;
            string[] ModeNames = F3DSharp.F3DEX095_Parameters.TextureModeNames;
            uint[] BitSizes = F3DSharp.F3DEX095_Parameters.BitSizes;
            uint[] GIMSize = F3DSharp.F3DEX095_Parameters.G_IM_ArrayLineBytes;

            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));


            TextureObject.GeometryModes = 0;
            for (int ThisCheck = 0; ThisCheck < F3DSharp.F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
            {
                if (TextureObject.GeometryBools[ThisCheck])
                {
                    TextureObject.GeometryModes |= F3DSharp.F3DEX095_Parameters.GeometryModes[ThisCheck];
                }
            }


            Output.Add("void GFX_" + TargetObject.objectName + " ()");
            Output.Add("{");


            //LOAD TEXTURE DATA RGBA16

            Output.Add("");
            Output.Add("\t//"+ TargetObject.objectName);
            Output.Add("\t//Start Texture Load");
            Output.Add("\t//" + TextureObject.textureName);
            Output.Add("");

            Output.Add("\tgSPTexture( " + GraphPtr + "++, 65535, 65535, 0, 0, 1);");            
            Output.Add("\tgDPPipeSync( " + GraphPtr + "++);");            
            Output.Add("\tgDPSetCombineMode( " + GraphPtr + "++, " + CombineNames[TextureObject.CombineModeA] + ", " + CombineNames[TextureObject.CombineModeB] + ");");            
            Output.Add("\tgDPSetRenderMode( " + GraphPtr + "++, " + RenderNames[TextureObject.RenderModeA] + ", " + RenderNames[TextureObject.RenderModeB] + ");");            
            Output.Add("\tgDPTileSync( " + GraphPtr + "++);");            
            Output.Add("\tgDPSetTile( " + GraphPtr + "++, " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", ((" + (TextureObject.textureWidth * GIMSize[TextureObject.BitSize]).ToString() + " + 7) >> 3), 0, 0, 0, " + ModeNames[TextureObject.TFlag]
                + ", " + heightex.ToString() + ", 0, " + ModeNames[TextureObject.SFlag] + ", " + widthex.ToString() + ", 0);");            
            Output.Add("\tgDPSetTileSize( " + GraphPtr + "++, 0, 0, 0, (" + TextureObject.textureWidth.ToString() + " - 1) << 2, (" + TextureObject.textureHeight.ToString() + " - 1) << 2);");

            Output.Add("\tgDPClearGeometryMode( " + GraphPtr + "++, " + F3DSharp.F3DEX095_Parameters.AllGeometryModes.ToString() + ");");
            Output.Add("\tgDPSetGeometryMode( " + GraphPtr + "++, " + TextureObject.GeometryModes.ToString() + ");");

            Output.Add("\tgDPSetTextureImage( " + GraphPtr + "++, " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", 1, &" + TextureObject.textureName +" );");            
            Output.Add("\tgDPTileSync( " + GraphPtr + "++);");            
            Output.Add("\tgDPSetTile( " + GraphPtr + "++, " + FormatNames[TextureObject.TextureFormat] + ", " + BitSizeNames[TextureObject.BitSize]
                + ", 0, 0, 7, 0, 0, 0, 0, 0, 0, 0);");            
            Output.Add("\tgDPLoadSync( " + GraphPtr + "++);");            
            Output.Add("\tgDPLoadBlock( " + GraphPtr + "++, 7, 0, 0, ((" +
                TextureObject.textureWidth.ToString() + " * " + TextureObject.textureHeight.ToString() + ") - 1), "
                + F3D.CALCDXT(Convert.ToUInt32(TextureObject.textureWidth), BitSizes[TextureObject.BitSize]).ToString() + ");");
            //END RGBA16 DRAW

            Output.Add("");
            Output.Add("\t//End Texture Load");
            Output.Add("\t//Start DrawCalls");
            Output.Add("");

            int VertIndex = 0;
            int LocalVertIndex = 0;
            int FaceIndex = 0;

            bool Finished = false; 

            while (!Finished)
            {
                Output.Add("");
                Output.Add("\tgSPVertex( " + GraphPtr + "++, &" + TargetObject.objectName + "_V[" + VertIndex.ToString() + "] , 30, 0);");
                LocalVertIndex = 0;
                for (int ThisFace = 0; ThisFace < 5; ThisFace++)
                {
                    if ((FaceIndex + 1) < TargetObject.modelGeometry.Length)
                    {
                        Output.Add("\t\tgSP2Triangles( " + GraphPtr + "++, " +
                            LocalVertIndex.ToString() + ", " +
                            (LocalVertIndex + 1).ToString() + ", " +
                            (LocalVertIndex + 2).ToString() + ", " +
                            "0, " +
                            (LocalVertIndex + 3).ToString() + ", " +
                            (LocalVertIndex + 4).ToString() + ", " +
                            (LocalVertIndex + 5).ToString() + ", " +
                            "0 );"
                            );

                        FaceIndex += 2;
                        VertIndex += 6;
                        LocalVertIndex += 6;
                    }
                    else if (FaceIndex < TargetObject.modelGeometry.Length)
                    {
                        Output.Add("\t\tgSP2Triangles( " + GraphPtr + "++, " +
                            "0, " +
                            "0, " +
                            "0, " +
                            "0, " +
                            VertIndex.ToString() +
                            (VertIndex + 1).ToString() + ", " +
                            (VertIndex + 2).ToString() + ", " +
                            "0 );"
                            );

                        FaceIndex += 1;
                        LocalVertIndex += 3;
                        VertIndex += 3;
                    }
                    else
                    {
                        Finished = true;
                        ThisFace = 5;
                    }
                    
                    
                }
                
            }

            Output.Add("");
            Output.Add("\t//End DrawCalls");
            Output.Add("\t//End "+TargetObject.objectName);            
            Output.Add("");


            Output.Add("}");
            return Output.ToArray();
        }

        public void ExportCData()
        {
            byte[] OutputData = new byte[0];
            byte[] HeaderData = new byte[0];

            int SegmentID = Convert.ToInt32(SegmentBox.SelectedIndex);
            int DataLength = Convert.ToInt32(MagicBox.Text, 16) + (MasterObjects.Length * 16);

            List<string> OutputFile = new List<string>();
            List<string> HFileOutput = new List<string>();

            for (int currentItem = 0; currentItem < MasterObjects.Length; currentItem++)
            {

                OutputData = TarmacGeo.writeModelTextures(OutputData, TextureObjects[currentItem], DataLength);
                OutputData = TarmacGeo.compileTextureObject(OutputData, TextureObjects[currentItem], DataLength, SegmentID);
                OutputData = TarmacGeo.compileF3DObject(OutputData, MasterObjects[currentItem], TextureObjects[currentItem], DataLength, SegmentID);

                foreach (var SubTexture in TextureObjects[currentItem])
                {
                    if (SubTexture.textureName != null)
                    {
                        OutputFile.AddRange(WriteTextureC(SubTexture));
                    }
                }

                foreach (var SubMesh in MasterObjects[currentItem])
                {
                    OutputFile.AddRange(WriteVertDataC(SubMesh));
                }

                foreach (var SubMesh in MasterObjects[currentItem])
                {
                    OutputFile.AddRange(WriteRSPCommands(SubMesh, TextureObjects[currentItem][SubMesh.materialID], "GraphPtrOffset"));

                    HFileOutput.Add("extern void GFX_" + SubMesh.objectName + "();");
                }

            }

            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                string savePath = Path.GetDirectoryName(FileSave.FileName);
                string fileName = Path.GetFileNameWithoutExtension(FileSave.FileName);
                File.WriteAllLines(Path.Combine(savePath, fileName + ".c"), OutputFile.ToArray());
                File.WriteAllLines(Path.Combine(savePath, fileName + ".h"), HFileOutput.ToArray());
            }


        }
        private void button4_Click(object sender, EventArgs e)
        {
            ExportCData();
        }
    }
}


/*
 * 
 * 



                
                

                

*/