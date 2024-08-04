using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("MeteorSpawner.tscn")]
public partial class MeteorSpawner : IHitable, IMinimapElement
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
        this.timer.Connect(CommonSignals.Timeout, this, nameof(NextSpawn));

        this.CollisionLayer = (int)(CollisionLayers.Player | CollisionLayers.Enemy | CollisionLayers.Block);
        this.CollisionMask = (int)(CollisionLayers.Player | CollisionLayers.Block);
    }

    private void NextSpawn()
    {
        this.pathFollow2D.Offset = GD.Randi();

        // Create a Mob instance and add it to the scene.
        var mob = UnitType.LargeMeteor.CreateUnit<LargeMeteor>();
        this.GetParent().AddChild(mob);

        // Set the mob's direction perpendicular to the path direction.
        float direction = this.pathFollow2D.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        mob.GlobalPosition = this.pathFollow2D.GlobalPosition;

        // Add some randomness to the direction.
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        // Choose the velocity for the mob.
        var velocity = new Vector2((float)GD.RandRange(250.0, 350.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);
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

        this.communicator.EmitSignal(nameof(Communicator.ScoreAdded), 1000);
        this.QueueFree();
    }
}
