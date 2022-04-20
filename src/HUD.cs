using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using System;

[SceneReference("HUD.tscn")]
public partial class HUD
{
    [Signal]
    public delegate void StartGame();

    [Export]
    public NodePath PlayerPath;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        if (this.PlayerPath != null)
        {
            this.minimap.PlayerPath = this.GetNode(this.PlayerPath).GetPath();
        }

        this.messageTimer.Connect(CommonSignals.Timeout, this, nameof(OnMessageTimerTimeout));
    }

    public void ShowMessage(string text)
    {
        this.messageLabel.Text = text;
        this.messageLabel.Show();

        this.messageTimer.Start();
    }

    public void UpdateScore(int score)
    {
        this.scoreLabel.Text = score.ToString();
    }
    private void OnMessageTimerTimeout()
    {
        this.messageLabel.Hide();
    }
    
    public void SetMapSize(Rect2 rect)
    {
        this.minimap.SetMapSize(rect);
    }
}
