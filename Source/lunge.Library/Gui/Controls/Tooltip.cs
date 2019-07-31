using Microsoft.Xna.Framework;

namespace lunge.Library.Gui.Controls
{
    public class Tooltip : ControlBase
    {
        public string Text { get; set; }
        public Vector2 Position { get; set; }

        public Tooltip(string name)
            : base(name)
        {

        }
    }
}