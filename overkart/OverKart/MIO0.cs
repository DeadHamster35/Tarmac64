using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using Be.IO;


namespace OverKart64

{
    public partial class MIO0 : Form
    {

        OpenFileDialog romopen = new OpenFileDialog();
        SaveFileDialog romsave = new SaveFileDialog();

        public MIO0()
        {
            InitializeComponent();
        }

        private void Textures_Load(object sender, EventArgs e)
        {

        }


        private void decompress(int offset, string path)
        {

            FileStream inputFile = File.Open(path, FileMode.Open);
            BeBinaryReader br = new BeBinaryReader(inputFile);

            byte[] file = br.ReadBytes((int)inputFile.Length);


            List<byte> newFile = new List<byte>();


            br.BaseStream.Position = offset;
            string magicNumber = Encoding.ASCII.GetString(br.ReadBytes(4));

            if (magicNumber == "MIO0")
            {
                int decompressedLength = br.ReadInt32();
                int compressedOffset = br.ReadInt32() + offset;
                int uncompressedOffset = br.ReadInt32() + offset;
                int currentOffset;

                try
                {

                    while (newFile.Count < decompressedLength)
                    {

                        byte bits = br.ReadByte(); //byte of layout bits
                        BitArray arrayOfBits = new BitArray(new byte[1] { bits });

                        for (int i = 7; i > -1 && (newFile.Count < decompressedLength); i--) //iterate through layout bits
                        {

                            if (arrayOfBits[i] == true)
                            {
                                //non-compressed
                                //add one byte from uncompressedOffset to newFile

                                currentOffset = (int)inputFile.Position;

                                inputFile.Seek(uncompressedOffset, SeekOrigin.Begin);

                                newFile.Add(br.ReadByte());
                                uncompressedOffset++;

                                inputFile.Seek(currentOffset, SeekOrigin.Begin);

                            }
                            else
                            {
                                //compressed
                                //read 2 bytes
                                //4 bits = length
                                //12 bits = offset

                                currentOffset = (int)inputFile.Position;
                                inputFile.Seek(compressedOffset, SeekOrigin.Begin);

                                byte byte1 = br.ReadByte();
                                byte byte2 = br.ReadByte();
                                compressedOffset += 2;

                                //Note: For Debugging, binary representations can be printed with:  Convert.ToString(numberVariable, 2);

                                byte byte1Upper = (byte)((byte1 & 0x0F));//offset bits
                                byte byte1Lower = (byte)((byte1 & 0xF0) >> 4); //length bits

                                int combinedOffset = ((byte1Upper << 8) | byte2);

                                int finalOffset = 1 + combinedOffset;
                                int finalLength = 3 + byte1Lower;

                                for (int k = 0; k < finalLength; k++) //add data for finalLength iterations
                                {
                                    newFile.Add(newFile[newFile.Count - finalOffset]); //add byte at offset (fileSize - finalOffset) to file
                                }

                                inputFile.Seek(currentOffset, SeekOrigin.Begin); //return to layout bits

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                inputFile.Close();

                if (romsave.ShowDialog() == DialogResult.OK)
                {

                    byte[] saveFile = newFile.ToArray();
                    File.WriteAllBytes(romsave.FileName, saveFile);
                }
                else
                {
                    MessageBox.Show("This is not an MIO0 file.");

                }
            }


        }



        private void export_Click(object sender, EventArgs e)
        {
            int miooffset = new int();
            miooffset = 0;
            int.TryParse(offsetbox.Text, out miooffset);
            if (romopen.ShowDialog() == DialogResult.OK)
            {
                decompress(miooffset, romopen.FileName);

            }
        }
    }
}
