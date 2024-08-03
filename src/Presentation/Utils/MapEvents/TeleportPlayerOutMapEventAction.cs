using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class TeleportPlayerOutMapEventAction : IMapEventAction
    {
        public TeleportPlayerOutMapEventAction(Vector2 teleportDirection)
        {
            TeleportDirection = teleportDirection;
        }

        public Vector2 TeleportDirection;

        public void Action(Game game)
        {
            var player = (Player)game.GetTree().GetNodesInGroup(Groups.PlayerUnit)[0];
            player.Teleport(this.TeleportDirection);
        }
    }
}
