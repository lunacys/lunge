using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GuiTests
{
    public static class Assets
    {
        public static SpriteFont MainFont { get; private set; }
        public static Texture2D HeroTexture { get; private set; }
        public static Texture2D GuiControlAtlas { get; private set; }

        public static void Load(ContentManager content)
        {
            MainFont = content.Load<SpriteFont>(Path.Combine("Fonts", "MainFont"));

            HeroTexture = content.Load<Texture2D>(Path.Combine("Images", "HeroTexture"));
            GuiControlAtlas = content.Load<Texture2D>(Path.Combine("Images", "GuiControlAtlas"));
        }
    }
}