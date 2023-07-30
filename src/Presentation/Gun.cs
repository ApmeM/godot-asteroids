using System;
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

    private int power = 1;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.shootTimer.Connect(CommonSignals.Timeout, this, nameof(OnPlayerShoot));
    }

    private void OnPlayerShoot()
    {
        if (Field == null)
        {
            GD.PrintErr("Field not set.");
            return;
        }

        var bullet = (Bullet)Bullet.Instance();

        this.GetNode(this.Field).AddChild(bullet);
        bullet.GlobalPosition = this.endOfGun.GlobalPosition;
        bullet.GlobalRotation = this.GlobalRotation;
        bullet.Power = power;
    }

    public void IncreaseShootSpeed()
    {
        if (this.shootTimer.WaitTime > 0.19f)
        {
            this.shootTimer.WaitTime -= 0.1f;
        }
    }

    internal void IncreaseShootPower()
    {
        if (power < 5)
        {
            power++;
        }
    }
}
