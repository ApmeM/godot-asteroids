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
        this.animationPlayer.Play("Boom");
        this.animationPlayer.Connect(CommonSignals.AnimationFinished, this, nameof(LifetimeAnimationFinished));
    }

    private void LifetimeAnimationFinished(string animationName)
    {
        this.QueueFree();
    }
}
