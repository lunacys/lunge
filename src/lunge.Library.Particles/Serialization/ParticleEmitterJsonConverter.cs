using System.Collections;
using System.Reflection;
using lunge.Library.Particles.Modifiers;
using lunge.Library.Particles.Profiles;
using Microsoft.Xna.Framework;
using Nez.Persistence;

namespace lunge.Library.Particles.Serialization;

public class ParticleEmitterJsonConverter : JsonTypeConverter<ParticleEmitter>
{
    public override bool WantsExclusiveWrite => true;
    
    public override void WriteJson(IJsonEncoder encoder, ParticleEmitter value)
    {
        encoder.EncodeKeyValuePair("Name", value.Name ?? "");
        encoder.EncodeKeyValuePair("Offset", value.Offset);
        encoder.EncodeKeyValuePair("Modifiers", value.Modifiers);
        encoder.EncodeKeyValuePair("Profile", value.Profile);
        encoder.EncodeKeyValuePair("LayerDepth", value.LayerDepth);
        encoder.EncodeKeyValuePair("Parameters", value.Parameters);
        encoder.EncodeKeyValuePair("TextureBase64", TextureBase64Converter.ToBase64(value.Sprite));
        encoder.EncodeKeyValuePair("ModifierExecutionStrategyType", value.ModifierExecutionStrategy.GetType().Name);
        encoder.EncodeKeyValuePair("Capacity", value.Capacity);
        encoder.EncodeKeyValuePair("LifeSpanSeconds", value.LifeSpan.TotalSeconds);

        encoder.EncodeKeyValuePair("AutoTrigger", value.AutoTrigger);
        encoder.EncodeKeyValuePair("AutoTriggerFrequency", value.AutoTriggerFrequency);
    }

    public override void OnFoundCustomData(ParticleEmitter instance, string key, object value)
    {
        switch (key)
        {
            case "Name": instance.Name = value.ToString(); break;
            case "Modifiers": instance.Modifiers = (List<Modifier>)value;
                break;
            //case "Offset": instance.Offset = (Vector2)value; break;
        }
    }
}

public class ParticleEmitterJsonFactory : JsonObjectFactory<ParticleEmitter>
{
    public override ParticleEmitter Create(Type objectType, IDictionary objectData)
    {
        var name = objectData["Name"].ToString();
        var offsetRaw = (Dictionary<string, object>)objectData["Offset"];
        var offset = new Vector2(Convert.ToSingle(offsetRaw["X"]), Convert.ToSingle(offsetRaw["Y"]));
        var layerDepth = Convert.ToSingle(objectData["LayerDepth"]);
        var autoTrigger = Convert.ToBoolean(objectData["AutoTrigger"]);
        var autoTriggerFrequency = Convert.ToSingle(objectData["AutoTriggerFrequency"]);
        var attrsRaw = (Dictionary<string, object>)objectData["TextureBase64"];
        var attrs = new Dictionary<string, string>();
        foreach (var o in attrsRaw)
        {
            attrs[o.Key] = o.Value.ToString();
        }
        var sprite = TextureBase64Converter.FromBase64(attrs);
        var capacity = Convert.ToInt32(objectData["Capacity"]);
        var timeSpan = TimeSpan.FromSeconds(Convert.ToDouble(objectData["LifeSpanSeconds"]));

        var profileRaw = (Dictionary<string, object>)objectData["Profile"];
        var profileType = profileRaw["@type"].ToString();
        var profileParams = profileRaw.Where(p => !p.Value.ToString().StartsWith("@") && p.Value.ToString() != "Name").ToList();




        var profile = Profile.Point();

        var modifierExecStrategyType = objectData["ModifierExecutionStrategyType"].ToString();
        var modifierExecStrategy = ParticleModifierExecutionStrategy.Parse(modifierExecStrategyType);

        var emitter = new ParticleEmitter(name, sprite, capacity, timeSpan, profile)
        {
            Offset = offset,
            LayerDepth = layerDepth,
            AutoTrigger = autoTrigger,
            AutoTriggerFrequency = autoTriggerFrequency,
            ModifierExecutionStrategy = modifierExecStrategy
        };

        return emitter;
    }
}