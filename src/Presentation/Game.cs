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
    public int MapEventsTotal;

    private bool gameOverStatusDirty = true;
    private IGameOver gameOverStatus = new InfinityGameOver();
    public IGameOver GameOverStatus
    {
        get => gameOverStatus;
        set
        {
            gameOverStatus = value;
            this.gameOverStatusDirty = true;
        }
    }
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

        GameTime += delta;
        while (this.MapEvents.Count > 0 && this.MapEvents.Peek().Condition.IsReady(this))
        {
            this.MapEvents.Dequeue().Action.Action(this);
        }

        if (this.gameOverStatusDirty)
        {
            this.gameOverStatusDirty = false;
            this.hUD.ClearStatus();
            gameOverStatus.InitializeStatus(this.hUD);
        }

        GameOverStatus.UpdateStatus(this);

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
        this.MapEvents.Clear();
        this.GameOverStatus = new InfinityGameOver();
    }

    public void AddMapEvents(List<MapEvent> unitsList)
    {
        foreach (var unit in unitsList)
        {
            this.MapEvents.Enqueue(unit);
        }

        MapEventsTotal = this.MapEvents.Count;
    }

    public void NewGame(int gameId)
    {
        this.GameId = gameId;
        this.GameTime = 0;

        this.MapEvents.Clear();
        this.AddMapEvents(maze.GenerateLevel(gameId));

        this.music.Play();
    }

    internal void ShowMinimap(Rect2 rect, NodePath playerPath)
    {
        this.hUD.Start(rect, playerPath);
    }

    internal void ShowDialog(string text, bool left)
    {
        this.hUD.ShowDialog(text, left);
    }
}
