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
using OverKart64_Library;

namespace OverKart64
{
    public partial class PathEditor : Form
    {
        public PathEditor()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        
        public class Offset
        {
            public List<int> offset { get; set; }
        }


        byte[] dataBytes = new byte[0];





        List<Offset> MKOffsets = new List<Offset>();

        OK64_Paths.Pathgroup[] coursePaths = new OK64_Paths.Pathgroup[20];

        

        int[] pathoffset = { 0x5568, 0x4480, 0x4F90, 0x4578, 0xD780, 0x34A0, 0xADE0, 0xB5B8, 0xA540, 0xEC80, 0x3B80, 0x6AC8, 0x4BF8, 0x1D90, 0x56A0, 0x71F0 };


        int cID = new int();
        bool loaded = false;
        int vertcount = new int();

        int[,] pathmarkers = new int[0, 0];


        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter binaryWriter = new BinaryWriter(Stream.Null);
        BinaryReader binaryReader = new BinaryReader(Stream.Null);


        OK64 mk = new OK64();
        OK64_Paths mkPath = new OK64_Paths();

        string filePath = "";

        byte[] flip4 = new byte[4];
        byte[] flip2 = new byte[2];

        int[] img_types = new int[] { 0, 0, 0, 3, 3, 3 };
        int[] img_heights = new int[] { 32, 32, 64, 32, 32, 64 };
        int[] img_widths = new int[] { 32, 64, 32, 32, 64, 32 };
        

        public static UInt32[] seg7_romptr = new UInt32[20];    // math derived from above data.

        OpenFileDialog fileOpen = new OpenFileDialog();
        SaveFileDialog fileSave = new SaveFileDialog();




        
        public void LoadPath()
        {
            
            cID = coursebox.SelectedIndex;



            byte[] fileData = File.ReadAllBytes(filePath);
            memoryStream = new MemoryStream(fileData);
            binaryReader = new BinaryReader(memoryStream);



            int[] markerCount = mkPath.loadmarkerCount(cID, fileData);
            int[] markerOffset = mkPath.loadmarkerOffsets(cID, fileData);



            byte[] seg6 = mk.dumpseg6(cID, fileData);
            seg6 = mk.decompressMIO0(seg6);
            

            memoryStream = new MemoryStream(seg6);
            binaryWriter= new BinaryWriter(memoryStream);
            binaryReader= new BinaryReader(memoryStream);



           

            for (int currentGroup = 0; currentGroup < 4; currentGroup++)
            {
                bool endpath = false;
                bool endlist = false;
                List<OK64_Paths.Pathlist> tempList = new List<OK64_Paths.Pathlist>();
                if (markerOffset[currentGroup] != -2146580616)   
                {
                    binaryReader.BaseStream.Position = markerOffset[currentGroup];

                    


                    for (int currentPath = 0; endlist == false; currentPath++)
                    {
                        endpath = false;

                        tempList.Add(new OK64_Paths.Pathlist());
                        tempList[currentPath].pathmarker = new List<OK64_Paths.Marker>();

                        for (int currentMarker = 0; (currentMarker < markerCount[currentGroup]) & endpath == false; currentMarker++)
                        {
                            int[] tempint = new int[4];

                            if (binaryReader.BaseStream.Position == binaryReader.BaseStream.Length)
                            {
                                endpath = true;
                            }
                            else
                            {

                                flip2 = BitConverter.GetBytes(binaryReader.ReadInt16());
                                Array.Reverse(flip2);
                                tempint[0] = BitConverter.ToInt16(flip2, 0);  //x

                                flip2 = BitConverter.GetBytes(binaryReader.ReadInt16());
                                Array.Reverse(flip2);
                                tempint[1] = BitConverter.ToInt16(flip2, 0);  //z

                                flip2 = BitConverter.GetBytes(binaryReader.ReadInt16());
                                Array.Reverse(flip2);
                                tempint[2] = BitConverter.ToInt16(flip2, 0);  //y 

                                flip2 = BitConverter.GetBytes(binaryReader.ReadUInt16());
                                Array.Reverse(flip2);
                                tempint[3] = BitConverter.ToUInt16(flip2, 0);
                                if (binaryReader.BaseStream.Position > 26400)
                                {

                                }
                                if (tempint[0] == -32768)
                                {
                                    endpath = true;
                                    if (tempint[1] == -32768)
                                    {
                                        endlist = true;
                                    }

                                }
                                else
                                {
                                    tempList[currentPath].pathmarker.Add(new OK64_Paths.Marker
                                    {
                                        xval = tempint[0],
                                        zval = tempint[1],
                                        yval = tempint[2],
                                        flag = tempint[3],
                                    });
                                }
                            }
                        }

                        tempList[currentPath].pathName = "path";

                    }
                }

                coursePaths[currentGroup] = new OK64_Paths.Pathgroup();
                coursePaths[currentGroup].pathList = tempList.ToArray();
                
            }
            //PATH OFFSET
            //Use below messagebox to find offset of end of path list. 
            //For reversing segment 6.
            //MessageBox.Show(cID.ToString() + "-- Offset " + binaryReader.BaseStream.Position.ToString());

            for(int currentGroup = 0; currentGroup < 4; currentGroup++)
            {   
                pathgroupbox.Items.Add("0x"+ markerOffset[currentGroup].ToString("X"));
            }
            pathgroupbox.SelectedIndex = 0;
            
        }







        private void Button2_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {


                //Get the path of specified file
                filePath = fileOpen.FileName;

                //Read the contents of the file into a stream

                string filetype = filePath.Substring(filePath.Length - 3);

                if (filetype == "n64" || filetype == "v64")
                {
                    MessageBox.Show("Only Supports .Z64 format");
                }
                else
                {


                    string[] items = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium", "Block Fort", "Skyscraper", "Double Decker", "DK's Jungle Parkway", "Big Donut" };


                    foreach (string item in items)
                    {
                        coursebox.Items.Add(item);

                    }

                    impbtn.Enabled = true;
                    expbtn.Enabled = true;
                    coursebox.SelectedIndex = 0;
                    coursebox.Enabled = true;
                    pathselect.Enabled = true;
                    markerselect.Enabled = true;
                    loaded = true;
                        
                    MessageBox.Show("ROM Loaded");
                       
                            
                    
                }
            }
            
        }


        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }

        private void Coursebox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPath();
        }

        private void Fbox_KeyUp(object sender, KeyEventArgs e)
        {
            int tempint = 0;
            if (loaded)
            {
                if (int.TryParse(fbox.Text, out tempint))
                {
                    coursePaths[pathgroupbox.SelectedIndex].pathList[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].flag = tempint;
                }
                
                
            }
        }

        private void Xbox_KeyUp(object sender, KeyEventArgs e)
        {
            int tempint = 0;
            if (loaded)
            {
                if (int.TryParse(xbox.Text, out tempint))
                {
                    coursePaths[pathgroupbox.SelectedIndex].pathList[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].xval = tempint;
                }


            }
        }

        private void Ybox_KeyUp(object sender, KeyEventArgs e)
        {
            int tempint = 0;
            if (loaded)
            {
                if (int.TryParse(ybox.Text, out tempint))
                {
                    coursePaths[pathgroupbox.SelectedIndex].pathList[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].yval = tempint;
                }


            }
        }

        private void Zbox_KeyUp(object sender, KeyEventArgs e)
        {
            int tempint = 0;
            if (loaded)
            {
                if (int.TryParse(zbox.Text, out tempint))
                {
                    coursePaths[pathgroupbox.SelectedIndex].pathList[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].zval = tempint;
                }


            }

        }

        

      
        

        private void expbtn_Click(object sender, EventArgs e)
        {
            cID = coursebox.SelectedIndex;
            fileSave.Filter = "OK64 3PL (.OK64.3PL)|*.OK64.3PL";

            if (fileSave.ShowDialog() == DialogResult.OK)
            {
                
                
                string savePath = fileSave.FileName;

                byte[] outputFile = mkPath.savePOP(coursePaths);

                File.WriteAllBytes(savePath, outputFile);

                MessageBox.Show("Finished");
                
            }
            fileSave.Filter = "";
        }



        private void Coursebox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            pathgroupbox.Items.Clear();
            LoadPath();
        }

        private void Pathselect_SelectedIndexChanged(object sender, EventArgs e)
        {
            markerselect.Items.Clear();
            int f = 0;

            if (coursePaths[pathgroupbox.SelectedIndex].pathList.Length > 0)
            {
                foreach (OK64_Paths.Marker mark in coursePaths[pathgroupbox.SelectedIndex].pathList[pathselect.SelectedIndex].pathmarker)
                {
                    markerselect.Items.Add("Marker" + f.ToString());
                    f = f + 1;
                }
            }
            else
            {
                markerselect.Items.Add("No Markers");
            }
            markerselect.SelectedIndex = 0;

        }

        private void Markerselect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(coursePaths[pathgroupbox.SelectedIndex].pathList.Length > 0)
            {
                if (coursePaths[pathgroupbox.SelectedIndex].pathList[pathselect.SelectedIndex].pathmarker.Count > 0)
                {
                    xbox.Text = coursePaths[pathgroupbox.SelectedIndex].pathList[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].xval.ToString();
                    ybox.Text = coursePaths[pathgroupbox.SelectedIndex].pathList[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].yval.ToString();
                    zbox.Text = coursePaths[pathgroupbox.SelectedIndex].pathList[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].zval.ToString();
                    fbox.Text = coursePaths[pathgroupbox.SelectedIndex].pathList[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].flag.ToString();
                }
                else
                {
                    xbox.Text = "";
                    ybox.Text = "";
                    zbox.Text = "";
                    fbox.Text = "";
                }
            }
            else
            {
                xbox.Text = "";
                ybox.Text = "";
                zbox.Text = "";
                fbox.Text = "";
            }
        }


        private void Pathgroupbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            pathselect.Items.Clear();
            int f = 0;
            
            
            foreach (OK64_Paths.Pathlist path in coursePaths[pathgroupbox.SelectedIndex].pathList)
            {
                pathselect.Items.Add("Path" + f.ToString());
                f = f + 1;
            }
            if (coursePaths[pathgroupbox.SelectedIndex].pathList.Length == 0)
            {
                pathselect.Items.Add("Empty");
            }
            pathselect.SelectedIndex = 0;
        }

        private void impbtn_Click(object sender, EventArgs e)
        {

            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                string popPath = fileOpen.FileName;

                OK64_Paths.Pathgroup[] importPath = mkPath.loadPOP(popPath);
                byte[] popData = mkPath.popMarkers(popPath);



                cID = coursebox.SelectedIndex;

                byte[] fileData = File.ReadAllBytes(filePath);

                byte[] seg6 = mk.dumpseg6(cID, fileData);
                seg6 = mk.decompressMIO0(seg6);


                memoryStream = new MemoryStream();

                memoryStream.Write(seg6, 0, seg6.Length);
                binaryWriter = new BinaryWriter(memoryStream);
                binaryReader = new BinaryReader(memoryStream);

                int[] markerCount = new int[4];
                markerCount[0] = importPath[0].pathList[0].pathmarker.Count;
                int[] popOffset = new int[4] { seg6.Length, 0, 0, 0 };               
                popOffset[0] = seg6.Length;
                int[] segmentOffset = new int[2];

                
                binaryWriter.Write(popData);

                seg6 = memoryStream.ToArray();


                memoryStream = new MemoryStream();
                memoryStream.Write(fileData, 0, fileData.Length);
                binaryWriter = new BinaryWriter(memoryStream);
                binaryReader = new BinaryReader(memoryStream);

                segmentOffset[0] = fileData.Length;

                byte[] compressedSegment6 = mk.compressMIO0(seg6);

                binaryWriter.Write(compressedSegment6);


                int addressAlign = 4 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 4);
                if (addressAlign == 4)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
                

                //0x800DC778

                fileData = memoryStream.ToArray();
                segmentOffset[1] = fileData.Length;


                fileData = mkPath.savemarkerCount(cID, fileData, markerCount);
                fileData = mkPath.savemarkerOffsets(cID, fileData, popOffset);


                memoryStream = new MemoryStream(fileData);
                binaryWriter = new BinaryWriter(memoryStream);
                binaryReader = new BinaryReader(memoryStream);

                binaryWriter.BaseStream.Position = (0x122390 + (0x30 * cID));

                byte[] segmentOffbyte = BitConverter.GetBytes(segmentOffset[0]);
                Array.Reverse(segmentOffbyte);
                binaryWriter.Write(segmentOffbyte);

                segmentOffbyte = BitConverter.GetBytes(segmentOffset[1]);
                Array.Reverse(segmentOffbyte);
                binaryWriter.Write(segmentOffbyte);

                if (fileSave.ShowDialog() == DialogResult.OK)
                {
                    string outputPath = fileSave.FileName;

                    File.WriteAllBytes(outputPath, fileData);

                }


            }
        }
    }
}

