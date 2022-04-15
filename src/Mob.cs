using Godot;
using GodotAnalysers;

[SceneReference("Mob.tscn")]
public partial class Mob : RigidBody2D
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.visibility.Connect("screen_exited", this, nameof(OnVisibilityScreenExited));

        this.animatedSprite.Playing = true;
        string[] mobTypes = this.animatedSprite.Frames.GetAnimationNames();
        this.animatedSprite.Animation = mobTypes[GD.Randi() % mobTypes.Length];
    }

    private void OnVisibilityScreenExited()
    {
        QueueFree();
    }
}
