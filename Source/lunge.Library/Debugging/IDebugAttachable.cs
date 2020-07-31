using lunge.Library.Graphics;

namespace lunge.Library.Debugging
{
    public interface IDebugAttachable
    {
        DebugData Attach();
        void Visualize(ImGuiRenderer renderer);
    }
}