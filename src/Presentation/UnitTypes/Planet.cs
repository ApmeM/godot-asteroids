using System;
using DodgeTheCreeps.UnitTypes;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("Planet.tscn")]
public partial class Planet : IHitter, IMinimapElement
{
    private Communicator communicator;
    public bool VisibleOnBorder => true;
    public Sprite Sprite => this.minimapTexture;
    public int Power => int.MaxValue;

    public int TotalLife => (int)this.lifeProgress.MaxValue;
    public int CurrentLife => (int)this.lifeProgress.Value;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.communicator = GetNode<Communicator>("/root/Main/Communicator");

        this.Connect(CommonSignals.BodyEntered, this, nameof(Hit));

        this.AddToGroup(Groups.MinimapElement);
        this.AddToGroup(Groups.DynamicGameObject);
        this.AddToGroup(Groups.PlanetUnit);

        this.CollisionLayer = 0;
        this.CollisionMask = (int)CollisionLayers.Enemy;
    }

    private void Hit(Node body)
    {
        if (body is IHitable hit)
        {
            hit.Hit(this);
        }

        this.lifeProgress.Value--;

        if (this.lifeProgress.Value > 0)
        {
            return;
        }

        this.CollisionLayer = 0;
        this.CollisionMask = 0;

        this.animatedSprite.Play("boom");
        this.animatedSprite.Connect(CommonSignals.AnimationFinished, this, nameof(LifetimeAnimationFinished));
    }

    private void LifetimeAnimationFinished()
    {
        this.QueueFree();
    }
}
