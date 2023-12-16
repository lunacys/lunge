using System.Collections;
using Nez.Persistence;

namespace lunge.Library.Particles.Serialization
{
    public sealed class ParticleJsonConverter : JsonTypeConverter<ParticleEffect>
    {
        public override bool WantsExclusiveWrite => true;

        public override void WriteJson(IJsonEncoder encoder, ParticleEffect value)
        {
            encoder.EncodeKeyValuePair("Name", value.Name ?? "");
            encoder.EncodeKeyValuePair("Emitters", value.Emitters);
        }

        public override void OnFoundCustomData(ParticleEffect instance, string key, object value)
        {
            /*switch (key)
            {
                case "Name": instance.Name = value.ToString() ?? "";
                    break;
                /*case "Emitters":
                    var emittersRaw = (IList)value;
                    var emitters = new List<ParticleEmitter>();
                    foreach (var emitter in emittersRaw)
                    {
                        var e = (ParticleEmitter)emitter;
                        emitters.Add(e);
                    }

                    instance.Emitters = emitters;
                    break;#1#
            }*/
        }
    }

    public class ParticleJsonFactory : JsonObjectFactory<ParticleEffect>
    {
        public override void OnFoundCustomData(object instance, string key, object value)
        {
            base.OnFoundCustomData(instance, key, value);
        }

        public override ParticleEffect Create(Type objectType, IDictionary objectData)
        {
            //throw new NotImplementedException();
            return new ParticleEffect();
        }
    }
}
