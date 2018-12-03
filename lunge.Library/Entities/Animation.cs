using Microsoft.Xna.Framework;

namespace lunge.Library.Entities
{
    public class Animation
    {
        public int FrameCount { get; }
        public float FramesPerSecond { get; }

        public int CurrentFrame { get; set; }
        public float TotalElapsed { get; private set; }
        public bool IsPaused { get; private set; }

        public Animation(int frameCount, float framesPerSec = 30.0f, int curFrame = 0, bool paused = false)
        {
            FrameCount = frameCount;
            FramesPerSecond = 1.0f / framesPerSec;
            CurrentFrame = curFrame;
            IsPaused = paused;
            TotalElapsed = 0;
        }

        /// <summary>
        /// Update current frame to change current frame
        /// </summary>
        /// <param name="gameTime">GameTime class</param>
        public void UpdateFrame(GameTime gameTime)
        {
            if (!IsPaused)
            {
                TotalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TotalElapsed > FramesPerSecond)
                {
                    CurrentFrame++;
                    CurrentFrame %= FrameCount;
                    TotalElapsed -= FramesPerSecond;
                }
            }
        }

        public void Reset()
        {
            CurrentFrame = 0;
            TotalElapsed = 0.0f;
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Play()
        {
            IsPaused = false;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Toggle()
        {
            IsPaused = !IsPaused;
        }
    }
}