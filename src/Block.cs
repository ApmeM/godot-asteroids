using DodgeTheCreeps.Utils;
using GodotAnalysers;
using Godot;

[SceneReference("Block.tscn")]
public partial class Block
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Constants.MinimapIconBlock);
        this.AddToGroup(Constants.DynamicGameObject);
    }
}
