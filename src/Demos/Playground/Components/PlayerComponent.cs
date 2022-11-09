using lunge.Library.Assets.Aseprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Playground.Scenes;

namespace Playground.Components;

public class PlayerComponent : Component, ITriggerListener, IUpdatable
{
    enum Animations
    {
        WalkUp,
        WalkDown,
        WalkRight,
        WalkLeft,
    }

    private SpriteAnimator _animator;
    private SubpixelVector2 _subpixelV2;
    private Mover _mover;
    private float _moveSpeed = 100f;
    private Vector2 _projectileVelocity = new Vector2(175);

    private VirtualButton _fireInput;
    private VirtualIntegerAxis _xAxisInput;
    private VirtualIntegerAxis _yAxisInput;

    public override void OnAddedToEntity()
    {
        SetupSprites();

        SetupInput();
    }

    public override void OnRemovedFromEntity()
    {
        _fireInput.Deregister();
    }

    private void SetupInput()
    {
        _fireInput = new VirtualButton();
        _fireInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.Z));
        _fireInput.Nodes.Add(new VirtualButton.GamePadButton(0, Buttons.A));

        _xAxisInput = new VirtualIntegerAxis();
        _xAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
        _xAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
        _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));

        _yAxisInput = new VirtualIntegerAxis();
        _yAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadUpDown());
        _yAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickY());
        _yAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));
    }

    private void SetupSprites()
    {
        /*var charPng = 1; //Random.Range(1, 7);
        Entity.Scene.Content.UnloadAsset<Texture2D>("Content/characters/" + charPng + ".png");
        var texture = Entity.Scene.Content.LoadTexture("Content/characters/" + charPng + ".png");
        var sprites = Sprite.SpritesFromAtlas(texture, 16, 16);*/

        Entity.RemoveComponent<SpriteMime>();
        Entity.RemoveComponent<Mover>();
        Entity.RemoveComponent<SpriteAnimator>();

        _animator = AsepriteLoader.BuildAnimator(Entity.Scene.Content, "Content/characters/ss_packed.json");

        _mover = Entity.AddComponent(new Mover());
        Entity.AddComponent(_animator);

        var shadow = Entity.AddComponent(new SpriteMime(Entity.GetComponent<SpriteRenderer>()));
        shadow.Color = new Color(10, 10, 10, 80);
        shadow.Material = Material.StencilRead();
        shadow.RenderLayer = -2;

        /*_animator.AddAnimation("WalkLeft", new[]
        {
            sprites[2],
            sprites[6],
            sprites[10],
            sprites[14]
        });
        _animator.AddAnimation("WalkRight", new[]
        {
            sprites[3],
            sprites[7],
            sprites[11],
            sprites[15]
        });
        _animator.AddAnimation("WalkDown", new[]
        {
            sprites[0],
            sprites[4],
            sprites[8],
            sprites[12]
        });
        _animator.AddAnimation("WalkUp", new[]
        {
            sprites[1],
            sprites[5],
            sprites[9],
            sprites[13]
        });*/
    }

    public void Update()
    {
        if (Input.IsKeyPressed(Keys.Space))
            SetupSprites();

        var moveDir = new Vector2(_xAxisInput.Value, _yAxisInput.Value);
        var animation = "WalkDown";

        if (moveDir.X < 0)
            animation = "WalkLeft";
        else if (moveDir.X > 0)
            animation = "WalkRight";

        if (moveDir.Y < 0)
            animation = "WalkUp";
        else if (moveDir.Y > 0)
            animation = "WalkDown";

        if (moveDir != Vector2.Zero)
        {
            if (!_animator.IsAnimationActive(animation))
                _animator.Play(animation);
            else 
                _animator.UnPause();

            var movement = moveDir * _moveSpeed * Time.DeltaTime;

            _mover.CalculateMovement(ref movement, out var res);
            _subpixelV2.Update(ref movement);
            _mover.ApplyMovement(movement);
        }
        else
        {
            _animator.Pause();
        }

        if (_fireInput.IsPressed)
        {
            var dir = Vector2.Zero;

            switch (_animator.CurrentAnimationName)
            {
                case "WalkUp":
                    dir.Y = -1;
                    break;
                case "WalkDown":
                    dir.Y = 1;
                    break;
                case "WalkRight":
                    dir.X = 1;
                    break;
                case "WalkLeft":
                    dir.X = -1;
                    break;
                default:
                    dir = new Vector2(1, 0);
                    break;
            }

            var scene = Entity.Scene as SimpleGameplayScene;
            scene?.CreateProjectiles(Entity.Transform.Position, _projectileVelocity * dir);
        }
    }

    public void OnTriggerEnter(Collider other, Collider local)
    {
        
    }

    public void OnTriggerExit(Collider other, Collider local)
    {
        
    }
}