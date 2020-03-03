using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace PeepsCompress
{

    class BigEndianBinaryWriter : BinaryWriter
    {


        public BigEndianBinaryWriter(Stream file)
            : base(file)
        {
        }
        public override void Write(byte[] buffer)
        {
            Array.Reverse(buffer);
            base.Write(buffer);
        }

        public override void Write(Single data)
        {
            byte[] buffer = new byte[4];
            buffer = BitConverter.GetBytes(data);
            Array.Reverse(buffer);
            data = BitConverter.ToSingle(buffer, 0);
            base.Write(data);
        }
        /*
        public override void Write(string value)
        {
            char[] array = value.ToCharArray();
            Array.Reverse(array);
            base.Write(array);
        }
        */
    }
}
