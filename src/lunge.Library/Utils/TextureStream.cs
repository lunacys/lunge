using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Utils
{
    public static class TextureStream
    {
        public static GraphicsDevice GraphicsDevice { get; set; }

        /// <summary>
        /// Set the Alpha channel and make the texture transparent.
        /// </summary>
        /// <param name="texture">Sprite which will make the alpha value transparent.</param>
        private static void PremultiplyAlpha(Texture2D texture)
        {
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);
            for (int i = 0; i < pixels.Length; i++)
            {
                Color p = pixels[i];
                pixels[i] = new Color(p.R, p.G, p.B) * (p.A / 255f);
            }
            texture.SetData(pixels);
        }

        /// <summary>
        /// Load non-xnb textures. (e.g. PNG, JPG, BMP etc.) and set an Alpha channel.
        /// </summary>
        /// <param name="texturePath">Path to the texture without Content directory.</param>
        public static Texture2D Load(string texturePath)
        {
            if (GraphicsDevice == null) 
                throw new ArgumentNullException(nameof(GraphicsDevice), "Current GraphicsDevice is null!");

            Texture2D texture;
            if (File.Exists(texturePath))
            {
                using (var textureFileStream = new FileStream(texturePath, FileMode.Open))
                {
                    texture = Texture2D.FromStream(GraphicsDevice, textureFileStream);
                    PremultiplyAlpha(texture);
                }
            }
            else
            {
                throw new FileNotFoundException($"File {texturePath} not found!");
            }

            return texture;
        }

        public static List<Texture2D> LoadToList(params string[] texturesPaths)
        {
            var list = new List<Texture2D>(texturesPaths.Length);
            list.AddRange(texturesPaths.Select(Load));
            return list;
        }

        public static Dictionary<string, Texture2D> LoadToDictionary(params string[] texturePaths)
        {
            var dict = new Dictionary<string, Texture2D>(texturePaths.Length);
            foreach (var tp in texturePaths)
                dict[tp] = Load(tp);
            return dict;
        }
    }
}