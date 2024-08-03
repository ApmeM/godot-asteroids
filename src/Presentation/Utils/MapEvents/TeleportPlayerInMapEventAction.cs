using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{

    public class TeleportPlayerInMapEventAction : IMapEventAction
    {
        public TeleportPlayerInMapEventAction(Rect2 fieldSize, Vector2 spawnPosition)
        {
            FieldSize = fieldSize;
            SpawnPosition = spawnPosition;
        }

        public Vector2 SpawnPosition;
        public Rect2 FieldSize;

        public void Action(Game game)
        {
            if (game.GetTree().GetNodesInGroup(Groups.PlayerUnit).Count != 0)
            {
                return;
            }

            var player = (Player)ResourceLoader.Load<PackedScene>($"res://Presentation/Player.tscn").Instance();
            player.Position = this.SpawnPosition;
            player.FieldPath = game.GetPath();
            game.AddChild(player);
            game.ShowMinimap(FieldSize, player.GetPath());
        }
    }
}
