using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace AssimpView
{
    public static class TextureHelper
    {
        public static Texture2D FromFile(Device device, string path)
        {
            using (Stream stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite | FileShare.Delete
                ))
            {
                var decoder = BitmapDecoder.Create(
                    stream,
                    BitmapCreateOptions.None,
                    BitmapCacheOption.Default
                    );
                var bmp = new WriteableBitmap(decoder.Frames[0]);
                bmp.Freeze();
                return FromBitmap(device, bmp);
            }
        }

        public static Texture2D FromBitmap(Device device, BitmapSource bitmapSource)
        {
            // Allocate DataStream to receive the WIC image pixels
            int stride = bitmapSource.PixelWidth * 4;
            using (var buffer = new DataStream(bitmapSource.PixelHeight * stride, true, true))
            {
                // Copy the content of the WIC to the buffer
                bitmapSource.CopyPixels(new Int32Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight), buffer.DataPointer, (int)buffer.Length, stride);
                return new Texture2D(device, new Texture2DDescription()
                {
                    Width = bitmapSource.PixelWidth,
                    Height = bitmapSource.PixelHeight,
                    ArraySize = 1,
                    BindFlags = BindFlags.ShaderResource,
                    Usage = ResourceUsage.Immutable,
                    CpuAccessFlags = CpuAccessFlags.None,
                    Format = Format.R8G8B8A8_UNorm,
                    MipLevels = 1,
                    OptionFlags = ResourceOptionFlags.None,
                    SampleDescription = new SampleDescription(1, 0),
                }, new DataRectangle(buffer.DataPointer, stride));
            }
        }
    }
}
