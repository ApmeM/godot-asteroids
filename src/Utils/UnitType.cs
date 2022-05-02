using Godot;

namespace DodgeTheCreeps.Utils
{
    public enum UnitType
    {
        LargeMeteor,
        Meteor,
        SmallMeteor,
        BlackHole,
    }

    public static class UnitTypeCreator
    {
        public static RigidBody2D CreateUnit(this UnitType unitType)
        {
            return (RigidBody2D)ResourceLoader.Load<PackedScene>($"res://UnitTypes/{unitType}.tscn").Instance();
        }
    }
}
