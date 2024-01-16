namespace DodgeTheCreeps.UnitTypes
{
    public interface IHitable
    {
        void Hit(IHitter byNode);
    }
}
