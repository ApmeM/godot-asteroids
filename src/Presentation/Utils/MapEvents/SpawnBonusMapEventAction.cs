using DodgeTheCreeps.Utils;
using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{

    public class SpawnBonusMapEventAction : IMapEventAction
    {
        public SpawnBonusMapEventAction(Vector2 spawnPosition, BonusType bonusTyoe)
        {
            SpawnPosition = spawnPosition;
            BonusType = bonusTyoe;
        }

        public Vector2 SpawnPosition;
        public BonusType BonusType;

        public void Action(Game game)
        {
            var player = (Node2D)game.GetTree().GetNodesInGroup(Groups.PlayerUnit)[0];
            var playerPosition = player.Position;

            if (this.SpawnPosition.DistanceSquaredTo(playerPosition) < 40000)
            {
                return;
            }

            var direction = (playerPosition - this.SpawnPosition).Angle();
            var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);

            var mob = this.BonusType.CreateBonus<Node2D>();
            mob.Position = this.SpawnPosition;
            game.AddChild(mob);
        }
    }
}