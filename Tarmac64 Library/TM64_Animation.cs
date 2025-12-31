using Assimp;
using F3DSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Tarmac64_Library
{
    public class TM64_Animation
    {

        F3DEX095 F3D = new F3DEX095();


        public class OK64Bone
        {
            public string Name { get; set; }
            public int FrameCount { get; set; }
            public short[] Origin { get; set; }
            public OK64Bone[] Children { get; set; }
            public int NodeCount { get; set; }
            public OK64Animation Animation { get; set; }
            public UInt32 TranslationOffset { get; set; }
            public UInt32 RotationOffset { get; set; }
            public UInt32 ScalingOffset { get; set; }
            public UInt32 MeshListOffset { get; set; }

        }


        public class OK64Animation
        {
            public string AnimationName { get; set; }
            public short[] TranslationTime { get; set; }
            public short[] RotationTime { get; set; }
            public short[] ScaleTime { get; set; }
            public short[][] TranslationData { get; set; }
            public short[][] RotationData { get; set; }
            public short[][] ScalingData { get; set; }

            public float[][] RotationFloat { get; set; }
        }






        public byte[] WriteAnimeNodes(OK64Bone Skeleton, TM64_Course.OKObjectType SaveObject, int Magic)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] flip2 = new byte[2];
            List<byte> AnimationData = new List<byte>();

            Skeleton.MeshListOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position + Magic);
            int MeshCount = 0;
            for (int ThisObject = 0; ThisObject < SaveObject.ModelData.Length; ThisObject++)
            {
                if (SaveObject.ModelData[ThisObject].BoneName == Skeleton.Name)
                {
                    MeshCount++;
                }
            }

            Skeleton.NodeCount = MeshCount;

            for (int ThisObject = 0; ThisObject < SaveObject.ModelData.Length; ThisObject++)
            {
                if (SaveObject.ModelData[ThisObject].BoneName == Skeleton.Name)
                {
                    flip2 = BitConverter.GetBytes(SaveObject.TextureData[SaveObject.ModelData[ThisObject].materialID].CCPosition);
                    Array.Reverse(flip2);
                    binaryWriter.Write(flip2);

                    flip2 = BitConverter.GetBytes(Convert.ToInt32(SaveObject.ModelData[ThisObject].meshPosition.Length));
                    Array.Reverse(flip2);
                    binaryWriter.Write(flip2);

                    flip2 = BitConverter.GetBytes(Convert.ToInt32(SaveObject.ModelData[ThisObject].ListPosition | 0x0A000000));
                    Array.Reverse(flip2);
                    binaryWriter.Write(flip2);
                }
            }

            for (int ThisChild = 0; ThisChild < Skeleton.Children.Length; ThisChild++)
            {
                binaryWriter.Write(WriteAnimeNodes(Skeleton.Children[ThisChild], SaveObject, Convert.ToInt32(binaryWriter.BaseStream.Length + Magic)));
            }

            return memoryStream.ToArray();
        }


        public byte[] WriteAnimationData(OK64Bone Skeleton, UInt32 Magic)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] flip2 = new byte[2];
            List<byte> AnimationData = new List<byte>();

            foreach (var ChildBone in Skeleton.Children)
            {
                binaryWriter.Write(WriteAnimationData(ChildBone, Convert.ToUInt32(binaryWriter.BaseStream.Position)));
            }



            Skeleton.RotationOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position + Magic);
            for (int ThisFrame = 0; ThisFrame < Skeleton.Animation.RotationData.Length; ThisFrame++)
            {
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.RotationData[ThisFrame][0])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.RotationData[ThisFrame][2])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.RotationData[ThisFrame][1])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.RotationTime[ThisFrame])));
            }

            Skeleton.TranslationOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position + Magic);
            for (int ThisFrame = 0; ThisFrame < Skeleton.Animation.TranslationData.Length; ThisFrame++)
            {
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.TranslationData[ThisFrame][0])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.TranslationData[ThisFrame][2])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.TranslationData[ThisFrame][1] * -1)));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.TranslationTime[ThisFrame])));
            }

            Skeleton.ScalingOffset = Convert.ToUInt32(binaryWriter.BaseStream.Position + Magic);
            for (int ThisFrame = 0; ThisFrame < Skeleton.Animation.ScalingData.Length; ThisFrame++)
            {
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.ScalingData[ThisFrame][0])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.ScalingData[ThisFrame][2])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.ScalingData[ThisFrame][1])));
                binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.ScaleTime[ThisFrame])));
            }


            return memoryStream.ToArray();
        }

        public byte[] BuildAnimationData(OK64Bone Skeleton, UInt32 Magic)
        {

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            byte[] flip2 = new byte[2];
            List<byte> AnimationData = new List<byte>();


            binaryWriter.Write(WriteAnimationData(Skeleton, Magic));

            return memoryStream.ToArray();
        }

        public byte[] WriteAnimeSkeleton(OK64Bone Skeleton, TM64_Course.OKObjectType SaveObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(F3D.BigEndian(Skeleton.TranslationOffset));
            binaryWriter.Write(F3D.BigEndian(Skeleton.RotationOffset));
            binaryWriter.Write(F3D.BigEndian(Skeleton.ScalingOffset));

            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.TranslationTime.Length)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.RotationTime.Length)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Animation.ScaleTime.Length)));
            //PAD
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(-1)));


            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.NodeCount)));
            binaryWriter.Write(F3D.BigEndian(Convert.ToInt16(Skeleton.Children.Length)));

            binaryWriter.Write(F3D.BigEndian(Convert.ToSingle(SaveObject.ModelScale)));
            binaryWriter.Write(F3D.BigEndian(Skeleton.MeshListOffset));


            foreach (var ChildBone in Skeleton.Children)
            {
                binaryWriter.Write(WriteAnimeSkeleton(ChildBone, SaveObject));
            }

            return memoryStream.ToArray();
        }


        public byte[] BuildAnimationTable(OK64Bone Skeleton, TM64_Course.OKObjectType SaveData)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            List<byte> AnimationData = new List<byte>();
            
            binaryWriter.Write(F3D.BigEndian(Skeleton.FrameCount));

            binaryWriter.Write(WriteAnimeSkeleton(Skeleton, SaveData));


            return memoryStream.ToArray();
        }





        public Point3D RotatePoint(Point3D Point, float[] ObjectAngles)
        {
            var id = Matrix3D.Identity;
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(1, 0, 0), ObjectAngles[0]));
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(0, 1, 0), ObjectAngles[1]));
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(0, 0, 1), ObjectAngles[2]));
            return id.Transform(Point);
        }

        public OK64Bone LoadBone(Node Base, Scene FBX, float ModelScale)
        {
            OK64Bone NewBone = new OK64Bone();
            NewBone.Name = Base.Name;
            NewBone.Children = new OK64Bone[Base.ChildCount];

            //Matrix4x4 OPrime = GetTotalTransform(Base, FBX);

            NewBone.Origin = new short[3];
            NewBone.Origin[0] = Convert.ToInt16(Base.Transform.A4 * 10 * ModelScale);
            NewBone.Origin[1] = Convert.ToInt16(Base.Transform.B4 * 10 * ModelScale);
            NewBone.Origin[2] = Convert.ToInt16(Base.Transform.C4 * 10 * ModelScale);


            //Base.Transform.
            for (int ThisChild = 0; ThisChild < Base.ChildCount; ThisChild++)
            {
                NewBone.Children[ThisChild] = LoadBone(Base.Children[ThisChild], FBX, ModelScale);
            }
            return NewBone;
        }


        public float[] ConvertEuler(Assimp.Quaternion Quat)
        {
            float[] Angle = new float[3];

            // roll (x-axis rotation)
            float sinr_cosp = 2 * (Quat.W * Quat.X + Quat.Y * Quat.Z);
            float cosr_cosp = 1 - 2 * (Quat.X * Quat.X + Quat.Y * Quat.Y);
            Angle[0] = Convert.ToSingle((Math.Atan2(sinr_cosp, cosr_cosp)));

            // pitch (y-axis rotation)
            float sinp = 2 * (Quat.W * Quat.Y - Quat.Z * Quat.X);
            if (Math.Abs(sinp) >= 1)
            {
                if (sinp > 0)
                {
                    Angle[1] = Convert.ToSingle((Math.PI / 2)); // use 90 degrees if out of range
                }
                else if (sinp < 0)
                {
                    Angle[1] = Convert.ToSingle((Math.PI / -2)); // use 90 degrees if out of range
                }
            }
            else
            {
                Angle[1] = Convert.ToSingle((Math.Asin(sinp)));
            }


            // yaw (z-axis rotation)
            float siny_cosp = 2 * (Quat.W * Quat.Z + Quat.X * Quat.Y);
            float cosy_cosp = 1 - 2 * (Quat.Y * Quat.Y + Quat.Z * Quat.Z);
            Angle[2] = Convert.ToSingle((Math.Atan2(siny_cosp, cosy_cosp)));



            return Angle;
        }

        public OK64Animation LoadAnimation(NodeAnimationChannel AnimeChannel, OK64Bone Bone, int FrameCount)
        {
            OK64Animation NewAnime = new OK64Animation();


            NewAnime.TranslationTime = new short[AnimeChannel.PositionKeyCount];
            NewAnime.AnimationName = AnimeChannel.NodeName + "_anime";
            NewAnime.TranslationData = new short[AnimeChannel.PositionKeyCount][];
            for (int ThisFrame = 0; ThisFrame < AnimeChannel.PositionKeyCount; ThisFrame++)
            {
                NewAnime.TranslationData[ThisFrame] = new short[3];
                NewAnime.TranslationTime[ThisFrame] = Convert.ToInt16(AnimeChannel.PositionKeys[ThisFrame].Time);
                for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                {
                    NewAnime.TranslationData[ThisFrame][ThisVector] = Convert.ToInt16(AnimeChannel.PositionKeys[ThisFrame].Value[ThisVector] * 10.0f);
                }
            }


            NewAnime.RotationTime = new short[AnimeChannel.RotationKeyCount];
            NewAnime.RotationData = new short[AnimeChannel.RotationKeyCount][];
            NewAnime.RotationFloat = new float[AnimeChannel.RotationKeyCount][];
            for (int ThisFrame = 0; ThisFrame < AnimeChannel.RotationKeyCount; ThisFrame++)
            {

                NewAnime.RotationData[ThisFrame] = new short[3];
                NewAnime.RotationFloat[ThisFrame] = new float[3];
                NewAnime.RotationTime[ThisFrame] = Convert.ToInt16(AnimeChannel.RotationKeys[ThisFrame].Time);

                float[] RotationTemp = ConvertEuler(AnimeChannel.RotationKeys[ThisFrame].Value);

                for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                {
                    NewAnime.RotationFloat[ThisFrame][ThisVector] = Convert.ToSingle(RotationTemp[ThisVector] / 0.01745329252);
                    if (Math.Abs(NewAnime.RotationFloat[ThisFrame][ThisVector]) < 0.01f)
                    {
                        NewAnime.RotationFloat[ThisFrame][ThisVector] = 0f;
                    }
                    NewAnime.RotationData[ThisFrame][ThisVector] = Convert.ToInt16(NewAnime.RotationFloat[ThisFrame][ThisVector] * 0xB6);
                }


            }



            NewAnime.ScaleTime = new short[AnimeChannel.ScalingKeyCount];
            NewAnime.ScalingData = new short[AnimeChannel.ScalingKeyCount][];

            for (int ThisFrame = 0; ThisFrame < AnimeChannel.ScalingKeyCount; ThisFrame++)
            {
                NewAnime.ScalingData[ThisFrame] = new short[3];
                NewAnime.ScaleTime[ThisFrame] = Convert.ToInt16(AnimeChannel.ScalingKeys[ThisFrame].Time);
                for (int ThisVector = 0; ThisVector < 3; ThisVector++)
                {
                    NewAnime.ScalingData[ThisFrame][ThisVector] = Convert.ToInt16(AnimeChannel.ScalingKeys[ThisFrame].Value[ThisVector] * 10);
                }
            }
            return NewAnime;
        }

        public OK64Bone ParseAnimation(Scene FBX, NodeAnimationChannel AnimeChannel, OK64Bone Bone, int FrameCount)
        {

            if (Bone.Name == AnimeChannel.NodeName)
            {
                Bone.Animation = LoadAnimation(AnimeChannel, Bone, FrameCount);
                Bone.FrameCount = FrameCount;
            }
            foreach (var Child in Bone.Children)
            {
                ParseAnimation(FBX, AnimeChannel, Child, FrameCount);
            }
            return Bone;
        }
        public OK64Bone LoadSkeleton(Scene FBX, float ModelScale)
        {

            Node Base = FBX.RootNode.FindNode("BodyBone");
            OK64Bone Skeleton = LoadBone(Base, FBX, ModelScale);

            Animation Anime = FBX.Animations[0];
            Skeleton.FrameCount = Convert.ToInt32(Anime.DurationInTicks + 1);
            for (int ThisNode = 0; ThisNode < Anime.NodeAnimationChannelCount; ThisNode++)
            {
                ParseAnimation(FBX, Anime.NodeAnimationChannels[ThisNode], Skeleton, Skeleton.FrameCount);
            }
            //GetTransforms(Skeleton, Skeleton.FrameCount, ModelScale);
            return Skeleton;
        }

    }
}
