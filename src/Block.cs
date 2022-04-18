using DodgeTheCreeps;
using Godot;
using GodotAnalysers;
using System;

[SceneReference("Block.tscn")]
public partial class Block : RigidBody2D
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Constants.MinimapIconBlock);
    }
}
