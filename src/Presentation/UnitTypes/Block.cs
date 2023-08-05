using DodgeTheCreeps.Utils;
using GodotAnalysers;
using Godot;

[SceneReference("Block.tscn")]
public partial class Block : IMinimapElement
{
    public bool VisibleOnBorder => false;
    public Sprite Sprite => this.minimapTexture;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Groups.MinimapElement);
        this.AddToGroup(Groups.DynamicGameObject);
    }
}
