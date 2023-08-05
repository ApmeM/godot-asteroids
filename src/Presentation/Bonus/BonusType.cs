using Godot;

namespace DodgeTheCreeps.Utils
{
    public enum BonusType
    {
        Weapon,
        RapidFire,
        Power
    }

    public static class BonusTypeCreator
    {
        public static T CreateBonus<T>(this BonusType bonusType) where T : Node
        {
            return (T)ResourceLoader.Load<PackedScene>($"res://Presentation/Bonus/{bonusType}.tscn").Instance();
        }
    }
}
