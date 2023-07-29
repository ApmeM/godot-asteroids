using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using System;

[SceneReference("TimerLabel.tscn")]
public partial class TimerLabel
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();        

        this.Text = "";
        this.messageTimer.OneShot = true;
        this.messageTimer.Connect(CommonSignals.Timeout, this, nameof(OnMessageTimerTimeout));
    }

    public void ShowMessage(string text, float timeout)
    {
        this.Text = text;
        this.messageTimer.WaitTime = timeout;
        this.messageTimer.Start();
    }

    private void OnMessageTimerTimeout()
    {
        this.Text = "";
    }
}
