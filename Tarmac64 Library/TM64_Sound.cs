using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Tarmac64_Library
{
    public class TM64_Sound
    {

        byte[] flip = new byte[4];
        public class OK64Song
        {
            public byte[] SequenceData;
            public byte[] InstrumentData;
        }
        public OK64Song ExtractSong(string filePath, int BankID, int SequenceID)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            return ExtractSong(fileData, BankID, SequenceID);
        }
        public OK64Song ExtractSong(byte[] fileData, int BankID, int SequenceID)
        {
            OK64Song outputSong = new OK64Song();

            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            memoryStream.Write(fileData, 0, fileData.Length);
            


            binaryReader.BaseStream.Position = 0x966264 + (BankID * 8);

            flip = BitConverter.GetBytes(binaryReader.ReadInt32());
            Array.Reverse(flip);
            int Offset = BitConverter.ToInt32(flip, 0);


            flip = BitConverter.GetBytes(binaryReader.ReadInt32());
            Array.Reverse(flip);
            int DataLength = BitConverter.ToInt32(flip, 0);

            binaryReader.BaseStream.Position = 0x966260 + Offset;
            outputSong.InstrumentData = binaryReader.ReadBytes(DataLength);

            binaryReader.BaseStream.Position = 0xBC5F64 + (SequenceID * 8);

            flip = BitConverter.GetBytes(binaryReader.ReadInt32());
            Array.Reverse(flip);
            Offset = BitConverter.ToInt32(flip, 0);


            flip = BitConverter.GetBytes(binaryReader.ReadInt32());
            Array.Reverse(flip);
            DataLength = BitConverter.ToInt32(flip, 0);


            binaryReader.BaseStream.Position = 0xBC5F60 + Offset;
            outputSong.SequenceData = binaryReader.ReadBytes(DataLength);


            return outputSong;
        }
        public OK64Song LoadSong(string filePath)
        {
            byte[] compressedData = File.ReadAllBytes(filePath);
            TM64 Tarmac = new TM64();
            byte[] fileData = Tarmac.DecompressMIO0(compressedData);
            return LoadSong(fileData);            
        }
        public OK64Song LoadSong(byte[] fileData)
        {
            
            OK64Song outputSong = new OK64Song();

            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);


            memoryStream.Write(fileData, 0, fileData.Length);
            memoryStream.Position = 0;

            flip = BitConverter.GetBytes(binaryReader.ReadInt32());
            Array.Reverse(flip);
            int DataLength = BitConverter.ToInt32(flip, 0);

            outputSong.SequenceData = binaryReader.ReadBytes(DataLength);


            int addressAlign = 64 - (Convert.ToInt32(binaryReader.BaseStream.Position) % 16);
            if (addressAlign == 64)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryReader.BaseStream.Position += 1;
            }

            flip = BitConverter.GetBytes(binaryReader.ReadInt32());
            Array.Reverse(flip);
            DataLength = BitConverter.ToInt32(flip, 0);

            outputSong.InstrumentData = binaryReader.ReadBytes(DataLength);

            return outputSong;

        }
        public byte[] SaveSong(OK64Song inputSong)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            int DataValue = 0;

            flip = BitConverter.GetBytes(Convert.ToInt32(inputSong.SequenceData.Length));
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            binaryWriter.Write(inputSong.SequenceData);

            int addressAlign = 64 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 64);
            if (addressAlign == 64)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            flip = BitConverter.GetBytes(Convert.ToInt32(inputSong.InstrumentData.Length));
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            binaryWriter.Write(inputSong.InstrumentData);

            addressAlign = 64 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 64);
            if (addressAlign == 64)
                addressAlign = 0;

            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            byte[] outputFile = memoryStream.ToArray();

            return outputFile;
        }
    }
}
