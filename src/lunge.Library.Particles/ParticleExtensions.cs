using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace lunge.Library.Particles
{
    public static class ParticleExtensions
    {
        public static void Draw(this Batcher batcher, ParticleEffect effect)
        {
            for (var i = 0; i < effect.Emitters.Count; i++)
                UnsafeDraw(batcher, effect.Emitters[i]);
        }

        public static void Draw(this Batcher batcher, ParticleEmitter emitter)
        {
            UnsafeDraw(batcher, emitter);
        }

        private static unsafe void UnsafeDraw(Batcher spriteBatch, ParticleEmitter emitter)
        {
            if (emitter.Sprite == null)
                return;

            var textureRegion = emitter.Sprite;
            var origin = new Vector2(textureRegion.Texture2D.Width / 2f, textureRegion.Texture2D.Height / 2f);
            var iterator = emitter.Buffer.Iterator;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var color = particle->Color.ToRgb();

                if (spriteBatch.GraphicsDevice.BlendState == BlendState.AlphaBlend)
                    color *= particle->Opacity;
                else
                    color.A = (byte)(particle->Opacity * 255);

                var position = new Vector2(particle->Position.X, particle->Position.Y);
                var scale = particle->Scale;
                var particleColor = color * particle->Opacity;
                var rotation = particle->Rotation;
                var layerDepth = particle->LayerDepth;

                spriteBatch.Draw(textureRegion, position, particleColor, rotation, origin, scale, SpriteEffects.None, layerDepth);
            }
        }
    }
}