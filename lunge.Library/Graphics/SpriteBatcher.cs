using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Graphics
{
    public class SpriteBatcher : DrawableGameComponent
    {
        public GraphicsDevice Graphics { get; }

        private Dictionary<SpriteBatchSettings, SpriteBatch> _drawCalls;

        public SpriteBatcher(Game game)
            : base(game)
        {
            Graphics = game.GraphicsDevice;

            _drawCalls = new Dictionary<SpriteBatchSettings, SpriteBatch>();
        }

        public SpriteBatch CreateSpriteBatch(SpriteBatchSettings settings)
        {
            if (_drawCalls.ContainsKey(settings))
                return _drawCalls[settings];

            var sb = new SpriteBatch(Graphics);
            _drawCalls[settings] = sb;

            return sb;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var drawCall in _drawCalls)
            {
                var sb = drawCall.Value;
                sb.Begin(drawCall.Key);

                sb.End();
            }
        }
    }
}