using Godot;
using GodotAnalysers;

[SceneReference("Communicator.tscn")]
public partial class Communicator : Node
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    [Signal]
    public delegate void ScoreAdded(int score);
}
