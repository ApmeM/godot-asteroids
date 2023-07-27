using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using System;

[SceneReference("HUD.tscn")]
public partial class HUD
{
    [Export]
    public NodePath PlayerPath;

    private bool isLeftPressed = false;
    private bool isRightPressed = false;
    private Communicator communicator;

    private int score = 0;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.communicator = GetNode<Communicator>("/root/Main/Communicator");
        this.communicator.Connect(nameof(Communicator.ScoreAdded), this, nameof(UpdateScore));

        if (this.PlayerPath != null)
        {
            this.minimap.PlayerPath = this.GetNode(this.PlayerPath).GetPath();
        }

        this.messageTimer.Connect(CommonSignals.Timeout, this, nameof(OnMessageTimerTimeout));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (leftButton.IsPressed() && !isLeftPressed)
        {
            isLeftPressed = true;
            Input.ActionPress("move_left");
        }
        else if (!leftButton.IsPressed() && isLeftPressed)
        {
            isLeftPressed = false;
            Input.ActionRelease("move_left");
        }

        if (rightButton.IsPressed() && !isRightPressed)
        {
            isRightPressed = true;
            Input.ActionPress("move_right");
        }
        else if (!rightButton.IsPressed() && isRightPressed)
        {
            isRightPressed = false;
            Input.ActionRelease("move_right");
        }
    }

    public void ShowMessage(string text)
    {
        this.messageLabel.Text = text;
        this.messageLabel.Show();

        this.messageTimer.Start();
    }

    public void UpdateScore(int scoreAdded)
    {
        this.score += scoreAdded;
        this.scoreLabel.Text = this.score.ToString();
    }
    private void OnMessageTimerTimeout()
    {
        this.messageLabel.Hide();
    }
    
    public void SetMapSize(Rect2 rect)
    {
        this.minimap.SetMapSize(rect);
    }

    public void SetScore(int score)
    {
        this.score = 0;
        UpdateScore(score);
    }
}
