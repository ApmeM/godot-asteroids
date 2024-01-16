using Godot;

namespace DodgeTheCreeps.Utils
{
    public enum UnitType
    {
        Planet,
        SmallMeteor,
        Meteor,
        LargeMeteor,
        MeteorGroup,
        BlackHole,
    }

    public static class UnitTypeCreator
    {
        public static T CreateUnit<T>(this UnitType unitType) where T : Node
        {
            return (T)ResourceLoader.Load<PackedScene>($"res://Presentation/UnitTypes/{unitType}.tscn").Instance();
        }
    }
}
