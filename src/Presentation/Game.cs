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

    [Export]
    public PackedScene playerScene;
    private Player player;

    private MazeGeneratorWrapper maze;
    private const int pathSize = 1;
    private Queue<MapEvent> UnitsList = new Queue<MapEvent>();

    public Game()
    {
        maze = MazeGeneratorWrapper.DefaultInstance;
    }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.Connect(CommonSignals.VisibilityChanged, this, nameof(VisibilityChanged));

        this.GetTree().CallGroup(Constants.DynamicGameObject, "queue_free");

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

    public float Progress = 0;

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (UnitsList == null)
        {
            return;
        }

        Progress += delta;
        while (this.UnitsList.Count > 0 && this.UnitsList.Peek().Condition.IsReady(this))
        {
            var unitItem = this.UnitsList.Dequeue();
            unitItem.Action.Action(this.player.Position, pathSize, this);
            this.hUD.Progress ++;
        }
    }

    private void PlayerHit()
    {
        this.music.Stop();
        this.deathSound.Play();

        this.EmitSignal(nameof(GameOver), 0);
     
        this.GetTree().CallGroup(Constants.DynamicGameObject, "queue_free");
    }

    public void NewGame()
    {
        this.hUD.Progress = 0;
        this.Progress = 0;

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
        
        this.player = playerScene.Instance<Player>();
        this.player.Connect(nameof(Player.Hit), this, nameof(PlayerHit));
        this.player.Position = startPosition;
        this.player.FieldPath = this.GetPath();
        this.AddChild(player);

        this.hUD.PlayerPath = this.player.GetPath();
        this.hUD.Start(rect);

        //this.music.Play();
    }
}
