using Godot;
using GodotAnalysers;

[SceneReference("Joystick.tscn")]
public partial class Joystick
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();       
    }
}
