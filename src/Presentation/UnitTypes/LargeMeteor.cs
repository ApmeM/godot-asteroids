using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using FateRandom;
using Godot;
using GodotAnalysers;

[SceneReference("LargeMeteor.tscn")]
public partial class LargeMeteor : IHitable
{
    private Communicator communicator;
    private Fate fate = Fate.GlobalFate;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.communicator = GetNode<Communicator>("/root/Main/Communicator");

        this.AddToGroup(Constants.MinimapIconEnemy);
        this.AddToGroup(Constants.DynamicGameObject);
        this.AddToGroup(Constants.GameTarget);
    }

    public void Hit(Node2D byNode)
    {
        this.lifeProgress.Value--;
        if (this.lifeProgress.Value > 0)
        {
            return;
        }

        this.CollisionLayer = 0;
        this.Layers = 0;

        Vector2 direction;
        var meteor = UnitType.Meteor.CreateUnit();
        direction = (this.Position - byNode.Position).Rotated(-Mathf.Pi / 2).Normalized();
        meteor.LinearVelocity = direction * (float)GD.RandRange(250.0, 350.0);
        meteor.Position = this.Position - direction * 50;
        this.GetParent().CallDeferred("add_child", meteor);

        meteor = UnitType.Meteor.CreateUnit();
        direction = (this.Position - byNode.Position).Rotated(Mathf.Pi / 2).Normalized();
        meteor.LinearVelocity = direction * (float)GD.RandRange(250.0, 350.0);
        meteor.Position = this.Position + direction * 50;
        this.GetParent().CallDeferred("add_child", meteor);

        if (fate.Chance(20))
        {
            var bonus = fate.Choose<BonusType>(BonusType.Weapon, BonusType.RapidFire).CreateBonus();
            bonus.Position = this.Position;
            this.GetParent().CallDeferred("add_child", bonus);
        }

        this.communicator.EmitSignal(nameof(Communicator.ScoreAdded), 100);

        this.QueueFree();
    }
}
