using DodgeTheCreeps.Utils;
using GodotAnalysers;
using Godot;
using DodgeTheCreeps.UnitTypes;

[SceneReference("Block.tscn")]
public partial class Block: IHitable
{
    public bool IsDead => false;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Constants.MinimapIconBlock);
        this.AddToGroup(Constants.DynamicGameObject);
    }

    public void Hit(Node2D byNode)
    {
    }
}
