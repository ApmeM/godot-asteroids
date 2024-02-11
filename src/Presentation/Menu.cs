using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;
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

        this.levelContainer.ClearChildren();
        for (int i = 0; i < MazeGeneratorWrapper.DefaultInstance.Levels.Count; i++)
        {
            var level = MazeGeneratorWrapper.DefaultInstance.Levels[i];
            var button = new Button
            {
                Text = level.Name
            };
            button.Connect(CommonSignals.Pressed, this, nameof(OnLevelPressed), new Godot.Collections.Array { i });
            this.levelContainer.AddChild(button);
        }
    }

    public void GameOver(int gameId, int score)
    {
        var button = (Button)this.levelContainer.GetChild(gameId);
        var level = MazeGeneratorWrapper.DefaultInstance.Levels[gameId];
        button.Text = $"{level.Name}: {score}";
        this.timerLabel.ShowMessage("Game Over", 2);
    }

    private void OnLevelPressed(int gameId)
    {
        EmitSignal(nameof(StartGame), gameId);
    }
}
