using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using System.Linq;

[SceneReference("Game.tscn")]
public partial class Game
{
    [Signal]
    public delegate void GameOver(int score);

    [Export]
    public PackedScene blockScene;

    private const int pathSize = 1;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.player.Connect(nameof(Player.Hit), this, nameof(PlayerHit));
        this.Connect(CommonSignals.VisibilityChanged, this, nameof(VisibilityChanged));

        GD.Randomize();
    }

    private void VisibilityChanged()
    {
        if (this.Visible)
        {
            this.hUD.Show();
        }
        else
        {
            this.hUD.Hide();
        }
    }

    private void PlayerHit()
    {
        this.music.Stop();
        this.deathSound.Play();

        this.camera2D.Current = false;

        this.EmitSignal(nameof(GameOver), 0);
    }

    public void NewGame()
    {
        this.GetTree().CallGroup(Constants.DynamicGameObject, "queue_free");

        var maze = MazeGeneratorWrapper.DefaultInstance;
        maze.GenerateLevel1();
        var startPosition = maze.State.StartPosition * 100 * pathSize + Vector2.One * 50 * pathSize;
        var rect = new Rect2(Vector2.Zero, new Vector2(maze.State.Map.GetLength(0) * 100 * pathSize + pathSize * 50, maze.State.Map.GetLength(1) * 100 * pathSize + pathSize * 50));

        for (var x = 0; x < maze.State.Map.GetLength(0); x++)
            for (var y = 0; y < maze.State.Map.GetLength(1); y++)
            {
                if (maze.State.Map[x, y] != 1)
                {
                    continue;
                }

                for (var x2 = 0; x2 < pathSize; x2++)
                    for (var y2 = 0; y2 < pathSize; y2++)
                    {
                        var block = (Block)blockScene.Instance();
                        block.Position = new Vector2(x * 100 * pathSize + x2 * 100, y * 100 * pathSize + y2 * 100);
                        this.AddChild(block);
                    }
            }

        foreach (var unitItem in maze.State.UnitsList)
        {
            var mobSpawnLocation = unitItem.Key * 100 * pathSize + Vector2.One * 50 * pathSize;

            var direction = (this.player.Position - mobSpawnLocation).Angle();
            var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);

            var mob = unitItem.Value.CreateUnit();
            mob.Position = mobSpawnLocation;
            mob.Rotation = direction;
            mob.LinearVelocity = velocity.Rotated(direction);
            this.AddChild(mob);
        }

        this.player.Start(startPosition, rect);

        this.camera2D.Current = true;

        this.hUD.SetScore(0);
        this.hUD.SetMapSize(rect);
        this.hUD.ShowMessage("Get Ready!");

        this.music.Play();
    }
}
