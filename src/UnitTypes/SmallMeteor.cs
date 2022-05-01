using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("SmallMeteor.tscn")]
public partial class SmallMeteor : IHitable
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Constants.MinimapIconEnemy);
        this.AddToGroup(Constants.DynamicGameObject);
    }

    public void Hit(Node2D byNode)
    {
        this.QueueFree();
    }
}
