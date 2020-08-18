using System.IO;
using lunge.Library;
using lunge.Library.Assets;

namespace AssetsLoading
{
    [Asset("Text", ".txt")]
    public class TextAssetLoader : IAssetLoader<string>
    {
        public string AssetType => "Text";
        public IGame Game { get; }

        public TextAssetLoader(IGame game)
        {
            Game = game;
        }
        
        public string Load(string assetName)
        {
            using (var sr = new StreamReader(assetName))
            {
                return sr.ReadToEnd();
            }
        }

        object IAssetLoader.Load(string assetName)
        {
            return Load(assetName);
        }
    }
}