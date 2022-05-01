using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("Meteor.tscn")]
public partial class Meteor: IHitable
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Constants.MinimapIconEnemy);
        this.AddToGroup(Constants.DynamicGameObject);
    }

    public void Hit(Node2D byNode)
    {
        Vector2 direction;
        var meteor = UnitType.SmallMeteor.CreateUnit();
        direction = (this.Position - byNode.Position).Rotated(-Mathf.Pi / 2).Normalized();
        meteor.LinearVelocity = direction * (float)GD.RandRange(250.0, 350.0);
        meteor.Position = this.Position - direction * 50;
        this.GetParent().AddChild(meteor);

        meteor = UnitType.SmallMeteor.CreateUnit();
        meteor.Position = this.Position;
        direction = (this.Position - byNode.Position).Rotated(Mathf.Pi / 2).Normalized();
        meteor.LinearVelocity = direction * (float)GD.RandRange(250.0, 350.0);
        meteor.Position = this.Position + direction * 50;
        this.GetParent().AddChild(meteor);

        this.QueueFree();
    }
}
