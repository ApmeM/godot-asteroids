using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;

[SceneReference("Main.tscn")]
public partial class Main
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.menu.Connect(nameof(Menu.StartGame), this, nameof(NewGame));
        this.game.Connect(nameof(Game.GameOver), this, nameof(GameOver));

        this.game.Hide();
        this.menu.Show();

        GD.Randomize();
    }

    private void GameOver(int score)
    {
        this.game.Hide();
        this.menu.Show();
        this.menu.GameOver(this.game.GameId, score);
    }

    private void NewGame(int gameId)
    {
        this.menu.Hide();
        this.game.Show();
        this.game.NewGame(gameId);
    }
}
