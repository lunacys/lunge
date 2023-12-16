using System.IO.Compression;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Textures;

namespace lunge.Library.Particles.Serialization;

public static class TextureBase64Converter
{
    public static Dictionary<string, string> ToBase64(Sprite texture)
    {
        using (var rawStream = new MemoryStream())
        {
            texture.Texture2D.SaveAsPng(rawStream, texture.Texture2D.Width, texture.Texture2D.Height);
            rawStream.Position = 0;

            using (var outStream = new MemoryStream())
            {
                using (var compressedStream = new GZipStream(outStream, CompressionLevel.Optimal))
                    rawStream.CopyTo(compressedStream);
                var bytes = outStream.ToArray();

                var attrs = new Dictionary<string, string>();
                attrs["name"] = "texture.png";
                attrs["data"] = Convert.ToBase64String(bytes);

                return attrs;
            }
        }
    }

    public static Sprite FromBase64(Dictionary<string, string> attrs)
    {
        var textureAttr = attrs["name"];
        var data = attrs["data"];

        using (var memoryStream = new MemoryStream(Convert.FromBase64String(data), false))
        {
            using (var stream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                using (var mem = new MemoryStream())
                {
                    stream.CopyTo(mem);

                    var bitmap = System.Drawing.Image.FromStream(mem) as System.Drawing.Bitmap;
                    var colors = new Color[bitmap.Width * bitmap.Height];

                    for (var x = 0; x < bitmap.Width; x++)
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            var drawColor = bitmap.GetPixel(x, y);
                            colors[x + y * bitmap.Width] = new Color(drawColor.R, drawColor.G, drawColor.B, drawColor.A);
                        }
                    }

                    var texture = new Texture2D(Core.GraphicsDevice, bitmap.Width, bitmap.Height);
                    texture.SetData(colors);
                    texture.Name = textureAttr;

                    return new Sprite(texture);
                }
            }
        }

        /*var buffer = Convert.FromBase64String(data);
        using var memStream = new MemoryStream(buffer, false);
        using var stream = new GZipStream(memStream, CompressionMode.Decompress);
        using var mem = new MemoryStream();
        stream.CopyTo(mem);

        var a = Texture2D.FromStream(Core.GraphicsDevice, stream);
        var image = System.Drawing.Image.FromStream(stream);
        var bitmap = image as System.Drawing.Bitmap;
        var colors = new Color[bitmap.Width * bitmap.Height];

        for (var x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                var drawColor = bitmap.GetPixel(x, y);
                colors[x + y * bitmap.Width] = new Color(drawColor.R, drawColor.G, drawColor.B, drawColor.A);
            }
        }

        var texture = new Texture2D(Core.GraphicsDevice, bitmap.Width, bitmap.Height);
        texture.SetData(colors);
        texture.Name = textureAttr;

        return new Sprite(texture);*/
    }
}