using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("BlackHole.tscn")]
public partial class BlackHole : IHitable
{
    public bool IsDead { get; private set; }
    private int LifeLeft = 20;
    private Communicator communicator;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.communicator = GetNode<Communicator>("/root/Main/Communicator");

        this.AddToGroup(Constants.MinimapIconEnemy);
        this.AddToGroup(Constants.DynamicGameObject);
        this.timer.Connect(CommonSignals.Timeout, this, nameof(NextSpawn));
    }

    private void NextSpawn()
    {
        this.pathFollow2D.Offset = GD.Randi();

        // Create a Mob instance and add it to the scene.
        var mob = UnitType.LargeMeteor.CreateUnit();
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

    public void Hit(Node2D byNode)
    {
        LifeLeft--;

        if (LifeLeft == 0)
        {
            this.CollisionLayer = 0;
            this.Layers = 0;

            this.communicator.EmitSignal(nameof(Communicator.ScoreAdded), 1000);
            this.QueueFree();
        }
    }
}
