using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("Bullet.tscn")]
public partial class Bullet : IHitter
{
    [Export]
    public float Speed = 1500;

    [Export]
    public PackedScene Explosion;

    public int Power { get; set; }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.sprite.Scale = new Vector2(1, 1 + Power / 2f);

        this.lifetime.Connect(CommonSignals.Timeout, this, nameof(LifetimeTimeout));
        this.Connect(CommonSignals.BodyEntered, this, nameof(Hit));

        this.CollisionLayer = 0;
        this.CollisionMask = (int)(CollisionLayers.Enemy | CollisionLayers.Block);
    }

    private void Hit(Node body)
    {
        if (body is IHitable hit)
        {
            hit.Hit(this);
        }

        this.Speed = 0;
        this.CollisionLayer = 0;
        this.CollisionMask = 0;
        this.sprite.Hide();
        this.sprite2.Show();
        this.animationPlayer.Play("Boom");
        this.animationPlayer.Connect(CommonSignals.AnimationFinished, this, nameof(LifetimeAnimationFinished));
    }

    private void LifetimeTimeout()
    {
        this.QueueFree();
    }

    private void LifetimeAnimationFinished(string animationName)
    {
        this.QueueFree();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        this.Position += Vector2.Right.Rotated(this.Rotation) * Speed * delta;
    }
}
