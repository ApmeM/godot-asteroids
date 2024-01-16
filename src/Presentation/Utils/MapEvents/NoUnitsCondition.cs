using DodgeTheCreeps.Utils;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class NoEnemyUnitsCondition : IMapEventCondition
    {
        public NoEnemyUnitsCondition()
        {
        }

        public bool IsReady(Game game)
        {
            return game.GetTree().GetNodesInGroup(Groups.EnemyUnit).Count == 0;
        }
    }
}