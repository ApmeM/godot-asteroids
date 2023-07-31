using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("Weapon.tscn")]
public partial class Weapon
{
    [Export]
    public float RotationSpeed = Mathf.Pi / 2;

    public override void _Ready()
    {
        this.FillMembers();

        this.Connect(CommonSignals.BodyEntered, this, nameof(Hit));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        this.Rotate(delta * RotationSpeed);
    }

    private void Hit(Node body)
    {
        if (!(body is IBonusCollector bonusCollector))
        {
            return;
        }

        bonusCollector.Collect(BonusType.Weapon);

        this.QueueFree();
    }
}
