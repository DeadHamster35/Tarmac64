using System;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PeepsCompress;
using System.Diagnostics;


namespace OverKart64
{





    public partial class RacerEditor : Form
    {
        string settings_path = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "PorkChop.txt");

        int debugmode = 0;
        float[] cc50 = new float[8];
        float[] cc100 = new float[8];
        float[] cc150 = new float[8];
        float[] ccextra = new float[8];
        float[] ccbattle = new float[8];
        float[] weigh = new float[8];
        float[,] acceler = new float[8, 10];
        float[] steerin = new float[8];
        float[] friction = new float[8];
        float[] gravity = new float[8];
        float[] jump = new float[8];
        float[] fall = new float[8];
        float[] top = new float[8];
        float[] tcoa = new float[8];
        float[] tcob = new float[8];
        float[] bbox = new float[8];


        bool loaded = new bool();




        int mkversion = new int();
        bool overwrite = new bool();


        string filePath = "";


        OpenFileDialog romopen = new OpenFileDialog();



        public RacerEditor()
        {
            InitializeComponent();
        }







        private void OverKart_Load(object sender, EventArgs e)
        {
            

            string[] items = { "Mario", "Luigi", "Yoshi", "Toad", "DK", "Wario", "Peach", "Bowser" };

            foreach (string item in items)
            {
                racer.Items.Add(item);
            }
        }



        private void Load_Click(object sender, EventArgs e)
        {
            




            if (romopen.ShowDialog() == DialogResult.OK)
            {

                racer.Enabled = true;
                save.Enabled = true;
                export.Enabled = true;


                //Get the path of specified file
                filePath = romopen.FileName;

                //Read the contents of the file into a stream

                string filetype = filePath.Substring(filePath.Length - 3);

                if (filetype == "n64" || filetype == "v64")
                {
                    MessageBox.Show("Only Supports .Z64 format");
                }
                else
                {
                    using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var ms = new MemoryStream())
                    using (var bw = new BigEndianBinaryWriter(ms))
                    using (var br = new BigEndianBinaryReader(ms))

                    {
                            fs.CopyTo(ms);
                            ms.Position = 0;
                        

                        

                        //0   MARIO
                        //1   LUIGI
                        //2   YOSHI
                        //3   TOAD
                        //4   DK
                        //5   WARIO
                        //6   PEACH
                        //7   BOWSER

                        br.BaseStream.Seek(932560, SeekOrigin.Begin);

                        

                        for (int i = 0; i != 8; i++)
                        {
                            for (int g = 0; g != 10; g++)
                            {
                                acceler[i, g] = br.ReadSingle();
                            }

                        }


                        br.BaseStream.Seek(930172, SeekOrigin.Begin);


                        for (int i = 0; i != 8; i++)
                        {
                            cc50[i] = br.ReadSingle();


                        }
                        for (int i = 0; i != 8; i++)
                        {
                            cc100[i] = br.ReadSingle();


                        }
                        for (int i = 0; i != 8; i++)
                        {
                            cc150[i] = br.ReadSingle();


                        }
                        for (int i = 0; i != 8; i++)
                        {
                            ccextra[i] = br.ReadSingle();


                        }
                        for (int i = 0; i != 8; i++)
                        {
                            ccbattle[i] = br.ReadSingle();


                        }

                        br.BaseStream.Seek(930448, SeekOrigin.Begin);

                        

                        for (int i = 0; i != 8; i++)
                        {
                            top[i] = br.ReadSingle();

                        }

                        for (int i = 0; i != 8; i++)
                        {
                            bbox[i] = br.ReadSingle();

                        }

                        br.BaseStream.Seek(930352, SeekOrigin.Begin);

                        

                        for (int i = 0; i != 8; i++)
                        {
                            friction[i] = br.ReadSingle();

                        }

                        br.BaseStream.Seek(934448, SeekOrigin.Begin);

                        

                        for (int i = 0; i != 8; i++)
                        {
                            steerin[i] = br.ReadSingle();

                        }


                        br.BaseStream.Seek(930416, SeekOrigin.Begin);

                        
                        for (int i = 0; i != 8; i++)
                        {
                            gravity[i] = br.ReadSingle();

                        }


                        br.BaseStream.Seek(934512, SeekOrigin.Begin);

                        

                        for (int i = 0; i != 8; i++)
                        {
                            tcoa[i] = br.ReadSingle();

                        }

                        for (int i = 0; i != 8; i++)
                        {
                            tcob[i] = br.ReadSingle();

                        }





                        br.BaseStream.Seek(934608, SeekOrigin.Begin);

                        

                        for (int i = 0; i != 8; i++)
                        {
                            jump[i] = br.ReadSingle();

                        }

                        br.BaseStream.Seek(934640, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            fall[i] = br.ReadSingle();

                        }




                        br.BaseStream.Seek(1187232, SeekOrigin.Begin);

                        

                        for (int i = 0; i != 8; i++)
                        {
                            weigh[i] = br.ReadSingle();

                        }


                        
                        //DataFlip();   // flips the endian of each data value
                        


                        br.BaseStream.Seek(1187232, SeekOrigin.Begin);

                        fiftycc.Enabled = true;
                        hundocc.Enabled = true;
                        hundofiftycc.Enabled = true;
                        extracc.Enabled = true;
                        battlecc.Enabled = true;
                        topspeed.Enabled = true;
                        weight.Enabled = true;
                        steering.Enabled = true;
                        frict.Enabled = true;
                        grav.Enabled = true;
                        jumper.Enabled = true;
                        faller.Enabled = true;
                        aa.Enabled = true;
                        ab.Enabled = true;
                        ac.Enabled = true;
                        ad.Enabled = true;
                        ae.Enabled = true;
                        af.Enabled = true;
                        ag.Enabled = true;
                        ah.Enabled = true;
                        ai.Enabled = true;
                        aj.Enabled = true;
                        bbbox.Enabled = true;
                        tcoboxa.Enabled = true;
                        tcoboxb.Enabled = true;




                        fiftycc.Text = cc50[0].ToString();
                        hundocc.Text = cc100[0].ToString();
                        hundofiftycc.Text = cc150[0].ToString();
                        extracc.Text = ccextra[0].ToString();
                        battlecc.Text = ccbattle[0].ToString();
                        topspeed.Text = top[0].ToString();

                        weight.Text = weigh[0].ToString();
                        steering.Text = steerin[0].ToString();
                        frict.Text = friction[0].ToString();
                        grav.Text = gravity[0].ToString();
                        jumper.Text = jump[0].ToString();
                        faller.Text = fall[0].ToString();
                        tcoboxa.Text = tcoa[0].ToString();
                        tcoboxb.Text = tcob[0].ToString();
                        bbbox.Text = bbox[0].ToString();


                        aa.Text = acceler[0, 0].ToString();
                        ab.Text = acceler[0, 1].ToString();
                        ac.Text = acceler[0, 2].ToString();
                        ad.Text = acceler[0, 3].ToString();
                        ae.Text = acceler[0, 4].ToString();
                        af.Text = acceler[0, 5].ToString();
                        ag.Text = acceler[0, 6].ToString();
                        ah.Text = acceler[0, 7].ToString();
                        ai.Text = acceler[0, 8].ToString();
                        aj.Text = acceler[0, 9].ToString();




                        racer.SelectedIndex = 0;
                        loaded = true;

                        br.Close();
                        bw.Close();
                        ms.Close();
                        fs.Close();


                    }
                }


            }
        }

        private void Racer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = racer.SelectedIndex;
            fiftycc.Text = cc50[i].ToString();
            hundocc.Text = cc100[i].ToString();
            hundofiftycc.Text = cc150[i].ToString();
            extracc.Text = ccextra[i].ToString();
            battlecc.Text = ccbattle[i].ToString();
            topspeed.Text = top[i].ToString();

            weight.Text = weigh[i].ToString();
            steering.Text = steerin[i].ToString();
            frict.Text = friction[i].ToString();
            grav.Text = gravity[i].ToString();
            jumper.Text = jump[i].ToString();
            faller.Text = fall[i].ToString();
            tcoboxa.Text = tcoa[i].ToString();
            tcoboxb.Text = tcob[i].ToString();
            bbbox.Text = bbox[i].ToString();


            aa.Text = acceler[i, 0].ToString();
            ab.Text = acceler[i, 1].ToString();
            ac.Text = acceler[i, 2].ToString();
            ad.Text = acceler[i, 3].ToString();
            ae.Text = acceler[i, 4].ToString();
            af.Text = acceler[i, 5].ToString();
            ag.Text = acceler[i, 6].ToString();
            ah.Text = acceler[i, 7].ToString();
            ai.Text = acceler[i, 8].ToString();
            aj.Text = acceler[i, 9].ToString();
        }

        private void Label23_Click(object sender, EventArgs e)
        {

        }

        private void Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog textsave = new SaveFileDialog();

            if (textsave.ShowDialog() == DialogResult.OK)
            {
                string listfile = textsave.FileName;
                string writetext = "";


                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - 50CC -" + cc50[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - 100CC -" + cc100[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - 150CC -" + cc150[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Extra -" + ccextra[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Battle -" + ccbattle[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }
                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Top Speed -" + top[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Weight -" + weigh[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }
                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Steering -" + steerin[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }
                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Friction -" + friction[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Gravity -" + gravity[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Jump Height -" + jump[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Fall Speed -" + fall[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Turn Speed Reduction Coefficient A  -" + tcoa[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Turn Speed Reduction Coefficient B -" + tcob[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Bounding Box -" + bbox[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i != 8; i++)
                {

                    writetext = racer.Items[i] + " - Acceleration -" + acceler[i, 0].ToString() + "," + acceler[i, 1].ToString() + "," + acceler[i, 2].ToString() + "," + acceler[i, 3].ToString() + "," + acceler[i, 4].ToString() + "," + acceler[i, 5].ToString() + "," + acceler[i, 6].ToString() + "," + acceler[i, 7].ToString() + "," + acceler[i, 8].ToString() + "," + acceler[i, 9].ToString() + ",";
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);

                }


            }
        }

        private void Save_Click(object sender, EventArgs e)
        {


            SaveFileDialog romsave = new SaveFileDialog();

            if (romsave.ShowDialog() == DialogResult.OK)
            {

                string rompath = romsave.FileName;
                if (rompath == filePath)
                {
                    overwrite = true;
                    rompath = "null";
                }


                string filetype = filePath.Substring(filePath.Length - 3);
                if (filetype == "n64" || filetype == "v64")
                {


                    MessageBox.Show("Only Supports .Z64 format");

                }
                else
                {


                    //DataFlip();   // flips the endian of each data value


                   
                    using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var ds = new FileStream(rompath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var ms = new MemoryStream())
                    using (var bw = new BigEndianBinaryWriter(ms))
                    using (var bs = new BigEndianBinaryWriter (ms))
                    using (var br = new BigEndianBinaryReader(ms))

                    {
                        fs.CopyTo(ms);

                        ms.Position = 0;


                        br.BaseStream.Seek(16, SeekOrigin.Begin);
                        mkversion = br.ReadInt16();

                        if (mkversion == 15952)
                        {
                            mkversion = 1;
                        }

                        if (mkversion == 9591)
                        {
                            mkversion = 2;
                        }

                        /*

                        br.BaseStream.Seek(738596, SeekOrigin.Begin);
                        if (enhance.Checked == true)
                        {
                            enhancedint16 = 9216;
                            bw.Write(enhancedint16);
                        }
                        else
                        {
                            enhancedint16 = 4160;
                            bw.Write(enhancedint16);
                        }

                        br.BaseStream.Seek(16, SeekOrigin.Begin);
                        if (enhance.Checked == true)
                        {
                            enhancedint16 = 32767;
                            bw.Write(enhancedint16);
                        }
                        else
                        {
                            enhancedint16 = -32768;
                            bw.Write(enhancedint16);
                        }




                        br.BaseStream.Seek(738724, SeekOrigin.Begin);
                        if (enhance.Checked == true)
                        {
                            enhancedint16 = 9216;
                            bw.Write(enhancedint16);
                        }
                        else
                        {
                            enhancedint16 = 4160;
                            bw.Write(enhancedint16);
                        }


                        br.BaseStream.Seek(16, SeekOrigin.Begin);
                        if (enhance.Checked == true)
                        {
                            enhancedint16 = 32767;
                            bw.Write(enhancedint16);
                        }
                        else
                        {
                            enhancedint16 = -32768;
                            bw.Write(enhancedint16);
                        }


                        br.BaseStream.Seek(738872, SeekOrigin.Begin);
                        if (enhance.Checked == true)
                        {
                            enhancedint16 = 9216;
                            bw.Write(enhancedint16);
                        }
                        else
                        {
                            enhancedint16 = 4160;
                            bw.Write(enhancedint16);
                        }


                        br.BaseStream.Seek(20, SeekOrigin.Begin);
                        if (enhance.Checked == true)
                        {
                            enhancedint16 = 32767;
                            bw.Write(enhancedint16);
                        }
                        else
                        {
                            enhancedint16 = -32768;
                            bw.Write(enhancedint16);
                        }

                        */




                        bw.BaseStream.Seek(932560, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            bw.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            for (int g = 0; g != 10; g++)
                            {
                                if (equalbox.Checked == true)
                                {                                    
                                    bw.Write(acceler[0, g]);
                                }
                                else
                                {
                                    bw.Write(acceler[i, g]);
                                }
                            }

                        }


                        bw.BaseStream.Seek(930172, SeekOrigin.Begin);


                        if (mkversion == 2)
                        {
                            bw.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            if (equalbox.Checked == true)
                            {
                                bw.Write(cc50[0]);
                            }
                            else
                            {
                                bw.Write(cc50[i]);
                            }



                        }
                        for (int i = 0; i != 8; i++)
                        {
                            if (equalbox.Checked == true)
                            {
                                bw.Write(cc100[0]);
                            }
                            else
                            {
                                bw.Write(cc100[i]);
                            }


                        }
                        for (int i = 0; i != 8; i++)
                        {

                            if (equalbox.Checked == true)
                            {
                                bw.Write(cc150[0]);
                            }
                            else
                            {
                                bw.Write(cc150[i]);
                            }


                        }
                        for (int i = 0; i != 8; i++)
                        {

                            if (equalbox.Checked == true)
                            {
                                bw.Write(ccextra[0]);
                            }
                            else
                            {
                                bw.Write(ccextra[i]);
                            }


                        }
                        for (int i = 0; i != 8; i++)
                        {

                            if (equalbox.Checked == true)
                            {
                                bw.Write(ccbattle[0]);
                            }
                            else
                            {
                                bw.Write(ccbattle[i]);
                            }


                        }

                            bw.BaseStream.Seek(930448, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            bw.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {

                            if (equalbox.Checked == true)
                            {
                                bw.Write(top[0]);
                            }
                            else
                            {
                                bw.Write(top[i]);
                            }
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            if (equalbox.Checked == true)
                            {
                                bw.Write(bbox[0]);
                            }
                            else
                            {
                                bw.Write(bbox[i]);
                            }
                        }

                        bw.BaseStream.Seek(930352, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            if (equalbox.Checked == true)
                            {
                                bw.Write(friction[0]);
                            }
                            else
                            {
                                bw.Write(friction[i]);
                            }

                        }

                            bw.BaseStream.Seek(934448, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                         {

                            if (equalbox.Checked == true)
                            {
                                bw.Write(steerin[0]);
                            }
                            else
                            {
                                bw.Write(steerin[i]);
                            }

                         }


                            bw.BaseStream.Seek(930416, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                         {

                            if (equalbox.Checked == true)
                            {
                                bw.Write(gravity[0]);
                            }
                            else
                            {
                                bw.Write(gravity[i]);
                            }

                         }


                        bs.BaseStream.Seek(934512, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            if (equalbox.Checked == true)
                            {
                                
                                bs.Write(tcoa[0]);
                            }
                            else
                            {
                                
                                bs.Write(tcoa[i]);
                            }
                        }

                        for (int i = 0; i != 8; i++)
                        {

                            if (equalbox.Checked == true)
                            {
                                bw.Write(tcob[0]);
                            }
                            else
                            {
                                bw.Write(tcob[i]);
                            }
                        }


                            br.BaseStream.Seek(934608, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                            {

                            if (equalbox.Checked == true)
                            {
                                bw.Write(jump[0]);
                            }
                            else
                            {
                                bw.Write(jump[i]);
                            }

                            }

                            br.BaseStream.Seek(934640, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                            {

                            if (equalbox.Checked == true)
                            {
                                bw.Write(fall[0]);
                            }
                            else
                            {
                                bw.Write(fall[i]);
                            }

                            }

                        br.BaseStream.Seek(1016511, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(96, SeekOrigin.Current);
                        }

                        

                        br.BaseStream.Seek(1019879, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(96, SeekOrigin.Current);
                        }

                        



                        br.BaseStream.Seek(1187232, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(256, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {

                            if (equalbox.Checked == true)
                            {
                                bw.Write(weigh[0]);
                            }
                            else
                            {
                                bw.Write(weigh[i]);
                            }

                        }

                        




                        fs.Position = 0;
                            ds.Position = 0;
                            ms.Position = 0;
                            if (overwrite == true)
                            {
                                ms.CopyTo(fs);
                            }
                            else
                            {
                                ms.CopyTo(ds);
                            }

                            br.Close();
                            bw.Close();
                            ms.Close();
                            fs.Close();
                            ds.Close();


                            string command = @"rn64crc.exe";
                            string args = "";
                            if (overwrite == true)
                            {
                                args = "-u \"" + filePath + "\"";
                            }
                            else
                            {
                                args = "-u \"" + rompath + "\"";
                            }


                            Process process = new Process();
                            process.StartInfo.FileName = command;
                            process.StartInfo.Arguments = args;
                            process.StartInfo.CreateNoWindow = true;
                            process.Start();


                            MessageBox.Show("Save Completed");
                        }

                    }
            }
        }



        private void DataFlip()
        {

            byte[] flip = new byte[4];

            // c# binary writer defaults to the endian setup of the system running it. 
            // so values read or written are always little endian. However, MK64 stats are
            // written in Big Endian. This requires us to do a bit of extra work to flip byte order.
            // set all the values to little endian to be written properly by the writer.
            for (int i = 0; i != 8; i++)
            {
                
                flip = BitConverter.GetBytes(cc50[i]);
                Array.Reverse(flip);
                cc50[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int i = 0; i != 8; i++)
            {
                
                flip = BitConverter.GetBytes(cc100[i]);
                Array.Reverse(flip);
                cc100[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int i = 0; i != 8; i++)
            {
                
                flip = BitConverter.GetBytes(cc150[i]);
                Array.Reverse(flip);
                cc150[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int i = 0; i != 8; i++)
            {
                
                flip = BitConverter.GetBytes(ccextra[i]);
                Array.Reverse(flip);
                ccextra[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int i = 0; i != 8; i++)
            {
                
                flip = BitConverter.GetBytes(ccbattle[i]);
                Array.Reverse(flip);
                ccbattle[i] = BitConverter.ToSingle(flip, 0);
            }
            for (int i = 0; i != 8; i++)
            {
                flip = BitConverter.GetBytes(top[i]);
                Array.Reverse(flip);
                top[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int i = 0; i != 8; i++)
            {
                flip = BitConverter.GetBytes(weigh[i]);
                Array.Reverse(flip);
                weigh[i] = BitConverter.ToSingle(flip, 0);
            }
            for (int i = 0; i != 8; i++)
            {
                flip = BitConverter.GetBytes(steerin[i]);
                Array.Reverse(flip);
                steerin[i] = BitConverter.ToSingle(flip, 0);
            }
            for (int i = 0; i != 8; i++)
            {
                flip = BitConverter.GetBytes(friction[i]);
                Array.Reverse(flip);
                friction[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int i = 0; i != 8; i++)
            {
                flip = BitConverter.GetBytes(gravity[i]);
                Array.Reverse(flip);
                gravity[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int i = 0; i != 8; i++)
            {
                flip = BitConverter.GetBytes(jump[i]);
                Array.Reverse(flip);
                jump[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int i = 0; i != 8; i++)
            {
                flip = BitConverter.GetBytes(fall[i]);
                Array.Reverse(flip);
                fall[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int i = 0; i != 8; i++)
            {
                flip = BitConverter.GetBytes(tcoa[i]);
                Array.Reverse(flip);
                tcoa[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int i = 0; i != 8; i++)
            {
                flip = BitConverter.GetBytes(tcob[i]);
                Array.Reverse(flip);
                tcob[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int i = 0; i != 8; i++)
            {
                flip = BitConverter.GetBytes(bbox[i]);
                Array.Reverse(flip);
                bbox[i] = BitConverter.ToSingle(flip, 0);
            }

            for (int n = 0; n != 8; n++)
            {
                for (int i = 0; i != 10; i++)
                {
                    flip = BitConverter.GetBytes(acceler[n, i]);
                    Array.Reverse(flip);
                    acceler[n, i] = BitConverter.ToSingle(flip, 0);
                }

            }


            // finish fixing bytes
            // finish fixing bytes
            // finish fixing bytes
            // finish fixing bytes

        }


        private void Fiftycc_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(fiftycc.Text, out cc50[racer.SelectedIndex]);
        }

        private void Hundocc_TextChanged(object sender, EventArgs e)
        {
         
            Single.TryParse(hundocc.Text, out cc100[racer.SelectedIndex]);
        }

        private void Hundofiftycc_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(hundofiftycc.Text, out cc150[racer.SelectedIndex]);
        }

        private void Extracc_TextChanged(object sender, EventArgs e)        {
            
            Single.TryParse(extracc.Text, out ccextra[racer.SelectedIndex]);
        }

        private void Battlecc_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(battlecc.Text, out ccbattle[racer.SelectedIndex]);
        }

        private void Topspeed_TextChanged(object sender, EventArgs e)
        {
            
            
        }

        private void Frict_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(frict.Text, out friction[racer.SelectedIndex]);
        }

        private void Weight_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(weight.Text, out weigh[racer.SelectedIndex]);
        }

        

        private void Jumper_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(jumper.Text, out jump[racer.SelectedIndex]);
        }

        private void Faller_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(faller.Text, out fall[racer.SelectedIndex]);
        }

        private void Aa_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(aa.Text, out acceler[racer.SelectedIndex,0]);
        }

        private void Ab_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(ab.Text, out acceler[racer.SelectedIndex, 1]);
        }

        private void Ac_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(ac.Text, out acceler[racer.SelectedIndex, 2]);
        }

        private void Ad_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(ad.Text, out acceler[racer.SelectedIndex, 3]);
        }

        private void Ae_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(ae.Text, out acceler[racer.SelectedIndex, 4]);
        }

        private void Af_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(af.Text, out acceler[racer.SelectedIndex, 5]);
        }

        private void Ag_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(ag.Text, out acceler[racer.SelectedIndex, 6]);
        }

        private void Ah_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(ah.Text, out acceler[racer.SelectedIndex, 7]);
        }

        private void Ai_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(ai.Text, out acceler[racer.SelectedIndex, 8]);
        }
        private void Aj_TextChanged(object sender, EventArgs e)
        {
            
            Single.TryParse(aj.Text, out acceler[racer.SelectedIndex, 9]);
        }

        

       

      


    

        private void Equalbox_CheckedChanged(object sender, EventArgs e)
        {
            if (equalbox.Checked == true)
            {
                racer.SelectedIndex = 0;
                racer.Enabled = false;                
            }
            else
            {
                racer.Enabled = true;
            }
        }

        private void OverKart_KeyUp(object sender, KeyEventArgs e)
        {
            if (racer.Enabled == true)
            {
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Up)
                {
                    if (racer.SelectedIndex > 0)
                    {
                        racer.SelectedIndex = racer.SelectedIndex - 1;
                    }
                }
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Down)
                {
                    if (racer.SelectedIndex < 7)
                    {
                        racer.SelectedIndex = racer.SelectedIndex + 1;
                    }
                }
            }
        }

        private void Topspeed_TextChanged(object sender, KeyEventArgs e)
        {
            Single.TryParse(topspeed.Text, out top[racer.SelectedIndex]);
        }

        private void Grav_TextChanged(object sender, KeyEventArgs e)
        {
            Single.TryParse(grav.Text, out gravity[racer.SelectedIndex]);
        }

        private void Steering_TextChanged(object sender, KeyEventArgs e)
        {
            Single.TryParse(steering.Text, out steerin[racer.SelectedIndex]);
        }

        private void Equalbox_CheckedChanged_1(object sender, EventArgs e)
        {
            
        }
        private void Tcoboxa_KeyUp(object sender, KeyEventArgs e)
        {
            Single.TryParse(tcoboxa.Text, out tcoa[racer.SelectedIndex]);
        }

        private void Tcoboxb_KeyUp(object sender, KeyEventArgs e)
        {
            Single.TryParse(tcoboxb.Text, out tcob[racer.SelectedIndex]);
        }

        private void Bbbox_KeyUp(object sender, KeyEventArgs e)
        {
            Single.TryParse(bbbox.Text, out bbox[racer.SelectedIndex]);
        }



    }
}