using System;
using System.Collections.Generic;
using lunge.Library.InputMgr;
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
        private Vector2 _oldPosition;
        private Player _player;

        public MoveCommand(Player player, PlayerDirection direction, float speed)
        {
            _player = player;
            Direction = direction;
            Speed = speed;
        }

        public void Execute(Player entity)
        {
            _oldPosition = entity.Position;

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

        public void Undo()
        {
            _player.Position = _oldPosition;
        }
    }

    public class Player : IInputHandleable
    {
        public Texture2D Texture { get; }
        public Vector2 Position;
        public InputHandler<Keys, Player> _handler;
        public float MovementSpeed { get; set; }

        private MoveCommand _moveUp;
        private MoveCommand _moveRight;
        private MoveCommand _moveDown;
        private MoveCommand _moveLeft;

        public Player(Texture2D texture)
        {
            Texture = texture;
            MovementSpeed = 4.0f;
            _handler = new InputHandler<Keys, Player>();

            _moveUp = new MoveCommand(this, PlayerDirection.Up, MovementSpeed);
            _moveRight = new MoveCommand(this, PlayerDirection.Right, MovementSpeed);
            _moveDown = new MoveCommand(this, PlayerDirection.Down, MovementSpeed);
            _moveLeft = new MoveCommand(this, PlayerDirection.Left, MovementSpeed);

            _handler.Register(
                new InputEntry<Keys>(Keys.Space, InputManager.WasKeyPressed),
                new JumpCommand()
            );
            _handler.Register(
                Keys.W, InputManager.IsKeyDown,
                _moveUp
            );
            _handler.Register(
                Keys.D, InputManager.IsKeyDown,
                _moveRight
            );
            _handler.Register(
                Keys.S, InputManager.IsKeyDown,
                _moveDown
            );
            _handler.Register(
                Keys.A, InputManager.IsKeyDown,
                _moveLeft
            );
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.WasKeyPressed(Keys.R))
            {
                _moveUp.Undo();
                _moveRight.Undo();
                _moveDown.Undo();
                _moveLeft.Undo();
            }

            _handler.Handle(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}