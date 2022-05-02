using Godot;

namespace DodgeTheCreeps.Utils
{
    public enum BonusType
    {
        Booster
    }

    public static class BonusTypeCreator
    {
        public static Node2D CreateBonus(this BonusType bonusType)
        {
            return (Node2D)ResourceLoader.Load<PackedScene>($"res://Bonus/{bonusType}.tscn").Instance();
        }
    }
}
