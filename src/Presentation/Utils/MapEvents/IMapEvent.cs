using DodgeTheCreeps.Utils;
using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public interface IMapEvent
    {
        void Action(Vector2 playerPosition, int pathSize, Node toAdd);
        bool IsReady(double progress);
    }

    public class UnitSpawnMap : IMapEvent
    {
        public Godot.Vector2 Position;
        public UnitType UnitType;
        public float SpawnTime;

        public void Action(Vector2 playerPosition, int pathSize, Node toAdd)
        {
            var mobSpawnLocation = this.Position * 100 * pathSize + Vector2.One * 50 * pathSize;

            if (mobSpawnLocation.DistanceSquaredTo(playerPosition) < 40000)
            {
                return;
            }

            var direction = (playerPosition - mobSpawnLocation).Angle();
            var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);

            var mob = this.UnitType.CreateUnit<Node2D>();
            mob.Position = mobSpawnLocation;
            mob.Rotation = direction;
            if (mob is RigidBody2D body)
            {
                body.LinearVelocity = velocity.Rotated(direction);
            }

            toAdd.AddChild(mob);
        }

        public bool IsReady(double progress)
        {
            return this.SpawnTime < progress;
        }
    }
}