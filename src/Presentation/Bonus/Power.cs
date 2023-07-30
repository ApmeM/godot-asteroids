using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("Power.tscn")]
public partial class Power
{
    public override void _Ready()
    {
        this.FillMembers();

        this.Connect(CommonSignals.BodyEntered, this, nameof(Hit));
    }

    private void Hit(Node body)
    {
        if (!(body is IBonusCollector bonusCollector))
        {
            return;
        }

        bonusCollector.Collect(BonusType.Power);

        this.QueueFree();
    }
}
