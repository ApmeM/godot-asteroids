using DodgeTheCreeps;
using Godot;
using GodotAnalysers;

[SceneReference("Menu.tscn")]
public partial class Menu : Node2D
{
    [Signal]
    public delegate void StartGame();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.startButton.Connect(CommonSignals.Pressed, this, nameof(OnStartButtonPressed));
        this.messageTimer.Connect(CommonSignals.Timeout, this, nameof(OnMessageTimerTimeout));
    }

    public void GameOver(int score)
    {
        this.scoreLabel.Text = score.ToString();
        this.messageLabel.Text = "Game Over";
        this.messageTimer.Start();
    }

    private void OnStartButtonPressed()
    {
        EmitSignal(nameof(StartGame));
    }

    private void OnMessageTimerTimeout()
    {
        this.messageLabel.Text = "Dodge the\nCreeps!";
        this.messageTimer.Stop();
    }
}
