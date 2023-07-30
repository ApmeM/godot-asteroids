using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("Bullet.tscn")]
public partial class Bullet
{
    [Export]
    public float Speed = 1500;

    [Export]
    public PackedScene Explosion;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.lifetime.Connect(CommonSignals.Timeout, this, nameof(LifetimeTimeout));
        this.Connect(CommonSignals.BodyEntered, this, nameof(Hit));
    }

    private void Hit(Node body)
    {
        if (body is IHitable hit)
        {
            hit.Hit(this);
        }

        var explosion = (Node2D)Explosion.Instance();
        explosion.Position = this.Position;
        this.GetParent().AddChild(explosion);

        this.QueueFree();
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
