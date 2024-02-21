namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class PlayerExistsMapEventCondition : IMapEventCondition
    {
        public PlayerExistsMapEventCondition()
        {
        }

        public bool IsReady(Game game)
        {
            return game.GetTree().GetNodesInGroup(Groups.PlayerUnit).Count != 0;
        }
    }
}