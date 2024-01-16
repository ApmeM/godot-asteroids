using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;

[SceneReference("Player.tscn")]
public partial class Player : IBonusCollector, IMinimapElement
{
    public bool VisibleOnBorder => true;
    public Sprite Sprite => this.minimapTexture;

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
    public NodePath FieldPath;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.Connect(CommonSignals.BodyEntered, this, nameof(OnPlayerBodyEntered));

        this.AddToGroup(Groups.DynamicGameObject);
        this.AddToGroup(Groups.MinimapElement);

        this.CollisionLayer = (int)(CollisionLayers.Player | CollisionLayers.Bonus);
        this.CollisionMask = (int)CollisionLayers.Player;

        this.guns.ClearChildren();
        this.Collect(BonusType.Weapon);
        this.camera2D.Current = true;
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

    private void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        EmitSignal(nameof(Hit));
        this.CollisionLayer = 0;
        this.Layers = 0;
        this.camera2D.Current = false;
    }

    private void AddGun(Vector2 position, float rotation = 0)
    {
        var gun = (Gun)Gun.Instance();
        this.guns.AddChild(gun);
        gun.Position = position;
        gun.Rotation = rotation;
        gun.Field = this.GetNode(this.FieldPath).GetPath();
    }

    public void Collect(BonusType bonus)
    {
        switch (bonus)
        {
            case BonusType.Weapon:
                switch (this.guns.GetChildCount())
                {
                    case 0:
                        // this.AddGun(new Vector2(40, 0));
                        this.AddGun(new Vector2(35, -20));
                        this.AddGun(new Vector2(35, 20));
                        break;
                    case 1:
                        // this.AddGun(new Vector2(35, -20));
                        break;
                    case 2:
                        // this.AddGun(new Vector2(35, 20));
                        this.AddGun(new Vector2(40, 0));
                        break;
                    case 3:
                        this.AddGun(new Vector2(30, 35), Mathf.Pi / 18);
                        break;
                    case 4:
                        this.AddGun(new Vector2(30, -35), -Mathf.Pi / 18);
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
