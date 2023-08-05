using DodgeTheCreeps.Utils;
using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{

    public class SpawnUnitMapEventAction : IMapEventAction
    {
        public SpawnUnitMapEventAction(Vector2 position, UnitType unitType)
        {
            Position = position;
            UnitType = unitType;
        }
        
        public Vector2 Position;
        public UnitType UnitType;

        public void Action(Game game)
        {
            var player = (Node2D)game.GetTree().GetNodesInGroup(Groups.PlayerUnit)[0];
            var playerPosition = player.Position;
            var mobSpawnLocation = this.Position * 100 * Game.PathSize + Vector2.One * 50 * Game.PathSize;

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

            game.AddChild(mob);
        }
    }
}