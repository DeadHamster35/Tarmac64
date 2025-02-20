using Assimp;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
                Status = -1;
                Effect = -1;
                HitResult = 0;
                CollideResult = 0;
                BoxAngle = 0;
                SolidObject = true;
            }

            public OK64Collide(XmlDocument XMLDoc, string Parent, int ChildIndex)
            {
                //LoadXML
                TM64 Tarmac = new TM64();
                XmlNode Owner = XMLDoc.SelectSingleNode(Parent);

                string HeaderName = Parent + "/" + "Hitbox_" + ChildIndex.ToString();

                Type = Convert.ToInt16(Tarmac.LoadElement(XMLDoc, HeaderName, "Type"));
                Effect = Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "Effect"));
                Status = Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "Status"));
                CollideResult = Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "CollideResult"));
                HitResult = Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "HitResult"));
                SolidObject = Convert.ToBoolean(Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "SolidObject")));
                Scale = Convert.ToSingle(Tarmac.LoadElement(XMLDoc,HeaderName, "Scale"));
                BoxAngle = Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "BoxAngle"));

                Origin = new short[3];
                Origin[0] = Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "Origin0"));
                Origin[1] = Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "Origin1"));
                Origin[2] = Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "Origin2"));
                Size = new short[3];
                Size[0] = Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "Size0"));
                Size[1] = Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "Size1"));
                Size[2] = Convert.ToInt16(Tarmac.LoadElement(XMLDoc,HeaderName, "Size2"));
            }

            public void SaveXML(XmlDocument XMLDoc, XmlElement Parent,int ThisHit)
            {
                XmlElement HitXML = XMLDoc.CreateElement("Hitbox_" + ThisHit.ToString());

                TM64 Tarmac = new TM64();

                Tarmac.GenerateElement(XMLDoc, HitXML, "Type", Type);
                Tarmac.GenerateElement(XMLDoc, HitXML, "Effect", Effect);
                Tarmac.GenerateElement(XMLDoc, HitXML, "Status", Status);
                Tarmac.GenerateElement(XMLDoc, HitXML, "CollideResult", CollideResult);
                Tarmac.GenerateElement(XMLDoc, HitXML, "HitResult", HitResult);
                Tarmac.GenerateElement(XMLDoc, HitXML, "SolidObject", Convert.ToInt32(SolidObject));
                Tarmac.GenerateElement(XMLDoc, HitXML, "Scale", Scale);
                Tarmac.GenerateElement(XMLDoc, HitXML, "BoxAngle", BoxAngle);

                Tarmac.GenerateElement(XMLDoc, HitXML, "Origin0", Origin[0]);
                Tarmac.GenerateElement(XMLDoc, HitXML, "Origin1", Origin[1]);
                Tarmac.GenerateElement(XMLDoc, HitXML, "Origin2", Origin[2]);

                Tarmac.GenerateElement(XMLDoc, HitXML, "Size0", Size[0]);
                Tarmac.GenerateElement(XMLDoc, HitXML, "Size1", Size[1]);
                Tarmac.GenerateElement(XMLDoc, HitXML, "Size2", Size[2]);

                Parent.AppendChild(HitXML);
            }
            //
        }

        public class OK64Behavior
        {
            public string Name { get; set; }
            public OK64Parameter[] Parameters { get; set; }
        }

        public class OK64Parameter
        {
            public string Name { get; set; }

            public int Value { get; set; }
        }


        public OK64Behavior[] LoadBehaviorXML(string TargetPath)
        {
            
            string SavePath = TargetPath;

            XmlDocument XMLDoc = new XmlDocument();
            if (File.Exists(TargetPath))
            {
                XMLDoc.Load(TargetPath);
            }
            else
            {
                return new OK64Behavior[0];
            }

            TM64 Tarmac = new TM64();
            string Parent = "BehaviorArray";
            
            int BehaviorCount = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Parent, "BehaviorCount", "0"));

            OK64Behavior[] BehaviorArray = new OK64Behavior[BehaviorCount];
            for (int ThisBehavior = 0; ThisBehavior < BehaviorCount; ThisBehavior++)
            {
                BehaviorArray[ThisBehavior] = new OK64Behavior();
                string Target = Parent + "/Behavior_" + ThisBehavior.ToString();
                int ParameterCount = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, Target, "/ParameterCount", "0"));
                BehaviorArray[ThisBehavior].Parameters = new OK64Parameter[ParameterCount];
                BehaviorArray[ThisBehavior].Name = Tarmac.LoadElement(XMLDoc, Target, "/Name", "NULL");

                for (int ThisParameter = 0; ThisParameter < ParameterCount; ThisParameter++)
                {
                    string SubTarget = Parent + "/Behavior_" + ThisBehavior.ToString();
                    BehaviorArray[ThisBehavior].Parameters[ThisParameter] = new OK64Parameter();
                    BehaviorArray[ThisBehavior].Parameters[ThisParameter].Name = Tarmac.LoadElement(XMLDoc, SubTarget, "/Parameter_" + ThisParameter.ToString() + "_Name", "NULL");
                    BehaviorArray[ThisBehavior].Parameters[ThisParameter].Value = Convert.ToInt32(Tarmac.LoadElement(XMLDoc, SubTarget, "/Parameter_" + ThisParameter.ToString() + "_Value", "0"));
                }
            }


            return BehaviorArray;
                
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
                    HitboxData[CurrentBox].SolidObject = true;


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
