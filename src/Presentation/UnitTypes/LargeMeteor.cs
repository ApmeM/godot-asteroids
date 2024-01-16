using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("LargeMeteor.tscn")]
public partial class LargeMeteor : IHitable, IMinimapElement
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

        this.CollisionLayer = (int)(CollisionLayers.Player | CollisionLayers.Enemy);
        this.CollisionMask = (int)CollisionLayers.Player;
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
        var meteor = UnitType.Meteor.CreateUnit<Meteor>();
        direction = (this.Position - byNode.Position).Rotated(-Mathf.Pi / 2).Normalized();
        meteor.LinearVelocity = direction * (float)GD.RandRange(250.0, 350.0);
        meteor.Position = this.Position - direction * 50;
        this.GetParent().CallDeferred("add_child", meteor);

        meteor = UnitType.Meteor.CreateUnit<Meteor>();
        direction = (this.Position - byNode.Position).Rotated(Mathf.Pi / 2).Normalized();
        meteor.LinearVelocity = direction * (float)GD.RandRange(250.0, 350.0);
        meteor.Position = this.Position + direction * 50;
        this.GetParent().CallDeferred("add_child", meteor);

        this.communicator.EmitSignal(nameof(Communicator.ScoreAdded), 100);

        this.QueueFree();
    }
}
