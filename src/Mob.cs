using DodgeTheCreeps;
using Godot;
using GodotAnalysers;

[SceneReference("Mob.tscn")]
public partial class Mob : RigidBody2D
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.visibility.Connect(CommonSignals.ScreenExited, this, nameof(OnVisibilityScreenExited));
    }

    private void OnVisibilityScreenExited()
    {
        QueueFree();
    }
}
