using Godot;

namespace DodgeTheCreeps.UnitTypes
{
    public interface IHitable
    {
        void Hit(Bullet byNode);
    }
}
