using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using F3DSharp;

namespace OverKart64_Retail.Windows
{
    public partial class ColorCombineEditor : Form
    {
        public ColorCombineEditor()
        {
            InitializeComponent();
        }
        public int[] ValueArray = new int[8];
        public int CCMode = 0;
        bool Locked = false;

        public event EventHandler UpdateParent;
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var Mode in F3DSharp.F3DEX095_Parameters.ColorCombineNames)
            {
                ColorA.Items.Add(Mode);
                ColorB.Items.Add(Mode);
                ColorC.Items.Add(Mode);
                ColorD.Items.Add(Mode);                
            }
            foreach (var Mode in F3DSharp.F3DEX095_Parameters.GACModeNames)
            {
                AlphaA.Items.Add(Mode);
                AlphaB.Items.Add(Mode);
                AlphaC.Items.Add(Mode);
                AlphaD.Items.Add(Mode);
            }

        }
        public void UpdateUI()
        {

            Locked = true;
            ColorA.SelectedIndex = ValueArray[0];
            ColorB.SelectedIndex = ValueArray[1];
            ColorC.SelectedIndex = ValueArray[2];
            ColorD.SelectedIndex = ValueArray[3];

            AlphaA.SelectedIndex = ValueArray[4];
            AlphaB.SelectedIndex = ValueArray[5];
            AlphaC.SelectedIndex = ValueArray[6];
            AlphaD.SelectedIndex = ValueArray[7];
            Locked = false;
        }
        private void UpdateArray()
        {
            ValueArray[0] = ColorA.SelectedIndex;
            ValueArray[1] = ColorB.SelectedIndex;
            ValueArray[2] = ColorC.SelectedIndex;
            ValueArray[3] = ColorD.SelectedIndex;

            ValueArray[4] = AlphaA.SelectedIndex;
            ValueArray[5] = AlphaB.SelectedIndex;
            ValueArray[6] = AlphaC.SelectedIndex;
            ValueArray[7] = AlphaD.SelectedIndex;
        }

        private void MasterSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Locked)
            {
                UpdateArray();
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (UpdateParent != null)
            {
                UpdateParent(this, EventArgs.Empty);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
