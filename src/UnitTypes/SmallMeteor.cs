using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("SmallMeteor.tscn")]
public partial class SmallMeteor : IHitable
{
    private Communicator communicator;

    public bool IsDead { get; private set; }

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
        IsDead = true;

        var bonus = BonusType.Booster.CreateBonus();
        bonus.Position = this.Position;
        this.GetParent().CallDeferred("add_child", bonus);

        this.communicator.EmitSignal(nameof(Communicator.ScoreAdded), 1);

        this.QueueFree();
    }
}
