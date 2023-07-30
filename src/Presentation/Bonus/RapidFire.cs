using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("RapidFire.tscn")]
public partial class RapidFire
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

        bonusCollector.Collect(BonusType.RapidFire);

        this.QueueFree();
    }
}
