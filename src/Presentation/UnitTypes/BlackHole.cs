using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("BlackHole.tscn")]
public partial class BlackHole : IMinimapElement
{
    public bool VisibleOnBorder => true;
    public Sprite Sprite => this.minimapTexture;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Groups.MinimapElement);
        this.AddToGroup(Groups.DynamicGameObject);

        this.Connect(CommonSignals.BodyEntered, this, nameof(Hit));

        this.CollisionLayer = 0;
        this.CollisionMask = (int)CollisionLayers.Player;
    }

    private void Hit(Node body)
    {
        body.QueueFree();
    }
}
