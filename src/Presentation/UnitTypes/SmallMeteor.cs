using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("SmallMeteor.tscn")]
public partial class SmallMeteor : IHitable
{
    private Communicator communicator;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.communicator = GetNode<Communicator>("/root/Main/Communicator");

        this.AddToGroup(Groups.MinimapIconEnemy);
        this.AddToGroup(Groups.DynamicGameObject);
        this.AddToGroup(Groups.EnemyUnit);
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

        this.communicator.EmitSignal(nameof(Communicator.ScoreAdded), 1);

        this.QueueFree();
    }
}
