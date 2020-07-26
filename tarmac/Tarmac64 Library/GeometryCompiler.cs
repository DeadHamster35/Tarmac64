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
using PeepsCompress;

using Tarmac64_Library;
using Tarmac64_Geometry;
using Tarmac64_Paths;
using System.Text.RegularExpressions;
using Tarmac64_Library.Properties;
using SharpGL;
using System.Drawing.Design;
using System.Windows.Input;

namespace Tarmac64
{
    public partial class GeometryCompiler : Form
    {
        public GeometryCompiler()
        {
            InitializeComponent();
        }


        bool loadGL = false;
        bool wideScreen = false;






        TM64_Geometry mk = new TM64_Geometry();
        TM64_Paths mkPath = new TM64_Paths();

        TM64_Geometry.OK64SectionList[] sectionList = new TM64_Geometry.OK64SectionList[0];
        TM64_Geometry.OK64SectionList[] surfaceList = new TM64_Geometry.OK64SectionList[0];

        MemoryStream bs = new MemoryStream();
        BinaryReader br = new BinaryReader(Stream.Null);
        BinaryWriter bw = new BinaryWriter(Stream.Null);
        MemoryStream ds = new MemoryStream();
        BinaryReader dr = new BinaryReader(Stream.Null);
        BinaryWriter dw = new BinaryWriter(Stream.Null);
        MemoryStream vs = new MemoryStream();
        BinaryReader vr = new BinaryReader(Stream.Null);

        uint[] gltextureArray = new uint[0];

        string[] viewString = new string[] { "North", "East", "South", "West" };

        string[] surfaceType = new string[] { "Solid", "Dirt", "Dirt Track", "Cement", "Snow Track", "Wood", "Dirt Off-Road", "Grass", "Ice", "Beach Sand", "Snow Off-Road", "Rock Walls", "Dirt Off-Road", "Train Tracks", "Cave Interior", "Rickety Wood Bridge", "Solid Wood Bridge", "DK Parkyway Boost", "Out-Of-Bounds", "Royal Raceway Boost", "Walls" };
        int[] surfaceTypeID = new int[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0xFC, 0xFD, 0xFE, 0xFF };

        string[] songNames = new string[] { "None", "Title", "Menu", "Raceways", "Moo Moo Farm", "Choco Mountain", "Koopa Troopa Beach", "Banshee Boardwalk", "Snowland", "Bowser's Castle", "Kalimari Desert", "#- GP Startup", "#- Final Lap (1st)", "#- Final Lap", "#- You Lose", "#- Race Results", "Star Music", "Rainbow Road", "DK Parkway", "#- Credits Failure", "Toad's Turnpike", "#- VS/Battle Start", "#- VS/Battle Results", "#- Retry/Quit", "Big Donut / Skyscraper", "#- Trophy A", "#- Trophy B1 (Win)", "Credits", "#- Trophy B2 (Lose)" }; 

        bool loaded = false;

        string FBXfilePath = "";

        TM64_Geometry.OK64F3DObject[] masterObjects = new TM64_Geometry.OK64F3DObject[0];
        int masterCount = 0;


        List<TM64_Geometry.OK64F3DObject> masterList = new List<TM64_Geometry.OK64F3DObject>();

        TM64_Geometry.OK64F3DObject[] surfaceObjects = new TM64_Geometry.OK64F3DObject[0];
        int surfaceCount = 0;

        TM64_Geometry.OK64Texture[] textureArray = new TM64_Geometry.OK64Texture[0];

        int lastMaterial = 0;


        byte[] skyBytes = new byte[6];

        AssimpSharp.FBX.FBXImporter assimpSharpImporter = new AssimpSharp.FBX.FBXImporter();
        AssimpSharp.Scene fbx = new AssimpSharp.Scene();


        OpenFileDialog fileOpen = new OpenFileDialog();
        SaveFileDialog fileSave = new SaveFileDialog();
        FolderBrowserDialog folderOpen = new FolderBrowserDialog();
        private void GeometryCompiler_Load(object sender, EventArgs e)
        {
            this.Width = 575;
            courseBox.SelectedIndex = 0;
            cupBox.SelectedIndex = 0;
            setBox.SelectedIndex = 0;

            rbbox.Text = "216";
            gbbox.Text = "232";
            bbbox.Text = "248";


            rtbox.Text = "128";
            gtbox.Text = "184";
            btbox.Text = "248";
            colorUpdate();

            for (int songIndex = 0; songIndex < songNames.Length; songIndex++)
            {
                songBox.Items.Add(songNames[songIndex]);
            }
            songBox.SelectedIndex = 3;

            sp1Box.Text = "2";
            sp2Box.Text = "2";
            sp3Box.Text = "2";



        }




        private void colorUpdate()
        {

            int rr = 0;
            int.TryParse(rtbox.Text, out rr);
            int gg = 0;
            int.TryParse(gtbox.Text, out gg);
            int bb = 0;
            int.TryParse(btbox.Text, out bb);
            System.Drawing.Color tbuttoncolor = System.Drawing.Color.FromArgb(rr, gg, bb);



            rr = 0;
            int.TryParse(rbbox.Text, out rr);
            gg = 0;
            int.TryParse(gbbox.Text, out gg);
            bb = 0;
            int.TryParse(bbbox.Text, out bb);
            System.Drawing.Color bbuttoncolor = System.Drawing.Color.FromArgb(rr, gg, bb);

            cptop.BackColor = tbuttoncolor;
            cpbot.BackColor = bbuttoncolor;

        }

     



        private void Button6_Click(object sender, EventArgs e)
        {

            int cID = (cupBox.SelectedIndex * 4) + courseBox.SelectedIndex;
            int setID = setBox.SelectedIndex;


            string romPath = romBox.Text;

            MessageBox.Show("Please select an output Directory");
            if (folderOpen.ShowDialog() == DialogResult.OK)
            {
                string outputDirectory = folderOpen.SelectedPath;

                string popFile = pathBox.Text;
                List<byte[]> Segments = new List<byte[]>();
                byte[] rom = File.ReadAllBytes(romPath);


                byte[] segment4 = new byte[0];
                byte[] segment6 = new byte[0];
                byte[] segment7 = new byte[0];
                byte[] segment9 = new byte[0];

                byte[] collisionList = new byte[0];
                byte[] renderList = new byte[0];
                byte[] popList = new byte[0];


                byte[] popData = Resources.popResources;

                byte[] surfaceTable = new byte[0];
                byte[] displayTable = new byte[0];

                int magic = 0;

                int vertMagic = 0;

                // Game speed multiplier. Default is 2
                byte[] gameSpeed = new byte[3];

                Byte.TryParse(sp1Box.Text, out gameSpeed[0]);
                Byte.TryParse(sp2Box.Text, out gameSpeed[1]);
                Byte.TryParse(sp3Box.Text, out gameSpeed[2]);


                
                //Course Music

                byte songID = Convert.ToByte(songBox.SelectedIndex);


                // This command writes all the bitmaps to the end of the ROM

                rom = mk.writeTextures(rom, textureArray);
                segment9 = mk.compiletextureTable(textureArray);


                

                
                popList = mkPath.popMarkers(popFile);
                

                //build segment 7 out of the main course objects and surface geometry
                //build segment 4 out of the same objects.

                mk.compileF3DObject(ref vertMagic, ref segment4, ref segment7, segment4, segment7, masterObjects, textureArray, vertMagic);
                byte[] tempBytes = new byte[0];
                mk.compileF3DObject(ref vertMagic, ref segment4, ref segment7, segment4, segment7, surfaceObjects, textureArray, vertMagic);


                // build various segment data

     
                renderList = mk.compileF3DList(ref sectionList, fbx, masterObjects, sectionList);






                surfaceTable = mk.compilesurfaceTable(surfaceObjects);

                magic = (8 + 7968 + 952 + 8 + 528 + (surfaceObjects.Length * 8));
                // 8 bytes for header
                // 7968 bytes for the POP data
                // 952 bytes for the POP resources
                // 8 bytes for Surface Table Footer
                // 528 bytes for the Display Table itself.
                // The surface table is 8 bytes per object.
                // We tracked the number of surface meshes while loading into surfaceObjects.
                // magic is the size of data written before the display lists.
                // it's needed to properly calculate the offsets.
                // We're calculating hardcoded offsets before writing them.
                // So we need to use magic to do it.

                // Build the display table with the above magic value

                displayTable = mk.compilesectionviewTable(sectionList, magic);




                //First, let's build Segment 6.


                bs = new MemoryStream();
                br = new BinaryReader(bs);
                bw = new BinaryWriter(bs);
                byte[] byteArray = new byte[0];

                byteArray = BitConverter.GetBytes(0xB8000000);
                Array.Reverse(byteArray);
                bw.Write(byteArray);
                byteArray = BitConverter.GetBytes(0x00000000);
                Array.Reverse(byteArray);
                bw.Write(byteArray);

                bw.Write(popList);
                bw.Write(popData);
                bw.Write(displayTable);
                bw.Write(surfaceTable);
                bw.Write(renderList);

                segment6 = bs.ToArray();


                //Compress appropriate segment data

                byte[] cseg7 = mk.compress_seg7(segment7);



                string courseName = nameBox.Text;
                string previewImage = previewBox.Text;
                string bannerImage = bannerBox.Text;
                string mapImage = mapBox.Text;
                string customASM = asmBox.Text;
                string ghostData = ghostBox.Text;

                byte[] skyColor = skyBytes;


                Int16[] mapCoords = new Int16[2];

                Int16.TryParse(xBox.Text, out mapCoords[0]);
                Int16.TryParse(yBox.Text, out mapCoords[1]);


                byte[] cseg4 = mk.compressMIO0(segment4);
                byte[] cseg6 = mk.compressMIO0(segment6);

                rom = mk.compileHotswap(segment4, segment6, segment7, segment9, courseName, previewImage, bannerImage, mapImage, mapCoords, customASM, ghostData, skyColor, songID, gameSpeed, rom, cID, setID);



                string savepath = "";

                savepath = Path.Combine(outputDirectory, "Mario Kart 64 (U) [!].z64");
                File.WriteAllBytes(savepath, rom);
                savepath = Path.Combine(outputDirectory, "Segment 4.bin");
                File.WriteAllBytes(savepath, segment4);

                savepath = Path.Combine(outputDirectory, "Compressed Segment 4.bin");
                File.WriteAllBytes(savepath, cseg4);

                savepath = Path.Combine(outputDirectory, "Segment 6.bin");
                File.WriteAllBytes(savepath, segment6);

                savepath = Path.Combine(outputDirectory, "Compressed Segment 6.bin");
                File.WriteAllBytes(savepath, cseg6);

                savepath = Path.Combine(outputDirectory, "Segment 7.bin");
                File.WriteAllBytes(savepath, segment7);

                savepath = Path.Combine(outputDirectory, "Compressed Segment 7.bin");
                File.WriteAllBytes(savepath, cseg7);

                savepath = Path.Combine(outputDirectory, "Segment 9.bin");
                File.WriteAllBytes(savepath, segment9);


                MessageBox.Show("Finished");
            }
        }





        


        private void loadBtn_click(object sender, EventArgs e)
        {

            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                FBXfilePath = fileOpen.FileName;
            }


            bool simpleFormat = false;  // flipped if simpleFormat is detected.
            
            fbx = assimpSharpImporter.ReadFile(FBXfilePath);

            
            int materialCount = fbx.Materials.Count;
            int textureCount = 0;

            int mastervertCount = 0;
            int masterfaceCount = 0;

            int surfacevertCount = 0;
            int surfacefaceCount = 0;


            int masterObjectCount = 0;



            AssimpSharp.Node masterNode = fbx.RootNode.FindNode("Course Master Objects");
            if (masterNode == null)
            {
                simpleFormat = true;
            }
            else
            {
                simpleFormat = false;
            }

            
            AssimpSharp.Node pathNode = fbx.RootNode.FindNode("Course Paths");
            AssimpSharp.Mesh countObj = null;
            int sectionCount = 0;


            for (int searchSection = 1; ; searchSection++)
            {
                AssimpSharp.Node searchNode = fbx.RootNode.FindNode("Section " + searchSection.ToString());
                if (searchNode != null)
                {
                    sectionCount++;
                }
                else
                {
                    break;
                }

            }
            
            
            //
            // Textures
            //




            textureArray = mk.loadTextures(fbx);
            
            materialCount = textureArray.Length;

            //
            // Course Objects
            // Surface Map
            //

            if (simpleFormat == false)
            {
                masterObjects = mk.loadMaster(fbx, textureArray);
                masterObjectCount = masterObjects.Length;
                surfaceObjects = mk.loadCollision(fbx, sectionCount, textureArray, simpleFormat);
                sectionList = mk.loadSection(fbx, sectionCount, masterObjects);
            }
            else
            {
                masterObjects = mk.createMaster(fbx,sectionCount, textureArray);
                masterObjectCount = masterObjects.Length;
                surfaceObjects = mk.loadCollision(fbx, sectionCount, textureArray, simpleFormat);
                sectionList = mk.automateSection(sectionCount, surfaceObjects, masterObjects, fbx);
            }

            //
            // Section Views
            //
            






            countBox.Text = sectionCount.ToString();
            surfcountBox.Text = sectionCount.ToString();


            

            for (int currentChild = 0; currentChild < masterObjects.Length; currentChild++)
            {
                masterBox.Items.Add(masterObjects[currentChild].objectName);
            }

            for (int currentIndex = 0; currentIndex < surfaceObjects.Length; currentIndex++)
            {
                surfaceobjectBox.Items.Add(surfaceObjects[currentIndex].objectName);
            }
            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                surfsectionBox.Items.Add("Section " + (currentSection + 1).ToString());
                sectionBox.Items.Add("Section " + (currentSection + 1).ToString());
                
            }
            for (int surfacematerialIndex = 0; surfacematerialIndex < surfaceType.Length; surfacematerialIndex++)
            {
                surfmaterialBox.Items.Add(surfaceTypeID[surfacematerialIndex].ToString("X") + "- " + surfaceType[surfacematerialIndex]);
            }
            foreach (var viewstring in viewString)
            {
                viewBox.Items.Add(viewstring);
            }

            

            for (int materialIndex = 0; materialIndex < materialCount;materialIndex++)
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
                        textureBox.Items.Add("Material " + materialIndex.ToString() + " - " + textureArray[materialIndex].textureName);
                    }
                    textureCount++;
                }
                else
                {
                    MessageBox.Show("Warning! Material " + fbx.Materials[materialIndex].Name + " does not have a diffuse texture and cannot be used.");
                    textureArray[materialIndex].textureName = fbx.Materials[materialIndex].Name;
                    textureBox.Items.Add("UNUSABLE " + materialIndex.ToString() + " - " + textureArray[materialIndex].textureName);
                }
            }

            viewBox.SelectedIndex = 0;

            mcountBox.Text = materialCount.ToString();
            tcountBox.Text = textureCount.ToString();

            textureBox.SelectedIndex = 0;
            lastMaterial = 0;


            objcountBox.Text = surfaceCount.ToString();

            MessageBox.Show("Finished Loading .FBX");
            loaded = true;
            sectionBox.SelectedIndex = 0;

            compilebtn.Enabled = true;
            mvertBox.Text = mastervertCount.ToString();
            mfaceBox.Text = masterfaceCount.ToString();
            mobjectbox.Text = masterObjectCount.ToString();
        }
        private void Matbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updateTXDisplay())
            {
                lastMaterial = textureBox.SelectedIndex;
            }
            else
            {
                textureBox.SelectedIndex = lastMaterial;
                MessageBox.Show("Selected Material Unavailable!");
            }
        }

        private void SectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded == true)
            {
                updateSVDisplay();
            }
        }

        private void ViewBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded == true)
            {
                updateSVDisplay();
            }
        }

        private void updateSVDisplay()
        {
            if (loaded == true)
            {
                int vertCount = 0;
                int faceCount = 0;
                foreach (int checkedIndex in masterBox.CheckedIndices)
                {
                    masterBox.SetItemCheckState(checkedIndex, CheckState.Unchecked);
                }
                foreach (var subObject in sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList)
                {
                    masterBox.SetItemCheckState(subObject, CheckState.Checked);
                    vertCount = vertCount + masterObjects[subObject].vertCount;
                    faceCount = faceCount + masterObjects[subObject].faceCount;
                }
                updateCounter(faceCount, vertCount);
            }
        }
        //Seperate loading the counters to prevent infinite loop. 
        private void updateCounter(int faceCount, int vertCount)
        {

            vertBox.Text = vertCount.ToString();
            faceBox.Text = faceCount.ToString();
            objectBox.Text = masterBox.CheckedItems.Count.ToString();
        }
        //
        private void updateSMDisplay()
        {
            if (loaded == true)
            {
                int objectIndex = surfaceobjectBox.SelectedIndex;

                surfsectionBox.SelectedIndex = surfaceObjects[objectIndex].surfaceID - 1;
                int materialIndex = Array.IndexOf(surfaceTypeID,surfaceObjects[objectIndex].surfaceMaterial);
                surfmaterialBox.SelectedIndex = materialIndex;
                surfcheckA.Checked = surfaceObjects[objectIndex].flagA;
                surfcheckB.Checked = surfaceObjects[objectIndex].flagB;
                surfcheckC.Checked = surfaceObjects[objectIndex].flagC;
                surfvertBox.Text = surfaceObjects[objectIndex].vertCount.ToString();
                surffaceBox.Text = surfaceObjects[objectIndex].faceCount.ToString();
            }
        }

        private bool updateTXDisplay()
        {
            if (textureArray[textureBox.SelectedIndex].texturePath != null)
            {
                bitm.ImageLocation = textureArray[textureBox.SelectedIndex].texturePath;
                heightBox.Text = textureArray[textureBox.SelectedIndex].textureHeight.ToString();
                widthBox.Text = textureArray[textureBox.SelectedIndex].textureWidth.ToString();
                formatBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureFormat;
                classBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureClass;
                tBox.Checked = textureArray[textureBox.SelectedIndex].textureTransparent;
                return true;
            }
            else
            {
                return false;
            }
        }


        private void ObjectBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
            this.BeginInvoke(new MethodInvoker(CheckList), null);
        }

        private void CheckList()
        {
            if (loaded == true)
            {
                int vertCount = 0;
                int faceCount = 0;
                List<int> checkList = new List<int>();
                foreach(var checkObject in masterBox.CheckedItems)
                {
                    int checkIndex = masterBox.Items.IndexOf(checkObject);
                    checkList.Add(checkIndex);
                    vertCount = vertCount + masterObjects[checkIndex].vertCount;
                    faceCount = faceCount + masterObjects[checkIndex].faceCount;
                }
                sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList = checkList.ToArray();
                
                updateCounter(faceCount, vertCount);
            }
        }

        private void Formatbox_SelectedIndexChanged(object sender, EventArgs e)
        {

            //this is disabled because I currently don't know how to write the F3DEX to load a CI8 format texture.
            //the code for converting the file to CI8, MIO0 compressing and writing it to ROM is fine...
            // but in the section where Course Objects are written to segment 7 there is no switch for CI8...
            // or the material types 3-5. Only RGBA16 is supported at this time, until that code is written.


            textureArray[textureBox.SelectedIndex].textureFormat = formatBox.SelectedIndex;
            mk.textureClass(textureArray[textureBox.SelectedIndex]);
            updateTXDisplay();
        }

        private void SurfaceobjectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateSMDisplay();
        }

        private void SurfsectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            surfaceObjects[surfaceobjectBox.SelectedIndex].surfaceID = surfsectionBox.SelectedIndex + 1;
        }

        private void SurfmaterialBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] surfacesplit = surfmaterialBox.Items[surfmaterialBox.SelectedIndex].ToString().Split('-');
            int surfaceIndex = Convert.ToInt32(surfacesplit[0], 16);
        }

        private void Rtbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(rtbox.Text, out skyBytes[0]);
            colorUpdate();
        }

        private void Gtbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(gtbox.Text, out skyBytes[1]);
            colorUpdate();
        }

        private void Btbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(btbox.Text, out skyBytes[2]);
            colorUpdate();
        }

        private void Rbbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(rbbox.Text, out skyBytes[3]);
            colorUpdate();
        }

        private void Gbbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(gbbox.Text, out skyBytes[4]);
            colorUpdate();
        }

        private void Bbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(bbbox.Text, out skyBytes[5]);
            colorUpdate();
        }

        private void Cptop_Click(object sender, EventArgs e)
        {
            ColorDialog skyDialog = new ColorDialog();

            skyDialog.AllowFullOpen = true;
            skyDialog.ShowHelp = true;


            int rr = 0;
            int.TryParse(rtbox.Text, out rr);
            int gg = 0;
            int.TryParse(gtbox.Text, out gg);
            int bb = 0;
            int.TryParse(btbox.Text, out bb);
            System.Drawing.Color buttonColor = System.Drawing.Color.FromArgb(rr, gg, bb);

            skyDialog.Color = buttonColor;

            // Update the text box color if the user clicks OK 
            if (skyDialog.ShowDialog() == DialogResult.OK)
            {
                buttonColor = skyDialog.Color;
                rtbox.Text = skyDialog.Color.R.ToString();
                gtbox.Text = skyDialog.Color.G.ToString();
                btbox.Text = skyDialog.Color.B.ToString();
            }
            colorUpdate();

        }

        private void Cpbot_Click(object sender, EventArgs e)
        {
            ColorDialog skyDialog = new ColorDialog();

            skyDialog.AllowFullOpen = true;
            skyDialog.ShowHelp = true;


            int rr = 0;
            int.TryParse(rbbox.Text, out rr);
            int gg = 0;
            int.TryParse(gbbox.Text, out gg);
            int bb = 0;
            int.TryParse(bbbox.Text, out bb);
            System.Drawing.Color buttonColor = System.Drawing.Color.FromArgb(rr, gg, bb);

            skyDialog.Color = buttonColor;

            // Update the text box color if the user clicks OK 
            if (skyDialog.ShowDialog() == DialogResult.OK)
            {
                buttonColor = skyDialog.Color;
                rbbox.Text = skyDialog.Color.R.ToString();
                gbbox.Text = skyDialog.Color.G.ToString();
                bbbox.Text = skyDialog.Color.B.ToString();
            }
            colorUpdate();

        }

        private void RomBtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                romBox.Text = fileOpen.FileName;
            }

        }

        private void PathBtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                pathBox.Text = fileOpen.FileName;
            }
        }

        private void PreviewBtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                previewBox.Text = fileOpen.FileName;
            }
        }

        private void BannerBtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                bannerBox.Text = fileOpen.FileName;
            }
        }

        private void MapBtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                mapBox.Text = fileOpen.FileName;
            }
        }

        private void AsmBtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                asmBox.Text = fileOpen.FileName;
            }
        }


        private void ghostbtn_Click(object sender, EventArgs e)
        {
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                ghostBox.Text = fileOpen.FileName;
            }
        }

        private void tBox_CheckedChanged(object sender, EventArgs e)
        {
            textureArray[textureBox.SelectedIndex].textureTransparent = tBox.Checked;
        }






        public byte[] pngtoByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        public byte[] bmptoByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            return ms.ToArray();
        }




        private void drawObjects()
        {
            OpenGL gl = openGLControl.OpenGL;
            List<string> drawnObjects = new List<string>();
            //draw faces

            switch (tabControl1.SelectedIndex)
            {
                case 3:

                    foreach (var subObject in surfaceObjects)
                    {
                        gl.Begin(OpenGL.GL_TRIANGLES);
                        if (subObject.surfaceID == (sectionBox.SelectedIndex + 1))
                        {
                            drawnObjects.Add(subObject.objectName);

                            foreach (var subFace in subObject.modelGeometry)
                            {
                                foreach (var subVert in subFace.vertData)
                                {
                                    gl.Color(1.0f, 0.0f, 0.0f, 1.0f);
                                    gl.Vertex(subVert.position.x, subVert.position.z, -1 * subVert.position.y);
                                }

                            }
                        }
                        gl.End();
                    }
                    foreach (var subIndex in sectionList[sectionBox.SelectedIndex].viewList[viewBox.SelectedIndex].objectList)
                    {
                        gl.Begin(OpenGL.GL_TRIANGLES);
                        

                        foreach (var subFace in masterObjects[subIndex].modelGeometry)
                        {
                            foreach (var subVert in subFace.vertData)
                            {
                                gl.Color(masterObjects[subIndex].objectColor[0], masterObjects[subIndex].objectColor[1], masterObjects[subIndex].objectColor[2], 1.0f);
                                gl.Vertex(subVert.position.x, subVert.position.z, -1 * subVert.position.y);
                            }

                        }
                        gl.End();

                    }
                    
                    break;
                case 4:
                    foreach (var subObject in surfaceObjects)
                    {
                        gl.Begin(OpenGL.GL_TRIANGLES);
                        drawnObjects.Add(subObject.objectName);

                        foreach (var subFace in subObject.modelGeometry)
                        {
                            foreach (var subVert in subFace.vertData)
                            {
                                gl.Color(subObject.objectColor[0], subObject.objectColor[1], subObject.objectColor[2], 1.0f);
                                gl.Vertex(subVert.position.x, subVert.position.z, -1 * subVert.position.y);
                            }

                        }
                        gl.End();
                    }
                    break;
                default:
                    foreach (var subObject in masterObjects)
                    {

                        gl.Begin(OpenGL.GL_TRIANGLES);
                        

                        foreach (var subFace in subObject.modelGeometry)
                        {
                            foreach (var subVert in subFace.vertData)
                            {
                                gl.Color(subObject.objectColor[0], subObject.objectColor[1], subObject.objectColor[2], 1.0f);
                                gl.Vertex(subVert.position.x, subVert.position.z, -1 * subVert.position.y);
                            }

                        }
                        gl.End();
                    }
                    break;

            }
            loadGL = true;
        }

       

        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.Begin(OpenGL.GL_TRIANGLES);

            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(lookX + 0.0f, lookY + 1.0f, lookZ + 0.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(lookX + -1.0f, lookY + -1.0f, lookZ + 1.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(lookX + 1.0f, lookY + -1.0f, lookZ + 1.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(lookX + 0.0f, lookY + 1.0f, lookZ + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(lookX + 1.0f, lookY + -1.0f, lookZ + 1.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(lookX + 1.0f, lookY + -1.0f, lookZ + -1.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(lookX + 0.0f, lookY + 1.0f, lookZ + 0.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(lookX + 1.0f, lookY + -1.0f, lookZ + -1.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(lookX + -1.0f, lookY + -1.0f, lookZ + -1.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(lookX + 0.0f, lookY + 1.0f, lookZ + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(lookX + -1.0f, lookY + -1.0f, lookZ + -1.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(lookX + -1.0f, lookY + -1.0f, lookZ + 1.0f);
            gl.End();

            drawObjects();

            openGLControl_Resized(null, null);

        }

        double cameraX = 0;
        double cameraY = 5;
        double cameraZ = -30;
        double cameraH = 1;
        double lookX = 0;
        double lookY = 0;
        double lookZ = 0;

        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            gl.Perspective(90.0f, (double)Width / (double)Height, 0.01, 50000.0);

            //  Use the 'look at' helper function to position and aim the camera.
            gl.LookAt(cameraX, cameraY, cameraZ, lookX, lookY , lookZ, 0, 1, 0);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        public static System.Windows.Point RotatePoint(System.Windows.Point pointToRotate, System.Windows.Point centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new System.Windows.Point
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }

        private void openGLControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (loadGL)
            {
                
                if (Keyboard.IsKeyDown(Key.W))
                {
                    
                    double midZ = (lookZ + cameraZ)/2;
                    double midX = (lookX + cameraX) / 2;
                    double gapZ = cameraZ - midZ;
                    double gapX = cameraX - midX;
                    cameraZ = midZ;
                    lookZ -= gapZ;
                    cameraX = midX;
                    lookX -= gapX;

                }
                if (Keyboard.IsKeyDown(Key.S))
                {
                    
                    double midZ = (lookZ + cameraZ) / 2;
                    double midX = (lookX + cameraX) / 2;
                    double gapZ = cameraZ - midZ;
                    double gapX = cameraX - midX;
                    cameraZ += gapZ;
                    lookZ += gapZ;
                    cameraX += gapX;
                    lookX += gapX;


                }
                if (Keyboard.IsKeyDown(Key.Q))
                {
                    double midZ = (lookZ + cameraZ) / 2;
                    double midX = (lookX + cameraX) / 2;
                    double gapZ = cameraZ - midZ;
                    double gapX = cameraX - midX;
                    cameraZ -= gapZ;
                    lookZ -= gapZ;
                    cameraX += gapX;
                    lookX += gapX;
                }
                if (Keyboard.IsKeyDown(Key.E))
                {
                    double midZ = (lookZ + cameraZ) / 2;
                    double midX = (lookX + cameraX) / 2;
                    double gapZ = cameraZ - midZ;
                    double gapX = cameraX - midX;
                    cameraZ += gapZ;
                    lookZ += gapZ;
                    cameraX -= gapX;
                    lookX -= gapX;

                }
                if (Keyboard.IsKeyDown(Key.R))
                {

                    cameraY += 10;
                    lookY += 10;

                }
                if (Keyboard.IsKeyDown(Key.F))
                {

                    cameraY -= 10;
                    lookY -= 10;

                }
                if (Keyboard.IsKeyDown(Key.A))
                {
                    var centerPoint = new System.Windows.Point(cameraX, cameraZ);
                    var originalPoint = new System.Windows.Point(lookX,lookZ);                    
                    var newPoint = RotatePoint(originalPoint, centerPoint, 5);
                    double midZ = (lookZ + cameraZ) / 90;
                    double midX = (lookX + cameraX) / 90;

                    lookX = newPoint.X + midX;
                    lookZ = newPoint.Y + midZ;

                }
                if (Keyboard.IsKeyDown(Key.D))
                {

                    var centerPoint = new System.Windows.Point(cameraX, cameraZ);
                    var originalPoint = new System.Windows.Point(lookX, lookZ);
                    var newPoint = RotatePoint(originalPoint, centerPoint, -5);
                    double midZ = (lookZ + cameraZ) / 90;
                    double midX = (lookX + cameraX) / 90;

                    lookX = newPoint.X + midX;
                    lookZ = newPoint.Y + midZ;

                }
                
                if (Keyboard.IsKeyDown(Key.C))
                {

                    double midZ = (lookZ + cameraZ) / 2;
                    double midX = (lookX + cameraX) / 2;
                    double gapZ = cameraZ - midZ;
                    double gapX = cameraX - midX;
                    
                    lookZ += gapZ;                    
                    lookX += gapX;

                }
                if (Keyboard.IsKeyDown(Key.V))
                {

                    double midZ = (lookZ + cameraZ) / 2;
                    double midX = (lookX + cameraX) / 2;
                    double gapZ = cameraZ - midZ;
                    double gapX = cameraX - midX;
                    
                    lookZ -= gapZ;                    
                    lookX -= gapX;

                }
                if (Keyboard.IsKeyDown(Key.G))
                {

                    lookX = 0;
                    lookZ = 0;
                    lookY = 0;

                }

                cxBox.Text = cameraX.ToString();
                czBox.Text = cameraZ.ToString();
                cyBox.Text = cameraY.ToString();

                lxBox.Text = lookX.ToString();
                lzBox.Text = lookZ.ToString();
                lyBox.Text = lookY.ToString();

                openGLControl_Resized(null, null);
            }
        }

        private void wideBtn_Click(object sender, EventArgs e)
        {
            if (wideScreen)
            {
                wideScreen = false;
                this.Width = 575;
                wideBtn.Text = ">>>";
            }
            else
            {
                wideScreen = true;
                this.Width = 1235;
                wideBtn.Text = "<<<";
            }
        }

        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the clear color.
            gl.ClearColor(.0f, .0f, .0f, 0);
            
        }

        private void openGLControl_Load(object sender, EventArgs e)
        {

        }
    }
}




