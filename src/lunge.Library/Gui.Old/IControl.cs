using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace lunge.Library.Gui.Old
{
    [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
    public interface IControl : IUpdatable
    {
        string Name { get; }
        float DrawDepth { get; set; }
        IControl ParentControl { get; set; }
        ControlList ChildControls { get; set; }
        Canvas UsedCanvas { get; set; }
        Vector2 Position { get; set; }
        Vector2 Size { get; set; }

        event EventHandler MouseHover;
        event EventHandler MouseIn;
        event EventHandler MouseOut;
        
        void Close();

        void Initialize(IControl parent);
        void Render(Batcher batcher, Camera camera);

        RectangleF GetBounds();
    }
}