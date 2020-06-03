using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AssimpSharp.FBX;
using System.Text;
using System.Windows.Media.Effects;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using PixelShader = SharpDX.Direct3D11.PixelShader;

namespace AssimpView
{
    /// <summary>
    /// SharpDX MiniCube Direct3D 11 Sample
    /// </summary>
    static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Args: filepath");
                return;
            }
            else
            {
                var file = args[0];
                var form = new RenderForm("Assimp Viewer");
                form.Show();
                using (var app = new AssimpViewWindow(file))
                {
                    app.Initialize(form);

                    using (var loop = new RenderLoop(form))
                    {
                        while (loop.NextFrame())
                        {
                            app.Update();
                            app.Render();
                        }
                    }
                }
            }
        }
    }
}
