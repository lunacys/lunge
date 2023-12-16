using lunge.Library.Particles.Serialization;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.Particles
{
    public class ParticleEffect : RenderableComponent, IUpdatable, IDisposable
    {
        public override RectangleF Bounds => _bounds;

        public ParticleEffect()
            : this(null)
        {
            
        }

        public ParticleEffect(string? name = null)
        {
            Name = name;
            Emitters = new List<ParticleEmitter>();
        }

        public void Dispose()
        {
            foreach (var emitter in Emitters)
                emitter.Dispose();
        }

        public string? Name { get; set; }
        public List<ParticleEmitter> Emitters { get; set; }
        public int ActiveParticles => Emitters.Sum(t => t.ActiveParticles);

        public void FastForward(Vector2 position, float seconds, float triggerPeriod)
        {
            var time = 0f;
            while (time < seconds)
            {
                Update(triggerPeriod);
                Trigger(position);
                time += triggerPeriod;
            }
        }

        /*public static ParticleEffect FromFile(ITextureRegionService textureRegionService, string path)
        {
            using (var stream = TitleContainer.OpenStream(path))
            {
                return FromStream(textureRegionService, stream);
            }
        }

        public static ParticleEffect FromStream(ITextureRegionService textureRegionService, Stream stream)
        {
            var serializer = new ParticleJsonSerializer(textureRegionService);

            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<ParticleEffect>(jsonReader);
            }
        }*/

        public void Update()
        {
            Update(Time.DeltaTime);
        }

        public void Update(float dt)
        {
            for (var i = 0; i < Emitters.Count; i++)
            {
                var bounds = Emitters[i].Update(dt, Entity.Position);
                if (bounds.HasValue)
                    _bounds = bounds.Value;
            }
        }

        public void Trigger()
        {
            Trigger(Entity.Position);
        }

        public void Trigger(Vector2 position, float layerDepth = 0)
        {
            for (var i = 0; i < Emitters.Count; i++)
                Emitters[i].Trigger(position, layerDepth);
        }

        public void Trigger(LineSegment line, float layerDepth = 0)
        {
            for (var i = 0; i < Emitters.Count; i++)
                Emitters[i].Trigger(line, layerDepth);
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            batcher.Draw(this);
        }

        public override string ToString()
        {
            return Name;
        }

        public override void OnRemovedFromEntity()
        {
            Dispose();
        }
    }
}
