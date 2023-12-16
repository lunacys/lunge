using System;
using Nez;

namespace lunge.Library.AI.Steering;

public static class EntityExtensions
{
    public static SteeringHost ToHost(this Entity entity)
    {
        var r = entity as SteeringHost;
        if (r == null)
            throw new Exception("The entity is not SteeringHost!");
        return r;
    }
}