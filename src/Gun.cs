using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("Gun.tscn")]
public partial class Gun
{
    [Export]
    public PackedScene Bullet;

    [Export]
    public NodePath Field;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.shootTimer.Connect(CommonSignals.Timeout, this, nameof(OnPlayerShoot));
    }

    private void OnPlayerShoot()
    {
        var bullet = (Node2D)Bullet.Instance();

        this.GetNode(this.Field).AddChild(bullet);
        bullet.GlobalPosition = this.endOfGun.GlobalPosition;
        bullet.GlobalRotation = this.GlobalRotation;
    }
}
