using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("SmallMeteor.tscn")]
public partial class SmallMeteor : IHitable
{
    public bool IsDead { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Constants.MinimapIconEnemy);
        this.AddToGroup(Constants.DynamicGameObject);
    }

    public void Hit(Node2D byNode)
    {
        IsDead = true;

        var bonus = BonusType.Booster.CreateBonus();
        bonus.Position = this.Position;
        this.GetParent().AddChild(bonus);

        this.QueueFree();
    }
}
