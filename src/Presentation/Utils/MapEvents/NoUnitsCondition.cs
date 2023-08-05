using DodgeTheCreeps.Utils;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class NoUnitsCondition : IMapEventCondition
    {
        public NoUnitsCondition()
        {
        }

        public bool IsReady(Game game)
        {
            return game.GetTree().GetNodesInGroup(Groups.EnemyUnit).Count == 0;
        }
    }
}