using Godot;

namespace DodgeTheCreeps.UnitTypes
{
    public interface IHitter
    {
        int Power { get; }
        
        Vector2 Position { get; }
    }
}
