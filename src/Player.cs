using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using System;

[SceneReference("Player.tscn")]
public partial class Player
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

    [Export]
    public PackedScene Bullet;

    [Export]
    public NodePath Field;

    private Rect2 fieldSize;
    private Vector2 initialPosition;
    private bool initialize = false;


    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.Connect(CommonSignals.BodyEntered, this, nameof(OnPlayerBodyEntered));
        this.shootTimer.Connect(CommonSignals.Timeout, this, nameof(OnPlayerShoot));
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

        this.AppliedForce = Vector2.Right.Rotated(this.Rotation) * Force;
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        base._IntegrateForces(state);

        if (initialize)
        {
            initialize = false;

            state.Transform = new Transform2D(0, initialPosition);
            state.LinearVelocity = Vector2.Zero;
            state.AngularVelocity = 0;
        }

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
        this.initialPosition = pos;
        this.initialize = true;
        this.collisionShape2D.Disabled = false;
    }

    private void OnPlayerShoot()
    {
        var bullet = (Node2D)Bullet.Instance();
        bullet.GlobalPosition = this.endOfGun.GlobalPosition;
        bullet.Rotation = this.Rotation;
        this.GetNode(this.Field).AddChild(bullet);
    }

    private void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        EmitSignal(nameof(Hit));
        this.collisionShape2D.SetDeferred("disabled", true);
    }
}
