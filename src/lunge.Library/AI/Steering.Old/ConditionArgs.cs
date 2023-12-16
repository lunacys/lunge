namespace lunge.Library.AI.Steering.Old
{
    public class ConditionArgs
    {
        public ISteeringEntity Entity { get; }
        public ISteeringTarget Target { get; }

        public ConditionArgs(ISteeringEntity entity, ISteeringTarget target)
        {
            Entity = entity;
            Target = target;
        }
    }
}