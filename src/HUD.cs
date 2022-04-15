using DodgeTheCreeps;
using Godot;
using GodotAnalysers;

[SceneReference("HUD.tscn")]
public partial class HUD : CanvasLayer
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

    public void ShowMessage(string text)
    {
        this.messageLabel.Text = text;
        this.messageLabel.Show();

        this.messageTimer.Start();
    }

    public async void ShowGameOver()
    {
        ShowMessage("Game Over");

        await ToSignal(this.messageTimer, CommonSignals.Timeout);

        this.messageLabel.Text = "Dodge the\nCreeps!";
        this.messageLabel.Show();

        this.startButton.Show();
    }

    public void UpdateScore(int score)
    {
        this.scoreLabel.Text = score.ToString();
    }

    private void OnStartButtonPressed()
    {
        this.startButton.Hide();
        EmitSignal(nameof(StartGame));
    }

    private void OnMessageTimerTimeout()
    {
        this.messageLabel.Hide();
    }
}
