using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Entities
{
    public class Sprite
    {
        public Texture2D Image { get; private set; }
        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float Depth { get; set; }
        
        public bool IsAnimated { get; }

        private readonly Animation _animation;
        private readonly Texture2D[] _frameCache;

        public int Width => Image.Width;
        public int Height => Image.Height;

        public int CurrentFrame
        {
            get
            {
                if (_animation != null)
                    return _animation.CurrentFrame;
                return 0;
            }
            set
            {
                if (_animation != null)
                    _animation.CurrentFrame = value;
            }
        }

        public Sprite(Texture2D image)
        {
            IsAnimated = false;
            Image = image;
        }

        public Sprite(Texture2D animationSheet, int frameCount, Point frameSize, float framesPerSec = 30.0f, int curFrame = 0)
        {
            IsAnimated = true;
            _animation = new Animation(frameCount, framesPerSec, curFrame);
            _frameCache = new Texture2D[frameCount];

            for (int i = 0; i < frameCount; i++)
            {
                _frameCache[i] = animationSheet.Crop(frameSize.X * i, 0, frameSize.X, frameSize.Y);
            }

            Image = _frameCache[curFrame];
        }

        public void Update(GameTime gameTime)
        {
            if (IsAnimated)
            {
                _animation.UpdateFrame(gameTime);
                Image = _frameCache[_animation.CurrentFrame];
            }
        }

        public static explicit operator Texture2D(Sprite sprite)
        {
            return sprite.Image;
        }

        public static explicit operator Sprite(Texture2D texture2D)
        {
            return new Sprite(texture2D);
        }
    }
}