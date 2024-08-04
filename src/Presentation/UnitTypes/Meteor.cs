using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("Meteor.tscn")]
public partial class Meteor : IHitable, IMinimapElement
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

        Vector2 direction;
        var meteor = UnitType.SmallMeteor.CreateUnit<SmallMeteor>();
        direction = (this.Position - byNode.Position).Rotated(-Mathf.Pi / 2).Normalized();
        meteor.LinearVelocity = direction * (float)GD.RandRange(250.0, 350.0);
        meteor.Position = this.Position - direction * 50;
        this.GetParent().CallDeferred("add_child", meteor);

        meteor = UnitType.SmallMeteor.CreateUnit<SmallMeteor>();
        direction = (this.Position - byNode.Position).Rotated(Mathf.Pi / 2).Normalized();
        meteor.LinearVelocity = direction * (float)GD.RandRange(250.0, 350.0);
        meteor.Position = this.Position + direction * 50;
        this.GetParent().CallDeferred("add_child", meteor);

        this.communicator.EmitSignal(nameof(Communicator.ScoreAdded), 20);

        this.QueueFree();
    }
}
