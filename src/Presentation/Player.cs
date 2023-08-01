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

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.Connect(CommonSignals.BodyEntered, this, nameof(OnPlayerBodyEntered));

        this.guns.ClearChildren();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.guns.GlobalRotation = GetGlobalMousePosition().AngleToPoint(Position);
    }

    public override void _PhysicsProcess(float delta)
    {
        this.AppliedForce = Vector2.Zero;

        var vector = BugFixExt.InputGetVector("move_left", "move_right", "move_up", "move_down");
        if (vector == Vector2.Zero)
        {
            return;
        }

        var rotation = this.Transform.x;
        var angle = rotation.AngleTo(vector);
        this.Rotation += Mathf.Clamp(angle, -10 * delta, 10 * delta);
        this.AppliedForce = vector * Force;
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

        var vector = BugFixExt.InputGetVector("move_left", "move_right", "move_up", "move_down", 0.01f);

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

    private void AddGun(Vector2 position, float rotation = 0)
    {
        var gun = (Gun)Gun.Instance();
        this.guns.AddChild(gun);
        gun.Position = position;
        gun.Rotation = rotation;
        gun.Field = this.GetNode(this.Field).GetPath();
    }

    public void Collect(BonusType bonus)
    {
        switch (bonus)
        {
            case BonusType.Weapon:
                switch (this.guns.GetChildCount())
                {
                    case 1:
                        AddGun(new Vector2(35, -20));
                        break;
                    case 2:
                        AddGun(new Vector2(35, 20));
                        break;
                    case 3:
                        AddGun(new Vector2(30, 35), Mathf.Pi / 18);
                        break;
                    case 4:
                        AddGun(new Vector2(30, -35), -Mathf.Pi / 18);
                        break;
                    case 5:
                        AddGun(new Vector2(10, 45), Mathf.Pi / 18);
                        break;
                    case 6:
                        AddGun(new Vector2(10, -45), -Mathf.Pi / 18);
                        break;
                }
                break;
            case BonusType.RapidFire:
                foreach (Gun gun in this.guns.GetChildren())
                {
                    gun.IncreaseShootSpeed();
                }
                break;
            case BonusType.Power:
                {
                    foreach (Gun gun in this.guns.GetChildren())
                    {
                        gun.IncreaseShootPower();
                    }
                }
                break;
        }
    }
}
