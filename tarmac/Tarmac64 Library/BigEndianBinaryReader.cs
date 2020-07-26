using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PeepsCompress
{
    class BigEndianBinaryReader : BinaryReader
    {

        public BigEndianBinaryReader(Stream file)
            : base(file)
        {
        }
        public override UInt16 ReadUInt16()
        {
            byte[] int16 = base.ReadBytes(2);
            Array.Reverse(int16);
            return BitConverter.ToUInt16(int16, 0);
        }
        public override Int16 ReadInt16()
        {
            byte[] int16 = base.ReadBytes(2);
            Array.Reverse(int16);
            return BitConverter.ToInt16(int16, 0);
        }
        public override UInt32 ReadUInt32()
        {
            byte[] int32 = base.ReadBytes(4);
            Array.Reverse(int32);
            return BitConverter.ToUInt32(int32, 0);
        }
        public override int ReadInt32()
        {
            byte[] int32 = base.ReadBytes(4);
            Array.Reverse(int32);
            return BitConverter.ToInt32(int32, 0);
        }
        public override Int64 ReadInt64()
        {
            byte[] int64 = base.ReadBytes(8);
            Array.Reverse(int64);
            return BitConverter.ToInt64(int64, 0);
        }
        public override float ReadSingle()
        {
            byte[] flt = base.ReadBytes(4);
            Array.Reverse(flt);
            return BitConverter.ToSingle(flt, 0);
        }


        /*
        public override byte ReadByte()
        {
            byte[] int8[0] = base.ReadByte();
            Array.Reverse(int8);
            return Convert.ToByte(BitConverter.ToBoolean(int8, 0));
        }
        */
    }
}