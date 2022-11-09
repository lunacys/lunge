using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace Playground.Components.WorldGeneration;

public class SimplePlayerComponent : Component, IUpdatable
{
    private Mover _mover;
    private VirtualIntegerAxis _xAxisInput;
    private VirtualIntegerAxis _yAxisInput;

    private float MoveSpeed = 100;

    public override void OnAddedToEntity()
    {
        _mover = Entity.GetComponent<Mover>();

        SetUpInput();
    }


    public void Update()
    {
        var moveDir = new Vector2(_xAxisInput.Value, _yAxisInput.Value);

        if (moveDir != Vector2.Zero)
        {
            var movement = moveDir * MoveSpeed * Time.DeltaTime;

            _mover.CalculateMovement(ref movement, out _);
            _mover.ApplyMovement(movement);
        }
    }

    private void SetUpInput()
    {
        _xAxisInput = new VirtualIntegerAxis();
        _xAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
        _xAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
        _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));

        _yAxisInput = new VirtualIntegerAxis();
        _yAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadUpDown());
        _yAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickY());
        _yAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));
    }
}