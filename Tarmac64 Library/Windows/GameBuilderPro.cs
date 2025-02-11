using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tarmac64_Library;

namespace Tarmac64_Library
{
    public partial class GameBuilderPro : Form
    {

        bool FormLoaded = false;

        public GameBuilderPro()
        {
            InitializeComponent();
        }




        TM64_Course.Course[][][] CourseArray = new TM64_Course.Course[5][][];
        private void GameBuilderPro_Load(object sender, EventArgs e)
        {
            this.Height = 290;
            CupBox.SelectedIndex = 0;
            SetBox.SelectedIndex = 0;
            FormLoaded = true;
            for (int This = 0; This < 5; This++)
            {
                CourseArray[This] = new TM64_Course.Course[5][];
                for (int That = 0; That < 5; That++)
                {
                    CourseArray[This][That] = new TM64_Course.Course[4];
                }
            }
        }


        private string LoadCourseFile(int InputValue)
        {

            TM64 Tarmac = new TM64();

            TM64.OK64Settings okSettings = new TM64.OK64Settings();
            okSettings.LoadSettings();

            TM64_Course TarmacCourse = new TM64_Course();
            OpenFileDialog FileOpen = new OpenFileDialog();
            FileOpen.Filter = "Tarmac Course|*.ok64.Course|All Files (*.*)|*.*";
            FileOpen.InitialDirectory = okSettings.ProjectDirectory;
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                byte[] FileData = File.ReadAllBytes(FileOpen.FileName);
                TM64_Course.Course NewCourse = TarmacCourse.LoadOK64Course(FileData);



                if ((CupBox.SelectedIndex == 4) && (NewCourse.Gametype != 1))
                {
                    MessageBox.Show("Error - Attempted to load Race Course for Battle Cup");
                    return null;
                }

                if ((CupBox.SelectedIndex != 4) && (NewCourse.Gametype == 1))
                {
                    MessageBox.Show("Error - Attempted to load Battle Course for Race Cup");
                    return null;
                }



                CourseArray[SetBox.SelectedIndex][CupBox.SelectedIndex][InputValue] = NewCourse;
                

                
                return NewCourse.Settings.Name;
            }
            return null;
        }

        private void RefreshNames()
        {
            if (FormLoaded)
            {

                int CourseIndex = 0;
                CourseBox1.Text = null;
                CourseBox2.Text = null;
                CourseBox3.Text = null;
                CourseBox4.Text = null;
                if (CourseArray[SetBox.SelectedIndex][CupBox.SelectedIndex][CourseIndex] != null)
                {
                    CourseBox1.Text = CourseArray[SetBox.SelectedIndex][CupBox.SelectedIndex][CourseIndex].Settings.Name;
                }
                CourseIndex++;
                if (CourseArray[SetBox.SelectedIndex][CupBox.SelectedIndex][CourseIndex] != null)
                {
                    CourseBox2.Text = CourseArray[SetBox.SelectedIndex][CupBox.SelectedIndex][CourseIndex].Settings.Name;
                }
                CourseIndex++;
                if (CourseArray[SetBox.SelectedIndex][CupBox.SelectedIndex][CourseIndex] != null)
                {
                    CourseBox3.Text = CourseArray[SetBox.SelectedIndex][CupBox.SelectedIndex][CourseIndex].Settings.Name;
                }
                CourseIndex++;
                if (CourseArray[SetBox.SelectedIndex][CupBox.SelectedIndex][CourseIndex] != null)
                {
                    CourseBox4.Text = CourseArray[SetBox.SelectedIndex][CupBox.SelectedIndex][CourseIndex].Settings.Name;
                }



            }
        }

        private void CourseBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CourseBox1.Text = LoadCourseFile(0);
        }

        private void CourseBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CourseBox2.Text = LoadCourseFile(1);
        }

        private void CourseBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CourseBox3.Text = LoadCourseFile(2);
        }

        private void CourseBox4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CourseBox4.Text = LoadCourseFile(3);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshNames();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TM64 Tarmac = new TM64();
            TM64_Course TarmacCourse = new TM64_Course();

            TM64.OK64Settings okSettings = new TM64.OK64Settings();
            okSettings.LoadSettings();


            uint HeaderAddress;
            if (!UInt32.TryParse(HeaderBox.Text, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out HeaderAddress))
            {
                MessageBox.Show("Invalid Header Address");
                return;
            }

            CommonOpenFileDialog FolderOpen = new CommonOpenFileDialog();
            FolderOpen.InitialDirectory = okSettings.ROMDirectory;
            FolderOpen.IsFolderPicker = false;
            string FileName = "";
            uint HeaderOffset = Convert.ToUInt32(HeaderBox.Text, 16);
            MessageBox.Show("Select ROM");


            if (FolderOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (File.Exists(FolderOpen.FileName))
                {
                    FileName = FolderOpen.FileName;


                    string outputDirectory = FolderOpen.FileName;
                    byte[] rom = File.ReadAllBytes(FileName);

                    if (!Tarmac.CheckPatch(rom))
                    {
                        MessageBox.Show("Applying Tarmac Patch");
                        string PatchPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Resources", "PatchData.ok64.Patch");
                        if (!File.Exists(PatchPath))
                        {
                            MessageBox.Show("Patch File Not Found \n" + PatchPath);
                            return;
                        }
                        byte[] PatchData = File.ReadAllBytes(PatchPath);

                        rom = Tarmac.ApplyPatch(rom, PatchData);
                    }

                    int Index = 0;
                    for (int ThisSet = 0; ThisSet < 5; ThisSet++)
                    {
                        for (int ThisCup = 0; ThisCup < 5; ThisCup++)
                        {
                            for (int ThisCourse = 0; ThisCourse < 4; ThisCourse++)
                            {
                                TM64_Course.Course LocalCourse = CourseArray[ThisSet][ThisCup][ThisCourse];
                                if (LocalCourse == null)
                                {
                                    continue;
                                }


                                rom = TarmacCourse.CompileOverKart(LocalCourse, rom, ThisCourse + (4 * ThisCup), ThisSet, HeaderAddress);



                                if (DebugBox.Checked)
                                {
                                    File.WriteAllBytes(outputDirectory + LocalCourse.Settings.Name + " - Course " + ThisCourse.ToString() + " Segment6.bin", LocalCourse.Segment6);
                                    File.WriteAllBytes(outputDirectory + LocalCourse.Settings.Name + " - Course " + ThisCourse.ToString() + " Segment9.bin", LocalCourse.Segment9);
                                    File.WriteAllBytes(outputDirectory + LocalCourse.Settings.Name + " - Course " + ThisCourse.ToString() + " Segment7.bin", LocalCourse.Segment7);
                                }


                            }
                        }
                    }

                    MemoryStream memoryStream = new MemoryStream();
                    BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
                    binaryWriter.Write(rom);
                    binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;


                    int addressAlign = 1048576 - (Convert.ToInt32(binaryWriter.BaseStream.Length) % 1048576);
                    if (addressAlign == 1048576)
                        addressAlign = 0;


                    for (int align = 0; align < addressAlign; align++)
                    {
                        binaryWriter.Write(Convert.ToByte(0x00));
                    }

                    binaryWriter.BaseStream.Position = 0xBFFFFC;
                    byte[] flip = BitConverter.GetBytes(Convert.ToInt32(binaryWriter.BaseStream.Length));
                    Array.Reverse(flip);
                    binaryWriter.Write(flip);

                    SaveFileDialog FileSave = new SaveFileDialog();
                    FileSave.Filter = "Z64 ROM|*.z64|All Files (*.*)|*.*";
                    FileSave.DefaultExt = "z64";
                    if (FileSave.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(FileSave.FileName, memoryStream.ToArray());

                    }
                }

            }
        }

        private void SetBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshNames();
        }

        private void CupBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshNames();
        }

        private void CourseBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                int CourseIndex = ((SetBox.SelectedIndex * 20) + (CupBox.SelectedIndex * 4));
                CourseArray[CourseIndex] = null;
                RefreshNames();
            }
        }

        private void CourseBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                int CourseIndex = ((SetBox.SelectedIndex * 20) + (CupBox.SelectedIndex * 4));
                CourseArray[CourseIndex + 1] = null;
                RefreshNames();
            }
        }

        private void CourseBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                int CourseIndex = ((SetBox.SelectedIndex * 20) + (CupBox.SelectedIndex * 4));
                CourseArray[CourseIndex + 2] = null;
                RefreshNames();
            }
        }

        private void CourseBox4_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                int CourseIndex = ((SetBox.SelectedIndex * 20) + (CupBox.SelectedIndex * 4));
                CourseArray[CourseIndex + 3] = null;
                RefreshNames();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            byte[] Data = new byte[0];

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            TM64 Tarmac = new TM64();
            TM64_Course TarmacCourse = new TM64_Course();

            for (int ThisSet = 0; ThisSet < 5; ThisSet++)
            {
                for (int ThisCup = 0; ThisCup < 5; ThisCup++)
                {
                    for (int ThisCourse = 0; ThisCourse < 4; ThisCourse++)
                    {
                        TM64_Course.Course LocalCourse = CourseArray[ThisSet][ThisCup][ThisCourse];
                        if (LocalCourse != null)
                        {
                            binaryWriter.Write(TarmacCourse.SaveOK64Course(LocalCourse));
                        }
                        else
                        {
                            binaryWriter.Write("NULL");
                        }
                    }
                }
            }

            SaveFileDialog FileSave = new SaveFileDialog();
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(FileSave.FileName, memoryStream.ToArray());
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                byte[] Data = File.ReadAllBytes(FileOpen.FileName);

                MemoryStream memoryStream = new MemoryStream();
                BinaryReader binaryReader = new BinaryReader(memoryStream);
                memoryStream.Write(Data, 0, Data.Length);
                memoryStream.Position = 0;

                TM64 Tarmac = new TM64();
                TM64_Course TarmacCourse = new TM64_Course();
                long Index = 0;
                for (int ThisSet = 0; ThisSet < 5; ThisSet++)
                {
                    for (int ThisCup = 0; ThisCup < 5; ThisCup++)
                    {
                        for (int ThisCourse = 0; ThisCourse < 4; ThisCourse++)
                        {
                            Index = binaryReader.BaseStream.Position;
                            if (binaryReader.ReadString() == "NULL")
                            {
                                continue;
                            }


                            byte[] Buffer = new byte[memoryStream.Length - memoryStream.Position];
                            memoryStream.Read(Buffer, 0, Buffer.Length);
                            CourseArray[ThisSet][ThisCup][ThisCourse] = TarmacCourse.LoadOK64Course(Buffer);
                        }
                    }
                }

            }
            
        }
        bool Expand = false;
        private void button1_Click(object sender, EventArgs e)
        {
            Expand = !Expand;
            if (Expand)
            {
                ExpandBtn.Text = " ▲ ";
                this.Height = 380;
            }
            else
            {
                ExpandBtn.Text = " ▼ ";
                this.Height = 290;
            }
        }
    }
}
