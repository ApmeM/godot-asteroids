using DodgeTheCreeps.Utils;
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
    }

    public void GameOver(int score)
    {
        this.scoreLabel.Text = score.ToString();
        this.timerLabel.ShowMessage("Game Over", 2);
    }

    private void OnStartButtonPressed()
    {
        EmitSignal(nameof(StartGame));
    }
}
