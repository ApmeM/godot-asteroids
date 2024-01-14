using System;
using Godot;
using GodotAnalysers;

[SceneReference("HUD.tscn")]
public partial class HUD
{
    private Communicator communicator;

    public double MaxProgress
    {
        get => this.gameProgressBar.MaxValue;
        set => this.gameProgressBar.MaxValue = value;
    }

    public double Progress
    {
        get => this.gameProgressBar.Value;
        set => this.gameProgressBar.Value = value;
    }
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
        this.timerLabel.ShowMessage("Get Ready!", 1);

        this.minimap.SetMapSizeToNode(rect);
        this.minimap.CenterNodePath = playerPath;
    }

    internal void Stop()
    {
        this.minimap.CenterNodePath = null;
    }

    internal void ShowDialog(string text)
    {
        this.dialog.Show(text);
    }
}
