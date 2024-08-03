using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class NoPlayerMapEventCondition : IMapEventCondition
    {
        public NoPlayerMapEventCondition()
        {
        }

        public bool IsReady(Game game)
        {
            return game.GetTree().GetNodesInGroup(Groups.PlayerUnit).Count == 0;
        }
    }
}