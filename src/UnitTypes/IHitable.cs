using Godot;

namespace DodgeTheCreeps.UnitTypes
{
    public interface IHitable
    {
        void Hit(Node2D byNode);
    }
}
