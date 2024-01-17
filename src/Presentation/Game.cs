using DodgeTheCreeps.Presentation.Utils.GameOver;
using DodgeTheCreeps.Presentation.Utils.MapEvents;
using DodgeTheCreeps.Utils;
using Godot;
using GodotAnalysers;
using System;
using System.Collections.Generic;

[SceneReference("Game.tscn")]
public partial class Game
{
    [Signal]
    public delegate void GameOver(int score);

    [Export]
    public PackedScene blockScene;

    [Export]
    public PackedScene playerScene;

    private MazeGeneratorWrapper maze;
    public const int PathSize = 2;

    private readonly Queue<MapEvent> MapEvents = new Queue<MapEvent>();
    public int MapEventsLeft => this.MapEvents.Count;

    public IGameOver GameOverStatus { get; set; } = new NoMapEventsGameOver();

    public float GameTime = 0;
    public int GameId;

    public bool DialogVisible => this.hUD.DialogVisible;

    public Game()
    {
        maze = MazeGeneratorWrapper.DefaultInstance;
    }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.Connect(CommonSignals.VisibilityChanged, this, nameof(VisibilityChanged));

        this.GetTree().CallGroup(Groups.DynamicGameObject, "queue_free");

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

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (MapEvents == null)
        {
            return;
        }

        GameTime += delta;
        while (this.MapEvents.Count > 0 && this.MapEvents.Peek().Condition.IsReady(this))
        {
            var unitItem = this.MapEvents.Dequeue();
            unitItem.Action.Action(this);
            this.hUD.Progress++;
        }

        if (GameOverStatus.CheckGameOver(this) != GameOverState.None)
        {
            FinishGame();
        }
    }

    public void FinishGame()
    {
        this.music.Stop();
        this.hUD.Stop();
        this.EmitSignal(nameof(GameOver), this.hUD.Score);
        this.GetTree().CallGroup(Groups.DynamicGameObject, "queue_free");
    }

    private void PlayerHit()
    {
        this.deathSound.Play();
        this.FinishGame();
    }


    public void AddMapEvents(List<MapEvent> unitsList)
    {
        foreach (var unit in unitsList)
        {
            this.MapEvents.Enqueue(unit);
        }

        this.hUD.Progress = 0;
        this.hUD.MaxProgress = unitsList.Count;
    }

    public void NewGame(int gameId)
    {
        this.GameId = gameId;
        this.GameTime = 0;

        var state = maze.GenerateLevel(gameId);

        for (var x = 0; x < state.Map.GetLength(0); x++)
            for (var y = 0; y < state.Map.GetLength(1); y++)
            {
                if (state.Map[x, y] != 1)
                {
                    continue;
                }

                for (var x2 = 0; x2 < PathSize; x2++)
                    for (var y2 = 0; y2 < PathSize; y2++)
                    {
                        var block = (Block)blockScene.Instance();
                        block.Position = new Vector2(x * 100 * PathSize + x2 * 100, y * 100 * PathSize + y2 * 100);
                        this.AddChild(block);
                    }
            }

        this.MapEvents.Clear();
        this.hUD.MaxProgress = 0;
        this.AddMapEvents(state.UnitsList);

        var startPosition = state.StartPosition * 100 * PathSize + Vector2.One * 50 * PathSize;
        var rect = new Rect2(Vector2.Zero, new Vector2(state.Map.GetLength(0) * 100 * PathSize + PathSize * 50, state.Map.GetLength(1) * 100 * PathSize + PathSize * 50));

        var player = playerScene.Instance<Player>();
        player.Connect(nameof(Player.Hit), this, nameof(PlayerHit));
        player.Position = startPosition;
        player.FieldPath = this.GetPath();
        player.AddToGroup(Groups.PlayerUnit);
        this.AddChild(player);

        this.hUD.Start(rect, player.GetPath());

        this.music.Play();
    }

    internal void ShowDialog(string text)
    {
        this.hUD.ShowDialog(text);
    }
}
