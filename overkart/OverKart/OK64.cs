using PeepsCompress;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace OverKart64
{
    class OK64
    {

        // These are various functions for decompressing and handling the segment data for Mario Kart 64.
        // <3 Hamp


        byte[] flip2 = new byte[2];
        byte[] flip4 = new byte[4];

        UInt16 value16 = new UInt16();
        Int16 valuesign16 = new Int16();

        UInt32 value32 = new UInt32();
        Int32 valuesign32 = new Int32();


        public string translate_F3D(byte commandbyte, BinaryReader mainseg, BinaryReader seg4r, int vertoffset)
        {
            // mainseg is the segment that contained the F3DEX command. for Mario Kart 64 it will most likely be Seg6 or Seg7.
            // seg4r is an uncompressed Segment 4. This contains all the vertices for a Mario Kart 64 course.

            // if you don't need to draw triangles, you can pass any value for vertoffset. Otherwise we need to know the last position
            // a vert was loaded from, and the program will treat that position as the new 0 index. Not the same process but the same result.

            // The Vert Offset is a manipulation of two specific "quirks" to how Mario Kart 64 loads vertices. 
            // F3DEX has 32 vert registers, meaning you can only load 32 verts at the same time. Command 0x04 loads them
            // 0x04 loads a certain number of verts from an offset into segment 4 at a certain index in the current vert register. 
            // So it can for example load 5 vertices from offset 0x4FF0 into segment 4 starting at vert index 7, replacing verts 7-12. 
            // HOWEVER, it never does this! it always loads the verts at index 0! 
            // Because it always loads to index 0 and because we have access to the entire segment 4 vert cache, we can cheat! :)
            // When we get a vert index we multiply it by the size of the vert structure (14 bytes compressed / 16 bytes uncompressed) and add this 
            // to the vert offset loaded from 0x04. This becomes an offset directly to that verts data in Segment 4. This is much easier and quicker.
            // Now for command 0x04 we only set the vertoffset to the value in the F3DEX command. It will ALWAYS be segment 4 for Mario Kart 64....
            // but if it ever comes across verts outside segment 4 it will throw up an error message to warn the user. 

            // The commands for 0xB1 and 0xBF return the 3 vert positions seperated by , with each vert seperated by ;
            // there is an alternative that will return a maxscript command to be used by an accompanying script.

            // command 0x06 will return the segment and offset of the display lists to run on seperate lines.

            // command 0xB8 represents the end of a display list and will return "ENDSECTION"

            // command 0x04 will return the vertoffset described above, which should be updated and maintained by the calling function to be passed again.
            // translate_F3D needs a proper vertoffset provided every time for either 0xB1 or 0xBF commands, it is not maintained automatically.


            int v0 = new int();
            int v1 = new int();
            int v2 = new int();

            int[] xval = new int[3];
            int[] yval = new int[3];
            int[] zval = new int[3];
            int[] sval = new int[3];
            int[] tval = new int[3];




            string outputstring = "";

            //mainseg Either Seg6 or Seg7 Uncompressed
            //seg4r Seg4 Uncompressed
            bool breakoff = true;

            if (commandbyte == 0xE4)
            {

            }
            if (commandbyte == 0xB1)
            {
                for (int i = 0; i < 2; i++)
                {



                    v0 = mainseg.ReadByte();
                    v1 = mainseg.ReadByte();
                    v2 = mainseg.ReadByte();

                    v0 = v0 / 2;
                    v1 = v1 / 2;
                    v2 = v2 / 2;

                    //outputstring = outputstring + v0.ToString() + "-" + v1.ToString() + "-" + v2.ToString() + "-" + vertoffset.ToString() +"-"+ mainseg.BaseStream.Position.ToString() + Environment.NewLine;
                    // outputs the 3 vert indexes as well as the vertoffset and offset into the segment that called this command.


                    //
                    seg4r.BaseStream.Seek(vertoffset + (v0 * 16), SeekOrigin.Begin);

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    xval[0] = BitConverter.ToInt16(flip2, 0); //x

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    zval[0] = BitConverter.ToInt16(flip2, 0); //z

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    yval[0] = BitConverter.ToInt16(flip2, 0); //y
                    //


                    //
                    seg4r.BaseStream.Seek(vertoffset + (v1 * 16), SeekOrigin.Begin);

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    xval[1] = BitConverter.ToInt16(flip2, 0); //x

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    zval[1] = BitConverter.ToInt16(flip2, 0); //z

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    yval[1] = BitConverter.ToInt16(flip2, 0); //y
                    //


                    //
                    seg4r.BaseStream.Seek(vertoffset + (v2 * 16), SeekOrigin.Begin);

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    xval[2] = BitConverter.ToInt16(flip2, 0); //x

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    zval[2] = BitConverter.ToInt16(flip2, 0); //z

                    flip2 = seg4r.ReadBytes(2);
                    Array.Reverse(flip2);
                    yval[2] = BitConverter.ToInt16(flip2, 0); //y
                    //

                    outputstring = outputstring + "vertbox = mesh vertices:#([" + xval[0].ToString() + ",(" + (0 - yval[0]).ToString() + ")," + zval[0].ToString() + "],[" + xval[1].ToString() + ",(" + (0 - yval[1]).ToString() + ")," + zval[1].ToString() + "],[" + xval[2].ToString() + ",(" + (0 - yval[2]).ToString() + ")," + zval[2].ToString() + "]) faces:#([1,2,3]) MaterialIDS:#(1) " + Environment.NewLine;
                    //outputstring = outputstring + xval[0].ToString() + "," + yval[0].ToString() + "," + zval[0].ToString() + ";" + xval[1].ToString() + "," + yval[1].ToString() + "," + zval[1].ToString() + ";" + xval[2].ToString() + "," + yval[2].ToString() + "," + zval[2].ToString() + "," + Environment.NewLine;
                    //
                    mainseg.BaseStream.Seek(1, SeekOrigin.Current);
                }
            }
            if (commandbyte == 0xBF)
            {

                mainseg.BaseStream.Seek(4, SeekOrigin.Current);


                v0 = mainseg.ReadByte();
                v1 = mainseg.ReadByte();
                v2 = mainseg.ReadByte();

                v0 = v0 / 2;
                v1 = v1 / 2;
                v2 = v2 / 2;

                //outputstring = outputstring + v0.ToString() + "-" + v1.ToString() + "-" + v2.ToString() + "-" + vertoffset.ToString() + "-" + mainseg.BaseStream.Position.ToString() + Environment.NewLine;
                // outputs the 3 vert indexes as well as the vertoffset and offset into the segment that called this command.


                //
                seg4r.BaseStream.Seek(vertoffset + (v0 * 16), SeekOrigin.Begin);

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                xval[0] = BitConverter.ToInt16(flip2, 0); //x

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                zval[0] = BitConverter.ToInt16(flip2, 0); //z

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                yval[0] = BitConverter.ToInt16(flip2, 0); //y
                //


                //
                seg4r.BaseStream.Seek(vertoffset + (v1 * 16), SeekOrigin.Begin);

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                xval[1] = BitConverter.ToInt16(flip2, 0); //x

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                zval[1] = BitConverter.ToInt16(flip2, 0); //z

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                yval[1] = BitConverter.ToInt16(flip2, 0); //y
                                                          //


                //
                seg4r.BaseStream.Seek(vertoffset + (v2 * 16), SeekOrigin.Begin);

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                xval[2] = BitConverter.ToInt16(flip2, 0); //x

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                zval[2] = BitConverter.ToInt16(flip2, 0); //z

                flip2 = seg4r.ReadBytes(2);
                Array.Reverse(flip2);
                yval[2] = BitConverter.ToInt16(flip2, 0); //y
                                                          //

                outputstring = outputstring + "vertbox = mesh vertices:#([" + xval[0].ToString() + ",(" + (0 - yval[0]).ToString() + ")," + zval[0].ToString() + "],[" + xval[1].ToString() + ",(" + (0 - yval[1]).ToString() + ")," + zval[1].ToString() + "],[" + xval[2].ToString() + ",(" + (0 - yval[2]).ToString() + ")," + zval[2].ToString() + "]) faces:#([1,2,3]) MaterialIDS:#(1) " + Environment.NewLine;
                //outputstring = outputstring + xval[0].ToString() + "," + yval[0].ToString() + "," + zval[0].ToString() + ";" + xval[1].ToString() + "," + yval[1].ToString() + "," + zval[1].ToString() + ";" + xval[2].ToString() + "," + yval[2].ToString() + "," + zval[2].ToString() + "," + Environment.NewLine;
                //
                mainseg.BaseStream.Seek(1, SeekOrigin.Current);





            }
            if (commandbyte == 0x04)
            {

                mainseg.BaseStream.Seek(3, SeekOrigin.Current);
                byte[] rsp_add = mainseg.ReadBytes(4);

                Array.Reverse(rsp_add);

                int Value = BitConverter.ToInt32(rsp_add, 0);
                String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                int segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                uint location = Convert.ToUInt32(Binary.Substring(8, 24), 2);




                if (segid == 4)
                {
                    outputstring = location.ToString();
                    //MessageBox.Show(outputstring +"-"+ mainseg.BaseStream.Position.ToString());
                }
                else
                {
                    MessageBox.Show("ERROR D35-01 :: VERTS LOADED FROM OUTSIDE SEGMENT 4");
                }


            }


            if (commandbyte == 0x06)
            {
                mainseg.BaseStream.Seek(3, SeekOrigin.Current);

                byte[] rsp_add = mainseg.ReadBytes(4);

                Array.Reverse(rsp_add);

                int Value = BitConverter.ToInt32(rsp_add, 0);
                String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                int segid = Convert.ToByte(Binary.Substring(0, 8), 2);
                int location = Convert.ToInt32(Binary.Substring(8, 24), 2);



                outputstring = segid + Environment.NewLine + location.ToString() + Environment.NewLine;

            }

            if (commandbyte == 0xB8)
            {
                outputstring = "ENDSECTION" + Environment.NewLine;
            }




            return outputstring;

        }



        public byte[] dump_ASM(string filePath)
        {

            // This is specfically designed for Mario Kart 64 USA 1.0 ROM. It should dump to binary the majority of it's ASM commands.
            // It uses a list provided by MiB to find the ASM sections, there could be plenty of code I'm missing

            byte[] rombytes = File.ReadAllBytes(filePath);
            byte[] asm = new byte[1081936];

            byte[] buffer = new byte[1];
            buffer[0] = 0xFF;


            Buffer.BlockCopy(rombytes, 4096, asm, 0, 887664);

            for (int i = 0; i < 8; i++)
            {
                Buffer.BlockCopy(buffer, 0, asm, 887664 + i, 1);
            }

            Buffer.BlockCopy(rombytes, 1013008, asm, 887672, 174224);

            for (int i = 0; i < 8; i++)
            {
                Buffer.BlockCopy(buffer, 0, asm, 1061896 + i, 1);
            }

            Buffer.BlockCopy(rombytes, 1193536, asm, 1061904, 20032);
            return asm;

        }



        public void translate_ASM(string savePath, string filePath)
        {

            // This is specfically designed for Mario Kart 64 USA 1.0 ROM. It should convert to plaintext the majority of it's ASM commands.            
            // Also, there are a few ASM commands that MK64 uses that I currently haven't defined yet. 



            byte[] asm = dump_ASM(filePath);



            MemoryStream asmm = new MemoryStream(asm);
            BinaryReader asmr = new BinaryReader(asmm);
            string output = "";
            byte[] asmbytes = new byte[4];
            int compare = new int();
            byte commandbyte = new byte();
            bool unknown = false;
            asmr.BaseStream.Seek(0, SeekOrigin.Begin);

            bool debug_bool = false;
            bool combo = true;

            Int16 rt = new Int16();
            Int16 rs = new Int16();
            Int16 rd = new Int16();
            Int16 sa = new Int16();

            Int32 target = new Int32();
            Int32 asmbase = new Int32();

            float fs = new float();
            float ft = new float();
            float fd = new float();

            byte[] immbyte = new byte[2];

            Int16 imm = new Int16();
            Int16 offset = new Int16();


            long current_offset = 0;

            for (int i = 0; i < asm.Length; i += 4)
            {


                asmr.BaseStream.Seek(current_offset, SeekOrigin.Begin);
                asmbytes = asmr.ReadBytes(4);

                commandbyte = asmbytes[0];

                debug_bool = false;  //set FALSE to ONLY print debug commands

                unknown = true;
                String CommandBinary = Convert.ToString(commandbyte, 2).PadLeft(8, '0');
                compare = Convert.ToInt16(CommandBinary.Substring(0, 6), 2);
                //MessageBox.Show("Command "+compare.ToString()+"- 0x "+BitConverter.ToString(asmbytes).Replace("-", " "));
                if (compare == 1)
                {
                    Array.Copy(asmbytes, 0, flip4, 0, 4);
                    Array.Reverse(flip4);
                    int Value = BitConverter.ToInt32(flip4, 0);
                    String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);

                    if (rt == 0)
                    {
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLTZ} Branch on Less than Zero - If the value at register " + rs.ToString() + " is < 0 then Branch within the 256MB region at 0x" + Value.ToString("X");

                    }
                    if (rt == 1)
                    {
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BGEZ} Branch on Greater than or Equal to Zero - If the value at register " + rs.ToString() + " is >= 0 then Branch within the 256MB region at 0x" + Value.ToString("X");

                    }
                    if (rt == 2)
                    {
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLTZL} Branch on Less than Zero Likely - If the value at register " + rs.ToString() + " is < 0 then Branch within the 256MB region at 0x" + Value.ToString("X");

                    }
                    if (rt == 3)
                    {
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BGEZL} Branch on Greater than or Equal to Zero Likely - If the value at register " + rs.ToString() + " is >= 0 then Branch within the 256MB region at 0x" + Value.ToString("X");

                    }
                    if (rt == 16)
                    {
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLTZAL} Branch on Less than or Equal to Zero And Link- If the value at register " + rs.ToString() + " is  < 0 then Branch within the 256MB region at 0x" + Value.ToString("X") + " and return address";

                    }
                    if (rt == 17)
                    {
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BGEZAL} Branch on Greater than or Equal to Zero And Link- If the value at register " + rs.ToString() + " is >= 0 then Branch within the 256MB region at 0x" + Value.ToString("X") + " and return address";

                    }
                    if (rt == 18)
                    {
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLTZALL} Branch on Less than or Equal to Zero And Link Likely- If the value at register " + rs.ToString() + " is < 0 then Branch within the 256MB region at 0x" + Value.ToString("X") + " and return address";

                    }
                    if (rt == 19)
                    {
                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BGEZALL} Branch on Greater than or Equal to Zero And Link Likely- If the value at register " + rs.ToString() + " is >= 0 then Branch within the 256MB region at 0x" + Value.ToString("X") + " and return address";

                    }







                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 2)
                {
                    Array.Copy(asmbytes, 0, flip4, 0, 4);
                    Array.Reverse(flip4);
                    int Value = BitConverter.ToInt32(flip4, 0);
                    String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                    Value = Convert.ToInt32(Binary.Substring(6, 26), 2);






                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{J} Jump - Branch within the 256MB region at 0x" + Value.ToString("X");

                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 3)
                {
                    Array.Copy(asmbytes, 0, flip4, 0, 4);
                    Array.Reverse(flip4);
                    int Value = BitConverter.ToInt32(flip4, 0);
                    String Binary = Convert.ToString(Value, 2).PadLeft(32, '0');


                    Value = Convert.ToInt32(Binary.Substring(6, 26), 2);




                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{JAL} Jump and Link - Procedure Call within the 256MB region at 0x" + Value.ToString("X");

                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 4)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BEQ} Branch on Equal - If the values are equal at register " + rs.ToString() + " and at register " + rt.ToString() + "then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");

                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 5)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);


                    debug_bool = true;


                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BNE} Branch on Not Equal - If the values are not equal at register " + rs.ToString() + " and at register " + rt.ToString() + "then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");

                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 8)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ADDI} ADD Immediate Signed Word - Add the Signed value 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the Signed value at register " + rs.ToString() + " and write it to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 9)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);


                    debug_bool = true;


                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ADDIU} ADD Immediate Unsigned Word - Add the Unsigned value 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the Unsigned value at register " + rs.ToString() + " and write it to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 10)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SLTI} Set on Less Than Immediate - Set a 0/1 True/False value at register " + rt.ToString() + " if the Signed value at register " + rs.ToString() + " is less than the Signed value " + BitConverter.ToString(immbyte).Replace("-", "");
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 11)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SLTIU} Set on Less Than Immediate Unsigned - Set a 0/1 True/False value at register " + rt.ToString() + " if the Unsigned value at register " + rs.ToString() + " is less than the Unsigned value " + BitConverter.ToString(immbyte).Replace("-", "");
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 12)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ANDI} AND Immediate- Perform a Bitwise Logical AND for the value at register " + rs.ToString() + " and the value " + BitConverter.ToString(immbyte).Replace("-", "") + " and write it to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 13)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ORI} OR Immediate- Perform a Bitwise Logical OR for the value at register " + rs.ToString() + " and the value " + BitConverter.ToString(immbyte).Replace("-", "") + " and write it to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 14)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{XORI} XOR Immediate- Perform a Bitwise Logical XOR for the value at register " + rs.ToString() + " and the value " + BitConverter.ToString(immbyte).Replace("-", "") + " and write it to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }

                if (compare == 15)
                {

                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    //MessageBox.Show(rt.ToString());

                    Array.Copy(asmbytes, 2, immbyte, 0, 2);



                    debug_bool = true;



                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LUI} Load Upper Immediate - at register " + rt.ToString() + " load value 0x" + BitConverter.ToString(immbyte).Replace("-", "");
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 20)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BEQL} Branch on Equal Likely- If the values are equal at register " + rs.ToString() + " and at register " + rt.ToString() + "then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 21)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BNEL} Branch on NOT Equal Likely- If the values are not equal at register " + rs.ToString() + " and at register " + rt.ToString() + "then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 22)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);

                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLEZL} Branch on Less Than or Equal to 0 Likely- If the value at register " + rs.ToString() + " is less than or equal to 0 then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 23)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);

                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{BLEZL} Branch on Greater Than or Equal to 0 Likely- If the value at register " + rs.ToString() + " is greater than or equal to 0 then branch to 0x" + BitConverter.ToString(immbyte).Replace("-", "");
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 24)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);




                    //Oh, behave.
                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{DADDI} Doubleword ADD Immediate- Add the value at register " + rs.ToString() + " to the value 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " and write it to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 25)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{DADDIU} Doubleword ADD Unsigned Immediate- Add the value at register " + rs.ToString() + " to the value 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " and write it to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 26)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LDL} Load Doubleword Left- Reads the value at register " + rs.ToString() + " and sets the Most-Significant bytes to the value of ( 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " and writes it to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 27)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LDL} Load Doubleword Right- Reads the value at register " + rs.ToString() + " and sets the Least-Significant bytes to the value of ( 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " and writes it to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 32)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);


                    debug_bool = true;


                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LB} Load Byte- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to a signed byte that is written to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 33)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LH} Load Halfword- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to a signed Halfword that is written to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 34)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LWL} Load World Left- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to a signed word whose Most-Significant bytes are added to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 35)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LW} Load Word- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to a signed word that is loaded to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 36)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LBU} Load Unsigned Byte- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to an unsigned byte that is written to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 37)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LHU} Load Halfword- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to an unsigned Halfword that is written to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 38)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LWLU} Load World Left- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to an unsigned word whose Most-Significant bytes are added to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 39)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{LWU} Load Word- Adds the value of 0x" + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " to form the address to an unsigned word that is loaded to register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 40)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SB} Store Byte- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the byte at register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 41)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SH} Store HalfWord- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the halfword at register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 42)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SWL} Store Word Left- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the Most Significant bytes from the word at register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 43)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SW} Store Word- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the word at register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 44)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SDL} Store Doubleword Left- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the Most-Significant bytes of the Doubleword at register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 45)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SDR} Store Doubleword Right- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the Least-Significant bytes of the Doubleword at register " + rt.ToString();
                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 46)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SWR} Store Word Right- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the Least Significant bytes from the word at register " + rt.ToString();                    //MessageBox.Show(output);
                    unknown = false;
                }
                if (compare == 61)
                {
                    Array.Copy(asmbytes, 0, flip2, 0, 2);
                    Array.Reverse(flip2);
                    valuesign16 = BitConverter.ToInt16(flip2, 0);
                    String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');


                    rs = Convert.ToInt16(Binary.Substring(6, 5), 2);   //base?
                    rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                    Array.Copy(asmbytes, 2, immbyte, 0, 2);





                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SDC1} Store Doubleword from Float- Creates a Memory Address by adding " + BitConverter.ToString(immbyte).Replace("-", "") + " to the base at register " + rs.ToString() + " and writes the doubleword located at register " + rt.ToString();                    //MessageBox.Show(output);
                    unknown = false;
                }


                if (compare == 0xFF)
                {
                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "-BREAK-";
                    unknown = false;
                }



                // If commandbyte is 0x00, there is a secondary commandbyte at the far end
                if (compare == 0x00)
                {
                    commandbyte = asmbytes[3];
                    CommandBinary = Convert.ToString(commandbyte, 2).PadLeft(8, '0');
                    compare = Convert.ToInt16(CommandBinary.Substring(2, 6), 2);

                    //MessageBox.Show(commandbyte.ToString() + "-" + compare.ToString() + "---" + BitConverter.ToString(asmbytes).Replace("-", " "));
                    if (compare == 0)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);
                        sa = Convert.ToInt16(Binary.Substring(21, 5), 2);

                        if (rt != 0 || rd != 0 || sa != 0)  // If all values are 0 then the hex string was [00 00 00 00] and can be skipped.
                        {


                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SLL} Shift Word Left Logical - Left-Shift the word at register " + rt.ToString() + " by " + sa.ToString() + " and write it to register " + rd.ToString();
                            //MessageBox.Show(output);
                        }
                        else
                        {

                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "[00 00 00 00]";
                        }
                    }
                    if (compare == 2)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);
                        sa = Convert.ToInt16(Binary.Substring(21, 5), 2);

                        if (rt != 0 || rd != 0 || sa != 0)  // If all values are 0 then the hex string was [00 00 00 00] and can be skipped.
                        {


                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SRL} Shift Word Right Logical - Left-Shift the word at register " + rt.ToString() + " by " + sa.ToString() + " and write it to register " + rd.ToString();
                            //MessageBox.Show(output);
                        }
                    }
                    if (compare == 3)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);
                        sa = Convert.ToInt16(Binary.Substring(21, 5), 2);

                        if (rt != 0 || rd != 0 || sa != 0)  // If all values are 0 then the hex string was [00 00 00 00] and can be skipped.
                        {


                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SRL} Shift Word Right Arithmetic - Left-Shift the word at register " + rt.ToString() + " by " + sa.ToString() + " and write it to register " + rd.ToString();
                            //MessageBox.Show(output);
                        }
                    }
                    if (compare == 7)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);


                        if (rt != 0 || rd != 0 || sa != 0)  // If all values are 0 then the hex string was [00 00 00 00] and can be skipped.
                        {


                            output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SRL} Shift Word Right Arithmetic - Left-Shift the word at register " + rt.ToString() + " by the amount at register " + rs.ToString() + " and write it to register " + rd.ToString();
                            //MessageBox.Show(output);
                        }
                    }
                    if (compare == 8)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);









                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{JR} Jump Register - Jump to Address in Register " + rs.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 16)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 2, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                        rd = Convert.ToInt16(Binary.Substring(0, 5), 2);







                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{MFHI} Move From HI Register - Move the special HI register to register " + rd.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 18)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 2, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                        rd = Convert.ToInt16(Binary.Substring(0, 5), 2);









                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{MFLO} Move From LO Register - Move the special HI register to register " + rd.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 24)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 2, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{MULT} Multiply Word- Multiply Signed 32 Bit Integers at  register " + rs.ToString() + " and register " + rt.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 26)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 2, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{DIV} Divide Word- Divide Signed 32 Bit Integer at register " + rs.ToString() + " by register " + rt.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 27)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 2, flip2, 0, 2);
                        Array.Reverse(flip2);
                        valuesign16 = BitConverter.ToInt16(flip2, 0);
                        String Binary = Convert.ToString(valuesign16, 2).PadLeft(16, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{DIV} Divide Word- Divide Unsigned 32 Bit Integer at register " + rs.ToString() + " by register " + rt.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 32)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ADD} Add Word- Add the Signed Word at register " + rs.ToString() + " to register " + rt.ToString() + " and write it to register " + rd.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 33)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{ADDU} Add Word- Add the Unsigned Word at register " + rs.ToString() + " to register " + rt.ToString() + " and write it to register " + rd.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 34)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SUB} Subtract Word- Subtract the Signed Word at register " + rt.ToString() + " from the Word at register " + rt.ToString() + " and write it to register " + rd.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 35)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SUBU} Subtract Word- Subtract the Unsigned Word at register " + rt.ToString() + " from the Word at register " + rt.ToString() + " and write it to register " + rd.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 36)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{AND} AND- Perform a Bitwise Logical AND for the value at register " + rs.ToString() + " and the value at register " + rt.ToString() + " and write it to register " + rd.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 37)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{OR} OR- Perform a Bitwise Logical OR for the value at register " + rs.ToString() + " and the value at register " + rt.ToString() + " and write it to register " + rd.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 37)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{OR} OR- Perform a Bitwise Logical OR for the value at register " + rs.ToString() + " and the value at register " + rt.ToString() + " and write it to register " + rd.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 38)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{XOR} XOR- Perform a Bitwise Logical XOR for the value at register " + rs.ToString() + " and the value at register " + rt.ToString() + " and write it to register " + rd.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 42)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SLT} Set on Less Than- Set a 0/1 True/False value at register " + rd.ToString() + " if the Signed value at register " + rs.ToString() + " is less than the Signed value at register " + rt.ToString();
                        //MessageBox.Show(output);
                    }
                    if (compare == 43)
                    {
                        unknown = false;

                        Array.Copy(asmbytes, 0, flip4, 0, 4);
                        Array.Reverse(flip4);
                        int value32 = BitConverter.ToInt32(flip4, 0);
                        String Binary = Convert.ToString(value32, 2).PadLeft(32, '0');

                        rs = Convert.ToInt16(Binary.Substring(6, 5), 2);
                        rt = Convert.ToInt16(Binary.Substring(11, 5), 2);
                        rd = Convert.ToInt16(Binary.Substring(16, 5), 2);






                        output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "{SLTU} Set on Less Than- Set a 0/1 True/False value at register " + rd.ToString() + " if the Unsigned value at register " + rs.ToString() + " is less than the Unsigned value at register " + rt.ToString();
                        //MessageBox.Show(output);
                    }
                }



                if (unknown)
                {
                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "";
                    //MessageBox.Show("-Unknown Command 0x" + compare.ToString("X").PadLeft(2, '0') + "-  @0x" + current_offset.ToString("X").PadLeft(2, '0'));
                    output = "  || [" + BitConverter.ToString(asmbytes).Replace("-", " ") + "]  ||  " + "-Unknown Command -" + compare.ToString() + "-  @0x" + current_offset.ToString("X").PadLeft(2, '0');
                }

                current_offset += 4;
                asmr.BaseStream.Seek(current_offset, SeekOrigin.Begin);


                if (debug_bool)
                {



                    output = "0x" + (asmr.BaseStream.Position - 4).ToString("X").PadLeft(8, '0') + output;

                    if (output != "")
                    {
                        System.IO.File.AppendAllText(savePath, output + Environment.NewLine);
                    }
                }
                else
                {
                    combo = false;
                }
            }







        }




        // https://github.com/Daniel-McCarthy/Mr-Peeps-Compressor 



        public int[] findAllMatches(ref List<byte> dictionary, byte match)
        {
            List<int> matchPositons = new List<int>();

            for (int i = 0; i < dictionary.Count; i++)
            {
                if (dictionary[i] == match)
                {
                    matchPositons.Add(i);
                }
            }

            return matchPositons.ToArray();
        }

        public int[] findLargestMatch(ref List<byte> dictionary, int[] matchesFound, ref byte[] file, int fileIndex, int maxMatch)
        {
            int[] matchSizes = new int[matchesFound.Length];

            for (int i = 0; i < matchesFound.Length; i++)
            {
                int matchSize = 1;
                bool matchFound = true;

                while (matchFound && matchSize < maxMatch && (fileIndex + matchSize < file.Length) && (matchesFound[i] + matchSize < dictionary.Count)) //NOTE: This could be relevant to compression issues? I suspect it's more related to writing
                {
                    if (file[fileIndex + matchSize] == dictionary[matchesFound[i] + matchSize])
                    {
                        matchSize++;
                    }
                    else
                    {
                        matchFound = false;
                    }

                }

                matchSizes[i] = matchSize;
            }

            int[] bestMatch = new int[2];

            bestMatch[0] = matchesFound[0];
            bestMatch[1] = matchSizes[0];

            for (int i = 1; i < matchesFound.Length; i++)
            {
                if (matchSizes[i] > bestMatch[1])
                {
                    bestMatch[0] = matchesFound[i];
                    bestMatch[1] = matchSizes[i];
                }
            }

            return bestMatch;

        }



        public List<byte> decompress_MIO0(int offset, string path)
        {

            // This is Peep's Decompression Algorithim for MIO0 decompression. 
            // It's pretty much taken verbatim with a couple adjustments to variable names.
            // Thanks.

            FileStream inputFile = File.Open(path, FileMode.Open);
            BigEndianBinaryReader mio0r = new BigEndianBinaryReader(inputFile);

            byte[] file = mio0r.ReadBytes((int)inputFile.Length);


            List<byte> newFile = new List<byte>();


            mio0r.BaseStream.Position = offset;
            string magicNumber = Encoding.ASCII.GetString(mio0r.ReadBytes(4));

            if (magicNumber == "MIO0")
            {
                int decompressedLength = mio0r.ReadInt32();
                int compressedOffset = mio0r.ReadInt32() + offset;
                int uncompressedOffset = mio0r.ReadInt32() + offset;
                int currentOffset;

                try
                {

                    while (newFile.Count < decompressedLength)
                    {

                        byte bits = mio0r.ReadByte(); //byte of layout bits
                        BitArray arrayOfBits = new BitArray(new byte[1] { bits });

                        for (int i = 7; i > -1 && (newFile.Count < decompressedLength); i--) //iterate through layout bits
                        {

                            if (arrayOfBits[i] == true)
                            {
                                //non-compressed
                                //add one byte from uncompressedOffset to newFile

                                currentOffset = (int)inputFile.Position;

                                inputFile.Seek(uncompressedOffset, SeekOrigin.Begin);

                                newFile.Add(mio0r.ReadByte());
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

                                byte byte1 = mio0r.ReadByte();
                                byte byte2 = mio0r.ReadByte();
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




            }
            inputFile.Close();
            return newFile;
        }


        public List<byte> decompress_MIO0(int offset, byte[] file)
        {

            // This is Peep's Decompression Algorithim for MIO0 decompression. 
            // It's pretty much taken verbatim with a couple adjustments to variable names.
            // Thanks.

            
            MemoryStream inputFile = new MemoryStream(file);
            BigEndianBinaryReader mio0r = new BigEndianBinaryReader(inputFile);

            List<byte> newFile = new List<byte>();


            mio0r.BaseStream.Position = offset;
            string magicNumber = Encoding.ASCII.GetString(mio0r.ReadBytes(4));

            if (magicNumber == "MIO0")
            {
                int decompressedLength = mio0r.ReadInt32();
                int compressedOffset = mio0r.ReadInt32() + offset;
                int uncompressedOffset = mio0r.ReadInt32() + offset;
                int currentOffset;

                try
                {

                    while (newFile.Count < decompressedLength)
                    {

                        byte bits = mio0r.ReadByte(); //byte of layout bits
                        BitArray arrayOfBits = new BitArray(new byte[1] { bits });

                        for (int i = 7; i > -1 && (newFile.Count < decompressedLength); i--) //iterate through layout bits
                        {

                            if (arrayOfBits[i] == true)
                            {
                                //non-compressed
                                //add one byte from uncompressedOffset to newFile

                                currentOffset = (int)inputFile.Position;

                                inputFile.Seek(uncompressedOffset, SeekOrigin.Begin);

                                newFile.Add(mio0r.ReadByte());
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

                                byte byte1 = mio0r.ReadByte();
                                byte byte2 = mio0r.ReadByte();
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




            }
            inputFile.Close();
            return newFile;
        }

        public byte[] compressInitialization(string path, bool fileInputMode)
        {
            if (fileInputMode)
            {
                FileStream inputFile = File.Open(path, FileMode.Open);
                BinaryReader br = new BinaryReader(inputFile);
                byte[] file = br.ReadBytes((int)inputFile.Length);

                inputFile.Close();

                return compress_MIO0(file, 0);
            }
            else
            {
                byte[] stringToFile = Encoding.ASCII.GetBytes(path);

                return compress_MIO0(stringToFile, 0);
            }
        }

        public byte[] compress_MIO0(byte[] file, int offset)
        {
            List<byte> layoutBits = new List<byte>();
            List<byte> dictionary = new List<byte>();

            List<byte> uncompressedData = new List<byte>();
            List<int[]> compressedData = new List<int[]>();

            int maxDictionarySize = 4096;
            int maxMatchLength = 18;
            int minimumMatchSize = 2;
            int decompressedSize = 0;

            for (int i = 0; i < file.Length; i++)
            {
                if (dictionary.Contains(file[i]))
                {
                    //check for best match
                    int[] matches = findAllMatches(ref dictionary, file[i]);
                    int[] bestMatch = findLargestMatch(ref dictionary, matches, ref file, i, maxMatchLength);

                    if (bestMatch[1] > minimumMatchSize)
                    {
                        //add to compressedData
                        layoutBits.Add(0);
                        bestMatch[0] = dictionary.Count - bestMatch[0]; //sets offset in relation to end of dictionary

                        for (int j = 0; j < bestMatch[1]; j++)
                        {
                            dictionary.Add(file[i + j]);
                        }

                        i = i + bestMatch[1] - 1;

                        compressedData.Add(bestMatch);
                        decompressedSize += bestMatch[1];
                    }
                    else
                    {
                        //add to uncompressed data
                        layoutBits.Add(1);
                        uncompressedData.Add(file[i]);
                        dictionary.Add(file[i]);
                        decompressedSize++;
                    }
                }
                else
                {
                    //uncompressed data
                    layoutBits.Add(1);
                    uncompressedData.Add(file[i]);
                    dictionary.Add(file[i]);
                    decompressedSize++;
                }

                if (dictionary.Count > maxDictionarySize)
                {
                    int overflow = dictionary.Count - maxDictionarySize;
                    dictionary.RemoveRange(0, overflow);
                }
            }

            return buildMIO0CompressedBlock(ref layoutBits, ref uncompressedData, ref compressedData, decompressedSize, offset);
        }

        public byte[] buildMIO0CompressedBlock(ref List<byte> layoutBits, ref List<byte> uncompressedData, ref List<int[]> offsetLengthPairs, int decompressedSize, int offset)
        {
            List<byte> finalMIO0Block = new List<byte>();           //the final compressed file
            List<byte> layoutBytes = new List<byte>();              //holds the layout bits in byte form
            List<byte> compressedDataBytes = new List<byte>();      //holds length/offset in 2byte form

            int compressedOffset = 16 + offset; //header size
            int uncompressedOffset;

            //added magic number
            finalMIO0Block.AddRange(Encoding.ASCII.GetBytes("MIO0")); //4 byte magic number

            //add decompressed data size
            byte[] decompressedSizeArray = BitConverter.GetBytes(decompressedSize);
            Array.Reverse(decompressedSizeArray);
            finalMIO0Block.AddRange(decompressedSizeArray);         //4 byte decompressed size

            //assemble layout bits into bytes
            while (layoutBits.Count > 0)                            //convert layout binary bits to bytes
            {
                //pad bits to full byte if necessary
                while (layoutBits.Count < 8)                         //pad last byte if necessary
                {
                    layoutBits.Add(0);
                }

                string layoutBitsString = layoutBits[0].ToString() + layoutBits[1].ToString() + layoutBits[2].ToString() + layoutBits[3].ToString()
                                        + layoutBits[4].ToString() + layoutBits[5].ToString() + layoutBits[6].ToString() + layoutBits[7].ToString();

                byte[] layoutByteArray = new byte[1];
                layoutByteArray[0] = Convert.ToByte(layoutBitsString, 2);
                layoutBytes.Add(layoutByteArray[0]);
                layoutBits.RemoveRange(0, (layoutBits.Count < 8) ? layoutBits.Count : 8);

            }


            foreach (int[] offsetLengthPair in offsetLengthPairs)
            {
                offsetLengthPair[0] -= 1;                           //removes '1' that is added to offset on decompression
                offsetLengthPair[1] -= 3;                           //removes '3' that is added to length on decompression

                //combine offset and length into 16 bit block
                int compressedInt = (offsetLengthPair[1] << 12) | (offsetLengthPair[0]);

                //split int16 into two bytes to be written
                byte[] compressed2Byte = new byte[2];
                compressed2Byte[0] = (byte)(compressedInt & 0xFF);
                compressed2Byte[1] = (byte)((compressedInt >> 8) & 0xFF);

                compressedDataBytes.Add(compressed2Byte[1]);        //used to be 0 then 1, but this seems to be correct
                compressedDataBytes.Add(compressed2Byte[0]);

            }

            //pad layout bits if needed
            while (layoutBytes.Count % 4 != 0)
            {
                layoutBytes.Add(0);
            }

            compressedOffset += layoutBytes.Count;

            //add final compressed offset
            byte[] compressedOffsetArray = BitConverter.GetBytes(compressedOffset);
            Array.Reverse(compressedOffsetArray);
            finalMIO0Block.AddRange(compressedOffsetArray);

            //add final uncompressed offset
            uncompressedOffset = compressedOffset + compressedDataBytes.Count;
            byte[] uncompressedOffsetArray = BitConverter.GetBytes(uncompressedOffset);
            Array.Reverse(uncompressedOffsetArray);
            finalMIO0Block.AddRange(uncompressedOffsetArray);

            //add layout bits
            foreach (byte layoutByte in layoutBytes)                 //add layout bytes to file
            {
                finalMIO0Block.Add(layoutByte);
            }

            //add compressed data
            foreach (byte compressedByte in compressedDataBytes)     //add compressed bytes to file
            {
                finalMIO0Block.Add(compressedByte);
            }

            //add uncompressed data
            foreach (byte uncompressedByte in uncompressedData)      //add noncompressed bytes to file
            {
                finalMIO0Block.Add(uncompressedByte);
            }

            return finalMIO0Block.ToArray();
        }





        // https://github.com/Daniel-McCarthy/Mr-Peeps-Compressor 




        public byte[] decompress_seg7(string filePath)
        {

            // This will decompress Segment 7's compressed display lists to regular F3DEX commands.
            // This is used exclusively by Mario Kart 64's Segment 7.


            int cID = CourseExporter.cID;

            int v0 = 0;
            int v1 = 0;
            int v2 = 0;



            uint seg7_addr = (CourseExporter.seg7_ptr[cID] - CourseExporter.seg47_buf[cID]) + CourseExporter.seg4_addr[cID];




            byte[] ROM = File.ReadAllBytes(filePath);
            byte[] seg7 = new byte[CourseExporter.seg7_size[cID] + 8];



            MemoryStream romm = new MemoryStream(ROM);
            BinaryReader romr = new BinaryReader(romm);
            MemoryStream seg7m = new MemoryStream(seg7);
            BinaryWriter seg7w = new BinaryWriter(seg7m);

            seg7w.BaseStream.Seek(0, SeekOrigin.Begin);

            int compare = new int();
            byte commandbyte = new byte();
            byte[] byte29 = new byte[2];



            romr.BaseStream.Seek(seg7_addr, SeekOrigin.Begin);

            int vertoffset = 0;
            byte[] voffset = new byte[2];

            bool DispEnd = true;

            for (int i = 0; DispEnd; i++)
            {


                commandbyte = romr.ReadByte();

                if (i > 2415)
                {
                    //MessageBox.Show(i.ToString()+"-Execute Order 0x" + commandbyte.ToString("X"));
                }
                if (commandbyte == 0xFF)
                {


                    DispEnd = false;
                }

                if (commandbyte >= 0x00 && commandbyte <= 0x14)
                {

                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBC000002));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x80000040));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x03860010));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x09000000 | (commandbyte * 0x18) + 8));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x03880010));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x09000000 | commandbyte * 0x18));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x15)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFC121824));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFF33FFFF));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);

                }
                if (commandbyte == 0x16)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFC127E24));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFFFFF3F9));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x17)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFCFFFFFF));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFFFE793C));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x18)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB900031D));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00552078));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x19)
                {
                    //
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB900031D));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00553078));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if ((commandbyte >= 0x1A && commandbyte <= 0x1F) | commandbyte == 0x2C)
                {

                    UInt32 ImgSize = 0, ImgType = 0, ImgFlag1 = 0, ImgFlag2 = 0, ImgFlag3 = 0;
                    UInt32[] ImgTypes = { 0, 0, 0, 3, 3, 3, 0 }; //0=RGBA, 3=IA
                    UInt32[] ImgFlag1s = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; //looks like
                    UInt32[] ImgFlag2s = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 };
                    byte[] Param = new byte[2];

                    Param[0] = romr.ReadByte();
                    Param[1] = romr.ReadByte();


                    if (commandbyte == 0x2C)
                    {
                        ImgType = ImgTypes[6];
                        ImgFlag1 = ImgFlag1s[6];
                        ImgFlag2 = ImgFlag2s[6];
                        ImgFlag3 = 0x100;
                    }
                    else
                    {
                        ImgType = ImgTypes[commandbyte - 0x1A];
                        ImgFlag1 = ImgFlag1s[commandbyte - 0x1A];
                        ImgFlag2 = ImgFlag2s[commandbyte - 0x1A];
                        ImgFlag3 = 0;
                    }
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xE8000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32((((ImgType << 0x15) | 0xF5100000) | ((((ImgFlag2 << 1) + 7) >> 3) << 9)) | ImgFlag3));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32((((((Param[1] & 0xF) << 0x12) | (((Param[1] & 0xF0) >> 4) << 0xE)) | ((Param[0] & 0xF) << 8)) | (((Param[0] & 0xF0) >> 4) << 4))));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xF2000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32((((ImgFlag2 - 1) << 0xE) | ((ImgFlag1 - 1) << 2))));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte >= 0x20 && commandbyte <= 0x25)
                {
                    UInt32 ImgSize = 0, ImgType = 0, ImgFlag1 = 0, ImgFlag2 = 0, ImgFlag3 = 0;
                    UInt32[] ImgTypes = { 0, 0, 0, 3, 3, 3, 0 }; //0=RGBA, 3=IA
                    UInt32[] ImgFlag1s = { 0x20, 0x20, 0x40, 0x20, 0x20, 0x40, 0x20 }; //looks like
                    UInt32[] ImgFlag2s = { 0x20, 0x40, 0x20, 0x20, 0x40, 0x20, 0x20 };
                    byte[] Param = new byte[3];

                    Param[0] = romr.ReadByte();
                    Param[1] = romr.ReadByte();
                    Param[2] = romr.ReadByte();


                    ImgType = ImgTypes[commandbyte - 0x20];
                    ImgFlag1 = ImgFlag1s[commandbyte - 0x20];
                    ImgFlag2 = ImgFlag2s[commandbyte - 0x20];
                    ImgFlag3 = 0;

                    flip4 = BitConverter.GetBytes(Convert.ToUInt32((ImgType | 0xFD000000) | 0x100000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32((Param[0] << 0xB) + 0x05000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xE8000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32((((ImgType << 0x15) | 0xF5000000) | 0x100000) | (Param[2] & 0xF)));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32((((Param[2] & 0xF0) >> 4) << 0x18)));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xE6000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);

                    ImgSize = (ImgFlag2 * ImgFlag1) - 1;
                    if (ImgSize > 0x7FF) ImgSize = 0x7FF;

                    UInt32 Unknown2x = new UInt32();

                    Unknown2x = 1;
                    Unknown2x = (ImgFlag2 << 1) >> 3; //purpose of this value is unknown

                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xF3000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32((((Unknown2x + 0x7FF) / Unknown2x) | (((Param[2] & 0xF0) >> 4) << 0x18)) | (ImgSize << 0xC)));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);

                }
                if (commandbyte == 0x26)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBB000001));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFFFFFFFF));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x27)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBB000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00010001));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x28)
                {
                    value16 = romr.ReadUInt16();
                    value16 = romr.ReadUInt16();
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x040681FF));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x04050500));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x29)
                {
                    value16 = romr.ReadUInt16();
                    v0 = (value16 >> 10) & 0x1F;
                    v1 = (value16 >> 5) & 0x1F;
                    v2 = value16 & 0x1F;



                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBF000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32((v2 << 17) | (v1 << 9) | v0 << 1));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x2A)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB8000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x2B)
                {
                    value16 = romr.ReadUInt16();
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x06000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32((0x07000000 | (value16 * 8))));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x2D)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBE000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00000140));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x2E)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D002E));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D002E));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x2F)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D002F));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D002F));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x30)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D0030));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xD00D0030));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte >= 0x33 && commandbyte <= 0x52)
                {
                    value16 = romr.ReadUInt16();

                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x04000000 | (((commandbyte - 0x32) * 0x410) - 1)));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x04000000 | (value16 * 16)));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }

                if (commandbyte == 0x53)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFCFFFFFF));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xFFFCF279));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }

                if (commandbyte == 0x54)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB900031D));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00442D58));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x55)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB900031D));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00404DD8));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x56)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB7000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00002000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x57)
                {
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB6000000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x00002000));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                }
                if (commandbyte == 0x58)
                {

                    value16 = romr.ReadUInt16();
                    v0 = (value16 >> 10) & 0x1F;
                    v1 = (value16 >> 5) & 0x1F;
                    v2 = value16 & 0x1F;



                    flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xB1000000 | (v2 << 17) | (v1 << 9) | v0 << 1));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);
                    value16 = romr.ReadUInt16();
                    v0 = (value16 >> 10) & 0x1F;
                    v1 = (value16 >> 5) & 0x1F;
                    v2 = value16 & 0x1F;

                    flip4 = BitConverter.GetBytes(Convert.ToUInt32((v2 << 17) | (v1 << 9) | v0 << 1));
                    Array.Reverse(flip4);
                    seg7w.Write(flip4);

                }
                if (i > 2415)
                {
                    //MessageBox.Show(i.ToString() + "-Finished Order 0x" + commandbyte.ToString("X"));
                }
            }

            return (seg7);





        }




   





        //   Compression Algorithm.
        public byte[] compress_seg7(string filePath)
        {

            // This will compress compatible F3DEX commands into a compressed Segment 7.
            // This is used exclusively by Mario Kart 64's Segment 7.



            //You may ask yourself, "What is that beautiful house?"
            //You may ask yourself, "Where does that highway go to?"
            //And you may ask yourself, "Am I right? Am I wrong?"
            //And you may say to yourself, "My God! What have I done?"
            






            int v0 = 0;
            int v1 = 0;
            int v2 = 0;



            




            byte[] ROM = File.ReadAllBytes(filePath);
            
            


            MemoryStream romm = new MemoryStream(ROM);
            BinaryReader romr = new BinaryReader(romm);
            MemoryStream seg7m = new MemoryStream();
            BinaryWriter seg7w = new BinaryWriter(seg7m);

            seg7w.BaseStream.Seek(0, SeekOrigin.Begin);

            int compare = new int();

            string commandbyte = "";  //keeping the same name from above decompress process
            byte[] byte29 = new byte[2];
            string compar = "";
            byte F3Dbyte = new byte();
            byte[] parambyte = new byte[2];





            int vertoffset = 0;
            byte[] voffset = new byte[2];

            byte compressbyte = new byte();

            bool DispEnd = true;
            romr.BaseStream.Position = 0;

            for (int i = 0; (romr.BaseStream.Position < romr.BaseStream.Length); i++)
            {

                F3Dbyte = romr.ReadByte();
                commandbyte = F3Dbyte.ToString("x").PadLeft(2, '0').ToUpper();

                //MessageBox.Show(F3Dbyte.ToString("x").PadLeft(2,'0').ToUpper() + "--" + romr.BaseStream.Position.ToString()); ;




                if (commandbyte == "BC")
                {


                    MessageBox.Show("Unsupported Command -BC-");
                    //0x00 -- 0x14

                    //curently unsupported, ??not featured in stock MK64 racing tracks?? Can't find in multiple courses.


                    //flip4 = BitConverter.GetBytes(Convert.ToUInt32(0xBC000002));
                    //Array.Reverse(flip4);
                    //seg7w.Write(flip4);
                    //flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x80000040));
                    //Array.Reverse(flip4);
                    //seg7w.Write(flip4);
                    //flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x03860010));
                    //Array.Reverse(flip4);
                    //seg7w.Write(flip4);
                    //flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x09000000 | (commandbyte * 0x18) + 8));
                    //Array.Reverse(flip4);
                    //seg7w.Write(flip4);
                    //flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x03880010));
                    //Array.Reverse(flip4);
                    //seg7w.Write(flip4);
                    //flip4 = BitConverter.GetBytes(Convert.ToUInt32(0x09000000 | commandbyte * 0x18));
                    //Array.Reverse(flip4);
                    //seg7w.Write(flip4);


                }
                if (commandbyte == "FC")
                {

                    byte29 = romr.ReadBytes(2);
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

                    romr.BaseStream.Seek(5, SeekOrigin.Current);
                    seg7w.Write(compressbyte);

                }
                if (commandbyte == "B9")
                {


                    romr.BaseStream.Seek(5, SeekOrigin.Current);
                    byte29 = romr.ReadBytes(2);
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
                    // 000000 00000000
                    //0x1A -> 0x1F + 0x2C
                    romr.BaseStream.Seek(7, SeekOrigin.Current);





                    byte29 = romr.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");
                    byte29 = romr.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");

                    byte[] Param = new byte[2];


                    //don't ask me I don't know
                    //don't ask me I don't know
                    byte[] parameters = romr.ReadBytes(4);
                    Array.Reverse(parameters);
                    value32 = BitConverter.ToUInt32(parameters, 0);
                    uint opint = new uint();
                    opint = value32 >> 14;

                    Param[0] = Convert.ToByte((opint & 0xF0) >> 4 | (opint & 0xF) << 4);

                    opint = value32 >> 4;

                    Param[1] = Convert.ToByte((opint & 0xF0) >> 4 | (opint & 0xF) << 4);

                    Array.Reverse(Param);

                    romr.BaseStream.Seek(4, SeekOrigin.Current);
                    byte29 = romr.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");
                    byte29 = romr.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");
                    //MessageBox.Show(compar);
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
                    //MessageBox.Show(BitConverter.ToString(Param));
                    seg7w.Write(compressbyte);
                    seg7w.Write(Param);
                    //don't ask me I don't know
                    //don't ask me I don't know
                }
                if (commandbyte == "FD")
                {


                    //0x20  ->  0x25


                    romr.BaseStream.Seek(1, SeekOrigin.Current);


                    byte[] Param = new byte[3];


                    byte29 = romr.ReadBytes(2);
                    compar = BitConverter.ToString(byte29).Replace("-", "");

                    //dont ask me I don't know
                    //dont ask me I don't know
                    //dont ask me I don't know

                    byte[] parambytes = romr.ReadBytes(4);
                    Array.Reverse(parambytes);
                    value32 = BitConverter.ToUInt32(parambytes, 0);


                    Param[0] = Convert.ToByte((value32 - 0x05000000) >> 11);
                    Param[1] = 0x00;
                    Param[2] = 0x70;
                    //dont ask me I don't know
                    //dont ask me I don't know
                    //dont ask me I don't know
                    //MessageBox.Show(value32.ToString("x")+"--"+Param[0].ToString("x"));
                    romr.BaseStream.Seek(28, SeekOrigin.Current);
                    byte29 = romr.ReadBytes(2);
                    compar = compar + BitConverter.ToString(byte29).Replace("-", "");
                    byte29 = romr.ReadBytes(2);
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

                    //0x26 000001  FFFFFFFF
                    //0x27    00010001
                    romr.BaseStream.Seek(3, SeekOrigin.Current);
                    byte29 = romr.ReadBytes(2);
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

                    romr.BaseStream.Seek(2, SeekOrigin.Current);




                }

                if (commandbyte == "BF")
                {



                    compressbyte = 0x29;
                    seg7w.Write(compressbyte);
                    romr.BaseStream.Seek(4, SeekOrigin.Current);

                    v2 = romr.ReadByte();
                    v1 = romr.ReadByte();
                    v0 = romr.ReadByte();

                    v0 = v0 / 2;
                    v1 = v1 / 2;
                    v2 = v2 / 2;


                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((v2) | (v1 << 5) | v0 << 10));


                    seg7w.Write(flip4);
                }
                if (commandbyte == "B8")
                {

                    //0x2A
                    compressbyte = 0x2A;
                    seg7w.Write(compressbyte);
                    romr.BaseStream.Seek(7, SeekOrigin.Current);

                }
                if (commandbyte == "06")
                {

                    //0x2B
                    compressbyte = 0x2B;


                    romr.BaseStream.Seek(3, SeekOrigin.Current);

                    byte[] parambytes = romr.ReadBytes(4);
                    Array.Reverse(parambytes);
                    value32 = BitConverter.ToUInt32(parambytes, 0);

                    value32 = value32 & 0x00FFFFFF;



                    seg7w.Write(compressbyte);
                    seg7w.Write(Convert.ToUInt16(value32 / 8));
                }
                if (commandbyte == "BE")
                {

                    //0x2D
                    compressbyte = 0x2D;
                    romr.BaseStream.Seek(7, SeekOrigin.Current);
                    seg7w.Write(compressbyte);
                }
                if (commandbyte == "D0")
                {


                    romr.BaseStream.Seek(3, SeekOrigin.Current);
                    byte29 = romr.ReadBytes(2);
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
                    romr.BaseStream.Seek(8, SeekOrigin.Current);
                }
                if (commandbyte == "04")
                {

                    //0x33->0x52
                    romr.BaseStream.Seek(1, SeekOrigin.Current);
                    byte[] Param = romr.ReadBytes(2);
                    Array.Reverse(Param);
                    value16 = BitConverter.ToUInt16(Param, 0);

                    compressbyte = Convert.ToByte(((value16 + 1) / 0x410) + 0x32);
                    seg7w.Write(compressbyte);

                    byte[] parambytes = romr.ReadBytes(4);
                    Array.Reverse(parambytes);
                    value32 = BitConverter.ToUInt32(parambytes, 0);
                    value32 = (value32 - 0x04000000) / 16;

                    value16 = Convert.ToUInt16(value32);
                    seg7w.Write(value16);
                }

                if (commandbyte == "FC")
                {

                    //0x53
                    compressbyte = 0x53;
                    romr.BaseStream.Seek(7, SeekOrigin.Current);
                    seg7w.Write(compressbyte);
                }

                if (commandbyte == "B9")
                {

                    romr.BaseStream.Seek(3, SeekOrigin.Current);
                    byte29 = romr.ReadBytes(2);
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
                    romr.BaseStream.Seek(2, SeekOrigin.Current);




                }
                if (commandbyte == "B7")
                {

                    //0x56
                    compressbyte = 0x56;
                    romr.BaseStream.Seek(15, SeekOrigin.Current);

                    seg7w.Write(compressbyte);
                }
                if (commandbyte == "B6")
                {

                    //0x57
                    compressbyte = 0x57;
                    romr.BaseStream.Seek(15, SeekOrigin.Current);
                    seg7w.Write(compressbyte);
                }
                if (commandbyte == "B1")
                {

                    //0x58
                    compressbyte = 0x58;
                    seg7w.Write(compressbyte);


                    v2 = romr.ReadByte();
                    v1 = romr.ReadByte();
                    v0 = romr.ReadByte();

                    v0 = v0 / 2;
                    v1 = v1 / 2;
                    v2 = v2 / 2;


                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((v2) | (v1 << 5) | v0 << 10));


                    seg7w.Write(flip4);


                    //twice for second set of verts
                    //have to move the reader forward by 1 position first

                    romr.BaseStream.Seek(1, SeekOrigin.Current);

                    v2 = romr.ReadByte();
                    v1 = romr.ReadByte();
                    v0 = romr.ReadByte();

                    v0 = v0 / 2;
                    v1 = v1 / 2;
                    v2 = v2 / 2;

                    flip4 = BitConverter.GetBytes(Convert.ToUInt16((v2) | (v1 << 5) | v0 << 10));


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

            //fin


            MessageBox.Show("Compressed");
            byte[] seg7 = seg7m.ToArray();
            return (seg7);

    



        }
    //

    }

}

  