using System;
using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;

[SceneReference("HUD.tscn")]
public partial class HUD
{
    private Communicator communicator;

    public int Score => this.scoreLabel.Value;

    public bool DialogVisible => this.dialog.Visible;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.communicator = GetNode<Communicator>("/root/Main/Communicator");
        this.communicator.Connect(nameof(Communicator.ScoreAdded), this, nameof(UpdateScore));
    }

    public void UpdateScore(int scoreAdded)
    {
        this.scoreLabel.Value += scoreAdded;
    }

    private void SetScore(int score)
    {
        UpdateScore(score);
    }

    internal void Start(Rect2 rect, NodePath playerPath)
    {
        this.scoreLabel.Value = 0;

        this.minimap.SetMapSizeToNode(rect);
        this.minimap.CenterNodePath = playerPath;
    }

    internal void Stop()
    {
        this.minimap.CenterNodePath = null;
    }

    internal void ShowDialog(string text, bool left)
    {
        this.dialog.Show(text, left);
    }

    internal void ClearStatus()
    {
        this.statusContainer.ClearChildren();
    }

    internal void AddStatus(Control control)
    {
        this.statusContainer.AddChild(control);
    }
}
