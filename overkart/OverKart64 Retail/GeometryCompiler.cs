using System;
using System.Windows;
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

using OverKart64;
using System.Text.RegularExpressions;
using OverKart64_Retail.Properties;

namespace OverKart64
{
    public partial class GeometryCompiler : Form
    {
        public GeometryCompiler()
        {
            InitializeComponent();
        }

        


        


        OK64 mk = new OK64();
        
        OK64.OK64SectionList[] sectionList = new OK64.OK64SectionList[0];
        OK64.OK64SectionList[] surfaceList = new OK64.OK64SectionList[0];

        MemoryStream bs = new MemoryStream();
        BinaryReader br = new BinaryReader(Stream.Null);
        BinaryWriter bw = new BinaryWriter(Stream.Null);
        MemoryStream ds = new MemoryStream();
        BinaryReader dr = new BinaryReader(Stream.Null);
        BinaryWriter dw = new BinaryWriter(Stream.Null);
        MemoryStream vs = new MemoryStream();
        BinaryReader vr = new BinaryReader(Stream.Null);


        string[] viewString = new string[] { "North", "East", "South", "West" };

        string[] surfaceType = new string[] { "Solid", "Dirt", "Dirt Track", "Cement", "Snow Track", "Wood", "Dirt Off-Road", "Grass", "Ice", "Beach Sand", "Snow Off-Road", "Rock Walls", "Dirt Off-Road", "Train Tracks", "Cave Interior", "Rickety Wood Bridge", "Solid Wood Bridge", "DK Parkyway Boost", "Out-Of-Bounds", "Royal Raceway Boost", "Walls" };
        int[] surfaceTypeID = new int[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0xFC, 0xFD, 0xFE, 0xFF };

        bool loaded = false;

        string FBXfilePath = "";

        OK64.OK64F3DObject[] masterObjects = new OK64.OK64F3DObject[0];
        int masterCount = 0;


        List<OK64.OK64F3DObject> masterList = new List<OK64.OK64F3DObject>();

        OK64.OK64F3DObject[] surfaceObjects = new OK64.OK64F3DObject[0];
        int surfaceCount = 0;

        OK64.OK64Texture[] textureArray = new OK64.OK64Texture[0];


        


        byte[] flip4 = new byte[4];
        byte[] flip2 = new byte[2];

        int[] img_types = new int[] { 0, 0, 0, 3, 3, 3 };
        int[] img_heights = new int[] { 32, 32, 64, 32, 32, 64 };
        int[] img_widths = new int[] { 32, 64, 32, 32, 64, 32 };



        byte[] skyColor = new byte[6];

        AssimpSharp.FBX.FBXImporter assimpSharpImporter = new AssimpSharp.FBX.FBXImporter();
        AssimpSharp.Scene fbx = new AssimpSharp.Scene();


        OpenFileDialog vertopen = new OpenFileDialog();
        SaveFileDialog vertsave = new SaveFileDialog();
        FolderBrowserDialog textsave = new FolderBrowserDialog();
        private void GeometryCompiler_Load(object sender, EventArgs e)
        {
            
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
        }




        private void colorUpdate()
        {

            int rr = 0;
            int.TryParse(rtbox.Text, out rr);
            int gg = 0;
            int.TryParse(gtbox.Text, out gg);
            int bb = 0;
            int.TryParse(btbox.Text, out bb);
            Color tbuttoncolor = Color.FromArgb(rr, gg, bb);



            rr = 0;
            int.TryParse(rbbox.Text, out rr);
            gg = 0;
            int.TryParse(gbbox.Text, out gg);
            bb = 0;
            int.TryParse(bbbox.Text, out bb);
            Color bbuttoncolor = Color.FromArgb(rr, gg, bb);

            cptop.BackColor = tbuttoncolor;
            cpbot.BackColor = bbuttoncolor;

        }




        private void Button6_Click(object sender, EventArgs e)
        {

            int cID = (cupBox.SelectedIndex * 4) + courseBox.SelectedIndex;
            int setID = setBox.SelectedIndex;


            string romPath = romBox.Text;

            string outputDirectory = outBox.Text;
            string popFile = pathBox.Text;




            int mdlOffset = 0;
            int sfcOffset = 0;


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







            // This command writes all the bitmaps to the end of the ROM

            rom = mk.writeTextures(rom, textureArray);
            segment9 = mk.compiletextureTable(textureArray);

            //
            //
            //
            //


            //build segment 7 out of the main course objects and surface geometry
            //build segment 4 out of the same objects.

            mk.compileF3DObject(ref vertMagic, ref segment4, ref segment7, fbx, segment4, segment7, masterObjects, textureArray, vertMagic);
            byte[] tempBytes = new byte[0];
            mk.compileF3DObject(ref vertMagic, ref segment4, ref segment7, fbx, segment4, segment7, surfaceObjects, textureArray, vertMagic);
            



            


            //
            //

            // build various segment data

            
            popList = mk.popMarkers(popFile);
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

            int[] mapCoords = new int[]{ Convert.ToInt16(xBox.Text), Convert.ToInt16(yBox.Text) };
            
            colorUpdate();

            byte[] cseg4 = mk.fakeCompress(segment4);
            byte[] cseg6 = mk.fakeCompress(segment6);

            rom = mk.compileHotswap(cseg4, cseg6, cseg7, segment9, courseName, previewImage, bannerImage, mapImage, mapCoords, customASM, skyColor, rom, cID, setID);



            string savepath = "";
            
            savepath = Path.Combine(outputDirectory , "Mario Kart 64 (U) [!].z64");
            File.WriteAllBytes(savepath, rom);
            savepath = Path.Combine(outputDirectory , "Segment 4.bin");
            File.WriteAllBytes(savepath, segment4);

            savepath = Path.Combine(outputDirectory, "Compressed Segment 4.bin");
            File.WriteAllBytes(savepath, cseg4);

            savepath = Path.Combine(outputDirectory, "Segment 6.bin");
            File.WriteAllBytes(savepath, segment6);

            savepath = Path.Combine(outputDirectory , "Compressed Segment 6.bin");
            File.WriteAllBytes(savepath, cseg6);
            
            savepath = Path.Combine(outputDirectory , "Segment 7.bin");
            File.WriteAllBytes(savepath, segment7);

            savepath = Path.Combine(outputDirectory, "Compressed Segment 7.bin");
            File.WriteAllBytes(savepath, cseg7);

            savepath = Path.Combine(outputDirectory, "Segment 9.bin");
            File.WriteAllBytes(savepath, segment9);


            MessageBox.Show("Finished");

        }


        public static IEnumerable<OK64.OK64F3DObject> NaturalSort(IEnumerable<OK64.OK64F3DObject> list)
        {
            int maxLen = list.Select(s => s.objectName.Length).Max();
            Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

            return list
                    .Select(s =>
                        new
                        {
                            OrgStr = s,
                            SortStr = Regex.Replace(s.objectName, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                        })
                    .OrderBy(x => x.SortStr)
                    .Select(x => x.OrgStr);
        }




        


        private void loadBtn_click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                FBXfilePath = vertopen.FileName;
            }
            
            
            fbx = assimpSharpImporter.ReadFile(FBXfilePath);

            
            int textureCount = fbx.Materials.Count;

            int mastervertCount = 0;
            int masterfaceCount = 0;

            int surfacevertCount = 0;
            int surfacefaceCount = 0;


            int masterObjectCount = 0;




            var count_node = fbx.RootNode.FindNode("section count");
            var count_obj = fbx.Meshes[count_node.Meshes[0]];
            int sectionCount = Convert.ToInt32(count_obj.Vertices[0].X);



            //
            // Textures
            //


            Array.Resize(ref textureArray, textureCount);


            for (int textureIndex = 0; textureIndex < textureCount; textureIndex++)
            {
                textureArray[textureIndex] = new OK64.OK64Texture();
                textureArray[textureIndex].texturePath = fbx.Materials[textureIndex].TextureDiffuse.TextureBase;
                textureArray[textureIndex].textureName = Path.GetFileName(textureArray[textureIndex].texturePath);
                textureArray[textureIndex].textureFormat = 0;

                Image bitmapFile = Image.FromFile(textureArray[textureIndex].texturePath);

                textureArray[textureIndex].textureHeight = bitmapFile.Height;
                textureArray[textureIndex].textureWidth = bitmapFile.Width;

                textureBox.Items.Add("Material " + textureIndex.ToString()+" - "+ textureArray[textureIndex].textureName);
                
                mk.textureClass(textureArray[textureIndex]); 

                if (textureArray[textureIndex].textureClass == -1)
                {
                    MessageBox.Show("Error! Texture wrong dimensions -" + textureArray[textureIndex].textureName+"- Height: "+textureArray[textureIndex].textureHeight+"   Width: "+ textureArray[textureIndex].textureWidth);
                }
            }

            matcountbox.Text = textureCount.ToString();
            textureBox.SelectedIndex = 0;



            //
            // Course Objects
            //

            for (int surfacematerialIndex = 0 ; surfacematerialIndex < surfaceType.Length ; surfacematerialIndex++)
            {
                surfmaterialBox.Items.Add(surfaceTypeID[surfacematerialIndex].ToString("X")+"- "+surfaceType[surfacematerialIndex] );
            }



            foreach(var viewstring in viewString)
            {
                viewBox.Items.Add(viewstring);
            }
            viewBox.SelectedIndex = 0;

            var masterNode = fbx.RootNode.FindNode("Course Master Objects");
            int masterCount = masterNode.Children.Count;

            Array.Resize(ref masterObjects, masterCount);

            for (int currentChild = 0; currentChild < masterNode.Children.Count; currentChild++)
            {
                masterObjects[currentChild] = new OK64.OK64F3DObject();
                masterObjects[currentChild].objectName = masterNode.Children[currentChild].Name;
                masterObjects[currentChild].meshID = masterNode.Children[currentChild].Meshes.ToArray();
                masterObjects[currentChild].materialID = fbx.Meshes[masterObjects[currentChild].meshID[0]].MaterialIndex;
                int vertCount = 0;
                int faceCount = 0;
                foreach (var childMesh in masterNode.Children[currentChild].Meshes)
                {
                    
                    vertCount = vertCount + fbx.Meshes[childMesh].NumVertices;
                    faceCount = faceCount + fbx.Meshes[childMesh].NumFaces;
                    mastervertCount = mastervertCount + vertCount;
                    masterfaceCount = masterfaceCount + faceCount;

                }
                masterObjects[currentChild].vertCount = vertCount;
                masterObjects[currentChild].faceCount = faceCount;

            }
            List<OK64.OK64F3DObject> masterList = new List<OK64.OK64F3DObject>(masterObjects);



            masterObjects = NaturalSort(masterList).ToArray();


            for (int currentChild = 0; currentChild < masterNode.Children.Count; currentChild++)
            {
                masterBox.Items.Add(masterObjects[currentChild].objectName);
            }


            masterObjectCount = masterObjects.Count();

            //
            // Section Views
            //


            countBox.Text = sectionCount.ToString();
            surfcountBox.Text = sectionCount.ToString();

            Array.Resize(ref sectionList, sectionCount);

            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                sectionBox.Items.Add("Section " + (currentSection + 1).ToString());

                sectionList[currentSection] = new OK64.OK64SectionList();
                sectionList[currentSection].viewList = new OK64.OK64ViewList[4];
                for (int view = 0; view < 4; view++)
                {
                    sectionList[currentSection].viewList[view] = new OK64.OK64ViewList();

                    var parentNode = fbx.RootNode.FindNode("Section " + (currentSection + 1).ToString() + " " + viewString[view]);
                    sectionList[currentSection].viewList[view].objectList = new int[parentNode.Children.Count];

                    masterList = new List<OK64.OK64F3DObject>(masterObjects);

                    for (int currentObject = 0; currentObject < parentNode.Children.Count; currentObject++)
                    {
                        string searchObject = parentNode.Children[currentObject].Name;
                        var foundObject = masterObjects.FirstOrDefault(b => b.objectName == searchObject);
                        int masterIndex = Array.IndexOf(masterObjects, foundObject);
                        if (masterIndex == -1)
                        {
                            MessageBox.Show("Error- Object not Found- " + searchObject + "- Section " + currentSection.ToString() + "- View " + viewString[view]);
                        }
                        else
                        {
                            sectionList[currentSection].viewList[view].objectList[currentObject] = masterIndex;
                        }
                    }
                }

            }






            //
            //Surface Map
            //


            int surfacechildCount = 0;
            
            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                var surfaceNode = fbx.RootNode.FindNode("Section " + (currentSection + 1).ToString() + " Surface");
                surfacechildCount = surfacechildCount + surfaceNode.Children.Count;
            }

            
            int surfaceChild = 0;
            surfaceObjects = new OK64.OK64F3DObject[surfacechildCount];
            for (int currentSection = 0; currentSection < sectionCount; currentSection++)
            {
                var surfaceNode = fbx.RootNode.FindNode("Section " + (currentSection + 1).ToString() + " Surface");
                surfsectionBox.Items.Add("Section " + (currentSection + 1).ToString());
                int subobjectCount = surfaceNode.Children.Count;
                
                for (int currentsubObject = 0; currentsubObject < subobjectCount; currentsubObject++)
                {
                    surfaceObjects[surfaceChild] = new OK64.OK64F3DObject();
                    surfaceObjects[surfaceChild].objectName = surfaceNode.Children[currentsubObject].Name;
                    int vertCount = 0;
                    int faceCount = 0;
                    foreach (var childMesh in surfaceNode.Children[currentsubObject].Meshes)
                    {

                        vertCount = vertCount + fbx.Meshes[childMesh].NumVertices;
                        faceCount = faceCount + fbx.Meshes[childMesh].NumFaces;
                        surfacevertCount = surfacevertCount + vertCount;
                        surfacefaceCount = surfacefaceCount + faceCount;

                    }

                    surfaceObjects[surfaceChild].faceCount = faceCount;
                    surfaceObjects[surfaceChild].vertCount = vertCount;
                    surfaceObjects[surfaceChild].meshID = surfaceNode.Children[currentsubObject].Meshes.ToArray();

                    surfaceCount = surfaceCount + surfaceObjects[surfaceChild].meshID.Length;

                    surfaceObjects[surfaceChild].surfaceID = currentSection + 1;

                    string[] nameSplit = surfaceObjects[surfaceChild].objectName.Split('_');

                    surfaceObjects[surfaceChild].surfaceMaterial = Convert.ToInt32(nameSplit[0]);
                    surfaceObjects[surfaceChild].materialID = 0;
                    surfaceObjects[surfaceChild].flagA = false;
                    surfaceObjects[surfaceChild].flagB = false;
                    surfaceObjects[surfaceChild].flagC = false;


                    surfaceobjectBox.Items.Add(surfaceObjects[surfaceChild].objectName);
                    
                    surfaceChild++;
                }
            }


            
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
            updateTXDisplay();
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

        private void updateTXDisplay()
        {
            bitm.ImageLocation = textureArray[textureBox.SelectedIndex].texturePath;
            heightBox.Text = textureArray[textureBox.SelectedIndex].textureHeight.ToString();
            widthBox.Text = textureArray[textureBox.SelectedIndex].textureWidth.ToString();
            formatBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureFormat;
            classBox.SelectedIndex = textureArray[textureBox.SelectedIndex].textureClass;
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
            // or the material types 3-5. 


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
            Byte.TryParse(rtbox.Text, out skyColor[0]);
            colorUpdate();
        }

        private void Gtbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(gtbox.Text, out skyColor[1]);
            colorUpdate();
        }

        private void Btbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(btbox.Text, out skyColor[2]);
            colorUpdate();
        }

        private void Rbbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(rbbox.Text, out skyColor[3]);
            colorUpdate();
        }

        private void Gbbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(gbbox.Text, out skyColor[4]);
            colorUpdate();
        }

        private void Bbox_TextChanged(object sender, EventArgs e)
        {
            Byte.TryParse(bbbox.Text, out skyColor[5]);
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
            Color buttonColor = Color.FromArgb(rr, gg, bb);

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
            Color buttonColor = Color.FromArgb(rr, gg, bb);

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
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                romBox.Text = vertopen.FileName;
            }

        }

        private void PathBtn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                pathBox.Text = vertopen.FileName;
            }
        }

        private void PreviewBtn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                previewBox.Text = vertopen.FileName;
            }
        }

        private void BannerBtn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                bannerBox.Text = vertopen.FileName;
            }
        }

        private void MapBtn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                mapBox.Text = vertopen.FileName;
            }
        }

        private void AsmBtn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                asmBox.Text = vertopen.FileName;
            }
        }

        private void OutBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                outBox.Text = fb.SelectedPath;
            }
        }
    }
}




