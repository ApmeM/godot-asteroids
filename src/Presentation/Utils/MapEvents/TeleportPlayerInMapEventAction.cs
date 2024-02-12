using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{

    public class TeleportPlayerInMapEventAction : IMapEventAction
    {
        public TeleportPlayerInMapEventAction(int gameSize, Vector2 position)
        {
            GameSize = gameSize;
            Position = position;
        }

        public Vector2 Position;
        public int GameSize;

        public void Action(Game game)
        {
            var mobSpawnLocation = this.Position * 100 * Game.PathSize + Vector2.One * 50 * Game.PathSize;

            var player = (Player)ResourceLoader.Load<PackedScene>($"res://Presentation/Player.tscn").Instance();
            player.Position = mobSpawnLocation;
            player.FieldPath = game.GetPath();
            game.AddChild(player);
            var rect = new Rect2(Vector2.Zero, new Vector2(GameSize * 100 * Game.PathSize + Game.PathSize * 50, GameSize * 100 * Game.PathSize + Game.PathSize * 50));
            game.ShowMinimap(rect, player.GetPath());
        }
    }
}
