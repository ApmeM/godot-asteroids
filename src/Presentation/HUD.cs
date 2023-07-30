using Godot;
using GodotAnalysers;

[SceneReference("HUD.tscn")]
public partial class HUD
{
    [Export]
    public NodePath PlayerPath;

    private Communicator communicator;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.communicator = GetNode<Communicator>("/root/Main/Communicator");
        this.communicator.Connect(nameof(Communicator.ScoreAdded), this, nameof(UpdateScore));

        if (this.PlayerPath != null)
        {
            this.minimap.PlayerPath = this.GetNode(this.PlayerPath).GetPath();
        }
    }

    public void UpdateScore(int scoreAdded)
    {
        this.scoreLabel.Value += scoreAdded;
    }

    private void SetScore(int score)
    {
        UpdateScore(score);
    }

    internal void Start(Rect2 rect)
    {
        this.scoreLabel.Value = 0;
        this.minimap.SetMapSize(rect);

        this.timerLabel.ShowMessage("Get Ready!", 1);
    }
}
