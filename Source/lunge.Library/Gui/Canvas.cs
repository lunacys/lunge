using System;
using System.Collections.Generic;
using System.Text;
using lunge.Library.Entities.Systems;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Gui
{
    /// <summary>
    /// Canvas is the main class for using GUI components, it has no parent and contains
    /// all the IControl elements. Canvas cannot have another canvas as a child.
    /// </summary>
    public class Canvas : DrawableGameComponent
    {
        // TODO: Make controls to be stored with its name (e.g. change List<> to Dictionary<>)
        public int Width { get; set; }
        public int Height { get; set; }

        private SpriteBatch _spriteBatch;

        private readonly List<IControl> _controlList;
        private readonly Dictionary<string, IControl> _controls;
        private readonly List<IControl> _controlsToAdd;

        private bool _isUpdating;

        protected InputHandler InputHandler { get; }

        public Canvas(Game game, int width, int height)
            : base(game)
        {
            Width = width;
            Height = height;

            _controls = new Dictionary<string, IControl>();
            _controlList = new List<IControl>();
            _controlsToAdd = new List<IControl>();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            InputHandler = new InputHandler(game);
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
            // TODO: Add destroying control by name
            _controlList.Remove(control);
            _controls.Remove(control.Name);
        }

        public void RemoveControl(string name)
        {
            _controlList.Remove(_controls[name]);
            _controls.Remove(name);
        }

        private void AddControlInternal(IControl control)
        {
            _controlList.Add(control);
            _controls[control.Name] = control;
            control.Initialize(this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            

            base.LoadContent();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var control in _controlList)
                {
                    control.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
