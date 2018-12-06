namespace lunge.Library.Entities.Systems
{
    public interface ISystem
    {
        ISystemManager SystemManager { get; set; }
        bool IsActive { get; set; }

        void Initialize(World world);
    }
}