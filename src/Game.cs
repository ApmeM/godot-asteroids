using DodgeTheCreeps.Utils;
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

    private const int pathSize = 5;

    private Vector2 mobSpawnLocation;

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
        this.GetTree().CallGroup(Constants.DynamicGameObject, "queue_free");
        this.score = 0;

        var rect = new Rect2(Vector2.Zero, Vector2.Zero);
        
        var map = MazeGeneratorWrapper.Generate();
        var startPosition = Vector2.Zero;
        for (var x = 0; x < map.GetLength(0); x++)
            for (var y = 0; y < map.GetLength(0); y++)
            {
                rect = rect.Expand(new Vector2(x * 100 * pathSize + pathSize * 50, y * 100 * pathSize + pathSize * 50));

                if (map[x, y] != 1)
                {
                    if (startPosition == Vector2.Zero)
                    {
                        startPosition = new Vector2(x * 100 * pathSize + pathSize * 50, y * 100 * pathSize + pathSize * 50);
                    }

                    mobSpawnLocation = new Vector2(x * 100 * pathSize + pathSize * 50, y * 100 * pathSize + pathSize * 50);
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

        this.player.Start(startPosition, rect);

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
        var direction = (this.player.Position - this.mobSpawnLocation).Angle();
        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);

        var mob = (Mob)mobScene.Instance();
        mob.Position = this.mobSpawnLocation;
        mob.Rotation = direction;
        mob.LinearVelocity = velocity.Rotated(direction);
        this.AddChild(mob);
    }
}
