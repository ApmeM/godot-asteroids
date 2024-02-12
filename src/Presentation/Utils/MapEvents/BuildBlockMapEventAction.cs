using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{

    public class BuildBlockMapEventAction : IMapEventAction
    {
        public BuildBlockMapEventAction(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position;

        public void Action(Game game)
        {
            for (var x2 = 0; x2 < Game.PathSize; x2++)
                for (var y2 = 0; y2 < Game.PathSize; y2++)
                {
                    var block = (Block)ResourceLoader.Load<PackedScene>($"res://Presentation/UnitTypes/Block.tscn").Instance();
                    block.Position = new Vector2(this.Position.x * 100 * Game.PathSize + x2 * 100, this.Position.y * 100 * Game.PathSize + y2 * 100);
                    game.AddChild(block);
                }
        }
    }
}
