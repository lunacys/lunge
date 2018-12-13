using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Graphics
{
    public struct SpriteBatchSettings
    {
        public SpriteSortMode SpriteSortMode { get; set; }
        public BlendState BlendState { get; set; }
        public SamplerState SamplerState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public RasterizerState RasterizerState { get; set; }
        public Effect Effect { get; set; }
        public Matrix? TransformMatrix { get; set; }

        public SpriteBatchSettings(
            SpriteSortMode spriteSortMode = SpriteSortMode.Deferred,
            BlendState blendState = null, 
            SamplerState samplerState = null, 
            DepthStencilState depthStencilState = null,
            RasterizerState rasterizerState = null, 
            Effect effect = null,
            Matrix? transformMatrix = null)
        {
            SpriteSortMode = spriteSortMode;
            BlendState = blendState;
            SamplerState = samplerState;
            DepthStencilState = depthStencilState;
            RasterizerState = rasterizerState;
            Effect = effect;
            TransformMatrix = transformMatrix;
        }

        public static SpriteBatchSettings GetStandard()
        {
            return new SpriteBatchSettings();
        }

        public static SpriteBatchSettings GetMatrix(Matrix transformMatrix)
        {
            return new SpriteBatchSettings(transformMatrix: transformMatrix);
        }
    }
}