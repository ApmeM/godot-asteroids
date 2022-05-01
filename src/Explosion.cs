using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using System;

[SceneReference("Explosion.tscn")]
public partial class Explosion
{
    public override void _Ready()
    {
        this.FillMembers();

        this.lifetime.Connect(CommonSignals.Timeout, this, nameof(LifetimeOver));
    }

    private void LifetimeOver()
    {
        this.QueueFree();
    }
}
