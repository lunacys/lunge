// using System;
// using lunge.Library.Entities;
// using Microsoft.Xna.Framework.Graphics;
// using MonoGame.Extended;
//
// namespace lunge.Library.Gui.Old.Controls
// {
//     [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
//     public class Image : ControlBase
//     {
//         public override Size2 Size
//         {
//             get
//             {
//                 var width = Sprite?.Width ?? 0;
//                 var height = Sprite?.Height ?? 0;
//
//                 return new Size2(width, height);
//             }
//             set => throw new NotImplementedException();
//         }
//         public Sprite Sprite { get; set; }
//
//         public Image(string name, Sprite sprite, IControl parentControl = null)
//             : base(name, parentControl)
//         {
//             Sprite = sprite;
//         }
//
//         public override void Draw(SpriteBatch spriteBatch)
//         {
//             spriteBatch.Draw(Sprite, Position);
//         }
//     }
// }