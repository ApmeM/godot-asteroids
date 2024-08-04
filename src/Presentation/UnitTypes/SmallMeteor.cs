using DodgeTheCreeps.UnitTypes;
using Godot;
using GodotAnalysers;

[SceneReference("SmallMeteor.tscn")]
public partial class SmallMeteor : IHitable, IMinimapElement
{
    private Communicator communicator;
    public bool VisibleOnBorder => true;
    public Sprite Sprite => this.minimapTexture;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.communicator = GetNode<Communicator>("/root/Main/Communicator");

        this.AddToGroup(Groups.MinimapElement);
        this.AddToGroup(Groups.DynamicGameObject);
        this.AddToGroup(Groups.EnemyUnit);

        this.CollisionLayer = (int)(CollisionLayers.Player | CollisionLayers.Enemy | CollisionLayers.Block);
        this.CollisionMask = (int)(CollisionLayers.Player | CollisionLayers.Block);
    }

    public void Hit(IHitter byNode)
    {
        this.lifeProgress.Value -= byNode.Power;
        if (this.lifeProgress.Value > 0)
        {
            return;
        }

        this.CollisionLayer = 0;
        this.CollisionMask = 0;

        this.communicator.EmitSignal(nameof(Communicator.ScoreAdded), 1);

        this.QueueFree();
    }
}
