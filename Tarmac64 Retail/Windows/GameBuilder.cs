using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public class CourseSet
        {
            public int Set { get; set; }
            public int Cup { get; set; }
            public int Course { get; set; }                
        }

        List<TM64_Course.Course> CourseData = new List<TM64_Course.Course>();
        List<CourseSet> SetCollection = new List<CourseSet>();

        int Set, Course, Cup = 0;
        int BattleSet, BattleCourse;

        private void CourseLoader_Load(object sender, EventArgs e)
        {
            NameBox.Items.Clear();
        }

        private void NameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyBox.Text = CourseData[NameBox.SelectedIndex].SerialNumber;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            RemoveCourse();
        }

        private void RemoveCourse()
        {
            if (NameBox.Items.Count > 0)
            {
                int Index = NameBox.SelectedIndex;
                NameBox.Items.RemoveAt(Index);
                CourseData.RemoveAt(Index);
                SetCollection.RemoveAt(Index);

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
            TM64.OK64Settings okSettings = new TM64.OK64Settings();
            TM64 Tarmac = new TM64();
            okSettings = Tarmac.LoadSettings();
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
                    if (NewCourse.Name.Length > 0)
                    {
                        Index = NameBox.Items.Add(NewCourse.Name);
                    }
                    else
                    {
                        Index = NameBox.Items.Add(FileOpen.FileName);
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
                        CourseData.Add(NewCourse);
                        CourseSet ThisSet = new CourseSet();
                        ThisSet.Course = Course;
                        ThisSet.Cup = Cup;
                        ThisSet.Set = Set;
                        SetCollection.Add(ThisSet);
                        Course++;
                    }
                    NameBox.SelectedIndex = Index;
                }
                else
                {
                    int Index;
                    if (NewCourse.Name.Length > 0)
                    {
                        Index = NameBox.Items.Add(NewCourse.Name);
                    }
                    else
                    {
                        Index = NameBox.Items.Add(FileOpen.FileName);
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
                        CourseData.Add(NewCourse);
                        CourseSet ThisSet = new CourseSet();
                        ThisSet.Course = BattleCourse;                        
                        ThisSet.Set = BattleSet;
                        ThisSet.Cup = 4; //battle
                        SetCollection.Add(ThisSet);
                        BattleCourse++;
                    }
                    NameBox.SelectedIndex = Index;
                }
                    

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TM64 Tarmac = new TM64();
            TM64_Course TarmacCourse = new TM64_Course();
            TM64.OK64Settings okSettings = Tarmac.LoadSettings();
            CommonOpenFileDialog FolderOpen = new CommonOpenFileDialog();
            FolderOpen.InitialDirectory = okSettings.ProjectDirectory;
            FolderOpen.IsFolderPicker = false;
            string FileName = "";
            uint HeaderOffset = Convert.ToUInt32(HeaderBox.Text, 16);
            MessageBox.Show("Select Patched ROM");


            if (FolderOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FileName = FolderOpen.FileName;
                

                string outputDirectory = FolderOpen.FileName;
                byte[] rom = File.ReadAllBytes(FileName);
                for (int ThisCourse = 0; ThisCourse < CourseData.Count; ThisCourse++)
                {
                    rom = TarmacCourse.CompileOverKart(CourseData[ThisCourse], rom, (SetCollection[ThisCourse].Cup * 4) + SetCollection[ThisCourse].Course, SetCollection[ThisCourse].Set);
                    /*
                    File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString()+ " Segment6.bin", CourseData[ThisCourse].Segment6);
                    File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString() + " Segment9.bin", CourseData[ThisCourse].Segment9);
                    File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString() + " Segment7.bin", CourseData[ThisCourse].Segment7);
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
                byte[] flip = BitConverter.GetBytes(binaryWriter.BaseStream.Length);
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
