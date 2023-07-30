using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using FateRandom;
using Godot;
using GodotAnalysers;

[SceneReference("SmallMeteor.tscn")]
public partial class SmallMeteor : IHitable
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

    public void Hit(Bullet byNode)
    {
        this.lifeProgress.Value -= byNode.Power;
        if (this.lifeProgress.Value > 0)
        {
            return;
        }

        this.CollisionLayer = 0;
        this.Layers = 0;

        if (fate.Chance(20))
        {
            var bonus = fate.Choose<BonusType>(BonusType.Weapon, BonusType.RapidFire, BonusType.Power).CreateBonus();
            bonus.Position = this.Position;
            this.GetParent().CallDeferred("add_child", bonus);
        }

        this.communicator.EmitSignal(nameof(Communicator.ScoreAdded), 1);

        this.QueueFree();
    }
}
