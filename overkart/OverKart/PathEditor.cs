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

        public class Pathgroup
        {
            public List<Pathlist> pathlist { get; set; }
        }

        public class Pathlist
        {
            
            public List<Marker> pathmarker { get; set; }

           

        }

        public class Marker
        {

            public int xval { get; set; }
            public int yval { get; set; }
            public int zval { get; set; }
            public int flag { get; set; }

        }

        



        List<Offset> MKOffsets = new List<Offset>();

        List<Pathgroup> pathgroup = new List<Pathgroup>();

        

        int[] pathoffset = { 0x5568, 0x4480, 0x4F90, 0x4578, 0xD780, 0x34A0, 0xADE0, 0xB5B8, 0xA540, 0xEC80, 0x3B80, 0x6AC8, 0x4BF8, 0x1D90, 0x56A0, 0x71F0 };

        private void loadoffsets()
        {
            
            MKOffsets.Add(new Offset { });

            MKOffsets[0].offset = new List<int>();
            MKOffsets[0].offset.Add(0x5568);            
            MKOffsets.Add(new Offset { });
            MKOffsets[1].offset = new List<int>();
            MKOffsets[1].offset.Add(0x4480);
            MKOffsets.Add(new Offset { });
            MKOffsets[2].offset = new List<int>();
            MKOffsets[2].offset.Add(0x4F90);
            MKOffsets.Add(new Offset { });
            MKOffsets[3].offset = new List<int>();
            MKOffsets[3].offset.Add(0x4578);
            MKOffsets.Add(new Offset { });
            MKOffsets[4].offset = new List<int>();
            MKOffsets[4].offset.Add(0xD780);
            MKOffsets[4].offset.Add(0xD9C8);
            MKOffsets[4].offset.Add(0xDC18);
            MKOffsets[4].offset.Add(0xDEA8);            
            MKOffsets.Add(new Offset { });
            MKOffsets[5].offset = new List<int>();
            MKOffsets[5].offset.Add(0x34A0);
            MKOffsets.Add(new Offset { });
            MKOffsets[6].offset = new List<int>();
            MKOffsets[6].offset.Add(0xADE0);
            MKOffsets.Add(new Offset { });
            MKOffsets[7].offset = new List<int>();
            MKOffsets[7].offset.Add(0xB5B8);
            MKOffsets.Add(new Offset { });
            MKOffsets[8].offset = new List<int>();
            MKOffsets[8].offset.Add(0xA540);
            MKOffsets.Add(new Offset { });
            MKOffsets[9].offset = new List<int>();
            MKOffsets[9].offset.Add(0xEC80);
            MKOffsets.Add(new Offset { });
            MKOffsets[10].offset = new List<int>();
            MKOffsets[10].offset.Add(0x3B80);
            MKOffsets.Add(new Offset { });
            MKOffsets[11].offset = new List<int>();
            MKOffsets[11].offset.Add(0x6AC8);
            MKOffsets.Add(new Offset { });
            MKOffsets[12].offset = new List<int>();
            MKOffsets[12].offset.Add(0x4BF8);
            MKOffsets.Add(new Offset { });
            MKOffsets[13].offset = new List<int>();
            MKOffsets[13].offset.Add(0x1D90);
            MKOffsets.Add(new Offset { });
            MKOffsets[14].offset = new List<int>();
            MKOffsets[14].offset.Add(0x56A0);
            MKOffsets.Add(new Offset { });
            MKOffsets[15].offset = new List<int>();
            MKOffsets[15].offset.Add(0x71F0);
        }


        int cID = new int();
        bool loaded = false;
        int vertcount = new int();

        int[,] pathmarkers = new int[0, 0];


        FileStream fs = new FileStream("null", FileMode.OpenOrCreate);
        MemoryStream bs = new MemoryStream();
        BinaryReader br = new BinaryReader(Stream.Null);
        BinaryWriter bw = new BinaryWriter(Stream.Null);
        MemoryStream ds = new MemoryStream();
        BinaryReader dr = new BinaryReader(Stream.Null);
        BinaryWriter dw = new BinaryWriter(Stream.Null);
        MemoryStream vs = new MemoryStream();
        BinaryReader vr = new BinaryReader(Stream.Null);


        OK64 mk = new OK64();

        string filePath = "";
        string savePath = "";

        Int16 valuesign16 = new Int16();

        byte[] flip4 = new byte[4];
        byte[] flip2 = new byte[2];

        int[] img_types = new int[] { 0, 0, 0, 3, 3, 3 };
        int[] img_heights = new int[] { 32, 32, 64, 32, 32, 64 };
        int[] img_widths = new int[] { 32, 64, 32, 32, 64, 32 };
        public static UInt32[] seg6_addr = new UInt32[20];      //(0x00) ROM address at which segment 6 file begins
        public static UInt32[] seg6_end = new UInt32[20];       //(0x04) ROM address at which segment 6 file ends
        public static UInt32[] seg4_addr = new UInt32[20];      //(0x08) ROM address at which segment 4 file begins
        public static UInt32[] seg7_end = new UInt32[20];       //(0x0C) ROM address at which segment 7 (not 4) file ends
        public static UInt32[] seg9_addr = new UInt32[20];      //(0x10) ROM address at which segment 9 file begins
        public static UInt32[] seg9_end = new UInt32[20];       //(0x14) ROM address at which segment 9 file ends
        public static UInt32[] seg47_buf = new UInt32[20];      //(0x18) RSP address of compressed segments 4 and 7
        public static UInt32[] numVtxs = new UInt32[20];        //(0x1C) number of vertices in the vertex file
        public static UInt32[] seg7_ptr = new UInt32[20];       //(0x20) RSP address at which segment 7 data begins
        public static UInt32[] seg7_size = new UInt32[20];      //(0x24) Size of segment 7 data after decompression, minus 8 bytes for some reason
        public static UInt32[] texture_addr = new UInt32[20];   //(0x28) RSP address of texture list
        public static UInt16[] flag = new UInt16[20];           //(0x2C) Unknown
        public static UInt16[] unused = new UInt16[20];         //(0x2E) Padding

        public static UInt32[] seg7_romptr = new UInt32[20];    // math derived from above data.

        OpenFileDialog vertopen = new OpenFileDialog();
        SaveFileDialog vertsave = new SaveFileDialog();




        private void CloseStreams()
        {
            fs.Close();
            bs.Close();
            br.Close();
            bw.Close();
            ds.Close();
            dr.Close();
            dw.Close();
            vs.Close();
            vr.Close();
        }

        public void SaveMarkers()
        {
            if (vertsave.ShowDialog() == DialogResult.OK)
            {

                savePath = vertsave.FileName;

                cID = coursebox.SelectedIndex;

                byte[] seg6 = dump_segment_6();
                List<byte> tempbytes = mk.decompress_MIO0(0, seg6);
                seg6 = tempbytes.ToArray();

                bs = new MemoryStream(seg6);

                bw = new BinaryWriter(bs);
                br = new BinaryReader(bs);
                bool endpath = true;
                bool endlist = true;

                
                int x = 0;
                foreach (Pathgroup group in pathgroup)
                {


                    br.BaseStream.Position = MKOffsets[cID].offset[x];
                    for (int n = 0; n < pathgroup[x].pathlist.Count; n = n + 1)
                    {
                        endpath = true;



                        for (int i = 0; i < pathgroup[x].pathlist[n].pathmarker.Count; i = i + 1)
                        {



                            int[] tempint = new int[4];



                            flip2 = BitConverter.GetBytes(Convert.ToInt16(pathgroup[x].pathlist[n].pathmarker[i].xval));
                            Array.Reverse(flip2);
                            bw.Write(flip2);  //x

                            flip2 = BitConverter.GetBytes(Convert.ToInt16(pathgroup[x].pathlist[n].pathmarker[i].zval));
                            Array.Reverse(flip2);
                            bw.Write(flip2);  //z

                            flip2 = BitConverter.GetBytes(Convert.ToInt16(pathgroup[x].pathlist[n].pathmarker[i].yval));
                            Array.Reverse(flip2);
                            bw.Write(flip2);  //y 

                            flip2 = BitConverter.GetBytes(Convert.ToUInt16(pathgroup[x].pathlist[n].pathmarker[i].flag));
                            Array.Reverse(flip2);
                            bw.Write(flip2);  //flag






                        }
                    }
                    x = x + 1;
                }
                seg6 = mk.compress_MIO0(seg6, 0);
                File.WriteAllBytes(savePath, seg6);

                MessageBox.Show("Finished");
            }
        }

        public void LoadPath()
        {
            loadoffsets();
            cID = coursebox.SelectedIndex;

            byte[] seg6 = dump_segment_6();
            List<byte> tempbytes = mk.decompress_MIO0(0, seg6);
            seg6 = tempbytes.ToArray();

            bs = new MemoryStream(seg6);
            
            bw = new BinaryWriter(bs);
            br = new BinaryReader(bs);

            pathgroup.Clear();
            
            int x = 0;
            
            
            foreach (int off in MKOffsets[cID].offset)
            {
                bool endpath = true;
                bool endlist = true;
                br.BaseStream.Position = MKOffsets[cID].offset[x];

                pathgroup.Add(new Pathgroup { });
                pathgroup[x].pathlist = new List<Pathlist>();
                int n = 0;
                for (; endlist;)
                {
                    endpath = true;
                    
                    pathgroup[x].pathlist.Add(new Pathlist { });




                    pathgroup[x].pathlist[n].pathmarker = new List<Marker>();

                    for (int i = 0; endpath; i = i + 1)
                    {



                        int[] tempint = new int[4];



                        flip2 = BitConverter.GetBytes(br.ReadInt16());
                        Array.Reverse(flip2);
                        tempint[0] = BitConverter.ToInt16(flip2, 0);  //x

                        flip2 = BitConverter.GetBytes(br.ReadInt16());
                        Array.Reverse(flip2);
                        tempint[1] = BitConverter.ToInt16(flip2, 0);  //z

                        flip2 = BitConverter.GetBytes(br.ReadInt16());
                        Array.Reverse(flip2);
                        tempint[2] = BitConverter.ToInt16(flip2, 0);  //y 

                        flip2 = BitConverter.GetBytes(br.ReadUInt16());
                        Array.Reverse(flip2);
                        tempint[3] = BitConverter.ToUInt16(flip2, 0);
                        if (br.BaseStream.Position > 26400) {

                        }
                        if (tempint[0] == -32768)
                        {
                            endpath = false;
                            if (tempint[1] == -32768)
                            {
                                endlist = false;
                            }

                        }
                        else
                        {

                            pathgroup[x].pathlist[n].pathmarker.Add(new Marker
                            {
                                xval = tempint[0],
                                zval = tempint[1],
                                yval = tempint[2],
                                flag = tempint[3],


                            }
                            );

                        }


                    }
                    n = n + 1;
                }
                x = x + 1;
            }

            x = 0;
            foreach (Pathgroup group in pathgroup)
            {
                int f = 0;
                pathgroupbox.Items.Add("0x"+MKOffsets[cID].offset[x].ToString("X"));
                
                x = x + 1;
            }
            pathgroupbox.SelectedIndex = 0;
            
        }

        public byte[] dump_segment_6()
        {


            cID = coursebox.SelectedIndex;
            
            
            uint seg7_addr = (seg7_ptr[cID] - seg47_buf[cID]) + seg4_addr[cID];
            

            byte[] rombytes = File.ReadAllBytes(filePath);
            byte[] seg6 = new byte[(seg6_end[cID] - seg6_addr[cID])];

            Buffer.BlockCopy(rombytes, Convert.ToInt32(seg6_addr[cID]), seg6, 0, (Convert.ToInt32(seg6_end[cID]) - Convert.ToInt32(seg6_addr[cID])));

            CloseStreams();

            return seg6;
            
        }






        private void Button2_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            if (vertopen.ShowDialog() == DialogResult.OK)
            {


                //Get the path of specified file
                filePath = vertopen.FileName;

                //Read the contents of the file into a stream

                string filetype = filePath.Substring(filePath.Length - 3);

                if (filetype == "n64" || filetype == "v64")
                {
                    MessageBox.Show("Only Supports .Z64 format");
                }
                else
                {

                       



                    fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    bw = new BinaryWriter(fs);
                    br = new BinaryReader(fs);
                    {
                        br.BaseStream.Seek(1188752, SeekOrigin.Begin);

                        for (int i = 0; i < 20; i++)
                        {
                            seg6_addr[i] = br.ReadUInt32();
                            seg6_end[i] = br.ReadUInt32();
                            seg4_addr[i] = br.ReadUInt32();
                            seg7_end[i] = br.ReadUInt32();
                            seg9_addr[i] = br.ReadUInt32();
                            seg9_end[i] = br.ReadUInt32();
                            seg47_buf[i] = br.ReadUInt32();
                            numVtxs[i] = br.ReadUInt32();
                            seg7_ptr[i] = br.ReadUInt32();
                            seg7_size[i] = br.ReadUInt32();
                            texture_addr[i] = br.ReadUInt32();
                            flag[i] = br.ReadUInt16();
                            unused[i] = br.ReadUInt16();


                            byte[] flip = new byte[4];

                            flip = BitConverter.GetBytes(seg6_addr[i]);
                            Array.Reverse(flip);
                            seg6_addr[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg6_end[i]);
                            Array.Reverse(flip);
                            seg6_end[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg4_addr[i]);
                            Array.Reverse(flip);
                            seg4_addr[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg7_end[i]);
                            Array.Reverse(flip);
                            seg7_end[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg9_addr[i]);
                            Array.Reverse(flip);
                            seg9_addr[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg9_end[i]);
                            Array.Reverse(flip);
                            seg9_end[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg47_buf[i]);
                            Array.Reverse(flip);
                            seg47_buf[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(numVtxs[i]);
                            Array.Reverse(flip);
                            numVtxs[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg7_ptr[i]);
                            Array.Reverse(flip);
                            seg7_ptr[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(seg7_size[i]);
                            Array.Reverse(flip);
                            seg7_size[i] = BitConverter.ToUInt32(flip, 0);

                            flip = BitConverter.GetBytes(texture_addr[i]);
                            Array.Reverse(flip);
                            texture_addr[i] = BitConverter.ToUInt32(flip, 0);

                            byte[] flop = new byte[2];

                            flop = BitConverter.GetBytes(flag[i]);
                            Array.Reverse(flip);
                            flag[i] = BitConverter.ToUInt16(flip, 0);

                            flop = BitConverter.GetBytes(unused[i]);
                            Array.Reverse(flip);
                            unused[i] = BitConverter.ToUInt16(flip, 0);



                            seg7_romptr[i] = seg7_ptr[i] - seg47_buf[i] + seg4_addr[i];

                        }



                        CloseStreams();
                        string[] items = { "Mario Raceway", "Choco Mountain", "Bowser's Castle", "Banshee Boardwalk", "Yoshi Valley", "Frappe Snowland", "Koopa Troopa Beach", "Royal Raceway", "Luigi's Turnpike", "Moo Moo Farm", "Toad's Turnpike", "Kalimari Desert", "Sherbet Land", "Rainbow Road", "Wario Stadium","DK's Jungle Parkway"};
                        
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
                CloseStreams();
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
                    pathgroup[pathgroupbox.SelectedIndex].pathlist[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].flag = tempint;
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
                    pathgroup[pathgroupbox.SelectedIndex].pathlist[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].xval = tempint;
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
                    pathgroup[pathgroupbox.SelectedIndex].pathlist[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].yval = tempint;
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
                    pathgroup[pathgroupbox.SelectedIndex].pathlist[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].zval = tempint;
                }


            }

        }

        

      
        

        private void Button2_Click_1(object sender, EventArgs e)
        {
            if (loaded)
            {
                if (vertsave.ShowDialog() == DialogResult.OK)
                {

                    savePath = vertsave.FileName;

                    ds = new MemoryStream();
                    dw = new BinaryWriter(ds);
                    for (int i = 0; i < vertcount; i++)
                    {
                       // flip2 = BitConverter.GetBytes(Convert.ToInt16(xcor[i]));
                        Array.Reverse(flip2);
                        dw.Write(BitConverter.ToInt16(flip2, 0));

                       // flip2 = BitConverter.GetBytes(Convert.ToInt16(zcor[i]));
                        Array.Reverse(flip2);
                        dw.Write(BitConverter.ToInt16(flip2, 0));

                       // flip2 = BitConverter.GetBytes(Convert.ToInt16(ycor[i]));
                        Array.Reverse(flip2);
                        dw.Write(BitConverter.ToInt16(flip2, 0));

                       // flip2 = BitConverter.GetBytes(Convert.ToInt16(scor[i]));
                        Array.Reverse(flip2);
                        dw.Write(BitConverter.ToInt16(flip2, 0));

                       // flip2 = BitConverter.GetBytes(Convert.ToInt16(tcor[i]));
                        Array.Reverse(flip2);
                        dw.Write(BitConverter.ToInt16(flip2, 0));

                     
                        
                    }
                    byte[] seg4 = ds.ToArray();
                    File.WriteAllBytes(savePath, seg4);

                    MessageBox.Show("Finished");

                }
            }
        }

        private void Impbtn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {

                string importpath = vertopen.FileName;

                string[] reader = File.ReadAllLines(importpath);
                string[] positions = new string[3];


                int n = -1;
                int x = -1;
                int i = 0;
                for (int m = 0; m < reader.Length; m++)
                {
                    if (reader[m] == "NEW GROUP")
                    {
                        x = x + 1;
                        n = -1;
                        i = 0;
                    }
                    else
                    {


                        if (reader[m] == "NEW PATH")
                        {
                            n = n + 1;
                            i = 0;
                        }
                        else
                        {

                            positions = reader[m].Split(',').ToArray();
                            pathgroup[x].pathlist[n].pathmarker[i].xval = Convert.ToInt32(Convert.ToDouble(positions[0]));
                            pathgroup[x].pathlist[n].pathmarker[i].yval = Convert.ToInt32(Convert.ToDouble(positions[1]));
                            pathgroup[x].pathlist[n].pathmarker[i].zval = Convert.ToInt32(Convert.ToDouble(positions[2]));
                            i = i + 1;
                        }
                    }
                    
                }




            }
        }

        private void expbtn_Click(object sender, EventArgs e)
        {
            cID = coursebox.SelectedIndex;
            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                
                
                savePath = vertsave.FileName;
                System.IO.File.AppendAllText(savePath, pathgroup.Count() + Environment.NewLine);
                int x = 0;
                foreach (Pathgroup group in pathgroup)
                {
                    int n = 0;
                    System.IO.File.AppendAllText(savePath, "NEW GROUP" + Environment.NewLine);
                    System.IO.File.AppendAllText(savePath, "GROUP " + x.ToString()+ Environment.NewLine);
                    System.IO.File.AppendAllText(savePath, pathgroup[x].pathlist.Count() + Environment.NewLine);
                    foreach (Pathlist path in pathgroup[x].pathlist)
                    {
                        
                        int i = 0;
                        System.IO.File.AppendAllText(savePath, "NEW PATH" + Environment.NewLine);
                        System.IO.File.AppendAllText(savePath, "PATH " + n.ToString() + Environment.NewLine);
                        
                        foreach (Marker mark in pathgroup[x].pathlist[n].pathmarker)
                        {

                            System.IO.File.AppendAllText(savePath, pathgroup[x].pathlist[n].pathmarker[i].xval.ToString() + Environment.NewLine);
                            System.IO.File.AppendAllText(savePath, pathgroup[x].pathlist[n].pathmarker[i].yval.ToString() + Environment.NewLine);
                            System.IO.File.AppendAllText(savePath, pathgroup[x].pathlist[n].pathmarker[i].zval.ToString() + Environment.NewLine);
                            System.IO.File.AppendAllText(savePath, pathgroup[x].pathlist[n].pathmarker[i].flag.ToString() + Environment.NewLine);
                            i = i + 1;
                        }
                        n = n + 1;
                    }
                    x = x + 1;
                }
                MessageBox.Show("Finished");
                CloseStreams();
                
            }
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
            
            
            foreach (Marker mark in pathgroup[pathgroupbox.SelectedIndex].pathlist[pathselect.SelectedIndex].pathmarker)
            {
                markerselect.Items.Add("Marker" + f.ToString());
                f = f + 1;
            }
            markerselect.SelectedIndex = 0;

        }

        private void Markerselect_SelectedIndexChanged(object sender, EventArgs e)
        {
            xbox.Text = pathgroup[pathgroupbox.SelectedIndex].pathlist[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].xval.ToString();
            ybox.Text = pathgroup[pathgroupbox.SelectedIndex].pathlist[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].yval.ToString();
            zbox.Text = pathgroup[pathgroupbox.SelectedIndex].pathlist[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].zval.ToString();
            fbox.Text = pathgroup[pathgroupbox.SelectedIndex].pathlist[pathselect.SelectedIndex].pathmarker[markerselect.SelectedIndex].flag.ToString();
        }

        private void dumpseg6_btn(object sender, EventArgs e)
        {
            SaveMarkers();
        }

        private void Pathgroupbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            pathselect.Items.Clear();
            int f = 0;
            
            
            foreach (Pathlist path in pathgroup[pathgroupbox.SelectedIndex].pathlist)
            {
                pathselect.Items.Add("Path" + f.ToString());
                f = f + 1;
            }
            pathselect.SelectedIndex = 0;
        }

        
    }
}

