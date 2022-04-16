using DodgeTheCreeps;
using Godot;
using GodotAnalysers;

[SceneReference("HUD.tscn")]
public partial class HUD : Node2D
{
    [Signal]
    public delegate void StartGame();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

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
}
