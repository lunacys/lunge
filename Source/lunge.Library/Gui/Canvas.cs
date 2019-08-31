﻿using System;
using System.Collections.Generic;
using System.Text;
using lunge.Library.Entities.Systems;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui
{
    /// <summary>
    /// Canvas is the main class for using GUI components, it has no parent and contains
    /// all the IControl elements. Canvas cannot have another canvas as a child.
    /// </summary>
    public class Canvas : DrawableGameComponent, IControl
    {
        // TODO: Add transitions for controls
        public Point Position { get; set; }
        public Size2 Size { get; set; }
        public Canvas UsedCanvas
        {
            get => this;
            set => throw new InvalidOperationException();
        }

        private readonly SpriteBatch _spriteBatch;

        private readonly List<IControl> _controlList;
        private readonly Dictionary<string, IControl> _controls;
        private readonly List<IControl> _controlsToAdd;
        private readonly List<IControl> _controlsToRemove;

        private bool _isUpdating;

        protected InputHandler InputHandler { get; }

        
        public event EventHandler MouseHover;
        public event EventHandler MouseIn;
        public event EventHandler MouseOut;

        public Canvas(Game game, string name, Point position, Size2 size)
            : base(game)
        {
            Position = position;
            Size = size;
            Name = name;

            _controls = new Dictionary<string, IControl>();
            _controlList = new List<IControl>();
            _controlsToAdd = new List<IControl>();
            _controlsToRemove = new List<IControl>();

            ChildControls = new ControlList();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            InputHandler = new InputHandler(game);
        }

        public void Close()
        {
            foreach (var childControl in ChildControls)
            {
                childControl.Close();
            }
        }

        public void AddControl(IControl control)
        {
            if (_isUpdating)
                _controlsToAdd.Add(control);
            else 
                AddControlInternal(control);
        }

        public void RemoveControl(IControl control)
        {
            if (!_isUpdating)
                RemoveControlInternal(control);
            else
                _controlsToRemove.Add(control);
        }

        public void RemoveControl(string name)
        {
            var control = _controls[name];

            if (!_isUpdating)
                RemoveControlInternal(control);
            else
                _controlsToRemove.Add(control);
        }

        private void RemoveControlInternal(IControl control)
        {
            _controlList.Remove(control);
            _controls.Remove(control.Name);
        }

        private void AddControlInternal(IControl control)
        {
            control.UsedCanvas = UsedCanvas;

            if (control.ParentControl == null)
            {
                control.ParentControl = this;
                ChildControls.Add(control);
            }
            else
            {
                control.ParentControl.ChildControls.Add(control);
            }

            _controlList.Add(control);

            _controlList.Sort((c1, c2) =>
            {
                if (c1.DrawDepth < c2.DrawDepth)
                    return 1;
                if (c1.DrawDepth > c2.DrawDepth)
                    return -1;
                return 0;
            });

            _controls[control.Name] = control;
            control.Initialize(this);
        }

        public override void Update(GameTime gameTime)
        {
            InputHandler.Update(gameTime);

            _isUpdating = true;

            foreach (var control in _controlList)
            {
                control.Update(gameTime, InputHandler);
            }

            _isUpdating = false;

            foreach (var control in _controlsToAdd)
            {
                AddControlInternal(control);
            }

            _controlsToAdd.Clear();

            foreach (var control in _controlsToRemove)
            {
                RemoveControlInternal(control);
            }

            _controlsToRemove.Clear();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            foreach (var control in _controlList)
            {
                control.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public string Name { get; }
        public float DrawDepth { get; set; } = 1.0f;

        public IControl ParentControl
        {
            get => throw new InvalidOperationException("Canvas cannot have a parent control");
            set => throw new InvalidOperationException("Canvas cannot have a parent control");
        }

        public ControlList ChildControls { get; set; }

        public virtual void Initialize(Canvas canvas) { }

        public virtual void Update(GameTime gameTime, InputHandler inputHandler) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public RectangleF GetBounds()
        {
            return new RectangleF(Position, Size);
        }

    }
}
