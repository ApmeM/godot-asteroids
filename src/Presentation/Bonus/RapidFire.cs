using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("RapidFire.tscn")]
public partial class RapidFire: IMinimapElement
{
    public bool VisibleOnBorder => true;
    public Sprite Sprite => this.minimapTexture;

    [Export]
    public float RotationSpeed = Mathf.Pi / 2;

    public override void _Ready()
    {
        this.FillMembers();

        this.Connect(CommonSignals.BodyEntered, this, nameof(Hit));
        this.AddToGroup(Groups.DynamicGameObject);
        this.AddToGroup(Groups.MinimapElement);
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

        bonusCollector.Collect(BonusType.RapidFire);

        this.QueueFree();
    }
}
