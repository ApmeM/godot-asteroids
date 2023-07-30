using Godot;

namespace DodgeTheCreeps.Utils
{
    public enum BonusType
    {
        Weapon,
        RapidFire
    }

    public static class BonusTypeCreator
    {
        public static Node2D CreateBonus(this BonusType bonusType)
        {
            return (Node2D)ResourceLoader.Load<PackedScene>($"res://Presentation/Bonus/{bonusType}.tscn").Instance();
        }
    }
}
