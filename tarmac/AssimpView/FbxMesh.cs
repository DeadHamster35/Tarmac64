using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AssimpSharp;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace AssimpView
{
    class FbxMesh
    {
        private BasicEffect basicEffect;
        private Buffer vertices;
        private Buffer indices;
        private Device device;
        private int indexCount;

        private string path;

        public FbxMesh(AssimpSharp.Mesh mesh, AssimpSharp.Material mat, Device device, string path)
        {
            this.device = device;
            this.path = path;
            LoadMesh(mesh);
            LoadMaterial(mat);
        }
        public FbxMesh(Device device)
        {
            this.device = device;
            LoadCube();
        }

        public void LoadMaterial(AssimpSharp.Material material)
        {
            // Creates a basic effect
            basicEffect = new BasicEffect(device);
            //if (material.TextureDiffuse != null)
            //{
            //    basicEffect.Texture = Texture2D.Load(GraphicsDevice, Path.Combine(Path.GetDirectoryName(path), material.TextureDiffuse.TextureBase));
            //    basicEffect.TextureEnabled = true;
            //}
            //if (material.ColorDiffuse.HasValue)
            //{
            //    basicEffect.DiffuseColor = material.ColorDiffuse.Value.ToVector4();
            //}
        }

        public void LoadMesh(AssimpSharp.Mesh mesh)
        {
            var vertexSource = new VertexPositionNormalTexture[mesh.Vertices.Length];
            for (int i = 0; i < mesh.Vertices.Length; i++)
            {
                vertexSource[i].Position = mesh.Vertices[i];
                vertexSource[i].Normal = mesh.Normals[i];
                vertexSource[i].TextureCoordinate = Vector2.Zero;
            }
            if (mesh.HasTextureCoords(0))
            {
                var channel = mesh.TextureCoords[0];
                for(int i=0; i<channel.Length; i++)
                {
                    vertexSource[i].TextureCoordinate.X = channel[i].X;
                    vertexSource[i].TextureCoordinate.Y = 1 - channel[i].Y;
                }
            }
            var vbd = new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = VertexPositionNormalTexture.Size * vertexSource.Length,
                Usage = ResourceUsage.Default,
                StructureByteStride = VertexPositionNormalTexture.Size,
            };
            vertices = Buffer.Create(device, vertexSource, vbd);

            var indexSource = new List<int>();
            for(int i=0; i<mesh.Faces.Length; i++)
            {
                var face = mesh.Faces[i];
                if (face.Indices.Length == 3)
                {
                    indexSource.AddRange(face.Indices.Reverse());
                }
                else if (face.Indices.Length == 4)
                {
                    indexSource.Add(face.Indices[2]);
                    indexSource.Add(face.Indices[1]);
                    indexSource.Add(face.Indices[0]);
                    indexSource.Add(face.Indices[0]);
                    indexSource.Add(face.Indices[3]);
                    indexSource.Add(face.Indices[2]);
                }
                else if (face.Indices.Length == 5)
                {
                    indexSource.Add(face.Indices[2]);
                    indexSource.Add(face.Indices[1]);
                    indexSource.Add(face.Indices[0]);
                    indexSource.Add(face.Indices[0]);
                    indexSource.Add(face.Indices[3]);
                    indexSource.Add(face.Indices[2]);
                    indexSource.Add(face.Indices[0]);
                    indexSource.Add(face.Indices[4]);
                    indexSource.Add(face.Indices[3]);
                }
                else
                {
                    throw (new Exception("invalid vertex count of polygon"));
                }
            }
            var ibd = new BufferDescription()
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = Utilities.SizeOf<int>() * indexSource.Count,
                Usage = ResourceUsage.Default,
                StructureByteStride = Utilities.SizeOf<int>(),
            };
            indices = Buffer.Create(device, indexSource.ToArray(), ibd);
            indexCount = indexSource.Count;
        }

        public void LoadCube()
        {
            //// Creates a basic effect
            //basicEffect = new BasicEffect(GraphicsDevice)
            //{
            //    VertexColorEnabled = true,
            //    View = Matrix.LookAtRH(new Vector3(0, 0, 5), new Vector3(0, 0, 0), Vector3.UnitY),
            //    Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f),
            //    World = Matrix.Identity
            //};

            //// Creates vertices for the cube
            //vertices = Buffer.Vertex.New(
            //    GraphicsDevice,
            //    new[]
            //        {
            //            new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.Orange), // Back
            //            new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.Orange),
            //            new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.Orange),
            //            new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.Orange),
            //            new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.Orange),
            //            new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.Orange),

            //            new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Orange), // Front
            //            new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.Orange),
            //            new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.Orange),
            //            new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Orange),
            //            new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.Orange),
            //            new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.Orange),

            //            new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.OrangeRed), // Top
            //            new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.OrangeRed),
            //            new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.OrangeRed),
            //            new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.OrangeRed),
            //            new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.OrangeRed),
            //            new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.OrangeRed),

            //            new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.OrangeRed), // Bottom
            //            new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.OrangeRed),
            //            new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.OrangeRed),
            //            new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.OrangeRed),
            //            new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.OrangeRed),
            //            new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.OrangeRed),

            //            new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.DarkOrange), // Left
            //            new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.DarkOrange),
            //            new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.DarkOrange),
            //            new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.DarkOrange),
            //            new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.DarkOrange),
            //            new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.DarkOrange),

            //            new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.DarkOrange), // Right
            //            new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.DarkOrange),
            //            new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.DarkOrange),
            //            new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.DarkOrange),
            //            new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.DarkOrange),
            //            new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.DarkOrange),
            //        });
            //// Create an input layout from the vertices
            //inputLayout = VertexInputLayout.FromBuffer(0, vertices);
        }

        public void Draw(float elapsedTime, Matrix view, Matrix projection, Matrix world, DeviceContext context)
        {
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.Apply(context);
            context.InputAssembler.SetVertexBuffers(0,
                new VertexBufferBinding(vertices, VertexPositionNormalTexture.Size, 0));
            context.InputAssembler.SetIndexBuffer(indices, Format.R32_UInt, 0);
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            context.DrawIndexed(indexCount, 0, 0);
        }
    }
}
