using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("MeteorGroup.tscn")]
public partial class MeteorGroup
{
    [Export]
    public int NumberOfMeteors = 10;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Constants.DynamicGameObject);
        this.timer.Connect(CommonSignals.Timeout, this, nameof(NextSpawn));
    }

    private void NextSpawn()
    {
        if (NumberOfMeteors < 0)
        {
            this.QueueFree();
        }

        this.NumberOfMeteors--;
        this.pathFollow2D.Offset = GD.Randi();

        // Create a Mob instance and add it to the scene.
        var mob = UnitType.SmallMeteor.CreateUnit<SmallMeteor>();
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
}
