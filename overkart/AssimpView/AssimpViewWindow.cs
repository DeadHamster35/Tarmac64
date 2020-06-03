// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Device = SharpDX.Direct3D11.Device;

namespace AssimpView
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using System.Linq;

    /// <summary>
    /// Simple MiniCube application using SharpDX.Toolkit.
    /// The purpose of this application is to show a rotating cube using <see cref="BasicEffect"/>.
    /// </summary>
    public class AssimpViewWindow : IDisposable
    {
        private Device device;
        private SwapChain swapChain;
        private RenderForm form;
        private List<FbxMesh> meshes;
        bool userResized = true;
        Texture2D backBuffer = null;
        RenderTargetView renderView = null;
        Texture2D depthBuffer = null;
        DepthStencilView depthView = null;
        private AssimpSharp.Scene scene;
        private string file;
        Stopwatch clock;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssimpViewWindow" /> class.
        /// </summary>
        public AssimpViewWindow(string file)
        {
            this.file = file;
        }

        public void Initialize(RenderForm form)
        {
            this.form = form;
            LoadDevice();
            LoadContent();
        }

        protected void LoadDevice()
        {
            // SwapChain description
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Used for debugging dispose object references
            // Configuration.EnableObjectTracking = true;

            // Disable throws on shader compilation errors
            //Configuration.ThrowOnShaderCompileError = false;

            // Create Device and SwapChain
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.Debug, desc, out device, out swapChain);

            // Ignore all windows events
            var factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);

            // Setup handler on resize form
            form.UserResized += (sender, args) => userResized = true;

            // Setup full screen mode change F5 (Full) F4 (Window)
            form.KeyUp += (sender, args) =>
            {
                if (args.KeyCode == Keys.F5)
                    swapChain.SetFullscreenState(true, null);
                else if (args.KeyCode == Keys.F4)
                    swapChain.SetFullscreenState(false, null);
                else if (args.KeyCode == Keys.Escape)
                    form.Close();
            };

            // Use clock
            clock = new Stopwatch();
            clock.Start();
        }

        protected void LoadContent()
        {
            meshes = FbxMeshLoader.Load(file, device, out scene);
        }

        public void Update()
        {
        }

        public void Render()
        {
            var context = device.ImmediateContext;
            var time = clock.ElapsedMilliseconds / 1000.0f;

            // If Form resized
            if (userResized)
            {
                // Dispose all previous allocated resources
                Utilities.Dispose(ref backBuffer);
                Utilities.Dispose(ref renderView);
                Utilities.Dispose(ref depthBuffer);
                Utilities.Dispose(ref depthView);

                // SwapChain description
                var desc = new SwapChainDescription()
                {
                    BufferCount = 1,
                    ModeDescription =
                        new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                                            new Rational(60, 1), Format.R8G8B8A8_UNorm),
                    IsWindowed = true,
                    OutputHandle = form.Handle,
                    SampleDescription = new SampleDescription(1, 0),
                    SwapEffect = SwapEffect.Discard,
                    Usage = Usage.RenderTargetOutput
                };

                // Resize the backbuffer
                swapChain.ResizeBuffers(desc.BufferCount, form.ClientSize.Width, form.ClientSize.Height, Format.Unknown, SwapChainFlags.None);

                // Get the backbuffer from the swapchain
                backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);

                // Renderview on the backbuffer
                renderView = new RenderTargetView(device, backBuffer);

                // Create the depth buffer
                depthBuffer = new Texture2D(device, new Texture2DDescription()
                {
                    Format = Format.D32_Float_S8X24_UInt,
                    ArraySize = 1,
                    MipLevels = 1,
                    Width = form.ClientSize.Width,
                    Height = form.ClientSize.Height,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Default,
                    BindFlags = BindFlags.DepthStencil,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None
                });

                // Create the depth buffer view
                depthView = new DepthStencilView(device, depthBuffer);

                // Setup targets and viewport for rendering
                context.Rasterizer.SetViewport(new Viewport(0, 0, form.ClientSize.Width, form.ClientSize.Height, 0.0f, 1.0f));
                context.OutputMerger.SetTargets(depthView, renderView);

                // Setup new projection matrix with correct aspect ratio
                //proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, form.ClientSize.Width / (float)form.ClientSize.Height, 0.1f, 100.0f);

                // We are done resizing
                userResized = false;
            }

            // Clear views
            context.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
            context.ClearRenderTargetView(renderView, Color.CornflowerBlue);

            //// Draw
            var view = Matrix.LookAtRH(new Vector3(0, 0, 500), new Vector3(0, 0, 0), Vector3.UnitY);
            var projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, form.ClientSize.Width / (float)form.ClientSize.Height, 0.1f, 1000.0f);
            var world = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);
            DrawSceneNode(scene.RootNode, view, projection, world, 0);

            swapChain.Present(1, PresentFlags.None);
        }

        void DrawSceneNode(AssimpSharp.Node node, Matrix view, Matrix projection, Matrix transform, float elapsedTime)
        {
            var context = device.ImmediateContext;
            transform = node.Transformation * transform;
            foreach(var meshId in node.Meshes ?? Enumerable.Empty<int>())
            {
                meshes[meshId].Draw(elapsedTime, view, projection, transform,context);
            }
            foreach(var child in node.Children ?? Enumerable.Empty<AssimpSharp.Node>())
            {
                DrawSceneNode(child, view, projection, transform, elapsedTime);
            }
        }

        public void Dispose()
        {
            Utilities.Dispose(ref renderView);
            Utilities.Dispose(ref backBuffer);
            Utilities.Dispose(ref depthView);
            Utilities.Dispose(ref renderView);
            Utilities.Dispose(ref swapChain);
            Utilities.Dispose(ref device);
        }
    }
}