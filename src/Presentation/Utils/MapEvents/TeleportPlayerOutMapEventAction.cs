using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class TeleportPlayerOutMapEventAction : IMapEventAction
    {
        public TeleportPlayerOutMapEventAction(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position;

        public void Action(Game game)
        {
            var teleportDirection = this.Position * 100 * Game.PathSize + Vector2.One * 50 * Game.PathSize;

            var player = (Player)game.GetTree().GetNodesInGroup(Groups.PlayerUnit)[0];
            player.Teleport(teleportDirection);
        }
    }
}
