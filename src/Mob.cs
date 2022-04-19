using DodgeTheCreeps;
using Godot;
using GodotAnalysers;

[SceneReference("Mob.tscn")]
public partial class Mob
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Constants.MinimapIconEnemy);
    }
}
