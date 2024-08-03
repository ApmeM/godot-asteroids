using DodgeTheCreeps.Utils;
using FateRandom;
using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{

    public class SpawnUnitMapEventAction : IMapEventAction
    {
        private Fate random = Fate.GlobalFate;

        public SpawnUnitMapEventAction(Vector2 spawnPosition, UnitType unitType)
        {
            SpawnPosition = spawnPosition;
            UnitType = unitType;
        }

        public Vector2 SpawnPosition;
        public UnitType UnitType;

        public void Action(Game game)
        {

            var direction = random.NextAngle();
            var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);

            var mob = this.UnitType.CreateUnit<Node2D>();
            mob.Position = this.SpawnPosition;
            mob.Rotation = direction;
            if (mob is RigidBody2D body)
            {
                body.LinearVelocity = velocity.Rotated(direction);
            }

            game.AddChild(mob);
        }
    }
}
