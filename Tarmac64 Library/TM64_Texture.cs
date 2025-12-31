using Aspose.ThreeD.Render;
using Cereal64.Common;
using F3DSharp;
using F3DSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Tarmac64_Library.Properties;
using Texture64;
using static Tarmac64_Library.TM64_Geometry;

namespace Tarmac64_Library
{
    public class TM64_Texture
    {
        F3DEX095 F3D = new F3DEX095();


        public class OK64TexelData
        {

            public string textureName { get; set; }
            public Image textureBitmap { get; set; }
            public byte[] compressedTexture { get; set; }
            public byte[] PaletteData { get; set; }
            public byte[] TextureData { get; set; }
            public int compressedSize { get; set; }
            public int fileSize { get; set; }
            public int imagePosition { get; set; }
            public int palettePosition { get; set; }
            public int paletteSize { get; set; }
            public int F3DPosition { get; set; }

            //
            public string texturePath { get; set; }
            public string alphaPath { get; set; }
            public int textureWidth { get; set; }
            public int textureHeight { get; set; }
            public int BitSize { get; set; }
            public int TextureFormat { get; set; }
            public int SFlag { get; set; }
            public int TFlag { get; set; }
            public int textureScrollS { get; set; }
            public int textureScrollT { get; set; }
            public int textureScreen { get; set; }
        }
        public class OK64ColorCombine
        {
            public int CycleMode { get; set; }
            public bool AdvancedModeA { get; set; }
            public bool AdvancedModeB { get; set; }
            public int[] CombineValuesA { get; set; }
            public int[] CombineValuesB { get; set; }
            public int CombineModeA { get; set; }
            public int CombineModeB { get; set; }
            public int RenderModeA { get; set; }
            public int RenderModeB { get; set; }
            public UInt32 GeometryModes { get; set; }
            public bool[] GeometryBools { get; set; }
            public int TextureFilter { get; set; }
            public System.Drawing.Color Environment { get; set; }
            public System.Drawing.Color Primary { get; set; }
            public int EnvironmentAlpha { get; set; }
            public int PrimaryAlpha { get; set; }
        }
        public class OK64Texture
        {

            public int CCPosition { get; set; }

            public List<OK64TexelData> TexelData { get; set; }
            public OK64ColorCombine ColorCombine { get; set; }

            
            public double GLShiftS { get; set; }
            public double GLShiftT { get; set; }

            public OK64Texture()
            {
                ColorCombine = new OK64ColorCombine();
                TexelData = new List<OK64TexelData>();

                TexelData.Add(new OK64TexelData());
                TexelData[0].textureBitmap = Resources.TextureNotFound;
                
                ColorCombine.Environment = System.Drawing.Color.White;
                ColorCombine.Primary = System.Drawing.Color.White;
                ColorCombine.EnvironmentAlpha = 255;
                ColorCombine.PrimaryAlpha = 255;

                ColorCombine.AdvancedModeA = false;
                ColorCombine.AdvancedModeB = false;
                ColorCombine.CombineValuesA = new int[8];
                ColorCombine.CombineValuesB = new int[8];
            }
            public OK64Texture(XmlDocument XMLDoc, string Parent, int ChildIndex)
            {
                TM64 Tarmac = new TM64();
                XmlNode Owner = XMLDoc.SelectSingleNode(Parent);
                XmlNode Target = Owner.ChildNodes[ChildIndex];
                ColorCombine = new OK64ColorCombine();
                TexelData = new List<OK64TexelData>();
                

                string HeaderName = "Texture_" + ChildIndex.ToString();
                int TexelCount = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "TexelCount", "1"));

                for (int ThisTexel = 0; ThisTexel < TexelCount; ThisTexel++) 
                {
                    //backwards compatability
                    //append "1" to texel1, leave texel0 blank.
                    string Footer = "";
                    if (ThisTexel > 0)
                    {
                        Footer = "1";
                    }
                    //
                    //

                    TexelData.Add(new OK64TexelData());
                    TexelData[ThisTexel].textureName = Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "textureName", "Texel" + ThisTexel.ToString());
                    TexelData[ThisTexel].texturePath = Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "texturePath");
                    TexelData[ThisTexel].alphaPath = Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "alphaPath");
                    TexelData[ThisTexel].TextureFormat = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "TextureFormat", "0"));
                    TexelData[ThisTexel].SFlag = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "SFlag", "0"));
                    TexelData[ThisTexel].TFlag = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "TFlag", "0"));
                    TexelData[ThisTexel].textureScrollS = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "textureScrollS", "0"));
                    TexelData[ThisTexel].textureScrollT = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "textureScrollT", "0"));
                    TexelData[ThisTexel].textureScreen = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "textureScreen", "0"));
                    TexelData[ThisTexel].BitSize = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "BitSize", "0"));


                    //
                    // Process TexelData
                    //

                    if (File.Exists(TexelData[ThisTexel].texturePath))
                    {
                        using (var fs = new FileStream(TexelData[ThisTexel].texturePath, FileMode.Open, FileAccess.Read))
                        {
                            Image Raw = Image.FromStream(fs);
                            TexelData[ThisTexel].textureBitmap = Raw;
                            TexelData[ThisTexel].textureWidth = Raw.Width;
                            TexelData[ThisTexel].textureHeight = Raw.Height;
                            fs.Close();
                        }
                    }
                }



                ColorCombine.CycleMode = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "CycleMode", "0"));
                ColorCombine.TextureFilter = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "TextureFilter", "0"));

                ColorCombine.CombineModeA = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "CombineModeA", "0"));
                ColorCombine.CombineModeB = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "CombineModeB", "0"));

                ColorCombine.RenderModeA = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "RenderModeA", "0"));
                ColorCombine.RenderModeB = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "RenderModeB", "0"));

                ColorCombine.GeometryModes = Convert.ToUInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "GeometryModes", "0"));
                ColorCombine.GeometryBools = new bool[12];

                int[] KDL = Tarmac.LoadElements(XMLDoc, Parent + "/" + HeaderName, "GeometryBools", "0");
                for (int ThisBool = 0; ThisBool < 12; ThisBool++)
                {
                    ColorCombine.GeometryBools[ThisBool] = Convert.ToBoolean(KDL[ThisBool]);
                }

                GLShiftS = Convert.ToDouble(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "GLShiftS", "0"));
                GLShiftT = Convert.ToDouble(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "GLShiftT", "0"));

                ColorCombine.AdvancedModeA = Convert.ToBoolean(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "AdvanceA", "false"));
                ColorCombine.AdvancedModeB = Convert.ToBoolean(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "AdvanceB", "false"));

                ColorCombine.CombineValuesA = Tarmac.LoadElements(XMLDoc, Parent + "/" + HeaderName, "CombineValuesA", "0");
                ColorCombine.CombineValuesB = Tarmac.LoadElements(XMLDoc, Parent + "/" + HeaderName, "CombineValuesB", "0");


                int R, G, B;

                R = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "EnvR", "255"));
                G = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "EnvG", "255"));
                B = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "EnvB", "255"));

                ColorCombine.Environment = System.Drawing.Color.FromArgb(255, R, G, B);
                ColorCombine.EnvironmentAlpha = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "EnvA", "255"));

                R = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "PrimR", "255"));
                G = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "PrimG", "255"));
                B = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "PrimB", "255"));

                ColorCombine.Primary = System.Drawing.Color.FromArgb(255, R, G, B);
                ColorCombine.PrimaryAlpha = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent + "/" + HeaderName, "PrimA", "255"));

            }

            public void SaveXML(XmlDocument XMLDoc, XmlElement Parent, int TextureID)
            {
                TM64 Tarmac = new TM64();
                XmlElement TextureXML = XMLDoc.CreateElement("Texture_" + TextureID.ToString());
                Parent.AppendChild(TextureXML);

                

                Tarmac.GenerateElement(XMLDoc, TextureXML, "TexelCount", TexelData.Count);

                for (int ThisTexel = 0; ThisTexel < TexelData.Count; ThisTexel++)
                {
                    //backwards compatability
                    //append "1" to texel1, leave texel0 blank.
                    string Footer = "";
                    if (ThisTexel > 0)
                    {
                        Footer = "1";
                    }
                    //
                    //

                    Tarmac.GenerateElement(XMLDoc, TextureXML, "textureName", TexelData[ThisTexel].textureName);
                    if (TexelData[ThisTexel].texturePath != null)
                    {
                        Tarmac.GenerateElement(XMLDoc, TextureXML, "texturePath" + Footer, TexelData[ThisTexel].texturePath);
                    }
                    else
                    {
                        Tarmac.GenerateElement(XMLDoc, TextureXML, "texturePath" + Footer, "NULL");
                    }


                    Tarmac.GenerateElement(XMLDoc, TextureXML, "alphaPath" + Footer, TexelData[ThisTexel].alphaPath);
                    Tarmac.GenerateElement(XMLDoc, TextureXML, "BitSize" + Footer, TexelData[ThisTexel].BitSize);
                    Tarmac.GenerateElement(XMLDoc, TextureXML, "TextureFormat" + Footer, TexelData[ThisTexel].TextureFormat);
                    Tarmac.GenerateElement(XMLDoc, TextureXML, "SFlag" + Footer, TexelData[ThisTexel].SFlag);
                    Tarmac.GenerateElement(XMLDoc, TextureXML, "TFlag" + Footer, TexelData[ThisTexel].TFlag);
                    Tarmac.GenerateElement(XMLDoc, TextureXML, "textureScrollS" + Footer, TexelData[ThisTexel].textureScrollS);
                    Tarmac.GenerateElement(XMLDoc, TextureXML, "textureScrollT" + Footer, TexelData[ThisTexel].textureScrollT);
                    Tarmac.GenerateElement(XMLDoc, TextureXML, "textureScreen" + Footer, TexelData[ThisTexel].textureScreen);

                }



                Tarmac.GenerateElement(XMLDoc, TextureXML, "CycleMode", ColorCombine.CycleMode);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "TextureFilter", ColorCombine.TextureFilter);

                Tarmac.GenerateElement(XMLDoc, TextureXML, "CombineModeA", ColorCombine.CombineModeA);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "CombineModeB", ColorCombine.CombineModeB);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "RenderModeA", ColorCombine.RenderModeA);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "RenderModeB", ColorCombine.RenderModeB);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "GeometryModes", ColorCombine.GeometryModes);
                
                Tarmac.GenerateElement(XMLDoc, TextureXML, "GeometryBools", ColorCombine.GeometryBools);

                Tarmac.GenerateElement(XMLDoc, TextureXML, "AdvanceA", ColorCombine.AdvancedModeA);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "AdvanceA", ColorCombine.AdvancedModeB);

                Tarmac.GenerateElement(XMLDoc, TextureXML, "CombineValuesA", ColorCombine.CombineValuesA);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "CombineValuesB", ColorCombine.CombineValuesB);

                Tarmac.GenerateElement(XMLDoc, TextureXML, "EnvR", ColorCombine.Environment.R);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "EnvG", ColorCombine.Environment.G);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "EnvB", ColorCombine.Environment.B);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "EnvA", ColorCombine.EnvironmentAlpha);

                Tarmac.GenerateElement(XMLDoc, TextureXML, "PrimR", ColorCombine.Primary.R);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "PrimG", ColorCombine.Primary.G);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "PrimB", ColorCombine.Primary.B);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "PrimA", ColorCombine.PrimaryAlpha);



                Tarmac.GenerateElement(XMLDoc, TextureXML, "GLShiftS", GLShiftS);
                Tarmac.GenerateElement(XMLDoc, TextureXML, "GLShiftT", GLShiftT);

                


            }

        }

        public byte[] ColorCombine(OK64Texture TextureObject, UInt32 Segment, bool GeometryToggle = true, bool FogToggle = false, bool Transparent = false)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];



            //pipe sync.
            binaryWriter.Write(
                F3D.gsDPPipeSync()
            );


            binaryWriter.Write(
                F3D.gsDPSetTextureFilter(F3DEX095_Parameters.TextureFilters[TextureObject.ColorCombine.TextureFilter])
            );

            binaryWriter.Write(
                F3D.gsDPSetCycleType(F3DEX095_Parameters.CycleTypes[TextureObject.ColorCombine.CycleMode])
            );

            binaryWriter.Write(F3D.gsDPSetPrimColor(0, 0,
                Convert.ToUInt32(TextureObject.ColorCombine.Primary.R),
                Convert.ToUInt32(TextureObject.ColorCombine.Primary.G),
                Convert.ToUInt32(TextureObject.ColorCombine.Primary.B),
                Convert.ToUInt32(TextureObject.ColorCombine.PrimaryAlpha)
            ));

            binaryWriter.Write(F3D.gsDPSetEnvColor(
                Convert.ToUInt32(TextureObject.ColorCombine.Environment.R),
                Convert.ToUInt32(TextureObject.ColorCombine.Environment.G),
                Convert.ToUInt32(TextureObject.ColorCombine.Environment.B),
                Convert.ToUInt32(TextureObject.ColorCombine.EnvironmentAlpha)
            ));

            uint[] CombineA = new uint[8];
            uint[] CombineB = new uint[8];


            if (TextureObject.ColorCombine.AdvancedModeA)
            {
                for (int This = 0; This < 4; This++)
                {
                    CombineA[This] = F3DEX095_Parameters.ColorCombineModes[Convert.ToInt32(TextureObject.ColorCombine.CombineValuesA[This])];
                    CombineA[This + 4] = F3DEX095_Parameters.AlphaCombineModes[Convert.ToInt32(TextureObject.ColorCombine.CombineValuesA[This + 4])];
                }
            }
            else
            {
                CombineA = F3DEX095_Parameters.GCCModes[TextureObject.ColorCombine.CombineModeA];
            }

            if (TextureObject.ColorCombine.AdvancedModeB)
            {
                for (int This = 0; This < 4; This++)
                {
                    CombineB[This] = F3DEX095_Parameters.ColorCombineModes[Convert.ToInt32(TextureObject.ColorCombine.CombineValuesB[This])];
                    CombineB[This + 4] = F3DEX095_Parameters.AlphaCombineModes[Convert.ToInt32(TextureObject.ColorCombine.CombineValuesB[This + 4])];
                }
            }
            else
            {
                CombineB = F3DEX095_Parameters.GCCModes[TextureObject.ColorCombine.CombineModeB];
            }


            binaryWriter.Write(
                F3D.gsDPSetCombineMode(
                    CombineA,
                    CombineB
                )
            );



            //set render mode
            if (FogToggle)
            {
                binaryWriter.Write(
                    F3D.gsDPSetRenderMode(
                        F3DEX095_Parameters.G_RM_FOG_SHADE_A,
                        F3DEX095_Parameters.RenderModesSimple[TextureObject.ColorCombine.RenderModeB]
                    )
                );
            }
            else
            {
                binaryWriter.Write(
                    F3D.gsDPSetRenderMode(
                        F3DEX095_Parameters.RenderModesSimple[TextureObject.ColorCombine.RenderModeA],
                        F3DEX095_Parameters.RenderModesSimple[TextureObject.ColorCombine.RenderModeB]
                    )
                );
            }

            //
            //



            //setup the Geometry Mode parameter
            //clear existing modes
            binaryWriter.Write(F3D.gsSPClearGeometryMode(F3DEX095_Parameters.AllGeometryModes));

            TextureObject.ColorCombine.GeometryModes = 0;
            for (int ThisCheck = 0; ThisCheck < F3DEX095_Parameters.GeometryModes.Length; ThisCheck++)
            {
                if (TextureObject.ColorCombine.GeometryBools[ThisCheck])
                {
                    TextureObject.ColorCombine.GeometryModes |= F3DEX095_Parameters.GeometryModes[ThisCheck];
                }
            }
            if (FogToggle)
            {
                TextureObject.ColorCombine.GeometryModes |= F3DEX095_Parameters.G_FOG;
            }
            //set the mode we made above.
            binaryWriter.Write(F3D.gsSPSetGeometryMode(TextureObject.ColorCombine.GeometryModes)); 
            



            return memoryStream.ToArray();
        }

        public byte[] UntexturedPolygons(OK64Texture TextureObject, bool GeometryToggle = true, bool FogToggle = false)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            //set MIP levels to 0.


            binaryWriter.Write(
                F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE)
            );

            binaryWriter.Write
            (
                F3D.gsSPTexture(65535, 65535, 0, 0, 1)
            );

            //pipe sync.
            binaryWriter.Write(
                F3D.gsDPPipeSync()
            );

            binaryWriter.Write(F3D.gsSPEndDisplayList());                                             //End the Display List





            return memoryStream.ToArray();

        }

        public byte[] RGBA(OK64Texture TextureObject, UInt32 Segment, uint Tile, uint TMEM, bool FogToggle = false)
        {
            byte[] SegmentByte = BitConverter.GetBytes(Segment);
            Array.Reverse(SegmentByte);
            int SegmentID = BitConverter.ToInt32(SegmentByte, 0);

            uint LoadTile = Convert.ToUInt32(6 + Tile);
            uint RenderTile = Convert.ToUInt32(Tile);
            int TextureIndex = Convert.ToInt32(Tile);

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];
            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.TexelData[TextureIndex].textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.TexelData[TextureIndex].textureWidth) / Math.Log(2));




            binaryWriter.Write(
                F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE)
            );

            binaryWriter.Write
            (
                F3D.gsSPTexture(65535, 65535, 0, 0, 1)
            );


            //Load Texture Data
            binaryWriter.Write(
                F3D.gsNinLoadTextureImage(
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].imagePosition | Convert.ToUInt32(Segment << 24)),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TexelData[TextureIndex].TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.TexelData[TextureIndex].BitSize],
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureWidth),
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureHeight),
                    TMEM,
                    RenderTile
                )
            );


            //Load Texture Settings
            binaryWriter.Write(
                F3D.gsNinSetupTileDescription(
                    F3DEX095_Parameters.TextureFormats[TextureObject.TexelData[TextureIndex].TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.TexelData[TextureIndex].BitSize],
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureWidth),
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureHeight),
                    TMEM,
                    RenderTile,
                    F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].SFlag],
                    widthex,
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].TFlag],
                    heightex,
                    0
                )
            );

            //pipe sync.
            binaryWriter.Write(
                F3D.gsDPPipeSync()
            );
                


            return memoryStream.ToArray();

        }


        public byte[] CI(OK64Texture TextureObject, UInt32 Segment, uint Tile, uint TMEM, bool FogToggle = false)
        {

            byte[] SegmentByte = BitConverter.GetBytes(Segment);
            Array.Reverse(SegmentByte);
            int SegmentID = BitConverter.ToInt32(SegmentByte, 0);


            uint LoadTile = Convert.ToUInt32(6 + Tile);
            uint RenderTile = Convert.ToUInt32(Tile);
            int TextureIndex = Convert.ToInt32(Tile);

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];
            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.TexelData[TextureIndex].textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.TexelData[TextureIndex].textureWidth) / Math.Log(2));


            binaryWriter.Write(F3D.gsDPSetTextureLUT(F3DSharp.F3DEX095_Parameters.G_TT_RGBA16));

            binaryWriter.Write
            (
                F3D.gsSPTexture(65535, 65535, 0, 0, 1)
            );


            if (TextureObject.TexelData[TextureIndex].BitSize < 1)
            {
                //Macro 4-bit Texture Load
                binaryWriter.Write(F3D.gsDPLoadTLUT_pal16(Tile, Convert.ToUInt32(TextureObject.TexelData[TextureIndex].palettePosition | SegmentID)));
                binaryWriter.Write(F3D.gsDPLoadTextureBlock_4b_Tile(Convert.ToUInt32(TextureObject.TexelData[TextureIndex].imagePosition | SegmentID),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TexelData[TextureIndex].TextureFormat], 
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureWidth), 
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureHeight),
                    0, 
                    F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].SFlag], widthex, 0, 
                    F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].TFlag], heightex, 0, 
                    LoadTile, RenderTile, TMEM)
                );
            }
            else
            {
                //hahahaha fuck
                //okay I guess we have to figure this out eventually.
                MessageBox.Show("Ay knock this shit off ->" + TextureObject.TexelData[TextureIndex].texturePath);

                binaryWriter.Write(F3D.gsDPLoadTLUT_pal256(0,
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].palettePosition | SegmentID))
                );

                //Load Texture Settings
                binaryWriter.Write(
                    F3D.gsNinSetupTileDescription(
                        F3DEX095_Parameters.TextureFormats[TextureObject.TexelData[TextureIndex].TextureFormat],
                        F3DEX095_Parameters.BitSizes[TextureObject.TexelData[TextureIndex].BitSize],
                        Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureWidth),
                        Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureHeight),
                        0,
                        0,
                        F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].SFlag],
                        widthex,
                        0,
                        F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].TFlag],
                        heightex,
                        0
                    )
                );
                //Load Texture Data
                binaryWriter.Write(F3D.gsDPLoadTextureBlock(
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].imagePosition | SegmentID),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TexelData[TextureIndex].TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.TexelData[TextureIndex].BitSize],
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureWidth),
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureHeight),
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].SFlag],
                    widthex,
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].TFlag],
                    heightex,
                    0));


            }


            binaryWriter.Write(
                F3D.gsDPTileSync()
            );




            return memoryStream.ToArray();

        }


        public byte[] IA(OK64Texture TextureObject, UInt32 Segment, uint Tile, uint TMEM, bool FogToggle = false)
        {
            byte[] SegmentByte = BitConverter.GetBytes(Segment);
            Array.Reverse(SegmentByte);
            int SegmentID = BitConverter.ToInt32(SegmentByte, 0);


            uint LoadTile = Convert.ToUInt32(6 + Tile);
            uint RenderTile = Convert.ToUInt32(Tile);
            int TextureIndex = Convert.ToInt32(Tile);

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];
            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.TexelData[TextureIndex].textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.TexelData[TextureIndex].textureWidth) / Math.Log(2));


            binaryWriter.Write(F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE));


            binaryWriter.Write
            (
                F3D.gsSPTexture(65535, 65535, 0, 0, 1)
            );


            if (TextureObject.TexelData[TextureIndex].BitSize < 1)
            {
                //Macro 4-bit Texture Load
                binaryWriter.Write(F3D.gsDPLoadTextureBlock_4b_Tile(Convert.ToUInt32(TextureObject.TexelData[TextureIndex].imagePosition | SegmentID),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TexelData[TextureIndex].TextureFormat], 
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureWidth), 
                    Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureHeight),
                    0, F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].SFlag], widthex, 
                    0, F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].TFlag], heightex, 
                    0, LoadTile, RenderTile, TMEM)
                );
            }
            else
            {
                //Load Texture Settings
                binaryWriter.Write(
                    F3D.gsNinSetupTileDescription(
                        F3DEX095_Parameters.TextureFormats[TextureObject.TexelData[TextureIndex].TextureFormat],
                        F3DEX095_Parameters.BitSizes[TextureObject.TexelData[TextureIndex].BitSize],
                        Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureWidth),
                        Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureHeight),
                        TMEM,
                        RenderTile,
                        F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].SFlag],
                        widthex,
                        0,
                        F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].TFlag],
                        heightex,
                        0
                    )
                );
                //Load Texture Data
                binaryWriter.Write(
                    F3D.gsDPLoadTextureBlock_Tile(
                        Convert.ToUInt32(TextureObject.TexelData[TextureIndex].imagePosition | SegmentID),
                        F3DEX095_Parameters.TextureFormats[TextureObject.TexelData[TextureIndex].TextureFormat],
                        F3DEX095_Parameters.BitSizes[TextureObject.TexelData[TextureIndex].BitSize],
                        Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureWidth),
                        Convert.ToUInt32(TextureObject.TexelData[TextureIndex].textureHeight),
                        0,
                        F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].SFlag],
                        widthex,
                        0,
                        F3DEX095_Parameters.TextureModes[TextureObject.TexelData[TextureIndex].TFlag],
                        heightex,
                        0,
                        LoadTile, 
                        RenderTile,
                        TMEM

                    )
                );


            }





            return memoryStream.ToArray();

        }



        public byte[] CompileTextureObjects(byte[] SegmentData, OK64Texture[] textureObject, int vertMagic, int SegmentID = 5, bool FogToggle = false)
        {
            byte[] byteArray = new byte[0];

            bool Transparent = false;

            int relativeZero = vertMagic;


            MemoryStream seg7m = new MemoryStream();
            BinaryReader seg7r = new BinaryReader(seg7m);
            BinaryWriter seg7w = new BinaryWriter(seg7m);


            MemoryStream seg4m = new MemoryStream();
            BinaryReader seg4r = new BinaryReader(seg4m);
            BinaryWriter seg4w = new BinaryWriter(seg4m);

            //prewrite existing Segment 7 data, OR, prefix Segment 7 with a 0xB8 Command. 
            if (SegmentData.Length > 0)
            {
                seg7w.Write(SegmentData);
            }

            for (int materialID = 0; materialID < textureObject.Length; materialID++)
            {
                uint TMEM = 0; 

                if ((textureObject[materialID].TexelData[0].texturePath != null) && (textureObject[materialID].TexelData[0].texturePath != "NULL"))
                {
                    //Textured Polygons (Slow)
                    textureObject[materialID].CCPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;

                    seg7w.Write(ColorCombine(textureObject[materialID], Convert.ToUInt32(SegmentID), true, FogToggle, Transparent));

                    for (int ThisTexel = 0; ThisTexel < textureObject[materialID].TexelData.Count; ThisTexel++)
                    {
                        //Used for individual texture scrolling.
                        textureObject[materialID].TexelData[ThisTexel].F3DPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;

                        switch (textureObject[materialID].TexelData[ThisTexel].TextureFormat)
                        {

                            case 0:
                            default:
                            {
                                seg7w.Write(RGBA(textureObject[materialID], Convert.ToUInt32(SegmentID), Convert.ToUInt32(ThisTexel), TMEM, FogToggle));
                                break;
                            }
                            case 2:
                            {
                                seg7w.Write(CI(textureObject[materialID], Convert.ToUInt32(SegmentID), Convert.ToUInt32(ThisTexel), TMEM, FogToggle));
                                break;
                            }
                            case 3:
                            case 4:
                            {
                                seg7w.Write(IA(textureObject[materialID], Convert.ToUInt32(SegmentID), Convert.ToUInt32(ThisTexel), TMEM, FogToggle));
                                break;
                            }
                            case 1:
                            {
                                MessageBox.Show("ERROR - " + textureObject[materialID].TexelData[ThisTexel].textureName + " - YUV Format not supported.");
                                break;
                            }
                        }



                        int width = textureObject[materialID].TexelData[ThisTexel].textureWidth;
                        int height = textureObject[materialID].TexelData[ThisTexel].textureHeight;
                        // bitsize: 0=4, 1=8, 2=16, 3=32
                        int[] bitDepths = { 4, 8, 16, 32 };
                        int bitsPerTexel = bitDepths[textureObject[materialID].TexelData[ThisTexel].BitSize];

                        // Now you can use bitsPerTexel directly
                        int totalBits = width * height * bitsPerTexel;
                        int totalBytes = (totalBits + 7) / 8;   // round up to nearest byte
                        int tmemWords = (totalBytes + 7) / 8;  // round up to nearest 64‑bit word

                        TMEM += (uint)tmemWords;


                    }


                }
                else
                {
                    // Gouraud or Flat Shading (Fast)
                    textureObject[materialID].CCPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;
                    seg7w.Write(ColorCombine(textureObject[materialID], Convert.ToUInt32(SegmentID), true, FogToggle, Transparent));
                    seg7w.Write(UntexturedPolygons(textureObject[materialID], true, FogToggle));

                }



                seg7w.Write(F3D.gsSPEndDisplayList());


            }
            return seg7m.ToArray();
        }






        public OK64Texture[] loadTextures(Assimp.Scene fbx, string filePath)
        {

            int materialCount = fbx.Materials.Count;
            OK64Texture[] textureArray = new OK64Texture[materialCount];

            for (int materialIndex = 0; materialIndex < materialCount; materialIndex++)
            {


                //Color Combine 
                ///
                ///
                ///
                ///
                ///
                ///

                textureArray[materialIndex] = new TM64_Texture.OK64Texture();
                textureArray[materialIndex].ColorCombine.GeometryBools = new bool[F3DEX095_Parameters.GeometryModes.Length];
                
                textureArray[materialIndex].ColorCombine.GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_ZBUFFER)] = true;
                textureArray[materialIndex].ColorCombine.GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_SHADE)] = true;
                textureArray[materialIndex].ColorCombine.GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_SHADING_SMOOTH)] = true;
                textureArray[materialIndex].ColorCombine.GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_CULL_BACK)] = true;
                textureArray[materialIndex].ColorCombine.GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_CLIPPING)] = true;
                textureArray[materialIndex].ColorCombine.GeometryModes = 0;

                textureArray[materialIndex].ColorCombine.TextureFilter = Array.IndexOf(F3DEX095_Parameters.TextureFilters, F3DEX095_Parameters.G_TF_BILERP);

                textureArray[materialIndex].ColorCombine.CombineModeA = Array.IndexOf(F3DEX095_Parameters.GCCModes, F3DEX095_Parameters.G_CC_MODULATERGBA);
                textureArray[materialIndex].ColorCombine.CombineModeB = Array.IndexOf(F3DEX095_Parameters.GCCModes, F3DEX095_Parameters.G_CC_MODULATERGBA);
                
                textureArray[materialIndex].ColorCombine.RenderModeA = Array.IndexOf(F3DEX095_Parameters.RenderModesSimple, F3DEX095_Parameters.G_RM_AA_ZB_OPA_SURF);
                textureArray[materialIndex].ColorCombine.RenderModeB = Array.IndexOf(F3DEX095_Parameters.RenderModesSimple, F3DEX095_Parameters.G_RM_AA_ZB_OPA_SURF2);

                textureArray[materialIndex].ColorCombine.CycleMode = 0;





                if (fbx.Materials[materialIndex].HasColorAmbient)
                {
                    textureArray[materialIndex].ColorCombine.Environment = System.Drawing.Color.FromArgb(
                        255,
                        Convert.ToInt32(fbx.Materials[materialIndex].ColorAmbient.R * 255.0f),
                        Convert.ToInt32(fbx.Materials[materialIndex].ColorAmbient.G * 255.0f),
                        Convert.ToInt32(fbx.Materials[materialIndex].ColorAmbient.B * 255.0f)
                    );
                    textureArray[materialIndex].ColorCombine.EnvironmentAlpha = Convert.ToInt32(fbx.Materials[materialIndex].ColorAmbient.A * 255.0f);
                }
                else
                {
                    textureArray[materialIndex].ColorCombine.Environment = System.Drawing.Color.White;
                    textureArray[materialIndex].ColorCombine.EnvironmentAlpha = 255;
                }


                if (fbx.Materials[materialIndex].HasColorDiffuse)
                {
                    textureArray[materialIndex].ColorCombine.Primary = System.Drawing.Color.FromArgb(
                        255,
                        Convert.ToInt32(fbx.Materials[materialIndex].ColorDiffuse.R * 255.0f),
                        Convert.ToInt32(fbx.Materials[materialIndex].ColorDiffuse.G * 255.0f),
                        Convert.ToInt32(fbx.Materials[materialIndex].ColorDiffuse.B * 255.0f)
                    );
                    textureArray[materialIndex].ColorCombine.PrimaryAlpha = Convert.ToInt32(fbx.Materials[materialIndex].ColorDiffuse.A * 255.0f);
                }
                else
                {
                    textureArray[materialIndex].ColorCombine.Primary = System.Drawing.Color.White;
                    textureArray[materialIndex].ColorCombine.PrimaryAlpha = 255;
                }




                ///
                ///
                ///
                ///
                ///
                ///
                //Texel Data
                ///
                ///
                ///
                ///
                ///
                ///


                textureArray[materialIndex].TexelData = new List<OK64TexelData>();
                textureArray[materialIndex].TexelData.Add(new OK64TexelData());

                textureArray[materialIndex].TexelData[0].textureName = fbx.Materials[materialIndex].Name;
                textureArray[materialIndex].TexelData[0].textureScrollS = 0;
                textureArray[materialIndex].TexelData[0].textureScrollT = 0;
                textureArray[materialIndex].TexelData[0].textureScreen = 0;
                textureArray[materialIndex].TexelData[0].alphaPath = "";


                string mainDirectory = Path.GetDirectoryName(filePath);
                string workDirectory;
                if ((fbx.Materials[materialIndex].TextureDiffuse.FilePath != null) && (fbx.Materials[materialIndex].TextureDiffuse.FilePath != ""))
                {
                    workDirectory = fbx.Materials[materialIndex].TextureDiffuse.FilePath;
                    workDirectory = Path.Combine(mainDirectory, workDirectory);
                    textureArray[materialIndex].TexelData[0].texturePath = Path.GetFullPath(workDirectory);
                    textureArray[materialIndex].TexelData[0].textureName = Path.GetFileNameWithoutExtension(workDirectory);

                    if ((fbx.Materials[materialIndex].TextureOpacity.FilePath != null) && (fbx.Materials[materialIndex].TextureOpacity.FilePath != ""))
                    {
                        workDirectory = fbx.Materials[materialIndex].TextureOpacity.FilePath;

                        workDirectory = Path.Combine(mainDirectory, workDirectory);
                        textureArray[materialIndex].TexelData[0].alphaPath = Path.GetFullPath(workDirectory);
                    }

                    

                    switch (fbx.Materials[materialIndex].TextureDiffuse.WrapModeU)
                    {
                        case Assimp.TextureWrapMode.Wrap:
                        {
                            textureArray[materialIndex].TexelData[0].SFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_WRAP);
                            break;
                        }
                        case Assimp.TextureWrapMode.Mirror:
                        {
                            textureArray[materialIndex].TexelData[0].SFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_MIRROR);
                            break;
                        }
                        case Assimp.TextureWrapMode.Clamp:
                        {
                            textureArray[materialIndex].TexelData[0].SFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_CLAMP);
                            break;
                        }
                    }
                    switch (fbx.Materials[materialIndex].TextureDiffuse.WrapModeV)
                    {
                        case Assimp.TextureWrapMode.Wrap:
                        {
                            textureArray[materialIndex].TexelData[0].TFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_WRAP);
                            break;
                        }
                        case Assimp.TextureWrapMode.Mirror:
                        {
                            textureArray[materialIndex].TexelData[0].TFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_MIRROR);
                            break;
                        }
                        case Assimp.TextureWrapMode.Clamp:
                        {
                            textureArray[materialIndex].TexelData[0].TFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_CLAMP);
                            break;
                        }
                    }

                    textureArray[materialIndex].TexelData[0].TextureFormat = Array.IndexOf(F3DEX095_Parameters.TextureFormats, F3DEX095_Parameters.G_IM_FMT_RGBA);
                    textureArray[materialIndex].TexelData[0].BitSize = Array.IndexOf(F3DEX095_Parameters.BitSizes, F3DEX095_Parameters.G_IM_SIZ_16b);


                    if (File.Exists(textureArray[materialIndex].TexelData[0].texturePath))
                    {
                        using (var fs = new FileStream(textureArray[materialIndex].TexelData[0].texturePath, FileMode.Open, FileAccess.Read))
                        {
                            textureArray[materialIndex].TexelData[0].textureBitmap = Image.FromStream(fs);
                            fs.Close();
                        }
                        textureArray[materialIndex].TexelData[0].textureHeight = textureArray[materialIndex].TexelData[0].textureBitmap.Height;
                        textureArray[materialIndex].TexelData[0].textureWidth = textureArray[materialIndex].TexelData[0].textureBitmap.Width;

                    }
                    else
                    {

                        while (!(File.Exists(textureArray[materialIndex].TexelData[0].texturePath)))
                        {
                            /*MessageBox.Show(textureArray[materialIndex].texturePath + " not found, browse to file!");
                            OpenFileDialog fileOpen = new OpenFileDialog();
                            if (fileOpen.ShowDialog() == DialogResult.OK)
                            {
                                textureArray[materialIndex].texturePath = fileOpen.FileName;
                                using (var fs = new FileStream(textureArray[materialIndex].texturePath, FileMode.Open, FileAccess.Read))
                                {
                                    textureArray[materialIndex].textureBitmap = Image.FromStream(fs);
                                }


                                textureArray[materialIndex].textureHeight = textureArray[materialIndex].textureBitmap.Height;
                                textureArray[materialIndex].textureWidth = textureArray[materialIndex].textureBitmap.Width;
                            }
                            else
                            {
                                MessageBox.Show("ERROR FILE NOT SELECTED");
                            */
                            textureArray[materialIndex].TexelData[0].textureHeight = 32;
                            textureArray[materialIndex].TexelData[0].textureWidth = 32;
                            break;
                            //}
                        }
                    }


                }
            }

            return textureArray;
        }


        public byte[] WriteRawTextures(byte[] SegmentData, OK64Texture[] textureObject, int DataLength)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            int TextureSize = SegmentData.Length;


            binaryWriter.Write(SegmentData);



            int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;


            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            int textureCount = (textureObject.Length);
            for (int currentTexture = 0; currentTexture < textureCount; currentTexture++)
            {
                for (int ThisTexel = 0; ThisTexel < textureObject[currentTexture].TexelData.Count; ThisTexel++)
                {


                    if ((textureObject[currentTexture].TexelData[ThisTexel].texturePath != null) && (textureObject[currentTexture].TexelData[ThisTexel].texturePath != "NULL"))
                    {
                        // Establish codec and convert texture. Compress converted texture data via MIO0 compression

                        N64Codec[][] n64Codec = new N64Codec[][] {
                            new N64Codec[]{ N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA32 },
                            new N64Codec[]{ N64Codec.ONEBPP, N64Codec.ONEBPP , N64Codec.ONEBPP , N64Codec.ONEBPP },
                            new N64Codec[]{ N64Codec.CI4, N64Codec.CI8, N64Codec.CI8, N64Codec.CI8 },
                            new N64Codec[]{ N64Codec.IA4, N64Codec.IA8, N64Codec.IA16, N64Codec.IA16 },
                            new N64Codec[]{ N64Codec.I4, N64Codec.I8, N64Codec.I8, N64Codec.I8 }
                        };

                        byte[] imageData = null;
                        byte[] paletteData = null;

                        Bitmap bitmapData;
                        try
                        {
                            bitmapData = new Bitmap(textureObject[currentTexture].TexelData[ThisTexel].texturePath);
                        }
                        catch
                        {
                            bitmapData = new Bitmap(Tarmac64_Library.Properties.Resources.TextureNotFound);
                        }



                        if (textureObject[currentTexture].TexelData[ThisTexel].alphaPath != "")
                        {
                            if (File.Exists(textureObject[currentTexture].TexelData[ThisTexel].alphaPath))
                            {
                                Bitmap MaskedTexture = new Bitmap(bitmapData.Width, bitmapData.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                Bitmap alphaData = new Bitmap(textureObject[currentTexture].TexelData[ThisTexel].alphaPath);

                                for (int ThisY = 0; ThisY < bitmapData.Height; ThisY++)
                                {
                                    for (int ThisX = 0; ThisX < bitmapData.Width; ThisX++)
                                    {

                                        System.Drawing.Color AlphaData = alphaData.GetPixel(ThisX, ThisY);
                                        System.Drawing.Color ColorData = bitmapData.GetPixel(ThisX, ThisY);
                                        System.Drawing.Color NewColor = System.Drawing.Color.FromArgb(AlphaData.R, ColorData.R, ColorData.G, ColorData.B);
                                        MaskedTexture.SetPixel(ThisX, ThisY, NewColor);
                                        System.Drawing.Color CheckColor = MaskedTexture.GetPixel(ThisX, ThisY);
                                    }
                                }
                                N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TexelData[ThisTexel].TextureFormat][textureObject[currentTexture].TexelData[ThisTexel].BitSize], MaskedTexture);
                            }
                            else
                            {
                                N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TexelData[ThisTexel].TextureFormat][textureObject[currentTexture].TexelData[ThisTexel].BitSize], bitmapData);
                            }
                        }
                        else
                        {
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TexelData[ThisTexel].TextureFormat][textureObject[currentTexture].TexelData[ThisTexel].BitSize], bitmapData);
                        }



                        // finish setting texture parameters based on new texture and compressed data.

                        textureObject[currentTexture].TexelData[ThisTexel].compressedSize = imageData.Length;
                        textureObject[currentTexture].TexelData[ThisTexel].fileSize = imageData.Length;
                        textureObject[currentTexture].TexelData[ThisTexel].imagePosition = Convert.ToInt32(binaryWriter.BaseStream.Position + DataLength);  // we need this to build out F3DEX commands later. 
                        TextureSize = TextureSize + textureObject[currentTexture].TexelData[ThisTexel].fileSize;


                        //adjust the MIO0 offset to an 8-byte address as required for N64.
                        binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;

                        addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                        if (addressAlign == 16)
                            addressAlign = 0;


                        for (int align = 0; align < addressAlign; align++)
                        {
                            binaryWriter.Write(Convert.ToByte(0x00));
                        }



                        // write compressed MIO0 texture to end of ROM.
                        binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                        binaryWriter.Write(imageData);

                        int SegPosition = Convert.ToInt32(binaryWriter.BaseStream.Length) + DataLength;
                        if (paletteData != null)
                        {
                            textureObject[currentTexture].TexelData[ThisTexel].PaletteData = paletteData;
                            textureObject[currentTexture].TexelData[ThisTexel].paletteSize = paletteData.Length;
                            textureObject[currentTexture].TexelData[ThisTexel].palettePosition = SegPosition;
                            SegPosition += textureObject[currentTexture].TexelData[ThisTexel].paletteSize;
                            addressAlign = 0x1000 - (SegPosition % 0x1000);
                            if (addressAlign == 0x1000)
                                addressAlign = 0;
                            SegPosition += addressAlign;

                            binaryWriter.Write(textureObject[currentTexture].TexelData[ThisTexel].PaletteData);
                            addressAlign = 0x1000 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 0x1000);
                            if (addressAlign == 0x1000)
                                addressAlign = 0;
                            for (int align = 0; align < addressAlign; align++)
                            {
                                binaryWriter.Write(Convert.ToByte(0x00));
                            }
                        }


                    }
                }
            }

            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));


            byte[] romOut = memoryStream.ToArray();

            return romOut;


        }

        public byte[] WriteModelTextures(byte[] SegmentData, OK64Texture[] textureObject, int DataLength)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            int TextureSize = SegmentData.Length;


            binaryWriter.Write(SegmentData);
            int textureCount = (textureObject.Length);
            for (int currentTexture = 0; currentTexture < textureCount; currentTexture++)
            {
                for (int ThisTexel = 0; ThisTexel < textureObject[currentTexture].TexelData.Count; ThisTexel++)
                {


                    if ((textureObject[currentTexture].TexelData[ThisTexel].texturePath != null) && (textureObject[currentTexture].TexelData[ThisTexel].texturePath != "NULL"))
                    {
                        // Establish codec and convert texture. Compress converted texture data via MIO0 compression

                        N64Codec[][] n64Codec = new N64Codec[][] {
                        new N64Codec[]{ N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA32 },
                        new N64Codec[]{ N64Codec.ONEBPP, N64Codec.ONEBPP , N64Codec.ONEBPP , N64Codec.ONEBPP },
                        new N64Codec[]{ N64Codec.CI4, N64Codec.CI8, N64Codec.CI8, N64Codec.CI8 },
                        new N64Codec[]{ N64Codec.IA4, N64Codec.IA8, N64Codec.IA16, N64Codec.IA16 },
                        new N64Codec[]{ N64Codec.I4, N64Codec.I8, N64Codec.I8, N64Codec.I8 }
                    };
                        byte[] imageData = null;
                        byte[] paletteData = null;
                        Bitmap bitmapData = new Bitmap(textureObject[currentTexture].TexelData[ThisTexel].texturePath);
                        if (textureObject[currentTexture].TexelData[ThisTexel].alphaPath != "")
                        {
                            if (File.Exists(textureObject[currentTexture].TexelData[ThisTexel].alphaPath))
                            {
                                Bitmap MaskedTexture = new Bitmap(bitmapData.Width, bitmapData.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                Bitmap alphaData = new Bitmap(textureObject[currentTexture].TexelData[ThisTexel].alphaPath);

                                for (int ThisY = 0; ThisY < bitmapData.Height; ThisY++)
                                {
                                    for (int ThisX = 0; ThisX < bitmapData.Width; ThisX++)
                                    {
                                        System.Drawing.Color AlphaData = alphaData.GetPixel(ThisX, ThisY);
                                        System.Drawing.Color ColorData = bitmapData.GetPixel(ThisX, ThisY);
                                        System.Drawing.Color NewColor = System.Drawing.Color.FromArgb(AlphaData.R, ColorData.R, ColorData.G, ColorData.B);
                                        MaskedTexture.SetPixel(ThisX, ThisY, NewColor);
                                        System.Drawing.Color CheckColor = MaskedTexture.GetPixel(ThisX, ThisY);
                                    }
                                }
                                N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TexelData[ThisTexel].TextureFormat][textureObject[currentTexture].TexelData[ThisTexel].BitSize], MaskedTexture);
                            }
                            else
                            {
                                N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TexelData[ThisTexel].TextureFormat][textureObject[currentTexture].TexelData[ThisTexel].BitSize], bitmapData);
                            }
                        }
                        else
                        {
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TexelData[ThisTexel].TextureFormat][textureObject[currentTexture].TexelData[ThisTexel].BitSize], bitmapData);
                        }



                        //adjust the MIO0 offset to an 8-byte address as required for N64.
                        binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                        int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                        if (addressAlign == 16)
                            addressAlign = 0;


                        for (int align = 0; align < addressAlign; align++)
                        {
                            binaryWriter.Write(Convert.ToByte(0x00));
                        }



                        // finish setting texture parameters based on new texture and compressed data.

                        textureObject[currentTexture].TexelData[ThisTexel].compressedSize = imageData.Length;
                        textureObject[currentTexture].TexelData[ThisTexel].fileSize = imageData.Length;
                        textureObject[currentTexture].TexelData[ThisTexel].imagePosition = Convert.ToInt32(binaryWriter.BaseStream.Position + DataLength);


                        // write compressed MIO0 texture to end of ROM.


                        binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                        textureObject[currentTexture].TexelData[ThisTexel].TextureData = imageData;
                        binaryWriter.Write(imageData);

                        int SegPosition = Convert.ToInt32(binaryWriter.BaseStream.Length) + DataLength;
                        if (paletteData != null)
                        {
                            textureObject[currentTexture].TexelData[ThisTexel].PaletteData = paletteData;
                            textureObject[currentTexture].TexelData[ThisTexel].paletteSize = paletteData.Length;
                            textureObject[currentTexture].TexelData[ThisTexel].imagePosition = SegPosition;
                            SegPosition += textureObject[currentTexture].TexelData[ThisTexel].paletteSize;
                            addressAlign = 0x1000 - (SegPosition % 0x1000);
                            if (addressAlign == 0x1000)
                                addressAlign = 0;
                            SegPosition += addressAlign;

                            binaryWriter.Write(textureObject[currentTexture].TexelData[ThisTexel].PaletteData);
                            addressAlign = 0x1000 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 0x1000);
                            if (addressAlign == 0x1000)
                                addressAlign = 0;
                            for (int align = 0; align < addressAlign; align++)
                            {
                                binaryWriter.Write(Convert.ToByte(0x00));
                            }
                        }

                    }
                }
            }

            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));


            byte[] romOut = memoryStream.ToArray();

            return romOut;


        }

        public void BuildTextures(OK64Texture[] TextureArray)
        {
            int segment5Position = 0;
            TM64 Tarmac = new TM64();

            for (int currentTexture = 0; currentTexture < TextureArray.Length; currentTexture++)
            {
                for (int ThisTexel = 0; ThisTexel < TextureArray[currentTexture].TexelData.Count; ThisTexel++)
                {


                    if ((TextureArray[currentTexture].TexelData[ThisTexel].texturePath != null) && (TextureArray[currentTexture].TexelData[ThisTexel].texturePath != "NULL"))
                    {
                        // Establish codec and convert texture. Compress converted texture data via MIO0 compression


                        N64Codec[][] n64Codec = new N64Codec[][] {
                        new N64Codec[]{ N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA16, N64Codec.RGBA32 },
                        new N64Codec[]{ N64Codec.ONEBPP, N64Codec.ONEBPP , N64Codec.ONEBPP , N64Codec.ONEBPP },
                        new N64Codec[]{ N64Codec.CI4, N64Codec.CI8, N64Codec.CI8, N64Codec.CI8 },
                        new N64Codec[]{ N64Codec.IA4, N64Codec.IA8, N64Codec.IA16, N64Codec.IA16 },
                        new N64Codec[]{ N64Codec.I4, N64Codec.I8, N64Codec.I8, N64Codec.I8 }
                        };
                        byte[] imageData = null;
                        byte[] paletteData = null;
                        Bitmap TextureData;
                        try
                        {
                            TextureData = new Bitmap(TextureArray[currentTexture].TexelData[ThisTexel].texturePath);
                        }
                        catch
                        {
                            TextureData = new Bitmap(Tarmac64_Library.Properties.Resources.TextureNotFound);
                        }


                        if (TextureArray[currentTexture].TexelData[ThisTexel].alphaPath != "")
                        {
                            if (File.Exists(TextureArray[currentTexture].TexelData[ThisTexel].alphaPath))
                            {
                                Bitmap MaskedTexture = new Bitmap(TextureData.Width, TextureData.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                Bitmap alphaData = new Bitmap(TextureArray[currentTexture].TexelData[ThisTexel].alphaPath);

                                for (int ThisY = 0; ThisY < TextureData.Height; ThisY++)
                                {
                                    for (int ThisX = 0; ThisX < TextureData.Width; ThisX++)
                                    {

                                        System.Drawing.Color AlphaData = alphaData.GetPixel(ThisX, ThisY);
                                        System.Drawing.Color ColorData = TextureData.GetPixel(ThisX, ThisY);
                                        System.Drawing.Color NewColor = System.Drawing.Color.FromArgb(AlphaData.R, ColorData.R, ColorData.G, ColorData.B);
                                        MaskedTexture.SetPixel(ThisX, ThisY, NewColor);
                                        System.Drawing.Color CheckColor = MaskedTexture.GetPixel(ThisX, ThisY);
                                    }
                                }
                                N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[TextureArray[currentTexture].TexelData[ThisTexel].TextureFormat][TextureArray[currentTexture].TexelData[ThisTexel].BitSize], MaskedTexture);
                            }
                            else
                            {
                                N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[TextureArray[currentTexture].TexelData[ThisTexel].TextureFormat][TextureArray[currentTexture].TexelData[ThisTexel].BitSize], TextureData);
                            }
                        }
                        else
                        {
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[TextureArray[currentTexture].TexelData[ThisTexel].TextureFormat][TextureArray[currentTexture].TexelData[ThisTexel].BitSize], TextureData);
                        }

                        TextureArray[currentTexture].TexelData[ThisTexel].compressedTexture = Tarmac.CompressMIO0(imageData);
                        TextureArray[currentTexture].TexelData[ThisTexel].TextureData = imageData;
                        TextureArray[currentTexture].TexelData[ThisTexel].PaletteData = paletteData;

                        // finish setting texture parameters based on new texture and compressed data.

                        TextureArray[currentTexture].TexelData[ThisTexel].compressedSize = TextureArray[currentTexture].TexelData[ThisTexel].compressedTexture.Length;
                        TextureArray[currentTexture].TexelData[ThisTexel].fileSize = imageData.Length;
                        TextureArray[currentTexture].TexelData[ThisTexel].imagePosition = segment5Position;  // we need this to build out F3DEX commands later.                     
                        segment5Position += TextureArray[currentTexture].TexelData[ThisTexel].fileSize;
                        if (paletteData != null)
                        {
                            TextureArray[currentTexture].TexelData[ThisTexel].paletteSize = paletteData.Length;
                            TextureArray[currentTexture].TexelData[ThisTexel].palettePosition = segment5Position;
                            segment5Position += TextureArray[currentTexture].TexelData[ThisTexel].paletteSize;
                            int addressAlign = 0x1000 - (segment5Position % 0x1000);
                            if (addressAlign == 0x1000)
                                addressAlign = 0;
                            segment5Position += addressAlign;
                        }
                    }
                }
            }
        }

        public byte[] WriteTextures(byte[] FileData, TM64_Course.Course Course)
        {
            OK64Texture[] TextureArray = Course.ModelData.TextureObjects;
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            TM64 Tarmac = new TM64();




            for (int CurrentTexture = 0; CurrentTexture < TextureArray.Length; CurrentTexture++)
            {
                for (int ThisTexel = 0; ThisTexel < TextureArray[CurrentTexture].TexelData.Count; ThisTexel++)
                {


                    if ((TextureArray[CurrentTexture].TexelData[ThisTexel].texturePath != null) && (TextureArray[CurrentTexture].TexelData[ThisTexel].texturePath != "NULL"))
                    {

                        // write compressed MIO0 texture to end of ROM.


                        binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                        binaryWriter.Write(TextureArray[CurrentTexture].TexelData[ThisTexel].TextureData);
                        if (TextureArray[CurrentTexture].TexelData[ThisTexel].PaletteData != null)
                        {
                            binaryWriter.Write(TextureArray[CurrentTexture].TexelData[ThisTexel].PaletteData);
                            int addressAlign = 0x1000 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 0x1000);
                            if (addressAlign == 0x1000)
                                addressAlign = 0;
                            for (int align = 0; align < addressAlign; align++)
                            {
                                binaryWriter.Write(Convert.ToByte(0x00));
                            }
                        }
                    }
                }
            }
            byte[] UncompressedData = memoryStream.ToArray();
            byte[] CompressedData = Tarmac.CompressMIO0(UncompressedData);


            memoryStream = new MemoryStream();
            binaryReader = new BinaryReader(memoryStream);
            binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(FileData);
            Course.Segment5ROM = Convert.ToInt32(binaryWriter.BaseStream.Position);
            Course.Segment5Length = UncompressedData.Length;
            Course.Segment5CompressedLength = CompressedData.Length;
            binaryWriter.Write(CompressedData);


            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            binaryWriter.Write(Convert.ToInt32(0));
            byte[] FileOut = memoryStream.ToArray();
            return FileOut;
        }

        public byte[] CompileTextureTable(TM64_Course.Course CourseData)
        {
            OK64Texture[] textureObject = CourseData.ModelData.TextureObjects;
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            byte[] byteArray = new byte[0];



            MemoryStream seg9m = new MemoryStream();
            BinaryReader seg9r = new BinaryReader(seg9m);
            BinaryWriter seg9w = new BinaryWriter(seg9m);



            //Compressing all texture data together increases efficiency
            if (CourseData.Segment5Length > 0)
            {
                //We have texture data


                byteArray = BitConverter.GetBytes(Convert.ToInt32(CourseData.Segment5ROM - 0x641F70));
                Array.Reverse(byteArray);
                seg9w.Write(byteArray);

                byteArray = BitConverter.GetBytes(Convert.ToInt32(CourseData.Segment5CompressedLength));
                Array.Reverse(byteArray);
                seg9w.Write(byteArray);

                byteArray = BitConverter.GetBytes(Convert.ToInt32(CourseData.Segment5Length));
                Array.Reverse(byteArray);
                seg9w.Write(byteArray);

                byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
                Array.Reverse(byteArray);
                seg9w.Write(byteArray);
            }



            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);
            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);
            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);
            byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(byteArray);
            seg9w.Write(byteArray);


            byte[] seg9Out = seg9m.ToArray();
            return seg9Out;
        }



    }
}
