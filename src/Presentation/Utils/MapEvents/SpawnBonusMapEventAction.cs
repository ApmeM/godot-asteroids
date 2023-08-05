using DodgeTheCreeps.Utils;
using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{

    public class SpawnBonusMapEventAction : IMapEventAction
    {
        public SpawnBonusMapEventAction(Vector2 position, BonusType bonusTyoe)
        {
            Position = position;
            BonusType = bonusTyoe;
        }

        public Vector2 Position;
        public BonusType BonusType;

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

            var mob = this.BonusType.CreateBonus<Node2D>();
            mob.Position = mobSpawnLocation;
            game.AddChild(mob);
        }
    }
}