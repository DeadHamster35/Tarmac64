using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using Tarmac64_Library;
using Texture64;
using Assimp;
using F3DSharp;

namespace TarmacCL
{
    class Program
    {

        
        static AssimpContext FBXParser = new AssimpContext();
        static float ProgramVersion = 0.2f;


        static void Main(string[] args)
        {
            TM64 Tarmac64 = new TM64();
            short CWidth, CHeight;
            CWidth = 90;
            CHeight = 40;
            Console.SetWindowSize(CWidth, CHeight);


            
            if (args.Length == 0)
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Tarmac Command Line");
                Console.WriteLine("Build - " + ProgramVersion.ToString());
                Console.WriteLine("");
                Console.WriteLine("Available Commands:");
                Console.WriteLine("");
                Console.WriteLine("\t geometry <string>[filepath-directory] <string>[Compilation Type, -C or -B]");
                Console.WriteLine("\t\t Parse a directory for FBX files.");                
                Console.WriteLine("\t\t TarmacCL.exe geometry \"C:\\ModelData\\Directory\"");
                Console.WriteLine("");
                Console.WriteLine("\t geometry <string>[filepath-fbx-file] <string>[Compilation Type, -C or -B]");
                Console.WriteLine("\t\t Compile a single FBX file.");
                Console.WriteLine("\t\t TarmacCL.exe geometry \"C:\\ModelData\\Directory\\ModelData.FBX\"");
                Console.WriteLine("");
                Console.WriteLine("\t texture <string>[filepath-fbx-file] <int> CodecType");
                Console.WriteLine("\t\t Compile a single Texture file from a PNG/JPEG to specified Codec:");
                Console.WriteLine("\t\t\t 0-RGBA16, 1-RGBA32, 2-IA16, 3-IA8");
                Console.WriteLine("\t\t\t 4-IA4, 5-I8, 6-I4, 7-CI8, 8-CI4");
                Console.WriteLine("\t\t TarmacCL.exe texture \"C:\\TextureData\\Directory\\TextureFile.PNG\" 0");
                Console.WriteLine("");
                Console.WriteLine("\t batchtexture <string>[filepath-fbx-file] <int> CodecType");
                Console.WriteLine("\t\t Compile a directory of Texture files from a PNG/JPEG to specified Codec.");
                Console.WriteLine("\t\t TarmacCL.exe batchtexture \"C:\\TextureData\\Directory\\\" 0");


                return;
            }

            switch (args[0])
            {
                case "help":
                default:
                    {

                        break;
                    }
                case "geometry":
                    {
                        if (args.Length >= 2)
                        {
                            string FilePath = args[1];
                            FileAttributes FileAttr = File.GetAttributes(FilePath);
                            if (File.Exists(FilePath))
                            {

                            }
                            else if ((FileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                string[] FileList = Directory.GetFiles(FilePath, "*.FBX", SearchOption.AllDirectories);

                                TM64_Geometry.OK64F3DObject[][] MasterObjects = new TM64_Geometry.OK64F3DObject[FileList.Length][];
                                TM64_Geometry.OK64Texture[][] TextureObjects = new TM64_Geometry.OK64Texture[FileList.Length][];
                                foreach (var ThisFile in FileList)
                                {
                                    ProcessFBX(ThisFile);
                                }
                                //Directory parse for any files
                                

                            }

                        }
                        break;
                    }
                case "texture":
                    {
                        Tarmac64.WriteTextureFile(args[1], Convert.ToInt32(args[2]));
                        break;
                    }
                case "batchtexture":
                    {
                        Tarmac64.WriteBatchTextures(args[1], Convert.ToInt32(args[2]));
                        break;
                    }
            }

        }





        static string GetFullPathWithoutExtension(string path)
        {
            return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileNameWithoutExtension(path));
        }
        static void WriteTextureName(TM64_Geometry.OK64Texture TargetTexture)
        {
            Console.WriteLine("-----");
            Console.WriteLine(TargetTexture.textureName);
            Console.WriteLine("-----");
            Console.WriteLine();
        }
        static void SubProcessFBX(string TargetPath)
        {
            Console.Clear();
            string savePath = Path.GetDirectoryName(TargetPath);
            savePath = Path.Combine(savePath, "Textures");

            List<string> OutputFile = new List<string>();
            List<string> HFileOutput = new List<string>();

            TM64_Geometry.OK64Texture[] TextureObjects = new TM64_Geometry.OK64Texture[0];
            TM64_Geometry.OK64F3DObject[] MasterObjects = new TM64_Geometry.OK64F3DObject[0];
            TM64_Geometry TM64Geometry = new TM64_Geometry();
            Scene Mode = new Scene();
            Scene ModelData = FBXParser.ImportFile(TargetPath, PostProcessPreset.TargetRealTimeMaximumQuality);
            string filename = Path.GetFileNameWithoutExtension(TargetPath);
            int materialCount = ModelData.MaterialCount;
            Console.WriteLine("-----");
            Console.WriteLine("Compiling New Model");
            Console.WriteLine(filename);
            Console.WriteLine("-----");

            Console.WriteLine("Please Insert Segment ID (Default 10)");
            int SegmentID = Convert.ToInt32(Console.ReadLine());

            Assimp.Node masterNode = ModelData.RootNode.FindNode("Master Objects");

            if (masterNode != null)
            {
                TextureObjects = TM64Geometry.loadTextures(ModelData, TargetPath);
                MasterObjects = TM64Geometry.CreateObjects(ModelData, TextureObjects);
            }

            for (int ThisTexture = 0; ThisTexture < TextureObjects.Length; ThisTexture++)
            {


                //
                //
                Console.Clear();
                WriteTextureName(TextureObjects[ThisTexture]);
                Console.WriteLine("Select Texture Codec");
                for (int ThisCodec = 0; ThisCodec < F3DEX095_Parameters.TextureFormatNames.Length; ThisCodec++)
                {
                    Console.WriteLine(ThisCodec.ToString() + " - " + F3DEX095_Parameters.TextureFormatNames[ThisCodec]);
                }
                TextureObjects[ThisTexture].TextureFormat = Convert.ToInt32(Console.ReadLine());

                //
                //
                Console.Clear();
                WriteTextureName(TextureObjects[ThisTexture]);
                Console.WriteLine("Select Bit Depth");
                for (int ThisBit = 0; ThisBit < F3DEX095_Parameters.BitSizeNames.Length; ThisBit++)
                {
                    Console.WriteLine(ThisBit.ToString() + " - " + F3DEX095_Parameters.BitSizeNames[ThisBit]);
                }
                TextureObjects[ThisTexture].CombineModeB = Convert.ToInt32(Console.ReadLine());

                //
                //
                Console.Clear();
                WriteTextureName(TextureObjects[ThisTexture]);
                Console.WriteLine("Select Combine Mode A");
                for (int ThisCC = 0; ThisCC < F3DEX095_Parameters.ColorCombineNames.Length; ThisCC++)
                {
                    Console.WriteLine(ThisCC.ToString() + " - " + F3DEX095_Parameters.ColorCombineNames[ThisCC]);
                }
                TextureObjects[ThisTexture].CombineModeA = Convert.ToInt32(Console.ReadLine());

                //
                //
                Console.Clear();
                WriteTextureName(TextureObjects[ThisTexture]);
                Console.WriteLine("Select Combine Mode B");
                for (int ThisCC = 0; ThisCC < F3DEX095_Parameters.ColorCombineNames.Length; ThisCC++)
                {
                    Console.WriteLine(ThisCC.ToString() + " - " + F3DEX095_Parameters.ColorCombineNames[ThisCC]);
                }
                TextureObjects[ThisTexture].CombineModeB = Convert.ToInt32(Console.ReadLine());

                //
                //
                Console.Clear();
                WriteTextureName(TextureObjects[ThisTexture]);
                Console.WriteLine("Select Render Mode A");
                for (int ThisRM = 0; ThisRM < F3DEX095_Parameters.RenderModeNames.Length; ThisRM++)
                {
                    Console.WriteLine(ThisRM.ToString() + " - " + F3DEX095_Parameters.RenderModeNames[ThisRM]);
                }
                TextureObjects[ThisTexture].RenderModeA = Convert.ToInt32(Console.ReadLine());


                //
                //
                Console.Clear();
                WriteTextureName(TextureObjects[ThisTexture]);
                Console.WriteLine("Select Render Mode B");
                for (int ThisRM = 0; ThisRM < F3DEX095_Parameters.RenderModeNames.Length; ThisRM++)
                {
                    Console.WriteLine(ThisRM.ToString() + " - " + F3DEX095_Parameters.RenderModeNames[ThisRM]);
                }
                TextureObjects[ThisTexture].RenderModeB  = Convert.ToInt32(Console.ReadLine());

                //
                //
                Console.Clear();
                WriteTextureName(TextureObjects[ThisTexture]);
                Console.WriteLine("Select S Texture Coordinate Mode");
                for (int ThisMode = 0; ThisMode < F3DEX095_Parameters.TextureModeNames.Length; ThisMode++)
                {
                    Console.WriteLine(ThisMode.ToString() + " - " + F3DEX095_Parameters.TextureModeNames[ThisMode]);
                }
                TextureObjects[ThisTexture].SFlag = Convert.ToInt32(Console.ReadLine());

                //
                //
                Console.Clear();
                WriteTextureName(TextureObjects[ThisTexture]);
                Console.WriteLine("Select T Texture Coordinate Mode");
                for (int ThisMode = 0; ThisMode < F3DEX095_Parameters.TextureModeNames.Length; ThisMode++)
                {
                    Console.WriteLine(ThisMode.ToString() + " - " + F3DEX095_Parameters.TextureModeNames[ThisMode]);
                }
                TextureObjects[ThisTexture].TFlag = Convert.ToInt32(Console.ReadLine());

                Console.Clear();
                int DataLength = 0;
                byte[] TempBuffer = new byte[0];
                TM64Geometry.WriteModelTextures(TempBuffer, TextureObjects, DataLength);
                TM64Geometry.CompileTextureObjects(TempBuffer, TextureObjects, DataLength, SegmentID);

                OutputFile.AddRange(TM64Geometry.WriteTextureC(TextureObjects[ThisTexture]));
                HFileOutput.Add("extern unsigned short[] " + TextureObjects[ThisTexture].textureName + "();");

                
                Directory.CreateDirectory(savePath);
                File.WriteAllLines(Path.Combine(savePath, TextureObjects[ThisTexture].textureName + ".c"), OutputFile.ToArray());
                
            }

            File.WriteAllLines(Path.Combine(savePath, Path.GetFileNameWithoutExtension(TargetPath) + ".h"), HFileOutput.ToArray());
        }

        static void ProcessFBX(string TargetPath)
        {
            if (File.Exists(TargetPath))
            {
                byte[] FileData = File.ReadAllBytes(TargetPath);
                string ChecksumPath = GetFullPathWithoutExtension(TargetPath) + ".OK64.MD5";
                if (File.Exists(ChecksumPath))
                {
                    //Existing checksum file found- compare to current FBX file.
                    string[] CheckSum = File.ReadAllLines(ChecksumPath);
                    MD5 CheckMD5 = MD5.Create();
                    string NewCheck = BitConverter.ToString(CheckMD5.ComputeHash(FileData)).Replace("-", "");
                    if (NewCheck == CheckSum[0])
                    {
                        Console.WriteLine("Skipping " + Path.GetFileNameWithoutExtension(TargetPath) + " with unchanged checksum");
                    }
                    else
                    {
                        Console.WriteLine("New Checksum for " + Path.GetFileNameWithoutExtension(TargetPath));

                    }
                }
                else
                {
                    SubProcessFBX(TargetPath);
                    // No MD5 file found- process new geometry.
                }
            }
        }


    }

}
