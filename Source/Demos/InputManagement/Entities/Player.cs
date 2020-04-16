using System;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InputManagement.Entities
{
    public class JumpCommand : IInputCommand<Player>
    {
        public void Execute(Player entity)
        {
            Console.WriteLine("Jumping!");
        }
    }

    public enum PlayerDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    public class MoveCommand : IInputCommand<Player>
    {
        public PlayerDirection Direction { get; }
        public float Speed { get; set; }

        public MoveCommand(PlayerDirection direction, float speed)
        {
            Direction = direction;
            Speed = speed;
        }

        public void Execute(Player entity)
        {
            switch (Direction)
            {
                case PlayerDirection.Up:
                    entity.Position.Y -= Speed;
                    break;
                case PlayerDirection.Right:
                    entity.Position.X += Speed;
                    break;
                case PlayerDirection.Down:
                    entity.Position.Y += Speed;
                    break;
                case PlayerDirection.Left:
                    entity.Position.X -= Speed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class Player : IInputHandleable
    {
        public Texture2D Texture { get; }
        public Vector2 Position;
        public InputHandler<Keys, Player> _handler;
        public float MovementSpeed { get; set; }

        public Player(Texture2D texture)
        {
            Texture = texture;
            MovementSpeed = 4.0f;
            _handler = new InputHandler<Keys, Player>();

            _handler.Register(
                new InputEntry<Keys>(Keys.Space, InputManager.WasKeyPressed),
                new JumpCommand()
            );
            _handler.Register(
                new InputEntry<Keys>(Keys.W, InputManager.IsKeyDown),
                new MoveCommand(PlayerDirection.Up, MovementSpeed)
            );
            _handler.Register(
                new InputEntry<Keys>(Keys.D, InputManager.IsKeyDown),
                new MoveCommand(PlayerDirection.Right, MovementSpeed)
            );
            _handler.Register(
                new InputEntry<Keys>(Keys.S, InputManager.IsKeyDown),
                new MoveCommand(PlayerDirection.Down, MovementSpeed)
            );
            _handler.Register(
                new InputEntry<Keys>(Keys.A, InputManager.IsKeyDown),
                new MoveCommand(PlayerDirection.Left, MovementSpeed)
            );
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Check high CPU usage. Probably because of 'yield return'
            foreach (var cmd in _handler.Handle())
            {
                cmd.Execute(this);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}