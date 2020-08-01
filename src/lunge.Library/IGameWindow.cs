using System.Drawing;

namespace lunge.Library
{
    public interface IGameWindow
    {
        Rectangle ClientBounds { get; }
        string Title { get; set; }
        bool AllowUserResizing { get; set; }
        bool IsFullscreen { get; set; }
        int CursorPosition { get; set; }
        
        void Resize(int width, int height);
        void Maximize();
        void Minimize();
        void Restore();
    }
}