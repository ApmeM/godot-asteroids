using Godot;

namespace DodgeTheCreeps.UnitTypes
{
    public interface IHitable
    {
        bool IsDead { get; }
        void Hit(Node2D byNode);
    }
}
