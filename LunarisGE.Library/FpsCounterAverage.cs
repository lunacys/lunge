using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LunarisGE.Library
{
    public class FpsCounterAverage : FpsCounter
    {
        public FpsCounterAverage(Game game)
            : base(game)
        { }

        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public override float FramesPerSecond { get; protected set; }

        public const int MaximumSamples = 100;

        private readonly Queue<float> _sampleBuffer = new Queue<float>();

        public override void Draw(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            FramesPerSecond = 1.0f / deltaTime;

            _sampleBuffer.Enqueue(FramesPerSecond);

            if (_sampleBuffer.Count > MaximumSamples)
            {
                _sampleBuffer.Dequeue();
                FramesPerSecond = _sampleBuffer.Average(i => i);
            }
            else
            {
                FramesPerSecond = FramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
        }

        public override string ToString()
        {
            return $"Average FPS: {FramesPerSecond}";
        }
    }
}