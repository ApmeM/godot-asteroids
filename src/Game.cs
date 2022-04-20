using DodgeTheCreeps;
using Godot;
using GodotAnalysers;

[SceneReference("Game.tscn")]
public partial class Game
{
    [Signal]
    public delegate void GameOver(int score);

    [Export]
    public PackedScene mobScene;
    [Export]
    public PackedScene blockScene;

    public int score;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.mobTimer.Connect(CommonSignals.Timeout, this, nameof(OnMobTimerTimeout));
        this.scoreTimer.Connect(CommonSignals.Timeout, this, nameof(OnScoreTimerTimeout));
        this.startTimer.Connect(CommonSignals.Timeout, this, nameof(OnStartTimerTimeout));
        this.hUD.Connect(nameof(HUD.StartGame), this, nameof(NewGame));
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
        this.mobTimer.Stop();
        this.scoreTimer.Stop();
        this.music.Stop();
        this.deathSound.Play();

        this.camera2D.Current = false;

        this.EmitSignal(nameof(GameOver), this.score);
    }

    public void NewGame()
    {
        this.GetTree().CallGroup("mobs", "queue_free");
        this.score = 0;

        var points = this.mobPath.Curve.GetBakedPoints();
        var rect = new Rect2(points[0], Vector2.Zero);
        foreach (var point in points)
        {
            rect = rect.Expand(point);
        }

        this.player.Start(this.startPosition.Position, rect);


        for (var x = rect.Position.x; x < rect.End.x + 101; x+=100)
        {
            Block block;

            block = (Block)blockScene.Instance();
            block.Position = new Vector2(x, rect.Position.y - 100);
            this.AddChild(block);

            block = (Block)blockScene.Instance();
            block.Position = new Vector2(x, rect.End.y + 100);
            this.AddChild(block);
        }

        for (var y = rect.Position.y; y < rect.End.y + 101; y+=100)
        {
            Block block;

            block = (Block)blockScene.Instance();
            block.Position = new Vector2(rect.Position.x - 100, y);
            this.AddChild(block);

            block = (Block)blockScene.Instance();
            block.Position = new Vector2(rect.End.x + 100, y);
            this.AddChild(block);
        }


        this.camera2D.Current = true;

        this.startTimer.Start();

        this.hUD.SetMapSize(rect);
        this.hUD.UpdateScore(score);
        this.hUD.ShowMessage("Get Ready!");

        this.music.Play();
    }

    private void OnStartTimerTimeout()
    {
        this.mobTimer.Start();
        this.scoreTimer.Start();
    }

    private void OnScoreTimerTimeout()
    {
        this.score++;

        this.hUD.UpdateScore(score);
    }

    private void OnMobTimerTimeout()
    {
        this.mobSpawnLocation.Offset = GD.Randi();
        var direction = this.mobSpawnLocation.Rotation + Mathf.Pi / 2 + (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);

        var mob = (Mob)mobScene.Instance();
        mob.Position = this.mobSpawnLocation.Position;
        mob.Rotation = direction;
        mob.LinearVelocity = velocity.Rotated(direction);
        this.AddChild(mob);
    }
}
