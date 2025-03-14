﻿using Assimp;
using F3DSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tarmac64_Library;
using Texture64;



namespace Tarmac64_Library
{
    public class TM64_Course
    {

        
        TM64 Tarmac = new TM64();
        TM64_Geometry TarmacGeometry = new TM64_Geometry();
        TM64_Objects TarmacObject = new TM64_Objects();
        F3DEX095 F3D =new F3DEX095();
        

        public class OKObject
        {
            public short[] OriginPosition { get; set; }
            public short[] OriginAngle { get; set; }
            public float[] Velocity { get; set; }  //multiply by 10 to get short value.
            public short[] AngularVelocity { get; set; }            
            public short ObjectIndex { get; set; }
            public short GameMode { get; set; }
            public short ObjectiveClass { get; set; }
            public short BattlePlayer { get; set; }
            public short Flag { get; set; }
        }

        public class OKObjectAnimations
        {
            public TM64_Geometry.OK64Bone WalkAnimation { get; set; }
            public int WalkPosition { get; set; }
            public TM64_Geometry.OK64Bone TargetAnimation { get; set; }
            public int TargetPosition { get; set; }
            public TM64_Geometry.OK64Bone DeathAnimation { get; set; }
            public int DeathPosition { get; set; }
        }



        public class OKObjectBehaviorDead
        {
            //nothing unique
        }

        public class OKObjectBehaviorExist
        {
            //nothing unique
        }


        public class OKObjectBehaviorFloat
        {
            //nothing unique
        }
        public class OKObjectBehaviorPath
        {

            public short PathIndex { get; set; }
            public short Direction { get; set; }
            public float MaxSpeed { get; set; }
        }


        public class OKObjectBehaviorWander
        {

            public short Range { get; set; }
            public short Sight { get; set; }
            public short Viewcone { get; set; }
            public float MaxSpeed { get; set; }
        }
        

        public class OKObjectBehaviorSearch
        {

            public short Range { get; set; }
            public short Sight { get; set; }
            public short Viewcone { get; set; }
            public float MaxSpeed { get; set; }
        }

        public class OKObjectBehaviorBounce
        {
            //nothing unique
        }

        public class OKObjectType
        {
            public string Path { get; set; }
            public short Range { get; set; }
            public short Sight { get; set; }
            public short Viewcone { get; set; }           
            public short BehaviorClass { get; set; }
            public short RenderRadius { get; set; }
            public short BumpRadius { get; set; }
            public float MaxSpeed { get; set; }            
            public float ModelScale { get; set; }
            public short SoundRadius { get; set; }
            public short SoundType { get; set; }
            public int SoundID { get; set; }
            public short Flag { get; set; }
            public TM64_Geometry.OK64Texture[] TextureData { get; set; }
            public TM64_Geometry.OK64F3DObject[] ModelData { get; set; }
            public OKObjectAnimations ObjectAnimations { get; set; }
            public TM64_Objects.OK64Collide[] ObjectHitbox { get; set; }
            public int ModelPosition { get; set; }
            public UInt32 AnimationOffset { get; set; }
            public UInt32 HitboxOffset { get; set; }
            public UInt32 XLUOffset { get; set; }
            public int ModelCount { get; set; }
            public int XLUCount { get; set; }
            public byte GravityToggle { get; set; }
            public byte CameraAlligned { get; set; }
            public byte ZSortToggle { get; set; }
            public OKPathfinding[] PathfindingData { get; set; }
            public string Name { get; set; }
            
        }
        public class OKPathfinding
        {
            float Radius { get; set; }
            short[] Position { get; set; }
            short[] BoxSize { get; set; }
            float[] Angle { get; set; }
            short CollisionType;
            short EffectType;
        }

        public class PathEffect
        {
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
            public int Power { get; set; }
            public int Type { get; set; }
            public Tarmac64_Library.TM64_Geometry.OK64Color BodyColor { get; set; }
            public Tarmac64_Library.TM64_Geometry.OK64Color AdjColor { get; set; }
        }

        public class Course
        {
            public TM64_Geometry.OK64F3DObject[] MasterObjects { get; set; }
            public TM64_Geometry.OK64F3DObject[] SurfaceObjects { get; set; }
            public TM64_Geometry.OK64Texture[] TextureObjects { get; set; }
            public TM64_Geometry.OK64SectionList[] SectionList { get; set; }

            public byte[] Segment4 { get; set; }
            public byte[] Segment6 { get; set; }
            public byte[] Segment7 { get; set; }
            public byte[] Segment9 { get; set; }
            public int Segment5ROM { get; set; }
            public int Segment5Length { get; set; }
            public int Segment5CompressedLength { get; set; }
            public byte[] ScrollData { get; set; }
            public byte[] WaterData { get; set; }
            public byte[] ScreenData { get; set; }
            public byte[] KillDisplayData { get; set; }
            public byte[] ObjectModelData { get; set; }
            public byte[] ObjectHitboxData { get; set; }
            public byte[] ObjectListData { get; set; }
            public byte[] ObjectTypeData { get; set; }
            public byte[] ObjectAnimationData { get; set; }
            public string Credits { get; set; }
            public string Name { get; set; }
            public string PreviewPath { get; set; }
            public string BannerPath { get; set; }
            public byte[] PreviewData { get; set; }
            public byte[] BannerData { get; set; }
            public byte[] RadarData { get; set; }
            public string AssmeblyPath { get; set; }
            public string GhostPath { get; set; }
            public byte[] GhostData { get; set; }
            public int GhostCharacter { get; set; }
            public int[] PathSurface { get; set; }
            public short PathCount { get; set; }
            public int LapCount { get; set; }
            public short DistributeBool { get; set; }
            public short GoalBannerBool { get; set; }
            public short SkyboxBool { get; set; }

            public int EchoOffset { get; set; }
            public int EchoEndOffset { get; set; }
            public uint[] PathOffsets { get; set; }
            public PathEffect[] PathEffects { get; set; }
            public TM64_Geometry.OK64Color EchoColor { get; set; }
            public TM64_Geometry.OK64Color EchoAdjustColor { get; set; }
            public int MusicID { get; set; }
            public string OK64SongPath { get; set; }
            public TM64_Sound.OK64Song SongData { get; set; }
            public MiniMap MapData { get; set; }
            public Sky SkyColors { get; set; }
            public MenuHeader MenuHeaderData { get; set; }
            public OK64Header OK64HeaderData { get; set; }
            public string SerialNumber { get; set; }
            public int Gametype { get; set; }
            public VSBomb[] BombArray { get; set; }
            public OKFog Fog { get; set; }
            public int ManualTempo { get; set; }

        }
        public class OKFog
        {

            public short FogToggle { get; set; }
            public TM64_Geometry.OK64Color FogColor { get; set; }
            public short StartDistance { get; set; }
            public short StopDistance { get; set; }
        }

        public class VSBomb
        {
            public short Point { get; set; }
            public short Type { get; set; }
        }
        public class MiniMap
        {
            public string MinimapPath { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public Vector2D MapCoord { get; set; }
            public Vector2D StartCoord { get; set; }
            public Vector2D LineCoord { get; set; }
            public TM64_Geometry.OK64Color MapColor { get; set; }
            public float MapScale { get; set; }

        }
        public class MenuHeader
        {
            public int Preview { get; set; }
            public int Banner { get; set; }
        }
        public class OK64Header
        {
            public int Version { get; set; }
            public CourseHeader MapHeader { get; set; }
            public int SectionViewPosition { get; set; }
            public int SurfaceMapPosition { get; set; }
            public int XLUViewPosition { get; set; }
            
            public int Sky { get; set; }
            public int Credits { get; set; }
            public int CourseName { get; set; }
            public int SerialKey { get; set; }
            public int Ghost { get; set; }
            public int Maps { get; set; }
            public int ObjectDataStart { get; set; }
            public int ObjectModelStart { get; set; }
            public int ObjectAnimationStart { get; set; }
            public int ObjectDataEnd { get; set; }
            public int BombOffset { get; set; }
            public int EchoStart { get; set; }
            public int EchoEnd { get; set; }
            public byte GoalBannerToggle { get; set; }
            public byte SkyboxToggle { get; set; }
            public short[] PathLength { get; set; }
            public float WaterLevel { get; set; }
            public int WaterType { get; set; }
            public int ScrollStart { get; set; }
            public int ScrollEnd { get; set; }
            public UInt32 PathOffset { get; set; }

            public int LapCount { get; set; }
            public int DragLength { get; set; }

        }

        public class CourseHeader
        {
            public UInt32 s6Start { get; set; }
            public UInt32 s6End { get; set; }
            public UInt32 s47Start { get; set; }
            public UInt32 s47End { get; set; }
            public UInt32 s7Start { get; set; }
            public UInt32 s9Start { get; set; }
            public UInt32 s9End { get; set; }
            public UInt32 VertCount { get; set; }
            public UInt32 S7Size { get; set; }
            public UInt32 TexturePointer { get; set; }
        }

        public class Sky
        {
            
            public int SkyType { get; set; }
            public int WeatherType { get; set; }
            public TM64_Geometry.OK64Color TopColor { get; set; }
            public TM64_Geometry.OK64Color MidColor { get; set; }
            public TM64_Geometry.OK64Color BotColor { get; set; }
        }

        public class Header
        {
            public byte[] s6Start { get; set; }
            public byte[] s6End { get; set; }
            public byte[] s47Start { get; set; }
            public byte[] s47End { get; set; }
            public byte[] s9Start { get; set; }
            public byte[] s9End { get; set; }
            public byte[] S47Buffer { get; set; }
            public byte[] VertCount { get; set; }
            public byte[] S7Pointer { get; set; }
            public byte[] S7Size { get; set; }
            public byte[] TexturePointer { get; set; }
            public byte[] FlagPadding { get; set; }
        }


        public class MemoryCard
        {
            public GhostData[] ghostData { get; set; }
            public int fileType { get; set; }

        }

        public class GhostData
        {
            public float raceTime { get; set; }
            public int character { get; set; }
            public int course { get; set; }
            public byte[] ghostInput { get; set; }
            public byte[] header { get; set; }

        }



        public TM64_Course.OKObject NewOKObject()
        {
            TM64_Course.OKObject NewObject = new TM64_Course.OKObject();
            NewObject.OriginPosition = new short[3] { 0, 0, 0 };
            NewObject.OriginAngle = new short[3] { 0, 0, 0 };
            NewObject.Velocity = new float[3] { 0, 0, 0, };
            NewObject.AngularVelocity = new short[3] { 0, 0, 0 };
            NewObject.ObjectIndex = 0;
            NewObject.BattlePlayer = 0;
            NewObject.ObjectiveClass = 0;

            return NewObject;
        }
        public OKObject[] LoadOKObject(byte[] FileData)
        {
            MemoryStream memoryStream = new MemoryStream(FileData);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            int OKObjectCount = binaryReader.ReadInt32();
            OKObject[] TheseObjects = new OKObject[OKObjectCount];
            for (int ThisObject = 0; ThisObject < OKObjectCount; ThisObject++)
            {
                TheseObjects[ThisObject] = new OKObject();
                TheseObjects[ThisObject].ObjectIndex = binaryReader.ReadInt16();
                binaryReader.ReadInt16();
                TheseObjects[ThisObject].OriginPosition = new short[] { binaryReader.ReadInt16(), binaryReader.ReadInt16(), binaryReader.ReadInt16() };
                TheseObjects[ThisObject].OriginAngle = new short[] { binaryReader.ReadInt16(), binaryReader.ReadInt16(), binaryReader.ReadInt16() };
                TheseObjects[ThisObject].Velocity = new float[] { binaryReader.ReadInt16() / 100, binaryReader.ReadInt16() / 100, binaryReader.ReadInt16() / 100 };
                TheseObjects[ThisObject].AngularVelocity = new short[] { binaryReader.ReadInt16(), binaryReader.ReadInt16(), binaryReader.ReadInt16() };
            }
            return TheseObjects;
        }            
        public byte[] SaveOKObjectListRaw(OKObject[] ObjectList)
        {
            byte[] flip = new byte[0];
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            
            binaryWriter.Write(F3D.BigEndian(ObjectList.Length));


            for (int ThisObject = 0; ThisObject < ObjectList.Length; ThisObject++)
            {
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].ObjectIndex - 6)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].Flag)));

                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].OriginPosition[0])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].OriginPosition[2])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].OriginPosition[1] * -1)));

                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].OriginAngle[0])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].OriginAngle[2])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].OriginAngle[1])));

                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].Velocity[0])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].Velocity[2])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].Velocity[1] * -1)));

                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].AngularVelocity[0])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].AngularVelocity[2])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ObjectList[ThisObject].AngularVelocity[1])));
            }
            return memoryStream.ToArray();
        }



        public byte[] SaveHitboxRaw(TM64_Objects.OK64Collide[] Hitbox)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            if (Hitbox == null)
            {
                binaryWriter.Write(F3D.BigEndian(0)); //padding
                binaryWriter.Write(F3D.BigEndian(100));

                binaryWriter.Write(Convert.ToInt16(0)); //placeholder for the angle data
                binaryWriter.Write(Convert.ToInt16(0)); //placeholder for the angle data
                binaryWriter.Write(Convert.ToInt16(0));

                binaryWriter.Write(Convert.ToInt16(0));
                binaryWriter.Write(Convert.ToInt16(0));
                binaryWriter.Write(Convert.ToInt16(0));

                binaryWriter.Write(Convert.ToInt16(0));
                binaryWriter.Write(Convert.ToInt16(0));
                binaryWriter.Write(Convert.ToInt16(0));

                binaryWriter.Write(Convert.ToInt16(0)); //PAD

                binaryWriter.Write(Convert.ToInt16(0));
                binaryWriter.Write(Convert.ToInt16(0));
                binaryWriter.Write(Convert.ToInt16(0));
                binaryWriter.Write(Convert.ToInt16(0));
            }
            else
            {
                foreach (var Hit in Hitbox)
                {
                    binaryWriter.Write(F3D.BigEndian(Hit.Type)); //padding
                    binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Hit.Scale * 100)));

                    binaryWriter.Write(Convert.ToInt16(0)); //placeholder for the angle data
                    binaryWriter.Write(Convert.ToInt16(0)); //placeholder for the angle data
                    binaryWriter.Write(F3D.BigEndian(Hit.BoxAngle)); //ayy real Z angles lessgo.

                    binaryWriter.Write(F3D.BigEndian(Hit.Size[0]));
                    binaryWriter.Write(F3D.BigEndian(Hit.Size[2]));
                    binaryWriter.Write(F3D.BigEndian(Hit.Size[1]));

                    binaryWriter.Write(F3D.BigEndian(Hit.Origin[0]));
                    binaryWriter.Write(F3D.BigEndian(Hit.Origin[2]));
                    binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Hit.Origin[1] * -1)));


                    if (Hit.BoxAngle != 0)
                    {
                        binaryWriter.Write(Convert.ToSByte(1));
                    }
                    else
                    {
                        binaryWriter.Write(Convert.ToSByte(0));
                    }

                    if (Hit.SolidObject)
                    {
                        binaryWriter.Write(Convert.ToSByte(1));
                    }
                    else
                    {
                        binaryWriter.Write(Convert.ToSByte(0));
                    }

                    binaryWriter.Write(Convert.ToSByte(Hit.Status));
                    binaryWriter.Write(Convert.ToSByte(Hit.Effect));
                    binaryWriter.Write(Convert.ToSByte(Hit.CollideResult));
                    binaryWriter.Write(Convert.ToSByte(Hit.HitResult));


                }
            }
            return memoryStream.ToArray();
        }
        public byte[] CompileObjectHitbox(OKObjectType[] ObjectTypes, uint Magic)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            foreach (var ThisType in ObjectTypes)
            {
                
                ThisType.HitboxOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position + Magic);
                binaryWriter.Write(SaveHitboxRaw(ThisType.ObjectHitbox));
            }
            return memoryStream.ToArray();
        }

        public OKObjectType LoadObjectType(string InputPath)
        {
            OKObjectType NewType = new OKObjectType();
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            var Input = File.ReadAllBytes(InputPath);
            var Decompressed = Tarmac.DecompressMIO0(Input);
            memoryStream.Write(Decompressed, 0, Decompressed.Length);
            memoryStream.Position = 0;

            NewType.Path = InputPath;
            NewType.Name = binaryReader.ReadString();
            NewType.Flag = binaryReader.ReadInt16();
            NewType.BehaviorClass = binaryReader.ReadInt16();
            NewType.Range = binaryReader.ReadInt16();
            NewType.Sight = binaryReader.ReadInt16();
            NewType.Viewcone = binaryReader.ReadInt16();
            NewType.MaxSpeed = binaryReader.ReadSingle();
            NewType.ModelScale = binaryReader.ReadSingle();
            NewType.BumpRadius = binaryReader.ReadInt16();
            NewType.SoundID = binaryReader.ReadInt32();
            NewType.SoundRadius = binaryReader.ReadInt16();
            NewType.SoundType = binaryReader.ReadInt16();
            NewType.RenderRadius = binaryReader.ReadInt16();
            NewType.GravityToggle = binaryReader.ReadByte();
            NewType.CameraAlligned = binaryReader.ReadByte();
            NewType.ZSortToggle = binaryReader.ReadByte();



            /*
             * 
             * binaryWriter.Write(TextureData[ThisTexture].textureName);
                binaryWriter.Write(TextureData[ThisTexture].CombineModeA);
                binaryWriter.Write(TextureData[ThisTexture].CombineModeB);

                for (int ThisBool = 0; ThisBool < F3DEX095_Parameters.GeometryModes.Length; ThisBool++)
                {
                    binaryWriter.Write(TextureData[ThisTexture].GeometryBools[ThisBool]);
                }

                binaryWriter.Write(TextureData[ThisTexture].RenderModeA);
                binaryWriter.Write(TextureData[ThisTexture].RenderModeB);

                if (TextureData[ThisTexture].texturePath != null)
                {
                    binaryWriter.Write(TextureData[ThisTexture].texturePath);
                    binaryWriter.Write(TextureData[ThisTexture].textureScrollS);
                    binaryWriter.Write(TextureData[ThisTexture].textureScrollT);
                    binaryWriter.Write(TextureData[ThisTexture].textureScreen);

                    binaryWriter.Write(TextureData[ThisTexture].SFlag);
                    binaryWriter.Write(TextureData[ThisTexture].TFlag);


                    binaryWriter.Write(TextureData[ThisTexture].TextureFormat);
                    binaryWriter.Write(TextureData[ThisTexture].BitSize);


                    binaryWriter.Write(TextureData[ThisTexture].vertAlpha);

                    binaryWriter.Write(TextureData[ThisTexture].textureWidth);
                    binaryWriter.Write(TextureData[ThisTexture].textureHeight);
                }
                else
                {
                    binaryWriter.Write("NULL");
                }

            */
            int TextureCount = binaryReader.ReadInt32();
            NewType.TextureData = new TM64_Geometry.OK64Texture[TextureCount];
            for (int ThisTexture = 0; ThisTexture < TextureCount; ThisTexture++)
            {
                NewType.TextureData[ThisTexture] = new TM64_Geometry.OK64Texture();
                NewType.TextureData[ThisTexture].textureName = binaryReader.ReadString();

                NewType.TextureData[ThisTexture].CombineModeA = binaryReader.ReadInt32();
                NewType.TextureData[ThisTexture].CombineModeB = binaryReader.ReadInt32();

                NewType.TextureData[ThisTexture].GeometryBools = new bool[F3DEX095_Parameters.GeometryModes.Length];
                for (int ThisBool = 0; ThisBool < F3DEX095_Parameters.GeometryModes.Length; ThisBool++)
                {
                    NewType.TextureData[ThisTexture].GeometryBools[ThisBool] = binaryReader.ReadBoolean();
                }

                NewType.TextureData[ThisTexture].RenderModeA = binaryReader.ReadInt32();
                NewType.TextureData[ThisTexture].RenderModeB = binaryReader.ReadInt32();

                NewType.TextureData[ThisTexture].texturePath = binaryReader.ReadString();
                if ((NewType.TextureData[ThisTexture].texturePath != "NULL") && (NewType.TextureData[ThisTexture].texturePath != null))
                {
                    
                    NewType.TextureData[ThisTexture].textureScrollS = binaryReader.ReadInt32();
                    NewType.TextureData[ThisTexture].textureScrollT = binaryReader.ReadInt32();
                    NewType.TextureData[ThisTexture].textureScreen = binaryReader.ReadInt32();

                    NewType.TextureData[ThisTexture].SFlag = binaryReader.ReadInt32();
                    NewType.TextureData[ThisTexture].TFlag = binaryReader.ReadInt32();


                    NewType.TextureData[ThisTexture].TextureFormat = binaryReader.ReadInt32();
                    NewType.TextureData[ThisTexture].BitSize = binaryReader.ReadInt32();

                    NewType.TextureData[ThisTexture].vertAlpha = binaryReader.ReadInt32();
                    
                    NewType.TextureData[ThisTexture].textureWidth = binaryReader.ReadInt32();
                    NewType.TextureData[ThisTexture].textureHeight = binaryReader.ReadInt32();
                   
                }
            }
            int ModelCount = binaryReader.ReadInt32();
            NewType.ModelData = new TM64_Geometry.OK64F3DObject[ModelCount];
            for (int ThisModel = 0; ThisModel < ModelCount; ThisModel++)
            {
                NewType.ModelData[ThisModel] = new TM64_Geometry.OK64F3DObject();
                NewType.ModelData[ThisModel].objectName = binaryReader.ReadString();
                NewType.ModelData[ThisModel].BoneName = binaryReader.ReadString();
                NewType.ModelData[ThisModel].materialID = binaryReader.ReadInt32();
                NewType.ModelData[ThisModel].vertCount = binaryReader.ReadInt32();
                NewType.ModelData[ThisModel].faceCount = binaryReader.ReadInt32();

                NewType.ModelData[ThisModel].KillDisplayList = new bool[6];
                for (int ThisBool = 0; ThisBool < 6; ThisBool++)
                {
                    NewType.ModelData[ThisModel].KillDisplayList[ThisBool] = binaryReader.ReadBoolean();
                }

                Random ColorRandom = new Random();
                NewType.ModelData[ThisModel].objectColor = new float[] { Convert.ToSingle(ColorRandom.NextDouble()), Convert.ToSingle(ColorRandom.NextDouble()), Convert.ToSingle(ColorRandom.NextDouble()) };
                int MeshLength = binaryReader.ReadInt32();
                NewType.ModelData[ThisModel].meshID = new int[MeshLength];
                for (int ThisMesh = 0; ThisMesh < MeshLength; ThisMesh++)
                {
                    NewType.ModelData[ThisModel].meshID[ThisMesh] = binaryReader.ReadInt32();
                }
                int ModelLength = binaryReader.ReadInt32();
                NewType.ModelData[ThisModel].modelGeometry = new TM64_Geometry.Face[ModelLength];
                for (int ThisGeo = 0; ThisGeo < ModelLength; ThisGeo++)
                {
                    NewType.ModelData[ThisModel].modelGeometry[ThisGeo] = new TM64_Geometry.Face();
                    NewType.ModelData[ThisModel].modelGeometry[ThisGeo].LowX = binaryReader.ReadInt32();
                    NewType.ModelData[ThisModel].modelGeometry[ThisGeo].HighX = binaryReader.ReadInt32();
                    NewType.ModelData[ThisModel].modelGeometry[ThisGeo].LowY = binaryReader.ReadInt32();
                    NewType.ModelData[ThisModel].modelGeometry[ThisGeo].HighY = binaryReader.ReadInt32();
                    int X, Y, Z;
                    X = binaryReader.ReadInt32();
                    Y = binaryReader.ReadInt32();
                    Z = binaryReader.ReadInt32();
                    NewType.ModelData[ThisModel].modelGeometry[ThisGeo].CenterPosition = new Vector3D(X, Y, Z);
                    NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertIndex = new TM64_Geometry.VertIndex();
                    NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertIndex.IndexA = binaryReader.ReadInt32();
                    NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertIndex.IndexB = binaryReader.ReadInt32();
                    NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertIndex.IndexC = binaryReader.ReadInt32();
                    NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData = new TM64_Geometry.Vertex[3];
                    for (int ThisVert = 0; ThisVert < 3; ThisVert++)
                    {
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert] = new TM64_Geometry.Vertex();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position = new TM64_Geometry.Position();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.x = binaryReader.ReadInt16();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.y = binaryReader.ReadInt16();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.z = binaryReader.ReadInt16();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.u = binaryReader.ReadSingle();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.v = binaryReader.ReadSingle();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.sBase = binaryReader.ReadSingle();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.tBase = binaryReader.ReadSingle();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.sPure = binaryReader.ReadSingle();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].position.tPure = binaryReader.ReadSingle();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].color = new TM64_Geometry.OK64Color();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].color.R = binaryReader.ReadByte();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].color.G = binaryReader.ReadByte();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].color.B = binaryReader.ReadByte();
                        NewType.ModelData[ThisModel].modelGeometry[ThisGeo].VertData[ThisVert].color.A = binaryReader.ReadByte();
                    }
                }
            }



            if (binaryReader.ReadBoolean())
            {
                int Count = binaryReader.ReadInt32();
                NewType.ObjectHitbox = new TM64_Objects.OK64Collide[Count];
                for (int ThisHit = 0; ThisHit < Count; ThisHit++)
                {
                    NewType.ObjectHitbox[ThisHit] = new TM64_Objects.OK64Collide(ThisHit.ToString());
                    NewType.ObjectHitbox[ThisHit].Name = binaryReader.ReadString();
                    NewType.ObjectHitbox[ThisHit].Type = binaryReader.ReadInt16();
                    NewType.ObjectHitbox[ThisHit].Status = binaryReader.ReadInt16();
                    NewType.ObjectHitbox[ThisHit].Scale = binaryReader.ReadSingle();
                    NewType.ObjectHitbox[ThisHit].Effect = binaryReader.ReadInt16();
                    NewType.ObjectHitbox[ThisHit].CollideResult = binaryReader.ReadInt16();
                    NewType.ObjectHitbox[ThisHit].HitResult = binaryReader.ReadInt16();
                    NewType.ObjectHitbox[ThisHit].BoxAngle = binaryReader.ReadInt16();
                    NewType.ObjectHitbox[ThisHit].SolidObject = binaryReader.ReadBoolean();

                    NewType.ObjectHitbox[ThisHit].Origin = new short[3];
                    NewType.ObjectHitbox[ThisHit].Size = new short[3];
                    for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                    {
                        NewType.ObjectHitbox[ThisHit].Origin[ThisVector] = binaryReader.ReadInt16();
                        NewType.ObjectHitbox[ThisHit].Size[ThisVector] = binaryReader.ReadInt16();
                    }
                }

            }



            if (binaryReader.ReadBoolean())
            {
                int Position = Convert.ToInt32(binaryReader.BaseStream.Position);
                byte[] NewData = binaryReader.ReadBytes(Convert.ToInt32(memoryStream.Length - binaryReader.BaseStream.Position));
                
                int DataRead = 0;
                NewType.ObjectAnimations = new OKObjectAnimations();

                NewType.ObjectAnimations.WalkAnimation = TarmacGeometry.LoadAnimationObject(out DataRead, NewData);
                Position += DataRead;
                binaryReader.BaseStream.Position = Position;

                NewData = binaryReader.ReadBytes(Convert.ToInt32(memoryStream.Length - binaryReader.BaseStream.Position));

                NewType.ObjectAnimations.TargetAnimation = TarmacGeometry.LoadAnimationObject(out DataRead, NewData);
                Position += DataRead;
                binaryReader.BaseStream.Position = Position;

                NewData = binaryReader.ReadBytes(Convert.ToInt32(memoryStream.Length - binaryReader.BaseStream.Position));
                NewType.ObjectAnimations.DeathAnimation = TarmacGeometry.LoadAnimationObject(out DataRead, NewData);
            }
            else
            {
                NewType.ObjectAnimations = null;
            }
            return NewType;

        }
        public byte[] SaveObjectType(OKObjectType SaveData)
        {
            
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(SaveData.Name);
            binaryWriter.Write(SaveData.Flag);
            binaryWriter.Write(SaveData.BehaviorClass);
            binaryWriter.Write(SaveData.Range);
            binaryWriter.Write(SaveData.Sight);            
            binaryWriter.Write(SaveData.Viewcone);
            binaryWriter.Write(SaveData.MaxSpeed);
            binaryWriter.Write(SaveData.ModelScale);
            binaryWriter.Write(SaveData.BumpRadius);
            binaryWriter.Write(SaveData.SoundID);
            binaryWriter.Write(SaveData.SoundRadius);
            binaryWriter.Write(SaveData.SoundType);
            binaryWriter.Write(SaveData.RenderRadius);
            binaryWriter.Write(SaveData.GravityToggle);
            binaryWriter.Write(SaveData.CameraAlligned);
            binaryWriter.Write(SaveData.ZSortToggle);

            binaryWriter.Write(TarmacGeometry.WriteTextureObjects(SaveData.TextureData));
            binaryWriter.Write(TarmacGeometry.WriteMasterObjects(SaveData.ModelData));
            if (SaveData.ObjectHitbox != null)
            {
                binaryWriter.Write(true);
                binaryWriter.Write(TarmacObject.SaveHitboxFile(SaveData.ObjectHitbox));
            }
            else
            {
                binaryWriter.Write(false);                
            }
            
            if (SaveData.ObjectAnimations != null)
            {
                binaryWriter.Write(true);
                binaryWriter.Write(TarmacGeometry.WriteAnimationObjects(SaveData.ObjectAnimations.WalkAnimation));
                binaryWriter.Write(TarmacGeometry.WriteAnimationObjects(SaveData.ObjectAnimations.TargetAnimation));
                binaryWriter.Write(TarmacGeometry.WriteAnimationObjects(SaveData.ObjectAnimations.DeathAnimation));
            }
            else
            {
                binaryWriter.Write(false);
            }

            return memoryStream.ToArray();
        }
        public byte[] CompileObjectModels(OKObjectType[] SaveData, bool FogToggle)
        {
            byte[] flip = new byte[0];
            byte[] OutputData = new byte[0];
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            int DataLength = 0;
            F3DEX095 TarmacF3D = new F3DEX095();


            
            for (int currentItem = 0; currentItem < SaveData.Length; currentItem++)
            {
                OutputData = TarmacGeometry.WriteRawTextures(OutputData, SaveData[currentItem].TextureData, DataLength);
                OutputData = TarmacGeometry.CompileTextureObjects(OutputData, SaveData[currentItem].TextureData, DataLength, 0xA, true, FogToggle);
                OutputData = TarmacGeometry.CompileF3DObject(OutputData, SaveData[currentItem].ModelData, SaveData[currentItem].TextureData, DataLength, 0xA);                
            }
            binaryWriter.Write(OutputData);
            //Mesh List
            for (int currentItem = 0; currentItem < SaveData.Length; currentItem++)
            {
                for (int ThisModel = 0; ThisModel < SaveData[currentItem].ModelData.Length; ThisModel++)
                {
                    SaveData[currentItem].ModelData[ThisModel].ListPosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                    for (int ThisMesh = 0; ThisMesh < SaveData[currentItem].ModelData[ThisModel].meshPosition.Length; ThisMesh++)
                    {
                        binaryWriter.Write(F3D.BigEndian(Convert.ToUInt32(0x0A000000 | SaveData[currentItem].ModelData[ThisModel].meshPosition[ThisMesh])));
                    }
                }
            }
            int ModelCount = 0;
            for (int currentItem = 0; currentItem < SaveData.Length; currentItem++)
            {
                SaveData[currentItem].ModelPosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
                for (int ThisModel = 0; ThisModel < SaveData[currentItem].ModelData.Length; ThisModel++)
                {
                    for (int ThisZSort = 0; ThisZSort < 5; ThisZSort++)
                    {
                        if (ThisZSort == TarmacGeometry.ZSort(SaveData[currentItem].TextureData[SaveData[currentItem].ModelData[ThisModel].materialID]))
                        {
                            ModelCount++;

                            binaryWriter.Write(F3D.BigEndian(SaveData[currentItem].TextureData[SaveData[currentItem].ModelData[ThisModel].materialID].f3dexPosition));
                            binaryWriter.Write(F3D.BigEndian(SaveData[currentItem].ModelData[ThisModel].ListPosition));
                            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(SaveData[currentItem].ModelData[ThisModel].meshPosition.Length)));
                            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(SaveData[currentItem].ModelScale * 100)));
                        }
                    }

                }

                SaveData[currentItem].ModelCount = ModelCount;
                ModelCount = 0;

                SaveData[currentItem].XLUOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position);
                for (int ThisModel = 0; ThisModel < SaveData[currentItem].ModelData.Length; ThisModel++)
                {
                    for (int ThisZSort = 5; ThisZSort < 8; ThisZSort++)
                    {
                        if (ThisZSort == TarmacGeometry.ZSort(SaveData[currentItem].TextureData[SaveData[currentItem].ModelData[ThisModel].materialID]))
                        {
                            ModelCount++;
                            
                            binaryWriter.Write(F3D.BigEndian(SaveData[currentItem].TextureData[SaveData[currentItem].ModelData[ThisModel].materialID].f3dexPosition));
                            binaryWriter.Write(F3D.BigEndian(SaveData[currentItem].ModelData[ThisModel].ListPosition));
                            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(SaveData[currentItem].ModelData[ThisModel].meshPosition.Length)));
                            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(SaveData[currentItem].ModelScale * 100)));
                        }
                    }

                }
                    
                SaveData[currentItem].XLUCount = ModelCount;
                


            }
            
            return memoryStream.ToArray();
        }

        public byte[] CompileObjectAnimation(OKObjectType[] SaveData, uint Magic)
        {
            byte[] flip = new byte[0];
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            for (int ThisObject = 0; ThisObject < SaveData.Length; ThisObject++)
            {
                if (SaveData[ThisObject].ObjectAnimations != null)
                {
                    if (SaveData[ThisObject].ObjectAnimations.WalkAnimation != null)
                    {
                        
                        binaryWriter.Write(TarmacGeometry.BuildAnimationData(SaveData[ThisObject].ObjectAnimations.WalkAnimation, Convert.ToUInt32(Magic + binaryWriter.BaseStream.Position)));

                        int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                        if (addressAlign == 16)
                            addressAlign = 0;
                        for (int align = 0; align < addressAlign; align++)
                        {
                            binaryWriter.Write(Convert.ToByte(0x00));
                        }
                        binaryWriter.Write(TarmacGeometry.WriteAnimationModels(SaveData[ThisObject].ObjectAnimations.WalkAnimation, SaveData[ThisObject], Convert.ToInt32(Magic + binaryWriter.BaseStream.Position)));

                        SaveData[ThisObject].ObjectAnimations.WalkPosition = Convert.ToInt32(binaryWriter.BaseStream.Position + Magic);
                        binaryWriter.Write(TarmacGeometry.BuildAnimationTable(SaveData[ThisObject].ObjectAnimations.WalkAnimation, SaveData[ThisObject]));

                        addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                        if (addressAlign == 16)
                            addressAlign = 0;
                        for (int align = 0; align < addressAlign; align++)
                        {
                            binaryWriter.Write(Convert.ToByte(0x00));
                        }
                    }
                    else
                    {
                        SaveData[ThisObject].ObjectAnimations.WalkPosition = Convert.ToInt32(0xFFFFFFFF);
                    }

                    if (SaveData[ThisObject].ObjectAnimations.TargetAnimation != null)
                    {
                        
                        binaryWriter.Write(TarmacGeometry.BuildAnimationData(SaveData[ThisObject].ObjectAnimations.TargetAnimation, Convert.ToUInt32(Magic + binaryWriter.BaseStream.Position)));
                        int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                        if (addressAlign == 16)
                            addressAlign = 0;
                        for (int align = 0; align < addressAlign; align++)
                        {
                            binaryWriter.Write(Convert.ToByte(0x00));
                        }
                        binaryWriter.Write(TarmacGeometry.WriteAnimationModels(SaveData[ThisObject].ObjectAnimations.TargetAnimation, SaveData[ThisObject], Convert.ToInt32(Magic + binaryWriter.BaseStream.Position)));
                        
                        SaveData[ThisObject].ObjectAnimations.TargetPosition = Convert.ToInt32(binaryWriter.BaseStream.Position + Magic);
                        binaryWriter.Write(TarmacGeometry.BuildAnimationTable(SaveData[ThisObject].ObjectAnimations.TargetAnimation, SaveData[ThisObject]));
                        addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                        if (addressAlign == 16)
                            addressAlign = 0;
                        for (int align = 0; align < addressAlign; align++)
                        {
                            binaryWriter.Write(Convert.ToByte(0x00));
                        }
                        
                        
                    }
                    else
                    {
                        SaveData[ThisObject].ObjectAnimations.TargetPosition = Convert.ToInt32(0xFFFFFFFF);
                    }

                    if (SaveData[ThisObject].ObjectAnimations.DeathAnimation != null)
                    {
                        
                        binaryWriter.Write(TarmacGeometry.BuildAnimationData(SaveData[ThisObject].ObjectAnimations.DeathAnimation, Convert.ToUInt32(Magic + binaryWriter.BaseStream.Position)));
                        int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                        if (addressAlign == 16)
                            addressAlign = 0;
                        for (int align = 0; align < addressAlign; align++)
                        {
                            binaryWriter.Write(Convert.ToByte(0x00));
                        }
                        binaryWriter.Write(TarmacGeometry.WriteAnimationModels(SaveData[ThisObject].ObjectAnimations.DeathAnimation, SaveData[ThisObject], Convert.ToInt32(Magic + binaryWriter.BaseStream.Position)));



                        SaveData[ThisObject].ObjectAnimations.DeathPosition = Convert.ToInt32(binaryWriter.BaseStream.Position + Magic);
                        binaryWriter.Write(TarmacGeometry.BuildAnimationTable(SaveData[ThisObject].ObjectAnimations.DeathAnimation, SaveData[ThisObject]));
                        addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                        if (addressAlign == 16)
                            addressAlign = 0;
                        for (int align = 0; align < addressAlign; align++)
                        {
                            binaryWriter.Write(Convert.ToByte(0x00));
                        }
                        
                    }
                    else
                    {
                        SaveData[ThisObject].ObjectAnimations.DeathPosition = Convert.ToInt32(0xFFFFFFFF);
                    }
                    F3DEX095 F3D = new F3DEX095();

                    SaveData[ThisObject].AnimationOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position + Magic);
                    binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(0x0A000000 | SaveData[ThisObject].ObjectAnimations.WalkPosition)));
                    binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(0x0A000000 | SaveData[ThisObject].ObjectAnimations.TargetPosition)));
                    binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(0x0A000000 | SaveData[ThisObject].ObjectAnimations.DeathPosition)));
                }
                else
                {
                    SaveData[ThisObject].AnimationOffset = Convert.ToUInt32(0xFFFFFFFF);
                }
            }
            

            return memoryStream.ToArray();
        }

        /*
        public byte[] CompileSingleAnimation(OKObjectAnimations ObjectAnimations, int Magic, bool WriteModels)
        {
            byte[] flip = new byte[0];
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);


            binaryWriter.Write(TarmacGeometry.BuildAnimationData(ObjectAnimations.WalkAnimation, Convert.ToInt32(Magic + binaryWriter.BaseStream.Position)));

            int addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            binaryWriter.Write(TarmacGeometry.WriteAnimationModels(ObjectAnimations.WalkAnimation, SaveData[ThisObject], Convert.ToInt32(Magic + binaryWriter.BaseStream.Position)));

            ObjectAnimations.WalkPosition = Convert.ToInt32(binaryWriter.BaseStream.Position + Magic);
            binaryWriter.Write(TarmacGeometry.BuildAnimationTable(ObjectAnimations.WalkAnimation, SaveData[ThisObject]));

            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
                    

        SaveData[ThisObject].AnimationPosition = Convert.ToUInt32(binaryWriter.BaseStream.Position + Magic);
        binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(0x0A000000 | SaveData[ThisObject].ObjectAnimations.WalkPosition)));
                    


            return memoryStream.ToArray();
        }
        */
        public byte[] SaveObjectTypeRaw(OKObjectType[] SaveData)
        {
            byte[] flip = new byte[0];
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            

            flip = BitConverter.GetBytes(SaveData.Length);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            for (int ThisType = 0; ThisType < SaveData.Length; ThisType++)
            {
                binaryWriter.Write(F3D.BigEndian(SaveData[ThisType].BehaviorClass));
                binaryWriter.Write(F3D.BigEndian(SaveData[ThisType].Range));//
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(SaveData[ThisType].BumpRadius)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(SaveData[ThisType].MaxSpeed * 100)));//
                binaryWriter.Write(F3D.BigEndian(SaveData[ThisType].Sight));
                binaryWriter.Write(F3D.BigEndian(SaveData[ThisType].Viewcone));//
                binaryWriter.Write(F3D.BigEndian(SaveData[ThisType].SoundRadius));
                binaryWriter.Write(F3D.BigEndian(SaveData[ThisType].RenderRadius));//

                binaryWriter.Write(Convert.ToByte(SaveData[ThisType].SoundType));
                binaryWriter.Write(Convert.ToByte(SaveData[ThisType].ZSortToggle)); //
                binaryWriter.Write(Convert.ToByte(SaveData[ThisType].GravityToggle));
                binaryWriter.Write(Convert.ToByte(SaveData[ThisType].CameraAlligned));

                binaryWriter.Write(Convert.ToByte(SaveData[ThisType].ModelCount));
                binaryWriter.Write(Convert.ToByte(SaveData[ThisType].XLUCount)); //
                if (SaveData[ThisType].ObjectHitbox != null)
                {
                    binaryWriter.Write(Convert.ToByte(SaveData[ThisType].ObjectHitbox.Length)); //
                }
                else
                {
                    binaryWriter.Write(Convert.ToByte(0xFF));
                }                
                binaryWriter.Write(Convert.ToByte(SaveData[ThisType].Flag)); //

                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(SaveData[ThisType].ModelScale * 100.0f)));
                binaryWriter.Write(Convert.ToInt16(0)); //Padding

                binaryWriter.Write(F3D.BigEndian(SaveData[ThisType].SoundID));
                if (SaveData[ThisType].ObjectHitbox != null)
                {
                    binaryWriter.Write(F3D.BigEndian(SaveData[ThisType].HitboxOffset));
                }
                else
                {
                    binaryWriter.Write(0xFFFFFFFF);
                }
                binaryWriter.Write(F3D.BigEndian(SaveData[ThisType].ModelPosition));
                binaryWriter.Write(F3D.BigEndian(SaveData[ThisType].XLUOffset));
                binaryWriter.Write(F3D.BigEndian(SaveData[ThisType].AnimationOffset));
            }
            return memoryStream.ToArray();
        }



        public MemoryCard LoadGhost(byte[] fileData)
        {
            
            MemoryCard memoryCard = new MemoryCard();
            memoryCard.ghostData = new GhostData[2];
            int typeOffset = 0;
            if (memoryCard.fileType == 2)
            {
                typeOffset = 0x1040;
            }
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            byte[] endianFlip = new byte[0];

            memoryStream.Write(fileData, 0, fileData.Length);

            memoryCard.ghostData[0] = new GhostData();
            memoryCard.ghostData[1] = new GhostData();


            binaryReader.BaseStream.Position = typeOffset + 0x500;

            endianFlip = binaryReader.ReadBytes(4);
            Array.Reverse(endianFlip);

            memoryCard.ghostData[0].raceTime = BitConverter.ToInt32(endianFlip, 0);
            if (memoryCard.ghostData[0].raceTime > 0)
            {
                memoryCard.ghostData[0].raceTime = memoryCard.ghostData[0].raceTime / 100;
                binaryReader.BaseStream.Seek(1, SeekOrigin.Current);

                memoryCard.ghostData[0].course = binaryReader.ReadByte();
                memoryCard.ghostData[0].character = binaryReader.ReadByte();

                binaryReader.BaseStream.Position = typeOffset + 0x500;
                memoryCard.ghostData[0].header = binaryReader.ReadBytes(0x80);

                binaryReader.BaseStream.Position = typeOffset + 0x600;
                memoryCard.ghostData[0].ghostInput = binaryReader.ReadBytes(0x3C00);
            }



            binaryReader.BaseStream.Position = typeOffset + 0x580;
            endianFlip = binaryReader.ReadBytes(4);
            Array.Reverse(endianFlip);

            memoryCard.ghostData[1].raceTime = BitConverter.ToInt32(endianFlip, 0);
            if (memoryCard.ghostData[1].raceTime > 0)
            {
                memoryCard.ghostData[1].raceTime = memoryCard.ghostData[1].raceTime / 100;
                binaryReader.BaseStream.Seek(1, SeekOrigin.Current);

                memoryCard.ghostData[1].course = binaryReader.ReadByte();
                memoryCard.ghostData[1].character = binaryReader.ReadByte();

                binaryReader.BaseStream.Position = typeOffset + 0x580;
                memoryCard.ghostData[1].header = binaryReader.ReadBytes(0x80);

                binaryReader.BaseStream.Position = typeOffset + 0x4200;
                memoryCard.ghostData[1].ghostInput = binaryReader.ReadBytes(0x3C00);
            }

            return memoryCard;
        }


        public Header[] loadHeader(byte[] fileData)
        {


            Header[] courseHeader = new Header[20];

            MemoryStream memoryStream = new MemoryStream(fileData);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            binaryReader.BaseStream.Seek(0x122390, SeekOrigin.Begin);
            for (int i = 0; i < 20; i++)
            {
                courseHeader[i] = new Header();

                courseHeader[i].s6Start = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s6Start);

                courseHeader[i].s6End = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s6End);

                courseHeader[i].s47Start = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s47Start);

                courseHeader[i].s47End = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s47End);

                courseHeader[i].s9Start = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s9Start);

                courseHeader[i].s9End = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].s9End);

                courseHeader[i].S47Buffer = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].S47Buffer);

                courseHeader[i].VertCount = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].VertCount);

                courseHeader[i].S7Pointer = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].S7Pointer);

                courseHeader[i].S7Size = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].S7Size);

                courseHeader[i].TexturePointer = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].TexturePointer);

                courseHeader[i].FlagPadding = binaryReader.ReadBytes(4);
                Array.Reverse(courseHeader[i].FlagPadding);

            }


            return courseHeader;
        }
        public string OK64Serial(Course CourseData)
        {

            char[] Alphabet = new char[] { 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Z' };
            Int64 SerialValue = 0;
            Int64 TimeTick = DateTime.Now.Ticks % 100000000;
            SerialValue += TimeTick;
            SerialValue += CourseData.Segment4.Length;
            SerialValue += CourseData.Segment6.Length;
            SerialValue += CourseData.Segment7.Length;
            SerialValue = Math.Abs(SerialValue);

            TimeTick = DateTime.Now.Ticks % 10;
            string KeyStringC = Alphabet[TimeTick].ToString();
            TimeTick += 1;            

            string KeyStringA = Alphabet[TimeTick] + SerialValue.ToString("X").PadRight(8, Alphabet[TimeTick+1]) + KeyStringC;

            SerialValue = 0;
            TimeTick = DateTime.Now.Ticks % 1000000;
            SerialValue += TimeTick;
            SerialValue -= CourseData.Segment4.Length / 100;
            SerialValue += CourseData.Segment6.Length / 100;
            SerialValue += CourseData.Segment7.Length / 100;
            SerialValue = Math.Abs(SerialValue);

            TimeTick = DateTime.Now.Ticks % 10 + 3;
            KeyStringC = Alphabet[TimeTick].ToString();
            TimeTick += 1;

            string KeyStringB = Alphabet[TimeTick + 1] + SerialValue.ToString("X").PadLeft(6, Alphabet[TimeTick]) + KeyStringC; ;

            
            

            string SerialString = KeyStringA + "-" + KeyStringB;
            return SerialString;
        }
        public Course LoadOK64Course(byte[] FileData)
        {
            TM64 Tarmac = new TM64();

            byte[] UncompressedData = Tarmac.DecompressMIO0(FileData);
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            memoryStream.Write(UncompressedData, 0, UncompressedData.Length);
            memoryStream.Position = 0;

            int DataLength = 0;
            float[] FloatArray = new float[2];
            int[] IntArray = new int[2];

            Course CourseData = new Course();
            DataLength = binaryReader.ReadInt32();
            CourseData.Segment4 = binaryReader.ReadBytes(DataLength);
            DataLength = binaryReader.ReadInt32();
            CourseData.Segment6 = binaryReader.ReadBytes(DataLength);
            DataLength = binaryReader.ReadInt32();
            CourseData.Segment7 = binaryReader.ReadBytes(DataLength);

            CourseData.Fog = new OKFog();
            CourseData.Fog.FogToggle = binaryReader.ReadInt16();
            CourseData.Fog.StartDistance = binaryReader.ReadInt16();
            CourseData.Fog.StopDistance = binaryReader.ReadInt16();
            CourseData.Fog.FogColor = new TM64_Geometry.OK64Color();
            CourseData.Fog.FogColor.R = binaryReader.ReadByte();
            CourseData.Fog.FogColor.G = binaryReader.ReadByte();
            CourseData.Fog.FogColor.B = binaryReader.ReadByte();
            CourseData.Fog.FogColor.A = binaryReader.ReadByte();
            CourseData.LapCount = binaryReader.ReadInt32();

            CourseData.Gametype = binaryReader.ReadInt32();
            CourseData.Credits = binaryReader.ReadString();            
            CourseData.Name = binaryReader.ReadString();
            CourseData.SerialNumber = binaryReader.ReadString();


            DataLength = binaryReader.ReadInt32();
            if (DataLength > 0)
            {
                CourseData.BannerData = binaryReader.ReadBytes(DataLength);
            }
            else
            {
                CourseData.BannerData = new byte[0];
            }


            DataLength = binaryReader.ReadInt32();
            if (DataLength > 0)
            {
                CourseData.PreviewData = binaryReader.ReadBytes(DataLength);
            }
            else
            {
                CourseData.PreviewData = new byte[0];
            }

            DataLength = binaryReader.ReadInt32();
            if (DataLength > 0)
            {
                CourseData.RadarData = binaryReader.ReadBytes(DataLength);
            }
            else
            {
                CourseData.RadarData = new byte[0];
            }



            CourseData.MapData = new MiniMap();

            CourseData.MapData.MapCoord = new Vector2D(binaryReader.ReadSingle(), binaryReader.ReadSingle());            
            CourseData.MapData.StartCoord = new Vector2D(binaryReader.ReadSingle(), binaryReader.ReadSingle());
            CourseData.MapData.LineCoord = new Vector2D(binaryReader.ReadSingle(), binaryReader.ReadSingle());

            CourseData.MapData.MapColor = new TM64_Geometry.OK64Color();
            CourseData.MapData.MapColor.R = binaryReader.ReadByte();
            CourseData.MapData.MapColor.G = binaryReader.ReadByte();
            CourseData.MapData.MapColor.B = binaryReader.ReadByte();
            CourseData.MapData.MapColor.A = binaryReader.ReadByte();
            CourseData.MapData.MapScale = binaryReader.ReadSingle();
            CourseData.MapData.Height = binaryReader.ReadInt32();
            CourseData.MapData.Width = binaryReader.ReadInt32();

            DataLength = binaryReader.ReadInt32();
            CourseData.PathEffects = new PathEffect[DataLength];
            for (int ThisEcho = 0; ThisEcho < DataLength; ThisEcho++)
            {
                CourseData.PathEffects[ThisEcho] = new PathEffect();
                CourseData.PathEffects[ThisEcho].StartIndex = binaryReader.ReadInt32();
                CourseData.PathEffects[ThisEcho].EndIndex = binaryReader.ReadInt32();
                CourseData.PathEffects[ThisEcho].Type = binaryReader.ReadInt32();
                CourseData.PathEffects[ThisEcho].Power = binaryReader.ReadInt32();
                CourseData.PathEffects[ThisEcho].BodyColor = new TM64_Geometry.OK64Color();
                CourseData.PathEffects[ThisEcho].BodyColor.R = binaryReader.ReadByte();
                CourseData.PathEffects[ThisEcho].BodyColor.G = binaryReader.ReadByte();
                CourseData.PathEffects[ThisEcho].BodyColor.B = binaryReader.ReadByte();
                CourseData.PathEffects[ThisEcho].AdjColor = new TM64_Geometry.OK64Color();
                CourseData.PathEffects[ThisEcho].AdjColor.R = binaryReader.ReadByte();
                CourseData.PathEffects[ThisEcho].AdjColor.G = binaryReader.ReadByte();
                CourseData.PathEffects[ThisEcho].AdjColor.B = binaryReader.ReadByte();
            }
            CourseData.BombArray = new VSBomb[7];
            for (int ThisBomb = 0; ThisBomb < 7; ThisBomb++)
            {
                CourseData.BombArray[ThisBomb] = new VSBomb();
                CourseData.BombArray[ThisBomb].Point = binaryReader.ReadInt16();
                CourseData.BombArray[ThisBomb].Type = binaryReader.ReadInt16();
            }

            CourseData.GhostPath = binaryReader.ReadString();
            CourseData.SkyColors = new Sky();
            CourseData.SkyColors.TopColor = new TM64_Geometry.OK64Color();
            CourseData.SkyColors.TopColor.R = binaryReader.ReadByte();
            CourseData.SkyColors.TopColor.G = binaryReader.ReadByte();
            CourseData.SkyColors.TopColor.B = binaryReader.ReadByte();
            CourseData.SkyColors.TopColor.A = binaryReader.ReadByte();
            CourseData.SkyColors.MidColor = new TM64_Geometry.OK64Color();
            CourseData.SkyColors.MidColor.R = binaryReader.ReadByte();
            CourseData.SkyColors.MidColor.G = binaryReader.ReadByte();
            CourseData.SkyColors.MidColor.B = binaryReader.ReadByte();
            CourseData.SkyColors.MidColor.A = binaryReader.ReadByte();
            CourseData.SkyColors.BotColor = new TM64_Geometry.OK64Color();
            CourseData.SkyColors.BotColor.R = binaryReader.ReadByte();
            CourseData.SkyColors.BotColor.G = binaryReader.ReadByte();
            CourseData.SkyColors.BotColor.B = binaryReader.ReadByte();
            CourseData.SkyColors.BotColor.A = binaryReader.ReadByte();
            CourseData.SkyColors.SkyType = binaryReader.ReadInt32();
            CourseData.SkyColors.WeatherType = binaryReader.ReadInt32();

            CourseData.MusicID = binaryReader.ReadInt32();
            DataLength = binaryReader.ReadInt32();
            CourseData.SongData = new TM64_Sound.OK64Song();
            CourseData.SongData.SequenceData = binaryReader.ReadBytes(DataLength);
            DataLength = binaryReader.ReadInt32();
            CourseData.SongData.InstrumentData = binaryReader.ReadBytes(DataLength);

            CourseData.GoalBannerBool = binaryReader.ReadInt16();
            CourseData.SkyboxBool = binaryReader.ReadInt16();


            DataLength = binaryReader.ReadInt32();
            CourseData.TextureObjects = new TM64_Geometry.OK64Texture[DataLength];
            for (int CurrentTexture = 0; CurrentTexture < CourseData.TextureObjects.Length; CurrentTexture++)
            {
                CourseData.TextureObjects[CurrentTexture] = new TM64_Geometry.OK64Texture();
                CourseData.TextureObjects[CurrentTexture].texturePath = binaryReader.ReadString();
                if (CourseData.TextureObjects[CurrentTexture].texturePath != "NULL")
                {
                    CourseData.TextureObjects[CurrentTexture].compressedSize = binaryReader.ReadInt32();
                    CourseData.TextureObjects[CurrentTexture].fileSize = binaryReader.ReadInt32();
                    CourseData.TextureObjects[CurrentTexture].compressedTexture = binaryReader.ReadBytes(CourseData.TextureObjects[CurrentTexture].compressedSize);
                    CourseData.TextureObjects[CurrentTexture].rawTexture = binaryReader.ReadBytes(CourseData.TextureObjects[CurrentTexture].fileSize);

                    DataLength = binaryReader.ReadInt32();
                    if (DataLength != 0)
                    {
                        CourseData.TextureObjects[CurrentTexture].PaletteData = binaryReader.ReadBytes(DataLength);
                    }
                }
            }

            DataLength = binaryReader.ReadInt32();
            CourseData.ScrollData = binaryReader.ReadBytes(DataLength);
            DataLength = binaryReader.ReadInt32();
            CourseData.WaterData = binaryReader.ReadBytes(DataLength);
            DataLength = binaryReader.ReadInt32();
            CourseData.ScreenData = binaryReader.ReadBytes(DataLength);
            DataLength = binaryReader.ReadInt32();
            CourseData.KillDisplayData = binaryReader.ReadBytes(DataLength);
            DataLength = binaryReader.ReadInt32();
            CourseData.GhostData = binaryReader.ReadBytes(DataLength);

            CourseData.GhostCharacter = binaryReader.ReadInt32();

            DataLength = binaryReader.ReadInt32();
            CourseData.ObjectListData = binaryReader.ReadBytes(DataLength);
            DataLength = binaryReader.ReadInt32();
            CourseData.ObjectTypeData = binaryReader.ReadBytes(DataLength);
            DataLength = binaryReader.ReadInt32();
            CourseData.ObjectModelData = binaryReader.ReadBytes(DataLength);
            DataLength = binaryReader.ReadInt32();
            CourseData.ObjectAnimationData = binaryReader.ReadBytes(DataLength);
            DataLength = binaryReader.ReadInt32();
            CourseData.ObjectHitboxData = binaryReader.ReadBytes(DataLength);

            CourseData.DistributeBool = binaryReader.ReadInt16();
            CourseData.PathCount = binaryReader.ReadInt16();            
            CourseData.OK64HeaderData = new OK64Header();
            CourseData.OK64HeaderData.PathLength = new short[4];
            CourseData.OK64HeaderData.PathLength[0] = binaryReader.ReadInt16();
            CourseData.OK64HeaderData.PathLength[1] = binaryReader.ReadInt16();
            CourseData.OK64HeaderData.PathLength[2] = binaryReader.ReadInt16();
            CourseData.OK64HeaderData.PathLength[3] = binaryReader.ReadInt16();
            CourseData.PathSurface = new int[4];
            CourseData.PathSurface[0] = binaryReader.ReadInt32();
            CourseData.PathSurface[1] = binaryReader.ReadInt32();
            CourseData.PathSurface[2] = binaryReader.ReadInt32();
            CourseData.PathSurface[3] = binaryReader.ReadInt32();
            CourseData.OK64HeaderData.WaterLevel = binaryReader.ReadSingle();
            CourseData.OK64HeaderData.WaterType = binaryReader.ReadInt32();
            CourseData.OK64HeaderData.SectionViewPosition = binaryReader.ReadInt32();
            CourseData.OK64HeaderData.XLUViewPosition = binaryReader.ReadInt32();
            CourseData.OK64HeaderData.SurfaceMapPosition = binaryReader.ReadInt32();
            CourseData.PathOffsets = new uint[4];
            CourseData.PathOffsets[0] = binaryReader.ReadUInt32();
            CourseData.PathOffsets[1] = binaryReader.ReadUInt32();
            CourseData.PathOffsets[2] = binaryReader.ReadUInt32();
            CourseData.PathOffsets[3] = binaryReader.ReadUInt32();
            CourseData.ManualTempo = binaryReader.ReadInt32();


            return CourseData;
        }
        public byte[] SaveOK64Course(Course CourseData)
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            MemoryStream memoryStream = new MemoryStream();            
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            TM64 Tarmac = new TM64();
            
            binaryWriter.Write(CourseData.Segment4.Length);
            binaryWriter.Write(CourseData.Segment4);
            binaryWriter.Write(CourseData.Segment6.Length);
            binaryWriter.Write(CourseData.Segment6);
            binaryWriter.Write(CourseData.Segment7.Length);
            binaryWriter.Write(CourseData.Segment7);


            binaryWriter.Write(CourseData.Fog.FogToggle);
            binaryWriter.Write(CourseData.Fog.StartDistance);
            binaryWriter.Write(CourseData.Fog.StopDistance);
            binaryWriter.Write(CourseData.Fog.FogColor.R);
            binaryWriter.Write(CourseData.Fog.FogColor.G);
            binaryWriter.Write(CourseData.Fog.FogColor.B);
            binaryWriter.Write(CourseData.Fog.FogColor.A);
            binaryWriter.Write(CourseData.LapCount);


            binaryWriter.Write(CourseData.Gametype);
            binaryWriter.Write(CourseData.Credits);            
            binaryWriter.Write(CourseData.Name);
            binaryWriter.Write(CourseData.SerialNumber);
            if (CourseData.BannerPath.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                if (File.Exists(CourseData.BannerPath))
                {
                    Bitmap bitmapData = new Bitmap(CourseData.BannerPath);
                    N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                    CourseData.BannerData = Tarmac.CompressMIO0(imageData);
                    binaryWriter.Write(CourseData.BannerData.Length);
                    binaryWriter.Write(CourseData.BannerData);
                }
                else
                {
                    MessageBox.Show("Please select replacement for missing Banner Texture");
                    if (FileOpen.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(FileOpen.FileName))
                        {
                            Bitmap bitmapData = new Bitmap(FileOpen.FileName);
                            N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                            CourseData.BannerData = Tarmac.CompressMIO0(imageData);
                            binaryWriter.Write(CourseData.BannerData.Length);
                            binaryWriter.Write(CourseData.BannerData);
                        }
                    }
                    else
                    {
                        binaryWriter.Write(0);
                    }
                }
            }
            else
            {
                binaryWriter.Write(0);
            }

            if (CourseData.PreviewPath.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                if (File.Exists(CourseData.PreviewPath))
                {
                    Bitmap bitmapData = new Bitmap(CourseData.PreviewPath);
                    N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                    CourseData.PreviewData = Tarmac.CompressMIO0(imageData);
                    binaryWriter.Write(CourseData.PreviewData.Length);
                    binaryWriter.Write(CourseData.PreviewData);

                }
                else
                {
                    MessageBox.Show("Please select replacement for missing Preview Texture");
                    if (FileOpen.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(FileOpen.FileName))
                        {
                            Bitmap bitmapData = new Bitmap(FileOpen.FileName);
                            N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.RGBA16, bitmapData);
                            CourseData.PreviewData = Tarmac.CompressMIO0(imageData);
                            binaryWriter.Write(CourseData.PreviewData.Length);
                            binaryWriter.Write(CourseData.PreviewData);
                        }
                    }
                    else
                    {
                        binaryWriter.Write(0);
                    }
                }
            }
            else
            {
                binaryWriter.Write(0);
            }

            if (CourseData.MapData.MinimapPath.Length > 0)
            {
                N64Codec[] n64Codec = new N64Codec[] { N64Codec.RGBA16, N64Codec.CI8 };
                byte[] imageData = null;
                byte[] paletteData = null;
                if (File.Exists(CourseData.MapData.MinimapPath))
                {

                    Bitmap bitmapData = new Bitmap(CourseData.MapData.MinimapPath);
                    N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.I4, bitmapData);
                    CourseData.RadarData = Tarmac.CompressMIO0(imageData);
                    binaryWriter.Write(CourseData.RadarData.Length);
                    binaryWriter.Write(CourseData.RadarData);
                    CourseData.MapData.Width = bitmapData.Width;
                    CourseData.MapData.Height = bitmapData.Height;
                }
                else
                {
                    MessageBox.Show("Please select replacement for missing Minimap Texture");
                    if (FileOpen.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(FileOpen.FileName))
                        {
                            Bitmap bitmapData = new Bitmap(FileOpen.FileName);
                            N64Graphics.Convert(ref imageData, ref paletteData, N64Codec.I4, bitmapData);
                            CourseData.RadarData = Tarmac.CompressMIO0(imageData);
                            binaryWriter.Write(CourseData.RadarData.Length);
                            binaryWriter.Write(CourseData.RadarData);
                            CourseData.MapData.Width = bitmapData.Width;
                            CourseData.MapData.Height = bitmapData.Height;
                        }
                    }
                    else
                    {
                        binaryWriter.Write(0);
                    }
                }
            }
            else
            {
                binaryWriter.Write(0);
            }

            binaryWriter.Write(CourseData.MapData.MapCoord[0]);
            binaryWriter.Write(CourseData.MapData.MapCoord[1]);
            binaryWriter.Write(CourseData.MapData.StartCoord[0]);
            binaryWriter.Write(CourseData.MapData.StartCoord[1]);
            binaryWriter.Write(CourseData.MapData.LineCoord[0]);
            binaryWriter.Write(CourseData.MapData.LineCoord[1]);
            binaryWriter.Write(CourseData.MapData.MapColor.R);
            binaryWriter.Write(CourseData.MapData.MapColor.G);
            binaryWriter.Write(CourseData.MapData.MapColor.B);
            binaryWriter.Write(CourseData.MapData.MapColor.A);
            binaryWriter.Write(CourseData.MapData.MapScale);
            binaryWriter.Write(CourseData.MapData.Height);
            binaryWriter.Write(CourseData.MapData.Width);

            binaryWriter.Write(CourseData.PathEffects.Length);
            for(int ThisEcho = 0; ThisEcho < CourseData.PathEffects.Length; ThisEcho++)
            {
                binaryWriter.Write(CourseData.PathEffects[ThisEcho].StartIndex);
                binaryWriter.Write(CourseData.PathEffects[ThisEcho].EndIndex);
                binaryWriter.Write(CourseData.PathEffects[ThisEcho].Type);
                binaryWriter.Write(CourseData.PathEffects[ThisEcho].Power);
                binaryWriter.Write(CourseData.PathEffects[ThisEcho].BodyColor.R);
                binaryWriter.Write(CourseData.PathEffects[ThisEcho].BodyColor.G);
                binaryWriter.Write(CourseData.PathEffects[ThisEcho].BodyColor.B);
                binaryWriter.Write(CourseData.PathEffects[ThisEcho].AdjColor.R);
                binaryWriter.Write(CourseData.PathEffects[ThisEcho].AdjColor.G);
                binaryWriter.Write(CourseData.PathEffects[ThisEcho].AdjColor.B);
            }
            
            for (int ThisBomb = 0; ThisBomb < 7; ThisBomb++)
            {
                binaryWriter.Write(CourseData.BombArray[ThisBomb].Point);
                binaryWriter.Write(CourseData.BombArray[ThisBomb].Type);
            }
            
            binaryWriter.Write(CourseData.GhostPath);
            binaryWriter.Write(CourseData.SkyColors.TopColor.R);
            binaryWriter.Write(CourseData.SkyColors.TopColor.G);
            binaryWriter.Write(CourseData.SkyColors.TopColor.B);
            binaryWriter.Write(CourseData.SkyColors.TopColor.A);
            binaryWriter.Write(CourseData.SkyColors.MidColor.R);
            binaryWriter.Write(CourseData.SkyColors.MidColor.G);
            binaryWriter.Write(CourseData.SkyColors.MidColor.B);
            binaryWriter.Write(CourseData.SkyColors.MidColor.A);
            binaryWriter.Write(CourseData.SkyColors.BotColor.R);
            binaryWriter.Write(CourseData.SkyColors.BotColor.G);
            binaryWriter.Write(CourseData.SkyColors.BotColor.B);
            binaryWriter.Write(CourseData.SkyColors.BotColor.A);
            binaryWriter.Write(CourseData.SkyColors.SkyType);
            binaryWriter.Write(CourseData.SkyColors.WeatherType);

            binaryWriter.Write(CourseData.MusicID);
            binaryWriter.Write(CourseData.SongData.SequenceData.Length);
            binaryWriter.Write(CourseData.SongData.SequenceData);
            binaryWriter.Write(CourseData.SongData.InstrumentData.Length);
            binaryWriter.Write(CourseData.SongData.InstrumentData);

            binaryWriter.Write(CourseData.GoalBannerBool);
            binaryWriter.Write(CourseData.SkyboxBool);


            binaryWriter.Write(CourseData.TextureObjects.Length);
            List<int> SkippedMaterials = new List<int>();
            for (int CurrentTexture = 0; CurrentTexture < CourseData.TextureObjects.Length; CurrentTexture++)
            {
                foreach (var Index in CourseData.TextureObjects[CurrentTexture].TextureOverWrite)
                {
                    SkippedMaterials.Add(Index);
                }
            }
            for (int CurrentTexture= 0;CurrentTexture < CourseData.TextureObjects.Length;CurrentTexture++)
            {
                if (!SkippedMaterials.Contains(CurrentTexture) && ((CourseData.TextureObjects[CurrentTexture].texturePath != null) && (CourseData.TextureObjects[CurrentTexture].texturePath != "NULL")) )
                {
                    binaryWriter.Write(CourseData.TextureObjects[CurrentTexture].texturePath);
                    binaryWriter.Write(CourseData.TextureObjects[CurrentTexture].compressedSize);
                    binaryWriter.Write(CourseData.TextureObjects[CurrentTexture].fileSize);                    
                    binaryWriter.Write(CourseData.TextureObjects[CurrentTexture].compressedTexture);
                    binaryWriter.Write(CourseData.TextureObjects[CurrentTexture].rawTexture);
                    if (CourseData.TextureObjects[CurrentTexture].PaletteData != null)
                    {
                        binaryWriter.Write(CourseData.TextureObjects[CurrentTexture].PaletteData.Length);
                        binaryWriter.Write(CourseData.TextureObjects[CurrentTexture].PaletteData);
                    }
                    else
                    {
                        binaryWriter.Write(0);
                    }

                }
                else
                {
                    binaryWriter.Write("NULL");
                }
                
                
            }

            binaryWriter.Write(CourseData.ScrollData.Length);
            binaryWriter.Write(CourseData.ScrollData);
            binaryWriter.Write(CourseData.WaterData.Length);
            binaryWriter.Write(CourseData.WaterData);            
            binaryWriter.Write(CourseData.ScreenData.Length);
            binaryWriter.Write(CourseData.ScreenData);
            binaryWriter.Write(CourseData.KillDisplayData.Length);
            binaryWriter.Write(CourseData.KillDisplayData);
            binaryWriter.Write(CourseData.GhostData.Length);
            binaryWriter.Write(CourseData.GhostData);
            binaryWriter.Write(CourseData.GhostCharacter);

            binaryWriter.Write(CourseData.ObjectListData.Length);
            binaryWriter.Write(CourseData.ObjectListData);
            binaryWriter.Write(CourseData.ObjectTypeData.Length);
            binaryWriter.Write(CourseData.ObjectTypeData);
            binaryWriter.Write(CourseData.ObjectModelData.Length);
            binaryWriter.Write(CourseData.ObjectModelData);
            binaryWriter.Write(CourseData.ObjectAnimationData.Length);
            binaryWriter.Write(CourseData.ObjectAnimationData);
            binaryWriter.Write(CourseData.ObjectHitboxData.Length);
            binaryWriter.Write(CourseData.ObjectHitboxData);


            binaryWriter.Write(CourseData.DistributeBool);
            binaryWriter.Write(CourseData.PathCount);
            binaryWriter.Write(CourseData.OK64HeaderData.PathLength[0]);
            binaryWriter.Write(CourseData.OK64HeaderData.PathLength[1]);
            binaryWriter.Write(CourseData.OK64HeaderData.PathLength[2]);
            binaryWriter.Write(CourseData.OK64HeaderData.PathLength[3]);
            binaryWriter.Write(CourseData.PathSurface[0]);
            binaryWriter.Write(CourseData.PathSurface[1]);
            binaryWriter.Write(CourseData.PathSurface[2]);
            binaryWriter.Write(CourseData.PathSurface[3]);
            binaryWriter.Write(CourseData.OK64HeaderData.WaterLevel);
            binaryWriter.Write(CourseData.OK64HeaderData.WaterType);
            binaryWriter.Write(CourseData.OK64HeaderData.SectionViewPosition);
            binaryWriter.Write(CourseData.OK64HeaderData.XLUViewPosition);
            binaryWriter.Write(CourseData.OK64HeaderData.SurfaceMapPosition);
            binaryWriter.Write(CourseData.PathOffsets[0]);
            binaryWriter.Write(CourseData.PathOffsets[1]);
            binaryWriter.Write(CourseData.PathOffsets[2]);
            binaryWriter.Write(CourseData.PathOffsets[3]);
            binaryWriter.Write(CourseData.ManualTempo);
            return Tarmac.CompressMIO0(memoryStream.ToArray());


        }
        public byte[] CompileOverKart(Course courseData, byte[] fileData, int cID, int setID, uint HeaderAddress = 0xBE9178)
        {
            //HOTSWAP

            ///This takes precompiled segments and inserts them into the ROM file. It also updates the course header table to reflect
            /// the new data sizes. This allows for proper loading of the course so long as the segments are properly setup. All segment
            /// data should be precompressed where applicable, this assumes that segment 4 and segment 6 are MIO0 compressed and that
            /// Segment 7 has had it's special compression ran. Segment 9 has no compression. fileData is the ROM file as a byte array, and CID
            /// is the ID of the course we're looking to replace based on it's location in the course header table. 


            /// This writes all segments to the end of the file for simplicity. If data was larger than original (which it almost always will be for custom courses)
            /// then it cannot fit in the existing space without overwriting other course data. 
            /// 



            byte[] flip = new byte[0];

            TM64_Geometry mk = new TM64_Geometry();

            fileData = mk.WriteTextures(fileData, courseData);
            courseData.Segment9 = mk.CompileTextureTable(courseData);
            int addressAlign = 0;




            byte[] seg6 = Tarmac.CompressMIO0(courseData.Segment6);
            byte[] seg4 = Tarmac.CompressMIO0(courseData.Segment4);
            byte[] seg7 = Tarmac.compress_seg7(courseData.Segment7);


            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(fileData, 0, fileData.Length);
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            BinaryReader binaryReader = new BinaryReader(memoryStream);


            




            binaryWriter.BaseStream.Position = binaryWriter.BaseStream.Length;



            //allignment

            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            //


            courseData.MenuHeaderData = new MenuHeader();


            //Write Course Banner Texture
            if (courseData.BannerData.Length > 0)
            {

                byte[] compressedData = courseData.BannerData;
                courseData.MenuHeaderData.Banner = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(compressedData);
                addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                if (addressAlign == 16)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
            }
            else
            {
                courseData.MenuHeaderData.Banner = Convert.ToInt32(0);
            }
            //


            //Course Preview Texture
            if (courseData.PreviewData.Length > 0)
            {
                byte[] compressedData = courseData.PreviewData;
                courseData.MenuHeaderData.Preview = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(compressedData);




                addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                if (addressAlign == 16)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
            }
            else
            {
                
                courseData.MenuHeaderData.Preview = Convert.ToInt32(0);
                
            }






            //begin writing header info


            courseData.OK64HeaderData.Version = 6;

            //add sky colors


            //first table
            courseData.OK64HeaderData.Sky = Convert.ToInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.TopColor.R);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.TopColor.G);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.TopColor.B);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidColor.R);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidColor.G);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidColor.B);

            //0x00FF 0x00FF
            binaryWriter.Write(Convert.ToByte(0x00)); //padding to match existing ROM.
            binaryWriter.Write(Convert.ToByte(0xFF)); //padding to match existing ROM.
            binaryWriter.Write(Convert.ToByte(0x00)); //padding to match existing ROM.
            binaryWriter.Write(Convert.ToByte(0xFF)); //padding to match existing ROM.


            //second table
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidColor.R);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidColor.G);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.MidColor.B);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.BotColor.R);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.BotColor.G);
            binaryWriter.Write(Convert.ToByte(0x00));
            binaryWriter.Write(courseData.SkyColors.BotColor.B);


            //0x0000 0x0000
            binaryWriter.Write(Convert.ToByte(0x00)); //padding to match existing ROM.
            binaryWriter.Write(Convert.ToByte(0x00)); //padding to match existing ROM.
            binaryWriter.Write(Convert.ToByte(0x00)); //padding to match existing ROM.
            binaryWriter.Write(Convert.ToByte(0x00)); //padding to match existing ROM.



            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            //Credits
            if (courseData.Credits.Length > 0)
            {
                courseData.OK64HeaderData.Credits = Convert.ToInt32(binaryWriter.BaseStream.Position);

                flip = BitConverter.GetBytes(Convert.ToInt32(courseData.Credits.Length));
                Array.Reverse(flip);
                binaryWriter.Write(flip);
                binaryWriter.Write(Encoding.UTF8.GetBytes(courseData.Credits));
                binaryWriter.Write(0x00);

                addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                if (addressAlign == 16)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
            }
            else
            {
                courseData.OK64HeaderData.Credits = Convert.ToInt32(0);
            }
            //

            //Name
            if (courseData.Name.Length > 0)
            {
                courseData.OK64HeaderData.CourseName = Convert.ToInt32(binaryWriter.BaseStream.Position);

                flip = BitConverter.GetBytes(Convert.ToInt32(courseData.Name.Length));
                Array.Reverse(flip);
                binaryWriter.Write(flip);
                binaryWriter.Write(Encoding.UTF8.GetBytes(courseData.Name));
                binaryWriter.Write(0x00);

                addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                if (addressAlign == 16)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
            }
            else
            {
                courseData.OK64HeaderData.CourseName = Convert.ToInt32(0);
            }
            //


            //Serial
            courseData.OK64HeaderData.SerialKey = Convert.ToInt32(binaryWriter.BaseStream.Position);

            flip = BitConverter.GetBytes(Convert.ToInt32(courseData.SerialNumber.Length));
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            binaryWriter.Write(Encoding.UTF8.GetBytes(courseData.SerialNumber));
            binaryWriter.Write(0x00);

            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            //


            //Staff Ghost
            if (courseData.GhostData.Length > 0)
            {
                courseData.OK64HeaderData.Ghost = Convert.ToInt32(binaryWriter.BaseStream.Position);
                
                binaryWriter.Write(courseData.GhostData);


                addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                if (addressAlign == 16)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }
            }
            else
            {
                courseData.OK64HeaderData.Ghost = Convert.ToInt32(0);
            }
            //





            //Write Course Map Texture
            if (courseData.RadarData.Length > 0)
            {
                courseData.OK64HeaderData.Maps = Convert.ToInt32(binaryWriter.BaseStream.Position);

                byte[] compressedData = courseData.RadarData;



                addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                if (addressAlign == 16)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }


                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.MapData.MapCoord.X)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.MapData.MapCoord.Y)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.MapData.StartCoord.X)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.MapData.StartCoord.Y)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.MapData.LineCoord.X)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.MapData.LineCoord.Y)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.MapData.Height)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.MapData.Width)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.MapData.MapColor.R)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.MapData.MapColor.G)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.MapData.MapColor.B)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(0)));
                binaryWriter.Write(F3D.BigEndian((courseData.MapData.MapScale)));



                binaryWriter.Write(compressedData);
            }



            //OBJECTS
            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            courseData.OK64HeaderData.ObjectDataStart = Convert.ToInt32(binaryWriter.BaseStream.Position);
            
            
            
            binaryWriter.Write(courseData.ObjectTypeData);
            binaryWriter.Write(courseData.ObjectListData);


            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }


            courseData.OK64HeaderData.ObjectModelStart = Convert.ToInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(courseData.ObjectModelData);
            courseData.OK64HeaderData.ObjectAnimationStart = Convert.ToInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(courseData.ObjectAnimationData);            
            binaryWriter.Write(courseData.ObjectHitboxData);
            courseData.OK64HeaderData.ObjectDataEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);
            



            //

            //echo
            courseData.EchoOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);
            flip = BitConverter.GetBytes(Convert.ToInt32(courseData.PathEffects.Length));
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            for (int ThisEcho = 0; ThisEcho < courseData.PathEffects.Length; ThisEcho++)
            {
                flip = BitConverter.GetBytes(Convert.ToInt16(courseData.PathEffects[ThisEcho].StartIndex));
                Array.Reverse(flip);
                binaryWriter.Write(flip);
                flip = BitConverter.GetBytes(Convert.ToInt16(courseData.PathEffects[ThisEcho].EndIndex));
                Array.Reverse(flip);
                binaryWriter.Write(flip);

                binaryWriter.Write(Convert.ToByte(courseData.PathEffects[ThisEcho].Type));
                binaryWriter.Write(Convert.ToByte(courseData.PathEffects[ThisEcho].Power));
                binaryWriter.Write(Convert.ToByte(courseData.PathEffects[ThisEcho].BodyColor.R));
                binaryWriter.Write(Convert.ToByte(courseData.PathEffects[ThisEcho].BodyColor.G));
                binaryWriter.Write(Convert.ToByte(courseData.PathEffects[ThisEcho].BodyColor.B));
                binaryWriter.Write(Convert.ToByte(courseData.PathEffects[ThisEcho].AdjColor.R));
                binaryWriter.Write(Convert.ToByte(courseData.PathEffects[ThisEcho].AdjColor.G));
                binaryWriter.Write(Convert.ToByte(courseData.PathEffects[ThisEcho].AdjColor.B));
            }
            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            courseData.EchoEndOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);


            courseData.OK64HeaderData.GoalBannerToggle = Convert.ToByte(courseData.GoalBannerBool);
            courseData.OK64HeaderData.SkyboxToggle = Convert.ToByte(courseData.SkyboxBool);

            //bombdata
            courseData.OK64HeaderData.BombOffset = Convert.ToInt32(binaryWriter.BaseStream.Position);
            for (int ThisBomb = 0; ThisBomb < 7; ThisBomb++)
            {


                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.BombArray[ThisBomb].Point)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.BombArray[ThisBomb].Type)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToSingle(8.33333333f)));


                binaryWriter.Write(0);
                binaryWriter.Write(0);
                binaryWriter.Write(0);
                binaryWriter.Write(0);
            }
            //allignment
            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }


            //music
            if (courseData.SongData.SequenceData.Length > 0)
            {
                
                int[] tempMusicOffset = new int[2]; //use inside this IF statement to handle the positions of data.
                int[] tempMusicSizes = new int[2]; //use inside this IF statement to handle the positions of data.

                tempMusicOffset[0] = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(courseData.SongData.SequenceData);

                addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                if (addressAlign == 16)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }



                tempMusicSizes[0] = Convert.ToInt32(binaryWriter.BaseStream.Position - tempMusicOffset[0]);

                tempMusicOffset[1] = Convert.ToInt32(binaryWriter.BaseStream.Position);
                binaryWriter.Write(courseData.SongData.InstrumentData);

                addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                if (addressAlign == 16)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }

                tempMusicSizes[1] = Convert.ToInt32(binaryWriter.BaseStream.Position - tempMusicOffset[1]);



                courseData.MusicID = Convert.ToInt32(binaryWriter.BaseStream.Position);

                flip = BitConverter.GetBytes(Convert.ToInt32(tempMusicOffset[0]));
                Array.Reverse(flip);
                binaryWriter.Write(flip);
                flip = BitConverter.GetBytes(Convert.ToInt32(tempMusicSizes[0]));
                Array.Reverse(flip);
                binaryWriter.Write(flip);
                flip = BitConverter.GetBytes(Convert.ToInt32(tempMusicOffset[1]));
                Array.Reverse(flip);
                binaryWriter.Write(flip);
                flip = BitConverter.GetBytes(Convert.ToInt32(tempMusicSizes[1]));
                Array.Reverse(flip);
                binaryWriter.Write(flip);
                binaryWriter.Write(Convert.ToInt32(0));

                addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
                if (addressAlign == 16)
                    addressAlign = 0;
                for (int align = 0; align < addressAlign; align++)
                {
                    binaryWriter.Write(Convert.ToByte(0x00));
                }

            }
            else
            {
                courseData.MusicID = courseData.MusicID;
            }

            //PathOffsets
            courseData.OK64HeaderData.PathOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToUInt32(courseData.PathOffsets[0]))));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToUInt32(courseData.PathOffsets[1]))));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToUInt32(courseData.PathOffsets[2]))));
            binaryWriter.Write(F3D.BigEndian(BitConverter.GetBytes(Convert.ToUInt32(courseData.PathOffsets[3]))));


            //WaterVertex (translucency) and Map Scrolling


            courseData.OK64HeaderData.ScrollStart = Convert.ToInt32(binaryWriter.BaseStream.Position);


            //scroll data
            binaryWriter.Write(courseData.ScrollData);
            binaryWriter.Write(courseData.WaterData);
            binaryWriter.Write(courseData.ScreenData);
            binaryWriter.Write(courseData.KillDisplayData);

            courseData.OK64HeaderData.ScrollEnd = Convert.ToInt32(binaryWriter.BaseStream.Position);

            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            courseData.OK64HeaderData.MapHeader = new CourseHeader();




            // Segment 6

            courseData.OK64HeaderData.MapHeader.s6Start = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(seg6, 0, seg6.Length);

            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            courseData.OK64HeaderData.MapHeader.s6End = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            //


            // Segment 9
            courseData.OK64HeaderData.MapHeader.s9Start = Convert.ToUInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(courseData.Segment9, 0, courseData.Segment9.Length);

            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            courseData.OK64HeaderData.MapHeader.s9End = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            //




            // Segment 4/7
            courseData.OK64HeaderData.MapHeader.s47Start = Convert.ToUInt32(binaryWriter.BaseStream.Position);

            binaryWriter.Write(seg4, 0, seg4.Length);

            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            courseData.OK64HeaderData.MapHeader.s7Start = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(seg7, 0, seg7.Length);


            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }
            courseData.OK64HeaderData.MapHeader.s47End = Convert.ToUInt32(binaryWriter.BaseStream.Position);
            UInt32 seg7RSP = Convert.ToUInt32(0x0F000000 | (courseData.OK64HeaderData.MapHeader.s7Start - courseData.OK64HeaderData.MapHeader.s47Start));

            //






            // Flip Endian on Course Header offsets.

            flip = BitConverter.GetBytes(courseData.OK64HeaderData.MapHeader.s6Start);
            Array.Reverse(flip);
            courseData.OK64HeaderData.MapHeader.s6Start = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(courseData.OK64HeaderData.MapHeader.s6End);
            Array.Reverse(flip);
            courseData.OK64HeaderData.MapHeader.s6End = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(courseData.OK64HeaderData.MapHeader.s47Start);
            Array.Reverse(flip);
            courseData.OK64HeaderData.MapHeader.s47Start = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(courseData.OK64HeaderData.MapHeader.s47End);
            Array.Reverse(flip);
            courseData.OK64HeaderData.MapHeader.s47End = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(courseData.OK64HeaderData.MapHeader.s9Start);
            Array.Reverse(flip);
            courseData.OK64HeaderData.MapHeader.s9Start = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(courseData.OK64HeaderData.MapHeader.s9End);
            Array.Reverse(flip);
            courseData.OK64HeaderData.MapHeader.s9End = BitConverter.ToUInt32(flip, 0);

            flip = BitConverter.GetBytes(seg7RSP);
            Array.Reverse(flip);
            seg7RSP = BitConverter.ToUInt32(flip, 0);
            //


            //calculate # verts

            courseData.OK64HeaderData.MapHeader.VertCount = Convert.ToUInt32(courseData.Segment4.Length / 14);
            flip = BitConverter.GetBytes(courseData.OK64HeaderData.MapHeader.VertCount);
            Array.Reverse(flip);
            courseData.OK64HeaderData.MapHeader.VertCount = BitConverter.ToUInt32(flip, 0);
            //



            //seg7 size

            UInt32 seg7size = Convert.ToUInt32(courseData.Segment7.Length);
            flip = BitConverter.GetBytes(seg7size);
            Array.Reverse(flip);
            seg7size = BitConverter.ToUInt32(flip, 0);
            //


            /// After Calculating the offsets and values above we now write them past the end of the ROM.








            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            uint headerOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position);


            // Version 5

            flip = BitConverter.GetBytes(courseData.OK64HeaderData.Version);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //
            //



            //courseheader


            binaryWriter.Write(courseData.OK64HeaderData.MapHeader.s6Start);
            binaryWriter.Write(courseData.OK64HeaderData.MapHeader.s6End);
            binaryWriter.Write(courseData.OK64HeaderData.MapHeader.s47Start);
            binaryWriter.Write(courseData.OK64HeaderData.MapHeader.s47End);
            binaryWriter.Write(courseData.OK64HeaderData.MapHeader.s9Start);
            binaryWriter.Write(courseData.OK64HeaderData.MapHeader.s9End);

            flip = BitConverter.GetBytes(0x0F000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            binaryWriter.Write(courseData.OK64HeaderData.MapHeader.VertCount);

            binaryWriter.Write(seg7RSP);


            binaryWriter.Write(seg7size);

            flip = BitConverter.GetBytes(0x09000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            flip = BitConverter.GetBytes(0x00000000);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            //


            //ok64header 13 pointers in
            flip = BitConverter.GetBytes(courseData.OK64HeaderData.SectionViewPosition);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(courseData.OK64HeaderData.XLUViewPosition);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(courseData.OK64HeaderData.SurfaceMapPosition);
            Array.Reverse(flip);
            binaryWriter.Write(flip);



            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.Sky));

            binaryWriter.Write(Convert.ToByte(courseData.SkyColors.SkyType));
            binaryWriter.Write(Convert.ToByte(courseData.SkyColors.WeatherType));
            binaryWriter.Write(Convert.ToByte(courseData.DistributeBool));
            binaryWriter.Write(Convert.ToByte(courseData.PathCount));

            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.Credits));
            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.CourseName));
            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.SerialKey));
            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.Ghost));
            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.Maps));
            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.ObjectDataStart));
            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.ObjectModelStart));
            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.ObjectAnimationStart));
            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.ObjectDataEnd));
            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.BombOffset));
            binaryWriter.Write(F3D.BigEndian(courseData.EchoOffset));
            binaryWriter.Write(F3D.BigEndian(courseData.EchoEndOffset));

            binaryWriter.Write(courseData.OK64HeaderData.GoalBannerToggle);
            binaryWriter.Write(courseData.OK64HeaderData.SkyboxToggle); ;            
            binaryWriter.Write(Convert.ToChar(courseData.ManualTempo));
            binaryWriter.Write(Convert.ToChar(courseData.LapCount));

            binaryWriter.Write(Convert.ToByte(courseData.PathSurface[0]));
            binaryWriter.Write(Convert.ToByte(courseData.PathSurface[1]));
            binaryWriter.Write(Convert.ToByte(courseData.PathSurface[2]));
            binaryWriter.Write(Convert.ToByte(courseData.PathSurface[3]));

            binaryWriter.Write(F3D.BigEndian(courseData.MusicID));

            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.OK64HeaderData.PathLength[0])));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.OK64HeaderData.PathLength[1])));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.OK64HeaderData.PathLength[2])));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.OK64HeaderData.PathLength[3])));


            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.OK64HeaderData.WaterType)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(courseData.OK64HeaderData.WaterLevel)));

            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.ScrollStart));

            int ScrollDataSize = courseData.ScrollData.Length;
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ScrollDataSize)));  //WaterVertex Offset
            ScrollDataSize += courseData.WaterData.Length;
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ScrollDataSize)));  //ScreenData Offset
            ScrollDataSize += courseData.ScreenData.Length;
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ScrollDataSize)));  //KillDisplay Data
            ScrollDataSize += courseData.KillDisplayData.Length;
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(ScrollDataSize)));  //Total Data

            binaryWriter.Write(F3D.BigEndian(courseData.OK64HeaderData.PathOffset));


            if (courseData.Fog.FogToggle > 0)
            {
                binaryWriter.Write(F3D.BigEndian(courseData.Fog.StartDistance));
                binaryWriter.Write(F3D.BigEndian(courseData.Fog.StopDistance));
            }
            else
            {
                binaryWriter.Write(F3D.BigEndian(-1));
                binaryWriter.Write(F3D.BigEndian(-1));
            }
            binaryWriter.Write(courseData.Fog.FogColor.R);
            binaryWriter.Write(courseData.Fog.FogColor.G);
            binaryWriter.Write(courseData.Fog.FogColor.B);
            binaryWriter.Write(courseData.Fog.FogColor.A);


            for (int currentPad = 0; currentPad < 16; currentPad++)
            {
                flip = BitConverter.GetBytes(0xFFFFFFFF);
                Array.Reverse(flip);
                binaryWriter.Write(flip);
            }

            addressAlign = 16 - (Convert.ToInt32(binaryWriter.BaseStream.Position) % 16);
            if (addressAlign == 16)
                addressAlign = 0;
            for (int align = 0; align < addressAlign; align++)
            {
                binaryWriter.Write(Convert.ToByte(0x00));
            }

            //




            binaryWriter.BaseStream.Position = (HeaderAddress + (setID * 0x50) + (cID * 4));
            flip = BitConverter.GetBytes(headerOffset);
            Array.Reverse(flip);
            binaryWriter.Write(flip);

            binaryWriter.BaseStream.Position = (HeaderAddress + 0x1400 + (setID * 0xA0) + (cID * 8));

            flip = BitConverter.GetBytes(courseData.MenuHeaderData.Banner);
            Array.Reverse(flip);
            binaryWriter.Write(flip);
            flip = BitConverter.GetBytes(courseData.MenuHeaderData.Preview);
            Array.Reverse(flip);
            binaryWriter.Write(flip);








            byte[] newROM = memoryStream.ToArray();
            return newROM;

        }






    }
}

