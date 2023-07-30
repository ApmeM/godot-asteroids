using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;

[SceneReference("Player.tscn")]
public partial class Player : IBonusCollector
{
    [Signal]
    public delegate void Hit();

    [Export]
    public int Force = 5000;

    [Export]
    public int MaxSpeed = 500;

    [Export]
    public int MaxAngularSpeed = 2;

    [Export]
    public PackedScene Gun;
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
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.guns.GlobalRotation = GetGlobalMousePosition().AngleToPoint(Position);
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
            this.guns.ClearChildren();
            this.AddGun(new Vector2(40, 0));
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

    private void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        EmitSignal(nameof(Hit));
        this.collisionShape2D.SetDeferred("disabled", true);
    }

    private void AddGun(Vector2 position)
    {
        var gun = (Gun)Gun.Instance();
        this.guns.AddChild(gun);
        gun.Position = position;
        gun.Field = this.GetNode(this.Field).GetPath();
    }

    public void Collect(BonusType bonus)
    {
        if (bonus == BonusType.Booster)
        {
            this.powerUp++;
            this.guns.ClearChildren();
            switch (this.powerUp)
            {
                case 1:
                    this.AddGun(new Vector2(35, -20));
                    this.AddGun(new Vector2(35, 20));
                    break;
                default:
                    this.AddGun(new Vector2(60, 0));
                    this.AddGun(new Vector2(35, -20));
                    this.AddGun(new Vector2(35, 20));
                    break;
            }
        }
    }
}
