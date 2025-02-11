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

namespace Tarmac64_Library
{
    public partial class GameBuilder : Form
    {
        public GameBuilder()
        {
            InitializeComponent();
        }

        List<TM64_Course.Course> RaceCourses = new List<TM64_Course.Course>();
        List<TM64_Course.Course> BattleCourses = new List<TM64_Course.Course>();

        int Set, Course, Cup = 0;
        int BattleSet, BattleCourse;

        private void CourseLoader_Load(object sender, EventArgs e)
        {
            RaceNameBox.Items.Clear();
        }

        private void NameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyBox.Text = RaceCourses[RaceNameBox.SelectedIndex].SerialNumber;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            RemoveCourse();
        }

        private void button4_Click(object sender, EventArgs e)
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



                if (NewCourse.Gametype == 0)
                {
                    MessageBox.Show("Wrong Course Type (Race)");
                }
                else
                {
                    int Index;
                    if (NewCourse.Settings.Name.Length > 0)
                    {
                        Index = RaceNameBox.Items.Add(NewCourse.Settings.Name);
                    }
                    else
                    {
                        Index = BattleNameBox.Items.Add(FileOpen.FileName);
                    }


                    if (BattleCourse > 3)
                    {
                        BattleCourse = 0;
                        BattleSet++;
                    }
                    if (BattleSet > 4)
                    {
                        MessageBox.Show("FATAL ERROR - TOO MANY LEVELS");
                    }
                    else
                    {
                        BattleCourses.Add(NewCourse);
                        BattleCourse++;
                    }
                    BattleNameBox.SelectedIndex = Index;
                }


            }
        }

        private void BattleNameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BattleKeyBox.Text = BattleCourses[BattleNameBox.SelectedIndex].SerialNumber;
        }

        private void RemoveBattleCourse()
        {
            if (BattleNameBox.Items.Count >= 1)
            {
                int Index = BattleNameBox.SelectedIndex;
                BattleNameBox.Items.RemoveAt(Index);
                BattleCourses.RemoveAt(Index);

                BattleCourse--;
                if (BattleCourse < 0)
                {
                    BattleCourse = 3;
                    BattleSet--;
                }
            }
        }
        private void RemoveCourse()
        {
            if (RaceNameBox.Items.Count >= 1)
            {
                int Index = RaceNameBox.SelectedIndex;
                RaceNameBox.Items.RemoveAt(Index);
                RaceCourses.RemoveAt(Index);

                Course--;
                if (Course < 0)
                {
                    Course = 3;
                    Cup--;
                }
                if (Cup < 0)
                {
                    Cup = 3;
                    Set--;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
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



                if (NewCourse.Gametype == 0)
                {
                    int Index;
                    if (NewCourse.Settings.Name.Length > 0)
                    {
                        Index = RaceNameBox.Items.Add(NewCourse.Settings.Name);
                    }
                    else
                    {
                        Index = RaceNameBox.Items.Add(FileOpen.FileName);
                    }
                    
                    
                    if (Course > 3)
                    {
                        Course = 0;
                        Cup++;
                    }
                    if (Cup > 3)
                    {
                        Cup = 0;
                        Set++;
                    }
                    if (Set > 4)
                    {
                        MessageBox.Show("FATAL ERROR - TOO MANY LEVELS");
                    }
                    else
                    {
                        RaceCourses.Add(NewCourse);
                        Course++;
                    }
                    RaceNameBox.SelectedIndex = Index;
                }
                else
                {
                    MessageBox.Show("Wrong Course Type (Battle)");
                }
                    

            }
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
                            MessageBox.Show("Patch File Not Found \n" +  PatchPath);
                            return;
                        }
                        byte[] PatchData = File.ReadAllBytes(PatchPath);

                        rom = Tarmac.ApplyPatch(rom, PatchData);
                    }


                    int SetID = 0;
                    for (int ThisCourse = 0; ThisCourse < RaceCourses.Count; ThisCourse++)
                    {
                        rom = TarmacCourse.CompileOverKart(RaceCourses[ThisCourse], rom, Convert.ToInt32(ThisCourse % 16), SetID, HeaderAddress);
                        if (ThisCourse % 16 == 15)
                        {
                            SetID++;
                        }

                        
                        /*
                        File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString()+ " Segment6.bin", CourseData[ThisCourse].Segment6);
                        File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString() + " Segment9.bin", CourseData[ThisCourse].Segment9);
                        File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString() + " Segment7.bin", CourseData[ThisCourse].Segment7);
                        */

                    }
                    SetID = 0;
                    for (int ThisCourse = 0; ThisCourse < BattleCourses.Count; ThisCourse++)
                    {
                        rom = TarmacCourse.CompileOverKart(BattleCourses[ThisCourse], rom, Convert.ToInt32(16 + (ThisCourse % 4)), SetID, HeaderAddress);
                        if (ThisCourse % 4 == 3)
                        {
                            SetID++;
                        }


                        /*
                        File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString()+ " Segment6.bin", BattleCourses[ThisCourse].Segment6);
                        File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString() + " Segment9.bin", BattleCourses[ThisCourse].Segment9);
                        File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString() + " Segment7.bin", BattleCourses[ThisCourse].Segment7);
                        */

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
    }
}
