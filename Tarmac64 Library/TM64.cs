using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using System.Xml;
using Tarmac64_Library;
using Cereal64;


//custom libraries

using Assimp;  //for handling model data
using Texture64;  //for handling texture data


using Cereal64.Microcodes.F3DEX.DataElements;
using Cereal64.Common.DataElements;
using Cereal64.Common.Rom;
using Cereal64.Common.Utils.Encoding;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Drawing.Drawing2D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace Tarmac64_Library
{
    public class TM64
    {

        /// These are various functions for decompressing and handling the segment data for Mario Kart 64.


        public static int newint = 4;


        byte[] flip2 = new byte[2];
        byte[] flip4 = new byte[4];

        UInt16 value16 = new UInt16();
        Int16 valuesign16 = new Int16();

        UInt32 value32 = new UInt32();


        /// These classes are used by the underlying functions.
        /// These are used by the Geometry Builder
        

        public class OK64Settings
        {
            public int Version { get; set; }
            public string ProjectDirectory { get; set; }
            public string ObjectDirectory { get; set; }
            public string ROMDirectory { get; set; }
            public float ImportScale { get; set; }
            public bool AlphaCH2 { get; set; }
            public int ImportMode { get; set; }
            public string JRDirectory { get; set; }
            public bool Valid { get; set; }
            public int Version { get; set; }


            public OK64Settings LoadSettings()
            {
                string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string SettingsPath = Path.Combine(AppData, "Tarmac64.OK64Settings");
                string[] InputData = new string[0];

                if (File.Exists(SettingsPath))
                {
                    InputData = File.ReadAllLines(SettingsPath);
                    int Parse = -1;

                    if (int.TryParse(InputData[0], out Parse))
                    {
                        if (Parse == 800)
                        {
                            Version = Parse;
                            ProjectDirectory = InputData[1];
                            ObjectDirectory = InputData[2];
                            ROMDirectory = InputData[3];
                            ImportScale = Convert.ToSingle(InputData[4]);
                            AlphaCH2 = Convert.ToBoolean(InputData[5]);
                            ImportMode = Convert.ToInt32(InputData[6]);
                            return this;
                        }
                    }
                }

                MessageBox.Show("Error Loading Settings. Restoring defaults");
                Version = 7;
                ProjectDirectory = "";
                ObjectDirectory = "";
                ROMDirectory = "";
                ImportScale = 1.0f;
                AlphaCH2 = true;
                ImportMode = 0;
                Version = 800;
                SaveSettings();
                return this;
                
            }
            public void SaveSettings()
            {
                string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string SettingsPath = Path.Combine(AppData, "Tarmac64.OK64Settings");
                string[] settings = new string[0];


                string[] SaveData = new string[7];
                SaveData[0] = Convert.ToString(Version);
                SaveData[1] = ProjectDirectory;
                SaveData[2] = ObjectDirectory;
                SaveData[3] = ROMDirectory;
                SaveData[4] = Convert.ToString(ImportScale);
                SaveData[5] = Convert.ToString(AlphaCH2);
                SaveData[6] = Convert.ToString(ImportMode);

                File.WriteAllLines(SettingsPath, SaveData);
            }
        }


        ///

        public bool CheckPatch(byte[] ROMData)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            memoryStream.Write(ROMData, 0, ROMData.Length);
            binaryReader.BaseStream.Position = 0x20;


            string ROMName = new string(binaryReader.ReadChars(11));

            if (ROMName == "MARIOKART64")
            {
                return false;
            }


            return true;
        }
        public byte[] ApplyPatch(byte[] InputData, byte[] PatchData)
        {
            MemoryStream PatchStream = new MemoryStream();
            BinaryReader PatchReader = new BinaryReader(PatchStream);
            PatchStream.Write(PatchData, 0, PatchData.Length);
            PatchStream.Position = 0;

            MemoryStream InputStream = new MemoryStream();
            BinaryWriter InputWriter = new BinaryWriter(InputStream);
            InputStream.Write(InputData, 0, InputData.Length);
            InputStream.Position = 0;

            int CurrentByte = 0;
            while(CurrentByte < PatchData.Length)
            {
                int Offset = PatchReader.ReadInt32();
                int Length = PatchReader.ReadInt32();
                
                
                byte[] CopyData = new byte[Length];
                Array.Copy(PatchData, CurrentByte + 8, CopyData, 0, Length);

                InputWriter.BaseStream.Position = Offset;
                InputWriter.Write(CopyData);

                PatchReader.BaseStream.Position += Length;
                CurrentByte += Length + 8;
            }




            return InputStream.ToArray();
        }


        public byte[] CreatePatch(byte[] InputData, byte[] PatchedData)
        {
            MemoryStream PatchStream = new MemoryStream();
            BinaryReader PatchReader = new BinaryReader(PatchStream);
            PatchStream.Write(PatchedData, 0, PatchedData.Length);
            PatchStream.Position = 0;

            MemoryStream InputStream = new MemoryStream();
            BinaryReader InputReader = new BinaryReader(InputStream);
            PatchStream.Write(InputData, 0, InputData.Length);
            PatchStream.Position = 0;

            MemoryStream NewData = new MemoryStream();
            BinaryWriter OutputData = new BinaryWriter(NewData);


            
            int Length = PatchedData.Length;
            
            
            for (int ThisByte = 0; ThisByte < Length; ThisByte++)
            {
                int PatchBytes = 0;
                int InitialOffset = 0;
                List<byte> PatchData = new List<byte>();
                
                if (ThisByte < InputData.Length)
                {

                    if (InputData[ThisByte] != PatchedData[ThisByte])
                    {
                        InitialOffset = ThisByte;

                        while (true)
                        {
                            PatchData.Add(PatchedData[ThisByte]);
                            PatchBytes++;
                            if (InputData[ThisByte + 1] == PatchedData[ThisByte + 1])
                            {
                                break;
                            }
                            else
                            {
                                ThisByte++;
                            }
                        }
                        OutputData.Write(InitialOffset);
                        OutputData.Write(PatchBytes);
                        OutputData.Write(PatchData.ToArray(), 0, PatchData.Count);
                    }
                }
                else
                {
                    byte[] EndData = new byte[Length - ThisByte];
                    Array.Copy(PatchedData, ThisByte, EndData, 0, EndData.Length);

                    OutputData.Write(InputData.Length);
                    OutputData.Write(EndData.Length);
                    for (int ThisEnd = 0; ThisEnd < EndData.Length; ThisEnd++)
                    {
                        OutputData.Write(EndData[ThisEnd]);
                    }
                    ThisByte = Length;
                }


            }

            return NewData.ToArray();
        }

        public void GenerateElement(XmlDocument XMLDoc, XmlElement Parent, string Name, bool[] Values)
        {
            XmlElement NewElement = XMLDoc.CreateElement(Name);
            for (int ThisVal = 0; ThisVal < Values.Length;)
            {
                NewElement.InnerText += Convert.ToInt32(Values[ThisVal]).ToString();

                ThisVal++;

                if (ThisVal < Values.Length)
                {
                    NewElement.InnerText += ",";
                }

            }
            Parent.AppendChild(NewElement);
        }

        public void GenerateElement(XmlDocument XMLDoc, XmlElement Parent, string Name, int[] Values)
        {
            XmlElement NewElement = XMLDoc.CreateElement(Name);
            for (int ThisVal = 0; ThisVal < Values.Length;)
            {
                NewElement.InnerText += Values[ThisVal].ToString();

                ThisVal++;

                if (ThisVal < Values.Length)
                {
                    NewElement.InnerText += ",";
                }
                    
            }
            Parent.AppendChild(NewElement);
        }


        public void GenerateElement(XmlDocument XMLDoc, XmlElement Parent, string Name, float[] Values)
        {
            XmlElement NewElement = XMLDoc.CreateElement(Name);
            for (int ThisVal = 0; ThisVal < Values.Length;)
            {
                NewElement.InnerText += Values[ThisVal].ToString();

                ThisVal++;

                if (ThisVal < Values.Length)
                {
                    NewElement.InnerText += ",";
                }

            }
            Parent.AppendChild(NewElement);
        }
        public void GenerateElement(XmlDocument XMLDoc, XmlElement Parent, string Name, byte Value)
        {
            XmlElement NewElement = XMLDoc.CreateElement(Name);
            NewElement.InnerText = Value.ToString();
            Parent.AppendChild(NewElement);
        }
        public void GenerateElement(XmlDocument XMLDoc, XmlElement Parent, string Name, short Value)
        {
            XmlElement NewElement = XMLDoc.CreateElement(Name);
            NewElement.InnerText = Value.ToString();
            Parent.AppendChild(NewElement);
        }
        public void GenerateElement(XmlDocument XMLDoc, XmlElement Parent, string Name, int Value)
        {
            XmlElement NewElement = XMLDoc.CreateElement(Name);
            NewElement.InnerText = Value.ToString();
            Parent.AppendChild(NewElement);
        }

        public void GenerateElement(XmlDocument XMLDoc, XmlElement Parent, string Name, float Value)
        {
            XmlElement NewElement = XMLDoc.CreateElement(Name);
            NewElement.InnerText = Value.ToString();
            Parent.AppendChild(NewElement);
        }


        public void GenerateElement(XmlDocument XMLDoc, XmlElement Parent, string Name, double Value)
        {
            XmlElement NewElement = XMLDoc.CreateElement(Name);
            NewElement.InnerText = Value.ToString();
            Parent.AppendChild(NewElement);
        }

        public void GenerateElement(XmlDocument XMLDoc, XmlElement Parent, string Name, string Value)
        {
            XmlElement NewElement = XMLDoc.CreateElement(XmlConvert.EncodeName(Name));
            NewElement.InnerText = XmlConvert.EncodeName(Value);
            Parent.AppendChild(NewElement);
        }

        
        public void GenerateElementRaw(XmlDocument XMLDoc, XmlElement Parent, string Name, string Value)
        {
            XmlElement NewElement = XMLDoc.CreateElement(XmlConvert.EncodeName(Name));
            NewElement.InnerText = (Value);
            Parent.AppendChild(NewElement);
        }
        public float[] LoadElementsF(XmlDocument XMLDoc, string Parent, string Name, string Default = "")
        {
            List<float> Elements = new List<float>();
            XmlNode CheckNode = XMLDoc.SelectSingleNode("/" + Parent + "/" + Name);
            if (CheckNode != null)
            {
                string Item = XmlConvert.DecodeName(CheckNode.InnerText);
                string[] Items = Item.Split(',');
                foreach (var Tag in Items)
                {
                    Elements.Add(Convert.ToSingle(Tag));
                }
            }
            return Elements.ToArray();
        }

        public int[] LoadElements(XmlDocument XMLDoc, string Parent, string Name, string Default = "")
        {
            List<int> Elements = new List<int>();
            XmlNode CheckNode = XMLDoc.SelectSingleNode("/" + Parent + "/" + Name);
            if (CheckNode != null)
            {
                string Item = XmlConvert.DecodeName(CheckNode.InnerText);
                string[] Items = Item.Split(',');
                foreach (var Tag in Items)
                {
                    Elements.Add(Convert.ToInt32(Tag));
                }
            }
            return Elements.ToArray();
        }
        public string LoadElement(XmlDocument XMLDoc, string Parent, string Name, string Default = "")
        {
            XmlNode CheckNode = XMLDoc.SelectSingleNode("/" + Parent + "/" +  Name);
            if (CheckNode != null)
            {
                return XmlConvert.DecodeName(CheckNode.InnerText);
            }
            return Default;
        }

        public string LoadElement(XmlReader XMLDoc, string Parent, string Name, string Default = "")
        {
            return XMLDoc.ReadElementContentAsString(Parent + "/" + Name, "");
        }

        public byte[] DecompressMIO0(byte[] inputFile)
        {
            byte[] outputFile = Cereal64.Common.Utils.Encoding.MIO0.Decode(inputFile);
            
            return outputFile;
        }
        public byte[] CompressMIO0(byte[] inputFile)
        {
            byte[] outputFile = Cereal64.Common.Utils.Encoding.MIO0.Encode(inputFile);
            return outputFile;
        }
        public byte[] CompressTKMK(byte[] inputFile, int Width, int Height)
        {
            TKMK00Encoder TKMK = new TKMK00Encoder();
            byte[] outputFile = TKMK.Encode(inputFile, Width, Height, 0);
            return outputFile;
        }
        public byte[] compress_seg7(byte[] ROM)
        {

            /// This will compress compatible F3DEX commands into a compressed Segment 7.
            /// This is used exclusively by Mario Kart 64's Segment 7.
            /// No, I don't know how this works. Don't ask me how it works.
            /// I have no fucking clue how I wrote this. If you find out, let me know!



            ///You may ask yourself, "What is that beautiful house?"
            ///You may ask yourself, "Where does that highway go to?"
            ///And you may ask yourself, "Am I right? Am I wrong?"
            ///And you may say to yourself, "My God! What have I done?"







            int indexA = 0;
            int indexB = 0;
            int indexC = 0;


            MemoryStream romm = new MemoryStream(ROM);
            BinaryReader mainseg = new BinaryReader(romm);
            MemoryStream seg7m = new MemoryStream();
            BinaryWriter seg7w = new BinaryWriter(seg7m);

            seg7w.BaseStream.Seek(0, SeekOrigin.Begin);


            string commandbyte = "";  ///keeping the same name from above decompress process
            byte[] byte29 = new byte[2];
            string compar = "";
            byte F3Dbyte = new byte();
            byte[] parambyte = new byte[2];






            byte[] voffset = new byte[2];

            byte compressbyte = new byte();

            mainseg.BaseStream.Position = 0;

            for (int i = 0; (mainseg.BaseStream.Position < mainseg.BaseStream.Length); i++)
            {

                F3Dbyte = mainseg.ReadByte();
                commandbyte = F3Dbyte.ToString("x").PadLeft(2, '0').ToUpper();

                ///MessageBox.Show(F3Dbyte.ToString("x").PadLeft(2,'0').ToUpper() + "--" + mainseg.BaseStream.Position.ToString()); ;




                if (commandbyte == "BC")
                {


                    MessageBox.Show("Unsupported Command -BC-");
                    ///0x00 -- 0x14

                    ///curently unsupported, ??not featured in stock MK64 racing tracks?? Can't find in multiple courses.


                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBC000002));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);
                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x80000040));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);
                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x03860010));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);
                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x09000000 | (commandbyte * 0x18) + 8));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);
                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x03880010));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);
                    ///flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x09000000 | commandbyte * 0x18));
                    ///Array.Reverse(flip4);
                    ///seg7w.Write(flip4);


                }
                if (commandbyte == "FC")
                {

                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    if (compar == "1218")
                    {
                        compressbyte = 0x15;
                    }
                    if (compar == "127E")
                    {
                        compressbyte = 0x16;
                    }
                    if (compar == "FFFF")
                    {
                        compressbyte = 0x17;
                    }

                    mainseg.BaseStream.Seek(5, SeekOrigin.Current);
                    seg7w.Write(compressbyte);

                }
                if (commandbyte == "B9")
                {


                    mainseg.BaseStream.Seek(5, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    if (compar == "2078")
                    {
                        compressbyte = 0x18;
                    }
                    if (compar == "3078")
                    {
                        compressbyte = 0x19;
                    }
                    seg7w.Write(compressbyte);

                }
                if (commandbyte == "E8")
                {
                    /// 00000000 00000000
                    ///0x1A -> 0x1F + 0x2C
                    mainseg.BaseStream.Seek(7, SeekOrigin.Current);





                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");
                    byte29 = mainseg.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");

                    byte[] Param = new byte[2];


                    ///don't ask me I don't know
                    ///don't ask me I don't know
                    byte[] parameters = mainseg.ReadBytes(4);
                    Array.Reverse(parameters);
                    value32 = BitConverter.ToUInt32(parameters, 0);
                    uint opint = new uint();
                    byte height = Convert.ToByte((value32 >> 14) & 0xF);
                    byte heightmode = Convert.ToByte((value32 >> 18) & 0xF);
                    byte width = Convert.ToByte((value32 >> 4) & 0xF);
                    byte widthmode = Convert.ToByte((value32 >> 8) & 0xF);

                    Param[0] = Convert.ToByte((height) << 4 | (heightmode));

                    opint = value32 >> 4;

                    Param[1] = Convert.ToByte((width) << 4 | (widthmode));

                    Array.Reverse(Param);

                    mainseg.BaseStream.Seek(4, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");
                    byte29 = mainseg.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");
                    ///MessageBox.Show(compar);
                    if (compar == "F51011000007C07C")
                    {
                        compressbyte = 0x2C;
                    }
                    if (compar == "F51010000007C07C")
                    {

                        compressbyte = 0x1A;
                    }
                    if (compar == "F5102000000FC07C")
                    {

                        compressbyte = 0x1B;
                    }
                    if (compar == "F51010000007C0FC")
                    {
                        compressbyte = 0x1C;
                    }
                    if (compar == "F57010000007C07C")
                    {
                        compressbyte = 0x1D;
                    }
                    if (compar == "F5702000000FC07C")
                    {
                        compressbyte = 0x1E;
                    }
                    if (compar == "F57010000007C0FC")
                    {
                        compressbyte = 0x1F;
                    }
                    ///MessageBox.Show(BitConverter.ToString(Param));
                    seg7w.Write(compressbyte);
                    seg7w.Write(Param);
                    ///don't ask me I don't know
                    ///don't ask me I don't know
                }
                if (commandbyte == "FD")
                {


                    ///0x20  ->  0x25


                    mainseg.BaseStream.Seek(1, SeekOrigin.Current);


                    byte[] Param = new byte[3];


                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    ///dont ask me I don't know
                    ///dont ask me I don't know
                    ///dont ask me I don't know

                    byte[] parambytes = mainseg.ReadBytes(4);
                    Array.Reverse(parambytes);
                    value32 = BitConverter.ToUInt32(parambytes, 0);


                    Param[0] = Convert.ToByte((value32 - 0x05000000) >> 11);
                    Param[1] = 0x00;
                    Param[2] = 0x70;
                    ///dont ask me I don't know
                    ///dont ask me I don't know
                    ///dont ask me I don't know
                    ///MessageBox.Show(value32.ToString("x")+"--"+Param[0].ToString("x"));
                    mainseg.BaseStream.Seek(28, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");
                    byte29 = mainseg.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");




                    if (compar == "0000073FF100")
                    {

                        compressbyte = 0x20;
                    }
                    if (compar == "0000077FF080")
                    {

                        compressbyte = 0x21;
                    }
                    if (compar == "0000077FF100")
                    {
                        compressbyte = 0x22;
                    }
                    if (compar == "0003073FF100")
                    {
                        compressbyte = 0x23;
                    }
                    if (compar == "0003077FF080")
                    {
                        compressbyte = 0x24;
                    }
                    if (compar == "0003077FF100")
                    {
                        compressbyte = 0x25;
                    }

                    seg7w.Write(compressbyte);
                    seg7w.Write(Param);


                }
                if (commandbyte == "BB")
                {

                    ///0x26 000001  FFFFFFFF
                    ///0x27    00010001
                    mainseg.BaseStream.Seek(3, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    if (compar == "0001")
                    {
                        compressbyte = 0x27;
                    }
                    if (compar == "FFFF")
                    {
                        compressbyte = 0x26;
                    }
                    seg7w.Write(compressbyte);

                    mainseg.BaseStream.Seek(2, SeekOrigin.Current);




                }

                if (commandbyte == "BF")
                {



                    compressbyte = 0x29;
                    seg7w.Write(compressbyte);
                    mainseg.BaseStream.Seek(4, SeekOrigin.Current);

                    indexC = mainseg.ReadByte();
                    indexB = mainseg.ReadByte();
                    indexA = mainseg.ReadByte();

                    indexA = indexA / 2;
                    indexB = indexB / 2;
                    indexC = indexC / 2;


                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((indexC) | (indexB << 5) | indexA << 10));


                    seg7w.Write(flip4);
                }
                if (commandbyte == "B8")
                {

                    ///0x2A
                    compressbyte = 0x2A;
                    seg7w.Write(compressbyte);
                    mainseg.BaseStream.Seek(7, SeekOrigin.Current);

                }
                if (commandbyte == "06")
                {

                    ///0x2B
                    compressbyte = 0x2B;


                    mainseg.BaseStream.Seek(3, SeekOrigin.Current);

                    byte[] parambytes = mainseg.ReadBytes(4);
                    Array.Reverse(parambytes);
                    value32 = BitConverter.ToUInt32(parambytes, 0);

                    value32 = value32 & 0x00FFFFFF;



                    seg7w.Write(compressbyte);
                    seg7w.Write(Convert.ToUInt16(value32 / 8));
                }
                if (commandbyte == "BE")
                {

                    ///0x2D
                    compressbyte = 0x2D;
                    mainseg.BaseStream.Seek(7, SeekOrigin.Current);
                    seg7w.Write(compressbyte);
                }
                if (commandbyte == "D0")
                {


                    mainseg.BaseStream.Seek(3, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    if (compar == "002E")
                    {
                        compressbyte = 0x2E;
                    }
                    if (compar == "002F")
                    {
                        compressbyte = 0x2F;
                    }
                    if (compar == "0030")
                    {
                        compressbyte = 0x30;
                    }

                    seg7w.Write(compressbyte);
                    mainseg.BaseStream.Seek(8, SeekOrigin.Current);
                }
                if (commandbyte == "04")
                {

                    ///0x33->0x52
                    mainseg.BaseStream.Seek(1, SeekOrigin.Current);
                    byte[] Param = mainseg.ReadBytes(2);
                    Array.Reverse(Param);
                    value16 = BitConverter.ToUInt16(Param, 0);

                    compressbyte = Convert.ToByte(((value16 + 1) / 0x410) + 0x32);
                    seg7w.Write(compressbyte);

                    byte[] parambytes = mainseg.ReadBytes(4);
                    Array.Reverse(parambytes);
                    value32 = BitConverter.ToUInt32(parambytes, 0);
                    value32 = (value32 - 0x04000000) / 16;

                    value16 = Convert.ToUInt16(value32);
                    seg7w.Write(value16);
                }

                if (commandbyte == "FC")
                {

                    ///0x53
                    compressbyte = 0x53;
                    mainseg.BaseStream.Seek(7, SeekOrigin.Current);
                    seg7w.Write(compressbyte);
                }

                if (commandbyte == "B9")
                {

                    mainseg.BaseStream.Seek(3, SeekOrigin.Current);
                    byte29 = mainseg.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    if (compar == "0044")
                    {
                        compressbyte = 0x54;
                    }
                    if (compar == "0040")
                    {
                        compressbyte = 0x55;
                    }
                    seg7w.Write(compressbyte);
                    mainseg.BaseStream.Seek(2, SeekOrigin.Current);




                }
                if (commandbyte == "B7")
                {

                    ///0x56
                    compressbyte = 0x56;
                    mainseg.BaseStream.Seek(15, SeekOrigin.Current);

                    seg7w.Write(compressbyte);
                }
                if (commandbyte == "B6")
                {

                    ///0x57
                    compressbyte = 0x57;
                    mainseg.BaseStream.Seek(15, SeekOrigin.Current);
                    seg7w.Write(compressbyte);
                }
                if (commandbyte == "B1")
                {

                    ///0x58
                    compressbyte = 0x58;
                    seg7w.Write(compressbyte);


                    indexC = mainseg.ReadByte();
                    indexB = mainseg.ReadByte();
                    indexA = mainseg.ReadByte();

                    indexA = indexA / 2;
                    indexB = indexB / 2;
                    indexC = indexC / 2;


                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((indexC) | (indexB << 5) | indexA << 10));


                    seg7w.Write(flip4);


                    ///twice for second set of verts
                    ///have to move the reader forward by 1 position first

                    mainseg.BaseStream.Seek(1, SeekOrigin.Current);

                    indexC = mainseg.ReadByte();
                    indexB = mainseg.ReadByte();
                    indexA = mainseg.ReadByte();

                    indexA = indexA / 2;
                    indexB = indexB / 2;
                    indexC = indexC / 2;

                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((indexC) | (indexB << 5) | indexA << 10));


                    seg7w.Write(flip4);





                }

            }


            flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFF000000));
            Array.Reverse(flip4);
            seg7w.Write(flip4);
            flip4 = BitConverter.GetBytes(Convert.ToUInt32(0));
            Array.Reverse(flip4);
            seg7w.Write(flip4);

            seg7w.Write(flip4);

            seg7w.Write(flip4);

            ///fin


            //MessageBox.Show("Compressed");
            byte[] seg7 = seg7m.ToArray();
            return (seg7);





        }
        public void WriteBatchTextures(string FolderPath, int Codec)
        {
            TM64 Tarmac = new TM64();
            N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.RGBA32, N64Codec.IA16, N64Codec.IA8, N64Codec.IA4, N64Codec.I8, N64Codec.I4, N64Codec.CI8, N64Codec.CI4 };


            string[] PNGList = Directory.GetFiles(FolderPath, "*.PNG", SearchOption.AllDirectories);
            string[] BMPList = Directory.GetFiles(FolderPath, "*.BMP", SearchOption.AllDirectories);
            string[] JPGList = Directory.GetFiles(FolderPath, "*.JPG", SearchOption.AllDirectories);
            string[] JPEGList = Directory.GetFiles(FolderPath, "*.JPEG", SearchOption.AllDirectories);
            string[] imageList = PNGList.Concat(BMPList).Concat(JPGList).Concat(JPEGList).ToArray();
            string parentDirectory = Path.Combine(FolderPath, "output");
            Directory.CreateDirectory(parentDirectory);

            string hText = "";
            
            string childDirectory = Path.Combine(parentDirectory, "ImageData");



            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            foreach (var textureAddress in imageList)
            {
                string fileName = Path.GetFileNameWithoutExtension(textureAddress);

                byte[] imageData = null;
                byte[] paletteData = null;
                Bitmap bitmapData = new Bitmap(textureAddress);
                N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[Codec], bitmapData);
                byte[] compressedTexture = Tarmac.CompressMIO0(imageData);

                int SegmentPosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(imageData);
                int PalettePosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                if (paletteData != null)
                {

                    binaryWriter.Write(paletteData);
                }


                hText += "#define " + fileName + "_Offset 0x" + SegmentPosition.ToString("X") + Environment.NewLine;
                hText += "#define " + fileName + "_Size 0x" + imageData.Length.ToString("X") + Environment.NewLine;
                if (paletteData != null)
                {
                    hText += "#define " + fileName + "_Offset 0x" + PalettePosition.ToString("X") + Environment.NewLine;
                    hText += "#define " + fileName + "_PaletteSize 0x" + paletteData.Length.ToString("X") + Environment.NewLine;
                }


            }


            File.WriteAllText(Path.Combine(childDirectory + ".h"), hText);
            File.WriteAllBytes(childDirectory + ".RAW", memoryStream.ToArray());
            File.WriteAllBytes(childDirectory + ".MIO0", Tarmac.CompressMIO0(memoryStream.ToArray()));

            binaryWriter.Close();
        }


        public void WriteTextureFile(string textureAddress, int CodecType)
        {
            TM64 Tarmac = new TM64();
            N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.RGBA32, N64Codec.IA16, N64Codec.IA8, N64Codec.IA4, N64Codec.I8, N64Codec.I4, N64Codec.CI8, N64Codec.CI4 };


            string parentDirectory = Path.Combine(Path.GetDirectoryName(textureAddress), "output");
            Directory.CreateDirectory(parentDirectory);
            string fileName = Path.GetFileName(textureAddress);
            string childDirectory = Path.Combine(parentDirectory, fileName);

            byte[] imageData = null;
            byte[] paletteData = null;
            Bitmap bitmapData = new Bitmap(textureAddress);
            N64Graphics.Convert(ref imageData, ref paletteData, n64Codec[CodecType], bitmapData);
            byte[] compressedTexture = Tarmac.CompressMIO0(imageData);
            byte[] TKMK = Tarmac.CompressTKMK(imageData, bitmapData.Width, bitmapData.Height);


            File.WriteAllBytes(childDirectory + ".RAW", imageData);
            File.WriteAllBytes(childDirectory + ".RAW.MIO0", compressedTexture);
            File.WriteAllBytes(childDirectory + ".RAW.TKMK00", TKMK);
            if (paletteData != null)
            {
                byte[] compressedPalette = Tarmac.CompressMIO0(paletteData);
                File.WriteAllBytes(childDirectory + ".PALETTE", paletteData);
                File.WriteAllBytes(childDirectory + ".PALETTE.MIO0", compressedTexture);
            }


            bitmapData.Dispose();
        }



    }

    

}

// phew! We made it. I keep saying we, but it's me doing all the work!
// maybe try pitching in sometime and updating the program! I'd love the help!

// Thank you so much for-a reading my source!

// OverKart 64 Library
// For Mario Kart 64 1.0 USA ROM
// <3 Hamp