using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using System;
using System.Linq;

[SceneReference("Menu.tscn")]
public partial class Menu
{
    [Signal]
    public delegate void StartGame(int gameId);

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.startLevel1.Connect(CommonSignals.Pressed, this, nameof(OnStartLevel1Pressed));
        this.startInfinity.Connect(CommonSignals.Pressed, this, nameof(OnStartInfinityPressed));
    }

    public void GameOver(int gameId, int score)
    {
        switch (gameId)
        {
            case 1:
                this.startLevel1.Text = $"Level 1: {score}";
                this.timerLabel.ShowMessage("Game Over", 2);
                break;
        }
    }

    private void OnStartLevel1Pressed()
    {
        EmitSignal(nameof(StartGame), 1);
    }

    private void OnStartInfinityPressed()
    {
        EmitSignal(nameof(StartGame), 2);
    }
}
