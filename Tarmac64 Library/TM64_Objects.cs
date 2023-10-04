using Assimp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tarmac64_Library
{

    public class TM64_Objects
    {

        TM64_Geometry TarmacGeometry = new TM64_Geometry();
        public class OK64Collide
        {

            //Internal Name in Tarmac
            public string Name { get; set; }
            //Cube or Sphere type. 
            public short Type { get; set; }

            //Various result values for hits
            public short Status { get; set; }
            public short Effect { get; set; }
            public float Scale { get; set; }
            public short CollideResult { get; set; }
            public short HitResult { get; set; }

            // Sizes. Uses FixedStorage in ROM. x10
            //Cubetype uses 3 element XYZ size. Z-up in Tarmac Y-up in ROM.
            //Spheretype uses first size element for Radius.
            public short[] Origin { get; set; }
            public short[] Size { get; set; }

            public short BoxAngle { get; set; }
            public bool SolidObject { get; set; }

            public OK64Collide(string InputName)
            {
                Name = InputName;
                Type = 0;
                Origin = new short[3] { 0, 0, 0 };
                Size = new short[3] { 0, 0, 0 };
                Scale = 1.0f;
                Status = 0;
                Effect = -1;
                HitResult = 0;
                CollideResult = 0;
                BoxAngle = 0;
                SolidObject = true;
            }
            //
        }

        public OK64Collide[] LoadHBK(string inputFile)
        {
            TM64.OK64Settings TarmacSettings = new TM64.OK64Settings();
            TarmacSettings.LoadSettings();
            OK64Collide[] HitboxData = new OK64Collide[0];
            string[] FileData = File.ReadAllLines(inputFile);

            if (FileData[0] == "HBK")
            {
                HitboxData = new OK64Collide[Convert.ToInt32(FileData[1])];

                int CurrentLine = 2;
                int CurrentBox = 0;
                while (CurrentLine < FileData.Length)
                {
                    HitboxData[CurrentBox] = new OK64Collide("Collide" + CurrentBox.ToString());
                    HitboxData[CurrentBox].Effect = 0;
                    HitboxData[CurrentBox].Status = 0;
                    HitboxData[CurrentBox].Scale = TarmacSettings.ImportScale;
                    HitboxData[CurrentBox].CollideResult = 0;
                    HitboxData[CurrentBox].HitResult = 0;


                    string VectorInput = FileData[CurrentLine++];
                    VectorInput = VectorInput.Replace("[", "");
                    VectorInput = VectorInput.Replace("]", "");
                    string[] Vector = VectorInput.Split(',');
                    HitboxData[CurrentBox].Origin[0] = Convert.ToInt16(Convert.ToSingle(Vector[0]));
                    HitboxData[CurrentBox].Origin[1] = Convert.ToInt16(Convert.ToSingle(Vector[1]));
                    HitboxData[CurrentBox].Origin[2] = Convert.ToInt16(Convert.ToSingle(Vector[2]));


                    HitboxData[CurrentBox].Type = Convert.ToInt16(Convert.ToSingle(FileData[CurrentLine++]));

                    switch(HitboxData[CurrentBox].Type)
                    {
                        case 0:
                            {
                                //Sphere
                                HitboxData[CurrentBox].Size[0] = Convert.ToInt16(Convert.ToSingle(FileData[CurrentLine++]));
                                break;
                            }

                        case 1:
                            {
                                //cylinder
                                HitboxData[CurrentBox].Size[0] = Convert.ToInt16(Convert.ToSingle(FileData[CurrentLine++]));
                                HitboxData[CurrentBox].Size[1] = Convert.ToInt16(Convert.ToSingle(FileData[CurrentLine++]));
                                break;
                            }

                        case 2:
                            {
                                //box
                                HitboxData[CurrentBox].Size[0] = Convert.ToInt16(Convert.ToSingle(FileData[CurrentLine++]));
                                HitboxData[CurrentBox].Size[1] = Convert.ToInt16(Convert.ToSingle(FileData[CurrentLine++]));
                                HitboxData[CurrentBox].Size[2] = Convert.ToInt16(Convert.ToSingle(FileData[CurrentLine++]));
                                HitboxData[CurrentBox].BoxAngle = Convert.ToInt16(Convert.ToSingle(FileData[CurrentLine++]) * 100.0f);
                                break;
                            }
                    }

                    CurrentBox++;
                }
            }
            

            return HitboxData;
        }



        public OK64Collide[] LoadHitbox(Scene FBX)
        {
            List<OK64Collide> Hitbox = new List<OK64Collide>();
            Node Base = FBX.RootNode.FindNode("Collide Objects");
            for (int ThisChild = 0; ThisChild < Base.ChildCount; ThisChild++)
            {
                Hitbox.Add(new OK64Collide(ThisChild.ToString()));
                Matrix4x4 OPrime = TarmacGeometry.GetTotalTransform(Base.Children[ThisChild], FBX);
                Hitbox[ThisChild].Origin = new short[3] { Convert.ToInt16(OPrime.A4 * 100), Convert.ToInt16(OPrime.B4 * 100), Convert.ToInt16(OPrime.C4 * 100) };
                Hitbox[ThisChild].Size = new short[3] { 5, 5, 5 };
                Hitbox[ThisChild].Status = 0;
                Hitbox[ThisChild].Effect = 0;
                Hitbox[ThisChild].Name = Base.Children[ThisChild].Name;

            }


            return Hitbox.ToArray();
        }



        public OK64Collide[] LoadHitboxFile(byte[] FileData)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            memoryStream.Write(FileData, 0, FileData.Length);
            memoryStream.Position = 0;
            int Count = binaryReader.ReadInt32();
            OK64Collide[] Hitbox = new OK64Collide[Count];
            for (int ThisHit = 0; ThisHit < Count; ThisHit++)
            {
                Hitbox[ThisHit] = new OK64Collide(ThisHit.ToString());
                Hitbox[ThisHit].Name = binaryReader.ReadString();
                Hitbox[ThisHit].Type = binaryReader.ReadInt16();
                Hitbox[ThisHit].Status = binaryReader.ReadInt16();
                Hitbox[ThisHit].Scale = binaryReader.ReadSingle();
                Hitbox[ThisHit].Effect = binaryReader.ReadInt16();
                Hitbox[ThisHit].CollideResult = binaryReader.ReadInt16();
                Hitbox[ThisHit].HitResult = binaryReader.ReadInt16();
                Hitbox[ThisHit].BoxAngle = binaryReader.ReadInt16();
                Hitbox[ThisHit].SolidObject = binaryReader.ReadBoolean();

                // Initialize Arrays
                Hitbox[ThisHit].Origin = new short[3];
                Hitbox[ThisHit].Size = new short[3];
                //

                for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                {
                    Hitbox[ThisHit].Origin[ThisVector] = binaryReader.ReadInt16();
                    Hitbox[ThisHit].Size[ThisVector] = binaryReader.ReadInt16();
                }
            }
            return Hitbox;

        }

        public byte[] SaveHitboxFile(OK64Collide[] Hitbox)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(Hitbox.Length);
            foreach (var Hit in Hitbox)
            {
                binaryWriter.Write(Hit.Name);
                binaryWriter.Write(Hit.Type);
                binaryWriter.Write(Hit.Status);
                binaryWriter.Write(Hit.Scale);
                binaryWriter.Write(Hit.Effect);
                binaryWriter.Write(Hit.CollideResult);
                binaryWriter.Write(Hit.HitResult);
                binaryWriter.Write(Hit.BoxAngle);
                binaryWriter.Write(Hit.SolidObject);
                for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                {
                    binaryWriter.Write(Hit.Origin[ThisVector]);
                    binaryWriter.Write(Hit.Size[ThisVector]);
                }
            }

            return memoryStream.ToArray();

        }


    }
}
