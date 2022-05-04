using Godot;

public class Communicator : Node
{
    [Signal]
    public delegate void ScoreAdded(int score);
}