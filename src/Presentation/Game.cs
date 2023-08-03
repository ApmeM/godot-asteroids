using DodgeTheCreeps.Presentation.Utils.MapEvents;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using System.Collections.Generic;

[SceneReference("Game.tscn")]
public partial class Game
{
    [Signal]
    public delegate void GameOver(int score);

    [Export]
    public PackedScene blockScene;

    private MazeGeneratorWrapper maze;
    private const int pathSize = 1;
    private Queue<IMapEvent> UnitsList = new Queue<IMapEvent>();

    public Game()
    {
        maze = MazeGeneratorWrapper.DefaultInstance;
    }

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

    private float progress = 0;

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (UnitsList == null)
        {
            return;
        }

        progress += delta;
        while (this.UnitsList.Count > 0 && this.UnitsList.Peek().IsReady(progress))
        {
            var unitItem = this.UnitsList.Dequeue();
            unitItem.Action(this.player.Position, pathSize, this);
            this.hUD.Progress ++;
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

        this.hUD.Progress = 0;
        this.progress = 0;

        var state = maze.GenerateLevel1();

        for (var x = 0; x < state.Map.GetLength(0); x++)
            for (var y = 0; y < state.Map.GetLength(1); y++)
            {
                if (state.Map[x, y] != 1)
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

        this.UnitsList.Clear();
        foreach (var unit in state.UnitsList)
        {
            this.UnitsList.Enqueue(unit);
        }

        this.hUD.MaxProgress = state.UnitsList.Count;

        var startPosition = state.StartPosition * 100 * pathSize + Vector2.One * 50 * pathSize;
        var rect = new Rect2(Vector2.Zero, new Vector2(state.Map.GetLength(0) * 100 * pathSize + pathSize * 50, state.Map.GetLength(1) * 100 * pathSize + pathSize * 50));
        this.player.Start(startPosition, rect);

        this.camera2D.Current = true;

        this.hUD.Start(rect);

        //this.music.Play();
    }
}
