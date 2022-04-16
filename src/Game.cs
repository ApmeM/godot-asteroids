using DodgeTheCreeps;
using Godot;
using GodotAnalysers;

[SceneReference("Game.tscn")]
public partial class Game : Node2D
{
    [Signal]
    public delegate void GameOver(int score);

    [Export]
    public PackedScene mobScene;

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

        GD.Randomize();
    }

    private void PlayerHit()
    {
        this.mobTimer.Stop();
        this.scoreTimer.Stop();
        this.music.Stop();
        this.deathSound.Play();

        this.EmitSignal(nameof(GameOver), this.score);
    }

    public void NewGame()
    {
        this.GetTree().CallGroup("mobs", "queue_free");
        this.score = 0;

        this.player.Start(this.startPosition.Position);

        this.startTimer.Start();

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
        var direction = this.mobSpawnLocation.Rotation + (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);

        var mob = (Mob)mobScene.Instance();
        mob.Position = this.mobSpawnLocation.Position;
        mob.Rotation = direction;
        mob.LinearVelocity = velocity.Rotated(direction);
        this.AddChild(mob);
    }
}
