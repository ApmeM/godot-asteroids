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

        public void Action(Vector2 playerPosition, int pathSize, Node toAdd)
        {
            var mobSpawnLocation = this.Position * 100 * pathSize + Vector2.One * 50 * pathSize;

            if (mobSpawnLocation.DistanceSquaredTo(playerPosition) < 40000)
            {
                return;
            }

            var direction = (playerPosition - mobSpawnLocation).Angle();
            var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);

            var mob = this.BonusType.CreateBonus<Node2D>();
            mob.Position = mobSpawnLocation;
            toAdd.AddChild(mob);
        }
    }
}