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
using Texture64;
using static Tarmac64_Library.TM64_Geometry;

namespace Tarmac64_Library
{
    public class TM64_Texture
    {
        F3DEX095 F3D = new F3DEX095();


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
                F3D.gsDPSetCycleType(Convert.ToUInt32(TextureObject.ColorCombine.CycleMode))
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


            //SetCombine
            if (FogToggle)
            {
                if (Transparent)
                {
                    binaryWriter.Write(
                    F3D.gsDPSetCombineMode(
                        F3DEX095_Parameters.G_CC_DECALRGBA,
                        F3DEX095_Parameters.G_CC_PASS2
                        )
                    );
                }
                else
                {
                    binaryWriter.Write(
                    F3D.gsDPSetCombineMode(
                        F3DEX095_Parameters.GCCModes[TextureObject.ColorCombine.CombineModeA],
                        F3DEX095_Parameters.G_CC_PASS2
                        )
                    );
                }
            }
            else
            {
                binaryWriter.Write(
                    F3D.gsDPSetCombineMode(
                        F3DEX095_Parameters.GCCModes[TextureObject.ColorCombine.CombineModeA],
                        F3DEX095_Parameters.GCCModes[TextureObject.ColorCombine.CombineModeB]
                    )
                );
            }
            //
            //




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

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];
            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));

            uint LoadTile = 6 + Tile;
            uint RenderTile = Tile;


            binaryWriter.Write(
                F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE)
            );

            binaryWriter.Write
            (
                F3D.gsSPTexture(65535, 65535, 0, 0, 1)
            );

            //Load Texture Settings
            binaryWriter.Write(
                F3D.gsNinSetupTileDescription(
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                    Convert.ToUInt32(TextureObject.textureWidth),
                    Convert.ToUInt32(TextureObject.textureHeight),
                    TMEM,
                    RenderTile,
                    F3DEX095_Parameters.TextureModes[TextureObject.SFlag],
                    widthex,
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.TFlag],
                    heightex,
                    0
                )
            );


            //Load Texture Data
            binaryWriter.Write(
                F3D.gsNinLoadTextureImage(
                    Convert.ToUInt32(TextureObject.TexelData.segmentPosition | Convert.ToUInt32(Segment << 24)),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                    Convert.ToUInt32(TextureObject.textureWidth),
                    Convert.ToUInt32(TextureObject.textureHeight),
                    TMEM,
                    LoadTile
                )
            );

            //pipe sync.
            binaryWriter.Write(
                F3D.gsDPPipeSync()
            );

            binaryWriter.Write(F3D.gsSPEndDisplayList());                                             //End the Display List





            return memoryStream.ToArray();

        }


        public byte[] CI(OK64Texture TextureObject, UInt32 Segment, uint Tile, uint TMEM, bool FogToggle = false)
        {

            byte[] SegmentByte = BitConverter.GetBytes(Segment);
            Array.Reverse(SegmentByte);
            int SegmentID = BitConverter.ToInt32(SegmentByte, 0);

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];
            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));

            uint LoadTile = 6 + Tile;
            uint RenderTile = Tile;

            binaryWriter.Write(F3D.gsDPSetTextureLUT(F3DSharp.F3DEX095_Parameters.G_TT_RGBA16));

            binaryWriter.Write
            (
                F3D.gsSPTexture(65535, 65535, 0, 0, 1)
            );


            if (TextureObject.BitSize < 1)
            {
                //Macro 4-bit Texture Load
                binaryWriter.Write(F3D.gsDPLoadTLUT_pal16(Tile, Convert.ToUInt32(TextureObject.TexelData.palettePosition | SegmentID)));
                binaryWriter.Write(F3D.gsDPLoadTextureBlock_4b_Tile(Convert.ToUInt32(TextureObject.TexelData.segmentPosition | SegmentID),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat], Convert.ToUInt32(TextureObject.textureWidth), Convert.ToUInt32(TextureObject.textureHeight),
                    0, F3DEX095_Parameters.TextureModes[TextureObject.SFlag], widthex, 0, F3DEX095_Parameters.TextureModes[TextureObject.TFlag], heightex, 0, LoadTile, RenderTile, TMEM));
            }
            else
            {
                //hahahaha fuck
                //okay I guess we have to figure this out eventually.
                MessageBox.Show("Ay knock this shit off ->" + TextureObject.texturePath);

                binaryWriter.Write(F3D.gsDPLoadTLUT_pal256(0, Convert.ToUInt32(TextureObject.TexelData.palettePosition | SegmentID)));

                //Load Texture Settings
                binaryWriter.Write(
                    F3D.gsNinSetupTileDescription(
                        F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                        F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                        Convert.ToUInt32(TextureObject.textureWidth),
                        Convert.ToUInt32(TextureObject.textureHeight),
                        0,
                        0,
                        F3DEX095_Parameters.TextureModes[TextureObject.SFlag],
                        widthex,
                        0,
                        F3DEX095_Parameters.TextureModes[TextureObject.TFlag],
                        heightex,
                        0
                    )
                );
                //Load Texture Data
                binaryWriter.Write(F3D.gsDPLoadTextureBlock(
                    Convert.ToUInt32(TextureObject.TexelData.segmentPosition | SegmentID),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                    F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                    Convert.ToUInt32(TextureObject.textureWidth),
                    Convert.ToUInt32(TextureObject.textureHeight),
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.SFlag],
                    widthex,
                    0,
                    F3DEX095_Parameters.TextureModes[TextureObject.TFlag],
                    heightex,
                    0));


            }


            binaryWriter.Write(
                F3D.gsDPTileSync()
                );


            binaryWriter.Write(F3D.gsSPEndDisplayList());                                             //End the Display List





            return memoryStream.ToArray();

        }


        public byte[] IA(OK64Texture TextureObject, UInt32 Segment, uint Tile, uint TMEM, bool FogToggle = false)
        {
            byte[] SegmentByte = BitConverter.GetBytes(Segment);
            Array.Reverse(SegmentByte);
            int SegmentID = BitConverter.ToInt32(SegmentByte, 0);

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] byteArray = new byte[2];
            UInt32 heightex = Convert.ToUInt32(Math.Log(TextureObject.textureHeight) / Math.Log(2));
            UInt32 widthex = Convert.ToUInt32(Math.Log(TextureObject.textureWidth) / Math.Log(2));

            uint LoadTile = 6 + Tile;
            uint RenderTile = Tile;


            binaryWriter.Write(F3D.gsDPSetTextureLUT(F3DEX095_Parameters.G_TT_NONE));


            binaryWriter.Write
            (
                F3D.gsSPTexture(65535, 65535, 0, 0, 1)
            );


            if (TextureObject.BitSize < 1)
            {
                //Macro 4-bit Texture Load
                binaryWriter.Write(F3D.gsDPLoadTextureBlock_4b_Tile(Convert.ToUInt32(TextureObject.TexelData.segmentPosition | SegmentID),
                    F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat], Convert.ToUInt32(TextureObject.textureWidth), Convert.ToUInt32(TextureObject.textureHeight),
                    0, F3DEX095_Parameters.TextureModes[TextureObject.SFlag], widthex, 0, F3DEX095_Parameters.TextureModes[TextureObject.TFlag], heightex, 0, LoadTile, RenderTile, TMEM));
            }
            else
            {
                //Load Texture Settings
                binaryWriter.Write(
                    F3D.gsNinSetupTileDescription(
                        F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                        F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                        Convert.ToUInt32(TextureObject.textureWidth),
                        Convert.ToUInt32(TextureObject.textureHeight),
                        TMEM,
                        RenderTile,
                        F3DEX095_Parameters.TextureModes[TextureObject.SFlag],
                        widthex,
                        0,
                        F3DEX095_Parameters.TextureModes[TextureObject.TFlag],
                        heightex,
                        0
                    )
                );
                //Load Texture Data
                binaryWriter.Write(
                    F3D.gsDPLoadTextureBlock_Tile(
                        Convert.ToUInt32(TextureObject.TexelData.segmentPosition | SegmentID),
                        F3DEX095_Parameters.TextureFormats[TextureObject.TextureFormat],
                        F3DEX095_Parameters.BitSizes[TextureObject.BitSize],
                        Convert.ToUInt32(TextureObject.textureWidth),
                        Convert.ToUInt32(TextureObject.textureHeight),
                        0,
                        F3DEX095_Parameters.TextureModes[TextureObject.SFlag],
                        widthex,
                        0,
                        F3DEX095_Parameters.TextureModes[TextureObject.TFlag],
                        heightex,
                        0,
                        LoadTile, 
                        RenderTile,
                        TMEM

                    )
                );


            }




            binaryWriter.Write(F3D.gsSPEndDisplayList());                                             //End the Display List





            return memoryStream.ToArray();

        }



        public byte[] compileCourseTexture(byte[] SegmentData, OK64Texture[] textureObject, int vertMagic, int SegmentID = 5, bool FogToggle = false)
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


            /*
                #define G_IM_FMT_RGBA	0
                #define G_IM_FMT_YUV	1
                #define G_IM_FMT_CI	    2
                #define G_IM_FMT_IA	    3
                #define G_IM_FMT_I	    4
            */
            for (int materialID = 0; materialID < textureObject.Length; materialID++)
            {
                uint TMEM = 0;
                uint Tile = 0; 
                if ((textureObject[materialID].texturePath != null) && (textureObject[materialID].texturePath != "NULL"))
                {
                    //Textured Polygons (Slow)
                    textureObject[materialID].CCPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;

                    seg7w.Write(ColorCombine(textureObject[materialID], Convert.ToUInt32(SegmentID), true, FogToggle, Transparent));


                    switch (textureObject[materialID].TextureFormat)
                    {

                        case 0:
                        default:
                        {
                            seg7w.Write(RGBA(textureObject[materialID], Convert.ToUInt32(SegmentID), Tile, TMEM, FogToggle));
                            break;
                        }
                        case 2:
                        {
                            seg7w.Write(CI(textureObject[materialID], Convert.ToUInt32(SegmentID), Tile, TMEM, FogToggle));
                            break;
                        }
                        case 3:
                        case 4:
                        {
                            seg7w.Write(IA(textureObject[materialID], Convert.ToUInt32(SegmentID), Tile, TMEM, FogToggle));
                            break;
                        }
                        case 1:
                        {
                            MessageBox.Show("ERROR - " + textureObject[materialID].TexelData.textureName + " - YUV Format not supported.");
                            break;
                        }
                    }


                }
                else
                {
                    // Gouraud or Flat Shading (Fast)
                    textureObject[materialID].CCPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;
                    seg7w.Write(ColorCombine(textureObject[materialID], Convert.ToUInt32(SegmentID), true, FogToggle, Transparent));
                    seg7w.Write(UntexturedPolygons(textureObject[materialID], true, FogToggle));

                }
            }
            return seg7m.ToArray();
        }



        public byte[] CompileTextureObjects(byte[] SegmentData, OK64Texture[] textureObject, int vertMagic, int SegmentID = 5, bool GeometryMode = true, bool FogToggle = false)
        {
            byte[] byteArray = new byte[0];

            bool Transparent = false;


            UInt32 ImgSize = 0, ImgType = 0, ImgFlag1 = 0, ImgFlag2 = 0, ImgFlag3 = 0;
            UInt32[] ImgTypes = { 0, 0, 0, 3, 3, 3, 0 }; ///0=RGBA, 3=IA
            UInt32[] STheight = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; ///looks like
            UInt32[] STwidth = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 }; ///texture sizes...
            byte[] heightex = { 5, 5, 6, 5, 5, 6, 5 };
            byte[] widthex = { 5, 6, 5, 5, 6, 5, 5 };

            byte[] SegmentByte = BitConverter.GetBytes(SegmentID);
            Array.Reverse(SegmentByte);

            int relativeZero = vertMagic;
            int relativeIndex = 0;


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


            int addressAlign = 16 - (Convert.ToInt32(seg7w.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;


            for (int align = 0; align < addressAlign; align++)
            {
                seg7w.Write(Convert.ToByte(0x00));
            }


            for (int materialID = 0; materialID < textureObject.Length; materialID++)
            {
                uint TMEM = 0;
                uint Tile = 0;
                if ((textureObject[materialID].texturePath != null) && (textureObject[materialID].texturePath != "NULL"))
                {
                    //Textured Polygons (Slow)
                    textureObject[materialID].CCPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;

                    seg7w.Write(ColorCombine(textureObject[materialID], Convert.ToUInt32(SegmentID), GeometryMode, FogToggle, Transparent));


                    switch (textureObject[materialID].TextureFormat)
                    {

                        case 0:
                        default:
                        {
                            seg7w.Write(RGBA(textureObject[materialID], Tile, TMEM, Convert.ToUInt32(SegmentID), FogToggle));
                            break;
                        }
                        case 2:
                        {
                            seg7w.Write(CI(textureObject[materialID], Tile, TMEM, Convert.ToUInt32(SegmentID), FogToggle));
                            break;
                        }
                        case 3:
                        case 4:
                        {
                            seg7w.Write(IA(textureObject[materialID], Tile, TMEM, Convert.ToUInt32(SegmentID), FogToggle));
                            break;
                        }
                        case 1:
                        {
                            MessageBox.Show("ERROR - " + textureObject[materialID].TexelData.textureName + " - YUV Format not supported.");
                            break;
                        }
                    }


                }
                else
                {
                    // Gouraud or Flat Shading (Fast)
                    textureObject[materialID].CCPosition = Convert.ToInt32(seg7w.BaseStream.Position) + vertMagic;
                    seg7w.Write(UntexturedPolygons(textureObject[materialID], GeometryMode, FogToggle));

                }
            }
            return seg7m.ToArray();
        }






        public OK64Texture[] loadTextures(Assimp.Scene fbx, string filePath)
        {

            int materialCount = fbx.Materials.Count;
            OK64Texture[] textureArray = new OK64Texture[materialCount];

            for (int materialIndex = 0; materialIndex < materialCount; materialIndex++)
            {
                textureArray[materialIndex] = new TM64_Geometry.OK64Texture();
                textureArray[materialIndex].ColorCombine.GeometryBools = new bool[F3DEX095_Parameters.GeometryModes.Length];
                textureArray[materialIndex].TexelData.textureName = fbx.Materials[materialIndex].Name;
                textureArray[materialIndex].ColorCombine.GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_ZBUFFER)] = true;
                textureArray[materialIndex].ColorCombine.GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_SHADE)] = true;
                textureArray[materialIndex].ColorCombine.GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_SHADING_SMOOTH)] = true;
                textureArray[materialIndex].ColorCombine.GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_CULL_BACK)] = true;
                textureArray[materialIndex].ColorCombine.GeometryBools[Array.IndexOf(F3DEX095_Parameters.GeometryModes, F3DEX095_Parameters.G_CLIPPING)] = true;
                textureArray[materialIndex].ColorCombine.GeometryModes = 0;
                textureArray[materialIndex].ColorCombine.TextureFilter = Array.IndexOf(F3DEX095_Parameters.TextureFilters, F3DEX095_Parameters.G_TF_BILERP);
                textureArray[materialIndex].ColorCombine.CombineModeA = Array.IndexOf(F3DEX095_Parameters.GCCModes, F3DEX095_Parameters.G_CC_MODULATERGBA);
                textureArray[materialIndex].ColorCombine.CombineModeB = Array.IndexOf(F3DEX095_Parameters.GCCModes, F3DEX095_Parameters.G_CC_MODULATERGBA);

                textureArray[materialIndex].textureScrollS = 0;
                textureArray[materialIndex].textureScrollT = 0;
                textureArray[materialIndex].textureScreen = 0;

                textureArray[materialIndex].ColorCombine.RenderModeA = Array.IndexOf(F3DEX095_Parameters.RenderModesSimple, F3DEX095_Parameters.G_RM_AA_ZB_OPA_SURF);
                textureArray[materialIndex].ColorCombine.RenderModeB = Array.IndexOf(F3DEX095_Parameters.RenderModesSimple, F3DEX095_Parameters.G_RM_AA_ZB_OPA_SURF2);
                textureArray[materialIndex].alphaPath = "";
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



                if ((fbx.Materials[materialIndex].TextureDiffuse.FilePath != null) && (fbx.Materials[materialIndex].TextureDiffuse.FilePath != ""))
                {
                    string mainDirectory = Path.GetDirectoryName(filePath);


                    textureArray[materialIndex].texturePath = fbx.Materials[materialIndex].TextureDiffuse.FilePath;

                    textureArray[materialIndex].texturePath = Path.Combine(mainDirectory, textureArray[materialIndex].texturePath);
                    textureArray[materialIndex].texturePath = Path.GetFullPath(textureArray[materialIndex].texturePath);


                    if ((fbx.Materials[materialIndex].TextureOpacity.FilePath != null) && (fbx.Materials[materialIndex].TextureOpacity.FilePath != ""))
                    {
                        textureArray[materialIndex].alphaPath = fbx.Materials[materialIndex].TextureOpacity.FilePath;

                        textureArray[materialIndex].alphaPath = Path.Combine(mainDirectory, textureArray[materialIndex].alphaPath);
                        textureArray[materialIndex].alphaPath = Path.GetFullPath(textureArray[materialIndex].alphaPath);


                    }

                    textureArray[materialIndex].TexelData.textureName = Path.GetFileNameWithoutExtension(textureArray[materialIndex].texturePath);

                    switch (fbx.Materials[materialIndex].TextureDiffuse.WrapModeU)
                    {
                        case Assimp.TextureWrapMode.Wrap:
                        {
                            textureArray[materialIndex].SFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_WRAP);
                            break;
                        }
                        case Assimp.TextureWrapMode.Mirror:
                        {
                            textureArray[materialIndex].SFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_MIRROR);
                            break;
                        }
                        case Assimp.TextureWrapMode.Clamp:
                        {
                            textureArray[materialIndex].SFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_CLAMP);
                            break;
                        }
                    }
                    switch (fbx.Materials[materialIndex].TextureDiffuse.WrapModeV)
                    {
                        case Assimp.TextureWrapMode.Wrap:
                        {
                            textureArray[materialIndex].TFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_WRAP);
                            break;
                        }
                        case Assimp.TextureWrapMode.Mirror:
                        {
                            textureArray[materialIndex].TFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_MIRROR);
                            break;
                        }
                        case Assimp.TextureWrapMode.Clamp:
                        {
                            textureArray[materialIndex].TFlag = Array.IndexOf(F3DEX095_Parameters.TextureModes, F3DEX095_Parameters.G_TX_CLAMP);
                            break;
                        }
                    }
                    //fbx.Materials[materialIndex].TextureDiffuse.WrapModeU

                    textureArray[materialIndex].TextureFormat = Array.IndexOf(F3DEX095_Parameters.TextureFormats, F3DEX095_Parameters.G_IM_FMT_RGBA);
                    textureArray[materialIndex].BitSize = Array.IndexOf(F3DEX095_Parameters.BitSizes, F3DEX095_Parameters.G_IM_SIZ_16b);


                    if (File.Exists(textureArray[materialIndex].texturePath))
                    {
                        using (var fs = new FileStream(textureArray[materialIndex].texturePath, FileMode.Open, FileAccess.Read))
                        {
                            textureArray[materialIndex].TexelData.textureBitmap = Image.FromStream(fs);
                            fs.Close();
                        }
                        textureArray[materialIndex].textureHeight = textureArray[materialIndex].TexelData.textureBitmap.Height;
                        textureArray[materialIndex].textureWidth = textureArray[materialIndex].TexelData.textureBitmap.Width;

                        int TextureMass = (textureArray[materialIndex].textureHeight * textureArray[materialIndex].textureWidth);


                        if (TextureMass > 2048)
                        {

                        }

                    }
                    else
                    {

                        while (!(File.Exists(textureArray[materialIndex].texturePath)))
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
                            textureArray[materialIndex].textureHeight = 32;
                            textureArray[materialIndex].textureWidth = 32;
                            break;
                            //}
                        }
                    }


                }
            }

            return textureArray;
        }



        public byte[] WriteTextureObjects(OK64Texture[] TextureData)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);


            binaryWriter.Write(TextureData.Length);
            for (int ThisTexture = 0; ThisTexture < TextureData.Length; ThisTexture++)
            {


                binaryWriter.Write(TextureData[ThisTexture].TexelData.textureName);
                binaryWriter.Write(TextureData[ThisTexture].ColorCombine.CombineModeA);
                binaryWriter.Write(TextureData[ThisTexture].ColorCombine.CombineModeB);

                for (int ThisBool = 0; ThisBool < F3DEX095_Parameters.GeometryModes.Length; ThisBool++)
                {
                    binaryWriter.Write(TextureData[ThisTexture].ColorCombine.GeometryBools[ThisBool]);
                }

                binaryWriter.Write(TextureData[ThisTexture].ColorCombine.RenderModeA);
                binaryWriter.Write(TextureData[ThisTexture].ColorCombine.RenderModeB);

                if (TextureData[ThisTexture].texturePath != null)
                {
                    binaryWriter.Write(TextureData[ThisTexture].texturePath);
                    binaryWriter.Write(TextureData[ThisTexture].textureScrollS);
                    binaryWriter.Write(TextureData[ThisTexture].textureScrollT);
                    binaryWriter.Write(TextureData[ThisTexture].textureScreen);

                    binaryWriter.Write(TextureData[ThisTexture].SFlag);
                    binaryWriter.Write(TextureData[ThisTexture].TFlag);



                    binaryWriter.Write(TextureData[ThisTexture].TextureFormat);
                    binaryWriter.Write(TextureData[ThisTexture].BitSize);



                    binaryWriter.Write(TextureData[ThisTexture].textureWidth);
                    binaryWriter.Write(TextureData[ThisTexture].textureHeight);
                }
                else
                {
                    binaryWriter.Write("NULL");
                }

            }

            return memoryStream.ToArray();
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
                if ((textureObject[currentTexture].texturePath != null) && (textureObject[currentTexture].texturePath != "NULL"))
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
                        bitmapData = new Bitmap(textureObject[currentTexture].texturePath);
                    }
                    catch
                    {
                        bitmapData = new Bitmap(Tarmac64_Library.Properties.Resources.TextureNotFound);
                    }



                    if (textureObject[currentTexture].alphaPath != "")
                    {
                        if (File.Exists(textureObject[currentTexture].alphaPath))
                        {
                            Bitmap MaskedTexture = new Bitmap(bitmapData.Width, bitmapData.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            Bitmap alphaData = new Bitmap(textureObject[currentTexture].alphaPath);

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
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], MaskedTexture);
                        }
                        else
                        {
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], bitmapData);
                        }
                    }
                    else
                    {
                        N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], bitmapData);
                    }



                    // finish setting texture parameters based on new texture and compressed data.

                    textureObject[currentTexture].TexelData.compressedSize = imageData.Length;
                    textureObject[currentTexture].TexelData.fileSize = imageData.Length;
                    textureObject[currentTexture].TexelData.segmentPosition = Convert.ToInt32(binaryWriter.BaseStream.Position + DataLength);  // we need this to build out F3DEX commands later. 
                    TextureSize = TextureSize + textureObject[currentTexture].TexelData.fileSize;


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
                        textureObject[currentTexture].TexelData.PaletteData = paletteData;
                        textureObject[currentTexture].TexelData.paletteSize = paletteData.Length;
                        textureObject[currentTexture].TexelData.palettePosition = SegPosition;
                        SegPosition += textureObject[currentTexture].TexelData.paletteSize;
                        addressAlign = 0x1000 - (SegPosition % 0x1000);
                        if (addressAlign == 0x1000)
                            addressAlign = 0;
                        SegPosition += addressAlign;

                        binaryWriter.Write(textureObject[currentTexture].TexelData.PaletteData);
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
                if ((textureObject[currentTexture].texturePath != null) && (textureObject[currentTexture].texturePath != "NULL"))
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
                    Bitmap bitmapData = new Bitmap(textureObject[currentTexture].texturePath);
                    if (textureObject[currentTexture].alphaPath != "")
                    {
                        if (File.Exists(textureObject[currentTexture].alphaPath))
                        {
                            Bitmap MaskedTexture = new Bitmap(bitmapData.Width, bitmapData.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            Bitmap alphaData = new Bitmap(textureObject[currentTexture].alphaPath);

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
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], MaskedTexture);
                        }
                        else
                        {
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], bitmapData);
                        }
                    }
                    else
                    {
                        N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[textureObject[currentTexture].TextureFormat][textureObject[currentTexture].BitSize], bitmapData);
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

                    textureObject[currentTexture].TexelData.compressedSize = imageData.Length;
                    textureObject[currentTexture].TexelData.fileSize = imageData.Length;
                    textureObject[currentTexture].TexelData.segmentPosition = Convert.ToInt32(binaryWriter.BaseStream.Position + DataLength);


                    // write compressed MIO0 texture to end of ROM.


                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    textureObject[currentTexture].TexelData.TextureData = imageData;
                    binaryWriter.Write(imageData);

                    int SegPosition = Convert.ToInt32(binaryWriter.BaseStream.Length) + DataLength;
                    if (paletteData != null)
                    {
                        textureObject[currentTexture].TexelData.PaletteData = paletteData;
                        textureObject[currentTexture].TexelData.paletteSize = paletteData.Length;
                        textureObject[currentTexture].TexelData.segmentPosition = SegPosition;
                        SegPosition += textureObject[currentTexture].TexelData.paletteSize;
                        addressAlign = 0x1000 - (SegPosition % 0x1000);
                        if (addressAlign == 0x1000)
                            addressAlign = 0;
                        SegPosition += addressAlign;

                        binaryWriter.Write(textureObject[currentTexture].TexelData.PaletteData);
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
                if ((TextureArray[currentTexture].texturePath != null) && (TextureArray[currentTexture].texturePath != "NULL"))
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
                        TextureData = new Bitmap(TextureArray[currentTexture].texturePath);
                    }
                    catch
                    {
                        TextureData = new Bitmap(Tarmac64_Library.Properties.Resources.TextureNotFound);
                    }


                    if (TextureArray[currentTexture].alphaPath != "")
                    {
                        if (File.Exists(TextureArray[currentTexture].alphaPath))
                        {
                            Bitmap MaskedTexture = new Bitmap(TextureData.Width, TextureData.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            Bitmap alphaData = new Bitmap(TextureArray[currentTexture].alphaPath);

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
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[TextureArray[currentTexture].TextureFormat][TextureArray[currentTexture].BitSize], MaskedTexture);
                        }
                        else
                        {
                            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[TextureArray[currentTexture].TextureFormat][TextureArray[currentTexture].BitSize], TextureData);
                        }
                    }
                    else
                    {
                        N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[TextureArray[currentTexture].TextureFormat][TextureArray[currentTexture].BitSize], TextureData);
                    }

                    TextureArray[currentTexture].TexelData.compressedTexture = Tarmac.CompressMIO0(imageData);
                    TextureArray[currentTexture].TexelData.TextureData = imageData;
                    TextureArray[currentTexture].TexelData.PaletteData = paletteData;

                    // finish setting texture parameters based on new texture and compressed data.

                    TextureArray[currentTexture].TexelData.compressedSize = TextureArray[currentTexture].TexelData.compressedTexture.Length;
                    TextureArray[currentTexture].TexelData.fileSize = imageData.Length;
                    TextureArray[currentTexture].TexelData.segmentPosition = segment5Position;  // we need this to build out F3DEX commands later.                     
                    segment5Position += TextureArray[currentTexture].TexelData.fileSize;
                    if (paletteData != null)
                    {
                        TextureArray[currentTexture].TexelData.paletteSize = paletteData.Length;
                        TextureArray[currentTexture].TexelData.palettePosition = segment5Position;
                        segment5Position += TextureArray[currentTexture].TexelData.paletteSize;
                        int addressAlign = 0x1000 - (segment5Position % 0x1000);
                        if (addressAlign == 0x1000)
                            addressAlign = 0;
                        segment5Position += addressAlign;
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
                if ((TextureArray[CurrentTexture].texturePath != null) && (TextureArray[CurrentTexture].texturePath != "NULL"))
                {

                    // write compressed MIO0 texture to end of ROM.


                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;
                    binaryWriter.Write(TextureArray[CurrentTexture].TexelData.TextureData);
                    if (TextureArray[CurrentTexture].TexelData.PaletteData != null)
                    {
                        binaryWriter.Write(TextureArray[CurrentTexture].TexelData.PaletteData);
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


            /*
            
            //Old Code for loading textures individually. 
            //Custom levels do not (currently) have capacity for shared textures.
            
            int textureCount = (textureObject.Length);
            for (int currentTexture = 0; currentTexture < textureCount; currentTexture++) 
            {
                // write out segment 9 texture reference.
                if ((textureObject[currentTexture].texturePath != null) && (textureObject[currentTexture].texturePath != "NULL"))
                {
                    byteArray = BitConverter.GetBytes(Convert.ToUInt32(0x0F000000 | textureObject[currentTexture].romPosition - 0x641F70));
                    Array.Reverse(byteArray);
                    seg9w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(Convert.ToUInt32(textureObject[currentTexture].compressedSize));
                    Array.Reverse(byteArray);
                    seg9w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(Convert.ToUInt32(textureObject[currentTexture].fileSize));
                    Array.Reverse(byteArray);
                    seg9w.Write(byteArray);

                    byteArray = BitConverter.GetBytes(Convert.ToUInt32(0));
                    Array.Reverse(byteArray);
                    seg9w.Write(byteArray);
                }
            }
            */


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
