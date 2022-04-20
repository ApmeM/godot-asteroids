using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("Bullet.tscn")]
public partial class Bullet
{
    [Export]
    public float Speed = 1000;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.lifetime.Connect(CommonSignals.Timeout, this, nameof(LifetimeTimeout));
        this.Connect(CommonSignals.BodyEntered, this, nameof(Hit));
    }

    private void Hit(Node body)
    {
        GD.Print($"Hit {body.GetType()}");
        if (!(body is Mob bullet))
        {
            return;
        }

        this.QueueFree();
        bullet.QueueFree();
    }

    private void LifetimeTimeout()
    {
        this.QueueFree();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        this.Position += Vector2.Right.Rotated(this.Rotation) * Speed * delta;
    }
}
