using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using System;

[SceneReference("Player.tscn")]
public partial class Player : IBonusCollector
{
    [Signal]
    public delegate void Hit();

    [Export]
    public int Force = 3000;

    [Export]
    public int Torque = 200000;

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
    private int powerUp = 0;

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

        var vector = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        if (vector == Vector2.Zero)
        {
            return;
        }

        var rotation = this.Transform.x;
        var angle = rotation.AngleTo(vector);
        this.Rotation += Mathf.Clamp(angle, -10 * delta, 10 * delta);
        this.AppliedForce = this.Transform.x * Force;
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        base._IntegrateForces(state);

        if (initialize)
        {
            initialize = false;

            state.Transform = new Transform2D(0, initialPosition);
            state.LinearVelocity = Vector2.Zero;
        }

        var vector = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        if (vector != Vector2.Zero)
        {
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
        Node2D bullet;
        switch (this.powerUp)
        {
            case 0:
                bullet = (Node2D)Bullet.Instance();
                bullet.GlobalPosition = this.endOfGun.GlobalPosition;
                bullet.GlobalRotation = this.GlobalRotation;
                this.GetNode(this.Field).AddChild(bullet);
                break;
            case 1:
                bullet = (Node2D)Bullet.Instance();
                bullet.GlobalPosition = this.endOfGun.GlobalPosition + Vector2.Down.Rotated(this.Rotation) * 20;
                bullet.GlobalRotation = this.GlobalRotation;
                this.GetNode(this.Field).AddChild(bullet);

                bullet = (Node2D)Bullet.Instance();
                bullet.GlobalPosition = this.endOfGun.GlobalPosition - Vector2.Down.Rotated(this.Rotation) * 20;
                bullet.GlobalRotation = this.GlobalRotation;
                this.GetNode(this.Field).AddChild(bullet);
                break;
            case 2:
                bullet = (Node2D)Bullet.Instance();
                bullet.GlobalPosition = this.endOfGun.GlobalPosition + Vector2.Down.Rotated(this.Rotation) * 20;
                bullet.GlobalRotation = this.GlobalRotation;
                this.GetNode(this.Field).AddChild(bullet);

                bullet = (Node2D)Bullet.Instance();
                bullet.GlobalPosition = this.endOfGun.GlobalPosition - Vector2.Down.Rotated(this.Rotation) * 20;
                bullet.GlobalRotation = this.GlobalRotation;
                this.GetNode(this.Field).AddChild(bullet);

                bullet = (Node2D)Bullet.Instance();
                bullet.GlobalPosition = this.endOfGun.GlobalPosition + Vector2.Right.Rotated(this.Rotation) * 20;
                bullet.GlobalRotation = this.GlobalRotation;
                this.GetNode(this.Field).AddChild(bullet);
                break;
            default:
                bullet = (Node2D)Bullet.Instance();
                bullet.GlobalPosition = this.endOfGun.GlobalPosition + Vector2.Down.Rotated(this.Rotation) * 20;
                bullet.GlobalRotation = this.GlobalRotation;
                this.GetNode(this.Field).AddChild(bullet);

                bullet = (Node2D)Bullet.Instance();
                bullet.GlobalPosition = this.endOfGun.GlobalPosition + Vector2.Down.Rotated(this.Rotation) * 20;
                bullet.GlobalRotation = this.GlobalRotation + Mathf.Pi / 8;
                this.GetNode(this.Field).AddChild(bullet);

                bullet = (Node2D)Bullet.Instance();
                bullet.GlobalPosition = this.endOfGun.GlobalPosition - Vector2.Down.Rotated(this.Rotation) * 20;
                bullet.GlobalRotation = this.GlobalRotation;
                this.GetNode(this.Field).AddChild(bullet);

                bullet = (Node2D)Bullet.Instance();
                bullet.GlobalPosition = this.endOfGun.GlobalPosition - Vector2.Down.Rotated(this.Rotation) * 20;
                bullet.GlobalRotation = this.GlobalRotation - Mathf.Pi / 8;
                this.GetNode(this.Field).AddChild(bullet);

                bullet = (Node2D)Bullet.Instance();
                bullet.GlobalPosition = this.endOfGun.GlobalPosition + Vector2.Right.Rotated(this.Rotation) * 20;
                bullet.GlobalRotation = this.GlobalRotation;
                this.GetNode(this.Field).AddChild(bullet);
                break;
        }
    }

    private void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        EmitSignal(nameof(Hit));
        this.collisionShape2D.SetDeferred("disabled", true);
    }

    public void Collect(BonusType bonus)
    {
        if (bonus == BonusType.Booster)
        {
            this.powerUp++;
        }
    }
}
