namespace lunge.Library.AI.Steering
{
    public class SteeringPhysicalParams
    {
        public float? Mass { get; set; }
        public float? MaxVelocity { get; set; }
        public float? MaxForce { get; set; }

        public static SteeringPhysicalParams Defaults()
        {
            return new SteeringPhysicalParams
            {
                Mass = 10f,
                MaxForce = 3.8f,
                MaxVelocity = 3.9f
            };
        }
    }
}