using System.ComponentModel;
using FMOD;
using lunge.Library.Particles.Modifiers;
using lunge.Library.Particles.Profiles;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Textures;

namespace lunge.Library.Particles
{
    public unsafe class ParticleEmitter : IDisposable
    {
        private readonly FastRandom _random = new FastRandom(Math.Abs(Guid.NewGuid().GetHashCode()));
        private float _totalSeconds;

#pragma warning disable CS8618
        internal ParticleEmitter()
#pragma warning restore CS8618
        {
            // Used for JSON deserialization
        }

        public ParticleEmitter(string? name, Sprite sprite, int capacity, TimeSpan lifeSpan, Profile profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            _lifeSpanSeconds = (float)lifeSpan.TotalSeconds;

            Name = name;
            Sprite = sprite;
            Buffer = new ParticleBuffer(capacity);
            Offset = Vector2.Zero;
            Profile = profile;
            Modifiers = new List<Modifier>();
            ModifierExecutionStrategy = ParticleModifierExecutionStrategy.Serial;
            Parameters = new ParticleReleaseParameters();
        }

        public ParticleEmitter(Sprite sprite, int capacity, TimeSpan lifeSpan, Profile profile)
            : this(null, sprite, capacity, lifeSpan, profile)
        {
        }

        public void Dispose()
        {
            Buffer.Dispose();
            GC.SuppressFinalize(this);
        }
        
        ~ParticleEmitter()
        {
            Dispose();
        }

        public string? Name { get; set; }
        public int ActiveParticles => Buffer.Count;
        public Vector2 Offset { get; set; }
        public List<Modifier> Modifiers { get; internal set; }
        public Profile Profile { get; set; }
        public float LayerDepth { get; set; }
        public ParticleReleaseParameters Parameters { get; set; }
        public Sprite Sprite { get; set; }
        public RectangleF Bounds = RectangleF.Empty;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ParticleModifierExecutionStrategy ModifierExecutionStrategy { get; set; }

        internal ParticleBuffer Buffer;

        public int Capacity
        {
            get { return Buffer.Size; }
            set
            {
                var oldBuffer = Buffer;
                oldBuffer.Dispose();
                Buffer = new ParticleBuffer(value);
            }
        }

        private float _lifeSpanSeconds;
        public TimeSpan LifeSpan
        {
            get { return TimeSpan.FromSeconds(_lifeSpanSeconds); }
            set { _lifeSpanSeconds = (float) value.TotalSeconds; }
        }

        private float _nextAutoTrigger;

        private bool _autoTrigger = true;
        public bool AutoTrigger
        {
            get { return _autoTrigger; }
            set
            {
                _autoTrigger = value;
                _nextAutoTrigger = 0;
            }
        }

        private float _autoTriggerFrequency;
        public float AutoTriggerFrequency
        {
            get { return _autoTriggerFrequency; }
            set
            {
                _autoTriggerFrequency = value;
                _nextAutoTrigger = 0;
            }
        }

        private void ReclaimExpiredParticles()
        {
            var iterator = Buffer.Iterator;
            var expired = 0;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();

                if (_totalSeconds - particle->Inception < _lifeSpanSeconds)
                    break;

                expired++;
            }

            if (expired != 0)
                Buffer.Reclaim(expired);
        }

        //private Vector2? _oldPosition;

        public RectangleF? Update(float elapsedSeconds, Vector2 position = default(Vector2))
        {
            
            _totalSeconds += elapsedSeconds;
            /*
            if (_oldPosition == null)
                _oldPosition = position;

            var posDelta = position - _oldPosition.Value;
            if (posDelta != Vector2.Zero)
            {
                _oldPosition = null;
            }
            */

            if (_autoTrigger)
            {
                _nextAutoTrigger -= elapsedSeconds;

                if (_nextAutoTrigger <= 0)
                {
                    Trigger(position, this.LayerDepth);
                    _nextAutoTrigger = _autoTriggerFrequency;
                }
            }

            if (Buffer.Count == 0)
                return null;

            ReclaimExpiredParticles();

            var iterator = Buffer.Iterator;

            var min = new Vector2(float.MaxValue, float.MaxValue);
            var max = new Vector2(float.MinValue, float.MinValue);
            var maxParticleSize = new Vector2(float.MinValue, float.MinValue);

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Age = (_totalSeconds - particle->Inception) / _lifeSpanSeconds;
                particle->Position = particle->Position + particle->Velocity * elapsedSeconds; //+ posDelta;

                var pos = particle->Position;
                
                Vector2.Min(ref min, ref pos, out min);
                Vector2.Max(ref max, ref pos, out max);
                Vector2.Max(ref maxParticleSize, ref particle->Scale, out maxParticleSize);
            }

            Bounds.Location = min;
            Bounds.Width = max.X - min.X;
            Bounds.Height = max.Y - min.Y;

            maxParticleSize /= new Vector2(Sprite.SourceRect.Width, Sprite.SourceRect.Height);
            Bounds.Inflate(Sprite.SourceRect.Width * maxParticleSize.X, Sprite.SourceRect.Height * maxParticleSize.Y);

            ModifierExecutionStrategy.ExecuteModifiers(Modifiers, elapsedSeconds, iterator);
            return Bounds;
        }

        public void Trigger(Vector2 position, float layerDepth = 0)
        {
            var numToRelease = _random.Next(Parameters.Quantity);
            Release(position + Offset, numToRelease, layerDepth);
        }

        public void Trigger(LineSegment line, float layerDepth = 0)
        {
            var numToRelease = _random.Next(Parameters.Quantity);
            var lineVector = line.ToVector();

            for (var i = 0; i < numToRelease; i++)
            {
                var offset = lineVector * _random.NextSingle();
                Release(line.Origin + offset, 1, layerDepth);
            }
        }

        private void Release(Vector2 position, int numToRelease, float layerDepth)
        {
            var iterator = Buffer.Release(numToRelease);

            while (iterator.HasNext)
            {
                var particle = iterator.Next();

                Vector2 heading;
                Profile.GetOffsetAndHeading(out particle->Position, out heading);

                particle->Age = 0f;
                particle->Inception = _totalSeconds;
                particle->Position += position;
                particle->TriggerPos = position;

                var speed = _random.NextSingle(Parameters.Speed);

                particle->Velocity = heading * speed;

                _random.NextColor(out particle->Color, Parameters.Color);

                particle->Opacity = _random.NextSingle(Parameters.Opacity);
                
                if(Parameters.MaintainAspectRatioOnScale)
                {
                    var scale = _random.NextSingle(Parameters.Scale);
                    particle->Scale = new Vector2(scale, scale);
                }
                else
                {
                    particle->Scale = new Vector2(_random.NextSingle(Parameters.ScaleX), _random.NextSingle(Parameters.ScaleY));
                }
                
                particle->Rotation = _random.NextSingle(Parameters.Rotation);
                particle->Mass = _random.NextSingle(Parameters.Mass);
                particle->LayerDepth = layerDepth;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
