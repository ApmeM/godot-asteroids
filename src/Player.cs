using DodgeTheCreeps;
using Godot;
using GodotAnalysers;

[SceneReference("Player.tscn")]
public partial class Player : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public int speed = 400; // How fast the player will move (pixels/sec).

    public Vector2 screenSize; // Size of the game window.

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.Connect(CommonSignals.BodyEntered, this, nameof(OnPlayerBodyEntered));
        screenSize = GetViewportRect().Size;
    }

    public override void _Process(float delta)
    {
        var velocity = Vector2.Zero; // The player's movement vector.

        if (Input.IsActionPressed("move_right"))
        {
            velocity.x += 1;
        }

        if (Input.IsActionPressed("move_left"))
        {
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("move_down"))
        {
            velocity.y += 1;
        }

        if (Input.IsActionPressed("move_up"))
        {
            velocity.y -= 1;
        }

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * speed;
            this.Rotation = velocity.Angle();
        }

        this.Position += velocity * delta;
        this.Position = new Vector2(
            x: Mathf.Clamp(Position.x, 0, screenSize.x),
            y: Mathf.Clamp(Position.y, 0, screenSize.y)
        );
    }

    public void Start(Vector2 pos)
    {
        Position = pos;
        this.collisionShape2D.Disabled = false;
    }

    private void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        EmitSignal(nameof(Hit));
        this.collisionShape2D.SetDeferred("disabled", true);
    }
}
