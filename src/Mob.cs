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

        this.AddToGroup(Constants.MinimapIconEnemy);
    }
}
