using Microsoft.WindowsAPICodePack.Dialogs;
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

namespace Tarmac64_Retail
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
        TM64.OK64Settings TarmacSettings = new TM64.OK64Settings();
        TM64 Tarmac = new TM64();
        bool UILock = false;

        public void UpdateUI()
        {
            if (!UILock)
            {
                CourseDIRBox.Text = TarmacSettings.ProjectDirectory;
                ObjectDIRBox.Text = TarmacSettings.ObjectDirectory;
                ScaleBox.Text = Convert.ToString(TarmacSettings.ImportScale);
                AlphaBox.Checked = TarmacSettings.AlphaCH2;
            }
        }

        public bool UpdateSettings()
        {
            float ParseF = 0.0f;
            bool Valid = false;
            TarmacSettings.ProjectDirectory = CourseDIRBox.Text;
            TarmacSettings.ObjectDirectory = ObjectDIRBox.Text;
            if (float.TryParse(ScaleBox.Text, out ParseF))
            {
                TarmacSettings.ImportScale = ParseF;
                Valid = true;
            }
            TarmacSettings.AlphaCH2 = AlphaBox.Checked;
            return Valid;
        }
        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            TarmacSettings.LoadSettings();
            UpdateUI();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (UpdateSettings())
            {
                TarmacSettings.SaveSettings();
            }
            else
            {
                MessageBox.Show("Invalid Scale Value");
            }

        }

        private void CourseDIRButton_Click(object sender, EventArgs e)
        {

            CommonOpenFileDialog FileOpen = new CommonOpenFileDialog();
            FileOpen.IsFolderPicker = true;

            if (FileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                CourseDIRBox.Text = FileOpen.FileName;
            }

        }

        private void ObjectDIRButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog FileOpen = new CommonOpenFileDialog();
            FileOpen.IsFolderPicker = true;

            if (FileOpen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ObjectDIRBox.Text = FileOpen.FileName;
            }
        }
    }
}
