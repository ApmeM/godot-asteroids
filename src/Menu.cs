using DodgeTheCreeps;
using Godot;
using GodotAnalysers;
using System.Linq;

[SceneReference("Menu.tscn")]
public partial class Menu
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

    public void Show()
    {
        foreach (Node2D item in this.GetChildren().OfType<Node2D>())
        {
            item.Show();
        }
        foreach (Control item in this.GetChildren().OfType<Control>())
        {
            item.Show();
        }
    }

    public void Hide()
    {
        foreach (var item in this.GetChildren().OfType<Node2D>())
        {
            item.Hide();
        }
        foreach (Control item in this.GetChildren().OfType<Control>())
        {
            item.Hide();
        }
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
