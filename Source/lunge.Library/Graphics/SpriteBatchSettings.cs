using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Graphics
{
    public class SpriteBatchSettings
    {
        public SpriteSortMode SpriteSortMode { get; set; }
        public BlendState BlendState { get; set; }
        public SamplerState SamplerState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public RasterizerState RasterizerState { get; set; }
        public Effect Effect { get; set; }

        public Matrix? TransformMatrix
        {
            get => _getTransformMatrix();
            set => _getTransformMatrix = () => value;
        }

        private Func<Matrix?> _getTransformMatrix;

        public SpriteBatchSettings(
            SpriteSortMode spriteSortMode = SpriteSortMode.Deferred,
            BlendState blendState = null, 
            SamplerState samplerState = null, 
            DepthStencilState depthStencilState = null,
            RasterizerState rasterizerState = null, 
            Effect effect = null,
            Func<Matrix?> getTransformMatrix = null)
        {
            SpriteSortMode = spriteSortMode;
            BlendState = blendState;
            SamplerState = samplerState;
            DepthStencilState = depthStencilState;
            RasterizerState = rasterizerState;
            Effect = effect;
            _getTransformMatrix = getTransformMatrix ?? (() => null);
        }

        public static SpriteBatchSettings GetStandard()
        {
            return new SpriteBatchSettings();
        }
    }
}