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




        TM64_Course.Course[] CourseArray = new TM64_Course.Course[4 * 5 * 5];
        private void GameBuilderPro_Load(object sender, EventArgs e)
        {
            CupBox.SelectedIndex = 0;
            SetBox.SelectedIndex = 0;
            FormLoaded = true;
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


                int CourseIndex = ((SetBox.SelectedIndex * 20) + (CupBox.SelectedIndex * 4) + InputValue);
                

                CourseArray[CourseIndex] = NewCourse;
                

                
                return NewCourse.Settings.Name;
            }
            return null;
        }

        private void RefreshNames()
        {
            if (FormLoaded)
            {
            
                int CourseIndex = ((SetBox.SelectedIndex * 20) + (CupBox.SelectedIndex * 4));


                CourseBox1.Text = null;
                CourseBox2.Text = null;
                CourseBox3.Text = null;
                CourseBox4.Text = null;
                if (CourseArray[CourseIndex] != null)
                {
                    CourseBox1.Text = CourseArray[CourseIndex++].Settings.Name;
                }
                if (CourseArray[CourseIndex] != null)
                {
                    CourseBox2.Text = CourseArray[CourseIndex++].Settings.Name;
                }
                if (CourseArray[CourseIndex] != null)
                {
                    CourseBox3.Text = CourseArray[CourseIndex++].Settings.Name;
                }
                if (CourseArray[CourseIndex] != null)
                {
                    CourseBox4.Text = CourseArray[CourseIndex++].Settings.Name;
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
            MessageBox.Show("Select Patched ROM");


            if (FolderOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (File.Exists(FolderOpen.FileName))
                {
                    FileName = FolderOpen.FileName;


                    string outputDirectory = FolderOpen.FileName;
                    byte[] rom = File.ReadAllBytes(FileName);
                    int SetID = 0;
                    for (int ThisCourse = 0; ThisCourse < 100; ThisCourse++)
                    {
                        if (CourseArray[ThisCourse] == null)
                        {
                            continue;
                        }


                        rom = TarmacCourse.CompileOverKart(CourseArray[ThisCourse], rom, Convert.ToInt32(ThisCourse % 20), SetID, HeaderAddress);
                        if (ThisCourse % 20 == 19)
                        {
                            SetID++;
                        }


                        if (DebugBox.Checked)
                        {
                            File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString() + " Segment6.bin", CourseArray[ThisCourse].Segment6);
                            File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString() + " Segment9.bin", CourseArray[ThisCourse].Segment9);
                            File.WriteAllBytes(outputDirectory + "Course " + ThisCourse.ToString() + " Segment7.bin", CourseArray[ThisCourse].Segment7);
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

            for (int ThisCourse = 0; ThisCourse < 100; ThisCourse++)
            {
                if (CourseArray[ThisCourse] != null)
                {
                    binaryWriter.Write(TarmacCourse.SaveOK64Course(CourseArray[ThisCourse]));
                }
                else
                {
                    binaryWriter.Write("NULL");
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
                for (int ThisCourse = 0; ThisCourse < 100; ThisCourse++)
                {
                    Index = binaryReader.BaseStream.Position;
                    if (binaryReader.ReadString() == "NULL")
                    {
                        continue;
                    }

                    
                    byte[] Buffer = new byte[memoryStream.Length - memoryStream.Position];
                    memoryStream.Read(Buffer, 0, Buffer.Length);
                    CourseArray[ThisCourse] = TarmacCourse.LoadOK64Course(Buffer);
                }

            }
            
        }
    }
}
