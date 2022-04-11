using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tarmac64_Library;
using System.IO;

namespace Tarmac64_Retail
{
    public partial class GhostExtractor : Form
    {
        public GhostExtractor()
        {
            InitializeComponent();
        }
        public string[] characterNames = { "Mario", "Luigi", "Yoshi", "Toad", "D.K.", "Wario", "Peach", "Bowser" };

        TM64_Course.MemoryCard GhostData = new TM64_Course.MemoryCard();
        TM64_Course TarmacCourse = new TM64_Course();
        TM64 Tarmac = new TM64();
        private void GhostExtractor_Load(object sender, EventArgs e)
        {

            foreach (var name in characterNames)
            {
                ui_ghostCharacterSelect.Items.Add(name);
            }
        }




        private void UpdateGhostUI(TM64_Course.MemoryCard memoryCard)
        {
            bool DataFound = false;

            
            if (memoryCard.ghostData[0].raceTime > 0)
            {
                ui_ghostSelect.Items.Add("Ghost 0");
                ui_ghostSelect.SelectedIndex = 0;
                DataFound = true;
            }
            else
            {
                ui_ghostSelect.Items.Add("null");
            }
            if (memoryCard.ghostData[1].raceTime > 0)
            {
                    
                ui_ghostSelect.Items.Add("Ghost 1");
                ui_ghostSelect.SelectedIndex = 1;
                DataFound = true;
            }
            else
            {
                ui_ghostSelect.Items.Add("null");
            }
            ui_ghostSelect.Enabled = DataFound;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            if (FileOpen.ShowDialog() == DialogResult.OK)
            {
                MemoryStream memoryStream = new MemoryStream();
                BinaryReader binaryReader = new BinaryReader(memoryStream);
                string filePath = FileOpen.FileName;
                byte[] FileData = File.ReadAllBytes(filePath);
                memoryStream.Write(FileData, 0, FileData.Length);
                string fileCheck = "";

                binaryReader.BaseStream.Position = 0x300;
                byte[] fileTest = binaryReader.ReadBytes(4);
                char[] inputString = System.Text.Encoding.UTF8.GetString(fileTest).ToCharArray();
                fileCheck = new string(inputString);

                if (fileCheck == "NKTJ")
                {
                    GhostData.fileType = 01;
                    GhostData = TarmacCourse.LoadGhost(FileData);
                    UpdateGhostUI(GhostData);
                }
                else
                {
                    binaryReader.BaseStream.Position = 0x1340;
                    inputString = binaryReader.ReadChars(4);
                    fileCheck = new string(inputString);
                    if (fileCheck == "NKTJ")
                    {
                        GhostData.fileType = 02;
                        GhostData = TarmacCourse.LoadGhost(FileData);
                        UpdateGhostUI(GhostData);
                    }
                }
            }
            
        }

        private void ui_ghostSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ui_ghostCharacterSelect.SelectedIndex = GhostData.ghostData[ui_ghostSelect.SelectedIndex].character;
            ui_ghostTimeBox.Text = GhostData.ghostData[ui_ghostSelect.SelectedIndex].raceTime.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TM64_Course.GhostData localGhost = GhostData.ghostData[ui_ghostSelect.SelectedIndex];
            SaveFileDialog saveFile = new SaveFileDialog();
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
                //Ghost data is dirty, needs to be cleaned.
                //Decompress and Recompress to clean ghost data.
                byte[] CleanData = Tarmac.CompressMIO0(Tarmac.DecompressMIO0(localGhost.ghostInput));

                binaryWriter.Write(localGhost.character);
                binaryWriter.Write(CleanData);
                File.WriteAllBytes(saveFile.FileName, memoryStream.ToArray());
            }
        }
    }
}
