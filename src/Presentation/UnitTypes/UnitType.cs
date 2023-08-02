using Godot;

namespace DodgeTheCreeps.Utils
{
    public enum UnitType
    {
        LargeMeteor,
        Meteor,
        SmallMeteor,
        BlackHole,
        MeteorGroup,
    }

    public static class UnitTypeCreator
    {
        public static T CreateUnit<T>(this UnitType unitType) where T : Node
        {
            return (T)ResourceLoader.Load<PackedScene>($"res://Presentation/UnitTypes/{unitType}.tscn").Instance();
        }
    }
}
