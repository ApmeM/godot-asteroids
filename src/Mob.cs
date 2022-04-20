using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("Mob.tscn")]
public partial class Mob
{
    [Export]
    public PackedScene Bullet;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Constants.MinimapIconEnemy);
        this.AddToGroup(Constants.DynamicGameObject);
    }
}
