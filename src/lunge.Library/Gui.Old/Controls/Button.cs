// using System;
// using lunge.Library.InputMgr;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using MonoGame.Extended;
// using MonoGame.Extended.TextureAtlases;
//
// namespace lunge.Library.Gui.Old.Controls
// {
//     [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
//     public enum ButtonState
//     {
//         None,
//         Hovered,
//         Clicking,
//         Pressed
//     }
//
//     [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
//     public class Button : ControlBase
//     {
//         public string Text { get; set; }
//
//         public event EventHandler Clicked;
//
//         public RectangleF BoundingRect => new RectangleF(Position, Size);
//
//         public ButtonState State { get; private set; }
//
//         public SpriteFont Font { get; set; }
//         public TextureAtlas TextureAtlas { get; set; }
//
//         public Color DefaultTint { get; set; } = Color.White;
//         public Color HoveredTint { get; set; } = new Color(.85f, .85f, .85f, 1f);
//         public Color ClickingTint { get; set; } = new Color(.65f, .65f, .65f, 1f);
//         public Color PressedTint { get; set; } = new Color(.65f, .65f, .65f, 1f);
//
//         public Button(string name, Vector2 position, int width, int height, SpriteFont font) 
//             : base(name)
//         {
//             Position = position;
//
//             Size = new Size2(width, height);
//             State = ButtonState.None;
//             Font = font;
//         }
//
//         public override void Update(GameTime gameTime)
//         {
//             if (BoundingRect.Contains(InputManager.MousePosition))
//             {
//                 if (InputManager.IsMouseButtonDown(MouseButton.Left))
//                 {
//                     State = ButtonState.Clicking;
//                 }
//                 else if (InputManager.WasMouseButtonReleased(MouseButton.Left))
//                 {
//                     if (State == ButtonState.Clicking)
//                     {
//                         State = ButtonState.Pressed;
//
//                         Clicked?.Invoke(this, EventArgs.Empty);
//                     }
//                 }
//                 else
//                 {
//                     State = ButtonState.Hovered;
//                 }
//             }
//             else
//             {
//                 State = ButtonState.None;
//             }
//
//             base.Update(gameTime);
//         }
//
//         public override void Draw(SpriteBatch spriteBatch)
//         {
//             if (TextureAtlas == null)
//             {
//                 switch (State)
//                 {
//                     case ButtonState.None:
//                         spriteBatch.FillRectangle(BoundingRect, Color.Red);
//                         break;
//                     case ButtonState.Hovered:
//                         spriteBatch.FillRectangle(BoundingRect, Color.Green);
//                         break;
//                     case ButtonState.Pressed:
//                         spriteBatch.FillRectangle(BoundingRect, Color.Blue);
//                         break;
//                     case ButtonState.Clicking:
//                         spriteBatch.FillRectangle(BoundingRect, Color.Wheat);
//                         break;
//                     default:
//                         throw new ArgumentOutOfRangeException();
//                 }
//             }
//             else
//             {
//                 DrawAtlas(spriteBatch);
//             }
//
//             if (!string.IsNullOrEmpty(Text) && Font != null)
//             {
//                 spriteBatch.DrawString(Font, Text, BoundingRect.ToRectangle(), Color.Black, TextAlignment.Center);
//             }
//
//             base.Draw(spriteBatch);
//         }
//
//         public override RectangleF GetBounds()
//         {
//             return BoundingRect;
//         }
//
//         private void DrawAtlas(SpriteBatch spriteBatch)
//         {
//             Color tint;
//
//             switch (State)
//             {
//                 case ButtonState.None:
//                     tint = DefaultTint;
//                     break;
//                 case ButtonState.Hovered:
//                     tint = HoveredTint;
//                     break;
//                 case ButtonState.Clicking:
//                     tint = ClickingTint;
//                     break;
//                 case ButtonState.Pressed:
//                     tint = PressedTint;
//                     break;
//                 default:
//                     throw new ArgumentOutOfRangeException();
//             }
//
//             // TODO: Refactor & Optimize this shit
//             var center = TextureAtlas["Center"];
//             // Edges
//             var topLeft = TextureAtlas["TopLeft"];
//             var bottomLeft = TextureAtlas["BottomLeft"];
//             var topRight = TextureAtlas["TopRight"];
//             var bottomRight = TextureAtlas["BottomRight"];
//             //Sides
//             var top = TextureAtlas["Top"];
//             var bottom = TextureAtlas["Bottom"];
//             var left = TextureAtlas["Left"];
//             var right = TextureAtlas["Right"];
//             // Positions
//             var topLeftPosition = Position;
//             var bottomLeftPosition = new Vector2(Position.X, Position.Y + Size.Height - bottomLeft.Height);
//             var topRightPosition = new Vector2(Position.X + Size.Width - topRight.Width, Position.Y);
//             var bottomRightPosition = new Vector2(Position.X + Size.Width - bottomRight.Width,
//                 Position.Y + Size.Height - bottomRight.Height);
//
//             // Draw Center
//             {
//                 var rect = new Rectangle(
//                     x:      (int)(topLeftPosition.X + topLeft.Width),
//                     y:      (int)(topLeftPosition.Y + topLeft.Height),
//                     width:  (int)(topRightPosition.X - (topLeftPosition.X + topLeft.Width)), 
//                     height: (int)(bottomRightPosition.Y - (topLeftPosition.Y + topLeft.Height))
//                     );
//                 spriteBatch.Draw(center.Texture, rect, center.Bounds, tint);
//             }
//
//             // Draw Sides:
//             // Top
//             {
//                 var rect = new Rectangle(
//                     x:      (int)(topLeftPosition.X + topLeft.Width), 
//                     y:      (int)(topLeftPosition.Y),
//                     width:  (int)(topRightPosition.X - (topLeftPosition.X + topLeft.Width)), 
//                     height: top.Height
//                     );
//                 spriteBatch.Draw(top.Texture, rect, top.Bounds, tint);
//             }
//             // Bottom
//             {
//                 var rect = new Rectangle(
//                     x:      (int)(bottomLeftPosition.X + bottomLeft.Width), 
//                     y:      (int)(bottomLeftPosition.Y),
//                     width:  (int)(bottomRightPosition.X - (bottomLeftPosition.X + bottomLeft.Width)), 
//                     height: (int)top.Height
//                     );
//                 spriteBatch.Draw(bottom.Texture, rect, bottom.Bounds, tint);
//             }
//             // Left
//             {
//                 var rect = new Rectangle(
//                     x:      (int)(topLeftPosition.X),
//                     y:      (int)(topLeftPosition.Y + topLeft.Height),
//                     width:  (int)(left.Width),
//                     height: (int)(bottomLeftPosition.Y - (topLeftPosition.Y + topLeft.Height))
//                     );
//                 spriteBatch.Draw(left.Texture, rect, left.Bounds, tint);
//             }
//             // Right
//             {
//                 var rect = new Rectangle(
//                     x:      (int)(topRightPosition.X),
//                     y:      (int)(topRightPosition.Y + topRight.Height),
//                     width:  (int)(right.Width),
//                     height: (int)(bottomRightPosition.Y - (topRightPosition.Y + topRight.Height))
//                 );
//                 spriteBatch.Draw(right.Texture, rect, right.Bounds, tint);
//             }
//
//             // Draw Edges
//             spriteBatch.Draw(topLeft.Texture, topLeftPosition, topLeft.Bounds, tint);
//             spriteBatch.Draw(bottomLeft.Texture, bottomLeftPosition, bottomLeft.Bounds, tint);
//             spriteBatch.Draw(topRight.Texture, topRightPosition, topRight.Bounds, tint);
//             spriteBatch.Draw(bottomRight.Texture, bottomRightPosition, bottomRight.Bounds, tint);
//         }
//     }
// }