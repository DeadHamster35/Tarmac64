using System;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.IO;
using System.Diagnostics;


namespace PorkChop
{





    public partial class PorkChop : Form
    {
        string settings_path = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "PorkChop.txt");


        float[] cc50 = new float[8];
        float[] cc100 = new float[8];
        float[] cc150 = new float[8];
        float[] ccextra = new float[8];
        float[] ccbattle = new float[8];
        float[] weigh = new float[8];
        float[,] acceler = new float[8,10];
        float[] steerin = new float[8];
        float[] friction = new float[8];
        float[] gravity = new float[8];
        float[] jump = new float[8];
        float[] fall = new float[8];
        float[] top = new float[8];
        int mkversion = new int();
        bool overwrite = new bool();


        string filePath = "";
        

        OpenFileDialog romopen = new OpenFileDialog();



        public PorkChop()
        {
            InitializeComponent();
            string[] items = { "Mario","Luigi","Yoshi","Toad","DK","Wario","Peach", "Bowser" };

            foreach (string item in items)
            {
                racer.Items.Add(item);
            }

                       
        }



        private void PorkChop_Load(object sender, EventArgs e)
        {

        }

      
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OKAbout f2 = new OKAbout();
            f2.ShowDialog();
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
                    using (var bw = new BeBinaryWriter(ms))
                    using (var br = new BeBinaryReader(ms))

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

                        //0   MARIO
                        //1   LUIGI
                        //2   YOSHI
                        //3   TOAD
                        //4   DK
                        //5   WARIO
                        //6   PEACH
                        //7   BOWSER

                        br.BaseStream.Seek(932560, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            for (int g = 0; g != 10; g++)
                            {
                                acceler[i, g] = br.ReadSingle();
                            }

                        }


                        br.BaseStream.Seek(930172, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

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

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            top[i] = br.ReadSingle();

                        }

                        br.BaseStream.Seek(930352, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            friction[i] = br.ReadSingle();

                        }

                        br.BaseStream.Seek(934448, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            steerin[i] = br.ReadSingle();

                        }


                        br.BaseStream.Seek(930416, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            gravity[i] = br.ReadSingle();

                        }

                        br.BaseStream.Seek(1187232, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(256, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                            weigh[i] = br.ReadSingle();

                        }

                        br.BaseStream.Seek(934608, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

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


                for (int i = 0; i!= 8; i++)
                {
                    
                    writetext = racer.Items[i] + " - 50CC -"+ cc50[i].ToString();
                    System.IO.File.AppendAllText(listfile, writetext);
                    System.IO.File.AppendAllText(listfile, Environment.NewLine);
                }

                for (int i = 0; i!= 8; i++)
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
                    
                    writetext = racer.Items[i] + " - Acceleration -" + acceler[i,0].ToString() +","+ acceler[i,1].ToString() +"," + acceler[i,2].ToString() +"," + acceler[i,3].ToString() +"," + acceler[i,4].ToString() +"," + acceler[i,5].ToString() +"," + acceler[i,6].ToString() +"," + acceler[i,7].ToString() +"," + acceler[i,8].ToString() +"," + acceler[i,9].ToString() +",";
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
                        using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        using (var ds = new FileStream(rompath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        using (var ms = new MemoryStream())
                        using (var bw = new BeBinaryWriter(ms))
                        using (var br = new BeBinaryReader(ms))

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



                        br.BaseStream.Seek(932560, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                            {
                                for (int g = 0; g != 10; g++)
                                {
                                    bw.Write(acceler[i, g]);

                                }

                            }


                            br.BaseStream.Seek(930172, SeekOrigin.Begin);


                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                        {
                          bw.Write(cc50[i]);



                        }
                        for (int i = 0; i != 8; i++)
                        {
                            bw.Write(cc100[i]);
                            


                        }
                        for (int i = 0; i != 8; i++)
                        {
                            bw.Write(cc150[i]);



                        }
                        for (int i = 0; i != 8; i++)
                        {
                            bw.Write(ccextra[i]);



                        }
                        for (int i = 0; i != 8; i++)
                        {
                            bw.Write(ccbattle[i]);



                        }

                            br.BaseStream.Seek(930448, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                            {
                                bw.Write(top[i]);


                            }

                            br.BaseStream.Seek(930352, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                            {
                                bw.Write(friction[i]);


                            }

                            br.BaseStream.Seek(934448, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                            {
                                bw.Write(steerin[i]);


                            }


                            br.BaseStream.Seek(930416, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                            {
                                bw.Write(gravity[i]);


                            }

                            br.BaseStream.Seek(1187232, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(256, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                            {
                                bw.Write(weigh[i]);


                            }

                            br.BaseStream.Seek(934608, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                            {
                                bw.Write(jump[i]);


                            }

                            br.BaseStream.Seek(934640, SeekOrigin.Begin);

                        if (mkversion == 2)
                        {
                            br.BaseStream.Seek(32, SeekOrigin.Current);
                        }

                        for (int i = 0; i != 8; i++)
                            {
                                bw.Write(fall[i]);


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

        private void Fiftycc_TextChanged(object sender, EventArgs e)
        {
            cc50[racer.SelectedIndex] = Convert.ToSingle(fiftycc.Text);
        }

        private void Hundocc_TextChanged(object sender, EventArgs e)
        {
            cc100[racer.SelectedIndex] = Convert.ToSingle(hundocc.Text);
        }

        private void Hundofiftycc_TextChanged(object sender, EventArgs e)
        {
            cc150[racer.SelectedIndex] = Convert.ToSingle(hundofiftycc.Text);
        }

        private void Extracc_TextChanged(object sender, EventArgs e)
        {
            ccextra[racer.SelectedIndex] = Convert.ToSingle(extracc.Text);
        }

        private void Battlecc_TextChanged(object sender, EventArgs e)
        {
            ccbattle[racer.SelectedIndex] = Convert.ToSingle(battlecc.Text);
        }

        private void Topspeed_TextChanged(object sender, EventArgs e)
        {
            top[racer.SelectedIndex] = Convert.ToSingle(topspeed.Text);
        }

        private void Frict_TextChanged(object sender, EventArgs e)
        {
            friction[racer.SelectedIndex] = Convert.ToSingle(frict.Text);
        }

        private void Weight_TextChanged(object sender, EventArgs e)
        {
            weigh[racer.SelectedIndex] = Convert.ToSingle(weight.Text);
        }

        private void Steering_TextChanged(object sender, EventArgs e)
        {
            steerin[racer.SelectedIndex] = Convert.ToSingle(steering.Text);
        }

        private void Grav_TextChanged(object sender, EventArgs e)
        {
            gravity[racer.SelectedIndex] = Convert.ToSingle(grav.Text);
        }

        private void Jumper_TextChanged(object sender, EventArgs e)
        {
            jump[racer.SelectedIndex] = Convert.ToSingle(jumper.Text);
        }

        private void Faller_TextChanged(object sender, EventArgs e)
        {
            fall[racer.SelectedIndex] = Convert.ToSingle(faller.Text);
        }

        private void Aa_TextChanged(object sender, EventArgs e)
        {
            acceler[racer.SelectedIndex,0] = Convert.ToSingle(aa.Text);
        }

        private void Ab_TextChanged(object sender, EventArgs e)
        {
            acceler[racer.SelectedIndex, 1] = Convert.ToSingle(ab.Text);
        }

        private void Ac_TextChanged(object sender, EventArgs e)
        {
            acceler[racer.SelectedIndex, 2] = Convert.ToSingle(ac.Text);
        }

        private void Ad_TextChanged(object sender, EventArgs e)
        {
            acceler[racer.SelectedIndex, 3] = Convert.ToSingle(ad.Text);
        }

        private void Ae_TextChanged(object sender, EventArgs e)
        {
            acceler[racer.SelectedIndex, 4] = Convert.ToSingle(ae.Text);
        }

        private void Af_TextChanged(object sender, EventArgs e)
        {
            acceler[racer.SelectedIndex, 5] = Convert.ToSingle(af.Text);
        }

        private void Ag_TextChanged(object sender, EventArgs e)
        {
            acceler[racer.SelectedIndex, 6] = Convert.ToSingle(ag.Text);
        }

        private void Ah_TextChanged(object sender, EventArgs e)
        {
            acceler[racer.SelectedIndex, 7] = Convert.ToSingle(ah.Text);
        }

        private void Ai_TextChanged(object sender, EventArgs e)
        {
            acceler[racer.SelectedIndex, 8] = Convert.ToSingle(ai.Text);
        }

        private void Aj_TextChanged(object sender, EventArgs e)
        {
            acceler[racer.SelectedIndex, 9] = Convert.ToSingle(aj.Text);
        }

        
    }
}