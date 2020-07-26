using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace AssimpView
{
    public class BasicEffect : IDisposable
    {
        private VertexShader vertexShader;
        private PixelShader pixelShader;
        private InputLayout layout;
        private Buffer constantBuffer;

        public Matrix View = Matrix.Identity;
        public Matrix Projection = Matrix.Identity;
        public Matrix World = Matrix.Identity;

        public BasicEffect(Device device)
        {
            // Compile Vertex and Pixel shaders
            var vertexShaderByteCode = ShaderBytecode.CompileFromFile("BasicEffect.fx", "VS", "vs_4_0");
            vertexShader = new VertexShader(device, vertexShaderByteCode);

            var pixelShaderByteCode = ShaderBytecode.CompileFromFile("BasicEffect.fx", "PS", "ps_4_0");
            pixelShader = new PixelShader(device, pixelShaderByteCode);

            var signature = ShaderSignature.GetInputSignature(vertexShaderByteCode);
            // Layout from VertexShader input signature
            layout = new InputLayout(device, signature, new[]
                    {
                        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                        new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                        new InputElement("TEXTURECOORD", 0, Format.R32G32_Float, 24, 0)
                    });

            // Create Constant Buffer
            constantBuffer = new Buffer(device, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);

            Utilities.Dispose(ref vertexShaderByteCode);
            Utilities.Dispose(ref pixelShaderByteCode);
        }

        public void Apply(DeviceContext context)
        {
            var worldViewProj = World * (View * Projection);
            worldViewProj.Transpose();
            context.UpdateSubresource(ref worldViewProj, constantBuffer);
            context.VertexShader.Set(vertexShader);
            context.VertexShader.SetConstantBuffer(0, constantBuffer);
            context.PixelShader.Set(pixelShader);
            context.PixelShader.SetConstantBuffer(0, constantBuffer);
            context.InputAssembler.InputLayout = layout;
        }

        public void Dispose()
        {
            Utilities.Dispose(ref vertexShader);
            Utilities.Dispose(ref pixelShader);
            Utilities.Dispose(ref layout);
            Utilities.Dispose(ref constantBuffer);
        }
    }
}
