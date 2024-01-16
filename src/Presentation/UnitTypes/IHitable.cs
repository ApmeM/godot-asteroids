using Godot;

namespace DodgeTheCreeps.UnitTypes
{
    public interface IHitable
    {
        void Hit(IHitter byNode);
    }

    public interface IHitter
    {
        int Power { get; }
        
        Vector2 Position { get; }
    }
}
