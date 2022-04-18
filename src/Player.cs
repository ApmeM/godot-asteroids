using DodgeTheCreeps;
using Godot;
using GodotAnalysers;

[SceneReference("Player.tscn")]
public partial class Player : RigidBody2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public int Force = 3000;

    [Export]
    public int Torque = 100000;

    [Export]
    public int MaxSpeed = 500;

    [Export]
    public int MaxAngularSpeed = 2;

    public Rect2 fieldSize;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.Connect(CommonSignals.BodyEntered, this, nameof(OnPlayerBodyEntered));
    }

    public override void _PhysicsProcess(float delta)
    {
        this.AppliedForce = Vector2.Zero;
        this.AppliedTorque = 0;

        if (Input.IsActionPressed("move_right"))
        {
            this.AppliedTorque = this.Torque;
        }

        if (Input.IsActionPressed("move_left"))
        {
            this.AppliedTorque = -this.Torque;
        }

        if (Input.IsActionPressed("move_down"))
        {
            this.AppliedForce = -Vector2.Right.Rotated(this.Rotation) * Force;
        }

        if (Input.IsActionPressed("move_up"))
        {
            this.AppliedForce = Vector2.Right.Rotated(this.Rotation) * Force;
        }
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        base._IntegrateForces(state);


        if (state.LinearVelocity.LengthSquared() > MaxSpeed * MaxSpeed)
        {
            state.LinearVelocity = state.LinearVelocity.Normalized() * MaxSpeed;
        }

        if (state.AngularVelocity > MaxAngularSpeed)
        {
            state.AngularVelocity = MaxAngularSpeed;
        }

        if (state.AngularVelocity < -MaxAngularSpeed)
        {
            state.AngularVelocity = -MaxAngularSpeed;
        }
    }

    public void Start(Vector2 pos, Rect2 fieldSize)
    {
        this.fieldSize = fieldSize;
        Position = pos;
        this.collisionShape2D.Disabled = false;
    }

    private void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        EmitSignal(nameof(Hit));
        this.collisionShape2D.SetDeferred("disabled", true);
    }
}
